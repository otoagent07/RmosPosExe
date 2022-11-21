using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Kurlar : DevExpress.XtraEditors.XtraForm
    {
        public Kurlar()
        {
            InitializeComponent();
        }

        private void Kurlar_Load(object sender, EventArgs e)
        {
            BringToFront();

            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            dateEdit1.DateTime = Param.Tarih;

            DataTable dt = new DataTable();
            dt.Columns.Add("kod", typeof(string));
            dt.Columns.Add("ad", typeof(string));

            dt.Rows.Add("M", "Merkez");
            dt.Rows.Add("E", "Exchange");

            look_Cesit.Properties.DataSource = dt;
            look_Cesit.Properties.DisplayMember = "ad";
            look_Cesit.Properties.ValueMember = "kod";

            look_Cesit.ItemIndex = 0;

            gridyenile1();
        }

        private void gridyenile1()
        {
            gridColumn1.FieldName = "Mkodlar_Kod";
            gridColumn2.FieldName = "Mkodlar_Ad";
            gridColumn3.FieldName = "Doviz_Alis";
            gridColumn4.FieldName = "Doviz_Satis";
            gridColumn5.FieldName = "Efektif_Alis";
            gridColumn6.FieldName = "Efektif_Satis";
            gridColumn7.FieldName = "Kurlar_Id";
            gridColumn8.FieldName = "Mkodlar_Xml";

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Kurlar_Liste";
            com.Parameters.AddWithValue("@Tip", 0);
            com.Parameters.AddWithValue("@Kurlar_Tarih", dateEdit1.DateTime.Date);
            com.Parameters.AddWithValue("@Kurlar_Cesit", Convert.ToString(look_Cesit.EditValue));
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (con.State != ConnectionState.Closed) con.Close();

            gridControl1.DataSource = dt;

        }

        private void btn_Listele_Click(object sender, EventArgs e)
        {
            gridyenile1();
        }

        private void btn_Kaydet_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Kurlar_Kaydet";
                    cmd.Parameters.AddWithValue("@Kurlar_Cesit", Convert.ToString(look_Cesit.EditValue));
                    cmd.Parameters.AddWithValue("@Kurlar_Tarih", dateEdit1.DateTime.Date);
                    cmd.Parameters.AddWithValue("@Kurlar_Kodu", Convert.ToString(gridView1.GetRowCellValue(i, "Mkodlar_Kod")));
                    cmd.Parameters.AddWithValue("@Doviz_Alis", Convert.ToString(gridView1.GetRowCellValue(i, "Doviz_Alis")).Trim().Replace(",", "."));
                    cmd.Parameters.AddWithValue("@Doviz_Satis", Convert.ToString(gridView1.GetRowCellValue(i, "Doviz_Satis")).Trim().Replace(",", "."));
                    cmd.Parameters.AddWithValue("@Efektif_Alis", Convert.ToString(gridView1.GetRowCellValue(i, "Efektif_Alis")).Trim().Replace(",", "."));
                    cmd.Parameters.AddWithValue("@Efektif_Satis", Convert.ToString(gridView1.GetRowCellValue(i, "Efektif_Satis")).Trim().Replace(",", "."));

                    cmd.ExecuteNonQuery();
                }
                if (con.State != ConnectionState.Closed) con.Close();

                gridyenile1();

                MessageBox.Show(res_man.GetString("Kurlar Kaydedildi..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void btn_Transfer_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                string xmlKod = Convert.ToString(gridView1.GetRowCellValue(i, "Mkodlar_Xml"));
                if (xmlKod == "")
                {
                    continue;
                }

                DovizAl d = GetExchangeRate.ReadRate(dateEdit1.DateTime.Date, xmlKod);

                gridView1.SetRowCellValue(i, "Doviz_Alis", d.Doviz_Alis);
                gridView1.SetRowCellValue(i, "Doviz_Satis", d.Doviz_Satis);
                gridView1.SetRowCellValue(i, "Efektif_Alis", d.Efektif_Alis);
                gridView1.SetRowCellValue(i, "Efektif_Satis", d.Efektif_Satis);
            }

        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Sil_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                string id = Convert.ToString(gridView1.GetFocusedRowCellValue("Kurlar_Id"));
                if (id != "")
                {
                    dbtools.execcmd("delete Kurlar where Kurlar_Id = '" + id + "'");
                    gridyenile1();
                }
            }
        }

        private void btn_Aktar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(res_man.GetString("Kurlar Sonraki Güne Aktarılacak Onaylıyor Musunuz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            try
            {
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Kurlar_Kaydet";
                    cmd.Parameters.AddWithValue("@Kurlar_Cesit", Convert.ToString(look_Cesit.EditValue));
                    cmd.Parameters.AddWithValue("@Kurlar_Tarih", dateEdit1.DateTime.AddDays(1).Date);
                    cmd.Parameters.AddWithValue("@Kurlar_Kodu", Convert.ToString(gridView1.GetRowCellValue(i, "Mkodlar_Kod")));
                    cmd.Parameters.AddWithValue("@Doviz_Alis", Convert.ToString(gridView1.GetRowCellValue(i, "Doviz_Alis")).Trim().Replace(",", "."));
                    cmd.Parameters.AddWithValue("@Doviz_Satis", Convert.ToString(gridView1.GetRowCellValue(i, "Doviz_Satis")).Trim().Replace(",", "."));
                    cmd.Parameters.AddWithValue("@Efektif_Alis", Convert.ToString(gridView1.GetRowCellValue(i, "Efektif_Alis")).Trim().Replace(",", "."));
                    cmd.Parameters.AddWithValue("@Efektif_Satis", Convert.ToString(gridView1.GetRowCellValue(i, "Efektif_Satis")).Trim().Replace(",", "."));

                    cmd.ExecuteNonQuery();
                }
                if (con.State != ConnectionState.Closed) con.Close();

                gridyenile1();

                MessageBox.Show(res_man.GetString("Kurlar Kaydedildi..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void look_Cesit_EditValueChanged(object sender, EventArgs e)
        {
            gridyenile1();
        }
    }
}