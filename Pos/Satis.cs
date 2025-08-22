using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using Pos.Class;
using Pos.Controllers;
using Pos.Forms;
using Pos.Models;
using Pos.Print;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Pos
{
    public partial class Satis : DevExpress.XtraEditors.XtraForm
    {
        public string Masa_No;
        public bool Masa_Paket;
        string Ana_Grup;
        string Alt_Grup;
        SimpleButton btn_AltGrup;
        public string Ozel_Masa;
        public int Split = 0;
        public string Splitad = "";
        public bool AcikAdres = false;
        public string Sube;

        // Satış Satır Ayarları
        public string Aciklama = String.Empty;
        public decimal Miktar = 1;
        public string Garson = "";
        public int Kisi_Sayisi = 0;
        public string Paketci = "";
        public bool Rsat_SiparisPr;
        public bool Rsat_AbuyerPr = false;
        public string eMiktar = "T";
        //Yuvarlama Prm
        public decimal Yuv_Tutar = 0;
        public int Masa_Durum = 0;

        //Cari Ayarları
        public Cari mCari;

        //Direk Satış Ayarları
        string D_Mus_tipi;
        string D_Oda_No;
        int D_Folio;
        string D_Pansiyon;
        int D_Uye_Id;
        string D_Uye_Adsoyad;
        string D_Uye_Kartturu;
        string D_Cari_Kod;
        string D_Ind_Kodu;
        decimal D_Ind_Oran;
        string D_Odeme;
        int D_MasterId;
        public string PaketFiyat = "S";

        public string CariTel = String.Empty;


        public bool otomatikSatis = false;
        public string recetekod = "";
        public string recetekodCocuk = "";
        public string kartnom = "";
        public string hizmetOdemeKod = "";
        public int hizmetmiktar = 1;
        public int hizmetmiktarCocuk = 0;


        public string adres = "";

        public Satis()
        {
            InitializeComponent();
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        bool yazdirilmamisSiparis = false;
        string kisiyeSatisAktifmi = "0";
        private void Satis_Load(object sender, EventArgs e)
        {

            load();



        }

        public bool urunleriYenile = true;
        public bool tutarduzeltplus = false;
        public void load(bool urunleriYenile = true)
        {
            try
            {
                string deger1 = dbtools.DegerGetir("select top 1 isnull(tutarduzeltplus,0) as tutarduzeltplus from  RmosMuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'");
                tutarduzeltplus = Convert.ToBoolean(deger1);

                sayac = 0;
                kisiyiTemizle();
                this.urunleriYenile = urunleriYenile;

                //flp_AnaGrup.Controls.Clear();

                kisiyeSatisAktifmi = dbtools.DegerGetir($"select isnull(Kodlar_KisiyeSatis,0) as Kodlar_KisiyeSatis from Stok_Kodlar where Kodlar_Sinif='01' and Kodlar_Kod='{Departman.Dep_Kodu}'");

                if (kisiyeSatisAktifmi == "0" || kisiyeSatisAktifmi.ToLower() == "false")
                {
                    panelControl4.Visible = false;
                    txt_Not.Size = new Size(txt_Not.Size.Width, 35);
                    btnMasaSec.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                }
                AyarlarController ayarlar = new AyarlarController();
                panelControl2.Width = Convert.ToInt32(ayarlar.satisEkranGenislik);
                this.BringToFront();

                StatikSinif.masaKilitle(Masa_No);

                string var = dbtools.DegerGetir("select count(*) as toplam from Pos_Param where ISNULL(yazdirilmamissiparis,0)=1");
                if (var == "1")
                {
                    yazdirilmamisSiparis = true;
                }


                bar_Cari.Caption = "";

                //Ozel_Masa = dbtools.DegerGetir("Select Masa_Ozel From Pos_Masa Where Masa_No = '" + Masa_No + "' and Masa_Depart ='" + Departman.Dep_Kodu + "'");

                Bilgileri_Doldur();

                Paket_Servis();

                gridyenile();

                barkodAktifmi = dbtools.DegerGetir("select top 1 Kodlar_RecBarSis from Stok_Kodlar where Kodlar_Kod='" + Departman.Dep_Kodu + "'");


                if (Param.Param_Anagrup_Cikmasin)
                {
                    if (urunleriYenile)
                    {
                        Alt_Yenile();
                    }

                    layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
                else
                {
                    if (urunleriYenile)
                    {
                        Ust_Yenile(); // 09.01.2025 de kaldırıldı
                    }

                }

                btn_Siparis.Enabled = Departman.Siparis;


                btn_Mars.Visible = Departman.Kodlar_Mars;

                if (btn_Mars.Visible==false)
                {
                    btn_Siparis.Size = new Size(170,btn_Siparis.Height);
                }

                btn_MiktarDuzelt.Enabled = User.G_Miktarduzelt;
                btn_Tutarduzelt.Enabled = User.G_Tutarduzelt;
                btn_SatirSil.Enabled = User.G_Satirsil;
                btnTopluSil.Enabled = User.G_Satirsil;
                btn_Indirim.Enabled = User.G_Indirim_Satis;
                btn_Zayi.Enabled = User.G_Zayi;
                btnJokerAciklama.Enabled = User.G_Zayi;
                btn_Ikram.Enabled = User.G_Ikram;
                btn_SpSil.Enabled = User.S_Sp_Sil;
                btn_Arti.Enabled = User.Pos_ArtiEksi_Aktif;
                btn_Eksi.Enabled = User.Pos_ArtiEksi_Aktif;
                rdo_EMiktar.Enabled = User.Pos_YarimDubleAlan;



                chk_Eksi.Enabled = getEksileme();
                chk_Eksi.Visible = chk_Eksi.Enabled;


                if (Param.Param_CallerID)
                {
                    btnAdresGuncelle.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
                else
                {
                    btnAdresGuncelle.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                }


                bar_Tarih.Caption = "Tarih : " + Param.Tarih.ToShortDateString();

                chk_Fix.Visible = User.Pos_FixMenu;

                barkodFocuslan();

                //if (Convert.ToString(this.Tag) ==  "H") barkodFocuslan();
                MarsKontrol();
                SiparisKontrol();

                //SatisListesi a = new SatisListesi();

                int fisno = Convert.ToInt32(bartxt_FisNo.EditValue);
                Main.a.Listele(fisno);

                //dbtools.execcmd("update Pos_Masa set Masa_Durum='1' where Masa_No='"+Masa_No+"' and Masa_Depart='" + Departman.Dep_Kodu+"'");


                btnJokerAciklama.Visible = Departman.Kodlar_YS_Aktif;


                hesapYazmismi();

                marsSiparis(); // marşla doğru çalışıyor . sipariş yanlış çalışıyor. (iptal abuyerden çıkmıyor)

                btn_Bindirim.Enabled = User.G_Bindirim;



                if (kisiyeSatisAktifmi == "0" || kisiyeSatisAktifmi.ToLower() == "false")
                {
                    panelControl4.Visible = false;
                    txt_Not.Size = new Size(txt_Not.Size.Width, 35);
                }
                else
                {
                    string sayacimkisi = dbtools.DegerGetir($"select isnull(kisiyeSatisAdSoyad,'') as kisiyeSatisAdSoyad from Cst_Recete_Satis  where Rsat_Fisno=" + fisno + " order by Rsat_Id desc");

                    if (sayacimkisi != "" && sayacimkisi.Contains("-"))
                    {
                        sayac = Convert.ToInt32(sayacimkisi.Split('-')[0]) + 1;
                        txtKisiyeSatisSayac.Text = sayac + "";
                    }


                    if (kisiyeSatisAktifmi == "1" || kisiyeSatisAktifmi == "True")
                    {
                        txtKisiyeSatis.Text = "";
                        txtKisiyeSatis.Select();
                        txtKisiyeSatis.Focus();
                    }

                }

                dizaynyukle();


                gridView1.CustomColumnDisplayText += (s, e) =>
                {
                    if (e.Column.FieldName == "Rsat_Tutar" && e.Value != null)
                    {
                        // Değerin sayısal bir değer olduğunu kontrol edin
                        if (decimal.TryParse(e.Value.ToString(), out decimal tutarValue))
                        {
                            // Değeri noktalı formatta göstermek için manuel olarak formatlıyoruz
                            e.DisplayText = string.Format("{0:#,##0.00}", tutarValue);
                        }
                    }
                };


                textEditFisnobirlestir1.Visible = Param.hesapFisQrFisno;
                txtFisnoGit.Visible = Param.hesapFisQrFisno;


                chk_Yapma.Visible = User.satisYapma;
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "load()", "", ex);
            }
        }
        public void marsSiparis()
        {
            try
            {
                string bindirimReceteKod = dbtools.DegerGetir("select top 1 Param_Bindirim  from Pos_Param where Param_Id = '1'");

                string qq = "select count(*) as toplam from Cst_Recete_Satis where Rsat_Fisno='" + Convert.ToInt32(bartxt_FisNo.EditValue) + "' and isnull(Rsat_Mars,0)='1' and Rsat_Recete<>'" + bindirimReceteKod + "'";
                int count = Convert.ToInt32(dbtools.DegerGetir(qq));

                string qq2 = "select count(*) as toplam from Cst_Recete_Satis where Rsat_Fisno='" + Convert.ToInt32(bartxt_FisNo.EditValue) + "' and isnull(Rsat_SiparisPr,0)='1' and Rsat_Recete<>'" + bindirimReceteKod + "'";
                int count2 = Convert.ToInt32(dbtools.DegerGetir(qq2));

                if (count == 0 && count2 == 0)
                {
                    btn_Mars.Enabled = true;
                    btn_Siparis.Enabled = true;

                    btn_Mars.Enabled = Departman.Kodlar_Mars;
                }
                else if (count == 0)
                {
                    btn_Mars.Enabled = false;
                }
                else
                {

                }
                // todo: ramazan önceden yaptım
                //////count = Convert.ToInt32(dbtools.DegerGetir("select count(*) as toplam from Cst_Recete_Satis where Rsat_Fisno='" + Convert.ToInt32(bartxt_FisNo.EditValue) + "' and Rsat_Mars='1'"));

                //////if (count > 0)
                //////{
                //////    btn_Siparis.Enabled = false;
                //////}
            }
            catch (Exception ex)
            {

            }
        }


        string kodlarkodadisyonaktifmi = "";
        string adisyondahaoncedenyazdirilmismi = "";
        public void hesapYazmismi()
        {
            kodlarkodadisyonaktifmi = dbtools.DegerGetir("select isnull(Kodlar_Adisyon,0) Kodlar_Adisyon from Stok_Kodlar where Kodlar_Kod='" + Departman.Dep_Kodu + "' and Kodlar_Sinif='01'");
            adisyondahaoncedenyazdirilmismi = dbtools.DegerGetir("select isnull(Rsat_AdisyonPr,0) Rsat_AdisyonPr from Cst_Recete_Satis where Rsat_Fisno='" + Convert.ToInt32(bartxt_FisNo.EditValue) + "'");
        }


        public void hesapyazmissaRenginiGuncelle()
        {
            if (kodlarkodadisyonaktifmi == "True" && adisyondahaoncedenyazdirilmismi == "True")
            {
                //dbtools.execcmdR("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Convert.ToString(bartxt_MasaNo.EditValue) + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
            }
        }


        public string barkodAktifmi = "false";

        public bool getEksileme()
        {
            try
            {
                string deger = dbtools.DegerGetir("SELECT top 1 isnull(Pos_Eksileme,0) as Pos_Eksileme  FROM RmosMuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'");
                return Convert.ToBoolean(deger);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void MarsKontrol()
        {
            int Control = Convert.ToInt32(dbtools.DegerGetir("select Count(*) from Cst_Recete_Satis where rsat_fisno = '" + bartxt_FisNo.EditValue + "' and Rsat_Mars = 1"));
            if (Control > 0)
            {
                btn_Siparis.Enabled = false;
            }
        }

        private void SiparisKontrol()
        {
            int Control = Convert.ToInt32(dbtools.DegerGetir("select Count(*) from Cst_Recete_Satis where rsat_fisno = '" + bartxt_FisNo.EditValue + "' and Rsat_SiparisPr = 0"));
            if (Control > 0)
            {
                //btn_Cikis.Enabled = false;
                btn_Cikis.Enabled = Param.Param_SatisCikisButton; // sonradan ramazan ekledi bu satırı

                btn_Siparis.Enabled = true;
            }
            else
            {
                btn_Cikis.Enabled = true;
                btn_Siparis.Enabled = false;
            }

            marsKontrol(true);


        }

        private void Paket_Servis()
        {
            if (Masa_Paket)
            {
                PaketFiyat = "P";

                DataTable dt = dbtools.SelectTable("select Rsat_Cari,Rsat_Paketci from Cst_Recete_Satis WITH(NOLOCK) Where Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue) + "'");
                if (dt.Rows.Count > 0)
                {
                    mCari = Cari.Cari_Getir(Convert.ToString(dt.Rows[0]["Rsat_Cari"]));
                    bartxt_MusTipi.EditValue = "C";
                    bartxt_OdaNo.EditValue = mCari.Cari_Kod;

                    Paketci = Convert.ToString(dt.Rows[0]["Rsat_Paketci"]);
                    bartxt_Garson.EditValue = User.Isim_Getir(Paketci);
                    bar_Cari.Caption = "Cari Bilgisi : " + mCari.Cari_Ad + " " + mCari.Cari_Soyad + " | \nAdres1 : " + mCari.Cari_Adres1 + " | \nAdres2 : " + mCari.Cari_Adres2 + " | Telefon : " + mCari.Cari_Tel;

                }
                else
                {
                    if (mCari == null)
                    {
                        PaketFiyat = "P";

                        Paket_Servis paket = new Paket_Servis();
                        paket.txt_Cari_Telefon.Text = CariTel;
                        //Kapatma_Tekoda 
                        paket.ShowDialog();
                        mCari = paket.pCari;
                        AcikAdres = paket.AcikAdres;

                    }
                    else
                    {
                        bar_Cari.Caption = "Cari Bilgisi : " + mCari.Cari_Ad + " " + mCari.Cari_Soyad + " | \nAdres1 : " + mCari.Cari_Adres1 + " | \nAdres2 : " + mCari.Cari_Adres2 + " | Telefon : " + mCari.Cari_Tel;
                    }
                    bartxt_MusTipi.EditValue = "C";
                    if (!AcikAdres)
                    {
                        if (mCari == null)
                        {
                            this.Close();
                            return;
                        }
                        if (mCari.Cari_Kod == null)
                        {
                            mCari = null;
                            this.Close();
                            return;
                        }
                        bartxt_OdaNo.EditValue = mCari.Cari_Kod;
                    }

                    if (Param.Param_Paketci_Sor)
                    {
                        Garson_Sor pkt = new Garson_Sor();
                        pkt.Tag = "PAKET";
                        pkt.ShowDialog();
                        Paketci = pkt.Garson_Kod;
                        bartxt_Garson.EditValue = User.Isim_Getir(Paketci);
                    }
                    if (Param.Param_Paket_Kisi)
                    {
                        Klavye1 kisi = new Klavye1();
                        kisi.Tag = "KISISAYISI";
                        kisi.AutoScroll = true;
                        kisi.ShowDialog();
                        Kisi_Sayisi = Convert.ToInt32(kisi.sayi);
                    }
                }
            }


        }

        string YerliYabanci = "";

        private void Yerli_Yabanci()
        {
            if (Departman.Kodlar_YerliYabanci)
            {
                DataTable dt = dbtools.SelectTable("Select Count(*),MAX(Rsat_YerliYabanci) From  Cst_Recete_Satis Where Rsat_Fisno = '" + bartxt_FisNo.EditValue + "'");
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0][0]) != "0")
                    {
                        YerliYabanci = Convert.ToString(dt.Rows[0][1]);
                        return;
                    }
                    else
                    {
                        Pos_YerliYabanci a = new Pos_YerliYabanci();
                        a.ShowDialog();
                        YerliYabanci = a.YO;
                    }
                }


            }
        }

        private void Kisi_Garson()
        {
            DataTable dtKontrol = dbtools.SelectTable("select Rsat_Kisi from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + bartxt_FisNo.EditValue.ToString() + "'");


            if (dtKontrol.Rows.Count > 0 && Departman.Garson_Sor == false)
            {
                bartxt_Kisi.Caption = "Kisi: " + Convert.ToString(dtKontrol.Rows[0]["Rsat_Kisi"]);
                return;
            }

            if (Masa_Paket)
            {
                return;
            }

            if (((Convert.ToString(this.Tag) != "D" && Convert.ToString(this.Tag) != "H") && (Departman.Kisi_Sor || Departman.Garson_Sor)) || (Convert.ToString(this.Tag) == "D" && !Departman.Kodlar_Kisisorma_Pda))
            {
                if (Masa_No.Contains("_"))
                {
                    return;
                }

                string suanParcalimi = dbtools.DegerGetir("select count(*) as toplam from Pos_Masa where Masa_Depart='" + Departman.Dep_Kodu + "' and Masa_Durum='1' and Masa_No like '" + Masa_No + "[_]%' ");

                if (!suanParcalimi.Equals("0"))
                {
                    return;
                }

                Kisi_Garson sor = null;
                string text = "select top 1 isnull(Rsat_Kisi,1) as Rsat_Kisi,isnull(Rsat_Garson,'') as Rsat_Garson from cst_recete_satis where Rsat_Fisno='" + bartxt_FisNo.EditValue.ToString() + "'";

                var data = dbtools.SelectTableR(text);
                if (Param.kisivegarsonbirkeresoraktif && data != null && data.Rows.Count > 0)
                {
                    Garson = data.Rows[0]["Rsat_Garson"].ToString();
                    Kisi_Sayisi = Convert.ToInt32(data.Rows[0]["Rsat_Kisi"].ToString());
                }
                else
                {
                    sor = new Kisi_Garson();
                    sor.Tag = this.Tag;
                    sor.ShowDialog();
                    Garson = sor.Garson_Kodu;
                    Kisi_Sayisi = Convert.ToInt32(sor.Kisi);
                }


                // && Kisi_Sayisi>0 sonradan eklendi
                if (Departman.Kodlar_Kuver_Sat && Kisi_Sayisi > 0)
                {
                    Miktar = Kisi_Sayisi;
                    Urun_Sat(Departman.Kodlar_Kuver_Recete);
                }

                if (sor != null && sor.Iptal)
                {
                    this.Close();
                }

                bartxt_Garson.EditValue = User.Isim_Getir(Garson);
            }
        }


        public decimal CardF_Indirim = 0;

        private bool Direk_Satis()
        {
            if (Convert.ToString(this.Tag) == "D")
            {
                if (Param.Tesis_Tipi == 0)
                {
                    gridControl1.DataSource = null;
                }

                Arama ara = new Arama();// pos tatış nfc aktif değilse
                ara.Tag = "D";
                ara.ShowDialog();

                if (ara.Cikis)
                {
                    if (Param.Tesis_Tipi == 0)
                    {
                        this.Close();
                    }
                    return false;
                }

                if (Param.Tesis_Tipi == 0)
                {
                    if (Fronttools.HesapKapali_Kontrol(ara.Folio))
                    {
                        MessageBox.Show(res_man.GetString("Seçilen Hesap Kapatılmıştır.") + "\n" + res_man.GetString("Lüften Resepsiyon ile Görüşün..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Direk_Satis();
                        return true;
                    }
                }

                D_Mus_tipi = ara.Mus_tipi;
                D_Oda_No = ara.Oda_No;
                D_Folio = ara.Folio;
                D_Pansiyon = ara.Pansiyon;
                D_Uye_Id = ara.Uye_Id;
                D_Uye_Adsoyad = ara.Uye_Adsoyad;
                D_Uye_Kartturu = ara.Uye_Kartturu;
                D_Cari_Kod = ara.Cari_Kod;
                D_Ind_Kodu = ara.Ind_Kodu;
                D_Ind_Oran = ara.Ind_Oran;
                D_Odeme = ara.Odeme_Kodu;
                D_MasterId = ara.Master_Folio;
                Kart_No = ara.Kart_No;
                FolioKart_ID = ara.KartID;

                bartxt_OdaNo.EditValue = ara.Oda_No;

                CardF_Indirim = ara.CardF_Indirim;
                try
                {
                    string comp = Fronttools.DegerGetir("select top 1 CardF_MusteriTipi from kartf where ID='" + ara.KartID + "'");

                    if (comp.Equals("CM"))
                    {
                        label1.Text = "COMP";
                        label1.ForeColor = Color.Red;
                    }
                    else if (comp.Equals("OM"))
                    {
                        label1.Text = "OTEL MİSAFİRİ";
                        label1.ForeColor = Color.Red;
                    }

                    decimal folioBakiye = Fronttools.BalanceBul(D_MasterId, Kart_No, FolioKart_ID) * -1; //13,95
                    btnBakiye.Text = folioBakiye.ToString("N2");


                }
                catch (Exception ex) { }

                //btn_Cikis.Enabled = false;

                return true;
            }

            return false;
        }

        private void Bilgileri_Doldur()
        {
            bartxt_MasaNo.EditValue = Masa_No;
            bartxt_Garson.EditValue = User.Isim_Getir(Garson);

            Fisno_Al();


            bartxt_Kul.Caption = "Kullanıcı : " + User.P_Ad + " " + User.P_Soyad;
            bartxt_Dep.Caption = "Departman : " + Departman.Dep_Adi;

            if (Convert.ToString(this.Tag) != "D") { btn_Indirim.Visible = false; }

            btn_Indirim.Visible = true; // mahsun istedi diye eklendi


            if (Convert.ToString(this.Tag) == "D" || Departman.Kodlar_AndPos_NFC)
            {
                //panelControl2.Width = this.Width / 4;
                if (Param.Tesis_Tipi == 0)
                {
                    Direk_Satis();
                }
                Kapatma_Yenile();
                gridyenile();

                flp_Kapatma.Visible = true;
                layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                barkodFocuslan();
                txtAdisyon.EditValue = "";
            }



            if (Convert.ToString(this.Tag) == "H")
            {

                chk_Fix.Visible = false;
                btn_SpSil.Visible = false;

                //panelControl2.Width = this.Width / 4;

                Kapatma_Yenile();
                gridyenile();

                flp_Kapatma.Visible = true;
                layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                barkodFocuslan();
            }

            if (Param.Calisma_Sekli == 0)
            {
                if (Departman.Kodlar_AndPos_NFC && (Convert.ToString(this.Tag) != "H" || Convert.ToString(this.Tag) == "D"))
                {
                    //panelControl2.Width = this.Width / 4;
                    NFCBodrum();

                    Kapatma_Yenile();
                    gridyenile();

                    flp_Kapatma.Visible = true;
                    layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    barkodFocuslan();
                    txtAdisyon.EditValue = "";
                }
            }

            Kisi_Garson();

            Yerli_Yabanci();

            PRSor();
        }

        public string PRKodu = "";
        private void PRSor()
        {
            if (Departman.Kodlar_PRSor)
            {
                PRKodu = dbtools.DegerGetir("select ISNULL(MAX(Rsat_PR),'') from Cst_Recete_Satis where Rsat_Fisno= '" + bartxt_FisNo.EditValue + "'");

                if (PRKodu == "")
                {
                    Pos_PR a = new Pos_PR();
                    a.Tip = "S";
                    a.ShowDialog();

                    PRKodu = a.PRKodu;
                }
            }
        }

        private bool NFCBodrum()
        {
            //if (Convert.ToString(this.Tag) ==  "D")
            //{
            if (Param.Tesis_Tipi == 0)
            {
                gridControl1.DataSource = null;
            }
            if (otomatikSatis) return true;
            Arama ara = new Arama();
            if (Masa_Durum == 0)
            {

                ara.Tag = "M";
                ara.ShowDialog(); // burası çalıştı

                if (Param.Tesis_Tipi == 0)
                {
                    if (Fronttools.HesapKapali_Kontrol(ara.Folio))
                    {
                        MessageBox.Show(res_man.GetString("Seçilen Hesap Kapatılmıştır.") + "\n" + res_man.GetString("Lüften Resepsiyon ile Görüşün..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Direk_Satis();
                        return true;
                    }
                }

                D_Mus_tipi = ara.Mus_tipi;
                D_Oda_No = ara.Oda_No;
                D_Folio = ara.Folio;
                D_Pansiyon = ara.Pansiyon;
                D_Uye_Id = ara.Uye_Id;
                D_Uye_Adsoyad = ara.Uye_Adsoyad;
                D_Uye_Kartturu = ara.Uye_Kartturu;
                D_Cari_Kod = ara.Cari_Kod;
                D_Ind_Kodu = ara.Ind_Kodu;
                D_Ind_Oran = ara.Ind_Oran;
                D_Odeme = ara.Odeme_Kodu;
                D_MasterId = ara.Master_Folio;
                Kart_No = ara.Kart_No;
                //FolioKart_No = ara.Kart_No;
                FolioKart_ID = ara.KartID;

                bartxt_OdaNo.EditValue = ara.Oda_No;

                CardF_Indirim = ara.CardF_Indirim;
                try
                {
                    string comp = Fronttools.DegerGetir("select top 1 CardF_MusteriTipi from kartf where ID='" + ara.KartID + "'");

                    if (comp.Equals("CM"))
                    {
                        label1.Text = "COMP";
                        label1.ForeColor = Color.Red;
                    }
                    else if (comp.Equals("OM"))
                    {
                        label1.Text = "OTEL MİSAFİRİ";
                        label1.ForeColor = Color.Red;
                    }

                    decimal folioBakiye = Fronttools.BalanceBul(D_MasterId, Kart_No, FolioKart_ID) * -1; //13,95
                    btnBakiye.Text = folioBakiye.ToString("N2");


                }
                catch (Exception ex) { }

            }
            else
            {
                FolioKart_No = Convert.ToString(dbtools.DegerGetir("select ISNULL(MAX(Rsat_Kartno),0) from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + bartxt_FisNo.EditValue + "' and Rsat_Durum = 'A' and Rsat_Masa = '" + Masa_No + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'"));
                DataTable dt = Fronttools.NFCBilgiler(FolioKart_No);
                if (dt.Rows.Count > 0)
                {
                    D_Mus_tipi = null;
                    D_Oda_No = Convert.ToString(dt.Rows[0]["Rez_Odano"]);
                    D_Folio = Convert.ToInt32(dt.Rows[0]["Rez_Id"]);
                    //D_Pansiyon = ara.Pansiyon;
                    //D_Uye_Id = ara.Uye_Id;
                    //D_Uye_Adsoyad = ara.Uye_Adsoyad;
                    //D_Uye_Kartturu = ara.Uye_Kartturu;
                    //D_Cari_Kod = ara.Cari_Kod;
                    //D_Ind_Kodu = ara.Ind_Kodu;
                    //D_Ind_Oran = ara.Ind_Oran;
                    D_Odeme = Convert.ToString(dt.Rows[0]["Rez_Odeme"]);
                    D_MasterId = Convert.ToInt32(dt.Rows[0]["Rez_Master_id"]);
                    Kart_No = Convert.ToString(dt.Rows[0]["Rez_Kartno"]);
                    bartxt_OdaNo.EditValue = Convert.ToString(dt.Rows[0]["Rez_Odano"]);
                    FolioKart_ID = Convert.ToString(dt.Rows[0]["ID"]);
                }
                else
                {
                    ara = new Arama();
                    ara.Tag = "M";
                    ara.ShowDialog();

                    D_Mus_tipi = ara.Mus_tipi;
                    D_Oda_No = ara.Oda_No;
                    D_Folio = ara.Folio;
                    D_Pansiyon = ara.Pansiyon;
                    D_Uye_Id = ara.Uye_Id;
                    D_Uye_Adsoyad = ara.Uye_Adsoyad;
                    D_Uye_Kartturu = ara.Uye_Kartturu;
                    D_Cari_Kod = ara.Cari_Kod;
                    D_Ind_Kodu = ara.Ind_Kodu;
                    D_Ind_Oran = ara.Ind_Oran;
                    D_Odeme = ara.Odeme_Kodu;
                    D_MasterId = ara.Master_Folio;
                    Kart_No = ara.Kart_No;
                    //FolioKart_No = ara.Kart_No;
                    FolioKart_ID = ara.KartID;

                    bartxt_OdaNo.EditValue = ara.Oda_No;
                    CardF_Indirim = ara.CardF_Indirim;
                }
            }



            if (ara.Cikis)
            {
                if (Param.Tesis_Tipi == 0)
                {
                    this.Close();
                }
                return false;
            }



            //btn_Cikis.Enabled = false;

            return true;
            //}

            //return false;

        }

        bool cikis = true;
        int Fisno = 0;

        // aa

        private void Fisno_Al()
        {
            if (Convert.ToString(this.Tag) == "M" || Convert.ToString(this.Tag) == "P")
            {
                bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("select ISNULL((select TOP 1 isnull(Rsat_Fisno,0) from Cst_Recete_Satis WITH(NOLOCK) where  Rsat_Durum = 'A' and Rsat_Masa = '" + Masa_No + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'),0)"));
                if (Convert.ToInt32(bartxt_FisNo.EditValue) == 0)
                {
                    bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
                }

            }
            else if ((Convert.ToString(this.Tag) != "M" || Convert.ToString(this.Tag) == "P") && Convert.ToString(this.Tag) != "H" && Convert.ToString(this.Tag) != "D" && cikis == true)
            {
                //bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("select ISNULL((select TOP 1 isnull(Rsat_Fisno,0) from Cst_Recete_Satis WITH(NOLOCK) where  Rsat_Durum = 'A' and Rsat_Masa = '" + Masa_No + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'),0)"));
                //if (Convert.ToInt32(bartxt_FisNo.EditValue) == 0)
                //{
                //    bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
                //}
                bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
            }
            else if ((Convert.ToString(this.Tag) != "M" || Convert.ToString(this.Tag) == "P") && Convert.ToString(this.Tag) != "H" && Convert.ToString(this.Tag) != "D" && cikis == false)
            {

                //bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("select ISNULL((select TOP 1 isnull(Rsat_Fisno,0) from Cst_Recete_Satis WITH(NOLOCK) where  Rsat_Durum = 'A' and Rsat_Masa = '" + Masa_No + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'),0)"));
                bartxt_FisNo.EditValue = Fisno;
                if (Convert.ToInt32(bartxt_FisNo.EditValue) == 0)
                {
                    bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
                }
            }
            else
            {
                if (Param.Param_HizliSatisCekAc)
                {
                    dbtools.execcmdR($"update Cst_Recete_Satis set Rsat_Durum='K' where Rsat_Fisno='{Fisno}'");


                    bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
                    Fisno = Convert.ToInt32(bartxt_FisNo.EditValue.ToString());
                }
                else
                {
                    bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("select ISNULL((select TOP 1 isnull(Rsat_Fisno,0) from Cst_Recete_Satis WITH(NOLOCK) where  Rsat_Durum = 'A' and Rsat_Masa = '" + Masa_No + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'),0)"));
                    if (Convert.ToInt32(bartxt_FisNo.EditValue) == 0)
                    {
                        bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
                    }
                }
            }



        }


        private void Kapatma_Yenile()
        {
            flp_Kapatma.Controls.Clear();
            string filtre = String.Empty;

            if (Param.Fullcomp_Kodu == D_Odeme)
            {
                filtre = " and Pkod_Ozelkod in ('2','3') ";
            }
            else
            {
                filtre = " and Pkod_Ozelkod in ('0','1','5','6','7','3') ";
            }

            if (Departman.Kodlar_AndPos_NFC)
            {
                filtre = " and Pkod_Ozelkod in ('1') ";
            }


            DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");



            if (dt == null || dt.Rows.Count < 1)
            {
                dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43");
            }

            dt = Sabitler.getOdemeKodlari(dt);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string backcolor = Convert.ToString(dt.Rows[i]["Pkod_OdemeBtnRenk"]);
                    Color color = new Color();
                    if (backcolor != "")
                    {
                        color = System.Drawing.ColorTranslator.FromHtml(backcolor);
                    }

                    SimpleButton btn_Kapatma = new SimpleButton();
                    //if (Convert.ToString(this.Tag) ==  "H") btn_Kapatma.Size = new Size(150, 50);
                    btn_Kapatma.TabIndex = 0;
                    //btn_Kapatma.Size = new System.Drawing.Size(68, 50);
                    btn_Kapatma.Size = new System.Drawing.Size(90, 43);
                    btn_Kapatma.TabStop = false;
                    //btn_Kapatma.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                    btn_Kapatma.Font = new System.Drawing.Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    btn_Kapatma.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btn_Kapatma.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Kapatma.Appearance.BackColor = color;
                    btn_Kapatma.Appearance.Options.UseBackColor = true;

                    btn_Kapatma.Text = Convert.ToString(dt.Rows[i]["Pkod_Ad"]);
                    btn_Kapatma.Tag = Convert.ToString(dt.Rows[i]["Pkod_Kod"]);

                    btn_Kapatma.Click += new EventHandler(btn_Kapatma_Click);
                    flp_Kapatma.Controls.Add(btn_Kapatma);
                }
            }
            //if (Departman.Siparis)
            //{
            //    FisPr pr = new FisPr();
            //    pr.SiparisPr(Convert.ToInt32(bartxt_FisNo.EditValue), false, Split);
            //}

        }

        string FolioKart_No = String.Empty;
        string FolioKart_ID = "";

        private void Fis_Update()
        {
            if (Convert.ToString(D_Mus_tipi) != String.Empty)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_MusTipi = '" + D_Mus_tipi + "',Rsat_Odano = '" + D_Oda_No + "',Rsat_Folio = '" + D_Folio + "',Rsat_Pansiyon = '" + D_Pansiyon + "',Rsat_Uye_Id = '" + D_Uye_Id + "', "
                                        + " Rsat_Uye_Ad = '" + D_Uye_Adsoyad + "',Rsat_Uye_Kart_Turu = '" + D_Uye_Kartturu + "', Rsat_Indkodu = '" + D_Ind_Kodu + "',Rsat_Indoran = '" + D_Ind_Oran.ToString().Replace(",", ".") + "', "
                                         + " Rsat_Kartno = '" + Kart_No + "', Rsat_Kart_ID = '" + FolioKart_ID + "' "
                                        + " where Rsat_Fisno = '" + bartxt_FisNo.EditValue.ToString() + "' and Rsat_Ba = 'B' ");
                dbtools.execcmd("exec Pos_Satis_Induyg @Fisno = " + bartxt_FisNo.EditValue.ToString());
            }
        }

        private bool LimitKontrol()
        {
            try
            {
                if (otomatikSatis)
                {
                    return true;
                }


                if (Param.Tesis_Tipi == 1)
                {
                    return true;
                }

                if (!Departman.Kodlar_AndPos_NFC)
                {

                    if (Fronttools.Folio_LimitBakiye_Bul(D_MasterId))    //Folio içinde Limit Bakiyeden Bulunacak
                    {
                        string bilgi = "";
                        bilgi = bilgi.Contains("- Bakiye : ") ? bilgi.Substring(0, bilgi.IndexOf("- Bakiye : ")) : bilgi;

                        decimal folioBakiye = Fronttools.BalanceBul(D_MasterId, Kart_No, FolioKart_ID); //13,95

                        decimal odemeTutar = Convert.ToDecimal(gridColumn4.SummaryText);//15,28
                        bilgi = bilgi + " - Bakiye : " + (folioBakiye).ToString("N2");

                        if (Param.Calisma_Sekli == 1)
                        {
                            odemeTutar = Convert.ToDecimal(gridColumn5.SummaryText);//15,28

                        }

                        if (((-1) * folioBakiye) - odemeTutar < 0)
                        {
                            MessageBox.Show(D_Oda_No + " NL Hesap Bakiye Yetersizdir.", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return false;
                        }
                    }
                    else
                    {
                        if (Fronttools.LimitUyarı_Bul(D_MasterId) == "E")
                        {
                            string bilgi = "";
                            bilgi = bilgi.Contains("- Bakiye : ") ? bilgi.Substring(0, bilgi.IndexOf("- Bakiye : ")) : bilgi;

                            decimal limitBakiye = Fronttools.Folio_LimitTutar_Bul(D_MasterId); //13,95

                            decimal folioBakiye = Fronttools.BalanceBul(D_MasterId, FolioKart_ID.ToString(), FolioKart_ID); //13,95

                            decimal odemeTutar = Convert.ToDecimal(gridColumn4.SummaryText);//15,28


                            if (Param.Calisma_Sekli == 0)
                            {
                                odemeTutar = Convert.ToDecimal(gridColumn4.SummaryText);//15,28
                            }
                            else
                            {
                                odemeTutar = Convert.ToDecimal(gridColumn5.SummaryText);//15,28
                            }


                            bilgi = bilgi + " - Bakiye : " + limitBakiye.ToString("N2") + " - " + folioBakiye.ToString("N2") + "=" + (limitBakiye - folioBakiye).ToString("N2");

                            limitBakiye = limitBakiye - folioBakiye;
                            if (limitBakiye - odemeTutar < 0)
                            {
                                MessageBox.Show(D_Oda_No + " NL Hesap Bakiye Yetersizdir." + "\n" + " Kalan Bakiye : " + limitBakiye.ToString("N2"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return false;
                            }
                        }
                    }
                }
                else
                {


                    if (Fronttools.CardFLimitUyarı_Bul(FolioKart_ID.ToString()) == "E")
                    {
                        string bilgi = "";
                        bilgi = bilgi.Contains("- Bakiye : ") ? bilgi.Substring(0, bilgi.IndexOf("- Bakiye : ")) : bilgi;
                        decimal odemeTutar = 0;
                        decimal folioBakiye = Fronttools.BalanceBul(D_MasterId, FolioKart_ID.ToString(), FolioKart_ID); //13,95
                        if (Param.Calisma_Sekli == 0) // otelse
                        {
                            //odemeTutar = Convert.ToDecimal(gridColumn4.SummaryText);//15,28

                            odemeTutar = siparisToplamTutar; // 04.08.2022 özhan ile kontrol ederek ekledik
                        }
                        else
                        {
                            odemeTutar = Convert.ToDecimal(gridColumn5.SummaryText);//15,28
                        }
                        bilgi = bilgi + " - Bakiye : " + (folioBakiye).ToString("N2");
                        if (((-1) * folioBakiye) - odemeTutar < 0)
                        {
                            MessageBox.Show(D_Oda_No + " NL Hesap Bakiye Yetersizdir.", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                RHMesaj.alertMesaj2("HATA LimitKontrol() " + ex.Message);
            }


            return true;
        }

        string Kart_No = String.Empty;


        decimal siparisToplamTutar = 0;
        void btn_Kapatma_Click(object sender, EventArgs e)
        {

            try
            {
                if (gridColumn4.SummaryText == "") return;
                siparisToplamTutar = Convert.ToDecimal(gridColumn4.SummaryText);//15,28

                SimpleButton btn_Kapatma = (SimpleButton)sender;

                if (Convert.ToString(this.Tag) == "H")    // Hızlı Satış
                {

                    DataTable dtt = dbtools.SelectTable("select Pkod_Ozelkod,isnull(Pkod_Tekoda,0) as Pkod_Tekoda from Pos_Kodlar with(nolock) where Pkod_Sinif = '11' and Pkod_Kod  = '" + btn_Kapatma.Tag.ToString() + "'");
                    string ozelKod = Convert.ToString(dtt.Rows[0]["Pkod_Ozelkod"]);
                    bool tekOda = Convert.ToBoolean(dtt.Rows[0]["Pkod_Tekoda"]);


                    if (tekOda)
                    {
                        Kapatma_Tekoda(btn_Kapatma.Tag.ToString());
                    }
                    else
                    {

                        //else
                        //{
                        //string adSoyad = "";
                        //decimal kartBakiye = 0;
                        //if (ozelKod == "5")
                        //{
                        //    Cari_KartSor k = new Cari_KartSor(tutar);
                        //    k.ShowDialog();

                        //    if (k.Onay)
                        //    {
                        //        this.D_Mus_tipi = "C";
                        //        this.D_Cari_Kod = k.Cari_Kod;
                        //        adSoyad = k.Cari_Ad + " " + k.Cari_Soyad;
                        //        kartBakiye = k.kartBakiye;
                        //    }
                        //    else
                        //    {
                        //        return;
                        //    }
                        //}


                        if (Param.Param_HesapSor)
                        {
                            Hesap hes = new Hesap();
                            hes.look_Kapatma.EditValue = btn_Kapatma.Tag;

                            string kod = dbtools.DegerGetir("select Pkod_otoKur from Pos_Kodlar where Pkod_Sinif='11' and Pkod_Kod='" + btn_Kapatma.Tag + "'");


                            hes.look_DovizKod.EditValue = kod;

                            hes.Tag = Convert.ToInt32(bartxt_FisNo.EditValue);
                            hes.ShowDialog();
                            cikis = hes.cikis;
                            Fisno = hes.fisno;
                            gridyenile();
                            //return;
                        }


                        Arama ara = new Arama();


                        if (Param.Param_SatisArama == true)
                        {
                            ara.KapatmaKodu = Convert.ToString(btn_Kapatma.Tag);
                            ara.Odeme_Ozelkod = Convert.ToInt32(ozelKod);
                            ara.ShowDialog();

                            if (ara.HizliSatis == false)
                            {
                                return;
                            }

                            if (string.IsNullOrEmpty(ara.Oda_No))
                            {
                                return;
                            }

                            D_Mus_tipi = ara.Mus_tipi;
                            D_Oda_No = ara.Oda_No;
                            D_Folio = ara.Folio;
                            D_MasterId = ara.Master_Folio;
                            D_Pansiyon = ara.Pansiyon;
                            D_Uye_Id = ara.Uye_Id;
                            D_Uye_Adsoyad = ara.Uye_Adsoyad;
                            D_Uye_Kartturu = ara.Uye_Kartturu;
                            D_Cari_Kod = ara.Cari_Kod;
                            D_Ind_Kodu = ara.Ind_Kodu;
                            D_Ind_Oran = ara.Ind_Oran;
                            D_Odeme = ara.Odeme_Kodu;
                            Kart_No = ara.Kart_No;
                            FolioKart_ID = ara.KartID;
                            CardF_Indirim = ara.CardF_Indirim;
                        }


                        if (Param.Tesis_Tipi == 0)
                        {
                            if (Fronttools.HesapKapali_Kontrol(ara.Folio))
                            {
                                MessageBox.Show(res_man.GetString("Seçilen Hesap Harcamaya Kapatılmıştır.") + "\n" + res_man.GetString("Satış İşlemi Gerçekleşecektir."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }

                    if ((D_Folio == 0 || D_Oda_No == "") && D_Cari_Kod == "")
                    {
                        System.Windows.Forms.MessageBox.Show(res_man.GetString("Folio Bulunamadı...") + "\n" + res_man.GetString("Lütfen Hesabı Tekrar Kapatın..."), res_man.GetString("Uyarı"), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }

                    if (D_Ind_Kodu != null)
                    {
                        Fis_Update();
                    }


                    if (!LimitKontrol())
                    {
                        D_Mus_tipi = "";
                        D_Oda_No = "";
                        D_Folio = 0;
                        D_MasterId = 0;
                        D_Pansiyon = "";
                        D_Uye_Id = 0;
                        D_Uye_Adsoyad = "";
                        D_Uye_Kartturu = "";
                        D_Cari_Kod = "";
                        D_Ind_Kodu = "";
                        D_Ind_Oran = 0;
                        D_Odeme = "";

                        Fis_Update();

                        gridyenile();
                        return;
                    }

                    gridyenile();


                    decimal tutar, doviztutar;
                    if (Param.Calisma_Sekli == 1)       //Döviz
                    {
                        doviztutar = Convert.ToDecimal(gridColumn5.SummaryText);
                        tutar = doviztutar * Param.Doviz_Kuru;
                    }
                    else
                    {
                        string toplam = gridColumn4.SummaryText;
                        if (toplam == "")
                        {
                            toplam = "0";
                        }
                        tutar = Convert.ToDecimal(toplam);
                        doviztutar = tutar;
                    }

                    if (Departman.Siparis)
                    {
                        Siparis_Gonder(false);
                    }


                    Fis_Islem.Odeme_Al(Convert.ToInt32(bartxt_FisNo.EditValue), tutar, doviztutar, btn_Kapatma.Tag.ToString(), D_Mus_tipi, D_Oda_No, D_Folio, D_Cari_Kod, Split, Param.Doviz_Kodu, false);
                    Fis_Islem.Satis_Tip(Convert.ToInt32(bartxt_FisNo.EditValue), btn_Kapatma.Tag.ToString(), D_Pansiyon);


                    //if (Param.Tesis_Tipi == 0) // 07.01.2025 de yorum satırı yapıldı
                    //{
                    //    Fis_Islem.Onburo_At(Convert.ToInt32(bartxt_FisNo.EditValue), Kart_No, FolioKart_ID == "" ? 0 : Convert.ToInt32(FolioKart_ID));
                    //}


                    decimal folioBakiye = 0;

                    if (Param.Param_SatisArama == true)
                    {
                        folioBakiye = Fronttools.BalanceBul(D_MasterId, Kart_No, FolioKart_ID) * (-1);

                        FisPr pr = new FisPr();
                        string sonuc = "";
                        if (ozelKod == "1") sonuc = pr.SiparisPr(Convert.ToInt32(bartxt_FisNo.EditValue), false, Split, "   * * * SATIS FISI * * *   ", D_Oda_No, "Kalan Bakiye :  " + (folioBakiye - tutar).ToString("N2"), true);
                        else sonuc = pr.SiparisPr(Convert.ToInt32(bartxt_FisNo.EditValue), false, Split, "   * * * SATIS FISI * * *   ", "", "", true);
                        if (sonuc != "OK")
                        {
                            MessageBox.Show(sonuc);
                        }

                        if (Departman.Adisyon)
                        {
                            AdisyonPr ads = new AdisyonPr();
                            ads.Adisyon_Yaz(Convert.ToInt32(bartxt_FisNo.EditValue));
                            ads.Adisyon_Sayac_Arttir(Convert.ToInt32(bartxt_FisNo.EditValue));
                        }
                        else
                        {
                            //FisPr pr = new FisPr();
                            if (Param.Param_YeniHesapDkm)
                            {
                                pr.newHesapDokum(true, Convert.ToInt32(bartxt_FisNo.EditValue), 0, "* * * HESAP KAPATMA FİŞİ * * *");
                            }
                            else
                            {
                                pr.HesapDokum(true, Convert.ToInt32(bartxt_FisNo.EditValue), 0);
                            }
                        }

                        if (Departman.Fatura)
                        {
                            Fis_Islem.Fatura_Kes(Convert.ToInt32(bartxt_FisNo.EditValue), false);
                        }
                    }

                    btn_Cikis.Enabled = true;

                    Bilgileri_Doldur();





                    return;
                }

                if (Param.Tesis_Tipi == 0)//otel
                {


                    // Ödemesi Alınıyor...
                    bool etiketim = true;
                etiket:
                    decimal tutar, doviztutar;
                    if (Param.Calisma_Sekli == 1)       //Döviz
                    {
                        doviztutar = Convert.ToDecimal(gridColumn5.SummaryText);


                        tutar = doviztutar * StatikModel.getOdaGirisKur(D_Oda_No, Param.Doviz_Kuru);


                    }
                    else
                    {
                        tutar = Convert.ToDecimal(gridColumn4.SummaryText);
                        doviztutar = tutar;
                    }

                    string ozelKod = dbtools.DegerGetir("select Pkod_Ozelkod from Pos_Kodlar with(nolock) where Pkod_Sinif = '11' and Pkod_Kod  = '" + btn_Kapatma.Tag.ToString() + "'");

                    if (ozelKod == "5")
                    {
                        if (!Direk_Satis())
                        {
                            return;
                        }
                    }


                    // indirim uygulama 
                    if (Departman.Kodlar_AndPos_NFC && CardF_Indirim != 0 && etiketim == true) // User.Pos_KartfIndirimAktif 
                    {
                        etiketim = false;
                        Fis_Islem.Manuel_Indirim(Convert.ToInt32(bartxt_FisNo.EditValue), "Y", tutar, doviztutar, CardF_Indirim, Split);
                        gridyenile();
                        Application.DoEvents();
                        goto etiket;
                    }


                    if (!LimitKontrol())
                    {
                        return;
                    }


                    Fis_Islem.Odeme_Al(Convert.ToInt32(bartxt_FisNo.EditValue), tutar, doviztutar, btn_Kapatma.Tag.ToString(), D_Mus_tipi, D_Oda_No, D_Folio, D_Cari_Kod, Split, Param.Doviz_Kodu, false);
                    Fis_Islem.Satis_Tip(Convert.ToInt32(bartxt_FisNo.EditValue), btn_Kapatma.Tag.ToString(), D_Pansiyon);
                    Fis_Islem.Onburo_At(Convert.ToInt32(bartxt_FisNo.EditValue), Kart_No, FolioKart_ID == "" ? 0 : Convert.ToInt32(FolioKart_ID), ozelKod: ozelKod);

                    //if (Convert.ToString(this.Tag) ==  "D" && Param.Param_DirekAdisyonPrSor == true)
                    //{
                    //    DialogResult c = MessageBox.Show(res_man.GetString("Adisyon Yazdırılsın mı ? ", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //    if (c == DialogResult.Yes)
                    //    {
                    //if (Departman.Adisyon)
                    //{
                    //    AdisyonPr ads = new AdisyonPr();
                    //    ads.Adisyon_Yaz(Convert.ToInt32(bartxt_FisNo.EditValue));
                    //    ads.Adisyon_Sayac_Arttir(Convert.ToInt32(bartxt_FisNo.EditValue));
                    //}
                    //else
                    //{
                    //    FisPr fis = new FisPr();
                    //    fis.HesapDokum(false, Convert.ToInt32(bartxt_FisNo.EditValue), 0);
                    //}

                    //if (Departman.Fatura)
                    //{
                    //    Fis_Islem.Fatura_Kes(Convert.ToInt32(bartxt_FisNo.EditValue), false);
                    //}



                    //        return;
                    //    }
                    //    else
                    //    {
                    //Bilgileri_Doldur();
                    //        return;
                    //    }
                    //}

                    if (Departman.Kodlar_AndPos_NFC)
                    {
                        if (D_Ind_Kodu != null)
                        {
                            Fis_Update();
                        }

                        gridyenile();

                        //if (!LimitKontrol())
                        //{
                        //    D_Mus_tipi = "";
                        //    D_Oda_No = "";
                        //    D_Folio = 0;
                        //    D_MasterId = 0;
                        //    D_Pansiyon = "";
                        //    D_Uye_Id = 0;
                        //    D_Uye_Adsoyad = "";
                        //    D_Uye_Kartturu = "";
                        //    D_Cari_Kod = "";
                        //    D_Ind_Kodu = "";
                        //    D_Ind_Oran = 0;
                        //    D_Odeme = "";

                        //    Fis_Update();

                        //    gridyenile();
                        //    return;
                        //}

                        dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 16,@Masano = '" + Masa_No + "',@Dep_Kodu = '" + Departman.Dep_Kodu + "'");

                        string masa = Masa_No == null ? "" : Masa_No;
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, "Fiş Kapatma. Fisno:" + Convert.ToInt32(bartxt_FisNo.EditValue.ToString()) + " Masano:" + masa, Convert.ToString(bartxt_FisNo.EditValue.ToString()), "");


                    }


                    if (Param.Param_YeniHesapDkm)
                    {
                        FisPr fis = new FisPr();
                        fis.newHesapDokum(true, Convert.ToInt32(bartxt_FisNo.EditValue), 0, "* * * HESAP KAPATMA FİŞİ * * *");
                    }
                    else if (Departman.Adisyon)
                    {
                        AdisyonPr ads = new AdisyonPr();
                        ads.Adisyon_Yaz(Convert.ToInt32(bartxt_FisNo.EditValue));
                        ads.Adisyon_Sayac_Arttir(Convert.ToInt32(bartxt_FisNo.EditValue));
                    }
                    else
                    {
                        FisPr fis = new FisPr();
                        fis.HesapDokum(false, Convert.ToInt32(bartxt_FisNo.EditValue), 0);
                    }

                    if (Departman.Kodlar_AndPos_NFC) // User.Pos_KartfIndirimAktif 
                    {
                        Siparis_Gonder(false);
                    }


                    if (Departman.Fatura)
                    {
                        Fis_Islem.Fatura_Kes(Convert.ToInt32(bartxt_FisNo.EditValue), false);
                    }

                    if (Departman.Kodlar_AndPos_NFC)
                    {
                        this.Close();
                    }
                    else
                    {
                        Bilgileri_Doldur();
                    }
                }
                else
                {
                    Hesap hes = new Hesap();
                    hes.look_Kapatma.EditValue = btn_Kapatma.Tag;


                    string kod = dbtools.DegerGetir("select Pkod_otoKur from Pos_Kodlar where Pkod_Sinif='11' and Pkod_Kod='" + btn_Kapatma.Tag + "'");

                    hes.look_DovizKod.EditValue = kod;

                    hes.Tag = Convert.ToInt32(bartxt_FisNo.EditValue);
                    hes.tip = "D";
                    hes.ShowDialog();
                    cikis = hes.cikis;
                    Fisno = hes.fisno;
                    gridyenile();

                    if (Fisno == -2 || Convert.ToString(dbtools.DegerGetir("select Rsat_Durum from Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue) + "'")) == "K")
                    {
                        Bilgileri_Doldur();
                    }
                }

                barkodFocuslan();
                txtAdisyon.EditValue = "";
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btn_Kapatma_Click", "", ex);
            }
        }

        private void Kapatma_Tekoda(string kapatma)
        {
            DataTable dtOda = dbtools.SelectTable("select isnull(Pkod_Tekoda,0) as Pkod_Tekoda,Pkod_Odano,Pkod_Ozelkod from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '11' and Pkod_Kod = '" + kapatma + "' ");
            if (dtOda.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dtOda.Rows[0]["Pkod_Tekoda"]))
                {
                    string odano = Convert.ToString(dtOda.Rows[0]["Pkod_Odano"]);

                    if (Param.Tesis_Tipi == 1)
                    {
                        return;
                    }

                    HesapBul ara = new HesapBul();
                    ara.data = odano;
                    if (ara.Arama_Yap() != "OK")
                    {
                        MessageBox.Show(res_man.GetString("Hesap Bulunamadı..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    D_Mus_tipi = ara.Mus_tipi;
                    D_Oda_No = ara.Oda_No;
                    D_Folio = ara.Folio;
                    D_MasterId = ara.Master_Folio;
                    D_Pansiyon = ara.Pansiyon;
                    D_Uye_Id = ara.Uye_Id;
                    D_Uye_Adsoyad = ara.Uye_Adsoyad;
                    D_Uye_Kartturu = ara.Uye_Kartturu;
                    D_Cari_Kod = ara.Cari_Kod;
                    D_Ind_Kodu = ara.Ind_Kodu;
                    D_Ind_Oran = ara.Ind_Oran;
                    D_Odeme = ara.Odeme_Kodu;
                    //Kart_No = ara.
                }
            }
            barkodFocuslan();
        }


        public void barkodFocuslan()
        {
            txt_Barkod.Focus();
            txt_Barkod.Select();

            if (Param.urunAdinaOdaklan)
            {
                txt_Filtre.Focus();
                txt_Filtre.Select();
            }
            //if (barkodAktifmi.ToLower().Equals("false")) // barkod sistemi aktif değil
            //{
            //    txt_Filtre.Focus();
            //}
            //else
            //{
            //    txt_Barkod.Focus();
            //    txt_Barkod.Select();
            //}

            if (Param.hesapFisQrFisno)
            {
                fisnoBirlestirFocus();
            }

        }

        private bool LimitKontrol(decimal odemeTutar)
        {

            if (otomatikSatis)
            {
                return true;
            }

            if (Param.Tesis_Tipi == 1)
            {
                return true;
            }



            if (Departman.Kodlar_AndPos_NFC && Fronttools.LimitUyarı_Bul(D_MasterId) == "E") // && den sonrası 10.05.2023 tarihinde eklendi
            {
                if (D_MasterId > 0) // burası çalıştı 
                {
                    if (Fronttools.NFC_LimitUyarı_Bul(D_MasterId, Kart_No) == "GB")
                    {
                        string bilgi = "";
                        bilgi = bilgi.Contains("- Bakiye : ") ? bilgi.Substring(0, bilgi.IndexOf("- Bakiye : ")) : bilgi;

                        decimal folioBakiye = Fronttools.NFC_BalanceBul(D_MasterId, FolioKart_ID == "" ? 0 : Convert.ToInt32(FolioKart_ID)); //13,95

                        if ((folioBakiye) - odemeTutar < 0)
                        {
                            MessageBox.Show(Kart_No + " Nolu Hesap Bakiyesi Yetersizdir.", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return false;
                        }

                        return true;
                    }
                }
            }
            else
            {

                if (Convert.ToString(this.Tag) != "D")
                {
                    return true;
                }

                if (Fronttools.Folio_LimitBakiye_Bul(D_MasterId))    //Folio içinde Limit Bakiyeden Bulunacak
                {
                    string bilgi = "";
                    bilgi = bilgi.Contains("- Bakiye : ") ? bilgi.Substring(0, bilgi.IndexOf("- Bakiye : ")) : bilgi;

                    decimal folioBakiye = Fronttools.BalanceBul(D_MasterId, Kart_No, FolioKart_ID); //13,95

                    if (((-1) * folioBakiye) - odemeTutar < 0)
                    {
                        MessageBox.Show(D_Oda_No + " NL Hesap Bakiye Yetersizdir.", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }
                else
                {
                    if (Fronttools.LimitUyarı_Bul(D_MasterId) == "E")
                    {

                        decimal limitBakiye = Fronttools.Folio_LimitTutar_Bul(D_MasterId); //13,95
                        decimal folioBakiye = Fronttools.BalanceBul(D_MasterId, Kart_No, FolioKart_ID); //13,95

                        limitBakiye = limitBakiye - folioBakiye;
                        if (limitBakiye - odemeTutar < 0)
                        {
                            MessageBox.Show(D_Oda_No + " NL Hesap Bakiye Yetersizdir." + "\n" + "Kalan Bakiye : " + limitBakiye.ToString("N2"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void gridyenile()
        {
            rdo_EMiktar.SelectedIndex = 1;

            gridColumn1.FieldName = "Rec_Ad";
            gridColumn2.FieldName = "Rsat_Miktar";
            gridColumn3.FieldName = "Rsat_Emiktar";
            gridColumn4.FieldName = "Rsat_Tutar";
            gridColumn5.FieldName = "Rsat_Doviztutar";
            gridColumn6.FieldName = "Rsat_Aciklama";
            gridColumn7.FieldName = "Rsat_Recete";
            gridColumn8.FieldName = "Rsat_Ba";
            gridColumn9.FieldName = "Rsat_Id";

            if (Param.Calisma_Sekli == 1)   //Dövizli
            {
                gridColumn5.Visible = true;
            }
            else
            {
                gridColumn4.Visible = true;
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Convert.ToInt32(bartxt_FisNo.EditValue));
            com.Parameters.AddWithValue("@Rapor_Tipi", 0);
            com.Parameters.AddWithValue("@Split", Split);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);



            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    item["Rsat_Miktar"] = item["Rsat_Miktar"].ToString().Replace(",0000", "");
                    item["Rsat_Miktar"] = item["Rsat_Miktar"].ToString().Replace(",000", "");
                    //item["Rsat_Tutar"] = item["Rsat_Tutar"].ToString().Replace(",00", "");
                }
            }



            DataTable dtCloned = dt.Clone();
            dtCloned.Columns["Rsat_Tutar"].DataType = typeof(string);
            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }


            gridControl1.DataSource = dtCloned;


            //gridView1.BestFitColumns();

            if (dt.Rows.Count > 0)
            {
                txt_Not.Text = Convert.ToString(dt.Rows[0]["Rsat_Not"]);
                txtAdisyon.EditValue = Convert.ToString(dt.Rows[0]["Rsat_Adisyon"]);
                bartxt_AcilisSaat.Caption = Convert.ToString(dt.Rows[0]["Rsat_Acilis"]).Length > 8 ? "Açilış Saat : " + Convert.ToString(dt.Rows[0]["Rsat_Acilis"]).Substring(0, 8) : "";
            }

            if (Convert.ToString(this.Tag) == "D" || Departman.Siparis)
            {
                if (dt.Rows.Count == 0)
                {
                    btn_Cikis.Enabled = true;
                }
                else
                {
                    //btn_Cikis.Enabled = false;
                    btn_Cikis.Enabled = Param.Param_SatisCikisButton; // sonradan ramazan ekledi bu satırı
                }
            }
            SiparisKontrol();

            Main.a.Listele(Convert.ToInt32(bartxt_FisNo.EditValue));


            barkodFocuslan();
        }




        private void Ust_Yenile()
        {
            flp_AltGrup.Controls.Clear();
            flp_UrunGrup.Controls.Clear();
            flp_Urun.Controls.Clear();

            DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 0, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string size = Convert.ToString(dt.Rows[i]["Kodlar_Size"]) == "" ? "100;40" : Convert.ToString(dt.Rows[i]["Kodlar_Size"]);
                    string[] sizeArray = size.Split(';');

                    Font mfont = new System.Drawing.Font("Tahoma", 8F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    string font = Convert.ToString(dt.Rows[i]["Kodlar_Font"]);
                    if (font != "")
                    {
                        System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                        mfont = (Font)converter.ConvertFromString(font);
                    }

                    string backcolor = Convert.ToString(dt.Rows[i]["Kodlar_Backcolor"]);
                    Color color = new Color();
                    if (backcolor != "")
                    {
                        color = System.Drawing.ColorTranslator.FromHtml(backcolor);
                    }


                    SimpleButton btn_AnaGrup = new SimpleButton();
                    btn_AnaGrup.Size = new System.Drawing.Size(Convert.ToInt32(sizeArray[0]), Convert.ToInt32(sizeArray[1]));
                    btn_AnaGrup.TabIndex = 0;
                    btn_AnaGrup.TabStop = false;
                    btn_AnaGrup.Font = mfont;
                    btn_AnaGrup.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btn_AnaGrup.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_AnaGrup.Appearance.Options.UseBackColor = true;
                    btn_AnaGrup.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                    btn_AnaGrup.Appearance.BackColor = color;

                    btn_AnaGrup.Text = Convert.ToString(dt.Rows[i]["Ana_Grupad"]);
                    btn_AnaGrup.Tag = Convert.ToString(dt.Rows[i]["Kont_Anagrup"]);


                    btn_AnaGrup.Click += new EventHandler(btn_AnaGrup_Click);
                    flp_AnaGrup.Controls.Add(btn_AnaGrup);

                    if (i == 0)
                    {
                        btn_AnaGrup.PerformClick();
                    }
                }
            }
        }

        void btn_AnaGrup_Click(object sender, EventArgs e)
        {
            flp_AltGrup.Controls.Clear();
            flp_UrunGrup.Controls.Clear();
            flp_Urun.Controls.Clear();

            SimpleButton btn_AnaGrup = (SimpleButton)sender;
            Ana_Grup = btn_AnaGrup.Tag.ToString();

            Alt_Yenile();

            if (kisiyeSatisAktifmi == "1" || kisiyeSatisAktifmi == "True")
            {
                txtKisiyeSatis.Select();
                txtKisiyeSatis.Focus();
            }

        }

        private void Alt_Yenile()
        {
            flp_Urun.Controls.Clear();

            string filter = "";
            string filter2 = "";
            if (!Param.Param_Anagrup_Cikmasin)
            {
                filter = "and Kont_Anagrup = '" + Ana_Grup + "'";
                filter2 = " and Rec_Anagrup = '" + Ana_Grup + "'";
            }

            DataTable dt = dbtools.SelectTable("SELECT top 1 Rdep_Departman as Kont_Departman,Rec_Anagrup as Kont_Anagrup,'SIKKULLAN' as Kont_Aragrup,'SIKKULLAN' as Kont_Aragrup2,'(*) SIK KULLANILAN' as Kodlar_Ad,-10000 as Kodlar_Sira,NULL  as Kodlar_Size,NULL as Kodlar_Font,NULL as Kodlar_Backcolor "
                + " FROM Cst_Recete_Dep WITH(NOLOCK)  "
                + "     left join Cst_Recete on Rec_Genelkod = Rdep_Recete "
                + " WHERE Rdep_Departman = '" + Departman.Dep_Kodu + "' " + filter2 + " and ISNULL(Rdep_SikKullanilan,0) = 1 "
                + " UNION ALL "
                + " SELECT Kont_Departman, Kont_Anagrup, Kont_Aragrup,  Kont_Anagrup +'#'+ Kont_Aragrup as Kont_Aragrup2, Ara_Grup.Kodlar_Ad,isnull(Ara_Grup.Kodlar_Sira,0),ISNULL(NULLIF(Ara_Grup.Kodlar_Size,''),'100;40') as Kodlar_Size,Ara_Grup.Kodlar_Font,Ara_Grup.Kodlar_Backcolor "
                + " FROM Pos_Grup "
                + " left join Stok_Kodlar as Ana_Grup ON Kont_Anagrup = Ana_Grup.Kodlar_Kod AND Ana_Grup.Kodlar_Sinif = '08' "
                + " left join Stok_Kodlar as Ara_Grup ON Kont_Aragrup = Ara_Grup.Kodlar_Kod AND Ara_Grup.Kodlar_Sinif = '09' and Ara_Grup.Kodlar_Anagrup =  Kont_Anagrup "
                + " WHERE Kont_Departman = '" + Departman.Dep_Kodu + "' " + filter + " ORDER BY Kodlar_Sira, Kodlar_Ad, Kont_Anagrup, Kont_Aragrup, Kont_Departman");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string size = Convert.ToString(dt.Rows[i]["Kodlar_Size"]) == "" ? "150;60" : Convert.ToString(dt.Rows[i]["Kodlar_Size"]);
                    string[] sizeArray = size.Split(';');

                    Font mfont = new System.Drawing.Font("Tahoma", 8F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    string font = Convert.ToString(dt.Rows[i]["Kodlar_Font"]);
                    if (font != "")
                    {
                        System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                        mfont = (Font)converter.ConvertFromString(font);
                    }

                    string backcolor = Convert.ToString(dt.Rows[i]["Kodlar_Backcolor"]);
                    Color color = new Color();
                    if (backcolor != "")
                    {
                        color = System.Drawing.ColorTranslator.FromHtml(backcolor);
                    }


                    SimpleButton btn_AltGrup = new SimpleButton();
                    btn_AltGrup.Size = new System.Drawing.Size(Convert.ToInt32(sizeArray[0]), Convert.ToInt32(sizeArray[1]));
                    btn_AltGrup.TabIndex = 0;
                    btn_AltGrup.TabStop = false;
                    btn_AltGrup.Font = mfont;
                    btn_AltGrup.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btn_AltGrup.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_AltGrup.Appearance.Options.UseBackColor = true;
                    btn_AltGrup.Appearance.BackColor = color;
                    btn_AltGrup.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;


                    btn_AltGrup.Text = Convert.ToString(dt.Rows[i]["Kodlar_Ad"]);
                    btn_AltGrup.Tag = Param.Param_Anagrup_Cikmasin == true ? Convert.ToString(dt.Rows[i]["Kont_Aragrup2"]) : Convert.ToString(dt.Rows[i]["Kont_Aragrup"]);

                    if (Convert.ToString(dt.Rows[i]["Kodlar_Ad"]) == "(*) SIK KULLANILAN")
                    {
                        string[] sizeArraySikKullan = Param.Param_SikKullanSize.Split(';');
                        btn_AltGrup.Size = new System.Drawing.Size(Convert.ToInt32(sizeArraySikKullan[0]), Convert.ToInt32(sizeArraySikKullan[1]));
                    }

                    btn_AltGrup.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_AltGrup.Click += new EventHandler(btn_AltGrup_Click);
                    flp_AltGrup.Controls.Add(btn_AltGrup);

                    if (i == 0)
                    {
                        //btn_AltGrup.PerformClick();
                    }
                }
            }
        }

        void btn_AltGrup_Click(object sender, EventArgs e)
        {
            btn_AltGrup = (SimpleButton)sender;


            if (Param.Param_Anagrup_Cikmasin && Convert.ToString(btn_AltGrup.Tag) != "SIKKULLAN")
            {
                string[] kod = btn_AltGrup.Tag.ToString().Split('#');

                Ana_Grup = kod[0];
                Alt_Grup = kod[1];
            }
            else
            {
                Alt_Grup = btn_AltGrup.Tag.ToString();
            }


            Urun_Grup_Yenile();

            Urun_Yenile();

            if (kisiyeSatisAktifmi == "1" || kisiyeSatisAktifmi == "True")
            {
                txtKisiyeSatis.Select();
                txtKisiyeSatis.Focus();
            }

        }

        private void Urun_Yenile()
        {
            flp_Urun.Controls.Clear();


            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Cost_Recete_Liste";
            com.Parameters.AddWithValue("@Rec_Anagrup", Ana_Grup);
            com.Parameters.AddWithValue("@Rec_Altgrup", Alt_Grup);
            com.Parameters.AddWithValue("@Liste_Tipi", 1);
            com.Parameters.AddWithValue("@Urun_Filtre", Convert.ToString(txt_Filtre.EditValue));
            com.Parameters.AddWithValue("@Urun_Kodu", Convert.ToString(txt_Filtre.EditValue));
            com.Parameters.AddWithValue("@Departman", Departman.Dep_Kodu);
            com.Parameters.AddWithValue("@Tip", PaketFiyat);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count == 0 && (User.P_Kod.Equals("999") || User.P_Kod.ToUpper().Equals("RMOS")))
            {
                // dbtools.execcmd("delete from Pos_Grup where Kont_Anagrup ='" + Ana_Grup + "' and Kont_Aragrup='" + Alt_Grup + "' and Kont_Departman='" + Departman.Dep_Kodu + "'");

                if (urunleriYenile)
                {
                    flp_AnaGrup.Controls.Clear();
                    Ust_Yenile();
                }

            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleButton btn_Urun = new SimpleButton();
                    btn_Urun.Size = new System.Drawing.Size(100, 40);//100 40
                    btn_Urun.TabIndex = 0;
                    btn_Urun.TabStop = false;
                    btn_Urun.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    btn_Urun.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    btn_Urun.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Urun.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    btn_Urun.Appearance.Options.UseBackColor = false;
                    btn_Urun.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;

                    if (btn_AltGrup != null)
                    {
                        btn_Urun.Size = btn_AltGrup.Size;

                        if (Convert.ToString(dt.Rows[i]["Rec_Color"]) == "")
                        {
                            btn_Urun.Appearance.BackColor = btn_AltGrup.Appearance.BackColor;
                        }
                        else
                        {
                            btn_Urun.Appearance.BackColor = ColorTranslator.FromHtml(Convert.ToString(dt.Rows[i]["Rec_Color"]));
                        }

                        btn_Urun.Font = btn_AltGrup.Font;
                    }

                    if (Param.Calisma_Sekli == 0) btn_Urun.Text = Convert.ToString(dt.Rows[i]["Rec_Ad"]) + "\n" + Convert.ToString(dt.Rows[i]["Rec_Fiyat"]);

                    if (Param.Calisma_Sekli == 1)
                    {
                        btn_Urun.Text = Convert.ToString(dt.Rows[i]["Rec_Ad"]) + "\n" + Convert.ToString(dt.Rows[i]["Rec_Dovifiyat"]);
                    }
                    else if (Convert.ToBoolean(dt.Rows[i]["Rec_DovizliSatis"].ToString()))
                    {
                        btn_Urun.Text = Convert.ToString(dt.Rows[i]["Rec_Ad"]) + "\n" + Convert.ToString(dt.Rows[i]["Rec_Dovifiyat"]) + " " + Convert.ToString(dt.Rows[i]["DovizAdi"]);
                    }

                    btn_Urun.Tag = Convert.ToString(dt.Rows[i]["Rec_Genelkod"]);

                    btn_Urun.Click += new EventHandler(btn_Urun_Click);

                    flp_Urun.Controls.Add(btn_Urun);
                }
            }
        }

       
        void btn_Urun_Click(object sender, EventArgs e)
        {
            if (Sabitler.otomatikGunsonuKontrol() == false)
            {
                this.Close();
                return;
            }


            SimpleButton btn_Urun = (SimpleButton)sender;

            if (Convert.ToString(this.Tag) == "D" && Param.Param_DirekAdisyonZor == true && Convert.ToString(txtAdisyon.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Satış yapmadan önce Adisyon Numarasını giriniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //DataTable dtUstRecete = dbtools.SelectTable("select Rec_Ad from Cst_Recete WITH(NOLOCK) where Rec_Ust_Recete = '" + btn_Urun.Tag.ToString() + "'");
            if (btn_Urun.Text.StartsWith("(*) "))
            {
                Alt_Recete alt = new Alt_Recete();
                alt.ustReceteKodu = btn_Urun.Tag.ToString();
                alt.ustReceteAdi = btn_Urun.Text.Split('\n')[0].ToString();
                alt.ShowDialog();
                if (Convert.ToString(alt.altReceteKodu) != "")
                {
                    Urun_Sat(alt.altReceteKodu);
                }
            }
            else
            {
                Urun_Sat(btn_Urun.Tag.ToString());
            }

            SiparisKontrol();
            //Siparis_Kontrol();

            txt_Filtre.Text = String.Empty;

            if (kisiyeSatisAktifmi == "1" || kisiyeSatisAktifmi == "True")
            {
                txtKisiyeSatis.Focus();
                txtKisiyeSatis.Select();
            }

        }

        private void Siparis_Kontrol()
        {
            if (Departman.Siparis == true)
            {
                //btn_Cikis.Enabled = false;
                btn_Cikis.Enabled = Param.Param_SatisCikisButton; // sonradan ramazan ekledi bu satırı
            }
        }


        public bool malzemeTr = false;
        public int Rsat_UrunTahsilat = 0;
        bool Rec_Terazi = false;
        bool Rec_Miktar_Gr = false;


        public void Urun_Sat(string Urun_Kodu, bool siparisPr = false, decimal recFiyat = 0, bool yenilemeYapma = false, int otomiktar = 1, bool urunduzeltme = false, bool urunTransfer = false)
        {
            try
            {
                Recete_Islem rec = new Recete_Islem();
                string Rsat_Odano = String.Empty, Rsat_Adisyon,
                     Rsat_Cari = String.Empty, Rsat_Paketci = String.Empty, Rsat_Indkodu = String.Empty, Rsat_Garson2, Rsat_Uye_Kart_Turu = String.Empty, Rsat_Pansiyon = String.Empty, Rsat_MusTipi = String.Empty, Rsat_Uye_Ad = String.Empty, Rsat_Onbdep = String.Empty, Rsat_KartNo = String.Empty;
                int Rsat_Folio = 0, Rsat_Kisi, Rsat_Uye_Id = 0;
                decimal Rsat_Indoran = 0;

                Rsat_Adisyon = Convert.ToString(txtAdisyon.EditValue);
                Rsat_Garson2 = Garson;
                Rsat_Folio = 0;
                Rsat_Kisi = Kisi_Sayisi;
                Rsat_Paketci = Paketci;
                Rsat_MusTipi = mCari != null ? "C" : "";
                Rsat_Cari = mCari != null ? mCari.Cari_Kod : "";


                DataTable dtRecete = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 2, @Dep_Kodu = '" + Departman.Dep_Kodu + "',@Rec_Genelkod = '" + Urun_Kodu + "', @Tip = '" + PaketFiyat + "'");

                string Rec_Ad = Convert.ToString(dtRecete.Rows[0]["Rec_Ad"]);


                decimal Rec_Fiyat = Convert.ToDecimal(dtRecete.Rows[0]["Rec_Fiyat"]);


                string yedekEmiktar = eMiktar;


                if (malzemeTr) // malzeme transferinden geliyorsa demek
                {
                    eMiktar = "T";
                }

                if (recFiyat != 0 || urunTransfer)
                {
                    Rec_Fiyat = recFiyat;
                }
                decimal Rec_Kdv = rec.Kdv_Bul(Urun_Kodu); //Convert.ToDecimal(dtRecete.Rows[0]["Rec_Kdv"]);



                decimal Rec_Dovifiyat = Convert.ToDecimal(dtRecete.Rows[0]["Rec_Dovifiyat"]);


                string Rec_Dovizkodu = Convert.ToString(dtRecete.Rows[0]["Rec_Dovizkodu"]);
                bool Rec_AciklamaAP = Convert.ToBoolean(dtRecete.Rows[0]["Rec_AciklamaAP"]);

                Rsat_Onbdep = Convert.ToString(dtRecete.Rows[0]["Pkod_OnburoKod"]);

                bool Rec_Miktar_Sor = Convert.ToBoolean(dtRecete.Rows[0]["Rec_Miktar_Sor2"]);
                Rec_Miktar_Gr = Convert.ToBoolean(dtRecete.Rows[0]["Rec_Miktar_Gr2"]);

                bool Rec_Tutar_Sor = Convert.ToBoolean(dtRecete.Rows[0]["Rec_Tutar_Sor2"]);
                bool Rec_DovizliSatis = false;

                if (dtRecete.Rows[0]["Rec_DovizliSatis"].ToString() != "")
                {
                    Rec_DovizliSatis = Convert.ToBoolean(dtRecete.Rows[0]["Rec_DovizliSatis"]);
                }


                Rec_Terazi = Convert.ToBoolean(dtRecete.Rows[0]["Rec_Terazi"]);

                if (Rec_Miktar_Sor && urunduzeltme == false)
                {
                    Klavye1 klv = new Klavye1();
                    klv.txt_Sayi.EditValue = Miktar;
                    klv.Tag = "GRAMSOR";
                    klv.MiktarGR = Rec_Miktar_Gr;
                    klv.ShowDialog();

                    if (klv.Cikis == true)
                    {
                        return;
                    }

                    if (klv.sayi <= 0) return;
                    if (Rec_Miktar_Gr)
                    {
                        Miktar = klv.sayi / 1000;
                    }
                    else
                    {
                        Miktar = klv.sayi;
                    }
                }


                if (Rec_Terazi == true)
                {
                    //TeraziAÇ

                    Pos_TeraziEkran p = new Pos_TeraziEkran();
                    p.txt_UrunAdi.Text = Rec_Ad;
                    p.ShowDialog();

                    if (p.Kapanis == false)
                    {
                        return;
                    }
                    else
                    {
                        Miktar = Convert.ToDecimal(p.DonenDeger);
                    }
                }

                if (Rec_AciklamaAP)
                {
                    UrunAciklama a = new UrunAciklama();
                    a.receteKodu = Urun_Kodu;
                    a.ShowDialog();
                    Aciklama = a.Aciklama;

                }
                decimal Rsat_Kdv, Rsat_Net, Rsat_Tutar, Eksileme = 1;

                if (chk_Fix.Checked == true)
                {
                    Rsat_Tutar = 0;
                    Rsat_Net = 0;
                    Rsat_Kdv = 0;
                    //decimal Rec_Kur = 0;
                    Rec_Dovifiyat = 0;
                }
                else
                {
                    if (chk_Eksi.Checked == true)
                    {
                        Eksileme = -1;
                    }


                    if (Param.Calisma_Sekli == 1) //Dövizli Çalışma Şekli
                    {
                        if (Param.Ent_Onb == true)
                        {
                            Rsat_Tutar = Rec_Dovifiyat * StatikModel.getOdaGirisKur(D_Oda_No, Param.Doviz_Kuru);
                            Rsat_Net = ((Rsat_Tutar * 100) / (100 + Rec_Kdv));
                            Rsat_Kdv = (Rsat_Tutar - Rsat_Net);
                        }
                        else
                        {
                            decimal getKur = StatikSinif.getKur();

                            Rsat_Tutar = Rec_Dovifiyat * getKur;
                            Rsat_Net = ((Rsat_Tutar * 100) / (100 + Rec_Kdv));
                            Rsat_Kdv = (Rsat_Tutar - Rsat_Net);
                        }

                    }
                    else if (Rec_DovizliSatis == true)
                    {
                        if (Param.Ent_Onb == true)
                        {
                            var Doviz_Kuru = Fronttools.KurGetir(DateTime.Now, Rec_Dovizkodu);
                            Rsat_Tutar = Rec_Dovifiyat * StatikModel.getOdaGirisKur(D_Oda_No, Doviz_Kuru);
                            Rsat_Net = ((Rsat_Tutar * 100) / (100 + Rec_Kdv));
                            Rsat_Kdv = (Rsat_Tutar - Rsat_Net);
                        }
                        else
                        {
                            decimal getKur = StatikSinif.getKurRecete(Rec_Dovizkodu);

                            Rsat_Tutar = Rec_Dovifiyat * getKur;
                            Rsat_Net = ((Rsat_Tutar * 100) / (100 + Rec_Kdv));
                            Rsat_Kdv = (Rsat_Tutar - Rsat_Net);
                        }


                    }
                    else        // TL Çalışma
                    {
                        Rsat_Tutar = Rec_Fiyat;
                        Rsat_Net = ((Rsat_Tutar * 100) / (100 + Rec_Kdv));
                        Rsat_Kdv = Rsat_Tutar - Rsat_Net;
                    }


                    if (Departman.Aktif_Kur && Param.Tesis_Tipi == 0)
                    {
                        decimal Rec_Kur = Fronttools.KurGetir(Param.Tarih, Rec_Dovizkodu);
                        Rsat_Tutar = Rec_Dovifiyat * Rec_Kur;
                        Rsat_Net = (Rsat_Tutar * 100) / (100 + Rec_Kdv);
                        Rsat_Kdv = Rsat_Tutar - Rsat_Net;
                    }

                    // Recete Manuel Fiyat Aktif
                    if (Rec_Tutar_Sor)
                    {
                        Klavye1 klv = new Klavye1();
                        klv.txt_Sayi.EditValue = Rsat_Tutar;
                        klv.Tag = "TUTARDUZELT";
                        klv.ShowDialog();

                        Rsat_Tutar = klv.sayi;
                        Rsat_Net = (Rsat_Tutar * 100) / (100 + Rec_Kdv);
                        Rsat_Kdv = Rsat_Tutar - Rsat_Net;
                    }

                    AyarlarController ayarlar = new AyarlarController();
                    if (Urun_Kodu == ayarlar.getYuvarlamaRecete())
                    {
                        Rsat_Tutar = Yuv_Tutar;
                        Rsat_Net = (Rsat_Tutar * 100) / (100 + Rec_Kdv);
                        Rsat_Kdv = Rsat_Tutar - Rsat_Net;
                    }


                    //Rsat_Tutar = Rsat_Tutar;// * Miktar;
                    //Rsat_Net = Rsat_Net;// * Miktar;
                    //Rsat_Kdv = Rsat_Kdv;// * Miktar;
                    //Rec_Dovifiyat = Rec_Dovifiyat;// * Miktar;

                }

                //Sabitlenmiş Oda bilgileri (varsa)
                if (Convert.ToString(this.Tag) == "M" || Convert.ToString(this.Tag) == "P")
                {
                    //DataTable dtSatis = dbtools.SelectTable("select top 1 Rsat_Odano, Rsat_Adisyon, Rsat_Cari, Rsat_Paketci, Rsat_Garson2, "
                    //                            + "     Rsat_Uye_Kart_Turu, Rsat_Pansiyon, Rsat_MusTipi, Rsat_Uye_Ad, Rsat_Indkodu, isnull(Rsat_Folio,0) as Rsat_Folio, isnull(Rsat_Kisi,0) as Rsat_Kisi, "
                    //                            + "     isnull(Rsat_Uye_Id,0) as Rsat_Uye_Id, isnull(Rsat_Indoran,0) as Rsat_Indoran "
                    //                            + " FROM Cst_Recete_Satis with(nolock) where Rsat_Fisno = '" + bartxt_FisNo.EditValue + "' and Rsat_Ba = 'B' order by Rsat_Indkodu desc");

                    DataTable dtSatis = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 3, @Fisno = '" + bartxt_FisNo.EditValue + "' ");

                    if (dtSatis.Rows.Count > 0)
                    {
                        Rsat_Odano = Convert.ToString(dtSatis.Rows[0]["Rsat_Odano"]);
                        Rsat_Adisyon = Convert.ToString(txtAdisyon.EditValue);// dbtools.DegerGetir("select top 1 Rsat_Adisyon from Cst_Recete_Satis with(nolock) where Rsat_Fisno = '" + bartxt_FisNo.EditValue + "' and Rsat_Ba = 'B' and isnull(Rsat_Adisyon,'') <> '' ");
                        Rsat_Cari = Convert.ToString(dtSatis.Rows[0]["Rsat_Cari"]);
                        Rsat_Paketci = Convert.ToString(dtSatis.Rows[0]["Rsat_Paketci"]);
                        Rsat_Garson2 = Convert.ToString(dtSatis.Rows[0]["Rsat_Garson2"]);
                        Rsat_Uye_Kart_Turu = Convert.ToString(dtSatis.Rows[0]["Rsat_Uye_Kart_Turu"]);
                        Rsat_Pansiyon = Convert.ToString(dtSatis.Rows[0]["Rsat_Pansiyon"]);
                        Rsat_MusTipi = Convert.ToString(dtSatis.Rows[0]["Rsat_MusTipi"]);
                        Rsat_Uye_Ad = Convert.ToString(dtSatis.Rows[0]["Rsat_Uye_Ad"]);
                        Rsat_Folio = Convert.ToInt32(dtSatis.Rows[0]["Rsat_Folio"]);
                        Rsat_Kisi = Convert.ToInt32(dtSatis.Rows[0]["Rsat_Kisi"]);
                        Rsat_Uye_Id = Convert.ToInt32(dtSatis.Rows[0]["Rsat_Uye_Id"]);
                        Rsat_Indkodu = Convert.ToString(dtSatis.Rows[0]["Rsat_Indkodu"]);
                        Rsat_Indoran = Convert.ToDecimal(dtSatis.Rows[0]["Rsat_Indoran"]);
                    }
                }
                //Direk Satış Bilgileri
                if (Convert.ToString(this.Tag) == "D")
                {
                    Rsat_Odano = D_Oda_No;
                    Rsat_Adisyon = Convert.ToString(txtAdisyon.EditValue);
                    Rsat_Cari = D_Cari_Kod;
                    Rsat_Paketci = String.Empty;
                    Rsat_Garson2 = Garson;
                    Rsat_Uye_Kart_Turu = D_Uye_Kartturu;
                    Rsat_Pansiyon = D_Pansiyon;
                    Rsat_MusTipi = D_Mus_tipi;
                    Rsat_Uye_Ad = D_Uye_Adsoyad;
                    Rsat_Folio = D_Folio;
                    Rsat_Kisi = Kisi_Sayisi;
                    Rsat_Uye_Id = D_Uye_Id;
                    Rsat_Indkodu = D_Ind_Kodu;
                    Rsat_Indoran = D_Ind_Oran;
                    Rsat_KartNo = Kart_No;

                }

                decimal limitTutar = 0;
                if (Param.Calisma_Sekli == 1) //Dövizli Çalışma Şekli
                {
                    if (gridColumn5.SummaryText != "")
                    {
                        limitTutar = Convert.ToDecimal(gridColumn5.SummaryText) + Rec_Dovifiyat;
                    }
                }
                else
                {
                    if (gridColumn4.SummaryText != "")
                    {
                        limitTutar = Convert.ToDecimal(gridColumn4.SummaryText) + Rsat_Tutar;
                    }
                }
                if (!LimitKontrol(limitTutar))
                {
                    return;
                }

                string masano = Convert.ToString(bartxt_MasaNo.EditValue ?? Masa_No);

                int satisFisno = Convert.ToInt32(bartxt_FisNo.EditValue);

                if (Convert.ToString(this.Tag) == "M")//hızlı satış ise H . Direkt Satış ise D .08.08.2024 güncellendi
                {
                    int fisnoKontrol = Convert.ToInt32(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 4, @Dep_Kodu = '" + Departman.Dep_Kodu + "',@Masano = '" + masano + "'"));

                    if (fisnoKontrol != 0)
                    {
                        satisFisno = fisnoKontrol;
                        bartxt_FisNo.EditValue = satisFisno;
                    }
                }




                if (siparisPr == true)
                {
                    Rsat_SiparisPr = true;
                }

                if (otomatikSatis)
                {
                    Miktar = otomiktar;// hizmetmiktar;
                }

                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis_Ekle";

                com.Parameters.AddWithValue("@Rsat_Fisno", satisFisno);
                com.Parameters.AddWithValue("@Rsat_Tarih", Param.Tarih);
                com.Parameters.AddWithValue("@Rsat_Departman", Departman.Dep_Kodu);
                com.Parameters.AddWithValue("@Rsat_Recete", Urun_Kodu);
                com.Parameters.AddWithValue("@Rsat_Kdvoran", Rec_Kdv.ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Miktar", (Miktar * Eksileme).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Fiyat", (Rsat_Tutar * Eksileme).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Net", (Rsat_Net * Eksileme).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Kdv", (Rsat_Kdv * Eksileme).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Tutar", (Rsat_Tutar * Eksileme).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Dovizkodu", Param.Doviz_Kodu);
                com.Parameters.AddWithValue("@Rsat_Doviztutar", (Rec_Dovifiyat * Eksileme).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Odano", Rsat_Odano);
                com.Parameters.AddWithValue("@Rsat_Folio", Rsat_Folio);
                com.Parameters.AddWithValue("@Rsat_Adisyon", Rsat_Adisyon);
                com.Parameters.AddWithValue("@Rsat_Masa", masano);
                com.Parameters.AddWithValue("@Rsat_Garson", User.P_Kod);
                com.Parameters.AddWithValue("@Rsat_Kisi", Rsat_Kisi);
                com.Parameters.AddWithValue("@Rsat_Cari", Rsat_Cari);
                com.Parameters.AddWithValue("@Rsat_Split", Split);
                com.Parameters.AddWithValue("@Rsat_Aciklama", (Aciklama + txt_EkNot.Text).TrimEnd());
                com.Parameters.AddWithValue("@Rsat_Paketci", Rsat_Paketci);
                com.Parameters.AddWithValue("@Rsat_Emiktar", eMiktar);
                com.Parameters.AddWithValue("@Rsat_Garson2", Rsat_Garson2);
                com.Parameters.AddWithValue("@Rsat_Uye_Kart_Turu", Rsat_Uye_Kart_Turu);
                com.Parameters.AddWithValue("@Rsat_Pansiyon", Rsat_Pansiyon);
                com.Parameters.AddWithValue("@Rsat_MusTipi", Rsat_MusTipi);
                com.Parameters.AddWithValue("@Rsat_Uye_Id", Rsat_Uye_Id);
                com.Parameters.AddWithValue("@Rsat_Uye_Ad", Rsat_Uye_Ad);
                com.Parameters.AddWithValue("@Rsat_Indkodu", Rsat_Indkodu);
                com.Parameters.AddWithValue("@Rsat_Indoran", Rsat_Indoran.ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Onbdep", Rsat_Onbdep);
                com.Parameters.AddWithValue("@Rsat_Dovizkur", Param.Doviz_Kuru.ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Not", Convert.ToString(txt_Not.Text));
                com.Parameters.AddWithValue("@Rsat_Pda", Convert.ToBoolean(false));
                com.Parameters.AddWithValue("@Rsat_Splitad", Splitad);
                com.Parameters.AddWithValue("@Rsat_SiparisPr", Rsat_SiparisPr);
                com.Parameters.AddWithValue("@Rsat_Yapma", chk_Yapma.Checked);
                com.Parameters.AddWithValue("@Rsat_AbuyerPr", Rsat_AbuyerPr);
                com.Parameters.AddWithValue("@Rsat_AbuyerPr2", Rsat_AbuyerPr);
                com.Parameters.AddWithValue("@Rsat_AbuyerPr3", Rsat_AbuyerPr);
                com.Parameters.AddWithValue("@Rsat_AbuyerPr4", Rsat_AbuyerPr);
                com.Parameters.AddWithValue("@Rsat_Sube", string.IsNullOrEmpty(Sube) ? Departman.Kodlar_PosSubeKod : Sube);
                com.Parameters.AddWithValue("@Rsat_OzelMasaAdi", Ozel_Masa);
                com.Parameters.AddWithValue("@PaketFiyatTipi", PaketFiyat);
                com.Parameters.AddWithValue("@Rsat_Duzeltme", MiktarDuzeltme);
                com.Parameters.AddWithValue("@kisiyeSatisAdSoyad", txtKisiyeSatisSayac.Text + "-" + txtKisiyeSatis.Text);

                if (Departman.Kodlar_AndPos_NFC == true) com.Parameters.AddWithValue("@Rsat_Kart_ID", FolioKart_ID);
                if (Departman.Kodlar_AndPos_NFC == true) com.Parameters.AddWithValue("@Rsat_Kartno", Kart_No);
                if (Departman.Kodlar_Ingenico_IWE == true) com.Parameters.AddWithValue("@Rsat_Ingenico_Status", 1);
                com.Parameters.AddWithValue("@Rsat_UrunTahsilat", Rsat_UrunTahsilat);
                if (Departman.Kodlar_YerliYabanci == true) com.Parameters.AddWithValue("@Rsat_YerliYabanci", YerliYabanci);
                if (Departman.Kodlar_PRSor == true) com.Parameters.AddWithValue("@Rsat_PR", PRKodu);


                com.ExecuteNonQuery();
                con.Close();

                if (malzemeTr)
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Emiktar='" + yedekEmiktar + "' where Rsat_Id=(select top 1 Rsat_Id from Cst_Recete_Satis where Rsat_Fisno='" + satisFisno + "' order by Rsat_Id desc)");
                }


                if (Ozel_Masa != String.Empty) dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 11, @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Masano = '" + Masa_No + "', @Ozel_Masa = '" + Ozel_Masa + "'");


                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Satis, (chk_Fix.Checked == false ? Log.Log_Islem.Kaydet : Log.Log_Islem.FixKaydet), Departman.Dep_Adi + " Urun:" + Urun_Kodu + "-" + Rec_Ad + " Miktar:" + Miktar.ToString() + " Tutar:" + Rsat_Tutar.ToString("N2"), Convert.ToInt32(bartxt_FisNo.EditValue).ToString(), "", Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Recete")), Miktar, "", Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")), recete: Urun_Kodu, urunad: Rec_Ad);



                Miktar_Duzenle();
                chk_Yapma.Checked = false;

                if (yenilemeYapma == false)
                {
                    gridyenile();
                }
                txt_EkNot.Text = "";
                Aciklama = String.Empty;

                barkodFocuslan();
                MiktarDuzeltme = 0;
                //SiparisKontrol();
                Miktar = 1;

                //if (Convert.ToString(this.Tag) ==  "H") barkodFocuslan();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Urun_Sat", "", ex);
            }

        }

        private void Miktar_Duzenle()
        {


            btn_1.ForeColor = Color.DarkRed;
            btn_2.ForeColor = Color.Black;
            btn_3.ForeColor = Color.Black;
            btn_4.ForeColor = Color.Black;
            btn_5.ForeColor = Color.Black;
            btn_6.ForeColor = Color.Black;
            btn_7.ForeColor = Color.Black;
            btn_8.ForeColor = Color.Black;
            btn_9.ForeColor = Color.Black;


        }

        private void Urun_Grup_Yenile()
        {
            flp_UrunGrup.Controls.Clear();
            DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 7, @Anagrup = '" + Ana_Grup + "', @Altgrup = '" + Alt_Grup + "' ");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleButton btn_UrunGrup = new SimpleButton();
                    btn_UrunGrup.Size = new System.Drawing.Size(Convert.ToInt32(dt.Rows[0]["Pkod_AciklamaG"]), Convert.ToInt32(dt.Rows[0]["Pkod_AciklamaY"]));
                    btn_UrunGrup.TabIndex = 0;
                    btn_UrunGrup.TabStop = false;
                    btn_UrunGrup.Font = new System.Drawing.Font("Verdana", 7F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    btn_UrunGrup.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btn_UrunGrup.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_UrunGrup.Appearance.Options.UseBackColor = true;
                    btn_UrunGrup.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                    btn_UrunGrup.Margin = new System.Windows.Forms.Padding(1, 1, 1, 2);

                    btn_UrunGrup.Text = Convert.ToString(dt.Rows[i]["Pkod_Ad"]);
                    btn_UrunGrup.Tag = Convert.ToString(dt.Rows[i]["Pkod_Id"]);

                    btn_UrunGrup.Click += new EventHandler(btn_UrunGrup_Click);

                    flp_UrunGrup.Controls.Add(btn_UrunGrup);
                }
            }
        }

        void btn_UrunGrup_Click(object sender, EventArgs e)
        {
            SimpleButton btn_UrunGrup = (SimpleButton)sender;
            Aciklama += btn_UrunGrup.Text + ";";
        }

        // public bool PaketSube = false;

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            cikisyap();
        }

        public void cikisyap()
        {
            if (mCari != null)
            {
                if (Masa_Paket)
                {
                    if (!String.IsNullOrEmpty(mCari.Cari_Il) && !String.IsNullOrEmpty(mCari.Cari_Ilce) && !String.IsNullOrEmpty(mCari.Cari_Mahalle))
                    {
                        string SubeKod = dbtools.DegerGetir(@"select Adres_Sube from Pos_Adres 
                                            where 
                                            Adres_Sube is not null
                                            and
                                            Adres_UstGrup = '" + mCari.Cari_Il + @"'
                                            and
                                            Adres_AltGrup = '" + mCari.Cari_Ilce + @"'
                                            and
                                            Adres_Kod = '" + mCari.Cari_Mahalle + "'");

                        if (!String.IsNullOrEmpty(SubeKod))
                        {
                            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Sube = '" + SubeKod + "' Where Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue) + "'");
                        }
                    }
                }
            }
            canClose = true;
            this.Close();
        }

        private void Filtre_Click(object sender, EventArgs e)
        {
            SimpleButton btn_Filtre = (SimpleButton)sender;
            txt_Filtre.EditValue = btn_Filtre.Text; //Convert.ToString(txt_Filtre.EditValue); //+
        }

        private void btn_Sil_Click(object sender, EventArgs e)
        {
            if (txt_Filtre.EditValue.ToString().Length > 0)
            {
                txt_Filtre.EditValue = txt_Filtre.EditValue.ToString().Substring(0, txt_Filtre.EditValue.ToString().Length - 1);
            }
        }

        private void txt_Filtre_EditValueChanged(object sender, EventArgs e)
        {
            if (txt_Filtre.Text != "")
            {
                Ana_Grup = "";
                Alt_Grup = "";
                Urun_Yenile();
            }
        }

        private void btn_Miktar_Click(object sender, EventArgs e)
        {
            btn_1.ForeColor = Color.Black;
            btn_2.ForeColor = Color.Black;
            btn_3.ForeColor = Color.Black;
            btn_4.ForeColor = Color.Black;
            btn_5.ForeColor = Color.Black;
            btn_6.ForeColor = Color.Black;
            btn_7.ForeColor = Color.Black;
            btn_8.ForeColor = Color.Black;
            btn_9.ForeColor = Color.Black;
            btn_FarkliMiktar.ForeColor = Color.Black;

            SimpleButton btn_Miktar = (SimpleButton)sender;
            Miktar = Convert.ToInt32(btn_Miktar.Text);
            btn_Miktar.ForeColor = Color.DarkRed;
        }

        private void btn_FarkliMiktar_Click(object sender, EventArgs e)
        {
            btn_1.ForeColor = Color.Black;
            btn_2.ForeColor = Color.Black;
            btn_3.ForeColor = Color.Black;
            btn_4.ForeColor = Color.Black;
            btn_5.ForeColor = Color.Black;
            btn_6.ForeColor = Color.Black;
            btn_7.ForeColor = Color.Black;
            btn_8.ForeColor = Color.Black;
            btn_9.ForeColor = Color.Black;
            btn_FarkliMiktar.ForeColor = Color.DarkRed;

            Klavye1 klavye = new Klavye1();
            klavye.Tag = "FARKLIMIKTAR";
            klavye.ShowDialog();
            Miktar = klavye.sayi;
        }

        private void btn_MiktarDuzelt_Click(object sender, EventArgs e)
        {
            decimal deger = Convert.ToDecimal(gridColumn4.SummaryItem.SummaryValue);

            //if (deger == 0)
            //{
            //    RHMesaj.MyMessageInformation("Miktar düzeltilemez\nçünkü genel toplam eksiye düşer !");
            //    return;
            //}


            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "A")
            {
                MessageBox.Show(res_man.GetString("Alacak Satırı üzerinde İşlem Yapamazsınız..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            decimal eskiMiktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));

            Klavye1 klavye = new Klavye1();
            klavye.Tag = "MIKTARDUZELT";
            klavye.UrunAdi = Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad2"));
            klavye.txt_Sayi.EditValue = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar")).ToString("n3");
            Rec_Miktar_Gr = Convert.ToBoolean(gridView1.GetFocusedRowCellValue("Rec_Miktar_Gr"));
            if (Rec_Miktar_Gr == false)
            {
                klavye.ShowDialog();

                if (klavye.sayi != 0)
                {
                    Miktar = Convert.ToDecimal(klavye.sayi) - Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
                }

                if (klavye.iptal == true)
                {
                    Miktar = 1;
                    return;
                }
            }
            if (Rec_Miktar_Gr)
            {
                klavye.ShowDialog();
                if (klavye.sayi != 0)
                {
                    Miktar = Convert.ToDecimal(klavye.sayi); //- Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"))*1000;
                    Miktar = Miktar / 1000;
                }

                if (klavye.iptal == true)
                {
                    Miktar = 1;
                    return;
                }

                MiktarDuzeltme = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id"));
            }


            if (Miktar != 0)
            {
                Urun_Sat(Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Recete")), urunduzeltme: true);


                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Miktar_Duzelt, Log.Log_Islem.Duzelt, Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Miktarı " + eskiMiktar.ToString().Replace(",0000", "") + " iken " + Convert.ToDecimal(klavye.sayi).ToString() + " ile Değişti", Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")));
            }
        }

        int MiktarDuzeltme = 0;

        string Merkez_Sube = String.Empty;
        string Merkez_Sube_Kod = String.Empty;
        private void btn_Siparis_Click(object sender, EventArgs e)
        {
            int fisno = Convert.ToInt32(bartxt_FisNo.EditValue);

            siparisYazdir();


            // 21.02.2025 tarihinde hızlı satışta QR Hesap dökmek için eklendi, QR'la ilintili olarak masa takipde qr okutulunca verilen masa bilgisi bulunamadı
            //parametrede HesapFisQR'a bakacak şekilde false ye çekildi.
            if (Param.hesapFisQrFisno && Convert.ToString(this.Tag) == "D")
            {
                FisPr pr = new FisPr();
                if (Param.Param_YeniHesapDkm)
                {
                    pr.newHesapDokum(true, fisno, Split, "* * * HESAP DÖKÜM FİŞİ * * *");
                }

                Bilgileri_Doldur();

            }

        }

        public void siparisYazdir()
        {
            try
            {


                DialogResult c = System.Windows.Forms.DialogResult.Yes;

                if (Param.Param_Siparis_Uyari)
                {
                    c = MessageBox.Show(res_man.GetString("Sipariş Yazıcıya Gönderilsin Mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }


                // sonradan eklendi
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Not = '" + txt_Not.Text + "' where Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue) + "'");

                if (Masa_Paket && AcikAdres)
                {
                    CariHesap cari = new CariHesap();
                    cari.xtraTabControl1.SelectedTabPageIndex = 1;
                    cari.Tel = CariTel;
                    cari.AcikAdres = AcikAdres;
                    cari.ShowDialog();

                    // 22.10.2024 oguzhan istedi
                    if (cari.CariKod == "")
                    {
                        return;
                    }
                    mCari = Cari.Cari_Getir(cari.CariKod);

                    string carikod = cari.CariKod;
                    if (carikod != "")
                    {
                        dbtools.execcmd("update Cst_Recete_Satis set Rsat_Cari = '" + carikod + "' Where Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue) + "'");
                    }
                }
                string SubeKod = string.Empty;
                if (Masa_Paket)
                {
                    if (mCari != null)
                    {
                        if (!String.IsNullOrEmpty(mCari.Cari_Il) && !String.IsNullOrEmpty(mCari.Cari_Ilce) && !String.IsNullOrEmpty(mCari.Cari_Mahalle))
                        {
                            SubeKod = dbtools.DegerGetir(@"select Adres_Sube from Pos_Adres 
                                            where 
                                            Adres_Sube is not null
                                            and
                                            Adres_UstGrup = '" + mCari.Cari_Il + @"'
                                            and
                                            Adres_AltGrup = '" + mCari.Cari_Ilce + @"'
                                            and
                                            Adres_Kod = '" + mCari.Cari_Mahalle + "'");

                            if (!String.IsNullOrEmpty(SubeKod))
                            {
                                Merkez_Sube = dbtools.DegerGetir("select Pkod_MerkezSube from Pos_Kodlar Where Pkod_Sinif = '27' and Pkod_Kod = '" + SubeKod + "'");
                            }
                        }
                    }
                }

                if (Masa_Paket)
                {
                    PaketNot not = new PaketNot();
                    not.Fisno = Convert.ToInt32(bartxt_FisNo.EditValue);
                    not.ShowDialog();
                }


                if (Merkez_Sube == "S" && SubeKod != Departman.Kodlar_PosSubeKod)
                {
                    PaketCallCenter p = new PaketCallCenter();
                    p.SubeyeGonder(SubeKod, Convert.ToInt32(bartxt_FisNo.EditValue).ToString());
                    p.Odeme_Al(SubeKod, Convert.ToInt32(bartxt_FisNo.EditValue), Convert.ToDecimal(gridColumn4.SummaryItem.SummaryValue), Convert.ToString(bartxt_MasaNo.EditValue));
                }


                if (c == System.Windows.Forms.DialogResult.Yes && Merkez_Sube != "S")
                {
                    Siparis_Gonder(false);

                    //dbtools.execcmdR("update Pos_Log set Log_Yazdirilmis='E'  where Log_FisNo='" + bartxt_FisNo.EditValue.ToString() + "' and Log_Bolum<>'Satir_Sil' and Log_Aciklama NOT LIKE '%Yazdırılmamış%'");

                    string q = "update Pos_Log set Log_Yazdirilmis='E' where Log_Recete in (select Rsat_Recete  from Cst_Recete_Satis where Rsat_Fisno='" + bartxt_FisNo.EditValue.ToString() + "' and Rsat_Recete is not null and Rsat_Recete<>'' ) and Log_FisNo='" + bartxt_FisNo.EditValue.ToString() + "' and Log_Bolum<>'Satir_Sil' and Log_Aciklama NOT LIKE '%Yazdırılmamış%'";


                    dbtools.execcmdR(q);

                    if (Departman.Kodlar_AndPos_NFC == true)
                    {
                        HesapBul();
                    }

                }

                if (Merkez_Sube == String.Empty || Merkez_Sube == "M")
                {
                    if (Masa_Paket)
                    {

                        if (kisiyeSatisAktifmi == "True")
                        {

                        }
                        else
                        {
                            FisPr pr = new FisPr();
                            pr.PaketPr(Convert.ToInt32(bartxt_FisNo.EditValue), " * * * PAKET FİSİ * * * ", parcaliAdres: adres);
                        }

                    }
                }


                if (Convert.ToString(this.Tag) == "M" || Convert.ToString(this.Tag) == "P")
                {
                    if (kisiyeSatisAktifmi == "0" || kisiyeSatisAktifmi.ToLower() == "false")
                    {
                        btn_Cikis_Click(null, null);
                    }
                    else
                    {
                        kisiyiTemizle();

                    }

                }




            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA " + ex.Message);
            }
        }


        string DovizKodu = "";
        string OdenmezKodu = "";
        private void HesapBul()
        {

            OdemeTutar = 0;
            string KapatmaKodu = Departman.Kodlar_NFCKapatma;
            //KartID = Convert.ToInt32(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 30,@Masano = '" + Masa_No + "', @Dep_Kodu = '" + Departman.Dep_Kodu + "'"));
            //KartNo = Convert.ToString(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 29,@Masano = '" + Masa_No + "', @Dep_Kodu = '" + Departman.Dep_Kodu + "'"));
            DovizKodu = Convert.ToString(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 31,@Masano = '" + Masa_No + "', @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Fisno = '" + bartxt_FisNo.EditValue + "'"));
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
                                    where CardF_R_I_H = 'I' and ID = '" + FolioKart_ID + "'");


            if (dtHesap.Rows.Count > 0)
            {
                Oda_No = Convert.ToString(dtHesap.Rows[0]["Rez_Odano"]);
                Folio = Convert.ToInt32(dtHesap.Rows[0]["Rez_Id"]);
                Pansiyon = Convert.ToString(dtHesap.Rows[0]["Rez_Konaklama"]);
                Odeme_Kodu = Convert.ToString(dtHesap.Rows[0]["Rez_Odeme"]);
                Master_Folio = Convert.ToInt32(dtHesap.Rows[0]["Rez_Master_id"]);
                Kart_No = Convert.ToString(dtHesap.Rows[0]["Rez_Kartno"]);
                FolioKart_ID = Convert.ToString(dtHesap.Rows[0]["ID"]);
                Uye_Adsoyad = Convert.ToString(dtHesap.Rows[0]["Rez_Adi_1"]);
                Uye_Kartturu = Convert.ToString(dtHesap.Rows[0]["Rez_Adi_2"]);

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


            Fis_Islem.Odeme_Al(Convert.ToInt32(bartxt_FisNo.EditValue), OdemeTutar, OdemeTutar, Convert.ToString(KapatmaKodu), musTipi_A, odaNo_A, folio_A, cari_A, Split, DovizKodu, false);
            Fis_Islem.Satis_Tip(Convert.ToInt32(bartxt_FisNo.EditValue), Convert.ToString(KapatmaKodu), pansiyon_A);

            if (Param.Tesis_Tipi == 0)
            {
                if (Param.Tarih != Convert.ToDateTime(Fronttools.DegerGetir("select Fis_Curtar from Fishrk where Fis_anah = 1")))
                {
                    MessageBox.Show(res_man.GetString("Önbüro Çalışma Tarihi ile Sistem tarihi Farklı.Sistemden Çıkıp Yeniden Giriniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            Fis_Islem.Onburo_At(Convert.ToInt32(Fisno), Kart_No, FolioKart_ID == "" ? 0 : Convert.ToInt32(FolioKart_ID));



            //dbtools.execcmd("update Pos_Masa set Masa_Durum = '0', Masa_Ozel = '' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
            //dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 16,@Masano = '" + Masa_No + "',@Dep_Kodu = '" + Departman.Dep_Kodu + "'");

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, "Fiş Kapatma. Fisno:" + Convert.ToInt32(bartxt_FisNo.EditValue) + " Masano:" + Masa_No.ToString(), Convert.ToString(this.Tag), "");

            FisPr fis = new FisPr();
            string cevap = fis.HesapDokum(false, Convert.ToInt32(bartxt_FisNo.EditValue), Split);
            if (cevap != "OK")
            {
                MessageBox.Show(cevap);
                //temizle();
                this.Close();
                return;
            }


            //MessageBox.Show(res_man.GetString("Hesap Kapatıldı..");
            //this.Close();




        }

        decimal OdemeTutar = 0;
        private decimal Bakiye_bul_TL()
        {
            decimal bakiye = 0;
            DataTable dtBakiye = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 21,@Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue) + "', @Split = '" + Split + "'");

            bakiye = Convert.ToDecimal(dtBakiye.Rows[0]["TLBakiye"]);

            bakiye = Math.Abs(bakiye) < Convert.ToDecimal(0.03) ? 0 : bakiye;
            return bakiye;
        }
        string musTipi_A = String.Empty;
        string odaNo_A = String.Empty;
        int folio_A = 0;
        int masterFolio_A = 0;
        string pansiyon_A = String.Empty;
        int uyeId_A = 0;
        string uyeAdsoyad_A = String.Empty;
        string uyeKartturu_A = String.Empty;
        string cari_A = String.Empty;
        //int Split = 0;

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
        private void Siparis_Gonder(bool Mars)
        {
            FisPr pr = new FisPr();
            //
            string sonuc = "";


            string garson = ""; // bu istek 29.06.2022 ramazan üzmez yaptı
            if (Departman.Garson_Sor)
            {
                garson = bartxt_Garson.EditValue.ToString();
            }


            if (Param.Param_YeniSiparisDkm)
            {
                if (Param.tumPrinter) // 23.08.2024 emre bey istediği için eklendi. mantık reçete 1 den fazla ise tüm fişten çıksın
                {
                    sonuc = pr.newSiparisPr_Tumsiparis(Convert.ToInt32(bartxt_FisNo.EditValue), Mars, Split, garsonsor: garson, kisiyeSatis: txtKisiyeSatis.Text);

                }
                else
                {
                    if (this.Tag == null)
                    {
                        this.Tag = "D";
                    }
                    bool direksatismi = this.Tag.ToString() == "D" ? true : false;

                    sonuc = pr.newSiparisPr(Convert.ToInt32(bartxt_FisNo.EditValue), Mars, Split, garsonsor: garson, kisiyeSatis: txtKisiyeSatis.Text, direkSatis: direksatismi);
                }
            }
            else
            {
                sonuc = pr.SiparisPr(Convert.ToInt32(bartxt_FisNo.EditValue), Mars, Split, garsonsor: garson);
            }

            if (sonuc != "OK")
            {
                MessageBox.Show(sonuc);
            }
            else
            {


                dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr = 1 where Rsat_Fisno = '" + bartxt_FisNo.EditValue.ToString() + "' ");

                RHMesaj.alertMesaj2("Sipariş Yazdırıldı", 5);
            }

            btn_Cikis.Enabled = true;
        }

        private void btn_Indirim_Click(object sender, EventArgs e)
        {
            decimal deger = Convert.ToDecimal(gridColumn4.SummaryItem.SummaryValue);

            //if (deger == 0)
            //{
            //    RHMesaj.MyMessageInformation("İndirim düzeltilemez\nçünkü genel toplam eksiye düşer !");
            //    return;
            //}



            Indirim ind = new Indirim();
            ind.Tag = "I";
            ind.tutar = Convert.ToDecimal(gridColumn4.SummaryItem.SummaryValue);
            ind.ShowDialog();

            if (ind.degergirildi == false)
            {
                return;
            }

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

            if (ind.indTipi == "MY")
            {
                ind.indTipi = "Y";

            }
            if (ind.indTipi == "Y" )
            {
                oran = ind.indSayi;
            }

            if (oran > 0 || tutar > 0)
            {
                int fisno = Convert.ToInt32(bartxt_FisNo.EditValue);
                dbtools.execcmdR($"delete from Cst_Recete_Satis where Rsat_Fisno='{fisno}' and Rsat_Indkodu='MANUEL'");
                Fis_Islem.Manuel_Indirim(fisno, ind.indTipi, tutar, doviztutar, oran, Split);

                Fis_Islem.ServisPayi(Convert.ToInt32(bartxt_FisNo.EditValue));
                gridyenile();
            }
        }




        public static string MyClass = "Satis";

        private void btn_SatirSil_Click(object sender, EventArgs e)
        {
            try
            {

                string fisno = bartxt_FisNo.EditValue.ToString();

                if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) == String.Empty)
                {
                    return;
                }


                //string qq = $@"select count(*) as kapatmaVarmi from Cst_Recete_Satis where Rsat_Fisno={fisno} and Rsat_Kapatma is not null and Rsat_Kapatma<>''";
                //string kapatmaVarmi = dbtools.DegerGetir(qq);
                //if (kapatmaVarmi != "0")
                //{
                //    MessageBox.Show("Ödeme veya İndirim varken ürün silinemez !");
                //    return;
                //}

                string receteKod = gridView1.GetFocusedRowCellValue("Rsat_Recete").ToString();
                string bindirimReceteKod = dbtools.DegerGetir("select top 1 Param_Bindirim  from Pos_Param where Param_Id = '1'");
                string kullaniciTuru = dbtools.DegerGetir("select P_Kulturu from Rmosmuh.dbo.Pos_User where P_Kod='2'"); // 1 ise garsondur

                if (kullaniciTuru == "1" && receteKod == bindirimReceteKod)
                {
                    RHMesaj.alertMesaj2("Kullanıcı Türü Garson. Servis Payı Silemez ! ", 2);
                    return;
                }

                if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "A")
                {
                    MessageBox.Show(res_man.GetString("Ödemeler veya İndirimler Silinemez.."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                decimal deger = Convert.ToDecimal(gridColumn4.SummaryItem.SummaryValue);

                //if (deger == 0)
                //{
                //    RHMesaj.MyMessageInformation("Ödeme silinmeden satır silinemez\nçünkü genel toplam eksiye düşer !");
                //    return;
                //}

                //if (MessageBox.Show(res_man.GetString("Seçili Satırı Silmek İstediğinize Emin Misiniz...?", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                //{
                //    return;
                //}

                //try
                //{
                bool Rsat_SiparisPr = Convert.ToBoolean(dbtools.DegerGetir("select ISNULL((select ISNULL(Rsat_SiparisPr,0) from Cst_Recete_Satis WHERE Rsat_Id = " + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "),0)"));
                //}
                //catch
                //{
                //MessageBox.Show(res_man.GetString("Gruba Ait Printer Parametreleri Tanımlı Değil.",res_man.GetString("Uyarı"),MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return;
                //}


                if (Rsat_SiparisPr && !User.G_Satirsil_Y)
                {
                    MessageBox.Show(res_man.GetString("Yazdırılmış Satır Silme Yetkiniz Yoktur...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (Param.Param_SatirSil == true && Param.Param_SatirSilUser == "999")
                {
                    frmLogin a = new frmLogin();
                    a.Durum = "Satir";
                    a.ShowDialog();

                    if (a.DonenDeger == false)
                    {
                        return;
                    }
                }


                decimal Sil_Miktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
                if (Sil_Miktar > 1 && Convert.ToInt32(dbtools.DegerGetir("SELECT COUNT(*) FROM Cst_Recete_Satis WITH(NOLOCK) WHERE Rsat_Id = " + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + " AND ISNULL(Rsat_AdisyonPr,0) = 0")) > 0)
                {
                    Klavye1 klv = new Klavye1();
                    klv.txt_Sayi.Text = Sil_Miktar.ToString();
                    klv.Tag = "SATIRSIL";
                    klv.UrunAdi = Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad2"));
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

                int satirId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id"));
                indirimvarsaazalt(satirId, fisno, (int)Sil_Miktar);


                string neden = "";
                if (Rsat_SiparisPr && Departman.Kodlar_YazSipNedSor)
                {

                    DataTable dataTable = dbtools.SelectTableR("select Pkod_Ad from Pos_Kodlar where Pkod_Sinif='23' and Pkod_Kod like 'SS%' -- ürün iptal");

                    Klavye2 klv = new Klavye2(data: dataTable);
                    klv.ShowDialog();

                    if (klv.yazi == null)
                    {
                        MessageBox.Show(res_man.GetString("Hatalı Giriş..."));
                        return;
                    }

                    if (klv.yazi.Length == 0)
                    {
                        return;
                    }

                    neden = klv.yazi;
                }

                string yazdirilmissa = "Yazdırılmamış";
                if (Departman.Siparis && Param.satirsilfiscikmasinaktif == false)
                {
                    FisPr fis = new FisPr();
                    string sonuc = fis.newIptalPr(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), Sil_Miktar);


                    if (fis.yazdirilmismi)
                    {
                        yazdirilmissa = "Yazdırılmış";
                    }
                    if (sonuc != "OK")
                    {
                        MessageBox.Show(sonuc);
                    }
                }

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Satir_Sil, Log.Log_Islem.Sil, yazdirilmissa + " Sipariş-> " + "Recete : " + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Miktar : " + Sil_Miktar + " Silindi", Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")), Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")), Sil_Miktar, neden, Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")));



                Fis_Islem.Satir_Sil(satirId, Sil_Miktar);


                int satirsay = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + bartxt_FisNo.EditValue.ToString() + "' and Rsat_Ba = 'B' "));
                if (satirsay == 0)
                {
                    dbtools.execcmd("update Pos_Masa set Masa_Durum = 0, Masa_Ozel = '' where Masa_No = '" + Convert.ToString(bartxt_MasaNo.EditValue) + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                    dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Fisno = '" + bartxt_FisNo.EditValue.ToString() + "'");
                }


                gridyenile();

                if (kisiyeSatisAktifmi == "1" || kisiyeSatisAktifmi == "True")
                {
                    txtKisiyeSatis.Focus();
                    txtKisiyeSatis.Select();
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btn_SatirSil_Click", "", ex);
            }

        }

        public void indirimvarsaazalt(int satirId, string fisno, int Sil_Miktar)
        {
            try
            {
                string q1 = @"select Rsat_Id,Rsat_Ba,isnull(Rsat_Miktar,0) as Rsat_Miktar,Rsat_Fiyat,Rsat_Tutar,Rsat_Doviztutar,Rsat_Net,Rsat_Kdv from Cst_Recete_Satis 
where 
Rsat_Fisno='" + fisno + @"'  
and Rsat_Indkodu  in ('HAPPYHOUR','MANUEL') and Rsat_Ba='A'";
                DataTable indirimTable = dbtools.SelectTableR(q1);


                string q3 = @"select sum(Rsat_Fiyat) as Rsat_Fiyat,sum(Rsat_Tutar) as Rsat_Tutar,sum(Rsat_Doviztutar) as Rsat_Doviztutar from Cst_Recete_Satis 
where 
Rsat_Fisno='" + fisno + @"'  
and Rsat_Indkodu  in ('HAPPYHOUR','MANUEL') and Rsat_Ba='A'
group by Rsat_Fiyat,Rsat_Tutar,Rsat_Doviztutar";
                DataTable indirimTable2 = dbtools.SelectTable(q3);

                if (indirimTable != null && indirimTable.Rows.Count > 0)
                {
                    int Rsat_Miktar = Convert.ToInt32(Convert.ToDecimal(indirimTable.Rows[0]["Rsat_Miktar"].ToString()));
                    if (Rsat_Miktar == 0)
                    {
                        return;
                    }
                    int Rsat_Id = Convert.ToInt32(indirimTable.Rows[0]["Rsat_Id"].ToString());
                    decimal Rsat_Fiyat = Convert.ToDecimal(indirimTable2.Rows[0]["Rsat_Fiyat"].ToString());
                    decimal Rsat_Tutar = Convert.ToDecimal(indirimTable2.Rows[0]["Rsat_Tutar"].ToString());
                    decimal Rsat_Doviztutar = Convert.ToDecimal(indirimTable2.Rows[0]["Rsat_Doviztutar"].ToString());

                    string qq = @"select 
(select top 1 CASE 
        WHEN Rec_HappyHourFiyat IS NULL OR Rec_HappyHourFiyat = 0 THEN Rec_Fiyat 
        ELSE Rec_HappyHourFiyat 
    END AS Rec_HappyHourFiyat from Cst_Recete where Rec_Genelkod=satis.Rsat_Recete and Rec_HappyHour='1')*" + Sil_Miktar + @"
as uygulanacakindirim
from Cst_Recete_Satis as satis where Rsat_Id='" + satirId + @"'";
                    string deger = dbtools.DegerGetir(qq);

                    if (deger != null && deger != "" && deger != "0")
                    {
                        decimal Rsat_Kdvoran = Convert.ToDecimal(dbtools.DegerGetir("select top 1 Rsat_Kdvoran from Cst_Recete_Satis where Rsat_Id='" + satirId + "'"));

                        var oranAktifmi = dbtools.DegerGetir("select Pkod_Hh_Oran from Pos_Kodlar where Pkod_Sinif='20' and Pkod_Hh_Oran>0");

                        decimal uygulanacakindirim = Convert.ToDecimal(deger);

                        if (oranAktifmi != "" && oranAktifmi != "0")
                        {
                            uygulanacakindirim = uygulanacakindirim * (Convert.ToDecimal(oranAktifmi) / 100);
                        }


                        decimal fiyat = Rsat_Fiyat - uygulanacakindirim;
                        decimal tutar = Rsat_Tutar - uygulanacakindirim;
                        decimal doviztutar = Rsat_Doviztutar - uygulanacakindirim;
                        decimal net = tutar / ((100 + Rsat_Kdvoran) / 100);
                        decimal kdv = tutar - tutar / ((100 + Rsat_Kdvoran) / 100);
                        decimal miktar = Rsat_Miktar - Sil_Miktar;

                        string q2 = "update Cst_Recete_Satis set Rsat_Miktar='" + miktar + "',Rsat_Fiyat='" + fiyat.ToString().Replace(",", ".") + "',Rsat_Tutar='" + tutar.ToString().Replace(",", ".") + "',Rsat_Doviztutar='" + doviztutar.ToString().Replace(",", ".") + "',Rsat_Net='" + net.ToString().Replace(",", ".") + "',Rsat_Kdv='" + kdv.ToString().Replace(",", ".") + "' where Rsat_Id='" + Rsat_Id + "'";

                        dbtools.execcmdR(q2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("önemsiz hata_1\n" + ex.Message);
            }
        }

        private void btn_Tutarduzelt_Click(object sender, EventArgs e)
        {
            tutarDuzelt();
        }

        public void tutarDuzelt()
        {
            try
            {
                decimal deger = Convert.ToDecimal(gridColumn4.SummaryItem.SummaryValue);

                //if (deger == 0)
                //{
                //    RHMesaj.MyMessageInformation("Tutar düzeltilemez\nçünkü genel toplam eksiye düşer !");
                //    return;
                //}


                Klavye1 klv = new Klavye1();
                klv.UrunAdi = Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad2"));
                klv.Tag = "TUTARDUZELT";
                string eskitutar;

                if (Param.Calisma_Sekli == 1)   //Dövizli
                {
                    eskitutar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Doviztutar")).ToString();
                }
                else
                {
                    eskitutar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")).ToString();
                }
                klv.txt_Sayi.Text = eskitutar;
                klv.ShowDialog();
                decimal tutar = klv.sayi;
                if (klv.iptal)
                {
                    return;
                }

                //decimal eskitutarim = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar"));


                string recgenelkod = gridView1.GetFocusedRowCellValue("Rsat_Recete").ToString();


                decimal rectutar = Convert.ToDecimal(dbtools.DegerGetir($"select top 1 Rec_Fiyat from cst_recete where Rec_Genelkod='{recgenelkod}'"));

               int tur = Convert.ToInt32( dbtools.DegerGetir($"Select ISNULL(Kodlar_KategoriFiyatTur,0) as tur From Stok_Kodlar where Kodlar_Sinif = 1 and Kodlar_Kod = '{Departman.Dep_Kodu}'"));

                switch (tur)
                {
                    case 0:
                        break;
                    case 1:
                        rectutar = Convert.ToDecimal(dbtools.DegerGetir($"select top 1 isnull(Rec_KategoriFiyat1,Rec_Fiyat) as Rec_Fiyat from cst_recete where Rec_Genelkod='{recgenelkod}'"));
                        break;
                    case 2:
                        rectutar = Convert.ToDecimal(dbtools.DegerGetir($"select top 1 isnull(Rec_KategoriFiyat2,Rec_Fiyat) as Rec_Fiyat from cst_recete where Rec_Genelkod='{recgenelkod}'"));
                        break;
                    case 3:
                        rectutar = Convert.ToDecimal(dbtools.DegerGetir($"select top 1 isnull(Rec_KategoriFiyat3,Rec_Fiyat) as Rec_Fiyat from cst_recete where Rec_Genelkod='{recgenelkod}'"));
                        break;
                    case 4:
                        rectutar = Convert.ToDecimal(dbtools.DegerGetir($"select top 1 isnull(Rec_KategoriFiyat4,Rec_Fiyat) as Rec_Fiyat from cst_recete where Rec_Genelkod='{recgenelkod}'"));
                        break;
                    default:
                        break;
                }

                //if (eskitutarim != rectutar)

                if (tutarduzeltplus && rectutar > tutar)
                {
                    MessageBox.Show("Reçete tutarından düşük tutar girme yetkiniz yoktur !");
                    return;
                }

                Fis_Islem.Tutar_Duzelt(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), tutar);
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Tutar_Duzelt, Log.Log_Islem.Duzelt, Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + "'nın Fiyatı " + eskitutar + " iken " + tutar.ToString() + " ile Değişti", Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")));
                Fis_Islem.ServisPayi(Convert.ToInt32(bartxt_FisNo.EditValue));
                gridyenile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txt_EkNot_Click(object sender, EventArgs e)
        {
            Klavye2 klv = new Klavye2();
            klv.ShowDialog();
            txt_EkNot.Text = klv.yazi;
        }

        private void txt_Barkod_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Barkod_Oku();
        }

        private void txt_Barkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Barkod_Oku();
                txt_Barkod.Focus();

                if (Param.urunAdinaOdaklan)
                {
                    txt_Filtre.Focus();
                    txt_Filtre.Select();
                }
            }
        }

        private void Barkod_Oku()
        {
            try
            {

                string barkod = txt_Barkod.Text;
                txt_Barkod.Text = String.Empty;
                if (barkod.Length > 0)
                {
                    if (Convert.ToString(this.Tag) == "H")
                    {
                        if (Departman.Kodlar_RecBarSis == 0)
                        {
                            DataTable dtR = dbtools.SelectTable("select Rec_Genelkod from Cst_Recete WITH(NOLOCK)  where Rec_Barkod = '" + barkod + "'");
                            if (dtR.Rows.Count < 1)
                            {

                                if (File.Exists("urunyok.wav"))
                                {
                                    SoundPlayer player = new SoundPlayer("urunyok.wav");
                                    player.Play();
                                }

                                MessageBox.Show(res_man.GetString("Ürün Kodu Bulunamadı..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);





                                barkodFocuslan();
                                return;
                            }

                            //Miktar = 1;
                            Urun_Sat(Convert.ToString(dtR.Rows[0]["Rec_Genelkod"]));

                            barkodFocuslan();

                            return;
                        }
                        else
                        {
                            DataTable dtR = dbtools.SelectTable("select top(1) Recete_Kod from Cst_Recete_Barkod WITH(NOLOCK)  where Barkod = '" + barkod + "'");
                            if (dtR.Rows.Count < 1)
                            {

                                if (File.Exists("urunyok.wav"))
                                {
                                    SoundPlayer player = new SoundPlayer("urunyok.wav");
                                    player.Play();
                                }

                                MessageBox.Show(res_man.GetString("Ürün Kodu Bulunamadı..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);



                                barkodFocuslan();
                                return;
                            }

                            //Miktar = 1;
                            Urun_Sat(Convert.ToString(dtR.Rows[0]["Recete_Kod"]));

                            barkodFocuslan();
                            return;
                        }
                    }

                    if (Param.Param_KGAlgilama == false)
                    {
                        if (barkod.Substring(0, 2) == "27")
                        {
                            string urun_Kodu = barkod.Substring(Param.Barkod_Recbas, Param.Barkod_Rechane);
                            int KG = Convert.ToInt32(barkod.Substring(Param.Barkod_KGbas, Param.Barkod_KGhane));
                            int GR = Convert.ToInt32(barkod.Substring(Param.Barkod_GRbas, Param.Barkod_GRhane));

                            decimal urun_Miktar = KG + (Convert.ToDecimal(GR) / Convert.ToDecimal(Math.Pow(10, Param.Barkod_GRhane)));

                            int sayac = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete WITH(NOLOCK) where Rec_Genelkod = '" + urun_Kodu + "'"));
                            if (sayac < 1)
                            {

                                if (File.Exists("urunyok.wav"))
                                {
                                    SoundPlayer player = new SoundPlayer("urunyok.wav");
                                    player.Play();
                                }

                                MessageBox.Show(res_man.GetString("Ürün Kodu Bulunamadı..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);



                                barkodFocuslan();
                                return;
                            }

                            //if (urun_Miktar > Param.Max_Miktar)
                            //{
                            //    if (MessageBox.Show(res_man.GetString("Maximum Miktarı Geçtiniz...") + "\n" + res_man.GetString("Devam Etmek İstiyor Musunuz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                            //    {
                            //        return;
                            //    }
                            //}

                            Miktar = urun_Miktar;
                            Urun_Sat(urun_Kodu);
                            barkodFocuslan();
                        }
                        else
                        {
                            string urun_Kodu = barkod.Substring(Param.Barkod_Recbas, Param.Barkod_Rechane);
                            string Barkod = barkod;
                            //int KG = Convert.ToInt32(barkod.Substring(Param.Barkod_KGbas, Param.Barkod_KGhane));
                            //int GR = Convert.ToInt32(barkod.Substring(Param.Barkod_GRbas, Param.Barkod_GRhane));

                            //decimal urun_Miktar = 1; 

                            string query = "select Rec_Genelkod from Cst_Recete WITH(NOLOCK)  where (Rec_Barkod = '" + barkod + "' or Rec_Genelkod = '" + urun_Kodu + "')";
                            DataTable dtR = dbtools.SelectTable(query);
                            if (dtR.Rows.Count < 1)
                            {

                                if (File.Exists("urunyok.wav"))
                                {
                                    SoundPlayer player = new SoundPlayer("urunyok.wav");
                                    player.Play();
                                }


                                MessageBox.Show(res_man.GetString("Ürün Kodu Bulunamadı..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);



                                barkodFocuslan();
                                return;
                            }


                            //Miktar = urun_Miktar;
                            Urun_Sat(Convert.ToString(dtR.Rows[0]["Rec_Genelkod"]));
                            barkodFocuslan();

                        }


                    }
                    else
                    {

                        DataTable dtR = dbtools.SelectTable("select Rec_Genelkod from Cst_Recete WITH(NOLOCK)  where Rec_Barkod = '" + barkod + "'");
                        if (dtR.Rows.Count < 1)
                        {

                            if (File.Exists("urunyok.wav"))
                            {
                                SoundPlayer player = new SoundPlayer("urunyok.wav");
                                player.Play();
                            }


                            MessageBox.Show(res_man.GetString("Ürün Kodu Bulunamadı..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);


                            barkodFocuslan();
                            return;
                        }

                        //Miktar = 1;
                        Urun_Sat(Convert.ToString(dtR.Rows[0]["Rec_Genelkod"]));

                        barkodFocuslan();



                        return;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }


        }

        private void btn_Arti_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "A")
                {
                    MessageBox.Show(res_man.GetString("Alacak Satırı üzerinde İşlem Yapamazsınız..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                decimal eskiMiktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
                Miktar = 1;

                Urun_Sat(Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Recete")));

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Miktar_Duzelt, Log.Log_Islem.Duzelt, Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Miktarı " + eskiMiktar.ToString() + " iken Miktarı 1 Arttı.", Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")));
            }
        }

        private void btn_Eksi_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "A")
                {
                    MessageBox.Show(res_man.GetString("Alacak Satırı üzerinde İşlem Yapamazsınız...."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal eskiMiktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
                if (eskiMiktar == 1)
                {
                    MessageBox.Show(res_man.GetString("Son Ürün Eksiltilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }



                iptalFisCikar(kodlarkodadisyonaktifmi);

                Miktar = -1;

                Urun_Sat(Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Recete")));

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Miktar_Duzelt, Log.Log_Islem.Duzelt, Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Miktarı " + eskiMiktar.ToString() + " iken Miktarı 1 Azalttı.", Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")));




                if (kodlarkodadisyonaktifmi == "True")
                {

                    btn_Cikis.Enabled = true;
                    btn_Siparis.Enabled = false;
                }


            }
        }

        public void iptalFisCikar(string kodlarkodadisyonaktifmi)
        {
            string yazdirilmissa = "Yazdırılmamış";

            if (kodlarkodadisyonaktifmi == "True" && Departman.Siparis && Param.satirsilfiscikmasinaktif == false)
            {
                FisPr fis = new FisPr();
                string sonuc = fis.newIptalPr(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), 1);


                if (fis.yazdirilmismi)
                {
                    yazdirilmissa = "Yazdırılmış";
                }
                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc);
                }

            }
        }

        private void btn_Mars_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(this.Tag) != "D" && Convert.ToInt32(bartxt_FisNo.EditValue) > 0)
            {
                //Fis_Islem.Mars_Update(Convert.ToInt32(bartxt_FisNo.EditValue), Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")));
                //Siparis_Gonder(true);
                //this.Close();

                Mars_SatirSec mars = new Mars_SatirSec();
                mars.Tag = Convert.ToInt32(bartxt_FisNo.EditValue);
                mars.txt_Fisno.Text = Convert.ToInt32(bartxt_FisNo.EditValue).ToString();
                mars.ShowDialog();

                if (mars.cikis)
                {

                }
                else
                {
                    btn_Siparis.Enabled = false;
                    //Siparis_Gonder(true);
                    MarsKontrol();
                    this.Close();
                }
            }
        }

        private void Satis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                txt_Filtre.Focus();
            }

            if (e.KeyCode == Keys.F1)
            {
                kisiyiTemizle();
            }

            if (e.KeyCode == Keys.F3 && btn_Siparis.Enabled)
            {
                siparisYazdir();
            }

            if (e.KeyCode == Keys.F6)
            {
                FisPr pr = new FisPr();
                pr.newSiparisPr(Convert.ToInt32(bartxt_FisNo.EditValue), false, Split, kisiyeSatis: txtKisiyeSatis.Text, tumsiparisiTekrarGonder: true);
            }

            if (e.KeyCode == Keys.Escape && btn_Cikis.Enabled)
            {
                cikisyap();
            }
        }


        private void btn_Zayi_Click(object sender, EventArgs e)
        {

            decimal deger = Convert.ToDecimal(gridColumn4.SummaryItem.SummaryValue);

            //if (deger == 0)
            //{
            //    RHMesaj.MyMessageInformation("Zayi düzeltilemez\nçünkü genel toplam eksiye düşer !");
            //    return;
            //}

            DataTable dataTable = dbtools.SelectTableR("select Pkod_Ad from Pos_Kodlar where Pkod_Sinif='23' and Pkod_Kod like 'ZZ%' -- ikram");

            if (gridView1.RowCount > 0)
            {
                if (MessageBox.Show(res_man.GetString("Seçili Kaydı Zayi Yapmak İstediğinize Emin Misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                decimal Ilk_Miktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
                decimal Zayi_Miktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
                if (Zayi_Miktar > 1)
                {
                    Klavye1 klv1 = new Klavye1();
                    klv1.txt_Sayi.Text = Zayi_Miktar.ToString();
                    klv1.Tag = "ZAYISIL";
                    klv1.ShowDialog();
                    if (klv1.Cikis)
                    {
                        return;
                    }
                    if (klv1.sayi <= 0)
                    {
                        return;
                    }
                    if (Zayi_Miktar < klv1.sayi)
                    {
                        return;
                    }
                    Zayi_Miktar = klv1.sayi;
                }

                Klavye2 klv = new Klavye2(data: dataTable);
                klv.ShowDialog();

                if (klv.yazi.Length == 0)
                {
                    return;
                }

                Fis_Islem.Zayi(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), Convert.ToInt32(bartxt_FisNo.EditValue), klv.yazi, Zayi_Miktar);

                Miktar = Ilk_Miktar - Zayi_Miktar;
                if (Miktar > 0)
                {
                    string urun = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Recete"));
                    Rsat_SiparisPr = true;
                    Urun_Sat(urun, false, 0, true);
                }

                FisPr pr = new FisPr();
                string sonuc = "";
                sonuc = pr.ZayiPr(Convert.ToInt32(bartxt_FisNo.EditValue), false, Split, "   * * * ZAYİ FISI * * *   ", "", "", true);


                dbtools.execcmd(@"update Cst_Recete_Satis set Rsat_SiparisPr = 1,Rsat_Zayi = 1,Rsat_Kdv = 0,Rsat_Kdvoran= NULL  where Rsat_Id = '" + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "'");

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Zayi, Log.Log_Islem.Duzelt, "Recete : " + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Zayi Alındı", Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")));


                gridyenile();


                Miktar = 1;
                Rsat_SiparisPr = false;
            }
        }

        private void btn_Ikram_Click(object sender, EventArgs e)
        {

            decimal deger = Convert.ToDecimal(gridColumn4.SummaryItem.SummaryValue);

            //if (deger == 0)
            //{
            //    RHMesaj.MyMessageInformation("İkram düzeltilemez\nçünkü genel toplam eksiye düşer !");
            //    return;
            //}

            DataTable dataTable = dbtools.SelectTableR("select Pkod_Ad from Pos_Kodlar where Pkod_Sinif='23' and Pkod_Kod like 'II%' -- ikram");


            string neden = "";
            if (gridView1.RowCount > 0)
            {
                int Id = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id"));
                decimal miktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
                decimal ikramlogmiktar = 1;
                if (miktar > 1)
                {
                    Klavye1 klv = new Klavye1();
                    klv.txt_Sayi.Text = miktar.ToString();
                    klv.Tag = "IKRAM";
                    klv.ShowDialog();
                    if (klv.Cikis)
                    {
                        return;
                    }
                    if (klv.sayi <= 0)
                    {
                        return;
                    }
                    if (miktar < klv.sayi)
                    {
                        MessageBox.Show(res_man.GetString("Hatalı Giriş..."));
                        return;
                    }

                    ikramlogmiktar = klv.sayi;

                    Klavye2 klv2 = new Klavye2(data: dataTable);
                    klv2.ShowDialog();

                    if (klv2.yazi.Length == 0)
                    {
                        MessageBox.Show(res_man.GetString("Hatalı Giriş..."));
                        return;
                    }
                    neden = klv2.yazi;

                    decimal recfiyat = Convert.ToDecimal(dbtools.DegerGetir("select Rec_Fiyat from Cst_Recete where Rec_Genelkod=(select Rsat_Recete from Cst_Recete_Satis where Rsat_Id='" + Id + "')")) * klv.sayi;
                    bool Rsat_SiparisPr = Convert.ToBoolean(dbtools.DegerGetir("select top 1 isnull(Rsat_SiparisPr,0) as Rsat_SiparisPr from Cst_Recete_Satis where Rsat_Id='" + Id + "'"));


                    //dbtools.execcmd("Update Cst_Recete_Satis set Rsat_Miktar = '" + Convert.ToString(klv.sayi).Replace(",", ".") + "',Rsat_Net = 0,Rsat_Kdv = 0,Rsat_Tutar = 0,Rsat_Fiyat='" + recfiyat.ToString().Replace(",", ".") + "', Rsat_Doviztutar = 0,Rsat_SiparisPr = 1,Rsat_Ikram = 1,Rsat_IkramNeden = '" + klv2.yazi + "' where Rsat_Id = '" + Id + "'");

                    // 13.08.2024 tarihinde ürün transfer fiyat atıyor diye rsat_fiyat da sıfırlandı
                    dbtools.execcmd("Update Cst_Recete_Satis set Rsat_Miktar = '" + Convert.ToString(klv.sayi).Replace(",", ".") + "',Rsat_Net = 0,Rsat_Kdv = 0,Rsat_Tutar = 0,Rsat_Fiyat=0, Rsat_Doviztutar = 0,Rsat_SiparisPr = 1,Rsat_Ikram = 1,Rsat_IkramNeden = '" + klv2.yazi + "' where Rsat_Id = '" + Id + "'");


                    Miktar = miktar - klv.sayi;
                    if (Miktar > 0)
                    {
                        Urun_Sat(Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Recete")), siparisPr: Rsat_SiparisPr);
                    }
                }
                if (miktar == 1)
                {

                    Klavye2 klv2 = new Klavye2(data: dataTable);
                    klv2.ShowDialog();
                    if (klv2.yazi.Length == 0)
                    {
                        return;
                    }

                    ikramlogmiktar = 1;
                    neden = klv2.yazi;

                    dbtools.execcmd("Update Cst_Recete_Satis set Rsat_Tutar = 0,Rsat_Net=0, Rsat_Doviztutar = 0,Rsat_Kdv = 0,Rsat_Ikram = 1,Rsat_IkramNeden = '" + klv2.yazi + "' where Rsat_Id = '" + Id + "'");
                }

                try
                {
                    //
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.ikram, Log.Log_Islem.Duzelt, "Recete : " + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Miktar : " + ikramlogmiktar + " tanesi ikram edildi. Kalan Miktar:" + (miktar - ikramlogmiktar), Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")), Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")), ikramlogmiktar, neden, Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")));
                }
                catch (Exception ex)
                {


                }

                Fis_Islem.ServisPayi(Convert.ToInt32(bartxt_FisNo.EditValue));

                gridyenile();
            }
        }

        private void gridView1_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (e.Column == gridColumn1)
            {
                if (Param.Calisma_Sekli == 0)
                {
                    e.Info.DisplayText = res_man.GetString("Toplam : ") + Convert.ToDecimal(gridColumn4.SummaryItem.SummaryValue).ToString("N2");
                }
                else
                {
                    e.Info.DisplayText = res_man.GetString("Toplam : ") + Convert.ToDecimal(gridColumn5.SummaryItem.SummaryValue).ToString("N2");
                }
            }
        }

        private void btn_Aciklama_Click(object sender, EventArgs e)
        {
            Klavye2 klv = new Klavye2();
            klv.ShowDialog();
            txt_EkNot.Text = klv.yazi;
        }

        private void txt_Barkod_Leave(object sender, EventArgs e)
        {
            Barkod_Oku();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Pos_TeraziBarkodEkran p = new Pos_TeraziBarkodEkran();
            p.ShowDialog();

            if (p.Durum == true)
            {
                Miktar = Convert.ToDecimal(p.urun_Miktar);
                Urun_Sat(p.txt_UrunKodu.Text);
                barkodFocuslan();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) == String.Empty)
            {
                return;
            }

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "A")
            {
                MessageBox.Show(res_man.GetString("Ödemeler veya İndirimler Silinemez.."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //if (MessageBox.Show(res_man.GetString("Seçili Satırı Silmek İstediğinize Emin Misiniz...?", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            //{
            //    return;
            //}

            //try
            //{
            bool Rsat_SiparisPr = Convert.ToBoolean(dbtools.DegerGetir("select ISNULL((select ISNULL(Rsat_SiparisPr,0) from Cst_Recete_Satis WHERE Rsat_Id = " + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "),0)"));
            //}
            //catch
            //{
            //MessageBox.Show(res_man.GetString("Gruba Ait Printer Parametreleri Tanımlı Değil.",res_man.GetString("Uyarı"),MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //return;
            //}


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
                DataTable dataTable = dbtools.SelectTableR("select Pkod_Ad from Pos_Kodlar where Pkod_Sinif='23' and Pkod_Kod like 'SP%' -- AÇIKLAMA");

                Klavye2 klv = new Klavye2(data: dataTable);
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

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Satir_Sil, Log.Log_Islem.Sil, "Recete : " + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Miktar : " + Sil_Miktar + " Silindi", Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")), Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")), Sil_Miktar, neden, Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")));

            Fis_Islem.ServisPayi_Sil(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), Sil_Miktar);


            int satirsay = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + bartxt_FisNo.EditValue.ToString() + "' and Rsat_Ba = 'B' "));
            if (satirsay == 0)
            {
                dbtools.execcmd("update Pos_Masa set Masa_Durum = 0, Masa_Ozel = '' where Masa_No = '" + Convert.ToString(bartxt_MasaNo.EditValue) + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Fisno = '" + bartxt_FisNo.EditValue.ToString() + "'");
            }

            gridyenile();


        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) == String.Empty)
            {
                return;
            }

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "A")
            {
                MessageBox.Show(res_man.GetString("Ödemeler veya İndirimler Silinemez.."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //if (User.G_Satirsil_Y == false)
            //{
            //    if (User.Pos_ArtiEksi_Aktif == true)
            //    {
            //        if (Convert.ToBoolean(gridView1.GetFocusedRowCellValue("Rsat_SiparisPr2")) == true)
            //        {
            //            btn_Arti.Enabled = false;
            //            btn_Eksi.Enabled = false;
            //            btn_Arti.Visible = false;
            //            btn_Eksi.Visible = false;
            //        }
            //        else
            //        {
            //            btn_Arti.Enabled = true;
            //            btn_Eksi.Enabled = true;
            //            btn_Arti.Visible = true;
            //            btn_Eksi.Visible = true;
            //        }
            //    }
            //}
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) == String.Empty)
            {
                return;
            }

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "A")
            {
                MessageBox.Show(res_man.GetString("Ödemelere veya İndirimlere Açıklama Girilemez.."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string rsatid = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id"));

            DataTable dataTable = dbtools.SelectTableR("select Pkod_Ad from Pos_Kodlar where Pkod_Sinif='23' and Pkod_Kod like 'AC%' -- AÇIKLAMA");

            Klavye2 klv = new Klavye2(data: dataTable);
            klv.txt_Yazi.Text = dbtools.DegerGetir("select Rsat_Aciklama from Cst_Recete_Satis where Rsat_Id=" + rsatid);
            klv.ShowDialog();

            if (klv.cikisYeni == true)
            {
                return;
            }

            dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_Aciklama = '" + klv.yazi + "' where Rsat_Id = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "'");

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Satis, Log.Log_Islem.Kaydet, "Recete : " + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Reçeteye Açıklama Girildi : " + klv.yazi, Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")), Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")), 0, "", Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")));

            gridyenile();
        }

        private void alt_btnUp_Click(object sender, EventArgs e)
        {
            //int change = flp_AltGrup.VerticalScroll.Value + flp_AltGrup.VerticalScroll.SmallChange * 10;
            //flp_AltGrup.AutoScrollPosition = new Point(0, change);

            flp_AltGrup.AutoScrollPosition = new Point(0, flp_AltGrup.VerticalScroll.Value - flp_AltGrup.VerticalScroll.SmallChange * 30);

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            flp_AltGrup.AutoScrollPosition = new Point(0, flp_AltGrup.VerticalScroll.Value + flp_AltGrup.VerticalScroll.SmallChange * 30);
        }

        private void ana_btnUp_Click(object sender, EventArgs e)
        {
            flp_Urun.AutoScrollPosition = new Point(0, flp_Urun.VerticalScroll.Value - flp_Urun.VerticalScroll.SmallChange * 30);
        }

        private void ana_btnDown_Click(object sender, EventArgs e)
        {
            flp_Urun.AutoScrollPosition = new Point(0, flp_Urun.VerticalScroll.Value + flp_Urun.VerticalScroll.SmallChange * 30);
        }

        private void rdo_EMiktar_SelectedIndexChanged(object sender, EventArgs e)
        {
            eMiktar = (string)rdo_EMiktar.EditValue;
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) == String.Empty)
            {
                return;
            }

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "A")
            {
                MessageBox.Show(res_man.GetString("Ödemelere veya İndirimlere Açıklama Girilemez.."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Klavye2 klv = new Klavye2();
            klv.ShowDialog();

            dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_Joker = '-' + '" + klv.yazi + "' where Rsat_Id = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "'");
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Satis, Log.Log_Islem.Kaydet, "Recete : " + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Reçeteye Joker Aciklama Girildi : " + klv.yazi, Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")), Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")), 0, "", Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")));


            Klavye1 klv2 = new Klavye1();
            klv2.UrunAdi = Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad2"));
            klv2.Tag = "TUTARDUZELT";
            string eskitutar;

            if (Param.Calisma_Sekli == 1)   //Dövizli
            {
                eskitutar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Doviztutar")).ToString();
            }
            else
            {
                eskitutar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")).ToString();
            }
            klv2.txt_Sayi.Text = eskitutar;
            klv2.ShowDialog();
            decimal tutar = klv2.sayi;

            if (tutar != Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Tutar")))
            {
                Fis_Islem.Tutar_Duzelt(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), tutar);

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Tutar_Duzelt, Log.Log_Islem.Duzelt, Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + "'nın Fiyatı " + eskitutar + " iken " + tutar.ToString() + " ile Değişti", Convert.ToString(bartxt_FisNo.EditValue), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")));

            }



            gridyenile();
        }


        bool canClose = false;

        private void Satis_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = !canClose;

            if (isYazdirilmamisSiparis())
            {
                e.Cancel = true;
            }
        }

        public bool isYazdirilmamisSiparis()
        {
            try
            {
                if (yazdirilmamisSiparis == false)
                {
                    return false;
                }

                string deger = dbtools.DegerGetir("select count(Rsat_SiparisPr) as toplam from Cst_Recete_Satis where Rsat_Fisno='" + bartxt_FisNo.EditValue.ToString() + "' and (Rsat_SiparisPr='0' and isnull(Rsat_Mars,0)=0)");

                if (Convert.ToInt32(deger) > 0)
                {
                    MessageBox.Show("Yazdırılmamış sipariş vardır! ");
                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA " + ex.Message);
            }

            return false;

        }
        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_Not_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void panelControl5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRelogin_Click(object sender, EventArgs e)
        {
            Relogin();
        }
        private void Relogin()
        {
            frmLogin login = new frmLogin();
            login.ShowDialog();
            if (login.Cikis)
            {
                Application.Exit();
                return;
            }
            Bilgileri_Doldur();
        }

        private void btnYenile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridyenile();
        }

        private void Satis_FormClosed(object sender, FormClosedEventArgs e)
        {
            // bartxt_FisNo.EditValue.ToString()

            string toplam = dbtools.DegerGetir("select count(*) as toplam from Cst_Recete_Satis where Rsat_Fisno='" + bartxt_FisNo.EditValue.ToString() + "'");
            if (toplam.Equals("0"))
            {
                dbtools.execcmd("update Pos_Masa set Masa_Durum='0',Masa_NFC='0',Masa_Ozel=NULL where Masa_No='" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
            }


            hesapyazmissaRenginiGuncelle();
        }

        private void btnAdresGuncelle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Masa_Paket)
            {

                mCari = Cari.Cari_Getir(bartxt_OdaNo.EditValue.ToString());


                CariHesap cari = new CariHesap();
                cari.xtraTabControl1.SelectedTabPageIndex = 1;
                cari.Tel = CariTel;
                cari.AcikAdres = AcikAdres;
                cari.CariKod = Cari_Kod;
                cari.carim = mCari;
                cari.ShowDialog();

                string carikod = cari.CariKod;
                if (carikod != "")
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Cari = '" + carikod + "' Where Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue) + "'");
                }
            }
        }

        private void btnMenuAc_Click(object sender, EventArgs e)
        {
            panelMenu.Visible = !panelMenu.Visible;
        }

        private void btnMenuGizle_Click(object sender, EventArgs e)
        {
            panelMenu.Visible = false;
        }

        public void shownAc()
        {
            marsKontrol(false);
            // btn_Siparis.Enabled = true;
            StatikSinif.masaKilitle(Masa_No);

            btn_Cikis.Enabled = true;

            if (otomatikSatis)
            {
                Arama ara = new Arama();// pos tatış nfc aktif değilse
                ara.Tag = "D";
                ara.kartnom = kartnom;
                ara.otomatikSatis = true;
                ara.ShowDialog();

                D_Mus_tipi = ara.Mus_tipi;
                D_Oda_No = ara.Oda_No;
                D_Folio = ara.Folio;
                D_Pansiyon = ara.Pansiyon;
                D_Uye_Id = ara.Uye_Id;
                D_Uye_Adsoyad = ara.Uye_Adsoyad;
                D_Uye_Kartturu = ara.Uye_Kartturu;
                D_Cari_Kod = ara.Cari_Kod;
                D_Ind_Kodu = ara.Ind_Kodu;
                D_Ind_Oran = ara.Ind_Oran;
                D_Odeme = ara.Odeme_Kodu;
                D_MasterId = ara.Master_Folio;
                Kart_No = ara.Kart_No;
                FolioKart_ID = ara.KartID;

                bartxt_OdaNo.EditValue = ara.Oda_No;

                CardF_Indirim = ara.CardF_Indirim;
                try
                {
                    string comp = Fronttools.DegerGetir("select top 1 CardF_MusteriTipi from kartf where ID='" + ara.KartID + "'");

                    if (comp.Equals("CM"))
                    {
                        label1.Text = "COMP";
                        label1.ForeColor = Color.Red;
                    }
                    else if (comp.Equals("OM"))
                    {
                        label1.Text = "OTEL MİSAFİRİ";
                        label1.ForeColor = Color.Red;
                    }

                    decimal folioBakiye = Fronttools.BalanceBul(D_MasterId, Kart_No, FolioKart_ID) * -1; //13,95
                    btnBakiye.Text = folioBakiye.ToString("N2");


                }
                catch (Exception ex) { }


                Urun_Sat(recetekod, otomiktar: hizmetmiktar);
                Urun_Sat(recetekodCocuk, otomiktar: hizmetmiktarCocuk);

                foreach (SimpleButton item in flp_Kapatma.Controls)
                {
                    if (item.Tag.ToString() == hizmetOdemeKod)
                    {
                        item.PerformClick();
                        break;
                    }
                }
            }
        }
        private void Satis_Shown(object sender, EventArgs e)
        {
            shownAc();
        }

        public void marsKontrol(bool siparisten)
        {
            string Rsat_Mars = dbtools.DegerGetir("select top 1 Rsat_Mars from Cst_Recete_Satis where Rsat_Fisno='" + bartxt_FisNo.EditValue.ToString() + "'");

            if (Rsat_Mars.ToLower().Equals("true"))
            {
                btn_Siparis.Enabled = false;
                btn_Cikis.Enabled = true;
            }

            if (siparisten)
            {
                btn_Cikis.Enabled = false;

                if (Param.Param_SatisCikisButton)
                {
                    btn_Cikis.Enabled = Param.Param_SatisCikisButton;
                }
            }

            if (gridView1.DataRowCount < 1)
            {
                btn_Cikis.Enabled = true;
            }

            if (btn_Siparis.Enabled == false)
            {
                btn_Cikis.Enabled = true;
            }

        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            string Rsat_Id = gridView1.GetFocusedRowCellValue("Rsat_Id").ToString();
            string query = @"select top 1 Rsat_Masa,Rsat_OzelMasaAdi,Rsat_Kisi,Rsat_Fisno,Rsat_Aciklama,Rsat_Not,Rec_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_Tarih,Rsat_Acilis,Rsat_Garson from Cst_Recete_Satis 
left join Cst_Recete on Rsat_Recete=Rec_Genelkod
where  Rsat_Id='" + Rsat_Id + "'";
            DataTable dt = dbtools.SelectTable(query);


            DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'URUN'");
            if (dtDizayn.Rows.Count < 1)
            {
                RHMesaj.MyMessageInformation("Ürün Printer Dizaynı Yapılmamış...\nAyarlar-> Printer Ayarları -> Adisyon Fatura -> Ürün Dizayn");
                return;
            }

            foreach (DataRow item in dt.Rows)
            {
                RaporUrun siparis = new RaporUrun();
                xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), siparis);
                siparis.PrinterName = Main.ayarlar.getYazici();

                siparis.txtAciklama.Text = item["Rsat_Aciklama"].ToString();
                siparis.txtUrun.Text = item["Rec_Ad"].ToString();
                siparis.txtMiktar.Text = item["Rsat_Miktar"].ToString();
                siparis.txtTarih.Text = Convert.ToDateTime(item["Rsat_Tarih"].ToString()).ToString("dd.MM.yyyy");
                siparis.txtAcilis.Text = item["Rsat_Acilis"].ToString().Split(':')[0] + ":" + item["Rsat_Acilis"].ToString().Split(':')[1];
                siparis.txtGarson.Text = item["Rsat_Garson"].ToString();

                siparis.txtFisno.Text = item["Rsat_Fisno"].ToString();
                // siparis.txtKisiSayisi.Text = item["Rsat_Kisi"].ToString();

                string masa = item["Rsat_Masa"].ToString().Trim();
                string ozelMasa = item["Rsat_OzelMasaAdi"].ToString().Trim(); ;

                if (ozelMasa.Contains(masa))
                {
                    masa = ozelMasa;
                }
                else
                {
                    masa = masa + " " + ozelMasa;
                }

                siparis.txtMasaNo.Text = masa;

                siparis.Print();

                RHMesaj.alertMesaj2("SİPARİŞİNİZ YAZDIRILDI...", 1);
                break;
            }


        }

        private void btnTopluSil_Click(object sender, EventArgs e)
        {
            try
            {
                int fisno = Convert.ToInt32(bartxt_FisNo.EditValue);

                string odemevarmi = dbtools.DegerGetir(" select top 1 count(*) as toplam from Cst_Recete_Satis where Rsat_Fisno='" + fisno + "' and Rsat_Ba='A'");
                if (odemevarmi != "0")
                {
                    MessageBox.Show(res_man.GetString("Ödemeler veya İndirimler Silinemez.."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                string yazdirilmamisSiparisVarmi = dbtools.DegerGetir("select top 1 count(Rsat_SiparisPr) as toplam from Cst_Recete_Satis where Rsat_Fisno='" + fisno + "' and Rsat_SiparisPr='1' ");

                if (yazdirilmamisSiparisVarmi != "0" && !User.G_Satirsil_Y)
                {
                    MessageBox.Show(res_man.GetString("Yazdırılmış Satır Silme Yetkiniz Yoktur...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }


                ConfirmationForm confirmationForm = new ConfirmationForm("Hepsini silmek istediğine emin misin ?");
                confirmationForm.ShowDialog();

                if (confirmationForm.onay)
                {

                    string neden = "";
                    if (Departman.Kodlar_YazSipNedSor)
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


                    DataTable dataTable = dbtools.SelectTableR(@" select Rsat_Miktar ,Cst_Recete.Rec_Ad ,Rsat_Id,Rsat_Tutar from Cst_Recete_Satis
 left join cst_recete  on Cst_Recete.Rec_Genelkod=Rsat_Recete
 where Rsat_Fisno='" + fisno + "' and Rsat_Ba<>'A'");

                    foreach (DataRow item in dataTable.Rows)
                    {
                        decimal miktar = Convert.ToDecimal(item["Rsat_Miktar"].ToString());
                        int Rsat_Id = Convert.ToInt32(item["Rsat_Id"].ToString());
                        string Rec_Ad = item["Rec_Ad"].ToString();
                        decimal Rsat_Tutar = Convert.ToDecimal(item["Rsat_Tutar"].ToString());
                        string yazdirilmissa = "Yazdırılmamış";
                        if (Departman.Siparis)
                        {
                            FisPr fis = new FisPr();
                            string sonuc = fis.newIptalPr(Rsat_Id, miktar);
                            if (fis.yazdirilmismi)
                            {
                                yazdirilmissa = "Yazdırılmış";
                            }
                            if (sonuc != "OK")
                            {
                                MessageBox.Show(sonuc);
                            }
                        }

                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Satir_Sil, Log.Log_Islem.Sil, yazdirilmissa + " Sipariş-> " + "Recete : " + Rec_Ad + " Miktar : " + miktar + " Silindi", Convert.ToString(bartxt_FisNo.EditValue), Rsat_Id.ToString(), Rec_Ad, miktar, neden, Rsat_Tutar);

                        Fis_Islem.Satir_Sil(Rsat_Id, miktar);


                        int satirsay = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + bartxt_FisNo.EditValue.ToString() + "' and Rsat_Ba = 'B' "));
                        if (satirsay == 0)
                        {
                            dbtools.execcmd("update Pos_Masa set Masa_Durum = 0, Masa_Ozel = '' where Masa_No = '" + Convert.ToString(bartxt_MasaNo.EditValue) + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                            dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Fisno = '" + bartxt_FisNo.EditValue.ToString() + "'");
                        }
                    }


                    gridyenile();

                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnTopluSil_Click", "", ex);
            }

        }

        private void btnServisTutarDuzelt_Click(object sender, EventArgs e)
        {
            string receteKod = gridView1.GetFocusedRowCellValue("Rsat_Recete").ToString();
            string bindirimReceteKod = dbtools.DegerGetir("select top 1 Param_Bindirim  from Pos_Param where Param_Id = '1'");

            if (receteKod == bindirimReceteKod)
            {
                tutarDuzelt();
            }
            else
            {
                RHMesaj.alertMesaj("Sadece servis payı düzeltilebilir !");
            }
        }

        private void btn_Bindirim_Click(object sender, EventArgs e)
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

                    if (oran23 > User.P_Bindirim_Yuzde)
                    {
                        MessageBox.Show("Max Bindirim Yuzdesini Aştınız..." + "\n" + "Max Bindirim Yüzdeniz : %" + User.P_Bindirim_Yuzde.ToString() + "\n" + "Şuan ki Bindirim Oranı : %" + oran23.ToString("n2"));
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
                int fisno = Convert.ToInt32(bartxt_FisNo.EditValue);
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
        }

        int sayac = 1;
        private void btnTemizleKisiyeSatis_Click(object sender, EventArgs e)
        {
            kisiyiTemizle();
        }

        public void kisiyiTemizle()
        {

            txtKisiyeSatis.Text = "";
            txtKisiyeSatis.Select();
            txtKisiyeSatis.Focus();

            sayac++;
            txtKisiyeSatisSayac.Text = sayac + "";


        }

        private void btnKisiyeSatisYap_Click(object sender, EventArgs e)
        {
            int fisno = Convert.ToInt32(bartxt_FisNo.EditValue);
            if (fisno < 1)
            {
                return;
            }
            KisiyeSatisSec kisiyeSatisSec = new KisiyeSatisSec(fisno);
            kisiyeSatisSec.ShowDialog();

            if (kisiyeSatisSec.iptal == false)
            {
                txtKisiyeSatis.Text = kisiyeSatisSec.adsoyad;
                txtKisiyeSatisSayac.Text = kisiyeSatisSec.sayac;
            }
        }

        private void btnMasaSec_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MasaSecForm masaSecForm = new MasaSecForm();
            masaSecForm.ShowDialog();
        }

        public void dizaynyukle()
        {
            try
            {

                if (File.Exists(fileName))
                {
                    gridView1.RestoreLayoutFromXml(fileName);
                }

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "dizaynyukle", "RmosCrm.exe'nin bulunduğu yerde GridDizayn klasörünü açınız", ex);
            }

        }
        private void btnGridDizaynKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                gridView1.SaveLayoutToXml(fileName);
                MessageBox.Show("KAYDEDİLDİ");
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnGridDizaynKaydet_Click", "RmosCrm.exe'nin bulunduğu yerde GridDizayn klasörünü açınız", ex);
            }

        }
        string fileName = Departman.Dep_Kodu + "_" + MyClass + ".xml";

        private void btnGridDizaynTemizle_Click(object sender, EventArgs e)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            MessageBox.Show("SİLİNDİ");

        }

        private void btnFiyatBilgisi_Click(object sender, EventArgs e)
        {
            BarkodUrunBilgisiForm form = new BarkodUrunBilgisiForm();
            form.ShowDialog();
        }


        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool IsWow64Process(IntPtr hProcess, out bool Wow64Process);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool Wow64DisableWow64FsRedirection(out IntPtr OldValue);



        private void txt_Filtre_Click(object sender, EventArgs e)
        {
            klavyeac();

        }

        public void klavyeac()
        {
            try
            {
                return;
                bool bWow64 = false;
                IsWow64Process(Process.GetCurrentProcess().Handle, out bWow64);
                if (bWow64)
                {
                    IntPtr OldValue = IntPtr.Zero;
                    bool bRet = Wow64DisableWow64FsRedirection(out OldValue);
                }
                Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = "osk" });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txt_Not_Click(object sender, EventArgs e)
        {
            klavyeac();

        }

        private void txtFisnoGit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (txtFisnoGit.Text == "") return;
                    int okutulanFisno = Convert.ToInt32(txtFisnoGit.Text.ToString());


                    int yenifisno = Convert.ToInt32(bartxt_FisNo.EditValue);

                    string fisvarmi = dbtools.DegerGetir($"select count(*) as toplam from Cst_Recete_Satis where Rsat_Fisno='{okutulanFisno}'");

                    if (fisvarmi == "0")
                    {
                        string text = $"Okutulan Fis No Bulunamadı veya Kapatılmış\nFiş No: {okutulanFisno}";
                        UyariForm uyariForm = new UyariForm(text);
                        uyariForm.ShowDialog();
                        return;
                    }



                    dbtools.execcmdR($"update Cst_Recete_Satis set Rsat_Fisno='{yenifisno}',Rsat_Durum='A' where Rsat_Fisno='{okutulanFisno}'");

                    gridyenile();

                    fisnoBirlestirFocus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void fisnoBirlestirFocus()
        {
            txtFisnoGit.Text = "";
            txtFisnoGit.Select();
            txtFisnoGit.Focus();
        }

        private void btn_Not_Gonder_Click(object sender, EventArgs e)
        {
            int fisno = Convert.ToInt32(bartxt_FisNo.EditValue);

            if (fisno > 0 && Masa_No != String.Empty)
            {
                Siparis_Not not = new Siparis_Not();
                not.Fisno = fisno;
                not.ShowDialog();
            }
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            return;

            if (e.RowHandle < 0) return;

            GridView view = sender as GridView;

            // DataTable'dan ilgili değeri al
            object val = view.GetRowCellValue(e.RowHandle, "Rsat_SiparisPr");
            if (val == null || val == DBNull.Value) return;

            bool siparisPr = Convert.ToBoolean(val);

            // Tüm satır renklensin istiyorsan sadece RowHandle kontrolü yeterli
            if (!siparisPr) // false ise
            {
                e.Appearance.BackColor = Color.DarkGreen;
                e.Appearance.ForeColor = Color.White;
            }
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle < 0) return;

            GridView view = sender as GridView;
            object val = view.GetRowCellValue(e.RowHandle, "Rsat_SiparisPr");
            if (val == null || val == DBNull.Value) return;

            bool siparisPr = Convert.ToBoolean(val);

            if (e.Column.FieldName == "Rsat_Emiktar") // örnek: Masa kolonunun başına ikon koyalım
            {
                e.DefaultDraw();

                Image img = siparisPr
                    ? Properties.Resources.print_ok   // ✅ True
                    : Properties.Resources.printerCancel; // ❌ False

                int x = e.Bounds.Right - img.Width - 10;
                int y = e.Bounds.Y + (e.Bounds.Height - img.Height) / 2;

                e.Cache.DrawImage(img, new Rectangle(x, y, img.Width, img.Height));
            }
        }

        /*
string adsoyad = hesap.gridViewKisiyeSatis.GetFocusedRowCellValue("Ad Soyad").ToString();

string query = $@"select rec.Rec_Ad as 'Ürün Ad',Rsat_Miktar as Miktar,Rsat_Tutar as Toplam from Cst_Recete_Satis sat
left join Cst_Recete rec on rec.Rec_Genelkod=sat.Rsat_Recete
where Rsat_Fisno='{fisno}' and kisiyeSatisAdSoyad='{adsoyad}' and Rsat_Ba='B'  ";
DataTable dataTable = dbtools.SelectTableR(query);


if (dataTable != null && dataTable.Rows.Count > 0)
{
foreach (DataRow dr in dataTable.Rows)
{
dr["Miktar"] = dr["Miktar"].ToString().Replace(",0000", "");
dr["Miktar"] = dr["Miktar"].ToString().Replace(",000", "");
dr["Toplam"] = dr["Toplam"].ToString().Replace(",00", "");
dr["Toplam"] = dr["Toplam"].ToString().Replace(",000", "");
dr["Toplam"] = dr["Toplam"].ToString().Replace(",0000", "");
}

}
*/



    }
}