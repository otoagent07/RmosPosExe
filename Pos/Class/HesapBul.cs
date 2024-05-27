using System;
using System.Data;

namespace Pos.Class
{
    public class HesapBul
    {

        public string data { get; set; }


        public string Mus_tipi { get; set; }
        public string Oda_No { get; set; }
        public int Folio { get; set; }
        public int Master_Folio { get; set; }
        public string Pansiyon { get; set; }
        public string Odeme_Kodu { get; set; }
        public int Uye_Id { get; set; }
        public string Uye_Adsoyad { get; set; }
        public string Uye_Kartturu { get; set; }
        public string Cari_Kod { get; set; }

        public string Ind_Kodu { get; set; }
        public decimal Ind_Oran { get; set; }

        public string CariAdi { get; set; }

        public string Kart_No { get; set; }
        public string Bilgi { get; set; }

        //public string MusteriTipi { get; set; }


        public string Hesap_Bul(int Fisno)
        {
            string returnValue = String.Empty;
            DataTable dt = dbtools.SelectTable("select top 1 isnull(Rsat_Mustipi,'') as Rsat_Mustipi,Rsat_Folio,Rsat_Odano,Rsat_Pansiyon,isnull(Rsat_Cari,'') as Rsat_Cari, "
                        + " isnull(Rsat_Uye_Ad,'') as Rsat_Uye_Ad,Rsat_Uye_Id,Rsat_Uye_Kart_Turu,Rsat_Indkodu,Rsat_Indoran from Cst_Recete_Satis with(nolock) where Rsat_Fisno = '" + Fisno.ToString() + "' and Rsat_Ba = 'B' ");

            if (dt.Rows.Count > 0)
            {
                Mus_tipi = Convert.ToString(dt.Rows[0]["Rsat_Mustipi"]);
                if (Mus_tipi == "O" &&  Convert.ToInt32(Convert.ToString(dt.Rows[0]["Rsat_Folio"])) != 0)
                {
                    Oda_No = Convert.ToString(dt.Rows[0]["Rsat_Odano"]);
                    Folio = Convert.ToInt32(Convert.ToString(dt.Rows[0]["Rsat_Folio"]));
                    Master_Folio = Fronttools.MasterFolioBul(Folio);
                    Pansiyon = Convert.ToString(dt.Rows[0]["Rsat_Pansiyon"]);
                    
                    DataTable dt_Folio = Fronttools.SelectTable("select isnull(convert(nvarchar,Rez_Odano),'') + ' ' + isnull(Rez_Adi_1,'') +' '+ isnull(Rez_Adi_2,'') as Adsoyad,Rez_Odeme from Rez with(nolock) where Rez_Id = '" + Folio.ToString() + "'");
                    Odeme_Kodu = Convert.ToString(dt_Folio.Rows[0]["Rez_Odeme"]);

                    Ind_Kodu = Convert.ToString(dt.Rows[0]["Rsat_Indkodu"]);
                    Ind_Oran = Convert.ToDecimal(dt.Rows[0]["Rsat_Indoran"]);

                    Bilgi = Oda_No + "  " + Convert.ToString(dt_Folio.Rows[0]["Adsoyad"]);
                }
                else if (Mus_tipi == "C" || Mus_tipi == "Y")
                {
                    Cari_Kod = Convert.ToString(dt.Rows[0]["Rsat_Cari"]);

                    Bilgi = "Cari : " + Convert.ToString(dt.Rows[0]["Rsat_Cari"]);

                    CariAdi = dbtools.DegerGetir("Select Cari_Ad + ' ' + Cari_Soyad From Pos_Cari Where Cari_Kod = '" + Cari_Kod + "'");
                }
                else if (Mus_tipi == "U")
                {
                    Oda_No = Convert.ToString(dt.Rows[0]["Rsat_Odano"]);
                    Folio = Convert.ToInt32(Convert.ToString(dt.Rows[0]["Rsat_Folio"]));
                    Pansiyon = Convert.ToString(dt.Rows[0]["Rsat_Pansiyon"]);

                    Uye_Id = Convert.ToInt32(dt.Rows[0]["Rsat_Uye_Id"]);
                    Uye_Adsoyad = Convert.ToString(dt.Rows[0]["Rsat_Uye_Ad"]);
                    Uye_Kartturu = Convert.ToString(dt.Rows[0]["Rsat_Uye_Kart_Turu"]);

                    Ind_Kodu = Convert.ToString(dt.Rows[0]["Rsat_Indkodu"]);
                    Ind_Oran = Convert.ToDecimal(dt.Rows[0]["Rsat_Indoran"]);

                    Bilgi = "Uye : " + Convert.ToString(dt.Rows[0]["Rsat_Uye_Ad"]);
                }
                else
                {
                    returnValue = "HATA";
                }

                return "OK";
            }

            return "HATA";
        }
        

