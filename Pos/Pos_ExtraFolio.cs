using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Pos.Class;
using Pos.Print;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Pos_ExtraFolio : DevExpress.XtraEditors.XtraForm
    {
        public Pos_ExtraFolio()
        {
            InitializeComponent();
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            //    SelectDevice();
            //    establishContext();
        }

        //int Rez_Id = 0;

        int KartID = 0;

        public void gridviewSum(GridView grid)
        {
            DataTable dt = dbtools.SelectTable("select Kodlar_Ad From Stok_Kodlar WITH(NOLOCK) where Kodlar_Sinif = '08' order by Kodlar_Id");

            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        for (int j = 0; j < grid.Columns.Count; j++)
            //        {
            //            if (dt.Rows[i][0].ToString() == grid.Columns[j].FieldName.ToString())
            //            {
            //                if (grid.Columns.Count > 0)
            //                {
            //                    grid.Columns[j].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //                    grid.Columns[j].SummaryItem.FieldName = grid.Columns[j].FieldName;
            //                    grid.Columns[j].SummaryItem.DisplayFormat = "{0:n2}";
            //                    grid.UpdateTotalSummary();
            //                }
            //            }
            //        }
            //    }
            //}
            for (int j = 0; j < grid.Columns.Count; j++)
            {
                if (grid.Columns.Count > 0)
                {
                    if (grid.Columns[j].ColumnType == typeof(decimal))
                    {
                        grid.Columns[j].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grid.Columns[j].SummaryItem.FieldName = grid.Columns[j].FieldName;
                        grid.Columns[j].SummaryItem.DisplayFormat = "{0:n2}";
                        grid.UpdateTotalSummary();
                    }
                }
            }

        }
        private void KartNoSor()
        {
            //KartID = 0;
            if (txt_BakiyeKartNo.Text.Replace(" ", "") != "")
            {

                //DataTable dt = Fronttools.SelectTable(@"


                //        select CardF_RezID,CardF_Ad,CardF_Soyad,CardF_Odano,KartF.ID as ID ,CardF_GirisTrh,CardF_CikisTrh

                //        From KartF 
                //        left join Rez on CardF_RezID = Rez_Id 

                //        where  KartF.CardF_No = '" + txt_BakiyeKartNo.Text.Replace(" ", "") + "'");
                DataTable dt = Fronttools.SelectTable(@"


                        select CardF_RezID,CardF_Ad,CardF_Soyad,CardF_Odano,KartF.ID as ID ,CardF_GirisTrh,CardF_CikisTrh

                        From KartF 
                        left join Rez on CardF_RezID = Rez_Id 

                        where KartF.ID = '" + KartID + "'");

                if (dt.Rows.Count > 0)
                {
                    //DateTime GirisTarih = Convert.ToDateTime(Fronttools.DegerGetir("Select CardF_GirisTrh From KartF with(NOLOCK) where ID = '" + KartID + @"'"));
                    //DateTime CikisTarih = Convert.ToDateTime(Fronttools.DegerGetir("Select CardF_CikisTrh From KartF with(NOLOCK) where ID = '" + KartID + @"'"));


                    txt_BakiyeAdi.Text = Convert.ToString(dt.Rows[0]["CardF_Ad"]);
                    txt_BakiyeSoyad.Text = Convert.ToString(dt.Rows[0]["CardF_Soyad"]);
                    txt_BakiyeFolioID.Text = Convert.ToString(dt.Rows[0]["CardF_RezID"]);
                    txt_BakiyeOda.Text = Convert.ToString(dt.Rows[0]["CardF_Odano"]);
                    dateEdit4.DateTime = Convert.ToDateTime(dt.Rows[0]["CardF_GirisTrh"]).Date;
                    dateEdit1.DateTime = Convert.ToDateTime(dt.Rows[0]["CardF_CikisTrh"]).Date;
                    KartID = Convert.ToInt32(dt.Rows[0]["ID"]);

                    Bakiye(Convert.ToString(dt.Rows[0]["CardF_RezID"]), KartID);

                    FolioListele(Convert.ToString(dt.Rows[0]["CardF_RezID"]), dateEdit4.DateTime.Date, dateEdit1.DateTime.Date, KartID);
                }
                //else
                //{
                //    MessageBox.Show(res_man.GetString("Kayıtlı Kart No Bulunamadı..", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    Temizle(this);
                //    return;
                //}
            }
        }

        private void KartNoSor2()
        {
            //KartID = 0;
            if (txt_BakiyeKartNo.Text.Replace(" ", "") != "")
            {

                //DataTable dt = Fronttools.SelectTable(@"


                //        select CardF_RezID,CardF_Ad,CardF_Soyad,CardF_Odano,KartF.ID as ID ,CardF_GirisTrh,CardF_CikisTrh

                //        From KartF 
                //        left join Rez on CardF_RezID = Rez_Id 

                //        where  KartF.CardF_No = '" + txt_BakiyeKartNo.Text.Replace(" ", "") + "'");
                DataTable dt = Fronttools.SelectTable(@"


                        select CardF_RezID,CardF_Ad,CardF_Soyad,CardF_Odano,KartF.ID as ID ,CardF_GirisTrh,CardF_CikisTrh

                        From KartF 
                        left join Rez on CardF_RezID = Rez_Id 

                        where KartF.CardF_No = '" + txt_BakiyeKartNo.Text.Replace(" ", "") + "' and CardF_R_I_H = 'I'");

                if (dt.Rows.Count > 0)
                {
                    //DateTime GirisTarih = Convert.ToDateTime(Fronttools.DegerGetir("Select CardF_GirisTrh From KartF with(NOLOCK) where  KartF.CardF_No = '" + txt_BakiyeKartNo.Text.Replace(" ", "") + @"'"));
                    //DateTime CikisTarih = Convert.ToDateTime(Fronttools.DegerGetir("Select CardF_CikisTrh From KartF with(NOLOCK) where  KartF.CardF_No = '" + txt_BakiyeKartNo.Text.Replace(" ", "") + @"'"));


                    txt_BakiyeAdi.Text = Convert.ToString(dt.Rows[0]["CardF_Ad"]);
                    txt_BakiyeSoyad.Text = Convert.ToString(dt.Rows[0]["CardF_Soyad"]);
                    txt_BakiyeFolioID.Text = Convert.ToString(dt.Rows[0]["CardF_RezID"]);
                    txt_BakiyeOda.Text = Convert.ToString(dt.Rows[0]["CardF_Odano"]);
                    dateEdit4.DateTime = Convert.ToDateTime(dt.Rows[0]["CardF_GirisTrh"]).Date;
                    dateEdit1.DateTime = Convert.ToDateTime(dt.Rows[0]["CardF_CikisTrh"]).Date;
                    KartID = Convert.ToInt32(dt.Rows[0]["ID"]);

                    Bakiye(Convert.ToString(dt.Rows[0]["CardF_RezID"]), KartID);

                    FolioListele(Convert.ToString(dt.Rows[0]["CardF_RezID"]), dateEdit4.DateTime.Date, dateEdit1.DateTime.Date, KartID);
                }
                //else
                //{
                //    MessageBox.Show(res_man.GetString("Kayıtlı Kart No Bulunamadı..", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    Temizle(this);
                //    return;
                //}
            }
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void Doldur()
        {
            DataTable dt = Fronttools.SelectTable(@"select  Kodlar_Ad, Kodlar_Kod from Kodlar where Kodlar_Sinif  ='39'");
            if (dt.Rows.Count > 0)
            {
                look_Konaklama.Properties.DataSource = dt;
                look_Konaklama.Properties.DisplayMember = "Kodlar_Ad";
                look_Konaklama.Properties.ValueMember = "Kodlar_Kod";
            }

            DataTable dt1 = Fronttools.SelectTable(@"select  Kodlar_Ad, Kodlar_Kod from Kodlar where Kodlar_Sinif  ='02'");
            if (dt1.Rows.Count > 0)
            {
                look_KurKodu.Properties.DataSource = dt1;
                look_KurKodu.Properties.DisplayMember = "Kodlar_Ad";
                look_KurKodu.Properties.ValueMember = "Kodlar_Kod";
            }

            DataTable dt2 = Fronttools.SelectTable(@"select  Kodlar_Ad, Kodlar_Kod from Kodlar where Kodlar_Sinif  ='10' order by Kodlar_Kod");
            if (dt2.Rows.Count > 0)
            {
                look_OdemeSekli.Properties.DataSource = dt2;
                look_OdemeSekli.Properties.DisplayMember = "Kodlar_Ad";
                look_OdemeSekli.Properties.ValueMember = "Kodlar_Kod";
            }

            DataTable dt3 = Fronttools.SelectTable(@"select  Kodlar_Ad, Kodlar_Kod from Kodlar where Kodlar_Sinif  ='09' order by Kodlar_Kod");
            if (dt3.Rows.Count > 0)
            {
                look_MusteriTipi.Properties.DataSource = dt3;
                look_MusteriTipi.Properties.DisplayMember = "Kodlar_Ad";
                look_MusteriTipi.Properties.ValueMember = "Kodlar_Kod";
            }

            DataTable dt4 = Fronttools.SelectTable(@"select  Ac_Kodu, Ac_Adi  from Acenta with (NOLOCK)  where  Ac_Pasif <> 1 order by Ac_Kodu");
            if (dt4.Rows.Count > 0)
            {
                look_Acenta.Properties.DataSource = dt4;
                look_Acenta.Properties.DisplayMember = "Ac_Adi";
                look_Acenta.Properties.ValueMember = "Ac_Kodu";
            }

            DataTable NB = new DataTable();
            NB.Columns.Add("Kodu", typeof(string));
            NB.Columns.Add("Adi", typeof(string));
            NB.Rows.Add("DA", "Döviz Alış");
            NB.Rows.Add("DS", "Döviz Satış");
            NB.Rows.Add("EA", "Efektif Alış");
            NB.Rows.Add("ES", "Efektif Satış");
            look_DovizSekli.Properties.DataSource = NB;
            look_DovizSekli.Properties.ValueMember = "Kodu";
            look_DovizSekli.Properties.DisplayMember = "Adi";

            DataTable dt5 = Fronttools.SelectTable(@"select Kodlar_Kod, Kodlar_Ad from Kodlar with(NOLOCK)  where Kodlar_Sinif ='01'  and Kodlar_Ba = 'A' order by Kodlar_Kod");
            if (dt5.Rows.Count > 0)
            {
                look_Kapatma.Properties.DataSource = dt5;
                look_Kapatma.Properties.DisplayMember = "Kodlar_Ad";
                look_Kapatma.Properties.ValueMember = "Kodlar_Kod";

            }

            DataTable dt6 = Fronttools.SelectTable(@"select Kodlar_Kod, Kodlar_Ad from Kodlar with(NOLOCK)  where Kodlar_Sinif ='01'  and Kodlar_Ba = 'B' order by Kodlar_Kod");
            if (dt6.Rows.Count > 0)
            {
                look_IadeNkt.Properties.DataSource = dt6;
                look_IadeNkt.Properties.DisplayMember = "Kodlar_Ad";
                look_IadeNkt.Properties.ValueMember = "Kodlar_Kod";

                look_IadeKK.Properties.DataSource = dt6;
                look_IadeKK.Properties.DisplayMember = "Kodlar_Ad";
                look_IadeKK.Properties.ValueMember = "Kodlar_Kod";

                look_GelirIade.Properties.DataSource = dt6;
                look_GelirIade.Properties.DisplayMember = "Kodlar_Ad";
                look_GelirIade.Properties.ValueMember = "Kodlar_Kod";
            }


            DataTable dt7 = Fronttools.SelectTable(@"select Kodlar_Kod, Kodlar_Ad from Kodlar with(NOLOCK)  where Kodlar_Sinif ='01'  and Kodlar_Ba = 'A' 
            AND (Kodlar_Kod in (SELECT fieldvalue FROM dbo.stringArray('" + Param_ExtraFolio.Front_Kapatma + @"',',')))
            order by Kodlar_Kod");
            if (dt7.Rows.Count > 0)
            {
                look_OdemeDep.Properties.DataSource = dt7;
                look_OdemeDep.Properties.DisplayMember = "Kodlar_Ad";
                look_OdemeDep.Properties.ValueMember = "Kodlar_Kod";

            }

            txtCardF_Kartno.Focus();
        }

        private void DetayOzetList()
        {
            gridControl4.DataSource = null;
            gridView4.Columns.Clear();
            DataTable dt = Fronttools.SelectTable("exec KartF_Pos_Listele @Tarih = '" + dateEdit2.DateTime.Date + "', @Tarih2 = '" + dateEdit3.DateTime.Date + "', @RaporTipi ='" + radioGroup1.EditValue + "'");
            gridControl4.DataSource = dt;


            gridControl5.DataSource = null;
            gridView5.Columns.Clear();
            //DataTable dt2 = Fronttools.SelectTable(@"exec KartF_Pos_Listele @Tarih = '" + dateEdit2.DateTime.Date + "', @Tarih2 = '" + dateEdit3.DateTime.Date + "', @RaporTipi ='" + radioGroup1.EditValue + "'");
            DataTable dt2 = Fronttools.SelectTable(@"exec StpKumhrk_Bul @Kumhrk_KartTarih1 = '" + dateEdit2.DateTime.Date + "', @Kumhrk_KartTarih2 = '" + dateEdit3.DateTime.Date + "', @xTip = 57");
            gridControl5.DataSource = dt2;
        }

        private void NFC_Listele()
        {
            try
            {
                gridControl11.DataSource = null;
                //gridView11.Columns.Clear();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis_Rapor";
                com.Parameters.AddWithValue("@Rapor_Tipi", 26);
                com.Parameters.AddWithValue("@Tarih1", dateEdit6.DateTime.Date);
                com.Parameters.AddWithValue("@Tarih2", dateEdit5.DateTime.Date);
                com.Parameters.AddWithValue("@Departman", Departman.Dep_Kodu);
                com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                com.Parameters.AddWithValue("@KartFNo", txt_NfcKart.Text);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                //gridControl1.DataSource = dt;
                gridControl11.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kart No Boş \n " + ex.Message);
            }

        }

        private void Excel(DevExpress.XtraGrid.GridControl gc)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string fName = string.Empty;
            saveFileDialog1.Filter = "Excel Document (*.xls)|*.xls";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "Kart Listesi.xls";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != null)
                {
                    XlsExportOptions opt = new XlsExportOptions();
                    DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.WYSIWYG;
                    opt.ShowGridLines = true;
                    gc.ExportToXls(saveFileDialog1.FileName, opt);
                }
            }

        }
        private void Rapor_Print(string header, GridControl grid, DateTime Tarih1, DateTime Tarih2)
        {
            string leftColumn = header + "  -  ";
            string rightColumn = Tarih1.ToLongDateString() + " - " + Tarih2.ToLongDateString();


            PrintingSystem printingSystem1 = new PrintingSystem();
            PrintableComponentLink printableComponentLink1 = new PrintableComponentLink();
            printingSystem1.Links.AddRange(new object[] { printableComponentLink1 });
            printableComponentLink1.Component = grid;
            printableComponentLink1.Landscape = false;
            printableComponentLink1.Margins = new System.Drawing.Printing.Margins(20, 20, 50, 20);

            PageHeaderFooter phf = printableComponentLink1.PageHeaderFooter as PageHeaderFooter;
            phf.Header.Content.Clear();
            phf.Header.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            phf.Header.Content.AddRange(new string[] { leftColumn, rightColumn });
            phf.Header.LineAlignment = BrickAlignment.Far;
            printableComponentLink1.ShowPreview();
        }


        private void Pos_ExtraFolio_Load(object sender, EventArgs e)
        {
            DataTable dthizmetodemekod = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");
            hizmetOdemeKod.Properties.DataSource = dthizmetodemekod;
            hizmetOdemeKod.Properties.DisplayMember = "Pkod_Ad";
            hizmetOdemeKod.Properties.ValueMember = "Pkod_Kod";


            DataTable dtRecete = dbtools.SelectTable("select Rec_Genelkod,Rec_Ad from Cst_Recete WITH(NOLOCK) order by Rec_Genelkod");
            HizmetReceteKod.Properties.DataSource = dtRecete;
            HizmetReceteKod.Properties.DisplayMember = "Rec_Ad";
            HizmetReceteKod.Properties.ValueMember = "Rec_Genelkod";

            DataTable dtReceteCocuk = dbtools.SelectTable("select Rec_Genelkod,Rec_Ad from Cst_Recete WITH(NOLOCK) order by Rec_Genelkod");
            HizmetReceteKodCocuk.Properties.DataSource = dtReceteCocuk;
            HizmetReceteKodCocuk.Properties.DisplayMember = "Rec_Ad";
            HizmetReceteKodCocuk.Properties.ValueMember = "Rec_Genelkod";

            txtKartFIndirim.Enabled = User.Pos_KartfIndirimAktif;

            groupControl6.Visible = User.Pos_KartF_CheckOut;

            txtCardF_GirisTrh.DateTime = Param.Tarih.Date;
            txtCardF_CikisTrh.DateTime = Param.Tarih.Date;
            date_GelirTakip.DateTime = Param.Tarih.Date;

            look_DovizKod.Properties.DataSource = dbtools.SelectTable("select Mkodlar_Kod as kod,Mkodlar_Ad as ad from Muh_Kodlar where Mkodlar_sinif = '02' order by Mkodlar_Kod");
            look_DovizKod.Properties.DisplayMember = "ad";
            look_DovizKod.Properties.ValueMember = "kod";
            look_DovizKod.EditValue = Param.Doviz_Kodu;

            Param_ExtraFolio.Param_ExtraFolioYukle();


            xtraTabControl1.SelectedTabPage = tab_KartF;
            txtCardF_Odano.Text = Param_ExtraFolio.Front_KartF_Odano;



            CardF_Listele();
            if (Param.Param_ExtraFolioAcma == false)
            {
                if (txtCardF_Odano.Text != "")
                {
                    CardF_FolioBul(txtCardF_Odano.Text);
                }
            }
            else
            {
                CardF_FolioBul(xRez_Id.ToString());
            }
            txtCardF_Kartno.Focus();


            Doldur();

            txtCardF_Kartno.Focus();
            txtCardF_Kartno.Select();



            btnKatTanimSil.Visible = Convert.ToBoolean(dbtools.DegerGetir("select top 1 isnull(Pos_KartTanimSil,0) as Pos_KartTanimSil from Rmosmuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'"));


            btnKatTanimDuzelt.Visible = Convert.ToBoolean(dbtools.DegerGetir("select top 1 isnull(Pos_KartTanimDuzelt,0) as Pos_KartTanimDuzelt from Rmosmuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'"));

            btnKatTanimKartTransfer.Visible = Convert.ToBoolean(dbtools.DegerGetir("select top 1 isnull(Pos_KartTanimTransfer,0) as Pos_KartTanimTransfer from Rmosmuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'"));

            btnKatTanimBakiyeTransfer.Visible = Convert.ToBoolean(dbtools.DegerGetir("select top 1 isnull(Pos_KartTanimBakiyeTransfer,0) as Pos_KartTanimBakiyeTransfer from Rmosmuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'"));

            paramyukle();

        }

        public void paramyukle()
        {
            Param_ExtraFolio.Param_ExtraFolioYukle();

            

            look_Acenta.EditValue = Param_ExtraFolio.Front_Acenta;
            look_DovizSekli.EditValue = Param_ExtraFolio.Front_DovizSekli;
            look_Konaklama.EditValue = Param_ExtraFolio.Front_Konaklama;
            look_KurKodu.EditValue = Param_ExtraFolio.Front_KurKodu;
            look_MusteriTipi.EditValue = Param_ExtraFolio.Front_MusteriTipi;
            look_OdemeSekli.EditValue = Param_ExtraFolio.Front_OdemeSekli;
            look_IadeNkt.EditValue = Param_ExtraFolio.Front_Iade;
            look_IadeKK.EditValue = Param_ExtraFolio.Front_IadeKK;
            rdb_Kart_Onburo.EditValue = Param_ExtraFolio.Front_KartF_Onburo;
            txtParam_OdaNo.Text = Param_ExtraFolio.Front_KartF_Odano;
            look_Kapatma.EditValue = Param_ExtraFolio.Front_Kapatma;
            look_GelirIade.EditValue = Param_ExtraFolio.Front_GelirIade;
            HizmetReceteKod.EditValue = Param_ExtraFolio.HizmetReceteKod;
            HizmetReceteKodCocuk.EditValue = Param_ExtraFolio.HizmetReceteKodCocuk;
            hizmetOdemeKod.EditValue = Param_ExtraFolio.hizmetOdemeKod;
            hizmetBedeliAktif.Checked = Param_ExtraFolio.hizmetBedeliAktif;
        }

        CheckButton chk_Durum = null;
        private void checkBtn(object sender, EventArgs e)
        {
            //Temizle(tab_KartF);

            CheckButton chkbtn = (CheckButton)sender;

            if (chk_Durum == null)
            {
                chk_Durum = chkbtn;
            }

            if (chk_Durum == chk_Folio)
            {
                gridControl3.DataSource = null;
                CardF_MusteriTipi.SelectedIndex = 0;
                xtraTabControl1.SelectedTabPage = tab_KartF;
                chk_Folio.Checked = true;
                chk_Bakiye.Checked = false;
                chk_Param.Checked = false;
                chk_Rapor.Checked = false;
                Gitme = true;
                CardF_Listele();

                txtCardF_Odano.Text = Param_ExtraFolio.Front_KartF_Odano;
                if (Param.Param_ExtraFolioAcma == false)
                {
                    CardF_FolioBul(txtCardF_Odano.Text);
                }
                txtCardF_Kartno.Focus();
            }

            if (chk_Durum == chk_Bakiye)
            {
                gridControl2.DataSource = null;
                xtraTabControl1.SelectedTabPage = tab_BakiyeYukle;
                chk_Folio.Checked = false;
                chk_Bakiye.Checked = true;
                chk_Param.Checked = false;
                chk_Rapor.Checked = false;
                txt_BakiyeKartNo.Focus();
            }


            if (chk_Durum == chk_Param)
            {
                xtraTabControl1.SelectedTabPage = tab_Param;
                chk_Folio.Checked = false;
                chk_Bakiye.Checked = false;
                chk_Param.Checked = true;
                chk_Rapor.Checked = false;
                paramyukle();
            }


            if (chk_Durum == chk_Rapor)
            {
                gridControl4.DataSource = null;
                radioGroup1.SelectedIndex = 0;
                xtraTabControl1.SelectedTabPage = tab_Rapor;
                dateEdit2.DateTime = Param.Tarih;
                dateEdit3.DateTime = Param.Tarih;
                dateEdit6.DateTime = Param.Tarih;
                dateEdit5.DateTime = Param.Tarih;
                dateEdit7.DateTime = Param.Tarih;
                dateEdit8.DateTime = Param.Tarih;
                chk_Rapor.Checked = true;
                chk_Bakiye.Checked = false;
                chk_Param.Checked = false;
                chk_Folio.Checked = false;
                DetayOzetList();
            }

            chk_Durum = null;
        }

        private void CardF_FolioBul(string KartNo)
        {

            DataTable dt = new DataTable();
            if (Convert.ToString(lookCardF_FolioID.EditValue) == "" && (Convert.ToString(CardF_MusteriTipi.EditValue) == "OM" || Convert.ToString(CardF_MusteriTipi.EditValue) == "CM"))
            {
                dt = Fronttools.SelectTable(@"select Rez_Id,Rez_Adi_1,Rez_Odano,Rez_Adi_2,Rez_Giris_tarihi,Rez_Cikis_tarihi,Rez_limit_uyari_eh,ISNULL(Rez_Toplam_kisi,0) as Rez_Toplam_kisi from dbo.Rez where Rez_R_I_H = 'I' and Rez_Master_detay <> 'D' order by Rez_Odano ");
                lookCardF_FolioID.Properties.DataSource = dt;
                lookCardF_FolioID.Properties.DisplayMember = "Rez_Adi_1";
                lookCardF_FolioID.Properties.ValueMember = "Rez_Id";
            }

            if (Param.Param_ExtraFolioAcma == false)
            {
                dt = Fronttools.SelectTable(@"select Rez_Id,Rez_Adi_1,Rez_Adi_2,Rez_Odano,Rez_Giris_tarihi,Rez_Cikis_tarihi,Rez_limit_uyari_eh,ISNULL(Rez_Toplam_kisi,0) as Rez_Toplam_kisi from dbo.Rez where Rez_Odano = '" + KartNo + "' and Rez_R_I_H = 'I' and Rez_Master_detay <> 'D' order by Rez_Odano ");
            }
            else
            {
                dt = Fronttools.SelectTable(@"select Rez_Id,Rez_Adi_1,Rez_Adi_2,Rez_Odano,Rez_Giris_tarihi,Rez_Cikis_tarihi,Rez_limit_uyari_eh,ISNULL(Rez_Toplam_kisi,0) as Rez_Toplam_kisi from dbo.Rez where Rez_Id = '" + KartNo + "' and Rez_R_I_H = 'I' and Rez_Master_detay <> 'D' order by Rez_Odano ");

                if (dt == null || dt.Rows.Count < 1) // sonradan eklendi rambo
                {
                    dt = Fronttools.SelectTable(@"select Rez_Id,Rez_Adi_1,Rez_Adi_2,Rez_Odano,Rez_Giris_tarihi,Rez_Cikis_tarihi,Rez_limit_uyari_eh,ISNULL(Rez_Toplam_kisi,0) as Rez_Toplam_kisi from dbo.Rez where Rez_Odano = '" + KartNo + "' and Rez_R_I_H = 'I' and Rez_Master_detay <> 'D' order by Rez_Odano ");
                }

                if (dt == null || dt.Rows.Count < 1)
                {
                    KartNo = dbtools.DegerGetir("select top 1 Front_KartF_Odano from Pos_FolioParam ");

                    dt = Fronttools.SelectTable(@"select Rez_Id,Rez_Adi_1,Rez_Adi_2,Rez_Odano,Rez_Giris_tarihi,Rez_Cikis_tarihi,Rez_limit_uyari_eh,ISNULL(Rez_Toplam_kisi,0) as Rez_Toplam_kisi from dbo.Rez where Rez_Odano = '" + KartNo + "' and Rez_R_I_H = 'I' and Rez_Master_detay <> 'D' order by Rez_Odano ");
                }
            }

            if (dt.Rows.Count > 0)
            {
                lookCardF_FolioID.Properties.DataSource = dt;
                lookCardF_FolioID.Properties.DisplayMember = "Rez_Adi_1";
                lookCardF_FolioID.Properties.ValueMember = "Rez_Id";
                lookCardF_FolioID.ItemIndex = 0;

                txtCardF_Odano.EditValue = dt.Rows[0]["Rez_Odano"];

                txtCardF_Adi.Text = Convert.ToString(dt.Rows[0]["Rez_Adi_1"]);
                txtCardF_Soyad.Text = Convert.ToString(dt.Rows[0]["Rez_Adi_2"]);

                if (Convert.ToString(CardF_MusteriTipi.EditValue) == "GB")
                {
                    txtCardF_GirisTrh.EditValue = Param.Tarih;
                    txtCardF_CikisTrh.EditValue = Param.Tarih;
                }
                else
                {
                    txtCardF_GirisTrh.EditValue = dt.Rows[0]["Rez_Giris_tarihi"];
                    txtCardF_CikisTrh.EditValue = dt.Rows[0]["Rez_Cikis_tarihi"];
                }
                txtCardF_Kisi.EditValue = dt.Rows[0]["Rez_Toplam_kisi"];
                //rdbCardF_Uyari.EditValue = dt.Rows[0]["Rez_limit_uyari_eh"];

            }

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Temizle(Control Ctrl)
        {
            foreach (Control item in Ctrl.Controls)
            {
                if (item is TextEdit)
                    if (((TextEdit)item).Enabled)
                        ((TextEdit)item).EditValue = null;

                if (item is RadioGroup)
                    if (((RadioGroup)item).Enabled)
                        ((RadioGroup)item).EditValue = 0;

                if (item is DateEdit)
                    if (((DateEdit)item).Enabled)
                        ((DateEdit)item).EditValue = 0;

                if (item is LookUpEdit)
                    if (((LookUpEdit)item).Enabled)
                        ((LookUpEdit)item).EditValue = 0;

                if (item.Controls.Count > 0)
                    Temizle(item);
            }

            //Rez_Id = 0;
            CardF_ID = 0;
            rdb_RIH.SelectedIndex = 0;
            lookCardF_FolioID.EditValue = null;
            KartID = 0;
            txtCardF_Odano.Text = Param_ExtraFolio.Front_KartF_Odano;

            txtCardF_Adi.Text = "";
            txtCardF_Soyad.Text = "";
            xRez_Id = 0;
            lookCardF_FolioID.Properties.DataSource = null;
            txtCardF_Kartno.Focus();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            ParamKaydet();
            MessageBox.Show(res_man.GetString("Kayıt Edildi.."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ParamKaydet()
        {
            int durum = hizmetBedeliAktif.Checked == true ? 1 : 0;

            DataTable dt = dbtools.SelectTable("Select * From Pos_FolioParam Where Front_Departman = '" + Departman.Dep_Kodu + "'");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd(@"INSERT INTO dbo.Pos_FolioParam([Front_Acenta]
                                  ,[Front_DovizSekli]
                                  ,[Front_OdemeSekli]
                                  ,[Front_KurKodu]
                                  ,[Front_Konaklama]
                                  ,[Front_MusteriTipi]
                                  ,[Front_Departman]
                                  ,[Front_Iade]
                                 ,[Front_KartF_Onburo]
                                ,[Front_KartF_Odano]
                                ,Front_Kapatma
                                ,Front_IadeKK
                                ,Front_GelirIade,HizmetReceteKod,hizmetOdemeKod,hizmetBedeliAktif,HizmetReceteKodCocuk) 

VALUES('" + look_Acenta.EditValue + "','" + look_DovizSekli.EditValue + "','" + look_OdemeSekli.EditValue + "','" + look_KurKodu.EditValue + "','"
    + look_Konaklama.EditValue + "','" + look_MusteriTipi + "','" + Departman.Dep_Kodu + "','" + look_IadeNkt.EditValue + "','" + rdb_Kart_Onburo.EditValue + "','" + txtParam_OdaNo.Text + "','"
    + look_Kapatma.EditValue + "','" + look_IadeKK.EditValue + "','" + look_GelirIade.EditValue + "','" + HizmetReceteKod.EditValue + "','" + hizmetOdemeKod.EditValue + "','" + durum + "','" + HizmetReceteKodCocuk.EditValue + "')");


            }
            else
            {
                dbtools.execcmd(@"UPDATE [dbo].[Pos_FolioParam]
                                   SET [Front_Acenta] = '" + look_Acenta.EditValue + @"'
                                      ,[Front_DovizSekli] = '" + look_DovizSekli.EditValue + @"'
                                      ,[Front_OdemeSekli] = '" + look_OdemeSekli.EditValue + @"'
                                      ,[Front_KurKodu] = '" + look_KurKodu.EditValue + @"'
                                      ,[Front_Konaklama] = '" + look_Konaklama.EditValue + @"'
                                      ,[Front_MusteriTipi] = '" + look_MusteriTipi.EditValue + @"'
                                      ,[Front_Departman] = '" + Departman.Dep_Kodu + @"'
                                        ,[Front_Iade] = '" + look_IadeNkt.EditValue + @"'
                                        ,[Front_KartF_Onburo] = '" + rdb_Kart_Onburo.EditValue + @"'
                                        ,[Front_KartF_Odano] = '" + txtParam_OdaNo.EditValue + @"'
                                        ,[Front_Kapatma] = '" + look_Kapatma.EditValue + @"'
                                        ,[Front_IadeKK] = '" + look_IadeKK.EditValue + @"'
                                        ,[Front_GelirIade] = '" + look_GelirIade.EditValue + @"'
                                        ,[HizmetReceteKod] = '" + HizmetReceteKod.EditValue + @"'
                                        ,[hizmetOdemeKod] = '" + hizmetOdemeKod.EditValue + @"'
                                        ,[hizmetBedeliAktif] = '" + durum + @"'
                                        ,[HizmetReceteKodCocuk] = '" + HizmetReceteKodCocuk.EditValue + @"'
                                 WHERE Front_Departman = '" + Departman.Dep_Kodu + "'");
            }

            Param_ExtraFolio.Param_ExtraFolioYukle();
        }

        private void Bakiye(string FolioID, int Kart_ID)
        {
            decimal Borc = 0, Alacak = 0;

            string query = "exec StpKumhrk_Bul @xKumhrk_Tarih = '" + dateEdit4.DateTime.Date + "',@xKumhrk_Sirket='" + Fronttools.Sirket_Kodu + "',@xKumhrk_Posting_kodu=N'P',@xKumhrk_Re=N'E',@xKumhrk_Rez_id=N'" + FolioID + "',@xtip=55, @Kumhrk_Kart_id = '" + Kart_ID + "'";
            DataTable dt = Fronttools.SelectTable(query);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Param.Calisma_Sekli == 0)
                    {
                        if (Convert.ToString(dt.Rows[i]["Kumhrk_Ba"]) == "B")
                        {
                            Borc += Convert.ToDecimal(dt.Rows[i]["Tl_Tutar"]);
                        }
                        else
                        {
                            Alacak += Convert.ToDecimal(dt.Rows[i]["Tl_Tutar"]);
                        }
                    }
                    else
                    {
                        if (Convert.ToString(dt.Rows[i]["Kumhrk_Ba"]) == "B")
                        {
                            Borc += Convert.ToDecimal(dt.Rows[i]["Dv_Tutar"]);
                        }
                        else
                        {
                            Alacak += Convert.ToDecimal(dt.Rows[i]["Dv_Tutar"]);
                        }
                    }

                }
                txt_BakiyeBorc.Text = Borc.ToString("N2");
                txt_BakiyeAlacak.Text = Alacak.ToString("N2");
                txt_BakiyeBakiye.Text = (Convert.ToDecimal(Borc) - Convert.ToDecimal(Alacak)).ToString("N2");
            }
        }

        private void FolioListele(string FolioID, DateTime tarih1, DateTime tarih2, int Kart_ID)
        {
            string query = "exec StpKumhrk_Bul @xKumhrk_Tarih='" + Param.Tarih.Date + "',@xKumhrk_Sirket='" + Fronttools.Sirket_Kodu + "',@xKumhrk_Posting_kodu=N'P',@xKumhrk_Re=N'E',@xKumhrk_Rez_id=N'" + FolioID + "',@xtip=44,@Kumhrk_Fatura_bolum=0,@Kumhrk_KartTarih1 = '" + tarih1.Date + "', @Kumhrk_KartTarih2 = '" + tarih2.Date + "', @Kumhrk_Kart_id = '" + Kart_ID + "'";
            DataTable dt = Fronttools.SelectTable(query);
            gridControl2.DataSource = dt;

        }

        private void BakiyeYukle(int folio)
        {

            decimal tutar = 0, doviztutar = 0;
            decimal kur = Param.Doviz_Kuru;
            string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";
            decimal Parite = 0;

            if (Param.Tesis_Tipi == 0)   //Otel 
            {
                if (Param.Calisma_Sekli == 1)   //Dövizli
                {
                    if (Param.Doviz_Cinsi == 2) //Müşteri Giriş Günü Kuru
                    {
                        int Master_folio = Convert.ToInt32(Fronttools.DegerGetir("select top 1 isnull(Rez_Master_id,Rez_Id) from Rez WITH(NOLOCK) where Rez_Id = '" + folio.ToString() + "' "));
                        DateTime Giris_tarihi = Convert.ToDateTime(Fronttools.DegerGetir("select top 1 Rez_Giris_tarihi from Rez WITH(NOLOCK) where Rez_Id = '" + Master_folio.ToString() + "' "));
                        kur = Convert.ToDecimal(Fronttools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and  Convert(date,Kurlar_Tarih,105) = '" + Giris_tarihi.Date.ToString("yyyy-MM-dd") + "'"));
                        Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Giris_tarihi.Date.ToString("yyyy-MM-dd") + "'"));
                        //tutar = doviztutar * kur;
                    }
                    else
                    {
                        if (Param.Kurlar_Nerden == 0) // otel
                        {
                            kur = Convert.ToDecimal(Fronttools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                            Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                        }
                        else
                        {
                            kur = Convert.ToDecimal(dbtools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                            Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                        }
                        //tutar = doviztutar * kur;
                    }
                }
                else
                {
                    if (Param.Kurlar_Nerden == 0) // otel
                    {
                        kur = Convert.ToDecimal(Fronttools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                        Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));

                    }
                    else
                    {
                        kur = Convert.ToDecimal(dbtools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                        Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                    }
                    //tutar = doviztutar * kur;
                }
            }
            else
            {
                string dovizXml = dbtools.DegerGetir("select Mkodlar_Xml from Muh_Kodlar where Mkodlar_Sinif = '02' and Mkodlar_Kod = '" + look_DovizKod.EditValue + "'");
                //if (!(dovizXml == "" || dovizXml == "TL"))
                //{
                kur = Convert.ToDecimal(dbtools.DegerGetir("select isnull((select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'),1)"));
                Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                //tutar = doviztutar * kur;
                //}
            }



            this.Cursor = Cursors.WaitCursor;

            SqlConnection con = Fronttools.conn;
            if (con.State == ConnectionState.Closed) { con.Open(); }

            try
            {
                using (SqlCommand cmd = new SqlCommand("StpKumhrk_Kaydet", Fronttools.conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddWithValue("@Kumhrk_Tarih", Param.Tarih);
                    cmd.Parameters.AddWithValue("@Kumhrk_Dep_kodu", look_OdemeDep.EditValue);
                    cmd.Parameters.AddWithValue("@Kumhrk_Rez_id", txt_BakiyeFolioID.Text);
                    cmd.Parameters.AddWithValue("@Kumhrk_Oda", txt_BakiyeOda.Text);
                    cmd.Parameters.AddWithValue("@Kumhrk_Doviz_kodu", look_DovizKod.EditValue);
                    cmd.Parameters.AddWithValue("@Kumhrk_Kur", kur);
                    cmd.Parameters.AddWithValue("@Kumhrk_Tutar", Convert.ToDecimal(txt_BakiyeLimit.EditValue) * kur);
                    cmd.Parameters.AddWithValue("@Kumhrk_Doviz_tutar", Convert.ToDecimal(txt_BakiyeLimit.EditValue));
                    cmd.Parameters.AddWithValue("@Kumhrk_Def_doviz", Convert.ToDecimal(txt_BakiyeLimit.EditValue) * Parite);
                    cmd.Parameters.AddWithValue("@Kumhrk_Tipi", "S");
                    cmd.Parameters.AddWithValue("@Kumhrk_Bfd", "D");
                    cmd.Parameters.AddWithValue("@Kumhrk_Ba", "A");
                    cmd.Parameters.AddWithValue("@Kumhrk_Re", "E");
                    cmd.Parameters.AddWithValue("@Kumhrk_Me", "M");
                    cmd.Parameters.AddWithValue("@Kumhrk_Cekno", "");
                    cmd.Parameters.AddWithValue("@Kumhrk_Posting_kodu", "B");
                    cmd.Parameters.AddWithValue("@Kumhrk_Zaman", DateTime.Now.ToShortTimeString());
                    cmd.Parameters.AddWithValue("@Kumhrk_Yil_kodu", Param.Tarih.Year);
                    cmd.Parameters.AddWithValue("@Kumhrk_Sirket", Fronttools.Sirket_Kodu);
                    cmd.Parameters.AddWithValue("@Kumhrk_Otel", Fronttools.Otel_Kodu);
                    cmd.Parameters.AddWithValue("@Kumhrk_Kulanici_id", User.ID_Getir(User.P_Kod));
                    cmd.Parameters.AddWithValue("@Kumhrk_Kulanici_kodu", User.P_Kod);
                    cmd.Parameters.AddWithValue("@Kumhrk_Fatura_bolum", "");
                    cmd.Parameters.AddWithValue("@Kumhrk_Gunluk_aylik", "");
                    cmd.Parameters.AddWithValue("@Kumhrk_Kartno", txt_BakiyeKartNo.Text);
                    cmd.Parameters.AddWithValue("@Kumhrk_Islem_tarihi", Convert.ToDateTime(dbtools.DegerGetir("select getdate()")));
                    cmd.Parameters.AddWithValue("@Kumhrk_Sistem_tarihi", Convert.ToDateTime(dbtools.DegerGetir("select getdate()")));
                    cmd.Parameters.AddWithValue("@Kumhrk_Kart_id", KartID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                }
            }
            catch (Exception)
            {
                throw;
            }

            this.Cursor = Cursors.Default;
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kart, Log.Log_Islem.Kaydet, txt_BakiyeKartNo.Text + " Nolu Karta " + txt_BakiyeLimit.EditValue + " TL Bakiye Yüklenmiştir.", "", Convert.ToString(lookCardF_FolioID.EditValue));
        }

        public void otomatikReceteSatis()
        {
            try
            {
                if (hizmetBedeliAktif.Checked)
                {
                    Satis satis = new Satis();
                    satis.Visible = false;
                    satis.otomatikSatis = true;
                    satis.kartnom = txtCardF_Kartno.Text;
                    satis.recetekod = Param_ExtraFolio.HizmetReceteKod;
                    satis.recetekodCocuk = Param_ExtraFolio.HizmetReceteKodCocuk;
                    satis.hizmetOdemeKod = Param_ExtraFolio.hizmetOdemeKod;
                    satis.hizmetmiktar = Convert.ToInt32(txtCardF_Kisi.Text);
                    satis.hizmetmiktarCocuk = Convert.ToInt32(CardF_Cocuk.Text);
                    satis.ShowDialog();
                }
            }
            catch (Exception ex)
            {

                RHMesaj.MyMessageError(MyClass, "otomatikReceteSatis", "", ex);
            }

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {


            if (Convert.ToString(look_OdemeDep.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Bakiye Yüklemek için Ödeme Departmanını Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK);
                return;
            }

            if (txt_BakiyeKartNo.Text == "" || txt_BakiyeFolioID.Text == "" || txt_BakiyeOda.Text == "")
            {
                MessageBox.Show(res_man.GetString("Kart Okutunuz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK);
                return;
            }

            KartNoSor();

            if (Convert.ToDecimal(txt_BakiyeLimit.Text) > 0)
            {
                BakiyeYukle(Convert.ToInt32(txt_BakiyeFolioID.Text));
                Bakiye(txt_BakiyeFolioID.Text, KartID);
                FolioListele(txt_BakiyeFolioID.Text, dateEdit4.DateTime.Date, dateEdit1.DateTime.Date, KartID);

                for (int i = 0; i < 2; i++)
                {
                    FisPr pr = new FisPr();
                    pr.ExtraFolioPr(txt_BakiyeKartNo.Text, Convert.ToInt32(txt_BakiyeFolioID.Text), "BİLGİ FİŞİ", Convert.ToDecimal(txt_BakiyeLimit.EditValue), "Yüklenen", chk_Altbilgi.Checked, (string)look_DovizKod.EditValue);
                }


                MessageBox.Show(res_man.GetString("Bakiye Yüklenmiştir..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(res_man.GetString("Yüklenecek Limit Tutarını Giriniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK);
                return;
            }



            this.Close();

        }

        private void txt_BakiyeKartNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string Kart = txt_BakiyeKartNo.Text.Replace(" ", "");
                txt_BakiyeKartNo.Text = Kart;
                txt_BakiyeKartNo.Focus();
                KartNoSor2();
            }
        }

        private void IadeBakiye(string KapatmaKodu, string folio)
        {
            decimal tutar = 0, doviztutar = 0;
            decimal kur = Param.Doviz_Kuru;
            string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";
            decimal Parite = 0;

            if (Param.Tesis_Tipi == 0)   //Otel 
            {
                if (Param.Calisma_Sekli == 1)   //Dövizli
                {
                    if (Param.Doviz_Cinsi == 2) //Müşteri Giriş Günü Kuru
                    {
                        int Master_folio = Convert.ToInt32(Fronttools.DegerGetir("select top 1 isnull(Rez_Master_id,Rez_Id) from Rez WITH(NOLOCK) where Rez_Id = '" + folio.ToString() + "' "));
                        DateTime Giris_tarihi = Convert.ToDateTime(Fronttools.DegerGetir("select top 1 Rez_Giris_tarihi from Rez WITH(NOLOCK) where Rez_Id = '" + Master_folio.ToString() + "' "));
                        kur = Convert.ToDecimal(Fronttools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and  Convert(date,Kurlar_Tarih,105) = '" + Giris_tarihi.Date.ToString("yyyy-MM-dd") + "'"));
                        Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Giris_tarihi.Date.ToString("yyyy-MM-dd") + "'"));
                        //tutar = doviztutar * kur;
                    }
                    else
                    {
                        if (Param.Kurlar_Nerden == 0) // otel
                        {
                            kur = Convert.ToDecimal(Fronttools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                            Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                        }
                        else
                        {
                            kur = Convert.ToDecimal(dbtools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                            Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                        }
                        //tutar = doviztutar * kur; 
                    }
                }
                else
                {
                    if (Param.Kurlar_Nerden == 0) // otel
                    {
                        kur = Convert.ToDecimal(Fronttools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                        Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));

                    }
                    else
                    {
                        kur = Convert.ToDecimal(dbtools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                        Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                    }
                    //tutar = doviztutar * kur;
                }
            }
            if (Param.Tesis_Tipi == 1)
            {
                string dovizXml = dbtools.DegerGetir("select Mkodlar_Xml from Muh_Kodlar where Mkodlar_Sinif = '02' and Mkodlar_Kod = '" + look_DovizKod.EditValue + "'");
                //if (!(dovizXml == "" || dovizXml == "TL"))
                //{
                kur = Convert.ToDecimal(dbtools.DegerGetir("select isnull((select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + look_DovizKod.EditValue + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'),1)"));
                Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                //tutar = doviztutar * kur;
                //}
            }



            //decimal Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = 'M' and Kurlar_Tarih  = '" + Param.Tarih + "'"));

            this.Cursor = Cursors.WaitCursor;

            SqlConnection con = Fronttools.conn;
            if (con.State == ConnectionState.Closed) { con.Open(); }

            try
            {
                using (SqlCommand cmd = new SqlCommand("StpKumhrk_Kaydet", Fronttools.conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddWithValue("@Kumhrk_Tarih", Param.Tarih);
                    cmd.Parameters.AddWithValue("@Kumhrk_Dep_kodu", KapatmaKodu);
                    cmd.Parameters.AddWithValue("@Kumhrk_Rez_id", txt_BakiyeFolioID.Text);
                    cmd.Parameters.AddWithValue("@Kumhrk_Oda", txt_BakiyeOda.Text);
                    cmd.Parameters.AddWithValue("@Kumhrk_Doviz_kodu", look_DovizKod.EditValue);
                    cmd.Parameters.AddWithValue("@Kumhrk_Kur", kur);
                    cmd.Parameters.AddWithValue("@Kumhrk_Tutar", Math.Abs(Convert.ToDecimal(txt_BakiyeBakiye.EditValue) * kur));
                    cmd.Parameters.AddWithValue("@Kumhrk_Doviz_tutar", Math.Abs(Convert.ToDecimal(txt_BakiyeBakiye.EditValue)));
                    cmd.Parameters.AddWithValue("@Kumhrk_Def_doviz", Math.Abs(Convert.ToDecimal(txt_BakiyeBakiye.EditValue) * Parite));
                    cmd.Parameters.AddWithValue("@Kumhrk_Tipi", "S");
                    cmd.Parameters.AddWithValue("@Kumhrk_Bfd", "D");
                    cmd.Parameters.AddWithValue("@Kumhrk_Ba", "B");
                    cmd.Parameters.AddWithValue("@Kumhrk_Re", "E");
                    cmd.Parameters.AddWithValue("@Kumhrk_Me", "M");
                    cmd.Parameters.AddWithValue("@Kumhrk_Cekno", "");
                    cmd.Parameters.AddWithValue("@Kumhrk_Zaman", DateTime.Now.ToShortTimeString());
                    cmd.Parameters.AddWithValue("@Kumhrk_Yil_kodu", Param.Tarih.Year);
                    cmd.Parameters.AddWithValue("@Kumhrk_Sirket", Fronttools.Sirket_Kodu);
                    cmd.Parameters.AddWithValue("@Kumhrk_Otel", Fronttools.Otel_Kodu);
                    cmd.Parameters.AddWithValue("@Kumhrk_Kulanici_id", User.ID_Getir(User.P_Kod));
                    cmd.Parameters.AddWithValue("@Kumhrk_Kulanici_kodu", User.P_Kod);
                    cmd.Parameters.AddWithValue("@Kumhrk_Fatura_bolum", "");
                    cmd.Parameters.AddWithValue("@Kumhrk_Gunluk_aylik", "");
                    cmd.Parameters.AddWithValue("@Kumhrk_Kartno", txt_BakiyeKartNo.Text);
                    cmd.Parameters.AddWithValue("@Kumhrk_Islem_tarihi", Convert.ToDateTime(dbtools.DegerGetir("select getdate()")));
                    cmd.Parameters.AddWithValue("@Kumhrk_Sistem_tarihi", Convert.ToDateTime(dbtools.DegerGetir("select getdate()")));
                    cmd.Parameters.AddWithValue("@Kumhrk_Kart_id", KartID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);


                }
            }
            catch (Exception)
            {
                throw;
            }

            this.Cursor = Cursors.Default;
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kart, Log.Log_Islem.Kaydet, Convert.ToString(txtCardF_Kartno.Text) + " Nolu Kart Iade Görmüştür.", "", Convert.ToString(lookCardF_FolioID.EditValue));
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {

            int KisiSayisi = 0;
            decimal IadeBakiyeKo = 0;
            if (Param.Param_iadeKontrol == false)
            {
                if (txt_BakiyeFolioID.Text != "" && Math.Abs(Convert.ToDecimal(txt_BakiyeBakiye.EditValue)) > 0)
                {
                    IadeBakiye(Param_ExtraFolio.Front_Iade, txt_BakiyeFolioID.Text);
                    for (int i = 0; i < 2; i++)
                    {
                        FisPr pr = new FisPr();
                        pr.ExtraFolioPr(txt_BakiyeKartNo.Text, Convert.ToInt32(txt_BakiyeFolioID.Text), "BİLGİ FİŞİ", Convert.ToDecimal(txt_BakiyeBakiye.EditValue), "Iade", false, (string)look_DovizKod.EditValue);
                    }
                    Bakiye(txt_BakiyeFolioID.Text, KartID);
                    FolioListele(txt_BakiyeFolioID.Text, dateEdit4.DateTime.Date, dateEdit1.DateTime.Date, KartID);
                }
            }
            else
            {
                KisiSayisi = Convert.ToString(Fronttools.DegerGetir("Select ISNULL(CardF_Kisi,0) as CardF_Kisi From Kartf Where ID = '" + KartID + "'")) == "" ? 1 : Convert.ToInt32(Fronttools.DegerGetir("Select ISNULL(CardF_Kisi,0) as CardF_Kisi From Kartf Where ID = '" + KartID + "'"));
                IadeBakiyeKo = KisiSayisi * Param.Param_iadeLimit;

                decimal YuklenenKredi = Convert.ToDecimal(Fronttools.DegerGetir("Select SUM(ISNULL(Kumhrk_Tutar,0)) From Kumhrk Where Kumhrk_Kart_id = '" + KartID + "' and Kumhrk_Rez_id = '" + txt_BakiyeFolioID.Text + "' and Kumhrk_Ba = 'A'"));

                if (YuklenenKredi > IadeBakiyeKo)
                {
                    IadeBakiye(Param_ExtraFolio.Front_Iade, txt_BakiyeFolioID.Text);
                    for (int i = 0; i < 2; i++)
                    {
                        FisPr pr = new FisPr();
                        pr.ExtraFolioPr(txt_BakiyeKartNo.Text, Convert.ToInt32(txt_BakiyeFolioID.Text), "BİLGİ FİŞİ", Convert.ToDecimal(txt_BakiyeBakiye.EditValue), "Iade", false, (string)look_DovizKod.EditValue);
                    }
                    Bakiye(txt_BakiyeFolioID.Text, KartID);
                    FolioListele(txt_BakiyeFolioID.Text, dateEdit4.DateTime.Date, dateEdit1.DateTime.Date, KartID);
                }
                else
                {
                    MessageBox.Show(res_man.GetString("Kart limit Iadesi Yapılamaz.."), "Uyarı");
                    return;
                }
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            CardF_CheckOut();
            Temizle(tab_KartF);
            gridControl2.DataSource = null;
        }

        private void CardF_CheckOut()
        {
            try
            {
                if (txt_BakiyeFolioID.Text != "")
                {
                    //decimal Tutar = Convert.ToDecimal(Fronttools.DegerGetir("Select ISNULL( sum(Kumhrk_Tutar), 0)  as Tutar from Kumhrk  where Kumhrk_Rez_id = '" + txt_BakiyeFolioID.Text + "'"));

                    decimal Tutar = Convert.ToDecimal(Fronttools.BalanceBul(Convert.ToInt32(txt_BakiyeFolioID.Text), KartID.ToString(), KartID.ToString()));

                    DateTime Tarih1 = Convert.ToDateTime(Fronttools.DegerGetir("Select CardF_GirisTrh From KartF Where CardF_RezID = '" + txt_BakiyeFolioID.Text + "' and CardF_No = '" + txt_BakiyeKartNo.Text + "'"));
                    DateTime Tarih2 = Convert.ToDateTime(Fronttools.DegerGetir("Select CardF_CikisTrh From KartF Where CardF_RezID = '" + txt_BakiyeFolioID.Text + "' and CardF_No = '" + txt_BakiyeKartNo.Text + "'"));


                    if (Tutar > 0)
                    {
                        MessageBox.Show(res_man.GetString("Hareket Görmüş, Silinemez."), res_man.GetString("Uyarı"), MessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        DialogResult c = MessageBox.Show(txt_BakiyeFolioID.Text + " Folio Numarasını Check Out Yapmak İstediğinize Eminmisiniz ?", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo);
                        if (c == DialogResult.Yes)
                        {
                            //TopluGelirIade(true);
                            Fronttools.execcmd("Update KartF Set CardF_R_I_H = 'H' Where CardF_RezID = '" + txt_BakiyeFolioID.Text + "' and CardF_No = '" + txt_BakiyeKartNo.Text + "'");
                            for (int i = 0; i < 2; i++)
                            {
                                FisPr pr = new FisPr();
                                pr.ExtraFolioDokumPr(txt_BakiyeKartNo.Text, Convert.ToInt32(txt_BakiyeFolioID.Text), Tarih1, Tarih2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "CardF_CheckOut", "", ex);
            }

        }

        public static string MyClass = "Pos_ExtraFolio";

        int CardF_ID = 0;

        int xRez_Id = 0;

        int KayitTipi = 0;

        private void KartF_Kaydet(int ID)
        {

            try
            {
                string rezRIH = "I";
                this.Cursor = Cursors.WaitCursor;

                rezRIH = rdb_RIH.EditValue.ToString();

                if (rezRIH == "H" || rezRIH == "I")
                {

                }
                else
                {
                    rezRIH = "I";
                }

                if (Param.Param_ExtraFolioAcma == true)
                {
                    SqlConnection fcon = Fronttools.conn;
                    if (fcon.State == ConnectionState.Closed) { fcon.Open(); }






                    using (SqlCommand cmd = new SqlCommand("stpRez_Kaydet", Fronttools.conn) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@Kayit_Tip", KayitTipi);
                        cmd.Parameters.AddWithValue("@xRezim", xRez_Id);
                        cmd.Parameters.AddWithValue("@Rez_Odano", txtCardF_Odano.Text);
                        cmd.Parameters.AddWithValue("@Rez_Master_detay", "E");
                        cmd.Parameters.AddWithValue("@Rez_R_I_H", "I");
                        cmd.Parameters.AddWithValue("@Rez_Master_id", xRez_Id);
                        cmd.Parameters.AddWithValue("@Rez_Oda_sayisi", 0);
                        cmd.Parameters.AddWithValue("@Rez_Adi_1", txtCardF_Adi.Text);
                        cmd.Parameters.AddWithValue("@Rez_Adi_2", txtCardF_Soyad.Text);
                        cmd.Parameters.AddWithValue("@Rez_Macenta", Param_ExtraFolio.Front_Acenta);
                        cmd.Parameters.AddWithValue("@Rez_Otel_kodu", Fronttools.Otel_Kodu);
                        cmd.Parameters.AddWithValue("@Rez_Konaklama", Param_ExtraFolio.Front_Konaklama);
                        cmd.Parameters.AddWithValue("@Rez_Ulke", "TR");
                        cmd.Parameters.AddWithValue("@Rez_Kur_kodu", Param_ExtraFolio.Front_KurKodu);
                        cmd.Parameters.AddWithValue("@Rez_Odeme", Param_ExtraFolio.Front_OdemeSekli);
                        cmd.Parameters.AddWithValue("@Rez_Mus_tipi", Param_ExtraFolio.Front_MusteriTipi);
                        cmd.Parameters.AddWithValue("@Rez_Giris_tarihi", txtCardF_GirisTrh.DateTime);
                        cmd.Parameters.AddWithValue("@Rez_Cikis_tarihi", txtCardF_CikisTrh.DateTime);
                        cmd.Parameters.AddWithValue("@Rez_Sistem_tarihi", Param.Tarih);
                        cmd.Parameters.AddWithValue("@Rez_Alis_tarihi", Param.Tarih);
                        cmd.Parameters.AddWithValue("@Rez_Fiat_om", "M");
                        cmd.Parameters.AddWithValue("@Rez_C_W", "C");
                        cmd.Parameters.AddWithValue("@Rez_Yil_kodu", Param.Tarih.Year);
                        cmd.Parameters.AddWithValue("@Rez_Sirket", Fronttools.Sirket_Kodu);
                        cmd.Parameters.AddWithValue("@Rez_Doviz_Sekli", Param_ExtraFolio.Front_DovizSekli);
                        cmd.Parameters.AddWithValue("@Rez_Tfolio", "H");
                        cmd.Parameters.AddWithValue("@Rez_Blokeli", 0);
                        cmd.Parameters.AddWithValue("@Rez_Kartno", txtCardF_Kartno.Text);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            xRez_Id = Convert.ToInt32(dt.Rows[0][0]);

                        }


                    }

                }


                SqlConnection con = Fronttools.conn;
                if (con.State == ConnectionState.Closed) { con.Open(); }


                using (SqlCommand cmd = new SqlCommand("KartF_Kaydet", Fronttools.conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@CardF_No", txtCardF_Kartno.Text);
                    cmd.Parameters.AddWithValue("@CardF_Odano", txtCardF_Odano.Text);
                    cmd.Parameters.AddWithValue("@CardF_RezID", Param.Param_ExtraFolioAcma == false ? lookCardF_FolioID.EditValue : xRez_Id);
                    cmd.Parameters.AddWithValue("@CardF_GirisTrh", txtCardF_GirisTrh.DateTime);
                    cmd.Parameters.AddWithValue("@CardF_CikisTrh", txtCardF_CikisTrh.DateTime);
                    cmd.Parameters.AddWithValue("@CardF_Ad", txtCardF_Adi.Text);
                    cmd.Parameters.AddWithValue("@CardF_Soyad", txtCardF_Soyad.Text);
                    cmd.Parameters.AddWithValue("@CardF_R_I_H", rezRIH);
                    cmd.Parameters.AddWithValue("@CardF_Limit_Uyari", CardF_Limit_Uyari.Text.Substring(0, 1));
                    cmd.Parameters.AddWithValue("@CardF_MusteriTipi", Convert.ToString(CardF_MusteriTipi.SelectedIndex) == "-1" ? "GB" : Convert.ToString(CardF_MusteriTipi.EditValue));
                    cmd.Parameters.AddWithValue("@CardF_Kullanici", User.P_Kod);
                    cmd.Parameters.AddWithValue("@CardF_Kisi", txtCardF_Kisi.EditValue);
                    cmd.Parameters.AddWithValue("@CardF_Cocuk", CardF_Cocuk.EditValue);
                    cmd.Parameters.AddWithValue("@CardF_Indirim", txtKartFIndirim.EditValue.ToString().Replace(",","."));


                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);


                    if (dt.Rows.Count > 0)
                    {
                        KartID = Convert.ToInt32(dt.Rows[0][0]);
                    }

                    //if (dt.Rows.Count > 0)
                    //{
                    //    Fronttools.execcmd("Update Rez Set Rez_Master_id = '" + dt.Rows[0][0] + "' Where Rez_Id = " + dt.Rows[0][0]);
                    //}
                }

               


                this.Cursor = Cursors.Default;
                if (ID == 0)
                {
                    otomatikReceteSatis();

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kart, Log.Log_Islem.Kaydet, Convert.ToString(txtCardF_Kartno.Text) + " Nolu Kart Kayıt Edilmiştir", "", Convert.ToString(lookCardF_FolioID.EditValue));
                }
                else
                {
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kart, Log.Log_Islem.Duzelt, Convert.ToString(txtCardF_Kartno.Text) + " Nolu Kart Düzeltilmiştir", "", Convert.ToString(lookCardF_FolioID.EditValue));
                }
            }
            catch
            {
                //throw;
            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            if (txtCardF_Kartno.Text == "")
            {
                MessageBox.Show("Lütfen Kart No Giriniz !");
                txtCardF_Kartno.Focus();
                return;
            }
            int Kart = Convert.ToInt32(Fronttools.DegerGetir("select count(*) From KartF Where CardF_No = '" + txtCardF_Kartno.Text + "' and CardF_R_I_H = 'I'"));
            if (Kart > 0)
            {
                MessageBox.Show(res_man.GetString("Kart Kayıtlıdır. Yeni Kart Deneyiniz.."));
                return;
            }

            int _RezID = Convert.ToInt32(Fronttools.DegerGetir("select count(*) From Rez Where Rez_Id = '" + lookCardF_FolioID.EditValue + "' and Rez_R_I_H <> 'I'"));
            if (_RezID > 0)
            {
                MessageBox.Show(res_man.GetString("Oda Check-Out Olmuştur.Kayıt İşlemi İptal Edilmiştir."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            KayitTipi = 0;
            KartF_Kaydet(0);
            CardF_Listele();

            txt_BakiyeKartNo.Text = txtCardF_Kartno.Text;
            string Karts = txt_BakiyeKartNo.Text.Replace(" ", "");
            txt_BakiyeKartNo.Text = Karts;
            xtraTabControl1.SelectedTabPage = tab_BakiyeYukle;
            KartNoSor();
            txt_BakiyeLimit.Focus();
            //Temizle(tab_KartF);
        }

        private void CardF_Listele()
        {
            gridControl3.DataSource = Fronttools.SelectTable(@"SELECT [ID]
                                          ,[CardF_No]
                                 ,case when CardF_MusteriTipi ='GB' then 
										  (select SUM(Kumhrk_Tutar) as Tahsilat from kumhrk where Kumhrk_Kartno=CardF_No  and kumhrk_ba='A' )
										  else '0'
										  end as Tahsilat
                                          ,[CardF_RezID]
                                          ,[CardF_Odano]
                                          ,[CardF_GirisTrh]
                                          ,[CardF_CikisTrh]
                                          ,[CardF_Ad]
                                          ,[CardF_Soyad]
                                          ,[CardF_R_I_H]
                                          ,[CardF_Indirim]
                                          ,ISNULL(CardF_Kisi,0) as CardF_Kisi
                                          ,ISNULL(CardF_Cocuk,0) as CardF_Cocuk
                                          ,[CardF_Limit_Uyari]
                                          ,CardF_MusteriTipi
                                            , case CardF_MusteriTipi when 'OM' then 'OTEL MİSAFİRİ' When 'GB' then 'GÜNÜ BİRLİK' When 'DM' Then 'DEVRE MÜLK MİSAFİRİ' WHEN 'CM' THEN 'COMP' else 'TANITILMAMIŞ' end as  Musteri
                                      FROM [dbo].[KartF] Where CardF_R_I_H in (SELECT fieldvalue FROM dbo.stringArray('" + rdb_RIH.EditValue.ToString() + "',','))");


        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            CardF_Sil();
            Temizle(tab_KartF);
            CardF_Listele();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            Temizle(tab_KartF);
            CardF_Listele();
        }

        public string kisiSayisi = "1";

        bool Rack = true;
        private void gridView3_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0)
            {
                return;
            }

            if (Gitme == false)
            {
                return;
            }

            KartID = 0;
            Rack = false;
            CardF_ID = Convert.ToInt32(gridView3.GetFocusedRowCellValue("ID"));
            KartID = Convert.ToInt32(gridView3.GetFocusedRowCellValue("ID"));
            txtCardF_Kartno.Text = Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_No"));

            if (Param.Param_ExtraFolioAcma == false)
            {
                lookCardF_FolioID.EditValue = Convert.ToInt32(gridView3.GetFocusedRowCellValue("CardF_RezID"));
            }
            else
            {
                xRez_Id = Convert.ToInt32(gridView3.GetFocusedRowCellValue("CardF_RezID"));
                lookCardF_FolioID.EditValue = xRez_Id;
            }

            txtCardF_Odano.Text = Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_Odano"));
            txtCardF_GirisTrh.EditValue = Convert.ToDateTime(gridView3.GetFocusedRowCellValue("CardF_GirisTrh")).Date;
            txtCardF_CikisTrh.EditValue = Convert.ToDateTime(gridView3.GetFocusedRowCellValue("CardF_CikisTrh")).Date;
            txtCardF_Adi.Text = Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_Ad"));
            txtCardF_Soyad.Text = Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_Soyad"));
            CardF_MusteriTipi.EditValue = Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_MusteriTipi"));
            txtKartFIndirim.EditValue = Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_Indirim"));
            txtCardF_Kisi.Text = Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_Kisi")) == "" ? "1" : Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_Kisi"));
            CardF_Cocuk.Text = Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_Cocuk")) == "" ? "1" : Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_Cocuk"));

            kisiSayisi = txtCardF_Kisi.Text;
            CardF_Limit_Uyari.SelectedIndex = (Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_Limit_Uyari")) == "E" ? 0 : 1);


            decimal Alacak = 0;

            DataTable dt = Fronttools.SelectTable("exec StpKumhrk_Bul @xKumhrk_Tarih='" + Param.Tarih.Date + "',@xKumhrk_Sirket='" + Fronttools.Sirket_Kodu + "',@xKumhrk_Posting_kodu=N'P',@xKumhrk_Re=N'E',@xKumhrk_Rez_id=N'" + lookCardF_FolioID.EditValue + "',@xtip=55, @Kumhrk_Kart_id = '" + CardF_ID + "'");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["Kumhrk_Ba"]) == "A")
                    {
                        Alacak += Convert.ToDecimal(dt.Rows[i]["Tl_Tutar"]);
                    }
                }
            }
            txtCardF_Tutar.EditValue = Alacak;

            if (CardF_ID > 0)
            {
                txtCardF_Odano.Enabled = false;
            }
            else
            {
                txtCardF_Odano.Enabled = true;
            }

            Rack = true;
        }

        private void CardF_Sil()
        {
            CardF_ID = Convert.ToInt32(gridView3.GetFocusedRowCellValue("ID"));


            int rezId = Convert.ToInt32(gridView3.GetFocusedRowCellValue("CardF_RezID"));
            string query = "Select Kumhrk_Cekno from Kumhrk  where Kumhrk_Kart_id = '" + CardF_ID + "' and Kumhrk_Rez_id = '" + rezId + "' and (Kumhrk_Cekno<>'')";

            //string query = "Select ISNULL(sum(Kumhrk_Tutar), 0) as Tutar from Kumhrk  where Kumhrk_Kart_id = '" + CardF_ID + "' and Kumhrk_Rez_id = '" + rezId + "'";
            string textTutar = Fronttools.DegerGetir(query);
            //decimal Tutar = Convert.ToDecimal(textTutar);


            if (textTutar != "")//Tutar > 0
            {
                MessageBox.Show("Hareket Görmüş, Silinemez. ", res_man.GetString("Uyarı"), MessageBoxButtons.OK);
                return;
            }
            else
            {
                DialogResult c = MessageBox.Show(lookCardF_FolioID.Text + " Folio Numarasını Silmek İstediğinize Eminmisiniz ?", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo);
                if (c == DialogResult.Yes)
                {
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kart, Log.Log_Islem.Sil, Convert.ToString(txtCardF_Kartno.Text) + " Nolu Kart Silinmiştir", "", Convert.ToString(lookCardF_FolioID.EditValue));
                    Fronttools.execcmd("update KartF Set CardF_R_I_H = 'C' Where ID = '" + CardF_ID + "'");
                    CardF_Listele();
                }
            }
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            KayitTipi = 1;
            KartF_Kaydet(CardF_ID);
            CardF_Listele();
            Temizle(tab_KartF);

        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            int KisiSayisi = 0;
            decimal IadeBakiyeKo = 0;
            if (Param.Param_iadeKontrol == false)
            {

                if (txt_BakiyeFolioID.Text != "" && Math.Abs(Convert.ToDecimal(txt_BakiyeBakiye.EditValue)) > 0)
                {

                    IadeBakiye(Param_ExtraFolio.Front_IadeKK, txt_BakiyeFolioID.Text);
                    for (int i = 0; i < 2; i++)
                    {
                        FisPr pr = new FisPr();
                        pr.ExtraFolioPr(txt_BakiyeKartNo.Text, Convert.ToInt32(txt_BakiyeFolioID.Text), "BİLGİ FİŞİ", Convert.ToDecimal(txt_BakiyeBakiye.EditValue), "Iade", false, (string)look_DovizKod.EditValue);
                    }
                    Bakiye(txt_BakiyeFolioID.Text, KartID);
                    FolioListele(txt_BakiyeFolioID.Text, dateEdit4.DateTime.Date, dateEdit1.DateTime.Date, KartID);
                }
            }
            else
            {
                KisiSayisi = Convert.ToString(Fronttools.DegerGetir("Select ISNULL(CardF_Kisi,0) as CardF_Kisi From Kartf Where ID = '" + KartID + "'")) == "" ? 1 : Convert.ToInt32(Fronttools.DegerGetir("Select ISNULL(CardF_Kisi,0) as CardF_Kisi From Kartf Where ID = '" + KartID + "'"));
                IadeBakiyeKo = KisiSayisi * Param.Param_iadeLimit;

                decimal YuklenenKredi = Convert.ToDecimal(Fronttools.DegerGetir("Select SUM(ISNULL(Kumhrk_Tutar,0)) From Kumhrk Where Kumhrk_Kart_id = '" + KartID + "' and Kumhrk_Rez_id = '" + txt_BakiyeFolioID.Text + "' and Kumhrk_Ba = 'A'"));

                if (YuklenenKredi > IadeBakiyeKo)
                {
                    IadeBakiye(Param_ExtraFolio.Front_IadeKK, txt_BakiyeFolioID.Text);
                    for (int i = 0; i < 2; i++)
                    {
                        FisPr pr = new FisPr();
                        pr.ExtraFolioPr(txt_BakiyeKartNo.Text, Convert.ToInt32(txt_BakiyeFolioID.Text), "BİLGİ FİŞİ", Convert.ToDecimal(txt_BakiyeBakiye.EditValue), "Iade", false, (string)look_DovizKod.EditValue);
                    }
                    Bakiye(txt_BakiyeFolioID.Text, KartID);
                    FolioListele(txt_BakiyeFolioID.Text, dateEdit4.DateTime.Date, dateEdit1.DateTime.Date, KartID);
                }
                else
                {
                    MessageBox.Show(res_man.GetString("Kart limit Iadesi Yapılamaz.."), "Uyarı");
                    return;
                }
            }
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            try
            {
                DetayOzetList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Yazdir(DevExpress.XtraGrid.Views.Grid.GridView gC)
        {
            DynamicReport ayk = new DynamicReport(txt_BakiyeKartNo.Text + " Nolu Kart ", "    " + Param.Tesis_Adi, gC);
            ayk.ShowPreviewDialog();
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            Yazdir((DevExpress.XtraGrid.Views.Grid.GridView)gridControl2.MainView);
        }

        private void OdaDurumuKont()
        {

            ///*Temizle*/(this);
            lookCardF_FolioID.EditValue = null;
            if (Convert.ToInt32(CardF_MusteriTipi.SelectedIndex) == 0)
            {
                //Temizle(this);
                txtCardF_Odano.Enabled = false;
                txtCardF_Odano.Text = Param_ExtraFolio.Front_KartF_Odano;
                if (Param.Param_ExtraFolioAcma == false)
                {
                    CardF_FolioBul(txtCardF_Odano.Text);
                }
                else
                {
                    CardF_FolioBul(xRez_Id.ToString());
                }
            }
            else if (Convert.ToInt32(CardF_MusteriTipi.SelectedIndex) == 3)
            {
                txtCardF_Odano.Text = "";
                txtCardF_Odano.Enabled = true;

                Pos_XtraFolio_Rack p = new Pos_XtraFolio_Rack();
                p.Filtre = " and Rez_Odeme = '" + Param.Fullcomp_Kodu + "'";
                p.ShowDialog();
                if (p.Durum == false)
                {
                    return;
                }
                else
                {
                    CardF_FolioBul(p.txt_Odano.Text);
                }
            }
            else if (Convert.ToInt32(CardF_MusteriTipi.SelectedIndex) == 2)
            {
                txtCardF_Odano.Text = "";
                txtCardF_Odano.Enabled = true;

                Pos_XtraFolio_Rack p = new Pos_XtraFolio_Rack();
                p.Filtre = " and Rez_Odeme <> '" + Param.Fullcomp_Kodu + "'";
                p.ShowDialog();
                if (p.Durum == false)
                {
                    return;
                }
                else
                {
                    CardF_FolioBul(p.txt_Odano.Text);
                }

            }
            else
            {
                //Temizle(this);
                txtCardF_Odano.Enabled = false;
                txtCardF_Odano.Text = Param_ExtraFolio.Front_KartF_Odano;
                if (Param.Param_ExtraFolioAcma == false)
                {
                    CardF_FolioBul(txtCardF_Odano.Text);
                }
                else
                {
                    CardF_FolioBul(xRez_Id.ToString());
                }
            }

        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            if (CardF_ID <= 0)
            {
                MessageBox.Show(res_man.GetString("Kişi Bilgisi Bulunamadı.."));
                return;
            }

            Pos_XtraFolio_OdaTRF p = new Pos_XtraFolio_OdaTRF();
            p.KartFID = CardF_ID;
            p.ShowDialog();

            CardF_Listele();

        }

        private void txtCardF_Odano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CardF_FolioBul(txtCardF_Odano.Text);

                odaKontrol();
            }
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            NFC_Listele();
        }

        private void txt_NfcKart_Enter(object sender, EventArgs e)
        {
            string Kart = txt_NfcKart.Text.Replace(" ", "").Replace(";", "").Replace(":", "").Replace("9000", "");
            txt_NfcKart.Text = Kart;
            NFC_Listele();
        }

        private void Detay()
        {
            if (gridView11.FocusedRowHandle < 0)
            {
                return;
            }

            Detay detay = new Detay();

            detay.spn_Fisno.EditValue = Convert.ToInt32(gridView11.GetFocusedRowCellValue("Rsat_Fisno"));

            detay.ShowDialog();
        }
        private void simpleButton18_Click(object sender, EventArgs e)
        {
            Detay();
        }

        private void txtCardF_Kartno_EditValueChanged(object sender, EventArgs e)
        {
            string Kart = txtCardF_Kartno.Text.Replace(" ", "").Replace(";", "").Replace(":", "").Replace("9000", "");
            txtCardF_Kartno.Text = Kart;
            //txtCardF_Kartno.Focus();
        }

        private void txt_BakiyeKartNo_EditValueChanged(object sender, EventArgs e)
        {
            string Kart = txt_BakiyeKartNo.Text.Replace(" ", "").Replace(";", "").Replace(":", "").Replace("9000", "");
            txt_BakiyeKartNo.Text = Kart;
        }

        private void txt_NfcKart_EditValueChanged(object sender, EventArgs e)
        {
            string Kart = txt_NfcKart.Text.Replace(" ", "").Replace(";", "").Replace(":", "").Replace("9000", "");
            txt_NfcKart.Text = Kart;
        }

        bool Gitme = true;
        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0)
            {
                return;
            }

            KartID = Convert.ToInt32(gridView3.GetFocusedRowCellValue("ID"));

            chk_Folio.Checked = false;
            chk_Bakiye.Checked = true;
            chk_Param.Checked = false;
            chk_Rapor.Checked = false;
            xtraTabControl1.SelectedTabPage = tab_BakiyeYukle;
            txt_BakiyeAlacak.Text = "";
            txt_BakiyeBorc.Text = "";
            txt_BakiyeBakiye.Text = "";
            Gitme = false;

            //CheckButton chkbtn = (CheckButton)sender;

            //if (chk_Durum == null)
            //{
            //    chk_Durum = chkbtn;
            //}


            //chk_Durum = null;

            txt_BakiyeKartNo.Text = txtCardF_Kartno.Text;
            string Kart = txt_BakiyeKartNo.Text.Replace(" ", "");
            txt_BakiyeKartNo.Text = Kart;

            //KartID = Convert.ToInt32(gridView3.GetFocusedRowCellValue("ID"));
            KartNoSor();
            //KartNoSor();

            txt_BakiyeLimit.Focus();


        }

        private void lookCardF_FolioID_EditValueChanged(object sender, EventArgs e)
        {
            CardF_FolioBul(Convert.ToString(lookCardF_FolioID.EditValue));
        }

        private void txt_NfcKart_TextChanged(object sender, EventArgs e)
        {
            string Kart = txt_NfcKart.Text.Replace(" ", "").Replace(";", "").Replace(":", "").Replace("9000", "");
            txt_NfcKart.Text = Kart;
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            DialogResult c = MessageBox.Show(dateEdit2.DateTime.ToString("dd.MM.yyyy") + " Tarihindeki Toplu Gelir İade Kapatma Uygulansın mı ?", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (c == DialogResult.Yes)
            {
                TopluGelirIade(false);
            }
        }

        private void TopluGelirIade(bool Kart)
        {
            this.Cursor = Cursors.WaitCursor;

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) { con.Open(); }

            try
            {
                using (SqlCommand cmd = new SqlCommand("Pos_NFCBakiye_Onburo", dbtools.conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddWithValue("@SirketKod", Departman.MKodlar_P_SirketKodu);
                    cmd.Parameters.AddWithValue("@Tarih", dateEdit2.DateTime.Date);
                    cmd.Parameters.AddWithValue("@KapatmaKodu", Param_ExtraFolio.Front_GelirIade);
                    cmd.Parameters.AddWithValue("@Rsat_Garson", User.P_Kod);
                    cmd.Parameters.AddWithValue("@Departman", Departman.Dep_Kodu);
                    cmd.Parameters.AddWithValue("@PariteDovizKodu", Param_ExtraFolio.Front_KurKodu);
                    if (Kart == true) cmd.Parameters.AddWithValue("@CardID", KartID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                }



            }
            catch (Exception x)
            {
                //MessageBox.Show(res_man.GetString("Hata Mesajı..") + "\n" + x.Message);

            }
            finally
            {
                MessageBox.Show(res_man.GetString("İşlem Tamamlandı."));

                this.Cursor = Cursors.Default;

            }

        }

        private void departmanKoduDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0)
            {
                return;
            }

            if (Convert.ToString(gridView2.GetFocusedRowCellValue("Kumhrk_Ba")) == "A")
            {
                Pos_XtraFolio_OdemeTipi p = new Pos_XtraFolio_OdemeTipi();
                p.dt = (DataTable)look_OdemeDep.Properties.DataSource;
                p.Kumhrk_Id = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Kumhrk_Id"));
                p.ShowDialog();

                FolioListele(txt_BakiyeFolioID.Text, dateEdit4.DateTime.Date, dateEdit1.DateTime.Date, KartID);
            }
            else
            {
                MessageBox.Show(res_man.GetString("Ödeme Kodu Satırını Seçiniz.."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            if (gridView11.FocusedRowHandle < 0)
            {
                return;
            }

            NFC_Detay detay = new NFC_Detay();

            detay.kartId = Convert.ToInt32(gridView11.GetFocusedRowCellValue("CardFID"));
            detay.spn_KartNo.Text = Convert.ToString(gridView11.GetFocusedRowCellValue("CardFNo"));

            detay.ShowDialog();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            if (txt_BakiyeKartNo.Text == "")
            {
                MessageBox.Show(res_man.GetString("Kart Okutunuz..."), "Uyarı");
                return;
            }


            Pos_ExtraFolio_HarcamaDetayi d = new Pos_ExtraFolio_HarcamaDetayi();
            d.FolioID = Convert.ToInt32(txt_BakiyeFolioID.Text);
            d.KartID = KartID;

            d.ShowDialog();
        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0)
            {
                return;
            }

            if (Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_No")) == "" || Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_RezID")) == "" || Convert.ToString(gridView3.GetFocusedRowCellValue("ID")) == "")
            {
                return;
            }

            Pos_ExtraFolio_BakiyeTRF d = new Pos_ExtraFolio_BakiyeTRF();
            d.KartID = Convert.ToInt32(gridView3.GetFocusedRowCellValue("ID"));
            d.FolioID = Convert.ToInt32(gridView3.GetFocusedRowCellValue("CardF_RezID"));
            d.KartNo = Convert.ToString(gridView3.GetFocusedRowCellValue("CardF_No"));
            d.ShowDialog();

            CardF_Listele();

        }

        private void simpleButton23_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(chartDate_1.EditValue) == "" || Convert.ToString(chartDate_2.EditValue) == "")
            {
                chartDate_1.DateTime = Param.Tarih;
                chartDate_2.DateTime = Param.Tarih;
            }

            DataTable dt = Fronttools.SelectTable(@"Declare 
                                        @Tarih1 date = '" + chartDate_1.DateTime.Date + @"',
                                        @Tarih2 date = '" + chartDate_2.DateTime.Date + @"'

                                        select
                                        CardF_No as KartNo,
                                        CardF_Kisi as KisiSayisi,
                                        CardF_Odano as Odano
                                        from
                                        KartF
                                        
                                        where CardF_MusteriTipi = 'GB' and
                                        CardF_GirisTrh between @Tarih1 and @Tarih2");



            if (dt.Rows.Count > 0)
            {
                chartControl1.DataSource = dt;
                chartControl1.SeriesTemplate.ArgumentDataMember = "KartNo";
                chartControl1.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "KisiSayisi" });
                chartControl1.SeriesDataMember = "Odano";
            }


            gridView1.Columns.Clear();
            DataTable dtKisi = Fronttools.SelectTable(@"

select
Kumhrk_Rez_id,
Kumhrk_Kart_id,
SUM(Kumhrk_tutar) as Tutar,
Kumhrk_Tarih as Tarih

into #temp
from kumhrk
where  Kumhrk_Ba = 'A'
group by  Kumhrk_Rez_id,Kumhrk_Kart_id,Kumhrk_Tarih



select
Tarih as TARIH,
SUM(CardF_Kisi) as KISI_SAYISI
, SUM(t.Tutar) as TUTAR
 from KartF 
left join #temp as t on t.Kumhrk_Kart_id = KartF.ID and Kumhrk_Rez_id = CardF_RezID
where Tarih >= '" + chartDate_1.DateTime + "' and Tarih <= '" + chartDate_2.DateTime + @"' and CardF_MusteriTipi='GB' and CardF_Odano = '" + Param_ExtraFolio.Front_KartF_Odano + @"' 
group by Tarih
order by Tarih ");

            gridControl1.DataSource = dtKisi;
        }

        private void radioGroup2_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(rdb_RIH.EditValue) != "I")
            {
                txtCardF_GirisTrh.Enabled = true;
                simpleButton2.Enabled = true;
            }
            else
            {
                txtCardF_GirisTrh.Enabled = false;
                txtCardF_GirisTrh.DateTime = Param.Tarih;
                txtCardF_CikisTrh.DateTime = Param.Tarih;
                simpleButton2.Enabled = false;

            }

            CardF_Listele();
        }



        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string Filtre = "";

            if (rdb_RIH.SelectedIndex == 1)
            {
                Filtre = " and CardF_GirisTrh >= '" + txtCardF_GirisTrh.DateTime.Date + "' and CardF_GirisTrh <=  '" + txtCardF_CikisTrh.DateTime.Date + "'";
            }
            if (rdb_RIH.SelectedIndex == 2)
            {
                Filtre = "";
            }

            gridControl3.DataSource = Fronttools.SelectTable(@"SELECT [ID]
                                          ,[CardF_No]
                                          ,[CardF_RezID]
                                          ,[CardF_Odano]
                                          ,[CardF_GirisTrh]
                                          ,[CardF_CikisTrh]
                                          ,[CardF_Ad]
                                          ,[CardF_Soyad]
                                          ,[CardF_R_I_H]
                                          ,[CardF_Indirim]
                                          ,ISNULL(CardF_Kisi,0) as CardF_Kisi
                                          ,ISNULL(CardF_Cocuk,0) as CardF_Cocuk
                                          ,[CardF_Limit_Uyari]
                                          ,CardF_MusteriTipi
                                            , case CardF_MusteriTipi when 'OM' then 'OTEL MİSAFİRİ' When 'GB' then 'GÜNÜ BİRLİK' When 'DM' Then 'DEVRE MÜLK MİSAFİRİ' else 'TANITILMAMIŞ' end as  Musteri
                                      FROM [dbo].[KartF] Where CardF_R_I_H in (SELECT fieldvalue FROM dbo.stringArray('" + rdb_RIH.EditValue.ToString() + @"',',')) " + Filtre + " ");
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Pos_XtraFolio_Rack p = new Pos_XtraFolio_Rack();
            p.ShowDialog();
        }

        private void CardF_MusteriTipi_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (CardF_MusteriTipi.SelectedIndex == 2 || CardF_MusteriTipi.SelectedIndex == 3)
            {
                CardF_Limit_Uyari.EditValue = "HAYIR";
                CardF_Limit_Uyari.Text = "HAYIR";
            }
            else
            {
                CardF_Limit_Uyari.EditValue = "EVET";
                CardF_Limit_Uyari.Text = "EVET";
            }

            if (Rack)
            {
                OdaDurumuKont();
            }

        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            Excel(gridControl3);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Excel(gridControl1);
        }

        private void simpleButton24_Click(object sender, EventArgs e)
        {
            Rapor_Print("Toplam Kişi Sayısı Listesi   ", gridControl1, chartDate_1.DateTime.Date, chartDate_2.DateTime.Date);
        }

        private void GelirTakib()
        {
            gridView8.Columns.Clear();
            gridControl6.DataSource = null;
            gridControl6.DataSource = Fronttools.SelectTable(@"select
                                        TARIH,
                                        MUSTERI_TIPI,
                                        KART_NO,
                                        ODA as ODA_NO,
                                        ISNULL(A, 0) as ALACAK,
                                        ISNULL(B, 0) as BORC,                                     
                                        (ISNULL(A, 0) - ISNULL(B, 0)) as BAKIYE
                                        from
                                        (
                                        select
                                        Kumhrk_Tarih as TARIH,
                                        KartF.CardF_No as KART_NO,
                                        Kumhrk_Ba as BA,
                                        SUM(Kumhrk_Tutar) as TUTAR,
                                        case CardF_MusteriTipi when 'GB' then 'GÜNÜ BİRLİK' end as MUSTERI_TIPI,
                                        CardF_Odano as ODA
                                        from KartF
                                        left join Kumhrk on Kumhrk_Kart_id = KartF.ID and Kumhrk_Rez_id = KartF.CardF_RezID
                                        where Kumhrk_Tarih = '" + date_GelirTakip.DateTime.Date + @"' and KartF.CardF_MusteriTipi = 'GB'
                                        group by
                                        Kumhrk_Tarih,
                                        Kumhrk_Ba, KartF.CardF_No,KartF.CardF_MusteriTipi,CardF_Odano
                                        ) tablom
                                        Pivot
                                        (
                                            SUM(TUTAR)

                                            FOR BA 
	                                        in

                                            ([B],[A])
                                        ) PivotTablom");





        }
        private void simpleButton25_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(date_GelirTakip.EditValue) == "")
            {
                date_GelirTakip.DateTime = Param.Tarih;
            }
            GelirTakib();
        }

        private void simpleButton26_Click(object sender, EventArgs e)
        {
            DialogResult c = MessageBox.Show(date_GelirTakip.DateTime.ToString("dd.MM.yyyy") + " Tarihindeki Günü Birlik Kartlara Check Out Uygulansın mı ?", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (c == DialogResult.Yes)
            {
                // CardF_GirisTrh idi gökhan bey ile CardF_CikisTrh yaptık
                Fronttools.execcmd("Update KartF Set CardF_R_I_H = 'H' Where CardF_CikisTrh = '" + date_GelirTakip.DateTime + "' and CardF_MusteriTipi = 'GB'");
                MessageBox.Show(res_man.GetString("Kartlara Check Out İşlemi Uygulandı."));
                return;
            }
        }

        private void simpleButton28_Click(object sender, EventArgs e)
        {

            string ass = dbtools.DegerGetir("select Front_Kapatma from Pos_FolioParam WHERE Front_Departman = '" + Departman.Dep_Kodu + "'");

            gridControl7.DataSource = null;
            gridView7.Columns.Clear();

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 31);
            com.Parameters.AddWithValue("@Tarih1", dateEdit8.DateTime.Date);
            com.Parameters.AddWithValue("@Tarih2", dateEdit7.DateTime.Date);
            com.Parameters.AddWithValue("@Departman", Departman.Dep_Kodu);
            com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
            //com.Parameters.AddWithValue("@KartFNo", txt_NfcKart.Text);
            com.Parameters.AddWithValue("@NFCOdemeKodlar", ass);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            gridControl7.DataSource = dt;

            gridviewSum(gridView9);
        }

        private void simpleButton26_Click_1(object sender, EventArgs e)
        {
            Rapor_Print("Kart Harcama Listesi", gridControl7, dateEdit8.DateTime.Date, dateEdit7.DateTime.Date);
        }

        private void simpleButton27_Click(object sender, EventArgs e)
        {
            Excel(gridControl7);
        }

        private void simpleButton29_Click(object sender, EventArgs e)
        {
            Onb_Fatura a = new Onb_Fatura();
            a.CardFID = KartID;
            a.ShowDialog();
        }

        private void txtCardF_Odano_EditValueChanged(object sender, EventArgs e)
        {

        }


        public void odaKontrol()
        {
            DataTable dt = Fronttools.SelectTable(@"select Rez_Id,Rez_Adi_1,Rez_Adi_2,Rez_Giris_tarihi,Rez_Cikis_tarihi,Rez_limit_uyari_eh from dbo.Rez where Rez_Odano = '" + txtCardF_Odano.Text + "' and Rez_R_I_H = 'I' and Rez_Master_detay <> 'D'");
            if (dt.Rows.Count > 0)
            {
                lookCardF_FolioID.Properties.DataSource = dt;
                lookCardF_FolioID.Properties.DisplayMember = "Rez_Adi_1";
                lookCardF_FolioID.Properties.ValueMember = "Rez_Id";
                lookCardF_FolioID.ItemIndex = 0;

                //txtCardF_Adi.Text = Convert.ToString(txtCardF_Adi.Text);
                //txtCardF_Soyad.Text = Convert.ToString(txtCardF_Soyad.Text);

                if (Convert.ToString(CardF_MusteriTipi.EditValue) != "GB")
                {
                    txtCardF_GirisTrh.EditValue = dt.Rows[0]["Rez_Giris_tarihi"];
                    txtCardF_CikisTrh.EditValue = dt.Rows[0]["Rez_Cikis_tarihi"];
                }
                else
                {
                    txtCardF_GirisTrh.EditValue = Param.Tarih;
                    txtCardF_CikisTrh.EditValue = Param.Tarih;
                }
                //rdbCardF_Uyari.EditValue = dt.Rows[0]["Rez_limit_uyari_eh"];
            }
            else
            {
                MessageBox.Show(res_man.GetString("Oda In-House Değildir."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            CardF_Listele();
        }
    }
}