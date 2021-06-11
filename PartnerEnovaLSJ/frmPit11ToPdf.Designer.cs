namespace PartnerEnovaNormaPraca
{
    partial class frmPit11ToPdf
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPit11ToPdf));
            this.btnFolder = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.btnWydruk = new System.Windows.Forms.Button();
            this.btnAnuluj = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(12, 12);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(75, 23);
            this.btnFolder.TabIndex = 0;
            this.btnFolder.Text = "Folder";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(93, 14);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(302, 20);
            this.txtFolder.TabIndex = 1;
            // 
            // btnWydruk
            // 
            this.btnWydruk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnWydruk.Location = new System.Drawing.Point(106, 108);
            this.btnWydruk.Name = "btnWydruk";
            this.btnWydruk.Size = new System.Drawing.Size(92, 33);
            this.btnWydruk.TabIndex = 2;
            this.btnWydruk.Text = "Wydruk";
            this.btnWydruk.UseVisualStyleBackColor = true;
            this.btnWydruk.Click += new System.EventHandler(this.btnWydruk_Click);
            // 
            // btnAnuluj
            // 
            this.btnAnuluj.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnuluj.Location = new System.Drawing.Point(225, 108);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(92, 33);
            this.btnAnuluj.TabIndex = 3;
            this.btnAnuluj.Text = "Anuluj";
            this.btnAnuluj.UseVisualStyleBackColor = true;
            // 
            // frmPit11ToPdf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 159);
            this.Controls.Add(this.btnAnuluj);
            this.Controls.Add(this.btnWydruk);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.btnFolder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(424, 198);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(424, 198);
            this.Name = "frmPit11ToPdf";
            this.Text = "Wydruk PIT11/IFT1 do plików PDF";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPit11ToPdf_FormClosing);
            this.Load += new System.EventHandler(this.frmPit11ToPdf_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Button btnWydruk;
        private System.Windows.Forms.Button btnAnuluj;
    }
}