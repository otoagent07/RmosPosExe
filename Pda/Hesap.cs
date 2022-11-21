using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;
using System.Data.SqlClient;
using Pos.Print;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace Pda
{
    public partial class Hesap : DevExpress.XtraEditors.XtraForm
    {

        public string Masa_No = String.Empty;
        //string musTipi = String.Empty;



        string musTipi_A = String.Empty;
        string odaNo_A = String.Empty;
        int folio_A = 0;
        string pansiyon_A = String.Empty;
        int uyeId_A = 0;
        string uyeAdsoyad_A = String.Empty;
        string uyeKartturu_A = String.Empty;
        string cari_A = String.Empty;
        string indKodu_A = String.Empty;
        decimal indOran_A = 0;
        string odemeKodu_A = String.Empty;



        public Hesap()
        {
            InitializeComponent();
        }

  

        private void Hesap_Load(object sender, EventArgs e)
        {

      

            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;

            if (Param.Tesis_Tipi == 1)
            {
                txt_Hesapno.Properties.ReadOnly = true;
            }

            btn_YazdirKapat.Enabled = User.G_Yazdirkapat;
            btn_Yazdirmadankapat.Enabled = User.G_Yazdirmadankapat;
            btn_Odeme.Enabled = User.G_Odemeal;
            btn_OdemeSil.Enabled = User.G_Odemesil;
            btn_Indirim.Enabled = User.G_Indirim_Hesap;


            gridyenile();
            Bakiye_Kontrol();

            look_Kapatma.Properties.DataSource = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad from Pos_Kodlar with(nolock) where Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' and Pkod_Ozelkod <> '8'");
            look_Kapatma.Properties.DisplayMember = "Pkod_Ad";
            look_Kapatma.Properties.ValueMember = "Pkod_Kod";

        }

        private void gridyenile()
        {
            gridColumn1.FieldName = "Rec_Ad";
            gridColumn2.FieldName = "Rsat_Miktar";
            gridColumn3.FieldName = "Rsat_Emiktar";
            gridColumn4.FieldName = "Rsat_Tutar";
            gridColumn5.FieldName = "Rsat_Doviztutar";
            gridColumn6.FieldName = "Rsat_Ba";
            gridColumn7.FieldName = "Rsat_Id";

            if (Param.Calisma_Sekli == 1)
            {
                gridColumn5.Visible = true;
                gridColumn5.VisibleIndex = 4;
            }
            else
            {
                gridColumn4.Visible = true;
                gridColumn4.VisibleIndex = 4;
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Convert.ToInt32(this.Tag));
            com.Parameters.AddWithValue("@Rapor_Tipi", 2);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl1.DataSource = dt;
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Indirim_Click(object sender, EventArgs e)
        {
            Indirim ind = new Indirim();
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
                Fis_Islem.Manuel_Indirim(Convert.ToInt32(this.Tag), ind.indTipi, tutar, doviztutar, oran, 0);

                gridyenile();
                Bakiye_Kontrol();
            }
        }

        private void Bakiye_Kontrol()
        {
            decimal bakiye = Bakiye_Bul();
            txt_Odemetutari.EditValue = bakiye;

            if (bakiye == 0)
            {
                btn_Odeme.Enabled = false;
            }
            else
            {
                btn_Odeme.Enabled = User.G_Odemeal;
            }
        }

        private decimal Bakiye_Bul()
        {
            decimal bakiye = 0;

            if (Param.Calisma_Sekli == 1) //Döviz
            {
                bakiye = Convert.ToDecimal(gridColumn5.SummaryText);
            }
            else
            {
                bakiye = Convert.ToDecimal(gridColumn4.SummaryText);
            }
            return bakiye;
        }

        private void btn_OdemeSil_Click(object sender, EventArgs e)
        {
            //Manuel İndirimlerin  Silinmesi
            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")).StartsWith("MANUEL"))
            {
                dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Ba ='A' and Rsat_Indkodu = 'MANUEL' ");

                gridyenile();
                Bakiye_Kontrol();
                return;
            }

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) == String.Empty || Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "B")
            {
                MessageBox.Show("Sadece Ödeme Satırı Silinebilir...");
                return;
            }

            dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "'");

            gridyenile();
            Bakiye_Kontrol();
        }

        private void btn_HesapDok_Click(object sender, EventArgs e)
        {
            string fisno = this.Tag.ToString();
            if (Departman.Kodlar_Hesap_Adisyon)
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
            else
            {
                FisPr pr = new FisPr();
                //pr.HesapDokum(true, Convert.ToInt32(this.Tag), 0);
                if (Param.Param_YeniHesapDkm)
                {
                    pr.newHesapDokum(true, Convert.ToInt32(this.Tag), 0, "* * * HESAP DÖKÜMÜ * * *");
                }
                else
                {
                    pr.HesapDokum(false, Convert.ToInt32(this.Tag), 0);
                }
                //pr.HesapDokum(true, Convert.ToInt32(this.Tag), 0);
            }

            dbtools.execcmd("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Ingenico_Status='1' where Rsat_Fisno='" + fisno + "'");

            if (Param.Param_Hesap_Kilit)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Hesap_Kilit = 1 where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "'");
            }

            this.Close();
        }

        private void btn_Tutar_Click(object sender, EventArgs e)
        {
            Klavye1 k = new Klavye1();
            k.Tag = "ODEMETUTAR";
            k.ShowDialog();

            txt_Odemetutari.EditValue = k.sayi;
        }

        private void btn_HesapAra_Click(object sender, EventArgs e)
        {
            if (Param.Tesis_Tipi == 0)
            {
                Hesap_Ara();
            }
            else
            {
                DataTable dt = dbtools.SelectTable("select Pkod_Ozelkod from Pos_Kodlar where Pkod_Sinif = '11' and Pkod_Ozelkod in ('5','2','3') and Pkod_Kod = '" + Convert.ToString(look_Kapatma.EditValue) + "'");
                if (dt.Rows.Count > 0)
                {
                    Arama ara = new Arama();
                    ara.Odeme_Ozelkod = Convert.ToInt32(dt.Rows[0]["Pkod_Ozelkod"]);
                    ara.ShowDialog();
                    if (ara.Cari_Kod != "")
                    {
                        txt_Hesapno.Text = ara.Cari_Kod;
                        txt_Hesapno.Visible = true;
                        txt_Hesapno.Properties.ReadOnly = true;

                        musTipi_A = "C";
                        cari_A = ara.Cari_Kod;
                        lbl_Bilgi.Text = ara.Cari_Ad;

                        Fis_Update();
                        gridyenile();
                        Bakiye_Kontrol();
                    }
                }
            }
        }

        private void Hesap_Ara()
        {
            HesapBul ara = new HesapBul();
            ara.data = txt_Hesapno.Text;
            if (ara.Arama_Yap() != "OK")
            {
                MessageBox.Show("Hesap Bulunamadı...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_Hesapno.Text = String.Empty;
                return;
            }

            musTipi_A = ara.Mus_tipi;
            odaNo_A = ara.Oda_No;
            folio_A = ara.Folio;
            pansiyon_A = ara.Pansiyon;
            uyeId_A = ara.Uye_Id;
            uyeAdsoyad_A = ara.Uye_Adsoyad;
            uyeKartturu_A = ara.Uye_Kartturu;
            cari_A = ara.Cari_Kod;
            indKodu_A = ara.Ind_Kodu;
            indOran_A = ara.Ind_Oran;
            odemeKodu_A = ara.Odeme_Kodu;

            lbl_Bilgi.Text = ara.Bilgi;

            Fis_Update();
            gridyenile();
            Bakiye_Kontrol();
        }

        private void Fis_Update()
        {
            if (musTipi_A != String.Empty)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_MusTipi = '" + musTipi_A + "',Rsat_Odano = '" + odaNo_A + "',Rsat_Folio = '" + folio_A + "',Rsat_Pansiyon = '" + pansiyon_A + "',Rsat_Uye_Id = '" + uyeId_A + "', "
                                        + " Rsat_Uye_Ad = '" + uyeAdsoyad_A + "',Rsat_Uye_Kart_Turu = '" + uyeKartturu_A + "',Rsat_Cari = '" + cari_A + "',Rsat_Indkodu = '" + indKodu_A + "',Rsat_Indoran = '" + indOran_A.ToString().Replace(",", ".") + "' "
                                        + " where Rsat_Fisno = '" + this.Tag + "' and Rsat_Ba = 'B' ");
                dbtools.execcmd("exec Pos_Satis_Induyg @Fisno = " + this.Tag);
            }
        }

        private void btn_Odeme_Click(object sender, EventArgs e)
        {
            Odeme_Al();

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

            temizle();
        }

        private bool Odeme_Al()
        {
            if (txt_Hesapno.Text == String.Empty && Param.Tesis_Tipi == 0)
            {
                MessageBox.Show("Kapatma Hesabını Seciniz...");
                return false;
            }

            if (look_Kapatma.EditValue == null)
            {
                MessageBox.Show("Kapatma Kodunu Seçiniz...");
                return false;
            }
            int ozelkod = Convert.ToInt32(dbtools.DegerGetir("select Pkod_Ozelkod from Pos_Kodlar where Pkod_Sinif = '11' and Pkod_Kod = '" + Convert.ToString(look_Kapatma.EditValue) + "'"));
            if (((ozelkod == 5 || ozelkod == 2 || ozelkod == 3) && Param.Tesis_Tipi == 1) && txt_Hesapno.Text == String.Empty)
            {
                MessageBox.Show("Kapatma Hesabını Seciniz...");
                return false;
            }


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

            Fis_Islem.Odeme_Al(Convert.ToInt32(this.Tag), tutar, doviztutar, Convert.ToString(look_Kapatma.EditValue), musTipi_A, odaNo_A, folio_A, cari_A, 0, Param.Doviz_Kodu, false);
            Fis_Islem.Satis_Tip(Convert.ToInt32(this.Tag), Convert.ToString(look_Kapatma.EditValue), pansiyon_A);

            gridyenile();
            Bakiye_Kontrol();

            return true;
        }

        private void temizle()
        {
            txt_Hesapno.Text = String.Empty;
            look_Kapatma.EditValue = null;

            musTipi_A = String.Empty;
            odaNo_A = String.Empty;
            folio_A = 0;
            cari_A = String.Empty;
        }

        private void btn_YazdirKapat_Click(object sender, EventArgs e)
        {
            //Fatura Kesilecekse Tutar için Fiyatın belirlenmesi
            Kapat(true);
        }

        private void Kapat(bool Yazdir)
        {
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


            if (Odeme_Al())
            {
                Hesap_Kapat();

                if (Yazdir)
                {
                    if (Departman.Adisyon)
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
                    else
                    {
                        FisPr fis = new FisPr();
                        //string cevap = fis.HesapDokum(false, Convert.ToInt32(this.Tag), 0);
                        string cevap = "";
                        if (Param.Param_YeniHesapDkm)
                        {
                            cevap = fis.newHesapDokum(true, Convert.ToInt32(this.Tag), 0, "* * * HESAP KAPATMA FİŞİ * * *");
                        }
                        else
                        {
                            cevap = fis.HesapDokum(false, Convert.ToInt32(this.Tag), 0);
                        }
                        if (cevap != "OK")
                        {
                            MessageBox.Show(cevap);
                            return;
                        }
                    }
                }
                temizle();

                this.Close();
            }

        }


        //

        private void Hesap_Kapat()
        {
            //lang_tr
         
            //ResourceManager res_man = new ResourceManager("Dil.lang_" + , Assembly.GetExecutingAssembly());

            if (Bakiye_Bul() != 0)
            {
                MessageBox.Show("Ödeme Alındı Fakat Hesap Kapanmadı...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Fis_Islem.Onburo_At(Convert.ToInt32(this.Tag), "0", 0);

            dbtools.execcmd("update Pos_Masa set Masa_Durum = '0', Masa_Ozel = '' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
        }

        private void txt_Hesapno_Leave(object sender, EventArgs e)
        {
            if (Param.Tesis_Tipi == 0)
            {
                Hesap_Ara();
            }
        }

        private void txt_Hesapno_Click(object sender, EventArgs e)
        {
            //Klavye1 klv = new Klavye1();
            //klv.ShowDialog();
            //txt_Hesapno.Text = klv.deger;
        }

        private void btn_Yazdirmadankapat_Click(object sender, EventArgs e)
        {
            Kapat(false);
        }



    }
}