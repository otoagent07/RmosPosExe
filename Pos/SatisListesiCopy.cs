using Pos.Class;
using System;
using System.Data;

namespace Pos
{
    public partial class SatisListesiCopy : DevExpress.XtraEditors.XtraForm
    {
        public SatisListesiCopy()
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

        public void Listele(int Fisno)
        {
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
                lbl_Kalan.Text = dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 21,@Fisno = '" + Fisno + "', @Split = '0'");
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
            lbl_TesisAdi.Text = Param.Tesis_Adi;
        } 
    }
}