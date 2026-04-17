using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using SlideDeck.Services;

namespace SlideDeck
{
    /// <summary>
    /// Finestra dedicata alla gestione del template locale dell'applicazione.
    /// Da qui l'utente può:
    /// - ripristinare il template di default embedded
    /// - visualizzare i backup disponibili
    /// - ripristinare una versione di backup
    /// </summary>
    public partial class TemplateManagementForm : Form
    {
        private readonly TemplateStorageService _templateStorageService;
        private readonly TemplateBackupService _templateBackupService;

        /// <summary>
        /// Messaggio di risultato esposto al form principale dopo una modifica riuscita.
        /// </summary>
        public string ResultMessage { get; private set; }

        public TemplateManagementForm(
            TemplateStorageService templateStorageService,
            TemplateBackupService templateBackupService)
        {
            InitializeComponent();

            _templateStorageService = templateStorageService
                ?? throw new ArgumentNullException(nameof(templateStorageService));
            _templateBackupService = templateBackupService
                ?? throw new ArgumentNullException(nameof(templateBackupService));

            LoadBackupList();
        }

        /// <summary>
        /// Carica la lista dei backup disponibili dal filesystem.
        /// Per ogni backup vengono mostrati:
        /// - nome file
        /// - data modifica
        /// - dimensione
        /// </summary>
        private void LoadBackupList()
        {
            listViewBackups.BeginUpdate();

            try
            {
                listViewBackups.Items.Clear();

                var backups = _templateBackupService.GetAvailableBackups();

                foreach (FileInfo backupFile in backups)
                {
                    var item = new ListViewItem(backupFile.Name);
                    item.SubItems.Add(
                        backupFile.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture));
                    item.SubItems.Add(FormatFileSize(backupFile.Length));

                    // Salviamo il path completo nell'item per poterlo ripristinare
                    // senza mostrarlo esplicitamente all'utente.
                    item.Tag = backupFile.FullName;

                    listViewBackups.Items.Add(item);
                }

                UpdateBackupSelectionState();

                if (listViewBackups.Items.Count == 0)
                {
                    lblStatus.Text = "Nessun backup disponibile.";
                }
                else
                {
                    lblStatus.Text = $"Backup disponibili: {listViewBackups.Items.Count}.";
                }
            }
            finally
            {
                listViewBackups.EndUpdate();
            }
        }

        /// <summary>
        /// Ripristina il template di default embedded.
        /// Prima del ripristino viene creato automaticamente un backup del file corrente.
        /// </summary>
        private void btnRestoreDefaultTemplate_Click(object sender, EventArgs e)
        {
            DialogResult confirmation = MessageBox.Show(
                "Vuoi ripristinare il template di default?\n\n" +
                "Il file locale corrente verrà sostituito e, se presente, ne verrà creato un backup.",
                "Ripristina template di default",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                lblStatus.Text = "Ripristino del template di default annullato.";
                return;
            }

            try
            {
                _templateStorageService.RestoreDefaultTemplate();

                ResultMessage = "Template di default ripristinato correttamente.";
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Non è stato possibile ripristinare il template.\n\n" +
                    "Verifica che il file Excel locale non sia aperto in Excel e riprova.",
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                lblStatus.Text = "Ripristino non riuscito: file in uso.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Errore durante il ripristino del template di default:\n\n" + ex.Message,
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                lblStatus.Text = "Errore durante il ripristino del template di default.";
            }
        }

        /// <summary>
        /// Ripristina il backup selezionato nella lista.
        /// Prima del ripristino viene creato automaticamente un backup del file corrente.
        /// </summary>
        private void btnRestoreSelectedBackup_Click(object sender, EventArgs e)
        {
            string selectedBackupPath = GetSelectedBackupPath();

            if (string.IsNullOrWhiteSpace(selectedBackupPath))
            {
                MessageBox.Show(
                    "Seleziona prima un backup da ripristinare.",
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Vuoi ripristinare il backup selezionato?\n\n" +
                "Il file locale corrente verrà sostituito e, se presente, ne verrà creato un backup.",
                "Ripristina backup",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                lblStatus.Text = "Ripristino del backup annullato.";
                return;
            }

            try
            {
                _templateBackupService.RestoreBackupAsWorkingTemplate(selectedBackupPath);

                ResultMessage = "Backup ripristinato correttamente.";
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Non è stato possibile ripristinare il backup.\n\n" +
                    "Verifica che il file Excel locale non sia aperto in Excel e riprova.",
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                lblStatus.Text = "Ripristino backup non riuscito: file in uso.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Errore durante il ripristino del backup:\n\n" + ex.Message,
                    "SlideDeck",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                lblStatus.Text = "Errore durante il ripristino del backup.";
            }
        }

        /// <summary>
        /// Ricarica manualmente la lista dei backup dal filesystem.
        /// </summary>
        private void btnRefreshBackups_Click(object sender, EventArgs e)
        {
            LoadBackupList();
        }

        /// <summary>
        /// Aggiorna lo stato dei pulsanti in base alla selezione corrente.
        /// </summary>
        private void listViewBackups_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBackupSelectionState();
        }

        /// <summary>
        /// Chiude semplicemente la finestra di gestione template.
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Aggiorna lo stato abilitato/disabilitato del pulsante di ripristino backup
        /// in base alla selezione presente nella lista.
        /// </summary>
        private void UpdateBackupSelectionState()
        {
            btnRestoreSelectedBackup.Enabled = listViewBackups.SelectedItems.Count > 0;
        }

        /// <summary>
        /// Restituisce il path completo del backup selezionato nella lista.
        /// </summary>
        private string GetSelectedBackupPath()
        {
            if (listViewBackups.SelectedItems.Count == 0)
            {
                return null;
            }

            return listViewBackups.SelectedItems[0].Tag as string;
        }

        /// <summary>
        /// Formatta la dimensione del file in modo leggibile.
        /// Mostra B, KB, MB o GB a seconda del valore.
        /// </summary>
        private string FormatFileSize(long byteCount)
        {
            const double oneKb = 1024d;
            const double oneMb = oneKb * 1024d;
            const double oneGb = oneMb * 1024d;

            if (byteCount >= oneGb)
            {
                return $"{byteCount / oneGb:0.00} GB";
            }

            if (byteCount >= oneMb)
            {
                return $"{byteCount / oneMb:0.00} MB";
            }

            if (byteCount >= oneKb)
            {
                return $"{byteCount / oneKb:0.00} KB";
            }

            return $"{byteCount} B";
        }
    }
}