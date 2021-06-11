namespace PartnerEnovaNormaPraca
{
    partial class frmPrzeliczenieHistorii
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
            this.btnPrzelicz = new System.Windows.Forms.Button();
            this.cbxMiesiac = new System.Windows.Forms.ComboBox();
            this.cbxRok = new System.Windows.Forms.ComboBox();
            this.lblConnection = new System.Windows.Forms.LinkLabel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPrzelicz);
            this.panel1.Controls.Add(this.cbxMiesiac);
            this.panel1.Controls.Add(this.cbxRok);
            this.panel1.Controls.Add(this.lblConnection);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 100);
            this.panel1.TabIndex = 1;
            // 
            // btnPrzelicz
            // 
            this.btnPrzelicz.Location = new System.Drawing.Point(239, 12);
            this.btnPrzelicz.Name = "btnPrzelicz";
            this.btnPrzelicz.Size = new System.Drawing.Size(109, 23);
            this.btnPrzelicz.TabIndex = 3;
            this.btnPrzelicz.Text = "Przelicz";
            this.btnPrzelicz.UseVisualStyleBackColor = true;
            this.btnPrzelicz.Click += new System.EventHandler(this.btnPrzelicz_Click);
            // 
            // cbxMiesiac
            // 
            this.cbxMiesiac.FormattingEnabled = true;
            this.cbxMiesiac.Items.AddRange(new object[] {
            "styczeń",
            "luty",
            "marzec",
            "kwiecień",
            "maj",
            "czerwiec",
            "lipiec",
            "sierpień",
            "wrzesień",
            "październik",
            "listopad",
            "grudzień"});
            this.cbxMiesiac.Location = new System.Drawing.Point(112, 13);
            this.cbxMiesiac.Name = "cbxMiesiac";
            this.cbxMiesiac.Size = new System.Drawing.Size(121, 21);
            this.cbxMiesiac.TabIndex = 2;
            // 
            // cbxRok
            // 
            this.cbxRok.FormattingEnabled = true;
            this.cbxRok.Items.AddRange(new object[] {
            "2017",
            "2018",
            "2019",
            "2020",
            "2021"});
            this.cbxRok.Location = new System.Drawing.Point(21, 13);
            this.cbxRok.Name = "cbxRok";
            this.cbxRok.Size = new System.Drawing.Size(85, 21);
            this.cbxRok.TabIndex = 1;
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Location = new System.Drawing.Point(427, 13);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(60, 13);
            this.lblConnection.TabIndex = 0;
            this.lblConnection.TabStop = true;
            this.lblConnection.Text = "połączenie";
            this.lblConnection.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblConnection_LinkClicked);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 100);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(521, 329);
            this.listBox1.TabIndex = 2;
            // 
            // frmPrzeliczenieHistorii
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 429);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.panel1);
            this.Name = "frmPrzeliczenieHistorii";
            this.Text = "frmPrzeliczenieHistorii";
            this.Load += new System.EventHandler(this.frmPrzeliczenieHistorii_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPrzelicz;
        private System.Windows.Forms.ComboBox cbxMiesiac;
        private System.Windows.Forms.ComboBox cbxRok;
        private System.Windows.Forms.LinkLabel lblConnection;
        private System.Windows.Forms.ListBox listBox1;
    }
}