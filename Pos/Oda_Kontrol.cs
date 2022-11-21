using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pos
{
    public partial class Oda_Kontrol : DevExpress.XtraEditors.XtraForm
    {
        public Oda_Kontrol()
        {
            InitializeComponent();
            this.BringToFront();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            txt_Arama.Text += btn.Text;
        }

        private void Back_Space_Click(object sender, EventArgs e)
        {
            if (txt_Arama.Text.Length > 0)
            {
                txt_Arama.Text = txt_Arama.Text.Substring(0, txt_Arama.Text.Length - 1);
            }
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (txt_Arama.Text.Length > 0)
            {

                string Rez_Kartno = "Rez_Kartno";
                string Kimlik_Kart = "Kimlik_Kart";
                string kartno = txt_Arama.Text;

                string[] ozelKarakter = Param.Param_Kart_Yoksay.Split('#');
                if (ozelKarakter.Length > 0 && Param.Param_Kart_Yoksay != "")
                {
                    for (int i = 0; i < ozelKarakter.Length; i++)
                    {
                        Rez_Kartno = "REPLACE(" + Rez_Kartno + ",'" + ozelKarakter[i] + "','')";
                        Kimlik_Kart = "REPLACE(" + Kimlik_Kart + ",'" + ozelKarakter[i] + "','')";
                        kartno = kartno.Replace(ozelKarakter[i],"");
                    }
                }



                DataTable dt = Fronttools.SelectTable("select 'ODA' as Tip,Rez_Odano,Rez_Adi_1 + Rez_Adi_2 as AdSoyad, Rez_Kartno,Rez_Konaklama,Rez_Giris_tarihi,Rez_Cikis_tarihi, "
                            + " convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,Ac_Adi "
                            + " from Rez left join Acenta on Rez_Macenta = Acenta.Ac_Kodu "
                            + " where (Rez_Odano like '" + txt_Arama.Text + "%' or Rez_Adi_1 like '" + txt_Arama.Text + "%' or Rez_Adi_2 like '" + txt_Arama.Text + "%' or " + Rez_Kartno + " like '" + kartno + "%') and Rez_R_I_H = 'I' "
                            + " union all "
                            + " select 'UYE' as Tip,NULL, Kimlik_Ad + ' ' + Kimlik_Soyad,Kimlik_Kart,NULL,NULL,NULL,NULL,NULL "
                            + " from Previl "
                            + " where NULLIF(previl.Kart_Turu,'') is not null and (" + Kimlik_Kart + " like '" + kartno + "%' or Kimlik_Ad like '" + txt_Arama.Text + "&' or Kimlik_Soyad like '" + txt_Arama.Text + "%') "
                            + " order by Tip");
                if (dt.Rows.Count > 0)
                {
                    gridColumn_Acenta.FieldName = "Ac_Adi";
                    gridColumn_AdSoyad.FieldName = "AdSoyad";
                    gridColumn_GelTarihi.FieldName = "Rez_Giris_tarihi";
                    gridColumn_GidisTarihi.FieldName = "Rez_Cikis_tarihi";
                    gridColumn_Kisi.FieldName = "Kisi";
                    gridColumn_OdaNo.FieldName = "Rez_Odano";
                    gridColumn_Pansiyon.FieldName = "Rez_Konaklama";
                    gridColumn_KartNo.FieldName = "Rez_Kartno";
                    gridColumn_Tip.FieldName = "Tip";
                    gridControl1.DataSource = dt;
                    //labelControl1.Text = "Oda = " + dt.Rows[0]["Rez_Odano"].ToString() + "  Adı = " + dt.Rows[0]["Rez_Adi_1"].ToString() + " " + dt.Rows[0]["Rez_Adi_2"].ToString() + " Konaklama = " + dt.Rows[0]["Rez_Konaklama"].ToString();
                }
                else
                {
                    labelControl1.Text = "Uygun Kayıt Bulunamadı...";
                }
            }
        }


    }
}