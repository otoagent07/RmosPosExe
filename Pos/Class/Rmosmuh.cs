using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Pos.Class
{
    public class Rmosmuh
    {
        public static StreamReader oku = new StreamReader("WinLinePos.dll");
        public static string server = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");
        public static string users = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");
        public static string pwd = Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");
        public static string database = "Rmosmuh";//Crypto.Decrypt(Convert.ToString(oku.ReadLine()), "keykubat");


        public static String connstr = "Data Source='" + server + "';Initial Catalog=" + database + "; Persist Security Info=True;uid='" + users + "'; pwd='" + pwd + "'";
        //Data Adres*******************
        static DataSet dt;
        static SqlDataAdapter adap;
        public static SqlConnection conn = new SqlConnection(connstr);
        static SqlCommand cmd = null;
        public static int cust_cag = 0;


        public static bool execcmd(String cmds)
        {
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                cmd = new SqlCommand("set dateformat dmy ; " + cmds, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //////////////////////////////////

        public static String CheckDB()
        {
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                conn.Close();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //////////////////////////////

        public static String SelectTekData(String querytable, String where, String istenendeger)
        {
            dt = new DataSet();
            String str2 = "select " + istenendeger + " from " + querytable + " " + where;
            adap = new SqlDataAdapter(str2, conn);
            adap.Fill(dt, "tbl1");
            String str3;
            if (dt.Tables["tbl1"].Rows.Count > 0)
            {
                str3 = dt.Tables["tbl1"].Rows[0][0].ToString();
            }
            else
            {
                str3 = "";
            }
            return str3;
        }

        /// ////////////////////

        public static String DegerGetir(String sql)
        {
            dt = new DataSet();
            String str2 = "set dateformat dmy ; " + sql;
            adap = new SqlDataAdapter(str2, conn);

            adap.Fill(dt, "tbl1");
            String str3;
            if (dt.Tables["tbl1"].Rows.Count > 0)
            {
                str3 = dt.Tables["tbl1"].Rows[0][0].ToString();
            }
            else
            {
                str3 = "";
            }
            return str3;
        }

        ///////////////////////////////////////

        public static DataTable SelectTable(String sql1)
        {
            dt = new DataSet();
            adap = new SqlDataAdapter("set dateformat dmy ; " + sql1, conn);
            adap.Fill(dt, "q");
            return dt.Tables["q"];
        }

        public static string MacAdresi()
        {
            string mac;
            NetworkInterface[] arayuz = NetworkInterface.GetAllNetworkInterfaces();
            PhysicalAddress MacAdres = arayuz[0].GetPhysicalAddress();
            mac = MacAdres.ToString();
            return mac;
        }
    }
}
