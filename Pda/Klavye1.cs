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
    public partial class Klavye1 : DevExpress.XtraEditors.XtraForm
    {
        public decimal sayi = 0;
        int ilk = 1;
        public string deger = "";

        public Klavye1()
        {
            InitializeComponent();
        }

        private void Klavye1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;

            if (Convert.ToString(this.Tag) == "KISISOR")
            {
                lbl_Baslik.Text = "Kişi Sayısı Giriniz...";
            }
            if (Convert.ToString(this.Tag) == "MIKTARDUZELT")
            {
                lbl_Baslik.Text = "Miktar Giriniz...";
            }
            if (Convert.ToString(this.Tag) == "ODEMETUTAR")
            {
                lbl_Baslik.Text = "Tutar Giriniz...";
            }
            if (Convert.ToString(this.Tag) == "KLAVYE")
            {
                lbl_Baslik.Text = "...";
            }
            if (Convert.ToString(this.Tag) == "MALZEMETR")
            {
                lbl_Baslik.Text = "Miktar Giriniz...";
            }
            if (Convert.ToString(this.Tag) == "SATIRSIL")
            {
                lbl_Baslik.Text = "Miktar Giriniz...";
            }

            txt_Sayi.Focus();
        }

        private void btn_Sayi_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;

            if (ilk == 1) txt_Sayi.EditValue = "";
            txt_Sayi.EditValue = txt_Sayi.EditValue.ToString() + btn.Text;
            ilk = 0;
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            sayi = Convert.ToDecimal(txt_Sayi.EditValue);

            if (Convert.ToString(this.Tag) == "KISISOR")
            {

            }
            if (Convert.ToString(this.Tag) == "MIKTARDUZELT")
            {
                 if (sayi > Param.Max_Miktar)
                {
                    if (MessageBox.Show("Max Miktarı Geçtiniz...\nDevam etmek İstiyor Musunuz...?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }
            }
            if (Convert.ToString(this.Tag) == "ODEMETUTAR")
            {
                
            }

            if (Convert.ToString(this.Tag) == "KLAVYE")
            {
                deger = txt_Sayi.Text;
            }

            this.Close();
        }

        private void btn_Sil_Click(object sender, EventArgs e)
        {
            txt_Sayi.Text = "0,00";
            ilk = 1;
            txt_Sayi.Focus();
        }
    }
}