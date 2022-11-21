using Pos.Class;
using Pos.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Controllers
{
    public class EntegreMenuController
    {
        public static string MyClass = "EntegreMenuController";

        public void kaydet(entegreMenu entegreMenu)
        {
            try
            {
                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);

                var menu = db.entegreMenu.Where(x => x.recDep == Departman.Dep_Kodu
                && x.entegreId == entegreMenu.entegreId
                && x.entegreMenuMasterId == entegreMenu.entegreMenuMasterId
                ).FirstOrDefault();

                if (menu == null)
                {
                    db.entegreMenu.Add(entegreMenu);
                }
                else
                {
                    if (menu.recAd.Contains("Badem")) {
                        Console.WriteLine(); 
                    }
                    entegreMenu.id = menu.id;
                    entegreMenu.recAd = menu.recAd;
                    entegreMenu.recFiyat = menu.recFiyat;
                    entegreMenu.recId = menu.recId;
                    db.Entry(menu).CurrentValues.SetValues(entegreMenu);
                }


                db.SaveChanges();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "kaydet", "", ex);
            }
        }

        public void menuMasterIdKaydet()
        {
            try
            {
                RmosMerkez21Entities context = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                string query = @"select e1.id as kosulId, e2.id as eklenecekId from entegreMenu e1
inner join
(select id,entegreMenuMasterId from entegreMenu  
where  entegreId=entegreMenuMasterId
group by id,entegreMenuMasterId) e2 on e1.entegreMenuMasterId=e2.entegreMenuMasterId";

                DataTable dataTable = getQueryToDataTable(query, context);

                foreach (DataRow item in dataTable.Rows)
                {
                    getQueryToDataTable("update entegreMenu set masterId='" + item["eklenecekId"].ToString() + "' where id='" + item["kosulId"].ToString() + "'", context);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public DataTable getQueryToDataTable(string query, DbContext context)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = context.Database.Connection.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = query;
                    SqlDataAdapter da = new SqlDataAdapter((SqlCommand)cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;

        }
        public entegreMenu listele()
        {
            try
            {
                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                return db.entegreMenu.Where(x => x.recDep == Departman.Dep_Kodu).FirstOrDefault();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "listele", "", ex);
            }

            return null;
        }


    }
}
