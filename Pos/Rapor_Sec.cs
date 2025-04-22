using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using Pos.Class;
using Pos.Print;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Rapor_Sec : DevExpress.XtraEditors.XtraForm
    {
        public Rapor_Sec()
        {
            InitializeComponent();
        }

        private void Rapor_Sec_Load(object sender, EventArgs e)
        {
            btn_Raporlar.Enabled = User.R_Raporlar;
            btn_Istatistik.Enabled = User.R_Raporlar;
            btn_GuNSonu.Enabled = User.P_Gunsonu;
            btn_GunsonuMail.Enabled = User.P_Gunsonu;


            btn_Istatistik.Visible = !User.Pos_AdisyonPr;
        }

        private void btn_Raporlar_Click(object sender, EventArgs e)
        {
            Raporlar r = new Raporlar();
            r.ShowDialog();

            this.Close();
        }

        private void btn_Istatistik_Click(object sender, EventArgs e)
        {
            Raporlar2 r = new Raporlar2();
            r.ShowDialog();

            this.Close();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DataTable dtDep = new DataTable();
        private void btn_GuNSonu_Click(object sender, EventArgs e)
        {
            Rapor_Tarih r = new Rapor_Tarih();
            r.ShowDialog();

            if (r.cikis == false)
            {
                return;
            }


            Gun_Sonu g = new Gun_Sonu();

            string[] dep_Parcala;

            if (Convert.ToString(r.lookUpEdit1.EditValue) != "")
            {
                dep_Parcala = Convert.ToString(r.lookUpEdit1.EditValue.ToString().Replace(" ", "")).Split(',');

                dtDep.Columns.Add("dep");

                foreach (string a in dep_Parcala)
                {
                    dtDep.Rows.Add(a);
                }
            }
            if (dtDep.Rows.Count > 0)
            {
                for (int i = 0; i < dtDep.Rows.Count; i++)
                {
                    g.Rapor_Gunsonu_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(dtDep.Rows[i]["dep"]), Convert.ToString(r.chkCombo_Sube.EditValue)).ShowPreview();
                }
            }


            g.Rapor_Gunsonu2_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue)).ShowPreview();


            var report3 = g.Rapor_Gunsonu3_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue)); // ekrandan
            report3.CreateDocument();
            var report4 = g.Rapor_Gunsonu4_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue));
            report4.CreateDocument();
            var report5 = g.Rapor_Gunsonu5_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue));
            report5.CreateDocument();
            var report6 = g.Rapor_Gunsonu6_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue));
            report6.CreateDocument();
            var report7 = g.Rapor_Gunsonu7_Pr(r.date_Tarih1.DateTime.Date, Convert.ToString(r.chkCombo_Sube.EditValue));
            report7.CreateDocument();

            report3.Pages.AddRange(report4.Pages);
            report3.Pages.AddRange(report5.Pages);
            report3.Pages.AddRange(report6.Pages);
            report3.Pages.AddRange(report7.Pages);

            report3.PrintingSystem.ContinuousPageNumbering = true;

            ReportPrintTool printTool = new ReportPrintTool(report3);
            printTool.ShowPreview();




            FisPr pr = new FisPr();
            string kasaRapor = pr.KasaGunlukOzetString(r.date_Tarih1.DateTime, r.date_Tarih1.DateTime);
            Rapor_Kasa raporKasa = new Rapor_Kasa();
            raporKasa.xrLabel1.Text = kasaRapor;
            raporKasa.txtTarih.Text = "Tarih :" + Param.Tarih.ToString("dd.MM.yyyy");
            raporKasa.ShowPreview();

            cariRapGoster(false);



            g.muhasebeRapor(false);
            this.Close();
        }


        public string cariRapGoster(bool mailGitsin = false)
        {
            try
            {

                string basTar = "2000-01-01";
                string bitTar = "3000-01-01";

                //                string query = @"select Chrk_Cari as CariId,Cari_Ad as Ad,Cari_Soyad as Soyad,isnull(sum(Chrk_Borc-Chrk_Alacak),0) as Bakiye,10 as topHepsi from Pos_Carihrk as hrk 
                //left join Pos_Cari as cari on CONVERT(varchar(500), cari.Cari_Id)=hrk.Chrk_Cari 
                //where Chrk_Tarih between '" + basTar + @"' and '" + bitTar + @"' 
                //group by Cari_Ad,Cari_Soyad,Chrk_Cari";

                // 22.04.2025 de değiştirildi
                string query = $@"SELECT 
    Chrk_Cari AS CariId,
    (CASE 
        WHEN Cari_Tip = 'C' THEN 'Cari - ' + Cari_Ad
        WHEN Cari_Tip = 'O' THEN 'Ödenmez - ' + Cari_Ad
        ELSE Cari_Ad
     END) AS Ad,
    Cari_Soyad AS Soyad,
    ISNULL(SUM(Chrk_Borc - Chrk_Alacak), 0) AS Bakiye,
    10 AS topHepsi  
FROM 
    Pos_Carihrk AS hrk 
LEFT JOIN 
    Pos_Cari AS cari 
    ON CONVERT(VARCHAR(500), cari.Cari_Id) = hrk.Chrk_Cari 
WHERE 
    Chrk_Tarih BETWEEN '{basTar}' AND '{bitTar}' 
GROUP BY 
    Chrk_Cari,
    Cari_Tip,
    Cari_Ad,
    Cari_Soyad
";

                DataTable dataTable = dbtools.SelectTableR(query);

                decimal toplam = 0;
                if (dataTable != null && dataTable.Rows.Count > 0)
                    foreach (DataRow row in dataTable.Rows)
                    {
                        toplam += Convert.ToDecimal(row["Bakiye"].ToString());
                    }

                CariBakiyeKontrolRapor rapor = new CariBakiyeKontrolRapor();
                rapor.txtToplamBakiye.Text = "";
                rapor.DataSource = dataTable;
                rapor.txtTarih.Text = DateTime.Now.ToString("dd.MM.yyyy");
                rapor.txtDepAd.Text = Departman.Dep_Adi;
                rapor.txtToplamBakiye.Text = toplam.ToString();



                string klasor = "CariRapor";
                if (!Directory.Exists(klasor))
                {
                    Directory.CreateDirectory(klasor);
                }

                string path = klasor + "\\" + DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss") + ".pdf";

                gridControlCariRap3.DataSource = dataTable;

                gridviewCountYaz(gridViewCariRap3);

                gridViewCariRap3.ExportToPdf(path);

                if (mailGitsin)
                {
                    // Mail_Gonder(path);
                }
                else
                {
                    rapor.ShowPreview();
                }

                return path;
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError("Rapor_Sec", "btnCariRap3Listele_Click", "", ex);
            }

            return "";
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        public void Mail_Gonder(string fileName)
        {
            DataTable dt = dbtools.SelectTable("select ISNULL(Mail_Gonder,0) as Mail_Gonder,Mail_Isim,Mail_Adres,Mail_Parola,Mail_Host,Mail_Port,Mail_SSL, "
                                     + " Mail_Alici1,Mail_Alici2,Mail_Alici3,Mail_Alici4,Mail_Alici5, "
                                     + " isnull(Mail_Odeme_Tip,0) as Mail_Odeme_Tip,isnull(Mail_Servis_Paylari,0) as Mail_Servis_Paylari,isnull(Mail_Cari_Ozet,0) as Mail_Cari_Ozet, "
                                     + " isnull(Mail_Odenmez_Ozet,0) as Mail_Odenmez_Ozet,isnull(Mail_Malz_Ozet,0) as Mail_Malz_Ozet,isnull(Mail_Ana_Ozet,0) as Mail_Ana_Ozet, "
                                     + " isnull(Mail_Alt_Ozet,0) as Mail_Alt_Ozet,isnull(Mail_Iptal_Ozet,0) as Mail_Iptal_Ozet, "
                                     + " Mail_Alici6,Mail_Alici7,Mail_Alici8,Mail_Alici9,Mail_Alici10 from Pos_Mail WITH(NOLOCK) where Mail_Id = 1");
            if (dt.Rows.Count > 0)
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

                    MailMessage ePosta = new MailMessage();

                    ePosta.Attachments.Add(new Attachment(fileName));

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
                    ePosta.Subject = "CARİ BAKİYE KONTROL RAPOR " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                    ePosta.IsBodyHtml = true;
                    ePosta.Body = ePosta.Subject;


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
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Gun_Sonu, Log.Log_Islem.Kaydet, DateTime.Now.ToString("dd.MM.yyyy") + " Mail Gönderim Sırasında Hata Oldu...", "", "");
                    MessageBox.Show(res_man.GetString("Mail Gönderilemedi...") + "\n" + err.Message, res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }



        public void gridviewCountYaz(GridView grid)
        {
            if (grid.Columns.Count > 0)
            {
                grid.Columns[0].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                grid.Columns[0].SummaryItem.FieldName = grid.Columns[0].FieldName;
                grid.Columns[0].SummaryItem.DisplayFormat = "{0:n0}";
                grid.UpdateTotalSummary();
            }
        }


        public static string MyClass = "Rapor_Sec";
        private void btn_GunsonuMail_Click(object sender, EventArgs e)
        {
            try
            {
                Rapor_Tarih r = new Rapor_Tarih();
                r.ShowDialog();

                if (r.cikis == false)
                {
                    return;
                }

                string path = cariRapGoster(true);

                Gun_Sonu g = new Gun_Sonu();
                g.Mail_Gonder(r.date_Tarih1.DateTime.Date, Convert.ToString(r.lookUpEdit1.EditValue), Convert.ToString(r.chkCombo_Sube.EditValue), atachmentPath: path);

                this.Close();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btn_GunsonuMail_Click", "", ex);
            }

        }
    }
}