
namespace Pos
{
    partial class HesapKisiyeSatis
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControlKisiyeSatis = new DevExpress.XtraGrid.GridControl();
            this.gridViewKisiyeSatis = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControlFis = new DevExpress.XtraGrid.GridControl();
            this.gridViewFis = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlKisiyeSatis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewKisiyeSatis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlFis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewFis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(943, 15);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gridControlKisiyeSatis);
            this.panelControl2.Controls.Add(this.gridControlFis);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 15);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(943, 595);
            this.panelControl2.TabIndex = 1;
            // 
            // gridControlKisiyeSatis
            // 
            this.gridControlKisiyeSatis.Location = new System.Drawing.Point(2, 2);
            this.gridControlKisiyeSatis.MainView = this.gridViewKisiyeSatis;
            this.gridControlKisiyeSatis.Name = "gridControlKisiyeSatis";
            this.gridControlKisiyeSatis.Size = new System.Drawing.Size(513, 591);
            this.gridControlKisiyeSatis.TabIndex = 0;
            this.gridControlKisiyeSatis.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewKisiyeSatis});
            this.gridControlKisiyeSatis.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // gridViewKisiyeSatis
            // 
            this.gridViewKisiyeSatis.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridViewKisiyeSatis.Appearance.Row.Options.UseFont = true;
            this.gridViewKisiyeSatis.GridControl = this.gridControlKisiyeSatis;
            this.gridViewKisiyeSatis.Name = "gridViewKisiyeSatis";
            this.gridViewKisiyeSatis.OptionsBehavior.ReadOnly = true;
            this.gridViewKisiyeSatis.OptionsView.ColumnAutoWidth = false;
            this.gridViewKisiyeSatis.OptionsView.ShowAutoFilterRow = true;
            this.gridViewKisiyeSatis.OptionsView.ShowFooter = true;
            this.gridViewKisiyeSatis.OptionsView.ShowGroupPanel = false;
            this.gridViewKisiyeSatis.RowHeight = 30;
            // 
            // gridControlFis
            // 
            this.gridControlFis.Location = new System.Drawing.Point(515, 2);
            this.gridControlFis.MainView = this.gridViewFis;
            this.gridControlFis.Name = "gridControlFis";
            this.gridControlFis.Size = new System.Drawing.Size(426, 591);
            this.gridControlFis.TabIndex = 1;
            this.gridControlFis.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewFis});
            // 
            // gridViewFis
            // 
            this.gridViewFis.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gridViewFis.Appearance.Row.Options.UseFont = true;
            this.gridViewFis.GridControl = this.gridControlFis;
            this.gridViewFis.Name = "gridViewFis";
            this.gridViewFis.OptionsBehavior.ReadOnly = true;
            this.gridViewFis.OptionsView.ColumnAutoWidth = false;
            this.gridViewFis.OptionsView.ShowAutoFilterRow = true;
            this.gridViewFis.OptionsView.ShowFooter = true;
            this.gridViewFis.OptionsView.ShowGroupPanel = false;
            this.gridViewFis.RowHeight = 30;
            // 
            // panelControl3
            // 
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 610);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(943, 16);
            this.panelControl3.TabIndex = 1;
            // 
            // HesapKisiyeSatis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 626);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl1);
            this.Name = "HesapKisiyeSatis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hesap Kişiye Satış";
            this.Load += new System.EventHandler(this.HesapKisiyeSatis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlKisiyeSatis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewKisiyeSatis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlFis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewFis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraGrid.GridControl gridControlKisiyeSatis;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewKisiyeSatis;
        private DevExpress.XtraGrid.GridControl gridControlFis;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewFis;
    }
}