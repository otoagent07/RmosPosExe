namespace Pos
{
    partial class Hesap_Tr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hesap_Tr));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txt_Masano = new DevExpress.XtraEditors.TextEdit();
            this.textEdit8 = new DevExpress.XtraEditors.TextEdit();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Transfer = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Masano.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit8.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridColumn3
            // 
            resources.ApplyResources(this.gridColumn3, "gridColumn3");
            this.gridColumn3.FieldName = "Masa_Durum";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn2.AppearanceCell.Font")));
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn2, "gridColumn2");
            this.gridColumn2.FieldName = "Masa_Ad";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            // 
            // txt_Masano
            // 
            resources.ApplyResources(this.txt_Masano, "txt_Masano");
            this.txt_Masano.Name = "txt_Masano";
            this.txt_Masano.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("txt_Masano.Properties.Appearance.Font")));
            this.txt_Masano.Properties.Appearance.Options.UseFont = true;
            this.txt_Masano.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Masano.Properties.ReadOnly = true;
            // 
            // textEdit8
            // 
            resources.ApplyResources(this.textEdit8, "textEdit8");
            this.textEdit8.Name = "textEdit8";
            this.textEdit8.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("textEdit8.Properties.Appearance.Font")));
            this.textEdit8.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(37)))), ((int)(((byte)(127)))));
            this.textEdit8.Properties.Appearance.Options.UseFont = true;
            this.textEdit8.Properties.Appearance.Options.UseForeColor = true;
            this.textEdit8.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit8.Properties.ReadOnly = true;
            this.textEdit8.TabStop = false;
            // 
            // btn_Cikis
            // 
            resources.ApplyResources(this.btn_Cikis, "btn_Cikis");
            this.btn_Cikis.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Cikis.Appearance.Font")));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cikis.ImageOptions.Image")));
            this.btn_Cikis.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // btn_Transfer
            // 
            resources.ApplyResources(this.btn_Transfer, "btn_Transfer");
            this.btn_Transfer.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Transfer.Appearance.Font")));
            this.btn_Transfer.Appearance.Options.UseFont = true;
            this.btn_Transfer.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Transfer.ImageOptions.Image")));
            this.btn_Transfer.Name = "btn_Transfer";
            this.btn_Transfer.Click += new System.EventHandler(this.btn_Transfer_Click);
            // 
            // gridControl2
            // 
            resources.ApplyResources(this.gridControl2, "gridControl2");
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            gridFormatRule1.Column = this.gridColumn3;
            gridFormatRule1.ColumnApplyTo = this.gridColumn2;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleValue1.Appearance.ForeColor = System.Drawing.Color.Red;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.NotEqual;
            formatConditionRuleValue1.Value1 = 0;
            gridFormatRule1.Rule = formatConditionRuleValue1;
            this.gridView2.FormatRules.Add(gridFormatRule1);
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridView2.RowHeight = 32;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn1.AppearanceCell.Font")));
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn1, "gridColumn1");
            this.gridColumn1.FieldName = "Masa_No";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn4.AppearanceCell.Font")));
            this.gridColumn4.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceCell.Options.UseForeColor = true;
            resources.ApplyResources(this.gridColumn4, "gridColumn4");
            this.gridColumn4.FieldName = "Masa_Ozel";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowFocus = false;
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
            this.gridColumn8,
            this.gridColumn9});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.RowHeight = 32;
            this.gridView1.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView1_RowCellClick);
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn8.AppearanceCell.Font")));
            this.gridColumn8.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.gridColumn8, "gridColumn8");
            this.gridColumn8.FieldName = "Kodlar_Kod";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn9.AppearanceCell.Font")));
            this.gridColumn9.AppearanceCell.Options.UseFont = true;
            resources.ApplyResources(this.gridColumn9, "gridColumn9");
            this.gridColumn9.FieldName = "Kodlar_Ad";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowFocus = false;
            // 
            // Hesap_Tr
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_Cikis);
            this.Controls.Add(this.btn_Transfer);
            this.Controls.Add(this.gridControl2);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.txt_Masano);
            this.Controls.Add(this.textEdit8);
            this.Name = "Hesap_Tr";
            this.Load += new System.EventHandler(this.Hesap_Tr_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Masano.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit8.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.TextEdit txt_Masano;
        private DevExpress.XtraEditors.TextEdit textEdit8;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private DevExpress.XtraEditors.SimpleButton btn_Transfer;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}