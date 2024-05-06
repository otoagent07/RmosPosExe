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
        public static void odenmezVeyaIkramiseServisPayiSil(string fisno,string odemeKod)
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
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sabitler.odenmezVeyaIkramiseServisPayiSil()-> " + ex.Message);

            }
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
    }
}
