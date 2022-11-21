using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using Pos.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace Pos
{
    public partial class Kasa_Islem : DevExpress.XtraEditors.XtraForm
    {
        CheckButton chk_Islem = null;
        string Pkasa_GC = "";
        int Pkasa_Id = 0;
        int Kasa_Ciktisayisi;
        private Font printFont;
        string Kasa_Font;
        List<string> Liste;

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float yPos = 0;
            int count = 0;
            float leftMargin = 1;
            float topMargin = 1;

            for (int i = 0; i < Liste.Count; i++)
            {
                Font mFont = printFont;
                if (Liste[i].StartsWith("TOPLAM"))
                {
                    mFont = new Font(printFont.FontFamily, printFont.Size + 2, FontStyle.Bold);
                }
                if (Liste[i].StartsWith("  *"))
                {
                    mFont = new Font(printFont.FontFamily, printFont.Size + 2, FontStyle.Bold);
                }
                if (Liste[i].StartsWith("#"))
                {
                    Liste[i] = Liste[i].Replace("#", "");
                    mFont = new Font(printFont.FontFamily, printFont.Size + 2, FontStyle.Bold);
                }

                if (Liste[i].StartsWith("-#"))
                {
                    Liste[i] = Liste[i].Replace("-#", "");
                    mFont = new Font(printFont.FontFamily, printFont.Size + 4, FontStyle.Bold);
                }


                yPos = topMargin + (count *
                  printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(Liste[i], mFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }
        }

        public Kasa_Islem()
        {
            InitializeComponent();
        }

        public void depYukle()
        {
            try
            {
                DataTable dt_Dep = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar with(nolock) where Kodlar_Sinif = '01' and  Kodlar_Anadepo = 'False' and Kodlar_Satis=1 order by Kodlar_Kod");
                cmb_Departman.Properties.DataSource = dt_Dep;
                cmb_Departman.Properties.DisplayMember = "Kodlar_Ad";
                cmb_Departman.Properties.ValueMember = "Kodlar_Kod";
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "", "", ex);
            }
        }
        private void Kasa_Islem_Load(object sender, EventArgs e)
        {
            try
            {

                // tab_KasaRapor
                depYukle();


                xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;






                date_Tarih.DateTime = Param.Tarih;

                dateTarih1.DateTime = Param.Tarih;
                dateTarih2.DateTime = Param.Tarih;

                dateTarih1_1.DateTime = Param.Tarih;
                dateTarih2_2.DateTime = Param.Tarih;

                Tarih1.DateTime = Param.Tarih.Date;//.AddYears(-2);
                Tarih2.DateTime = Param.Tarih.Date;


                bool chk_K_KasaRapor = Convert.ToBoolean(dbtools.DegerGetir("select top 1 isnull(chk_K_KasaRapor,0) as chk_K_KasaRapor from Rmosmuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'"));

                if (chk_K_KasaRapor == false)
                {
                    tab_KasaRapor.PageVisible = false;
                    chk_KasaRapor.Visible = false;
                    chk_KasaGiris.Checked = true;
                }
                else
                {
                    chk_KasaRapor.Checked = true;
                }

                gridyenile_KasaGirCik();


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError("MyClass", "", "", ex);
            }

        }
        public static string MyClass = "Kasa_Islem";
        public void Mail_Gonder(DateTime tarih, DateTime tarih2, GridControl gC, string RaporAdi)
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
                        link.Component = gC;
                        link.Landscape = true;
                        link.PaperKind = System.Drawing.Printing.PaperKind.A4;
                        link.CreateDocument();
                        //link.PrintingSystemBase.ExportToPdf(tarih.ToShortDateString() + " - " + tarih2.ToShortDateString() + " Tarih Arası " + RaporAdi + ".pdf");

                        MemoryStream mem = new MemoryStream();
                        link.PrintingSystemBase.ExportToPdf(mem);
                        mem.Seek(0, System.IO.SeekOrigin.Begin);
                        Attachment att = new Attachment(mem, tarih.ToShortDateString() + " - " + tarih2.ToShortDateString() + " Tarih Arası " + RaporAdi + ".pdf", "application/pdf");



                        //MemoryStream mem = new MemoryStream();
                        ////gunsonu.ExportToPdf(mem);
                        //mem.Seek(0, System.IO.SeekOrigin.Begin);
                        //Attachment att = new Attachment(tarih.ToShortDateString() + " - " + tarih2.ToShortDateString() + " Tarih Arası " + RaporAdi + ".pdf");


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
                        ePosta.Subject = tarih.Date.ToLongDateString() + " - " + tarih2.ToShortDateString() + " Pos İki Tarih Arası " + RaporAdi + " Raporu";
                        ePosta.Attachments.Add(att);
                        //ePosta.Attachments.Add(att2);
                        //ePosta.Attachments.Add(att3);

                        string mailbody = Mail_Detay(tarih, tarih2, RaporAdi);


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

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private string Mail_Detay(DateTime tarih, DateTime tarih2, string RaporAdi)
        {
            return tarih.Date.ToLongDateString() + " - " + tarih2.Date.ToLongDateString() + " Tarihli Pos " + RaporAdi + " Ektedir.";
        }

        private void chk_Islem_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton chkbtn = (CheckButton)sender;
            if (chk_Islem == null)
            {
                chk_Islem = chkbtn;
            }

            if (chk_Islem == chk_KasaRapor)
            {
                chk_KasaRapor.Checked = true;
                chk_KasaGiris.Checked = false;
                chk_KasaCikis.Checked = false;

                xtraTabControl1.SelectedTabPage = tab_KasaRapor;
                gridyenile_KasaGirCik();
            }
            if (chk_Islem == chk_KasaGiris)
            {
                chk_KasaRapor.Checked = false;
                chk_KasaGiris.Checked = true;
                chk_KasaCikis.Checked = false;

                Pkasa_GC = "G";
                Pkasa_Id = 0;
                txt_KasaHrkBaslik.Text = res_man.GetString("Kasa Giriş Hareketi");
                xtraTabControl1.SelectedTabPage = tab_KasaHareket;
                GC_Doldur();
                gridyenile_Hareket();
            }
            if (chk_Islem == chk_KasaCikis)
            {
                chk_KasaRapor.Checked = false;
                chk_KasaGiris.Checked = false;
                chk_KasaCikis.Checked = true;

                Pkasa_GC = "C";
                Pkasa_Id = 0;
                txt_KasaHrkBaslik.Text = res_man.GetString("Kasa Çıkış Hareketi");
                xtraTabControl1.SelectedTabPage = tab_KasaHareket;
                GC_Doldur();
                gridyenile_Hareket();
            }
            chk_Islem = null;
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GC_Doldur()
        {
            string filter = Pkasa_GC == "G" ? " and Pkod_Kasagiris = 1 " : " and Pkod_Kasacikis = 1 ";
            look_GC.Properties.DataSource = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '22' " + filter);
            look_GC.Properties.DisplayMember = "Pkod_Ad";
            look_GC.Properties.ValueMember = "Pkod_Kod";
        }

        private void btn_Cikis2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridyenile_Hareket()
        {
            DataTable dataTable = dbtools.SelectTable("select * from Pos_Kasahrk where Pkasa_GC = '" + Pkasa_GC + "' and Pkasa_Tarih = '" + date_Tarih.DateTime.Date + "'");

            gridControl5.DataSource = dataTable;
            Pkasa_Id = 0;
            txt_Ad.Text = String.Empty;
            txt_Soyad.Text = String.Empty;
            look_GC.EditValue = null;
            txt_Tutar.Text = "0,00";
            txt_Aciklama.Text = String.Empty;
        }

        private void btn_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(look_GC.EditValue) == "" || Convert.ToDecimal(txt_Tutar.Text) <= 0 || txt_Ad.Text == "")
            {
                MessageBox.Show(res_man.GetString("Giriş-Çıkış Kodunu,Tutarı yada Adı yanlış girdiniz...") + "\n" + res_man.GetString("Kontrol ediniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Kasahrk_Kaydet";
            com.Parameters.AddWithValue("@Pkasa_Id", Pkasa_Id);
            com.Parameters.AddWithValue("@Pkasa_Tarih", date_Tarih.DateTime.Date);
            com.Parameters.AddWithValue("@Pkasa_GC", Pkasa_GC);
            com.Parameters.AddWithValue("@Pkasa_Kod", look_GC.EditValue);
            com.Parameters.AddWithValue("@Pkasa_User_Ad", txt_Ad.Text);
            com.Parameters.AddWithValue("@Pkasa_User_Soyad", txt_Soyad.Text);
            com.Parameters.AddWithValue("@Pkasa_Tutar", Convert.ToDecimal(txt_Tutar.Text));
            com.Parameters.AddWithValue("@Pkasa_Aciklama", txt_Aciklama.Text);
            com.Parameters.AddWithValue("@Pkasa_dep", Departman.Dep_Kodu);
            com.ExecuteNonQuery();

            if (con.State != ConnectionState.Closed) con.Close();

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kasa_Hrk, Log.Log_Islem.Kaydet, date_Tarih.DateTime.Date.ToString("dd.MM.yyyy") + " tarihinde " + txt_Tutar.Text + " tutarı kasa g-c kodu : " + Pkasa_GC + " eklendi", "", "");

            gridyenile_Hareket();
        }

        private void btn_Sil_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(res_man.GetString("Seçili Kaydı Silmek İstediğinize Emin Misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            dbtools.execcmd("delete from Pos_Kasahrk where Pkasa_Id = '" + Pkasa_Id.ToString() + "'");

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kasa_Hrk, Log.Log_Islem.Kaydet, date_Tarih.DateTime.Date.ToString("dd.MM.yyyy") + "  tarihinde " + txt_Tutar.Text + " tutarı kasa g-c kodu : " + Pkasa_GC + " silindi", "", "");

            gridyenile_Hareket();
        }

        private void gridView5_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView5.RowCount > 0)
            {
                Pkasa_Id = Convert.ToInt32(gridView5.GetFocusedRowCellValue("Pkasa_Id"));
                date_Tarih.DateTime = Convert.ToDateTime(gridView5.GetFocusedRowCellValue("Pkasa_Tarih"));
                txt_Ad.Text = Convert.ToString(gridView5.GetFocusedRowCellValue("Pkasa_User_Ad"));
                txt_Soyad.Text = Convert.ToString(gridView5.GetFocusedRowCellValue("Pkasa_User_Soyad"));
                look_GC.EditValue = Convert.ToString(gridView5.GetFocusedRowCellValue("Pkasa_Kod"));
                txt_Tutar.Text = Convert.ToDecimal(gridView5.GetFocusedRowCellValue("Pkasa_Tutar")).ToString();
                txt_Aciklama.Text = Convert.ToString(gridView5.GetFocusedRowCellValue("Pkasa_Aciklama"));
            }
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            if (Pkasa_Id > 0)
            {
                FisPr print = new FisPr();
                print.KasaMakbuzPr(Pkasa_Id);
            }
            else
            {
                MessageBox.Show(res_man.GetString("Makbuzu Önce Kayıt Ediniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void gridyenile_KasaGirCik()
        {
            try
            {

                string departman = cmb_Departman.EditValue.ToString();
                string filtreDep = "", filtreDep2 = "", filtreDep3 = "";

                if (departman != "")
                {
                    filtreDep = " and Rsat_Departman in(" + departman + ") ";
                    filtreDep2 = " and Pkasa_dep in(" + departman + ") ";
                    filtreDep3 = " and Chrk_Depart in(" + departman + ") ";
                }


                if (cmb_Departman.Properties.Items.GetCheckedValues().Count == (cmb_Departman.Properties.DataSource as DataTable).Rows.Count)
                {
                    filtreDep = "";
                    filtreDep2 = "";
                    filtreDep3 = "";
                }



                string sql = "select Rsat_Departman,Pkod_Kod,Pkod_Ad,SUM(Rsat_Tutar) AS Rsat_Tutar,SUM(Rsat_Doviztutar) AS Rsat_Doviztutar,Mkodlar_Ad as Dovizad  "
               + " from Cst_Recete_Satis WITH(NOLOCK) "
               + " LEFT JOIN Pos_Kodlar ON Rsat_Kapatma = Pkod_Kod and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' "
               + " LEFT JOIN Muh_Kodlar ON Mkodlar_Kod = Rsat_Dovizkodu and Mkodlar_sinif = '02' "
               + " WHERE Rsat_Ba = 'A' " + filtreDep + " and convert(date,Rsat_Tarih) >= '" + Tarih1.DateTime.Date + "' and convert(date,Rsat_Tarih) <= '" + Tarih2.DateTime.Date + "' and #AAA "
               + " group by Rsat_Departman,Pkod_Kod,Pkod_Ad,Rsat_Dovizkodu,Mkodlar_Ad "
               + " order by Pkod_Kod";


                string sorgu1 = sql.Replace("#AAA", "Pkod_Kasagiris = 1");
                string sorgu2 = sql.Replace("#AAA", "Pkod_Kasacikis = 1");
                gridControl1.DataSource = dbtools.SelectTable(sorgu1);
                gridControl2.DataSource = dbtools.SelectTable(sorgu2);

                string sql2 = "select Pkasa_dep,Pkasa_Kod,Pkod_Ad,SUM(Pkasa_Tutar) as Pkasa_Tutar "
                    + " from Pos_Kasahrk WITH(NOLOCK) "
                    + " LEFT JOIN Pos_Kodlar ON Pkasa_Kod = Pkod_Kod and Pkod_Sinif = '22' "
                    + " where convert(date,Pkasa_Tarih) >= '" + Tarih1.DateTime.Date + "' " + filtreDep2 + " and convert(date,Pkasa_Tarih) <= '" + Tarih2.DateTime.Date + "'  and #AAA "
                    + " group by Pkasa_dep,Pkasa_Kod,Pkod_Ad";

                string sorgu3 = sql2.Replace("#AAA", " Pkasa_GC = 'G' and Pkod_Kasagiris = 1 ");
                string sorgu4 = sql2.Replace("#AAA", " Pkasa_GC = 'C' and Pkod_Kasacikis = 1 ");
                gridControl4.DataSource = dbtools.SelectTable(sorgu3);
                gridControl3.DataSource = dbtools.SelectTable(sorgu4);

                string sql3 = @"
select Chrk_Depart,Pkod_Kod,Pkod_Ad,SUM(Chrk_Alacak) AS Tutar
from Pos_Carihrk WITH(NOLOCK) 
LEFT JOIN Pos_Kodlar ON Chrk_Odeme = Pkod_Kod and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' 
WHERE Chrk_Alacak > 0 and convert(date,Chrk_Tarih) >= '" + Tarih1.DateTime.Date + "' " + filtreDep3 + " and convert(date,Chrk_Tarih) <= '" + Tarih2.DateTime.Date + @"'   and #AAA 
group by Chrk_Depart,Pkod_Kod,Pkod_Ad
order by Pkod_Kod";
                //Chrk_Tarih

                string sorgu5 = sql3.Replace("#AAA", "Pkod_Kasagiris = 1");
                string sorgu6 = sql3.Replace("#AAA", "Pkod_Kasacikis = 1");
                gridControl6.DataSource = dbtools.SelectTable(sorgu5);
                gridControl7.DataSource = dbtools.SelectTable(sorgu6);

                txt_KasaGiris.Text = (Convert.ToDecimal(gridColumn3.SummaryItem.SummaryValue) + Convert.ToDecimal(gridColumn15.SummaryItem.SummaryValue) + Convert.ToDecimal(gridColumn27.SummaryItem.SummaryValue)).ToString("N2");
                txt_KasaCikis.Text = (Convert.ToDecimal(gridColumn7.SummaryItem.SummaryValue) + Convert.ToDecimal(gridColumn11.SummaryItem.SummaryValue) + Convert.ToDecimal(gridColumn30.SummaryItem.SummaryValue)).ToString("N2");

                txt_KasaGenel.Text = (Convert.ToDecimal(txt_KasaGiris.Text) - Convert.ToDecimal(txt_KasaCikis.Text)).ToString("N2");

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "gridyenile_KasaGirCik", "", ex);
            }

        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridControl8.DataSource = null;

            gridControl8.DataSource = dbtools.SelectTable("EXEC Pos_Satis_Rapor @Rapor_Tipi = 23, @Tarih1 = '" + dateTarih1.DateTime.Date + "', @Tarih2 = '" + dateTarih2.DateTime.Date + "', @Departman = '" + Departman.Dep_Kodu + "'");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            gridControl9.DataSource = null;

            gridControl9.DataSource = dbtools.SelectTable("EXEC Pos_Satis_Rapor @Rapor_Tipi = 24, @Tarih1 = '" + dateTarih1.DateTime.Date + "', @Tarih2 = '" + dateTarih2.DateTime.Date + "', @Departman = '" + Departman.Dep_Kodu + "'");
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Rapor_Print("Kasa Raporu 2", gridControl9);
        }

        private void Rapor_Print(string header, GridControl grid)
        {
            string leftColumn = header;
            string rightColumn = dateTarih1.DateTime.ToLongDateString() + "-" + dateTarih2.DateTime.ToLongDateString();


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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Rapor_Print("Kasa Raporu 1", gridControl8);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            gridyenile_KasaGirCik();
        }

        private void btn_OzetPrint_Click(object sender, EventArgs e)
        {
            FisPr pr = new FisPr();
            pr.KasaGunlukOzet(Tarih1.DateTime, Tarih2.DateTime);
        }

        private void btnGenelRaporPrint_Click(object sender, EventArgs e)
        {

            try
            {
                string departman = cmb_Departman.EditValue.ToString();
                string filtreDep = "", filtreDep2 = "", filtreDep3 = "";

                if (departman != "")
                {
                    filtreDep = " and Rsat_Departman in(" + departman + ") ";
                    filtreDep2 = " and Pkasa_dep in(" + departman + ") ";
                    filtreDep3 = " and Chrk_Depart in(" + departman + ") ";
                }


                if (cmb_Departman.Properties.Items.GetCheckedValues().Count == (cmb_Departman.Properties.DataSource as DataTable).Rows.Count)
                {
                    filtreDep = "";
                    filtreDep2 = "";
                    filtreDep3 = "";
                }


                FisPr pr = new FisPr();
                pr.KasaGunlukOzetYeni(Tarih1.DateTime, Tarih2.DateTime, filtreDep, filtreDep2, filtreDep3);


            }
            catch(Exception ex)
            {
                RHMesaj.MyMessageError(MyClass,"","",ex);
            }

        }
    }
}