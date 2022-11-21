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
    public partial class Pos_ExtraFolio_HarcamaDetayi : DevExpress.XtraEditors.XtraForm
    {
        public Pos_ExtraFolio_HarcamaDetayi()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public int KartID, FolioID;

        private void Listele()
        {
            gridView1.Columns.Clear();
            gridView1.BestFitColumns();
            gridControl1.DataSource = dbtools.SelectTable("Exec Pos_Satis_Rapor @Rapor_Tipi = '27', @KartID = '" + KartID + "', @FolioID = '" + FolioID + "', @Departman = '" + Departman.Dep_Kodu + "'");

            txt_AdSoyad.Text = Fronttools.CardFIsim(KartID);
            txt_Bakiye.Text = Fronttools.NFCBakiye(FolioID, KartID).ToString("n2");

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (gridControl1.DataSource != null)
            {
                FisPr pr = new FisPr();
                pr.ExtraFolioDetayDokum(KartID, FolioID);

            }
        }

        private void btn_Adisyonpr_Click(object sender, EventArgs e)
        {
            AdisyonPr ads = new AdisyonPr();
            ads.AdisyonKartID_Yaz(Convert.ToInt32(KartID), true);
        }

        private void btn_Faturapr_Click(object sender, EventArgs e)
        {
            Fis_Islem.Fatura_Kes(Convert.ToInt32(KartID), false, true, "H");
        }

        private void Pos_ExtraFolio_HarcamaDetayi_Load(object sender, EventArgs e)
        {
            Listele();
        }
    }
}