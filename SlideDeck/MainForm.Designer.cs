namespace SlideDeck
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.flowActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPreviewSlides = new System.Windows.Forms.Button();
            this.btnCreatePowerPoint = new System.Windows.Forms.Button();
            this.btnManageTemplate = new System.Windows.Forms.Button();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.tableMain.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.flowActions.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();

            // 
            // toolTipMain
            // 
            // Tooltip principale dell'applicazione.
            // Viene usato per spiegare rapidamente le funzioni dei controlli.
            this.toolTipMain.AutoPopDelay = 9000;
            this.toolTipMain.InitialDelay = 300;
            this.toolTipMain.IsBalloon = true;
            this.toolTipMain.ReshowDelay = 100;
            this.toolTipMain.ShowAlways = true;
            this.toolTipMain.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipMain.ToolTipTitle = "SlideDeck";

            // 
            // tableMain
            // 
            // Layout principale del form:
            // - intestazione
            // - area pulsanti
            // - footer stato
            this.tableMain.AutoSize = true;
            this.tableMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableMain.ColumnCount = 1;
            this.tableMain.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.panelHeader, 0, 0);
            this.tableMain.Controls.Add(this.flowActions, 0, 1);
            this.tableMain.Controls.Add(this.panelFooter, 0, 2);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Margin = new System.Windows.Forms.Padding(0);
            this.tableMain.Name = "tableMain";
            this.tableMain.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.tableMain.RowCount = 3;
            this.tableMain.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableMain.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableMain.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableMain.Size = new System.Drawing.Size(620, 235);
            this.tableMain.TabIndex = 0;

            // 
            // panelHeader
            // 
            // Header superiore con titolo e descrizione.
            this.panelHeader.AutoSize = true;
            this.panelHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelHeader.Controls.Add(this.lblSubtitle);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(23, 21);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(3, 3, 3, 14);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(574, 60);
            this.panelHeader.TabIndex = 0;

            // 
            // lblSubtitle
            // 
            // Sottotitolo con descrizione delle azioni disponibili.
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblSubtitle.Location = new System.Drawing.Point(2, 39);
            this.lblSubtitle.Margin = new System.Windows.Forms.Padding(2, 8, 2, 0);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(427, 13);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Usa i pulsanti oppure trascina un file .xlsx in qualsiasi punto della finestra pe" +
    "r aggiornare il template";

            // 
            // lblTitle
            // 
            // Titolo principale dell'app.
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font(
                "Segoe UI",
                16F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(120, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "SlideDeck";

            // 
            // flowActions
            // 
            // Contenitore responsive dei pulsanti principali.
            this.flowActions.AutoSize = true;
            this.flowActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowActions.Controls.Add(this.btnPreviewSlides);
            this.flowActions.Controls.Add(this.btnCreatePowerPoint);
            this.flowActions.Controls.Add(this.btnManageTemplate);
            this.flowActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowActions.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowActions.Location = new System.Drawing.Point(23, 98);
            this.flowActions.Margin = new System.Windows.Forms.Padding(3);
            this.flowActions.Name = "flowActions";
            this.flowActions.Size = new System.Drawing.Size(574, 96);
            this.flowActions.TabIndex = 1;
            this.flowActions.WrapContents = true;

            // 
            // btnPreviewSlides
            // 
            // Pulsante per aprire il file Excel locale.
            // Colore ispirato all'identità visiva di Excel.
            this.btnPreviewSlides.AutoSize = true;
            this.btnPreviewSlides.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPreviewSlides.BackColor = System.Drawing.Color.FromArgb(33, 115, 70);
            this.btnPreviewSlides.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPreviewSlides.FlatAppearance.BorderSize = 0;
            this.btnPreviewSlides.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviewSlides.Font = new System.Drawing.Font(
                "Segoe UI",
                10F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.btnPreviewSlides.ForeColor = System.Drawing.Color.White;
            this.btnPreviewSlides.Margin = new System.Windows.Forms.Padding(0, 0, 14, 14);
            this.btnPreviewSlides.MinimumSize = new System.Drawing.Size(150, 78);
            this.btnPreviewSlides.Name = "btnPreviewSlides";
            this.btnPreviewSlides.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.btnPreviewSlides.Size = new System.Drawing.Size(150, 78);
            this.btnPreviewSlides.TabIndex = 0;
            this.btnPreviewSlides.Text = "Visualizza Excel";
            this.btnPreviewSlides.UseVisualStyleBackColor = false;
            this.btnPreviewSlides.Click += new System.EventHandler(this.btnPreviewSlides_Click);

            // 
            // btnCreatePowerPoint
            // 
            // Pulsante per creare il PowerPoint.
            // Colore ispirato all'identità visiva di PowerPoint.
            this.btnCreatePowerPoint.AutoSize = true;
            this.btnCreatePowerPoint.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCreatePowerPoint.BackColor = System.Drawing.Color.FromArgb(211, 69, 32);
            this.btnCreatePowerPoint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreatePowerPoint.FlatAppearance.BorderSize = 0;
            this.btnCreatePowerPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreatePowerPoint.Font = new System.Drawing.Font(
                "Segoe UI",
                10F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.btnCreatePowerPoint.ForeColor = System.Drawing.Color.White;
            this.btnCreatePowerPoint.Margin = new System.Windows.Forms.Padding(0, 0, 14, 14);
            this.btnCreatePowerPoint.MinimumSize = new System.Drawing.Size(150, 78);
            this.btnCreatePowerPoint.Name = "btnCreatePowerPoint";
            this.btnCreatePowerPoint.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.btnCreatePowerPoint.Size = new System.Drawing.Size(150, 78);
            this.btnCreatePowerPoint.TabIndex = 1;
            this.btnCreatePowerPoint.Text = "Crea PowerPoint";
            this.btnCreatePowerPoint.UseVisualStyleBackColor = false;
            this.btnCreatePowerPoint.Click += new System.EventHandler(this.btnCreatePowerPoint_Click);

            // 
            // btnManageTemplate
            // 
            // Pulsante per aprire la finestra di gestione del template.
            this.btnManageTemplate.AutoSize = true;
            this.btnManageTemplate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnManageTemplate.BackColor = System.Drawing.Color.FromArgb(90, 102, 118);
            this.btnManageTemplate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManageTemplate.FlatAppearance.BorderSize = 0;
            this.btnManageTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageTemplate.Font = new System.Drawing.Font(
                "Segoe UI",
                10F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.btnManageTemplate.ForeColor = System.Drawing.Color.White;
            this.btnManageTemplate.Margin = new System.Windows.Forms.Padding(0, 0, 14, 14);
            this.btnManageTemplate.MinimumSize = new System.Drawing.Size(150, 78);
            this.btnManageTemplate.Name = "btnManageTemplate";
            this.btnManageTemplate.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.btnManageTemplate.Size = new System.Drawing.Size(150, 78);
            this.btnManageTemplate.TabIndex = 2;
            this.btnManageTemplate.Text = "Gestione Template";
            this.btnManageTemplate.UseVisualStyleBackColor = false;
            this.btnManageTemplate.Click += new System.EventHandler(this.btnManageTemplate_Click);

            // 
            // panelFooter
            // 
            // Footer con messaggio di stato.
            this.panelFooter.AutoSize = true;
            this.panelFooter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFooter.Controls.Add(this.lblStatus);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(23, 211);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(3, 14, 3, 3);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(574, 3);
            this.panelFooter.TabIndex = 2;

            // 
            // lblStatus
            // 
            // Etichetta di stato iniziale.
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblStatus.Location = new System.Drawing.Point(2, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(131, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Pronto per nuove azioni...";

            // 
            // MainForm
            // 
            // Configurazione generale del form.
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(620, 235);
            this.Controls.Add(this.tableMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.MinimumSize = new System.Drawing.Size(636, 274);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SlideDeck";
            this.tableMain.ResumeLayout(false);
            this.tableMain.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.flowActions.ResumeLayout(false);
            this.flowActions.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        /// <summary>
        /// Layout principale del form.
        /// </summary>
        private System.Windows.Forms.TableLayoutPanel tableMain;

        /// <summary>
        /// Pannello header con titolo e descrizione.
        /// </summary>
        private System.Windows.Forms.Panel panelHeader;

        /// <summary>
        /// Titolo dell'applicazione.
        /// </summary>
        private System.Windows.Forms.Label lblTitle;

        /// <summary>
        /// Sottotitolo descrittivo.
        /// </summary>
        private System.Windows.Forms.Label lblSubtitle;

        /// <summary>
        /// Contenitore responsive dei pulsanti.
        /// </summary>
        private System.Windows.Forms.FlowLayoutPanel flowActions;

        /// <summary>
        /// Pulsante per visualizzare il file Excel.
        /// </summary>
        private System.Windows.Forms.Button btnPreviewSlides;

        /// <summary>
        /// Pulsante per creare il PowerPoint.
        /// </summary>
        private System.Windows.Forms.Button btnCreatePowerPoint;

        /// <summary>
        /// Pulsante per la gestione del template.
        /// </summary>
        private System.Windows.Forms.Button btnManageTemplate;

        /// <summary>
        /// Footer inferiore.
        /// </summary>
        private System.Windows.Forms.Panel panelFooter;

        /// <summary>
        /// Etichetta di stato.
        /// </summary>
        private System.Windows.Forms.Label lblStatus;

        /// <summary>
        /// Tooltip principale dell'interfaccia.
        /// </summary>
        private System.Windows.Forms.ToolTip toolTipMain;
    }
}