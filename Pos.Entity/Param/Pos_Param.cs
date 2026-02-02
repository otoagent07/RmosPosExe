using Dapper.Contrib.Extensions;
using System;

namespace Pos.Entity.Param
{
    [Table("Pos_Param")]
    public class Pos_Param
    {
        [Key]
        public int Param_Id { get; set; }

        public string Param_TesisAdi { get; set; }

        public bool? Param_Onburo { get; set; }

        public bool? Param_Cost { get; set; }

        public bool? Param_Muh { get; set; }

        public int? Param_CalismaSekli { get; set; }

        public int? Param_Merkez_Ex { get; set; }

        public int? Param_Tar_Nere { get; set; }

        public int? Param_Kur_Nere { get; set; }

        public int? Param_Limittip { get; set; }

        public int? Param_Balance { get; set; }

        public int? Param_Fis_Dovizli { get; set; }

        public int? Param_Tesistip { get; set; }

        public DateTime? Param_Tarih { get; set; }

        public string Param_Dovizkod { get; set; }

        public decimal? Param_Kur { get; set; }

        public bool? Param_Yansit { get; set; }

        public string Param_Discount { get; set; }

        public bool? Param_Kartla_Giris { get; set; }

        public int? Param_Sirket { get; set; }

        public string Param_Otel { get; set; }

        public decimal? Param_Miktar { get; set; }

        public string Param_Server { get; set; }

        public string Param_Database { get; set; }

        public bool? Param_Hesap { get; set; }

        public int? Param_Urun_Sirala { get; set; }

        public bool? Param_Garson_Sifre { get; set; }

        public string Param_Doviz_Turu { get; set; }

        public bool? Param_SatirSil_EksiUrun { get; set; }

        public bool? Param_Hesap_Cift { get; set; }

        public bool? Param_Kurus_Farki { get; set; }

        public decimal? Param_Pda_Font { get; set; }

        public bool? Param_Miktar_Duzelt { get; set; }

        public bool? Param_OnbKontrol { get; set; }

        public string Param_Masa_Refresh { get; set; }

        public bool? Param_Pda_HotelSu { get; set; }

        public bool? Param_Yarim_Tam { get; set; } // Çek iptalde iptal fişi yazdırmak için kullanıyoruz

        public string Param_FullComp { get; set; }

        public bool? Param_HH_Ind { get; set; }

        public bool? Param_Fis_Urungrup { get; set; }

        public int? Param_Rec_Bas { get; set; }

        public int? Param_Rec_Hane { get; set; }

        public int? Param_KG_Bas { get; set; }

        public int? Param_KG_Hane { get; set; }

        public int? Param_GR_Bas { get; set; }

        public int? Param_GR_Hane { get; set; }

        public int? Param_Pda_Height { get; set; }

        public int? Param_Pda_Width { get; set; }

        public bool? Param_Masa_Geri { get; set; }

        public string Param_Rapor_Design { get; set; }

        public bool? Param_Pda_Kartsor { get; set; }

        public bool? Param_Anagrup_Cikmasin { get; set; }

        public bool? Param_Satis_YD { get; set; }

        public bool? Param_Paket_YD { get; set; }

        public bool? Param_Dep_Fiyat { get; set; }

        public bool? Param_Masatr_Uyari { get; set; }

        public string Param_Bindirim { get; set; }

        public bool? Param_Pda_Fullscreen { get; set; }

        public bool? Param_Printer_Tanim { get; set; }

        public bool? Param_Hesap_Disable { get; set; }

        public string Param_Yuvarla { get; set; }

        public int? Param_Yuv_Sayi { get; set; }

        public bool? Param_Hsifir_Ikram { get; set; }

        public bool? Param_Adispr_Uyari { get; set; }

        public bool? Param_Paketci_Sor { get; set; }

        public bool? Param_Gunsonu_Aktar { get; set; }

        public bool? Param_Masa_Garson { get; set; }

        public bool? Param_Extre_Cikmasin { get; set; }

        public string Param_Kart_Yoksay { get; set; }

        public bool? Param_Adis_Doviz { get; set; }

        public bool? Param_Tum_Paket { get; set; }

        public bool? Param_Siparis_Uyari { get; set; }

