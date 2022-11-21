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

namespace Pda
{
    public partial class SatisList : DevExpress.XtraEditors.XtraForm
    {
        public int Fis_No { get; set; }
        public SatisList()
        {
            InitializeComponent();
        }

        private void SatisList_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;
            this.Text = "Satıs" + "  - Fisno:" + Fis_No;

            gridyenile();
        }

        private void gridyenile()
        {
            gridColumn1.FieldName = "Rec_Ad";
            gridColumn2.FieldName = "Rsat_Miktar";
            gridColumn3.FieldName = "Rsat_Emiktar";
            gridColumn4.FieldName = "Rsat_Tutar";
            gridColumn5.FieldName = "Rsat_Doviztutar";
            gridColumn6.FieldName = "Rsat_Ba";
            gridColumn7.FieldName = "Rsat_Id";

            if (Param.Calisma_Sekli == 1)
            {
                gridColumn5.Visible = true;
                gridColumn5.VisibleIndex = 5;
            }
            else
            {
                gridColumn4.Visible = true;
                gridColumn4.VisibleIndex = 5;
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fis_No);
            com.Parameters.AddWithValue("@Rapor_Tipi", 2);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl1.DataSource = dt;
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}