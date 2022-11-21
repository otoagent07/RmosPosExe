using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pos
{
    public partial class Onb_Fatura : DevExpress.XtraEditors.XtraForm
    {
        public Onb_Fatura()
        {
            InitializeComponent();
        }

        public int CardFID = 0;
        private void Onb_Fatura_Load(object sender, EventArgs e)
        {
            Listele(CardFID);
        }

        int Rez_ID = 0, Previl_ID = 0;
        private void Listele(int ID)
        {
            DataTable dt = Fronttools.SelectTable("Select * From KartF where ID = '" + CardFID + "'");
            if (dt.Rows.Count > 0)
            {
                CardF_Ad.Text = Convert.ToString(dt.Rows[0]["CardF_Ad"]);
                CardF_Soyad.Text = Convert.ToString(dt.Rows[0]["CardF_Soyad"]);
                CardF_Fat_Tel.Text = Convert.ToString(dt.Rows[0]["CardF_Fat_Tel"]);
                CardF_Fat_eMail.Text = Convert.ToString(dt.Rows[0]["CardF_Fat_eMail"]);
                CardF_Fat_Adres.Text = Convert.ToString(dt.Rows[0]["CardF_Fat_Adres"]);
                CardF_Fat_ilce.Text = Convert.ToString(dt.Rows[0]["CardF_Fat_ilce"]);
                CardF_Fat_il.EditValue = Convert.ToString(dt.Rows[0]["CardF_Fat_il"]);
                CardF_Fat_VD.Text = Convert.ToString(dt.Rows[0]["CardF_Fat_VD"]);
                CardF_Fat_VKN.Text = Convert.ToString(dt.Rows[0]["CardF_Fat_VKN"]);
                CardF_Fat_Tipi.SelectedIndex = Convert.ToString(dt.Rows[0]["CardF_Fat_Tipi"]) == "" ? 0 : Convert.ToInt32(dt.Rows[0]["CardF_Fat_Tipi"]);
                CardF_Fat_Senaryo.SelectedIndex = Convert.ToString(dt.Rows[0]["CardF_Fat_Senaryo"]) == "" ? 0 : Convert.ToInt32(dt.Rows[0]["CardF_Fat_Senaryo"]);
                CardF_Fat_SirketKisi.SelectedIndex = Convert.ToString(dt.Rows[0]["CardF_Fat_SirketKisi"]) == "" ? 0 : Convert.ToInt32(dt.Rows[0]["CardF_Fat_SirketKisi"]);
                Rez_ID = Convert.ToInt32(dt.Rows[0]["CardF_RezID"]);
                Previl_ID = Convert.ToInt32(Fronttools.DegerGetir("Select ISNULL((select ISNULL(Rez_Previl_id,0) as Rez_Previl_id from Rez where Rez_Id = '" + Rez_ID + @"'),0) as Rez_Previl_id "));
            }
        }

        private void Kaydet(int ID)
        {

            try
            {
                Fronttools.execcmd("Update KartF Set CardF_Ad = '" + CardF_Ad.Text + "',CardF_Soyad = '" + CardF_Soyad.Text + "',CardF_Fat_eMail = '" + CardF_Fat_eMail.Text + "',CardF_Fat_Tel = '" + CardF_Fat_Tel.Text + "',CardF_Fat_Adres = '" + CardF_Fat_Adres.Text + "',CardF_Fat_ilce = '" + CardF_Fat_ilce.Text + "',CardF_Fat_il = '" + CardF_Fat_il.EditValue + "',CardF_Fat_VD = '" + CardF_Fat_VD.Text + "',CardF_Fat_VKN = '" + CardF_Fat_VKN.Text + "',CardF_Fat_Tipi = '" + CardF_Fat_Tipi.SelectedIndex + "',CardF_Fat_Senaryo = '" + CardF_Fat_Senaryo.SelectedIndex + "',CardF_Fat_SirketKisi = '" + CardF_Fat_SirketKisi.SelectedIndex + "' where ID = '" + ID + "' ");

                if (Previl_ID == 0)
                {



                    SqlConnection fcon = Fronttools.conn;
                    if (fcon.State == ConnectionState.Closed) { fcon.Open(); }

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("StpPrevil_Kaydet", Fronttools.conn) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.Parameters.AddWithValue("@xtip", 1);
                            cmd.Parameters.AddWithValue("@Fat_Adres1", CardF_Fat_Adres.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Fat_il", CardF_Fat_il.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Fat_ilce", CardF_Fat_ilce.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Mail", CardF_Fat_eMail.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Tel", CardF_Fat_Tel.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Vergidaire", CardF_Fat_VD.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Vergino", CardF_Fat_VKN.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Adres1", CardF_Fat_Adres.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Il", CardF_Fat_il.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Ilce", CardF_Fat_ilce.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Soyad", CardF_Soyad.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Ad", CardF_Ad.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Adresil", CardF_Fat_il.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Adresilce", CardF_Fat_ilce.Text);
                            cmd.Parameters.AddWithValue("@Kimlik_Tc", CardF_Fat_VKN.Text);

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            Fronttools.execcmd("Update Rez Set Rez_Previl_id = '" + dt.Rows[0][0] + "' where Rez_Id = '" + Rez_ID + "' ");
                        }

                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Mesajı : " + ex.Message);
                throw;
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Kaydet(CardFID);
            this.Close();
        }
    }
}