using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Pos.Class
{
    public class DbtoolsMerkez
    {

        public string server { set; get; }
        public string database { set; get; }

        public string users { set; get; }
        public string pwd { set; get; }

        public string connstr = "";

        public DataSet dt;
        public SqlDataAdapter adap;
        public SqlConnection conn = null;
        public SqlCommand cmd = null;
        public int cust_cag = 0;

        public DbtoolsMerkez(string server, string database, string users, string pwd)
        {
            this.server = server;
            this.database = database;
            this.users = users;
            this.pwd = pwd;
            connstr = "Data Source='" + server + "';Initial Catalog=" + database + "; Persist Security Info=True;uid='" + users + "'; pwd='" + pwd + "'";
            conn = new SqlConnection(connstr);
        }

        public void conYenile()
        {
            connstr = "Data Source='" + server + "';Initial Catalog=" + database + "; Persist Security Info=True;uid='" + users + "'; pwd='" + pwd + "'";
            conn = new SqlConnection(connstr);
        }

        public bool execcmd(String cmds)
        {
            if (Langs.Default.Dil == "") Langs.Default.Dil = "tr-TR";
            string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");

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
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        public String CheckDB()
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

        public String SelectTekData(String querytable, String where, String istenendeger)
        {
            if (Langs.Default.Dil == "") Langs.Default.Dil = "tr-TR";
            string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");


            dt = new DataSet();
            String str2 = "select " + istenendeger + " from " + querytable + " " + where;
            adap = new SqlDataAdapter(filter + str2, conn);
            adap.SelectCommand.CommandTimeout = 0;
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

        public String DegerGetir(String sql)
        {
            if (Langs.Default.Dil == "") Langs.Default.Dil = "tr-TR";
            string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");

            dt = new DataSet();
            String str2 = sql;
            adap = new SqlDataAdapter(filter + str2, conn);
            adap.SelectCommand.CommandTimeout = 0;
            adap.Fill(dt, "tbl1");
            String str3;
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

        public DataTable SelectTable(String sql1)
        {
            if (Langs.Default.Dil == "") Langs.Default.Dil = "tr-TR";
            string filter = (Langs.Default.Dil == "tr-TR" ? "set dateformat dmy ; " : "");
            if (filter == "")
            {
                filter = " set dateformat dmy ; ";
            }
            try
            {
                dt = new DataSet();
                adap = new SqlDataAdapter(filter + sql1, conn);
                adap.SelectCommand.CommandTimeout = 0;
                adap.Fill(dt, "q");

                return dt.Tables["q"];
            }
            catch
            {
                return null;
            }

        }
        public DataTable SelectTableR(string sql1)
        {
            dt = new DataSet();
            adap = new SqlDataAdapter(sql1, conn);
            adap.SelectCommand.CommandTimeout = 0;
            adap.Fill(dt, "q");
            return dt.Tables["q"];
        }

        public DataTable SelectTableConn(String sql1, string ConnectionString)
        {
            dt = new DataSet();
            adap = new SqlDataAdapter(sql1, ConnectionString);
            adap.SelectCommand.CommandTimeout = 0;
            adap.Fill(dt, "q");
            return dt.Tables["q"];
        }

        public DataTable Dizayn_Getir(String User, String Form_Adi, String Rapor_Adi)
        {
            dt = new DataSet();
            adap = new SqlDataAdapter("SELECT * FROM User_Dizayn WHERE "
            //+ " Diz_User = '" + User + "'  AND "
            + "  Diz_Form = '" + Form_Adi + "' " + (Rapor_Adi == "" ? "" : "and Diz_Id = '" + Rapor_Adi + "'") + " ", conn);
            adap.SelectCommand.CommandTimeout = 0;
            adap.Fill(dt, "tbl1");
            return dt.Tables["tbl1"];
        }

        public string MacAdresi()
        {
            string mac;
            NetworkInterface[] arayuz = NetworkInterface.GetAllNetworkInterfaces();
            PhysicalAddress MacAdres = arayuz[0].GetPhysicalAddress();
            mac = MacAdres.ToString();
            return mac;
        }

        public decimal KurGetir(DateTime tarih, string kurkodu)
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

            DataTable k = dbtools.SelectTable("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + cins + "' and Kurlar_Kodu = '" + kurkodu + "' and Kurlar_Tarih = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'");
            if (k.Rows.Count > 0)
            {
                return Convert.ToDecimal(k.Rows[0][Param.Doviz_Turu]);
            }
            else
            {
                return 0;
            }
        }
    }
}
