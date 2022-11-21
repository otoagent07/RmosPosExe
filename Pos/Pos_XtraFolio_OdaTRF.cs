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
using System.Resources;
using System.Reflection;

namespace Pos
{
    public partial class Pos_XtraFolio_OdaTRF : DevExpress.XtraEditors.XtraForm
    {
        public Pos_XtraFolio_OdaTRF()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string filtre = string.Empty;

        public int KartFID = 0;
        private void AraListele()
        {

            gridControl1.DataSource = Fronttools.SelectTable(@"
                                    SELECT Rez_Id,Rez_Odano,Rez_Adi_1,Rez_Adi_2, 
                                    Rez_Giris_tarihi,Rez_Cikis_tarihi
                                    FROM Rez WİTH(NOLOCK)
                                    WHERE  Rez_R_I_H = 'I' and Rez_Master_detay <> 'E' And Rez_Odano like N'" + txt_Odano.Text + "%' ");
        }

        private void Listele()
        {
            gridControl1.DataSource = Fronttools.SelectTable(@"
                                    SELECT Rez_Id,Rez_Odano,Rez_Adi_1,Rez_Adi_2, 
                                    Rez_Giris_tarihi,Rez_Cikis_tarihi
                                    FROM Rez WİTH(NOLOCK)
                                    WHERE  Rez_R_I_H = 'I' and Rez_Master_detay <> 'E'");
        }

        private void Pos_XtraFolio_OdaTRF_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void txt_Odano_TextChanged(object sender, EventArgs e)
        {
            AraListele();
        }

        int newFolioID = 0;
      
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (newFolioID == 0)
            {
                MessageBox.Show(res_man.GetString("Kişi Bilgisi Bulunamadı..") + "\n" + res_man.GetString("Listeneden Kişi Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Folio_Transfer";
                com.Parameters.AddWithValue("@Pos_Departman", Departman.Dep_Kodu);
                com.Parameters.AddWithValue("@new_FolioID", newFolioID);
                com.Parameters.AddWithValue("@KartFID", KartFID);
                com.Parameters.AddWithValue("@PosKullanici", User.P_Kod);
                com.ExecuteNonQuery();
                if (con.State == ConnectionState.Open) con.Close();
            }
            catch (Exception)
            {
                throw;
            }

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kart, Log.Log_Islem.Duzelt, Convert.ToString(KartFID) + " Kart IDsi, Transfer Edilmiştir. Yeni FolioID : " + Convert.ToString(newFolioID), "", Convert.ToString(newFolioID));

            MessageBox.Show(res_man.GetString("Oda Transfer Edildi..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            newFolioID = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rez_Id"));
        }
    }
}