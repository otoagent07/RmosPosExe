using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Pos_SubeMerkez : DevExpress.XtraEditors.XtraForm
    {
        public Pos_SubeMerkez()
        {
            InitializeComponent();
        }

        private void Pos_SubeMerkez_Load(object sender, EventArgs e)
        {
            dateTarih1.DateTime = Param.Tarih;
            //dateTarih2.DateTime = Param.Tarih;

            Doldur();
        }

        private void Doldur()
        {
            DataTable dt = dbtools.SelectTable("select * from Pos_kodlar where Pkod_Sinif = '27' and Pkod_MerkezSube = 'S' order by Pkod_Kod");

            if (dt.Rows.Count > 0)
            {
                cmb_Sube.Properties.DataSource = dt;
                cmb_Sube.Properties.DisplayMember = "Pkod_Ad";
                cmb_Sube.Properties.ValueMember = "Pkod_Kod";
                cmb_Sube.ItemIndex = -1;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void SubedenGetir()
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) { con.Open(); }

            try
            {
                using (SqlCommand cmd = new SqlCommand("Pos_Sube2Merkez", dbtools.conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddWithValue("@SubeKodu", cmb_Sube.EditValue);
                    cmd.Parameters.AddWithValue("@Tarih", dateTarih1.DateTime.Date);

                    cmd.ExecuteNonQuery();

                   MessageBox.Show(res_man.GetString("Transfer Edildi."), res_man.GetString("Uyarı"), MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show(res_man.GetString("İşlem Başarısız.") + "\n" + res_man.GetString(" Hata: '") + ex.Message + "'", "Uyarı");
                return;
            }
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (cmb_Sube.ItemIndex == -1)
            {
               MessageBox.Show(res_man.GetString("Şube Seçiniz.."), "Uyarı");
                return;
            }

            DialogResult c =MessageBox.Show(res_man.GetString("Şube Verileri Transfer edilecek. Eminmisiniz?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (c == DialogResult.Yes)
            {
                SubedenGetir();
            }
        }
    }
}