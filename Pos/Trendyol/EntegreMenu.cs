using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using Newtonsoft.Json;
using Pos.Class;
using Pos.Controllers;
using Pos.Entities;
using Pos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos.Trendyol
{
    public partial class EntegreMenu : Form
    {
        int tip = 0;
        public EntegreMenu(int tip) // yemeksepeti = 0; getir = 1; trendyol = 2;
        {
            InitializeComponent();
            this.tip = tip;
        }
        public void loadingAc()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
        }

        public void loadingKapat()
        {
            SplashScreenManager.CloseForm(false);
        }

        TrendyolApi trendyolApi = new TrendyolApi();
        public void tumMenuListele(bool bos = false)
        {
            try
            {
                loadingAc();

                string filtre = "";
                if (bos)
                {
                    filtre = " and recId='999888777'";
                }

                string query = @"select case 
when tip=0 then 'Yemek Sepeti'
when tip=1 then 'Getir'
when tip=2 then 'Trendyol'
end as tipAd,
* from entegreMenu where recDep='" + Departman.Dep_Kodu + "' and  tip='" + tip + @"' " + filtre + @" order by entegreAd";

                treeListMenu.ClearNodes();
                treeListMenu.DataSource = dbtools.SelectTableR(query);
                treeListMenu.RefreshDataSource();
                treeviewCountYaz(treeListMenu);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            loadingKapat();
        }



        public void treeviewCountYaz(TreeList tree)
        {
            if (tree.Nodes.Count > 0)
            {
                tree.OptionsView.ShowSummaryFooter = true;
                TreeListColumn column = tree.Columns["tipAd"];
                column.AllNodesSummary = true;
                column.SummaryFooterStrFormat = "{0:n0}";
                column.SummaryFooter = SummaryItemType.Count;
                tree.Update();
            }
        }


        private void EntegreMenu_Load(object sender, EventArgs e)
        {
            dataTableYemekSepetiMenu = null; yemekSepetiMenuModel = null;
            treeListMenu.OptionsSelection.MultiSelect = true;
            treeListMenu.OptionsView.ShowCheckBoxes = true;

            odemeTipListele();

            rmosReceteListele();
            tumMenuListele();

            ayarlariGetir();

            menuBottomGetir.Visible = false;
            menuBottomYemekSepeti.Visible = false;
            menuBottomTrendyol.Visible = false;

            switch (tip)
            {
                case 0: // yemek sepeti
                    menuBottomYemekSepeti.Visible = true;
                    break;
                case 1: // getir
                    menuBottomGetir.Visible = true;
                    break;
                case 2: // trendyol
                    menuBottomTrendyol.Visible = true;
                    break;
            }

        }

        public void ayarlariGetir()
        {
            try
            {
                var ayarlar = entegreAyarlarController.listele();
                if (ayarlar != null)
                {
                    txtEslesmeyenId.Text = ayarlar.eslesmeyenUrunId;
                    txtEslesmeyenAd.Text = ayarlar.eslesmeyenUrunAd;
                    txtEslesmeyenFiyat.Text = ayarlar.eslesmeyenUrunFiyat.ToString();
                    txtGetirResSecretKey.Text = ayarlar.getirRestaurantSecretKey;
                    txtGetirAppSecretKey.Text = ayarlar.getirAppSecretKey;
                    txtGetirAktifmi.Checked = Convert.ToBoolean(ayarlar.getirDurum);
                    txtTrendyolAktifmi.Checked = Convert.ToBoolean(ayarlar.trendyolDurum);
                    txtTrendyolTestApi.Checked = Convert.ToBoolean(ayarlar.trendyolApiTest);
                    txtTrendyolSaticiId.Text = ayarlar.trendyolSupplierId.ToString();
                    txtTrendyolRestoranId.Text = ayarlar.trendyolStoreId.ToString();
                    txtTrendyolApiKey.Text = ayarlar.trendyolApiKey;
                    txtTrendyolApiSecret.Text = ayarlar.trendyolApiSecret;
                    txtTrendyolApiLink.Text = ayarlar.trendyolApiLink;
                }

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "ayarlariGetir", "", ex);
            }
        }

        private void btnTumMenu_Click(object sender, EventArgs e)
        {
            tumuListele();
        }

        public void tumuListele()
        {
            try
            {
                loadingAc();

                tumMenuListele();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadingKapat();
        }

        public void rmosReceteListele()
        {
            try
            {
                string query = @"Select Rec_Genelkod as recId,Rec_Ad,Rec_Fiyat from Cst_Recete";
                DataTable dt = dbtools.SelectTableR(query);


                repositoryItemSearchLookUpEdit1.DataSource = dt;
                repositoryItemSearchLookUpEdit1.ValueMember = "recId";
                repositoryItemSearchLookUpEdit1.DisplayMember = "Rec_Ad";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static string MyClass = "EntegreMenu";


        private void treeListMenu_NodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            if (e.Node.Checked) // e.Node.Selected
            {
                e.Appearance.BackColor = Color.Blue;
                e.Appearance.ForeColor = Color.White;
            }
        }

        private void btnEslemeyenMenuListele_Click(object sender, EventArgs e)
        {
            tumMenuListele(true);
        }

        private void treeListMenu_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    Clipboard.SetText(treeListMenu.GetFocusedDisplayText());
                    e.Handled = true;
                    RHMesaj.alertMesajSagUst("\"" + treeListMenu.GetFocusedDisplayText() + "\"", "KOPYALANDI", 1);
                }
            }
            catch (Exception ex)
            {
                //RHMesaj.MyMessageError(MyClass, "gridView22_KeyDown", "", ex);
            }

        }

        EntegreAyarlarController entegreAyarlarController = new EntegreAyarlarController();

        private void btnAyarlariKaydet_Click(object sender, EventArgs e)
        {
            try
            {

                entegreAyarlar ayarlar1 = new entegreAyarlar();
                ayarlar1.eslesmeyenUrunId = txtEslesmeyenId.Text;
                ayarlar1.eslesmeyenUrunAd = txtEslesmeyenAd.Text;
                ayarlar1.eslesmeyenUrunFiyat = Convert.ToDecimal(txtEslesmeyenFiyat.Text);
                ayarlar1.getirRestaurantSecretKey = txtGetirResSecretKey.Text;
                ayarlar1.getirAppSecretKey = txtGetirAppSecretKey.Text;
                ayarlar1.getirDurum = txtGetirAktifmi.Checked;
                ayarlar1.trendyolDurum = txtTrendyolAktifmi.Checked;
                ayarlar1.trendyolApiTest = txtTrendyolTestApi.Checked;
                ayarlar1.trendyolSupplierId = Convert.ToInt32(txtTrendyolSaticiId.Text);
                ayarlar1.trendyolStoreId = Convert.ToInt32(txtTrendyolRestoranId.Text);
                ayarlar1.trendyolApiKey = txtTrendyolApiKey.Text;
                ayarlar1.trendyolApiSecret = txtTrendyolApiSecret.Text;
                ayarlar1.trendyolApiLink = txtTrendyolApiLink.Text;
                ayarlar1.recDep = Departman.Dep_Kodu;

                entegreAyarlarController.kaydet(ayarlar1);


                Program.main.timerTrendyol.Enabled = txtTrendyolAktifmi.Checked;

                Program.main.barButtonItem6.Enabled = txtTrendyolAktifmi.Checked;
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnAyarlariKaydet_Click", "", ex);
            }

            ayarlariGetir();
            RHMesaj.alertMesajSagUst("AYARLAR", "KAYDEDİLDİ", 2);
        }

        private void btnTrendyolInternetMenuCek_Click(object sender, EventArgs e)
        {
            try
            {
                loadingAc();
                trendyolApi = new TrendyolApi();
                trendyolApi.menuKaydet();
                tumMenuListele();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadingKapat();
        }

        private void btnOdemeListele_Click(object sender, EventArgs e)
        {
            odemeTipListele();
        }


        public void odemeTipListele()
        {
            try
            {
                OdemeTipController odemeTipController = new OdemeTipController();
                odemeTipController.kaydet();


                recOdemeTipListele();

                string query = @"select 
case   
when tip=0 then 'Yemek Sepeti' 
when tip=1 then 'Getir' 
when tip=2 then 'Trendyol' 
end as entegreAd,
* from entegreOdemeTip";
                DataTable dataTable = dbtools.SelectTableR(query);
                gridControlOdemeTip.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "odemeTipListele", "", ex);
            }
        }

        public void recOdemeTipListele()
        {
            try
            {
                string query = @"select Pkod_Ad,Pkod_Kod,Pkod_Dep from Pos_Kodlar where Pkod_Sinif = '11' order by Pkod_Kod";
                DataTable dt = dbtools.SelectTableR(query);


                repositoryItemLookUpEdit1.DataSource = dt;
                repositoryItemLookUpEdit1.ValueMember = "Pkod_Kod";
                repositoryItemLookUpEdit1.DisplayMember = "Pkod_Ad";

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "recOdemeTipListele", "", ex);
            }




        }

        private void repositoryItemLookUpEdit1_Leave(object sender, EventArgs e)
        {
            try
            {
                if ((sender as LookUpEdit).EditValue == null) { return; }

                DataRowView dataRow = (sender as LookUpEdit).GetSelectedDataRow() as DataRowView;
                if (dataRow == null) return;
                string Pkod_Kod = dataRow["Pkod_Kod"].ToString();
                string Pkod_Ad = dataRow["Pkod_Ad"].ToString();
                string Pkod_Dep = dataRow["Pkod_Dep"].ToString();

                string id = gridViewOdemeTip.GetFocusedRowCellValue("id").ToString();

                dbtools.execcmdR("update entegreOdemeTip set recOdemeKod='" + Pkod_Kod + "',recOdemeAd='" + Pkod_Ad + "',recDep='" + Pkod_Dep + "' where id='" + id + "'");

                RHMesaj.alertMesajSagUst("ÖDEME TİP", "Güncellendi...", 5);


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "repositoryItemLookUpEdit1_Leave", "", ex);
            }


        }

        public void yemeksepetiKarsilikGir(string F11RecGenelKod,string entegreMenuId,string entegreMenuMasterId) // 
        {
            try
            {
                if (tip != 0 || dataTableYemekSepetiMenu == null || dataTableYemekSepetiMenu.Rows.Count < 1 || yemekSepetiMenuModel==null || yemekSepetiMenuModel.Count<1)
                {
                    return;
                }

                for (int i = 2; i < yemekSepetiMenuModel.Count; i++)
                {
                    var menum = yemekSepetiMenuModel[i];

                    if (menum.Product == entegreMenuMasterId && menum.F18==entegreMenuId)
                    {
                        menum.F11 = F11RecGenelKod;
                    }
                }

              
            }
            catch (Exception ex)
            {

            }
        }

        private void repositoryItemSearchLookUpEdit1_QueryCloseUp(object sender, CancelEventArgs e)
        {
            try
            {
                if ((sender as SearchLookUpEdit).EditValue == null) { return; }

                //int newdataSourceRowIndex = (sender as SearchLookUpEdit).Properties.View.GetDataSourceRowIndex((sender as SearchLookUpEdit).Properties.View.FocusedRowHandle);


                var rec_fiyat = (sender as SearchLookUpEdit).Properties.View.GetFocusedDataRow().Field<decimal>("Rec_Fiyat");
                var Rec_Ad = (sender as SearchLookUpEdit).Properties.View.GetFocusedDataRow().Field<string>("Rec_Ad");
                string Rec_Genelkod = (sender as SearchLookUpEdit).Properties.View.GetFocusedDataRow().Field<string>("recId");



                string id = Convert.ToString(treeListMenu.GetFocusedRowCellValue(treeListMenu.Columns["id"]));
                string entegreAd = Convert.ToString(treeListMenu.GetFocusedRowCellValue(treeListMenu.Columns["entegreAd"]));

                string recFiyat = rec_fiyat.ToString().Replace(",", ".");
                dbtools.execcmdR("update entegreMenu set recId='" + Rec_Genelkod + "',recAd='" + Rec_Ad + "',recFiyat=" + recFiyat + " where id='" + id + "'");

                //sonradan eklendi
                string entegreMenuId = Convert.ToString(treeListMenu.GetFocusedRowCellValue(treeListMenu.Columns["entegreId"]));
                string entegreMenuMasterId = Convert.ToString(treeListMenu.GetFocusedRowCellValue(treeListMenu.Columns["entegreMenuMasterId"]));
                dbtools.execcmdR("update entegreSiparisUrunler set recId='" + Rec_Genelkod + "',recAd='" + Rec_Ad + "',recFiyat='" + recFiyat + "' where entegreMenuId='" + entegreMenuId + "' and entegreMenuMasterId='" + entegreMenuMasterId + "'");

                treeListMenu.SetFocusedRowCellValue("recFiyat", rec_fiyat);


                yemeksepetiKarsilikGir(Rec_Genelkod, entegreMenuId, entegreMenuMasterId);


                RHMesaj.alertMesajSagUst(entegreAd, Rec_Ad + "\nOlarak Güncellendi", 2);


            }
            catch (Exception ex)
            {
                //RHMesaj.MyMessageError(MyClass, "repositoryItemSearchLookUpEdit1_QueryCloseUp", "", ex);
            }
        }

        private void btnYemekSepetiMenuYukle_Click(object sender, EventArgs e)
        {
            excelYukle();
            tumuListele();
        }


        DataTable dataTableYemekSepetiMenu = null;
        List<YemekSepetiGoMenuModel> yemekSepetiMenuModel= null;
        public void excelYukle()
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Excel Seç";
                    dlg.Filter = "Excel Files (*.xls;*.xlsx;)|*.XLS;*.XLSX;";

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        string strFile = dlg.FileName;
                        dataTableYemekSepetiMenu = LoadExcel(strFile, "Worksheet");
                        if (dataTableYemekSepetiMenu == null || dataTableYemekSepetiMenu.Rows.Count < 1) { MessageBox.Show("Worksheet çalışma alanı yüklenemedi"); return; }
                        string json = JsonConvert.SerializeObject(dataTableYemekSepetiMenu);
                        yemekSepetiMenuModel = JsonConvert.DeserializeObject<List<YemekSepetiGoMenuModel>>(json);
                        var F11 = yemekSepetiMenuModel.FirstOrDefault().F11.ToString();
                        var F18 = yemekSepetiMenuModel.FirstOrDefault().F18.ToString();
                        if (!F11.Equals("remote_code")) MessageBox.Show("F11 sütünü remote_code değildir ! ");
                        if (!F18.Equals("internal_variation_code")) MessageBox.Show("F18 sütünü internal_variation_code değildir ! ");

                        RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                        var ayarlar2 = db.entegreAyarlar.Where(x => x.recDep == Departman.Dep_Kodu).FirstOrDefault();
                        if (ayarlar2 == null) { RHMesaj.MyMessageInformation("Ayarları bir kere kaydet yapın"); return; }

                        EntegreMenuController entegreMenuController = new EntegreMenuController();

                        for (int i = 2; i < yemekSepetiMenuModel.Count; i++)
                        {
                            var menum = yemekSepetiMenuModel[i];

                            menum.F19 = menum.F19 == null ? "" : menum.F19.ToString();
                            menum.F16 = menum.F16 == null ? "" : menum.F16.ToString();

                            entegreMenu entegreMenu = new entegreMenu();
                            entegreMenu.tip = 0;
                            entegreMenu.recDep = Departman.Dep_Kodu;
                            entegreMenu.urunSabitFiyat = menum.F22 == null ? 0 : Convert.ToDecimal(menum.F22);
                            entegreMenu.aktif = true;
                            entegreMenu.recId = ayarlar2.eslesmeyenUrunId;
                            entegreMenu.recFiyat = ayarlar2.eslesmeyenUrunFiyat;
                            entegreMenu.recAd = ayarlar2.eslesmeyenUrunAd;
                            entegreMenu.entegreMenuMasterId = menum.Product;
                            entegreMenu.entegreId = menum.F18;
                            entegreMenu.entegreAd = (menum.F14 + " " + menum.F19).Trim();
                            entegreMenu.entegreAciklama = menum.F16.Trim();
                            entegreMenu.entegreJson =json;

                            entegreMenuController.kaydet(entegreMenu);
                        }

                        entegreMenuController.menuMasterIdKaydet();
                        MessageBox.Show("YÜKLENDİ");

                    }
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "", "", ex);
            }

        }



        public void yazdir(TreeList gridControl)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2010) (.xlsx)|*.xlsx|Excel (2003)(.xls)|*.xls|RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".xls":
                            gridControl.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gridControl.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            gridControl.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            gridControl.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            gridControl.ExportToHtml(exportFilePath);
                            break;

                        default:
                            break;
                    }

                    //string dosyaKonum = Path.GetDirectoryName(Application.ExecutablePath);

                    string basePath = Path.GetDirectoryName(exportFilePath);

                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(basePath)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
            }
        }

        public void yazdirGrid(GridControl gridControl)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2010) (.xlsx)|*.xlsx|Excel (2003)(.xls)|*.xls|RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".xls":
                            gridControl.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gridControl.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            gridControl.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            gridControl.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            gridControl.ExportToHtml(exportFilePath);
                            break;

                        default:
                            break;
                    }

                    //string dosyaKonum = Path.GetDirectoryName(Application.ExecutablePath);

                    string basePath = Path.GetDirectoryName(exportFilePath);

                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(basePath)
                    {
                        UseShellExecute = true
                    };
                    p.Start();

                    MessageBox.Show("YAZDIRILDI");

                }
            }
        }
        private DataTable LoadExcel(string strFile, string sheetName)
        {
            System.Data.DataTable dtXLS = new System.Data.DataTable(sheetName);
            try
            {
                string strConnectionString = "";
                if (strFile.Trim().EndsWith(".xlsx"))
                {
                    strConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", strFile);
                }
                else if (strFile.Trim().EndsWith(".xls"))
                {
                    strConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", strFile);
                }
                else if (strFile.Trim().EndsWith(".xlsm"))
                {
                    strConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Macro;HDR=Yes;IMEX=1\";", strFile);
                }
                OleDbConnection SQLConn = new OleDbConnection(strConnectionString);
                SQLConn.Open();
                OleDbDataAdapter SQLAdapter = new OleDbDataAdapter();
                string sql = "SELECT * FROM [" + sheetName + "$]";
                OleDbCommand selectCMD = new OleDbCommand(sql, SQLConn);
                SQLAdapter.SelectCommand = selectCMD;
                SQLAdapter.Fill(dtXLS);
                SQLConn.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return dtXLS;
        }

        private void btnYemekSepetiMenuYazdir_Click(object sender, EventArgs e)
        {
            try
            {
                if (yemekSepetiMenuModel==null)
                {
                    RHMesaj.MyMessageInformation("Yemek Sepeti Menüsü Yüklenmeden\nYemek Sepeti Menüsü Yazdırılamaz...!");
                    return;
                }

                DataTable dataTable = treeListMenu.DataSource as DataTable;

                foreach (DataRow item in dataTable.Rows)
                {
                    string entegreId = item["entegreId"].ToString();
                    string entegreMenuMasterId = item["entegreMenuMasterId"].ToString();
                    string recGenelKod = item["recId"].ToString();

                    yemeksepetiKarsilikGir(recGenelKod, entegreId, entegreMenuMasterId);
                }


                gridViewYemekSepetiMenuYazdir.Columns.Clear();
                gridControlYemekSepetiMenuYazdir.DataSource = null;
                gridControlYemekSepetiMenuYazdir.DataSource = yemekSepetiMenuModel;
                yazdirGrid(gridControlYemekSepetiMenuYazdir);
            }
            catch(Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnYemekSepetiMenuYazdir_Click", "",ex);
            }

        }
    }
}
