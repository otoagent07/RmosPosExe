using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Klavye1 : DevExpress.XtraEditors.XtraForm
    {
        public decimal sayi = 0;
        public bool MiktarGR = false;
        public bool Cikis = false;
        bool ilk = true;
        public string UrunAdi = "";

        public Klavye1()
        {
            InitializeComponent();
        }

        private void Klavye1_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            txt_Sayi.Focus();
            lbl_UrunAdi.Text = "";
            if (Convert.ToString(this.Tag) == "FARKLIMIKTAR")
            {
                lbl_UrunAdi.Text = UrunAdi;
                lbl_Baslik.Text = res_man.GetString("Miktar Giriniz...");
            }

            if (Convert.ToString(this.Tag) == "MIKTARDUZELT")
            {
                lbl_UrunAdi.Text = UrunAdi;
                lbl_Baslik.Text = res_man.GetString("Miktar Giriniz...");
            }

            if (Convert.ToString(this.Tag) == "ODEMETUTAR")
            {
                lbl_Baslik.Text = res_man.GetString("Odeme Tutarını Giriniz...");
            }
            if (Convert.ToString(this.Tag) == "KISISAYISI")
            {
                lbl_Baslik.Text = res_man.GetString("Kişi Sayısı Giriniz...");
                btn_Cikis.Visible = false;
            }
            if (Convert.ToString(this.Tag) == "TUTARDUZELT")
            {
                lbl_UrunAdi.Text = UrunAdi;
                lbl_Baslik.Text = res_man.GetString("Tutar Giriniz...");
            }
            if (Convert.ToString(this.Tag) == "GRAMSOR")
            {
                if (MiktarGR) lbl_Baslik.Text = res_man.GetString("GRAM Giriniz...");
                else lbl_Baslik.Text = res_man.GetString("Miktar Giriniz...");
            }
            if (Convert.ToString(this.Tag) == "MALZEMETR")
            {
                lbl_UrunAdi.Text = UrunAdi;
                lbl_Baslik.Text = res_man.GetString("Miktar Giriniz...");
            }
            if (Convert.ToString(this.Tag) == "SATIRSIL")
            {
                lbl_UrunAdi.Text = UrunAdi;
                lbl_Baslik.Text = res_man.GetString("Miktar Giriniz...");
            }
            if (Convert.ToString(this.Tag) == "ZAYISIL")
            {
                lbl_UrunAdi.Text = UrunAdi;
                lbl_Baslik.Text = res_man.GetString("Miktar Giriniz...");
            }

            //Dil Çeviri IKRAM Eklenmemiş: Dil karşılığı olarak ekledim.
            if (lbl_Baslik.Text == "Tutar Giriniz...")
            {
                lbl_Baslik.Text = res_man.GetString("Tutar Giriniz...");
            }
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            sayi = Convert.ToDecimal(txt_Sayi.Text);
            Cikis = true;
            this.Close();
        }

        private void btn_Sayi_Click(object sender, EventArgs e)
        {
            if (ilk) txt_Sayi.Text = String.Empty;
            SimpleButton btn_Sayi = (SimpleButton)sender;
            txt_Sayi.Text = txt_Sayi.Text + btn_Sayi.Text;
            ilk = false;
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());


        public bool miktarKontrol(decimal sayi)
        {
            if (sayi <= 0)
            {
                RHMesaj.MyMessageInformation("Miktar Sıfırdan Büyük Olmalıdır..!");
                return false;
            }
            return true;
        }
        public void okey()
        {
            bool cik = true;
            sayi = Convert.ToDecimal(txt_Sayi.Text.Replace(".", ","));

            if (Convert.ToString(this.Tag) == "FARKLIMIKTAR")
            {
                if (sayi <= 0)
                {
                    cik = miktarKontrol(sayi);
                    return;
                }

                if (sayi > Param.Max_Miktar)
                {
                    if (MessageBox.Show("Max Miktarı Geçtiniz..." + "\n" + "Devam etmek İstiyor Musunuz...?", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        sayi = 1;
                    }
                }
            }

            if (Convert.ToString(this.Tag) == "MIKTARDUZELT")
            {
                if (sayi <= 0)
                {
                    cik = miktarKontrol(sayi);
                    return;
                }

                if (sayi > Param.Max_Miktar)
                {
                    if (MessageBox.Show("Max Miktarı Geçtiniz..." + "\n" + "Devam etmek İstiyor Musunuz...?", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }
            }

            if (this.Text == "ODEMETUTAR")
            {

            }
            if (Convert.ToString(this.Tag) == "KISISAYISI")
            {
                if (sayi > 10)
                {
                    DialogResult c = MessageBox.Show(res_man.GetString("Kişi Sayısını 10nun üzerindendir. Devam Etmek İstiyormusunuz?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (c == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            if (Convert.ToString(this.Tag) == "TUTARDUZELT")
            {

            }
            if (Convert.ToString(this.Tag) == "GRAMSOR")
            {
                if (sayi <= 0)
                {
                    cik = miktarKontrol(sayi);
                    return;
                }
            }

            if (Convert.ToString(this.Tag) == "MALZEMETR")
            {

            }
            if (Convert.ToString(this.Tag) == "SATIRSIL")
            {

            }

            if (cik)
            {
                this.Close();
            }
        }
        private void btn_OK_Click(object sender, EventArgs e)
        {
            okey();
        }

        private void btn_Sil_Click(object sender, EventArgs e)
        {
            txt_Sayi.Text = "0,00";
            ilk = true;
            txt_Sayi.Focus();
        }

        private void txt_Sayi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '-')
            {
                e.Handled = true;
            }
        }

        private void Klavye1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                okey();
            }

        }
    }
}