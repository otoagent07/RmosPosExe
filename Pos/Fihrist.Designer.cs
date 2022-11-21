namespace Pos
{
    partial class Fihrist
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fihrist));
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule1 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule2 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            this.txt_Soyad = new DevExpress.XtraEditors.TextEdit();
            this.textEdit58 = new DevExpress.XtraEditors.TextEdit();
            this.txt_Ad = new DevExpress.XtraEditors.TextEdit();
            this.textEdit60 = new DevExpress.XtraEditors.TextEdit();
            this.txt_Tel1 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit62 = new DevExpress.XtraEditors.TextEdit();
            this.txt_Tel2 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit3 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit4 = new DevExpress.XtraEditors.TextEdit();
            this.grd_Fihrist = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_Cikis = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Sil = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Kaydet = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Duzelt = new DevExpress.XtraEditors.SimpleButton();
            this.txt_Adres = new DevExpress.XtraEditors.MemoEdit();
            this.txt_AdresTarif = new DevExpress.XtraEditors.MemoEdit();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            this.btn_Print = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Soyad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit58.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Ad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit60.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Tel1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit62.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Tel2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grd_Fihrist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Adres.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_AdresTarif.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_Soyad
            // 
            resources.ApplyResources(this.txt_Soyad, "txt_Soyad");
            this.txt_Soyad.EnterMoveNextControl = true;
            this.txt_Soyad.Name = "txt_Soyad";
            this.txt_Soyad.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Soyad.Properties.MaxLength = 100;
            conditionValidationRule1.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule1.ErrorText = "This value is not valid";
            this.dxValidationProvider1.SetValidationRule(this.txt_Soyad, conditionValidationRule1);
            // 
            // textEdit58
            // 
            resources.ApplyResources(this.textEdit58, "textEdit58");
            this.textEdit58.Name = "textEdit58";
            this.textEdit58.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit58.Properties.ReadOnly = true;
            this.textEdit58.TabStop = false;
            // 
            // txt_Ad
            // 
            resources.ApplyResources(this.txt_Ad, "txt_Ad");
            this.txt_Ad.EnterMoveNextControl = true;
            this.txt_Ad.Name = "txt_Ad";
            this.txt_Ad.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Ad.Properties.MaxLength = 100;
            conditionValidationRule2.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule2.ErrorText = "This value is not valid";
            this.dxValidationProvider1.SetValidationRule(this.txt_Ad, conditionValidationRule2);
            // 
            // textEdit60
            // 
            resources.ApplyResources(this.textEdit60, "textEdit60");
            this.textEdit60.Name = "textEdit60";
            this.textEdit60.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit60.Properties.ReadOnly = true;
            this.textEdit60.TabStop = false;
            // 
            // txt_Tel1
            // 
            resources.ApplyResources(this.txt_Tel1, "txt_Tel1");
            this.txt_Tel1.EnterMoveNextControl = true;
            this.txt_Tel1.Name = "txt_Tel1";
            this.txt_Tel1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Tel1.Properties.MaxLength = 100;
            // 
            // textEdit62
            // 
            resources.ApplyResources(this.textEdit62, "textEdit62");
            this.textEdit62.Name = "textEdit62";
            this.textEdit62.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit62.Properties.ReadOnly = true;
            this.textEdit62.TabStop = false;
            // 
            // txt_Tel2
            // 
            resources.ApplyResources(this.txt_Tel2, "txt_Tel2");
            this.txt_Tel2.EnterMoveNextControl = true;
            this.txt_Tel2.Name = "txt_Tel2";
            this.txt_Tel2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Tel2.Properties.MaxLength = 100;
            // 
            // textEdit2
            // 
            resources.ApplyResources(this.textEdit2, "textEdit2");
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit2.Properties.ReadOnly = true;
            this.textEdit2.TabStop = false;
            // 
            // textEdit3
            // 
            resources.ApplyResources(this.textEdit3, "textEdit3");
            this.textEdit3.Name = "textEdit3";
            this.textEdit3.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit3.Properties.ReadOnly = true;
            this.textEdit3.TabStop = false;
            // 
            // textEdit4
            // 
            resources.ApplyResources(this.textEdit4, "textEdit4");
            this.textEdit4.Name = "textEdit4";
            this.textEdit4.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.textEdit4.Properties.ReadOnly = true;
            this.textEdit4.TabStop = false;
            // 
            // grd_Fihrist
            // 
            resources.ApplyResources(this.grd_Fihrist, "grd_Fihrist");
            this.grd_Fihrist.EmbeddedNavigator.AccessibleDescription = resources.GetString("grd_Fihrist.EmbeddedNavigator.AccessibleDescription");
            this.grd_Fihrist.EmbeddedNavigator.AccessibleName = resources.GetString("grd_Fihrist.EmbeddedNavigator.AccessibleName");
            this.grd_Fihrist.EmbeddedNavigator.AllowHtmlTextInToolTip = ((DevExpress.Utils.DefaultBoolean)(resources.GetObject("grd_Fihrist.EmbeddedNavigator.AllowHtmlTextInToolTip")));
            this.grd_Fihrist.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grd_Fihrist.EmbeddedNavigator.Anchor")));
            this.grd_Fihrist.EmbeddedNavigator.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grd_Fihrist.EmbeddedNavigator.BackgroundImage")));
            this.grd_Fihrist.EmbeddedNavigator.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("grd_Fihrist.EmbeddedNavigator.BackgroundImageLayout")));
            this.grd_Fihrist.EmbeddedNavigator.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grd_Fihrist.EmbeddedNavigator.ImeMode")));
            this.grd_Fihrist.EmbeddedNavigator.MaximumSize = ((System.Drawing.Size)(resources.GetObject("grd_Fihrist.EmbeddedNavigator.MaximumSize")));
            this.grd_Fihrist.EmbeddedNavigator.TextLocation = ((DevExpress.XtraEditors.NavigatorButtonsTextLocation)(resources.GetObject("grd_Fihrist.EmbeddedNavigator.TextLocation")));
            this.grd_Fihrist.EmbeddedNavigator.ToolTip = resources.GetString("grd_Fihrist.EmbeddedNavigator.ToolTip");
            this.grd_Fihrist.EmbeddedNavigator.ToolTipIconType = ((DevExpress.Utils.ToolTipIconType)(resources.GetObject("grd_Fihrist.EmbeddedNavigator.ToolTipIconType")));
            this.grd_Fihrist.EmbeddedNavigator.ToolTipTitle = resources.GetString("grd_Fihrist.EmbeddedNavigator.ToolTipTitle");
            this.grd_Fihrist.MainView = this.gridView1;
            this.grd_Fihrist.Name = "grd_Fihrist";
            this.grd_Fihrist.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            resources.ApplyResources(this.gridView1, "gridView1");
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7});
            this.gridView1.GridControl = this.grd_Fihrist;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView1_RowCellClick);
            // 
            // gridColumn1
            // 
            resources.ApplyResources(this.gridColumn1, "gridColumn1");
            this.gridColumn1.FieldName = "F_Id";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem()});
            // 
            // gridColumn2
            // 
            resources.ApplyResources(this.gridColumn2, "gridColumn2");
            this.gridColumn2.FieldName = "F_Ad";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem()});
            // 
            // gridColumn3
            // 
            resources.ApplyResources(this.gridColumn3, "gridColumn3");
            this.gridColumn3.FieldName = "F_Soyad";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem()});
            // 
            // gridColumn4
            // 
            resources.ApplyResources(this.gridColumn4, "gridColumn4");
            this.gridColumn4.FieldName = "F_Tel1";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem()});
            // 
            // gridColumn5
            // 
            resources.ApplyResources(this.gridColumn5, "gridColumn5");
            this.gridColumn5.FieldName = "F_Tel2";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem()});
            // 
            // gridColumn6
            // 
            resources.ApplyResources(this.gridColumn6, "gridColumn6");
            this.gridColumn6.FieldName = "F_Adres";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            this.gridColumn6.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem()});
            // 
            // gridColumn7
            // 
            resources.ApplyResources(this.gridColumn7, "gridColumn7");
            this.gridColumn7.FieldName = "F_AdresTarif";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            this.gridColumn7.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem()});
            // 
            // btn_Cikis
            // 
            resources.ApplyResources(this.btn_Cikis, "btn_Cikis");
            this.btn_Cikis.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Cikis.Appearance.Font")));
            this.btn_Cikis.Appearance.Options.UseFont = true;
            this.btn_Cikis.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cikis.ImageOptions.Image")));
            this.btn_Cikis.Name = "btn_Cikis";
            this.btn_Cikis.Click += new System.EventHandler(this.btn_Cikis_Click);
            // 
            // btn_Sil
            // 
            resources.ApplyResources(this.btn_Sil, "btn_Sil");
            this.btn_Sil.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Sil.Appearance.Font")));
            this.btn_Sil.Appearance.Options.UseFont = true;
            this.btn_Sil.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Sil.ImageOptions.Image")));
            this.btn_Sil.Name = "btn_Sil";
            this.btn_Sil.Click += new System.EventHandler(this.btn_Sil_Click);
            // 
            // btn_Kaydet
            // 
            resources.ApplyResources(this.btn_Kaydet, "btn_Kaydet");
            this.btn_Kaydet.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Kaydet.Appearance.Font")));
            this.btn_Kaydet.Appearance.Options.UseFont = true;
            this.btn_Kaydet.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_Kaydet.ImageOptions.Image")));
            this.btn_Kaydet.Name = "btn_Kaydet";
            this.btn_Kaydet.Click += new System.EventHandler(this.btn_Kaydet_Click);
            // 
            // btn_Duzelt
            // 
            resources.ApplyResources(this.btn_Duzelt, "btn_Duzelt");
            this.btn_Duzelt.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Duzelt.Appearance.Font")));
            this.btn_Duzelt.Appearance.Options.UseFont = true;
            this.btn_Duzelt.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Duzelt.ImageOptions.SvgImage")));
            this.btn_Duzelt.Name = "btn_Duzelt";
            this.btn_Duzelt.Click += new System.EventHandler(this.btn_Duzelt_Click);
            // 
            // txt_Adres
            // 
            resources.ApplyResources(this.txt_Adres, "txt_Adres");
            this.txt_Adres.Name = "txt_Adres";
            this.txt_Adres.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_Adres.Properties.MaxLength = 100;
            // 
            // txt_AdresTarif
            // 
            resources.ApplyResources(this.txt_AdresTarif, "txt_AdresTarif");
            this.txt_AdresTarif.Name = "txt_AdresTarif";
            this.txt_AdresTarif.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.txt_AdresTarif.Properties.MaxLength = 100;
            // 
            // btn_Print
            // 
            resources.ApplyResources(this.btn_Print, "btn_Print");
            this.btn_Print.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("btn_Print.Appearance.Font")));
            this.btn_Print.Appearance.Options.UseFont = true;
            this.btn_Print.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Print.ImageOptions.SvgImage")));
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Click += new System.EventHandler(this.btn_Print_Click);
            // 
            // Fihrist
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_Print);
            this.Controls.Add(this.txt_AdresTarif);
            this.Controls.Add(this.btn_Duzelt);
            this.Controls.Add(this.btn_Cikis);
            this.Controls.Add(this.btn_Sil);
            this.Controls.Add(this.btn_Kaydet);
            this.Controls.Add(this.grd_Fihrist);
            this.Controls.Add(this.textEdit4);
            this.Controls.Add(this.textEdit3);
            this.Controls.Add(this.txt_Tel2);
            this.Controls.Add(this.textEdit2);
            this.Controls.Add(this.txt_Soyad);
            this.Controls.Add(this.textEdit58);
            this.Controls.Add(this.txt_Ad);
            this.Controls.Add(this.textEdit60);
            this.Controls.Add(this.txt_Tel1);
            this.Controls.Add(this.textEdit62);
            this.Controls.Add(this.txt_Adres);
            this.Name = "Fihrist";
            this.Load += new System.EventHandler(this.Fihrist_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Soyad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit58.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Ad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit60.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Tel1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit62.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Tel2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grd_Fihrist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Adres.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_AdresTarif.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txt_Soyad;
        private DevExpress.XtraEditors.TextEdit textEdit58;
        private DevExpress.XtraEditors.TextEdit txt_Ad;
        private DevExpress.XtraEditors.TextEdit textEdit60;
        private DevExpress.XtraEditors.TextEdit txt_Tel1;
        private DevExpress.XtraEditors.TextEdit textEdit62;
        private DevExpress.XtraEditors.TextEdit txt_Tel2;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraEditors.TextEdit textEdit3;
        private DevExpress.XtraEditors.TextEdit textEdit4;
        private DevExpress.XtraGrid.GridControl grd_Fihrist;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.SimpleButton btn_Cikis;
        private DevExpress.XtraEditors.SimpleButton btn_Sil;
        private DevExpress.XtraEditors.SimpleButton btn_Kaydet;
        private DevExpress.XtraEditors.SimpleButton btn_Duzelt;
        private DevExpress.XtraEditors.MemoEdit txt_Adres;
        private DevExpress.XtraEditors.MemoEdit txt_AdresTarif;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
        private DevExpress.XtraEditors.SimpleButton btn_Print;
    }
}