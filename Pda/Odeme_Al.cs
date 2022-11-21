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

namespace Pda
{
    public partial class Odeme_Al : DevExpress.XtraEditors.XtraForm
    {
        public string Satis_Tip = String.Empty;
        public string Odeme_Kodu = String.Empty;
        public string Masa_No = String.Empty;

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

        string Bilgi = String.Empty;

        public Odeme_Al()
        {
            InitializeComponent();
        }

        private void Odeme_Al_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;

            if (Satis_Tip == "D")
            {
                btn_Cikis.Enabled = false;
                this.ControlBox = false;
            }

            if (Param.Tesis_Tipi == 1)
            {
                panelControl1.Visible = false;
            }

            txt_Hesapno.Text = dbtools.DegerGetir("select case when isnull(Pkod_Tekoda,0) = 1 then Pkod_Odano else '' end from Pos_Kodlar where Pkod_Sinif = '11' and Pkod_Kod = '" + Odeme_Kodu + "'");
            Hesap_Ara();

            gridyenile();

            this.Text = "Odeme Al - " + this.Tag.ToString();
        }

        private void Hesap_Ara()
        {
            if (txt_Hesapno.Text == String.Empty)
            {
                return;
            }
            if (Param.Tesis_Tipi == 0)
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

                //lbl_Bilgi.Text = ara.Bilgi;
                Bilgi = ara.Bilgi;


                DataTable dt = Fronttools.SelectTable("select top 1 Rez_Odano,Rez_Adi_1 +' '+ Rez_Adi_2 as AdSoyad, Rez_Kartno,Rez_Konaklama,Rez_Giris_tarihi,Rez_Cikis_tarihi, "
                                            + " convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,Ac_Adi "
                                            + " from Rez left join Acenta on Rez_Macenta = Acenta.Ac_Kodu "
                                            + " where (Rez_Odano = '" + odaNo_A + "') and (Rez_Master_detay = 'M' or Rez_Master_detay = 'D' or Rez_Master_detay = 'E') and Rez_R_I_H = 'I'");

                if (dt.Rows.Count > 0)
                {

                    lblAdSoyad.Text = Convert.ToString(dt.Rows[0]["AdSoyad"]);
                    lblOdaNo.Text = Convert.ToString(dt.Rows[0]["Rez_Odano"]);
                    lblKonaklama.Text = Convert.ToString(dt.Rows[0]["Rez_Konaklama"]);
                    lblGirisTarihi.Text = Convert.ToDateTime(dt.Rows[0]["Rez_Giris_tarihi"].ToString()).ToShortDateString();
                    lblCikisTarihi.Text = Convert.ToDateTime(dt.Rows[0]["Rez_Cikis_tarihi"].ToString()).ToShortDateString();
                    lblAcenta.Text = Convert.ToString(dt.Rows[0]["Ac_Adi"]);
                }
            }

            Fis_Update();
            gridyenile();
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

        private void btn_HesapAra_Click(object sender, EventArgs e)
        {
            Hesap_Ara();
        }

        private void txt_Hesapno_Leave(object sender, EventArgs e)
        {
            Hesap_Ara();
        }

        private void btn_Odeme_Click(object sender, EventArgs e)
        {
            Kapat(true);
        }

        private void Kapat(bool yazdir)
        {
            decimal tutar, doviztutar;
            if (Param.Calisma_Sekli == 1)       //Döviz
            {
                doviztutar = Convert.ToDecimal(gridColumn5.SummaryText);
                tutar = doviztutar * Param.Doviz_Kuru;
            }
            else
            {
                tutar = Convert.ToDecimal(gridColumn4.SummaryText);
                doviztutar = tutar;
            }


            if (Fis_Odeme_Al())
            {
                Hesap_Kapat();

                if (yazdir)
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
                        string cevap = fis.HesapDokum(false, Convert.ToInt32(this.Tag), 0);
                        if (cevap != "OK")
                        {
                            MessageBox.Show(cevap);
                            return;
                        }
                    }
                }
            }
            this.Close();
        }

        private bool Fis_Odeme_Al()
        {
            if (txt_Hesapno.Text == String.Empty && Param.Tesis_Tipi == 0)
            {
                MessageBox.Show("Kapatma Hesabını Seciniz...");
                return false;
            }

            decimal tutar, doviztutar;

            if (Param.Calisma_Sekli == 1)       //Döviz
            {
                doviztutar = Convert.ToDecimal(gridColumn5.SummaryText);
                tutar = doviztutar * Param.Doviz_Kuru;
            }
            else
            {
                tutar = Convert.ToDecimal(gridColumn4.SummaryText);
                doviztutar = tutar;
            }

            Fis_Islem.Odeme_Al(Convert.ToInt32(this.Tag), tutar, doviztutar, Odeme_Kodu, musTipi_A, odaNo_A, folio_A, cari_A, 0, Param.Doviz_Kodu, false);
            Fis_Islem.Satis_Tip(Convert.ToInt32(this.Tag), Odeme_Kodu, pansiyon_A);

            gridyenile();

            return true;
        }

        private void Hesap_Kapat()
        {

            Fis_Islem.Onburo_At(Convert.ToInt32(this.Tag), "0", 0);

            dbtools.execcmd("update Pos_Masa set Masa_Durum = '0', Masa_Ozel = '' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_Hesapno_Click(object sender, EventArgs e)
        {
            Klavye1 klv = new Klavye1();
            klv.Tag = "KLAVYE";
            klv.ShowDialog();
            txt_Hesapno.Text = klv.deger;
        }

        private void btn_HesapDok_Click(object sender, EventArgs e)
        {
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
                pr.HesapDokum(false, Convert.ToInt32(this.Tag), 0);
            }

            dbtools.execcmd("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Ingenico_Status='1' where Rsat_Fisno='" + Convert.ToInt32(this.Tag) + "'");


            if (Param.Param_Hesap_Kilit)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Hesap_Kilit = 1 where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "'");
            }

            this.Close();
        }

        private void btn_YazdirmadanKapat_Click(object sender, EventArgs e)
        {
            Kapat(false);
        }



    }
}