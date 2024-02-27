using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using Pos.Class;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Resources;
using System.Reflection;

namespace Pos
{
    public partial class Raporlar2 : DevExpress.XtraEditors.XtraForm
    {
        public Raporlar2()
        {
            InitializeComponent();
        }

        private void Raporlar2_Load(object sender, EventArgs e)
        {
            this.BringToFront();

            dateEdit1.DateTime = Param.Tarih;
            dateEdit2.DateTime = Param.Tarih;

            string depSorgu = "select Kodlar_Kod,Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif ='01' and Kodlar_Satis = 1 order by Kodlar_Kod";
            if (getSadeceDepXz())
            {
                depSorgu = "select Kodlar_Kod,Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif ='01' and Kodlar_Satis = 1 and Kodlar_Kod='"+Departman.Dep_Kodu+"' order by Kodlar_Kod";
            }
            look_TekDep.Properties.DataSource = dbtools.SelectTable(depSorgu);
            look_TekDep.Properties.DisplayMember = "Kodlar_Ad";
            look_TekDep.Properties.ValueMember = "Kodlar_Kod";

            look_TekGarson.Properties.DataSource = dbtools.SelectTable("select P_Kod as P_Kodu,P_Ad +  ' ' + P_Soyad as AdSoyad from Rmosmuh.dbo.Pos_User order by P_Kodu");
            look_TekGarson.Properties.DisplayMember = "AdSoyad";
            look_TekGarson.Properties.ValueMember = "P_Kodu";

            look_TekGrup.Properties.DataSource = dbtools.SelectTable("select Kodlar_Id,Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = '09' order by  Kodlar_Anagrup + Kodlar_Kod");
            look_TekGrup.Properties.DisplayMember = "Kodlar_Ad";
            look_TekGrup.Properties.ValueMember = "Kodlar_Id";

            cmb_DetayGrup.Properties.DataSource = dbtools.SelectTable("select Kodlar_Id,Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = '11' order by Kodlar_Kod");
            cmb_DetayGrup.Properties.DisplayMember = "Kodlar_Ad";
            cmb_DetayGrup.Properties.ValueMember = "Kodlar_Id";

            DataTable dtSub = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Sinif = '27' order by Pkod_Kod");
            if (dtSub.Rows.Count > 0)
            {
                chkCombo_Sube.Properties.DataSource = dtSub;
                chkCombo_Sube.Properties.DisplayMember = "Pkod_Ad";
                chkCombo_Sube.Properties.ValueMember = "Pkod_Kod";
            }


            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Ad");
            dataTable.Columns.Add("Kod");
            DataRow dataRow = dataTable.NewRow();
            dataRow["Ad"] = "Trendyol";
            dataRow["Kod"] = "T";
            dataTable.Rows.Add(dataRow);

            dataRow = dataTable.NewRow();
            dataRow["Ad"] = "Yemek Sepeti";
            dataRow["Kod"] = "Y";
            dataTable.Rows.Add(dataRow);

            dataRow = dataTable.NewRow();
            dataRow["Ad"] = "Getir";
            dataRow["Kod"] = "G";
            dataTable.Rows.Add(dataRow);


            checkedComboBoxEditOnlinePaket.Properties.DataSource = dataTable;
            checkedComboBoxEditOnlinePaket.Properties.DisplayMember = "Ad";
            checkedComboBoxEditOnlinePaket.Properties.ValueMember = "Kod";
        }

        private void chk_TekDep_CheckedChanged(object sender, EventArgs e)
        {
            look_TekDep.Properties.ReadOnly = !chk_TekDep.Checked;
        }

        private void chk_TekGarson_CheckedChanged(object sender, EventArgs e)
        {
            look_TekGarson.Properties.ReadOnly = !chk_TekGarson.Checked;
        }

        private void chk_TekGrup_CheckedChanged(object sender, EventArgs e)
        {
            look_TekGrup.Properties.ReadOnly = !chk_TekGrup.Checked;
        }

