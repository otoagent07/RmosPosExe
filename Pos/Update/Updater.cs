using Pos.Class;

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Pos.Update
{
    public partial class Updater : DevExpress.XtraEditors.XtraForm
    {
        public Updater()
        {
            InitializeComponent();
        }

        DateTime tarihExePc = Convert.ToDateTime("2019-12-19 10:00:00");
        DateTime tarihExeFtp;
        DateTime tarihSqlPc;
        DateTime tarihSqlFtp;

        static string klasor = "Update";
        string lclAdresExe = klasor + "\\Pos.exe";
        string lclAdresVersion = klasor + "\\version.txt";
        string lclAdresSqlMuh = klasor + "\\RmosMuh.sql";
        string lclAdresSqlBack = klasor + "\\RmosBack.sql";

        static string ftpAdresPos = "ftp://update.rmoscrm.com/Pos/";
        static string ftpAdresPBack = "ftp://update.rmoscrm.com/Back/";
        string ftpUser = "updateftp";
        string ftpPass = "r4z_N71g";

        string ftpAdresExe = ftpAdresPos + "Pos.exe";
        string ftpAdresSqlMuh = ftpAdresPBack + "RmosMuh.sql";
        string ftpAdresSqlBack = ftpAdresPBack + "RmosBack.sql";
        string ftpAdresversion = ftpAdresPos + "version.txt";

        private void GuncellemeKontrol()
        {
            {
                #region Klasör ve txt işlemleri
                if (Directory.Exists(klasor))
                {
                    if (!File.Exists(lclAdresVersion))
                    {
                        FileStream fs = File.Create(lclAdresVersion);
                        fs.Close();
                    }
                }
                else
                {
                    Directory.CreateDirectory(klasor);
                    FileStream fs = File.Create(lclAdresVersion);
                    fs.Close();
                }
                #endregion

                try
                {
                    #region Ftp den bilgilerin alınması
                    FtpWebRequest request;
                    request = (FtpWebRequest)WebRequest.Create(ftpAdresversion);
                    request.Credentials = new NetworkCredential(ftpUser, ftpPass);
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    using (Stream ftpStream = request.GetResponse().GetResponseStream())
                    using (Stream fileStream = File.Create(lclAdresVersion))
                    {
                        byte[] buffer = new byte[10240];
                        int read;
                        int total = 0;
                        while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            Application.DoEvents();
                            fileStream.Write(buffer, 0, read);
                        }
                    }
                    string[] lines = System.IO.File.ReadAllLines(lclAdresVersion);
                    #endregion

                    //tarihExePc = Convert.ToDateTime(File.GetLastWriteTime("Rmosback.exe").ToString("yyyy-MM-dd HH:mm"));
                    tarihExeFtp = Convert.ToDateTime(lines[0].Split('=')[1].Trim());

                    tarihSqlPc = Convert.ToDateTime(dbtools.DegerGetir("Select Convert(date,ISNULL(Param_UpdateTarih,datediff(d,1,GETDATE()))) as UpdateTarih From Pos_Param where Param_Id = 1 "));
                    tarihSqlFtp = Convert.ToDateTime(lines[1].Split('=')[1].Trim());

                    //if (Convert.ToDateTime(tarihSqlFtp.ToString("yyyy-MM-dd")) > Convert.ToDateTime(tarihSqlPc.ToString("yyyy-MM-dd")))
                    //{
                    DownloadSqlMuh();
                    //}

                    if (Convert.ToDateTime(tarihExeFtp.ToString("yyyy-MM-dd")) > Convert.ToDateTime(tarihExePc.ToString("yyyy-MM-dd")))
                    {
                        //txt_Aciklama.Text = "Exe Güncellenecek";
                        DownloadExe();
                        MessageBox.Show("Güncelleme Tamamlandı. Program Kapatılacak.");
                        Application.Exit();
                    }
                    else
                    {
                        //btn_Exe.ImageIndex = 1;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Güncelleme için FTP server a bağlantı kurulamadı.");
                }
                finally
                {

                    this.Close();
                }
            }
        }

        private void DownloadExe()
        {
            if (File.Exists(lclAdresExe))
                File.Delete(lclAdresExe);

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpAdresExe);
            request.Credentials = new NetworkCredential(ftpUser, ftpPass);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            using (Stream ftpStream = request.GetResponse().GetResponseStream())
            using (Stream fileStream = File.Create(lclAdresExe))
            {
                int bufferSizde = 32 * 1024;
                byte[] buffer = new byte[bufferSizde];
                int read;
                int total = 0;
                while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, read);
                    total += read;
                    //bar_Exe.Properties.Minimum = 0;
                    //bar_Exe.Properties.Maximum = (int)fileStream.Length;
                    //bar_Exe.EditValue = total;
                }
            }

            try
            {
                try
                {
                    string NewExe = "Pos.exe";
                    string OldExe = "Pos - Old.exe";

                    if (File.Exists(OldExe))
                        File.Delete(OldExe);

                    File.Move(NewExe, OldExe);
                    File.Move(lclAdresExe, NewExe);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Exe Güncellenemedi. Sebebi:" + Ex.Message);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void DownloadSqlBordro()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpAdresSqlBack);
            request.Credentials = new NetworkCredential(ftpUser, ftpPass);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            using (Stream ftpStream = request.GetResponse().GetResponseStream())
            using (Stream fileStream = File.Create(lclAdresSqlBack))
            {
                byte[] buffer = new byte[10240];
                int read;
                int total = 0;
                while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Application.DoEvents();
                    fileStream.Write(buffer, 0, read);
                    total += read;
                    //bar_Bordro.Properties.Minimum = 0;
                    //bar_Bordro.Properties.Maximum = (int)fileStream.Length;
                    //bar_Bordro.EditValue = total;
                }
            }

            try
            {
                string dosya = "Use " + dbtools.database + " \n" + File.ReadAllText(lclAdresSqlBack);

                SqlConnection con = new SqlConnection(dbtools.connstr);
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandTimeout = 0;
                SqlCommand cmd2 = con.CreateCommand();
                cmd2.CommandTimeout = 0;
                cmd2.CommandText = dosya;

                rch_Script = cmd2.CommandText;
                cmd2.ExecuteNonQuery();
                con.Close();

                dbtools.execcmd("Update Pos_Param Set Param_UpdateTarih = '" + Convert.ToDateTime(tarihSqlFtp) + "' where Param_Id = 1 ");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        string rch_Script = String.Empty;

        private void DownloadSqlMuh()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpAdresSqlMuh);
            request.Credentials = new NetworkCredential(ftpUser, ftpPass);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            using (Stream ftpStream = request.GetResponse().GetResponseStream())
            using (Stream fileStream = File.Create(lclAdresSqlMuh))
            {
                byte[] buffer = new byte[10240];
                int read;
                int total = 0;
                while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Application.DoEvents();
                    fileStream.Write(buffer, 0, read);
                    total += read;
                    //bar_Muh.Properties.Minimum = 0;
                    //bar_Muh.Properties.Maximum = (int)fileStream.Length;
                    //bar_Muh.EditValue = total;
                }
            }

            try
            {
                string dosya = "Use RmosMuh" + "\n" + File.ReadAllText(lclAdresSqlMuh);

                SqlConnection con = new SqlConnection(dbtools.connstr);
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandTimeout = 0;
                SqlCommand cmd2 = con.CreateCommand();
                cmd2.CommandTimeout = 0;
                cmd2.CommandText = dosya;

                rch_Script = cmd2.CommandText;
                cmd2.ExecuteNonQuery();
                con.Close();

                //btn_Muh.ImageIndex = 1;
                //btn_Muh.Refresh();
                this.Refresh();
                Application.DoEvents();

                DownloadSqlBordro();

                //Btn_Bordro.ImageIndex = 1;
                //Btn_Bordro.Refresh();
                this.Refresh();
                Application.DoEvents();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        //private void Listele()
        //{
        //    DataTable dt = dbtools.SelectTable("select * from UpdateFTP where ID = 1");
        //    if (dt.Rows.Count > 0)
        //    {
        //        ftpAdres = Convert.ToString(dt.Rows[0]["FTP_Adress"]);
        //        ftpUser = Convert.ToString(dt.Rows[0]["FTP_User"]);
        //        ftpPass = Convert.ToString(dt.Rows[0]["FTP_Pass"]);
        //    }
        //}

        private void Updater_Load(object sender, EventArgs e)
        {
            //Listele();
            this.BringToFront();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            GuncellemeKontrol();


        }
    }
}