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
    public partial class TipboxForm : DevExpress.XtraEditors.XtraForm
    {
        CheckButton chk_Sec = null;

        public string indTipi = String.Empty;
        public decimal indSayi = 0;
        bool ilk = true;

        public decimal tutar = 0;

        public decimal toplamTutar = 0;

        public TipboxForm()
        {
            InitializeComponent();
        }

        private void Indirim_Load(object sender, EventArgs e)
        {
            this.BringToFront();
        }


        public bool cikisyapti = false;

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

            indTipi = "T";
            indSayi = Convert.ToDecimal(btn.Tag);

            this.Close();
        }

        private void btn_Sayi_Click(object sender, EventArgs e)
        {
            if (ilk)
            {
                txt_Sayi.Text = String.Empty;
            }

            SimpleButton btn = (SimpleButton)sender;
            txt_Sayi.Text += btn.Text;
            ilk = false;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            indTipi = "T";
            indSayi = Convert.ToDecimal(txt_Sayi.Text);
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