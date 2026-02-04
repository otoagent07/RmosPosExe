using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos.Class
{
    public class StatikSinif
    {

        public static string MyClass = "StatikSinif";
        public static void eadisyonAc()
        {
            try
            {
                string connectSting = dbtoolsEfatura.connstr;

                RmosE_Fatura.classes.Constants constants = new RmosE_Fatura.classes.Constants();

                RmosE_Fatura.classes.Constants.KullaniciKod = "rmos";
                RmosE_Fatura.dbtools.Sirket_id = 1;

                RmosE_Fatura.classes.Constants.EFatura_SirketId = RmosE_Fatura.dbtools.Sirket_id;


                RmosE_Fatura.classes.Constants.cnnBack = dbtools.connstr;
                RmosE_Fatura.classes.Constants.cnnFront = Fronttools.connstr;


                RmosE_Fatura.UI_Uyumsoft.Uyumsoft_EAdisyon adisyon = new RmosE_Fatura.UI_Uyumsoft.Uyumsoft_EAdisyon();

                adisyon.ShowDialog();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "eadisyonAc", "", ex);
            }
        }
        public static string siranoarttir()
        {
            try
            {
                string sirano = dbtools.DegerGetir("exec Cost_Fis_Sira @depKod='" + Departman.Dep_Kodu + "'");
                return sirano;
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError("StatikSinif", "siranoarttir()", "", ex);
            }
            return "0";
        }
        public static void siranosifirla()
        {
            try
            {
                dbtools.execcmdR("update Stok_Kodlar set Kodlar_PosSiraNo=0 Where Kodlar_Sinif = '01' and Kodlar_Kod='" + Departman.Dep_Kodu + "'");
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError("StatikSinif", "siranosifirla()", "", ex);
            }
        }
        public static string getSira(string fisno)
        {
            string sirano = "0";
            try
            {
                sirano = dbtools.DegerGetir("Select top 1 isnull(sirano,0) as sirano  From Cst_Recete_Satis where Rsat_Fisno='" + fisno + "' order by sirano desc");

                if (sirano == "0" || sirano == "")
                {
                    sirano = siranoarttir();
                }

                dbtools.execcmdR("update Cst_Recete_Satis set sirano='" + sirano + "' where Rsat_Fisno='" + fisno + "' ");
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError("StatikSinif", "siranoarttir()", "", ex);
            }
            return sirano;
        }

        public static string getAlterQuery() // scriptgeç scriptbas
        {
            string query = @"

IF COL_LENGTH('Cst_Recete_Satis', 'PaymentCode') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD PaymentCode nvarchar(3500) END;
IF COL_LENGTH('Cst_Recete_Satis', 'PaymentLinkId') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD PaymentLinkId nvarchar(3500) END;



IF COL_LENGTH('Cst_Recete_Satis', 'PavoDurum') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD PavoDurum int END;



IF COL_LENGTH('Cst_Recete_Satis', 'deger1') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD deger1 nvarchar(3500) END;


IF COL_LENGTH('Cst_Recete_Satis', 'bekoDurum') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD bekoDurum int END;
IF COL_LENGTH('Cst_Recete_Satis', 'bekoAciklama') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD bekoAciklama nvarchar(MAX) END;
IF COL_LENGTH('Cst_Recete_Satis', 'bekoId') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD bekoId nvarchar(MAX) END;


IF COL_LENGTH('Pos_Param', 'tipboxReceteKod') IS NULL BEGIN ALTER TABLE Pos_Param ADD tipboxReceteKod nvarchar(200) END;

IF COL_LENGTH('Pos_Param', 'gunsonubitissaat') IS NULL BEGIN ALTER TABLE Pos_Param ADD gunsonubitissaat nvarchar(200) END;

IF COL_LENGTH('Pos_Param', 'siparisTekrarPrintName') IS NULL BEGIN ALTER TABLE Pos_Param ADD siparisTekrarPrintName nvarchar(500) END;

IF COL_LENGTH('Pos_Param', 'mobileCallerIdMacAdres') IS NULL BEGIN ALTER TABLE Pos_Param ADD mobileCallerIdMacAdres nvarchar(500) END;

IF COL_LENGTH('Pos_Param', 'masatakipKonumYukseklik') IS NULL BEGIN ALTER TABLE Pos_Param ADD masatakipKonumYukseklik nvarchar(500) END;

ALTER TABLE Pos_Log ALTER COLUMN Log_Recete VARCHAR(3500);

IF COL_LENGTH('Pos_FolioParam', 'HizmetReceteKodCocuk') IS NULL BEGIN ALTER TABLE Pos_FolioParam ADD HizmetReceteKodCocuk nvarchar(200) END;

IF COL_LENGTH('Pos_FolioParam', 'GelirReceteKod') IS NULL BEGIN ALTER TABLE Pos_FolioParam ADD GelirReceteKod nvarchar(2000) END;

ALTER TABLE RmosMuh..Pos_User ALTER COLUMN P_Bindirim_Yuzde int;


IF COL_LENGTH('Pos_Cari', 'Cari_indirimOran') IS NULL BEGIN ALTER TABLE Pos_Cari ADD Cari_indirimOran decimal(18, 2) END;

IF COL_LENGTH('Pos_Cari', 'Cari_KaraListede ') IS NULL BEGIN ALTER TABLE Pos_Cari ADD Cari_KaraListede bit END;


IF COL_LENGTH('Pos_FolioParam', 'hizmetBedeliAktif') IS NULL BEGIN ALTER TABLE Pos_FolioParam ADD hizmetBedeliAktif bit END;

IF COL_LENGTH('Cst_Recete_Satis', 'latitude') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD latitude nvarchar(200) END;
IF COL_LENGTH('Cst_Recete_Satis', 'longitude') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD longitude nvarchar(200) END;


IF COL_LENGTH('Cst_Recete_Satis', 'E_AdisyonNo') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD E_AdisyonNo nvarchar(200) END;
IF COL_LENGTH('Cst_Recete_Satis', 'E_AdisyonDurum') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD E_AdisyonDurum bit END;
IF COL_LENGTH('Cst_Recete_Satis', 'kisiyeSatisAdSoyad') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD kisiyeSatisAdSoyad nvarchar(200) END;
IF COL_LENGTH('Pos_Kodlar', 'Pkod_E_Adisyon') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD Pkod_E_Adisyon bit END;

IF COL_LENGTH('Pos_Kodlar', 'tumPrinter') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD tumPrinter bit END;


IF COL_LENGTH('Pos_Kodlar', 'Pkod_OdemeAktif') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD Pkod_OdemeAktif bit END;
IF COL_LENGTH('Pos_Kodlar', 'Pkod_OnburoLimitKontrolYapma') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD Pkod_OnburoLimitKontrolYapma bit END;


IF COL_LENGTH('Pos_CallerId', 'Caller_Durum') IS NULL BEGIN ALTER TABLE Pos_CallerId ADD Caller_Durum int END;


IF COL_LENGTH('Cst_Recete_Satis', 'rezevePrintCiktimi') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD rezevePrintCiktimi bit END;

IF COL_LENGTH('Pos_Kodlar', 'bit1') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD bit1 bit END;
IF COL_LENGTH('Pos_Kodlar', 'bit2') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD bit2 bit END;
IF COL_LENGTH('Pos_Kodlar', 'bit3') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD bit3 bit END;
IF COL_LENGTH('Pos_Kodlar', 'bit4') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD bit4 bit END;
IF COL_LENGTH('Pos_Kodlar', 'bit5') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD bit5 bit END;
IF COL_LENGTH('Pos_Kodlar', 'bit6') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD bit6 bit END;
IF COL_LENGTH('Pos_Kodlar', 'bit7') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD bit7 bit END;

IF COL_LENGTH('Cst_Recete_Satis', 'Rsat_AcilisTar') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD Rsat_AcilisTar  datetime null end;


IF COL_LENGTH('Cst_Recete_Satis', 'paketAtamaTarih') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD paketAtamaTarih  datetime null end;


IF COL_LENGTH('Cst_Recete_Satis', 'sepetDurum') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD sepetDurum int END;




IF COL_LENGTH('Cst_Recete_Satis', 'ustgrup') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD ustgrup nvarchar(200) END;

IF COL_LENGTH('Cst_Recete_Satis', 'printjson') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD printjson nvarchar(4000) END;

IF COL_LENGTH('Cst_Recete_Satis', 'sirano') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD sirano int END;
IF COL_LENGTH('Cst_Recete_Satis', 'altgrup') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD altgrup nvarchar(200) END;
IF COL_LENGTH('Cst_Recete_Satis', 'konumposta') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD konumposta nvarchar(200) END;
IF COL_LENGTH('Cst_Recete_Satis', 'BankaID') IS NULL BEGIN ALTER TABLE Cst_Recete_Satis ADD BankaID int END;
IF COL_LENGTH('Cst_Recete', 'Rec_DovizliSatis') IS NULL BEGIN ALTER TABLE Cst_Recete ADD Rec_DovizliSatis bit END;
IF COL_LENGTH('Pos_Param', 'kartnoSayisi') IS NULL BEGIN ALTER TABLE Pos_Param ADD kartnoSayisi int END;

IF COL_LENGTH('Pos_Param', 'hesapFisQr') IS NULL BEGIN ALTER TABLE Pos_Param ADD hesapFisQr bit END;


IF COL_LENGTH('Pos_Param', 'paketotohesapkapat') IS NULL BEGIN ALTER TABLE Pos_Param ADD paketotohesapkapat bit END;
IF COL_LENGTH('Pos_Param', 'sepetaktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD sepetaktif bit END;
IF COL_LENGTH('Pos_Param', 'merkezaktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD merkezaktif bit END;
IF COL_LENGTH('Pos_Param', 'ingenico2') IS NULL BEGIN ALTER TABLE Pos_Param ADD ingenico2 bit END;
IF COL_LENGTH('Pos_Param', 'ekranKlavyesiAktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD ekranKlavyesiAktif bit END;
IF COL_LENGTH('Pos_Param', 'ingenicoOdemeAtmasin') IS NULL BEGIN ALTER TABLE Pos_Param ADD ingenicoOdemeAtmasin bit END;
IF COL_LENGTH('Pos_Param', 'satisdaOzelMasa') IS NULL BEGIN ALTER TABLE Pos_Param ADD satisdaOzelMasa bit END;



IF COL_LENGTH('Pos_Param', 'hesapFisQrFisno') IS NULL BEGIN ALTER TABLE Pos_Param ADD hesapFisQrFisno bit END;


IF COL_LENGTH('Pos_Param', 'bekoFaturaKesimLimit') IS NULL BEGIN ALTER TABLE Pos_Param ADD bekoFaturaKesimLimit  decimal(18, 2) END;


IF COL_LENGTH('Pos_Param', 'masatrTutSurukle') IS NULL BEGIN ALTER TABLE Pos_Param ADD masatrTutSurukle bit END;
IF COL_LENGTH('Pos_Param', 'servispayFooterda') IS NULL BEGIN ALTER TABLE Pos_Param ADD servispayFooterda bit END;

IF COL_LENGTH('Pos_Param', 'servispayOdenmezIkramSil') IS NULL BEGIN ALTER TABLE Pos_Param ADD servispayOdenmezIkramSil bit END;
IF COL_LENGTH('Pos_Param', 'tumPrinter') IS NULL BEGIN ALTER TABLE Pos_Param ADD tumPrinter bit END;
IF COL_LENGTH('Pos_Param', 'urunAdinaOdaklan') IS NULL BEGIN ALTER TABLE Pos_Param ADD urunAdinaOdaklan bit END;
IF COL_LENGTH('Pos_Param', 'ikinciEkranAktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD ikinciEkranAktif bit END;
IF COL_LENGTH('Pos_Param', 'otomatikOdenmez') IS NULL BEGIN ALTER TABLE Pos_Param ADD otomatikOdenmez bit END;
IF COL_LENGTH('Pos_Param', 'mobilCallerIdAktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD mobilCallerIdAktif bit END;



IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_ServisPayiDuzelt') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_ServisPayiDuzelt] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'satisYapma') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [satisYapma] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_OdenmezIkramPasif') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_OdenmezIkramPasif] bit END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'coklugunsonu') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [coklugunsonu] bit END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'cariTarihGecmisAktif') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [cariTarihGecmisAktif] bit END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'kapaliMasayaGir') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [kapaliMasayaGir] bit END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_AcikmasalariGizle') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_AcikmasalariGizle] bit END;


IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'ingenicoaktif') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [ingenicoaktif] bit END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'tutarduzeltplus') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [tutarduzeltplus] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'otoDirekSatis') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [otoDirekSatis] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'otoMasaEkraniAc ') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [otoMasaEkraniAc ] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'otoMasaEkraniAc') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [otoMasaEkraniAc] bit END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'merkezsubeaktif') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [merkezsubeaktif] bit END;