        public void onlineYemekSepetiListele()
        {
            string bastar = dateEdit1.DateTime.ToString("yyyy-MM-dd");
            string bittar = dateEdit2.DateTime.ToString("yyyy-MM-dd");

            string query = $@"SELECT r.Rec_Ad  as Ad , SUM(s.Rsat_Miktar) AS Miktar ,SUM(s.Rsat_Tutar) AS Tutar 
FROM Cst_Recete_Satis s
JOIN Pos_Cari c ON s.Rsat_Cari = c.Cari_Kod
JOIN Cst_Recete r ON s.Rsat_Recete = r.Rec_Genelkod
WHERE c.Cari_Tip = '{checkedComboBoxEditOnlinePaket.EditValue.ToString()}'  and  Convert(date,s.Rsat_Tarih) >= '{bastar}' and Convert(date,s.Rsat_Tarih) <= '{bittar}'
GROUP BY r.Rec_Ad;";

            gridColumnMaliyet.Visible = true;
            gridControl1.DataSource = null;
            gridControl1.DataSource = dbtools.SelectTableR(query);
        }
        private void btn_Listele_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (checkEditOnlinePaket.Checked)
            {
                onlineYemekSepetiListele();
                return;
            }
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();

            try
            {
                if (getSadeceDepXz() && ( look_TekDep.Properties.ReadOnly == true||look_TekDep.EditValue == null||look_TekDep.EditValue.ToString().Equals("")))
                {
                    MessageBox.Show("LÜTFEN DEPARTMAN SEÇİNİZ!");
                    return;
                }

                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis_Istatistik";
                com.Parameters.AddWithValue("@Tarih1", dateEdit1.DateTime.Date);
                com.Parameters.AddWithValue("@Tarih2", dateEdit2.DateTime.Date);
                if (chk_TekDep.Checked) com.Parameters.AddWithValue("@TekDepartman", look_TekDep.EditValue);
                if (chk_TekGarson.Checked) com.Parameters.AddWithValue("@TekGarson", look_TekGarson.EditValue);
                if (chk_TekGrup.Checked) com.Parameters.AddWithValue("@TekGrup", look_TekGrup.EditValue);
                com.Parameters.AddWithValue("@Departman", chk_Dep.Checked);
                com.Parameters.AddWithValue("@Kasiyer", chk_Kasiyer.Checked);
                com.Parameters.AddWithValue("@Garson", chk_Garson.Checked);
                com.Parameters.AddWithValue("@Anagrup", chk_Anagrup.Checked);
                com.Parameters.AddWithValue("@Grup", chk_Grup.Checked);
                com.Parameters.AddWithValue("@Malzeme", chk_Malzeme.Checked);
                com.Parameters.AddWithValue("@MalzemePorsiyon", chk_MalzemePors.Checked);
                com.Parameters.AddWithValue("@Saat", chk_Saat.Checked);
                com.Parameters.AddWithValue("@Tahsilat", chk_Tahsilat.Checked);
                com.Parameters.AddWithValue("@Kisi", chk_Kisi.Checked);
                com.Parameters.AddWithValue("@Konum", chk_Konum.Checked);
                com.Parameters.AddWithValue("@Paket", chk_Paket.Checked);
                com.Parameters.AddWithValue("@SifirTutar", chk_SifirTutar.Checked);
                com.Parameters.AddWithValue("@UrunGrup", chk_UrunGrup.Checked);
                com.Parameters.AddWithValue("@UrunGrupNet", chk_UrunGrupNet.Checked);
                com.Parameters.AddWithValue("@Sube", chkCombo_Sube.EditValue);
                if (chk_DetayGrup.Checked) com.Parameters.AddWithValue("@DetayGrup", cmb_DetayGrup.EditValue);
                com.Parameters.AddWithValue("@PR", chk_PR.Checked); 
                com.Parameters.AddWithValue("@Kdv", chk_Kdv.Checked);


                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);


                gridColumnMaliyet.Visible = false;

                gridControl1.DataSource = dt;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
            finally
            {
                if (con.State != ConnectionState.Closed) con.Close();
            }
        }

