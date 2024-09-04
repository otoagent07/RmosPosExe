using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos.Class
{
    public class Sabitler
    {


        /*
         Pkod_FisTipi P ise ikram O ise ödenmez
         */
        public static bool odenmezVeyaIkramiseServisPayiSil(string fisno,string odemeKod)
        {
            try
            {
                if (Param.servispayOdenmezIkramSil)
                {

                    string fistipquery = $"select top 1 Pkod_FisTipi from Pos_Kodlar where Pkod_Sinif='11' and Pkod_Kod='{odemeKod}'";
                    string fistip = dbtools.DegerGetir(fistipquery);

                    if ((fistip == "P" || fistip == "O"))
                    {
                        string query = $@"delete from Cst_Recete_Satis where Rsat_Fisno = {fisno} 
and Rsat_Recete =
(select top 1 Kodlar_Servis_Recete from Stok_Kodlar where Kodlar_Sinif = '01' and Kodlar_Kod = 
(select top 1 Rsat_Departman from Cst_Recete_Satis where Rsat_Fisno = {fisno} and Rsat_Ba = 'B' order by Rsat_Id))";

                        dbtools.execcmdR(query);

                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sabitler.odenmezVeyaIkramiseServisPayiSil()-> " + ex.Message);

            }

            return false;
        }
        public static DataTable getOdemeKodlari(DataTable dt)
        {
            try
            {
                bool Pos_OdenmezIkramPasif = Convert.ToBoolean(dbtools.DegerGetir("select top 1 isnull(Pos_OdenmezIkramPasif,0) as Pos_OdenmezIkramPasif from Rmosmuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'"));

                if (Pos_OdenmezIkramPasif)
                {
                    dt = dt.Select("Pkod_FisTipi<>'O'").CopyToDataTable();
                    dt = dt.Select("Pkod_FisTipi<>'P'").CopyToDataTable();




                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sabitler.getOdemeKodlari()-> " + ex.Message);
            }
           

            return dt;
        }


        public static string cst_satis_index =$@"
if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Genelkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Genelkod] ON [Cst_Recete] ( [Rec_Genelkod] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Ad' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Ad] ON [Cst_Recete] ( [Rec_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Barkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Barkod] ON [Cst_Recete] ( [Rec_Barkod] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_AnaAltAd' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_AnaAltAd] ON [Cst_Recete] ( [Rec_Anagrup], [Rec_Altgrup], [Rec_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_UstRecete' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_UstRecete] ON [Cst_Recete] ( [Rec_YS_UrunID] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Dep_DepRecete' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Dep_DepRecete] ON [Cst_Recete_Dep] ( [Rdep_Departman], [Rdep_Recete] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Detay_Recete' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Detay_Recete] ON [Cst_Recete_Detay] ( [Detay_Recete] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Detay_ReceteStokkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Detay_ReceteStokkod] ON [Cst_Recete_Detay] ( [Detay_Recete], [Detay_Stokkod] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Menu_DepGenelkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Menu_DepGenelkod] ON [Cst_Recete_Menu] ( [RMenu_Dep], [RMenu_ReceteGenelKod] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Menu_Genelkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Menu_Genelkod] ON [Cst_Recete_Menu] ( [RMenu_ReceteGenelKod] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Satis_Fisno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Satis_Fisno] ON [Cst_Recete_Satis] ( [Rsat_Fisno] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Satis_TarihDep' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Satis_TarihDep] ON [Cst_Recete_Satis] ( [Rsat_Tarih], [Rsat_Departman] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Satis_MasaDurum' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Satis_MasaDurum] ON [Cst_Recete_Satis] ( [Rsat_Masa], [Rsat_Durum] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Recete_Satis_Cari' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Recete_Satis_Cari] ON [Cst_Recete_Satis] ( [Rsat_Cari] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Satis_Ipt_Fisno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Satis_Ipt_Fisno] ON [Cst_Satis_Ipt] ( [Rsat_Fisno] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Satis_Ipt_Tarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Satis_Ipt_Tarih] ON [Cst_Satis_Ipt] ( [Rsat_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Satis_Ipt_Id' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Satis_Ipt_Id] ON [Cst_Satis_Ipt] ( [Rsat_Id] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Satis_Ipt_DepIptZaman' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Satis_Ipt_DepIptZaman] ON [Cst_Satis_Ipt] ( [Rsat_Departman], [Rsat_Iptal_Zaman] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Sayim_DepTarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Sayim_DepTarih] ON [Cst_Sayim] ( [Sayim_Departman], [Sayim_Tarih], [Sayim_ReceteKod] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Sayim_Tarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Sayim_Tarih] ON [Cst_Sayim] ( [Sayim_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_SayimDetay_SayimId' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_SayimDetay_SayimId] ON [Cst_SayimDetay] ( [Detay_SayimId] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_SayimDetay_Stokkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_SayimDetay_Stokkod] ON [Cst_SayimDetay] ( [Detay_StokKod] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Uretim_TarihFisTipNo' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Uretim_TarihFisTipNo] ON [Cst_Uretim] ( [Tarih], [Fistipi], [Fisno] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Uretim_Tarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Uretim_Tarih] ON [Cst_Uretim] ( [Uret_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Cst_Uretim_Fisno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Cst_Uretim_Fisno] ON [Cst_Uretim] ( [Fisno] )
End

if not exists(select name from sys.indexes where name ='IX_Duyuru_Baslik' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Duyuru_Baslik] ON [Duyuru] ( [Baslik] )
End

if not exists(select name from sys.indexes where name ='IX_Duyuru_Kullanici' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Duyuru_Kullanici] ON [Duyuru] ( [Kullanici] )
End

if not exists(select name from sys.indexes where name ='IX_Kasa_Hrk_KoduTarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Kasa_Hrk_KoduTarih] ON [Kasa_Hrk] ( [Kasa_Kodu], [Kasa_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Kasa_Hrk_TarihFistipi' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Kasa_Hrk_TarihFistipi] ON [Kasa_Hrk] ( [Kasa_Tarih], [Kasa_Fistipi] )
End

if not exists(select name from sys.indexes where name ='IX_Kurlar_Key' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Kurlar_Key] ON [Kurlar] ( [Kurlar_Cesit], [Kurlar_Tarih], [Kurlar_Kodu] )
End

if not exists(select name from sys.indexes where name ='IX_Ldry_Tutanak_Fisno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Ldry_Tutanak_Fisno] ON [Ldry_Tutanak] ( [Fisno] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Cari_Kod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Cari_Kod] ON [Muh_Cari] ( [Cari_Kod] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Hrk_Fisno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Hrk_Fisno] ON [Muh_Hrk] ( [Mhrk_Fisno] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Hrk_TarihHesap' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Hrk_TarihHesap] ON [Muh_Hrk] ( [Mhrk_Tarih], [Mhrk_Hesap] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Hrk_CarikodTarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Hrk_CarikodTarih] ON [Muh_Hrk] ( [Mhrk_Carikod], [Mhrk_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Hrk_Fatno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Hrk_Fatno] ON [Muh_Hrk] ( [Mhrk_Fatno] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Hrk_Log_Tarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Hrk_Log_Tarih] ON [Muh_Hrk_Log] ( [Mhrk_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Hrk_Log_Fisno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Hrk_Log_Fisno] ON [Muh_Hrk_Log] ( [Mhrk_Fisno] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Hrk_Log_Hesap' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Hrk_Log_Hesap] ON [Muh_Hrk_Log] ( [Mhrk_Hesap] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Hrk_Log_Kullanici' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Hrk_Log_Kullanici] ON [Muh_Hrk_Log] ( [Kullanici] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_HrkRez_Id' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_HrkRez_Id] ON [Muh_HrkRez] ( [RezId] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Info' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Info] ON [Muh_Info] ( [Info_Hesap], [Info_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Ipt_KayitIptTarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Ipt_KayitIptTarih] ON [Muh_Ipt] ( [Ipt_Kayittarih], [Ipt_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Kdv_Key' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Kdv_Key] ON [Muh_Kdv] ( [Kdv_Yil], [Kdv_Ay], [Kdv_Oran] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Kodlar_Key' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Kodlar_Key] ON [Muh_Kodlar] ( [Mkodlar_Sinif], [Mkodlar_Kod] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Kodlar' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Kodlar] ON [Muh_Kodlar] ( [Mkodlar_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Plan_Ad' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Plan_Ad] ON [Muh_Plan] ( [Plan_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Plan_TcKimlik' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Plan_TcKimlik] ON [Muh_Plan] ( [Plan_TcKimlik] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Plan_Vergino' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Plan_Vergino] ON [Muh_Plan] ( [Plan_Vergino] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Sirket_Log_Kullanici' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Sirket_Log_Kullanici] ON [Muh_Sirket_Log] ( [Kullanici] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Sirket_Log_Tarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Sirket_Log_Tarih] ON [Muh_Sirket_Log] ( [Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Sube_Sirketkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Sube_Sirketkod] ON [Muh_Sube] ( [Sube_SirketKod] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Sube_Ad' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Sube_Ad] ON [Muh_Sube] ( [Sube_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_Tablolar' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_Tablolar] ON [Muh_Tablolar] ( [Tablo_Kod] )
End

if not exists(select name from sys.indexes where name ='IX_Muh_TCode_FisId' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Muh_TCode_FisId] ON [Muh_TCode] ( [T_Fis_Id] )
End

if not exists(select name from sys.indexes where name ='IX_Nakit_Hrk_Fisno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Nakit_Hrk_Fisno] ON [Nakit_Hrk] ( [Nakit_Fisno] )
End

if not exists(select name from sys.indexes where name ='IX_Nakit_Hrk_TarihKodu' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Nakit_Hrk_TarihKodu] ON [Nakit_Hrk] ( [Nakit_Tarih], [Nakit_Kodu] )
End

if not exists(select name from sys.indexes where name ='IX_Nakit_Hrk_HesapnoTarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Nakit_Hrk_HesapnoTarih] ON [Nakit_Hrk] ( [Nakit_Hesap_No], [Nakit_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_P_Users_Kodu' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_P_Users_Kodu] ON [P_Users] ( [P_Kodu] )
End

if not exists(select name from sys.indexes where name ='IX_P_Users_Kart' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_P_Users_Kart] ON [P_Users] ( [P_Kart] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Adres_SinifKod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Adres_SinifKod] ON [Pos_Adres] ( [Adres_Sinif], [Adres_Kod] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Adres_Ad' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Adres_Ad] ON [Pos_Adres] ( [Adres_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_CallerId_Carikod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_CallerId_Carikod] ON [Pos_CallerId] ( [Caller_Carikod] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_CallerId_Telno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_CallerId_Telno] ON [Pos_CallerId] ( [Caller_Telno] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_CallerId_Tarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_CallerId_Tarih] ON [Pos_CallerId] ( [Caller_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Cari_Kod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Cari_Kod] ON [Pos_Cari] ( [Cari_Kod] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Cari_Ad' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Cari_Ad] ON [Pos_Cari] ( [Cari_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Cari_Kart' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Cari_Kart] ON [Pos_Cari] ( [Cari_Kart] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Cari_Tel' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Cari_Tel] ON [Pos_Cari] ( [Cari_Tel] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Carihrk_CariTarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Carihrk_CariTarih] ON [Pos_Carihrk] ( [Chrk_Cari], [Chrk_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Fatura_Cekno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Fatura_Cekno] ON [Pos_Fatura] ( [PFat_Cekno] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Fatura_Tarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Fatura_Tarih] ON [Pos_Fatura] ( [PFat_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Fihrist_AdSoyad' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Fihrist_AdSoyad] ON [Pos_Fihrist] ( [F_Ad], [F_Soyad] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Fihrist_Tel1' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Fihrist_Tel1] ON [Pos_Fihrist] ( [F_Tel1] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Grup_AnaAraGrup' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Grup_AnaAraGrup] ON [Pos_Grup] ( [Kont_Anagrup], [Kont_Aragrup] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Ingenico_Fisno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Ingenico_Fisno] ON [Pos_Ingenico] ( [Fisno] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Ingenico_YFisno' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Ingenico_YFisno] ON [Pos_Ingenico] ( [YFisno] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Kasahrk_Tarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Kasahrk_Tarih] ON [Pos_Kasahrk] ( [Pkasa_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_KategoriFiyat_Adi' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_KategoriFiyat_Adi] ON [Pos_KategoriFiyat] ( [Kategori_Adi] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Kodlar_SinifKod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Kodlar_SinifKod] ON [Pos_Kodlar] ( [Pkod_Sinif], [Pkod_Kod] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Kodlar_Ad' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Kodlar_Ad] ON [Pos_Kodlar] ( [Pkod_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Log_FisnoIslem' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Log_FisnoIslem] ON [Pos_Log] ( [Log_FisNo], [Log_Islem] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Log_Tarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Log_Tarih] ON [Pos_Log] ( [Log_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Log_PosTarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Log_PosTarih] ON [Pos_Log] ( [Log_Pos_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Log' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Log] ON [Pos_Log] ( [Log_Id] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Masa_DepMasano' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Masa_DepMasano] ON [Pos_Masa] ( [Masa_Depart], [Masa_Konum], [Masa_No] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Masa_No' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Masa_No] ON [Pos_Masa] ( [Masa_No] )
End

if not exists(select name from sys.indexes where name ='IX_Pos_Masa_Ad' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Pos_Masa_Ad] ON [Pos_Masa] ( [Masa_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Rapor_Dizayn_Kod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Rapor_Dizayn_Kod] ON [Rapor_Dizayn] ( [Rapor_Kod] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Anlasma' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Anlasma] ON [Stok_Anlasma] ( [Anlas_Satici], [Anlas_Malzeme], [Anlas_Baslangic] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Barkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Barkod] ON [Stok_Barkod] ( [B_Barkod] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Barkod_Stokkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Barkod_Stokkod] ON [Stok_Barkod] ( [B_Stokkod] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Hrk_FisnoTipi' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Hrk_FisnoTipi] ON [Stok_Hrk] ( [Shrk_Fisno], [Shrk_Fistipi] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Hrk_TarihStokkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Hrk_TarihStokkod] ON [Stok_Hrk] ( [Shrk_Tarih], [Shrk_Stokkod] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Hrk_CarikodTarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Hrk_CarikodTarih] ON [Stok_Hrk] ( [Shrk_Cari], [Shrk_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Hrk_TarihAlanservis' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Hrk_TarihAlanservis] ON [Stok_Hrk] ( [Shrk_Tarih], [Shrk_Alanservis] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Hrk' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Hrk] ON [Stok_Hrk] ( [Shrk_Sirketkod] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Hrk_1' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Hrk_1] ON [Stok_Hrk] ( [Shrk_Recete] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Hrk_2' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Hrk_2] ON [Stok_Hrk] ( [Shrk_Satisfisno] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Kodlar_Key' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Kodlar_Key] ON [Stok_Kodlar] ( [Kodlar_Sinif], [Kodlar_Kod] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Kodlar_Ad' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Kodlar_Ad] ON [Stok_Kodlar] ( [Kodlar_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Master_Ad' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Master_Ad] ON [Stok_Master] ( [Master_Ad] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Master_Barkod1' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Master_Barkod1] ON [Stok_Master] ( [Master_Barkod1] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Master_AnaAraAltGrup' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Master_AnaAraAltGrup] ON [Stok_Master] ( [Master_Anagrup], [Master_Aragrup], [Master_Altgrup] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Sayim_TarihStokkod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Sayim_TarihStokkod] ON [Stok_Sayim] ( [Sayim_Tarih], [Sayim_Stokkod] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Sayim_BarkodTarih' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Sayim_BarkodTarih] ON [Stok_Sayim] ( [Sayim_Barkod], [Sayim_Tarih] )
End

if not exists(select name from sys.indexes where name ='IX_Stok_Sayim_TarihTipDep' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Stok_Sayim_TarihTipDep] ON [Stok_Sayim] ( [Sayim_Tarih], [Sayim_Tipi], [Sayim_Departman] )
End

if not exists(select name from sys.indexes where name ='IX_User_Dizayn' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_User_Dizayn] ON [User_Dizayn] ( [Diz_User], [Diz_Form] )
End

if not exists(select name from sys.indexes where name ='IX_User_Log_TarihUser' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_User_Log_TarihUser] ON [User_Log] ( [Log_Tarih], [Log_User] )
End

if not exists(select name from sys.indexes where name ='IX_Users_Kod' and is_primary_key=0) 
Begin
CREATE NONCLUSTERED INDEX [IX_Users_Kod] ON [Users] ( [User_Kod] )
End

CREATE NONCLUSTERED INDEX [NonClusteredIndex] ON [dbo].[Stok_Kodlar]
(
	[Kodlar_Kod] ASC,
	[Kodlar_Ad] ASC,
	[Kodlar_Sinif] ASC,
	[Kodlar_Anagrup] ASC,
	[Kodlar_Aragrup] ASC,
	[Kodlar_Sirket] ASC,
	[Kodlar_Garson] ASC,
	[Kodlar_Anadepkodu] ASC,
	[Kodlar_Altgrup] ASC,
	[Kodlar_Size] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]




CREATE NONCLUSTERED INDEX [NonClusteredIndex] ON [dbo].[Pos_Masa]
(
	[Masa_Depart] ASC,
	[Masa_No] ASC,
	[Masa_Ad] ASC,
	[Masa_Konum] ASC,
	[Masa_Durum] ASC,
	[Masa_Ozel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]




CREATE NONCLUSTERED INDEX [NonClusteredIndex] ON [dbo].[Pos_Kodlar]
(
	[Pkod_Kod] ASC,
	[Pkod_Ad] ASC,
	[Pkod_Sinif] ASC,
	[Pkod_Ozelkod] ASC,
	[Pkod_Tekoda] ASC,
	[Pkod_Odano] ASC,
	[Pkod_Urungrup] ASC,
	[Pkod_Konumkod] ASC,
	[Pkod_Ustgrup] ASC,
	[Pkod_Altgrup] ASC,
	[Pkod_Sira] ASC,
	[Pkod_Bosrenk] ASC,
	[Pkod_Dolurenk] ASC,
	[Pkod_Hesaprenk] ASC,
	[Pkod_AndroBosrenk] ASC,
	[Pkod_AndroDolurenk] ASC,
	[Pkod_AndroHesaprenk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]




CREATE NONCLUSTERED INDEX [NonClusteredIndex] ON [dbo].[Pos_Cari]
(
	[Cari_Kod] ASC,
	[Cari_Ad] ASC,
	[Cari_Soyad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]




CREATE NONCLUSTERED INDEX [NonClusteredIndex] ON [dbo].[Muh_Kodlar]
(
	[Mkodlar_Kod] ASC,
	[Mkodlar_Ad] ASC,
	[Mkodlar_Sinif] ASC,
	[Mkodlar_Anagrup] ASC,
	[Mkodlar_Doviz_Kodu] ASC,
	[Mkodlar_Ba] ASC,
	[MKodlar_P_DovizKodu] ASC,
	[MKodlar_P_DovizCins] ASC,
	[MKodlar_P_DovizTuru] ASC,
	[MKodlar_P_OtelKodu] ASC,
	[MKodlar_P_SirketKodu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]





CREATE NONCLUSTERED INDEX [NonClusteredIndex] ON [dbo].[Cst_Recete_Satis]
(
	[Rsat_Fisno] ASC,
	[Rsat_Tarih] ASC,
	[Rsat_Departman] ASC,
	[Rsat_Recete] ASC,
	[Rsat_Dovizkodu] ASC,
	[Rsat_Satistip] ASC,
	[Rsat_Kapatma] ASC,
	[Rsat_Masa] ASC,
	[Rsat_Garson] ASC,
	[Rsat_Kisi] ASC,
	[Rsat_Durum] ASC,
	[Rsat_Garson2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]




CREATE NONCLUSTERED INDEX [NonClusteredIndex] ON [dbo].[Cst_Recete]
(
	[Rec_Departman] ASC,
	[Rec_Grup] ASC,
	[Rec_Anagrup] ASC,
	[Rec_Altgrup] ASC,
	[Rec_Kodu] ASC,
	[Rec_Genelkod] ASC,
	[Rec_Ad] ASC,
	[Rec_Dovizkodu] ASC,
	[Rec_Urungrup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]



";
    }
}
