using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using Pos.Class;
using DevExpress.XtraGrid.Columns;
using System.Xml;
using System.Resources;
using System.Reflection;

namespace Pos
{
    public partial class Split_Bol : DevExpress.XtraEditors.XtraForm
    {
        public int Fisno { get; set; }
        public string Masa_No { get; set; }

        int Split = 1;

        public Split_Bol()
        {
            InitializeComponent();
        }

        private void Split_Bol_Load(object sender, EventArgs e)
        {
            txt_Masa.Text = Masa_No;

            gridyenile1();
            gridyenile2();
            Split_Ayarla();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridyenile1()
        {
            gridColumn7.FieldName = "Rsat_Id";
            gridColumn8.FieldName = "Rec_Ad2";
            gridColumn9.FieldName = "Rsat_Miktar";
            gridColumn10.FieldName = "Rsat_Emiktar";
            gridColumn11.FieldName = "Rsat_Tutar";
            gridColumn12.FieldName = "Rsat_Doviztutar";
            gridColumn13.FieldName = "Rsat_Ba";
            gridColumn15.FieldName = "Rsat_Split";

            if (Param.Calisma_Sekli == 1)
            {
                gridColumn5.Visible = true;
                gridColumn5.VisibleIndex = 99;
            }
            else
            {
                gridColumn4.Visible = true;
                gridColumn4.VisibleIndex = 99;
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Rapor_Tipi", 0);
            com.Parameters.AddWithValue("@Split", 0);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl1.DataSource = dt;
            gridView1.BestFitColumns();

            gridView1.ActiveFilter.Add(gridView1.Columns["Rsat_Split"], new ColumnFilterInfo("[Rsat_Split] = '0'", ""));
        }

        private void gridyenile2()
        {


            gridColumn1.FieldName = "Rec_Ad2";
            gridColumn2.FieldName = "Rsat_Miktar";
            gridColumn3.FieldName = "Rsat_Emiktar";
            gridColumn4.FieldName = "Rsat_Tutar";
            gridColumn5.FieldName = "Rsat_Doviztutar";
            gridColumn6.FieldName = "Rsat_Ba";
            gridColumn14.FieldName = "Rsat_Id";

            if (Param.Calisma_Sekli == 1)
            {
                gridColumn12.Visible = true;
                gridColumn12.VisibleIndex = 99;
            }
            else
            {
                gridColumn11.Visible = true;
                gridColumn11.VisibleIndex = 99;
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Rapor_Tipi", 0);
            com.Parameters.AddWithValue("@Split", Split);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl2.DataSource = dt;
            gridView2.BestFitColumns();
        }

        CheckButton btnSecilen;
        private void Split_CheckedChanged(object sender, EventArgs e)
        {
            if (btnSecilen == null)
            {
                btnSecilen = (CheckButton)sender;

                cbtn_1.Checked = false;
                cbtn_2.Checked = false;
                cbtn_3.Checked = false;
                cbtn_4.Checked = false;
                cbtn_5.Checked = false;
                cbtn_6.Checked = false;
                cbtn_7.Checked = false;
                cbtn_8.Checked = false;
                cbtn_9.Checked = false;

                cbtn_1.ForeColor = Color.Black;
                cbtn_2.ForeColor = Color.Black;
                cbtn_3.ForeColor = Color.Black;
                cbtn_4.ForeColor = Color.Black;
                cbtn_5.ForeColor = Color.Black;
                cbtn_6.ForeColor = Color.Black;
                cbtn_7.ForeColor = Color.Black;
                cbtn_8.ForeColor = Color.Black;
                cbtn_9.ForeColor = Color.Black;

                Split = Convert.ToInt32(btnSecilen.Tag);
                btnSecilen.Checked = true;
                btnSecilen.ForeColor = Color.Red;

                btnSecilen = null;

                gridyenile1();
                gridyenile2();
                Split_Ayarla();
            }
        }

        private void Split_Ayarla()
        {

            DataTable dtSplit = dbtools.SelectTable("select distinct ISNULL(Rsat_Split,0) as Rsat_Split,Rsat_Splitad from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Masa = '" + Masa_No + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "' and Rsat_Durum = 'A' and Rsat_Fisno = '" + Fisno + "' ");
            for (int i = 0; i < dtSplit.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 1) { cbtn_1.ForeColor = Color.Red; txt_1.Text = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 2) { cbtn_2.ForeColor = Color.Red; txt_2.Text = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 3) { cbtn_3.ForeColor = Color.Red; txt_3.Text = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 4) { cbtn_4.ForeColor = Color.Red; txt_4.Text = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 5) { cbtn_5.ForeColor = Color.Red; txt_5.Text = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 6) { cbtn_6.ForeColor = Color.Red; txt_6.Text = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 7) { cbtn_7.ForeColor = Color.Red; txt_7.Text = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 8) { cbtn_8.ForeColor = Color.Red; txt_8.Text = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 9) { cbtn_9.ForeColor = Color.Red; txt_9.Text = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
            }
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        private void btn_SplitAt_Click(object sender, EventArgs e)
        {
            int Rsat_Id = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id"));
            decimal miktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
            if (miktar == 1)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Split = '" + Split.ToString() + "' where Rsat_Id = '" + Rsat_Id + "'");
            }
            else
            {
                Klavye1 klv1 = new Klavye1();
                klv1.Tag = "MIKTARSOR";
                klv1.txt_Sayi.Text = miktar.ToString();
                klv1.ShowDialog();
                if (klv1.Cikis)
                {
                    return;
                }

                if (klv1.sayi > miktar)
                {
                   MessageBox.Show(res_man.GetString("Hatalı Giriş..."));
                    return;
                }

                if (klv1.sayi != miktar)
                {
                    int yeniId = Fis_Islem.Satir_Kopyala(Rsat_Id);
                    Fis_Islem.Satir_Sil(yeniId, klv1.sayi);
                    Fis_Islem.Satir_Sil(Rsat_Id, miktar - klv1.sayi);
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Split = '" + Split.ToString() + "' where Rsat_Id = '" + yeniId + "'");
                }

                if (klv1.sayi == miktar)
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Split = '" + Split.ToString() + "' where Rsat_Id = '" + Rsat_Id + "'");
                }
             


                //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Split = '" + Split.ToString() + "' where Rsat_Id = '" + Rsat_Id + "'");
            }

            gridyenile1();
            gridyenile2();
            Split_Ayarla();
        }

        private void btn_SplitGeri_Click(object sender, EventArgs e)
        {
            int Rsat_Id = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Id"));
            decimal miktar = Convert.ToDecimal(gridView2.GetFocusedRowCellValue("Rsat_Miktar"));
            if (miktar == 1)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Split = '0' where Rsat_Id = '" + Rsat_Id + "'");
            }
            else
            {
                Klavye1 klv1 = new Klavye1();
                klv1.Tag = "MIKTARSOR";
                klv1.txt_Sayi.Text = miktar.ToString();
                klv1.ShowDialog();
                if (klv1.Cikis)
                {
                    return;
                }
                if (klv1.sayi > miktar)
                {
                   MessageBox.Show(res_man.GetString("Hatalı Giriş..."));
                    return;
                }

                if (klv1.sayi != miktar)
                {
                    int yeniId = Fis_Islem.Satir_Kopyala(Rsat_Id);
                    Fis_Islem.Satir_Sil(yeniId, klv1.sayi);
                }
                Fis_Islem.Satir_Sil(Rsat_Id, miktar - klv1.sayi);

                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Split = '0' where Rsat_Id = '" + Rsat_Id + "'");
            }


            //int Rsat_Id = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Id"));
            //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Split = '0' where Rsat_Id = '" + Rsat_Id + "'");

            gridyenile1();
            gridyenile2();
            Split_Ayarla();
        }

        private void txt_Split_Click(object sender, EventArgs e)
        {
            TextEdit txt = (TextEdit)sender;

            Klavye2 klv = new Klavye2();
            klv.ShowDialog();
            txt.Text = klv.yazi;
        }

        private void txt_Split_Leave(object sender, EventArgs e)
        {
            TextEdit txt = (TextEdit)sender;
            if (txt.Text != "")
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Splitad = '" + txt.Text + "' where Rsat_Fisno = '" + Fisno + "' and Rsat_Split = '" + txt.Tag + "'");
            }
        }


    }
}