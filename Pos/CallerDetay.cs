using Pos.Class;
using System;
using System.Data;

namespace Pos
{
    public partial class CallerDetay : DevExpress.XtraEditors.XtraForm
    {
        public string Tel { get; set; }
        public CallerDetay()
        {
            InitializeComponent();
        }

        private void CallerDetay_Load(object sender, EventArgs e)
        {
            lbl_Tel.Text = "Tel No : "+Tel;

            ekranyenile();
        }

        int F_Id = 0;

        private void ekranyenile()
        {
            gridControl1.DataSource = dbtools.SelectTable("select Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Adres1 + ' ' +Cari_Adres2 + ' ' +Cari_Adres3 as Adres from Pos_Cari where Cari_Tel = '" + Tel + "' ");
            //DataTable dt = dbtools.SelectTable("select * from Pos_Fihrist where (F_Tel1 = '" + Tel + "' or F_Tel2 = '" + Tel + "')");
            //if (dt.Rows.Count > 0)
            //{
            //    F_Id = Convert.ToInt32(dt.Rows[0]["F_Id"]);
            //    txt_Ad.Text = Convert.ToString(dt.Rows[0]["F_Ad"]);
            //    txt_Soyad.Text = Convert.ToString(dt.Rows[0]["F_Soyad"]);
            //    txt_Tel1.Text = Convert.ToString(dt.Rows[0]["F_Tel1"]);
            //    txt_Tel2.Text = Convert.ToString(dt.Rows[0]["F_Tel2"]);
            //    txt_Adres.Text = Convert.ToString(dt.Rows[0]["F_Adres"]);
            //    txt_Tarif.Text = Convert.ToString(dt.Rows[0]["F_AdresTarif"]);
            //}
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_CariEkle_Click(object sender, EventArgs e)
        {
            CariHesap hes = new CariHesap();
            hes.Tel = Tel;
            hes.ShowDialog();

            ekranyenile();
        }

        private void btn_Satis_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Masa_No,Masa_Ad,Masa_Paket  from Pos_Masa where Masa_Paket = '1' and Masa_Depart='" + Departman.Dep_Kodu + "' and Masa_Durum = '0'");
            if (dt.Rows.Count > 0)
            {
                Satis satis = new Satis();
                satis.Tag = "M";
                satis.Masa_No = Convert.ToString(dt.Rows[0]["Masa_No"]);
                satis.Masa_Paket = Convert.ToBoolean(dt.Rows[0]["Masa_Paket"]); 
                satis.Ozel_Masa = "";
                satis.Split = 0;
                satis.Splitad = "";
                satis.mCari = Cari.Cari_Getir(Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod")));
                satis.ShowDialog();
            }
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            if (F_Id > 0)
            {
                //Fishrist
                FisPr pr = new FisPr();
                pr.Fihrist_Adres(F_Id); 
            }
        }

        private void btn_Print2_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                //Cari 
                FisPr pr = new FisPr();
                pr.Cari_Adres(Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"))); 
            }
        }




    }
}