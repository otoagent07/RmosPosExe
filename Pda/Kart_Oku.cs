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
    public partial class Kart_Oku : DevExpress.XtraEditors.XtraForm
    {
        public string Ozel_Masa_Ad { get; set; }

        public Kart_Oku()
        {
            InitializeComponent();
        }

        private void Kart_Oku_Load(object sender, EventArgs e)
        {

        }

        private void btn_Ara_Click(object sender, EventArgs e)
        {
            OdaBul();
            this.Close();
        }

        private void OdaBul()
        {
            if (txt_KartNo.Text.Length < 1)
            {
                return;
            }

            DataTable dt = Fronttools.SelectTable("select top 1 Rez_Odano,Rez_Adi_1 +' '+ Rez_Adi_2 as AdSoyad, Rez_Kartno,Rez_Konaklama,Rez_Giris_tarihi,Rez_Cikis_tarihi, "
                                                + " convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,Ac_Adi "
                                                + " from Rez left join Acenta on Rez_Macenta = Acenta.Ac_Kodu "
                                                + " where Rez_Kartno = '" + txt_KartNo.Text + "' and Rez_R_I_H = 'I'");
            if (dt.Rows.Count > 0)
            {
                lblHata.Text = "-";

                lblAdSoyad.Text = Convert.ToString(dt.Rows[0]["AdSoyad"]);
                lblOdaNo.Text = Convert.ToString(dt.Rows[0]["Rez_Odano"]);
                lblKartNo.Text = Convert.ToString(dt.Rows[0]["Rez_Kartno"]);

                Ozel_Masa_Ad = String.Format("{0} {1}", lblOdaNo.Text, lblAdSoyad.Text);
                Ozel_Masa_Ad = Ozel_Masa_Ad.Length > 40 ? Ozel_Masa_Ad.Substring(0, 40) : Ozel_Masa_Ad;
            }
            else
            {
                lblHata.Text = "Oda Bulunamadı...";

                lblAdSoyad.Text = "-";
                lblOdaNo.Text = "-";
                lblKartNo.Text = "-";

                Ozel_Masa_Ad = String.Empty;
            }
        }


    }
}