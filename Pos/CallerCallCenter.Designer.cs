namespace Pos
{
    partial class CallerCallCenter
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CallerCallCenter));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip_LogRapor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnGridDizaynKaydet = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGridDizaynSil = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_CariEkle = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Satis = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_Tel = new DevExpress.XtraEditors.LabelControl();
            this.btn_CariDuzenle = new DevExpress.XtraEditors.SimpleButton();
            this.btn_AcikAdres = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelDakika = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnGelAl = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.contextMenuStrip_LogRapor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip_LogRapor;
            resources.ApplyResources(this.gridControl1, "gridControl1");
            this.gridControl1.EmbeddedNavigator.Margin = ((System.Windows.Forms.Padding)(resources.GetObject("gridControl1.EmbeddedNavigator.Margin")));
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // contextMenuStrip_LogRapor
            // 
            this.contextMenuStrip_LogRapor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnGridDizaynKaydet,
            this.btnGridDizaynSil});
            this.contextMenuStrip_LogRapor.Name = "contextMenuStrip_LogRapor";
            resources.ApplyResources(this.contextMenuStrip_LogRapor, "contextMenuStrip_LogRapor");
            // 
            // btnGridDizaynKaydet
            // 
            this.btnGridDizaynKaydet.Image = global::Pos.Properties.Resources.Apply_16x16;
            this.btnGridDizaynKaydet.Name = "btnGridDizaynKaydet";
            resources.ApplyResources(this.btnGridDizaynKaydet, "btnGridDizaynKaydet");
            this.btnGridDizaynKaydet.Click += new System.EventHandler(this.btnGridDizaynKaydet_Click);
            // 
            // btnGridDizaynSil
            // 
            this.btnGridDizaynSil.Image = global::Pos.Properties.Resources.Cancel_16x16;
            this.btnGridDizaynSil.Name = "btnGridDizaynSil";
            resources.ApplyResources(this.btnGridDizaynSil, "btnGridDizaynSil");
            this.btnGridDizaynSil.Click += new System.EventHandler(this.btnGridDizaynSil_Click);
            // 
            // gridView1
            // 
            this.gridView1.Appearance.Row.Font = ((System.Drawing.Font)(resources.GetObject("gridView1.Appearance.Row.Font")));
            this.gridView1.Appearance.Row.Options.UseFont = true;
            this.gridView1.Appearance.ViewCaption.Font = ((System.Drawing.Font)(resources.GetObject("gridView1.Appearance.ViewCaption.Font")));
            this.gridView1.Appearance.ViewCaption.Options.UseFont = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowViewCaption = true;
            resources.ApplyResources(this.gridView1, "gridView1");
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // gridColumn1
            // 
            resources.ApplyResources(this.gridColumn1, "gridColumn1");
            this.gridColumn1.FieldName = "Cari_Id";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn2
            // 
            resources.ApplyResources(this.gridColumn2, "gridColumn2");
            this.gridColumn2.FieldName = "Cari_Kod";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn3
            // 
            resources.ApplyResources(this.gridColumn3, "gridColumn3");
            this.gridColumn3.FieldName = "Cari_Ad";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn4
            // 
            resources.ApplyResources(this.gridColumn4, "gridColumn4");
            this.gridColumn4.FieldName = "Cari_Tel";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn5
            // 
            resources.ApplyResources(this.gridColumn5, "gridColumn5");
            this.gridColumn5.FieldName = "Cari_Adres";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn6
            // 
            resources.ApplyResources(this.gridColumn6, "gridColumn6");
            this.gridColumn6.FieldName = "ilKod";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn7
            // 
            resources.ApplyResources(this.gridColumn7, "gridColumn7");
            this.gridColumn7.FieldName = "ilAd";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn8
            // 
            resources.ApplyResources(this.gridColumn8, "gridColumn8");
            this.gridColumn8.FieldName = "ilceKod";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn9
            // 
            resources.ApplyResources(this.gridColumn9, "gridColumn9");
            this.gridColumn9.FieldName = "ilceAd";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn10
            // 
            resources.ApplyResources(this.gridColumn10, "gridColumn10");
            this.gridColumn10.FieldName = "mahKod";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn11
            // 
            resources.ApplyResources(this.gridColumn11, "gridColumn11");
            this.gridColumn11.FieldName = "mahAd";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn12
            // 
            resources.ApplyResources(this.gridColumn12, "gridColumn12");
            this.gridColumn12.FieldName = "subeKod";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn13
            // 
            resources.ApplyResources(this.gridColumn13, "gridColumn13");
            this.gridColumn13.FieldName = "subeAd";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowFocus = false;
            // 
            // btn_CariEkle
            // 
            resources.ApplyResources(this.btn_CariEkle, "btn_CariEkle");
            this.btn_CariEkle.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_CariEkle.Appearance.Font")));
            this.btn_CariEkle.Appearance.Options.UseFont = true;
            this.btn_CariEkle.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_CariEkle.ImageOptions.Image")));
            this.btn_CariEkle.Name = "btn_CariEkle";
            this.btn_CariEkle.Click += new System.EventHandler(this.btn_CariEkle_Click);
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Cikis.Appearance.Font")));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.btn_Cikis, "btn_Cikis");
            this.btn_Cikis.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Cikis.ImageOptions.SvgImage")));
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // btn_Satis
            // 
            resources.ApplyResources(this.btn_Satis, "btn_Satis");
            this.btn_Satis.Appearance.BackColor = System.Drawing.Color.RoyalBlue;
            this.btn_Satis.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Satis.Appearance.Font")));
            this.btn_Satis.Appearance.Options.UseBackColor = true;
            this.btn_Satis.Appearance.Options.UseFont = true;
            this.btn_Satis.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Satis.ImageOptions.SvgImage")));
            this.btn_Satis.Name = "btn_Satis";
            this.btn_Satis.Click += new System.EventHandler(this.btn_Satis_Click);
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
            // btn_CariDuzenle
            // 
            resources.ApplyResources(this.btn_CariDuzenle, "btn_CariDuzenle");
            this.btn_CariDuzenle.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_CariDuzenle.Appearance.Font")));
            this.btn_CariDuzenle.Appearance.Options.UseFont = true;
            this.btn_CariDuzenle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_CariDuzenle.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_CariDuzenle.ImageOptions.Image")));
            this.btn_CariDuzenle.Name = "btn_CariDuzenle";
            this.btn_CariDuzenle.Click += new System.EventHandler(this.btn_CariDuzenle_Click);
            // 
            // btn_AcikAdres
            // 
            resources.ApplyResources(this.btn_AcikAdres, "btn_AcikAdres");
            this.btn_AcikAdres.Appearance.BackColor = System.Drawing.Color.Red;
            this.btn_AcikAdres.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_AcikAdres.Appearance.Font")));
            this.btn_AcikAdres.Appearance.Options.UseBackColor = true;
            this.btn_AcikAdres.Appearance.Options.UseFont = true;
            this.btn_AcikAdres.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_AcikAdres.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_AcikAdres.ImageOptions.Image")));
            this.btn_AcikAdres.Name = "btn_AcikAdres";
            this.btn_AcikAdres.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridControl1);
            this.panelControl1.Controls.Add(this.panel1);
            resources.ApplyResources(this.panelControl1, "panelControl1");
            this.panelControl1.Name = "panelControl1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_Tel);
            this.panel1.Controls.Add(this.labelDakika);
            this.panel1.Controls.Add(this.btn_Cikis);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // labelDakika
            // 
            resources.ApplyResources(this.labelDakika, "labelDakika");
            this.labelDakika.Name = "labelDakika";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnGelAl);
            this.panelControl2.Controls.Add(this.btn_Satis);
            this.panelControl2.Controls.Add(this.btn_CariDuzenle);
            this.panelControl2.Controls.Add(this.btn_AcikAdres);
            this.panelControl2.Controls.Add(this.btn_CariEkle);
            resources.ApplyResources(this.panelControl2, "panelControl2");
            this.panelControl2.Name = "panelControl2";
            // 
            // btnGelAl
            // 
            resources.ApplyResources(this.btnGelAl, "btnGelAl");
            this.btnGelAl.Appearance.BackColor = System.Drawing.Color.DarkBlue;
            this.btnGelAl.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btnGelAl.Appearance.Font")));
            this.btnGelAl.Appearance.Options.UseBackColor = true;
            this.btnGelAl.Appearance.Options.UseFont = true;
            this.btnGelAl.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnGelAl.ImageOptions.Image")));
            this.btnGelAl.Name = "btnGelAl";
            this.btnGelAl.Click += new System.EventHandler(this.btnGelAl_Click_1);
            // 
            // CallerCallCenter
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CallerCallCenter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CallerCallCenter_FormClosed);
            this.Load += new System.EventHandler(this.CallerCallCenter_Load);
            this.Shown += new System.EventHandler(this.CallerCallCenter_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.contextMenuStrip_LogRapor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btn_CariEkle;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private DevExpress.XtraEditors.SimpleButton btn_Satis;
        private DevExpress.XtraEditors.LabelControl lbl_Tel;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraEditors.SimpleButton btn_CariDuzenle;
        private DevExpress.XtraEditors.SimpleButton btn_AcikAdres;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_LogRapor;
        private System.Windows.Forms.ToolStripMenuItem btnGridDizaynKaydet;
        private System.Windows.Forms.ToolStripMenuItem btnGridDizaynSil;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelDakika;
        private DevExpress.XtraEditors.SimpleButton btnGelAl;
    }
}