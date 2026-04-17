using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using Pos.Class;
using System;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Fatura : DevExpress.XtraEditors.XtraForm
    {
        //public static string xAd = "";
        //public static string xAdres1 = "";
        //public static string xAdres2 = "";
        //public static string xAdres3 = "";
        //public static string xVergi_Daire = "";
        //public static string xVergiNo = "";

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());


        public string Odemekodu = String.Empty;
        public decimal TL_Tutar;
        public string Tip = String.Empty;

        public Fatura()
        {
            InitializeComponent();
        }

        private void Fatura_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            txt_Fatno.Text = DateTime.Now.ToString("yyyyMMddHHmmss");
            PFat_Senaryo.SelectedIndex = 0;
            PFat_Efatura.SelectedIndex = 2;
            PFat_Sk.SelectedIndex = 1;



            DataTable dt = dbtools.SelectTable("select * from rmosmuh..il where ulke_id = 1 order by ad");
            if (dt.Rows.Count > 0)
            {
                PFat_Sehir.Properties.DataSource = dt;
                PFat_Sehir.Properties.DisplayMember = "ad";
                PFat_Sehir.Properties.ValueMember = "id";
            }

            if (Tip != "F")
            {
                string aa = "Select CardF_Ad,CardF_Soyad,CardF_Fat_VKN,CardF_Fat_VD From KartF Where ID = '" + Convert.ToString(this.Tag) + "'";
                DataTable dtf = Fronttools.SelectTable(aa);
                if (dtf.Rows.Count > 0)
                {
                    PFat_Ad.Text = Convert.ToString(dtf.Rows[0]["CardF_Ad"]);
                    PFat_Soyad.Text = Convert.ToString(dtf.Rows[0]["CardF_Soyad"]);
                    txt_Vergino.Text = Convert.ToString(dtf.Rows[0]["CardF_Fat_VKN"]);
                    txt_Vergidaire.Text = Convert.ToString(dtf.Rows[0]["CardF_Fat_VD"]);

                }
            }

            gridyenile();


            
        }

        //public void buttonEkle(string columnName)
        //{
        //    try
        //    {
        //        GridView gridView = gridControl1.MainView as GridView;
        //        RepositoryItemButtonEdit repositoryButton = new RepositoryItemButtonEdit();
        //        repositoryButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
        //        repositoryButton.Buttons[0].Caption = columnName;
        //        repositoryButton.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
        //        repositoryButton.ButtonClick += RepositoryButton_ButtonClick;
        //        gridView.Columns[columnName].ColumnEdit = repositoryButton;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //private void RepositoryButton_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        //{
        //    GridView gridView = gridControl1.FocusedView as GridView;

        //    if (gridView != null && gridView.FocusedRowHandle >= 0)
        //    {
        //        gridView.DeleteRow(gridView.FocusedRowHandle);
        //    }
        //}

        private void gridyenile()
        {
            gridColumn1.FieldName = "Aciklama";
            gridColumn2.FieldName = "Rec_Kdv";
            gridColumn3.FieldName = "Net";
            gridColumn4.FieldName = "Kdv";
            gridColumn5.FieldName = "Tutar";
            gridColumn6.FieldName = "Onburo";


            DataTable dt = new DataTable();
            if (Tip == "F")
            {


                dt = dbtools.SelectTable(@"



                    declare @Fis_Tutar decimal(18,2) = (select SUM(Satis.Rsat_Tutar)   
                    FROM Cst_Recete_Satis as satis WITH(NOLOCK) 
                    LEFT JOIN Pos_Kodlar as  kodlar WITH(NOLOCK) ON Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' and Pkod_Ozelkod <> '4'
                    where Satis.Rsat_Fisno = '" + Convert.ToString(this.Tag) + @"' and Satis.Rsat_Ba = 'B' )



                          "


                        + " declare @Katsayi decimal(18,8) = (" + (TL_Tutar).ToString().Replace(",", ".") + " / @Fis_Tutar ) "
                        + " if @Katsayi = 0 begin set @Katsayi = 1 end " +
                        "" +
                        "" +
                        "" +
                        "" +
                        ""
                        + " SELECT MIN(Rsat_Tarih) as Rsat_Tarih,MIN(Rsat_Fisno) as Rsat_Fisno,Kodlar_Ad + ' BEDELI' as Aciklama,MIN(Rsat_Kdvoran) as Rec_Kdv, "
                        + "     ((SUM(Rsat_Tutar) * 100 / (100 + MIN(Rsat_Kdvoran))) * MIN(Rsat_Kdvoran) / 100) * @Katsayi as Kdv,  "
                        + "     (SUM(Rsat_Tutar) * 100 / (100 + MIN(Rsat_Kdvoran))) * @Katsayi as Net, "
                        + "     (SUM(Rsat_Tutar)) * @Katsayi as Tutar, "
                        + " MIN(Rsat_Onbdep) as Onburo "
                        + " FROM Cst_Recete_Satis WITH(NOLOCK) "
                        + "     LEFT JOIN Cst_Recete WITH(NOLOCK) on Rec_Genelkod = Rsat_Recete "
                        + "     LEFT JOIN Stok_Kodlar WITH(NOLOCK) on Rec_Urungrup = Kodlar_Kod AND Kodlar_Sinif = '10' "
                        + " WHERE Rsat_Fisno = '" + Convert.ToString(this.Tag) + "' AND Rsat_Ba = 'B' "
                        + " GROUP BY Kodlar_Ad "
                        + " ORDER BY Kodlar_Ad desc");
            }
            else
            {

                dt = dbtools.SelectTable(@"



                    declare @Fis_Tutar decimal(18,2) = (select SUM(Satis.Rsat_Tutar)   
                    FROM Cst_Recete_Satis as satis WITH(NOLOCK) 
                    LEFT JOIN Pos_Kodlar as  kodlar WITH(NOLOCK) ON Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' and Pkod_Ozelkod <> '4'
                    where Satis.Rsat_Kart_ID = '" + Convert.ToString(this.Tag) + @"' and Satis.Rsat_Ba = 'B' ) "


                        + " declare @Katsayi decimal(18,8) = (" + (TL_Tutar).ToString().Replace(",", ".") + " / @Fis_Tutar ) "
                        + " if @Katsayi = 0 begin set @Katsayi = 1 end "
                        + " SELECT MIN(Rsat_Tarih) as Rsat_Tarih,MIN(Rsat_Fisno) as Rsat_Fisno,Kodlar_Ad + ' BEDELI' as Aciklama,MIN(Rsat_Kdvoran) as Rec_Kdv, "
                        + "     ((SUM(Rsat_Tutar) * 100 / (100 + MIN(Rsat_Kdvoran))) * MIN(Rsat_Kdvoran) / 100) * @Katsayi as Kdv,  "
                        + "     (SUM(Rsat_Tutar) * 100 / (100 + MIN(Rsat_Kdvoran))) * @Katsayi as Net, "
                        + "     (SUM(Rsat_Tutar)) * @Katsayi as Tutar, "
                        + " MIN(Rsat_Onbdep) as Onburo "
                        + " FROM Cst_Recete_Satis WITH(NOLOCK) "
                        + "     LEFT JOIN Cst_Recete WITH(NOLOCK) on Rec_Genelkod = Rsat_Recete "
                        + "     LEFT JOIN Stok_Kodlar WITH(NOLOCK) on Rec_Urungrup = Kodlar_Kod AND Kodlar_Sinif = '10' "
                        + " WHERE Rsat_Kart_ID = '" + Convert.ToString(this.Tag) + "' AND Rsat_Ba = 'B' "
                        + " GROUP BY Kodlar_Ad "
                        + " ORDER BY Kodlar_Ad desc");
            }

            dt.Columns["Rec_Kdv"].DefaultValue = 0.00;
            dt.Columns["Net"].DefaultValue = 0.00;
            dt.Columns["Kdv"].DefaultValue = 0.00;
            dt.Columns["Tutar"].DefaultValue = 0.00;


            //string columname = "SİL";
            //dt.Columns.Add(columname, typeof(string));
            //buttonEkle(columname);

            gridControl1.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                date_Tarih.DateTime = Convert.ToDateTime(dt.Rows[0]["Rsat_Tarih"]);
            }
            else
            {
                date_Tarih.DateTime = Param.Tarih;
            }

            Toplamlar();
        }

        private void Toplamlar()
        {
            decimal Matrah18 = 0, Kdv18 = 0, Toplam18 = 0;
            decimal Matrah8 = 0, Kdv8 = 0, Toplam8 = 0;
            decimal Matrah1 = 0, Kdv1 = 0, Toplam1 = 0;

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                if (Convert.ToInt32(gridView1.GetRowCellValue(i, "Rec_Kdv")) == 18)
                {
                    Matrah18 = Convert.ToDecimal(Matrah18 + Convert.ToDecimal(gridView1.GetRowCellValue(i, "Net")));
                    Kdv18 = Convert.ToDecimal(Kdv18 + Convert.ToDecimal(gridView1.GetRowCellValue(i, "Kdv")));
                    Toplam18 = Convert.ToDecimal(Toplam18 + Convert.ToDecimal(gridView1.GetRowCellValue(i, "Tutar")));

                }
                if (Convert.ToInt32(gridView1.GetRowCellValue(i, "Rec_Kdv")) == 8)
                {
                    Matrah8 = Convert.ToDecimal(Matrah8 + Convert.ToDecimal(gridView1.GetRowCellValue(i, "Net")));
                    Kdv8 = Convert.ToDecimal(Kdv8 + Convert.ToDecimal(gridView1.GetRowCellValue(i, "Kdv")));
                    Toplam8 = Convert.ToDecimal(Toplam8 + Convert.ToDecimal(gridView1.GetRowCellValue(i, "Tutar")));
                }
                if (Convert.ToInt32(gridView1.GetRowCellValue(i, "Rec_Kdv")) == 1)
                {
                    Matrah1 = Convert.ToDecimal(Matrah1 + Convert.ToDecimal(gridView1.GetRowCellValue(i, "Net")));
                    Kdv1 = Convert.ToDecimal(Kdv1 + Convert.ToDecimal(gridView1.GetRowCellValue(i, "Kdv")));
                    Toplam1 = Convert.ToDecimal(Toplam1 + Convert.ToDecimal(gridView1.GetRowCellValue(i, "Tutar")));
                }
            }


            txt_Matrah18.Text = Convert.ToDecimal(Matrah18).ToString("n2");
            txt_Kdv18.Text = Convert.ToDecimal(Kdv18).ToString("n2");
            txt_Tutar18.Text = Convert.ToDecimal(Toplam18).ToString("n2");

            txt_Matrah8.Text = Convert.ToDecimal(Matrah8).ToString("n2");
            txt_Kdv8.Text = Convert.ToDecimal(Kdv8).ToString("n2");
            txt_Tutar8.Text = Convert.ToDecimal(Toplam8).ToString("n2");

            txt_Matrah1.Text = Convert.ToDecimal(Matrah1).ToString("n2");
            txt_Kdv1.Text = Convert.ToDecimal(Kdv1).ToString("n2");
            txt_Tutar1.Text = Convert.ToDecimal(Toplam1).ToString("n2");

            txt_MatrahGenel.Text = Convert.ToString(Convert.ToDecimal(txt_Matrah18.Text) + Convert.ToDecimal(txt_Matrah8.Text) + Convert.ToDecimal(txt_Matrah1.Text));
            txt_KdvGenel.Text = Convert.ToString(Convert.ToDecimal(txt_Kdv18.Text) + Convert.ToDecimal(txt_Kdv8.Text) + Convert.ToDecimal(txt_Kdv1.Text));
            txt_TutarGenel.Text = Convert.ToString(Convert.ToDecimal(txt_Tutar18.Text) + Convert.ToDecimal(txt_Tutar8.Text) + Convert.ToDecimal(txt_Tutar1.Text));
        }


        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Yazdir_Click(object sender, EventArgs e)
        {
            if (PFat_Efatura.SelectedIndex != 0)
            {
                if (PFat_Senaryo.SelectedIndex == -1)
                {
                    MessageBox.Show(res_man.GetString("TEMEL FATURA veya TICARI FATURA Olarak Belirtiniz.."));
                    return;
                }

                if (PFat_Sk.SelectedIndex == -1)
                {
                    MessageBox.Show(res_man.GetString("KİŞİ veya ŞİRKET Olarak Belirtiniz.."));
                    return;
                }

                if (txt_Vergidaire.Text == "" || txt_Vergino.Text == "")
                {
                    MessageBox.Show(res_man.GetString("Vergi Daire ve Vergi No Alanlarını Doldurunuz.."));
                    return;
                }

            }

            Fatura_Kaydet();

            if (PFat_Efatura.SelectedIndex == 0)
            {
                Fatura_Yazdir();
            }

            this.Close();
        }


        //private DataTable KdvAyrim(int Fisno)
        //{
        //    return dbtools.SelectTable(@"
        //        declare @indOran decimal(18,2) = (100 * ((select Rsat_Tutar from Cst_Recete_Satis left join Pos_Kodlar on Pkod_Kod = Rsat_Kapatma and Pkod_Sinif = '01'  where Rsat_Fisno = '" + Fisno + "' and Rsat_Ba = 'A' and Pkod_Ozelkod = '4') / (select SUM(Rsat_Tutar) from Cst_Recete_Satis  where Rsat_Fisno = '" + Fisno+ @"' and Rsat_Ba = 'B')))  
        //        SELECT Rsat_Kdvoran, (SUM(Rsat_Kdv) * (100 - isnull(@indOran,0)) / 100) as Tutar  "
        //                + " FROM Cst_Recete_Satis WITH(NOLOCK) "
        //                + " LEFT JOIN Cst_Recete WITH(NOLOCK) on Rec_Genelkod = Rsat_Recete "
        //                + " LEFT JOIN Stok_Kodlar WITH(NOLOCK) on Rec_Urungrup = Kodlar_Kod AND Kodlar_Sinif = '10' "
        //                + " LEFT JOIN Pos_Kodlar as  kodlar WITH(NOLOCK) ON Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' and Pkod_Ozelkod <> '4'"
        //                + " WHERE Rsat_Fisno = '" + Fisno.ToString() + "' AND Rsat_Ba = 'B'"
        //                + " GROUP BY Rsat_Kdvoran "
        //                + " ORDER BY Rsat_Kdvoran desc");
        //}

        private DataTable KdvAyrim(int Fisno)
        {
            //DataTable dt = dbtools.SelectTable(@"declare @Fis_Tutar decimal(18,2) = (select SUM(Rsat_Tutar) FROM Cst_Recete_Satis WITH(NOLOCK) 
            //    where Rsat_Fisno = '" + Fisno.ToString() + "' and Rsat_Ba = 'B') "
            //            + " declare @Alacak_Tutar decimal(18,2) = (select SUM(Rsat_Tutar) FROM Cst_Recete_Satis as odeme WITH(NOLOCK) LEFT JOIN Pos_Kodlar as  kodlar WITH(NOLOCK) ON odeme.Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' where Rsat_Fisno = '" + Fisno.ToString() + "' and Pkod_Ozelkod <> '4' and Rsat_Ba = 'A') "
            //            + " declare @Katsayi decimal(18,8) = (@Alacak_Tutar / @Fis_Tutar ) "
            //            + " if ISNULL(@Katsayi,0) = 0 begin set @Katsayi = 1 end "
            //            + " SELECT Rsat_Kdvoran,(SUM(Rsat_Kdv)) * @Katsayi as Tutar  "
            //            + " FROM Cst_Recete_Satis WITH(NOLOCK) "
            //            + " LEFT JOIN Cst_Recete WITH(NOLOCK) on Rec_Genelkod = Rsat_Recete "
            //            + " LEFT JOIN Stok_Kodlar WITH(NOLOCK) on Rec_Urungrup = Kodlar_Kod AND Kodlar_Sinif = '10' "
            //            + " WHERE Rsat_Fisno = '" + Fisno.ToString() + "' AND Rsat_Ba = 'B' "
            //            + " GROUP BY Rsat_Kdvoran "
            //            + " ORDER BY Rsat_Kdvoran desc");
            DataTable dt = new DataTable();
            if (Fisno > 0)
            {

                dt = dbtools.SelectTable(@" declare @Fis_Tutar decimal(18, 2) = (select SUM(Satis.Rsat_Tutar)
                                                  FROM Cst_Recete_Satis as satis WITH(NOLOCK)
                                                  LEFT JOIN Pos_Kodlar as kodlar WITH(NOLOCK) ON Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' and Pkod_Ozelkod <> '4'
                                                  where Satis.Rsat_Fisno = '" + Fisno.ToString() + @"' and Satis.Rsat_Ba = 'B' ) "


                            + " declare @Katsayi decimal(18,8) = (" + TL_Tutar.ToString().Replace(",", ".") + " / @Fis_Tutar ) "
                            + " if @Katsayi = 0 begin set @Katsayi = 1 end "
                            + " SELECT ((SUM(Rsat_Tutar) * 100 / (100 + (Rsat_Kdvoran))) * (Rsat_Kdvoran) / 100) * @Katsayi as Tutar,  "
                            + "     (SUM(Rsat_Tutar) * 100 / (100 + (Rsat_Kdvoran))) * @Katsayi as Net,(Rsat_Kdvoran) as Rsat_Kdvoran "
                            + " FROM Cst_Recete_Satis WITH(NOLOCK) "
                            + "     LEFT JOIN Cst_Recete WITH(NOLOCK) on Rec_Genelkod = Rsat_Recete "
                            + "     LEFT JOIN Stok_Kodlar WITH(NOLOCK) on Rec_Urungrup = Kodlar_Kod AND Kodlar_Sinif = '10' "
                            + " WHERE Rsat_Fisno = '" + Fisno.ToString() + "' AND Rsat_Ba = 'B' "
                            + "Group by Rsat_Kdvoran");



            }
            else
            {
                dt = (DataTable)gridControl1.DataSource;




            }

            return dt;
        }


        private void Fatura_Yazdir()
        {

            string Masano = String.Empty, Kasiyer = String.Empty, Garson = String.Empty;
            DataTable dtFis = dbtools.SelectTable("select top 1 Rsat_Masa,Kasiyer.P_Ad + ' ' + Kasiyer.P_Soyad as Kasiyer,Garson.P_Ad + ' ' + garson.P_Soyad as Garson from Cst_Recete_Satis with(nolock) "
                                       + " LEFT JOIN Rmosmuh.dbo.Pos_User as Kasiyer with(nolock) on Rsat_Garson = Kasiyer.P_Kod "
                                       + " LEFT JOIN Rmosmuh.dbo.Pos_User as Garson with(nolock) on Rsat_Garson2 = Garson.P_Kod "
                                       + "where Rsat_Ba = 'B' and Rsat_Fisno = '" + this.Tag.ToString() + "'  ");
            if (dtFis.Rows.Count > 0)
            {
                Masano = Convert.ToString(dtFis.Rows[0]["Rsat_Masa"]);
                Kasiyer = Convert.ToString(dtFis.Rows[0]["Kasiyer"]);
                Garson = Convert.ToString(dtFis.Rows[0]["Garson"]);
            }

            string Printer = String.Empty;

            DataTable dtPrinter = dbtools.SelectTable("SELECT Pkod_Ad, Pkod_Satir FROM Pos_Kodlar  where Pkod_Sinif = '16' and Pkod_Ustgrup = 'FAT' and Pkod_Kod = '" + Departman.Dep_Kodu + "' ");
            if (dtPrinter.Rows.Count > 0)
            {
                Printer = dtPrinter.Rows[0]["Pkod_Ad"].ToString();
            }
            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'FAT' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                Printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }
            if (Printer == String.Empty)
            {
                MessageBox.Show(res_man.GetString("Adisyon için Bir Printer Seçimi Yapılmamış..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }



            DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'POS_FATURA'");
            if (dtDizayn.Rows.Count > 0)
            {

                Print.Fatura fatura = new Print.Fatura();
                xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), fatura);

                if (chk_Faturun.Checked)
                {
                    fatura.DataSource = dbtools.SelectTable("declare @indOran decimal(18,2) = (100 * ((select Rsat_Tutar from Cst_Recete_Satis left join Pos_Kodlar on Pkod_Kod = Rsat_Kapatma and Pkod_Sinif = '01'  where Rsat_Fisno = '" + this.Tag + "' and Rsat_Ba = 'A' and Pkod_Ozelkod = '4') / (select SUM(Rsat_Tutar) from Cst_Recete_Satis  where Rsat_Fisno = '" + this.Tag + "' and Rsat_Ba = 'B'))) "
                                            + " select  Rec_Ad as Urun_Grup, "
                                            + "        Rec_Kdv, "
                                            + "        Rsat_Net * (100 - isnull(@indOran,0)) /100 as Kdvsiz "
                                            + " from Cst_Recete_Satis "
                                            + "     left join Cst_Recete on Rsat_Recete = Rec_Genelkod "
                                            + " where Rsat_Fisno = '" + this.Tag + "' and Rsat_Ba = 'B'");
                }
                else
                {
                    fatura.DataSource = gridControl1.DataSource;
                }

                fatura.xrLabelSirket_IsimUnvan.Text += PFat_Ad.Text;
                fatura.xrLabel_VergiDaire.Text += txt_Vergidaire.Text;
                fatura.xrLabel_VergiNo.Text += txt_Vergino.Text;
                fatura.xrLabel_FaturaAdres1.Text = txt_Adres1.Text;
                fatura.xrLabel_FaturaAdres2.Text = txt_Adres2.Text;
                fatura.xrLabel_FaturaAdres3.Text = txt_Adres3.Text;

                fatura.xrLabel_Departman.Text = Departman.Dep_Adi;
                fatura.xrLabel_Tarih.Text = date_Tarih.DateTime.Date.ToString("dd.MM.yyyy");
                fatura.xrLabel_Masa.Text += Masano;
                fatura.xrLabel_Kasiyer.Text += Kasiyer;
                fatura.xrLabel_Garson.Text += Garson;
                fatura.xrLabel_CekNo.Text += this.Tag.ToString();

                fatura.xrLabel_Matrah.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Net", "{0:n2}") });

                fatura.xrLabel_Aciklama.Text = "[Aciklama]".ToString();
                fatura.xrLabel_KDV.Text = "[Rec_Kdv]".ToString();
                fatura.xrLabel_Matrah.Text = "[Net]".ToString();



                DataTable dtKdvAyrim = new DataTable();
                dtKdvAyrim = KdvAyrim(Convert.ToInt32(this.Tag));
                for (int j = 0; j < dtKdvAyrim.Rows.Count; j++)
                {
                    XRTableRow row = new XRTableRow();

                    XRTableCell cell1 = new XRTableCell();
                    if (Convert.ToInt32(this.Tag) > 0)
                    {
                        cell1.Text = "KDV" + "" + Convert.ToDecimal(dtKdvAyrim.Rows[j]["Rsat_Kdvoran"]).ToString("n2");
                    }
                    else
                    {
                        cell1.Text = "KDV" + "" + Convert.ToDecimal(dtKdvAyrim.Rows[j]["Rec_Kdv"]).ToString("n2");
                    }
                    cell1.WidthF = fatura.table_KdvAyrim.WidthF * 60 / 100;
                    row.Cells.Add(cell1);

                    XRTableCell cell2 = new XRTableCell();
                    if (Convert.ToInt32(this.Tag) > 0)
                    {
                        cell2.Text = String.Format("{0:0.00}", dtKdvAyrim.Rows[j]["Tutar"]);
                    }
                    else
                    {
                        cell2.Text = String.Format("{0:0.00}", dtKdvAyrim.Rows[j]["Kdv"]);
                    }
                    cell2.WidthF = fatura.table_KdvAyrim.WidthF * 40 / 100;
                    row.Cells.Add(cell2);


                    fatura.table_KdvAyrim.Rows.Add(row);
                }



                if (chk_Faturun.Checked)
                {
                    DataTable dtToplam = dbtools.SelectTable("declare @indOran decimal(18,2) = (100 * ((select Rsat_Tutar from Cst_Recete_Satis left join Pos_Kodlar on Pkod_Kod = Rsat_Kapatma and Pkod_Sinif = '01'  where Rsat_Fisno = '" + this.Tag + "' and Rsat_Ba = 'A' and Pkod_Ozelkod = '4') / (select SUM(Rsat_Tutar) from Cst_Recete_Satis  where Rsat_Fisno = '" + this.Tag + "' and Rsat_Ba = 'B'))) "
                                            + " select  Convert(decimal(18,2),(SUM(Rsat_Net) * (100 - isnull(@indOran,0)) /100)) as Rsat_Net, "
                                            + "         Convert(decimal(18,2),(SUM(Rsat_Kdv) * (100 - isnull(@indOran,0)) /100)) as Rsat_Kdv, "
                                            + "         Convert(decimal(18,2),(SUM(Rsat_Tutar) * (100 - isnull(@indOran,0)) /100)) as Rsat_Tutar "
                                            + " from Cst_Recete_Satis "
                                            + " where Rsat_Fisno = '" + this.Tag + "' and Rsat_Ba = 'B' ");

                    fatura.xrLabel_Toplam.Text = Convert.ToString(dtToplam.Rows[0]["Rsat_Net"]);
                    fatura.xrLabel_ToplamKDV.Text = Convert.ToString(dtToplam.Rows[0]["Rsat_Kdv"]);
                    fatura.xrLabel_GenelToplam.Text = Convert.ToString(dtToplam.Rows[0]["Rsat_Tutar"]);

                    fatura.xrLabel_Yaziyla.Text = Yaziya_Cevir.Cevir(Convert.ToDecimal(dtToplam.Rows[0]["Rsat_Tutar"]));
                }
                else
                {

                    fatura.xrLabel_Toplam.Text = Convert.ToDecimal(txt_MatrahGenel.Text).ToString("n2");
                    fatura.xrLabel_ToplamKDV.Text = Convert.ToDecimal(txt_KdvGenel.Text).ToString("n2");
                    fatura.xrLabel_GenelToplam.Text = Convert.ToDecimal(txt_TutarGenel.Text).ToString("n2");

                    fatura.xrLabel_Yaziyla.Text = Yaziya_Cevir.Cevir(Convert.ToDecimal(txt_TutarGenel.Text));
                }



                fatura.Print(Printer);

                this.Close();
            }
            else
            {
                MessageBox.Show(res_man.GetString("Fatura Dizaynı Yapılmamış...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void Fatura_Kaydet()
        {
            int sayac = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Pos_Fatura with(nolock) where PFat_Cekno = '" + this.Tag + "'"));
            if (sayac > 0)
            {
                return;
            }


            for (int i = 0; i < gridView1.RowCount - 1; i++)
            {
                dbtools.execcmd("INSERT INTO Pos_Fatura "
                                                      + " (PFat_Tarih, PFat_Ad,PFat_Soyad,PFat_Sehir,PFat_Ilce,PFat_Senaryo,PFat_Mail,PFat_Efatura,PFat_Adres1, PFat_Adres2, PFat_Adres3, PFat_VergiDaire, PFat_VergiNo, PFat_Aciklama, PFat_Kdvoran, PFat_Kdv, PFat_Matrah, "
                                                      + " PFat_Toplam, PFat_Not, PFat_Departman, PFat_KulKodu, PFat_OdemeKodu, PFat_Cekno, PFat_OnbDep,PFat_Indirim,PFat_Fatno,PFat_Sk) "
                                + " VALUES     ('" + date_Tarih.DateTime.Date.ToString("yyyy-MM-dd") + "','" + Convert.ToString(PFat_Ad.Text) + "','" + Convert.ToString(PFat_Soyad.Text) + "','" + Convert.ToString(PFat_Sehir.Text) +
                                "','" + Convert.ToString(PFat_Ilce.Text) + "','" + PFat_Senaryo.EditValue + "','" + Convert.ToString(PFat_Mail.Text) + "','" + PFat_Efatura.EditValue + "','" + Convert.ToString(txt_Adres1.Text) + "','" + Convert.ToString(txt_Adres2.Text) + "','" + Convert.ToString(txt_Adres3.Text) + "','" + Convert.ToString(txt_Vergidaire.Text) + "','" + Convert.ToString(txt_Vergino.Text) + "',"
                                   + " '" + Convert.ToString(gridView1.GetRowCellValue(i, "Aciklama")) + "','" + Convert.ToString(gridView1.GetRowCellValue(i, "Rec_Kdv")).Replace(",", ".") + "','" + Convert.ToString(gridView1.GetRowCellValue(i, "Kdv")).Replace(",", ".") + "', "
                                   + " '" + Convert.ToString(gridView1.GetRowCellValue(i, "Net")).Replace(",", ".") + "','" + Convert.ToString(gridView1.GetRowCellValue(i, "Tutar")).Replace(",", ".") + "','" + Convert.ToString(txt_Aciklama.Text) + "',"
                                   + " '" + Convert.ToString(Departman.Dep_Kodu) + "','" + Convert.ToString(User.P_Kod) + "','" + Convert.ToString(Odemekodu) + "','" + this.Tag + "','" + Convert.ToString(gridView1.GetRowCellValue(i, "Onburo")) + "','" + Convert.ToString(txt_Indtoplam.EditValue).Replace(",", ".") + "','" + txt_Fatno.Text + "', "
                                   + "'" + PFat_Sk.EditValue + "')");
            }
        }

        private void btn_Satirsil_Click(object sender, EventArgs e)
        {
            gridView1.DeleteSelectedRows();
            Toplamlar();
        }

        private void btn_Ara_Click(object sender, EventArgs e)
        {
            //Cari_Ara cari = new Cari_Ara();
            //cari.Tag = "F";
            //cari.ShowDialog();
            //txt_Adsoyad.Text = xAd;
            //txt_Adres1.Text = xAdres1;
            //txt_Adres2.Text = xAdres2;
            //txt_Adres3.Text = xAdres3;
            //txt_Vergidaire.Text = xVergi_Daire;
            //txt_Vergino.Text = xVergiNo;


            Arama ara = new Arama();
            //ara.Odeme_Ozelkod = 5;
            ara.ShowDialog();

            string q1 = $@"select* from Pos_Cari WITH(NOLOCK) where Cari_Kod = '{ara.Cari_Kod }' and ISNULL(Cari_Aktif,1) = 1 ";
            DataTable dt = dbtools.SelectTable(q1);
            if (dt.Rows.Count > 0)
            {
                PFat_Ad.Text = Convert.ToString(dt.Rows[0]["Cari_Funvan"]);
                PFat_Soyad.Text = Convert.ToString(dt.Rows[0]["Cari_Funvan2"]);
                txt_Adres1.Text = Convert.ToString(dt.Rows[0]["Cari_Fadres1"]);
                txt_Adres2.Text = Convert.ToString(dt.Rows[0]["Cari_Fadres2"]);
                txt_Adres3.Text = "";// Convert.ToString(dt.Rows[0]["Cari_Adres3"]);
                txt_Vergidaire.Text = Convert.ToString(dt.Rows[0]["Cari_Vergidarie"]);
                txt_Vergino.Text = Convert.ToString(dt.Rows[0]["Cari_Vergino"]);
                PFat_Ilce.Text = Convert.ToString(dt.Rows[0]["Cari_Ilce"]);
                PFat_Sehir.EditValue = Convert.ToString(dt.Rows[0]["Cari_Il"]);
                PFat_Mail.Text = Convert.ToString(dt.Rows[0]["Cari_Mail"]);


            }

        }

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            Toplamlar();
        }

        private void repo_Tutar_Leave(object sender, EventArgs e)
        {
            /*
            
            gridColumn1.FieldName = "Aciklama";
            gridColumn2.FieldName = "Rec_Kdv";
            gridColumn3.FieldName = "Net";
            gridColumn4.FieldName = "Kdv";
            gridColumn5.FieldName = "Tutar";
            
             */

            if (Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rec_Kdv")) > 0 && Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Tutar")) > 0)
            {
                gridView1.SetFocusedRowCellValue("Net", Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Tutar")) / ((100 + Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rec_Kdv"))) / 100));
                gridView1.SetFocusedRowCellValue("Kdv", Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Tutar")) - (Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Tutar")) / ((100 + Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rec_Kdv"))) / 100)));
            }
            else
            {
                gridView1.SetFocusedRowCellValue("Net", 0);
                gridView1.SetFocusedRowCellValue("Kdv", 0);
            }
        }

        private void txt_Vergino_Leave(object sender, EventArgs e)
        {
            string Deger = dbtools.DegerGetir("Select Count(*) From Rmosmuh..E_Kayitli_Mukellef Where E_Vkn = '" + txt_Vergino.Text + "'");
            if (Deger != "0")
            {
                PFat_Efatura.SelectedIndex = 1;
            }
        }

        private void Fatura_Shown(object sender, EventArgs e) 
        {
            
        }

        private void btnOtoCari_Click(object sender, EventArgs e)
        {
            txt_Vergino.Text = "";
        }

        private void btnFatTemizle_Click(object sender, EventArgs e)
        {
            txt_Fatno.Text = "";
        }
    }
}