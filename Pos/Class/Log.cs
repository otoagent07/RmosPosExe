using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace Pos.Class
{
    public class Log
    {
        public enum Log_Bolum
        {
            Satis,
            Satir_Sil,
            Fis_Iptal,
            Tutar_Duzelt,
            Indirim_Uygula,
            ServisPayi_Uygula,
            Masa_Transfer,
            Malz_Transfer,
            Miktar_Duzelt,
            Ozel_Masa,
            Prm_Genel,
            Prm_PrintGrup,
            Prm_PrintAyar,
            Prm_HesapFis,
            Prm_IptalFis,
            Prm_SiparisFis,
            Prm_Adisyon,
            Prm_Fatura,
            Prm_OdemeKodu,
            Prm_OnbEntegre,
            Prm_CostEntegre,
            Prm_MacPrint,
            Prm_MasaTanim,
            Prm_MasaKonum,
            Prm_CariTanim,
            Prm_CariHesap,
            Prm_HH,
            Prm_Kullanici,
            Prm_Pda,
            Prm_Posta,
            Prm_PrinterTanim,
            Hesap_Transfer,
            Gun_Sonu,
            Prm_Kasagc,
            Kasa_Hrk,
            Prm_AciklamaItem,
            Zayi,
            Raporlar,
            Hesap,
            Kisi_Sayisi,
            Prm_IlTanim,
            Prm_IlceTanim,
            Prm_MahalleTanim,
            Prm_SubeTanim,
            Kart_Transfer,
            Kart,
            Menu,
            Urun_Tahsilat,
            Print,
            ikram,
            UrunIade,
            Ingenico2,
        };

        public enum Log_Islem
        {
            Kaydet,
            Duzelt,
            Sil,
            FixKaydet,
            UrunIade
        };

        public enum Log_Program
        {
            Pos,
            Pda,
            Android
        }

        public static void Log_Kaydet(Log_Program Program, Log_Bolum Bolum, Log_Islem Islem, string Aciklama, string Fisno, string Islem_ID)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Log_Ekle";
            com.Parameters.AddWithValue("@Log_Prog", Program.ToString());
            com.Parameters.AddWithValue("@Log_Bolum", Bolum.ToString());
            com.Parameters.AddWithValue("@Log_Islem", Islem.ToString());
            com.Parameters.AddWithValue("@Log_User", User.P_Kod);
            com.Parameters.AddWithValue("@Log_Bilg", SystemInformation.ComputerName);
            com.Parameters.AddWithValue("@Log_Aciklama", Aciklama);
            com.Parameters.AddWithValue("@Log_FisNo", Fisno);
            com.Parameters.AddWithValue("@Log_IslemId", Islem_ID);
            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        public static void Log_KaydetUrun(Log_Program Program, Log_Bolum Bolum, Log_Islem Islem, string Aciklama, string Fisno, string Islem_ID,string Log_Recete="",string Log_Urun="")
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Log_Ekle";
            com.Parameters.AddWithValue("@Log_Prog", Program.ToString());
            com.Parameters.AddWithValue("@Log_Bolum", Bolum.ToString());
            com.Parameters.AddWithValue("@Log_Islem", Islem.ToString());
            com.Parameters.AddWithValue("@Log_User", User.P_Kod);
            com.Parameters.AddWithValue("@Log_Bilg", SystemInformation.ComputerName);
            com.Parameters.AddWithValue("@Log_Aciklama", Aciklama);
            com.Parameters.AddWithValue("@Log_FisNo", Fisno);
            com.Parameters.AddWithValue("@Log_IslemId", Islem_ID);
            com.Parameters.AddWithValue("@Log_Recete", Log_Recete);
            com.Parameters.AddWithValue("@Log_Urun", Log_Urun);
            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }
        public static void Log_Kaydet(Log_Program Program, Log_Bolum Bolum, Log_Islem Islem, string Aciklama, string Fisno, string Islem_ID,string neden="")
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Log_Ekle";
            com.Parameters.AddWithValue("@Log_Prog", Program.ToString());
            com.Parameters.AddWithValue("@Log_Bolum", Bolum.ToString());
            com.Parameters.AddWithValue("@Log_Islem", Islem.ToString());
            com.Parameters.AddWithValue("@Log_User", User.P_Kod);
            com.Parameters.AddWithValue("@Log_Bilg", SystemInformation.ComputerName);
            com.Parameters.AddWithValue("@Log_Aciklama", Aciklama);
            com.Parameters.AddWithValue("@Log_FisNo", Fisno);
            com.Parameters.AddWithValue("@Log_IslemId", Islem_ID);
            com.Parameters.AddWithValue("@Log_Neden", neden);
            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        public static bool Log_Kaydet(DateTime Log_Tarih, String Log_Sirket, String Log_Bolum, String Log_Islem, String Log_User, String Log_Comp, String Log_Aciklama, String Log_FisNo, String Log_IslemId,DbtoolsMerkez dbtoolsMerkez)
        {
            try
            {
                string query = "set dateformat dmy ; " + "INSERT INTO User_Log(Log_Tarih,Log_Sirket,Log_Bolum,Log_Islem,Log_User,Log_Comp,Log_Aciklama,Log_FisNo,Log_IslemId) "
                  + " VALUES ('" + Log_Tarih + "','" + Log_Sirket + "','" + Log_Bolum + "','" + Log_Islem + "','" + Log_User + "','" + Log_Comp + "','" + Convert.ToString(Log_Aciklama).Replace("'", "''") + "','" + Log_FisNo + "','" + Log_IslemId + "')";

                dbtoolsMerkez.execcmd(query);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL kodunda hataya rastlandı >>> " + ex.Message);
                return false;
            }
            finally
            {
            }
        }

        public static void Log_Kaydet(Log_Program Program, Log_Bolum Bolum, Log_Islem Islem, string Aciklama, string Fisno, string Islem_ID, string urun, decimal miktar)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Log_Ekle";
            com.Parameters.AddWithValue("@Log_Prog", Program.ToString());
            com.Parameters.AddWithValue("@Log_Bolum", Bolum.ToString());
            com.Parameters.AddWithValue("@Log_Islem", Islem.ToString());
            com.Parameters.AddWithValue("@Log_User", User.P_Kod);
            com.Parameters.AddWithValue("@Log_Bilg", SystemInformation.ComputerName);
            com.Parameters.AddWithValue("@Log_Aciklama", Aciklama);
            com.Parameters.AddWithValue("@Log_FisNo", Fisno);
            com.Parameters.AddWithValue("@Log_IslemId", Islem_ID);
            com.Parameters.AddWithValue("@Log_Urun", urun);
            com.Parameters.AddWithValue("@Log_Miktar", miktar);

            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

        public static void Log_Kaydet(Log_Program Program, Log_Bolum Bolum, Log_Islem Islem, string Aciklama, string Fisno, string Islem_ID, string urun, decimal miktar, string neden, decimal Tutar,string recete="",string urunad="")
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Log_Ekle";
            com.Parameters.AddWithValue("@Log_Prog", Program.ToString());
            com.Parameters.AddWithValue("@Log_Bolum", Bolum.ToString());
            com.Parameters.AddWithValue("@Log_Islem", Islem.ToString());
            com.Parameters.AddWithValue("@Log_User", User.P_Kod);
            com.Parameters.AddWithValue("@Log_Bilg", SystemInformation.ComputerName);
            com.Parameters.AddWithValue("@Log_Aciklama", Aciklama);
            com.Parameters.AddWithValue("@Log_FisNo", Fisno);
            com.Parameters.AddWithValue("@Log_IslemId", Islem_ID);
            com.Parameters.AddWithValue("@Log_Urun", urunad);
            com.Parameters.AddWithValue("@Log_Miktar", miktar);
            com.Parameters.AddWithValue("@Log_Neden", neden);
            com.Parameters.AddWithValue("@Log_Recete", recete);
            com.Parameters.AddWithValue("@Log_Tutar", Tutar);

            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();
        }

    }
}
