using Pos.Class;
using Pos.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Controllers
{
    public class SatisController
    {
        public static string MyClass = "SatisController";
        public void kaydet(DataRow seciliSiparis, string masaNo)
        {
            try
            {
                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);

                string siparisId = seciliSiparis["siparisId"].ToString();

                var varmi = db.Cst_Recete_Satis.Where(x => x.Rsat_EntegreId == siparisId).ToList();
                if (varmi.Count > 0)
                {
                    RHMesaj.alertMesaj("Satış tablosunda zaten var. ama onay gönderildi!");
                    int fisnom = Convert.ToInt32(varmi.FirstOrDefault().Rsat_Fisno);
                    dbtools.execcmdR("update entegreSiparis set fisno='" + fisnom + "' where siparisId='" + siparisId + "'");
                    siparisGonder(fisnom);
                    return;
                }

                string cariId = seciliSiparis["cariId"].ToString();
                string cariKod = dbtools.DegerGetir("select top 1 Cari_Kod from Pos_Cari where Cari_TrendyolId='" + cariId + "'");
                string cariTip = dbtools.DegerGetir("select top 1 Cari_Tip from Pos_Cari where Cari_TrendyolId='" + cariId + "'");



                Cst_Recete_Satis satis = new Cst_Recete_Satis();
                int fisno = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));

                var siparis = db.entegreSiparis.Where(x => x.siparisId == siparisId).FirstOrDefault();
                var urunler = db.entegreSiparisUrunler.Where(x => x.siparisId == siparisId).ToList();
                Recete_Islem rec = new Recete_Islem();

                foreach (var urun in urunler)
                {
                    string Pkod_OnburoKod = dbtools.DegerGetir(@"select top 1 Pkod_OnburoKod FROM Cst_Recete 
left join Pos_Kodlar  on Pkod_Urungrup = Rec_Urungrup and Pkod_Sinif = '12'  and Pkod_Kod =" + Departman.Dep_Kodu + @"
where Rec_Genelkod='" + urun.recId + @"'");

                    satis = new Cst_Recete_Satis();
                    satis.Rsat_Fisno = fisno;
                    satis.Rsat_Tarih = Param.Tarih;
                    satis.Rsat_EntegreAd = urun.entegreMenuAd;
                    satis.Rsat_EntegreToplamFiyat = siparis.siparisTutar;
                    satis.Rsat_Departman = Departman.Dep_Kodu;
                    satis.Rsat_Recete = urun.recId;
                    satis.Rsat_Kdvoran = rec.Kdv_Bul(urun.recId);
                    satis.Rsat_Miktar = urun.adet;
                    satis.Rsat_Fiyat = urun.fiyat * urun.adet;
                    satis.Rsat_Doviztutar = urun.fiyat * urun.adet;
                    satis.Rsat_Tutar = urun.fiyat * urun.adet;
                    satis.Rsat_Dovizkodu = Param.Doviz_Kodu;
                    satis.Rsat_Net = satis.Rsat_Tutar / (1 + (satis.Rsat_Kdvoran / 100));
                    satis.Rsat_Kdv = satis.Rsat_Tutar - (satis.Rsat_Tutar / (1 + (satis.Rsat_Kdvoran / 100)));
                    satis.Rsat_Odano = "";
                    satis.Rsat_Folio = 0;
                    satis.Rsat_Masa = masaNo;
                    satis.Rsat_Garson = User.P_Kod;
                    satis.Rsat_Kisi = 1;
                    satis.Rsat_Cari = cariKod;
                    satis.Rsat_Split = 0;
                    string aciklama= siparis.siparisNot == null ? "" : siparis.siparisNot; 
                    satis.Rsat_Aciklama = "";// 
                    string notumuz = aciklama == "" ? "" : "(Sipariş Notu : " + aciklama + ")\n";
                    satis.Rsat_Not = notumuz + "(Ödeme : Online Odeme)";
                    satis.Rsat_Paketci = "";
                    satis.Rsat_Emiktar = "T";
                    satis.Rsat_Garson2 = User.P_Kod;
                    satis.Rsat_Uye_Kart_Turu = "";
                    satis.Rsat_Pansiyon = "";
                    satis.Rsat_MusTipi = cariTip;
                    satis.Rsat_Uye_Id = 0;
                    satis.Rsat_Uye_Ad = siparis.ad + " " + siparis.soyad;
                    satis.Rsat_Indkodu = "";
                    satis.Rsat_Indoran = 0;
                    satis.Rsat_Onbdep = Pkod_OnburoKod;
                    satis.Rsat_Dovizkur = Param.Doviz_Kuru;
                    satis.Rsat_Pda = false;
                    satis.Rsat_Splitad = "";
                    satis.Rsat_SiparisPr = false;
                    satis.Rsat_Yapma = false;
                    satis.Rsat_AbuyerPr = false;
                    satis.Rsat_AbuyerPr2 = false;
                    satis.Rsat_AbuyerPr3 = false;
                    satis.Rsat_AbuyerPr4 = false;
                    satis.Rsat_AdisyonPr = false;

                    satis.Rsat_Sube = "";
                    satis.Rsat_OzelMasaAdi = masaNo;
                    satis.Rsat_Satistip = "S";
                    satis.Rsat_Maliyet = Convert.ToDecimal(dbtools.DegerGetir("select top 1 isnull(SUM(Detay_Maliyet),0)*" + urun.adet + " AS Maliyet from Cst_Recete_Detay where Detay_Recete='" + urun.recId + "'"));

                    satis.Rsat_Odenmez = "";
                    satis.Rsat_Ba = "B";
                    satis.Rsat_Acilis = DateTime.Now.TimeOfDay;
                    satis.Rsat_Durum = "A";
                    satis.Rsat_Happy_Hour = false;
                    satis.Rsat_Satissaat = satis.Rsat_Acilis;
                    satis.Rsat_Kart_ID = 0;
                    satis.Rsat_Ingenico_Status = 0;
                    satis.Rsat_EntegreId = siparisId;
                    satis.Rsat_EntegreDurumKod = RestoranTip.hazirlaniyorKod;

                    db.Cst_Recete_Satis.Add(satis);
                    db.SaveChanges();
                }

                dbtools.execcmdR("update entegreSiparis set fisno='"+ fisno + "' where siparisId='"+siparisId+"'");
                siparisGonder(fisno);


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "kaydet", "", ex);
            }
        }


        private void siparisGonder(int Fis)
        {
            try
            {
                FisPr pr = new FisPr();


                string sonucSiparis = pr.newSiparisPr(Fis, false, 0);

                string sonucPaket = pr.PaketPrTrendyol(Fis, " * * * TRENDYOL PAKET FİSİ * * * ");
                
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr = 1 where Rsat_Fisno = '" + Fis + "' ");
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "siparisGonder", "", ex);
            }
        }
    }
}
