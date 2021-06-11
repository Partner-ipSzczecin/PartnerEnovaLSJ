namespace PartnerEnovaNormaPraca
{
    partial class frmKonwersja
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnKonwertuj = new System.Windows.Forms.Button();
            this.cbxCzasPracy = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxHarmonogram = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkPominBrakiKodow = new System.Windows.Forms.CheckBox();
            this.btnLog = new System.Windows.Forms.Button();
            this.cbxLog = new System.Windows.Forms.CheckBox();
            this.chkZamienUrlop = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxFirma = new System.Windows.Forms.ComboBox();
            this.btnPlik = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lblVersion = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnKonwertuj);
            this.panel1.Controls.Add(this.cbxCzasPracy);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbxHarmonogram);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.chkPominBrakiKodow);
            this.panel1.Controls.Add(this.btnLog);
            this.panel1.Controls.Add(this.cbxLog);
            this.panel1.Controls.Add(this.chkZamienUrlop);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbxFirma);
            this.panel1.Controls.Add(this.btnPlik);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(765, 175);
            this.panel1.TabIndex = 0;
            // 
            // btnKonwertuj
            // 
            this.btnKonwertuj.Enabled = false;
            this.btnKonwertuj.Location = new System.Drawing.Point(325, 75);
            this.btnKonwertuj.Name = "btnKonwertuj";
            this.btnKonwertuj.Size = new System.Drawing.Size(135, 28);
            this.btnKonwertuj.TabIndex = 13;
            this.btnKonwertuj.Text = "Konwertuj";
            this.btnKonwertuj.UseVisualStyleBackColor = true;
            this.btnKonwertuj.Click += new System.EventHandler(this.btnKonwertuj_Click);
            // 
            // cbxCzasPracy
            // 
            this.cbxCzasPracy.Enabled = false;
            this.cbxCzasPracy.FormattingEnabled = true;
            this.cbxCzasPracy.Location = new System.Drawing.Point(110, 80);
            this.cbxCzasPracy.Name = "cbxCzasPracy";
            this.cbxCzasPracy.Size = new System.Drawing.Size(209, 21);
            this.cbxCzasPracy.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Czas pracy:";
            // 
            // cbxHarmonogram
            // 
            this.cbxHarmonogram.Enabled = false;
            this.cbxHarmonogram.FormattingEnabled = true;
            this.cbxHarmonogram.Location = new System.Drawing.Point(110, 53);
            this.cbxHarmonogram.Name = "cbxHarmonogram";
            this.cbxHarmonogram.Size = new System.Drawing.Size(209, 21);
            this.cbxHarmonogram.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Harmonogram:";
            // 
            // chkPominBrakiKodow
            // 
            this.chkPominBrakiKodow.AutoSize = true;
            this.chkPominBrakiKodow.Checked = true;
            this.chkPominBrakiKodow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPominBrakiKodow.Location = new System.Drawing.Point(12, 150);
            this.chkPominBrakiKodow.Name = "chkPominBrakiKodow";
            this.chkPominBrakiKodow.Size = new System.Drawing.Size(333, 17);
            this.chkPominBrakiKodow.TabIndex = 7;
            this.chkPominBrakiKodow.Text = "Pomiń pracowników bez przypisanych kodów w programie enova";
            this.chkPominBrakiKodow.UseVisualStyleBackColor = true;
            // 
            // btnLog
            // 
            this.btnLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLog.Enabled = false;
            this.btnLog.Location = new System.Drawing.Point(663, 146);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(90, 23);
            this.btnLog.TabIndex = 6;
            this.btnLog.Text = "Zapisz log";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // cbxLog
            // 
            this.cbxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxLog.AutoSize = true;
            this.cbxLog.Location = new System.Drawing.Point(548, 150);
            this.cbxLog.Name = "cbxLog";
            this.cbxLog.Size = new System.Drawing.Size(109, 17);
            this.cbxLog.TabIndex = 5;
            this.cbxLog.Text = "Log szczegółowy";
            this.cbxLog.UseVisualStyleBackColor = true;
            // 
            // chkZamienUrlop
            // 
            this.chkZamienUrlop.AutoSize = true;
            this.chkZamienUrlop.Checked = true;
            this.chkZamienUrlop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkZamienUrlop.Location = new System.Drawing.Point(12, 127);
            this.chkZamienUrlop.Name = "chkZamienUrlop";
            this.chkZamienUrlop.Size = new System.Drawing.Size(339, 17);
            this.chkZamienUrlop.TabIndex = 4;
            this.chkZamienUrlop.Text = "Zamień Urlop wypoczynkowy na Urlop wypoczynkowy prac.tymcz.";
            this.chkZamienUrlop.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Plik z firmy:";
            // 
            // cbxFirma
            // 
            this.cbxFirma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFirma.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cbxFirma.ItemHeight = 20;
            this.cbxFirma.Items.AddRange(new object[] {
            "Ammega",
            "KK Wind Solutions"});
            this.cbxFirma.Location = new System.Drawing.Point(110, 19);
            this.cbxFirma.Name = "cbxFirma";
            this.cbxFirma.Size = new System.Drawing.Size(209, 28);
            this.cbxFirma.TabIndex = 2;
            this.cbxFirma.SelectedIndexChanged += new System.EventHandler(this.cbxFirma_SelectedIndexChanged);
            // 
            // btnPlik
            // 
            this.btnPlik.Location = new System.Drawing.Point(325, 19);
            this.btnPlik.Name = "btnPlik";
            this.btnPlik.Size = new System.Drawing.Size(135, 28);
            this.btnPlik.TabIndex = 1;
            this.btnPlik.Text = "Wybierz plik";
            this.btnPlik.UseVisualStyleBackColor = true;
            this.btnPlik.Click += new System.EventHandler(this.btnPlik_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(0, 175);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(765, 306);
            this.listBox1.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 481);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(765, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssLabel1
            // 
            this.tssLabel1.Name = "tssLabel1";
            this.tssLabel1.Size = new System.Drawing.Size(39, 17);
            this.tssLabel1.Text = "Status";
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(684, 484);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(55, 13);
            this.lblVersion.TabIndex = 20;
            this.lblVersion.Text = "v. 20.01.1";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmKonwersja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 503);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "frmKonwersja";
            this.Text = "Konwersja plików z czasem pracy";
            this.Load += new System.EventHandler(this.frmKonwersja_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPlik;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox cbxFirma;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkZamienUrlop;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssLabel1;
        private System.Windows.Forms.CheckBox cbxLog;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox chkPominBrakiKodow;
        private System.Windows.Forms.ComboBox cbxHarmonogram;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxCzasPracy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnKonwertuj;
        private System.Windows.Forms.Label lblVersion;
    }
}