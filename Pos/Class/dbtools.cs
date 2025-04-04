using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

namespace Pos.Class
{
    public class dbtools
    {

        public static StreamReader oku = new StreamReader("WinlinePos.dll");
        public static string server = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");
        public static string users = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");
        public static string pwd = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");
        public static string database = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");


        public static string connstr = "Data Source='" + server + "';Initial Catalog=" + database + "; Persist Security Info=True;uid='" + users + "'; pwd='" + pwd + "'";
        //Data Adres*******************
        static DataSet dt;
        static SqlDataAdapter adap;
        public static SqlConnection conn = new SqlConnection(connstr);
        static SqlCommand cmd = null;
        public static int cust_cag = 0;




        public static void conYenile(string server, string database, string users, string pwd)
        {
            string conyeni = "Data Source='" + server + "';Initial Catalog=" + database + "; Persist Security Info=True;uid='" + users + "'; pwd='" + pwd + "'";
            conn = new SqlConnection(conyeni);
        }

        public static void coneskiyedon()
        {
            conn = new SqlConnection(connstr);
            Departman.Dep_Param_Yukle();
            Param.Param_Yukle();
            FisPr.Param_Yukle();
        }

        public static string MyClass = "dbtools";
        public static bool execcmd(String cmds)
        {
            if (Langs.Default.Dil == "") Langs.Default.Dil = "tr-TR";
            string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");

            for (int i = 0; i <= denemeSayisi; i++)
            {


                try
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    conn.Open();
                    cmd = new SqlCommand(filter + "" + cmds, conn);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("was deadlocked on lock"))
                    {
                        Thread.Sleep(beklemeSuresi);
                        continue;
                    }
                    else
                    {
                        RHMesaj.MyMessageError(MyClass, "execcmd", "", ex);
                        return false;
                    }
                }

            }