IF COL_LENGTH('Pos_FolioParam', 'hizmetOdemeKod') IS NULL BEGIN ALTER TABLE Pos_FolioParam ADD hizmetOdemeKod nvarchar(200) END;
IF COL_LENGTH('Pos_FolioParam', 'HizmetReceteKod') IS NULL BEGIN ALTER TABLE Pos_FolioParam ADD HizmetReceteKod nvarchar(200) END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_KartfIndirimAktif') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_KartfIndirimAktif] bit END;
IF COL_LENGTH('Pos_Param', 'onburoikramsifiryazaktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD onburoikramsifiryazaktif bit END;
IF COL_LENGTH('Pos_Param', 'cariindirimAktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD cariindirimAktif bit END;
IF COL_LENGTH('Pos_Param', 'masatakiphesappasif') IS NULL BEGIN ALTER TABLE Pos_Param ADD masatakiphesappasif bit END;
IF COL_LENGTH('Pos_Param', 'satirsilfiscikmasinaktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD satirsilfiscikmasinaktif bit END;
IF COL_LENGTH('Pos_Param', 'hesapkapatfiscikmasinaktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD hesapkapatfiscikmasinaktif bit END;
IF COL_LENGTH('Pos_Param', 'otoMasaEkraniAc') IS NULL BEGIN ALTER TABLE Pos_Param ADD otoMasaEkraniAc bit END;



IF COL_LENGTH('Pos_Param', 'kisivegarsonbirkeresoraktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD kisivegarsonbirkeresoraktif bit END;
IF COL_LENGTH('Pos_Param', 'yazdirilmamissiparis') IS NULL BEGIN ALTER TABLE Pos_Param ADD yazdirilmamissiparis bit END;
IF COL_LENGTH('Pos_Param', 'masamusait') IS NULL BEGIN ALTER TABLE Pos_Param ADD masamusait bit END;


IF COL_LENGTH('RmosMuh.dbo.Pos_User_XZ', 'hesapyazici') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User_XZ ADD [hesapyazici] nvarchar(250) END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User_XZ', 'cariTahsilatlari') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User_XZ ADD [cariTahsilatlari] bit END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User_XZ', 'servisPayiKdvOran') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User_XZ ADD [servisPayiKdvOran] bit END;



IF COL_LENGTH('Stok_Kodlar', 'Kodlar_parakasa') IS NULL BEGIN ALTER TABLE Stok_Kodlar ADD Kodlar_parakasa nvarchar(200) END;


IF COL_LENGTH('Stok_Kodlar', 'Kodlar_Beko') IS NULL BEGIN ALTER TABLE Stok_Kodlar ADD Kodlar_Beko bit END;


IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_Eksileme') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD Pos_Eksileme bit END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_XZdepartman') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD Pos_XZdepartman bit END;
ALTER TABLE GetirYemek_Option ALTER COLUMN  GOption_price    decimal(18, 2)
ALTER TABLE GetirYemek_Menu_Option ALTER COLUMN Option_price decimal(18, 2)
ALTER TABLE GetirYemek_Menu_Product ALTER COLUMN Product_price decimal(18, 2)
ALTER TABLE GetirYemek_Menu_Product ALTER COLUMN Product_stuckPrice decimal(18, 2)
ALTER TABLE Cst_Recete ALTER COLUMN Rec_GetirMenuID nvarchar(2500)
ALTER TABLE GetirYemek_Option ALTER COLUMN GOption_price decimal(18, 2)
ALTER TABLE GetirYemek_Product ALTER COLUMN GProducts_price decimal(18, 2)
ALTER TABLE GetirYemek_Product ALTER COLUMN GProducts_optionPrice decimal(18, 2)
ALTER TABLE GetirYemek_Product ALTER COLUMN GProducts_priceWithOption decimal(18, 2)
ALTER TABLE GetirYemek_Product ALTER COLUMN GProducts_totalPrice decimal(18, 2)
ALTER TABLE GetirYemek_Product ALTER COLUMN GProducts_totalOptionPrice decimal(18, 2)
ALTER TABLE GetirYemek_Product ALTER COLUMN GProducts_totalPriceWithOption decimal(18, 2)

ALTER TABLE Cst_Recete_Satis ALTER COLUMN Rsat_Recete nvarchar(200);
ALTER TABLE Cst_Recete_Satis ALTER COLUMN Rsat_Kapatma nvarchar(30);
ALTER TABLE Cst_Recete_Satis ALTER COLUMN Rsat_Adisyon nvarchar(150);
ALTER TABLE Cst_Recete_Satis ALTER COLUMN Rsat_Aciklama nvarchar(2000);
ALTER TABLE Cst_Recete_Satis ALTER COLUMN Rsat_ZayiNeden nvarchar(1000);
ALTER TABLE Cst_Recete_Satis ALTER COLUMN Rsat_IkramNeden nvarchar(1000);
ALTER TABLE Cst_Recete_Satis ALTER COLUMN Rsat_OzelMasaAdi nvarchar(150);
ALTER TABLE Cst_Recete_Satis ALTER COLUMN Rsat_YSOrderID nvarchar(500);

ALTER TABLE Cst_Satis_Ipt ALTER COLUMN Rsat_Recete nvarchar(200);
ALTER TABLE Cst_Satis_Ipt ALTER COLUMN Rsat_Aciklama nvarchar(2000);
ALTER TABLE Cst_Satis_Ipt ALTER COLUMN Rsat_IptalNot nvarchar(1000);



ALTER TABLE Pos_Cari ALTER COLUMN Cari_Adres1 nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_Adres2 nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_Adres3 nvarchar(2000);


ALTER TABLE Pos_Masa ALTER COLUMN Masa_Posta nvarchar(50);
ALTER TABLE Pos_Masa ALTER COLUMN Masa_Konum nvarchar(50);
ALTER TABLE Pos_Masa ALTER COLUMN Masa_Depart nvarchar(50);



ALTER TABLE Pos_Param ALTER COLUMN Param_Fis_Aciklama nvarchar(1000);
ALTER TABLE Pos_Param ALTER COLUMN Param_Kur decimal(10,6);


ALTER TABLE Rapor_Dizayn ALTER COLUMN Rapor_Sekil varbinary(MAX);


ALTER TABLE Pos_Cari ALTER COLUMN Cari_YS_AddressId nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_YS_CustomerID nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_Funvan2 nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_Getir_ID nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_Getir_AddressId nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_MuhasebeKodu nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_Mahalle nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_Ilce nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_Il nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_YS_AddressId nvarchar(2000);
ALTER TABLE Pos_Cari ALTER COLUMN Cari_YS_AddressId nvarchar(2000);

IF COL_LENGTH('Pos_Param', 'Param_SatisCikisButton') IS NULL BEGIN ALTER TABLE Pos_Param ADD [Param_SatisCikisButton] bit END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'U_BackUser') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [U_BackUser] nvarchar(50) END;


IF COL_LENGTH('Pos_Param', 'Param_nfcBarkodAktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD [Param_nfcBarkodAktif] bit END;

IF COL_LENGTH('Pos_Param', 'Param_StokAnlikAtmasin') IS NULL BEGIN ALTER TABLE Pos_Param ADD [Param_StokAnlikAtmasin] bit END;


IF COL_LENGTH('Pos_Param', 'Param_ParcaliMasaAktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD [Param_ParcaliMasaAktif] bit END;
IF COL_LENGTH('Pos_Masa', 'Masa_Parcali') IS NULL BEGIN ALTER TABLE Pos_Masa ADD [Masa_Parcali] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'chk_K_KasaRapor') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [chk_K_KasaRapor] bit END;

IF COL_LENGTH('Pos_Masa', 'Masa_Musait') IS NULL BEGIN ALTER TABLE Pos_Masa ADD Masa_Musait nvarchar(100) END;

IF COL_LENGTH('Pos_Masa', 'Masa_Sirano') IS NULL BEGIN ALTER TABLE Pos_Masa ADD [Masa_Sirano] int END;


IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_KartTanimDuzelt') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_KartTanimDuzelt] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_KartTanimTransfer') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_KartTanimTransfer] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_KartTanimBakiyeTransfer') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_KartTanimBakiyeTransfer] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'urunIade') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [urunIade] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User_XZ', 'xzraporyazici') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User_XZ ADD [xzraporyazici] nvarchar(250) END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'postema') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [postema] nvarchar(2500) END;

