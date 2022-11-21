using Pos.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos.YemekSepeti
{
    public static class YS_RestoInfo
    {
        public static string YS_Sirket_Kodu { get; set; }
        public static string YS_Dep_Kodu { get; set; }
        public static string YS_CatalogName { get; set; }
        public static string YS_CatagoryName { get; set; }
        public static string YS_DisplayName { get; set; }
        public static int YS_ServiceTime { get; set; }
        public static string YS_Speed { get; set; }
        public static string YS_Serving { get; set; }
        public static string YS_Flavour { get; set; }
        public static bool RestorantYukle(string depKodu, string sirketKodu)
        {
            DataTable dt = dbtools.SelectTable("Select * From YS_Restaurant Where YS_Dep_Kodu = '" + depKodu + "'");
            if (dt.Rows.Count > 0)
            {
                YS_CatalogName = Convert.ToString(dt.Rows[0]["YS_CatalogName"]);
                YS_CatagoryName = Convert.ToString(dt.Rows[0]["YS_CatagoryName"]);
                YS_DisplayName = Convert.ToString(dt.Rows[0]["YS_DisplayName"]);
                YS_ServiceTime = Convert.ToInt32(dt.Rows[0]["YS_ServiceTime"]);
                YS_Speed = Convert.ToString(dt.Rows[0]["YS_Speed"]);
                YS_Serving = Convert.ToString(dt.Rows[0]["YS_Serving"]);
                YS_Flavour = Convert.ToString(dt.Rows[0]["YS_Flavour"]);

                return true;
            }
            else
            {
                MessageBox.Show("Kayıtlı Restoran Bilgileri Bulunamadı..", "", MessageBoxButtons.OK);
                return false;
            }
        }

        
    }
}
