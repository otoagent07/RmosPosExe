
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // btnEfatura
            // 
            this.btnEfatura.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            this.btnEfatura.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnEfatura.Appearance.Options.UseBackColor = true;
            this.btnEfatura.Appearance.Options.UseFont = true;
            this.btnEfatura.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.btnEfatura.Location = new System.Drawing.Point(14, 96);
            this.btnEfatura.Name = "btnEfatura";
            this.btnEfatura.Size = new System.Drawing.Size(183, 104);
            this.btnEfatura.TabIndex = 0;
            this.btnEfatura.Text = "E-FATURA\r\n(Tip: 1)";
            this.btnEfatura.Click += new System.EventHandler(this.btnEfatura_Click);
            // 
            // btnEarsiv
            // 
            this.btnEarsiv.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question;
            this.btnEarsiv.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnEarsiv.Appearance.Options.UseBackColor = true;
            this.btnEarsiv.Appearance.Options.UseFont = true;
            this.btnEarsiv.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.ImageOptions.Image")));
            this.btnEarsiv.Location = new System.Drawing.Point(226, 96);
            this.btnEarsiv.Name = "btnEarsiv";
            this.btnEarsiv.Size = new System.Drawing.Size(183, 104);
            this.btnEarsiv.TabIndex = 1;
            this.btnEarsiv.Text = "E-ARŞİV\r\n(Tip: 2)";
            this.btnEarsiv.Click += new System.EventHandler(this.btnEarsiv_Click);
            // 
            // btnFatura
            // 
            this.btnFatura.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            this.btnFatura.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnFatura.Appearance.Options.UseBackColor = true;
            this.btnFatura.Appearance.Options.UseFont = true;
            this.btnFatura.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.ImageOptions.Image")));
            this.btnFatura.Location = new System.Drawing.Point(437, 96);
            this.btnFatura.Name = "btnFatura";
            this.btnFatura.Size = new System.Drawing.Size(183, 104);
            this.btnFatura.TabIndex = 2;
            this.btnFatura.Text = "FATURA\r\n(Tip: 3)";
            this.btnFatura.Click += new System.EventHandler(this.btnFatura_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.MidnightBlue;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(126, 21);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(398, 50);
            this.labelControl1.TabIndex = 109;
            this.labelControl1.Text = "12 BİN TL DEN YUKARI OLDUĞU İÇİN\r\n LÜTFEN FATURA TİPİNİ SEÇİNİZ...";
            // 
            // BekoEfaturaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Bisque;
            this.ClientSize = new System.Drawing.Size(639, 212);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnFatura);
            this.Controls.Add(this.btnEarsiv);
            this.Controls.Add(this.btnEfatura);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BekoEfaturaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Beko Efatura Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnEfatura;
        private DevExpress.XtraEditors.SimpleButton btnEarsiv;
        private DevExpress.XtraEditors.SimpleButton btnFatura;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}