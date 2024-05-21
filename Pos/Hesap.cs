using Pos.Class;
using Pos.Controllers;
using Pos.Ingenico;
using Pos.Models;
using SimpleTCP;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace Pos
{
    public partial class Hesap : DevExpress.XtraEditors.XtraForm
    {
        public string Masa_No = String.Empty;
        public int Split = 0;
        public string Splitad = "";
        public int Odeme_Ozelkod = -1;

        //string musTipi = String.Empty;


        string musTipi_A = String.Empty;
        string odaNo_A = String.Empty;
        int folio_A = 0;
        int masterFolio_A = 0;
        string pansiyon_A = String.Empty;
        int uyeId_A = 0;
        string uyeAdsoyad_A = String.Empty;
        string uyeKartturu_A = String.Empty;
        string cari_A = String.Empty;
        string indKodu_A = String.Empty;
        decimal indOran_A = 0;
        string odemeKodu_A = String.Empty;
        string CartFKartNo = String.Empty;
        string MusteriTipi = String.Empty;

        public string CariKodu = String.Empty;

        public bool cikis = true;

        public string tip = "";


        public Hesap()
        {
            InitializeComponent();
        }

        public bool Eksileme = false;

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());


        public void ingenicoButtonVisible()
        {
            DataRowView dataRow = look_Kapatma.GetSelectedDataRow() as DataRowView;
            if (dataRow == null) return;
            string fisTip = dataRow["Pkod_FisTipi"].ToString().ToLower();
            string text = look_Kapatma.Text.ToLower();
            if (fisTip.Equals("o") || fisTip.Equals("p") || text.Contains("cari hesap") || text.Contains("carı hesap"))
            {
                btnIngenicoKapat.Enabled = false;
            }
            else
            {
                btnIngenicoKapat.Enabled = true;
            }

        }

        DataTable dovizKurlari = new DataTable();

        public bool otoYazdirmadanKapat = false;


        public void lookKapatmaYukle()
        {
            DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");

            if (dt == null || dt.Rows.Count < 1)
            {
                dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43");
            }

            dt=Sabitler.getOdemeKodlari(dt);
            look_Kapatma.Properties.DataSource = dt;

            look_Kapatma.Properties.DisplayMember = "Pkod_Ad";
            look_Kapatma.Properties.ValueMember = "Pkod_Kod";
        }
        private void Hesap_Load(object sender, EventArgs e)
        {
            try
            {
                string kisiyeSatisAktifmi = dbtools.DegerGetir($"select isnull(Kodlar_KisiyeSatis,0) as Kodlar_KisiyeSatis from Stok_Kodlar where Kodlar_Sinif='01' and Kodlar_Kod='{Departman.Dep_Kodu}'");

                if (kisiyeSatisAktifmi == "0" || kisiyeSatisAktifmi.ToLower() == "false")
                {
                    gridControlKisiyeSatis.Visible = false;
                    gridControlFis.Visible = false;
                   gridControl1.Size = new System.Drawing.Size(gridControl1.Size.Width, 500);
                }

                btnSpSil.Visible = User.S_Sp_Sil;

                string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";
                string tar = Param.Tarih.Date.ToString("yyyy-MM-dd");
                dovizKurlari = dbtools.SelectTableR("exec Kurlar_Liste @Tip=0,@Kurlar_Tarih='" + tar + "',@Kurlar_Cesit=N'" + kur_cesit + "'");

                this.BringToFront();
                if (Param.Tesis_Tipi == 1)
                {
                    txt_Hesapno.Visible = false;
                }

                if (Split != 0)
                {
                    lbl_Split.Text = "Split:" + Split.ToString() + "\n" + Splitad;
                }


                //if (Param.Param_Yuvarla != "" && Convert.ToDecimal(Param.Param_Yuv_Sayi) > 0)
                //{
                //    dbtools.execcmd("Delete From Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Recete = '" + Param.Param_Yuvarla + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");
                //}

                YuvarlaModel model = ayarlar.getYuvarlama(Departman.Dep_Kodu);
                if (model != null && model.yuvarlamaFiyat > 0)
                {
                    dbtools.execcmd("Delete From Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Recete = '" + model.yuvarlamaRecete + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");
                }


                if (tip == "D")
                {
                    dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_SiparisPr = 1 Where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "'");
                }

                Hesap_Bul();
                gridyenile();
                Bakiye_Kontrol();

                look_DovizKod.Properties.DataSource = dbtools.SelectTable("select Mkodlar_Kod as kod,Mkodlar_Ad as ad from Muh_Kodlar where Mkodlar_sinif = '02' order by Mkodlar_Kod");
                look_DovizKod.Properties.DisplayMember = "ad";
                look_DovizKod.Properties.ValueMember = "kod";





                lookKapatmaYukle();


                ingenicoButtonVisible();





                Kapatma_Tekoda();

                chk_Adisyon.Checked = Departman.Adisyon;
                chk_Hesapdok.Checked = Departman.Kodlar_Hesap_Adisyon;

                btn_HesapDokumu.Enabled = User.G_Hesapdokumu;
                btn_Odemeal.Enabled = User.G_Odemeal;
                btn_Odemesil.Enabled = User.G_Odemesil;
                btn_Indirim.Enabled = User.G_Indirim_Hesap;
                btn_Yazdirkapat.Enabled = User.G_Yazdirkapat;
                btn_Yazdirmadankapat.Enabled = User.G_Yazdirmadankapat;
                btn_Bindirim.Enabled = User.G_Bindirim;
                btnTipBox.Enabled = User.G_Bindirim;
                btnIngenicoKapat.Visible = Departman.Kodlar_Ingenico;
                // simpleButton2.Visible = Departman.Kodlar_Ingenico;
                chk_AdisyonGR.Visible = true;// User.Pos_AdisyonPr;
                //gridColumn16.Visible = User.Pos_HesapArti;
                //gridColumn17.Visible = User.Pos_HesapArti;
                gridColumn18.Visible = User.Pos_HesapArti;

                if (Departman.Kodlar_Ingenico)
                {
                    //btn_Yazdirkapat.Enabled = false;
                    btn_Yazdirmadankapat.Enabled = false;
                }


                Yuvarlama();




                bool Pos_HesapFisIptal = Convert.ToBoolean(dbtools.DegerGetir("select top 1 isnull(Pos_HesapFisIptal,0) as Pos_HesapFisIptal from Rmosmuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'"));


                if (Pos_HesapFisIptal) btnFisIptal.Visible = true;
                else btnFisIptal.Visible = false;


                lbl_Bilgi.Text = "Cari : " + CariKodu;

                if (look_Kapatma.Text.Trim().Equals(""))
                {
                    btn_Yazdirkapat.Enabled = false;
                    btn_Yazdirmadankapat.Enabled = false;
                }



                string kod = dbtools.DegerGetir("select Pkod_otoKur from Pos_Kodlar where Pkod_Sinif='11' and Pkod_Kod='" + look_Kapatma.EditValue + "'");

                if (kod.Equals(""))
                {
                    look_DovizKod.EditValue = Param.Doviz_Kodu;
                }
                else
                {
                    look_DovizKod.EditValue = kod;
                }


                kisiyeSatisController = new KisiyeSatisController(this, Convert.ToInt32(this.Tag));
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Hesap_Load", "", ex);
            }

        }

        KisiyeSatisController kisiyeSatisController = null;


        AyarlarController ayarlar = new AyarlarController();
        private void Yuvarlama()
        {
            try
            {
                YuvarlaModel model = ayarlar.getYuvarlama(Departman.Dep_Kodu);

                if (model != null && model.yuvarlamaFiyat > 0)
                {
                    dbtools.execcmd("Delete From Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Recete = '" + model.yuvarlamaRecete + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");

                    Satis s = new Satis();
                    s.Tag = "M";
                    s.bartxt_FisNo.EditValue = Convert.ToInt32(this.Tag);
                    s.Miktar = 1;
                    s.Masa_No = Masa_No;
                    decimal artik = Convert.ToDecimal(txt_Odemetutari.EditValue) % (model.yuvarlamaFiyat / 100);
                    s.Yuv_Tutar = (model.yuvarlamaFiyat / 100) - artik;
                    s.Urun_Sat(model.yuvarlamaRecete, siparisPr: true);

                    gridyenile();
                    Bakiye_Kontrol();
                }
            }
            catch (Exception ex)
            {
                // RHMesaj.MyMessageError(MyClass, "Yuvarlama", "", ex);
            }
        }

        //private void YuvarlamaEski()
        //{
        //    if (Param.Param_Yuvarla != "" && Convert.ToDecimal(Param.Param_Yuv_Sayi) > 0)
        //    {
        //        dbtools.execcmd("Delete From Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Recete = '" + Param.Param_Yuvarla + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");

        //        Satis s = new Satis();
        //        s.Tag = "M";
        //        s.bartxt_FisNo.EditValue = Convert.ToInt32(this.Tag);
        //        s.Miktar = 1;
        //        s.Masa_No = Masa_No;
        //        decimal artik = Convert.ToDecimal(txt_Odemetutari.Text) % (Convert.ToDecimal(Param.Param_Yuv_Sayi) / 100);
        //        s.Yuv_Tutar = (Convert.ToDecimal(Param.Param_Yuv_Sayi) / 100) - artik;
        //        s.Urun_Sat(Param.Param_Yuvarla);

        //        gridyenile();
        //        Bakiye_Kontrol();
        //    }
        //}

        private void Hesap_Bul()
        {
            try
            {
                HesapBul ara = new HesapBul();
                string cevap = ara.Hesap_Bul(Convert.ToInt32(this.Tag));

                if (cevap != "OK")
                {
                    return;
                }

                if (ara.Mus_tipi != "C" && ara.Mus_tipi != "Y")
                {
                    return;
                }

                musTipi_A = ara.Mus_tipi;
                odaNo_A = ara.Oda_No;
                folio_A = ara.Folio;
                masterFolio_A = ara.Master_Folio;
                pansiyon_A = ara.Pansiyon;
                uyeId_A = ara.Uye_Id;
                uyeAdsoyad_A = ara.Uye_Adsoyad;
                uyeKartturu_A = ara.Uye_Kartturu;
                cari_A = ara.Cari_Kod;
                indKodu_A = ara.Ind_Kodu;
                indOran_A = ara.Ind_Oran;
                odemeKodu_A = ara.Odeme_Kodu;
                lbl_CariAdi.Text = ara.CariAdi;
                FolioKart_No = ara.Kart_No;


                txt_Hesapno.EditValue = odaNo_A;
                lbl_Bilgi.Text = ara.Bilgi;
            }
            catch (Exception ex)
            {
                RHMesaj.alertMesajSagUst("Hesap.Hesap_Bul", "" + ex.Message, 6);
            }

        }

        private void Bakiye_Kontrol()
        {
            try
            {
                decimal bakiye = Bakiye_bul_TL();

                if (bakiye < 1)
                {
                    txt_Odemetutari.EditValue = Convert.ToDecimal(bakiye / Param.Doviz_Kuru).ToString("n2");

                    if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                    {


                        //DataTable dtDovizDagilim2 = Fis_Islem.Doviz_Dagilim(bakiye / Param.Doviz_Kuru); // burayı 23.5 yap

                        //gridControl2.DataSource = dtDovizDagilim2;

                    }
                    else
                    {
                        return;

                    }



                }

                string tutar = Convert.ToDecimal(bakiye / Param.Doviz_Kuru).ToString("n2");
                txt_Odemetutari.EditValue = tutar;
                txt_Odemetutari.Text = tutar;


                if (bakiye == 0)
                {
                    btn_Odemeal.Enabled = false;
                }
                else
                {
                    //btn_Odemeal.Enabled = true;
                    btn_Odemeal.Enabled = User.G_Odemeal;
                }



                DataTable dtDovizDagilim = new DataTable();

                if (Param.Kurlar_Nerden == 0)
                {

                    dtDovizDagilim = Fis_Islem.Doviz_DagilimFront(bakiye / Param.Doviz_Kuru);
                }
                else
                {
                    dtDovizDagilim = Fis_Islem.Doviz_Dagilim(bakiye / Param.Doviz_Kuru); // burayı 23.5 yap

                    if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                    {
                        // bakiye = Convert.ToDecimal(bakiye / getKurKarsilik(Param.Doviz_Kodu));
                        bakiye = Bakiye_bul_TL2();
                        dtDovizDagilim = Fis_Islem.Doviz_Dagilim(bakiye / Param.Doviz_Kuru); // burayı 23.5 yap

                    }



                }

                gridControl2.DataSource = dtDovizDagilim;

                string kod = dbtools.DegerGetir("select Pkod_otoKur from Pos_Kodlar where Pkod_Sinif='11' and Pkod_Kod='" + look_Kapatma.EditValue + "'");


                if (kod.Equals(""))
                {
                    look_DovizKod.EditValue = Param.Doviz_Kodu;
                }
                else
                {
                    look_DovizKod.EditValue = kod;
                }


                if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                {
                    //txt_Odemetutari.EditValue = Convert.ToDecimal(bakiye / getKurKarsilik(Param.Doviz_Kodu)).ToString("n2");
                    txt_Odemetutari.EditValue = bakiye;

                }

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Bakiye_Kontrol", "", ex);

            }

        }

        private void gridyenile(bool urunBazli = false)
        {
            gridColumn1.FieldName = "Rec_Ad";
            gridColumn2.FieldName = "Rsat_Miktar";
            gridColumn3.FieldName = "Rsat_Emiktar";
            gridColumn4.FieldName = "Rsat_Tutar";
            gridColumn5.FieldName = "Rsat_Doviztutar";
            gridColumn6.FieldName = "Rsat_Ba";
            gridColumn7.FieldName = "Rsat_Id";


            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Convert.ToInt32(this.Tag));
            com.Parameters.AddWithValue("@Rapor_Tipi", 2);
            com.Parameters.AddWithValue("@Split", Split);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
            {
                foreach (DataRow dr in dt.Rows)
                {



                    if (dr != null && dr["Rsat_Ba"].ToString() == "A")
                    {
                        decimal kur = 0;
                        foreach (DataRow item in dovizKurlari.Rows)
                        {
                            if (Param.Doviz_Kodu == item["MKodlar_Kod"].ToString())
                            {
                                kur = Convert.ToDecimal(item["Doviz_Alis"].ToString());
                                break;
                            }
                        }
                        decimal tlTutar = Convert.ToDecimal(dr["Rsat_Tutar"].ToString());
                        dr["Rsat_Doviztutar"] = tlTutar / kur;

                    }
                }

            }

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Rsat_Miktar"] = dr["Rsat_Miktar"].ToString().Replace(",0000", "");
                    dr["Rsat_Miktar"] = dr["Rsat_Miktar"].ToString().Replace(",000", "");
                    dr["Rsat_Tutar"] = dr["Rsat_Tutar"].ToString().Replace(",00", "");
                }

            }




            gridControl1.DataSource = dt;


            if (Param.Calisma_Sekli == 1)
            {
                gridColumn5.Visible = true;
                //gridColumn5.VisibleIndex = 6;
            }
            else
            {
                gridColumn4.Visible = true;
                //gridColumn4.VisibleIndex = 6;
            }

            if (dt.Rows.Count > 0)
            {
                txt_Not.Text = Convert.ToString(dt.Rows[0]["Rsat_Not"]);
                lbl_Masaadi.Text = Convert.ToString(dt.Rows[0]["Rsat_Masa"]);
                lbl_KisiSayisi.Text = Convert.ToString(dt.Rows[0]["Rsat_Kisi"]);
            }
            Main.a.Listele(Convert.ToInt32(this.Tag));

            if (urunBazli == false)
            {
                decimal toplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                if (Param.Calisma_Sekli == 1)
                {
                    toplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Doviztutar"].SummaryItem.SummaryValue);

                }

                txt_Odemetutari.EditValue = toplamTutar;
                //txt_Odemetutari.Text = toplamTutar.ToString();

            }

            if (kisiyeSatisController != null)
                kisiyeSatisController.listele();

        }




        public int fisno = 0;
        private void btn_Cikis_Click(object sender, EventArgs e)
        {

            if (Convert.ToString(dbtools.DegerGetir("select COUNT(*) from cst_Recete_Satis where Rsat_Fisno = '" + this.Tag + "' and Rsat_Durum = 'K'")) != "0")
            {
                gridyenile();
                Bakiye_Kontrol();
                MessageBox.Show("Lütfen YAZDIR KAPAT Yapınız.\nYazar Kasadan Fiş Çıkmamışşa Fişi Manuel Alınız veya Rapordan Fişi İptal Edip Tekrardan Ödeme Alınız", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            cikis = false;
            canClose = true;
            fisno = Convert.ToInt32(this.Tag);
            this.Close();
        }

        private void btn_Odemetutari_Click(object sender, EventArgs e)
        {
            Klavye1 k = new Klavye1();
            k.Tag = "ODEMETUTAR";
            k.ShowDialog();

            if (k.Cikis == false)
            {
                txt_Odemetutari.EditValue = k.sayi;
            }



        }

        string FolioKart_No = String.Empty;

        string FolioKart_ID = "";
        string cariIndirimDec = "0";

        private void btn_Hesapara_Click(object sender, EventArgs e)
        {
            Arama ara = new Arama();
            ara.KapatmaKodu = Convert.ToString(look_Kapatma.EditValue);
            ara.Odeme_Ozelkod = Odeme_Ozelkod;
            ara.ShowDialog();

            if (Param.Tesis_Tipi == 0)
            {
                if (Fronttools.HesapKapali_Kontrol(ara.Folio))
                {
                    MessageBox.Show(res_man.GetString("Seçilen Hesap Harcamaya Kapatılmıştır.") + "\n" + res_man.GetString("Satış İşlemi Gerçekleşecektir."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            musTipi_A = ara.Mus_tipi;
            odaNo_A = ara.Oda_No;
            folio_A = ara.Folio;
            masterFolio_A = ara.Master_Folio;
            pansiyon_A = ara.Pansiyon;
            uyeId_A = ara.Uye_Id;
            uyeAdsoyad_A = ara.Uye_Adsoyad;
            uyeKartturu_A = ara.Uye_Kartturu;
            cari_A = ara.Cari_Kod;
            indKodu_A = ara.Ind_Kodu;
            indOran_A = ara.Ind_Oran;
            odemeKodu_A = ara.Odeme_Kodu;
            FolioKart_No = ara.Kart_No;
            FolioKart_ID = Convert.ToString(ara.KartID);
            cariIndirimDec = ara.Cari_indirimOran.ToString().Replace(",", ".");
            txt_Hesapno.Text = Convert.ToString(odaNo_A) == null ? cari_A : odaNo_A;
            lbl_Bilgi.Text = ara.Bilgi; // burada

            txtCariAd.Text = ara.Uye_Adsoyad;

            if (Convert.ToString(indOran_A) != "")
            {
                Fis_Update();
            }


            if (Odeme_Ozelkod == 2 && musTipi_A == "C" && !string.IsNullOrEmpty(cari_A))
            {
                DataTable dtCari = dbtools.SelectTable("select ISNULL(Cari_Limit,0) as Cari_Limit, ISNULL(Cari_LimitTutar,0) as Cari_LimitTutar from Pos_Cari where Cari_Kod = 'OH' and ISNULL(Cari_Aktif,1) = 1 ");
                if (dtCari.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dtCari.Rows[0]["Cari_Limit"]))
                    {
                        decimal limit = Convert.ToDecimal(dtCari.Rows[0]["Cari_LimitTutar"]);
                        decimal toplamBorc = Convert.ToDecimal(dbtools.DegerGetir("select ISNULL((select SUM(Chrk_Borc) from Pos_Carihrk where Chrk_Cari = '" + cari_A + "' and DATEPART(Year,Chrk_Tarih) = '" + Param.Tarih.Year + "' and DATEPART(MONTH,Chrk_Tarih) = '" + Param.Tarih.Month + "'),0) as Borc"));

                        lbl_Bilgi.Text += " Limit : " + (limit - toplamBorc).ToString("N2");

                        if ((limit - toplamBorc) < Bakiye_Bul(false))
                        {
                            MessageBox.Show(res_man.GetString("Seçili Hesap Limit Tutarını Aşmıştır."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            if (Param.cariindirimAktif && ara.Cari_indirimOran > 0)
            {
                string query = $@"exec Pos_Manuel_Indirim @Fisno={this.Tag},@Ind_Tip=N'Y',@Ind_Tutar=N'0',@Ind_Doviztutar=N'0',@Ind_Oran=N'{cariIndirimDec}',@Ind_Turu=N'MANUEL',@Split=0,@Ind_User=N'1',@aciklama=N'{uyeAdsoyad_A} %{cariIndirimDec} indirim',@Cari_id='{ara.Cari_Kod}'";
                dbtools.execcmdR(query);
            }
            gridyenile();
            Bakiye_Kontrol();
        }

        private void Fis_Update()
        {
            try
            {
                if (Convert.ToString(musTipi_A) != String.Empty)
                {

                    string query2 = "update Cst_Recete_Satis set Rsat_MusTipi = '" + musTipi_A + "',Rsat_Odano = '" + odaNo_A + "',Rsat_Folio = '" + folio_A + "',Rsat_Pansiyon = '" + pansiyon_A + "',Rsat_Uye_Id = '" + uyeId_A + "', "
                                            + " Rsat_Uye_Ad = '" + uyeAdsoyad_A + "',Rsat_Uye_Kart_Turu = '" + uyeKartturu_A + "', Rsat_Indkodu = '" + indKodu_A + "',Rsat_Indoran = '" + indOran_A.ToString().Replace(",", ".") + "', "
                                             + " Rsat_Kartno = '" + FolioKart_No + "', Rsat_Kart_ID = '" + FolioKart_ID + "' "

                                            + " where Rsat_Fisno = '" + this.Tag + "' and Rsat_Ba = 'B'";

                    dbtools.execcmd(query2);

                    dbtools.execcmd("exec Pos_Satis_Induyg @Fisno = " + this.Tag);


                    if (Param.Calisma_Sekli == 1)   //Dövizli
                    {
                        string dovkod = Param.Doviz_Kodu;
                        if (look_DovizKod.EditValue != null)
                        {
                            dovkod = look_DovizKod.EditValue.ToString();
                        }

                        string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";
                        if (Param.Doviz_Cinsi == 2) //Müşteri Giriş Günü Kuru
                        {
                            int Master_folio = Convert.ToInt32(Fronttools.DegerGetir("select top 1 isnull(Rez_Master_id,Rez_Id) from Rez WITH(NOLOCK) where Rez_Id = '" + folio_A.ToString() + "' "));
                            DateTime Giris_tarihi = Convert.ToDateTime(Fronttools.DegerGetir("select top 1 Rez_Giris_tarihi from Rez WITH(NOLOCK) where Rez_Id = '" + Master_folio.ToString() + "' "));
                            Param.Doviz_Kuru = Convert.ToDecimal(Fronttools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + dovkod + "' and  Convert(date,Kurlar_Tarih,105) = '" + Giris_tarihi.Date.ToString("yyyy-MM-dd") + "'"));

                            //Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Giris_tarihi.Date.ToString("yyyy-MM-dd") + "'"));
                            //tutar = doviztutar * kur;

                            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Tutar = Rsat_Doviztutar * " + Param.Doviz_Kuru.ToString().Replace(",", ".") + ",Rsat_Net = ((Rsat_Doviztutar * " + Param.Doviz_Kuru.ToString().Replace(",", ".") + ") *100 ) / (100 * Rsat_Kdvoran), Rsat_Kdv = (Rsat_Doviztutar * " + Param.Doviz_Kuru.ToString().Replace(",", ".") + ") - ((Rsat_Doviztutar * " + Param.Doviz_Kuru.ToString().Replace(",", ".") + ") *100 ) / (100 * Rsat_Kdvoran) where Rsat_Fisno = '" + this.Tag + "' and Rsat_Ba = 'B'");

                        }
                        else
                        {
                            if (Param.Kurlar_Nerden == 0) // otel
                            {

                                string query = "select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + dovkod + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'";
                                Param.Doviz_Kuru = Convert.ToDecimal(Fronttools.DegerGetir(query));
                                //Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                            }
                            else
                            {
                                Param.Doviz_Kuru = Convert.ToDecimal(dbtools.DegerGetir("select " + Param.Doviz_Turu + "  from Kurlar where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Kodu = '" + dovkod + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                                //Parite = Convert.ToDecimal(Fronttools.DegerGetir(@"select ISNULL(Kurlar_Parite,0)  from Kurlar with(NOLOCK) where Kurlar_Kodu =  '" + look_DovizKod.EditValue + "' and Kurlar_Cesit  = '" + kur_cesit + "' and Kurlar_Tarih  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                            }
                            //tutar = doviztutar * kur;
                        }
                    }

                    LimitKontrol();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }




        string Kart_No = String.Empty;
        private bool LimitKontrol()
        {
            if (Param.Tesis_Tipi == 1)
            {
                return true;
            }



            if (Odeme_Ozelkod == 2 || Odeme_Ozelkod == 5)
            {
                return true;
            }

            if (Fronttools.Folio_LimitBakiye_Bul(masterFolio_A))    //Folio içinde Limit Bakiyeden Bulunacak
            {
                string bilgi = lbl_Bilgi.Text;
                bilgi = bilgi.Contains("- Bakiye : ") ? bilgi.Substring(0, bilgi.IndexOf("- Bakiye : ")) : bilgi;

                decimal folioBakiye = Fronttools.BalanceBul(masterFolio_A, FolioKart_No); //13,95

                decimal odemeTutar = Convert.ToDecimal(txt_Odemetutari.EditValue);//15,28
                lbl_Bilgi.Text = bilgi + " - Bakiye : " + (folioBakiye).ToString("N2");
                if (((-1) * folioBakiye) - odemeTutar < 0)
                {
                    MessageBox.Show(odaNo_A + " " + res_man.GetString("NL Hesap Bakiye Yetersizdir."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            else
            {
                string LimitUyariBul = Fronttools.LimitUyarı_Bul(masterFolio_A);

                if (Param.Param_LimitFolio == true && Departman.Kodlar_AndPos_NFC == true)
                {
                    LimitUyariBul = "E";
                }

                if (LimitUyariBul == "E")
                {
                    string bilgi = lbl_Bilgi.Text;
                    bilgi = bilgi.Contains("- Bakiye : ") ? bilgi.Substring(0, bilgi.IndexOf("- Bakiye : ")) : bilgi;

                    decimal limitBakiye = Fronttools.Folio_LimitTutar_Bul(masterFolio_A); //13,95
                    decimal folioBakiye = Fronttools.BalanceBul(masterFolio_A, FolioKart_No); //13,95  

                    decimal odemeTutar = Convert.ToDecimal(txt_Odemetutari.EditValue);//15,28
                    lbl_Bilgi.Text = bilgi + " - Bakiye : " + limitBakiye.ToString("N2") + " - " + folioBakiye.ToString("N2") + "=" + (limitBakiye - folioBakiye).ToString("N2");

                    limitBakiye = limitBakiye - folioBakiye;
                    if (limitBakiye - odemeTutar < 0)
                    {
                        MessageBox.Show(odaNo_A + " " + res_man.GetString("NL Hesap Bakiye Yetersizdir.") + "\n" + res_man.GetString("Kalan Bakiye : ") + limitBakiye.ToString("N2"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }
            }


            //if (Fronttools.LimitUyarı_Bul(masterFolio_A) == "E")
            //{
            //    string bilgi = lbl_Bilgi.Text;
            //    bilgi = bilgi.Contains("- Bakiye : ") ? bilgi.Substring(0, bilgi.IndexOf("- Bakiye : ")) : bilgi;

            //    decimal folioBakiye = Fronttools.BalanceBul(masterFolio_A); //13,95



            //    decimal odemeTutar = Convert.ToDecimal(txt_Odemetutari.Text);//15,28
            //    lbl_Bilgi.Text = bilgi + " - Bakiye : " + (folioBakiye).ToString("N2");
            //    if (((-1) * folioBakiye) - odemeTutar < 0)
            //    {
            //        MessageBox.Show(odaNo_A + " NL Hesap Bakiye Yetersizdir."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        return false;
            //    }
            //}

            return true;
        }

        public void hesapDokum(bool sifirli)
        {
            try
            {
                string fisno = Convert.ToString(this.Tag);

                if (chk_Hesapdok.Checked)
                {
                    AdisyonPr adisyon = new AdisyonPr();
                    string cevap = adisyon.Adisyon_Yaz(Convert.ToInt32(Convert.ToInt32(this.Tag)), false);
                    if (cevap != "OK")
                    {
                        MessageBox.Show(cevap);
                        return;
                    }
                    adisyon.Adisyon_Sayac_Arttir(Convert.ToInt32(this.Tag));


                    //AdisyonPr adisyon = new AdisyonPr();
                    //string cevap = adisyon.Adisyon_Yaz(Convert.ToInt32(this.Tag));
                    //if (cevap != "OK")
                    //{
                    //    MessageBox.Show(cevap);
                    //    return;
                    //}
                    //adisyon.Adisyon_Sayac_Arttir(Convert.ToInt32(this.Tag));
                }
                else
                {
                    FisPr pr = new FisPr();
                    if (Param.Param_YeniHesapDkm)
                    {
                        pr.newHesapDokum(true, Convert.ToInt32(this.Tag), Split, "* * * HESAP DÖKÜM FİŞİ * * *", sifirli);
                    }
                    else
                    {
                        pr.HesapDokum(true, Convert.ToInt32(this.Tag), Split);
                    }
                }

                dbtools.execcmd("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Ingenico_Status='1' where Rsat_Fisno='" + fisno + "'");

                if (Param.Param_Hesap_Kilit)
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Hesap_Kilit = 1 where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "'");
                }

                decimal toplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                string aciklama = "Fisno : " + fisno + " . HESAP DÖKÜM FİŞİ ALINDI. Toplam Tutar : " + toplamTutar;

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, aciklama, fisno, "");

                this.Close();
            }
            catch (Exception ex)
            {
                RHMesaj.alertMesaj("HATA " + ex.Message);
            }
        }

        private void btn_HesapDokumu_Click(object sender, EventArgs e)
        {
            try
            {
                if (odemeKodSaatAraligindaKapalimi())
                {
                    MessageBox.Show("Ödeme kodu kapalı");
                    return;
                }

                bool sifirmi = getTutarsifirmi();
                hesapDokum(sifirmi);
            }
            catch (Exception ex)
            {

                RHMesaj.alertMesaj("HATA " + ex.Message);
            }

        }

        public bool getTutarsifirmi()
        {
            bool hesapsifir = false;
            try
            {
                hesapsifir = Convert.ToBoolean(dbtools.DegerGetir("select ISNULL(hesapDokTutarSifir,0) as hesapDokTutarSifir,* from Pos_Kodlar where Pkod_Kod='" + look_Kapatma.EditValue.ToString() + "' and Pkod_Sinif='11' "));
            }
            catch (Exception ex)
            {

            }
            return hesapsifir;
        }

        public bool odemeKontrol()
        {
            try
            {



                if (txt_Odemetutari != null && !txt_Odemetutari.Text.Equals(""))
                {
                    decimal sayi = Convert.ToDecimal(txt_Odemetutari.EditValue);

                    decimal toplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);


                    if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                    {

                        sayi = tlKarsilik(sayi);
                        sayi = StatikSinif.getTutarKontrol(sayi, toplamTutar);
                    }
                    if (sayi > toplamTutar)
                    {

                        RHMesaj.MyMessageInformation("Toplam tutardan büyük ödeme yapılamaz ! ");
                        //txt_Odemetutari.Text = toplamTutar.ToString();
                        txt_Odemetutari.EditValue = toplamTutar;



                        return false;
                    }



                }

            }
            catch (Exception ex)
            {
                // RHMesaj.MyMessageError(MyClass,"","",ex);
            }
            return true;
        }
        public decimal tlKarsilik(decimal doviztutar)
        {
            decimal girilenTLKarsilik = getKurKarsilik(Convert.ToString(look_DovizKod.EditValue));

            decimal tlKarsilik = girilenTLKarsilik * doviztutar;
            return tlKarsilik;
        }

        public void odemeAl(bool araodeme)
        {
            try
            {
                if (odemeKontrol() == false)
                {
                    return;
                }

                cikis = false;

                if (Convert.ToDecimal(gridColumn4.SummaryText) <= 0)
                {
                    return;
                }

                Odeme_Al(araodeme);

                decimal tutar = 0, doviztutar = 0;
                if (Param.Calisma_Sekli == 1)       //Döviz
                {
                    doviztutar = Convert.ToDecimal(txt_Odemetutari.EditValue);
                    tutar = doviztutar * Param.Doviz_Kuru;


                }
                else
                {
                    tutar = Convert.ToDecimal(txt_Odemetutari.EditValue);
                    doviztutar = tutar;
                }
                if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                {
                    doviztutar = getDovizTutar(doviztutar);
                }
                //if (Convert.ToString(indOran_A) != "")
                //{
                //    Fis_Update();
                //}


                temizle();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "odemeAl", "", ex);
            }

        }


        private void btn_Odemeal_Click(object sender, EventArgs e)
        {
            araOdemeAl(true);

        }

        public void araOdemeAl(bool araodeme)
        {
            try
            {
                if (look_Kapatma.EditValue == null)
                {
                    MessageBox.Show("Ödeme Kodu Seçiniz !");
                    return;
                }
                var look_KapatmaSecili = look_Kapatma.EditValue.ToString();
                var look_DovizKodSecili = look_DovizKod.EditValue.ToString();
                odemeAl(araodeme);


                look_Kapatma.EditValue = null;
                //look_Kapatma.EditValue = look_KapatmaSecili;
                look_DovizKod.EditValue = null;
                look_DovizKod.EditValue = look_DovizKodSecili;
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btn_Odemeal_Click", "", ex);
            }

        }


        public void ikramR()
        {
            Arama ara = new Arama();
            ara.KapatmaKodu = Convert.ToString(look_Kapatma.EditValue);
            ara.Odeme_Ozelkod = -1;
            ara.Cari_Kod = CariKodu;
            //ara.ShowDialog();

            if (Param.Tesis_Tipi == 0)
            {
                if (Fronttools.HesapKapali_Kontrol(ara.Folio))
                {
                    MessageBox.Show(res_man.GetString("Seçilen Hesap Harcamaya Kapatılmıştır.") + "\n" + res_man.GetString("Satış İşlemi Gerçekleşecektir."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            musTipi_A = ara.Mus_tipi;
            odaNo_A = ara.Oda_No;
            folio_A = ara.Folio;
            masterFolio_A = ara.Master_Folio;
            pansiyon_A = ara.Pansiyon;
            uyeId_A = ara.Uye_Id;
            uyeAdsoyad_A = ara.Uye_Adsoyad;
            uyeKartturu_A = ara.Uye_Kartturu;
            cari_A = ara.Cari_Kod;
            indKodu_A = ara.Ind_Kodu;
            indOran_A = ara.Ind_Oran;
            odemeKodu_A = ara.Odeme_Kodu;
            FolioKart_No = ara.Kart_No;
            FolioKart_ID = Convert.ToString(ara.KartID);

            txt_Hesapno.Text = Convert.ToString(odaNo_A) == null ? cari_A : odaNo_A;
            lbl_Bilgi.Text = ara.Bilgi; // burada

            if (Convert.ToString(indOran_A) != "")
            {
                Fis_Update();
            }




            gridyenile();
            Bakiye_Kontrol();


        }

        public int ozelKod2 = 0;
        private bool Odeme_Al(bool araodememi=false)
        {
            try
            {
                if (ozelKod2 == -1)
                {

                    ikramR();
                }

                if (txt_Hesapno.Text == String.Empty && Param.Tesis_Tipi == 0 && Odeme_Ozelkod != 5)
                {
                    MessageBox.Show(res_man.GetString("Kapatma Hesabını Seciniz..."));
                    return false;
                }

                if (txt_Hesapno.Text == String.Empty && Param.Tesis_Tipi == 1 && (Odeme_Ozelkod == 2 || Odeme_Ozelkod == 3 || Odeme_Ozelkod == 5))
                {
                    MessageBox.Show(res_man.GetString("Kapatma Hesabını Seciniz..."));
                    return false;
                }

                if (look_Kapatma.EditValue == null)
                {
                    MessageBox.Show(res_man.GetString("Kapatma Kodunu Seçiniz..."));
                    return false;
                }

                if (Param.Tesis_Tipi == 0)
                {
                    if (!LimitKontrol())
                    {
                        return false;
                    }
                }

                string fisnom = this.Tag.ToString();
                var donen = Sabitler.odenmezVeyaIkramiseServisPayiSil(fisnom, look_Kapatma.EditValue.ToString());

                if (donen==true && araodememi==false) // 09.05.2024 de gridi yenilemek zorunda kaldık hepap katırken odenmez otomatik sildiği için tutarı alması için txttutar ı yenilemek gerekiyordu
                {
                    gridyenile();
                }


                decimal tutar, doviztutar;
                Console.WriteLine("odemealdayim");
                if (Param.Calisma_Sekli == 1)       //Döviz
                {
                    if (Eksileme == false)
                    {
                        doviztutar = Math.Abs(Convert.ToDecimal(txt_Odemetutari.EditValue));
                        tutar = doviztutar * Param.Doviz_Kuru;
                        if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                        {
                            decimal dovizTLKarsilik = getKurKarsilik(Param.Doviz_Kodu);
                            decimal girilenTLKarsilik = getKurKarsilik(Convert.ToString(look_DovizKod.EditValue));

                            decimal tlKarsilik = girilenTLKarsilik * doviztutar;

                            tutar = tlKarsilik / dovizTLKarsilik;
                            // doviztutar = tutar;
                        }


                    }
                    else
                    {
                        doviztutar = (Convert.ToDecimal(txt_Odemetutari.EditValue));
                        tutar = doviztutar * Param.Doviz_Kuru;
                        if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                        {
                            decimal dovizTLKarsilik = getKurKarsilik(Param.Doviz_Kodu);
                            decimal girilenTLKarsilik = getKurKarsilik(Convert.ToString(look_DovizKod.EditValue));

                            decimal tlKarsilik = girilenTLKarsilik * doviztutar;
                            tutar = tlKarsilik / dovizTLKarsilik;
                            // doviztutar = tutar;
                        }


                    }
                }
                else
                {
                    if (Eksileme == false)

                    {
                        tutar = Math.Abs(Convert.ToDecimal(txt_Odemetutari.EditValue));
                        doviztutar = tutar;
                    }
                    else
                    {
                        tutar = Convert.ToDecimal(txt_Odemetutari.EditValue);
                        doviztutar = tutar;
                    }
                }

                //if (Convert.ToDecimal(gridColumn4.SummaryText) > 0)
                {
                    //kurdan dolayı küsüratı yuvarlama
                    decimal mevcutToplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);


                    //decimal indirimOran = getIndirimFront();
                    //if (indirimOran != 0)
                    //{
                    //    string query = "exec Pos_Manuel_Indirim @Fisno=" + Convert.ToInt32(this.Tag) + ",@Ind_Tip=N'Y',@Ind_Tutar=N'0',@Ind_Doviztutar=N'0',@Ind_Oran=" + indirimOran.ToString().Replace(",", ".") + ",@Ind_Turu = 'EXTRA'";
                    //    dbtools.execcmdR(query);
                    //    gridyenile();
                    //}





                    Fis_Islem.Odeme_Al(Convert.ToInt32(this.Tag), tutar, doviztutar, Convert.ToString(look_Kapatma.EditValue), musTipi_A, odaNo_A, folio_A, cari_A, Split, Convert.ToString(look_DovizKod.EditValue), chk_AdsPr.Checked, mevcutToplamTutar, kisiyeAdSoyad: kisiyeSatisController.kisiyesatisAdSoyad);



                    Fis_Islem.Satis_Tip(Convert.ToInt32(this.Tag), Convert.ToString(look_Kapatma.EditValue), pansiyon_A);

                    if (Param.Param_HesapKapamaAds)
                    {
                        Klavye2 klv = new Klavye2();
                        klv.ShowDialog();
                        txt_Not.Text = klv.yazi;
                    }

                    Fis_Islem.Not_Ekle(Convert.ToInt32(this.Tag), txt_Not.Text);

                    MiktarDuzelt();
                }

                gridyenile();

                Bakiye_Kontrol();

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Odeme_Al", "", ex);
            }

            return true;

        }

        public decimal getIndirimFront()
        {
            decimal indirim = 0;
            try
            {
                indirim = Convert.ToDecimal(Fronttools.DegerGetir("select top 1 Rez_Ext_indirim from rez where Rez_Odano='" + odaNo_A + "' and Rez_R_I_H='I' and Rez_Master_id='" + masterFolio_A + "'"));

            }
            catch (Exception ex)
            {

            }

            return indirim;
        }
        private void temizle()
        {
            try
            {
                txt_Hesapno.Text = String.Empty;
                look_Kapatma.EditValue = null;

                musTipi_A = String.Empty;
                odaNo_A = String.Empty;
                folio_A = 0;
                masterFolio_A = 0;
                cari_A = String.Empty;
            }
            catch (Exception ex)
            {

            }



        }

        private void btn_Odemesil_Click(object sender, EventArgs e)
        {
            try
            {
                //Manuel İndirimlerin  Silinmesi
                var look_KapatmaSecili = look_Kapatma.EditValue;


                if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")).StartsWith("MA"))
                {
                    dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Ba ='A' and Rsat_Indkodu = 'MANUEL'");


                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Sil, "Ödeme Silme Fisno : " + Convert.ToString(this.Tag) + " Tutar:" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Tutar")), Convert.ToString(this.Tag), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")));

                    Fis_Islem.ServisPayi(Convert.ToInt32(Convert.ToString(this.Tag)));
                    gridyenile();
                    Bakiye_Kontrol();

                    look_Kapatma.EditValue = null;
                    look_Kapatma.EditValue = look_KapatmaSecili;

                    return;
                }

                if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) == String.Empty || Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "B")
                {
                    MessageBox.Show(res_man.GetString("Sadece Ödeme Satırı Silinebilir..."));
                    return;
                }

                string id = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id"));
                dbtools.execcmd(@"
                            delete from Pos_Carihrk
                            where Chrk_Id in (
                            select Chrk_Id from
                            Pos_Carihrk as chrk left join Cst_Recete_Satis as satis on 
                            chrk.Chrk_Cek = satis.Rsat_Fisno and chrk.Chrk_Cekid = satis.Rsat_Fisno and chrk.Chrk_Tarih = satis.Rsat_Tarih
                            and satis.Rsat_Departman = chrk.Chrk_Depart and satis.Rsat_Kapatma = Chrk_Odeme 
                            and satis.Rsat_Tutar = Chrk_Borc
                            where satis.Rsat_Id = '" + id + "')");


                dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "'");


                if (tip == "O")
                {
                    if (Param.Tesis_Tipi == 0)
                    {
                        Fronttools.execcmd(@"delete from Kumhrk where Kumhrk_Cekno = '" + this.Tag + @"' and  Kumhrk_Re = 'E' 
                            and (Kumhrk_Cekno <> '''' or Kumhrk_Cekno is not null) and Kumhrk_Tarih  = '" + Param.Tarih + "'");
                    }
                }


                MiktarSil();
                Fis_Islem.ServisPayi(Convert.ToInt32(Convert.ToString(this.Tag)));
                gridyenile();
                Bakiye_Kontrol();

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Sil, "Ödeme Silme Fisno : " + Convert.ToString(this.Tag) + " Tutar:" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Tutar")), Convert.ToString(this.Tag), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")));

                look_Kapatma.EditValue = null;
                look_Kapatma.EditValue = look_KapatmaSecili;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                
            }
        }

        private void btn_Indirim_Click(object sender, EventArgs e)
        {
            //if (Param.Param_Yuvarla != "" && Convert.ToDecimal(Param.Param_Yuv_Sayi) > 0)
            //{
            //    dbtools.execcmd("Delete From Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Recete = '" + Param.Param_Yuvarla + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");
            //}

            /*
            if (look_DovizKod.EditValue != null)
            {
                if (!look_DovizKod.EditValue.Equals(Param.Doviz_Kodu))
                {
                    MessageBox.Show("Lütfen Ödeme Şeklini " + Param.Doviz_Kodu + " Seçiniz...");
                    return;
                }
            }*/

            int fisno = Convert.ToInt32(this.Tag);


            var look_KapatmaSecili = look_Kapatma.EditValue;


            YuvarlaModel model = ayarlar.getYuvarlama(Departman.Dep_Kodu);
            if (model != null && model.yuvarlamaFiyat > 0)
            {
                dbtools.execcmd("Delete From Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Recete = '" + model.yuvarlamaRecete + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");
            }



            Indirim ind = new Indirim();
            ind.Tag = "I";
            ind.tutar = Convert.ToDecimal(gridColumn4.SummaryText);
            ind.ShowDialog();


            string neden = "";
            if (Departman.Kodlar_YazSipNedSor && ind.cikisyapti == false)
            {
                Klavye2 klv = new Klavye2();
                klv.ShowDialog();
                neden = klv.yazi == null ? "" : klv.yazi;
            }


            decimal tutar = 0, doviztutar = 0, oran = 0;
            if (ind.indTipi == "T")
            {
                if (Param.Calisma_Sekli == 1)       //Dövizli
                {
                    doviztutar = ind.indSayi;
                    tutar = doviztutar * Param.Doviz_Kuru;
                    if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                    {
                        decimal dovizTLKarsilik = getKurKarsilik(Param.Doviz_Kodu);
                        decimal girilenTLKarsilik = getKurKarsilik(Convert.ToString(look_DovizKod.EditValue));

                        tutar = girilenTLKarsilik * doviztutar;


                        decimal toplamTutar23 = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                        decimal oran23 = (tutar / toplamTutar23) * 100;

                        if (oran23 > User.P_Indirim_Yuzde)
                        {
                            MessageBox.Show("Max Indirim Yuzdesini Aştınız..." + "\n" + "Max İndirim Yüzdeniz : %" + User.P_Indirim_Yuzde.ToString() + "\n" + "Şuan ki İndirim Oranı : %" + oran23.ToString("n2"));
                            return;
                        }

                    }
                }
                else
                {
                    tutar = ind.indSayi;
                    doviztutar = tutar / Param.Doviz_Kuru;

                    decimal toplamTutar23 = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                    decimal oran23 = (tutar / toplamTutar23) * 100;

                    if (oran23 > User.P_Indirim_Yuzde)
                    {
                        MessageBox.Show("Max Indirim Yuzdesini Aştınız..." + "\n" + "Max İndirim Yüzdeniz : %" + User.P_Indirim_Yuzde.ToString() + "\n" + "Şuan ki İndirim Oranı : %" + oran23.ToString("n2"));
                        return;
                    }
                }
            }
            if (ind.indTipi == "Y")
            {
                oran = ind.indSayi;
            }

            if (ind.indTipi == "MY")
            {
                oran = ind.indSayi;
                ind.indTipi = "Y";
            }

            if (oran > 0 || tutar > 0)
            {
                Fis_Islem.Manuel_Indirim(fisno, ind.indTipi, tutar, doviztutar, oran, Split, neden: neden);

                decimal toplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                if (oran > 0)
                {
                    string aciklama = "İNDİRİM UYGULANDI . Fisno:" + fisno + " Masano:" + Masa_No + " İNDİRİM ORANI : " + oran + " İNDİRİMSİZ TOPLAM TUTAR : " + toplamTutar;
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Indirim_Uygula, Log.Log_Islem.Kaydet, aciklama, fisno.ToString(), "", neden: neden);
                }
                else if (tutar > 0)
                {
                    string aciklama = "İNDİRİM UYGULANDI . Fisno:" + fisno + " Masano:" + Masa_No + " İNDİRİM TUTARI : " + tutar + " İNDİRİMSİZ TOPLAM TUTAR : " + toplamTutar;
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Indirim_Uygula, Log.Log_Islem.Kaydet, aciklama, fisno.ToString(), "", neden: neden);
                }

                //gridyenile();
                //Bakiye_Kontrol();
            }


            Fis_Islem.ServisPayi(fisno);
            gridyenile();
            Bakiye_Kontrol();

            Yuvarlama();

            look_Kapatma.EditValue = null;
            look_Kapatma.EditValue = look_KapatmaSecili;

        }

        private void txt_Hesapno_Leave(object sender, EventArgs e)
        {

            if (Param.Tesis_Tipi == 1)
            {
                return;
            }

            if (txt_Hesapno.Text.Length == 0)
            {
                return;
            }

            if (musTipi_A != String.Empty)
            {
                return;
            }

            HesapBul ara = new HesapBul();
            ara.data = txt_Hesapno.Text;
            if (ara.Arama_Yap() != "OK")
            {
                MessageBox.Show(res_man.GetString("Hesap Bulunamadı..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_Hesapno.Text = String.Empty;
                txt_Hesapno.Focus();
                return;
            }

            musTipi_A = ara.Mus_tipi;
            odaNo_A = ara.Oda_No;
            folio_A = ara.Folio;
            masterFolio_A = ara.Master_Folio;
            pansiyon_A = ara.Pansiyon;
            uyeId_A = ara.Uye_Id;
            uyeAdsoyad_A = ara.Uye_Adsoyad;
            uyeKartturu_A = ara.Uye_Kartturu;
            cari_A = ara.Cari_Kod;
            indKodu_A = ara.Ind_Kodu;
            indOran_A = ara.Ind_Oran;
            odemeKodu_A = ara.Odeme_Kodu;
            FolioKart_No = ara.Kart_No;
            //MusteriTipi = ara.MusteriTipi;


            lbl_Bilgi.Text = ara.Bilgi;
            txt_Hesapno.EditValue = odaNo_A;

            if (Convert.ToString(indOran_A) != "")
            {
                Fis_Update();
            }

            gridyenile();
            Bakiye_Kontrol();
        }

        //decimal eksiYap = 1;


        public void onburoExtraIndrimVarsaUygula()
        {
            try
            {
                if (Param.Tesis_Tipi == 0) // önbüro ise
                {
                    decimal indirimOran = getIndirimFront();
                    if (indirimOran != 0)
                    {
                        string query = "exec Pos_Manuel_Indirim @Fisno=" + Convert.ToInt32(this.Tag) + ",@Ind_Tip=N'Y',@Ind_Tutar=N'0',@Ind_Doviztutar=N'0',@Ind_Oran=" + indirimOran.ToString().Replace(",", ".") + ",@Ind_Turu = 'EXTRA'";
                        dbtools.execcmdR(query);
                        gridyenile();

                    }
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "onburoExtraIndrimVarsaUygula", "", ex);
            }
        }
        public void yazdirmadanKapat()
        {
            try
            {


                if (odemeKontrol() == false)
                {
                    return;
                }

                FisKontrol();
                cikis = true;
                btn_Yazdirkapat.Enabled = false;
                btn_Yazdirmadankapat.Enabled = false;


                onburoExtraIndrimVarsaUygula();

               
                if (Odeme_Al())
                {



                    if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                    {

                    }
                    else
                    {
                        if (Bakiye_bul_TL() < 0 && Eksileme == false)
                        {
                            MessageBox.Show(res_man.GetString("Ödeme Tutarını Kontrol ediniz."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);

                            btn_Yazdirkapat.Enabled = User.G_Yazdirkapat;
                            btn_Yazdirmadankapat.Enabled = User.G_Yazdirmadankapat;

                            return;
                        }
                    }


                    Hesap_Kapat();



                    if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                    {

                    }
                    else
                    {
                        if (Bakiye_bul_TL() < 0)
                        {
                            MessageBox.Show(res_man.GetString("Ödeme Tutarlarında Bir Yanlışlık Var."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);

                            btn_Yazdirkapat.Enabled = User.G_Yazdirkapat;
                            btn_Yazdirmadankapat.Enabled = User.G_Yazdirmadankapat;
                            return;

                        }
                    }


                    temizle();

                    if (Departman.Kodlar_Ingenico_IWE)
                    {
                        dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_IWERep = 1 Where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "'");
                    }

                    this.Close();
                }

                if (Departman.Kodlar_Ingenico_IWE)
                {
                    dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_IWERep = 1 Where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "'");
                }

            }
            catch (Exception ex)
            {
                RHMesaj.alertMesaj("Bir hata oldu ", 3);
            }
        }

        private void btn_Yazdirmadankapat_Click(object sender, EventArgs e)
        {
            if (odemeKodSaatAraligindaKapalimi())
            {
                MessageBox.Show("Ödeme kodu kapalı");
                return;
            }
            yazdirmadanKapat();

            indirimYaz();
            eadisyonAc();
        }

        public void eadisyonAc()
        {
            try
            {
                if (E_AdisyonDurum.Checked)
                {
                    // StatikSinif.eadisyonAc();
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "eadisyonAc", "", ex);
            }
        }

        public void indirimYaz()
        {
            try
            {
                dbtools.execcmdR("exec indirimYaz @Rsat_Fisno='" + Convert.ToInt32(this.Tag) + "'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Hesap_Kapat()
        {
            int fis_no = Convert.ToInt32(this.Tag);

            decimal bakiye = Bakiye_bul_TL();

            if (bakiye > Convert.ToDecimal("0,1"))
            {
                MessageBox.Show(res_man.GetString("Ödeme Alındı Fakat Hesap Kapanmadı..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            DataRowView dataRow = look_Kapatma.GetSelectedDataRow() as DataRowView;
            string fisTip = dataRow["Pkod_FisTipi"].ToString().ToLower();
            if (fisTip.Equals("o") || fisTip.Equals("p"))
            {
                dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_Kdv = 0 Where Rsat_Fisno = '" + fis_no + "'");
            }

            if (Param.Tesis_Tipi == 0)
            {
                if (Param.Tarih != Convert.ToDateTime(Fronttools.DegerGetir("select Fis_Curtar from Fishrk where Fis_anah = 1")))
                {
                    MessageBox.Show(res_man.GetString("Önbüro Çalışma Tarihi ile Sistem tarihi Farklı.") + "\n" + res_man.GetString("Sistemden Çıkıp Yeniden Giriniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }




            //if (Param.Tesis_Tipi==1) // 1 ise pos
            //{
            //    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Durum = 'K' where Rsat_Fisno = '" + fis_no.ToString() + "'");
            //}
            //else if(Param.Tesis_Tipi == 0) // önbüro
            //{
            //    Fis_Islem.Onburo_At(fis_no, FolioKart_No, FolioKart_ID == "" ? 0 : Convert.ToInt32(FolioKart_ID));
            //}


            Fis_Islem.Onburo_At(fis_no, FolioKart_No, FolioKart_ID == "" ? 0 : Convert.ToInt32(FolioKart_ID));


            if (Param.Param_OdaKrediCompOdenmez)
            {
                if (Param.Fullcomp_Kodu == (Fronttools.DegerGetir("select Rez_Odeme From Rez Where Rez_Id = '" + folio_A + "'")))
                {
                    string Odenmez = dbtools.DegerGetir("select Pkod_Kod from Pos_Kodlar with(nolock) where Pkod_Sinif = '11' and Pkod_Ozelkod = '2'");
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Kapatma = '" + Odenmez + "' Where Rsat_BA = 'A' and Rsat_Fisno = '" + fis_no + "' ");
                }
            }



            //dbtools.execcmd("update Pos_Masa set Masa_Durum = '0', Masa_Ozel = '' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
            dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 16,@Masano = '" + Masa_No + "',@Dep_Kodu = '" + Departman.Dep_Kodu + "'");
            dbtools.execcmd("Update Cst_Recete_Satis set Rsat_SistemDate = Getdate(), Rsat_AdisyonTR = '" + Convert.ToInt32(chk_AdisyonGR.Checked) + "', E_AdisyonDurum = '" + Convert.ToInt32(E_AdisyonDurum.Checked) + "' where Rsat_Fisno = '" + fis_no + "'");

            if (E_AdisyonDurum.Checked) // raporlar için eklendi-> eadisyon ise adisyondur.
            {
                dbtools.execcmd("Update Cst_Recete_Satis  set Rsat_AdisyonTR = '" + Convert.ToInt32(E_AdisyonDurum.Checked) + "' where Rsat_Fisno = '" + fis_no + "'");
            }

            if (tip == "O")
            {
                dbtools.execcmd("Update Cst_Recete_Satis set Rsat_RecAP = 2 where Rsat_Fisno = '" + fis_no + "'");
            }

            if (Departman.Kodlar_Ingenico_IWE)
            {
                dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_Ingenico_Status = 5 Where Rsat_Fisno = '" + fis_no + "'");
            }

            Main.a.Listele(0);


            parcalimi();
            tumcarileriyaz(fis_no);
            string aciklama = "Fiş Kapatma. Fisno:" + fis_no + " Masano:" + Masa_No.ToString() + " Ödeme Şekli:" + look_Kapatma.Text + " Hesap No:" + txt_Hesapno.Text;

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, aciklama, fis_no.ToString(), "");
        }

        public void tumcarileriyaz(int fisno)
        {
            try
            {
                string cariat = $"select top 1 Rsat_Cari from Cst_Recete_Satis where Rsat_Fisno='{fisno}' and Rsat_Cari<>'' and Rsat_Cari is not null";
                string cari = dbtools.DegerGetir(cariat);
                if (cari == null || cari == "") return;

                dbtools.execcmdR($"Update Cst_Recete_Satis Set Rsat_Cari = '{cari}' Where Rsat_Fisno = {fisno} ");
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "", "", ex);
            }
        }

        public bool parcalimi() // false ise parçalı değildir. true ise parçalıdır
        {
            try
            {
                if (!Masa_No.Contains("_"))
                {
                    return false;
                }
                string anaMasano = Masa_No.Substring(0, Masa_No.IndexOf("_"));

                string varmi = dbtools.DegerGetir("select count(*) as toplam from Pos_Masa where Masa_Depart='" + Departman.Dep_Kodu + "' and Masa_Durum='1' and Masa_No like '" + anaMasano + "[_]%'");

                if (varmi.Equals("0"))
                {
                    dbtools.execcmd("update  Pos_Masa set Masa_Durum='0' where Masa_No='" + anaMasano + "'");
                    return false; // parcali değiltir
                }

            }
            catch (Exception ex)
            {

            }


            return true;
        }

        private decimal Bakiye_bul_TL()
        {
            decimal bakiye = 0;
            string query = "exec Pos_Sorgu @Sorgu_Tipi = 21,@Fisno = '" + this.Tag + "', @Split = '" + Split + "'";
            DataTable dtBakiye = dbtools.SelectTable(query);
            if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
            {
                string srg = $"select sum(case when Rsat_Ba = 'B' then Rsat_Tutar when Rsat_Ba = 'A' then Rsat_Tutar  *-1 end) as DovizBakiye from Cst_Recete_Satis where Rsat_Fisno = '{this.Tag}' and(ISNULL(Rsat_Split, 0) = {Split} or {Split} = 0)";
                DataTable dt = dbtools.SelectTableR(srg);
                bakiye = Convert.ToDecimal(dt.Rows[0][0].ToString());
                return bakiye;

            }
            if (Convert.ToString(dtBakiye.Rows[0]["TLBakiye"]) != "" || Convert.ToString(dtBakiye.Rows[0]["DovizBakiye"]) != "")
            {
                bakiye = Convert.ToDecimal(dtBakiye.Rows[0]["TLBakiye"]);

                if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                {
                    bakiye = Convert.ToDecimal(dtBakiye.Rows[0]["DovizBakiye"]);

                }


                bakiye = Math.Abs(bakiye) < Convert.ToDecimal(0.03) ? 0 : bakiye;
                return bakiye;
            }


            return bakiye;
        }


        private decimal Bakiye_bul_TL2()
        {
            decimal bakiye = 0;

            string srg = $"select  Rsat_Tutar,Rsat_Ba from Cst_Recete_Satis where Rsat_Fisno = '{this.Tag}' and(ISNULL(Rsat_Split, 0) = {Split} or {Split} = 0)";
            DataTable dtOdeme = dbtools.SelectTableR(srg);

            decimal odenen = 0;
            decimal toplam = 0;

            for (int i = 0; i < dtOdeme.Rows.Count; i++)
            {
                decimal tutar = Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]);
                if (dtOdeme.Rows[i]["Rsat_Ba"].ToString() == "A")
                {

                    odenen += tutar;
                }
                else
                {
                    toplam += tutar;
                }

            }
            bakiye = toplam - odenen;
            foreach (DataRow item in dovizKurlari.Rows)
            {
                if (Param.Doviz_Kodu.ToString() == item["MKodlar_Kod"].ToString())
                {
                    bakiye = bakiye / Convert.ToDecimal(item["Doviz_Alis"].ToString());
                    break;
                }

            }

            bakiye = Math.Abs(bakiye) < Convert.ToDecimal(0.03) ? 0 : bakiye;
            return bakiye;


        }

        private decimal Bakiye_Bul(bool dbBakiye)
        {
            decimal bakiye = 0;

            if (dbBakiye)
            {
                //DataTable dtBakiye = dbtools.SelectTable("select sum(case when Rsat_Ba = 'B' then Rsat_Tutar when Rsat_Ba = 'A' then Rsat_Tutar  * -1 end) as TLBakiye,sum(case when Rsat_Ba = 'B' then Rsat_Doviztutar when Rsat_Ba = 'A' then Rsat_Doviztutar  * -1 end) as DovizBakiye from Cst_Recete_Satis where Rsat_Fisno = '" + this.Tag + "' ");
                DataTable dtBakiye = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 21,@Fisno = '" + this.Tag + "', @Split = '" + Split + "'");
                if (Param.Calisma_Sekli == 1)       // Döviz
                {
                    bakiye = Convert.ToDecimal(dtBakiye.Rows[0]["DovizBakiye"]);
                }
                else
                {
                    bakiye = Convert.ToDecimal(dtBakiye.Rows[0]["TLBakiye"]);
                }
            }
            else
            {
                if (Param.Calisma_Sekli == 1) //Döviz
                {
                    bakiye = Convert.ToDecimal(gridColumn5.SummaryText);
                }
                else
                {
                    bakiye = Convert.ToDecimal(gridColumn4.SummaryText);
                }
            }

            bakiye = Math.Abs(bakiye) < Convert.ToDecimal(0.03) ? 0 : bakiye;

            return bakiye;
        }

        public decimal getKurKarsilik(string dovizKodu)
        {
            try
            {
                for (int i = 0; i < gridView2.RowCount; i++)
                {
                    if (dovizKodu == Convert.ToString(gridView2.GetRowCellValue(i, "Mkodlar_Kod")))
                    {
                        return Convert.ToDecimal(gridView2.GetRowCellValue(i, "Kur"));
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }
        public decimal getDovizTutar(decimal doviztutar)
        {

            decimal dovizTLKarsilik = getKurKarsilik(Param.Doviz_Kodu);
            decimal girilenTLKarsilik = getKurKarsilik(Convert.ToString(look_DovizKod.EditValue));

            decimal tlKarsilik = girilenTLKarsilik * doviztutar;
            decimal tutar = tlKarsilik / dovizTLKarsilik;
            return tutar;
        }

        public bool odemeKodSaatAraligindaKapalimi()
        {
            bool kapalimi = false;
            try
            {
                string saatAralik = dbtools.DegerGetir("select top 1 isnull(saatAralikDurdur,'') as saatAralikDurdur from Pos_Kodlar where Pkod_Sinif='11' and Pkod_Kod='" + look_Kapatma.EditValue + "'");

                if (!saatAralik.Equals("") && saatAralik.Contains("-"))
                {
                    string bastext = saatAralik.Split('-')[0].ToString();
                    string bittext = saatAralik.Split('-')[1].ToString();

                    int bassaat = Convert.ToInt32(bastext.Split(':')[0].ToString());
                    int basdk = Convert.ToInt32(bastext.Split(':')[1].ToString());

                    int bitsaat = Convert.ToInt32(bittext.Split(':')[0].ToString());
                    int bitdk = Convert.ToInt32(bittext.Split(':')[1].ToString());

                    string suankiTar = dbtools.DegerGetir("select convert(varchar(10), GETDATE(), 108)");
                    int sqlSaat = Convert.ToInt32(suankiTar.Split(':')[0].ToString());
                    int sqldk = Convert.ToInt32(suankiTar.Split(':')[1].ToString());


                    //bassaat = 15; basdk = 10;
                    //bitsaat = 15; bitdk = 20;
                    //sqlSaat = 15; sqldk = 25;
                    if (sqlSaat >= bassaat && sqlSaat <= bitsaat)
                    {
                        kapalimi = true;
                        if (sqlSaat == bassaat)
                        {
                            if (sqldk < basdk)
                            {
                                kapalimi = false;
                            }
                        }
                        if (sqlSaat == bitsaat)
                        {
                            if (sqldk > bitdk)
                            {
                                kapalimi = false;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return kapalimi;
        }


        public void yazdirKapat()
        {
            try
            {
                if (odemeKodSaatAraligindaKapalimi())
                {
                    MessageBox.Show("Ödeme kodu kapalı");
                    return;
                }

                if (odemeKontrol() == false)
                {
                    return;
                }

                FisKontrol();
                cikis = true;
                btn_Yazdirkapat.Enabled = false;
                btn_Yazdirmadankapat.Enabled = false;

                //AdisyonPr adisyon2 = new AdisyonPr();
                //string cevap2 = adisyon2.Adisyon_Yaz2(Convert.ToInt32(this.Tag));
                //return;

                //Fatura Kesilecekse Tutar için Fiyatın belirlenmesi
                decimal tutar, doviztutar;
                if (Param.Calisma_Sekli == 1)       //Döviz
                {
                    doviztutar = Convert.ToDecimal(txt_Odemetutari.EditValue);
                    tutar = doviztutar * Param.Doviz_Kuru;

                    if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                    {


                        tutar = getDovizTutar(doviztutar);
                    }


                }
                else
                {
                    tutar = Convert.ToDecimal(txt_Odemetutari.EditValue);
                    doviztutar = tutar;
                }


                onburoExtraIndrimVarsaUygula();

                if (Odeme_Al())
                {
                    Hesap_Kapat();


                    if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                    {

                    }
                    else
                    {
                        if (Bakiye_bul_TL() < 0)
                        {
                            MessageBox.Show(res_man.GetString("Ödeme Tutarlarında Bir Yanlışlık Var."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);

                            btn_Yazdirkapat.Enabled = User.G_Yazdirkapat;
                            btn_Yazdirmadankapat.Enabled = User.G_Yazdirmadankapat;

                            return;

                        }
                    }


                    if (chk_Adisyon.Checked)
                    {
                        DialogResult c = DialogResult.Yes;
                        if (Param.Param_Adispr_Uyari)
                        {
                            c = MessageBox.Show(res_man.GetString("Adisyon Dökülecek... Devam Etmek İstiyor Musunuz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        }
                        if (c == System.Windows.Forms.DialogResult.Yes)
                        {
                            AdisyonPr adisyon = new AdisyonPr();
                            string cevap = adisyon.Adisyon_Yaz(Convert.ToInt32(this.Tag), yazdirkapat: "1");
                            if (cevap != "OK")
                            {
                                MessageBox.Show(cevap);
                                return;
                            }
                            adisyon.Adisyon_Sayac_Arttir(Convert.ToInt32(this.Tag));
                        }
                    }
                    else
                    {
                        string cevap = "";
                        FisPr pr = new FisPr();
                        if (Param.Param_YeniHesapDkm)
                        {
                            bool sifirmi = getTutarsifirmi();
                            if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1)
                            {

                                cevap = pr.newHesapDokum2(true, Convert.ToInt32(this.Tag), Split, "* * * HESAP KAPATMA FİŞİ * * *", gridView2);
                            }
                            else
                            {
                                cevap = pr.newHesapDokum(true, Convert.ToInt32(this.Tag), Split, "* * * HESAP KAPATMA FİŞİ * * *", sifirli: sifirmi);
                                // x2
                            }
                        }
                        else
                        {
                            cevap = pr.HesapDokum(false, Convert.ToInt32(this.Tag), Split);
                        }

                        //fis.newHesapDokum(false, Convert.ToInt32(this.Tag), Split);
                        if (cevap != "OK")
                        {
                            MessageBox.Show(cevap);
                            temizle();
                            this.Close();
                            return;
                        }
                    }



                    Fis_Islem.Fatura_Kes(Convert.ToInt32(this.Tag), chk_HesabaFatura.Checked, chk_HesabaFatura.Checked);

                    if (Departman.Kodlar_Ingenico_IWE)
                    {
                        dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_IWERep = 0 Where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "'");
                    }

                    temizle();

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "yazdirKapat", "", ex);
            }
        }
        private void btn_Yazdirkapat_Click(object sender, EventArgs e)
        {
            yazdirKapat();
            indirimYaz();

            eadisyonAc();
        }

        private void FisKontrol()
        {
            if (Convert.ToString(dbtools.DegerGetir("select COUNT(*) from cst_Recete_Satis where Rsat_Fisno = '" + this.Tag + "' and Rsat_Durum = 'K'")) != "0")
            {
                //gridyenile();
                //Bakiye_Kontrol();
                //MessageBox.Show("Fiş Durumu Daha Önce Kapatılmıs...\n\nSayfa Kapatlılıyor..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.Close();
            }
        }

        private void btn_Fisupdate_Click(object sender, EventArgs e)
        {
            Fis_Update();
            gridyenile();
            Bakiye_Kontrol();
        }


        private void look_Kapatma_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (odemeKodSaatAraligindaKapalimi())
                {
                    look_Kapatma.EditValue = null;
                    MessageBox.Show("Ödeme kodu kapalı");
                    return;
                }


                ingenicoButtonVisible();

                string kod = dbtools.DegerGetir("select Pkod_otoKur from Pos_Kodlar where Pkod_Sinif='11' and Pkod_Kod='" + look_Kapatma.EditValue + "'");





                if (kod.Equals(""))
                {
                    look_DovizKod.EditValue = Param.Doviz_Kodu;
                }
                else
                {
                    look_DovizKod.EditValue = kod;
                }



                //btn_Yazdirkapat.Enabled = true;
                //btn_Yazdirmadankapat.Enabled = true;
                btn_Yazdirkapat.Enabled = User.G_Yazdirkapat;
                btn_Yazdirmadankapat.Enabled = User.G_Yazdirmadankapat;

                // txt_Hesapno.Text = string.Empty;

                if (look_Kapatma.EditValue == null)
                {
                    return;
                }

                Odeme_Ozelkod = Convert.ToInt32(dbtools.DegerGetir("select Pkod_Ozelkod from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '11' and Pkod_Kod = '" + Convert.ToString(look_Kapatma.EditValue) + "' "));

                Kapatma_Tekoda();




            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "look_Kapatma_EditValueChanged", "", ex);
            }



        }

        private void Kapatma_Tekoda()
        {
            if (txt_Hesapno.Text != String.Empty)
            {
                return;
            }

            if (look_Kapatma.EditValue == null)
            {
                return;
            }

            DataTable dtOda = dbtools.SelectTable("select isnull(Pkod_Tekoda,0) as Pkod_Tekoda,Pkod_Odano,Pkod_Ozelkod,ISNULL(Pkod_AdisyonPr,0) as Pkod_AdisyonPr,isnull(Pkod_E_Adisyon,0) as Pkod_E_Adisyon from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '11' and Pkod_Kod = '" + Convert.ToString(look_Kapatma.EditValue) + "' ");
            if (dtOda.Rows.Count > 0)
            {
                if (Param.Tesis_Tipi == 0)
                {
                    if (Convert.ToBoolean(dtOda.Rows[0]["Pkod_Tekoda"]))
                    {
                        string odano = Convert.ToString(dtOda.Rows[0]["Pkod_Odano"]);
                        txt_Hesapno.Text = odano;

                        // sonradan ramo ekledi
                        Fronttools.execcmd("update rez set  Rez_Kur_uygulanan ='" + Param.Doviz_Kuru.ToString().Replace(",", ".") + "' where Rez_Odano='" + odano + "' and Rez_R_I_H='I'");

                        txt_Hesapno_Leave(null, null);
                        if (Param.Param_Hesap_Disable) txt_Hesapno.Enabled = false;
                    }
                    Odeme_Ozelkod = Convert.ToInt32(dtOda.Rows[0]["Pkod_Ozelkod"]);
                }

                chk_AdisyonGR.Checked = Convert.ToBoolean(dtOda.Rows[0]["Pkod_AdisyonPr"]);
                E_AdisyonDurum.Checked = Convert.ToBoolean(dtOda.Rows[0]["Pkod_E_Adisyon"]);

                if (E_AdisyonDurum.Checked)
                {
                    checkEditOtoCari.Checked = E_AdisyonDurum.Checked;
                }

            }
        }

        private void btn_Bindirim_Click(object sender, EventArgs e)
        {
            try
            {
                if (Param.Param_Bindirim == "")
                {
                    MessageBox.Show(res_man.GetString("Bindirim Recetesi Tanımlı Değil...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Indirim ind = new Indirim();
                ind.Tag = "B";
                ind.toplamTutar = 0;
                ind.ShowDialog();

                decimal tutar = 0, doviztutar = 0, oran = 0;
                if (ind.indTipi == "T")
                {
                    if (Param.Calisma_Sekli == 1)       //Dövizli
                    {
                        doviztutar = ind.indSayi;
                        tutar = doviztutar * Param.Doviz_Kuru;
                    }
                    else
                    {
                        tutar = ind.indSayi;
                        doviztutar = tutar / Param.Doviz_Kuru;

                        decimal toplamTutar23 = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                        decimal oran23 = (tutar / toplamTutar23) * 100;

                        if (oran23 > User.P_Indirim_Yuzde)
                        {
                            MessageBox.Show("Max Indirim Yuzdesini Aştınız..." + "\n" + "Max İndirim Yüzdeniz : %" + User.P_Indirim_Yuzde.ToString() + "\n" + "Şuan ki İndirim Oranı : %" + oran23.ToString("n2"));
                            return;
                        }
                    }



                }
                if (ind.indTipi == "Y")
                {
                    oran = ind.indSayi;
                }

                if (ind.indTipi == "MY")
                {
                    oran = ind.indSayi;
                    ind.indTipi = "Y";

                }



                if (oran > 0 || tutar > 0)
                {
                    int fisno = Convert.ToInt32(this.Tag);

                    Fis_Islem.Bindirim_Uygula(fisno, ind.indTipi, tutar, doviztutar, oran);

                    decimal toplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                    if (oran > 0)
                    {
                        string aciklama = "SERVİS PAYI UYGULANDI . Fisno:" + fisno + " Masano:" + Masa_No + " SERVİS PAYI ORANI : " + oran + " BİNDİRİMSİZ TOPLAM TUTAR : " + toplamTutar;
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.ServisPayi_Uygula, Log.Log_Islem.Kaydet, aciklama, fisno.ToString(), "");
                    }
                    else if (tutar > 0)
                    {
                        string aciklama = "SERVİS PAYI UYGULANDI . Fisno:" + fisno + " Masano:" + Masa_No + " SERVİS PAYI TUTARI : " + tutar + " BİNDİRİMSİZ TOPLAM TUTAR : " + toplamTutar;
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.ServisPayi_Uygula, Log.Log_Islem.Kaydet, aciklama, fisno.ToString(), "");
                    }

                }

                gridyenile();
                Bakiye_Kontrol();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void servisPayiveTipbox(bool servispayimi = true)
        {

        }
        private void btn_Yuvarla_Click(object sender, EventArgs e)
        {
            YuvarlaModel model = ayarlar.getYuvarlama(Departman.Dep_Kodu);
            if (model == null || model.yuvarlamaFiyat == 0)
            {
                MessageBox.Show(res_man.GetString("Yuvarlama Recetesi Tanımlı Değil..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Satis s = new Satis();
            s.Tag = "M";
            s.bartxt_FisNo.EditValue = Convert.ToInt32(this.Tag);
            s.Miktar = 1;
            s.Masa_No = Masa_No;
            decimal artik = Convert.ToDecimal(txt_Odemetutari.EditValue) % (model.yuvarlamaFiyat / 100);
            s.Yuv_Tutar = (model.yuvarlamaFiyat / 100) - artik;
            s.Urun_Sat(model.yuvarlamaRecete);

            gridyenile();
            Bakiye_Kontrol();
        }

        //private void btn_Yuvarla_ClickEski(object sender, EventArgs e)
        //{
        //    YuvarlaModel model = ayarlar.getYuvarlama(Departman.Dep_Kodu);
        //    if (model == null || model.yuvarlamaFiyat == 0)
        //    {
        //        MessageBox.Show(res_man.GetString("Yuvarlama Recetesi Tanımlı Değil..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    //if (Param.Param_Yuvarla == "")
        //    //{
        //    //    MessageBox.Show(res_man.GetString("Yuvarlama Recetesi Tanımlı Değil..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    //    return;
        //    //}

        //    Satis s = new Satis();
        //    s.Tag = "M";
        //    s.bartxt_FisNo.EditValue = Convert.ToInt32(this.Tag);
        //    s.Miktar = 1;
        //    s.Masa_No = Masa_No;
        //    decimal artik = Convert.ToDecimal(txt_Odemetutari.Text) % (Convert.ToDecimal(Param.Param_Yuv_Sayi) / 100);
        //    s.Yuv_Tutar = (Convert.ToDecimal(Param.Param_Yuv_Sayi) / 100) - artik;
        //    s.Urun_Sat(Param.Param_Yuvarla);

        //    gridyenile();
        //    Bakiye_Kontrol();
        //}

        decimal OdemeTutari = 0;

        public void toplamTutarYaz()
        {
            try
            {
                DataTable dataTable = gridControl2.DataSource as DataTable;
                for (int i = 0; i < gridView2.RowCount; i++)
                {
                    if (Convert.ToString(look_DovizKod.EditValue) == Convert.ToString(gridView2.GetRowCellValue(i, "Mkodlar_Kod")))
                    {

                        //txt_Odemetutari.Text = Convert.ToDecimal(gridView2.GetRowCellValue(i, "Doviz")).ToString("n2");

                        txt_Odemetutari.EditValue = Convert.ToDecimal(gridView2.GetRowCellValue(i, "Doviz")).ToString("n2");

                        OdemeTutari = Convert.ToDecimal(gridView2.GetRowCellValue(i, "Doviz"));
                    }
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "toplamTutarYaz", "", ex);
            }

        }
        private void look_DovizKod_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                toplamTutarYaz();

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "look_DovizKod_EditValueChanged", "", ex);
            }
        }


        public decimal getKurKarsilik(decimal karsilik)
        {
            try
            {
                for (int i = 0; i < gridView2.RowCount; i++)
                {
                    if (Convert.ToString(look_DovizKod.EditValue) == Convert.ToString(gridView2.GetRowCellValue(i, "Mkodlar_Kod")))
                    {
                        return Convert.ToDecimal(gridView2.GetRowCellValue(i, "Kur"));
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return karsilik;
        }

        private void btn_AdisYaz_Click(object sender, EventArgs e)
        {

            AdisyonPr adisyon = new AdisyonPr();
            adisyon.Adisyon_Sayac_Sifirla(Convert.ToInt32(this.Tag));
            string cevap = adisyon.Adisyon_Yaz(Convert.ToInt32(this.Tag));
            if (cevap != "OK")
            {
                MessageBox.Show(cevap);
                return;
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Indirim ind = new Indirim();
            ind.Tag = "MB";
            ind.ShowDialog();

            decimal tutar = 0, doviztutar = 0, oran = 0;
            if (ind.indTipi == "T")
            {
                if (Param.Calisma_Sekli == 1)       //Dövizli
                {
                    doviztutar = ind.indSayi;
                    tutar = doviztutar * Param.Doviz_Kuru;
                }
                else
                {
                    tutar = ind.indSayi;
                    doviztutar = tutar / Param.Doviz_Kuru;
                }
            }
            if (ind.indTipi == "Y")
            {
                oran = ind.indSayi;
            }

            if (oran > 0 || tutar > 0)
            {
                Fis_Islem.Manuel_Bindirim(Convert.ToInt32(this.Tag), ind.indTipi, tutar, doviztutar, oran, Split);
            }

            gridyenile();
            Bakiye_Kontrol();
        }

        SimpleTcpClient client;

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            IngenicoKapat();
        }

        public void onburoCekSil()
        {
            if (Param.Tesis_Tipi == 0)
            {
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Cek_Geri";
                com.Parameters.AddWithValue("@Fis_No", this.Tag.ToString());
                com.Parameters.AddWithValue("@Users", User.P_Kod);
                //com.Parameters.AddWithValue("@Rsat_IptalNot", iptalSebep);
                com.Parameters.AddWithValue("@Onb_Sil", "1");
                com.ExecuteNonQuery();
                if (con.State == ConnectionState.Open) con.Close();

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Fis_Iptal, Log.Log_Islem.Sil, this.Tag.ToString() + " NL Fis Cek Geri Alındı. Ingenicodan hata döndü ", this.Tag.ToString(), String.Empty);
            }
        }

        private void IngenicoKapat()
        {
            cikis = true;
            btn_Yazdirkapat.Enabled = false;
            btn_Yazdirmadankapat.Enabled = false;

            //AdisyonPr adisyon2 = new AdisyonPr();
            //string cevap2 = adisyon2.Adisyon_Yaz2(Convert.ToInt32(this.Tag));
            //return;

            //Fatura Kesilecekse Tutar için Fiyatın belirlenmesi
            decimal tutar, doviztutar;
            if (Param.Calisma_Sekli == 1)       //Döviz
            {
                doviztutar = Convert.ToDecimal(txt_Odemetutari.EditValue);
                tutar = doviztutar * Param.Doviz_Kuru;
            }
            else
            {
                tutar = Convert.ToDecimal(txt_Odemetutari.EditValue);
                doviztutar = tutar;
            }

            decimal bakiye;
            if (Param.Calisma_Sekli == 1) //Döviz
            {
                bakiye = Convert.ToDecimal(gridColumn5.SummaryText);
            }
            else
            {
                bakiye = Convert.ToDecimal(gridColumn4.SummaryText);
            }


            if (Odeme_Al())
            {
                Hesap_Kapat();

                if (Bakiye_bul_TL() < 0 && Eksileme == false)
                {
                    MessageBox.Show(res_man.GetString("Ödeme Tutarını Kontrol ediniz."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //btn_Yazdirkapat.Enabled = true;
                    //btn_Yazdirmadankapat.Enabled = true;
                    btn_Yazdirkapat.Enabled = User.G_Yazdirkapat;
                    btn_Yazdirmadankapat.Enabled = User.G_Yazdirmadankapat;
                    return;
                }

                Bakiye_Kontrol();


                if (Departman.Kodlar_Ingenico)
                {
                    if (Departman.Kodlar_IngenicoCon == 0)
                    {
                        IngenicoForm a = new IngenicoForm();
                        a.Fisno = Convert.ToInt32(this.Tag);
                        a.Islem = "SATIS";
                        a.ShowDialog();

                        if (a.DonenDeger == false)
                        {
                            MessageBox.Show(res_man.GetString("ODEME GERÇEKLEŞTİRİLEMEDİ... Tekrar Deneyiniz"));
                            btn_Yazdirkapat.Enabled = false;
                            btn_Cikis.Enabled = false;

                            onburoCekSil();
                            dbtools.execcmd("Delete From Cst_Recete_Satis Where Rsat_Fisno = '" + this.Tag.ToString() + "' and Rsat_Ba = 'A'");

                            gridyenile();
                            Bakiye_Kontrol();
                            return;
                        }
                    }
                    else
                    {



                        DataTable dtGiden = gridControl1.DataSource as DataTable;

                        IngenicoServer b = new IngenicoServer(dtGiden, bakiye);
                        b.Fisno = Convert.ToInt32(this.Tag);
                        b.ShowDialog();


                        dbtools.execcmdR($"update Cst_Recete_Satis set BankaID='{b.sonucTicket.BkmID}' where Rsat_Fisno='{this.Tag.ToString()}'");
                        // return;

                        string deger=  dbtools.DegerGetir("select isnull(Kodlar_Ingenico_IWEHesap,0) as Kodlar_Ingenico_IWEHesap from Stok_Kodlar where Kodlar_Sinif=01 and Kodlar_Kod='" + Departman.Dep_Kodu + "'");

                        if (deger=="1" || deger.ToLower()=="true")
                        {
                            dbtools.execcmdR($"update Pos_Masa set Masa_Durum='2'  where Masa_No='{Masa_No}'");
                            dbtools.execcmdR($"update Cst_Recete_Satis set Rsat_Durum='A' where Rsat_Fisno='{this.Tag.ToString()}'");
                        }
                       

                        if (b.DonenDeger == false)
                        {
                            MessageBox.Show(res_man.GetString("ODEME GERÇEKLEŞTİRİLEMEDİ... Tekrar Deneyiniz"));
                            btn_Yazdirkapat.Enabled = User.G_Yazdirkapat;
                            btn_Yazdirmadankapat.Enabled = User.G_Yazdirmadankapat;
                            btn_Cikis.Enabled = true;
                            onburoCekSil();

                            dbtools.execcmd("Delete From Cst_Recete_Satis Where Rsat_Fisno = '" + this.Tag.ToString() + "' and Rsat_Ba = 'A' or Rsat_Ba= 'K'");

                            gridyenile();
                            Bakiye_Kontrol();
                            return;
                        }

                    }

                    if (Param.Param_IngenicoSPR)
                    {
                        dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 14, @Fisno = '" + this.Tag.ToString() + "'");
                        FisPr pr = new FisPr();
                        string sonuc = pr.SiparisPr(Convert.ToInt32(this.Tag), false, Split);
                        if (sonuc != "OK")
                        {
                            MessageBox.Show(sonuc);
                        }
                        else
                        {
                            dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr = 1 where Rsat_Fisno = '" + this.Tag.ToString() + "' ");
                        }
                    }



                    if (chk_Adisyon.Checked)
                    {
                        DialogResult c = DialogResult.Yes;
                        if (Param.Param_Adispr_Uyari)
                        {
                            c = MessageBox.Show(res_man.GetString("Adisyon Dökülecek... Devam Etmek İstiyor Musunuz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        }
                        if (c == System.Windows.Forms.DialogResult.Yes)
                        {
                            AdisyonPr adisyon = new AdisyonPr();
                            string cevap = adisyon.Adisyon_Yaz(Convert.ToInt32(this.Tag));
                            if (cevap != "OK")
                            {
                                MessageBox.Show(cevap);
                                return;
                            }
                            adisyon.Adisyon_Sayac_Arttir(Convert.ToInt32(this.Tag));
                        }
                    }
                    else
                    {
                        FisPr fis = new FisPr();

                        if (Param.Param_YeniHesapDkm) // 21.05.2024 eklendi
                        {
                            fis.newHesapDokum(true, Convert.ToInt32(this.Tag), Split, "* * * HESAP DÖKÜM FİŞİ * * *" );
                        }
                        else
                        {
                            string cevap = fis.HesapDokum(false, Convert.ToInt32(this.Tag), Split);
                            if (cevap != "OK")
                            {
                                MessageBox.Show(cevap);
                                temizle();
                                this.Close();
                                return;
                            }
                        }

                        
                    }

                    //Fis_Islem.Fatura_Kes(Convert.ToInt32(this.Tag), chk_HesabaFatura.Checked, chk_HesabaFatura.Checked);

                    temizle();

                    this.Close();
                }
            }
        }

        Label txtStatus = new Label();
        void client_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;
            });
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            IngenicoForm a = new IngenicoForm();
            a.Fisno = Convert.ToInt32(this.Tag);
            a.Islem = "FISIPTAL";

            a.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            ParaUstu a = new ParaUstu();
            a.Fisno = Convert.ToInt32(this.Tag);
            a.ShowDialog();

            if (Departman.Kodlar_Ingenico)
            {
                if (Param.Param_ParaUstuIngenico)
                {
                    IngenicoKapat();
                }
            }
        }

        private void UrunTahsilat()
        {
            decimal ToplamtTutar = 0;

            for (int i = 0; i < gridView1.GetSelectedRows().Length; i++)
            {
                int index = gridView1.GetSelectedRows()[i];

                if (Convert.ToInt32(gridView1.GetRowCellValue(index, "Rsat_UrunTahsilat")) == 0)
                {
                    if (Convert.ToDecimal(gridView1.GetRowCellValue(index, "Rsat_Miktar")) > 1)
                    {
                        Klavye1 klv = new Klavye1();
                        klv.Tag = "MALZEMETR";
                        klv.UrunAdi = Convert.ToString(gridView1.GetRowCellValue(index, "Rec_Ad"));
                        klv.txt_Sayi.Text = Convert.ToDecimal(gridView1.GetRowCellValue(index, "Rsat_Miktar")).ToString();
                        klv.ShowDialog();
                        decimal deger = klv.sayi;

                        if (klv.Cikis)
                        {
                            return;
                        }

                        if (deger <= 0)
                        {
                            return;
                        }

                        if (deger > Convert.ToDecimal(gridView1.GetRowCellValue(index, "Rsat_Miktar")))
                        {
                            MessageBox.Show(res_man.GetString("Hatalı giriş..."));
                            return;
                        }

                        Fis_Islem.Satir_Sil(Convert.ToInt32(gridView1.GetRowCellValue(index, "Rsat_Id")), deger);


                        //int hedefFisno = 0;
                        //hedefFisno = Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));


                        Satis s = new Satis();
                        s.Tag = "M";
                        s.bartxt_FisNo.EditValue = this.Tag;
                        s.Miktar = deger;
                        s.Masa_No = Masa_No;
                        s.Rsat_SiparisPr = true;
                        s.Rsat_AbuyerPr = true;
                        s.Rsat_UrunTahsilat = 1;
                        s.eMiktar = getEmiktarConvert(index);
                        s.Urun_Sat(Convert.ToString(gridView1.GetRowCellValue(index, "Rsat_Recete")));


                        if (deger == Convert.ToDecimal(gridView1.GetRowCellValue(index, "Rsat_Miktar")))
                        {
                            dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView1.GetRowCellValue(index, "Rsat_Id")).ToString() + "'");
                        }
                        ToplamtTutar += (Convert.ToDecimal(gridView1.GetRowCellValue(index, "Rsat_Tutar")) / Convert.ToDecimal(gridView1.GetRowCellValue(index, "Rsat_Miktar"))) * deger;

                        //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Fisno = '" + this.Tag + "' where Rsat_Fisno = '" + hedefFisno + "'");

                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Urun_Tahsilat, Log.Log_Islem.Kaydet, Convert.ToString(gridView1.GetRowCellValue(index, "Rec_Ad")) + " ürünü '" + deger.ToString() + "' miktarı Tahsilatı Yapıldı.", this.Tag.ToString(), Convert.ToInt32(gridView1.GetRowCellValue(index, "Rsat_Id")).ToString(), Convert.ToString(gridView1.GetRowCellValue(index, "Rsat_Recete")), deger);
                    }
                    else
                    {
                        DataTable dtInd = dbtools.SelectTable("select Rsat_Fisno,Rsat_Indsatirid,Rsat_Indsatirid2 from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView1.GetRowCellValue(index, "Rsat_Id")) + "' ");
                        if (dtInd.Rows.Count > 0)
                        {
                            //İndirimlerin Silinmesi
                            dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid"]) + "'");
                            dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid2"]) + "'");
                        }

                        Fis_Islem.Satir_Sil(Convert.ToInt32(gridView1.GetRowCellValue(index, "Rsat_Id")), 1);

                        Satis s = new Satis();
                        s.Tag = "M";
                        s.bartxt_FisNo.EditValue = this.Tag;
                        s.Miktar = 1;
                        s.Masa_No = Masa_No;
                        s.Rsat_SiparisPr = true;// Convert.ToBoolean(gridView1.GetFocusedRowCellValue("Rsat_SiparisPr"));
                        s.Rsat_AbuyerPr = true;
                        s.Rsat_UrunTahsilat = 1;
                        s.eMiktar = getEmiktarConvert(index);
                        s.Urun_Sat(Convert.ToString(gridView1.GetRowCellValue(index, "Rsat_Recete")));

                        ToplamtTutar += (Convert.ToDecimal(gridView1.GetRowCellValue(index, "Rsat_Tutar")));

                        //dbtools.execcmd("update Cst_Recete_Satis set Rsat_UrunTahsilat = 1 where Rsat_Fisno = '" + this.Tag + "'");
                    }

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Urun_Tahsilat, Log.Log_Islem.Kaydet, Convert.ToString(gridView1.GetRowCellValue(index, "Rec_Ad")) + " ürünü 1 miktarı Tahsilatı Yapıldı.", this.Tag.ToString(), Convert.ToInt32(gridView1.GetRowCellValue(index, "Rsat_Id")).ToString(), Convert.ToString(gridView1.GetRowCellValue(index, "Rsat_Recete")), 1);

                }

                //txt_Odemetutari.EditValue = ToplamtTutar; // eskiden buradaydı


                //dbtools.execcmd("update Pos_Masa set Masa_Durum = 1 where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "'");
            }

            txt_Odemetutari.EditValue = ToplamtTutar;

        }

        public string getEmiktarConvert(int index)
        {
            string emiktar = Convert.ToString(gridView1.GetRowCellValue(index, "Rsat_Emiktar"));
            switch (emiktar)
            {
                case "DBL": emiktar = "D"; break;
                case "1BCK": emiktar = "B"; break;
                case "YRM": emiktar = "Y"; break;
                case "": emiktar = "T"; break;
            }
            return emiktar;
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            UrunTahsilat();
            gridyenile(true);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string Urunler = "";

            for (int i = 0; i < gridView1.GetSelectedRows().Length; i++)
            {
                int index = gridView1.GetSelectedRows()[i];
                Urunler += Convert.ToString(gridView1.GetRowCellValue(index, "Rsat_Id")) + ",";
            }
            FisPr pr = new FisPr();
            pr.newUrunBazliHesapDokum(true, Convert.ToInt32(this.Tag), Split, "* * * HESAP DÖKÜM FİŞİ * * *", Urunler.Substring(0, Urunler.Length - 1));

            for (int i = 0; i < gridView1.GetSelectedRows().Length; i++)
            {
                int index = gridView1.GetSelectedRows()[i];
                int ID = Convert.ToInt32(gridView1.GetRowCellValue(index, "Rsat_Id"));
                dbtools.execcmd("update Cst_Recete_Satis Set Rsat_UrunBazliHspDokum = 1 Where Rsat_Id = '" + ID + "'");

            }


            dbtools.execcmd("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Ingenico_Status='1' where Rsat_Fisno='" + Convert.ToInt32(this.Tag) + "'");

            if (Param.Param_Hesap_Kilit)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Hesap_Kilit = 1 where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "'");
            }
            this.Close();
        }


        //private void rBtnArti_Click(object sender, EventArgs e)
        //{
        //    if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "B")
        //    {
        //        txt_Odemetutari.EditValue = 0;
        //        int OdenenMiktar = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_UrunBazliHspAdet"));
        //        //int RsatID = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id"));

        //        if (OdenenMiktar < Convert.ToInt32(gridView1.GetFocusedRowCellValue(gridColumn2)))
        //        {
        //            OdenenMiktar++;
        //            gridView1.SetFocusedRowCellValue("Rsat_UrunBazliHspAdet", OdenenMiktar);
        //            //dbtools.execcmd("Update Cst_Recete_Satis set Rsat_UrunBazliHspAdet = '" + OdenenMiktar + "' where Rsat_Id = '" + RsatID + "'");

        //            OdemeTutari += ((Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")) / Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"))));

        //            txt_Odemetutari.EditValue = OdemeTutari;
        //        }
        //    }
        //}

        //private void rBtnEksi_Click(object sender, EventArgs e)
        //{
        //    if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "B")
        //    {
        //        txt_Odemetutari.EditValue = 0;
        //        int OdenenMiktar = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_UrunBazliHspAdet"));
        //        //int RsatID = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id"));

        //        if (OdenenMiktar <= Convert.ToInt32(gridView1.GetFocusedRowCellValue(gridColumn2)))
        //        {
        //            OdenenMiktar--;
        //            OdenenMiktar = (OdenenMiktar < 0 ? 0 : OdenenMiktar);
        //            gridView1.SetFocusedRowCellValue("Rsat_UrunBazliHspAdet", OdenenMiktar);

        //            //dbtools.execcmd("Update Cst_Recete_Satis set Rsat_UrunBazliHspAdet = '" + OdenenMiktar + "' where Rsat_Id = '" + RsatID + "'");

        //            OdemeTutari -= ((Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")) / Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"))));

        //            txt_Odemetutari.EditValue = ((OdemeTutari < 0 ? 0 : OdemeTutari));
        //        }
        //    }
        //}

        private void MiktarDuzelt()
        {
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (Convert.ToString(gridView1.GetRowCellValue(i, "Rsat_Ba")) == "B")
                {
                    dbtools.execcmd("Update Cst_Recete_Satis set Rsat_UrunBazliHspAdet = '" + Convert.ToString(gridView1.GetRowCellValue(i, "Rsat_UrunBazliHspAdet")) + "' where Rsat_Id = '" + Convert.ToString(gridView1.GetRowCellValue(i, "Rsat_Id")) + "'");
                }
            }
        }

        private void MiktarSil()
        {
            int s = Convert.ToInt32(dbtools.DegerGetir("select Count(*) From Cst_Recete_Satis Where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Ba = 'A'"));

            if (s == 0)
            {
                dbtools.execcmd("Update Cst_Recete_Satis set Rsat_UrunBazliHspAdet = 0 where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "'");
            }
        }

        bool canClose = false;
        private void Hesap_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = !canClose;
        }

        private void txt_Odemetutari_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {

            try
            {
                if (e.Value != null)
                {
                    if (e.Value is decimal)
                    {
                        decimal val = Math.Round((decimal)e.Value, 2, MidpointRounding.AwayFromZero);

                        if (val == 0)
                        {
                            e.DisplayText = "0";
                        }

                    }
                    else if (e.Value.ToString().Length > 0)
                    {
                        decimal val;
                        decimal.TryParse(e.Value.ToString(), out val);
                        val = Math.Round((decimal)val, 2, MidpointRounding.AwayFromZero);
                        txt_Odemetutari.EditValue = val;

                        if (val == 0)
                        {
                            e.DisplayText = "0";
                        }
                    }
                    else if (e.Value.ToString().Length == 0)
                    {
                        e.DisplayText = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                //RHMesaj.MyMessageError(MyClass, "txt_Odemetutari_CustomDisplayText", "", ex);
            }

        }

        private void Hesap_FormClosed(object sender, FormClosedEventArgs e)
        {
            MasaTakip.hes = null;
        }

        private void btnFisIptal_Click(object sender, EventArgs e)
        {
            LoginSadeceForm loginSadeceForm = new LoginSadeceForm();
            loginSadeceForm.ShowDialog();

            if (loginSadeceForm.okey == false)
            {
                RHMesaj.alertMesaj("ŞİFRE YANLIŞ");
                return;
            }

            FisIptal();
        }

        private void FisIptal()
        {
            try
            {
                Param.Param_Yukle();
                string iptalSebep = String.Empty;

                string Fisno = this.Tag.ToString();

                DateTime Tarih = Convert.ToDateTime(dbtools.DegerGetir("select top 1 Rsat_Tarih from Cst_Recete_Satis where Rsat_Fisno='" + Fisno + "'"));


                if (User.P_Kod.ToUpper() != "RMOS")//!User.R_Fisiptalgecmis
                {
                    if (Tarih.Date != Param.Tarih.Date)
                    {
                        MessageBox.Show(res_man.GetString("Farklı Tarihteki Fişi İptal Edemezsiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                Klavye2 klavye = new Klavye2();
                klavye.Tag = "FISIPTAL";
                klavye.ShowDialog();
                iptalSebep = klavye.yazi;

                if (iptalSebep.Length < 1)
                {
                    return;
                }


                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Cek_Iptal";
                com.Parameters.AddWithValue("@Fis_No", Fisno);
                com.Parameters.AddWithValue("@Users", User.P_Kod);
                com.Parameters.AddWithValue("@Rsat_IptalNot", iptalSebep);
                com.Parameters.AddWithValue("@Onb_Sil", Tarih.Date != Param.Tarih.Date ? 0 : 1);
                com.ExecuteNonQuery();
                if (con.State == ConnectionState.Open) con.Close();

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Fis_Iptal, Log.Log_Islem.Sil, Fisno + " NL Fis Silindi", Fisno, String.Empty);

                gridyenile();

                if (Departman.Kodlar_AndPos_NFC == true)
                {
                    FisPr fis = new FisPr();
                    string sonuc = fis.IptalPrNFC(Convert.ToInt32(Fisno));
                }


                fisno = -2; // fiş iptalden gidiyordur
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA " + ex.Message);
            }


        }

        private void btnSpSil_Click(object sender, EventArgs e)
        {
            int fisno = Convert.ToInt32(this.Tag);

            string masaAd = dbtools.DegerGetir("select top 1 Rsat_Masa from Cst_Recete_Satis where Rsat_Fisno='" + fisno + "'");


            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) == String.Empty)
            {
                return;
            }

            string receteKod = gridView1.GetFocusedRowCellValue("Rsat_Recete").ToString();

            if (!receteKod.Equals(Param.Param_Bindirim))
            {

                if (receteKod.Equals(Param.tipboxReceteKod))
                {

                }
                else
                {
                    MessageBox.Show("Sadece servis payı silinebilir ! ");
                    return;
                }

            }



            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "A")
            {
                MessageBox.Show(res_man.GetString("Ödemeler veya İndirimler Silinemez.."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            bool Rsat_SiparisPr = Convert.ToBoolean(dbtools.DegerGetir("select ISNULL((select ISNULL(Rsat_SiparisPr,0) from Cst_Recete_Satis WHERE Rsat_Id = " + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "),0)"));


            if (Rsat_SiparisPr && !User.G_Satirsil_Y)
            {
                MessageBox.Show(res_man.GetString("Yazdırılmış Satır Silme Yetkiniz Yoktur...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }


            decimal Sil_Miktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
            if (Sil_Miktar > 1 && Convert.ToInt32(dbtools.DegerGetir("SELECT COUNT(*) FROM Cst_Recete_Satis WITH(NOLOCK) WHERE Rsat_Id = " + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + " AND ISNULL(Rsat_AdisyonPr,0) = 0")) > 0)
            {
                Klavye1 klv = new Klavye1();
                klv.txt_Sayi.Text = Sil_Miktar.ToString();
                klv.Tag = "SATIRSIL";
                klv.ShowDialog();
                if (klv.Cikis)
                {
                    return;
                }
                if (klv.sayi <= 0)
                {
                    return;
                }
                if (Sil_Miktar < klv.sayi)
                {
                    MessageBox.Show(res_man.GetString("Hatalı Giriş..."));
                    return;
                }
                Sil_Miktar = klv.sayi;
            }

            string neden = "";
            if (Rsat_SiparisPr && Departman.Kodlar_YazSipNedSor)
            {
                Klavye2 klv = new Klavye2();
                klv.ShowDialog();

                if (klv.yazi == null)
                {
                    MessageBox.Show(res_man.GetString("Hatalı Giriş..."));
                    return;
                }

                if (klv.yazi.Length == 0)
                {
                    MessageBox.Show(res_man.GetString("Hatalı Giriş..."));
                    return;
                }

                neden = klv.yazi;
            }

            if (Departman.Siparis)
            {
                FisPr fis = new FisPr();
                string sonuc = fis.IptalPr(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), Sil_Miktar);
                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc);
                }
            }

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Satir_Sil, Log.Log_Islem.Sil, "Recete : " + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Miktar : " + Sil_Miktar + " Silindi", fisno.ToString(), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")), Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")), Sil_Miktar, neden, Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")));

            Fis_Islem.ServisPayi_Sil(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), Sil_Miktar);


            int satirsay = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + fisno.ToString() + "' and Rsat_Ba = 'B' "));
            if (satirsay == 0)
            {
                string query = "update Pos_Masa set Masa_Durum = 0, Masa_Ozel = '' where Masa_No = '" + masaAd + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'";
                dbtools.execcmd(query);
                dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Fisno = '" + fisno.ToString() + "'");
            }

            gridyenile();
        }

        public static string MyClass = "Hesap";

        private void Hesap_Shown(object sender, EventArgs e)
        {

            try
            {
                StatikSinif.masaKilitle(Masa_No);

                //if (Param.Calisma_Sekli == 1)
                //{
                //    decimal toplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Doviztutar"].SummaryItem.SummaryValue);
                //    txt_Odemetutari.EditValue = toplamTutar;
                //    txt_Odemetutari.Text = toplamTutar.ToString();
                //}


                otoIndirimYuvarlama();

                try
                {
                    if (otoYazdirmadanKapat)
                    {
                        yazdirmadanKapat();
                        this.Close();

                        if (Program.main.paketCallCenter != null)
                        {
                            Program.main.paketCallCenter.gridyenile();
                        }
                    }
                }
                catch (Exception ex)
                {
                    RHMesaj.alertMesaj("HATA Hesap-> Hesap_shown " + ex.Message);
                }




                string kod = dbtools.DegerGetir("select Pkod_otoKur from Pos_Kodlar where Pkod_Sinif='11' and Pkod_Kod='" + look_Kapatma.EditValue + "'");

                if (kod.Equals(""))
                {
                    look_DovizKod.EditValue = Param.Doviz_Kodu;
                }
                else
                {
                    try
                    {
                        look_DovizKod.EditValue = null;
                        look_DovizKod.EditValue = kod;
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Hesap_Shown", "", ex);
            }


        }

        public void otoIndirimYuvarlama()
        {
            try
            {
                AyarlarController ayarlar = new AyarlarController();
                IndirimModel model = ayarlar.getIndirimModel();
                if (model.aktif == false)
                {
                    return;
                }

                decimal toplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);
                decimal virguldensonrakiSayi = toplamTutar % 1;

                if (virguldensonrakiSayi > 0)
                {
                    int fisno = Convert.ToInt32(this.Tag);

                    Fis_Islem.Manuel_Indirim(fisno, "T", virguldensonrakiSayi, virguldensonrakiSayi, 0, 0);

                    string aciklama = "OTOMATİK İNDİRİM UYGULANDI . Fisno:" + fisno + " Masano:" + Masa_No + " İNDİRİM TUTARI : " + toplamTutar + " İNDİRİMSİZ TOPLAM TUTAR : " + toplamTutar;


                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Indirim_Uygula, Log.Log_Islem.Kaydet, aciklama, fisno.ToString(), "");

                    gridyenile();
                    Bakiye_Kontrol();
                }


            }
            catch (Exception ex)
            {
                //RHMesaj.MyMessageError(MyClass, "otoIndirimYuvarlama", "",ex);
            }
        }

        private void txt_Odemetutari_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '-')
            {
                e.Handled = true;
            }
        }

        private void txt_Odemetutari_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Param.Calisma_Sekli == 1)
                {
                    decimal textToplam = Convert.ToDecimal(txt_Odemetutari.EditValue);
                    decimal maxDeger = Convert.ToDecimal(gridView1.Columns["Rsat_Doviztutar"].SummaryItem.SummaryValue);
                    //DataTable dataTable = gridControl2.DataSource as DataTable;

                    for (int i = 0; i < gridView2.RowCount; i++)
                    {
                        if (Convert.ToString(look_DovizKod.EditValue) == Convert.ToString(gridView2.GetRowCellValue(i, "Mkodlar_Kod")))
                        {

                            maxDeger = Convert.ToDecimal(gridView2.GetRowCellValue(i, "Doviz"));
                            break;
                        }
                    }

                    if (textToplam > maxDeger)
                    {
                        txt_Odemetutari.EditValue = maxDeger;
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void textEdit4_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            int index = e.ControllerRow;
        }

        private void btnParcaliOde_Click(object sender, EventArgs e)
        {
            ParcaliOdeme parcaliOdeme = new ParcaliOdeme(this.Tag.ToString(), Masa_No);
            parcaliOdeme.ShowDialog();
        }

        private void checkEditOtoCari_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditOtoCari.Checked)
            {
                DataTable dataTable = dbtools.SelectTableR("select top 1  Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tip from Pos_Cari where Cari_Vergino='11111111111'");

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Cari_Vergino->11111111111 haneli Cari(C) Açınız! ");
                    return;
                }
                cari_A = dataTable.Rows[0]["Cari_Kod"].ToString();
                musTipi_A = dataTable.Rows[0]["Cari_Tip"].ToString();

                lbl_Bilgi.Text = "Cari : " + cari_A;
                txtCariAd.Text = dataTable.Rows[0]["Cari_Ad"].ToString() + " " + dataTable.Rows[0]["Cari_Soyad"].ToString();

            }
            else
            {
                lbl_Bilgi.Text = "...";
                txtCariAd.Text = "Cari Ad";
            }


        }

        private void btnTipBox_Click(object sender, EventArgs e)
        {
            try
            {
                if (Param.tipboxReceteKod == "")
                {
                    MessageBox.Show(res_man.GetString("Bindirim Recetesi Tanımlı Değil...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TipboxForm ind = new TipboxForm();
                ind.Tag = "B";
                ind.toplamTutar = 0;
                ind.ShowDialog();

                decimal tutar = 0, doviztutar = 0, oran = 0;
                if (ind.indTipi == "T")
                {
                    if (Param.Calisma_Sekli == 1)       //Dövizli
                    {
                        doviztutar = ind.indSayi;
                        tutar = doviztutar * Param.Doviz_Kuru;
                    }
                    else
                    {
                        tutar = ind.indSayi;
                        doviztutar = tutar / Param.Doviz_Kuru;
                    }
                }


                if (oran > 0 || tutar > 0)
                {
                    int fisno = Convert.ToInt32(this.Tag);

                    Fis_Islem.Bindirim_UygulaTipBox(fisno, ind.indTipi, tutar, doviztutar, oran);

                    decimal toplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                    string aciklama = "Tip Box UYGULANDI . Fisno:" + fisno + " Masano:" + Masa_No + " Tip Box TUTARI : " + tutar + " BİNDİRİMSİZ TOPLAM TUTAR : " + toplamTutar;
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.ServisPayi_Uygula, Log.Log_Islem.Kaydet, aciklama, fisno.ToString(), "");

                }

                gridyenile();
                Bakiye_Kontrol();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}

