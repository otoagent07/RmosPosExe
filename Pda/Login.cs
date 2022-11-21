using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pda
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        bool Param_Kartla_Giris = false;
        public bool Cikis = false;

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select isnull(Param_Kartla_Giris,0) as Param_Kartla_Giris from Pos_Param WITH(NOLOCK) where Param_Id = '1'");
            if (dt.Rows.Count > 0)
            {
                Param_Kartla_Giris = Convert.ToBoolean(dt.Rows[0]["Param_Kartla_Giris"]);
                if (Param_Kartla_Giris)
                {
                    lbl_Baslik.Text = "Kart Numaranızı Giriniz...";
                    txt_Giris.Properties.UseSystemPasswordChar = true;
                }
                else
                {
                    lbl_Baslik.Text = "Kullanıcı Kodunuzu Giriniz...";
                    txt_Giris.Properties.UseSystemPasswordChar = false;
                }
            }
        }

        int Giris = 0;
        private void btn_OK_Click(object sender, EventArgs e)
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
                            lbl_Baslik.Text = "Kullanıcı Şifrenizi Giriniz...";
                        }
                        else
                        {
                            User.P_Kod = String.Empty;
                            txt_Giris.Text = String.Empty;
                            txt_Giris.Properties.UseSystemPasswordChar = false;
                            lbl_Baslik.Text = "Kullanıcı Kodunuzu Tekrar Giriniz...";
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

                            this.Close();
                        }
                        else
                        {
                            lbl_Baslik.Text = "Yanlış Şifre Girdiniz...";
                            txt_Giris.Text = String.Empty;
                        }
                    }
                }
                txt_Giris.Focus();
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            txt_Giris.Text = txt_Giris.Text + btn.Text;
        }

        private void btn_Sil_Click(object sender, EventArgs e)
        {
            txt_Giris.Text = String.Empty;
            if (Param_Kartla_Giris)
            {
                lbl_Baslik.Text = "Kart Numaranızı Giriniz...";
            }
            else
            {
                lbl_Baslik.Text = "Kullanıcı Kodunuzu Giriniz...";
            }
            Giris = 0;
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
            Cikis = true;
        }
    }
}