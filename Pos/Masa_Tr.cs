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
    public partial class Masa_Tr : DevExpress.XtraEditors.XtraForm
    {
        public Masa_Tr()
        {
            InitializeComponent();
        }

        private void Masa_Tr_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            gridyenile_Konumlar();
            gridyenile_Masa();
        }

        public string OzelMasaAdi = string.Empty;

        private void gridyenile_Masa()
        {
            string filtre = String.Empty;

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Kod")) != "")
            {
                filtre = " and Masa_Konum = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Kod")) + "' ";
            }

            gridControl2.DataSource = dbtools.SelectTable("select Masa_No, Masa_Ad from Pos_Masa with(nolock) where Masa_Durum = 0 and Masa_Paket = 0 and Masa_Depart = '" + Departman.Dep_Kodu + "' " + filtre + " and Masa_No not like '%[_]%' order by Masa_No ");
        }

        private void gridyenile_Konumlar()
        {
            DataTable dt = dbtools.SelectTable("SELECT Pkod_Konumkod, Pkod_Ad FROM Pos_Kodlar with(nolock)  WHERE Pkod_Sinif = '14' AND Pkod_Kod = '" + Departman.Dep_Kodu + "' order by Pkod_Konumkod ");

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Kod", typeof(string));
            dt2.Columns.Add("Ad", typeof(string));
            dt2.Rows.Add("", "Tüm Masalar");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt2.Rows.Add(dt.Rows[i]["Pkod_Konumkod"], dt.Rows[i]["Pkod_Ad"]);
            }
            gridControl1.DataSource = dt2;
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            gridyenile_Masa();
        }

        private void btn_Transfer_Click(object sender, EventArgs e)
        {

            string suankiMasaDurum = dbtools.DegerGetir("select ISNULL(Masa_Durum,0) as Masa_Durum from Pos_Masa where Masa_No='" + txt_Masano.Text + "'");

            string hedefMasaNo2 = Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No"));

            if (StatikSinif.masaMusaitmi(hedefMasaNo2) == false)
            {
                return;
            }

           


            if (Departman.Siparis && Param.Param_Masatr_Uyari)
            {
                FisPr pr = new FisPr();
                string sonuc = pr.MasaTr(Convert.ToInt32(this.Tag), txt_Masano.Text, Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")));
                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc, "Uyarı");
                }
            }

            //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Masa = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' where Rsat_Fisno = '" + this.Tag + "' ");
            //dbtools.execcmd("UPDATE Pos_Masa SET Masa_Durum = 0,Masa_Ozel ='' WHERE Masa_No = '" + txt_Masano.Text + "' and  Masa_Depart = '" + Departman.Dep_Kodu + "' ");
            //dbtools.execcmd("UPDATE Pos_Masa SET Masa_Durum = 1 WHERE Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' and  Masa_Depart = '" + Departman.Dep_Kodu + "' ");

            if (Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) == "")
            {
                return;
            }


            dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 12, @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Yeni_Masano = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "',@Masano = '" + txt_Masano.Text + "',@Fisno = '" + this.Tag + "', @OzelMasaAdi = '" + OzelMasaAdi + "'");


            dbtools.execcmdR("update Pos_Masa set Masa_Durum ='"+ suankiMasaDurum + "' where Masa_No='"+ hedefMasaNo2 + "'");


            Log.Log_Kaydet(Log.Log_Program.Pos,Log.Log_Bolum.Masa_Transfer, Log.Log_Islem.Duzelt, txt_Masano.Text + " NOLU Masa " + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + " NOLU MASAYA TRANSFER OLDU", this.Tag.ToString(), "");


            this.Close();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Masa_Tr_Shown(object sender, EventArgs e)
        {
            StatikSinif.masaKilitle(txt_Masano.Text);
        }

       
    }
}