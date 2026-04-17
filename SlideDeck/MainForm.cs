using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlideDeck.Models;
using SlideDeck.Services;

namespace SlideDeck
{
    /// <summary>
    /// Form principale dell'applicazione SlideDeck.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Testo di stato standard mostrato quando l'app è inattiva.
        /// </summary>
        private const string DefaultStatusText = "Pronto per nuove azioni...";

        /// <summary>
        /// Messaggio mostrato quando l'utente sta trascinando un file valido
        /// sopra la finestra dell'applicazione.
        /// </summary>
        private const string DragDropReadyStatusText =
            "Rilascia il file .xlsx per sostituire il template locale.";

        /// <summary>
        /// Colore usato quando l'utente trascina un file valido sopra la finestra.
        /// </summary>
        private readonly Color _dragActiveBackColor = Color.FromArgb(236, 244, 255);

        private readonly TemplateBackupService _templateBackupService;
        private readonly ExcelPreviewService _excelPreviewService;
        private readonly PowerPointExportService _powerPointExportService;
        private readonly TemplateStorageService _templateStorageService;

        /// <summary>
        /// Indica se il form è attualmente occupato in un'operazione lunga.
        /// Serve anche per bloccare il drag and drop durante le operazioni.
        /// </summary>
        private bool _isBusy;

        /// <summary>
        /// Colore di default dello sfondo del form.
        /// </summary>
        private Color _defaultFormBackColor;

        /// <summary>
        /// Colore di default dello sfondo del layout principale.
        /// </summary>
        private Color _defaultMainTableBackColor;

        public MainForm()
        {
            InitializeComponent();

            _templateBackupService = new TemplateBackupService();
            _excelPreviewService = new ExcelPreviewService(_templateBackupService);
            _powerPointExportService = new PowerPointExportService();
            _templateStorageService = new TemplateStorageService(_templateBackupService);

            Text = "SlideDeck";

            ConfigureToolTips();
            ConfigureFormWideDragAndDrop();
        }

        /// <summary>
        /// Configura i tooltip dei controlli principali.
        /// I tooltip spiegano in modo rapido il comportamento delle funzioni disponibili.
        /// </summary>
        private void ConfigureToolTips()
        {
            toolTipMain.SetToolTip(
                btnPreviewSlides,
                "Apre il file Excel locale di lavoro e blocca l'interfaccia finché il file resta aperto.");

            toolTipMain.SetToolTip(
                btnCreatePowerPoint,
                "Genera una presentazione PowerPoint leggendo i fogli del file Excel locale.");

            toolTipMain.SetToolTip(
                btnManageTemplate,
                "Apre la finestra di gestione del template, da cui puoi ripristinare il default o una versione di backup.");

            toolTipMain.SetToolTip(
                tableMain,
                "Puoi trascinare un file .xlsx in qualsiasi punto della finestra per sostituire il template locale.");

            toolTipMain.SetToolTip(
                lblSubtitle,
                "Suggerimento: trascina un file .xlsx direttamente sopra la finestra per caricare un template personalizzato.");
        }

        /// <summary>
        /// Abilita il drag and drop su tutta la finestra.
        /// Per ottenere un comportamento davvero globale, registriamo gli handler
        /// ricorsivamente su tutti i controlli contenuti nel form.
        /// </summary>
        private void ConfigureFormWideDragAndDrop()
        {
            _defaultFormBackColor = BackColor;
            _defaultMainTableBackColor = tableMain.BackColor;

            RegisterDragDropHandlersRecursively(this);
        }

        /// <summary>
        /// Registra ricorsivamente gli handler drag and drop su un controllo
        /// e su tutti i suoi controlli figli.
        /// Questo permette di accettare il drop in qualunque punto della UI.
        /// </summary>
        private void RegisterDragDropHandlersRecursively(Control control)
        {
            control.AllowDrop = true;
            control.DragEnter += GlobalTemplateDragTarget_DragEnter;
            control.DragLeave += GlobalTemplateDragTarget_DragLeave;
            control.DragDrop += GlobalTemplateDragTarget_DragDrop;

            foreach (Control childControl in control.Controls)
            {
                RegisterDragDropHandlersRecursively(childControl);
            }
        }

