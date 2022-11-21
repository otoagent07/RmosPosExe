namespace Pos
{
    partial class CallerDetay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CallerDetay));
            this.lbl_Tel = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txt_Tarif = new DevExpress.XtraEditors.MemoEdit();
            this.textEdit12 = new DevExpress.XtraEditors.TextEdit();
            this.txt_Adres = new DevExpress.XtraEditors.TextEdit();
            this.textEdit10 = new DevExpress.XtraEditors.TextEdit();
            this.txt_Tel2 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit8 = new DevExpress.XtraEditors.TextEdit();
            this.txt_Tel1 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit6 = new DevExpress.XtraEditors.TextEdit();
            this.txt_Soyad = new DevExpress.XtraEditors.TextEdit();
            this.textEdit4 = new DevExpress.XtraEditors.TextEdit();
            this.txt_Ad = new DevExpress.XtraEditors.TextEdit();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.btn_Satis = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.btn_CariEkle = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Print = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Print2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Tarif.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit12.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Adres.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit10.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Tel2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit8.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Tel1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit6.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Soyad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Ad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Tel
            // 
            this.lbl_Tel.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("lbl_Tel.Appearance.Font")));
            this.lbl_Tel.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lbl_Tel.Appearance.Options.UseFont = true;
            this.lbl_Tel.Appearance.Options.UseForeColor = true;
            resources.ApplyResources(this.lbl_Tel, "lbl_Tel");
            this.lbl_Tel.Name = "lbl_Tel";
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
            this.gridView1.Appearance.ViewCaption.Font = ((System.Drawing.Font)(resources.GetObject("gridView1.Appearance.ViewCaption.Font")));
            this.gridView1.Appearance.ViewCaption.Options.UseFont = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowViewCaption = true;
            resources.ApplyResources(this.gridView1, "gridView1");
            // 
            // gridColumn1
            // 
            resources.ApplyResources(this.gridColumn1, "gridColumn1");
            this.gridColumn1.FieldName = "Cari_Kod";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn2
            // 
            resources.ApplyResources(this.gridColumn2, "gridColumn2");
            this.gridColumn2.FieldName = "Cari_Ad";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn3
            // 
            resources.ApplyResources(this.gridColumn3, "gridColumn3");
            this.gridColumn3.FieldName = "Cari_Soyad";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn4
            // 
            resources.ApplyResources(this.gridColumn4, "gridColumn4");
            this.gridColumn4.FieldName = "Adres";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txt_Tarif);
            this.groupControl1.Controls.Add(this.textEdit12);
            this.groupControl1.Controls.Add(this.txt_Adres);
            this.groupControl1.Controls.Add(this.textEdit10);
            this.groupControl1.Controls.Add(this.txt_Tel2);
            this.groupControl1.Controls.Add(this.textEdit8);
            this.groupControl1.Controls.Add(this.txt_Tel1);
            this.groupControl1.Controls.Add(this.textEdit6);
            this.groupControl1.Controls.Add(this.txt_Soyad);
            this.groupControl1.Controls.Add(this.textEdit4);
            this.groupControl1.Controls.Add(this.txt_Ad);
            this.groupControl1.Controls.Add(this.textEdit1);
            resources.ApplyResources(this.groupControl1, "groupControl1");
            this.groupControl1.Name = "groupControl1";
            // 
            // txt_Tarif
            // 
            resources.ApplyResources(this.txt_Tarif, "txt_Tarif");
            this.txt_Tarif.Name = "txt_Tarif";
            this.txt_Tarif.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Tarif.Properties.ReadOnly = true;
            // 
            // textEdit12
            // 
            resources.ApplyResources(this.textEdit12, "textEdit12");
            this.textEdit12.Name = "textEdit12";
            this.textEdit12.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit12.Properties.ReadOnly = true;
            // 
            // txt_Adres
            // 
            resources.ApplyResources(this.txt_Adres, "txt_Adres");
            this.txt_Adres.Name = "txt_Adres";
            this.txt_Adres.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Adres.Properties.ReadOnly = true;
            // 
            // textEdit10
            // 
            resources.ApplyResources(this.textEdit10, "textEdit10");
            this.textEdit10.Name = "textEdit10";
            this.textEdit10.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit10.Properties.ReadOnly = true;
            // 
            // txt_Tel2
            // 
            resources.ApplyResources(this.txt_Tel2, "txt_Tel2");
            this.txt_Tel2.Name = "txt_Tel2";
            this.txt_Tel2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Tel2.Properties.ReadOnly = true;
            // 
            // textEdit8
            // 
            resources.ApplyResources(this.textEdit8, "textEdit8");
            this.textEdit8.Name = "textEdit8";
            this.textEdit8.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit8.Properties.ReadOnly = true;
            // 
            // txt_Tel1
            // 
            resources.ApplyResources(this.txt_Tel1, "txt_Tel1");
            this.txt_Tel1.Name = "txt_Tel1";
            this.txt_Tel1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Tel1.Properties.ReadOnly = true;
            // 
            // textEdit6
            // 
            resources.ApplyResources(this.textEdit6, "textEdit6");
            this.textEdit6.Name = "textEdit6";
            this.textEdit6.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit6.Properties.ReadOnly = true;
            // 
            // txt_Soyad
            // 
            resources.ApplyResources(this.txt_Soyad, "txt_Soyad");
            this.txt_Soyad.Name = "txt_Soyad";
            this.txt_Soyad.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Soyad.Properties.ReadOnly = true;
            // 
            // textEdit4
            // 
            resources.ApplyResources(this.textEdit4, "textEdit4");
            this.textEdit4.Name = "textEdit4";
            this.textEdit4.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit4.Properties.ReadOnly = true;
            // 
            // txt_Ad
            // 
            resources.ApplyResources(this.txt_Ad, "txt_Ad");
            this.txt_Ad.Name = "txt_Ad";
            this.txt_Ad.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Ad.Properties.ReadOnly = true;
            // 
            // textEdit1
            // 
            resources.ApplyResources(this.textEdit1, "textEdit1");
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit1.Properties.ReadOnly = true;
            // 
            // btn_Satis
            // 
            this.btn_Satis.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Satis.Appearance.Font")));
            this.btn_Satis.Appearance.Options.UseFont = true;
            this.btn_Satis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Satis.ImageOptions.Image")));
            resources.ApplyResources(this.btn_Satis, "btn_Satis");
            this.btn_Satis.Name = "btn_Satis";
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Cikis.Appearance.Font")));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cikis.ImageOptions.Image")));
            resources.ApplyResources(this.btn_Cikis, "btn_Cikis");
            this.btn_Cikis.Name = "btn_Cikis";
            // 
            // btn_CariEkle
            // 
            this.btn_CariEkle.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_CariEkle.Appearance.Font")));
            this.btn_CariEkle.Appearance.Options.UseFont = true;
            this.btn_CariEkle.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_CariEkle.ImageOptions.Image")));
            resources.ApplyResources(this.btn_CariEkle, "btn_CariEkle");
            this.btn_CariEkle.Name = "btn_CariEkle";
            // 
            // btn_Print
            // 
            this.btn_Print.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Print.Appearance.Font")));
            this.btn_Print.Appearance.Options.UseFont = true;
            this.btn_Print.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Print.ImageOptions.Image")));
            resources.ApplyResources(this.btn_Print, "btn_Print");
            this.btn_Print.Name = "btn_Print";
            // 
            // btn_Print2
            // 
            this.btn_Print2.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Print2.Appearance.Font")));
            this.btn_Print2.Appearance.Options.UseFont = true;
            this.btn_Print2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Print2.ImageOptions.Image")));
            resources.ApplyResources(this.btn_Print2, "btn_Print2");
            this.btn_Print2.Name = "btn_Print2";
            // 
            // CallerDetay
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.btn_Print2);
            this.Controls.Add(this.btn_Print);
            this.Controls.Add(this.btn_CariEkle);
            this.Controls.Add(this.btn_Cikis);
            this.Controls.Add(this.btn_Satis);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.lbl_Tel);
            this.Name = "CallerDetay";
            this.Load += new System.EventHandler(this.CallerDetay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Tarif.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit12.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Adres.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit10.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Tel2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit8.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Tel1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit6.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Soyad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Ad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lbl_Tel;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.MemoEdit txt_Tarif;
        private DevExpress.XtraEditors.TextEdit textEdit12;
        private DevExpress.XtraEditors.TextEdit txt_Adres;
        private DevExpress.XtraEditors.TextEdit textEdit10;
        private DevExpress.XtraEditors.TextEdit txt_Tel2;
        private DevExpress.XtraEditors.TextEdit textEdit8;
        private DevExpress.XtraEditors.TextEdit txt_Tel1;
        private DevExpress.XtraEditors.TextEdit textEdit6;
        private DevExpress.XtraEditors.TextEdit txt_Soyad;
        private DevExpress.XtraEditors.TextEdit textEdit4;
        private DevExpress.XtraEditors.TextEdit txt_Ad;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.SimpleButton btn_Satis;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private DevExpress.XtraEditors.SimpleButton btn_CariEkle;
        private DevExpress.XtraEditors.SimpleButton btn_Print;
        private DevExpress.XtraEditors.SimpleButton btn_Print2;
    }
}