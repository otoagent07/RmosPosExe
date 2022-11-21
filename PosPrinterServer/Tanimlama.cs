using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;


namespace PosPrinterServer
{
    public partial class Tanimlama : DevExpress.XtraEditors.XtraForm
    {
        public Tanimlama()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            xtraTab_Ana.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            String pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                cmb_Yazicilar.Properties.Items.Add(pkInstalledPrinters);
            }
        }
        private void temizle(Control ctrl)
        {
            foreach (Control item in ctrl.Controls)
            {
                if (item is TextEdit)
                    if (((TextEdit)item).Enabled)
                        ((TextEdit)item).EditValue = null;

                if (item is LookUpEdit)
                    if (((LookUpEdit)item).Enabled)
                        ((LookUpEdit)item).EditValue = null;

                if (item is ComboBoxEdit)
                    if (((ComboBoxEdit)item).Enabled)
                        ((ComboBoxEdit)item).Text = null;

                if (item is RadioGroup)
                    if (((RadioGroup)item).Enabled)
                        ((RadioGroup)item).SelectedIndex = 0;

                if (item is CheckedComboBoxEdit)
                    if (((CheckedComboBoxEdit)item).Enabled)
                        ((CheckedComboBoxEdit)item).EditValue = null;

                if (item.Controls.Count > 0)
                    temizle(item);
            }

            ID = 0;

        }

        private void nBar_Kontrol(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            gridControl1.DataSource = null;

            temizle(this);

            DevExpress.XtraNavBar.NavBarItem nBar = (DevExpress.XtraNavBar.NavBarItem)sender;

            txt_Baslik.Text = String.Format("{0} - {1}", nBar.Caption.ToUpper(), nBar.Tag);
            txt_Baslik.Tag = nBar.Tag;

            if (Convert.ToString(nBar.Tag) == "1")
            {
                xtraTab_Ana.SelectedTabPage = anaTab_Printer;
                xtraTabControl1.SelectedTabPage = tab_YaziciTurleri;

                DataTable dt = new DataTable();
                dt.Columns.Add("Kodu", typeof(string));
                dt.Columns.Add("Adi", typeof(string));
                dt.Rows.Add("E", "ESKİ");
                dt.Rows.Add("Y", "YENİ");
                look_YaziciSekli.Properties.DataSource = dt;
                look_YaziciSekli.Properties.DisplayMember = "Adi";
                look_YaziciSekli.Properties.ValueMember = "Kodu";

                Listele(Convert.ToString(txt_Baslik.Tag));
            }

            if (Convert.ToString(nBar.Tag) == "2")
            {
                xtraTab_Ana.SelectedTabPage = anaTab_Printer;
                xtraTabControl1.SelectedTabPage = tab_Yazicilar;
                DataTable dt = dbtools.SelectTable("Select pr_Adi,pr_PrinterOzelKod From Pos_Printer Where pr_Sinif = 1");
                if (dt.Rows.Count == 0) return;

                look_YaziciTuru.Properties.DataSource = dt;
                look_YaziciTuru.Properties.DisplayMember = "pr_Adi";
                look_YaziciTuru.Properties.ValueMember = "pr_PrinterOzelKod";

                DataTable dtDep = dbtools.SelectTable("select Kodlar_Kod,Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = 1 and Kodlar_Satis = 1");
                if (dtDep.Rows.Count == 0) return;

                look_Departman.Properties.DataSource = dtDep;
                look_Departman.Properties.DisplayMember = "Kodlar_Ad";
                look_Departman.Properties.ValueMember = "Kodlar_Kod";

                if (dtDep.Rows.Count == 1) look_Departman.CheckAll();

                Listele(Convert.ToString(txt_Baslik.Tag));
            }

            if (Convert.ToString(nBar.Tag) == "3")
            {
                xtraTab_Ana.SelectedTabPage = anaTab_Dizayn;

                if (!Directory.Exists("Raporlar"))
                {
                    Directory.CreateDirectory("Raporlar");
                }

                DizaynListele();
            }

        }

        private void DizaynListele()
        {
            string[] klasorler = Directory.GetFiles("Raporlar");

            DataTable dt = new DataTable();
            dt.Columns.Add("Rapor", typeof(string));

            foreach (string item in klasorler)
            {
                string uzanti = item.Split('.')[item.Split('.').Length - 1];

                if (uzanti.ToLower() == "repx")
                    dt.Rows.Add(item);
            }

            gridControl2.DataSource = dt;

        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        int ID = 0;
        private void Kaydet(string Sinif)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) { con.Open(); }

            using (SqlCommand cmd = new SqlCommand("Pos_Printer_Ekle", con) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@pr_Kodu", txt_Kod.Text);
                cmd.Parameters.AddWithValue("@pr_Adi", txt_Ad.Text);
                cmd.Parameters.AddWithValue("@pr_Sinif", Sinif);
                if (Sinif == "1")
                {
                    cmd.Parameters.AddWithValue("@pr_YaziciSekli", look_YaziciSekli.EditValue);
                    cmd.Parameters.AddWithValue("@pr_PrinterOzelKod", rdb_prOzelKod.EditValue);
                }
                if (Sinif == "2")
                {
                    cmd.Parameters.AddWithValue("@pr_YaziciTuru", look_YaziciTuru.EditValue);
                    cmd.Parameters.AddWithValue("@pr_Yazici", cmb_Yazicilar.Text);
                    cmd.Parameters.AddWithValue("@pr_Departman", look_Departman.EditValue);
                }

                cmd.ExecuteNonQuery();
            }
        }

        private void Sil(int ID)
        {
            dbtools.execcmd("delete from Pos_Printer Where ID = " + ID);
        }

        private void Listele(string Sinif)
        {
            gridControl1.DataSource = dbtools.SelectTable("Select * From Pos_Printer where pr_Sinif = '" + Sinif + "'");
        }

        private void btn_Kaydet_Click(object sender, EventArgs e)
        {
            Kaydet(Convert.ToString(txt_Baslik.Tag));
            Listele(Convert.ToString(txt_Baslik.Tag));
            temizle(this);
        }

        private void btn_Sil_Click(object sender, EventArgs e)
        {
            Sil(ID);
            Listele(Convert.ToString(txt_Baslik.Tag));
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            ID = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID"));

            DataTable dt = dbtools.SelectTable("Select * From Pos_Printer Where ID = " + ID);
            if (dt.Rows.Count > 0)
            {
                txt_Kod.Text = Convert.ToString(dt.Rows[0]["pr_Kodu"]);
                txt_Ad.Text = Convert.ToString(dt.Rows[0]["pr_Adi"]);
                txt_Baslik.Tag = Convert.ToString(dt.Rows[0]["pr_Sinif"]);
                look_YaziciSekli.EditValue = Convert.ToString(dt.Rows[0]["pr_YaziciSekli"]);
                look_YaziciTuru.EditValue = Convert.ToString(dt.Rows[0]["pr_YaziciTuru"]);
                cmb_Yazicilar.Text = Convert.ToString(dt.Rows[0]["pr_Yazici"]);
                rdb_prOzelKod.EditValue = Convert.ToString(dt.Rows[0]["pr_PrinterOzelKod"]);
                look_Departman.EditValue = Convert.ToString(dt.Rows[0]["pr_Departman"]);
            }
        }

        private void newDesign()
        {
            MsSqlConnectionParameters connectionParam = new MsSqlConnectionParameters
            {
                ServerName = dbtools.server,
                DatabaseName = dbtools.database,
                UserName = dbtools.users,
                Password = dbtools.pwd,
                AuthorizationType = MsSqlAuthorizationType.SqlServer
            };

            CustomSqlQuery sql_Veri = new CustomSqlQuery
            {

            };
        }
    }
}