            RHMesaj.MyMessageError(MyClass, "execcmd", "", new Exception("Deadlock Hatası"));
            return false;
        }

        public static bool execcmdR(String cmds)
        {
            for (int i = 0; i <= denemeSayisi; i++)
            {
                try
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    conn.Open();
                    cmd = new SqlCommand(cmds, conn);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("was deadlocked on lock"))
                    {
                        Thread.Sleep(beklemeSuresi);
                        continue;
                    }
                    else
                    {
                        RHMesaj.MyMessageError(MyClass, "execcmdR", "", ex);
                        return false;
                    }
                }

            }

            RHMesaj.MyMessageError(MyClass, "execcmdR", "", new Exception("Deadlock Hatası"));
            return false;
        }


        static int denemeSayisi = 50;
        static int beklemeSuresi = 200; // mili saniye cinsinden


        public static bool execcmdRMesajsiz(String cmds)
        {
            for (int i = 0; i <= denemeSayisi; i++)
            {

                try
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    conn.Open();
                    cmd = new SqlCommand(cmds, conn);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("was deadlocked on lock"))
                    {
                        Thread.Sleep(beklemeSuresi);
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public static string DegerGetir(String sql)
        {
            for (int i = 0; i <= denemeSayisi; i++)
            {
                try
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    conn.Open();

                    if (Langs.Default.Dil == "") Langs.Default.Dil = "tr-TR";
                    string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");

                    dt = new DataSet();
                    string str2 = sql;
                    adap = new SqlDataAdapter(filter + str2, conn);
                    adap.SelectCommand.CommandTimeout = 0;
                    adap.Fill(dt, "tbl1");
                    string str3;
                    if (dt.Tables["tbl1"] != null && dt.Tables["tbl1"].Rows.Count > 0)
                    {
                        str3 = dt.Tables["tbl1"].Rows[0][0].ToString();
                    }
                    else
                    {
                        str3 = "";
                    }


                    return str3;

                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("was deadlocked on lock"))
                    {
                        Thread.Sleep(beklemeSuresi);
                        continue;
                    }
                    else
                    {
                        RHMesaj.MyMessageError(MyClass, "DegerGetir", "", ex);
                        return "";
                    }
                }

            }

            RHMesaj.MyMessageError(MyClass, "DegerGetir", "", new Exception("Deadlock Hatası"));
            return "";
        }

        public static DataTable SelectTable(string sql1)
        {
            if (Langs.Default.Dil == "") Langs.Default.Dil = "tr-TR";
            string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");
            if (filter == "")
            {
                filter = " set dateformat dmy ; ";
            }

            for (int i = 0; i <= denemeSayisi; i++)
            {

                try
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    conn.Open();

                    dt = new DataSet();
                    adap = new SqlDataAdapter(filter + sql1, conn);
                    adap.SelectCommand.CommandTimeout = 0;
                    adap.Fill(dt, "q");

                    return dt.Tables["q"];
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("was deadlocked on lock"))
                    {
                        Thread.Sleep(beklemeSuresi);
                        continue;
                    }
                    else
                    {
                        //RHMesaj.MyMessageError(MyClass, "SelectTable", "", ex);
                        return null;
                    }
                }

            }

            //RHMesaj.MyMessageError(MyClass, "SelectTable", "", new Exception("Deadlock Hatası"));
            return null;
        }

        public static DataTable SelectTableR(string sql1)
        {
            try
            {
                for (int i = 0; i <= denemeSayisi; i++)
                {
                    try
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        conn.Open();

                        dt = new DataSet();
                        adap = new SqlDataAdapter(sql1, conn);
                        adap.SelectCommand.CommandTimeout = 0;
                        adap.Fill(dt, "q");
                        return dt.Tables["q"];

                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("was deadlocked on lock"))
                        {
                            Thread.Sleep(beklemeSuresi);
                            continue;
                        }
                        else
                        {
                            RHMesaj.MyMessageError(MyClass, "SelectTableR", "", ex);
                            return null;
                        }
                    }

                }

                RHMesaj.MyMessageError(MyClass, "SelectTableR", "", new Exception("Deadlock Hatası"));

                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }


        public static DataTable Dizayn_Getir(string User, string Form_Adi, string Rapor_Adi)
        {
            for (int i = 0; i <= denemeSayisi; i++)
            {
                try
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    conn.Open();

                    dt = new DataSet();
                    adap = new SqlDataAdapter("SELECT * FROM User_Dizayn WHERE "
                    //+ " Diz_User = '" + User + "'  AND "
                    + "  Diz_Form = '" + Form_Adi + "' " + (Rapor_Adi == "" ? "" : "and Diz_Id = '" + Rapor_Adi + "'") + " ", conn);
                    adap.SelectCommand.CommandTimeout = 0;
                    adap.Fill(dt, "tbl1");
                    return dt.Tables["tbl1"];

                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("was deadlocked on lock"))
                    {
                        Thread.Sleep(beklemeSuresi);
                        continue;
                    }
                    else
                    {
                        RHMesaj.MyMessageError(MyClass, "Dizayn_Getir", "", ex);
                        return null;
                    }
                }


            }

            RHMesaj.MyMessageError(MyClass, "Dizayn_Getir", "", new Exception("Deadlock Hatası"));
            return null;
        }


        public static string MacAdresi()
        {
            string mac = "";

            try
            {
                NetworkInterface[] arayuz = NetworkInterface.GetAllNetworkInterfaces();
                PhysicalAddress MacAdres = arayuz[0].GetPhysicalAddress();
                mac = MacAdres.ToString();
            }
            catch (Exception ex)
            {

            }

            return mac;

        }

        public static decimal KurGetir(DateTime tarih, string kurkodu)
        {
            for (int i = 0; i <= denemeSayisi; i++)
            {
                try
                {
                    string cins;
                    switch (Param.Doviz_Cinsi)
                    {
                        case 0:
                            cins = "M";
                            break;
                        case 1:
                            cins = "E";
                            break;
                        case 2:
                            cins = "M"; //Giriş Günü Kuru
                            break;
                        default:
                            cins = "M";
                            break;
                    }

                    DataTable k = SelectTable("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + cins + "' and Kurlar_Kodu = '" + kurkodu + "' and Kurlar_Tarih = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'");
                    if (k.Rows.Count > 0)
                    {
                        return Convert.ToDecimal(k.Rows[0][Param.Doviz_Turu]);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("was deadlocked on lock"))
                    {
                        Thread.Sleep(beklemeSuresi);
                        continue;
                    }
                    else
                    {
                        RHMesaj.MyMessageError(MyClass, "KurGetir", "", ex);
                        return 0;
                    }
                }

            }

            RHMesaj.MyMessageError(MyClass, "KurGetir", "", new Exception("Deadlock Hatası"));
            return 0;
        }

    }
}
