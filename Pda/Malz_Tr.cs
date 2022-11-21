using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;
using System.Data.SqlClient;

namespace Pda
{
    public partial class Malz_Tr : DevExpress.XtraEditors.XtraForm
    {
        public int Kaynak_Fisno;

        public Malz_Tr()
        {
            InitializeComponent();
        }

        private void Malz_Tr_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;

            gridyenile_Konum();
            gridyenile_Masa();
            gridyenile_KaynakMasa();
        }

        private void gridyenile_Konum()
        {
            look_Konum.Properties.DataSource = dbtools.SelectTable("select Pkod_Konumkod,Pkod_Ad from Pos_Kodlar where Pkod_Sinif = '14' and Pkod_Kod = '" + Departman.Dep_Kodu + "'");
            look_Konum.Properties.DisplayMember = "Pkod_Ad";
            look_Konum.Properties.ValueMember = "Pkod_Konumkod";
        }

        private void gridyenile_Masa()
        {
            string filtre = "";
            if (Convert.ToString(look_Konum.EditValue) != "")
            {
                filtre = " and Masa_Konum = '" + Convert.ToString(look_Konum.EditValue) + "' ";
            }
            DataTable dt = dbtools.SelectTable("select Masa_No, Masa_Ad, Masa_Durum from Pos_Masa with(nolock) where Masa_Paket = 0 and Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No <> '" + Convert.ToString(txt_Masano.EditValue) + "' " + filtre + " order by Masa_No");
            //look_Masa.Properties.DataSource = dt;
            //look_Masa.Properties.DisplayMember = "Masa_Ad";
            //look_Masa.Properties.ValueMember = "Masa_No";

            gridColumn19.FieldName = "Masa_No";
            gridColumn20.FieldName = "Masa_Ad";
            gridColumn21.FieldName = "Masa_Durum";

            gridLookUpEdit1.Properties.DataSource = dt;
            gridLookUpEdit1.Properties.DisplayMember = "Masa_Ad";
            gridLookUpEdit1.Properties.ValueMember = "Masa_No";
        }

        private void gridyenile_KaynakMasa()
        {
            gridColumn1.FieldName = "Rec_Ad";
            gridColumn2.FieldName = "Rsat_Miktar";
            gridColumn3.FieldName = "Rsat_Emiktar";
            gridColumn4.FieldName = "Rsat_Tutar";
            gridColumn5.FieldName = "Rsat_Doviztutar";
            gridColumn6.FieldName = "Rsat_Aciklama";
            gridColumn7.FieldName = "Rsat_Recete";
            gridColumn8.FieldName = "Rsat_SiparisPr";
            gridColumn9.FieldName = "Rsat_Id";

            if (Param.Calisma_Sekli == 1)   //Dövizli
            {
                gridColumn5.Visible = true;
                gridColumn5.VisibleIndex = 4;
            }
            else
            {
                gridColumn4.Visible = true;
                gridColumn4.VisibleIndex = 4;
            }

            //SqlConnection con = dbtools.conn;
            //if (con.State == ConnectionState.Closed) con.Open();
            //SqlCommand com = new SqlCommand();
            //com.Connection = con;
            //com.CommandType = CommandType.StoredProcedure;
            //com.CommandTimeout = 0;
            //com.CommandText = "Pos_Satis";
            //com.Parameters.AddWithValue("@Fisno", Kaynak_Fisno);
            //com.Parameters.AddWithValue("@Rapor_Tipi", 0);
            //SqlDataAdapter da = new SqlDataAdapter(com);
            //DataTable dt = new DataTable();
            //da.Fill(dt);

            DataTable dt = dbtools.SelectTable("select Rsat_Id,Rsat_Recete,Rec_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_SiparisPr "
                                                         + "  from Cst_Recete_Satis "
                                                         + "      left join Cst_Recete on Rec_Genelkod = Rsat_Recete "
                                                         + "  where Rsat_Fisno = '" + Kaynak_Fisno + "' and Rsat_Ba = 'B' and Rsat_Masa = '" + txt_Masano.EditValue + "'");

            gridControl1.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                dbtools.execcmd("update Pos_Masa set Masa_Durum = '0' where Masa_No = '" + txt_Masano.Text + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
            }
        }

        private void gridyenile_HedefMasa()
        {
            int Hedef_Fisno = Convert.ToInt32(dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Fisno from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Masa = '" + gridLookUpEdit1.EditValue + "' and Rsat_Durum = 'A' and Rsat_Departman = '" + Departman.Dep_Kodu + "'),0)"));

            gridColumn10.FieldName = "Rec_Ad";
            gridColumn11.FieldName = "Rsat_Miktar";
            gridColumn12.FieldName = "Rsat_Emiktar";
            gridColumn13.FieldName = "Rsat_Tutar";
            gridColumn14.FieldName = "Rsat_Doviztutar";
            gridColumn15.FieldName = "Rsat_Aciklama";
            gridColumn16.FieldName = "Rsat_Recete";
            gridColumn17.FieldName = "Rsat_Ba";
            gridColumn18.FieldName = "Rsat_Id";

