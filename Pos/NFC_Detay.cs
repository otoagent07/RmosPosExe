using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using Pos.Class;

namespace Pos
{
    public partial class NFC_Detay : DevExpress.XtraEditors.XtraForm
    {
        public NFC_Detay()
        {
            InitializeComponent();
        }

        private void NFC_Detay_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            gridyenile();
        }

        public int kartId;
        private void gridyenile()
        {
            if (Convert.ToInt32(kartId) == 0)
            {
                return;
            }

            gridColumn1.FieldName = "Rec_Ad";
            gridColumn2.FieldName = "Rsat_Miktar";
            gridColumn3.FieldName = "Rsat_Emiktar";
            gridColumn4.FieldName = "Rsat_Tutar";
            gridColumn5.FieldName = "Rsat_Doviztutar";
            gridColumn6.FieldName = "Rsat_Ba";

            if (Param.Calisma_Sekli == 1)
            {
                gridColumn5.Visible = true;
                gridColumn5.VisibleIndex = 4;
            }
            else
            {
                gridColumn4.Visible = true;
                gridColumn4.VisibleIndex = 4;
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@KartId", kartId);
            com.Parameters.AddWithValue("@Rapor_Tipi", 14);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl1.DataSource = dt;
        }

        private void btn_Fispr_Click(object sender, EventArgs e)
        {
            FisPr fis = new FisPr();
            //fis.HesapPr(Convert.ToInt32(spn_Fisno.EditValue));
            fis.KartDokum(false, Convert.ToInt32(kartId), 0);
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}