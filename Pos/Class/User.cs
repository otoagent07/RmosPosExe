using System;
using System.Data;

namespace Pos.Class
{
    public class User
    {

        public static string P_Kod { get; set; }
        public static string P_Sifre { get; set; }
        public static string P_Ad { get; set; }
        public static string P_Soyad { get; set; }
        public static string P_Kart { get; set; }
        public static int P_Kulturu { get; set; }

        public static string U_BackUser { get; set; }

        public static bool G_Miktarduzelt { get; set; }
        public static bool G_Tutarduzelt { get; set; }
        public static bool G_Satirsil { get; set; }
        public static bool G_Indirim_Satis { get; set; }
        public static bool G_Hesapdokumu { get; set; }
        public static bool G_Odemeal { get; set; }
        public static bool G_Odemesil { get; set; }
        public static bool G_Indirim_Hesap { get; set; }
        public static bool G_Yazdirkapat { get; set; }
        public static bool G_Yazdirmadankapat { get; set; }
        public static bool G_Bindirim { get; set; }

        public static bool M_Masatakip { get; set; }
        public static bool M_Satis { get; set; }
        public static bool M_Masatransfer { get; set; }
        public static bool M_Malzemetransfer { get; set; }
        public static bool M_Ozelmasa { get; set; }
        public static bool M_Odakontrol { get; set; }
        public static bool M_Masakilitle { get; set; }
        public static bool M_Hesapkapatma { get; set; }
        public static bool M_SatisRelogin { get; set; }
        public static bool M_HesapTr { get; set; }

        public static bool D_Direksatis { get; set; }

        public static bool R_Raporlar { get; set; }
        public static bool R_Detay { get; set; }
        public static bool R_XZ { get; set; }
        public static bool R_Mahsupkes { get; set; }
        public static bool R_Fisiptal { get; set; }
        public static bool R_Fisiptalgecmis { get; set; }

        public static bool A_Ayarlar { get; set; }
        public static bool A_Parametre { get; set; }
        public static bool A_Print { get; set; }
        public static bool A_Odeme { get; set; }
        public static bool otoDirekSatis { get; set; }
        public static bool otoMasaEkraniAc { get; set; }
        public static bool satisYapma { get; set; }
        public static bool merkezsubeaktif { get; set; }
        public static bool A_Entegre { get; set; }
        public static bool A_Masa { get; set; }
        public static bool A_Cari { get; set; }
        public static bool A_HH { get; set; }
        public static bool A_Kullanici { get; set; }
        public static bool A_Kasa { get; set; }

        public static bool P_Gunsonu { get; set; }
        public static string P_Departman { get; set; }
        public static string postema { get; set; }


        public static bool Pda_Masatakip { get; set; }
        public static bool Pda_Satis { get; set; }
        public static bool Pda_Satirsil { get; set; }
        public static bool Pda_Miktarduzelt { get; set; }

        public static bool Pda_Hesap { get; set; }
        public static bool Pda_Masatr { get; set; }
        public static bool Pda_Malztr { get; set; }
        public static bool Pda_Ozelmasa { get; set; }
        public static bool Pda_Odakontrol { get; set; }

        public static bool Pda_Direksatis { get; set; }

        public static bool K_Kasa { get; set; }

        public static int P_Indirim_Yuzde { get; set; }
        public static int P_Bindirim_Yuzde { get; set; }
        public static string P_Sabit_Masa { get; set; }
        public static bool M_MasaAc { get; set; }
        public static bool M_BaskaMasa { get; set; }

        public static bool G_Satirsil_Y { get; set; }

