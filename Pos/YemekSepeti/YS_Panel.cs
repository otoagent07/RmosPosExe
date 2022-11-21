using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos.YemekSepeti
{
    public partial class YS_Panel : DevExpress.XtraEditors.XtraForm
    {
        public YS_Panel()
        {
            InitializeComponent();
        }

        private void YS_Panel_Load(object sender, EventArgs e)
        {
            iws.AuthHeaderValue = new IntegrationWebService1.AuthHeader();
            iws.AuthHeaderValue.UserName = YS_AuthHeader.ah.UserName;
            iws.AuthHeaderValue.Password = YS_AuthHeader.ah.Password;

            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            //xtraTabControl1.SelectedTabPage = xtraTabPage3;
            dateTarih1.DateTime = Param.Tarih;
            dateTarih2.DateTime = Param.Tarih;

            repositoryItemLookUpEdit1.DataSource = dbtools.SelectTable("Select Rec_Genelkod,Rec_Ad from Cst_Recete");
            repositoryItemLookUpEdit1.ValueMember = "Rec_Genelkod";
            repositoryItemLookUpEdit1.DisplayMember = "Rec_Ad";
            gridControl1.DataSource = MenuListele();
        }

        IntegrationWebService1.Integration iws = new IntegrationWebService1.Integration();

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DataSet ds = new DataSet();

        public DevExpress.XtraNavBar.NavBarItem nBar;
        public void nBar_Kontrol(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            gridControl1.DataSource = null;
            nBar = (DevExpress.XtraNavBar.NavBarItem)sender;
            ds = new DataSet();

            if (nBar.Tag == "1")
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage1;

                RestoranListele();

                look_Restoran.EditValue = YS_RestoInfo.YS_CatagoryName;
            }

            if (nBar.Tag == "2")
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage2;

                repositoryItemLookUpEdit1.DataSource = dbtools.SelectTable("Select Rec_Genelkod,Rec_Ad from Cst_Recete");
                repositoryItemLookUpEdit1.ValueMember = "Rec_Genelkod";
                repositoryItemLookUpEdit1.DisplayMember = "Rec_Ad";


                gridControl1.DataSource = MenuListele();
            }

            if (nBar.Tag == "3")
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage4;
            }
        }

        private void SiparisYorumGetir()
        {
            DataTable dt = new DataTable();
            dt = iws.GetRestaurantPointsAndComments(YS_RestoInfo.YS_CatalogName, YS_RestoInfo.YS_CatagoryName, dateTarih1.DateTime.Date, dateTarih2.DateTime.Date);
            gridControl2.DataSource = dt;
        }
        DataTable dtresto = new DataTable();
        private void RestoranListele()
        {


            if (iws.GetRestaurantList() != null)
            {

                dtresto = (DataTable)iws.GetRestaurantList().Tables[0];

                look_Restoran.Properties.DataSource = dtresto;
                look_Restoran.Properties.DisplayMember = "DisplayName";
                look_Restoran.Properties.ValueMember = "CategoryName";


                //if (ds != null)
                //{
                //    foreach (DataRow r in ds.Tables)
                //    {
                //        YS_CatalogName.Text = Convert.ToString(r["CatalogName"]);
                //        YS_CatagoryName.Text = Convert.ToString(r["CategoryName"]);
                //        YS_DisplayName.Text = Convert.ToString(r["DisplayName"]);
                //        YS_ServiceTime.Text = Convert.ToString(r["ServiceTime"]);
                //        YS_Speed.Text = Convert.ToString(r["Speed"]);
                //        YS_Serving.Text = Convert.ToString(r["Serving"]);
                //        YS_Flavour.Text = Convert.ToString(r["Flavour"]);
                //        YS_Refresh.Text = (YS_Refresh.Text == "" ? "10" : YS_Refresh.Text);
                //    }
                //}
            }
        }

        private void RestoranKaydet()
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("YS_Restoran_Kaydet", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@YS_Dep_Kodu", Departman.Dep_Kodu);
                cmd.Parameters.AddWithValue("@YS_CatalogName", YS_CatalogName.Text);
                cmd.Parameters.AddWithValue("@YS_CatagoryName", YS_CatagoryName.Text);
                cmd.Parameters.AddWithValue("@YS_DisplayName", YS_DisplayName.Text);
                cmd.Parameters.AddWithValue("@YS_ServiceTime", YS_ServiceTime.Text);
                cmd.Parameters.AddWithValue("@YS_Speed", YS_Speed.Text);
                cmd.Parameters.AddWithValue("@YS_Serving", YS_Serving.Text);
                cmd.Parameters.AddWithValue("@YS_Flavour", YS_Flavour.Text);
                cmd.Parameters.AddWithValue("@YS_Refresh", YS_Refresh.Text);

                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show(res_man.GetString("Bilgiler Kayıt Edilemedi...") + ex.Message);
                return;
            }
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            RestoranKaydet();
            RestoranListele();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuGetir()
        {
            ds = iws.GetMenu();
            if (ds != null)
            {
                if (MessageBox.Show(res_man.GetString("Menu Getirilmeden Önce Eski Veriler Silinecektir. Emin misiniz ?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    dbtools.execcmd("Truncate table YS_Menu");
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        dbtools.execcmd("INSERT INTO YS_Menu Select '" + item["Id"] + "','" + item["Name"] + "','" + item["Title"] + "','" + item["Price"].ToString().Replace(",", ".") + "','" + item["Description"] + "'");
                    }
                }

            }
        }

        private DataTable MenuListele()
        {
            return dbtools.SelectTable("select Id,[Name],Rec_Genelkod,Rec_Fiyat,Rec_Paket_Tam,Price from YS_Menu left join Cst_Recete on YS_Menu.Id = Rec_YS_UrunID order by Rec_Genelkod,[Name]");
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            MenuGetir();
            gridControl1.DataSource = MenuListele();
        }

        private void MenuKaydet()
        {
            try
            {
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    if (Convert.ToString(gridView1.GetRowCellValue(i, "Rec_Genelkod")).Length > 0)
                    {
                        dbtools.execcmd("Update Cst_Recete set Rec_YS_UrunID = '" + Convert.ToString(gridView1.GetRowCellValue(i, "Id")) + "' Where Rec_Genelkod = '" + Convert.ToString(gridView1.GetRowCellValue(i, "Rec_Genelkod")) + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(res_man.GetString("Hata : ") + ex.Message);
                return;
            }
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            MenuKaydet();
            MenuListele();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(YS_ServiceTime.Text) >= 30 && Convert.ToInt32(YS_ServiceTime.Text) <= 75)
            {
                iws.SetRestaurantServiceTime(YS_CatalogName.Text, YS_CatagoryName.Text, Convert.ToInt32(YS_ServiceTime.Text));
                dbtools.execcmd("Update YS_Restaurant Set YS_ServiceTime ='" + Convert.ToInt32(YS_ServiceTime.Text) + "' where YS_CatagoryName = '" + YS_CatagoryName.Text + "' and YS_Dep_Kodu = '" + Departman.Dep_Kodu + "'");
            }
            else
            {
                MessageBox.Show(res_man.GetString("Servis Zamanı Minumum 30DK , Maximum 75DK arasında olmalıdır.."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            dbtools.execcmd("Update Cst_Recete Set Rec_YS_UrunID = NULL where Rec_Genelkod = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")) + "'");
            gridControl1.DataSource = MenuListele();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            SiparisYorumGetir();
        }

        private void look_Restoran_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow r in dtresto.Select("CategoryName = '" + look_Restoran.EditValue.ToString() + "'"))
                {
                    YS_CatalogName.Text = Convert.ToString(r["CatalogName"]);
                    YS_CatagoryName.Text = Convert.ToString(r["CategoryName"]);
                    YS_DisplayName.Text = Convert.ToString(r["DisplayName"]);
                    YS_ServiceTime.Text = Convert.ToString(r["ServiceTime"]);
                    YS_Speed.Text = Convert.ToString(r["Speed"]);
                    YS_Serving.Text = Convert.ToString(r["Serving"]);
                    YS_Flavour.Text = Convert.ToString(r["Flavour"]);
                    YS_Refresh.Text = (YS_Refresh.Text == "" ? "10" : YS_Refresh.Text);
                }
            }
            catch(Exception ex)
            {

            }
           
        }

        private void repositoryItemLookUpEdit1_Leave(object sender, EventArgs e)
        {
            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")) != "")
            {
                DataTable dt = dbtools.SelectTable("Select Rec_Fiyat,Rec_Paket_Tam From Cst_Recete Where Rec_Genelkod = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")) + "'");

                if (dt.Rows.Count > 0)
                {
                    gridView1.SetFocusedRowCellValue("Rec_Fiyat", dt.Rows[0][0]);
                    gridView1.SetFocusedRowCellValue("Rec_Paket_Tam", dt.Rows[0][1]);
                }
            }
        }

        private void receteyeGitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")) != "")
            {
                Rmosback.dbtools.conn = dbtools.conn;
                Rmosback.Classes.Constants.cnnBack = dbtools.conn.ConnectionString;
                Rmosback.Classes.Constants.cnnRmosMuh = Rmosmuh.conn.ConnectionString;
                Rmosback.dbtools.Kullanici_Kodu = User.U_BackUser;
                Rmosback.Classes.Constants.KullaniciKod = User.U_BackUser;
                Rmosback.Service.Cst_ReceteService recService = new Rmosback.Service.Cst_ReceteService();
                Rmosback.Entity.Cst_Recete rec = recService.GetByKod(Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")));
                Rmosback.Cst_Recete_Giris a = new Rmosback.Cst_Recete_Giris(rec);
                a.text_Genel.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod"));
                a.ShowDialog();
            }
        }
    }
}