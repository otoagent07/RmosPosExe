using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pos
{
    public partial class Cari_KartSor : DevExpress.XtraEditors.XtraForm
    {
        public bool Onay = false;
        public string Cari_Kod;
        public string Cari_Ad;
        public string Cari_Soyad;
        public decimal kartBakiye;

        public Cari_KartSor(decimal satisBakiye)
        {
            InitializeComponent();

            txt_SatisTutar.Text = satisBakiye.ToString();
        }

        private void Cari_KartSor_Load(object sender, EventArgs e)
        {
            
        }

        private void txt_KartNo_Leave(object sender, EventArgs e)
        {
            string kartno = txt_KartNo.Text;
            if (kartno.Length > 0)
            {
                DataTable dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Kart = '" + kartno + "'");
                if (dt.Rows.Count > 0)
                {
                    Cari_Kod = Convert.ToString(dt.Rows[0]["Cari_Kod"]);
                    Cari_Ad = Convert.ToString(dt.Rows[0]["Cari_Ad"]);
                    Cari_Soyad = Convert.ToString(dt.Rows[0]["Cari_Soyad"]);

                    lbl_AdSoyad.Text = Cari_Ad + " " + Cari_Soyad;

                    kartBakiye = Convert.ToDecimal(dbtools.DegerGetir("select ISNULL(SUM(Chrk_Alacak),0) - ISNULL(SUM(Chrk_Borc),0) from Pos_Carihrk where Chrk_Cari = '" + Cari_Kod + "'"));
                    txt_kartBakiye.Text = kartBakiye.ToString();

                    if (kartBakiye > Convert.ToDecimal(txt_SatisTutar.Text))
                    {
                        btn_OK.Enabled = true;
                    }
                    else
                    {
                        btn_OK.Enabled = false;
                    }
                }
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            Onay = true;
            this.Close();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            Onay = false;
            this.Close();
        }

        private void txt_KartNo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            txt_KartNo_Leave(null, null);
        }
    }
}