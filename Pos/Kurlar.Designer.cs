namespace Pos
{
    partial class Kurlar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Kurlar));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Aktar = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Sil = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Transfer = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Kaydet = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Listele = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.look_Cesit = new DevExpress.XtraEditors.LookUpEdit();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_Cesit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.AppearancePage.Header.Font = ((System.Drawing.Font)(resources.GetObject("xtraTabControl1.AppearancePage.Header.Font")));
            this.xtraTabControl1.AppearancePage.Header.Options.UseFont = true;
            this.xtraTabControl1.AppearancePage.Header.Options.UseTextOptions = true;
            this.xtraTabControl1.AppearancePage.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.xtraTabControl1.AppearancePage.Header.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            resources.ApplyResources(this.xtraTabControl1, "xtraTabControl1");
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            this.xtraTabControl1.Click += new System.EventHandler(this.xtraTabControl1_Click);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gridControl1);
            this.xtraTabPage1.Controls.Add(this.panelControl2);
            this.xtraTabPage1.Controls.Add(this.panelControl1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            resources.ApplyResources(this.xtraTabPage1, "xtraTabPage1");
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
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowHeight = 30;
            // 
            // gridColumn1
            // 
            resources.ApplyResources(this.gridColumn1, "gridColumn1");
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn2
            // 
            resources.ApplyResources(this.gridColumn2, "gridColumn2");
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn3
            // 
            resources.ApplyResources(this.gridColumn3, "gridColumn3");
            this.gridColumn3.DisplayFormat.FormatString = "N4";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn4
            // 
            resources.ApplyResources(this.gridColumn4, "gridColumn4");
            this.gridColumn4.DisplayFormat.FormatString = "N4";
            this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn5
            // 
            resources.ApplyResources(this.gridColumn5, "gridColumn5");
            this.gridColumn5.DisplayFormat.FormatString = "N4";
            this.gridColumn5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn5.Name = "gridColumn5";
            // 
            // gridColumn6
            // 
            resources.ApplyResources(this.gridColumn6, "gridColumn6");
            this.gridColumn6.DisplayFormat.FormatString = "N4";
            this.gridColumn6.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn6.Name = "gridColumn6";
            // 
            // gridColumn7
            // 
            resources.ApplyResources(this.gridColumn7, "gridColumn7");
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn8
            // 
            resources.ApplyResources(this.gridColumn8, "gridColumn8");
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btn_Aktar);
            this.panelControl2.Controls.Add(this.btn_Sil);
            this.panelControl2.Controls.Add(this.btn_Cikis);
            this.panelControl2.Controls.Add(this.btn_Transfer);
            this.panelControl2.Controls.Add(this.btn_Kaydet);
            this.panelControl2.Controls.Add(this.btn_Listele);
            resources.ApplyResources(this.panelControl2, "panelControl2");
            this.panelControl2.Name = "panelControl2";
            // 
            // btn_Aktar
            // 
            this.btn_Aktar.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Aktar.Appearance.Font")));
            this.btn_Aktar.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.btn_Aktar, "btn_Aktar");
            this.btn_Aktar.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Aktar.ImageOptions.SvgImage")));
            this.btn_Aktar.Name = "btn_Aktar";
            this.btn_Aktar.Click += new System.EventHandler(this.btn_Aktar_Click);
            // 
            // btn_Sil
            // 
            this.btn_Sil.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Sil.Appearance.Font")));
            this.btn_Sil.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.btn_Sil, "btn_Sil");
            this.btn_Sil.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Sil.ImageOptions.SvgImage")));
            this.btn_Sil.Name = "btn_Sil";
            this.btn_Sil.Click += new System.EventHandler(this.btn_Sil_Click);
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
            // btn_Transfer
            // 
            this.btn_Transfer.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Transfer.Appearance.Font")));
            this.btn_Transfer.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.btn_Transfer, "btn_Transfer");
            this.btn_Transfer.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Transfer.ImageOptions.SvgImage")));
            this.btn_Transfer.Name = "btn_Transfer";
            this.btn_Transfer.Click += new System.EventHandler(this.btn_Transfer_Click);
            // 
            // btn_Kaydet
            // 
            this.btn_Kaydet.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Kaydet.Appearance.Font")));
            this.btn_Kaydet.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.btn_Kaydet, "btn_Kaydet");
            this.btn_Kaydet.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Kaydet.ImageOptions.SvgImage")));
            this.btn_Kaydet.Name = "btn_Kaydet";
            this.btn_Kaydet.Click += new System.EventHandler(this.btn_Kaydet_Click);
            // 
            // btn_Listele
            // 
            this.btn_Listele.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Listele.Appearance.Font")));
            this.btn_Listele.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.btn_Listele, "btn_Listele");
            this.btn_Listele.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Listele.ImageOptions.SvgImage")));
            this.btn_Listele.Name = "btn_Listele";
            this.btn_Listele.Click += new System.EventHandler(this.btn_Listele_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.textEdit1);
            this.panelControl1.Controls.Add(this.textEdit2);
            this.panelControl1.Controls.Add(this.look_Cesit);
            this.panelControl1.Controls.Add(this.dateEdit1);
            resources.ApplyResources(this.panelControl1, "panelControl1");
            this.panelControl1.Name = "panelControl1";
            // 
            // textEdit1
            // 
            resources.ApplyResources(this.textEdit1, "textEdit1");
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("textEdit1.Properties.Appearance.Font")));
            this.textEdit1.Properties.Appearance.Options.UseFont = true;
            this.textEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit1.Properties.ReadOnly = true;
            // 
            // textEdit2
            // 
            resources.ApplyResources(this.textEdit2, "textEdit2");
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("textEdit2.Properties.Appearance.Font")));
            this.textEdit2.Properties.Appearance.Options.UseFont = true;
            this.textEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit2.Properties.ReadOnly = true;
            // 
            // look_Cesit
            // 
            resources.ApplyResources(this.look_Cesit, "look_Cesit");
            this.look_Cesit.Name = "look_Cesit";
            this.look_Cesit.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("look_Cesit.Properties.Appearance.Font")));
            this.look_Cesit.Properties.Appearance.Options.UseFont = true;
            this.look_Cesit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.look_Cesit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("look_Cesit.Properties.Buttons"))))});
            this.look_Cesit.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(resources.GetString("look_Cesit.Properties.Columns"), resources.GetString("look_Cesit.Properties.Columns1")),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(resources.GetString("look_Cesit.Properties.Columns2"), resources.GetString("look_Cesit.Properties.Columns3"), ((int)(resources.GetObject("look_Cesit.Properties.Columns4"))), ((DevExpress.Utils.FormatType)(resources.GetObject("look_Cesit.Properties.Columns5"))), resources.GetString("look_Cesit.Properties.Columns6"), ((bool)(resources.GetObject("look_Cesit.Properties.Columns7"))), ((DevExpress.Utils.HorzAlignment)(resources.GetObject("look_Cesit.Properties.Columns8"))), ((DevExpress.Data.ColumnSortOrder)(resources.GetObject("look_Cesit.Properties.Columns9"))), ((DevExpress.Utils.DefaultBoolean)(resources.GetObject("look_Cesit.Properties.Columns10"))))});
            this.look_Cesit.Properties.NullText = resources.GetString("look_Cesit.Properties.NullText");
            this.look_Cesit.Properties.ShowFooter = false;
            this.look_Cesit.Properties.ShowHeader = false;
            this.look_Cesit.EditValueChanged += new System.EventHandler(this.look_Cesit_EditValueChanged);
            // 
            // dateEdit1
            // 
            resources.ApplyResources(this.dateEdit1, "dateEdit1");
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("dateEdit1.Properties.Appearance.Font")));
            this.dateEdit1.Properties.Appearance.Options.UseFont = true;
            this.dateEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("dateEdit1.Properties.Buttons"))))});
            this.dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEdit1.Properties.Mask.MaskType = ((DevExpress.XtraEditors.Mask.MaskType)(resources.GetObject("dateEdit1.Properties.Mask.MaskType")));
            // 
            // Kurlar
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "Kurlar";
            this.Load += new System.EventHandler(this.Kurlar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_Cesit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.LookUpEdit look_Cesit;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_Transfer;
        private DevExpress.XtraEditors.SimpleButton btn_Kaydet;
        private DevExpress.XtraEditors.SimpleButton btn_Listele;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.SimpleButton btn_Sil;
        private DevExpress.XtraEditors.SimpleButton btn_Aktar;
    }
}