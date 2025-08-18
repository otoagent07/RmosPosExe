using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pos
{
    public partial class Indirim : DevExpress.XtraEditors.XtraForm
    {
        CheckButton chk_Sec = null;

        public string indTipi = String.Empty;
        public decimal indSayi = 0;
        bool ilk = true;

        public decimal tutar = 0;

        public decimal toplamTutar = 0;

        public Indirim()
        {
            InitializeComponent();
        }

        private void Indirim_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
        }

        private void chk_Secim_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton chk = (CheckButton)sender;

            if (chk_Sec == null)
            {
                chk_Sec = chk;
            }
            if (chk_Sec == chk_Yuzde)
            {
                chk_Yuzde.Checked = true;
                chk_Tutar.Checked = false;
                chk_MY.Checked = false;
                xtraTabControl1.SelectedTabPage = tab_Yuzde;
            }

            if (chk_Sec == chk_Tutar)
            {
                chk_Yuzde.Checked = false;
                chk_Tutar.Checked = true;
                chk_MY.Checked = false;
                xtraTabControl1.SelectedTabPage = tab_Tutar;
            }

            if (chk_Sec == chk_MY)
            {
                chk_Yuzde.Checked = false;
                chk_Tutar.Checked = false;
                chk_MY.Checked = true;
                xtraTabControl1.SelectedTabPage = tab_MY;
            }


            chk_Sec = null;
        }

        public bool cikisyapti = false;
        public bool degergirildi = false;

        private void btn_Y_Cikis_Click(object sender, EventArgs e)
        {
            cikisyapti = true;
            indTipi = String.Empty;
            indSayi = 0;
            this.Close();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            indTipi = String.Empty;
            indSayi = 0;
            this.Close();
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void btn_Y_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;

            indTipi = "Y";
            string sayi = btn.Tag.ToString();
            indSayi = Convert.ToDecimal(sayi);

            if (Convert.ToString(this.Tag) == "I")
            {
                if (indSayi > User.P_Indirim_Yuzde)
                {
                    MessageBox.Show("Max Indirim Yuzdesini Aştınız...");
                   // MessageBox.Show(res_man.GetString("Max Indirim Yuzdesini Aştınız..."));
                    return;
                }
            }
            if (Convert.ToString(this.Tag) == "B")
            {
                if (indSayi > User.P_Bindirim_Yuzde)
                {
                    MessageBox.Show("Max Servis Payı Yuzdesini Aştınız...");
                    //MessageBox.Show(res_man.GetString("Max Servis Payı Yuzdesini Aştınız..."));
                    return;
                }
            }


            if (Convert.ToString(this.Tag) == "MB")
            {
                if (indSayi > User.P_Bindirim_Yuzde)
                {
                    MessageBox.Show("Max Bindirim Yuzdesini Aştınız...");
                    return;
                }
            }

            degergirildi = true;
            this.Close();
        }

        private void btn_Sayi_Click(object sender, EventArgs e)
        {
            if (ilk)
            {
                if (chk_Tutar.Checked)
                {
                    txt_Sayi.Text = String.Empty;
                }
                if (chk_MY.Checked)
                {
                    txt_ManuelYuzde.Text = String.Empty;
                }
            }
            SimpleButton btn = (SimpleButton)sender;
            if (chk_Tutar.Checked)
            {
                txt_Sayi.Text += btn.Text;
            }
            if (chk_MY.Checked)
            {
                txt_ManuelYuzde.Text += btn.Text;
            }

            ilk = false;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {


            indTipi = "T";

            if (Tag.Equals("B")) // bindirim
            {

            }

            if (xtraTabControl1.SelectedTabPage.Name== "tab_Tutar")
            {
                indSayi = Convert.ToDecimal(txt_Sayi.Text);
                degergirildi = true;
                this.Close();
                return;
            }

            decimal oran = (Convert.ToDecimal(txt_Sayi.Text) / tutar) * 100;
         
            if (oran > User.P_Indirim_Yuzde)
            {
                MessageBox.Show("Max Indirim Yuzdesini Aştınız..." + "\n" + "Max İndirim Yüzdeniz : %" + User.P_Indirim_Yuzde.ToString() + "\n" + "Şuan ki İndirim Oranı : %" + oran.ToString());
                return;
            }

            indSayi = Convert.ToDecimal(txt_Sayi.Text);
            degergirildi = true;

            this.Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            indSayi = Convert.ToDecimal(txt_ManuelYuzde.Text);

            if (Convert.ToString(this.Tag) == "I")
            {
                if (Convert.ToDecimal(txt_ManuelYuzde.Text) > User.P_Indirim_Yuzde)
                {
                    MessageBox.Show("Max Indirim Yuzdesini Aştınız..." + "\n" +"Max İndirim Yüzdeniz : %" + User.P_Indirim_Yuzde.ToString() );
                    return;
                }
            }

            if (Convert.ToString(this.Tag) == "B")
            {
                if (indSayi > User.P_Bindirim_Yuzde)
                {
                    MessageBox.Show("Max Servis Payı Yuzdesini Aştınız...");
                    return;
                }
            }


            if (Convert.ToString(this.Tag) == "MB")
            {
                if (indSayi > User.P_Bindirim_Yuzde)
                {
                    MessageBox.Show("Max Bindirim Yuzdesini Aştınız...");
                    return;
                }
            }

            indTipi = "MY";
            degergirildi = true;
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            indTipi = String.Empty;
            indSayi = 0;
            this.Close();
        }
    }
}