using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Pos
{
    public partial class YonetimIslemleri : DevExpress.XtraEditors.XtraForm
    {
        public YonetimIslemleri()
        {
            InitializeComponent();
        }
        public string SubeMac { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string Users { get; set; }
        public string Password { get; set; }
        public string connstr { get; set; }

        SqlDataAdapter adap;
        SqlConnection conn;
        DataSet dts;
        DataTable dt;
        SqlCommand cmd;
        public bool ServerGetir()
        {
            dt = dbtools.SelectTable(@"select 
                                        Pkod_SubeMac as [SubeMac],
                                        Pkod_Server as [Server],
                                        Pkod_Database as [Database],
                                        Pkod_User as [User],
                                        Pkod_Password as [Password]
                                        from pos_kodlar
                                        where
                                        Pkod_Sinif = 27
                                        and
                                        Pkod_MerkezSube = 'M'");

            if (dt.Rows.Count > 0)
            {
                SubeMac = Convert.ToString(dt.Rows[0]["SubeMac"]);
                Server = Convert.ToString(dt.Rows[0]["Server"]);
                Database = Convert.ToString(dt.Rows[0]["Database"]);
                Users = Convert.ToString(dt.Rows[0]["User"]);
                Password = Convert.ToString(dt.Rows[0]["Password"]);

                connstr = "Data Source='" + Server + "'; Initial Catalog=" + Database + "; Persist Security Info=True; uid='" + Users + "'; pwd='" + Password + "'";
                conn = new SqlConnection(connstr);

                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("İlgili Server Bilgileri Yoktur.");
                return false;
            }
        }
        private bool CheckConnectionIP(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (!CheckConnectionIP(conn.ConnectionString))
            {
                return;
            }


            Rmosback.dbtools.conn = conn;
            Rmosback.Classes.Constants.cnnBack = conn.ConnectionString;
            //Rmosback.Classes.Constants.cnnRmosMuh = Rmosmuh.connstr; // conn.ConnectionString;


            DataTable dt = dbtools.SelectTable("select top 1 Pkod_Server,Pkod_Database,Pkod_User,Pkod_Password from Pos_Kodlar where Pkod_Sinif='27'");

            if (dt == null || dt.Rows.Count < 1)
            {
                System.Windows.Forms.MessageBox.Show("Lütfen Şube Tanımlamaya SQL bilgilerini Giriniz !");
                return;
            }


            string connstr = "Data Source='" + dt.Rows[0]["Pkod_Server"] + "';Initial Catalog=Rmosmuh; Persist Security Info=True;uid='" + dt.Rows[0]["Pkod_User"] + "'; pwd='" + dt.Rows[0]["Pkod_Password"] + "'";

            Rmosback.Classes.Constants.cnnRmosMuh = connstr;
            Rmosback.Service.RmosMuh.SirketService sirketService = new Rmosback.Service.RmosMuh.SirketService();

            Rmosback.Classes.Constants.MuhSirketKod = sirketService.GetAll().Where(x => x.Sirket_Database.ToUpper() == Database.ToUpper()).FirstOrDefault().Sirket_Kod;

            //System.Windows.Forms.MessageBox.Show("Test" + Rmosback.Classes.Constants.MuhSirketKod);

            Rmosback.dbtools.Kullanici_Kodu = User.U_BackUser;
            Rmosback.Classes.Constants.KullaniciKod = User.U_BackUser;
            Rmosback.Stk_Sayim sayim = new Rmosback.Stk_Sayim();
            sayim.ShowDialog();

            //System.Windows.Forms.MessageBox.Show("Test 3");

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (!CheckConnectionIP(conn.ConnectionString))
            {
                return;
            }

            Rmosback.dbtools.conn = conn;

            

            Rmosback.Classes.Constants.cnnBack = conn.ConnectionString;
            /*Rmosback.Classes.Constants.cnnRmosMuh = Rmosmuh.connstr;
            Rmosback.Service.RmosMuh.SirketService sirketService = new Rmosback.Service.RmosMuh.SirketService();
            Rmosback.Classes.Constants.MuhSirketKod = sirketService.GetAll().Where(x => x.Sirket_Database == Database).FirstOrDefault().Sirket_Kod;
            */

            DataTable dt = dbtools.SelectTable("select top 1 Pkod_Server,Pkod_Database,Pkod_User,Pkod_Password from Pos_Kodlar where Pkod_Sinif='27'");

            if (dt == null || dt.Rows.Count < 1)
            {
                System.Windows.Forms.MessageBox.Show("Lütfen Şube Tanımlamaya SQL bilgilerini Giriniz !");
                return;
            }


            string connstr = "Data Source='" + dt.Rows[0]["Pkod_Server"] + "';Initial Catalog=Rmosmuh; Persist Security Info=True;uid='" + dt.Rows[0]["Pkod_User"] + "'; pwd='" + dt.Rows[0]["Pkod_Password"] + "'";

            Rmosback.Classes.Constants.cnnRmosMuh = connstr;
            Rmosback.Service.RmosMuh.SirketService sirketService = new Rmosback.Service.RmosMuh.SirketService();

            Rmosback.Classes.Constants.MuhSirketKod = sirketService.GetAll().Where(x => x.Sirket_Database.ToUpper() == Database.ToUpper()).FirstOrDefault().Sirket_Kod;





            Rmosback.dbtools.Kullanici_Kodu = User.U_BackUser;
            Rmosback.Classes.Constants.KullaniciKod = User.U_BackUser;
            Rmosback.Sat_Talep sayim = new Rmosback.Sat_Talep();
            sayim.ShowDialog();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (!CheckConnectionIP(conn.ConnectionString))
            {
                return;
            }

            Rmosback.dbtools.conn = conn;

            Rmosback.Classes.Constants.cnnBack = conn.ConnectionString;

            /* Rmosback.Classes.Constants.cnnRmosMuh = Rmosmuh.connstr;
             Rmosback.Service.RmosMuh.SirketService sirketService = new Rmosback.Service.RmosMuh.SirketService();
             Rmosback.Classes.Constants.MuhSirketKod = sirketService.GetAll().Where(x => x.Sirket_Database == Database).FirstOrDefault().Sirket_Kod;*/

            DataTable dt = dbtools.SelectTable("select top 1 Pkod_Server,Pkod_Database,Pkod_User,Pkod_Password from Pos_Kodlar where Pkod_Sinif='27'");

            if (dt == null || dt.Rows.Count < 1)
            {
                System.Windows.Forms.MessageBox.Show("Lütfen Şube Tanımlamaya SQL bilgilerini Giriniz !");
                return;
            }


            string connstr = "Data Source='" + dt.Rows[0]["Pkod_Server"] + "';Initial Catalog=Rmosmuh; Persist Security Info=True;uid='" + dt.Rows[0]["Pkod_User"] + "'; pwd='" + dt.Rows[0]["Pkod_Password"] + "'";

            Rmosback.Classes.Constants.cnnRmosMuh = connstr;
            Rmosback.Service.RmosMuh.SirketService sirketService = new Rmosback.Service.RmosMuh.SirketService();

            Rmosback.Classes.Constants.MuhSirketKod = sirketService.GetAll().Where(x => x.Sirket_Database.ToUpper() == Database.ToUpper()).FirstOrDefault().Sirket_Kod;


            Rmosback.dbtools.Kullanici_Kodu = User.U_BackUser;
            Rmosback.Classes.Constants.KullaniciKod = User.U_BackUser;
            Rmosback.Sat_Talep sayim = new Rmosback.Sat_Talep();
            sayim.Tag = "I";
            sayim.ShowDialog();
        }


        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        private void YonetimIslemleri_Load(object sender, EventArgs e)
        {
            this.Text= res_man.GetString("Yönetim İşlemleri");

            simpleButton1.Text = res_man.GetString("Remote Stok Sayım");
            simpleButton2.Text = res_man.GetString("Remote Satın Alma Talebi");
            simpleButton4.Text = res_man.GetString("Remote Urun Iade");
            simpleButton3.Text = res_man.GetString("Çıkış");

            if (!ServerGetir())
            {
                System.Windows.Forms.MessageBox.Show("SERVER BİLGİLERİ GİRİLMEMİŞ !");
                return;
            }
        }
    }
}