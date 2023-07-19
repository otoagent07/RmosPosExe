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
using System.Data.SqlClient;

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

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            
        }

        private void btnFisIptal_Click(object sender, EventArgs e)
        {
            try
            {
                int seciliSatir = gridView1.FocusedRowHandle;
                if (seciliSatir < 0)
                {
                    MessageBox.Show("Lütfen satır seçiniz ! ");
                    return;
                }

                string Fisno = gridView1.GetFocusedRowCellValue("Fisno").ToString();
                var Tarih = Convert.ToDateTime(gridView1.GetFocusedRowCellValue("Tarih").ToString());

                if (Tarih.Date != Param.Tarih.Date)
                {
                    MessageBox.Show("Fiş tarihi ile sistem tarihi farklı\nFiş Tarihi : "+ Tarih.Date +Environment.NewLine+"Sistem Tarihi : "+ Param.Tarih.Date);
                    return;
                }

                Klavye2 klavye = new Klavye2();
                klavye.Tag = "FISIPTAL";
                klavye.ShowDialog();
                string iptalSebep = klavye.yazi;

                if (iptalSebep.Length < 1)
                {
                    return;
                }


                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Cek_Iptal";
                com.Parameters.AddWithValue("@Fis_No", Fisno);
                com.Parameters.AddWithValue("@Users", User.P_Kod);
                com.Parameters.AddWithValue("@Rsat_IptalNot", iptalSebep);
                com.Parameters.AddWithValue("@Onb_Sil", Tarih.Date != Param.Tarih.Date ? 0 : 1);
                com.ExecuteNonQuery();
                if (con.State == ConnectionState.Open) con.Close();

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Fis_Iptal, Log.Log_Islem.Sil, Fisno + " NL Fis Silindi", Fisno, String.Empty);




                if (Departman.Kodlar_AndPos_NFC == true)
                {
                    FisPr fis = new FisPr();
                    string sonuc = fis.IptalPrNFC(Convert.ToInt32(Fisno));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Listele();
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void Pos_ExtraFolio_HarcamaDetayi_Load(object sender, EventArgs e)
        {
            btnFisIptal.Visible = User.R_Fisiptal ;

            Listele();
        }
    }
}