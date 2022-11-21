namespace Pos
{
    partial class Rapor_Sec
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rapor_Sec));
            this.btn_Raporlar = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Istatistik = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.btn_GuNSonu = new DevExpress.XtraEditors.SimpleButton();
            this.btn_GunsonuMail = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btn_Raporlar
            // 
            resources.ApplyResources(this.btn_Raporlar, "btn_Raporlar");
            this.btn_Raporlar.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Raporlar.Appearance.Font")));
            this.btn_Raporlar.Appearance.Options.UseFont = true;
            this.btn_Raporlar.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Raporlar.ImageOptions.SvgImage")));
            this.btn_Raporlar.Name = "btn_Raporlar";
            this.btn_Raporlar.Click += new System.EventHandler(this.btn_Raporlar_Click);
            // 
            // btn_Istatistik
            // 
            resources.ApplyResources(this.btn_Istatistik, "btn_Istatistik");
            this.btn_Istatistik.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Istatistik.Appearance.Font")));
            this.btn_Istatistik.Appearance.Options.UseFont = true;
            this.btn_Istatistik.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Istatistik.ImageOptions.SvgImage")));
            this.btn_Istatistik.Name = "btn_Istatistik";
            this.btn_Istatistik.Click += new System.EventHandler(this.btn_Istatistik_Click);
            // 
            // btn_Cikis
            // 
            resources.ApplyResources(this.btn_Cikis, "btn_Cikis");
            this.btn_Cikis.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Cikis.Appearance.Font")));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Cikis.ImageOptions.SvgImage")));
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // btn_GuNSonu
            // 
            resources.ApplyResources(this.btn_GuNSonu, "btn_GuNSonu");
            this.btn_GuNSonu.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_GuNSonu.Appearance.Font")));
            this.btn_GuNSonu.Appearance.Options.UseFont = true;
            this.btn_GuNSonu.Appearance.Options.UseTextOptions = true;
            this.btn_GuNSonu.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btn_GuNSonu.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_GuNSonu.ImageOptions.SvgImage")));
            this.btn_GuNSonu.Name = "btn_GuNSonu";
            this.btn_GuNSonu.Click += new System.EventHandler(this.btn_GuNSonu_Click);
            // 
            // btn_GunsonuMail
            // 
            resources.ApplyResources(this.btn_GunsonuMail, "btn_GunsonuMail");
            this.btn_GunsonuMail.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_GunsonuMail.Appearance.Font")));
            this.btn_GunsonuMail.Appearance.Options.UseFont = true;
            this.btn_GunsonuMail.Appearance.Options.UseTextOptions = true;
            this.btn_GunsonuMail.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btn_GunsonuMail.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_GunsonuMail.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_GunsonuMail.ImageOptions.SvgImage")));
            this.btn_GunsonuMail.Name = "btn_GunsonuMail";
            this.btn_GunsonuMail.Click += new System.EventHandler(this.btn_GunsonuMail_Click);
            // 
            // Rapor_Sec
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.btn_GunsonuMail);
            this.Controls.Add(this.btn_Cikis);
            this.Controls.Add(this.btn_GuNSonu);
            this.Controls.Add(this.btn_Istatistik);
            this.Controls.Add(this.btn_Raporlar);
            this.Name = "Rapor_Sec";
            this.Load += new System.EventHandler(this.Rapor_Sec_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_Raporlar;
        private DevExpress.XtraEditors.SimpleButton btn_Istatistik;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private DevExpress.XtraEditors.SimpleButton btn_GuNSonu;
        private DevExpress.XtraEditors.SimpleButton btn_GunsonuMail;
    }
}