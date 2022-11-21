namespace Pos.Print
{
    partial class DynamicReport
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xr_Baslik = new DevExpress.XtraReports.UI.XRLabel();
            this.txt_OtelAdi = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xr_Date = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 6.25F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 6.25F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xr_Date,
            this.xrLine1,
            this.xr_Baslik,
            this.txt_OtelAdi});
            this.PageHeader.HeightF = 79.00002F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrLine1
            // 
            this.xrLine1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 56.00001F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(789.9999F, 23F);
            this.xrLine1.StylePriority.UseBorderDashStyle = false;
            // 
            // xr_Baslik
            // 
            this.xr_Baslik.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.xr_Baslik.ForeColor = System.Drawing.Color.Black;
            this.xr_Baslik.LocationFloat = new DevExpress.Utils.PointFloat(0F, 33.00001F);
            this.xr_Baslik.Name = "xr_Baslik";
            this.xr_Baslik.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xr_Baslik.SizeF = new System.Drawing.SizeF(789.9999F, 23F);
            this.xr_Baslik.StylePriority.UseFont = false;
            this.xr_Baslik.StylePriority.UseForeColor = false;
            this.xr_Baslik.StylePriority.UseTextAlignment = false;
            this.xr_Baslik.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // txt_OtelAdi
            // 
            this.txt_OtelAdi.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_OtelAdi.ForeColor = System.Drawing.Color.Black;
            this.txt_OtelAdi.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.txt_OtelAdi.Name = "txt_OtelAdi";
            this.txt_OtelAdi.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txt_OtelAdi.SizeF = new System.Drawing.SizeF(643.1249F, 23F);
            this.txt_OtelAdi.StylePriority.UseFont = false;
            this.txt_OtelAdi.StylePriority.UseForeColor = false;
            this.txt_OtelAdi.StylePriority.UseTextAlignment = false;
            this.txt_OtelAdi.Text = "Otel Adi";
            this.txt_OtelAdi.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrLabel3,
            this.xrLine3});
            this.PageFooter.HeightF = 48F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Verdana", 8F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(744.8283F, 23F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(45.17151F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.xrLabel3.ForeColor = System.Drawing.Color.Black;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(686.4949F, 23F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(58.33337F, 23F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Sayfa :";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLine3
            // 
            this.xrLine3.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(789.9999F, 23F);
            this.xrLine3.StylePriority.UseBorderDashStyle = false;
            // 
            // xr_Date
            // 
            this.xr_Date.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.xr_Date.ForeColor = System.Drawing.Color.Black;
            this.xr_Date.LocationFloat = new DevExpress.Utils.PointFloat(643.1249F, 10.00001F);
            this.xr_Date.Name = "xr_Date";
            this.xr_Date.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xr_Date.SizeF = new System.Drawing.SizeF(146.875F, 23F);
            this.xr_Date.StylePriority.UseFont = false;
            this.xr_Date.StylePriority.UseForeColor = false;
            this.xr_Date.StylePriority.UseTextAlignment = false;
            this.xr_Date.Text = "99.99.9999 99:99:99";
            this.xr_Date.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // DynamicReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter});
            this.Margins = new System.Drawing.Printing.Margins(18, 19, 6, 6);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Version = "15.1";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.DynamicReport_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLine xrLine3;
        public DevExpress.XtraReports.UI.XRLabel txt_OtelAdi;
        private DevExpress.XtraReports.UI.XRLabel xr_Baslik;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        public DevExpress.XtraReports.UI.XRLabel xr_Date;
    }
}
