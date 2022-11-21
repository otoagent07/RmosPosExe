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
    public partial class Oda_Kontrol : DevExpress.XtraEditors.XtraForm
    {
        public Oda_Kontrol()
        {
            InitializeComponent();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            OdaBul();
        }

        private void OdaBul()
        {
            if (txtKartOdaNo.Text.Length < 1 )
            {
                return;
            }

            string Rez_Kartno = "Rez_Kartno";
            string Kimlik_Kart = "Kimlik_Kart";
            string kartno = Convert.ToString(txtKartOdaNo.EditValue);

            string[] ozelKarakter = Param.Param_Kart_Yoksay.Split('#');
            if (ozelKarakter.Length > 0 && Param.Param_Kart_Yoksay != "")
            {
                for (int i = 0; i < ozelKarakter.Length; i++)
                {
                    Rez_Kartno = "REPLACE(" + Rez_Kartno + ",'" + ozelKarakter[i] + "','')";
                    Kimlik_Kart = "REPLACE(" + Kimlik_Kart + ",'" + ozelKarakter[i] + "','')";
                    kartno = kartno.Replace(ozelKarakter[i], "");
                }
            }

            DataTable dt = Fronttools.SelectTable("select top 1 Rez_Odano,Rez_Adi_1 +' '+ Rez_Adi_2 as AdSoyad, Rez_Kartno,Rez_Konaklama,Rez_Giris_tarihi,Rez_Cikis_tarihi, "
                                                + " convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,Ac_Adi "
                                                + " from Rez left join Acenta on Rez_Macenta = Acenta.Ac_Kodu "
                                                + " where (Rez_Odano = '" + txtKartOdaNo.EditValue + "' or " + Rez_Kartno + " = '" + kartno + "') and Rez_Master_detay = 'M' and Rez_R_I_H = 'I'");
            if (dt.Rows.Count > 0)
            {
                lblHata.Text = "-";

                lblAdSoyad.Text = Convert.ToString(dt.Rows[0]["AdSoyad"]);
                lblOdaNo.Text = Convert.ToString(dt.Rows[0]["Rez_Odano"]);
                lblKartNo.Text = Convert.ToString(dt.Rows[0]["Rez_Kartno"]);
                lblKonaklama.Text = Convert.ToString(dt.Rows[0]["Rez_Konaklama"]);
                lblGirisTarihi.Text = Convert.ToDateTime(dt.Rows[0]["Rez_Giris_tarihi"].ToString()).ToShortDateString();
                lblCikisTarihi.Text = Convert.ToDateTime(dt.Rows[0]["Rez_Cikis_tarihi"].ToString()).ToShortDateString();
                lblKisi.Text = Convert.ToString(dt.Rows[0]["Kisi"]);
                lblAcenta.Text = Convert.ToString(dt.Rows[0]["Ac_Adi"]);
            }
            else
            {
                lblHata.Text = "Oda Bulunamadı...";

                lblAdSoyad.Text = "-";
                lblOdaNo.Text = "-";
                lblKartNo.Text = "-";
                lblKonaklama.Text = "-";
                lblGirisTarihi.Text = "-";
                lblCikisTarihi.Text = "-";
                lblKisi.Text = "-";
                lblAcenta.Text = "-";
            }
        }

        private void txtKartOdaNo_EditValueChanged(object sender, EventArgs e)
        {
            OdaBul();
        }

        private void Oda_Kontrol_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;
        }
    }
}