IF COL_LENGTH('Pos_Kodlar', 'hesapDokTutarSifir') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD hesapDokTutarSifir bit END;
IF COL_LENGTH('Pos_Kodlar', 'saatAralikDurdur') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD saatAralikDurdur nvarchar(500) END;

IF COL_LENGTH('Pos_Kodlar', 'pavoOdemeKod') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD pavoOdemeKod int END;
IF COL_LENGTH('Pos_Kodlar', 'bekoOdemeKod') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD bekoOdemeKod int END;

IF COL_LENGTH('Pos_Cari', 'Cari_TrendyolId') IS NULL BEGIN ALTER TABLE Pos_Cari ADD Cari_TrendyolId int END;

IF COL_LENGTH('Pos_Cari', 'adressecenek') IS NULL BEGIN ALTER TABLE Pos_Cari ADD adressecenek int END;


IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_dil') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD Pos_dil nvarchar(50) END;

IF COL_LENGTH('Pos_Kasahrk', 'Pkasa_dep') IS NULL BEGIN ALTER TABLE Pos_Kasahrk ADD Pkasa_dep nvarchar(100) END;
";

            return query;
        }

        public static void shrinkData()
        {
            try
            {

                dbtools.execcmdRMesajsiz("exec shrinkdataproc @dbname='" + dbtools.database + "'");
            }
            catch (Exception ex)
            {

            }
        }




        public static string getTriggerAcilisTar()
        {
            string queryTrig1 = @" CREATE TRIGGER trg_InsertRsat_AcilisTar
	ON Cst_Recete_Satis
	AFTER INSERT
	AS
	BEGIN
    SET NOCOUNT ON;

	declare @tarihim datetime 
	    SET @tarihim = (SELECT TOP 1 ISNULL(Rsat_AcilisTar, GETDATE()) AS tarih 
                    FROM Cst_Recete_Satis WITH (NOLOCK) 
                    WHERE Rsat_Fisno = (SELECT top 1 Rsat_Fisno FROM inserted) 
                    ORDER BY Rsat_Id);

    UPDATE Cst_Recete_Satis
    SET Rsat_AcilisTar = @tarihim
    FROM inserted
    WHERE Cst_Recete_Satis.Rsat_Id = inserted.Rsat_Id;
END;";


            string queryTrig = @"IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'trg_InsertRsat_AcilisTar' AND parent_class_desc = 'OBJECT_OR_COLUMN' AND parent_id = OBJECT_ID('Cst_Recete_Satis'))
BEGIN
    EXEC('
  " + queryTrig1 + @"
    ');
END
ELSE
BEGIN EXEC('
  " + queryTrig1.Replace("CREATE TRIGGER", "ALTER TRIGGER") + @"
    ') END";

            return queryTrig;
        }
        public static void dilDegis(string dil = "tr-TR") // en-US
        {
            try
            {
                dil = dbtools.DegerGetir("select top 1 isnull(Pos_dil,'tr-TR') as Pos_dil  from Rmosmuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'");

                Langs.Default.Dil = dil;
                Langs.Default.Save();

                CultureInfo culture = new CultureInfo(dil);
                culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
                //culture.DateTimeFormat.DateSeparator = ".";
                culture.DateTimeFormat.ShortTimePattern = "HH:mm";
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                Localize.ApplicationLanguage(culture.TwoLetterISOLanguageName);

                Program.main.dilYenile();
                Program.main.login.dilYenile();
            }
            catch (Exception ex)
            {

            }

        }
        public static void masaKilitle(string Masa_No)
        {
            try
            {
                if (Param.masamusait == false)
                {
                    return;
                }

                string kodAd = User.P_Kod + "-" + User.P_Ad + " " + User.P_Soyad;
                string query = "update Pos_Masa set Masa_Musait='" + kodAd + "' where Masa_No='" + Masa_No + "'";
                dbtools.execcmd(query);
            }
            catch (Exception ex)
            {

            }
        }

        public static void masaKilitAc()
        {
            try
            {
                // mobil uygulama yapılırsa aşağısını açabilirsin
                //if (Param.masamusait == false)
                //{
                //    return ;
                //}

                string kodAd = User.P_Kod + "-" + User.P_Ad + " " + User.P_Soyad;

                string query = "update Pos_Masa set Masa_Musait='" + 0 + "' where Masa_Musait='" + kodAd + "'";
                dbtools.execcmd(query);
            }
            catch (Exception ex)
            {

            }
        }

        public static bool masaMusaitmi(string Masa_No)
        {
            try
            {
                if (Masa_No == "") return true;
                if (Param.masamusait == false)
                {
                    return true;
                }

                string kodAd = User.P_Kod + "-" + User.P_Ad + " " + User.P_Soyad;

                string query = "select top 1 isnull(Masa_Musait,0) as  Masa_Musait from Pos_Masa where Masa_No='" + Masa_No + "'";
                string deger = dbtools.DegerGetir(query);
                if (deger.Equals("0"))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(deger + " Kullanıcısı şuan işlem yapıyor...\nMasaNo: " + Masa_No);
                    return false;
                }

            }
            catch (Exception ex)
            {

            }

            return true;
        }

        public static bool sadeceKendiMasanaGir(string fisno)
        {
            try
            {

                if (User.M_BaskaMasa == false)
                {
                    string varmi = dbtools.DegerGetir("select top 1 count(Rsat_Garson) as toplam from Cst_Recete_Satis  where Rsat_Fisno='" + fisno + "'");

                    if (varmi == "0")
                    {
                        return true;
                    }



                    string query = "select top 1 count(Rsat_Garson) as toplam from Cst_Recete_Satis  where Rsat_Fisno='" + fisno + "'  and Rsat_Garson='" + User.P_Kod + "' ";
                    string deger = dbtools.DegerGetir(query);
                    if (deger == "0")
                    {
                        MessageBox.Show("BAŞKASININ MASASINA SATIŞ YAPAMAZSIN !");
                        return false;
                    }

                }


            }
            catch (Exception ex)
            {

            }

            return true;
        }

        public static string getInstanceName()
        {
            string query = @"DECLARE @GetInstances TABLE
( Value nvarchar(100),
 InstanceNames nvarchar(100),
 Data nvarchar(100))

Insert into @GetInstances
EXECUTE xp_regread
  @rootkey = 'HKEY_LOCAL_MACHINE',
  @key = 'SOFTWARE\Microsoft\Microsoft SQL Server',
  @value_name = 'InstalledInstances'

Select InstanceNames from @GetInstances ";

            return dbtools.DegerGetir(query);
        }

        public static bool getDovizlimi()
        {
            try
            {
                if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;

        }

        public static decimal getTutarKontrol(decimal girilenDeger, decimal mevcutDeger)
        {
            decimal fark = mevcutDeger - girilenDeger;
            if (Math.Abs(fark) <= (decimal)0.9) // 0.9 idi samet bey ile iştarese sonucu 0.9 a çıkarıldı. tarih : 02.10.2025
            {
                girilenDeger = mevcutDeger;
            }
            return girilenDeger;
        }

        public static string defaultParametreCalistir() // scriptgeç scriptbas
        {
            string query = @"ALTER proc [dbo].[defaultParametre] as begin
                            -- exec defaultParametre
                 exec [varsayilanParametreOlustur] @ayarlar_key='urun_printer',@ayarlar_value='',@ayarlar_aciklama='bu alanda güncelleme yapmayınız!!!'
                 exec [varsayilanParametreOlustur] @ayarlar_key='paketOtoKapat',@ayarlar_value='0',@ayarlar_aciklama='Getir veya Yemek sepeti teslim edildi olunca otomatik yazdırmadan kapat yapar'

exec [varsayilanParametreOlustur] @ayarlar_key='yuvarlama',@ayarlar_value='',@ayarlar_aciklama='misal 30 kuruşu 50 kuruşa yuvarlar'
exec [varsayilanParametreOlustur] @ayarlar_key='otomatikIndirim',@ayarlar_value='',@ayarlar_aciklama='8.80 TL olanı 8 TL yapar'
exec [varsayilanParametreOlustur] @ayarlar_key='onburoFisTekAt',@ayarlar_value='0',@ayarlar_aciklama='Önbüroda folio çiftlememesi için'
exec [varsayilanParametreOlustur] @ayarlar_key='satisEkranGenislik',@ayarlar_value='288',@ayarlar_aciklama='Satıştaki sipariş ekranının genişliği'

                select 'Başarılı' as basarili
                end";

            return query;
        }


        public static decimal getKur()
        {
            decimal kur = 1;
            try
            {
                string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";

                string dovizXml = dbtools.DegerGetir("select Mkodlar_Xml from Muh_Kodlar where Mkodlar_Sinif = '02' and Mkodlar_Kod = '" + Param.Doviz_Kodu + "'");
                //if (!(dovizXml == "" || dovizXml == "TL"))
                //{
                kur = Convert.ToDecimal(dbtools.DegerGetir("select isnull((select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + Param.Doviz_Kodu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'),1)"));

            }
            catch (Exception ex)
            {

            }
            return kur;
        }
        public static decimal getKurRecete(String dovizKodu)
        {
            decimal kur = 1;
            try
            {
                string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";

                string dovizXml = dbtools.DegerGetir("select Mkodlar_Xml from Muh_Kodlar where Mkodlar_Sinif = '02' and Mkodlar_Kod = '" + dovizKodu + "'");
                //if (!(dovizXml == "" || dovizXml == "TL"))
                //{
                kur = Convert.ToDecimal(dbtools.DegerGetir("select isnull((select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + dovizKodu + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'),1)"));

            }
            catch (Exception ex)
            {

            }
            return kur;
        }
    }
}
