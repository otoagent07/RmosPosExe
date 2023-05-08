using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Pos.Class
{
    public class Param
    {
        public static bool Ent_Onb { get; set; }
        public static bool Ent_Cost { get; set; }
        public static bool Ent_Muh { get; set; }
        public static string Tesis_Adi { get; set; }
        public static int Tesis_Tipi { get; set; } // 1 ise pos dur -> 0 ise önbüro
        public static int Calisma_Sekli { get; set; } // 1 ise dövizlidir
        public static int Doviz_Cinsi { get; set; }
        public static int Kurlar_Nerden { get; set; }
        public static int Limit_Tipi { get; set; }
        public static int Fiste_Balance { get; set; }
        public static int Fis_Dovizli { get; set; }
        public static int Tarih_Nerden { get; set; }
        public static DateTime Tarih { get; set; }
        public static string Doviz_Kodu { get; set; }
        public static string Doviz_Adi { get; set; }
        public static string Doviz_Adi1 { get; set; }
        public static string Doviz_Turu { get; set; }
        
        public static decimal Doviz_Kuru { get; set; }
        public static int Masa_Refresh { get; set; }
        public static int Max_Miktar { get; set; }
        public static bool Yansit { get; set; }
        public static string Discount { get; set; }
        public static bool Tek_Dep { get; set; }
        public static string Tek_Dep_Kodu { get; set; }
        public static bool Kul_Kart { get; set; }
        public static bool Miktar_Duzelt { get; set; }
        public static bool Satis_YarimTam { get; set; }
        public static bool Fis_Urungrup { get; set; }
        public static string Indirim_Kodu { get; set; }
        public static string Fullcomp_Kodu { get; set; }
        public static int Barkod_Recbas { get; set; }
        public static int Barkod_Rechane { get; set; }
        public static int Barkod_KGbas { get; set; }
        public static int Barkod_KGhane { get; set; }
        public static int Barkod_GRbas { get; set; }
        public static int Barkod_GRhane { get; set; }
        public static int Pda_Height { get; set; }
        public static int Pda_Width { get; set; }
        public static bool RaporMasa_Geri { get; set; }
        public static string Param_Rapor_Design { get; set; }
        public static bool Param_Pda_Kartsor { get; set; }
        public static bool Param_Anagrup_Cikmasin { get; set; }
        public static bool Param_Satis_YD { get; set; }
        public static bool Param_Paket_YD { get; set; }
        public static bool Param_Dep_Fiyat { get; set; }
        public static bool Param_Masatr_Uyari { get; set; }
        public static string Param_Bindirim { get; set; }
        public static bool Param_Pda_Fullscreen { get; set; }
        public static bool Param_Printer_Tanim { get; set; }
        public static bool Param_Hesap_Disable { get; set; }
        public static string Param_Yuvarla { get; set; }
        public static int Param_Yuv_Sayi { get; set; }
        public static bool Param_Hsifir_Ikram { get; set; }
        public static bool Param_Adispr_Uyari { get; set; }
        public static bool Param_Paketci_Sor { get; set; }
        public static bool Param_Gunsonu_Aktar { get; set; }
        public static bool P_Sabitkonum { get; set; }
        public static string P_Sabitkonumkodu { get; set; }
        public static bool Param_Masa_Garson { get; set; }
        public static bool Param_Extre_Cikmasin { get; set; }
        public static string Param_Kart_Yoksay { get; set; }
        public static bool Param_Adis_Doviz { get; set; }
        public static bool Param_Tum_Paket { get; set; }
        public static bool Param_Siparis_Uyari { get; set; }
        public static bool Param_Hesap_Kilit { get; set; }
        public static bool Param_Masaacan_Garson { get; set; }
        public static string Param_Adres1 { get; set; }
        public static string Param_Adres2 { get; set; }
        public static string Param_Adres3 { get; set; }
        public static string Param_Adres4 { get; set; }
        public static string Param_Adres5 { get; set; }
        public static string Param_Fis_Aciklama { get; set; }
        public static string Param_Masa_Size { get; set; }
        public static bool Param_Sonmasa { get; set; }
        public static Color Param_Sonmasa_Renk { get; set; }
        public static bool Param_Paket_Form { get; set; }
        public static bool Param_Paket_Kisi { get; set; }
        public static bool Param_Hesap_DovizOzet { get; set; }
        public static bool Param_Hesap_DovizOzetToplam { get; set; }
        public static string Param_FrontPath { get; set; }
        public static string Param_SikKullanSize { get; set; }
        public static string Param_ComPort { get; set; }
        public static int Param_BaudRate { get; set; }
        public static int Param_DataBits { get; set; }
        public static string Param_Parity { get; set; }
        public static decimal Param_StopBits { get; set; }
        public static string Param_FlowControl { get; set; }
        public static int Param_bSizeW { get; set; }
        public static int Param_bSizeH { get; set; }
        public static bool Param_SatisArama { get; set; }
        public static bool Param_HesapSor { get; set; }
        public static bool Param_SiparisSayi { get; set; }
        public static bool Param_CallerID { get; set; }
        public static bool Param_CariSor { get; set; }
        public static bool Param_AutoUpdate { get; set; }
        public static bool Param_PaketSiparisPayi { get; set; }
        public static bool Param_SubeGonder { get; set; }
        public static bool Param_LimitFolio { get; set; }
        public static bool Param_DirekLimitUyari { get; set; }
        public static bool Param_OzelMasaSiralama { get; set; }
        public static bool Param_HesapFisiDokum { get; set; }
        public static bool Param_HspFontAlgilama { get; set; }
        public static bool Param_AdisyonFolioAdi { get; set; }
        public static bool Param_FullPos { get; set; }
        public static bool Param_CikisKapa { get; set; }
        public static bool Param_DirekAdisyonZor { get; set; }
        public static bool Param_DirekAdisyonPrSor { get; set; }
        public static bool Param_HesapRenkDegis { get; set; }
        public static bool Param_KGAlgilama { get; set; }
        public static bool Param_ExtraFolioAcma { get; set; }
        public static bool Param_SiparisAna { get; set; }
        public static bool Param_iadeKontrol { get; set; }
        public static decimal Param_iadeLimit { get; set; }
        public static bool Pos_HesapDkmRenk { get; set; }
        public static bool Param_AdisyonDegis { get; set; }
        public static bool Param_AdisyonIndAd { get; set; }
        public static bool Param_SiparisTutar { get; set; }
        public static bool Param_AnaEkranCiro { get; set; }
        public static bool Param_MasaTakipCiro { get; set; }
        public static bool Param_AcilisCekSil { get; set; }
        public static bool Param_CariAdSoyad { get; set; }
        public static bool Param_OdenmezAc { get; set; }
        public static bool Param_SatirSil { get; set; }
        public static string Param_SatirSilUser { get; set; }
        public static bool Param_MasaTakipMenu { get; set; }
        public static bool Param_ParaUstuIngenico { get; set; }
        public static int Param_SatisTabloID { get; set; }
        public static int Param_SatisTabloGonderi { get; set; }
        public static bool Param_AcilistaMenu { get; set; }
        public static bool Param_SatisTabloAktif { get; set; }

        public static bool Param_HizliSatisCekAc { get; set; }
        public static bool Param_IngenicoSPR { get; set; }

        public static bool Param_SiparisFisFont { get; set; }
        public static bool Param_KartfGBCheckOut { get; set; }
        public static bool Param_YeniHesapDkm { get; set; }
        public static bool Param_YeniSiparisDkm { get; set; }

        public static bool Param_OdaKrediCompOdenmez { get; set; }
        public static bool Param_KurTransfer { get; set; }
        public static bool Param_CallCenterPaket { get; set; }
        public static bool Param_PaketDipTotal { get; set; }
        public static bool Param_HesapKapamaAds { get; set; }
        public static bool Param_HesapDkmAciklama { get; set; }
        public static bool Param_AndroGeriYazdir { get; set; }
        public static bool Param_PaketKucukEkran { get; set; }

        public static Color Param_OzelMasaRengi { get; set; }
        public static Color Param_RezMasaRengi { get; set; }
        public static bool Param_GetirTest { get; set; }
        public static bool Param_GetirOtomatikOnay { get; set; }
        public static bool Param_SatisCikisButton { get; set; }
        public static bool Param_nfcBarkodAktif { get; set; }
        public static bool Param_ParcaliMasaAktif { get; set; }
        public static bool yazdirilmamissiparis { get; set; }
        public static bool masamusait { get; set; }
        public static bool masatakiphesappasif { get; set; }
        public static bool kisivegarsonbirkeresoraktif { get; set; }
        public static bool satirsilfiscikmasinaktif { get; set; }
        public static bool onburoikramsifiryazaktif { get; set; }


        public static void Param_Yukle()
        {
            try
            {

                DataTable dt = dbtools.SelectTable("select isnull(Param_Onburo,0) as Param_Onburo2,isnull(Param_Cost,0) as Param_Cost2,isnull(Param_Muh,0) as Param_Muh2, "
                        + " isnull(Param_Tar_Nere,0) as Param_Tar_Nere2,isnull(Param_Kur_Nere,0) as Param_Kur_Nere2,isnull(Param_Limittip,0) as Param_Limittip2, "
                        + " isnull(Param_Balance,0) as Param_Balance2,isnull(Param_Fis_Dovizli,0) as Param_Fis_Dovizli2,isnull(Param_Tesistip,0) as Param_Tesistip2, "
                        + " isnull(Param_Tarih,getdate()) as Param_Tarih2,Param_Dovizkod,isnull(Param_Kur,0) as Param_Kur2,isnull(Param_Yansit,0) as Param_Yansit2, "
                        + " Param_Discount,isnull(Param_Kartla_Giris,0) as Param_Kartla_Giris2,isnull(Param_CalismaSekli,0) as Param_CalismaSekli2,isnull(Param_Merkez_Ex,0) as Param_Merkez_Ex2, "
                        + " Param_TesisAdi,isnull(Param_Miktar,9) as Param_Miktar2,Param_Doviz_Turu,isnull(Param_Miktar_Duzelt,0) as Param_Miktar_Duzelt2, "
                        + " isnull(Param_Masa_Refresh,0) as Param_Masa_Refresh2,isnull(Param_Yarim_Tam,0) as Param_Yarim_Tam2,isnull(Param_Fis_Urungrup,0) as Param_Fis_Urungrup2, "
                        + " Param_FullComp,isnull(Param_Rec_Bas,0) as Param_Rec_Bas,isnull(Param_Rec_Hane,0) as Param_Rec_Hane, "
                        + " isnull(Param_KG_Bas,0) as Param_KG_Bas, isnull(Param_KG_Hane,0) as Param_KG_Hane,isnull(Param_GR_Bas,0) as Param_GR_Bas,isnull(Param_GR_Hane,0) as Param_GR_Hane, "
                        + " isnull(Param_Pda_Height,300) as Param_Pda_Height,isnull(Param_Pda_Width,250) as Param_Pda_Width, "
                        + " isnull(Param_Masa_Geri,0) as Param_Masa_Geri2,Param_Rapor_Design,isnull(Param_Pda_Kartsor,0) as Param_Pda_Kartsor2,isnull(Param_Anagrup_Cikmasin,0) as Param_Anagrup_Cikmasin2, "
                        + " isnull(Param_Satis_YD,0) as Param_Satis_YD2, isnull(Param_Paket_YD ,0) as Param_Paket_YD2,isnull(Param_Dep_Fiyat,0) as Param_Dep_Fiyat2, "
                        + " isnull(Param_Masatr_Uyari,0) as Param_Masatr_Uyari2,Param_Bindirim,isnull(Param_Pda_Fullscreen,0) as Param_Pda_Fullscreen2,isnull(Param_Printer_Tanim,0) as Param_Printer_Tanim2, "
                        + " isnull(Param_Hesap_Disable,0) as Param_Hesap_Disable2,Param_Yuvarla,isnull(Param_Yuv_Sayi,0) as Param_Yuv_Sayi2,isnull(Param_Hsifir_Ikram,0) as Param_Hsifir_Ikram2, "
                        + " isnull(Param_Adispr_Uyari,0) as Param_Adispr_Uyari2,isnull(Param_Paketci_Sor,0) as Param_Paketci_Sor2,isnull(Param_Gunsonu_Aktar,0) as Param_Gunsonu_Aktar2, "
                        + " isnull(Param_Masa_Garson,0) as Param_Masa_Garson2,isnull(Param_Extre_Cikmasin,0) as Param_Extre_Cikmasin2,Param_Kart_Yoksay,isnull(Param_Adis_Doviz,0) as Param_Adis_Doviz2, "
                        + " isnull(Param_Tum_Paket,0) as Param_Tum_Paket2,isnull(Param_Siparis_Uyari,0) as Param_Siparis_Uyari2,ISNULL(Param_Hesap_Kilit,0) as Param_Hesap_Kilit2,isnull(Param_Masaacan_Garson,0) as Param_Masaacan_Garson2, "
                        + " Param_Adres1,Param_Adres2,Param_Adres3,Param_Adres4,Param_Adres5,Param_Fis_Aciklama,ISNULL(NULLIF(Param_Masa_Size,''),'90;45') as Param_Masa_Size, "
                        + " ISNULL(Param_Sonmasa,0) as Param_Sonmasa,ISNULL(Param_Sonmasa_Renk,'#FF4500') as Param_Sonmasa_Renk,ISNULL(Param_Paket_Form,0) as Param_Paket_Form, "
                        + " ISNULL(Param_Paket_Kisi,0) as Param_Paket_Kisi,ISNULL(Param_Hesap_DovizOzet,0) as Param_Hesap_DovizOzet,ISNULL(Param_Hesap_DovizOzetToplam,0) as Param_Hesap_DovizOzetToplam,Param_FrontPath, "
                        + " ISNULL(NULLIF(Param_SikKullanSize,''),'90;45') as Param_SikKullanSize, ISNULL(Param_ComPort,'COM1') as Param_ComPort, ISNULL(Param_BaudRate,9600) as Param_BaudRate, "
                        + " ISNULL(Param_DataBits,8) as Param_DataBits, ISNULL(Param_Parity,'None') as Param_Parity, ISNULL(Param_StopBits,1) as Param_StopBits, ISNULL(Param_FlowControl,'None') as Param_FlowControl, "
                        + " ISNULL(Param_bSizeW,50) as Param_bSizeW, ISNULL(Param_bSizeH,30) as Param_bSizeH, ISNULL(Param_SatisArama,0) as Param_SatisArama, ISNULL(Param_HesapSor,0) as Param_HesapSor, "
                        + " ISNULL(Param_CariSor,0) as Param_CariSor, ISNULL(Param_AutoUpdate,0) as Param_AutoUpdate, ISNULL(Param_CallerID,0) as Param_CallerID, ISNULL(Param_SiparisSayi,0) as Param_SiparisSayi, "
                        + " ISNULL(Param_PaketSiparisPayi,0) as Param_PaketSiparisPayi, ISNULL(Param_SubeGonder,0) as Param_SubeGonder, ISNULL(Param_LimitFolio,0) as Param_LimitFolio, "
                        + " ISNULL(Param_DirekLimitUyari,0) as Param_DirekLimitUyari, ISNULL(Param_OzelMasaSiralama,0) as Param_OzelMasaSiralama, ISNULL(Param_HesapFisiDokum,0) as Param_HesapFisiDokum, "
                        + " ISNULL(Param_HspFontAlgilama,0) as Param_HspFontAlgilama, ISNULL(Param_AdisyonFolioAdi,0) as Param_AdisyonFolioAdi, ISNULL(Param_FullPos,0) as Param_FullPos, ISNULL(Param_CikisKapa,0) as Param_CikisKapa, "
                        + " ISNULL(Param_DirekAdisyonZor,0) as Param_DirekAdisyonZor, ISNULL(Param_DirekAdisyonPrSor,0) as Param_DirekAdisyonPrSor, ISNULL(Param_KGAlgilama,0) as Param_KGAlgilama, "
                        + " ISNULL(Param_ExtraFolioAcma,0) as Param_ExtraFolioAcma, ISNULL(Param_SiparisAna,0) as Param_SiparisAna, ISNULL(Param_iadeKontrol,0) as Param_iadeKontrol, ISNULL(Param_iadeLimit,0) as Param_iadeLimit, "
                        + " ISNULL(Pos_HesapDkmRenk,0) as Pos_HesapDkmRenk, ISNULL(Param_AdisyonDegis,0) as Param_AdisyonDegis, ISNULL(Param_AdisyonIndAd,0) as Param_AdisyonIndAd , ISNULL(Param_SiparisTutar,0) as Param_SiparisTutar, "
                        + " ISNULL(Param_MasaTakipCiro,0) as Param_MasaTakipCiro, ISNULL(Param_AnaEkranCiro,0) as Param_AnaEkranCiro, ISNULL(Param_AcilisCekSil,0) as Param_AcilisCekSil, ISNULL(Param_CariAdSoyad,0) as Param_CariAdSoyad, "
                        + " ISNULL(Param_OdenmezAc,0) as Param_OdenmezAc, ISNULL(Param_SatirSil,0) as Param_SatirSil, ISNULL(Param_SatirSilUser,'') as Param_SatirSilUser, ISNULL(Param_MasaTakipMenu,0) as Param_MasaTakipMenu, ISNULL(Param_ParaUstuIngenico,0) as Param_ParaUstuIngenico, ISNULL(Param_SatisTabloID,0) as Param_SatisTabloID,  "
                        + " ISNULL(Param_SatisTabloGonderi,0) as Param_SatisTabloGonderi, ISNULL(Param_AcilistaMenu,0) as Param_AcilistaMenu, ISNULL(Param_SatisTabloAktif,0) as Param_SatisTabloAktif, ISNULL(Param_IngenicoSPR,0) as Param_IngenicoSPR , ISNULL(Param_SiparisFisFont,0) as Param_SiparisFisFont, "
                        + " ISNULL(Param_HizliSatisCekAc,0) as Param_HizliSatisCekAc,ISNULL(Param_KartfGBCheckOut,0) as Param_KartfGBCheckOut, ISNULL(Param_YeniHesapDkm,0) as Param_YeniHesapDkm, "
                        + " ISNULL(Param_YeniSiparisDkm,0) as Param_YeniSiparisDkm, ISNULL(Param_OdaKrediCompOdenmez,0) as Param_OdaKrediCompOdenmez,ISNULL(Param_KurTransfer,0) as Param_KurTransfer , "
                        + " ISNULL(Param_CallCenterPaket,0) as Param_CallCenterPaket, ISNULL(Param_PaketDipTotal,0) as Param_PaketDipTotal,ISNULL(Param_HesapKapamaAds,0) as Param_HesapKapamaAds,  "
                        + " ISNULL(Param_HesapDkmAciklama,0) as Param_HesapDkmAciklama,ISNULL(Param_AndroGeriYazdir,0) as Param_AndroGeriYazdir,ISNULL(Param_PaketKucukEkran,0) as Param_PaketKucukEkran,ISNULL(Param_OzelMasaRengi,'DarkOrange') as Param_OzelMasaRengi,ISNULL(Param_RezMasaRengi,'LavenderBlush') as Param_RezMasaRengi,ISNULL(Param_GetirTest,0) as Param_GetirTest,ISNULL(Param_GetirOtomatikOnay,0) as Param_GetirOtomatikOnay,ISNULL(Param_SatisCikisButton,0) as Param_SatisCikisButton" +
                        ",ISNULL(Param_nfcBarkodAktif,0) as Param_nfcBarkodAktif  " +
                        ",ISNULL(Param_ParcaliMasaAktif,0) as Param_ParcaliMasaAktif  " +
                        ",ISNULL(yazdirilmamissiparis,0) as yazdirilmamissiparis  " +
                        ",ISNULL(masamusait,0) as masamusait  " +
                        ",ISNULL(masatakiphesappasif,0) as masatakiphesappasif  " +
                        ",ISNULL(kisivegarsonbirkeresoraktif,0) as kisivegarsonbirkeresoraktif  " +
                        ",ISNULL(satirsilfiscikmasinaktif,0) as satirsilfiscikmasinaktif  " +
                        ",ISNULL(onburoikramsifiryazaktif,0) as onburoikramsifiryazaktif  " +
                         " from Pos_Param where Param_Id = '1' ");

                DataTable dtMac = dbtools.SelectTable("SELECT  isnull(P_Tek,0) as P_Tek, P_Mac, P_Dep, ISNULL(P_Sabitkonum,0) as P_Sabitkonum, P_Sabitkonumkodu   FROM  Rmosmuh.dbo.P_Bilg WHERE P_Mac='" + dbtools.MacAdresi() + "'");

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("HATA Pos_Param da Column Eksik olabilir !");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    Ent_Onb = Convert.ToBoolean(dt.Rows[0]["Param_Onburo2"]);
                    Ent_Cost = Convert.ToBoolean(dt.Rows[0]["Param_Cost2"]);
                    Ent_Muh = Convert.ToBoolean(dt.Rows[0]["Param_Muh2"]);
                    Tarih_Nerden = Convert.ToInt32(dt.Rows[0]["Param_Tar_Nere2"]);
                    Kurlar_Nerden = Convert.ToInt32(dt.Rows[0]["Param_Kur_Nere2"]);
                    Limit_Tipi = Convert.ToInt32(dt.Rows[0]["Param_Limittip2"]);
                    Fiste_Balance = Convert.ToInt32(dt.Rows[0]["Param_Balance2"]);
                    Fis_Dovizli = Convert.ToInt32(dt.Rows[0]["Param_Fis_Dovizli2"]);
                    Tesis_Tipi = Convert.ToInt32(dt.Rows[0]["Param_Tesistip2"]);
                    Tarih = Convert.ToDateTime(dt.Rows[0]["Param_Tarih2"]);
                    Yansit = Convert.ToBoolean(dt.Rows[0]["Param_Yansit2"]);
                    Discount = Convert.ToString(dt.Rows[0]["Param_Discount"]);
                    Kul_Kart = Convert.ToBoolean(dt.Rows[0]["Param_Kartla_Giris2"]);
                    Calisma_Sekli = Convert.ToInt32(dt.Rows[0]["Param_CalismaSekli2"]);
                    Tesis_Adi = Convert.ToString(dt.Rows[0]["Param_TesisAdi"]);
                    Max_Miktar = Convert.ToInt32(dt.Rows[0]["Param_Miktar2"]);
                    Miktar_Duzelt = Convert.ToBoolean(dt.Rows[0]["Param_Miktar_Duzelt2"]);
                    Masa_Refresh = Convert.ToInt32(dt.Rows[0]["Param_Masa_Refresh2"]);
                    Satis_YarimTam = Convert.ToBoolean(dt.Rows[0]["Param_Yarim_Tam2"]);
                    Fis_Urungrup = Convert.ToBoolean(dt.Rows[0]["Param_Fis_Urungrup2"]);
                    Fullcomp_Kodu = Convert.ToString(dt.Rows[0]["Param_FullComp"]);

                    Barkod_Recbas = Convert.ToInt32(dt.Rows[0]["Param_Rec_Bas"]);
                    Barkod_Rechane = Convert.ToInt32(dt.Rows[0]["Param_Rec_Hane"]);
                    Barkod_KGbas = Convert.ToInt32(dt.Rows[0]["Param_KG_Bas"]);
                    Barkod_KGhane = Convert.ToInt32(dt.Rows[0]["Param_KG_Hane"]);
                    Barkod_GRbas = Convert.ToInt32(dt.Rows[0]["Param_GR_Bas"]);
                    Barkod_GRhane = Convert.ToInt32(dt.Rows[0]["Param_GR_Hane"]);

                    Pda_Height = Convert.ToInt32(dt.Rows[0]["Param_Pda_Height"]);
                    Pda_Width = Convert.ToInt32(dt.Rows[0]["Param_Pda_Width"]);

                    RaporMasa_Geri = Convert.ToBoolean(dt.Rows[0]["Param_Masa_Geri2"]);
                    Param_Rapor_Design = Convert.ToString(dt.Rows[0]["Param_Rapor_Design"]);
                    Param_Pda_Kartsor = Convert.ToBoolean(dt.Rows[0]["Param_Pda_Kartsor2"]);
                    Param_Anagrup_Cikmasin = Convert.ToBoolean(dt.Rows[0]["Param_Anagrup_Cikmasin2"]);

                    Param_Satis_YD = Convert.ToBoolean(dt.Rows[0]["Param_Satis_YD2"]);
                    Param_Paket_YD = Convert.ToBoolean(dt.Rows[0]["Param_Paket_YD2"]);
                    Param_Dep_Fiyat = Convert.ToBoolean(dt.Rows[0]["Param_Dep_Fiyat2"]);
                    Param_Masatr_Uyari = Convert.ToBoolean(dt.Rows[0]["Param_Masatr_Uyari2"]);

                    Param_Bindirim = Convert.ToString(dt.Rows[0]["Param_Bindirim"]);

                    Param_Pda_Fullscreen = Convert.ToBoolean(dt.Rows[0]["Param_Pda_Fullscreen2"]);

                    Param_Printer_Tanim = Convert.ToBoolean(dt.Rows[0]["Param_Printer_Tanim2"]);
                    Param_Hesap_Disable = Convert.ToBoolean(dt.Rows[0]["Param_Hesap_Disable2"]);

                    Param_Yuvarla = Convert.ToString(dt.Rows[0]["Param_Yuvarla"]);
                    Param_Yuv_Sayi = Convert.ToInt32(dt.Rows[0]["Param_Yuv_Sayi2"]);
                    Param_Hsifir_Ikram = Convert.ToBoolean(dt.Rows[0]["Param_Hsifir_Ikram2"]);
                    Param_Adispr_Uyari = Convert.ToBoolean(dt.Rows[0]["Param_Adispr_Uyari2"]);
                    Param_Paketci_Sor = Convert.ToBoolean(dt.Rows[0]["Param_Paketci_Sor2"]);
                    Param_Gunsonu_Aktar = Convert.ToBoolean(dt.Rows[0]["Param_Gunsonu_Aktar2"]);
                    Param_Masa_Garson = Convert.ToBoolean(dt.Rows[0]["Param_Masa_Garson2"]);
                    Param_Extre_Cikmasin = Convert.ToBoolean(dt.Rows[0]["Param_Extre_Cikmasin2"]);

                    Param_Kart_Yoksay = Convert.ToString(dt.Rows[0]["Param_Kart_Yoksay"]);
                    Param_Adis_Doviz = Convert.ToBoolean(dt.Rows[0]["Param_Adis_Doviz2"]);
                    Param_Tum_Paket = Convert.ToBoolean(dt.Rows[0]["Param_Tum_Paket2"]);
                    Param_Siparis_Uyari = Convert.ToBoolean(dt.Rows[0]["Param_Siparis_Uyari2"]);
                    Param_Hesap_Kilit = Convert.ToBoolean(dt.Rows[0]["Param_Hesap_Kilit2"]);
                    Param_Masaacan_Garson = Convert.ToBoolean(dt.Rows[0]["Param_Masaacan_Garson2"]);

                    Param_Adres1 = Convert.ToString(dt.Rows[0]["Param_Adres1"]);
                    Param_Adres2 = Convert.ToString(dt.Rows[0]["Param_Adres2"]);
                    Param_Adres3 = Convert.ToString(dt.Rows[0]["Param_Adres3"]);
                    Param_Adres4 = Convert.ToString(dt.Rows[0]["Param_Adres4"]);
                    Param_Adres5 = Convert.ToString(dt.Rows[0]["Param_Adres5"]);

                    Param_Fis_Aciklama = Convert.ToString(dt.Rows[0]["Param_Fis_Aciklama"]);
                    Param_Masa_Size = Convert.ToString(dt.Rows[0]["Param_Masa_Size"]);

                    Param_Sonmasa = Convert.ToBoolean(dt.Rows[0]["Param_Sonmasa"]);
                    Param_Sonmasa_Renk = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dt.Rows[0]["Param_Sonmasa_Renk"]));
                    Param_Paket_Form = Convert.ToBoolean(dt.Rows[0]["Param_Paket_Form"]);
                    Param_Paket_Kisi = Convert.ToBoolean(dt.Rows[0]["Param_Paket_Kisi"]);

                    Param_Hesap_DovizOzet = Convert.ToBoolean(dt.Rows[0]["Param_Hesap_DovizOzet"]);
                    Param_Hesap_DovizOzetToplam = Convert.ToBoolean(dt.Rows[0]["Param_Hesap_DovizOzetToplam"]);
                    Param_FrontPath = Convert.ToString(dt.Rows[0]["Param_FrontPath"]);
                    Param_SikKullanSize = Convert.ToString(dt.Rows[0]["Param_SikKullanSize"]);

                    Param_ComPort = Convert.ToString(dt.Rows[0]["Param_ComPort"]);
                    Param_BaudRate = Convert.ToInt32(dt.Rows[0]["Param_BaudRate"]);
                    Param_DataBits = Convert.ToInt32(dt.Rows[0]["Param_DataBits"]);
                    Param_Parity = Convert.ToString(dt.Rows[0]["Param_Parity"]);
                    Param_StopBits = Convert.ToDecimal(dt.Rows[0]["Param_StopBits"]);
                    Param_FlowControl = Convert.ToString(dt.Rows[0]["Param_FlowControl"]);

                    Param_bSizeH = Convert.ToInt32(dt.Rows[0]["Param_bSizeH"]);
                    Param_bSizeW = Convert.ToInt32(dt.Rows[0]["Param_bSizeW"]);

                    Param_SatisArama = Convert.ToBoolean(dt.Rows[0]["Param_SatisArama"]);
                    Param_HesapSor = Convert.ToBoolean(dt.Rows[0]["Param_HesapSor"]);
                    Param_CariSor = Convert.ToBoolean(dt.Rows[0]["Param_CariSor"]);
                    Param_CallerID = Convert.ToBoolean(dt.Rows[0]["Param_CallerID"]);
                    Param_SiparisSayi = Convert.ToBoolean(dt.Rows[0]["Param_SiparisSayi"]);
                    Param_AutoUpdate = Convert.ToBoolean(dt.Rows[0]["Param_AutoUpdate"]);
                    Param_PaketSiparisPayi = Convert.ToBoolean(dt.Rows[0]["Param_PaketSiparisPayi"]);
                    Param_SubeGonder = Convert.ToBoolean(dt.Rows[0]["Param_SubeGonder"]);
                    Param_LimitFolio = Convert.ToBoolean(dt.Rows[0]["Param_LimitFolio"]);

                    Param_DirekLimitUyari = Convert.ToBoolean(dt.Rows[0]["Param_DirekLimitUyari"]);
                    Param_OzelMasaSiralama = Convert.ToBoolean(dt.Rows[0]["Param_OzelMasaSiralama"]);
                    Param_HesapFisiDokum = Convert.ToBoolean(dt.Rows[0]["Param_HesapFisiDokum"]);
                    Param_HspFontAlgilama = Convert.ToBoolean(dt.Rows[0]["Param_HspFontAlgilama"]);
                    Param_AdisyonFolioAdi = Convert.ToBoolean(dt.Rows[0]["Param_AdisyonFolioAdi"]);

                    Param_FullPos = Convert.ToBoolean(dt.Rows[0]["Param_FullPos"]);
                    Param_CikisKapa = Convert.ToBoolean(dt.Rows[0]["Param_CikisKapa"]);
                    Param_DirekAdisyonZor = Convert.ToBoolean(dt.Rows[0]["Param_DirekAdisyonZor"]);
                    Param_DirekAdisyonPrSor = Convert.ToBoolean(dt.Rows[0]["Param_DirekAdisyonPrSor"]);
                    Param_KGAlgilama = Convert.ToBoolean(dt.Rows[0]["Param_KGAlgilama"]);
                    Param_ExtraFolioAcma = Convert.ToBoolean(dt.Rows[0]["Param_ExtraFolioAcma"]);
                    Param_SiparisAna = Convert.ToBoolean(dt.Rows[0]["Param_SiparisAna"]);

                    Param_iadeKontrol = Convert.ToBoolean(dt.Rows[0]["Param_iadeKontrol"]);
                    Pos_HesapDkmRenk = Convert.ToBoolean(dt.Rows[0]["Pos_HesapDkmRenk"]);
                    Param_iadeLimit = Convert.ToDecimal(dt.Rows[0]["Param_iadeLimit"]);
                    Param_AdisyonDegis = Convert.ToBoolean(dt.Rows[0]["Param_AdisyonDegis"]);
                    Param_AdisyonIndAd = Convert.ToBoolean(dt.Rows[0]["Param_AdisyonIndAd"]);
                    Param_SiparisTutar = Convert.ToBoolean(dt.Rows[0]["Param_SiparisTutar"]);

                    Param_AnaEkranCiro = Convert.ToBoolean(dt.Rows[0]["Param_AnaEkranCiro"]);
                    Param_MasaTakipCiro = Convert.ToBoolean(dt.Rows[0]["Param_MasaTakipCiro"]);

                    Param_AcilisCekSil = Convert.ToBoolean(dt.Rows[0]["Param_AcilisCekSil"]);

                    Param_CariAdSoyad = Convert.ToBoolean(dt.Rows[0]["Param_CariAdSoyad"]);
                    Param_OdenmezAc = Convert.ToBoolean(dt.Rows[0]["Param_OdenmezAc"]);

                    Param_SatirSil = Convert.ToBoolean(dt.Rows[0]["Param_SatirSil"]);
                    Param_SatirSilUser = Convert.ToString(dt.Rows[0]["Param_SatirSilUser"]);
                    Param_MasaTakipMenu = Convert.ToBoolean(dt.Rows[0]["Param_MasaTakipMenu"]);

                    Param_ParaUstuIngenico = Convert.ToBoolean(dt.Rows[0]["Param_ParaUstuIngenico"]);

                    Param_SatisTabloID = Convert.ToInt32(dt.Rows[0]["Param_SatisTabloID"]);
                    Param_SatisTabloGonderi = Convert.ToInt32(dt.Rows[0]["Param_SatisTabloGonderi"]);
                    Param_AcilistaMenu = Convert.ToBoolean(dt.Rows[0]["Param_AcilistaMenu"]);
                    Param_SatisTabloAktif = Convert.ToBoolean(dt.Rows[0]["Param_SatisTabloAktif"]);
                    Param_IngenicoSPR = Convert.ToBoolean(dt.Rows[0]["Param_IngenicoSPR"]);
                    Param_SiparisFisFont = Convert.ToBoolean(dt.Rows[0]["Param_SiparisFisFont"]);

                    Param_HizliSatisCekAc = Convert.ToBoolean(dt.Rows[0]["Param_HizliSatisCekAc"]);

                    Param_YeniHesapDkm = Convert.ToBoolean(dt.Rows[0]["Param_YeniHesapDkm"]);

                    Param_YeniSiparisDkm = Convert.ToBoolean(dt.Rows[0]["Param_YeniSiparisDkm"]);

                    Param_OdaKrediCompOdenmez = Convert.ToBoolean(dt.Rows[0]["Param_OdaKrediCompOdenmez"]);

                    Param_KurTransfer = Convert.ToBoolean(dt.Rows[0]["Param_KurTransfer"]);

                    Param_CallCenterPaket = Convert.ToBoolean(dt.Rows[0]["Param_CallCenterPaket"]);

                    Param_PaketDipTotal = Convert.ToBoolean(dt.Rows[0]["Param_PaketDipTotal"]);

                    Param_HesapKapamaAds = Convert.ToBoolean(dt.Rows[0]["Param_HesapKapamaAds"]);

                    Param_HesapDkmAciklama = Convert.ToBoolean(dt.Rows[0]["Param_HesapDkmAciklama"]);
                    Param_AndroGeriYazdir = Convert.ToBoolean(dt.Rows[0]["Param_AndroGeriYazdir"]);
                    Param_PaketKucukEkran = Convert.ToBoolean(dt.Rows[0]["Param_PaketKucukEkran"]);


                    Param_OzelMasaRengi = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dt.Rows[0]["Param_OzelMasaRengi"]));

                    Param_RezMasaRengi = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dt.Rows[0]["Param_RezMasaRengi"]));

                    Param_GetirTest = Convert.ToBoolean(dt.Rows[0]["Param_GetirTest"]);
                    Param_GetirOtomatikOnay = Convert.ToBoolean(dt.Rows[0]["Param_GetirOtomatikOnay"]);
                    Param_SatisCikisButton = Convert.ToBoolean(dt.Rows[0]["Param_SatisCikisButton"]);
                    Param_nfcBarkodAktif = Convert.ToBoolean(dt.Rows[0]["Param_nfcBarkodAktif"]);
                    Param_ParcaliMasaAktif = Convert.ToBoolean(dt.Rows[0]["Param_ParcaliMasaAktif"]);
                    yazdirilmamissiparis = Convert.ToBoolean(dt.Rows[0]["yazdirilmamissiparis"]);
                    masamusait = Convert.ToBoolean(dt.Rows[0]["masamusait"]);
                    masatakiphesappasif = Convert.ToBoolean(dt.Rows[0]["masatakiphesappasif"]);
                    kisivegarsonbirkeresoraktif = Convert.ToBoolean(dt.Rows[0]["kisivegarsonbirkeresoraktif"]);
                    satirsilfiscikmasinaktif = Convert.ToBoolean(dt.Rows[0]["satirsilfiscikmasinaktif"]);
                    onburoikramsifiryazaktif = Convert.ToBoolean(dt.Rows[0]["onburoikramsifiryazaktif"]);

                }
                if (dtMac.Rows.Count > 0)
                {
                    Tek_Dep = Convert.ToBoolean(dtMac.Rows[0]["P_Tek"]);
                    Tek_Dep_Kodu = Convert.ToString(dtMac.Rows[0]["P_Dep"]);
                    P_Sabitkonum = Convert.ToBoolean(dtMac.Rows[0]["P_Sabitkonum"]);
                    P_Sabitkonumkodu = Convert.ToString(dtMac.Rows[0]["P_Sabitkonumkodu"]);
                }
                else
                {
                    Tek_Dep = false;
                }

                Indirim_Kodu = Convert.ToString(dbtools.DegerGetir("select isnull((select isnull(Pkod_Kod,'') from Pos_Kodlar with(nolock) where Pkod_Ozelkod = 4 and Pkod_Sinif = '11'),'')"));


                if (Tarih_Nerden == 0)
                {
                    DataTable f = Fronttools.SelectTable("SELECT * FROM Fishrk where Fis_Anah = 1");
                    if (f.Rows.Count > 0)
                    {
                        Tarih = Convert.ToDateTime(f.Rows[0]["Fis_Curtar"]);
                        dbtools.execcmd("update Pos_Param set Param_Tarih = '" + Tarih.Date + "' where Param_Id = 1 ");
                    }
                }



                DataTable dt2 = dbtools.SelectTable("select MKodlar_P_DovizKodu,isnull(MKodlar_P_DovizCins,0) as MKodlar_P_DovizCins,MKodlar_P_DovizTuru "
                                            + " from Muh_Kodlar "
                                            + " where Mkodlar_Sinif = '07' and Mkodlar_Kod = ( "
                                            + "            select top 1 Kodlar_Sirket "
                                            + "            from Stok_Kodlar "
                                            + "            where Kodlar_Sinif = '01' and Kodlar_Kod= '" + Departman.Dep_Kodu + "')");

                if (dt2.Rows.Count > 0)
                {
                    Doviz_Cinsi = Convert.ToInt32(dt2.Rows[0]["MKodlar_P_DovizCins"]);
                    Doviz_Kodu = Convert.ToString(dt2.Rows[0]["MKodlar_P_DovizKodu"]);
                    Doviz_Turu = Convert.ToString(dt2.Rows[0]["MKodlar_P_DovizTuru"]);

                    Doviz_Adi1 = dbtools.DegerGetir("select top 1 Mkodlar_Ad from Muh_Kodlar where Mkodlar_Sinif='02' and Mkodlar_Kod='" + Doviz_Kodu + "'").ToUpper();


                    if (Kurlar_Nerden == 0) // Döviz Ayarlarını Önbürodan Alması
                    {
                        Doviz_Adi = Fronttools.DovizAdi(Doviz_Kodu);
                        Doviz_Kuru = Fronttools.KurGetir(Tarih, Doviz_Kodu);
                    }
                    if (Kurlar_Nerden == 1)        //Döviz Ayarları Pos Parametresine Bağlı ise
                    {
                        Doviz_Kodu = Convert.ToString(dt.Rows[0]["Param_Dovizkod"]);
                        Doviz_Kuru = Convert.ToDecimal(dt.Rows[0]["Param_Kur2"]);
                        if (Doviz_Kuru == 0)
                        {
                            Doviz_Kuru = dbtools.KurGetir(Tarih, Doviz_Kodu);
                        }




                    }
                }
                else
                {

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Param.Param_Yukle() " + "\n" + dbtools.server + "\n" + dbtools.users + "\n" + dbtools.pwd + "\n" + dbtools.database + "\n" + "\ncondurum : " +dbtools.conn.State + "\nFront Con durum : " +Fronttools.conn.State+"\n"+ ex.Message);
            }

        }
    }
}
