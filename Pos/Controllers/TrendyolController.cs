using Pos.Class;
using Pos.Trendyol;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Controllers
{
    public class TrendyolController
    {
        public static string MyClass = "TrendyolController";
        TrendyolApi trendyolApi = new TrendyolApi();

        string siparisId = "", fisno = "";
        public TrendyolController(string siparisId, string fisno)
        {
            this.siparisId = siparisId;
            this.fisno = fisno;
        }

        public void hazirlandi()
        {
            try
            {
                GenelModel model = trendyolApi.siparisEndOfOrder2(siparisId);

                if (model.success == false)
                {
                    RHMesaj.MyMessageInformation("Bir hata oldu\n" + model.mesaj);
                }
                else
                {
                    dbtools.execcmdR("update Cst_Recete_Satis set Rsat_EntegreDurumKod='" + RestoranTip.hazirlandiKod + "' where Rsat_EntegreId='" + siparisId + "'");
                    RHMesaj.MyMessageInformation("Sipariş Hazırlandı.");
                }


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "hazirlandi", "", ex);
            }
        }

        public void yolaCikti()
        {
            try
            {
                GenelModel model = trendyolApi.siparisOrderOnWay3(siparisId);

                if (model.success == false)
                {
                    RHMesaj.MyMessageInformation("Bir hata oldu\n" + model.mesaj);
                }
                else
                {
                    dbtools.execcmdR("update Cst_Recete_Satis set Rsat_EntegreDurumKod='" + RestoranTip.yolaCiktiKod + "' where Rsat_EntegreId='" + siparisId + "'");

                    RHMesaj.MyMessageInformation("Sipariş Yola Çıktı.");
                }


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "yolaCikti", "", ex);
            }
        }

        public void teslimEdildi()
        {
            try
            {
                GenelModel model = trendyolApi.siparisOrderDelivery4(siparisId);

                if (model.success == false)
                {
                    RHMesaj.MyMessageInformation("Bir hata oldu " + model.mesaj);

                    if (model.mesaj.Contains("Sadece restoran kuryesi tarafından taşınan paketlerin statüsü manuel olarak güncellenebilir."))
                    {
                        trendyolHesapKapat();
                    }
                }
                else
                {
                 
                    trendyolHesapKapat();
                }


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnTrendyolSiparisTeslimEdildi_Click", "", ex);
            }
        }

        

        private void trendyolHesapKapat()
        {
            try
            {
                dbtools.execcmdR("update Cst_Recete_Satis set Rsat_EntegreDurumKod='" + RestoranTip.teslimEdildiKod + "' where Rsat_EntegreId='" + siparisId + "'");


                string KapatmaHesap = dbtools.DegerGetir("select top 1 recOdemeKod  from entegreOdemeTip where tip='2' and entegreOdemeKod='859'");


                string Masa_No = Convert.ToString(dbtools.DegerGetir("select Rsat_Masa From Cst_Recete_Satis Where Rsat_Fisno = '" + fisno + "' Group by Rsat_Masa"));
                Hesap hes = new Hesap();
                hes.Tag = fisno;
                hes.fisno = Convert.ToInt32(fisno);
                hes.Masa_No = Masa_No;

                DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");

                if (dt == null || dt.Rows.Count < 1)
                {
                    dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43");
                }

                hes.look_Kapatma.Properties.DataSource = dt;
                hes.look_Kapatma.Properties.DisplayMember = "Pkod_Ad";
                hes.look_Kapatma.Properties.ValueMember = "Pkod_Kod";

                hes.look_Kapatma.EditValue = KapatmaHesap == "" ? null : KapatmaHesap;
                hes.Split = 0;
                hes.Splitad = "";

                //hes.Visible = true;
                hes.otoYazdirmadanKapat = true;
                hes.Show();

                RHMesaj.MyMessageInformation("Sipariş Teslim Edildi.");

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "trendyolHesapKapat", "", ex);
            }
        }

    }
}
