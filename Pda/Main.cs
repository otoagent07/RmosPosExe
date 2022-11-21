using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;
using Pos.Class;
using Pos;

namespace Pda
{
    public partial class Main : DevExpress.XtraEditors.XtraForm
    {
        public string Posta { get; set; }
        public bool isLogin { get; set; }

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
        //    CultureInfo culture = new CultureInfo("TR-tr");
        //    culture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
        //    culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
        //    culture.DateTimeFormat.DateSeparator = ".";
        //    culture.DateTimeFormat.ShortTimePattern = "HH:mm";
        //    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        //    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

        //    Localize.ApplicationLanguage("tr");

            isLogin = false;

            Login login = new Login();
            login.ShowDialog();
            if (login.Cikis)
            {
                Application.Exit();
            }

            isLogin = true;

            btn_MasaTakip.Enabled = User.Pda_Masatakip;
            btn_DirekSatis.Enabled = User.Pda_Direksatis;

            Giris_Yap();

            Bilgi_Paneli();

            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
        }

        private void Giris_Yap()
        {
            string sonuc;

            //Departman Seçimi
            Dep_Secim dep = new Dep_Secim();
            dep.ShowDialog();
            Posta = dep.Posta;


            sonuc = Departman.Dep_Param_Yukle();

            //Departman Parametreleri ve Önbüro adresi yüklendi mi?
            if (sonuc == "OK")
            {
                Param.Param_Yukle();
                FisPr.Param_Yukle();
            }
            else
            {
                MessageBox.Show("Departman Parametreleri Yüklenemedi 1...");
            }
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            
        }

        private void Bilgi_Paneli()
        {
            lbl_Tarih.Text = Param.Tarih.ToString("dd.MM.yyyy");
            lbl_Departman.Text = Departman.Dep_Adi;
            lbl_Kullanici.Text = User.P_Ad + " " + User.P_Soyad;
            lbl_Kur.Text = Param.Doviz_Kuru.ToString("n4");
        }

        private void btn_DepDegistir_Click(object sender, EventArgs e)
        {
            Giris_Yap();

            Bilgi_Paneli();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_MasaTakip_Click(object sender, EventArgs e)
        {
            MasaTakip masa_takip = new MasaTakip();
            masa_takip.Posta = Posta;
            masa_takip.ShowDialog();
        }

        private void btn_DirekSatis_Click(object sender, EventArgs e)
        {
            Urun ur = new Urun();
            ur.Ozel_Masa = "";
            ur.Masa_No = User.P_Sabit_Masa;
            ur.Tag = Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));
            ur.Satis_Tip = "D";
            ur.ShowDialog();
        }

        int prmSayac = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            prmSayac++;
            if (prmSayac == 60 && isLogin == true)
            {
                Departman.Dep_Param_Yukle();
                Param.Param_Yukle();
                prmSayac = 0;
            }
        }

        private void btn_Relogin_Click(object sender, EventArgs e)
        {
            isLogin = false;

            Login login = new Login();
            login.ShowDialog();
            if (login.Cikis)
            {
                Application.Exit();
            }

            isLogin = true;

            btn_MasaTakip.Enabled = User.Pda_Masatakip;
            btn_DirekSatis.Enabled = User.Pda_Direksatis;

            Giris_Yap();

            Bilgi_Paneli();

            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
        }



    }
}