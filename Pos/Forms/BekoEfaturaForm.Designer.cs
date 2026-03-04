
namespace Pos.Forms
{
    partial class BekoEfaturaForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BekoEfaturaForm));
            this.btnEfatura = new DevExpress.XtraEditors.SimpleButton();
            this.btnEarsiv = new DevExpress.XtraEditors.SimpleButton();
            this.btnFatura = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnEfatura
            // 
            this.btnEfatura.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnEfatura.Appearance.Options.UseFont = true;
            this.btnEfatura.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.btnEfatura.Location = new System.Drawing.Point(23, 12);
            this.btnEfatura.Name = "btnEfatura";
            this.btnEfatura.Size = new System.Drawing.Size(183, 104);
            this.btnEfatura.TabIndex = 0;
            this.btnEfatura.Text = "E-FATURA";
            this.btnEfatura.Click += new System.EventHandler(this.btnEfatura_Click);
            // 
            // btnEarsiv
            // 
            this.btnEarsiv.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnEarsiv.Appearance.Options.UseFont = true;
            this.btnEarsiv.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.ImageOptions.Image")));
            this.btnEarsiv.Location = new System.Drawing.Point(235, 12);
            this.btnEarsiv.Name = "btnEarsiv";
            this.btnEarsiv.Size = new System.Drawing.Size(183, 104);
            this.btnEarsiv.TabIndex = 1;
            this.btnEarsiv.Text = "E-ARŞİV";
            this.btnEarsiv.Click += new System.EventHandler(this.btnEarsiv_Click);
            // 
            // btnFatura
            // 
            this.btnFatura.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnFatura.Appearance.Options.UseFont = true;
            this.btnFatura.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.ImageOptions.Image")));
            this.btnFatura.Location = new System.Drawing.Point(446, 12);
            this.btnFatura.Name = "btnFatura";
            this.btnFatura.Size = new System.Drawing.Size(183, 104);
            this.btnFatura.TabIndex = 2;
            this.btnFatura.Text = "FATURA";
            this.btnFatura.Click += new System.EventHandler(this.btnFatura_Click);
            // 
            // BekoEfaturaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 128);
            this.Controls.Add(this.btnFatura);
            this.Controls.Add(this.btnEarsiv);
            this.Controls.Add(this.btnEfatura);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BekoEfaturaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Beko Efatura Form";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnEfatura;
        private DevExpress.XtraEditors.SimpleButton btnEarsiv;
        private DevExpress.XtraEditors.SimpleButton btnFatura;
    }
}