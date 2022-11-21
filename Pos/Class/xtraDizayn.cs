using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Resources;
using System.Reflection;

namespace Pos.Class
{
 
    class xtraDizayn
    {
        static ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        
        public static bool SaveReportStream(string reportID, string report_Kod, DevExpress.XtraReports.UI.XtraReport myReport)
        {
            Boolean result = false;
            try
            {
                System.IO.MemoryStream myStream = new System.IO.MemoryStream();
                myReport.SaveLayout(myStream);

                SqlConnection con = dbtools.conn;
                con.Close();
                if(con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                if (reportID != "0") com.CommandText = "UPDATE Rapor_Dizayn SET Rapor_Sekil = @repxStream WHERE Rapor_Id = @ID ";
                if (reportID == "0") com.CommandText = "Insert Into Rapor_Dizayn (Rapor_Kod, Rapor_Sekil) values('" + report_Kod + "', @repxStream) ";
                com.Parameters.AddWithValue("@ID", reportID);
                com.Parameters.AddWithValue("@repxStream", myStream.GetBuffer());
                com.ExecuteNonQuery();
                con.Close();

                result = true;
            }
            catch (Exception)
            {
                MessageBox.Show(res_man.GetString("Rapor kayıt işlemi basarısız, Lütfen tekrar deneyiniz..."), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                result = false;
            }
            return result;
        }

        
        public static DevExpress.XtraReports.UI.XtraReport BuildReport(string reportID)
        {
            DevExpress.XtraReports.UI.XtraReport myReport = new DevExpress.XtraReports.UI.XtraReport();
            LoadReportStream(reportID, myReport);
            return myReport;
        }

        public static bool LoadReportStream(string reportID, DevExpress.XtraReports.UI.XtraReport myReport)
        {
            Boolean result = false;
            try
            {
                SqlConnection con = dbtools.conn;
                con.Close();
                if(con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "SELECT Rapor_Sekil FROM Rapor_Dizayn WHERE Rapor_Id='" + reportID + "' ";
                byte[] myByteArray;
                myByteArray = (byte[])com.ExecuteScalar();
                con.Close();
                if (myByteArray != null)
                {
                    MemoryStream aMemoryStream = new MemoryStream(myByteArray);
                    myReport.LoadLayout(aMemoryStream);
                    result = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(res_man.GetString("Dizany Yüklenemedi..."), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return result;
        }


    }
}