        public void sadeceDept(DataTable istatik)
        {
            try
            {
                bool depZorunlu = getSadeceDepXz();
                if (depZorunlu)
                {
                    for (int i = istatik.Rows.Count - 2; i >= 1; i--)
                    {
                        DataRow dr = istatik.Rows[i];
                        if (dr["Ad"].ToString() != Departman.Dep_Adi)
                        {
                            dr.Delete();
                        }
                    }
                    istatik.AcceptChanges();
                    if (istatik.Rows.Count == 3)
                    {
                        istatik.Rows[2]["Tutar"] = istatik.Rows[1]["Tutar"];
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        public bool getSadeceDepXz()
        {
            try
            {
                string deger = dbtools.DegerGetir("SELECT top 1 isnull(Pos_XZdepartman,0) as Pos_XZdepartman  FROM RmosMuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'");
                return Convert.ToBoolean(deger);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void btn_Print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Print(false);
        }

        private void btn_Onizleme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Print(true);
        }

        private void btn_Excel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string fName = string.Empty;
            saveFileDialog1.Filter = "Excel Document (*.xls)|*.xls";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "Pos_Istatistik.xls";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != null)
                {
                    XlsExportOptions opt = new XlsExportOptions();
                    opt.ShowGridLines = true;
                    gridControl1.ExportToXls(saveFileDialog1.FileName, opt);
                }
            }
        }

        private void btn_Cikis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


        private void Print(bool Onizleme)
        {
            string leftColumn = "Genel İstatistik";
            string rightColumn = dateEdit1.DateTime.ToLongDateString() + "-" + dateEdit2.DateTime.ToLongDateString();


            PrintingSystem printingSystem1 = new PrintingSystem();
            PrintableComponentLink printableComponentLink1 = new PrintableComponentLink();
            printingSystem1.Links.AddRange(new object[] { printableComponentLink1 });
            printableComponentLink1.Component = gridControl1;
            printableComponentLink1.Landscape = false;
            printableComponentLink1.Margins = new System.Drawing.Printing.Margins(20, 20, 50, 20);

            PageHeaderFooter phf = printableComponentLink1.PageHeaderFooter as PageHeaderFooter;
            phf.Header.Content.Clear();
            phf.Header.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            phf.Header.Content.AddRange(new string[] { leftColumn, rightColumn });
            phf.Header.LineAlignment = BrickAlignment.Far;
            if (Onizleme)
            {
                printableComponentLink1.ShowPreview();
            }
            else
            {
                printableComponentLink1.PrintDlg();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)gridControl1.DataSource;

            string konu = String.Empty;

            Control Ctrl = new Control();
            foreach (Control item in Ctrl.Controls)
            {
                if (item is CheckEdit)
                    if (((CheckEdit)item).Checked)
                    {
                        konu = ((CheckEdit)item).Text;
                    }
            }

            List<string> list;

            DataTable dt3 = dbtools.SelectTable("SELECT Pkod_Ad, Pkod_Satir FROM Pos_Kodlar  where Pkod_Sinif = '16' and Pkod_Ustgrup = 'HES' and Pkod_Kod = '" + Departman.Dep_Kodu + "' ");
            if (dt3.Rows.Count > 0)
            {
                string Printer = dt3.Rows[0]["Pkod_Ad"].ToString();
                int Bos_Satir = Convert.ToInt32(dt3.Rows[0]["Pkod_Satir"].ToString());

                string tesis_Adi = Param.Tesis_Adi;

                list = new List<string>();

                list.Add("   " + konu + "    ");

                list.Add(". ");
                list.Add("    " + tesis_Adi);
                list.Add("    " + dateEdit1.DateTime.ToShortDateString() + " - " + dateEdit2.DateTime.ToShortDateString());
                list.Add("    " + DateTime.Now.ToShortTimeString());
                list.Add(".");
                list.Add("--------------------------------");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["Ad"]) == "---")
                    {
                        list.Add("--------------------------------");
                    }
                    else if (Convert.ToString(dt.Rows[i]["Ad"]).StartsWith("#"))
                    {
                        list.Add(Convert.ToString(dt.Rows[i]["Ad"]));
                    }
                    else
                    {
                        list.Add(Convert.ToString(dt.Rows[i]["Ad"]).PadRight(27, " "[0]).Substring(0, 27) + " " + Convert.ToString(dt.Rows[i]["Miktar"]).PadLeft(5, " "[0]) + " " + Convert.ToString(dt.Rows[i]["Tutar"]).PadLeft(8, " "[0]));
                    }
                }

                for (int x = 0; x < Bos_Satir + 5; x++)
                {
                    list.Add(".");
                }



