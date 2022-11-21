using Pos.Class;
using Pos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Controllers
{
    public class OdemeTipController
    {
        public static string MyClass = "OdemeTipController";
        public void kaydet()
        {
            try
            {
                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                var cari = db.entegreOdemeTip.Where(x => x.tip == 2 && x.recOdemeKod == "859").FirstOrDefault();
                if (cari == null)
                {
                    cari = new entegreOdemeTip();
                    cari.tip = 2;
                    cari.entegreOdemeType = "859";
                    cari.entegreOdemeKod = "859";
                    cari.entegreOdemeAd = "Online Odeme";
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "kaydet", "", ex);
            }
        }

    }
}