        public static bool XZ_Odeme { get; set; }
        public static bool XZ_Servis { get; set; }
        public static bool XZ_Cari { get; set; }
        public static bool XZ_Odenmez { get; set; }
        public static bool XZ_Malzeme { get; set; }
        public static bool XZ_Anagrup { get; set; }
        public static bool XZ_Altgrup { get; set; }
        public static bool XZ_Iptal { get; set; }
        public static bool XZ_PaketServis { get; set; }
        public static bool XZ_IndirimMasa { get; set; }
        public static bool XZ_YiyecekIcecek { get; set; }
        public static bool XZ_MasaKonum { get; set; }
        public static bool XZ_GarsonOzet { get; set; }
        public static bool XZ_GarsonTahsil { get; set; }
        public static bool XZ_SifirTutar { get; set; }
        public static bool XZ_OzetKasa { get; set; }
        public static bool XZ_ExtKasaRapor { get; set; }
        public static bool XZ_ExtKasaDetay { get; set; }
        public static bool XZ_SiparisIptal { get; set; }
        public static bool XZ_GonderilmemisSiparisIptal { get; set; }
        public static bool XZ_SiparisDuzelt { get; set; }

        public static bool M_GarsonDegistir { get; set; }
        public static bool G_Zayi { get; set; }
        public static bool G_Ikram { get; set; }
        public static bool M_KisiSayisi { get; set; }
        public static bool R_MasaGeri { get; set; }
        public static bool M_SiparisTekrar { get; set; }
        public static bool Pda_HesapDok { get; set; }

        public static bool H_HizliSatis { get; set; }
        public static bool R_TopluIsle { get; set; }

        public static bool S_Sp_Sil { get; set; }
        public static bool urunIade { get; set; }

        public static bool ExtraFolio { get; set; }


        public static bool And_Yarim { get; set; }

        public static bool And_Tam { get; set; }
        public static bool And_Bucuk { get; set; }

        public static bool And_Duble { get; set; }

        public static bool Pos_SubeTrf { get; set; }

        public static bool Pos_OdemeDegistir { get; set; }

        public static bool Pos_ArtiEksi_Aktif { get; set; }
        public static bool Pos_MasaAnlikDurum { get; set; }

        public static bool Pos_MasaUrunSil { get; set; }
        public static bool Pos_IWERep { get; set; }
        public static bool Pos_KartF_CheckOut { get; set; }

        public static bool Pos_SatirSilYetkili { get; set; }

        public static bool Pos_MasaDirekS { get; set; }
        public static bool Pos_MasaPaketS { get; set; }
        public static bool Pos_YS_YetkiReddet { get; set; }

        public static bool Pos_YarimDubleAlan { get; set; }

        public static string Pos_Culture { get; set; }

        public static bool Pos_ReceteTanimlama { get; set; }

        public static bool Pos_AdisyonPr { get; set; }

        public static bool Pos_FixMenu { get; set; }

        public static bool Pos_HesapArti { get; set; }
        public static bool User_AP { get; set; }
        public static bool Pos_OdaKontrol { get; set; }
        public static bool Pos_KartfIndirimAktif { get; set; }
        public static bool Pos_ServisPayiDuzelt { get; set; }
        public static bool cariTahsilatlari { get; set; }
        public static bool servisPayiKdvOran { get; set; }
        public static bool coklugunsonu { get; set; }
        public static bool kapaliMasayaGir { get; set; }




        //public static bool MasaTakip_Paket { get; set; }


        static bool birkere = false;

