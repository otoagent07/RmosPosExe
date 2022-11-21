using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace IngenicoOKC.Class
{
    public class dbtools
    {


        public string connstr = "";
        //Data Adres*******************
        static DataSet dt;
        static SqlDataAdapter adap;
        //public SqlConnection conn = new SqlConnection(connstr);
        SqlCommand cmd = null;
        public int cust_cag = 0;



        public bool execcmd(String cmds)
        {
            string filter = "set dateformat dmy ; ";
            SqlConnection conn = new SqlConnection(connstr);
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //conn.Open();
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

        //////////////////////////////////


        public DataTable SelectTable(String sql1)
        {
            SqlConnection conn = new SqlConnection(connstr);
            string filter = "set dateformat dmy ; ";

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                dt = new DataSet();
                adap = new SqlDataAdapter(filter + sql1, conn);
                adap.Fill(dt, "q");
                conn.Close();
                return dt.Tables["q"];
            }
            catch
            {
                return null;
            }

        }



    }
}