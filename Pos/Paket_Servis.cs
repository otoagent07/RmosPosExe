using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Paket_Servis : DevExpress.XtraEditors.XtraForm
    {
        public Cari pCari { get; set; }
        public bool AcikAdres = false;
        //public string CariTel = String.Empty;

        public Paket_Servis()
        {
            InitializeComponent();
            this.BringToFront();
        }
        public bool canClose = false;
        private void Paket_Servis_Load(object sender, EventArgs e)
        {
            gridyenile1();
        }

        private void gridyenile1()
        {

            gv_Cari.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;
            gv_Cari.FocusedColumn = gv_Cari.Columns.ColumnByFieldName("Cari_Kod");
            gv_Cari.ShowEditor();

            var data = dbtools.SelectTable(@"select  Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Adres1,Cari_Adres2,Cari_Adres3, Cari_Kart,
    Cari_Funvan, Cari_Fadres1, Cari_Fadres2, Cari_Vergidarie, Cari_Vergino, Cari_Mail, Cari_Il, Cari_Ilce, Cari_Mahalle,
    0 as Bakiye

    from Pos_Cari WITH(NOLOCK)   order by Cari_Kod

        ");

            DataTable newDt = new DataTable();

            // Yeni tablo için sütunları oluştur
            newDt.Columns.Add("Cari_Kod", typeof(string));
            newDt.Columns.Add("Cari_Ad", typeof(string));
            newDt.Columns.Add("Cari_Soyad", typeof(string));
            newDt.Columns.Add("Cari_Tel", typeof(string));
            newDt.Columns.Add("Adres", typeof(string));
            newDt.Columns.Add("Cari_Kart", typeof(string));
            newDt.Columns.Add("Cari_Funvan", typeof(string));
            newDt.Columns.Add("Cari_Fadres1", typeof(string));
            newDt.Columns.Add("Cari_Fadres2", typeof(string));
            newDt.Columns.Add("Cari_Vergidarie", typeof(string));
            newDt.Columns.Add("Cari_Vergino", typeof(string));
            newDt.Columns.Add("Cari_Mail", typeof(string));
            newDt.Columns.Add("Cari_Il", typeof(string));
            newDt.Columns.Add("Cari_Ilce", typeof(string));
            newDt.Columns.Add("Cari_Mahalle", typeof(string));
            newDt.Columns.Add("Bakiye", typeof(decimal));

            // Adresleri ayrı satırlar olarak ekleme işlemi
            foreach (DataRow row in data.Rows)
            {
                string cariKod = row["Cari_Kod"].ToString();
                string cariAd = row["Cari_Ad"].ToString();
                string cariSoyad = row["Cari_Soyad"].ToString();
                string cariTel = row["Cari_Tel"].ToString();
                string cariKart = row["Cari_Kart"].ToString();
                string cariFunvan = row["Cari_Funvan"].ToString();
                string cariFadres1 = row["Cari_Fadres1"].ToString();
                string cariFadres2 = row["Cari_Fadres2"].ToString();
                string cariVergidarie = row["Cari_Vergidarie"].ToString();
                string cariVergino = row["Cari_Vergino"].ToString();
                string cariMail = row["Cari_Mail"].ToString();
                string cariIl = row["Cari_Il"].ToString();
                string cariIlce = row["Cari_Ilce"].ToString();
                string cariMahalle = row["Cari_Mahalle"].ToString();
                decimal bakiye = Convert.ToDecimal(row["Bakiye"]);

                string[] adresler = { row["Cari_Adres1"].ToString(), row["Cari_Adres2"].ToString(), row["Cari_Adres3"].ToString() };

                foreach (string adres in adresler)
                {
                    if (!string.IsNullOrWhiteSpace(adres)) // Boş veya null adresleri eklemiyoruz
                    {
                        DataRow newRow = newDt.NewRow();
                        newRow["Cari_Kod"] = cariKod;
                        newRow["Cari_Ad"] = cariAd;
                        newRow["Cari_Soyad"] = cariSoyad;
                        newRow["Cari_Tel"] = cariTel;
                        newRow["Adres"] = adres;
                        newRow["Cari_Kart"] = cariKart;
                        newRow["Cari_Funvan"] = cariFunvan;
                        newRow["Cari_Fadres1"] = cariFadres1;
                        newRow["Cari_Fadres2"] = cariFadres2;
                        newRow["Cari_Vergidarie"] = cariVergidarie;
                        newRow["Cari_Vergino"] = cariVergino;
                        newRow["Cari_Mail"] = cariMail;
                        newRow["Cari_Il"] = cariIl;
                        newRow["Cari_Ilce"] = cariIlce;
                        newRow["Cari_Mahalle"] = cariMahalle;
                        newRow["Bakiye"] = bakiye;

                        newDt.Rows.Add(newRow);
                    }
                }
            }

            // Yeni tabloyu GridControl'e bağla
            grd_Cari.DataSource = newDt;


        }


        //    private void gridyenile1()
        //    {

        //        gv_Cari.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;
        //        gv_Cari.FocusedColumn = gv_Cari.Columns.ColumnByFieldName("Cari_Kod");
        //        gv_Cari.ShowEditor();

        //        var data = dbtools.SelectTable(@"select  Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Adres1,Cari_Adres2,Cari_Adres3,
        //ISNULL(Cari_Adres1, '') + ' ' + ISNULL(Cari_Adres2, '') + ' ' + ISNULL(Cari_Adres3, '') as Adres, Cari_Kart,
        //Cari_Funvan, Cari_Fadres1, Cari_Fadres2, Cari_Vergidarie, Cari_Vergino, Cari_Mail, Cari_Il, Cari_Ilce, Cari_Mahalle,
        //0 as Bakiye

        //from Pos_Cari WITH(NOLOCK)   order by Cari_Kod

        //    ");

        //        grd_Cari.DataSource = data;


        //    }


        private void gridyenile2()
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Rapor_Tipi", 5);
            com.Parameters.AddWithValue("@Cari", Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Kod")));
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl1.DataSource = dt;
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void btn_Cari_Kaydet_Click(object sender, EventArgs e)
        {
            if (txt_Cari_Kod.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("Cari Kodu Boş Geçilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (txt_Cari_Ad.Text.Length > 0)
                {
                    DataTable dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Kod = '" + txt_Cari_Kod.EditValue + "' ");
                    if (dt.Rows.Count < 1)
                    {
                        dbtools.execcmd("INSERT INTO dbo.Pos_Cari (Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Adres1,Cari_Adres2,Cari_Adres3,Cari_Kart) VALUES ( '" + txt_Cari_Kod.EditValue + "','" + txt_Cari_Ad.EditValue + "','" + txt_Cari_Soyad.EditValue + "','" + txt_Cari_Telefon.EditValue + "', "
                            + " '" + txt_Cari_Adres1.EditValue + "','" + txt_Cari_Adres2.EditValue + "','" + txt_Cari_Adres3.EditValue + "', '" + txt_Cari_Kart_No.EditValue + "' )");
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Kaydet, txt_Cari_Kod.EditValue + " Kod ile Cari Kaydedildi.", String.Empty, String.Empty);
                    }
                    else
                    {
                        dbtools.execcmd("update dbo.Pos_Cari set Cari_Kod='" + txt_Cari_Kod.EditValue + "', Cari_Ad='" + txt_Cari_Ad.EditValue + "', Cari_Soyad='" + txt_Cari_Soyad.EditValue + "', Cari_Tel='" + txt_Cari_Telefon.EditValue + "', "
                        + " Cari_Adres1='" + txt_Cari_Adres1.EditValue + "', Cari_Adres2='" + txt_Cari_Adres2.EditValue + "', Cari_Adres3='" + txt_Cari_Adres3.EditValue + "', Cari_Kart = '" + txt_Cari_Kart_No.EditValue + "' where  Cari_Kod = '" + txt_Cari_Kod.EditValue + "'  ");
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Duzelt, txt_Cari_Kod.EditValue + " Kod ile Cari Duzeltildi.", String.Empty, String.Empty);
                    }

                    gridyenile1();

                    txt_Cari_Ad.EditValue = null;
                    txt_Cari_Soyad.EditValue = null;
                    txt_Cari_Telefon.EditValue = null;
                    txt_Cari_Adres1.EditValue = null;
                    txt_Cari_Adres2.EditValue = null;
                    txt_Cari_Adres3.EditValue = null;
                    txt_Cari_Kart_No.EditValue = null;
                }
                else
                {
                    MessageBox.Show(res_man.GetString("Ad Boş Geçilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txt_Cari_Kod_Leave(object sender, EventArgs e)
        {
            Cari c = Cari.Cari_Getir(Convert.ToString(txt_Cari_Kod.EditValue));
            if (c != null)
            {
                txt_Cari_Ad.EditValue = c.Cari_Ad;
                txt_Cari_Soyad.EditValue = c.Cari_Soyad;
                txt_Cari_Telefon.EditValue = c.Cari_Tel;
                txt_Cari_Adres1.EditValue = c.Cari_Adres1;
                txt_Cari_Adres2.EditValue = c.Cari_Adres2;
                txt_Cari_Adres3.EditValue = c.Cari_Adres3;
                txt_Cari_Kart_No.EditValue = c.Cari_Kart;
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

        private void btn_Satis_Click(object sender, EventArgs e)
        {

            string adres = "";
            try
            {
                adres = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Adres"));

                Paket.paketForm.satis.adres = adres;
            }
            catch (Exception ex)
            {

            }

            int seciliSatir = gv_Cari.FocusedRowHandle;
            if (seciliSatir== -2147483646)
            {
                if (gv_Cari.RowCount > 0)
                {
                    pCari = Cari.Cari_Getir(Convert.ToString(gv_Cari.GetRowCellValue(0, "Cari_Kod")));

                    adressecenekGuncelle(pCari.Cari_Kod,adres);

                    this.Close();
                }
            }
            else
            {
                if (gv_Cari.RowCount > 0)
                {
                    pCari = Cari.Cari_Getir(Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Kod")));
                    adressecenekGuncelle(pCari.Cari_Kod, adres);
                    this.Close();
                }
            }
         
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            canClose = true;
            this.Close();
        }

        private void btn_AcikAdres_Click(object sender, EventArgs e)
        {
            AcikAdres = true;
            this.Close();
        }

        private void gv_Cari_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0)
            //{
            //    return;
            //}

            txt_Cari_Kod.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Kod"));
            txt_Cari_Ad.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Ad"));
            txt_Cari_Soyad.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Soyad"));
            txt_Cari_Telefon.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Tel"));
            txt_Cari_Adres1.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Adres"));
            txt_Cari_Kart_No.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Kart"));

            if (chk_Listele.Checked)
            {
                gridyenile2();
            }
        }

        private void gv_Cari_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            // Cari_Tel
            if (e.Column.FieldName == "Cari_Tel") // Kolon adı
            {
                //e.Appearance.BackColor = Color.Red;
                e.Appearance.ForeColor = Color.Red;
            }
        }
    }
}