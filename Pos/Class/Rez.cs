using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Pos.Class
{
    class Rez
    {
        public int Rez_Id { get; set; }
        public DateTime Rez_Tarih { get; set; }
        public TimeSpan Rez_Saat { get; set; }
        public string Rez_Dep { get; set; }
        public string Rez_Adi { get; set; }
        public string Rez_Soyadi { get; set; }
        public int Rez_Kisi { get; set; }
        public int Rez_MasaSayisi { get; set; }
        public string Rez_Masano { get; set; }
        public string Rez_Tel { get; set; }
        public string Rez_Email { get; set; }
        public string Rez_Not { get; set; }


        public void Rez_Kaydet(Rez r)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Rez_Kayit";
            com.Parameters.AddWithValue("@Rez_Id", r.Rez_Id);
            com.Parameters.AddWithValue("@Rez_Tarih", r.Rez_Tarih.Date);
            com.Parameters.AddWithValue("@Rez_Saat", r.Rez_Saat.ToString());
            com.Parameters.AddWithValue("@Rez_Dep", r.Rez_Dep);
            com.Parameters.AddWithValue("@Rez_Adi", r.Rez_Adi);
            com.Parameters.AddWithValue("@Rez_Soyadi", r.Rez_Soyadi);
            com.Parameters.AddWithValue("@Rez_Kisi", r.Rez_Kisi);
            com.Parameters.AddWithValue("@Rez_MasaSayisi", r.Rez_MasaSayisi);
            com.Parameters.AddWithValue("@Rez_Masano", r.Rez_Masano);
            com.Parameters.AddWithValue("@Rez_Tel", r.Rez_Tel);
            com.Parameters.AddWithValue("@Rez_Email", r.Rez_Email);
            com.Parameters.AddWithValue("@Rez_Not", r.Rez_Not);
            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        public DataTable Rez_Liste(int rapor_Tipi, DateTime tarih1, DateTime tarih2)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Rez_Liste";
            com.Parameters.AddWithValue("@Rapor_Tipi", rapor_Tipi);
            com.Parameters.AddWithValue("@Tarih1",tarih1);
            com.Parameters.AddWithValue("@Tarih2", tarih2);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (con.State == ConnectionState.Open) con.Close();

            return dt;
        }

        public void Rez_Sil(int Rez_Id)
        {
            dbtools.execcmd("delete from Pos_Rez where Rez_Id = '" + Rez_Id + "'");
        }


    }
}
