using Pos.Class;
using System;

namespace Pos
{
    public partial class Hesap_Tr : DevExpress.XtraEditors.XtraForm
    {
        public string Masa_No { get; set; }
        public int Fisno { get; set; }

        public Hesap_Tr()
        {
            InitializeComponent();
        }

        private void Hesap_Tr_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Text += " Fisno : " + Fisno.ToString();
            txt_Masano.Text = Masa_No;
            gridyenile1();
        }

        private void gridyenile1()
        {
            gridControl1.DataSource = dbtools.SelectTable("select Kodlar_Kod,Kodlar_Ad from Stok_Kodlar WITH(NOLOCK) where ISNULL(Kodlar_Satis,0) = 1 and Kodlar_Kod <> '" + Departman.Dep_Kodu + "' order by Kodlar_Kod ");
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            gridyenile2();
        }

        private void gridyenile2()
        {
            gridControl2.DataSource = dbtools.SelectTable("select Masa_No,Masa_Ad,Masa_Durum,Masa_Ozel from Pos_Masa WITH(NOLOCK) Where Masa_Depart = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Kodlar_Kod")) + "' order by Masa_Sirano");
        }

        private void btn_Transfer_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0 && gridView2.RowCount > 0)
            {
                int hedefFisno = Fisno;
                if (Convert.ToString(dbtools.DegerGetir("select Masa_Durum from Pos_Masa where Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' and Masa_Depart = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Kodlar_Kod")) + "'")) != "0")
                {
                    hedefFisno = Convert.ToInt32(dbtools.DegerGetir("select top 1 Rsat_Fisno from Cst_Recete_Satis where Rsat_Masa = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' and Rsat_Departman = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Kodlar_Kod")) + "' and (Rsat_Durum = 'A')"));
                }
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Masa = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' , Rsat_Departman = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Kodlar_Kod")) + "', Rsat_Fisno = '" + hedefFisno + "' where Rsat_Fisno = '" + Fisno + "'");

                dbtools.execcmd("update Pos_Masa set Masa_Durum = 0, Masa_Ozel = '' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                dbtools.execcmd("update Pos_Masa set Masa_Durum = 1 where Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' and Masa_Depart = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Kodlar_Kod")) + "'");

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap_Transfer, Log.Log_Islem.Kaydet, Departman.Dep_Kodu + "-" + Departman.Dep_Adi + " " + Masa_No + " NL Masa Hesabı " +
                    Convert.ToString(gridView1.GetFocusedRowCellValue("Kodlar_Kod")) + "-" + Convert.ToString(gridView1.GetFocusedRowCellValue("Kodlar_Ad")) + " " + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + " NL Masaya Transfer Edildi.", Fisno.ToString(), "");
                this.Close();
            }
        }



    }
}