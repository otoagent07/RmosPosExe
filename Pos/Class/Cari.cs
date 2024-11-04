using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Pos.Class
{
    public class Cari
    {
        public string Cari_Kod { get; set; }
        public string Cari_Ad { get; set; }
        public string Cari_Soyad { get; set; }
        public string Cari_Tel { get; set; }
        public string Cari_Adres1 { get; set; }
        public string Cari_Adres2 { get; set; }
        public string Cari_Adres3 { get; set; }
        public string Cari_Funvan { get; set; }
        public string Cari_Fadres1 { get; set; }
        public string Cari_Fadres2 { get; set; }
        public string Cari_Vergidarie { get; set; }
        public string Cari_Vergino { get; set; }
        public string Cari_Mail { get; set; }
        public string Cari_Kart { get; set; }
        public decimal Cari_Bakiye { get; set; }
        public string Cari_Il { get; set; }
        public string Cari_Ilce { get; set; }
        public string Cari_Mahalle { get; set; }
        public string Cari_Tel2 { get; set; }
        public static Cari Cari_Getir(string Kod)
        {
            Cari c = new Cari();

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 11);
            com.Parameters.AddWithValue("@Cari", Kod);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                c.Cari_Kod = Convert.ToString(dt.Rows[0]["Cari_Kod"]);
                c.Cari_Ad = Convert.ToString(dt.Rows[0]["Cari_Ad"]);
                c.Cari_Soyad = Convert.ToString(dt.Rows[0]["Cari_Soyad"]);
                c.Cari_Tel = Convert.ToString(dt.Rows[0]["Cari_Tel"]);
                c.Cari_Adres1 = Convert.ToString(dt.Rows[0]["Cari_Adres1"]);
                c.Cari_Adres2 = Convert.ToString(dt.Rows[0]["Cari_Adres2"]);
                c.Cari_Adres3 = Convert.ToString(dt.Rows[0]["Cari_Adres3"]);
                c.Cari_Funvan = Convert.ToString(dt.Rows[0]["Cari_Funvan"]);
                c.Cari_Fadres1 = Convert.ToString(dt.Rows[0]["Cari_Fadres1"]);
                c.Cari_Fadres2 = Convert.ToString(dt.Rows[0]["Cari_Fadres2"]);
                c.Cari_Vergidarie = Convert.ToString(dt.Rows[0]["Cari_Vergidarie"]);
                c.Cari_Vergino = Convert.ToString(dt.Rows[0]["Cari_Vergino"]);
                c.Cari_Mail = Convert.ToString(dt.Rows[0]["Cari_Mail"]);
                c.Cari_Kart = Convert.ToString(dt.Rows[0]["Cari_Kart"]);
                c.Cari_Bakiye = Convert.ToDecimal(dt.Rows[0]["Bakiye"]);
                c.Cari_Il = Convert.ToString(dt.Rows[0]["Cari_Il"]);
                c.Cari_Ilce = Convert.ToString(dt.Rows[0]["Cari_Ilce"]);
                c.Cari_Mahalle = Convert.ToString(dt.Rows[0]["Cari_Mahalle"]);
                c.Cari_Tel2 = Convert.ToString(dt.Rows[0]["Cari_Tel2"]);

                return c;
            }
            else
            {
                return null;
            }

        }

        public static Cari Cari_Getir2(string Kod)
        {
            Cari c = new Cari();


            DataTable dt = dbtools.SelectTable("select * from Pos_Cari Where  Cari_Kod = '" + Kod + "'");


            if (dt.Rows.Count > 0)
            {
                c.Cari_Kod = Convert.ToString(dt.Rows[0]["Cari_Kod"]);
                c.Cari_Ad = Convert.ToString(dt.Rows[0]["Cari_Ad"]);
                c.Cari_Soyad = Convert.ToString(dt.Rows[0]["Cari_Soyad"]);
                c.Cari_Tel = Convert.ToString(dt.Rows[0]["Cari_Tel"]);
                c.Cari_Adres1 = Convert.ToString(dt.Rows[0]["Cari_Adres1"]);
                c.Cari_Adres2 = Convert.ToString(dt.Rows[0]["Cari_Adres2"]);
                c.Cari_Adres3 = Convert.ToString(dt.Rows[0]["Cari_Adres3"]);
                c.Cari_Funvan = Convert.ToString(dt.Rows[0]["Cari_Funvan"]);
                c.Cari_Fadres1 = Convert.ToString(dt.Rows[0]["Cari_Fadres1"]);
                c.Cari_Fadres2 = Convert.ToString(dt.Rows[0]["Cari_Fadres2"]);
                c.Cari_Vergidarie = Convert.ToString(dt.Rows[0]["Cari_Vergidarie"]);
                c.Cari_Vergino = Convert.ToString(dt.Rows[0]["Cari_Vergino"]);
                c.Cari_Mail = Convert.ToString(dt.Rows[0]["Cari_Mail"]);
                c.Cari_Kart = Convert.ToString(dt.Rows[0]["Cari_Kart"]);
                //c.Cari_Bakiye = Convert.ToDecimal(dt.Rows[0]["Bakiye"]);
                c.Cari_Il = Convert.ToString(dt.Rows[0]["Cari_Il"]);
                c.Cari_Ilce = Convert.ToString(dt.Rows[0]["Cari_Ilce"]);
                c.Cari_Mahalle = Convert.ToString(dt.Rows[0]["Cari_Mahalle"]);

                return c;
            }
            else
            {
                return null;
            }

        }
    }
}
