using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Pos.Class;
using Pos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos.Controllers
{
    public class StatikModel
    {

        public static void wait_loadingAc(Form form1)
        {
            try
            {
                if (SplashScreenManager.Default == null || !SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.ShowForm(form1, typeof(WaitForm1), true, true, false);
                }
            }
            catch (Exception ex)
            {

            }

        }

        public static void wait_loadingAc()
        {
            try
            {
                if (SplashScreenManager.Default == null || !SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.ShowForm(
          splashFormType: typeof(WaitFormRmos),
          useFadeIn: false,
          useFadeOut: false);
                }
            }
            catch (Exception ex)
            {

            }

        }

        public static void wait_loadingKapat()
        {
            try
            {
                SplashScreenManager.CloseForm(false);

            }
            catch (Exception ex)
            {

            }
        }


        public static decimal getOdaGirisKur(string odano, decimal kur)
        {
            try
            {
                string onburoGirisKurdanmiAlinsi = Fronttools.DegerGetir("select top 1 Fis_Hesapgir_kur_e_gk from Fishrk"); // K İSE GİRİŞTEN G ise günlük

                if (onburoGirisKurdanmiAlinsi.Equals("K"))
                {
                    string girisKur = Fronttools.DegerGetir("select top 1 Rez_Kur_uygulanan from rez where Rez_Odano='" + odano + "' and Rez_R_I_H='I'"); // K İSE GİRİŞTEN G ise günlük

                    return Convert.ToDecimal(girisKur);

                }
            }
            catch (Exception ex)
            {

            }
            return kur;
        }

        public static string getPaketSqlText(DateEdit dateTarih1, DateEdit dateTarih2, string depkod, string Sube, string Durum, bool sadeceAcikmasagozuksun = false, bool sistemTar = false)
        {
            //SUM(Rsat_Tutar) -

            string acikMasa = "";
            if (sadeceAcikmasagozuksun)
            {
                acikMasa = " and Rsat_Durum='A'";
            }

            string query = @" Select *
Into #Cst_Recete_Satis
From Cst_Recete_Satis
where 1 = 1
	AND (Rsat_Durum in (SELECT fieldvalue FROM dbo.stringArray('" + Durum + @"',',')))
	and Convert(date,Rsat_Tarih) >= '" + dateTarih1.DateTime.Date.ToString("yyyy-MM-dd") + @"' 
	and Convert(date,Rsat_Tarih) <= '" + dateTarih2.DateTime.Date.ToString("yyyy-MM-dd") + @"'
	and Rsat_Departman = '" + depkod + @"' " + Sube + @"
    and Rsat_Indkodu is not null -- Rsat_Indkodu NULL ve Rsat_Durum ='A' ise bu kapatmadır
        select distinct
            case when MIN(Rsat_Durum) = 'A' then 'Acik' else 'Kapali' end as Rsat_Durum,
            MAX(Rsat_Id) as Rsat_Id,
            MAX(Rsat_GetirOrderID) as entegreOdemeTip,
            Rsat_Tarih  ,
            Masa_No,
            Masa_Ad,
            Cst_Recete_Satis.Rsat_Fisno,
            Cari_Kod,(Cari_Ad + ' ' + Cari_Soyad) as Cari_AdSoyad,

           SUM(Rsat_Tutar) -  (Select ISNULL(SUM(Rsat_Tutar),0) From #Cst_Recete_Satis r Where Rsat_Ba = 'A' AND r.Rsat_Fisno = Cst_Recete_Satis.Rsat_Fisno) as Tutar,

            P_Kod,P_Ad + ' ' + P_Soyad as P_AdSoyad ,Rsat_Sube,sube.Pkod_Ad as subeAd,Rsat_SubeDurum,
            MAX(ISNULL(Cari_Adres1,'')) + ' ' + MAX(ISNULL(Cari_Adres2,'')) + ' ' + MAX(ISNULL(Cari_Adres3,'')) as Cari_Adres, Cari_Tip as Cari_Tip,
            case  Cari_Tip when 'Y' then 'YEMEK SEPETİ' when 'G' Then 'GETİR YEMEK'  when 'T' Then 'TRENDYOL' else 'PAKET' end as YSDurum,
            case  Cari_Tip when 'Y' then (case MIN(ISNULL(Rsat_YSDurum,'')) when 'Accepted' then 'HAZIRLANIYOR' When 'OnDelivery' then 'SİPARİŞ YOLDA' when 'Delivered' THEN 'TESLİM EDİLDİ' When 'Cancelled' then 'İPTAL EDİLDİ' else '' end) When 'G' Then 
            (Case MAX(ISNULL(Rsat_GetirDurum,''))
            When 325 then 'Ön Onay Bekliyor..İleri Tarihli Sipariş'
            when 350 then 'İleri tarihli sipariş, ön onay alındı'
            When 400 then 'Onay Bekleniyor'
            When 500 then 'Hazırlanıyor'
            When 550 then 'Sipariş hazırlandı'
            When 600 then 'Sipariş kuryeye teslim edildi'
            When 700 then 'Kurye yola çıktı'
            When 900 then 'Sipariş teslim edildi'
            When 1500 then 'Sipariş admin tarafından iptal edildi'
            When 1600 then 'Sipariş restoran tarafından iptal edildi' else 'Tanımlı Değil' end)
When 'T' Then 
            (Case MAX(ISNULL(Rsat_EntegreDurumKod,'0'))
            When 0 then 'Onay Bekliyor'
            when 1 then 'Hazırlanıyor'
            When 2 then 'Hazırlandı'
            When 3 then  'Yola Çıktı' 
            When 4 then  'Teslim Edildi'
            When 5 then 'İptal Edildi'
			else 'Tanımlı Değil' end) 

end as Rsat_YSDurum,
            case Cari_Tip when 'Y' then MIN(ISNULL(Rsat_YSOrderID,'')) when 'G' then MIN(ISNULL(Rsat_GetirOrderID,'')) else  '' end  as Rsat_YSOrderID,
            Max(Rsat_Not) as Rsat_Not,MIN(Rsat_Acilis) as Rsat_Acilis,Rsat_EntegreId,
            case when ISNULL(GOrder_deliveryType,2) = 1 then 'GETİR KURYESİ' else 'RESTORAN KURYESİ' end as Kurye,
            GOrder_deliveryType as GOrder_deliveryType,
            GOrder_confirmationId,GetirYemek_Order.ID
            from #Cst_Recete_Satis Cst_Recete_Satis 
            left join Pos_Masa on Rsat_Masa = Masa_No  and Masa_Depart = Rsat_Departman
            left join Pos_Cari on Rsat_Cari = Cari_Kod 
            left join Rmosmuh.dbo.Pos_User on P_Kod = Rsat_Paketci 
            left join Pos_Kodlar as sube on sube.Pkod_Kod = Rsat_Sube and sube.Pkod_Sinif = '27'
            Left Join GetirYemek_Order on GOrder_id = Rsat_GetirOrderID
            where (Rsat_Durum in (SELECT fieldvalue FROM dbo.stringArray('" + Durum + @"',',')))
            and Rsat_Ba = 'B' and Masa_Konum = 'P' " + acikMasa + @"
            group by Masa_No,Rsat_Tarih,Masa_Ad,Cst_Recete_Satis.Rsat_Fisno,Cari_Kod,Cari_Ad,
            Cari_Soyad,Cari_Adres1,P_Kod,P_Ad,P_Soyad ,Rsat_Sube,sube.Pkod_Ad ,Rsat_SubeDurum,Cari_Tip,GOrder_deliveryType,GOrder_confirmationId,GetirYemek_Order.ID,Rsat_EntegreId
            order by Cst_Recete_Satis.Rsat_Fisno desc 
            DROP TABLE #Cst_Recete_Satis ";

            if (sistemTar)
            {
                query= query.Replace(",Rsat_Tarih", ",Rsat_AcilisTar");
                query= query.Replace("Rsat_Tarih  ,", "Rsat_AcilisTar as Rsat_Tarih,");

            }
            return query;
        }
    }
}