        public bool? Param_Hesap_Kilit { get; set; }

        public bool? Param_Masaacan_Garson { get; set; }

        public string Param_Adres1 { get; set; }

        public string Param_Adres2 { get; set; }

        public string Param_Adres3 { get; set; }

        public string Param_Adres4 { get; set; }

        public string Param_Adres5 { get; set; }

        public string Param_Fis_Aciklama { get; set; }

        public string Param_Masa_Size { get; set; }

        public bool? Param_Sonmasa { get; set; }

        public string Param_Sonmasa_Renk { get; set; }

        public bool? Param_Paket_Form { get; set; }

        public bool? Param_Paket_Kisi { get; set; }

        public bool? Param_Hesap_DovizOzet { get; set; }

        public bool? Param_Hesap_DovizOzetToplam { get; set; }

        public string Param_FrontPath { get; set; }

        public string Param_SikKullanSize { get; set; }

        public string Param_ComPort { get; set; }

        public int? Param_BaudRate { get; set; }

        public int? Param_DataBits { get; set; }

        public string Param_Parity { get; set; }

        public decimal? Param_StopBits { get; set; }

        public string Param_FlowControl { get; set; }

        public int? Param_bSizeW { get; set; }

        public int? Param_bSizeH { get; set; }

        public bool? Param_SatisArama { get; set; }

        public bool? Param_HesapSor { get; set; }

        public bool? Param_CariSor { get; set; }

        public bool? Param_CallerID { get; set; }

        public bool? Param_SiparisSayi { get; set; }

        public bool? Param_AutoUpdate { get; set; }

        public DateTime? Param_UpdateTarih { get; set; }

        public bool? Param_PaketSiparisPayi { get; set; }

        public bool? Param_SubeGonder { get; set; }

        public bool? Param_LimitFolio { get; set; }

        public bool? Param_DirekLimitUyari { get; set; }

        public bool? Param_OzelMasaSiralama { get; set; }

        public bool? Param_HesapFisiDokum { get; set; }

        public bool? Param_HspFontAlgilama { get; set; }

        public bool? Param_AdisyonFolioAdi { get; set; }

        public bool? Param_FullPos { get; set; }

        public bool? Param_CikisKapa { get; set; }

        public bool? Param_DirekAdisyonZor { get; set; }

        public bool? Param_DirekAdisyonPrSor { get; set; }

        public bool? Param_KGAlgilama { get; set; }

        public bool? Param_HesapRenkDegis { get; set; }

        public bool? Param_ExtraFolioAcma { get; set; }

        public bool? Param_SiparisAna { get; set; }

        public bool? Param_iadeKontrol { get; set; }

        public decimal? Param_iadeLimit { get; set; }

        public bool? Pos_HesapDkmRenk { get; set; }

        public bool? Param_AdisyonDegis { get; set; }

        public bool? Param_AdisyonIndAd { get; set; }

        public bool? Param_SiparisTutar { get; set; }

        public bool? Param_AnaEkranCiro { get; set; }

        public bool? Param_MasaTakipCiro { get; set; }

        public bool? Param_AcilisCekSil { get; set; }

        public bool? Param_CariAdSoyad { get; set; }

        public bool? Param_OdenmezAc { get; set; }

        public bool? Param_SatirSil { get; set; }

        public string Param_SatirSilUser { get; set; }

        public bool? Param_MasaTakipMenu { get; set; }

        public bool? Param_ParaUstuIngenico { get; set; }

        public int? Param_SatisTabloID { get; set; }

        public bool? Param_SatisTabloAktif { get; set; }

        public int? Param_SatisTabloGonderi { get; set; }

        public bool? Param_AcilistaMenu { get; set; }

        public bool? Param_IngenicoSPR { get; set; }

        public bool? Param_SiparisFisFont { get; set; }

        public bool? Param_HizliSatisCekAc { get; set; }

        public bool? Param_KartfGBCheckOut { get; set; }

        public bool? Param_UrunPrint { get; set; }

        public bool? Param_YeniHesapDkm { get; set; }

        public bool? Param_YeniSiparisDkm { get; set; }

        public bool? Param_OdaKrediCompOdenmez { get; set; }

        public bool? Param_KurTransfer { get; set; }
    }
}
