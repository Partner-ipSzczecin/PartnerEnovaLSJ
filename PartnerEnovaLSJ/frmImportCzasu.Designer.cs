namespace PartnerEnovaNormaPraca
{
    partial class frmImportCzasu
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbxImportNieobecnosci = new System.Windows.Forms.GroupBox();
            this.cbxPominChorobowe = new System.Windows.Forms.CheckBox();
            this.cbxLog = new System.Windows.Forms.CheckBox();
            this.lblConnection = new System.Windows.Forms.LinkLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbxNieobecosci = new System.Windows.Forms.CheckBox();
            this.chkCzas = new System.Windows.Forms.CheckBox();
            this.chkPlan = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbNadpisz = new System.Windows.Forms.RadioButton();
            this.rbPomin = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxFirma = new System.Windows.Forms.ComboBox();
            this.btnPlik = new System.Windows.Forms.Button();
            this.btnImportuj = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.gbxImportNieobecnosci.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblVersion);
            this.panel1.Controls.Add(this.gbxImportNieobecnosci);
            this.panel1.Controls.Add(this.cbxLog);
            this.panel1.Controls.Add(this.lblConnection);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbxFirma);
            this.panel1.Controls.Add(this.btnPlik);
            this.panel1.Controls.Add(this.btnImportuj);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(802, 182);
            this.panel1.TabIndex = 2;
            // 
            // gbxImportNieobecnosci
            // 
            this.gbxImportNieobecnosci.Controls.Add(this.cbxPominChorobowe);
            this.gbxImportNieobecnosci.Enabled = false;
            this.gbxImportNieobecnosci.Location = new System.Drawing.Point(246, 46);
            this.gbxImportNieobecnosci.Name = "gbxImportNieobecnosci";
            this.gbxImportNieobecnosci.Size = new System.Drawing.Size(287, 67);
            this.gbxImportNieobecnosci.TabIndex = 18;
            this.gbxImportNieobecnosci.TabStop = false;
            this.gbxImportNieobecnosci.Text = "Import nieobecności";
            // 
            // cbxPominChorobowe
            // 
            this.cbxPominChorobowe.AutoSize = true;
            this.cbxPominChorobowe.Checked = true;
            this.cbxPominChorobowe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxPominChorobowe.Location = new System.Drawing.Point(6, 19);
            this.cbxPominChorobowe.Name = "cbxPominChorobowe";
            this.cbxPominChorobowe.Size = new System.Drawing.Size(163, 17);
            this.cbxPominChorobowe.TabIndex = 3;
            this.cbxPominChorobowe.Text = "Pomiń zwolnienia chorobowe";
            this.cbxPominChorobowe.UseVisualStyleBackColor = true;
            // 
            // cbxLog
            // 
            this.cbxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxLog.AutoSize = true;
            this.cbxLog.Location = new System.Drawing.Point(681, 138);
            this.cbxLog.Name = "cbxLog";
            this.cbxLog.Size = new System.Drawing.Size(109, 17);
            this.cbxLog.TabIndex = 17;
            this.cbxLog.Text = "Log szczegółowy";
            this.cbxLog.UseVisualStyleBackColor = true;
            // 
            // lblConnection
            // 
            this.lblConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnection.AutoSize = true;
            this.lblConnection.Location = new System.Drawing.Point(730, 12);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(60, 13);
            this.lblConnection.TabIndex = 16;
            this.lblConnection.TabStop = true;
            this.lblConnection.Text = "połączenie";
            this.lblConnection.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblConnection_LinkClicked);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxNieobecosci);
            this.groupBox2.Controls.Add(this.chkCzas);
            this.groupBox2.Controls.Add(this.chkPlan);
            this.groupBox2.Location = new System.Drawing.Point(14, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 67);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Importuj";
            // 
            // cbxNieobecosci
            // 
            this.cbxNieobecosci.AutoSize = true;
            this.cbxNieobecosci.Checked = true;
            this.cbxNieobecosci.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxNieobecosci.Enabled = false;
            this.cbxNieobecosci.Location = new System.Drawing.Point(6, 42);
            this.cbxNieobecosci.Name = "cbxNieobecosci";
            this.cbxNieobecosci.Size = new System.Drawing.Size(91, 17);
            this.cbxNieobecosci.TabIndex = 2;
            this.cbxNieobecosci.Text = "Nieobecności";
            this.cbxNieobecosci.UseVisualStyleBackColor = true;
            this.cbxNieobecosci.CheckedChanged += new System.EventHandler(this.chkNieobecosci_CheckedChanged);
            // 
            // chkCzas
            // 
            this.chkCzas.AutoSize = true;
            this.chkCzas.Checked = true;
            this.chkCzas.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCzas.Enabled = false;
            this.chkCzas.Location = new System.Drawing.Point(8, 19);
            this.chkCzas.Name = "chkCzas";
            this.chkCzas.Size = new System.Drawing.Size(78, 17);
            this.chkCzas.TabIndex = 1;
            this.chkCzas.Text = "Czas pracy";
            this.chkCzas.UseVisualStyleBackColor = true;
            // 
            // chkPlan
            // 
            this.chkPlan.AutoSize = true;
            this.chkPlan.Checked = true;
            this.chkPlan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPlan.Enabled = false;
            this.chkPlan.Location = new System.Drawing.Point(101, 19);
            this.chkPlan.Name = "chkPlan";
            this.chkPlan.Size = new System.Drawing.Size(76, 17);
            this.chkPlan.TabIndex = 0;
            this.chkPlan.Text = "Plan pracy";
            this.chkPlan.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbNadpisz);
            this.groupBox1.Controls.Add(this.rbPomin);
            this.groupBox1.Location = new System.Drawing.Point(14, 119);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 45);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "W przypadku istniejącego czasu";
            // 
            // rbNadpisz
            // 
            this.rbNadpisz.AutoSize = true;
            this.rbNadpisz.Location = new System.Drawing.Point(103, 19);
            this.rbNadpisz.Name = "rbNadpisz";
            this.rbNadpisz.Size = new System.Drawing.Size(63, 17);
            this.rbNadpisz.TabIndex = 14;
            this.rbNadpisz.Text = "Nadpisz";
            this.rbNadpisz.UseVisualStyleBackColor = true;
            // 
            // rbPomin
            // 
            this.rbPomin.AutoSize = true;
            this.rbPomin.Checked = true;
            this.rbPomin.Location = new System.Drawing.Point(8, 19);
            this.rbPomin.Name = "rbPomin";
            this.rbPomin.Size = new System.Drawing.Size(54, 17);
            this.rbPomin.TabIndex = 14;
            this.rbPomin.TabStop = true;
            this.rbPomin.Text = "Pomiń";
            this.rbPomin.UseVisualStyleBackColor = true;
            this.rbPomin.CheckedChanged += new System.EventHandler(this.rbPomin_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Plik z firmy:";
            // 
            // cbxFirma
            // 
            this.cbxFirma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFirma.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cbxFirma.ItemHeight = 20;
            this.cbxFirma.Items.AddRange(new object[] {
            "Incom",
            "Momox"});
            this.cbxFirma.Location = new System.Drawing.Point(115, 12);
            this.cbxFirma.Name = "cbxFirma";
            this.cbxFirma.Size = new System.Drawing.Size(174, 28);
            this.cbxFirma.TabIndex = 9;
            this.cbxFirma.SelectedIndexChanged += new System.EventHandler(this.cbxFirma_SelectedIndexChanged);
            this.cbxFirma.DropDownClosed += new System.EventHandler(this.cbxFirma_DropDownClosed);
            this.cbxFirma.TextChanged += new System.EventHandler(this.cbxFirma_TextChanged);
            // 
            // btnPlik
            // 
            this.btnPlik.Location = new System.Drawing.Point(295, 12);
            this.btnPlik.Name = "btnPlik";
            this.btnPlik.Size = new System.Drawing.Size(135, 28);
            this.btnPlik.TabIndex = 8;
            this.btnPlik.Text = "Wybierz plik";
            this.btnPlik.UseVisualStyleBackColor = true;
            this.btnPlik.Click += new System.EventHandler(this.btnPlik_Click);
            // 
            // btnImportuj
            // 
            this.btnImportuj.Enabled = false;
            this.btnImportuj.Location = new System.Drawing.Point(436, 12);
            this.btnImportuj.Name = "btnImportuj";
            this.btnImportuj.Size = new System.Drawing.Size(135, 28);
            this.btnImportuj.TabIndex = 0;
            this.btnImportuj.Text = "Importuj";
            this.btnImportuj.UseVisualStyleBackColor = true;
            this.btnImportuj.Click += new System.EventHandler(this.btnImportuj_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(730, 160);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(55, 13);
            this.lblVersion.TabIndex = 19;
            this.lblVersion.Text = "v. 20.01.1";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmImportCzasu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 182);
            this.Controls.Add(this.panel1);
            this.Name = "frmImportCzasu";
            this.Text = "Import Czasu Pracy";
            this.Load += new System.EventHandler(this.frmImportCzasu_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbxImportNieobecnosci.ResumeLayout(false);
            this.gbxImportNieobecnosci.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel lblConnection;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbxNieobecosci;
        private System.Windows.Forms.CheckBox chkCzas;
        private System.Windows.Forms.CheckBox chkPlan;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbNadpisz;
        private System.Windows.Forms.RadioButton rbPomin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxFirma;
        private System.Windows.Forms.Button btnPlik;
        private System.Windows.Forms.Button btnImportuj;
        private System.Windows.Forms.CheckBox cbxLog;
        private System.Windows.Forms.GroupBox gbxImportNieobecnosci;
        private System.Windows.Forms.CheckBox cbxPominChorobowe;
        private System.Windows.Forms.Label lblVersion;
    }
}