        /// <summary>
        /// Apre il file Excel di lavoro e mantiene la UI bloccata
        /// finché il workbook non viene chiuso davvero.
        /// Durante la sessione di editing vengono creati backup automatici
        /// ad ogni salvataggio.
        /// </summary>
        private async void btnPreviewSlides_Click(object sender, EventArgs e)
        {
            try
            {
                await ExecuteBusyActionAsync(async () =>
                {
                    string templatePath = _templateStorageService.EnsureWorkingTemplateExists();
                    await _excelPreviewService.OpenTemplateForEditingAsync(templatePath);
                }, "Apertura del file Excel in corso...");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Errore durante l'apertura di Excel:\n\n" + ex.Message,
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Crea la presentazione PowerPoint a partire dal file Excel locale.
        /// </summary>
        private async void btnCreatePowerPoint_Click(object sender, EventArgs e)
        {
            try
            {
                await ExecuteBusyActionAsync(async () =>
                {
                    string templatePath = _templateStorageService.EnsureWorkingTemplateExists();

                    var options = new ExportOptions
                    {
                        ExcelPath = templatePath,
                        SkipLastWorksheet = true,
                        PasteTimeoutMs = 60000
                    };

                    await _powerPointExportService.ExportAsync(options);
                }, "Creazione del PowerPoint in corso...");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Errore durante la creazione del PowerPoint:\n\n" + ex.Message,
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Apre la finestra di gestione del template.
        /// Da lì l'utente può:
        /// - ripristinare il template di default
        /// - ripristinare uno dei backup disponibili
        /// </summary>
        private void btnManageTemplate_Click(object sender, EventArgs e)
        {
            using (var form = new TemplateManagementForm(
                _templateStorageService,
                _templateBackupService))
            {
                // Allinea l'icona del form secondario a quella del form principale.
                form.Icon = Icon;

                DialogResult result = form.ShowDialog(this);

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(form.ResultMessage))
                {
                    lblStatus.Text = form.ResultMessage;
                }
            }
        }

        /// <summary>
        /// Gestisce l'ingresso del drag sopra qualunque controllo del form.
        /// Accetta solo un singolo file .xlsx e solo se l'app non è occupata.
        /// </summary>
        private void GlobalTemplateDragTarget_DragEnter(object sender, DragEventArgs e)
        {
            if (_isBusy)
            {
                e.Effect = DragDropEffects.None;
                UpdateGlobalDragAppearance(isActive: false);
                return;
            }

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                UpdateGlobalDragAppearance(isActive: false);
                return;
            }

            string droppedFile = TryGetSingleDroppedFile(e);

            if (string.IsNullOrWhiteSpace(droppedFile) ||
                !_templateStorageService.IsSupportedTemplateFile(droppedFile))
            {
                e.Effect = DragDropEffects.None;
                UpdateGlobalDragAppearance(isActive: false);
                return;
            }

            e.Effect = DragDropEffects.Copy;
            UpdateGlobalDragAppearance(isActive: true);
        }

        /// <summary>
        /// Gestisce l'uscita del drag da un controllo della finestra.
        /// Ripristina l'aspetto grafico normale.
        /// </summary>
        private void GlobalTemplateDragTarget_DragLeave(object sender, EventArgs e)
        {
            UpdateGlobalDragAppearance(isActive: false);
        }

        /// <summary>
        /// Gestisce il rilascio effettivo del file trascinato in qualunque punto del form.
        /// Chiede conferma all'utente e, se conferma, sostituisce il template locale.
        /// Prima della sostituzione viene creato un backup del file corrente.
        /// </summary>
        private async void GlobalTemplateDragTarget_DragDrop(object sender, DragEventArgs e)
        {
            UpdateGlobalDragAppearance(isActive: false);

            if (_isBusy)
            {
                return;
            }

            string droppedFile = TryGetSingleDroppedFile(e);

            if (string.IsNullOrWhiteSpace(droppedFile))
            {
                MessageBox.Show(
                    "Non è stato possibile leggere il file trascinato.",
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!_templateStorageService.IsSupportedTemplateFile(droppedFile))
            {
                MessageBox.Show(
                    "Il file selezionato non è supportato.\n\nPer ora sono accettati solo file .xlsx.",
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Vuoi sostituire il template locale corrente con il file trascinato?\n\n" +
                "Il nuovo file diventerà il template usato da SlideDeck.\n" +
                "Prima della sostituzione verrà creato un backup del file corrente, se presente.",
                "Conferma caricamento template",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                lblStatus.Text = "Caricamento template annullato.";
                return;
            }

            try
            {
                await ExecuteBusyActionAsync(async () =>
                {
                    _templateStorageService.ReplaceWorkingTemplate(droppedFile);
                    await Task.CompletedTask;
                }, "Caricamento del template custom in corso...");

                lblStatus.Text = "Template custom caricato correttamente.";
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Non è stato possibile sostituire il template.\n\n" +
                    "Verifica che il file Excel locale non sia aperto in Excel e riprova.",
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                lblStatus.Text = "Impossibile sostituire il template: file in uso.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Errore durante il caricamento del template custom:\n\n" + ex.Message,
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                lblStatus.Text = "Errore durante il caricamento del template.";
            }
        }

        /// <summary>
        /// Restituisce il path del singolo file trascinato, se presente.
        /// Se i file sono zero o più di uno, restituisce null.
        /// </summary>
        private string TryGetSingleDroppedFile(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return null;
            }

            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (files == null || files.Length != 1)
            {
                return null;
            }

            return files[0];
        }

        /// <summary>
        /// Aggiorna l'aspetto dell'intera finestra durante il drag and drop.
        /// In questo modo l'utente percepisce chiaramente che può rilasciare
        /// il file in qualsiasi punto del form.
        /// </summary>
        private void UpdateGlobalDragAppearance(bool isActive)
        {
            if (isActive)
            {
                BackColor = _dragActiveBackColor;
                tableMain.BackColor = _dragActiveBackColor;

                if (!_isBusy)
                {
                    lblStatus.Text = DragDropReadyStatusText;
                }
            }
            else
            {
                BackColor = _defaultFormBackColor;
                tableMain.BackColor = _defaultMainTableBackColor;

                if (!_isBusy && string.Equals(lblStatus.Text, DragDropReadyStatusText, StringComparison.Ordinal))
                {
                    lblStatus.Text = DefaultStatusText;
                }
            }
        }

        /// <summary>
        /// Esegue un'operazione lunga gestendo in modo uniforme
        /// lo stato busy della UI e il messaggio di stato.
        /// </summary>
        private async Task ExecuteBusyActionAsync(Func<Task> action, string busyMessage)
        {
            _isBusy = true;
            ToggleButtons(false);
            Cursor = Cursors.WaitCursor;
            lblStatus.Text = busyMessage;

            try
            {
                await action();
                lblStatus.Text = "Operazione completata.";
            }
            catch
            {
                lblStatus.Text = "Operazione terminata con errore.";
                throw;
            }
            finally
            {
                _isBusy = false;
                Cursor = Cursors.Default;
                ToggleButtons(true);
                UpdateGlobalDragAppearance(isActive: false);
            }
        }

        /// <summary>
        /// Abilita o disabilita i pulsanti principali del form.
        /// </summary>
        private void ToggleButtons(bool enabled)
        {
            btnPreviewSlides.Enabled = enabled;
            btnCreatePowerPoint.Enabled = enabled;
            btnManageTemplate.Enabled = enabled;
        }
    }
}