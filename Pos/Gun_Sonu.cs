using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Pos.Class;
using Pos.Forms;
using Pos.Print;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Gun_Sonu : DevExpress.XtraEditors.XtraForm
    {
        public Gun_Sonu()
        {
            InitializeComponent();
        }

        private void Gun_Sonu_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            dateTarih.DateTime = Param.Tarih;
            gridyenile();
            //chk_SubeGonder.Checked = Param.Param_SubeGonder;
            //chk_SubeGonder.Visible = Param.Param_SubeGonder;
        }

        private void gridyenile()
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 12);
            com.Parameters.AddWithValue("@Tarih1", Param.Tarih.ToString("yyyy-MM-dd"));
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            gridControl1.DataSource = dt;
        }

        private void btn_Kulara_Click(object sender, EventArgs e)
        {
            Klavye2 klv = new Klavye2();
            klv.ShowDialog();
            txt_Kulsifre.Text = klv.yazi;
        }

        private void btn_Gunsonusifre_Click(object sender, EventArgs e)
        {
            Klavye2 klv = new Klavye2();
            klv.ShowDialog();
            txt_Gunsonusifre.Text = klv.yazi;
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        public void loadingAc()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitFormRmos), true, true, false);
        }

        public void loadingKapat()
        {
            SplashScreenManager.CloseForm(false);
        }

        private void btn_Gunsonu_Click(object sender, EventArgs e)
        {
            if (User.P_Sifre != txt_Kulsifre.Text)
            {
                MessageBox.Show(res_man.GetString("Kullanıcı Şifreniz Yanlış..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txt_Gunsonusifre.Text.ToUpper() != "GUNSONU")
            {
                MessageBox.Show(res_man.GetString("Günsonu Şifreniz Yanlış..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dt2 = dbtools.SelectTable("SELECT * FROM Cst_Recete_Satis where Rsat_Durum = 'A' and Rsat_Tarih = '" + Param.Tarih + "' ");
            if (dt2.Rows.Count > 0 && !Param.Param_Gunsonu_Aktar)
            {
                MessageBox.Show(res_man.GetString("Kapatılmamış Çekler Bulunuyor. Lütfen Bütün Çeklerin Kapalı Olduğuna Emin Olun..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dateTarih.DateTime.AddDays(1).Date > Convert.ToDateTime(dbtools.DegerGetir("select getdate()")).Date)
            {
                var cvp = MessageBox.Show(res_man.GetString("Yeni tarih Sistem Server tarihinden Büyük Olamaz!!!!") + "\n" + res_man.GetString("Devam etmek İstiyor musunuz ?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (cvp == DialogResult.No)
                {
                    return;
                }
            }

            Rapor_Tarih r = new Rapor_Tarih();
            r.ShowDialog();


            if (r.cikis == false)
            {
                return;
            }

            gunsonuAl(dt2, r);

        }

        public void gunsonuAl(DataTable dt2, Rapor_Tarih r)
        {
            try
            {
                loadingAc();
                Gun_Sonu g = new Gun_Sonu();
                //g.Mail_Gonder(r.date_Tarih1.DateTime.Date, Convert.ToString(r.lookUpEdit1.EditValue));

                //this.Close();

                dbtools.execcmd("UPDATE Pos_Param SET Param_Tarih = '" + dateTarih.DateTime.AddDays(1) + "'  where Param_Id = '1'");

                StatikSinif.siranosifirla();

                // açık paket masaları kapatmak için eklendi 11.02.2022
                dbtools.execcmd("update Pos_Masa set Masa_Durum='0',Masa_NFC='0',Masa_Ozel=NULL where Masa_Paket='1' or Masa_Paket='true'");


                if (dt2.Rows.Count > 0 && Param.Param_Gunsonu_Aktar)
                {
                    dbtools.execcmd("UPDATE Cst_Recete_Satis set Rsat_Tarih = '" + dateTarih.DateTime.AddDays(1) + "' where Rsat_Durum = 'A' and Convert(date,Rsat_Tarih,105) = '" + Param.Tarih.ToString("yyyy-MM-dd") + "'");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Gun_Sonu, Log.Log_Islem.Duzelt, "GUN SONU. Acik Cekler Yeni Güne Transfer Oldu", "", "");
                }

                if (chk_Kurlar.Checked)
                {
                    dovizKaydet("E");
                    dovizKaydet("M");
                }

                if (Param.Param_SatisTabloAktif)
                {
                    Sube2Merkez a = new Sube2Merkez();
                    a.GonderSatis();
                    a.IptalGonder();

                    //a.satirlariOlustur(dateTarih);
                }


                //if (chk_SubeGonder.Checked == true)
                //{               
                //    string SubeKodu = dbtools.DegerGetir("Select MAX(Rsat_Sube) From Cst_Recete_Satis Where Rsat_Departman = '" + Departman.Dep_Kodu + "' and Convert(date,Rsat_Tarih) = '" + dateTarih.DateTime.Date + "' group by Rsat_Sube");

                //    SubedenMerkeze(Departman.Dep_Kodu, dateTarih.DateTime, SubeKodu);
                //}



                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Gun_Sonu, Log.Log_Islem.Kaydet, "GUN SONU... YENI TARIH : " + dateTarih.DateTime.AddDays(1).ToString("dd.MM.yyyy"), "", "");


                string cariBakiyeKontrolPath = cariRapGoster(true);
               var muhrapor1 = muhasebeRapor(true);
                //Mail Gönder

                loadingKapat();


                Mail_Gonder(r.date_Tarih1.DateTime.Date, Convert.ToString(r.lookUpEdit1.EditValue), Convert.ToString(r.chkCombo_Sube.EditValue), atachmentPath: cariBakiyeKontrolPath, muhRapor: muhrapor1);

                StatikSinif.shrinkData();

                //MessageBox.Show(res_man.GetString("Günsonu Işlemi Tamamlandı...") + "\n" + res_man.GetString("Programı Kapatıp Yeniden Programa Giriş Yapmanız Gerekmektedir..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);



                string mesaj = res_man.GetString("Günsonu Işlemi Tamamlandı...") + "\n" + res_man.GetString("Programı Kapatıp") + "\n" + res_man.GetString("Yeniden Programa Giriş Yapmanız Gerekmektedir...");

                UyariForm uyariForm = new UyariForm(mesaj);
                uyariForm.TopMost = true;
                uyariForm.ShowDialog();


                if (Param.Param_GetirOtomatikOnay)
                {
                    dbtools.execcmdRMesajsiz(Sabitler.cst_satis_index);
                    string query = "ALTER INDEX ALL ON dbo.Cst_Recete_Satis REBUILD;";
                    dbtools.execcmdR(query);
                }

                tumverileriSil();
                Application.Exit();

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "gunsonuAl", "", ex);
            }

            loadingKapat();
        }


        public void tumverileriSil()
        {
            try
            {
                if (Param.Param_AcilisCekSil)
                {
                    var cvp = MessageBox.Show("TÜM VERİLER SIFIRLANACAK EMİN MİSİNİZ ?", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (cvp == DialogResult.Yes)
                    {
                        dbtools.execcmdR("truncate table Cst_Recete_Satis");
                        dbtools.execcmdR("truncate table Pos_Log");
                        dbtools.execcmdR("truncate table Cst_Satis_Ipt");
                        dbtools.execcmdR("update Muh_Sirket set Sirket_Cfisno=0 ");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public string cariRapGoster(bool mailGitsin = true)
        {
            try
            {

                string basTar = "2000-01-01";
                string bitTar = "3000-01-01";

                string query = @"select Chrk_Cari as CariId,Cari_Ad as Ad,Cari_Soyad as Soyad,isnull(sum(Chrk_Borc-Chrk_Alacak),0) as Bakiye,10 as topHepsi from Pos_Carihrk as hrk 
left join Pos_Cari as cari on CONVERT(varchar(500), cari.Cari_Id)=hrk.Chrk_Cari 
where Chrk_Tarih between '" + basTar + @"' and '" + bitTar + @"' 
group by Cari_Ad,Cari_Soyad,Chrk_Cari";

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


        public MuhasebeRapor  muhasebeRapor(bool mailGitsin = true)
        {
            MuhasebeRapor rapor = new MuhasebeRapor();

            try
            {
                string tarih = Param.Tarih.ToString("yyyy-MM-dd");
                string query = @"declare @Fis_Tutar decimal(18,2) = (select SUM(Satis.Rsat_Tutar)   
FROM Cst_Recete_Satis as satis WITH(NOLOCK) 
LEFT JOIN Pos_Kodlar as  kodlar WITH(NOLOCK) ON Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' and Pkod_Ozelkod <> '4'
where  Rsat_Tarih='" + tarih + @"' AND Satis.Rsat_Ba = 'B' )  declare @Katsayi decimal(18,8) = ((select SUM(ISNULL(Rsat_Tutar,0)) as Tutar from Cst_Recete_Satis 
WITH(NOLOCK)  LEFT JOIN Pos_Kodlar as  kodlar 
WITH(NOLOCK) ON Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' 
where Rsat_Tarih='" + tarih + @"' and Pkod_Ozelkod <> '4' and Rsat_Ba = 'A'  group by Pkod_Fatura) / @Fis_Tutar )  
if @Katsayi = 0 begin set @Katsayi = 1 end  SELECT MIN(convert(date,Rsat_Tarih)) as Rsat_Tarih,
Kodlar_Ad + ' BEDELI' as Aciklama,
MIN(Rsat_Kdvoran) as Rec_Kdv,      
((SUM(Rsat_Tutar) * 100 / (100 + MIN(Rsat_Kdvoran))) * MIN(Rsat_Kdvoran) / 100) * @Katsayi as Kdv,       
(SUM(Rsat_Tutar) * 100 / (100 + MIN(Rsat_Kdvoran))) * @Katsayi as Net,      
(SUM(Rsat_Tutar)) * @Katsayi as Tutar
FROM Cst_Recete_Satis WITH(NOLOCK)      
LEFT JOIN Cst_Recete WITH(NOLOCK) on Rec_Genelkod = Rsat_Recete      
LEFT JOIN Stok_Kodlar WITH(NOLOCK) on Rec_Urungrup = Kodlar_Kod AND Kodlar_Sinif = '10'  
WHERE Rsat_Tarih='" + tarih + @"' AND Rsat_Ba = 'B' and Rsat_Satistip<>'O'
GROUP BY Kodlar_Ad  ORDER BY Kodlar_Ad desc";

                DataTable dataTable = dbtools.SelectTableR(query);

                decimal brutToplam = 0;
                decimal netToplam = 0;
                if (dataTable != null && dataTable.Rows.Count > 0)
                    foreach (DataRow row in dataTable.Rows)
                    {
                        brutToplam += Convert.ToDecimal(row["Tutar"].ToString());
                        netToplam += Convert.ToDecimal(row["Net"].ToString());
                    }

                rapor.DataSource = dataTable;
                rapor.txtTarih.Text = DateTime.Now.ToString("dd.MM.yyyy");
                rapor.txtDepAd.Text = Departman.Dep_Adi;
                //rapor.txtBrutToplam.Text = brutToplam.ToString();
                //rapor.txtNetToplam.Text = netToplam.ToString();

                var istatis = dbtools.SelectTableR("exec Pos_Satis_Istatistik @Tarih1='" + tarih + "', @Tarih2='" + tarih + "',@TekDepartman='" + Departman.Dep_Kodu + "',@Tahsilat=1");
                string odemeler = "";

                if (istatis != null && istatis.Rows.Count > 0)
                {
                    bool ilksatir = false;
                    foreach (DataRow item in istatis.Rows)
                    {
                        if (ilksatir == false)
                        {
                            ilksatir = true;
                            continue;
                        }

                        string ad = item["Ad"].ToString();
                        string tutar = item["Tutar"].ToString();
                       
                        int kalanbosluk = 15- ad.Length;
                        string bosluk = "";
                        for (int i = 0; i < kalanbosluk; i++)
                        {
                            bosluk = bosluk + " ";
                        }
                        int sonbosluk = bosluk.Length;
                        odemeler = odemeler + ad + bosluk + tutar + "\n";
                    }
                }
                odemeler = odemeler + "\nBRÜT TOPLAM :  " + brutToplam+"\n" + "NET TOPLAM  :  " + netToplam;
                rapor.txtTumOdeme.Text = odemeler;

                string klasor = "CariRapor";
                if (!Directory.Exists(klasor))
                {
                    Directory.CreateDirectory(klasor);
                }

                string path = klasor + "\\" + DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss") + "_muh.pdf";

                gridControlMuh.DataSource = dataTable;

                gridviewCountYaz(gridViewMuh);

                rapor.ExportToPdf(path);

                //gridViewMuh.ExportToPdf(path);

                if (mailGitsin)
                {
                    // Mail_Gonder(path);
                }
                else
                {
                    rapor.ShowPreview();
                }


               
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError("Rapor_Sec", "muhasebeRapor", "", ex);
            }

            return rapor;
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
        public void dovizKaydet(string Kurlar_Cesit)
        {
            try
            {


                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Kurlar_Liste";
                com.Parameters.AddWithValue("@Tip", 1);
                com.Parameters.AddWithValue("@Kurlar_Tarih", dateTarih.DateTime.Date.ToString("yyyy-MM-dd"));
                com.Parameters.AddWithValue("@Kurlar_Cesit", Kurlar_Cesit);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (con.State != ConnectionState.Closed) con.Close();

                SqlConnection con2 = dbtools.conn;
                if (con2.State == ConnectionState.Closed) con.Open();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con2;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Kurlar_Kaydet";
                    cmd.Parameters.AddWithValue("@Kurlar_Cesit", Kurlar_Cesit);
                    cmd.Parameters.AddWithValue("@Kurlar_Tarih", dateTarih.DateTime.AddDays(1).Date);
                    cmd.Parameters.AddWithValue("@Kurlar_Kodu", Convert.ToString(dt.Rows[i]["Mkodlar_Kod"]));
                    cmd.Parameters.AddWithValue("@Doviz_Alis", Convert.ToString(dt.Rows[i]["Doviz_Alis"]).Trim().Replace(",", "."));
                    cmd.Parameters.AddWithValue("@Doviz_Satis", Convert.ToString(dt.Rows[i]["Doviz_Satis"]).Trim().Replace(",", "."));
                    cmd.Parameters.AddWithValue("@Efektif_Alis", Convert.ToString(dt.Rows[i]["Efektif_Alis"]).Trim().Replace(",", "."));
                    cmd.Parameters.AddWithValue("@Efektif_Satis", Convert.ToString(dt.Rows[i]["Efektif_Satis"]).Trim().Replace(",", "."));

                    cmd.ExecuteNonQuery();
                }
                if (con2.State != ConnectionState.Closed) con2.Close();

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "dovizKaydet", "", ex);
            }
        }

        public string MyClass = "Gun_Sonu";


        public XtraReport CreateReport(List<XtraReport> raporList)
        {
            XtraReport output = new XtraReport();
            // output.CreateDocument(false);
            foreach (var report in raporList)
            {
                report.CreateDocument(false);
                output.Pages.AddRange(report.Pages);
            }
            if (output == null) output = new XtraReport();
            output.PrintingSystem.ContinuousPageNumbering = true;
            return output;
        }


        public void Mail_Gonder(DateTime tarih, string Departman, string Sube, string atachmentPath = "", MuhasebeRapor muhRapor=null)
        {

            try
            {
                DataTable dt = dbtools.SelectTable("select ISNULL(Mail_Gonder,0) as Mail_Gonder,Mail_Isim,Mail_Adres,Mail_Parola,Mail_Host,Mail_Port,Mail_SSL, "
                                            + " Mail_Alici1,Mail_Alici2,Mail_Alici3,Mail_Alici4,Mail_Alici5, "
                                            + " isnull(Mail_Odeme_Tip,0) as Mail_Odeme_Tip,isnull(Mail_Servis_Paylari,0) as Mail_Servis_Paylari,isnull(Mail_Cari_Ozet,0) as Mail_Cari_Ozet, "
                                            + " isnull(Mail_Odenmez_Ozet,0) as Mail_Odenmez_Ozet,isnull(Mail_Malz_Ozet,0) as Mail_Malz_Ozet,isnull(Mail_Ana_Ozet,0) as Mail_Ana_Ozet, "
                                            + " isnull(Mail_Alt_Ozet,0) as Mail_Alt_Ozet,isnull(Mail_Iptal_Ozet,0) as Mail_Iptal_Ozet, "
                                            + " Mail_Alici6,Mail_Alici7,Mail_Alici8,Mail_Alici9,Mail_Alici10 from Pos_Mail WITH(NOLOCK) where Mail_Id = 1");
                if (dt.Rows.Count > 0)
                {
                    bool Mail_Gonder = Convert.ToBoolean(dt.Rows[0]["Mail_Gonder"]);
                    if (Mail_Gonder == true)
                    {

                        #region
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
                        #endregion
                        try
                        {
                            System.Threading.Thread.Sleep(1 * 1000);
                            Application.DoEvents();
                            Print.Rapor_Gunsonu gunsonu = Rapor_Gunsonu_Pr(tarih, Departman, Sube);
                            List<XtraReport> raporlar = new List<XtraReport>();

                            XtraReport gunsonuSon = new XtraReport();
                            if (muhRapor != null)
                            {
                                raporlar.Add(gunsonu);
                                raporlar.Add(muhRapor);
                                gunsonuSon = CreateReport(raporlar);
                                //Attachment bakiyeKontrol = new Attachment(muhRapor);
                                //bakiyeKontrol.Name = "MuhasebeRapor.pdf";
                                //ePosta.Attachments.Add(bakiyeKontrol);
                            }

                            MemoryStream mem = new MemoryStream();

                            if (muhRapor!=null)
                            {
                                gunsonuSon.ExportToPdf(mem);
                            }
                            else
                            {
                                gunsonu.ExportToPdf(mem);
                            }

                            mem.Seek(0, System.IO.SeekOrigin.Begin);
                            Attachment att = new Attachment(mem, "Gunsonu.pdf", "application/pdf");



                            Print.Rapor_Gunsonu2 gunsonu2 = Rapor_Gunsonu2_Pr(tarih, Sube);
                            MemoryStream mem2 = new MemoryStream();
                            gunsonu2.ExportToPdf(mem2);
                            mem2.Seek(0, System.IO.SeekOrigin.Begin);
                            Attachment att2 = new Attachment(mem2, "Gunsonu2.pdf", "application/pdf");


                            FisPr pr = new FisPr();
                            string kasaRapor = pr.KasaGunlukOzetString(tarih, tarih);
                            Rapor_Kasa raporKasa = new Rapor_Kasa();
                            raporKasa.xrLabel1.Text = kasaRapor;
                            raporKasa.txtTarih.Text = "Tarih :" + Param.Tarih.ToString("dd.MM.yyyy");
                            MemoryStream memKasa = new MemoryStream();
                            raporKasa.ExportToPdf(memKasa);
                            memKasa.Seek(0, System.IO.SeekOrigin.Begin);
                            Attachment attKasa = new Attachment(memKasa, "GunsonuKasa.pdf", "application/pdf");

                            //Print.Rapor_Gunsonu3 gunsonu3 = Rapor_Gunsonu3_Pr(tarih);
                            //MemoryStream mem3 = new MemoryStream();
                            //gunsonu3.ExportToPdf(mem3);
                            //mem3.Seek(0, System.IO.SeekOrigin.Begin);
                            //Attachment att3 = new Attachment(mem3, "Gunsonu3.pdf", "application/pdf");

                            //Print.Rapor_Gunsonu4 gunsonu4 = Rapor_Gunsonu4_Pr(tarih);
                            //MemoryStream mem4 = new MemoryStream();
                            //gunsonu4.ExportToPdf(mem4);
                            //mem4.Seek(0, System.IO.SeekOrigin.Begin);
                            //Attachment att4 = new Attachment(mem4, "Gunsonu4.pdf", "application/pdf");

                            //Print.Rapor_Gunsonu5 gunsonu5 = Rapor_Gunsonu5_Pr(tarih);
                            //MemoryStream mem5 = new MemoryStream();
                            //gunsonu5.ExportToPdf(mem5);
                            //mem5.Seek(0, System.IO.SeekOrigin.Begin);
                            //Attachment att5 = new Attachment(mem5, "Gunsonu5.pdf", "application/pdf");

                            //var report3 = Rapor_Gunsonu3_Pr(Param.Tarih);
                            //report3.CreateDocument();
                            //var report4 = Rapor_Gunsonu4_Pr(Param.Tarih);
                            //report4.CreateDocument();
                            //var report5 = Rapor_Gunsonu5_Pr(Param.Tarih);
                            //report5.CreateDocument();
                            //var report6 = Rapor_Gunsonu6_Pr(Param.Tarih);
                            //report6.CreateDocument();

                            var report3 = Rapor_Gunsonu3_Pr(tarih, Sube);
                            report3.CreateDocument();
                            var report4 = Rapor_Gunsonu4_Pr(tarih, Sube);
                            report4.CreateDocument();
                            var report5 = Rapor_Gunsonu5_Pr(tarih, Sube);
                            report5.CreateDocument();
                            var report6 = Rapor_Gunsonu6_Pr(tarih, Sube);
                            report6.CreateDocument();


                            report3.Pages.AddRange(report4.Pages);
                            report3.Pages.AddRange(report5.Pages);
                            report3.Pages.AddRange(report6.Pages);

                            report3.PrintingSystem.ContinuousPageNumbering = true;

                            ReportPrintTool printTool = new ReportPrintTool(report3);
                            MemoryStream mem3 = new MemoryStream();
                            printTool.PrintingSystem.ExportToPdf(mem3);
                            mem3.Seek(0, System.IO.SeekOrigin.Begin);
                            Attachment att3 = new Attachment(mem3, "Gunsonu3.pdf", "application/pdf");


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
                            ePosta.Subject = tarih.Date.ToLongDateString() + " Pos Satış Listesi";
                            ePosta.Attachments.Add(att);
                            ePosta.Attachments.Add(att2);
                            ePosta.Attachments.Add(att3);
                            ePosta.Attachments.Add(attKasa);

                            if (atachmentPath != "")
                            {
                                Attachment bakiyeKontrol = new Attachment(atachmentPath);
                                bakiyeKontrol.Name = "CariBakiyeKontrol.pdf";
                                ePosta.Attachments.Add(bakiyeKontrol);
                            }

                            //if (muhRapor != "")
                            //{
                            //    Attachment bakiyeKontrol = new Attachment(muhRapor);
                            //    bakiyeKontrol.Name = "MuhasebeRapor.pdf";
                            //    ePosta.Attachments.Add(bakiyeKontrol);
                            //}

                            string mailbody = Mail_Detay(tarih);

                            ePosta.IsBodyHtml = true;
                            ePosta.Body = mailbody;

                            SmtpClient ss = new SmtpClient(Mail_Host, Convert.ToInt32(Mail_Port));
                            ss.EnableSsl = Mail_SSL;
                            ss.DeliveryMethod = SmtpDeliveryMethod.Network;
                            ss.UseDefaultCredentials = false;
                            ss.Credentials = new NetworkCredential(Mail_Adres, Mail_Parola);
                            ss.Send(ePosta);

                            RHMesaj.MyMessageInformation("Mail gönderme başarılı...");
                        }
                        catch (Exception err)
                        {
                            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Gun_Sonu, Log.Log_Islem.Kaydet, tarih.Date.ToString("dd.MM.yyyy") + " Mail Gönderim Sırasında Hata Oldu...", "", "");
                            MessageBox.Show(res_man.GetString("Mail Gönderilemedi...") + "\n" + err.Message, res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Mail_Gonder", "", ex);
            }


        }
        public class RaporTip0dan17yeTransfer
        {
            public decimal gunToplamKdv { set; get; }
            public decimal gunToplamIndirim { set; get; }
            public decimal gunToplamBrut { set; get; }
            public decimal gunToplamNet { set; get; }

            public decimal ayToplamKdv { set; get; }
            public decimal ayToplamIndirim { set; get; }
            public decimal ayToplamBrut { set; get; }
            public decimal ayToplamNet { set; get; }

            public decimal yilToplamKdv { set; get; }
            public decimal yilToplamIndirim { set; get; }
            public decimal yilToplamBrut { set; get; }
            public decimal yilToplamNet { set; get; }
        }

        public RaporTip0dan17yeTransfer getTransfer(DateTime tarih, string Departman)
        {
            /////////////////////GÜN RAPOR/////////////////////////
            RaporTip0dan17yeTransfer transfer = new RaporTip0dan17yeTransfer();

            string query2 = @" exec Pos_Satis_Rapor @Rapor_Tipi=0,@Tarih1='" + tarih.Date.ToString("yyyy-MM-dd") + @"',@Tarih2='" + tarih.Date.ToString("yyyy-MM-dd") + @"',@Satis_Tip=N'S, O, P, V, N',@Departman=N'" + Departman + @"',@Saat_Filtre=0,@Saat1=N'00:00:00',@Saat2=N'23:59:59',@Acik=1,@Kapali=1,@Kullanici=N'1',@GarsonKasiyer=N''";

            DataTable dt2 = dbtools.SelectTableR(query2);

            foreach (DataRow item in dt2.Rows)
            {
                transfer.gunToplamKdv += Convert.ToDecimal(item["Rsat_Kdv"].ToString());
                transfer.gunToplamBrut += Convert.ToDecimal(item["Rsat_Brut"].ToString());
                transfer.gunToplamIndirim += Convert.ToDecimal(item["Rsat_Ind"].ToString());
            }

            transfer.gunToplamNet = transfer.gunToplamBrut - transfer.gunToplamIndirim - transfer.gunToplamKdv;

            /////////////////////AY RAPOR/////////////////////////

            string ayilIlkGunu = new DateTime(tarih.Year, tarih.Month, 1).ToString("yyyy-MM-dd");


            query2 = @" exec Pos_Satis_Rapor @Rapor_Tipi=0,@Tarih1='" + ayilIlkGunu + @"',@Tarih2='" + tarih.Date.ToString("yyyy-MM-dd") + @"',@Satis_Tip=N'S, O, P, V, N',@Departman=N'" + Departman + @"',@Saat_Filtre=0,@Saat1=N'00:00:00',@Saat2=N'23:59:59',@Acik=1,@Kapali=1,@Kullanici=N'1',@GarsonKasiyer=N''";

            dt2 = dbtools.SelectTableR(query2);

            foreach (DataRow item in dt2.Rows)
            {
                transfer.ayToplamKdv += Convert.ToDecimal(item["Rsat_Kdv"].ToString());
                transfer.ayToplamBrut += Convert.ToDecimal(item["Rsat_Brut"].ToString());
                transfer.ayToplamIndirim += Convert.ToDecimal(item["Rsat_Ind"].ToString());
            }


            transfer.ayToplamNet = transfer.ayToplamBrut - transfer.ayToplamIndirim - transfer.ayToplamKdv;

            /////////////////////YIL RAPOR/////////////////////////

            string yilinIlkGunu = tarih.Year + "-01-01";


            query2 = @" exec Pos_Satis_Rapor @Rapor_Tipi=0,@Tarih1='" + yilinIlkGunu + @"',@Tarih2='" + tarih.Date.ToString("yyyy-MM-dd") + @"',@Satis_Tip=N'S, O, P, V, N',@Departman=N'" + Departman + @"',@Saat_Filtre=0,@Saat1=N'00:00:00',@Saat2=N'23:59:59',@Acik=1,@Kapali=1,@Kullanici=N'1',@GarsonKasiyer=N''";

            dt2 = dbtools.SelectTableR(query2);

            foreach (DataRow item in dt2.Rows)
            {
                transfer.yilToplamKdv += Convert.ToDecimal(item["Rsat_Kdv"].ToString());
                transfer.yilToplamBrut += Convert.ToDecimal(item["Rsat_Brut"].ToString());
                transfer.yilToplamIndirim += Convert.ToDecimal(item["Rsat_Ind"].ToString());
            }


            transfer.yilToplamNet = transfer.yilToplamBrut - transfer.yilToplamIndirim - transfer.yilToplamKdv;


            return transfer;
        }

        public DataTable toplamNetHesapla(DataTable dt, string gunAyYil)
        {
            double gunNetGelir = 0, gunToplamKdv = 0, gunToplamNet = 0;
            foreach (DataRow item in dt.Rows)
            {

                if (item[gunAyYil + "_Aciklama"].ToString().Equals("Net Gelir")) // "Gun_Aciklama"
                {
                    string aa = item[gunAyYil + "_Tutar"].ToString().Trim();
                    if (aa == "") aa = "0";
                    gunNetGelir = Convert.ToDouble(aa);
                }
                if (item[gunAyYil + "_Aciklama"].ToString().Equals("Toplam Kdv"))
                {
                    string aa = item[gunAyYil + "_Tutar"].ToString();
                    if (aa == "") aa = "0";
                    gunToplamKdv = Convert.ToDouble(aa);
                    gunToplamNet = gunNetGelir - gunToplamKdv;
                    break;
                }
            }
            foreach (DataRow item in dt.Rows)
            {
                if (item[gunAyYil + "_Aciklama"].ToString().Equals("Toplam Net"))
                {
                    item[gunAyYil + "_Tutar"] = gunToplamNet;
                    break;
                }
            }
            return dt;

        }

        DataTable dtDep = new DataTable();
        public Print.Rapor_Gunsonu Rapor_Gunsonu_Pr(DateTime tarih, string Departman, string Sube)
        {
            RaporTip0dan17yeTransfer transfer = getTransfer(tarih, Departman);

            string query = "EXEC Pos_Satis_Rapor @Rapor_Tipi = 17, @Tarih1 = '" + tarih.Date + "', @Departman = '" + Departman + "', @Sube = '" + Sube + "'";
            DataTable dt = dbtools.SelectTable(query);


            foreach (DataRow item in dt.Rows)
            {
                if (item["Gun_Aciklama"].ToString().Equals("Toplam Kdv"))
                {
                    item["Gun_Tutar"] = transfer.gunToplamKdv.ToString();
                }
                if (item["Gun_Aciklama"].ToString().Equals("Toplam Net"))
                {
                    item["Gun_Tutar"] = transfer.gunToplamNet.ToString();
                }

                if (item["Ay_Aciklama"].ToString().Equals("Toplam Kdv"))
                {
                    item["Ay_Tutar"] = transfer.ayToplamKdv.ToString();
                }
                if (item["Ay_Aciklama"].ToString().Equals("Toplam Net"))
                {
                    item["Ay_Tutar"] = transfer.ayToplamNet.ToString();
                }

                if (item["Yil_Aciklama"].ToString().Equals("Toplam Kdv"))
                {
                    item["Yil_Tutar"] = transfer.yilToplamKdv.ToString();
                }
                if (item["Yil_Aciklama"].ToString().Equals("Toplam Net"))
                {
                    item["Yil_Tutar"] = transfer.yilToplamNet.ToString();
                }
            }

            dt = toplamNetHesapla(dt, "Gun");
            dt = toplamNetHesapla(dt, "Ay");
            dt = toplamNetHesapla(dt, "Yil");

            Print.Rapor_Gunsonu gunsonu = new Print.Rapor_Gunsonu();
            gunsonu.DataSource = dt;

            gunsonu.lbl_SistemTarih.Text = tarih.Date.ToLongDateString();
            gunsonu.lbl_Restorant.Text = Param.Tesis_Adi;
            gunsonu.lbl_Kullanici.Text = User.P_Ad + " " + User.P_Soyad;
            gunsonu.lbl_RaporTarih.Text = Convert.ToDateTime(dbtools.DegerGetir("select getdate()")).ToString("dd.MM.yyyy HH:ss:mm");

            gunsonu.lbl_Gunluk_Aciklama.Text = "[Gun_Aciklama]".ToString();
            gunsonu.lbl_Gunluk_Tutar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Gun_Tutar", "{0:n2}") });
            gunsonu.lbl_Gunluk_Tutar.Text = "[Gun_Tutar]".ToString();

            gunsonu.lbl_Aylik_Aciklama.Text = "[Ay_Aciklama]".ToString();
            gunsonu.lbl_Aylik_Tutar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Ay_Tutar", "{0:n2}") });
            gunsonu.lbl_Aylik_Tutar.Text = "[Ay_Tutar]".ToString();

            gunsonu.lbl_Yillik_Aciklama.Text = "[Yil_Aciklama]".ToString();
            gunsonu.lbl_Yillik_Tutar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Yil_Tutar", "{0:n2}") });
            gunsonu.lbl_Yillik_Tutar.Text = "[Yil_Tutar]".ToString();


            DataTable dtChartGun = dt.Clone();
            DataTable dtChartAy = dt.Clone();
            DataTable dtChartYil = dt.Clone();


            DataRow[] rowsGun = dt.Select("Gun_Tip = 16");
            DataRow[] rowsAy = dt.Select("Ay_Tip = 16");
            DataRow[] rowsYil = dt.Select("Yil_Tip = 16");
            foreach (var row in rowsGun) dtChartGun.ImportRow(row);
            foreach (var row in rowsAy) dtChartAy.ImportRow(row);
            foreach (var row in rowsYil) dtChartYil.ImportRow(row);



            Series seriesGun = new Series("seriesGun", ViewType.Doughnut);
            seriesGun.DataSource = dtChartGun;
            seriesGun.ArgumentScaleType = ScaleType.Qualitative;
            seriesGun.ArgumentDataMember = "Gun_Aciklama";
            seriesGun.ValueScaleType = ScaleType.Numerical;
            seriesGun.ValueDataMembers.AddRange(new string[] { "Gun_Tutar" });
            //seriesGun.LegendPointOptions.PointView = PointView.Argument;
            gunsonu.chart_OdemeGun.Series.Add(seriesGun);


            Series seriesAy = new Series("seriesAy", ViewType.Doughnut);
            seriesAy.DataSource = dtChartAy;
            seriesAy.ArgumentScaleType = ScaleType.Qualitative;
            seriesAy.ArgumentDataMember = "Ay_Aciklama";
            seriesAy.ValueScaleType = ScaleType.Numerical;
            seriesAy.ValueDataMembers.AddRange(new string[] { "Ay_Tutar" });
            //seriesAy.LegendPointOptions.PointView = PointView.Argument;
            gunsonu.chart_OdemeAy.Series.Add(seriesAy);


            Series seriesYil = new Series("seriesYil", ViewType.Doughnut);
            seriesYil.DataSource = dtChartYil;
            seriesYil.ArgumentScaleType = ScaleType.Qualitative;
            seriesYil.ArgumentDataMember = "Yil_Aciklama";
            seriesYil.ValueScaleType = ScaleType.Numerical;
            seriesYil.ValueDataMembers.AddRange(new string[] { "Yil_Tutar" });
            //seriesYil.LegendPointOptions.PointView = PointView.Argument;
            gunsonu.chart_OdemeYil.Series.Add(seriesYil);





            return gunsonu;



        }

        public Print.Rapor_Gunsonu2 Rapor_Gunsonu2_Pr(DateTime tarih, string Sube)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();


            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Istatistik";
            com.Parameters.AddWithValue("@Tarih1", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Tarih2", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Departman", true);
            com.Parameters.AddWithValue("@Kasiyer", true);
            com.Parameters.AddWithValue("@Garson", true);
            com.Parameters.AddWithValue("@Anagrup", true);
            com.Parameters.AddWithValue("@Grup", true);
            com.Parameters.AddWithValue("@Malzeme", true);
            com.Parameters.AddWithValue("@Saat", true);
            com.Parameters.AddWithValue("@Tahsilat", true);
            com.Parameters.AddWithValue("@Kisi", false);
            com.Parameters.AddWithValue("@Konum", true);
            com.Parameters.AddWithValue("@Paket", true);
            com.Parameters.AddWithValue("@SifirTutar", true);
            com.Parameters.AddWithValue("@UrunGrup", true);
            com.Parameters.AddWithValue("@UrunGrupNet", false);
            //com.Parameters.AddWithValue("@DepartmanKod", Depart);
            com.Parameters.AddWithValue("@Sube", Sube);
            com.Parameters.AddWithValue("@DovizTahsilat", true);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);


            Print.Rapor_Gunsonu2 gunsonu2 = new Print.Rapor_Gunsonu2();
            gunsonu2.DataSource = dt;

            gunsonu2.lbl_SistemTarih.Text = tarih.Date.ToLongDateString();
            gunsonu2.lbl_Restorant.Text = Param.Tesis_Adi;
            gunsonu2.lbl_Kullanici.Text = User.P_Ad + " " + User.P_Soyad;
            gunsonu2.lbl_RaporTarih.Text = Convert.ToDateTime(dbtools.DegerGetir("select getdate()")).ToString("dd.MM.yyyy HH:ss:mm");

            gunsonu2.lbl_Aciklama.Text = "[Ad]".ToString();
            gunsonu2.lbl_Miktar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Miktar", "{0:n2}") });
            gunsonu2.lbl_Miktar.Text = "[Miktar]".ToString();
            gunsonu2.lbl_Tutar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Tutar", "{0:n2}") });
            gunsonu2.lbl_Tutar.Text = "[Tutar]".ToString();


            return gunsonu2;
        }

        public Print.Rapor_Gunsonu3 Rapor_Gunsonu3_Pr(DateTime tarih, string Sube)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();


            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 14);
            com.Parameters.AddWithValue("@Tarih1", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Tarih2", tarih.Date.ToString("yyyy-MM-dd"));
            //com.Parameters.AddWithValue("@Departman", Departman);
            com.Parameters.AddWithValue("@Sube", Sube);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);


            Print.Rapor_Gunsonu3 gunsonu3 = new Print.Rapor_Gunsonu3();
            gunsonu3.DataSource = dt;

            gunsonu3.lbl_SistemTarih.Text = tarih.Date.ToLongDateString();
            gunsonu3.lbl_Restorant.Text = Param.Tesis_Adi;
            gunsonu3.lbl_Kullanici.Text = User.P_Ad + " " + User.P_Soyad;
            gunsonu3.lbl_RaporTarih.Text = Convert.ToDateTime(dbtools.DegerGetir("select getdate()")).ToString("dd.MM.yyyy HH:ss:mm");

            gunsonu3.lbl_Dep.Text = "[Departman]".ToString();
            gunsonu3.lbl_Urun.Text = "[Recete]".ToString();
            gunsonu3.lbl_Miktar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Miktar", "{0:n2}") });
            gunsonu3.lbl_Miktar.Text = "[Miktar]".ToString();
            gunsonu3.lbl_User.Text = "[Users]".ToString();
            gunsonu3.lbl_Neden.Text = "[Neden]".ToString();


            return gunsonu3;
        }

        public Print.Rapor_Gunsonu4 Rapor_Gunsonu4_Pr(DateTime tarih, string Sube)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();


            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 18);
            com.Parameters.AddWithValue("@Tarih1", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Tarih2", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Sube", Sube);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);


            Print.Rapor_Gunsonu4 gunsonu4 = new Print.Rapor_Gunsonu4();
            gunsonu4.DataSource = dt;

            gunsonu4.lbl_SistemTarih.Text = tarih.Date.ToLongDateString();
            gunsonu4.lbl_Restorant.Text = Param.Tesis_Adi;
            gunsonu4.lbl_Kullanici.Text = User.P_Ad + " " + User.P_Soyad;
            gunsonu4.lbl_RaporTarih.Text = Convert.ToDateTime(dbtools.DegerGetir("select getdate()")).ToString("dd.MM.yyyy HH:ss:mm");

            gunsonu4.lbl_Fisno.Text = "[Rsat_Fisno]".ToString();
            gunsonu4.lbl_Masa.Text = "[Rsat_Masa]".ToString();

            gunsonu4.lbl_IndirimTutar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "IndirimTutar", "{0:n2}") });
            gunsonu4.lbl_IndirimTutar.Text = "[IndirimTutar]".ToString();

            gunsonu4.lbl_NetTutar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Net", "{0:n2}") });
            gunsonu4.lbl_NetTutar.Text = "[Net]".ToString();

            gunsonu4.lbl_ToplamTutar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Toplam", "{0:n2}") });
            gunsonu4.lbl_ToplamTutar.Text = "[Toplam]".ToString();


            return gunsonu4;
        }

        public Print.Rapor_Gunsonu5 Rapor_Gunsonu5_Pr(DateTime tarih, string Sube)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();


            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 19);
            com.Parameters.AddWithValue("@Tarih1", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Tarih2", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Sube", Sube);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);


            Print.Rapor_Gunsonu5 gunsonu5 = new Print.Rapor_Gunsonu5();
            gunsonu5.DataSource = dt;

            gunsonu5.lbl_SistemTarih.Text = tarih.Date.ToLongDateString();
            gunsonu5.lbl_Restorant.Text = Param.Tesis_Adi;
            gunsonu5.lbl_Kullanici.Text = User.P_Ad + " " + User.P_Soyad;
            gunsonu5.lbl_RaporTarih.Text = Convert.ToDateTime(dbtools.DegerGetir("select getdate()")).ToString("dd.MM.yyyy HH:ss:mm");

            gunsonu5.lbl_Fisno.Text = "[Rsat_Fisno]".ToString();
            gunsonu5.lbl_Masa.Text = "[Rsat_Masa]".ToString();

            gunsonu5.lbl_IkramTutar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "IkramTutar", "{0:n2}") });
            gunsonu5.lbl_IkramTutar.Text = "[IkramTutar]".ToString();

            gunsonu5.lbl_Cari.Text = "[CariAdSoyad]".ToString();
            gunsonu5.lbl_Garson.Text = "[GarsonAd]".ToString();
            gunsonu5.lbl_Not.Text = "[Rsat_Not]".ToString();
            gunsonu5.lbl_Recete.Text = "[Rec_Ad]".ToString();


            return gunsonu5;
        }

        public Print.Rapor_Gunsonu6 Rapor_Gunsonu6_Pr(DateTime tarih, string Sube)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();


            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 20);
            com.Parameters.AddWithValue("@Tarih1", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Tarih2", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Sube", Sube);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);


            Print.Rapor_Gunsonu6 gunsonu6 = new Print.Rapor_Gunsonu6();
            gunsonu6.DataSource = dt;

            gunsonu6.lbl_SistemTarih.Text = tarih.Date.ToLongDateString();
            gunsonu6.lbl_Restorant.Text = Param.Tesis_Adi;
            gunsonu6.lbl_Kullanici.Text = User.P_Ad + " " + User.P_Soyad;
            gunsonu6.lbl_RaporTarih.Text = Convert.ToDateTime(dbtools.DegerGetir("select getdate()")).ToString("dd.MM.yyyy HH:ss:mm");

            gunsonu6.lbl_Tarih.Text = "[Log_Tarih]".ToString();
            gunsonu6.lbl_Fisno.Text = "[Log_FisNo]".ToString();
            gunsonu6.lbl_Urun.Text = "[Log_Urun]".ToString();

            gunsonu6.lbl_Miktar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Log_Miktar", "{0:n2}") });
            gunsonu6.lbl_Miktar.Text = "[Log_Miktar]".ToString();

            gunsonu6.lbl_Bilgisayar.Text = "[Log_Bilg]".ToString();
            gunsonu6.lbl_Garson.Text = "[GarsonAdSoyad]".ToString();
            gunsonu6.lbl_Aciklama.Text = "[Log_Aciklama]".ToString();
            gunsonu6.lbl_Neden.Text = "[Log_Neden]".ToString();

            return gunsonu6;
        }


        public Print.Rapor_Gunsonu7 Rapor_Gunsonu7_Pr(DateTime tarih, string Sube)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();


            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 30);
            com.Parameters.AddWithValue("@Tarih1", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Tarih2", tarih.Date.ToString("yyyy-MM-dd"));
            com.Parameters.AddWithValue("@Sube", Sube);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);


            Print.Rapor_Gunsonu7 gunsonu7 = new Print.Rapor_Gunsonu7();
            gunsonu7.DataSource = dt;

            gunsonu7.lbl_SistemTarih.Text = tarih.Date.ToLongDateString();
            gunsonu7.lbl_Restorant.Text = Param.Tesis_Adi;
            gunsonu7.lbl_Kullanici.Text = User.P_Ad + " " + User.P_Soyad;
            gunsonu7.lbl_RaporTarih.Text = Convert.ToDateTime(dbtools.DegerGetir("select getdate()")).ToString("dd.MM.yyyy HH:ss:mm");

            gunsonu7.lbl_Tarih.Text = "[Log_Tarih]".ToString();
            gunsonu7.lbl_Fisno.Text = "[Log_FisNo]".ToString();
            gunsonu7.lbl_Urun.Text = "[Log_Urun]".ToString();

            gunsonu7.lbl_Miktar.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Log_Miktar", "{0:n2}") });
            gunsonu7.lbl_Miktar.Text = "[Log_Miktar]".ToString();

            gunsonu7.lbl_Bilgisayar.Text = "[Log_Bilg]".ToString();
            gunsonu7.lbl_Garson.Text = "[GarsonAdSoyad]".ToString();
            gunsonu7.lbl_Aciklama.Text = "[Log_Aciklama]".ToString();
            gunsonu7.lbl_Neden.Text = "[Log_Neden]".ToString();

            return gunsonu7;
        }

        //private string Mail_Detay()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    DataTable dtMail = dbtools.SelectTable("select isnull(Mail_Odeme_Tip,0) as Mail_Odeme_Tip,isnull(Mail_Servis_Paylari,0) as Mail_Servis_Paylari,isnull(Mail_Cari_Ozet,0) as Mail_Cari_Ozet, "
        //                                    + " isnull(Mail_Odenmez_Ozet,0) as Mail_Odenmez_Ozet,isnull(Mail_Malz_Ozet,0) as Mail_Malz_Ozet,isnull(Mail_Ana_Ozet,0) as Mail_Ana_Ozet, "
        //                                    + " isnull(Mail_Alt_Ozet,0) as Mail_Alt_Ozet,isnull(Mail_Iptal_Ozet,0) as Mail_Iptal_Ozet from Pos_Mail WITH(NOLOCK) where Mail_Id = 1");
        //    if (dtMail.Rows.Count > 0)
        //    {
        //        bool Mail_Odeme = Convert.ToBoolean(dtMail.Rows[0]["Mail_Odeme_Tip"]);
        //        bool Mail_Servis = Convert.ToBoolean(dtMail.Rows[0]["Mail_Servis_Paylari"]);
        //        bool Mail_Cari = Convert.ToBoolean(dtMail.Rows[0]["Mail_Cari_Ozet"]);
        //        bool Mail_Odenmez = Convert.ToBoolean(dtMail.Rows[0]["Mail_Odenmez_Ozet"]);
        //        bool Mail_Malzeme = Convert.ToBoolean(dtMail.Rows[0]["Mail_Malz_Ozet"]);
        //        bool Mail_Anagrup = Convert.ToBoolean(dtMail.Rows[0]["Mail_Ana_Ozet"]);
        //        bool Mail_Altgrup = Convert.ToBoolean(dtMail.Rows[0]["Mail_Alt_Ozet"]);
        //        bool Mail_Iptal = Convert.ToBoolean(dtMail.Rows[0]["Mail_Iptal_Ozet"]);



        //        SqlConnection con = dbtools.conn;
        //        if (con.State == ConnectionState.Closed) con.Open();
        //        SqlCommand com = new SqlCommand();
        //        com.Connection = con;
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.CommandText = "Pos_XZ_Raporu";
        //        com.Parameters.AddWithValue("@Tarih1", dateTarih.DateTime.Date);
        //        com.Parameters.AddWithValue("@Odeme", Mail_Odeme);
        //        com.Parameters.AddWithValue("@Servis", Mail_Servis);
        //        com.Parameters.AddWithValue("@Cari", Mail_Cari);
        //        com.Parameters.AddWithValue("@Odenmez", Mail_Odenmez);
        //        com.Parameters.AddWithValue("@Mazleme", Mail_Malzeme);
        //        com.Parameters.AddWithValue("@Ana", Mail_Anagrup);
        //        com.Parameters.AddWithValue("@Ara", Mail_Altgrup);
        //        com.Parameters.AddWithValue("@Iptal", Mail_Iptal);
        //        com.Parameters.AddWithValue("@Yeni", 1);
        //        SqlDataAdapter da = new SqlDataAdapter(com);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        con.Close();

        //        //ResourceManager resManager = new ResourceManager("Pos.CSS", Assembly.GetExecutingAssembly());
        //        //string css = (string)resManager.GetObject("bootstrap_css");
        //        //string css2 = (string)resManager.GetObject("bootstrap_css2");
        //        //resManager.ReleaseAllResources();

        //        sb.Append("<html>");
        //        sb.Append("<head>");
        //        sb.Append("<style>");
        //        //sb.Append(css + " " + css2);
        //        sb.Append("</style>");
        //        sb.Append("</head>");
        //        sb.Append("<body>");
        //        for (int i = 0; i < dt.Rows.Count - 1; i++)
        //        {
        //            if (Convert.ToString(dt.Rows[i + 1]["Aciklama"].ToString()) == "---")
        //            {
        //                //Tablo Balşıllarını ekle tablo aç
        //                sb.Append("<div class=\"table-responsive\" style=\"width:300px;\">");
        //                sb.Append("<table class=\"table table-bordered\">");

        //                sb.Append("<thead>");
        //                sb.Append("<tr>");
        //                sb.Append("<th >" + dt.Rows[i]["Aciklama"].ToString() + "</th>");
        //                sb.Append("<th >Miktar</th>");
        //                sb.Append("<th >Tutar</th>");
        //                sb.Append("</tr>");
        //                sb.Append("</thead>");
        //                sb.Append("<tbody>");

        //                continue;
        //            }

        //            if (Convert.ToString(dt.Rows[i]["Aciklama"].ToString()) == "---")
        //            {
        //                continue;
        //            }

        //            if (Convert.ToString(dt.Rows[i]["Aciklama"].ToString()) == ".")
        //            {
        //                //tablo kapat
        //                sb.Append("</tbody>");
        //                sb.Append("</table>");
        //                sb.Append("</div>");
        //                continue;
        //            }

        //            sb.Append("<tr>");
        //            sb.Append("<td >" + dt.Rows[i]["Aciklama"].ToString() + "</td>");
        //            sb.Append("<td >" + dt.Rows[i]["Miktar"].ToString() + "</td>");
        //            sb.Append("<td >" + dt.Rows[i]["Tutar"].ToString() + "</td>");
        //            sb.Append("</tr>");
        //        }
        //        sb.Append("</tbody>");
        //        sb.Append("</table>");
        //        sb.Append("</div>");
        //        sb.Append("</body>");
        //        sb.Append("</html>");

        //    }

        //    return sb.ToString();
        //}

        private string Mail_Detay(DateTime tarih)
        {
            return tarih.Date.ToLongDateString() + " Tarihli Pos Satış Raporu Ektedir.";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Rapor_Gunsonu_Pr(dateTarih.DateTime, "", "").ShowPreview();
        }

        private void SubedenMerkeze(string depKodu, DateTime Tarih, string SubeKod)
        {
            DataTable dtSubeConn = dbtools.SelectTable("select * from Pos_Kodlar where PKod_Sinif= '27' and Pkod_MerkezSube = 'M'");
            if (dtSubeConn.Rows.Count <= 0) return;

            string subeConnectionString = "Data Source='" + Convert.ToString(dtSubeConn.Rows[0]["Pkod_Server"]) + "';Initial Catalog=" + Convert.ToString(dtSubeConn.Rows[0]["Pkod_Database"]) + "; Persist Security Info=True;uid='" + Convert.ToString(dtSubeConn.Rows[0]["Pkod_User"]) + "'; pwd='" + Convert.ToString(dtSubeConn.Rows[0]["Pkod_Password"]) + "'";

            try
            {
                //if (string.IsNullOrEmpty(SubeKod))
                //{
                //    MessageBox.Show(res_man.GetString("Merkez Kodu Geçilemez.");
                //    return;
                //}

                DataTable dtSube = dbtools.SelectTable(@"SELECT Rsat_Id from Cst_Recete_Satis as Cst  Where Rsat_Departman = '" + depKodu + "' and Convert(date,Rsat_Tarih) = '" + Tarih.Date + "' and Rsat_Durum = 'K'");

                if (dtSube.Rows.Count > 0)
                {
                    SqlConnection con = new SqlConnection(subeConnectionString);

                    for (int i = 0; i < dtSube.Rows.Count; i++)
                    {
                        DataTable dtSubeGonder = dbtools.SelectTable(@"SELECT * from Cst_Recete_Satis as Cst Where Rsat_Departman = '" + depKodu + "' and Rsat_Id = '" + Convert.ToInt32(dtSube.Rows[i]["Rsat_Id"]) + "'");

                        string jsonData = JsonConvert.SerializeObject(dtSubeGonder, Newtonsoft.Json.Formatting.Indented);

                        SqlCommand cmd = new SqlCommand("insert into Pos_SubeToMerkez(Sube2Merkez_Data,Sube2Merkez_Pasif,Sube2Merkez_Tarih) values ('" + jsonData + "',1,'" + Tarih.ToString("yyyy-MM-dd") + "')", con);
                        if (con.State != ConnectionState.Open) con.Open();
                        cmd.ExecuteNonQuery();
                        if (con.State != ConnectionState.Closed) con.Close();
                    }
                }

                MessageBox.Show(res_man.GetString("Şubeden - Merkeze Satışlar Aktarıldı."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }
    }
}