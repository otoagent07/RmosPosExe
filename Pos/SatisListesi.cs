using AxWMPLib;
using Pos.Class;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Pos
{
    public partial class SatisListesi : DevExpress.XtraEditors.XtraForm
    {
        public SatisListesi()
        {
            InitializeComponent();
        }

        public static int Fisno = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Fisno > 0)
            {
                Listele(Fisno);
            }
            else
            {
                gridControl1.DataSource = null;
            }
        }

        public static int sabitSaniye = 30;

        public static int saniye = sabitSaniye;
        public void Listele(int Fisno)
        {
            saniye = sabitSaniye;
            //RefreshVideo();

            decimal Tutar = 0;
            DataTable dt = dbtools.SelectTable("Exec Pos_Satis @Fisno = '" + Fisno + "',@Rapor_Tipi = '2', @Split = 0");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["Rsat_Ba"]) == "B")
                    {
                        Tutar += Convert.ToDecimal(dt.Rows[i]["Rsat_Tutar"]);
                    }
                }

                gridControl1.DataSource = dt;

                lbl_Masa.Text = Convert.ToString(dt.Rows[0]["Rsat_Masa"]) + " - " + Convert.ToString(dt.Rows[0]["MasaKonumAdi"]);
                lbl_Fisno.Text = Fisno.ToString();
                lbl_Kasiyer.Text = Convert.ToString(dt.Rows[0]["Kasiyer"]);
                lbl_Toplam.Text = Tutar.ToString("n2");
                lbl_Kalan.Text = Convert.ToDecimal(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 21,@Fisno = '" + Fisno + "', @Split = '0'")).ToString("n2");
            }
            else
            {
                gridControl1.DataSource = null;
                lbl_Masa.Text = " ";
                lbl_Fisno.Text = " ";
                lbl_Kasiyer.Text = " ";
                lbl_Toplam.Text = "0,00";
                lbl_Kalan.Text = "0,00";
            }
        }


        private void SatisListesi_Load(object sender, EventArgs e)
        {
            //timer1.Enabled = true;
            try
            {
                lbl_TesisAdi.Text = Param.Tesis_Adi.Replace("&", "&&");
                if (File.Exists("sirketLogo.png"))
                {
                    pictureBox1.Image = Image.FromFile("sirketLogo.png");
                }
                if (File.Exists("rmosLogo.png"))
                {
                    pictureBox2.Image = Image.FromFile("rmosLogo.png");
                }

                //pictureBox3.Image = Image.FromFile("modulLogo.png");

                layoutView1.OptionsView.ShowHeaderPanel = false;
                layoutView1.OptionsView.ShowCardCaption = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        public void slaytGoster()
        {
            try
            {
                pictureBoxSlayt.Visible = true;
                if (File.Exists("sirketSlaytLogo.png"))
                {
                    pictureBoxSlayt.Image = Image.FromFile("sirketSlaytLogo.png");
                }
                panel2.Visible = false;
                panel1.Dock = System.Windows.Forms.DockStyle.Fill; // sonradan right olucak
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;

                if (File.Exists("sirketSlaytLogo.mp4"))
                {
                    player.Visible = true;
                    PlayMp4InPanel();
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void slaytKapat()
        {
            pictureBoxSlayt.Visible = false;
            panel2.Visible = true;
            panel1.Dock = System.Windows.Forms.DockStyle.Right; // sonradan right olucak
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;

            player.Visible = false;
        }

        bool birkere = false;
        public void PlayMp4InPanel()
        {
            try
            {
                if (birkere) return;
                //player.CreateControl();

                player.Dock = DockStyle.Fill;
                string videoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sirketSlaytLogo.mp4");
                player.URL = videoPath;
                player.uiMode = "none"; // controls göster
                player.settings.autoStart = true;

                panelSlayt.Controls.Clear(); // Öncekileri temizle
                panelSlayt.Controls.Add(this.player);
                birkere = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void timerSlayt_Tick(object sender, EventArgs e)
        {
            if (saniye > 0)
            {
                saniye--;
            }

            if (saniye == 0)
            {
                slaytGoster();
            }
            else
            {
                slaytKapat();
            }


        }

        public void RefreshVideo()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sirketSlaytLogo.mp4");
            if (File.Exists(path))
            {
                player.Ctlcontrols.stop();
                player.URL = "";
                player.URL = path;
                player.Ctlcontrols.play();
            }
        }



        private void player_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8)
            {
                birkere = false;
            }
        }
    }
}