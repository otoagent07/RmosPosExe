using Pos.Class;
using Pos.Entities;
using Pos.Trendyol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Controllers
{
    public class EntegreAyarlarController
    {
        public static string MyClass = "EntegreAyarlarController";
        public void kaydet(entegreAyarlar entegreAyarlar)
        {
            try
            {
                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                var ayarlar = db.entegreAyarlar.Where(x=>x.recDep == Departman.Dep_Kodu).FirstOrDefault();
                if (ayarlar == null)
                {
                    db.entegreAyarlar.Add(entegreAyarlar);
                }
                else
                {
                    entegreAyarlar.id = ayarlar.id;
                    db.Entry(ayarlar).CurrentValues.SetValues(entegreAyarlar);
                }
                

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "kaydet", "", ex);
            }
        }

        public entegreAyarlar listele()
        {
            try
            {
                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                return db.entegreAyarlar.Where(x => x.recDep == Departman.Dep_Kodu).FirstOrDefault();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "listele", "", ex);
            }

            return null;
        }


       
    }
}
