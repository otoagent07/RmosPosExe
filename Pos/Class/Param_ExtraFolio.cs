using System;
using System.Data;

namespace Pos.Class
{
    public static class Param_ExtraFolio
    {
        public static string Front_Acenta { get; set; }
        public static string Front_DovizSekli { get; set; }
        public static string Front_OdemeSekli { get; set; }
        public static string Front_KurKodu { get; set; }
        public static string Front_Konaklama { get; set; }
        public static string Front_MusteriTipi { get; set; }
        public static string Front_Departman { get; set; }
        public static string Front_Iade { get; set; }
        public static string Front_KartF_Onburo { get; set; }
        public static string Front_KartF_Odano { get; set; }
        public static string Front_Kapatma { get; set; }
        public static string Front_IadeKK { get; set; }
        public static string Front_GelirIade { get; set; }
        public static string HizmetReceteKod { get; set; }
        public static string GelirReceteKod { get; set; }
        public static string HizmetReceteKodCocuk { get; set; }
        public static string hizmetOdemeKod { get; set; }
        public static bool hizmetBedeliAktif { get; set; }
        public static void Param_ExtraFolioYukle()
        {
            DataTable dt = dbtools.SelectTable(@"Select
                  ISNULL([Front_Acenta],'') as [Front_Acenta]
                  ,ISNULL([Front_DovizSekli],'') as [Front_DovizSekli]
                  ,ISNULL([Front_OdemeSekli],'') as [Front_OdemeSekli]
                  ,ISNULL([Front_KurKodu],'') as [Front_KurKodu]
                  ,ISNULL([Front_Konaklama],'') as [Front_Konaklama]
                  ,ISNULL([Front_MusteriTipi],'') as [Front_MusteriTipi]
                  ,ISNULL([Front_Departman],'') as [Front_Departman]
                    ,ISNULL([Front_Iade],'') as [Front_Iade]
                    ,ISNULL([Front_KartF_Onburo],'O') as [Front_KartF_Onburo]
                    ,ISNULL([Front_KartF_Odano],'') as [Front_KartF_Odano]
                    ,ISNULL([Front_Kapatma],'') as [Front_Kapatma]
                    ,ISNULL([Front_IadeKK],'') as [Front_IadeKK]
                    ,ISNULL(Front_GelirIade,'') as Front_GelirIade
                    ,ISNULL(HizmetReceteKod,'') as HizmetReceteKod
                    ,ISNULL(GelirReceteKod,'') as GelirReceteKod
                    ,ISNULL(HizmetReceteKodCocuk,'') as HizmetReceteKodCocuk
                    ,ISNULL(hizmetOdemeKod,'') as hizmetOdemeKod
                    ,ISNULL(hizmetBedeliAktif,'0') as hizmetBedeliAktif
                    FROM [dbo].[Pos_FolioParam] Where Front_Departman = '" + Departman.Dep_Kodu + "'");

            if (dt.Rows.Count > 0)
            {
                Front_Acenta = Convert.ToString(dt.Rows[0]["Front_Acenta"]);
                Front_DovizSekli = Convert.ToString(dt.Rows[0]["Front_DovizSekli"]);
                Front_OdemeSekli = Convert.ToString(dt.Rows[0]["Front_OdemeSekli"]);
                Front_KurKodu = Convert.ToString(dt.Rows[0]["Front_KurKodu"]);
                Front_Konaklama = Convert.ToString(dt.Rows[0]["Front_Konaklama"]);
                Front_MusteriTipi = Convert.ToString(dt.Rows[0]["Front_MusteriTipi"]);
                Front_Departman = Convert.ToString(dt.Rows[0]["Front_Departman"]);
                Front_Iade = Convert.ToString(dt.Rows[0]["Front_Iade"]);
                Front_KartF_Onburo = Convert.ToString(dt.Rows[0]["Front_KartF_Onburo"]);
                Front_KartF_Odano = Convert.ToString(dt.Rows[0]["Front_KartF_Odano"]);
                Front_Kapatma = Convert.ToString(dt.Rows[0]["Front_Kapatma"]);
                Front_IadeKK = Convert.ToString(dt.Rows[0]["Front_IadeKK"]);
                Front_GelirIade= Convert.ToString(dt.Rows[0]["Front_GelirIade"]);
                HizmetReceteKod = Convert.ToString(dt.Rows[0]["HizmetReceteKod"]);
                GelirReceteKod = Convert.ToString(dt.Rows[0]["GelirReceteKod"]);
                HizmetReceteKodCocuk = Convert.ToString(dt.Rows[0]["HizmetReceteKodCocuk"]);
                hizmetOdemeKod = Convert.ToString(dt.Rows[0]["hizmetOdemeKod"]);
                hizmetBedeliAktif =Convert.ToBoolean( Convert.ToString(dt.Rows[0]["hizmetBedeliAktif"]));
            }
        }
    }
}
