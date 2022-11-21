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
using Pos.Print;
using DevExpress.XtraReports.UI;

namespace Pos
{
    public partial class Fis_Log : DevExpress.XtraEditors.XtraForm
    {
        public int Fisno { get; set; }

        public Fis_Log()
        {
            InitializeComponent();
        }

        private void Fis_Log_Load(object sender, EventArgs e)
        {
            txt_Fisno.Text = Fisno.ToString();

            gridyenile();
        }

        private void gridyenile()
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Rapor_Tipi", 8);
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(ds);
            dt1 = ds.Tables[0];
            dt2 = ds.Tables[1];
            if (con.State == ConnectionState.Open) con.Close();


            gridControl1.DataSource = dt1;
            gridView1.BestFitColumns();

            gridControl2.DataSource = dt2;
            gridView2.BestFitColumns();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Yazdir(DevExpress.XtraGrid.Views.Grid.GridView gC, string Baslik)
        {
            DynamicReport ayk = new DynamicReport(Baslik, Param.Tesis_Adi, gC);
            ayk.ShowPreviewDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Yazdir((DevExpress.XtraGrid.Views.Grid.GridView)gridControl2.MainView, "SATIŞLAR");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Yazdir((DevExpress.XtraGrid.Views.Grid.GridView)gridControl2.MainView, "FİŞ LOG KAYITLARI");
        }
    }
}