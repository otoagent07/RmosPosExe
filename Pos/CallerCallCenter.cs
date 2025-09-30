using Pos.Class;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class CallerCallCenter : DevExpress.XtraEditors.XtraForm
    {
        public string Tel { get; set; }
        public static int formYukseklik = 0; // her yeni gelen formun yükseliği kadar aşağıya inmesi için
        public CallerCallCenter()
        {
            InitializeComponent();
            this.TopMost = true;

            kucukEkranAktifmi();
            if (aktifmi)
            {

                //this.Size = new Size(476, 230);
                this.Size = new Size(520, 280);

                this.StartPosition = FormStartPosition.Manual;
                foreach (var scrn in Screen.AllScreens)
                {
                    if (scrn.Bounds.Contains(this.Location))
                    {
                        if (formYukseklik > scrn.Bounds.Bottom)
                        {
                            formYukseklik = 0;
                        }
                        this.Location = new Point(scrn.Bounds.Right - this.Width, scrn.Bounds.Top + formYukseklik);
                        formYukseklik = formYukseklik + this.Height;
                        return;
                    }
                }
            }
        }

        public void kucukEkranAktifmi()
        {
            string Param_PaketKucukEkran = dbtools.DegerGetir("select isnull(Param_PaketKucukEkran,0) as Param_PaketKucukEkran from Pos_Param where Param_Id = '1'");

            aktifmi = Convert.ToBoolean(Param_PaketKucukEkran);
        }

        public bool aktifmi = false;

        private void CallerCallCenter_Load(object sender, EventArgs e)
        {
            lbl_Tel.Text = "Tel No : " + Tel;
            //lbl_Tel.Text = "Tel No :  0 111 111 111";

            ekranyenile();

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod")) != "")
            {
                btn_Satis.Enabled = true;
                btn_AcikAdres.Enabled = false;
            }
            else
            {
                btn_Satis.Enabled = false;
                btn_AcikAdres.Enabled = true;
            }

            labelDakika.Text = "";
            karalisteKontrol();

            btnGelAl.Enabled = true;
        }

        private void ekranyenile()
        {
            string data = @"
        select Cari_Id,Cari_Kod,Cari_Ad,Cari_Tel,ISNULL(Cari_Adres1,'') as Cari_Adres1,ISNULL(Cari_Adres2,'') as Cari_Adres2, ISNULL(Cari_Adres3,'') as Cari_Adres3,
         	 il.Adres_Kod as ilKod,il.Adres_Ad as ilAd,ilce.Adres_Kod as ilceKod, ilce.Adres_Ad as ilceAd,mah.Adres_Kod as mahKod, mah.Adres_Ad as mahAd,
         	 sube.Pkod_Kod as subeKod, sube.Pkod_Ad as subeAd,ISNULL(Cari_KaraListede,0) as Cari_KaraListede
        from Pos_Cari as cari
        left join Pos_Adres as mah on cari.Cari_Mahalle = mah.Adres_Kod and mah.Adres_Sinif = '26'
        left join Pos_Adres as ilce on cari.Cari_Ilce = ilce.Adres_Kod and ilce.Adres_Sinif = '25'
        left join Pos_Adres as il on cari.Cari_Il = il.Adres_Kod and il.Adres_Sinif = '24'
        left join Pos_Kodlar as sube on mah.Adres_Sube = sube.Pkod_Kod and Pkod_Sinif = '27'
        where cari.Cari_Tel = '" + Tel + "'";


            DataTable dt = dbtools.SelectTable(data);

            DataTable newDt = new DataTable();

            // Yeni DataTable sütunlarını oluştur
            newDt.Columns.Add("Cari_Id", typeof(int));
            newDt.Columns.Add("Cari_Kod", typeof(string));
            newDt.Columns.Add("Cari_Ad", typeof(string));
            newDt.Columns.Add("Cari_Tel", typeof(string));
            newDt.Columns.Add("Cari_Adres", typeof(string));
            newDt.Columns.Add("ilKod", typeof(string));
            newDt.Columns.Add("ilAd", typeof(string));
            newDt.Columns.Add("ilceKod", typeof(string));
            newDt.Columns.Add("ilceAd", typeof(string));
            newDt.Columns.Add("mahKod", typeof(string));
            newDt.Columns.Add("mahAd", typeof(string));
            newDt.Columns.Add("subeKod", typeof(string));
            newDt.Columns.Add("subeAd", typeof(string));
            newDt.Columns.Add("Cari_KaraListede", typeof(bool));

            // Mevcut tablodaki her satır için 3 adresi ayrı satırlar olarak ekle
            foreach (DataRow row in dt.Rows)
            {
                string cariId = row["Cari_Id"].ToString();
                string cariKod = row["Cari_Kod"].ToString();
                string cariAd = row["Cari_Ad"].ToString();
                string cariTel = row["Cari_Tel"].ToString();
                string ilKod = row["ilKod"].ToString();
                string ilAd = row["ilAd"].ToString();
                string ilceKod = row["ilceKod"].ToString();
                string ilceAd = row["ilceAd"].ToString();
                string mahKod = row["mahKod"].ToString();
                string mahAd = row["mahAd"].ToString();
                string subeKod = row["subeKod"].ToString();
                string subeAd = row["subeAd"].ToString();
                bool cariKaraListede = Convert.ToBoolean(row["Cari_KaraListede"]);

                string[] adresler = { row["Cari_Adres1"].ToString(), row["Cari_Adres2"].ToString(), row["Cari_Adres3"].ToString() };

                foreach (string adres in adresler)
                {
                    if (!string.IsNullOrWhiteSpace(adres)) // Boş veya null adresleri eklemiyoruz
                    {
                        DataRow newRow = newDt.NewRow();
                        newRow["Cari_Id"] = cariId;
                        newRow["Cari_Kod"] = cariKod;
                        newRow["Cari_Ad"] = cariAd;
                        newRow["Cari_Tel"] = cariTel;
                        newRow["Cari_Adres"] = adres;
                        newRow["ilKod"] = ilKod;
                        newRow["ilAd"] = ilAd;
                        newRow["ilceKod"] = ilceKod;
                        newRow["ilceAd"] = ilceAd;
                        newRow["mahKod"] = mahKod;
                        newRow["mahAd"] = mahAd;
                        newRow["subeKod"] = subeKod;
                        newRow["subeAd"] = subeAd;
                        newRow["Cari_KaraListede"] = cariKaraListede;

                        newDt.Rows.Add(newRow);
                    }
                }
            }

            // Yeni tabloyu GridControl'e bağla
            gridControl1.DataSource = newDt;

            string fileName = getDizaynPath();
            if (File.Exists(fileName))
            {
                gridView1.RestoreLayoutFromXml(fileName);
            }


        }


        //private void ekranyenile()
        //{
        //    string data = @"
        //select Cari_Id,Cari_Kod,Cari_Ad,Cari_Tel,(ISNULL(Cari_Adres1,'') + ' ' + ISNULL(Cari_Adres2,'') +  ' ' + ISNULL(Cari_Adres3,'')) as Cari_Adres,
        //	 il.Adres_Kod as ilKod,il.Adres_Ad as ilAd,ilce.Adres_Kod as ilceKod, ilce.Adres_Ad as ilceAd,mah.Adres_Kod as mahKod, mah.Adres_Ad as mahAd,
        //	 sube.Pkod_Kod as subeKod, sube.Pkod_Ad as subeAd
        //from Pos_Cari as cari
        //left join Pos_Adres as mah on cari.Cari_Mahalle = mah.Adres_Kod and mah.Adres_Sinif = '26'
        //left join Pos_Adres as ilce on cari.Cari_Ilce = ilce.Adres_Kod and ilce.Adres_Sinif = '25'
        //left join Pos_Adres as il on cari.Cari_Il = il.Adres_Kod and il.Adres_Sinif = '24'
        //left join Pos_Kodlar as sube on mah.Adres_Sube = sube.Pkod_Kod and Pkod_Sinif = '27'
        //where cari.Cari_Tel = '" + Tel + "'";


        //    gridControl1.DataSource = dbtools.SelectTable(data);

        //    string fileName = getDizaynPath();
        //    if (File.Exists(fileName))
        //    {
        //        gridView1.RestoreLayoutFromXml(fileName);
        //    }


        //}
        public string getDizaynPath()
        {
            string klasorAd = "GridDizaynPos";
            if (!Directory.Exists(klasorAd))
            {
                Directory.CreateDirectory(klasorAd);
            }
            return klasorAd + @"\" + User.P_Kod + "_CallerCallCenter.xml";
        }


        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_CariEkle_Click(object sender, EventArgs e)
        {
            CariHesap hes = new CariHesap();
            hes.Tel = Tel;
            hes.ShowDialog();

            ekranyenile();
        }
        Cari cari = new Cari();

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        private void btn_Satis_Click(object sender, EventArgs e)
        {
            //string subeKod = Convert.ToString(gridView1.GetFocusedRowCellValue("subeKod"));

            //if (string.IsNullOrEmpty(subeKod))
            //{
            //    CallerSubeSec sube = new CallerSubeSec();
            //    sube.ShowDialog();

            //    subeKod = sube.kod;
            //}

            cari = Cari.Cari_Getir(Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod")));
            string adres = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Adres"));

            adressecenekGuncelle(cari.Cari_Kod, adres);


            if (cari.Cari_Kod != null)
            {
                DataTable dtSatis = dbtools.SelectTable("select distinct Rsat_Fisno,Rsat_Masa from Cst_Recete_Satis where Rsat_Cari = '" + cari.Cari_Kod + "' and Rsat_Durum = 'A' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");
                if (dtSatis.Rows.Count > 0)
                {
                    if (MessageBox.Show(res_man.GetString("Mevcut Cari için Açık çek bulunmaktadır. Açık çek üzerinde devam etmek istiyor musunuz?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string Masa_No = Convert.ToString(dtSatis.Rows[0]["Rsat_Masa"]);



                        Satis satis = new Satis();
                        satis.Tag = "M";
                        satis.Masa_No = Masa_No;
                        satis.Masa_Paket = true;
                        satis.Ozel_Masa = "";
                        satis.Split = 0;
                        satis.PaketFiyat = "P";
                        satis.Splitad = "";
                        satis.Cari_Kod = cari.Cari_Kod;
                        satis.adres = adres;
                        //satis.Sube = subeKod;
                        this.Close();
                        satis.ShowDialog();

                        return;
                    }
                }
            }


            DataTable dt = dbtools.SelectTable("select Masa_No,Masa_Ad,Masa_Paket  from Pos_Masa where Masa_Paket = '1' and Masa_Depart='" + Departman.Dep_Kodu + "' and Masa_Durum = '0'");
            if (dt.Rows.Count > 0)
            {
                Satis satis = new Satis();
                satis.Tag = "M";
                satis.Masa_No = Convert.ToString(dt.Rows[0]["Masa_No"]);
                satis.Masa_Paket = Convert.ToBoolean(dt.Rows[0]["Masa_Paket"]);
                satis.Ozel_Masa = "";
                satis.Split = 0;
                satis.Splitad = "";
                satis.mCari = cari;
                satis.CariTel = Tel;
                satis.PaketFiyat = "P";
                satis.Cari_Kod = cari.Cari_Kod;
                satis.adres = adres;

                //satis.AcikAdres = true;
                //satis.Sube = subeKod;
                this.Close();
                satis.ShowDialog();
            }


        }

        public void adressecenekGuncelle(string carikod, string adres)
        {
            try
            {
                if (adres == "") return;

                string q = $@"update Pos_Cari set adressecenek=(SELECT 
    CASE 
        WHEN EXISTS (SELECT 1 FROM Pos_Cari WHERE Cari_Kod = '{carikod}' AND Cari_Adres1 = '{adres}') THEN 1
        WHEN EXISTS (SELECT 1 FROM Pos_Cari WHERE Cari_Kod = '{carikod}' AND Cari_Adres2 = '{adres}') THEN 2
        WHEN EXISTS (SELECT 1 FROM Pos_Cari WHERE Cari_Kod = '{carikod}' AND Cari_Adres3 = '{adres}') THEN 3
        ELSE 1 
    END AS Aktif_Adres) WHERE Cari_Kod = '{carikod}'";

                dbtools.execcmdR(q);
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_CariDuzenle_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount == 0) return;

            CariHesap hes = new CariHesap();
            hes.CariKod = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
            hes.ShowDialog();

            ekranyenile();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Masa_No,Masa_Ad,Masa_Paket  from Pos_Masa where Masa_Paket = '1' and Masa_Depart='" + Departman.Dep_Kodu + "' and Masa_Durum = '0'");
            if (dt.Rows.Count > 0)
            {
                Satis satis = new Satis();
                satis.Tag = "M";
                satis.Masa_No = Convert.ToString(dt.Rows[0]["Masa_No"]);
                satis.Masa_Paket = Convert.ToBoolean(dt.Rows[0]["Masa_Paket"]);
                satis.Ozel_Masa = "";
                satis.Split = 0;
                satis.Splitad = "";
                satis.mCari = cari;
                satis.CariTel = Tel;
                satis.PaketFiyat = "P";
                satis.AcikAdres = true;
                //satis.Sube = subeKod;
                this.Close();
                satis.ShowDialog();
            }
            this.Close();


        }

        private void btnGridDizaynKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                gridView1.SaveLayoutToXml(getDizaynPath());
                MessageBox.Show("Grid Dizayn Kaydedildi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
            }

        }

        public static string MyClass = "CallerCallCenter";
        private void btnGridDizaynSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RHMesaj.MyMessageConfirmation("\"" + User.P_Kod + "\" Kullanıcısına Ait Grid Dizayn'ı Silmek İstediğinize Emin misiniz ?"))
                {
                    return;
                }


                string path = getDizaynPath();

                if (File.Exists(path))
                {
                    File.Delete(path);
                    RHMesaj.alertMesaj("Grid Dizayn Temizlendi");
                }
                else
                {
                    RHMesaj.alertMesaj("Grid Dizayn BULUNAMADI! \n " + path);
                }

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnGridDizaynTemizle1_Click", "", ex);
            }

        }

        private void CallerCallCenter_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (aktifmi && formYukseklik > 0)
            {
                formYukseklik = formYukseklik - this.Height;
            }
        }

        private void CallerCallCenter_Shown(object sender, EventArgs e)
        {
            this.TopMost = false;

        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            dakikayaz();
            karalisteKontrol();
        }

        public void dakikayaz()
        {
            try
            {
                string Cari_Kod = gridView1.GetFocusedRowCellValue("Cari_Kod").ToString();
                string q = "select top 1 ISNULL(ABS(DATEDIFF(MINUTE, MIN(paketAtamaTarih), GETDATE())), 0) AS AcikDakika from Cst_Recete_Satis where Rsat_Cari='" + Cari_Kod + "' and paketAtamaTarih is not null and Rsat_Durum='A'";

                string deger = dbtools.DegerGetir(q);

                if (deger == "0")
                {
                    labelDakika.Text = "";
                }
                else
                {
                    labelDakika.Text = deger + " dk";

                }
            }
            catch (Exception ex)
            {

            }
        }

        public void karalisteKontrol()
        {
            try
            {
                if (gridView1.RowCount > 0)
                {
                    var karaListeValue = gridView1.GetFocusedRowCellValue("Cari_KaraListede");
                    bool isKaraListede = karaListeValue != null && karaListeValue != DBNull.Value && Convert.ToBoolean(karaListeValue);
                    
                    labelKaraliste.Visible = isKaraListede;
                }
                else
                {
                    labelKaraliste.Visible = false;
                }
            }
            catch (Exception ex)
            {
                labelKaraliste.Visible = false;
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            dakikayaz();
            karalisteKontrol();
        }

        private void btnGelAl_Click(object sender, EventArgs e)
        {
            Paket_Servis paket = new Paket_Servis();
            paket.txt_Cari_Telefon.Text = Tel;
            paket.ShowDialog();
        }

        private void btnGelAl_Click_1(object sender, EventArgs e)
        {
            yenipaket();

            //Paket_Servis paket = new Paket_Servis();
            //paket.txt_Cari_Telefon.Text = Tel;
            //paket.ShowDialog();
        }


        public Satis satis;

        public void yenipaket()
        {
            DataTable dtMasa = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1 and Masa_Durum = 0 order by Masa_No");

            if (dtMasa.Rows.Count < 1)
            {
                MessageBox.Show(res_man.GetString("Boş Paket Masanız Bulunmamaktadır."));
                return;
            }

            var Masa_No = Convert.ToString(dtMasa.Rows[0]["Masa_No"]);

            string Cari_Adres = gridView1.GetFocusedRowCellValue("Cari_Adres").ToString();

            satis = new Satis();
            satis.Tag = "M";
            satis.txt_Not.Text = Tel;

            satis.Masa_No = Masa_No;
            satis.Masa_Paket = true;
            satis.Ozel_Masa = "";
            satis.Split = 0;
            satis.Splitad = "";
            satis.txt_Not.Text = "Telefon No :" +  Tel + "\n"+ Cari_Adres;
            satis.ShowDialog();
        }

    }
}