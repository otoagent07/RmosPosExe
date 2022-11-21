namespace Pos
{
    partial class Detay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Detay));
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.spn_Fisno = new DevExpress.XtraEditors.SpinEdit();
            this.btn_Fispr = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Adisyonpr = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Faturapr = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.btn_HesabaFatura = new DevExpress.XtraEditors.SimpleButton();
            this.btnHesapDokum = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdisyonR = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdisyonG = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spn_Fisno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridColumn6
            // 
            resources.ApplyResources(this.gridColumn6, "gridColumn6");
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            // 
            // textEdit1
            // 
            resources.ApplyResources(this.textEdit1, "textEdit1");
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit1.Properties.ReadOnly = true;
            this.textEdit1.TabStop = false;
            // 
            // spn_Fisno
            // 
            resources.ApplyResources(this.spn_Fisno, "spn_Fisno");
            this.spn_Fisno.Name = "spn_Fisno";
            this.spn_Fisno.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.spn_Fisno.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spn_Fisno.Properties.ReadOnly = true;
            this.spn_Fisno.TabStop = false;
            // 
            // btn_Fispr
            // 
            this.btn_Fispr.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Fispr.Appearance.Font")));
            this.btn_Fispr.Appearance.Options.UseFont = true;
            this.btn_Fispr.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Fispr.ImageOptions.Image")));
            resources.ApplyResources(this.btn_Fispr, "btn_Fispr");
            this.btn_Fispr.Name = "btn_Fispr";
            this.btn_Fispr.Click += new System.EventHandler(this.btn_Fispr_Click);
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
            this.gridColumn6});
            styleFormatCondition2.Appearance.ForeColor = System.Drawing.Color.Red;
            styleFormatCondition2.Appearance.Options.UseForeColor = true;
            styleFormatCondition2.ApplyToRow = true;
            styleFormatCondition2.Column = this.gridColumn6;
            styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition2.Value1 = "A";
            this.gridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition2});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowHeight = 25;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn1.AppearanceCell.Font")));
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.gridColumn1, "gridColumn1");
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(((DevExpress.Data.SummaryItemType)(resources.GetObject("gridColumn1.Summary"))))});
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn2.AppearanceCell.Font")));
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.gridColumn2, "gridColumn2");
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn3.AppearanceCell.Font")));
            this.gridColumn3.AppearanceCell.ForeColor = System.Drawing.Color.Red;
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseForeColor = true;
            resources.ApplyResources(this.gridColumn3, "gridColumn3");
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn4.AppearanceCell.Font")));
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.gridColumn4, "gridColumn4");
            this.gridColumn4.DisplayFormat.FormatString = "n2";
            this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(((DevExpress.Data.SummaryItemType)(resources.GetObject("gridColumn4.Summary"))))});
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn5.AppearanceCell.Font")));
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.gridColumn5, "gridColumn5");
            this.gridColumn5.DisplayFormat.FormatString = "n2";
            this.gridColumn5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(((DevExpress.Data.SummaryItemType)(resources.GetObject("gridColumn5.Summary"))))});
            // 
            // btn_Cikis
            // 
            this.btn_Cikis.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Cikis.Appearance.Font")));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cikis.ImageOptions.Image")));
            resources.ApplyResources(this.btn_Cikis, "btn_Cikis");
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // btn_HesabaFatura
            // 
            this.btn_HesabaFatura.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_HesabaFatura.Appearance.Font")));
            this.btn_HesabaFatura.Appearance.Options.UseFont = true;
            this.btn_HesabaFatura.Appearance.Options.UseTextOptions = true;
            this.btn_HesabaFatura.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btn_HesabaFatura.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_HesabaFatura.ImageOptions.Image")));
            resources.ApplyResources(this.btn_HesabaFatura, "btn_HesabaFatura");
            this.btn_HesabaFatura.Name = "btn_HesabaFatura";
            this.btn_HesabaFatura.Click += new System.EventHandler(this.btn_HesabaFatura_Click);
            // 
            // btnHesapDokum
            // 
            this.btnHesapDokum.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btnHesapDokum.Appearance.Font")));
            this.btnHesapDokum.Appearance.Options.UseFont = true;
            this.btnHesapDokum.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHesapDokum.ImageOptions.Image")));
            resources.ApplyResources(this.btnHesapDokum, "btnHesapDokum");
            this.btnHesapDokum.Name = "btnHesapDokum";
            this.btnHesapDokum.Click += new System.EventHandler(this.btnHesapDokum_Click);
            // 
            // btnAdisyonR
            // 
            this.btnAdisyonR.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            resources.ApplyResources(this.btnAdisyonR, "btnAdisyonR");
            this.btnAdisyonR.Name = "btnAdisyonR";
            this.btnAdisyonR.Click += new System.EventHandler(this.btnAdisyonR_Click);
            // 
            // btnAdisyonG
            // 
            this.btnAdisyonG.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.ImageOptions.Image")));
            resources.ApplyResources(this.btnAdisyonG, "btnAdisyonG");
            this.btnAdisyonG.Name = "btnAdisyonG";
            this.btnAdisyonG.Click += new System.EventHandler(this.btnAdisyonG_Click);
            // 
            // Detay
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAdisyonG);
            this.Controls.Add(this.btnAdisyonR);
            this.Controls.Add(this.btnHesapDokum);
            this.Controls.Add(this.btn_Cikis);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btn_HesabaFatura);
            this.Controls.Add(this.btn_Faturapr);
            this.Controls.Add(this.btn_Adisyonpr);
            this.Controls.Add(this.btn_Fispr);
            this.Controls.Add(this.spn_Fisno);
            this.Controls.Add(this.textEdit1);
            this.Name = "Detay";
            this.Load += new System.EventHandler(this.Detay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spn_Fisno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        public DevExpress.XtraEditors.SpinEdit spn_Fisno;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        public DevExpress.XtraEditors.SimpleButton btn_Fispr;
        public DevExpress.XtraEditors.SimpleButton btn_Adisyonpr;
        public DevExpress.XtraEditors.SimpleButton btn_Faturapr;
        public DevExpress.XtraEditors.SimpleButton btn_HesabaFatura;
        public DevExpress.XtraEditors.SimpleButton btnHesapDokum;
        private DevExpress.XtraEditors.SimpleButton btnAdisyonR;
        private DevExpress.XtraEditors.SimpleButton btnAdisyonG;
    }
}