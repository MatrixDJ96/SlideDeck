namespace SlideDeck
{
    partial class TemplateManagementForm
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Libera le risorse in uso.
        /// </summary>
        /// <param name="disposing">
        /// true se le risorse gestite devono essere eliminate; false altrimenti.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo richiesto dal designer.
        /// Non modificare il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelActions = new System.Windows.Forms.TableLayoutPanel();
            this.panelLeftActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefreshBackups = new System.Windows.Forms.Button();
            this.panelRightActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRestoreDefaultTemplate = new System.Windows.Forms.Button();
            this.groupBackups = new System.Windows.Forms.GroupBox();
            this.listViewBackups = new System.Windows.Forms.ListView();
            this.columnFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnModifiedAt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelBottomActions = new System.Windows.Forms.TableLayoutPanel();
            this.panelBottomLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRestoreSelectedBackup = new System.Windows.Forms.Button();
            this.panelBottomRight = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tableMain.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.panelLeftActions.SuspendLayout();
            this.panelRightActions.SuspendLayout();
            this.groupBackups.SuspendLayout();
            this.panelBottomActions.SuspendLayout();
            this.panelBottomLeft.SuspendLayout();
            this.panelBottomRight.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();

            // 
            // tableMain
            // 
            // Layout principale della finestra di gestione template.
            // La finestra è volutamente più alta che larga e lascia molto spazio
            // alla lista centrale dei backup.
            this.tableMain.ColumnCount = 1;
            this.tableMain.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.panelHeader, 0, 0);
            this.tableMain.Controls.Add(this.panelActions, 0, 1);
            this.tableMain.Controls.Add(this.groupBackups, 0, 2);
            this.tableMain.Controls.Add(this.panelBottomActions, 0, 3);
            this.tableMain.Controls.Add(this.panelFooter, 0, 4);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Name = "tableMain";
            this.tableMain.Padding = new System.Windows.Forms.Padding(16);
            this.tableMain.RowCount = 5;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableMain.Size = new System.Drawing.Size(560, 680);
            this.tableMain.TabIndex = 0;

            // 
            // panelHeader
            // 
            // Header della finestra con titolo e descrizione.
            this.panelHeader.AutoSize = true;
            this.panelHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelHeader.Controls.Add(this.lblSubtitle);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(19, 19);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(522, 58);
            this.panelHeader.TabIndex = 0;

            // 
            // lblSubtitle
            // 
            // Testo descrittivo delle opzioni disponibili.
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblSubtitle.Location = new System.Drawing.Point(2, 36);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(408, 13);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Ripristina il template di default oppure scegli una versione di backup già presen" +
    "te";

            // 
            // lblTitle
            // 
            // Titolo principale della finestra.
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font(
                "Segoe UI",
                14F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(173, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Gestione Template";

            // 
            // panelActions
            // 
            // Riga azioni superiore.
            // A sinistra il refresh, a destra il ripristino default.
            this.panelActions.ColumnCount = 2;
            this.panelActions.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelActions.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelActions.Controls.Add(this.panelLeftActions, 0, 0);
            this.panelActions.Controls.Add(this.panelRightActions, 1, 0);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelActions.Location = new System.Drawing.Point(19, 90);
            this.panelActions.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.panelActions.Name = "panelActions";
            this.panelActions.RowCount = 1;
            this.panelActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelActions.Size = new System.Drawing.Size(522, 48);
            this.panelActions.TabIndex = 1;

            // 
            // panelLeftActions
            // 
            // Area azioni sinistra della riga superiore.
            this.panelLeftActions.AutoSize = true;
            this.panelLeftActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelLeftActions.Controls.Add(this.btnRefreshBackups);
            this.panelLeftActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftActions.Location = new System.Drawing.Point(0, 0);
            this.panelLeftActions.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeftActions.Name = "panelLeftActions";
            this.panelLeftActions.Size = new System.Drawing.Size(261, 48);
            this.panelLeftActions.TabIndex = 0;

            // 
            // btnRefreshBackups
            // 
            // Pulsante per ricaricare la lista dei backup.
            this.btnRefreshBackups.AutoSize = true;
            this.btnRefreshBackups.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefreshBackups.BackColor = System.Drawing.Color.FromArgb(90, 102, 118);
            this.btnRefreshBackups.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshBackups.FlatAppearance.BorderSize = 0;
            this.btnRefreshBackups.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshBackups.Font = new System.Drawing.Font(
                "Segoe UI",
                9F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.btnRefreshBackups.ForeColor = System.Drawing.Color.White;
            this.btnRefreshBackups.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefreshBackups.MinimumSize = new System.Drawing.Size(120, 42);
            this.btnRefreshBackups.Name = "btnRefreshBackups";
            this.btnRefreshBackups.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.btnRefreshBackups.Size = new System.Drawing.Size(120, 42);
            this.btnRefreshBackups.TabIndex = 0;
            this.btnRefreshBackups.Text = "Aggiorna Elenco";
            this.btnRefreshBackups.UseVisualStyleBackColor = false;
            this.btnRefreshBackups.Click += new System.EventHandler(this.btnRefreshBackups_Click);

            // 
            // panelRightActions
            // 
            // Area azioni destra della riga superiore.
            // Qui teniamo separata la logica di ripristino default.
            this.panelRightActions.AutoSize = true;
            this.panelRightActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelRightActions.Controls.Add(this.btnRestoreDefaultTemplate);
            this.panelRightActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelRightActions.Location = new System.Drawing.Point(261, 0);
            this.panelRightActions.Margin = new System.Windows.Forms.Padding(0);
            this.panelRightActions.Name = "panelRightActions";
            this.panelRightActions.Size = new System.Drawing.Size(261, 48);
            this.panelRightActions.TabIndex = 1;

            // 
            // btnRestoreDefaultTemplate
            // 
            // Pulsante per ripristinare il template embedded di default.
            this.btnRestoreDefaultTemplate.AutoSize = true;
            this.btnRestoreDefaultTemplate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRestoreDefaultTemplate.BackColor = System.Drawing.Color.FromArgb(220, 120, 40);
            this.btnRestoreDefaultTemplate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestoreDefaultTemplate.FlatAppearance.BorderSize = 0;
            this.btnRestoreDefaultTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestoreDefaultTemplate.Font = new System.Drawing.Font(
                "Segoe UI",
                9F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.btnRestoreDefaultTemplate.ForeColor = System.Drawing.Color.White;
            this.btnRestoreDefaultTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.btnRestoreDefaultTemplate.MinimumSize = new System.Drawing.Size(140, 42);
            this.btnRestoreDefaultTemplate.Name = "btnRestoreDefaultTemplate";
            this.btnRestoreDefaultTemplate.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.btnRestoreDefaultTemplate.Size = new System.Drawing.Size(140, 42);
            this.btnRestoreDefaultTemplate.TabIndex = 0;
            this.btnRestoreDefaultTemplate.Text = "Ripristina Default";
            this.btnRestoreDefaultTemplate.UseVisualStyleBackColor = false;
            this.btnRestoreDefaultTemplate.Click += new System.EventHandler(this.btnRestoreDefaultTemplate_Click);

            // 
            // groupBackups
            // 
            // Gruppo che contiene la lista dei backup disponibili.
            this.groupBackups.Controls.Add(this.listViewBackups);
            this.groupBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBackups.Location = new System.Drawing.Point(19, 151);
            this.groupBackups.Name = "groupBackups";
            this.groupBackups.Padding = new System.Windows.Forms.Padding(12);
            this.groupBackups.Size = new System.Drawing.Size(522, 442);
            this.groupBackups.TabIndex = 2;
            this.groupBackups.TabStop = false;
            this.groupBackups.Text = "Versioni di backup disponibili";

            // 
            // listViewBackups
            // 
            // Lista dei backup con i metadati richiesti:
            // - nome file
            // - data modifica
            // - dimensione
            // Il ListView gestisce automaticamente lo scrolling verticale
            // quando il numero di elementi cresce.
            this.listViewBackups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFileName,
            this.columnModifiedAt,
            this.columnSize});
            this.listViewBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewBackups.FullRowSelect = true;
            this.listViewBackups.GridLines = true;
            this.listViewBackups.HideSelection = false;
            this.listViewBackups.Location = new System.Drawing.Point(12, 25);
            this.listViewBackups.MultiSelect = false;
            this.listViewBackups.Name = "listViewBackups";
            this.listViewBackups.Size = new System.Drawing.Size(498, 405);
            this.listViewBackups.TabIndex = 0;
            this.listViewBackups.UseCompatibleStateImageBehavior = false;
            this.listViewBackups.View = System.Windows.Forms.View.Details;
            this.listViewBackups.SelectedIndexChanged += new System.EventHandler(this.listViewBackups_SelectedIndexChanged);

            // 
            // columnFileName
            // 
            // Colonna nome file backup.
            this.columnFileName.Text = "Nome file";
            this.columnFileName.Width = 220;

            // 
            // columnModifiedAt
            // 
            // Colonna data ultima modifica backup.
            this.columnModifiedAt.Text = "Modificato il";
            this.columnModifiedAt.Width = 180;

            // 
            // columnSize
            // 
            // Colonna dimensione file backup.
            this.columnSize.Text = "Dimensione";
            this.columnSize.Width = 90;

            // 
            // panelBottomActions
            // 
            // Riga azioni inferiore.
            // A sinistra il ripristino backup, a destra la chiusura.
            this.panelBottomActions.ColumnCount = 2;
            this.panelBottomActions.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelBottomActions.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelBottomActions.Controls.Add(this.panelBottomLeft, 0, 0);
            this.panelBottomActions.Controls.Add(this.panelBottomRight, 1, 0);
            this.panelBottomActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottomActions.Location = new System.Drawing.Point(19, 606);
            this.panelBottomActions.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.panelBottomActions.Name = "panelBottomActions";
            this.panelBottomActions.RowCount = 1;
            this.panelBottomActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.panelBottomActions.Size = new System.Drawing.Size(522, 48);
            this.panelBottomActions.TabIndex = 3;

            // 
            // panelBottomLeft
            // 
            // Area azioni inferiore sinistra.
            this.panelBottomLeft.AutoSize = true;
            this.panelBottomLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBottomLeft.Controls.Add(this.btnRestoreSelectedBackup);
            this.panelBottomLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottomLeft.Location = new System.Drawing.Point(0, 0);
            this.panelBottomLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottomLeft.Name = "panelBottomLeft";
            this.panelBottomLeft.Size = new System.Drawing.Size(261, 48);
            this.panelBottomLeft.TabIndex = 0;

            // 
            // btnRestoreSelectedBackup
            // 
            // Pulsante per ripristinare il backup selezionato.
            this.btnRestoreSelectedBackup.AutoSize = true;
            this.btnRestoreSelectedBackup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRestoreSelectedBackup.BackColor = System.Drawing.Color.FromArgb(52, 120, 246);
            this.btnRestoreSelectedBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestoreSelectedBackup.Enabled = false;
            this.btnRestoreSelectedBackup.FlatAppearance.BorderSize = 0;
            this.btnRestoreSelectedBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestoreSelectedBackup.Font = new System.Drawing.Font(
                "Segoe UI",
                9F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.btnRestoreSelectedBackup.ForeColor = System.Drawing.Color.White;
            this.btnRestoreSelectedBackup.Margin = new System.Windows.Forms.Padding(0);
            this.btnRestoreSelectedBackup.MinimumSize = new System.Drawing.Size(140, 42);
            this.btnRestoreSelectedBackup.Name = "btnRestoreSelectedBackup";
            this.btnRestoreSelectedBackup.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.btnRestoreSelectedBackup.Size = new System.Drawing.Size(140, 42);
            this.btnRestoreSelectedBackup.TabIndex = 0;
            this.btnRestoreSelectedBackup.Text = "Ripristina Backup";
            this.btnRestoreSelectedBackup.UseVisualStyleBackColor = false;
            this.btnRestoreSelectedBackup.Click += new System.EventHandler(this.btnRestoreSelectedBackup_Click);

            // 
            // panelBottomRight
            // 
            // Area azioni inferiore destra.
            // Qui il pulsante Chiudi è posizionato a destra con colore dedicato.
            this.panelBottomRight.AutoSize = true;
            this.panelBottomRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBottomRight.Controls.Add(this.btnClose);
            this.panelBottomRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottomRight.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelBottomRight.Location = new System.Drawing.Point(261, 0);
            this.panelBottomRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottomRight.Name = "panelBottomRight";
            this.panelBottomRight.Size = new System.Drawing.Size(261, 48);
            this.panelBottomRight.TabIndex = 1;

            // 
            // btnClose
            // 
            // Pulsante di chiusura finestra.
            this.btnClose.AutoSize = true;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font(
                "Segoe UI",
                9F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.MinimumSize = new System.Drawing.Size(100, 42);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.btnClose.Size = new System.Drawing.Size(100, 42);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Chiudi";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // 
            // panelFooter
            // 
            // Footer con messaggio di stato locale alla finestra.
            this.panelFooter.AutoSize = true;
            this.panelFooter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFooter.Controls.Add(this.lblStatus);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(19, 667);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(522, 3);
            this.panelFooter.TabIndex = 4;

            // 
            // lblStatus
            // 
            // Etichetta di stato iniziale della finestra.
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblStatus.Location = new System.Drawing.Point(2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(110, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Caricamento backup...";

            // 
            // TemplateManagementForm
            // 
            // Configurazione generale della finestra di gestione template.
            // La finestra è volutamente più alta che larga.
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 680);
            this.Controls.Add(this.tableMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(576, 719);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(576, 719);
            this.Name = "TemplateManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestione Template";
            this.tableMain.ResumeLayout(false);
            this.tableMain.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelActions.ResumeLayout(false);
            this.panelActions.PerformLayout();
            this.panelLeftActions.ResumeLayout(false);
            this.panelLeftActions.PerformLayout();
            this.panelRightActions.ResumeLayout(false);
            this.panelRightActions.PerformLayout();
            this.groupBackups.ResumeLayout(false);
            this.panelBottomActions.ResumeLayout(false);
            this.panelBottomActions.PerformLayout();
            this.panelBottomLeft.ResumeLayout(false);
            this.panelBottomLeft.PerformLayout();
            this.panelBottomRight.ResumeLayout(false);
            this.panelBottomRight.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// Layout principale della finestra.
        /// </summary>
        private System.Windows.Forms.TableLayoutPanel tableMain;

        /// <summary>
        /// Header con titolo e sottotitolo.
        /// </summary>
        private System.Windows.Forms.Panel panelHeader;

        /// <summary>
        /// Titolo principale della finestra.
        /// </summary>
        private System.Windows.Forms.Label lblTitle;

        /// <summary>
        /// Sottotitolo descrittivo.
        /// </summary>
        private System.Windows.Forms.Label lblSubtitle;

        /// <summary>
        /// Riga azioni superiore.
        /// </summary>
        private System.Windows.Forms.TableLayoutPanel panelActions;

        /// <summary>
        /// Area azioni sinistra della riga superiore.
        /// </summary>
        private System.Windows.Forms.FlowLayoutPanel panelLeftActions;

        /// <summary>
        /// Area azioni destra della riga superiore.
        /// </summary>
        private System.Windows.Forms.FlowLayoutPanel panelRightActions;

        /// <summary>
        /// Pulsante per ripristinare il template di default.
        /// </summary>
        private System.Windows.Forms.Button btnRestoreDefaultTemplate;

        /// <summary>
        /// Pulsante per aggiornare la lista dei backup.
        /// </summary>
        private System.Windows.Forms.Button btnRefreshBackups;

        /// <summary>
        /// Gruppo che contiene la lista dei backup.
        /// </summary>
        private System.Windows.Forms.GroupBox groupBackups;

        /// <summary>
        /// Lista dei backup disponibili.
        /// </summary>
        private System.Windows.Forms.ListView listViewBackups;

        /// <summary>
        /// Colonna nome file backup.
        /// </summary>
        private System.Windows.Forms.ColumnHeader columnFileName;

        /// <summary>
        /// Colonna data modifica backup.
        /// </summary>
        private System.Windows.Forms.ColumnHeader columnModifiedAt;

        /// <summary>
        /// Colonna dimensione backup.
        /// </summary>
        private System.Windows.Forms.ColumnHeader columnSize;

        /// <summary>
        /// Riga azioni inferiore.
        /// </summary>
        private System.Windows.Forms.TableLayoutPanel panelBottomActions;

        /// <summary>
        /// Area azioni inferiore sinistra.
        /// </summary>
        private System.Windows.Forms.FlowLayoutPanel panelBottomLeft;

        /// <summary>
        /// Area azioni inferiore destra.
        /// </summary>
        private System.Windows.Forms.FlowLayoutPanel panelBottomRight;

        /// <summary>
        /// Pulsante per ripristinare il backup selezionato.
        /// </summary>
        private System.Windows.Forms.Button btnRestoreSelectedBackup;

        /// <summary>
        /// Pulsante di chiusura finestra.
        /// </summary>
        private System.Windows.Forms.Button btnClose;

        /// <summary>
        /// Footer con stato locale.
        /// </summary>
        private System.Windows.Forms.Panel panelFooter;

        /// <summary>
        /// Etichetta di stato della finestra.
        /// </summary>
        private System.Windows.Forms.Label lblStatus;
    }
}