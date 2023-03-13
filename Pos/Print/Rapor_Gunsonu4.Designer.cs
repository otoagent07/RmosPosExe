namespace Pos.Print
{
    partial class Rapor_Gunsonu4
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
            this.lbl_Masa = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_ToplamTutar = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_NetTutar = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_Fisno = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_IndirimTutar = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_RaporTarih = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_Kullanici = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_Restorant = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_SistemTarih = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11,
            this.lbl_Masa,
            this.lbl_ToplamTutar,
            this.lbl_NetTutar,
            this.lbl_Fisno,
            this.lbl_IndirimTutar});
            this.Detail.HeightF = 19.875F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lbl_Masa
            // 
            this.lbl_Masa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_Masa.LocationFloat = new DevExpress.Utils.PointFloat(100F, 0F);
            this.lbl_Masa.Name = "lbl_Masa";
            this.lbl_Masa.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_Masa.Scripts.OnBeforePrint = "lbl_Aciklama_BeforePrint";
            this.lbl_Masa.SizeF = new System.Drawing.SizeF(104.1667F, 19.875F);
            this.lbl_Masa.StylePriority.UseFont = false;
            this.lbl_Masa.StylePriority.UseTextAlignment = false;
            this.lbl_Masa.Text = "Masa";
            this.lbl_Masa.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lbl_Masa.WordWrap = false;
            // 
            // lbl_ToplamTutar
            // 
            this.lbl_ToplamTutar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_ToplamTutar.LocationFloat = new DevExpress.Utils.PointFloat(396.8753F, 0F);
            this.lbl_ToplamTutar.Name = "lbl_ToplamTutar";
            this.lbl_ToplamTutar.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_ToplamTutar.SizeF = new System.Drawing.SizeF(113.5418F, 19.875F);
            this.lbl_ToplamTutar.StylePriority.UseFont = false;
            this.lbl_ToplamTutar.StylePriority.UseTextAlignment = false;
            this.lbl_ToplamTutar.Text = "Toplam Tutar";
            this.lbl_ToplamTutar.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbl_NetTutar
            // 
            this.lbl_NetTutar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_NetTutar.LocationFloat = new DevExpress.Utils.PointFloat(306.2502F, 0F);
            this.lbl_NetTutar.Name = "lbl_NetTutar";
            this.lbl_NetTutar.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_NetTutar.SizeF = new System.Drawing.SizeF(90.62515F, 19.875F);
            this.lbl_NetTutar.StylePriority.UseFont = false;
            this.lbl_NetTutar.StylePriority.UseTextAlignment = false;
            this.lbl_NetTutar.Text = "NetTutar";
            this.lbl_NetTutar.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbl_Fisno
            // 
            this.lbl_Fisno.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_Fisno.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lbl_Fisno.Name = "lbl_Fisno";
            this.lbl_Fisno.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_Fisno.Scripts.OnBeforePrint = "lbl_Aciklama_BeforePrint";
            this.lbl_Fisno.SizeF = new System.Drawing.SizeF(100F, 19.875F);
            this.lbl_Fisno.StylePriority.UseFont = false;
            this.lbl_Fisno.StylePriority.UseTextAlignment = false;
            this.lbl_Fisno.Text = "Fis No";
            this.lbl_Fisno.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lbl_Fisno.WordWrap = false;
            // 
            // lbl_IndirimTutar
            // 
            this.lbl_IndirimTutar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_IndirimTutar.LocationFloat = new DevExpress.Utils.PointFloat(204.1667F, 0F);
            this.lbl_IndirimTutar.Name = "lbl_IndirimTutar";
            this.lbl_IndirimTutar.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_IndirimTutar.SizeF = new System.Drawing.SizeF(102.0835F, 19.875F);
            this.lbl_IndirimTutar.StylePriority.UseFont = false;
            this.lbl_IndirimTutar.StylePriority.UseTextAlignment = false;
            this.lbl_IndirimTutar.Text = "İndirim Tutar";
            this.lbl_IndirimTutar.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 45F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel10,
            this.xrLabel9,
            this.lbl_RaporTarih,
            this.xrLabel5,
            this.lbl_Kullanici,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1,
            this.lbl_Restorant,
            this.lbl_SistemTarih,
            this.xrLabel4});
            this.ReportHeader.HeightF = 132.375F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(396.8753F, 112.5F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(113.5418F, 19.875F);
            this.xrLabel7.StylePriority.UseBorders = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Toplam Tutar";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(306.2502F, 112.5F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(90.62512F, 19.875F);
            this.xrLabel6.StylePriority.UseBorders = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "NetTutar";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(204.1667F, 112.5F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(102.0835F, 19.875F);
            this.xrLabel10.StylePriority.UseBorders = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "İndirim Tutar";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 112.5F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(100F, 19.875F);
            this.xrLabel9.StylePriority.UseBorders = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Fis No";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lbl_RaporTarih
            // 
            this.lbl_RaporTarih.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.lbl_RaporTarih.LocationFloat = new DevExpress.Utils.PointFloat(587.5F, 25F);
            this.lbl_RaporTarih.Name = "lbl_RaporTarih";
            this.lbl_RaporTarih.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_RaporTarih.SizeF = new System.Drawing.SizeF(148.9587F, 19.87501F);
            this.lbl_RaporTarih.StylePriority.UseFont = false;
            this.lbl_RaporTarih.StylePriority.UseTextAlignment = false;
            this.lbl_RaporTarih.Text = "01.05.2014 03:05:20";
            this.lbl_RaporTarih.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(100F, 112.5F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(104.1666F, 19.875F);
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Masa";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lbl_Kullanici
            // 
            this.lbl_Kullanici.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.lbl_Kullanici.LocationFloat = new DevExpress.Utils.PointFloat(100F, 64.75F);
            this.lbl_Kullanici.Name = "lbl_Kullanici";
            this.lbl_Kullanici.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_Kullanici.SizeF = new System.Drawing.SizeF(225F, 19.875F);
            this.lbl_Kullanici.StylePriority.UseFont = false;
            this.lbl_Kullanici.StylePriority.UseTextAlignment = false;
            this.lbl_Kullanici.Text = "RMOS YAZILIM";
            this.lbl_Kullanici.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 44.875F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 19.875F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Restorant :";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(100F, 19.875F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Tarih       :";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel1.BorderWidth = 2F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(736.4588F, 23F);
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseBorderWidth = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Gün Sonu İndirim Raporu";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lbl_Restorant
            // 
            this.lbl_Restorant.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.lbl_Restorant.LocationFloat = new DevExpress.Utils.PointFloat(100F, 44.875F);
            this.lbl_Restorant.Name = "lbl_Restorant";
            this.lbl_Restorant.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_Restorant.SizeF = new System.Drawing.SizeF(225F, 19.875F);
            this.lbl_Restorant.StylePriority.UseFont = false;
            this.lbl_Restorant.StylePriority.UseTextAlignment = false;
            this.lbl_Restorant.Text = "ABC Restorant";
            this.lbl_Restorant.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lbl_SistemTarih
            // 
            this.lbl_SistemTarih.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.lbl_SistemTarih.LocationFloat = new DevExpress.Utils.PointFloat(100F, 25F);
            this.lbl_SistemTarih.Name = "lbl_SistemTarih";
            this.lbl_SistemTarih.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_SistemTarih.SizeF = new System.Drawing.SizeF(225F, 19.875F);
            this.lbl_SistemTarih.StylePriority.UseFont = false;
            this.lbl_SistemTarih.StylePriority.UseTextAlignment = false;
            this.lbl_SistemTarih.Text = "01.05.2014";
            this.lbl_SistemTarih.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 64.75F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(100F, 19.875F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Kullanıcı   :";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(510.4171F, 112.5F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(226.0416F, 19.875F);
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Neden";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(510.4171F, 0F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(226.0416F, 19.875F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "[Rsat_Aciklama]";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // Rapor_Gunsonu4
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.Margins = new System.Drawing.Printing.Margins(56, 57, 45, 100);
            this.Version = "19.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        public DevExpress.XtraReports.UI.XRLabel lbl_RaporTarih;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        public DevExpress.XtraReports.UI.XRLabel lbl_Kullanici;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        public DevExpress.XtraReports.UI.XRLabel lbl_Restorant;
        public DevExpress.XtraReports.UI.XRLabel lbl_SistemTarih;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        public DevExpress.XtraReports.UI.XRLabel lbl_Masa;
        public DevExpress.XtraReports.UI.XRLabel lbl_ToplamTutar;
        public DevExpress.XtraReports.UI.XRLabel lbl_NetTutar;
        public DevExpress.XtraReports.UI.XRLabel lbl_Fisno;
        public DevExpress.XtraReports.UI.XRLabel lbl_IndirimTutar;
        public DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
    }
}