        public string Arama_Yap()
        {
            if (data.Length > 0)
            {
                // Oda Araması
                if (Oda_Ara())
                {
                    return "OK";
                }
                if (Uye_Ara())
                {
                    return "OK";
                }
            }
            return "Hesap Bulunamadı...";
        }

        private bool Oda_Ara()
        {
            string Rez_Kartno = "Rez_Kartno";
            string dataKart = data;

            string[] ozelKarakter = Param.Param_Kart_Yoksay.Split('#');
            if (ozelKarakter.Length > 0 && Param.Param_Kart_Yoksay != "")
            {
                for (int i = 0; i < ozelKarakter.Length; i++)
                {
                    Rez_Kartno = "REPLACE(" + Rez_Kartno + ",'" + ozelKarakter[i] + "','')";
                    dataKart = dataKart.Replace(ozelKarakter[i], "");
                }
            }

            string filtre = String.Empty;
            if (Departman.Sorgu_Sekli == 0)     // Oda No ile Arama
            {
                filtre = " and Rez_Odano like '" + data + "%' ";
            }
            if (Departman.Sorgu_Sekli == 1)     // Kart No ile Arama
            {
                filtre = " and (" + Rez_Kartno + " like '" + dataKart + "%' or rez.Rez_Kartno11 like N'" + dataKart + "%' or rez.Rez_Kartno12 like N'" + dataKart  + "%' or rez.Rez_Kartno13 like N'" + dataKart + "%') ";
            }
            if (Departman.Sorgu_Sekli == 2)     // Odano ve Kart ile Arama
            {
                filtre = " and ( " + Rez_Kartno + " like '" + dataKart + "%' or  Rez_Odano like '" + data + "%' or rez.Rez_Kartno11 like N'" + dataKart + "%' or rez.Rez_Kartno12 like N'" + dataKart + "%' or rez.Rez_Kartno13 like N'" + dataKart + "%') ";
            }
            if (Param.Param_Extre_Cikmasin)
            {
                filtre += " and Rez_Master_detay <> 'D' ";
            }




            DataTable dtOda = Fronttools.SelectTable("SELECT Rez_Id,Rez_Odano,Rez_Adi_1 , Rez_Adi_2, Rez_Kartno,Rez_Konaklama,Rez_Giris_tarihi,Rez_Cikis_tarihi, "
                                   + " convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,Ac_Adi,Rez_Odeme,Kodlar.Kodlar_Ad as Kodlar_Ad, "
                                   + " case when Rez_Master_detay = 'D' then Rez_Master_id else Rez_Id end as Rez_Master_Id "
                                   + " FROM Rez WITH(NOLOCK) " 
                                   + " left join Acenta WITH(NOLOCK) on Rez_Macenta = Acenta.Ac_Kodu " 
                                   + " left join Kodlar WITH(NOLOCK) on Rez_Mus_tipi = Kodlar_Kod and Kodlar_Sinif = '09'"
                                   + " WHERE  Rez_R_I_H = 'I' " + filtre
                                   + " ORDER BY Rez_Master_detay desc ");

            if (dtOda.Rows.Count > 0)
            {
                Mus_tipi = "O";
                Oda_No = Convert.ToString(dtOda.Rows[0]["Rez_Odano"]);
                Folio = Convert.ToInt32(dtOda.Rows[0]["Rez_Id"]);
                Master_Folio = Convert.ToInt32(dtOda.Rows[0]["Rez_Master_Id"]);
                Pansiyon = Convert.ToString(dtOda.Rows[0]["Rez_Konaklama"]);
                Odeme_Kodu = Convert.ToString(dtOda.Rows[0]["Rez_Odeme"]);
                //MusteriTipi = Convert.ToString(dtOda.Rows[0]["Kodlar_Ad"]);

                DataTable dtInd = dbtools.SelectTable("declare @Ind nvarchar(200) = (select Rez_Uye_depind_kodu from " + Fronttools.DB_LinkServer + "." + Fronttools.DB_Database + ".dbo.Rez with(nolock) where Rez_Id = '" + Folio.ToString() + "') "
                                                       + " select Ind_Kodu,Ind_Oran "
                                                       + " from Cst_Indirim with(nolock) "
                                                       + " where Ind_Kodu in (select FieldValue from StringArray(@Ind,',')) "
                                                       + " ORDER BY Ind_Oran desc");
                if (dtInd.Rows.Count > 0)
                {
                    Ind_Oran = Convert.ToDecimal(dtInd.Rows[0]["Ind_Oran"]);
                    Ind_Kodu = Convert.ToString(dtInd.Rows[0]["Ind_Kodu"]);
                }
                else
                {
                    Ind_Oran = 0;
                    Ind_Kodu = String.Empty;
                }

                Bilgi = Oda_No + " " + Convert.ToString(dtOda.Rows[0]["Rez_Adi_1"]) + " " + Convert.ToString(dtOda.Rows[0]["Rez_Adi_2"]) + " - " + Convert.ToString(dtOda.Rows[0]["Kodlar_Ad"]);

                return true;
            }
            return false;
        }

