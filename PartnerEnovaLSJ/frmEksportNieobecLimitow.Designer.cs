namespace PartnerEnovaNormaPraca
{
    partial class frmEksportNieobecLimitow
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkLinitUrlPracTymcz = new System.Windows.Forms.CheckBox();
            this.chkZwolLekarskie = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblOkres = new System.Windows.Forms.Label();
            this.dtpDataDo = new System.Windows.Forms.DateTimePicker();
            this.dtpDataOd = new System.Windows.Forms.DateTimePicker();
            this.btnZapisz = new System.Windows.Forms.Button();
            this.chkZwolOpiekaZus = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkLinitUrlPracTymcz);
            this.groupBox2.Location = new System.Drawing.Point(19, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 49);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Eksport limitów nieobecności";
            // 
            // chkLinitUrlPracTymcz
            // 
            this.chkLinitUrlPracTymcz.AutoSize = true;
            this.chkLinitUrlPracTymcz.Checked = true;
            this.chkLinitUrlPracTymcz.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLinitUrlPracTymcz.Location = new System.Drawing.Point(6, 19);
            this.chkLinitUrlPracTymcz.Name = "chkLinitUrlPracTymcz";
            this.chkLinitUrlPracTymcz.Size = new System.Drawing.Size(210, 17);
            this.chkLinitUrlPracTymcz.TabIndex = 5;
            this.chkLinitUrlPracTymcz.Text = "Limit orlopu pracownika tymczasowego";
            this.chkLinitUrlPracTymcz.UseVisualStyleBackColor = true;
            // 
            // chkZwolLekarskie
            // 
            this.chkZwolLekarskie.AutoSize = true;
            this.chkZwolLekarskie.Checked = true;
            this.chkZwolLekarskie.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkZwolLekarskie.Location = new System.Drawing.Point(6, 19);
            this.chkZwolLekarskie.Name = "chkZwolLekarskie";
            this.chkZwolLekarskie.Size = new System.Drawing.Size(122, 17);
            this.chkZwolLekarskie.TabIndex = 3;
            this.chkZwolLekarskie.Text = "Zwolnienia lekarskie";
            this.chkZwolLekarskie.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkZwolOpiekaZus);
            this.groupBox1.Controls.Add(this.chkZwolLekarskie);
            this.groupBox1.Location = new System.Drawing.Point(19, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 65);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Eksport nieobecności:";
            // 
            // lblOkres
            // 
            this.lblOkres.AutoSize = true;
            this.lblOkres.Location = new System.Drawing.Point(16, 18);
            this.lblOkres.Name = "lblOkres";
            this.lblOkres.Size = new System.Drawing.Size(58, 13);
            this.lblOkres.TabIndex = 11;
            this.lblOkres.Text = "Zakres dat";
            // 
            // dtpDataDo
            // 
            this.dtpDataDo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataDo.Location = new System.Drawing.Point(180, 12);
            this.dtpDataDo.Name = "dtpDataDo";
            this.dtpDataDo.Size = new System.Drawing.Size(94, 20);
            this.dtpDataDo.TabIndex = 9;
            // 
            // dtpDataOd
            // 
            this.dtpDataOd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataOd.Location = new System.Drawing.Point(80, 12);
            this.dtpDataOd.Name = "dtpDataOd";
            this.dtpDataOd.Size = new System.Drawing.Size(94, 20);
            this.dtpDataOd.TabIndex = 10;
            // 
            // btnZapisz
            // 
            this.btnZapisz.Location = new System.Drawing.Point(45, 208);
            this.btnZapisz.Name = "btnZapisz";
            this.btnZapisz.Size = new System.Drawing.Size(190, 44);
            this.btnZapisz.TabIndex = 8;
            this.btnZapisz.Text = "Zapisz do pliku";
            this.btnZapisz.UseVisualStyleBackColor = true;
            this.btnZapisz.Click += new System.EventHandler(this.btnZapisz_Click);
            // 
            // chkZwolOpiekaZus
            // 
            this.chkZwolOpiekaZus.AutoSize = true;
            this.chkZwolOpiekaZus.Checked = true;
            this.chkZwolOpiekaZus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkZwolOpiekaZus.Location = new System.Drawing.Point(6, 42);
            this.chkZwolOpiekaZus.Name = "chkZwolOpiekaZus";
            this.chkZwolOpiekaZus.Size = new System.Drawing.Size(143, 17);
            this.chkZwolOpiekaZus.TabIndex = 3;
            this.chkZwolOpiekaZus.Text = "Zwolnienia opieka (ZUS)";
            this.chkZwolOpiekaZus.UseVisualStyleBackColor = true;
            // 
            // frmEksportNieobecLimitow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 276);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblOkres);
            this.Controls.Add(this.dtpDataDo);
            this.Controls.Add(this.dtpDataOd);
            this.Controls.Add(this.btnZapisz);
            this.Name = "frmEksportNieobecLimitow";
            this.Text = "Eksport nieobecności i limitów";
            this.Load += new System.EventHandler(this.frmEksportNieobecLimitow_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkLinitUrlPracTymcz;
        private System.Windows.Forms.CheckBox chkZwolLekarskie;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblOkres;
        private System.Windows.Forms.DateTimePicker dtpDataDo;
        private System.Windows.Forms.DateTimePicker dtpDataOd;
        private System.Windows.Forms.Button btnZapisz;
        private System.Windows.Forms.CheckBox chkZwolOpiekaZus;
    }
}