            if (Param.Calisma_Sekli == 1)   //Dövizli
            {
                gridColumn14.Visible = true;
                gridColumn14.VisibleIndex = 3;
            }
            else
            {
                gridColumn13.Visible = true;
                gridColumn13.VisibleIndex = 3;
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Hedef_Fisno);
            com.Parameters.AddWithValue("@Rapor_Tipi", 0);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl2.DataSource = dt;
        }


        private void btn_Transfer_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(gridLookUpEdit1.EditValue) == "")
            {
                MessageBox.Show("Masa Seçiniz...");
                return;
            }

            //if (gridView1.RowCount == 1)
            //{
            //    MessageBox.Show("Son ürün Transfer Edilemez...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    return;
            //}


            if (Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar")) > 1)
            {
                Klavye1 klv = new Klavye1();
                klv.Tag = "MALZEMETR";
                klv.txt_Sayi.Text = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar")).ToString();
                klv.ShowDialog();
                decimal deger = klv.sayi;

                if (deger <= 0)
                {
                    return;
                }
                if (deger > Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar")))
                {
                    MessageBox.Show("Hatalı giriş...");
                    return;
                }

                Fis_Islem.Satir_Sil(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")),deger);

                int hedefFisno = 0;
                if (Convert.ToString(dbtools.DegerGetir("select Masa_Durum from Pos_Masa where Masa_No = '" + gridLookUpEdit1.EditValue + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'")) != "0")
                {
                    hedefFisno = Convert.ToInt32(dbtools.DegerGetir("select top 1 Rsat_Fisno from Cst_Recete_Satis where Rsat_Masa = '" + gridLookUpEdit1.EditValue + "' and Rsat_Durum = 'A' and Rsat_Ba = 'B'"));
                }
                else
                {
                    hedefFisno = Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));
                }

                Satis s = new Satis();
                s.Tag = "M";
                s.Fisno = hedefFisno;
                s.Miktar = deger;
                s.Masa_No = Convert.ToString(gridLookUpEdit1.EditValue);
                s.Rsat_SiparisPr = true;// Convert.ToBoolean(gridView1.GetFocusedRowCellValue("Rsat_SiparisPr"));
                s.Rsat_AbuyerPr = true;
                s.Urun_Sat(Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Recete")));

                if (deger == Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar")))
                {
                    dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")).ToString() + "'");
                }
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Malz_Transfer, Log.Log_Islem.Duzelt, Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " ürünü " + deger + " miktarı " + Convert.ToString(gridLookUpEdit1.EditValue) + " NL masaya transfer oldu." + "Eski Masa :" + txt_Masano.Text, hedefFisno.ToString(), "");
            }
            else
            {
                DataTable dtInd = dbtools.SelectTable("select Rsat_Fisno,Rsat_Indsatirid,Rsat_Indsatirid2 from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "' ");
                if (dtInd.Rows.Count > 0)
                {
                    //İndirimlerin Silinmesi
                    dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid"]) + "'");
                    dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid2"]) + "'");
                }
                int hedefFisno = 0;

                if (Convert.ToString(dbtools.DegerGetir("select Masa_Durum from Pos_Masa where Masa_No = '" + gridLookUpEdit1.EditValue + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'")) != "0")
                {
                    hedefFisno = Convert.ToInt32(dbtools.DegerGetir("select top 1 Rsat_Fisno from Cst_Recete_Satis where Rsat_Masa = '" + gridLookUpEdit1.EditValue + "' and Rsat_Durum = 'A' and Rsat_Ba = 'B'"));
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Masa = '" + gridLookUpEdit1.EditValue + "',Rsat_Fisno = '" + hedefFisno + "' where Rsat_Id = '" + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "'");
                }
                else
                {
                    hedefFisno = Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));
                    Satis s = new Satis();
                    s.Tag = "M";
                    s.Fisno = hedefFisno;
                    s.Miktar = 1;
                    s.Masa_No = Convert.ToString(gridLookUpEdit1.EditValue);
                    s.Rsat_SiparisPr = true;// Convert.ToBoolean(gridView1.GetFocusedRowCellValue("Rsat_SiparisPr"));
                    s.Rsat_AbuyerPr = true;
                    s.Urun_Sat(Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Recete")));

                    dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")).ToString() + "'");
                }

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Malz_Transfer, Log.Log_Islem.Duzelt, Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " ürünü 1 miktarı " + Convert.ToString(gridLookUpEdit1.EditValue) + " NL masaya transfer oldu." + "Eski Masa :" + txt_Masano.Text, hedefFisno.ToString(), "");
            }
            





            //DataTable dtInd = dbtools.SelectTable("select Rsat_Fisno,Rsat_Indsatirid,Rsat_Indsatirid2 from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "' ");
            //if (dtInd.Rows.Count > 0)
            //{
            //    //İndirimlerin Silinmesi
            //    dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid"]) + "'");
            //    dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid2"]) + "'");
            //}

            //int hedefFisno = Convert.ToInt32(dbtools.DegerGetir("select top 1 Rsat_Fisno from Cst_Recete_Satis where Rsat_Masa = '" + Convert.ToString(look_Masa.EditValue) + "' and Rsat_Durum = 'A' and Rsat_Ba = 'B'"));

            //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Masa = '" + Convert.ToString(look_Masa.EditValue) + "',Rsat_Fisno = '" + hedefFisno + "' where Rsat_Id = '" + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "'");

            gridyenile_KaynakMasa();
            gridyenile_HedefMasa();
        }

        private void look_Masa_EditValueChanged(object sender, EventArgs e)
        {
            gridyenile_HedefMasa();
        }

        private void look_Konum_EditValueChanged(object sender, EventArgs e)
        {
            gridyenile_Masa();
        }

    }
}