namespace Pos
{
    partial class Garson_Sor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Garson_Sor));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.flp_Garson = new System.Windows.Forms.FlowLayoutPanel();
            this.lbl_Baslik = new DevExpress.XtraEditors.LabelControl();
            this.btnCikis = new DevExpress.XtraEditors.SimpleButton();
            this.btnPaketciSil = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("labelControl1.Appearance.Font")));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            resources.ApplyResources(this.labelControl1, "labelControl1");
            this.labelControl1.Name = "labelControl1";
            // 
            // flp_Garson
            // 
            resources.ApplyResources(this.flp_Garson, "flp_Garson");
            this.flp_Garson.Name = "flp_Garson";
            // 
            // lbl_Baslik
            // 
            this.lbl_Baslik.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("lbl_Baslik.Appearance.Font")));
            this.lbl_Baslik.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.lbl_Baslik.Appearance.Options.UseFont = true;
            this.lbl_Baslik.Appearance.Options.UseForeColor = true;
            resources.ApplyResources(this.lbl_Baslik, "lbl_Baslik");
            this.lbl_Baslik.Name = "lbl_Baslik";
            // 
            // btnCikis
            // 
            this.btnCikis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCikis.ImageOptions.Image")));
            resources.ApplyResources(this.btnCikis, "btnCikis");
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // btnPaketciSil
            // 
            this.btnPaketciSil.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton1.Appearance.Font")));
            this.btnPaketciSil.Appearance.ForeColor = System.Drawing.Color.Red;
            this.btnPaketciSil.Appearance.Options.UseFont = true;
            this.btnPaketciSil.Appearance.Options.UseForeColor = true;
            this.btnPaketciSil.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            resources.ApplyResources(this.btnPaketciSil, "btnPaketciSil");
            this.btnPaketciSil.Name = "btnPaketciSil";
            this.btnPaketciSil.Click += new System.EventHandler(this.btnPaketciSil_Click);
            // 
            // Garson_Sor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.btnPaketciSil);
            this.Controls.Add(this.btnCikis);
            this.Controls.Add(this.lbl_Baslik);
            this.Controls.Add(this.flp_Garson);
            this.Controls.Add(this.labelControl1);
            this.Name = "Garson_Sor";
            this.Load += new System.EventHandler(this.Garson_Sor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.FlowLayoutPanel flp_Garson;
        private DevExpress.XtraEditors.LabelControl lbl_Baslik;
        private DevExpress.XtraEditors.SimpleButton btnCikis;
        private DevExpress.XtraEditors.SimpleButton btnPaketciSil;
    }
}