        private bool Uye_Ara()
        {
            string filtre = String.Empty;
            if (Departman.Sorgu_Sekli == 1 || Departman.Sorgu_Sekli == 2)     //Kart No ile Arama veya Oda-Kart aynı anda arama
            {
                string Kimlik_Kart = "Kimlik_Kart";

                string[] ozelKarakter = Param.Param_Kart_Yoksay.Split('#');
                if (ozelKarakter.Length > 0 && Param.Param_Kart_Yoksay != "")
                {
                    for (int i = 0; i < ozelKarakter.Length; i++)
                    {
                        Kimlik_Kart = "REPLACE(" + Kimlik_Kart + ",'" + ozelKarakter[i] + "','')";
                        data = data.Replace(ozelKarakter[i], "");
                    }
                }


                filtre = " and " + Kimlik_Kart + " like '" + data + "%' ";
            }

            DataTable dtUye = Fronttools.SelectTable("select Kimlik_Id,Kimlik_Ad,Kimlik_Soyad,Kimlik_Kart,Kart_Turu from Previl with(nolock) where Len(Kart_Turu) > 0 " + filtre);
            if (dtUye.Rows.Count > 0)
            {
                Mus_tipi = "U";
                Oda_No = "U" + Departman.Dep_Kodu;
                Folio = Convert.ToInt32(Fronttools.DegerGetir("select isnull((select isnull(Rez_Id,0) from Rez with(nolock) where Rez_Odano = '" + Oda_No + "'),0) "));
                Uye_Id = Convert.ToInt32(dtUye.Rows[0]["Kimlik_Id"]);
                Uye_Adsoyad = Convert.ToString(Convert.ToString(dtUye.Rows[0]["Kimlik_Ad"]) + " " + Convert.ToString(dtUye.Rows[0]["Kimlik_Soyad"]));
                Uye_Adsoyad = Uye_Adsoyad.Length > 100 ? Uye_Adsoyad.Substring(0, 99) : Uye_Adsoyad;
                Uye_Kartturu = Convert.ToString(dtUye.Rows[0]["Kart_Turu"]);

                Ind_Kodu = Convert.ToString(dtUye.Rows[0]["Kart_Turu"]);
                Ind_Oran = Convert.ToDecimal(dbtools.DegerGetir("select isnull((select Ind_Oran from Cst_Indirim with(nolock)where Ind_Kodu = '" + Convert.ToString(dtUye.Rows[0]["Kart_Turu"]) + "'),0)"));

                Bilgi = "Uye : " + Uye_Adsoyad;

                return true;
            }
            return false;
        }


    }
}
