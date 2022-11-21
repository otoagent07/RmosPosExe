using Pos.Class;
using Pos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Controllers
{
    public class CariController
    {
        public static string MyClass = "CariController";
        public void kaydet(Pos_Cari posCari)
        {
            try
            {
                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                var cari = db.Pos_Cari.Where(x => x.Cari_TrendyolId == posCari.Cari_TrendyolId).FirstOrDefault();
                if (cari == null)
                {
                    db.Pos_Cari.Add(posCari);
                    db.SaveChanges();
                    posCari.Cari_Kod = posCari.Cari_Id.ToString();
                }
                else
                {
                    posCari.Cari_Id = cari.Cari_Id;
                    posCari.Cari_Kod = cari.Cari_Id + "";
                    db.Entry(cari).CurrentValues.SetValues(posCari);
                }

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "entegreAyarlarKaydet", "", ex);
            }
        }

        public void kaydetBySiparisId(string siparisId, string cariId)
        {
            try
            {
                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                var siparis = db.entegreSiparis.Where(x => x.siparisId == siparisId && x.cariId == cariId).FirstOrDefault();
                if (siparis != null)
                {
                    Pos_Cari cari = new Pos_Cari();
                    cari.Cari_Ad = siparis.ad;
                    cari.Cari_Soyad = siparis.soyad;
                    cari.Cari_Tel = siparis.tel;
                    cari.Cari_Tel2 = siparis.tel2;
                    cari.Cari_TrendyolId = Convert.ToInt32(siparis.cariId);
                    cari.Cari_Tip = "T";
                    cari.Cari_Mail = siparis.mail;
                    cari.Cari_Mahalle = siparis.mahalle;
                    cari.Cari_Il = siparis.il;
                    cari.Cari_Ilce = siparis.ilce;
                    cari.Cari_Adres1 = siparis.adres;
                    cari.Cari_Aktif = true;
                    kaydet(cari);
                }

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "entegreAyarlarKaydet", "", ex);
            }
        }
    }
}
