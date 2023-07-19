namespace Pos
{
    partial class Pos_ExtraFolio_HarcamaDetayi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pos_ExtraFolio_HarcamaDetayi));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.txt_AdSoyad = new DevExpress.XtraEditors.TextEdit();
            this.txt_Bakiye = new DevExpress.XtraEditors.TextEdit();
            this.textEdit3 = new DevExpress.XtraEditors.TextEdit();
            this.btn_Adisyonpr = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Faturapr = new DevExpress.XtraEditors.SimpleButton();
            this.btnFisIptal = new DevExpress.XtraEditors.SimpleButton();
            this.btnYenile = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_AdSoyad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Bakiye.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.gridControl1);
            resources.ApplyResources(this.groupControl1, "groupControl1");
            this.groupControl1.Name = "groupControl1";
            // 
            // gridControl1
            // 
            resources.ApplyResources(this.gridControl1, "gridControl1");
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton1.Appearance.Font")));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            resources.ApplyResources(this.simpleButton1, "simpleButton1");
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("simpleButton2.Appearance.Font")));
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.ImageOptions.Image")));
            resources.ApplyResources(this.simpleButton2, "simpleButton2");
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // textEdit1
            // 
            resources.ApplyResources(this.textEdit1, "textEdit1");
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("textEdit1.Properties.Appearance.Font")));
            this.textEdit1.Properties.Appearance.ForeColor = System.Drawing.Color.MidnightBlue;
            this.textEdit1.Properties.Appearance.Options.UseFont = true;
            this.textEdit1.Properties.Appearance.Options.UseForeColor = true;
            this.textEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit1.Properties.ReadOnly = true;
            this.textEdit1.TabStop = false;
            // 
            // txt_AdSoyad
            // 
            resources.ApplyResources(this.txt_AdSoyad, "txt_AdSoyad");
            this.txt_AdSoyad.Name = "txt_AdSoyad";
            this.txt_AdSoyad.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("txt_AdSoyad.Properties.Appearance.Font")));
            this.txt_AdSoyad.Properties.Appearance.ForeColor = System.Drawing.Color.MidnightBlue;
            this.txt_AdSoyad.Properties.Appearance.Options.UseFont = true;
            this.txt_AdSoyad.Properties.Appearance.Options.UseForeColor = true;
            this.txt_AdSoyad.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_AdSoyad.Properties.ReadOnly = true;
            this.txt_AdSoyad.TabStop = false;
            // 
            // txt_Bakiye
            // 
            resources.ApplyResources(this.txt_Bakiye, "txt_Bakiye");
            this.txt_Bakiye.Name = "txt_Bakiye";
            this.txt_Bakiye.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("txt_Bakiye.Properties.Appearance.Font")));
            this.txt_Bakiye.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txt_Bakiye.Properties.Appearance.Options.UseFont = true;
            this.txt_Bakiye.Properties.Appearance.Options.UseForeColor = true;
            this.txt_Bakiye.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Bakiye.Properties.ReadOnly = true;
            this.txt_Bakiye.TabStop = false;
            // 
            // textEdit3
            // 
            resources.ApplyResources(this.textEdit3, "textEdit3");
            this.textEdit3.Name = "textEdit3";
            this.textEdit3.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("textEdit3.Properties.Appearance.Font")));
            this.textEdit3.Properties.Appearance.ForeColor = System.Drawing.Color.MidnightBlue;
            this.textEdit3.Properties.Appearance.Options.UseFont = true;
            this.textEdit3.Properties.Appearance.Options.UseForeColor = true;
            this.textEdit3.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit3.Properties.ReadOnly = true;
            this.textEdit3.TabStop = false;
            // 
            // btn_Adisyonpr
            // 
            this.btn_Adisyonpr.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Adisyonpr.Appearance.Font")));
            this.btn_Adisyonpr.Appearance.Options.UseFont = true;
            this.btn_Adisyonpr.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Adisyonpr.ImageOptions.Image")));
            resources.ApplyResources(this.btn_Adisyonpr, "btn_Adisyonpr");
            this.btn_Adisyonpr.Name = "btn_Adisyonpr";
            this.btn_Adisyonpr.Click += new System.EventHandler(this.btn_Adisyonpr_Click);
            // 
            // btn_Faturapr
            // 
            this.btn_Faturapr.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Faturapr.Appearance.Font")));
            this.btn_Faturapr.Appearance.Options.UseFont = true;
            this.btn_Faturapr.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Faturapr.ImageOptions.Image")));
            resources.ApplyResources(this.btn_Faturapr, "btn_Faturapr");
            this.btn_Faturapr.Name = "btn_Faturapr";
            this.btn_Faturapr.Click += new System.EventHandler(this.btn_Faturapr_Click);
            // 
            // btnFisIptal
            // 
            this.btnFisIptal.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.ImageOptions.Image")));
            resources.ApplyResources(this.btnFisIptal, "btnFisIptal");
            this.btnFisIptal.Name = "btnFisIptal";
            this.btnFisIptal.Click += new System.EventHandler(this.btnFisIptal_Click);
            // 
            // btnYenile
            // 
            this.btnYenile.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.ImageOptions.Image1")));
            resources.ApplyResources(this.btnYenile, "btnYenile");
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.Click += new System.EventHandler(this.btnYenile_Click);
            // 
            // Pos_ExtraFolio_HarcamaDetayi
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnYenile);
            this.Controls.Add(this.btnFisIptal);
            this.Controls.Add(this.btn_Faturapr);
            this.Controls.Add(this.btn_Adisyonpr);
            this.Controls.Add(this.txt_Bakiye);
            this.Controls.Add(this.textEdit3);
            this.Controls.Add(this.txt_AdSoyad);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.groupControl1);
            this.Name = "Pos_ExtraFolio_HarcamaDetayi";
            this.Load += new System.EventHandler(this.Pos_ExtraFolio_HarcamaDetayi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_AdSoyad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Bakiye.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.TextEdit txt_AdSoyad;
        private DevExpress.XtraEditors.TextEdit txt_Bakiye;
        private DevExpress.XtraEditors.TextEdit textEdit3;
        public DevExpress.XtraEditors.SimpleButton btn_Adisyonpr;
        public DevExpress.XtraEditors.SimpleButton btn_Faturapr;
        private DevExpress.XtraEditors.SimpleButton btnFisIptal;
        private DevExpress.XtraEditors.SimpleButton btnYenile;
    }
}