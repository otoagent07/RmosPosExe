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
    public partial class Arama : DevExpress.XtraEditors.XtraForm
    {

        public int Odeme_Ozelkod;
        public string Cari_Kod;
        public string Cari_Ad;

        public Arama()
        {
            InitializeComponent();
        }

        private void Arama_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;


            Cari_Ara();

        }

        private void Cari_Ara()
        {
            string tipFilter = "";
            if (Odeme_Ozelkod == 5)
            {
                tipFilter = " and ISNULL(Cari_Tip,'C') = 'C' ";
            }
            if (Odeme_Ozelkod == 2)
            {
                tipFilter = " and ISNULL(Cari_Tip,'C') = 'O' ";
            }
            if (Odeme_Ozelkod == 3)
            {
                tipFilter = " and ISNULL(Cari_Tip,'C') = 'P' ";
            }


            gridColumn1.FieldName = "Cari_Kod";
            gridColumn2.FieldName = "Adsoyad";


            gridColumn1.Caption = "Kod";
            gridColumn2.Caption = "Ad Soyad";


            gridControl1.DataSource = dbtools.SelectTable("select Cari_Kod,Cari_Ad +' '+Cari_Soyad as Adsoyad,Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Adres1,Cari_Adres2,Cari_Adres3,Cari_Kart from Pos_Cari where 1=1 " + tipFilter);
            gridView1.BestFitColumns();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            Cari_Kod = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
            Cari_Ad = Convert.ToString(gridView1.GetFocusedRowCellValue("Adsoyad"));
            this.Close();
        }

    }
}