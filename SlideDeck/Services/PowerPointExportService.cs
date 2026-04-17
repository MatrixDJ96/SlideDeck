using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using SlideDeck.Helpers;
using SlideDeck.Models;

namespace SlideDeck.Services
{
    /// <summary>
    /// Gestisce l'esportazione dei fogli Excel in una presentazione PowerPoint.
    /// </summary>
    public sealed class PowerPointExportService
    {
        /// <summary>
        /// Avvia l'export in un thread STA separato.
        /// Office Interop lavora in modo più affidabile in un thread STA.
        /// </summary>
        public Task ExportAsync(ExportOptions options)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            var workerThread = new Thread(() =>
            {
                try
                {
                    ExportInternal(options);
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
        /// Implementazione reale dell'export Excel -> PowerPoint.
        /// </summary>
        private void ExportInternal(ExportOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            string fullExcelPath = Path.GetFullPath(options.ExcelPath);

            if (!File.Exists(fullExcelPath))
            {
                throw new FileNotFoundException("File Excel non trovato.", fullExcelPath);
            }

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;

            PowerPoint.Application powerPointApp = null;
            PowerPoint.Presentation presentation = null;
            PowerPoint.DocumentWindow powerPointWindow = null;

            bool createdExcelApp = false;
            bool openedWorkbookHere = false;
            bool createdPowerPointApp = false;
            bool keepPowerPointOpen = false;

            try
            {
                // Recupera Excel aperto, se presente.
                excelApp = TryGetRunningExcel();
                if (excelApp == null)
                {
                    excelApp = new Excel.Application
                    {
                        Visible = false,
                        DisplayAlerts = false
                    };

                    createdExcelApp = true;
                }

                // Recupera il workbook già aperto, se presente.
                workbook = FindOpenWorkbook(excelApp, fullExcelPath);
                if (workbook == null)
                {
                    workbook = excelApp.Workbooks.Open(
                        fullExcelPath,
                        UpdateLinks: 0,
                        ReadOnly: false);

                    openedWorkbookHere = true;
                }

                // Calcola fino a quale foglio processare.
                int lastSheetIndex = GetLastSheetIndex(workbook, options.SkipLastWorksheet);

                // Verifica se ci sono fogli esportabili.
                int exportableSheetsCount = CountExportableSheets(workbook, lastSheetIndex);
                if (exportableSheetsCount == 0)
                {
                    throw new InvalidOperationException("Non ci sono fogli validi da esportare.");
                }

                // Recupera PowerPoint aperto, se presente.
                powerPointApp = TryGetRunningPowerPoint();
                if (powerPointApp == null)
                {
                    powerPointApp = new PowerPoint.Application();
                    createdPowerPointApp = true;
                }

                // Crea una nuova presentazione inizialmente nascosta.
                presentation = powerPointApp.Presentations.Add(Office.MsoTriState.msoFalse);

                int slideIndex = 0;

                // Scorre i fogli dal primo al penultimo.
                for (int i = 1; i <= lastSheetIndex; i++)
                {
                    Excel.Worksheet worksheet = null;
                    Excel.Range realUsedRange = null;

                    PowerPoint.Slide slide = null;
                    PowerPoint.ShapeRange pastedShapeRange = null;
                    PowerPoint.Shape pastedShape = null;

                    try
                    {
                        worksheet = (Excel.Worksheet)workbook.Worksheets[i];
                        realUsedRange = GetRealUsedRange(worksheet);

                        // Se il foglio è vuoto, lo salta.
                        if (realUsedRange == null)
                        {
                            continue;
                        }

                        slideIndex++;

                        // Crea una slide vuota.
                        slide = presentation.Slides.Add(
                            slideIndex,
                            PowerPoint.PpSlideLayout.ppLayoutBlank);

                        // Copia il range come immagine bitmap,
                        // replicando il comportamento della macro VBA.
                        realUsedRange.CopyPicture(
                            Excel.XlPictureAppearance.xlScreen,
                            Excel.XlCopyPictureFormat.xlBitmap);

                        // Aspetta finché PowerPoint non riesce a incollare.
                        pastedShapeRange = PasteBitmapWhenReady(slide, options.PasteTimeoutMs);
                        if (pastedShapeRange == null)
                        {
                            throw new InvalidOperationException(
                                $"Impossibile incollare il foglio '{worksheet.Name}' in PowerPoint.");
                        }

                        pastedShape = pastedShapeRange[1];

                        // Ridimensiona e centra l'immagine nella slide.
                        FitShapeToSlide(pastedShape, presentation);
                    }
                    finally
                    {
                        ComReleaseHelper.Release(pastedShape);
                        ComReleaseHelper.Release(pastedShapeRange);
                        ComReleaseHelper.Release(slide);
                        ComReleaseHelper.Release(realUsedRange);
                        ComReleaseHelper.Release(worksheet);
                    }
                }

                // Mostra PowerPoint solo alla fine.
                powerPointApp.Visible = Office.MsoTriState.msoTrue;

                // Crea una finestra per la presentazione.
                powerPointWindow = presentation.NewWindow();

                // Attiva e massimizza la finestra, in modo coerente con Excel.
                powerPointWindow.Activate();
                powerPointWindow.WindowState = PowerPoint.PpWindowState.ppWindowMaximized;

                keepPowerPointOpen = true;
            }
            finally
            {
                // Se qualcosa è andato storto, chiudi ciò che è stato creato qui.
                if (!keepPowerPointOpen)
                {
                    try
                    {
                        presentation?.Close();
                    }
                    catch
                    {
                    }

                    try
                    {
                        if (createdPowerPointApp && powerPointApp != null)
                        {
                            powerPointApp.Quit();
                        }
                    }
                    catch
                    {
                    }
                }

                try
                {
                    if (openedWorkbookHere && workbook != null)
                    {
                        workbook.Close(false);
                    }
                }
                catch
                {
                }

                try
                {
                    if (createdExcelApp && excelApp != null)
                    {
                        excelApp.Quit();
                    }
                }
                catch
                {
                }

                // Rilascia i riferimenti COM usati dal servizio.
                ComReleaseHelper.Release(powerPointWindow);
                ComReleaseHelper.Release(presentation);
                ComReleaseHelper.Release(powerPointApp);
                ComReleaseHelper.Release(workbook);
                ComReleaseHelper.Release(excelApp);
            }
        }

        /// <summary>
        /// Prova ad agganciarsi a un'istanza di Excel già aperta.
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
        /// Prova ad agganciarsi a un'istanza di PowerPoint già aperta.
        /// </summary>
        private PowerPoint.Application TryGetRunningPowerPoint()
        {
            try
            {
                return (PowerPoint.Application)Marshal.GetActiveObject("PowerPoint.Application");
            }
            catch (COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Cerca un workbook già aperto che corrisponda al file Excel richiesto.
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
        /// Restituisce l'ultimo indice di foglio da processare.
        /// Se richiesto, esclude l'ultimo foglio.
        /// </summary>
        private int GetLastSheetIndex(Excel.Workbook workbook, bool skipLastWorksheet)
        {
            int worksheetCount = workbook.Worksheets.Count;

            if (skipLastWorksheet && worksheetCount > 1)
            {
                return worksheetCount - 1;
            }

            return worksheetCount;
        }

        /// <summary>
        /// Conta quanti fogli contengono davvero dati esportabili.
        /// </summary>
        private int CountExportableSheets(Excel.Workbook workbook, int lastSheetIndex)
        {
            int count = 0;

            for (int i = 1; i <= lastSheetIndex; i++)
            {
                Excel.Worksheet worksheet = null;
                Excel.Range realUsedRange = null;

                try
                {
                    worksheet = (Excel.Worksheet)workbook.Worksheets[i];
                    realUsedRange = GetRealUsedRange(worksheet);

                    if (realUsedRange != null)
                    {
                        count++;
                    }
                }
                finally
                {
                    ComReleaseHelper.Release(realUsedRange);
                    ComReleaseHelper.Release(worksheet);
                }
            }

            return count;
        }

        /// <summary>
        /// Restituisce il range realmente usato del foglio:
        /// da A1 fino all'ultima riga e colonna effettivamente valorizzate.
        /// Se il foglio è vuoto, restituisce null.
        /// </summary>
        private Excel.Range GetRealUsedRange(Excel.Worksheet worksheet)
        {
            Excel.Range afterCell = null;
            Excel.Range lastRowCell = null;
            Excel.Range lastColumnCell = null;
            Excel.Range firstCell = null;
            Excel.Range resultRange = null;

            try
            {
                afterCell = worksheet.Cells[1, 1];

                lastRowCell = worksheet.Cells.Find(
                    What: "*",
                    After: afterCell,
                    LookIn: Excel.XlFindLookIn.xlFormulas,
                    LookAt: Excel.XlLookAt.xlPart,
                    SearchOrder: Excel.XlSearchOrder.xlByRows,
                    SearchDirection: Excel.XlSearchDirection.xlPrevious,
                    MatchCase: false,
                    SearchFormat: false);

                lastColumnCell = worksheet.Cells.Find(
                    What: "*",
                    After: afterCell,
                    LookIn: Excel.XlFindLookIn.xlFormulas,
                    LookAt: Excel.XlLookAt.xlPart,
                    SearchOrder: Excel.XlSearchOrder.xlByColumns,
                    SearchDirection: Excel.XlSearchDirection.xlPrevious,
                    MatchCase: false,
                    SearchFormat: false);

                if (lastRowCell == null || lastColumnCell == null)
                {
                    return null;
                }

                firstCell = worksheet.Cells[1, 1];

                resultRange = worksheet.Range[
                    firstCell,
                    worksheet.Cells[lastRowCell.Row, lastColumnCell.Column]];

                return resultRange;
            }
            finally
            {
                ComReleaseHelper.Release(afterCell);
                ComReleaseHelper.Release(lastRowCell);
                ComReleaseHelper.Release(lastColumnCell);
                ComReleaseHelper.Release(firstCell);
            }
        }

        /// <summary>
        /// Tenta il paste in PowerPoint fino a quando la clipboard non è pronta
        /// oppure finché non scade il timeout.
        /// </summary>
        private PowerPoint.ShapeRange PasteBitmapWhenReady(PowerPoint.Slide slide, int timeoutMs)
        {
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.ElapsedMilliseconds < timeoutMs)
            {
                try
                {
                    return slide.Shapes.PasteSpecial(PowerPoint.PpPasteDataType.ppPasteBitmap);
                }
                catch (COMException)
                {
                    Thread.Sleep(100);
                }
            }

            return null;
        }

        /// <summary>
        /// Ridimensiona e centra una shape all'interno della slide,
        /// mantenendo le proporzioni.
        /// </summary>
        private void FitShapeToSlide(PowerPoint.Shape shape, PowerPoint.Presentation presentation)
        {
            float slideWidth = presentation.PageSetup.SlideWidth;
            float slideHeight = presentation.PageSetup.SlideHeight;

            float maxWidth = slideWidth - 10f;
            float maxHeight = slideHeight - 10f;

            shape.LockAspectRatio = Office.MsoTriState.msoTrue;

            if ((shape.Width / shape.Height) > (maxWidth / maxHeight))
            {
                shape.Width = maxWidth;
            }
            else
            {
                shape.Height = maxHeight;
            }

            shape.Left = (slideWidth - shape.Width) / 2f;
            shape.Top = (slideHeight - shape.Height) / 2f;
        }
    }
}