        public static void Yetki_Yukle()
        {
            if (birkere == false)
            {
                try
                {
                    dbtools.execcmdRMesajsiz(StatikSinif.getAlterQuery());
                    dbtools.execcmdRMesajsiz(StatikSinif.getTriggerAcilisTar());
                    dbtools.execcmdR(StatikSinif.defaultParametreCalistir());
                    birkere = true;
                }
                catch (Exception ex)
                {
                    RHMesaj.alertMesaj("StatikSinif.getAlterQuery() " + ex.Message);
                }

            }
            DataTable dt = dbtools.SelectTable("select P_Kod  ,P_Sifre, P_Ad ,P_Soyad ,P_Kart ,isnull(P_Kulturu,-1) as P_Kulturu, "
                    + " ISNULL(G_Miktarduzelt,0) AS  G_Miktarduzelt,ISNULL(G_Tutarduzelt,0) AS G_Tutarduzelt,ISNULL(G_Satirsil,0) AS G_Satirsil, "
                    + " ISNULL(G_Indirim_Satis,0) AS G_Indirim_Satis,ISNULL(G_Hesapdokumu,0) AS G_Hesapdokumu,ISNULL(G_Odemeal,0) AS G_Odemeal , "
                    + " ISNULL(G_Odemesil,0) AS G_Odemesil ,ISNULL(G_Indirim_Hesap,0) AS G_Indirim_Hesap ,ISNULL(G_Yazdirkapat,0) AS G_Yazdirkapat , "
                    + " ISNULL(G_Yazdirmadankapat,0) AS G_Yazdirmadankapat ,ISNULL(G_Bindirim,0) as G_Bindirim, "
                    + " ISNULL(M_Masatakip,0) AS M_Masatakip ,ISNULL(M_Satis,0) AS M_Satis ,ISNULL(M_Masatransfer,0) AS M_Masatransfer ,ISNULL(M_Malzemetransfer,0) AS M_Malzemetransfer , "
                    + " ISNULL(M_Ozelmasa,0) AS M_Ozelmasa ,ISNULL(M_Odakontrol,0) as M_Odakontrol ,ISNULL(M_Masakilitle,0) AS M_Masakilitle,ISNULL(M_Hesapkapatma,0) AS M_Hesapkapatma , "
                    + " ISNULL(M_SatisRelogin,0) AS M_SatisRelogin,isnull(M_HesapTr,0) as M_HesapTr, "
                    + " ISNULL(D_Direksatis,0) AS D_Direksatis, "
                    + " ISNULL(R_Raporlar,0) AS R_Raporlar ,ISNULL(R_Detay,0) AS R_Detay ,ISNULL(R_XZ,0) AS R_XZ ,ISNULL(R_Mahsupkes,0) AS R_Mahsupkes ,ISNULL(R_Fisiptal,0) AS R_Fisiptal ,ISNULL(R_Fisiptalgecmis,0) as R_Fisiptalgecmis, "
                    + " ISNULL(A_Ayarlar,0) AS A_Ayarlar ,ISNULL(A_Parametre,0) AS A_Parametre ,ISNULL(A_Print,0) AS A_Print ,ISNULL(A_Odeme,0) AS A_Odeme ,ISNULL(A_Entegre,0) AS A_Entegre , "
                    + " ISNULL(A_Masa,0) AS A_Masa ,ISNULL(A_Cari,0) AS A_Cari ,ISNULL(A_HH,0) AS A_HH ,ISNULL(A_Kullanici,0) AS A_Kullanici ,ISNULL(A_Kasa,0) as A_Kasa, "
                    + " ISNULL(P_Gunsonu,0) AS P_Gunsonu, P_Departman,ISNULL(postema,'Money Twins') AS postema, "
                    + " ISNULL(Pda_Masatakip,0) as Pda_Masatakip,ISNULL(Pos_AdisyonPr,0) as Pos_AdisyonPr, "
                    + " ISNULL(Pda_Satis,0) as Pda_Satis, ISNULL(Pda_Satirsil,0) as Pda_Satirsil, ISNULL(Pda_Miktarduzelt,0) as Pda_Miktarduzelt, "
                    + " ISNULL(Pda_Hesap,0) as Pda_Hesap, ISNULL(Pda_Masatr,0) as Pda_Masatr, ISNULL(Pda_Malztr,0) as Pda_Malztr, ISNULL(Pda_Ozelmasa,0) as Pda_Ozelmasa, ISNULL(Pda_Odakontrol,0) as Pda_Odakontrol, "
                    + " ISNULL(Pda_Direksatis,0) as Pda_Direksatis, "
                    + " ISNULL(K_Kasa,0) as K_Kasa, ISNULL(P_Indirim_Yuzde,100) as P_Indirim_Yuzde,ISNULL(P_Bindirim_Yuzde,100) as P_Bindirim_Yuzde,P_Sabit_Masa,ISNULL(M_MasaAc,0) as M_MasaAc,ISNULL(M_BaskaMasa,0) as M_BaskaMasa, "
                    + " ISNULL(G_Satirsil_Y,0) as G_Satirsil_Y,ISNULL(M_GarsonDegistir,0) as M_GarsonDegistir,ISNULL(G_Zayi,0) as G_Zayi,ISNULL(G_Ikram,0) as G_Ikram,ISNULL(M_KisiSayisi,0) as M_KisiSayisi, "
                    + " ISNULL(R_MasaGeri,0) as R_MasaGeri,ISNULL(M_SiparisTekrar,0) as M_SiparisTekrar,ISNULL(Pda_HesapDok,0) as Pda_HesapDok,ISNULL(H_HizliSatis,0) as H_HizliSatis,ISNULL(R_TopluIsle,0) as R_TopluIsle, "
                    + " ISNULL(S_Sp_Sil,0) as S_Sp_Sil, Isnull(ExtraFolio,0) as ExtraFolio , Isnull(And_Yarim,0) as And_Yarim , Isnull(And_Tam,0) as And_Tam, "
                    + " Isnull(And_Bucuk,0) as And_Bucuk,Isnull(And_Duble,0) as And_Duble,Isnull(Pos_SubeTrf,0) as Pos_SubeTrf, ISNULL(Pos_OdemeDegistir,0) as Pos_OdemeDegistir, ISNULL(Pos_ArtiEksi_Aktif,0) as Pos_ArtiEksi_Aktif, ISNULL(Pos_MasaAnlikDurum,0) as Pos_MasaAnlikDurum , ISNULL(Pos_MasaUrunSil,0) as Pos_MasaUrunSil, ISNULL(Pos_IWERep,0) as Pos_IWERep, ISNULL(Pos_KartF_CheckOut,0) as Pos_KartF_CheckOut, ISNULL(Pos_SatirSilYetkili,0) as Pos_SatirSilYetkili, "
                    + " ISNULL(Pos_MasaPaketS,0) as Pos_MasaPaketS, ISNULL(Pos_MasaDirekS,0) as Pos_MasaDirekS,ISNULL(Pos_YS_YetkiReddet,0) as Pos_YS_YetkiReddet,ISNULL(Pos_YarimDubleAlan,1) as Pos_YarimDubleAlan, "
                    + " ISNULL(Pos_Culture,'tr-TR') as Pos_Culture, ISNULL(Pos_ReceteTanimlama,1) as Pos_ReceteTanimlama, ISNULL(Pos_FixMenu,0) as Pos_FixMenu, "
                    + " ISNULL(Pos_HesapArti,0) as Pos_HesapArti, ISNULL(User_AP,1) as User_AP, ISNULL(Pos_OdaKontrol,0) as Pos_OdaKontrol,U_BackUser "
                    + ", ISNULL(Pos_KartfIndirimAktif,0) as Pos_KartfIndirimAktif "
                    + ", ISNULL(Pos_ServisPayiDuzelt,0) as Pos_ServisPayiDuzelt "
                    + ", ISNULL(urunIade,0) as urunIade "
                    + ", ISNULL(otoDirekSatis,0) as otoDirekSatis "
                    + ", ISNULL(otoMasaEkraniAc,0) as otoMasaEkraniAc "
                    + ", ISNULL(merkezsubeaktif,0) as merkezsubeaktif "
                    + ", ISNULL(coklugunsonu,0) as coklugunsonu "
                    + ", ISNULL(kapaliMasayaGir,0) as kapaliMasayaGir "
                    + ", ISNULL(satisYapma,0) as satisYapma "

                    + " from Rmosmuh.dbo.Pos_User with(nolock) where P_Kod = '" + P_Kod + "'");

            if (dt.Rows.Count > 0)
            {
                P_Kulturu = Convert.ToInt32(dt.Rows[0]["P_Kulturu"]);

                G_Miktarduzelt = Convert.ToBoolean(dt.Rows[0]["G_Miktarduzelt"]);
                G_Tutarduzelt = Convert.ToBoolean(dt.Rows[0]["G_Tutarduzelt"]);
                G_Satirsil = Convert.ToBoolean(dt.Rows[0]["G_Satirsil"]);
                G_Indirim_Satis = Convert.ToBoolean(dt.Rows[0]["G_Indirim_Satis"]);
                G_Hesapdokumu = Convert.ToBoolean(dt.Rows[0]["G_Hesapdokumu"]);
                G_Odemeal = Convert.ToBoolean(dt.Rows[0]["G_Odemeal"]);
                G_Odemesil = Convert.ToBoolean(dt.Rows[0]["G_Odemesil"]);
                G_Indirim_Hesap = Convert.ToBoolean(dt.Rows[0]["G_Indirim_Hesap"]);
                G_Yazdirkapat = Convert.ToBoolean(dt.Rows[0]["G_Yazdirkapat"]);
                G_Yazdirmadankapat = Convert.ToBoolean(dt.Rows[0]["G_Yazdirmadankapat"]);
                G_Bindirim = Convert.ToBoolean(dt.Rows[0]["G_Bindirim"]);

                M_Masatakip = Convert.ToBoolean(dt.Rows[0]["M_Masatakip"]);
                M_Satis = Convert.ToBoolean(dt.Rows[0]["M_Satis"]);
                M_Masatransfer = Convert.ToBoolean(dt.Rows[0]["M_Masatransfer"]);
                M_Malzemetransfer = Convert.ToBoolean(dt.Rows[0]["M_Malzemetransfer"]);
                M_Ozelmasa = Convert.ToBoolean(dt.Rows[0]["M_Ozelmasa"]);
                M_Odakontrol = Convert.ToBoolean(dt.Rows[0]["M_Odakontrol"]);
                M_Masakilitle = Convert.ToBoolean(dt.Rows[0]["M_Masakilitle"]);
                M_Hesapkapatma = Convert.ToBoolean(dt.Rows[0]["M_Hesapkapatma"]);
                M_SatisRelogin = Convert.ToBoolean(dt.Rows[0]["M_SatisRelogin"]);
                M_HesapTr = Convert.ToBoolean(dt.Rows[0]["M_HesapTr"]);

                D_Direksatis = Convert.ToBoolean(dt.Rows[0]["D_Direksatis"]);

                R_Raporlar = Convert.ToBoolean(dt.Rows[0]["R_Raporlar"]);
                R_Detay = Convert.ToBoolean(dt.Rows[0]["R_Detay"]);
                R_XZ = Convert.ToBoolean(dt.Rows[0]["R_XZ"]);
                R_Mahsupkes = Convert.ToBoolean(dt.Rows[0]["R_Mahsupkes"]);
                R_Fisiptal = Convert.ToBoolean(dt.Rows[0]["R_Fisiptal"]);
                R_Fisiptalgecmis = Convert.ToBoolean(dt.Rows[0]["R_Fisiptalgecmis"]);

                A_Ayarlar = Convert.ToBoolean(dt.Rows[0]["A_Ayarlar"]);
                A_Parametre = Convert.ToBoolean(dt.Rows[0]["A_Parametre"]);
                A_Print = Convert.ToBoolean(dt.Rows[0]["A_Print"]);
                A_Odeme = Convert.ToBoolean(dt.Rows[0]["A_Odeme"]);
                A_Entegre = Convert.ToBoolean(dt.Rows[0]["A_Entegre"]);
                A_Masa = Convert.ToBoolean(dt.Rows[0]["A_Masa"]);
                A_Cari = Convert.ToBoolean(dt.Rows[0]["A_Cari"]);
                A_HH = Convert.ToBoolean(dt.Rows[0]["A_HH"]);
                A_Kullanici = Convert.ToBoolean(dt.Rows[0]["A_Kullanici"]);
                A_Kasa = Convert.ToBoolean(dt.Rows[0]["A_Kasa"]);

                P_Gunsonu = Convert.ToBoolean(dt.Rows[0]["P_Gunsonu"]);
                P_Departman = Convert.ToString(dt.Rows[0]["P_Departman"]);
                postema = Convert.ToString(dt.Rows[0]["postema"]);

                Pda_Masatakip = Convert.ToBoolean(dt.Rows[0]["Pda_Masatakip"]);

                Pda_Satis = Convert.ToBoolean(dt.Rows[0]["Pda_Satis"]);
                Pda_Satirsil = Convert.ToBoolean(dt.Rows[0]["Pda_Satirsil"]);
                Pda_Miktarduzelt = Convert.ToBoolean(dt.Rows[0]["Pda_Miktarduzelt"]);

                Pda_Hesap = Convert.ToBoolean(dt.Rows[0]["Pda_Hesap"]);
                Pda_Masatr = Convert.ToBoolean(dt.Rows[0]["Pda_Masatr"]);
                Pda_Malztr = Convert.ToBoolean(dt.Rows[0]["Pda_Malztr"]);
                Pda_Ozelmasa = Convert.ToBoolean(dt.Rows[0]["Pda_Ozelmasa"]);
                Pda_Odakontrol = Convert.ToBoolean(dt.Rows[0]["Pda_Odakontrol"]);

                Pda_Direksatis = Convert.ToBoolean(dt.Rows[0]["Pda_Direksatis"]);

                K_Kasa = Convert.ToBoolean(dt.Rows[0]["K_Kasa"]);

                P_Indirim_Yuzde = Convert.ToInt32(dt.Rows[0]["P_Indirim_Yuzde"]);
                P_Bindirim_Yuzde = Convert.ToInt32(dt.Rows[0]["P_Bindirim_Yuzde"]);
                P_Sabit_Masa = Convert.ToString(dt.Rows[0]["P_Sabit_Masa"]);
                M_MasaAc = Convert.ToBoolean(dt.Rows[0]["M_MasaAc"]);
                M_BaskaMasa = Convert.ToBoolean(dt.Rows[0]["M_BaskaMasa"]);

                G_Satirsil_Y = Convert.ToBoolean(dt.Rows[0]["G_Satirsil_Y"]);
                M_GarsonDegistir = Convert.ToBoolean(dt.Rows[0]["M_GarsonDegistir"]);
                G_Zayi = Convert.ToBoolean(dt.Rows[0]["G_Zayi"]);
                G_Ikram = Convert.ToBoolean(dt.Rows[0]["G_Ikram"]);
                M_KisiSayisi = Convert.ToBoolean(dt.Rows[0]["M_KisiSayisi"]);
                R_MasaGeri = Convert.ToBoolean(dt.Rows[0]["R_MasaGeri"]);
                M_SiparisTekrar = Convert.ToBoolean(dt.Rows[0]["M_SiparisTekrar"]);
                Pda_HesapDok = Convert.ToBoolean(dt.Rows[0]["Pda_HesapDok"]);
                H_HizliSatis = Convert.ToBoolean(dt.Rows[0]["H_HizliSatis"]);
                R_TopluIsle = Convert.ToBoolean(dt.Rows[0]["R_TopluIsle"]);
                S_Sp_Sil = Convert.ToBoolean(dt.Rows[0]["S_Sp_Sil"]);
                urunIade = Convert.ToBoolean(dt.Rows[0]["urunIade"]);
                ExtraFolio = Convert.ToBoolean(dt.Rows[0]["ExtraFolio"]);


                And_Bucuk = Convert.ToBoolean(dt.Rows[0]["And_Bucuk"]);
                And_Yarim = Convert.ToBoolean(dt.Rows[0]["And_Yarim"]);
                And_Tam = Convert.ToBoolean(dt.Rows[0]["And_Tam"]);
                And_Duble = Convert.ToBoolean(dt.Rows[0]["And_Duble"]);
                Pos_SubeTrf = Convert.ToBoolean(dt.Rows[0]["Pos_SubeTrf"]);
                Pos_OdemeDegistir = Convert.ToBoolean(dt.Rows[0]["Pos_OdemeDegistir"]);
                Pos_ArtiEksi_Aktif = Convert.ToBoolean(dt.Rows[0]["Pos_ArtiEksi_Aktif"]);
                Pos_MasaAnlikDurum = Convert.ToBoolean(dt.Rows[0]["Pos_MasaAnlikDurum"]);
                Pos_MasaUrunSil = Convert.ToBoolean(dt.Rows[0]["Pos_MasaUrunSil"]);
                Pos_IWERep = Convert.ToBoolean(dt.Rows[0]["Pos_IWERep"]);
                Pos_KartF_CheckOut = Convert.ToBoolean(dt.Rows[0]["Pos_KartF_CheckOut"]);

                Pos_MasaDirekS = Convert.ToBoolean(dt.Rows[0]["Pos_MasaDirekS"]);
                Pos_MasaPaketS = Convert.ToBoolean(dt.Rows[0]["Pos_MasaPaketS"]);
                Pos_YS_YetkiReddet = Convert.ToBoolean(dt.Rows[0]["Pos_YS_YetkiReddet"]);
                Pos_YarimDubleAlan = Convert.ToBoolean(dt.Rows[0]["Pos_YarimDubleAlan"]);

                Pos_Culture = Convert.ToString(dt.Rows[0]["Pos_Culture"]);
                Pos_ReceteTanimlama = Convert.ToBoolean(dt.Rows[0]["Pos_ReceteTanimlama"]);
                Pos_FixMenu = Convert.ToBoolean(dt.Rows[0]["Pos_FixMenu"]);

                Pos_AdisyonPr = Convert.ToBoolean(dt.Rows[0]["Pos_AdisyonPr"]);

                Pos_HesapArti = Convert.ToBoolean(dt.Rows[0]["Pos_HesapArti"]);
                User_AP = Convert.ToBoolean(dt.Rows[0]["User_AP"]);
                Pos_OdaKontrol = Convert.ToBoolean(dt.Rows[0]["Pos_OdaKontrol"]);
                U_BackUser = Convert.ToString(dt.Rows[0]["U_BackUser"]);
                Pos_KartfIndirimAktif = Convert.ToBoolean(dt.Rows[0]["Pos_KartfIndirimAktif"]);
                Pos_ServisPayiDuzelt = Convert.ToBoolean(dt.Rows[0]["Pos_ServisPayiDuzelt"]);

                //MasaTakip_Paket = Convert.ToBoolean(dt.Rows[0]["MasaTakip_Paket"]);

                otoDirekSatis = Convert.ToBoolean(dt.Rows[0]["otoDirekSatis"]);
                otoMasaEkraniAc = Convert.ToBoolean(dt.Rows[0]["otoMasaEkraniAc"]);
                satisYapma = Convert.ToBoolean(dt.Rows[0]["satisYapma"]);
                merkezsubeaktif = Convert.ToBoolean(dt.Rows[0]["merkezsubeaktif"]);
                coklugunsonu = Convert.ToBoolean(dt.Rows[0]["coklugunsonu"]);
                kapaliMasayaGir = Convert.ToBoolean(dt.Rows[0]["kapaliMasayaGir"]);

            }

            DataTable dtXZ = dbtools.SelectTable("SELECT 0,Id,P_Kod,ISNULL(Odeme,0) as Odeme,ISNULL(Servis,0) as Servis,ISNULL(Cari,0) as Cari,ISNULL(Odenmez,0) as Odenmez,ISNULL(Malzeme,0) as Malzeme,ISNULL(Anagrup,0) as Anagrup,ISNULL(Altgrup,0) as Altgrup,ISNULL(Iptal,0) as Iptal,ISNULL(PaketServis,0) as PaketServis,ISNULL(IndirimMasa,0) as IndirimMasa,ISNULL(YiyecekIcecek,0) as YiyecekIcecek,ISNULL(MasaKonum,0) as MasaKonum,ISNULL(GarsonOzet,0) as GarsonOzet,ISNULL(GarsonTahsil,0) as GarsonTahsil,ISNULL(SifirTutar,0) as SifirTutar,ISNULL(OzetKasa,0) as OzetKasa,ISNULL(ExtKasaRapor,0) as ExtKasaRapor,ISNULL(ExtKasaDetay,0) as ExtKasaDetay,ISNULL(SiparisIptal,0) as SiparisIptal,ISNULL(GonderilmemisSiparisIptal,0) as GonderilmemisSiparisIptal,ISNULL(SiparisDuzelt,0) as SiparisDuzelt,ISNULL(cariTahsilatlari,0) as cariTahsilatlari,ISNULL(servisPayiKdvOran,0) as servisPayiKdvOran FROM Rmosmuh.dbo.Pos_User_XZ where P_Kod = '" + P_Kod + "' ");
            if (dtXZ.Rows.Count > 0)
            {
                XZ_Odeme = Convert.ToBoolean(dtXZ.Rows[0]["Odeme"]);
                XZ_Servis = Convert.ToBoolean(dtXZ.Rows[0]["Servis"]);
                XZ_Cari = Convert.ToBoolean(dtXZ.Rows[0]["Cari"]);
                XZ_Odenmez = Convert.ToBoolean(dtXZ.Rows[0]["Odenmez"]);
                XZ_Malzeme = Convert.ToBoolean(dtXZ.Rows[0]["Malzeme"]);
                XZ_Anagrup = Convert.ToBoolean(dtXZ.Rows[0]["Anagrup"]);
                XZ_Altgrup = Convert.ToBoolean(dtXZ.Rows[0]["Altgrup"]);
                XZ_Iptal = Convert.ToBoolean(dtXZ.Rows[0]["Iptal"]);
                XZ_PaketServis = Convert.ToBoolean(dtXZ.Rows[0]["PaketServis"]);
                XZ_IndirimMasa = Convert.ToBoolean(dtXZ.Rows[0]["IndirimMasa"]);
                XZ_YiyecekIcecek = Convert.ToBoolean(dtXZ.Rows[0]["YiyecekIcecek"]);
                XZ_MasaKonum = Convert.ToBoolean(dtXZ.Rows[0]["MasaKonum"]);
                XZ_GarsonOzet = Convert.ToBoolean(dtXZ.Rows[0]["GarsonOzet"]);
                XZ_GarsonTahsil = Convert.ToBoolean(dtXZ.Rows[0]["GarsonTahsil"]);
                XZ_SifirTutar = Convert.ToBoolean(dtXZ.Rows[0]["SifirTutar"]);
                XZ_OzetKasa = Convert.ToBoolean(dtXZ.Rows[0]["OzetKasa"]);
                XZ_ExtKasaRapor = Convert.ToBoolean(dtXZ.Rows[0]["ExtKasaRapor"]);
                XZ_ExtKasaDetay = Convert.ToBoolean(dtXZ.Rows[0]["ExtKasaDetay"]);
                XZ_SiparisIptal = Convert.ToBoolean(dtXZ.Rows[0]["SiparisIptal"]);
                XZ_GonderilmemisSiparisIptal = Convert.ToBoolean(dtXZ.Rows[0]["GonderilmemisSiparisIptal"]);
                XZ_SiparisDuzelt = Convert.ToBoolean(dtXZ.Rows[0]["SiparisDuzelt"]);
                cariTahsilatlari = Convert.ToBoolean(dtXZ.Rows[0]["cariTahsilatlari"]);
                servisPayiKdvOran = Convert.ToBoolean(dtXZ.Rows[0]["servisPayiKdvOran"]);

            }
        }

        public static string Isim_Getir(string User_Kod)
        {
            return Convert.ToString(dbtools.DegerGetir("select P_Ad + ' ' + P_Soyad as Adsoyad from Rmosmuh.dbo.Pos_User WITH(NOLOCK) where P_Kod = '" + User_Kod + "'"));
        }

        public static string ID_Getir(string User_Kod)
        {
            return Convert.ToString(dbtools.DegerGetir("select P_Id from Rmosmuh.dbo.Pos_User WITH(NOLOCK) where P_Kod = '" + User_Kod + "'"));
        }
    }
}
