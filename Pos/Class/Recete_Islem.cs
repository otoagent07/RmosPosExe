using System;
using System.Data;

namespace Pos.Class
{
    public class Recete_Islem
    {
        public decimal Kdv_Bul(string Recete_Kodu)
        {
            DataTable dtDep = dbtools.SelectTable("select ISNULL(Kodlar_Kdv_Aktif,0) as Kodlar_Kdv_Aktif,ISNULL(Kodlar_Kdv,0) as Kodlar_Kdv from Stok_Kodlar where Kodlar_Sinif = '01' and Kodlar_Kod = '" + Departman.Dep_Kodu + "'");
            if (Convert.ToBoolean(dtDep.Rows[0]["Kodlar_Kdv_Aktif"]))
            {
                return Convert.ToDecimal(dtDep.Rows[0]["Kodlar_Kdv"]);
            }
            else
            {
                DataTable dtUrunGrup = dbtools.SelectTable("select ISNULL(Kodlar_Kdv_Aktif,0) as Kodlar_Kdv_Aktif,ISNULL(Kodlar_Kdv,0) as Kodlar_Kdv,Rec_Kdv from Cst_Recete left join Stok_Kodlar on Rec_Urungrup = Kodlar_Kod and Kodlar_Sinif = '10' where Rec_Genelkod = '" + Recete_Kodu + "'");
                if (Convert.ToBoolean(dtUrunGrup.Rows[0]["Kodlar_Kdv_Aktif"]))
                {
                    return Convert.ToDecimal(dtUrunGrup.Rows[0]["Kodlar_Kdv"]);
                }
                else
                {
                    return Convert.ToDecimal(dtUrunGrup.Rows[0]["Rec_Kdv"]);
                }
            }
        }
    }
}
