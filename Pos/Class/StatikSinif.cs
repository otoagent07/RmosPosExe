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
        public static string getAlterQuery() // scriptgeç scriptbas
        {
            string query = @"
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_ServisPayiDuzelt') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_ServisPayiDuzelt] bit END;
IF COL_LENGTH('Pos_FolioParam', 'hizmetOdemeKod') IS NULL BEGIN ALTER TABLE Pos_FolioParam ADD hizmetOdemeKod nvarchar(200) END;
IF COL_LENGTH('Pos_FolioParam', 'HizmetReceteKod') IS NULL BEGIN ALTER TABLE Pos_FolioParam ADD HizmetReceteKod nvarchar(200) END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_KartfIndirimAktif') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_KartfIndirimAktif] bit END;
IF COL_LENGTH('Pos_Param', 'onburoikramsifiryazaktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD onburoikramsifiryazaktif bit END;
IF COL_LENGTH('Pos_Param', 'masatakiphesappasif') IS NULL BEGIN ALTER TABLE Pos_Param ADD masatakiphesappasif bit END;
IF COL_LENGTH('Pos_Param', 'satirsilfiscikmasinaktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD satirsilfiscikmasinaktif bit END;
IF COL_LENGTH('Pos_Param', 'kisivegarsonbirkeresoraktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD kisivegarsonbirkeresoraktif bit END;
IF COL_LENGTH('Pos_Param', 'yazdirilmamissiparis') IS NULL BEGIN ALTER TABLE Pos_Param ADD yazdirilmamissiparis bit END;
IF COL_LENGTH('Pos_Param', 'masamusait') IS NULL BEGIN ALTER TABLE Pos_Param ADD masamusait bit END;
IF COL_LENGTH('RmosMuh.dbo.Pos_User_XZ', 'hesapyazici') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User_XZ ADD [hesapyazici] nvarchar(250) END;
IF COL_LENGTH('Stok_Kodlar', 'Kodlar_parakasa') IS NULL BEGIN ALTER TABLE Stok_Kodlar ADD Kodlar_parakasa nvarchar(200) END;
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

ALTER TABLE Ys_Order ALTER COLUMN Address nvarchar(MAX);

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

IF COL_LENGTH('Pos_Param', 'Param_ParcaliMasaAktif') IS NULL BEGIN ALTER TABLE Pos_Param ADD [Param_ParcaliMasaAktif] bit END;
IF COL_LENGTH('Pos_Masa', 'Masa_Parcali') IS NULL BEGIN ALTER TABLE Pos_Masa ADD [Masa_Parcali] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'chk_K_KasaRapor') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [chk_K_KasaRapor] bit END;

IF COL_LENGTH('Pos_Masa', 'Masa_Musait') IS NULL BEGIN ALTER TABLE Pos_Masa ADD Masa_Musait nvarchar(100) END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_KartTanimDuzelt') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_KartTanimDuzelt] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_KartTanimTransfer') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_KartTanimTransfer] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User', 'Pos_KartTanimBakiyeTransfer') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User ADD [Pos_KartTanimBakiyeTransfer] bit END;

IF COL_LENGTH('RmosMuh.dbo.Pos_User_XZ', 'xzraporyazici') IS NULL BEGIN ALTER TABLE RmosMuh.dbo.Pos_User_XZ ADD [xzraporyazici] nvarchar(250) END;

IF COL_LENGTH('Pos_Kodlar', 'hesapDokTutarSifir') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD hesapDokTutarSifir bit END;
IF COL_LENGTH('Pos_Kodlar', 'saatAralikDurdur') IS NULL BEGIN ALTER TABLE Pos_Kodlar ADD saatAralikDurdur nvarchar(500) END;
IF COL_LENGTH('Pos_Cari', 'Cari_TrendyolId') IS NULL BEGIN ALTER TABLE Pos_Cari ADD Cari_TrendyolId int END;

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
            if (Math.Abs(fark) <= (decimal)0.09)
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
    }
}
