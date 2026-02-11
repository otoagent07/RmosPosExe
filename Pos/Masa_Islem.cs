using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Masa_Islem : DevExpress.XtraEditors.XtraForm
    {
        public string Masa_No { get; set; }
        public bool Masa_Paket { get; set; }
        public int Fisno { get; set; }

        public Masa_Islem()
        {
            InitializeComponent();
        }

        public void marsSiparis()
        {
            try
            {
                string bindirimReceteKod = dbtools.DegerGetir("select top 1 Param_Bindirim  from Pos_Param where Param_Id = '1'");

                string qq = "select count(*) as toplam from Cst_Recete_Satis where Rsat_Fisno='" + Fisno + "' and isnull(Rsat_Mars,0)='1' and Rsat_Recete<>'" + bindirimReceteKod + "'";
                int count = Convert.ToInt32(dbtools.DegerGetir(qq));

                if (count == 0)
                {
                    btn_Marsla.Enabled = false;
                }

            }
            catch (Exception ex)
            {

            }
        }


        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void Masa_Islem_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            lbl_Baslik.Text = "Masa No : " + Masa_No + "  \nFisno : " + Fisno;

            btn_OdaKontrol.Enabled = User.M_Odakontrol;

            btn_HesapTr.Enabled = User.M_HesapTr;
            btn_SiparisTekrar.Enabled = Departman.Siparis && User.M_SiparisTekrar;
            btn_Tekrargonder.Enabled = Departman.Siparis && User.M_SiparisTekrar;
            btn_Marsla.Enabled = Departman.Kodlar_Mars;
            btn_PaketPr.Enabled = Masa_Paket;
            btn_Kilit_Ac.Enabled = User.M_MasaAc;
            btn_GarsonDegistir.Enabled = User.M_GarsonDegistir;
            btn_KisiSayisiDegistir.Enabled = User.M_KisiSayisi;
            simpleButton2.Enabled = User.Pos_MasaAnlikDurum;

            marsSiparis();
        }

        private void btn_OdaKontrol_Click(object sender, EventArgs e)
        {
            Oda_Kontrol oda = new Oda_Kontrol();
            oda.Show();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_SiparisTekrar_Click(object sender, EventArgs e)
        {
            if (Fisno > 0 && Masa_No != String.Empty)
            {
                dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 14, @Fisno = '" + Fisno + "'");
                FisPr pr = new FisPr();
                string sonuc = pr.SiparisPr(Fisno, false, 0);
                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc);
                }
                dbtools.execcmd($"update Cst_Recete_Satis set Rsat_SiparisPr = 1 where Rsat_Fisno={Fisno}");
            }
        }

        private void btn_Marsla_Click(object sender, EventArgs e)
        {
            if (Fisno > 0 && Masa_No != String.Empty)
            {
                Mars_SatirSec m = new Mars_SatirSec();
                m.Tag = Fisno;
                m.txt_Fisno.Text = Fisno.ToString();
                m.ShowDialog();


                //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Mars = 1 where Rsat_Fisno = '" + Fisno.ToString() + "'");

                //FisPr pr = new FisPr();
                //string sonuc = pr.MarsPr(Fisno);
                //if (sonuc != "OK")
                //{
                //    MessageBox.Show(sonuc);
                //}
            }
        }

        private void btn_MalzemeTransfer_Click(object sender, EventArgs e)
        {
            if (Fisno > 0 && Masa_No != String.Empty)
            {
                Malzeme_Tr malz = new Malzeme_Tr();
                malz.txt_Masano.EditValue = Masa_No;
                malz.kaynakFisno = Convert.ToInt32(Fisno);
                malz.ShowDialog();
            }
        }

        private void btn_HesapTr_Click(object sender, EventArgs e)
        {
            if (Fisno > 0 && Masa_No != String.Empty)
            {
                Hesap_Tr tr = new Hesap_Tr();
                tr.Fisno = Fisno;
                tr.Masa_No = Masa_No;
                tr.ShowDialog();
            }
        }

        private void btn_PaketPr_Click(object sender, EventArgs e)
        {
            if (Fisno > 0 && Masa_No != String.Empty)
            {
                FisPr pr = new FisPr();
                string sonuc = pr.PaketPr(Fisno, " * * * PAKET FİSİ * * * ");
                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc);
                }
            }
        }

        private void btn_Kilit_Ac_Click(object sender, EventArgs e)
        {
            if (Fisno > 0)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Hesap_Kilit = 0 where Rsat_Fisno = '" + Fisno + "'");
                dbtools.execcmd("update Pos_Masa set Masa_Durum = '1',Masa_Musait=0 where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "' ");

                this.Close();
            }
        }

        private void btn_Masaozel_Click(object sender, EventArgs e)
        {
            Klavye2 k2 = new Klavye2();
            k2.ShowDialog();

            string deger = k2.yazi;

            dbtools.execcmd("update Pos_Masa set Masa_Ozel = '" + deger + "' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_Durum = '1'");
            dbtools.execcmd("update Cst_Recete_Satis set Rsat_OzelMasaAdi = '" + deger + "' where Rsat_Fisno = '" + Fisno + "' and Rsat_Departman ='" + Departman.Dep_Kodu + "'");
            this.Close();
        }

        private void btn_Tekrargonder_Click(object sender, EventArgs e)
        {
            Siparis_Tekrar tt = new Siparis_Tekrar();
            tt.Tag = Fisno;
            tt.ShowDialog();
        }

        private void btn_GarsonDegistir_Click(object sender, EventArgs e)
        {
            GarsonDegistir gde = new GarsonDegistir();
            gde.Masa_No = Masa_No;
            gde.ShowDialog();
            this.Close();
        }

        private void btn_Not_Gonder_Click(object sender, EventArgs e)
        {
            if (Fisno > 0 && Masa_No != String.Empty)
            {
                Siparis_Not not = new Siparis_Not();
                not.Fisno = Fisno;
                not.ShowDialog();
            }
        }

        private void btn_MasaLog_Click(object sender, EventArgs e)
        {
            if (Fisno > 0)
            {
                Fis_Log log = new Fis_Log();
                log.Fisno = Fisno;
                log.ShowDialog();
            }
        }



        private void btn_ManuelFatura_Click(object sender, EventArgs e)
        {
            Fis_Islem.Fatura_Kes(0, true);
        }

        private void btn_KisiSayisiDegistir_Click(object sender, EventArgs e)
        {
            DataTable dtKisi = dbtools.SelectTable("select Rsat_Kisi from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + Fisno + "'");
            if (dtKisi.Rows.Count <= 0)
            {
                return;
            }

            Klavye1 klavye = new Klavye1();
            klavye.Tag = "KISISAYISI";
            klavye.txt_Sayi.Text = Convert.ToInt32(dtKisi.Rows[0]["Rsat_Kisi"]).ToString();
            klavye.ShowDialog();
            int Kisi_Sayisi = Convert.ToInt32(klavye.sayi);

            Fis_Islem.Kisi_Sayisi(Fisno, Kisi_Sayisi);

            if (Departman.Kodlar_Kuver_Sat == true)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Miktar = '0',Rsat_Net=0,Rsat_Fiyat=0,Rsat_Kdv=0,Rsat_Tutar=0,Rsat_Maliyet=0 where Rsat_Fisno = '" + Fisno + "' and Rsat_Recete = '" + Departman.Kodlar_Kuver_Recete + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");
                Satis s = new Satis();
                s.Miktar = Kisi_Sayisi;
                s.bartxt_FisNo.EditValue = Fisno;
                s.Urun_Sat(Departman.Kodlar_Kuver_Recete);

            }

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kisi_Sayisi, Log.Log_Islem.Duzelt, "Fis No:" + Fisno.ToString() + " Kisi Sayısı Duzeltildi.Eski Kisi: " + Convert.ToInt32(dtKisi.Rows[0]["Rsat_Kisi"]).ToString() + " Yeni Kisi: " + Kisi_Sayisi.ToString(), Fisno.ToString(), "");
        }

        private void btn_KisiSayisiGoster_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable(@"select
                                                MAX(ISNULL(Rsat_Kisi,0)) as Sayi
                                                into #Acik
                                                from Cst_Recete_Satis 
                                                where Rsat_Ba = 'B' and Rsat_Durum = 'A' and Rsat_Departman = '" + Departman.Dep_Kodu + @"' and Rsat_Tarih  ='" + Param.Tarih + @"'
                                                group by Rsat_Fisno
                                                select
                                                MAX(ISNULL(Rsat_Kisi,0)) as Sayi
                                                into #Kapali
                                                from Cst_Recete_Satis 
                                                where Rsat_Ba = 'B' and Rsat_Durum = 'K' and Rsat_Departman = '" + Departman.Dep_Kodu + @"' and Rsat_Tarih  ='" + Param.Tarih + @"'
                                                group by Rsat_Fisno


                                                select
                                                SUM(ISNULL(Sayi,0))
                                                From #Acik
                                                union all
                                                select
                                                SUM(ISNULL(Sayi,0))
                                                From #Kapali

                                                drop table #Acik
                                                drop table #Kapali");

            if (dt.Rows.Count > 0)
            {
                MessageBox.Show(res_man.GetString("Açık Masa Kişi Sayısı : ") + dt.Rows[0][0] + "\n" + res_man.GetString("Kapalı Masa Kişi Sayısı : ") + dt.Rows[1][0] + "\n" + res_man.GetString("Toplam Masa Kişi Sayısı :") + (Convert.ToInt32(dt.Rows[0][0]) + Convert.ToInt32(dt.Rows[1][0])), "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


        }

        int KartID = 0;
        string KartNo = string.Empty;

        //ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private decimal Bakiye_bul_TL()
        {
            decimal bakiye = 0;
            DataTable dtBakiye = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 21,@Fisno = '" + Fisno + "', @Split = '" + 0 + "'");

            bakiye = Convert.ToDecimal(dtBakiye.Rows[0]["TLBakiye"]);

            bakiye = Math.Abs(bakiye) < Convert.ToDecimal(0.03) ? 0 : bakiye;
            return bakiye;
        }

        //string Filtre = "";
        string DovizKodu = "";
        string OdenmezKodu = "";
        private void HesapBul()
        {
            string KapatmaKodu = Departman.Kodlar_NFCKapatma;
            KartID = Convert.ToInt32(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 30,@Masano = '" + Masa_No + "', @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Fisno = '" + Fisno + "'"));
            KartNo = Convert.ToString(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 29,@Masano = '" + Masa_No + "', @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Fisno = '" + Fisno + "'"));
            DovizKodu = Convert.ToString(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 31,@Masano = '" + Masa_No + "', @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Fisno = '" + Fisno + "'"));
            OdenmezKodu = Convert.ToString(dbtools.DegerGetir("select Pkod_Kod from Pos_Kodlar  where Pkod_Ozelkod = 2 and Pkod_Sinif = 11"));


            DataTable dtHesap = Fronttools.SelectTable(@"

                                    Select
                                    CardF_RezID as Rez_Id,
                                    CardF_Odano as Rez_Odano,
                                    CardF_Ad as Rez_Adi_1,
                                    CardF_Soyad as Rez_Adi_2,
                                    CardF_No as Rez_Kartno,
                                    null as Rez_Konaklama,
                                    CardF_GirisTrh as Rez_Giris_tarihi,
                                    CardF_CikisTrh as Rez_Cikis_tarihi,
                                    convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,
                                    Acenta.Ac_Adi as Ac_Adi,
                                    Rez_Odeme as Rez_odeme,
                                    CardF_RezID as Rez_Master_id,
                                    ID as ID,
                                    ''

                                    from KartF with(NOLOCK) 
                                    left join  Rez WITH(NOLOCK) on Rez_Id = CardF_RezID
                                    left join Acenta WITH(NOLOCK) on Rez_Macenta = Acenta.Ac_Kodu  
                                    left join Kodlar WITH(NOLOCK) on Kodlar_Sinif = '10' and Rez_Odeme = Kodlar_Kod 
                                    where CardF_R_I_H = 'I' and ID = '" + KartID + "'");


            if (dtHesap.Rows.Count > 0)
            {
                Oda_No = Convert.ToString(dtHesap.Rows[0]["Rez_Odano"]);
                Folio = Convert.ToInt32(dtHesap.Rows[0]["Rez_Id"]);
                Pansiyon = Convert.ToString(dtHesap.Rows[0]["Rez_Konaklama"]);
                Odeme_Kodu = Convert.ToString(dtHesap.Rows[0]["Rez_Odeme"]);
                Master_Folio = Convert.ToInt32(dtHesap.Rows[0]["Rez_Master_id"]);
                KartNo = Convert.ToString(dtHesap.Rows[0]["Rez_Kartno"]);
                KartID = Convert.ToInt32(dtHesap.Rows[0]["ID"]);

                musTipi_A = Mus_tipi;
                odaNo_A = Oda_No;
                folio_A = Folio;
                masterFolio_A = Master_Folio;
                pansiyon_A = Pansiyon;
                uyeId_A = Uye_Id;
                uyeAdsoyad_A = Uye_Adsoyad;
                uyeKartturu_A = Uye_Kartturu;
                cari_A = Cari_Kod;
                odemeKodu_A = Odeme_Kodu;

            }
            else
            {
                MessageBox.Show(res_man.GetString("Hesap Bulunamadı..."));

                return;
            }

            OdemeTutar = Bakiye_bul_TL();


            if (odemeKodu_A == Param.Fullcomp_Kodu)
            {
                KapatmaKodu = OdenmezKodu;
            }


            Fis_Islem.Odeme_Al(Convert.ToInt32(Fisno), OdemeTutar, OdemeTutar, Convert.ToString(KapatmaKodu), musTipi_A, odaNo_A, folio_A, cari_A, Split, DovizKodu, false);
            Fis_Islem.Satis_Tip(Convert.ToInt32(Fisno), Convert.ToString(KapatmaKodu), pansiyon_A);

            if (Param.Tesis_Tipi == 0)
            {
                if (Param.Tarih != Convert.ToDateTime(Fronttools.DegerGetir("select Fis_Curtar from Fishrk where Fis_anah = 1")))
                {
                    MessageBox.Show(res_man.GetString("Önbüro Çalışma Tarihi ile Sistem tarihi Farklı.") + "\n" + res_man.GetString("Sistemden Çıkıp Yeniden Giriniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            Fis_Islem.Onburo_At(Convert.ToInt32(Fisno), KartNo, KartID);



            //dbtools.execcmd("update Pos_Masa set Masa_Durum = '0', Masa_Ozel = '' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
            dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 16,@Masano = '" + Masa_No + "',@Dep_Kodu = '" + Departman.Dep_Kodu + "'");

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, "Fiş Kapatma. Fisno:" + Convert.ToInt32(this.Tag) + " Masano:" + Masa_No.ToString(), Convert.ToString(this.Tag), "");

            //MessageBox.Show(res_man.GetString("Masa Kapatıldı..");
            this.Close();

        }

        decimal OdemeTutar = 0;


        string musTipi_A = String.Empty;
        string odaNo_A = String.Empty;
        int folio_A = 0;
        int masterFolio_A = 0;
        string pansiyon_A = String.Empty;
        int uyeId_A = 0;
        string uyeAdsoyad_A = String.Empty;
        string uyeKartturu_A = String.Empty;
        string cari_A = String.Empty;
        int Split = 0;

        string odemeKodu_A = String.Empty;

        public string Mus_tipi;
        public string Oda_No;
        public int Folio;
        public int Master_Folio;
        public string Pansiyon;
        public string Odeme_Kodu;
        public int Uye_Id;
        public string Uye_Adsoyad;
        public string Uye_Kartturu;
        public string Cari_Kod;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (Departman.Kodlar_AndPos_NFC == true)
            {
                HesapBul();
            }
            else
            {
                MessageBox.Show(res_man.GetString("Temassız Kartla Çalışan Tesisler İçin Geçerlidir."), "Uyarı");
                return;
            }

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (Departman.Kodlar_AndPos_NFC == true)
            {
                dbtools.execcmd("Update Pos_Masa Set Masa_NFC = 0, Masa_Durum = 0,Masa_Musait=0 where Masa_Depart = '" + Departman.Dep_Kodu + "'  and Masa_No = '" + Masa_No + "'"); // and Masa_NFC = 1

                if (User.Pos_MasaUrunSil)
                {
                    string iptalSebep = String.Empty;
                    DateTime Tarih = Convert.ToString(dbtools.DegerGetir("Select Top(1) Rsat_Tarih From Cst_Recete_Satis Where Rsat_Fisno = '" + Fisno + "'")) == "" ? Param.Tarih : Convert.ToDateTime(dbtools.DegerGetir("Select Top(1) Rsat_Tarih From Cst_Recete_Satis Where Rsat_Fisno = '" + Fisno + "'"));


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

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Fis_Iptal, Log.Log_Islem.Sil, Fisno + " NL Fis Silindi... Masa İşlemler Yetkili Silme.", Fisno.ToString(), String.Empty);

                }

                MessageBox.Show(res_man.GetString("Masa İzinleri Açıldı.."));
                this.Close();
            }
            else
            {
                MessageBox.Show(res_man.GetString("Temassız Kartla Çalışan Tesisler İçin Geçerlidir."), "Uyarı");
                return;
            }
        }

        //string AcikCekler = "", KapaliCekler = "";
        //int AcikSayi = 0, KapaliSayi = 0, ToplamSayi = 0, ServisSayisi = 0, KuverSayisi = 0;

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (Departman.Kodlar_PRSor)
            {
                Pos_PR a = new Pos_PR();
                a.Tip = "D";
                a.Fisno = Fisno.ToString();
                a.ShowDialog();

            }
        }

        //decimal AcikTutar = 0, KapaliTutar = 0, ToplamTutar = 0, ServisTutar = 0, KuverTutar = 0;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            AnlikDurum a = new AnlikDurum();
            a.ShowDialog();
        }

        private void btnTumNfcMasaAc_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Masa_No from Pos_Masa where Masa_Durum='1' and Masa_NFC='1' and Masa_Depart='" + Departman.Dep_Kodu + "'");
            foreach (DataRow item in dt.Rows)
            {
                string masaNo = item["Masa_No"].ToString();
                string fisNo = dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 4, @Dep_Kodu = '" + Departman.Dep_Kodu + "',@Masano = '" + masaNo + "'");
                if (fisNo=="0")
                {
                    dbtools.execcmd("Update Pos_Masa Set Masa_NFC = 0, Masa_Durum = 0,Masa_Musait=0 where Masa_Depart = '" + Departman.Dep_Kodu + "'  and Masa_No = '" + masaNo + "'"); // and Masa_NFC = 1
                }
            }


            dbtools.execcmd($"Update Pos_Masa Set Masa_NFC = 0, Masa_Durum = 0,Masa_Musait=0 where Masa_Depart = '{Departman.Dep_Kodu}'");

            MessageBox.Show("Siparişi Olmayan ve Fiş No almayan TÜM Masaların İzni Açıldı !");

        }

        private void btnParcaliMasa_Click(object sender, EventArgs e)
        {
            Main.masa_takip.parcaliMasaIlk();
        }
    }
}