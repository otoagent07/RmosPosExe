using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Class
{
    public class Sabitler
    {
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

            }
           

            return dt;
        }
    }
}
