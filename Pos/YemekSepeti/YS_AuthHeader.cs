using Pos.Class;
using Pos.IntegrationWebService1;
using System;
using System.Data;

namespace Pos.YemekSepeti
{

    public class YS_AuthHeader
    {
                
        public static AuthHeader ah = new AuthHeader();

        public static string UserCode { get; set; }

        public static string UserPassword { get; set; }

        public static string CatalogName { get; set; }

        public static string CatagoryName { get; set; }

        public static string DisplayName { get; set; }

        public static string ServiceTime { get; set; }

        public static void YS_Yukle(string Dep_kodu)
        {
            DataTable dt = new DataTable();

            dt = dbtools.SelectTable("Select ISNULL(Kodlar_YS_User,'') as Kodlar_YS_User, ISNULL(Kodlar_YS_Pass,'') as Kodlar_YS_Pass, ISNULL(Kodlar_Sirket,'001') as Kodlar_Sirket From Stok_Kodlar Where Kodlar_Sinif = 1 and Kodlar_YS_Aktif = 1 and Kodlar_Kod = '" + Dep_kodu + "'");
            if (dt.Rows.Count > 0)
            {
                UserCode = Convert.ToString(dt.Rows[0]["Kodlar_YS_User"]);
                UserPassword = Convert.ToString(dt.Rows[0]["Kodlar_YS_Pass"]);

                ah.UserName = UserCode;
                ah.Password = UserPassword;

                YS_RestoInfo.RestorantYukle(Dep_kodu, Convert.ToString(dt.Rows[0]["Kodlar_Sirket"]));

            }

        }
    }
}
