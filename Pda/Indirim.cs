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
    public partial class Indirim : DevExpress.XtraEditors.XtraForm
    {
        public string indTipi = String.Empty;
        public decimal indSayi = 0;
        bool ilk = true;

        public Indirim()
        {
            InitializeComponent();
        }

        private void Indirim_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;
        }

        private void btn_Y_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;

            indTipi = "Y";
            indSayi = Convert.ToDecimal(btn.Tag);

            this.Close();
        }

        private void btn_Sayi_Click(object sender, EventArgs e)
        {
            if (ilk) txt_Sayi.Text = String.Empty;
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

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Cikis2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}