                try
                {
                    string XZ_Font = dbtools.DegerGetir("select Pkod_Font from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'XZRAPOR'");
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(XZ_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Lucida Console", 9);
                    }


                    stringToPrint = string.Join(Environment.NewLine, list.ToArray());
                    //Liste = list;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                    pd.PrinterSettings.PrinterName = Printer;
                    pd.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(res_man.GetString("Bu Departmana Ait Yazıcı Ayarlarını Kontrol Ediniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        string stringToPrint = "";

        private Font printFont;
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
            //ev.Graphics.DrawString(Liste[i], printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            //count++;




            float yPos = 0;
            int count = 0;
            float leftMargin = 1;
            float topMargin = 3;

            int charactersOnPage = 0;
            int linesPerPage = 0;
            e.Graphics.MeasureString(stringToPrint, printFont, e.PageBounds.Size, new StringFormat(), out charactersOnPage, out linesPerPage);


            string[] yazi = stringToPrint.Split('\n');
            for (int i = 0; i < yazi.Length; i++)
            {
                yPos = topMargin + (count * printFont.GetHeight(e.Graphics));
                e.Graphics.DrawString(yazi[i], printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }

            stringToPrint = stringToPrint.Substring(charactersOnPage);

            e.HasMorePages = (stringToPrint.Length > 0);
        }

        public void Mail_Gonder(DateTime tarih, DateTime tarih2)
        {
            DataTable dt = dbtools.SelectTable("select ISNULL(Mail_Gonder,0) as Mail_Gonder,Mail_Isim,Mail_Adres,Mail_Parola,Mail_Host,Mail_Port,Mail_SSL, "
                                     + " Mail_Alici1,Mail_Alici2,Mail_Alici3,Mail_Alici4,Mail_Alici5, "
                                     + " isnull(Mail_Odeme_Tip,0) as Mail_Odeme_Tip,isnull(Mail_Servis_Paylari,0) as Mail_Servis_Paylari,isnull(Mail_Cari_Ozet,0) as Mail_Cari_Ozet, "
                                     + " isnull(Mail_Odenmez_Ozet,0) as Mail_Odenmez_Ozet,isnull(Mail_Malz_Ozet,0) as Mail_Malz_Ozet,isnull(Mail_Ana_Ozet,0) as Mail_Ana_Ozet, "
                                     + " isnull(Mail_Alt_Ozet,0) as Mail_Alt_Ozet,isnull(Mail_Iptal_Ozet,0) as Mail_Iptal_Ozet, "
                                     + " Mail_Alici6,Mail_Alici7,Mail_Alici8,Mail_Alici9,Mail_Alici10 from Pos_Mail WITH(NOLOCK) where Mail_Id = 1");
            if (dt.Rows.Count > 0)
            {
                //bool Mail_Gonder = Convert.ToBoolean(dt.Rows[0]["Mail_Gonder"]);
                //if (Mail_Gonder == true)
                {

                    string Mail_Isim = Convert.ToString(dt.Rows[0]["Mail_Isim"]);
                    string Mail_Adres = Convert.ToString(dt.Rows[0]["Mail_Adres"]);
                    string Mail_Parola = Convert.ToString(dt.Rows[0]["Mail_Parola"]);
                    string Mail_Host = Convert.ToString(dt.Rows[0]["Mail_Host"]);
                    string Mail_Port = Convert.ToString(dt.Rows[0]["Mail_Port"]);
                    bool Mail_SSL = Convert.ToBoolean(dt.Rows[0]["Mail_SSL"]);

                    string Mail_Alici1 = Convert.ToString(dt.Rows[0]["Mail_Alici1"]);
                    string Mail_Alici2 = Convert.ToString(dt.Rows[0]["Mail_Alici2"]);
                    string Mail_Alici3 = Convert.ToString(dt.Rows[0]["Mail_Alici3"]);
                    string Mail_Alici4 = Convert.ToString(dt.Rows[0]["Mail_Alici4"]);
                    string Mail_Alici5 = Convert.ToString(dt.Rows[0]["Mail_Alici5"]);
                    string Mail_Alici6 = Convert.ToString(dt.Rows[0]["Mail_Alici6"]);
                    string Mail_Alici7 = Convert.ToString(dt.Rows[0]["Mail_Alici7"]);
                    string Mail_Alici8 = Convert.ToString(dt.Rows[0]["Mail_Alici8"]);
                    string Mail_Alici9 = Convert.ToString(dt.Rows[0]["Mail_Alici9"]);
                    string Mail_Alici10 = Convert.ToString(dt.Rows[0]["Mail_Alici10"]);

                    try
                    {

                        System.Threading.Thread.Sleep(1 * 1000);
                        Application.DoEvents();

                        DevExpress.XtraPrinting.PrintingSystemBase ps = new DevExpress.XtraPrinting.PrintingSystemBase();
                        DevExpress.XtraPrintingLinks.PrintableComponentLinkBase link = new DevExpress.XtraPrintingLinks.PrintableComponentLinkBase(ps);
                        link.Component = gridControl1;
                        link.Landscape = true;
                        link.PaperKind = System.Drawing.Printing.PaperKind.A4;
                        link.CreateDocument();
                        link.PrintingSystemBase.ExportToPdf(tarih.ToShortDateString() + " - " + tarih2.ToShortDateString() + " Tarih Arası Genel İstatistik Raporu.pdf");


                        MemoryStream mem = new MemoryStream();
                        //gunsonu.ExportToPdf(mem);
                        mem.Seek(0, System.IO.SeekOrigin.Begin);
                        Attachment att = new Attachment(tarih.ToShortDateString() + " - " + tarih2.ToShortDateString() + " Tarih Arası Genel İstatistik Raporu.pdf");


                        MailMessage ePosta = new MailMessage();
                        ePosta.From = new MailAddress(Mail_Adres, Mail_Isim);
                        if (Mail_Alici1.Length > 0) ePosta.To.Add(Mail_Alici1);
                        if (Mail_Alici2.Length > 0) ePosta.To.Add(Mail_Alici2);
                        if (Mail_Alici3.Length > 0) ePosta.To.Add(Mail_Alici3);
                        if (Mail_Alici4.Length > 0) ePosta.To.Add(Mail_Alici4);
                        if (Mail_Alici5.Length > 0) ePosta.To.Add(Mail_Alici5);
                        if (Mail_Alici6.Length > 0) ePosta.To.Add(Mail_Alici6);
                        if (Mail_Alici7.Length > 0) ePosta.To.Add(Mail_Alici7);
                        if (Mail_Alici8.Length > 0) ePosta.To.Add(Mail_Alici8);
                        if (Mail_Alici9.Length > 0) ePosta.To.Add(Mail_Alici9);
                        if (Mail_Alici10.Length > 0) ePosta.To.Add(Mail_Alici10);
                        ePosta.Priority = MailPriority.Normal;
                        ePosta.Subject = tarih.Date.ToLongDateString() + " - " + tarih2.ToShortDateString() + " Pos İki Tarih Arası Genel İstatistik Raporu";
                        ePosta.Attachments.Add(att);
                        //ePosta.Attachments.Add(att2);
                        //ePosta.Attachments.Add(att3);

                        string mailbody = Mail_Detay(tarih, tarih2);


                        ePosta.IsBodyHtml = true;
                        ePosta.Body = mailbody;


                        SmtpClient ss = new SmtpClient(Mail_Host, Convert.ToInt32(Mail_Port));
                        ss.EnableSsl = Mail_SSL;
                        ss.DeliveryMethod = SmtpDeliveryMethod.Network;
                        ss.UseDefaultCredentials = false;
                        ss.Credentials = new NetworkCredential(Mail_Adres, Mail_Parola);
                        ss.Send(ePosta);

                        MessageBox.Show(res_man.GetString("Mail Başarıyla Gönderildi."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception err)
                    {
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Gun_Sonu, Log.Log_Islem.Kaydet, tarih.Date.ToString("dd.MM.yyyy") + " Mail Gönderim Sırasında Hata Oldu...", "", "");
                        MessageBox.Show(res_man.GetString("Mail Gönderilemedi...") + "\n" + err.Message, res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }


        private string Mail_Detay(DateTime tarih, DateTime tarih2)
        {
            return tarih.Date.ToLongDateString() + " - " + tarih2.Date.ToLongDateString() + " Tarihli Pos Genel İstatistik Raporu Ektedir.";
        }


        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Mail_Gonder(dateEdit1.DateTime, dateEdit2.DateTime);
        }

        private void chk_DetayGrup_CheckedChanged(object sender, EventArgs e)
        {
            cmb_DetayGrup.Properties.ReadOnly = !chk_DetayGrup.Checked;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();

            try
            {

                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis_Istatistik";
                com.Parameters.AddWithValue("@Tarih1", dateEdit1.DateTime.Date);
                com.Parameters.AddWithValue("@Tarih2", dateEdit2.DateTime.Date);
                if (chk_TekDep.Checked) com.Parameters.AddWithValue("@TekDepartman", look_TekDep.EditValue);
                if (chk_TekGarson.Checked) com.Parameters.AddWithValue("@TekGarson", look_TekGarson.EditValue);
                if (chk_TekGrup.Checked) com.Parameters.AddWithValue("@TekGrup", look_TekGrup.EditValue);
                com.Parameters.AddWithValue("@Departman", chk_Dep.Checked);
                com.Parameters.AddWithValue("@Kasiyer", chk_Kasiyer.Checked);
                com.Parameters.AddWithValue("@Garson", chk_Garson.Checked);
                com.Parameters.AddWithValue("@Anagrup", chk_Anagrup.Checked);
                com.Parameters.AddWithValue("@Grup", chk_Grup.Checked);
                com.Parameters.AddWithValue("@Malzeme", chk_Malzeme.Checked);
                com.Parameters.AddWithValue("@MalzemePorsiyon", chk_MalzemePors.Checked);
                com.Parameters.AddWithValue("@Saat", chk_Saat.Checked);
                com.Parameters.AddWithValue("@Tahsilat", 1);
                com.Parameters.AddWithValue("@Kisi", chk_Kisi.Checked);
                com.Parameters.AddWithValue("@Konum", chk_Konum.Checked);
                com.Parameters.AddWithValue("@Paket", chk_Paket.Checked);
                com.Parameters.AddWithValue("@SifirTutar", chk_SifirTutar.Checked);
                com.Parameters.AddWithValue("@UrunGrup", chk_UrunGrup.Checked);
                com.Parameters.AddWithValue("@UrunGrupNet", 1);
                com.Parameters.AddWithValue("@Sube", chkCombo_Sube.EditValue);
                if (chk_DetayGrup.Checked) com.Parameters.AddWithValue("@DetayGrup", cmb_DetayGrup.EditValue);
                com.Parameters.AddWithValue("@PR", chk_PR.Checked);
                com.Parameters.AddWithValue("@Kdv", 1);


                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);


                gridControl1.DataSource = dt;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
            finally
            {
                if (con.State != ConnectionState.Closed) con.Close();
            }
        }

        private void btnEncoksatan_Click(object sender, EventArgs e)
        {
            string bastar = dateEdit1.DateTime.ToString("yyyy-MM-dd");
            string bittar= dateEdit2.DateTime.ToString("yyyy-MM-dd");

            string query = @"select top 25   Recete.Rec_Ad as Ad  , sum(Rsat_Miktar) as   Miktar,sum(Rsat_Tutar) as Tutar  from Cst_Recete_Satis satis
left join Cst_Recete  Recete on satis.Rsat_Recete=Rec_Genelkod   
where satis.Rsat_Tarih between '" + bastar + @"' and '"+ bittar + @"'
group by Recete.Rec_Ad 
order by (Miktar) desc";

            gridColumnMaliyet.Visible = false;

            gridControl1.DataSource = null;
            gridControl1.DataSource = dbtools.SelectTableR(query); 
        }

        private void btnMaliyetSatis_Click(object sender, EventArgs e)
        {
            string bastar = dateEdit1.DateTime.ToString("yyyy-MM-dd");
            string bittar = dateEdit2.DateTime.ToString("yyyy-MM-dd");

            string query = @"select Recete.Rec_Ad as Ad  , sum(Rsat_Miktar) as   Miktar,sum(Rsat_Tutar) as Tutar,sum(Rsat_Maliyet) as Rsat_Maliyet  from Cst_Recete_Satis satis
left join Cst_Recete  Recete on satis.Rsat_Recete=Rec_Genelkod   
where satis.Rsat_Tarih between '" + bastar + @"' and '" + bittar + @"'
group by Recete.Rec_Ad ,Rsat_Maliyet
order by (Miktar) desc";

            gridColumnMaliyet.Visible = true;
            gridControl1.DataSource = null;
            gridControl1.DataSource = dbtools.SelectTableR(query);
        }

        private void checkEditOnlinePaket_CheckedChanged(object sender, EventArgs e)
        {
            checkedComboBoxEditOnlinePaket.Properties.ReadOnly = !checkEditOnlinePaket.Checked;
        }
    }
}
