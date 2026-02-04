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

namespace Pos
{
    public partial class Detay : DevExpress.XtraEditors.XtraForm
    {
        public Detay()
        {
            InitializeComponent();
        }

        private void Detay_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            gridyenile();

            string q = $@"select top 1 (u.P_Ad+' '+u.P_Soyad) as adSoyad from Cst_Recete_Satis s
left join RmosMuh..Pos_User as u on u.P_Kod=s.Rsat_Garson
where Rsat_Fisno='{spn_Fisno.EditValue.ToString()}'";

            txtAcilisGarson.Text = "Açan Garson: "  + dbtools.DegerGetir(q);
        }

        private void gridyenile()
        {
            if (Convert.ToInt32(spn_Fisno.EditValue) == 0)
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
            com.Parameters.AddWithValue("@Fisno", Convert.ToInt32(spn_Fisno.EditValue));
            com.Parameters.AddWithValue("@Rapor_Tipi", 2);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl1.DataSource = dt;
        }

        private void btn_Fispr_Click(object sender, EventArgs e)
        {


            FisPr pr = new FisPr();
            if (Param.Param_YeniHesapDkm)
            {
                pr.newHesapDokum(true, Convert.ToInt32(spn_Fisno.EditValue), 0, "* * * HESAP FİŞİ * * *");
            }
            else
            {
                pr.HesapDokum(true, Convert.ToInt32(spn_Fisno.EditValue), 0);
            }
        }

        private void btn_Adisyonpr_Click(object sender, EventArgs e)
        {
            AdisyonPr ads = new AdisyonPr();
            ads.Adisyon_Yaz(Convert.ToInt32(spn_Fisno.EditValue), true);
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string masano = "";
        private void btn_Faturapr_Click(object sender, EventArgs e)
        {
            Fis_Islem.Fatura_Kes(Convert.ToInt32(spn_Fisno.EditValue), false, true, "F");
        }

        private void btn_HesabaFatura_Click(object sender, EventArgs e)
        {
            Fis_Islem.Fatura_Kes(Convert.ToInt32(spn_Fisno.EditValue), true, true, "F");
        }

        private void btnHesapDokum_Click(object sender, EventArgs e)
        {
            FisPr pr = new FisPr();
            if (Param.Param_YeniHesapDkm)
            {
                pr.newHesapDokum(true, Convert.ToInt32(spn_Fisno.Text), 0, "* * * HESAP DÖKÜM FİŞİ * * *");
            }
            else
            {
                pr.HesapDokum(true, Convert.ToInt32(spn_Fisno.Text), 0);
            }

            string aciklama = "Fisno : " + spn_Fisno.Text + " . HESAP DÖKÜM FİŞİ ALINDI. ";

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, aciklama, Convert.ToString(spn_Fisno.Text), "");

        }

        private void btnAdisyonR_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("Update Cst_Recete_Satis set  Rsat_AdisyonTR = '1' where Rsat_Fisno = '" + spn_Fisno.Text + "'");
            MessageBox.Show("Güncellendi");
        }

        private void btnAdisyonG_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("Update Cst_Recete_Satis set  Rsat_AdisyonTR = '0' where Rsat_Fisno = '" + spn_Fisno.Text + "'");
            MessageBox.Show("Güncellendi");
        }

        private void btn_GarsonDegistir_Click(object sender, EventArgs e)
        {
            GarsonDegistir gde = new GarsonDegistir();
            gde.Masa_No = masano;
            gde.ShowDialog();
            this.Close();
        }
    }
}