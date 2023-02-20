using DevExpress.XtraEditors;
using Pos.Class;
using Pos.Forms;
using System;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        bool Param_Kartla_Giris = false;
        public bool Cikis = false;
        bool xCikis = false;

        int Giris = 0;

        public bool DonenDeger = false;

        public string Durum = "Giris";
        public frmLogin()
        {
            InitializeComponent();
        }

        public void dilYenile()
        {
            try
            {
                if (Param_Kartla_Giris)
                {
                    if (Langs.Default.Dil == "tr-TR")
                    {
                        lblBilgi.Text = "Kart Numaranızı Giriniz...";
                    }
                    else
                    {
                        lblBilgi.Text = "Enter Your Card Number ...";
                    }
                }
                else
                {
                    if (Langs.Default.Dil == "tr-TR")
                    {
                        lblBilgi.Text = "Kullanıcı Kodunuzu Giriniz...";
                    }
                    else
                    {
                        lblBilgi.Text = "Enter Your User Code ...";
                    }
                }



                if (Langs.Default.Dil == "tr-TR")
                {
                    btn_Giris.Text = "Giriş";
                    Button_Cikis.Text = "Çıkış";
                    btn_Sil.Text = "Sil";
                }
                else
                {
                    btn_Giris.Text = "Login";
                    Button_Cikis.Text = "Close";
                    btn_Sil.Text = "Del";
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Size = new System.Drawing.Size(469, 678);

            DataTable dt = dbtools.SelectTable("select isnull(Param_Kartla_Giris,0) as Param_Kartla_Giris from Pos_Param where Param_Id = '1'");
            if (dt.Rows.Count > 0)
            {
                Param_Kartla_Giris = Convert.ToBoolean(dt.Rows[0]["Param_Kartla_Giris"]);
                if (Param_Kartla_Giris)
                {
                    if (Langs.Default.Dil == "tr-TR")
                    {
                        lblBilgi.Text = "Kart Numaranızı Giriniz...";
                    }
                    else
                    {
                        lblBilgi.Text = "Enter Your Card Number ...";
                    }
                    txt_Giris.Properties.UseSystemPasswordChar = true;
                }
                else
                {
                    if (Langs.Default.Dil == "tr-TR")
                    {
                        lblBilgi.Text = "Kullanıcı Kodunuzu Giriniz...";
                    }
                    else
                    {
                        lblBilgi.Text = "Enter Your User Code ...";
                    }

                }
            }
        }


        bool yeniCikis = false;
        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            ConfirmationForm confirmationForm = new ConfirmationForm(res_man.GetString("PROGRAMI KAPATMAK İSTEDİĞİNİZE EMİN MİSİNİZ?"));
            confirmationForm.ShowDialog();
            if (confirmationForm.onay)
            {
                Cikis = true;
                xCikis = false;
                yeniCikis = true; // 28.11.2022 de eklendi
                this.Close();
            }
           
        }

        public static string MyClass = "frmLogin";
        private void btn_Giris_Click(object sender, EventArgs e)
        {
            try
            {
                if (Durum == "Giris")
                {
                    if (txt_Giris.Text.Length > 0)
                    {
                        DataTable dt_P_User;

                        if (Param_Kartla_Giris)    //Kart ile Giriş
                        {
                            dt_P_User = Rmosmuh.SelectTable("select * from Pos_User with(nolock) where P_Kart = '" + txt_Giris.Text + "' ");
                            if (dt_P_User.Rows.Count > 0)
                            {
                                User.P_Kod = Convert.ToString(dt_P_User.Rows[0]["P_Kod"]);
                                User.P_Sifre = Convert.ToString(dt_P_User.Rows[0]["P_Sifre"]);
                                User.P_Ad = Convert.ToString(dt_P_User.Rows[0]["P_Ad"]);
                                User.P_Soyad = Convert.ToString(dt_P_User.Rows[0]["P_Soyad"]);
                                User.P_Kart = Convert.ToString(dt_P_User.Rows[0]["P_Kart"]);

                                User.Yetki_Yukle();



                                xCikis = true;
                                yeniCikis = true;
                                this.Close();
                            }
                        }
                        else     //Kullanıcı Kodu - Şifre ile giriş
                        {
                            // Kullanıcı Kodu Kontrolü
                            if (Giris == 0)
                            {
                                dt_P_User = Rmosmuh.SelectTable("select * from Pos_User with(nolock) where P_Kod = '" + txt_Giris.Text + "'");
                                if (dt_P_User.Rows.Count > 0)
                                {
                                    Giris = 1;

                                    User.P_Kod = txt_Giris.Text;
                                    txt_Giris.Text = String.Empty;
                                    txt_Giris.Properties.UseSystemPasswordChar = true;

                                    if (Langs.Default.Dil == "tr-TR")
                                    {
                                        lblBilgi.Text = "Kullanıcı Şifrenizi Giriniz...";
                                    }
                                    else
                                    {
                                        lblBilgi.Text = "Enter Your User Password ...";
                                    }
                                }
                                else
                                {
                                    User.P_Kod = String.Empty;
                                    txt_Giris.Text = String.Empty;
                                    txt_Giris.Properties.UseSystemPasswordChar = false;
                                    if (Langs.Default.Dil == "tr-TR")
                                    {
                                        lblBilgi.Text = "Kullanıcı Kodunuzu Giriniz...";
                                    }
                                    else
                                    {
                                        lblBilgi.Text = "Enter Your User Code ...";
                                    }
                                }
                            }

                            //Kullanıcı Şifresi Kontrolü
                            if (Giris == 1 && txt_Giris.Text != String.Empty)
                            {
                                dt_P_User = Rmosmuh.SelectTable("select * from Pos_User with(nolock) where P_Kod = '" + User.P_Kod + "' and P_Sifre = '" + txt_Giris.Text + "' ");
                                if (dt_P_User.Rows.Count > 0)
                                {
                                    User.P_Kod = Convert.ToString(dt_P_User.Rows[0]["P_Kod"]);
                                    User.P_Sifre = Convert.ToString(dt_P_User.Rows[0]["P_Sifre"]);
                                    User.P_Ad = Convert.ToString(dt_P_User.Rows[0]["P_Ad"]);
                                    User.P_Soyad = Convert.ToString(dt_P_User.Rows[0]["P_Soyad"]);
                                    User.P_Kart = Convert.ToString(dt_P_User.Rows[0]["P_Kart"]);

                                    User.Yetki_Yukle();

                                    xCikis = true;
                                    yeniCikis = true;
                                    this.Close();
                                }
                                else
                                {
                                    if (Langs.Default.Dil == "tr-TR")
                                    {
                                        lblBilgi.Text = "Yanlış Şifre Girdiniz...";
                                    }
                                    else
                                    {
                                        lblBilgi.Text = "Wrong Password ...";
                                    }
                                    txt_Giris.Text = String.Empty;
                                }
                            }
                        }
                        txt_Giris.Focus();
                    }
                }
                else if (Durum == "Satir")
                {
                    if (txt_Giris.Text.Length > 0)
                    {
                        int dt_P_User = Convert.ToInt32(Rmosmuh.DegerGetir("select ISNULL(Count(*),0) from Pos_User with(nolock) where P_Kart = '" + txt_Giris.Text + "' and Pos_SatirSilYetkili = 1"));
                        if (dt_P_User > 0)
                        {
                            DonenDeger = true;
                        }
                        else
                        {
                            DonenDeger = false;
                        }

                        xCikis = true;
                        yeniCikis = true;
                        this.Close();
                    }
                }
                else if (Durum == "YS")
                {



                    if (txt_Giris.Text.Length > 0)
                    {
                        int dt_P_User = Convert.ToInt32(Rmosmuh.DegerGetir("select ISNULL(Count(*),0) from Pos_User with(nolock) where P_Kart = '" + txt_Giris.Text + "' and Pos_YS_YetkiReddet = 1"));
                        if (dt_P_User > 0)
                        {
                            DonenDeger = true;
                        }
                        else
                        {
                            DonenDeger = false;
                        }

                        xCikis = true;
                        yeniCikis = true;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btn_Giris_Click", "", ex);
            }

            StatikSinif.dilDegis("tr-TR");

        }



        public string sonuc = string.Empty;
        void tikla_click(object sender, EventArgs e)
        {
            Control mybutton = (Control)sender;
            Departman.Dep_Kodu = Convert.ToString(mybutton.Tag);

            sonuc = Departman.Dep_Param_Yukle();

            //Departman Parametreleri ve Önbüro adresi yüklendi mi?
            if (sonuc == "OK")
            {
                Param.Param_Yukle();
                FisPr.Param_Yukle();
            }
            else
            {
                MessageBox.Show(sonuc + "\n" + "Departman Parametreleri Yüklenemedi...");
            }

            this.Close();
        }


        private void btn_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            txt_Giris.Text += btn.Text;
            if (txt_Giris.Text == "19830126")
            {
                btn_Geri.Enabled = true;
            }
            else
            {
                btn_Geri.Enabled = false;
            }
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void btn_Sil_Click(object sender, EventArgs e)
        {
            txt_Giris.Text = String.Empty;
            if (Param_Kartla_Giris)
            {
                if (Langs.Default.Dil == "tr-TR")
                {
                    lblBilgi.Text = "Kart Numaranızı Giriniz...";
                }
                else
                {
                    lblBilgi.Text = "Enter Your Card Number ...";
                }
            }
            else
            {
                if (Langs.Default.Dil == "tr-TR")
                {
                    lblBilgi.Text = "Kullanıcı Kodunuzu Giriniz...";
                }
                else
                {
                    lblBilgi.Text = "Enter Your User Code ...";
                }
            }
            Giris = 0;
        }

        private void btn_Geri_Click(object sender, EventArgs e)
        {
            Language d = new Language();
            d.ShowDialog();
        }

        private void txt_Giris_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (txt_Giris.Text == "19830126")
            {
                btn_Geri.Enabled = true;
            }
            else
            {
                btn_Geri.Enabled = false;
            }
        }

        private bool _altF4Pressed = false;
        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_altF4Pressed)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                    e.Cancel = true;
                _altF4Pressed = false;
            }

            if (yeniCikis == false)
            {
                e.Cancel = true;
            }
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                _altF4Pressed = true;
            }
        }
    }
}