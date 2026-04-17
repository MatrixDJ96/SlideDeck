using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using SlideDeck.Helpers;

namespace SlideDeck.Services
{
    /// <summary>
    /// Gestisce l'apertura del file Excel da compilare.
    /// La Task termina solo quando il workbook viene realmente chiuso.
    /// Excel può rimanere aperto: a noi interessa soltanto il file.
    /// 
    /// In questa versione il backup automatico viene creato PRIMA di ogni salvataggio,
    /// usando l'evento Excel BeforeSave del workbook.
    /// In questo modo il backup contiene davvero la versione precedente del file.
    /// </summary>
    public sealed class ExcelPreviewService
    {
        private readonly TemplateBackupService _templateBackupService;

        /// <summary>
        /// Costruttore del servizio.
        /// </summary>
        public ExcelPreviewService(TemplateBackupService templateBackupService)
        {
            _templateBackupService = templateBackupService
                ?? throw new ArgumentNullException(nameof(templateBackupService));
        }

        /// <summary>
        /// Apre il file Excel in un thread STA separato.
        /// La UI principale può quindi restare bloccata finché il file è aperto,
        /// senza congelare l'applicazione.
        /// </summary>
        public Task OpenTemplateForEditingAsync(string excelPath)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            var workerThread = new Thread(() =>
            {
                try
                {
                    OpenTemplateForEditingInternal(excelPath);
                    taskCompletionSource.SetResult(true);
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            });

            workerThread.IsBackground = true;
            workerThread.SetApartmentState(ApartmentState.STA);
            workerThread.Start();

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Implementazione reale dell'apertura del file Excel.
        /// Il metodo ritorna solo quando il workbook non risulta più aperto.
        /// </summary>
        private void OpenTemplateForEditingInternal(string excelPath)
        {
            string fullPath = Path.GetFullPath(excelPath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("File Excel non trovato.", fullPath);
            }

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.WorkbookEvents_Event workbookEvents = null;

            // Handler dell'evento BeforeSave.
            // Lo manteniamo in una variabile per poterlo sganciare correttamente nel finally.
            Excel.WorkbookEvents_BeforeSaveEventHandler beforeSaveHandler = null;

            // Flag per evitare rientri indesiderati durante il backup.
            bool isBackupInProgress = false;

            try
            {
                // Prova a riutilizzare un'istanza Excel già aperta.
                // Se non esiste, ne crea una nuova.
                excelApp = TryGetRunningExcel() ?? new Excel.Application();

                // Se il workbook è già aperto, usa quello.
                // Altrimenti lo apre.
                workbook = FindOpenWorkbook(excelApp, fullPath);
                if (workbook == null)
                {
                    workbook = excelApp.Workbooks.Open(
                        fullPath,
                        UpdateLinks: 0,
                        ReadOnly: false);
                }

                // Aggancia l'interfaccia eventi del workbook per intercettare il salvataggio.
                workbookEvents = (Excel.WorkbookEvents_Event)workbook;

                /// <summary>
                /// Evento lanciato da Excel immediatamente prima del salvataggio del workbook.
                /// Qui creiamo il backup della versione corrente su disco,
                /// così il backup contiene davvero lo stato precedente.
                /// Se il backup fallisce, annulliamo il salvataggio.
                /// </summary>
                beforeSaveHandler = (bool saveAsUi, ref bool cancel) =>
                {
                    // Evita eventuali rientri multipli.
                    if (isBackupInProgress)
                    {
                        return;
                    }

                    try
                    {
                        isBackupInProgress = true;

                        // Creiamo il backup solo se il file locale esiste già su disco.
                        // È il caso normale del template gestito dall'app.
                        if (File.Exists(fullPath))
                        {
                            _templateBackupService.CreateBackupOfFile(fullPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Se il backup fallisce, annulliamo il salvataggio.
                        // In questo modo non perdiamo la garanzia di avere
                        // una copia della versione precedente.
                        cancel = true;

                        MessageBox.Show(
                            "Impossibile creare il backup automatico prima del salvataggio.\n\n" +
                            "Il salvataggio è stato annullato.\n\n" +
                            ex.Message,
                            "SlideDeck - Backup non riuscito",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        isBackupInProgress = false;
                    }
                };

                workbookEvents.BeforeSave += beforeSaveHandler;

                // Mostra Excel all'utente in finestra massimizzata.
                excelApp.DisplayAlerts = false;
                excelApp.Visible = true;
                excelApp.WindowState = Excel.XlWindowState.xlMaximized;

                // Porta il workbook in primo piano.
                workbook.Activate();

                // Rimane in attesa finché il file non viene chiuso.
                // Non ci interessa se Excel rimane aperto.
                WaitUntilWorkbookIsClosed(excelApp, fullPath);
            }
            finally
            {
                // Sgancia l'handler dell'evento prima di rilasciare gli oggetti COM.
                try
                {
                    if (workbookEvents != null && beforeSaveHandler != null)
                    {
                        workbookEvents.BeforeSave -= beforeSaveHandler;
                    }
                }
                catch
                {
                    // Ignorato volutamente:
                    // non vogliamo che un problema nello sgancio dell'evento
                    // comprometta la chiusura del servizio.
                }

                // Rilascia i riferimenti COM usati da questo servizio.
                ComReleaseHelper.Release(workbookEvents);
                ComReleaseHelper.Release(workbook);
                ComReleaseHelper.Release(excelApp);
            }
        }

        /// <summary>
        /// Resta in attesa finché il workbook indicato risulta aperto
        /// nell'istanza Excel corrente.
        /// </summary>
        private void WaitUntilWorkbookIsClosed(Excel.Application excelApp, string fullPath)
        {
            while (true)
            {
                bool workbookStillOpen;

                try
                {
                    workbookStillOpen = IsWorkbookOpen(excelApp, fullPath);
                }
                catch (COMException)
                {
                    // Se Excel viene chiuso del tutto o la connessione COM cade,
                    // consideriamo la sessione conclusa.
                    break;
                }
                catch
                {
                    // In caso di errore imprevisto, non lasciamo la UI bloccata.
                    break;
                }

                if (!workbookStillOpen)
                {
                    break;
                }

                // Piccola pausa per evitare polling troppo aggressivo.
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Prova ad agganciarsi a un'istanza Excel già aperta.
        /// </summary>
        private Excel.Application TryGetRunningExcel()
        {
            try
            {
                return (Excel.Application)Marshal.GetActiveObject("Excel.Application");
            }
            catch (COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Cerca un workbook già aperto che corrisponda al path indicato.
        /// </summary>
        private Excel.Workbook FindOpenWorkbook(Excel.Application excelApp, string fullPath)
        {
            if (excelApp == null)
            {
                return null;
            }

            Excel.Workbooks workbooks = null;

            try
            {
                workbooks = excelApp.Workbooks;

                for (int i = 1; i <= workbooks.Count; i++)
                {
                    Excel.Workbook currentWorkbook = null;
                    bool isMatch = false;

                    try
                    {
                        currentWorkbook = workbooks[i];

                        isMatch = string.Equals(
                            Path.GetFullPath(currentWorkbook.FullName),
                            fullPath,
                            StringComparison.OrdinalIgnoreCase);

                        if (isMatch)
                        {
                            return currentWorkbook;
                        }
                    }
                    catch
                    {
                        // Se un workbook non è leggibile, lo ignoriamo.
                    }
                    finally
                    {
                        if (!isMatch)
                        {
                            ComReleaseHelper.Release(currentWorkbook);
                        }
                    }
                }

                return null;
            }
            finally
            {
                ComReleaseHelper.Release(workbooks);
            }
        }

        /// <summary>
        /// Verifica se il workbook è ancora aperto nell'istanza Excel corrente.
        /// </summary>
        private bool IsWorkbookOpen(Excel.Application excelApp, string fullPath)
        {
            Excel.Workbook workbook = null;

            try
            {
                workbook = FindOpenWorkbook(excelApp, fullPath);
                return workbook != null;
            }
            finally
            {
                ComReleaseHelper.Release(workbook);
            }
        }
    }
}