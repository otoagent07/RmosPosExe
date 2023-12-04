using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Newtonsoft.Json;
using Pos.Class;
using Pos.Ingenico;
using Pos.Models;
using Pos.Print;
using Pos.YemekSepeti;
using RmosAcentex.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using System.ServiceProcess;
using System.Windows.Forms;

namespace Pos
{
    public partial class Ayarlar : DevExpress.XtraEditors.XtraForm
    {
        IntegrationWebService1.Integration iws = new IntegrationWebService1.Integration();
        public Ayarlar()
        {
            InitializeComponent();
        }
        DataTable dt2, dt3;

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        public void yaziciYukle()
        {
            try
            {
                string query = @"SELECT Pkod_Ad FROM Pos_Kodlar  
where Pkod_Sinif = '16' and Pkod_Ustgrup = 'HES'
group by Pkod_Ad";

                DataTable dataTable = dbtools.SelectTableR(query);
                lookUpEditYazici.Properties.DataSource = dataTable;
                lookUpEditYazici.Properties.ValueMember = "Pkod_Ad";
                lookUpEditYazici.Properties.DisplayMember = "Pkod_Ad";

                lookUpEditYazici.EditValue = dataTable.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {

            }
        }
        public void yaziciYukleHesap()
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Pkod_Ad", typeof(string));
                foreach (var item in PrinterSettings.InstalledPrinters)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["Pkod_Ad"] = item;
                    dataTable.Rows.Add(dataRow);
                }

                lookUpEditHesapDokYazici.Properties.DataSource = dataTable;
                lookUpEditHesapDokYazici.Properties.ValueMember = "Pkod_Ad";
                lookUpEditHesapDokYazici.Properties.DisplayMember = "Pkod_Ad";
            }
            catch (Exception ex)
            {

            }
        }
        public void dilYukle()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Kod", typeof(string));
                dt.Columns.Add("Ad", typeof(string));

                dt.Rows.Add("tr-TR", "TÜRKÇE");
                dt.Rows.Add("en-US", "ENGLISH");
                dt.Rows.Add("ru", "RUSSIAN");


                Pos_dil.Properties.DataSource = dt;
                Pos_dil.Properties.DisplayMember = "Ad";
                Pos_dil.Properties.ValueMember = "Kod";

                Pos_dil.EditValue = Langs.Default.Dil == "" ? "tr-TR" : Langs.Default.Dil;
            }
            catch (Exception ex)
            {

            }
        }
        private void Ayarlar_Load(object sender, EventArgs e)
        {
            dilYukle();
            yaziciYukle();
            yaziciYukleHesap();
            panelControl1.Visible = false;
            if (User.P_Kod.Equals("999") || User.P_Kart.Equals("999"))
            {
                panelControl1.Visible = true;
            }
            iws.AuthHeaderValue = new IntegrationWebService1.AuthHeader();
            iws.AuthHeaderValue.UserName = YS_AuthHeader.ah.UserName;
            iws.AuthHeaderValue.Password = YS_AuthHeader.ah.Password;

            this.BringToFront();
            // xtraTabControl1.SelectedTabPage = tab_Parametre;
            xtraTabControl1.SelectedTabPage = tab_Home;
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            dt2 = dbtools.SelectTable("select Kodlar_Kod,Kodlar_Kod +' - '+ Kodlar_Ad AS Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif ='01' and Kodlar_Anadepo = 'False' and Kodlar_Satis = 'True' order by Kodlar_Kod");
            dt3 = dbtools.SelectTable("select Kodlar_Kod,Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = '10'");

            if (User.P_Kod.ToUpper() == "RMOS")
            {
                date_Prm_Tarih.Properties.ReadOnly = false;
            }

            //nav_Parametre.Visible = User.A_Parametre;
            navBarItem1.Visible = User.A_Parametre;
            navBarItem18.Visible = User.A_Parametre;
            navBarItem19.Visible = User.A_Parametre;

            nav_Print.Visible = User.A_Print;
            nav_Odeme.Visible = User.A_Odeme;
            nav_Entegre.Visible = User.A_Entegre;
            nav_Masa.Visible = User.A_Masa;
            nav_Cari.Visible = false;
            nav_HH.Visible = User.A_HH;
            nav_Kullanici.Visible = User.A_Kullanici;
            nav_Kasa.Visible = User.A_Kasa;
            navBarItem20.Visible = User.Pos_ReceteTanimlama;


            DataTable dt = Rmosmuh.SelectTable(@"Select User_Kod, User_Ad, User_Soyad, User_Ad + ' ' + User_Soyad As AdSoyad From Users");
            U_BackUser.Properties.DataSource = dt;
            U_BackUser.Properties.DisplayMember = "AdSoyad";
            U_BackUser.Properties.ValueMember = "User_Kod";


            txtUrunSize.Text = dbtools.DegerGetir("select top 1 isnull(Kodlar_Size,'140;60') as Kodlar_Size from Stok_Kodlar where Kodlar_Sinif='09' ");
            //tabParametreler();


            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                comboBoxEditUrunPrinter.Properties.Items.Add(PrinterSettings.InstalledPrinters[i]);
            }

            DataTable dataTableOtoKur = dbtools.SelectTableR("Select Mkodlar_Kod,Mkodlar_Ad From Muh_Kodlar Where MKodlar_Sinif = '02' Order By Mkodlar_Kod");
            lookUpOtoKurSec.Properties.DataSource = dataTableOtoKur;
            lookUpOtoKurSec.Properties.DisplayMember = "Mkodlar_Ad";
            lookUpOtoKurSec.Properties.ValueMember = "Mkodlar_Kod";

            lookUpEditUrunPrinterDepartman.Properties.DataSource = dt2;
            lookUpEditUrunPrinterDepartman.Properties.DisplayMember = "Kodlar_Ad";
            lookUpEditUrunPrinterDepartman.Properties.ValueMember = "Kodlar_Kod";


            yuvarlamaDepartman.Properties.DataSource = dt2;
            yuvarlamaDepartman.Properties.DisplayMember = "Kodlar_Ad";
            yuvarlamaDepartman.Properties.ValueMember = "Kodlar_Kod";

            lookUpOtoIndirimDep.Properties.DataSource = dt2;
            lookUpOtoIndirimDep.Properties.DisplayMember = "Kodlar_Ad";
            lookUpOtoIndirimDep.Properties.ValueMember = "Kodlar_Kod";

            MyGridDoldurAyarlar();
        }

        public void tabParametreler()
        {
            try
            {
                Param.Param_Yukle();
                xtraTabControl1.SelectedTabPage = tab_Parametre;

                date_Prm_Tarih.DateTime = DateTime.Now.Date;

                DataTable dt4 = dbtools.SelectTable("select Mkodlar_Ad, Mkodlar_Kod from Muh_Kodlar where Mkodlar_Sinif = '02'");
                look_Prm_Kurkodu.Properties.DataSource = dt4;
                look_Prm_Kurkodu.Properties.DisplayMember = "Mkodlar_Ad";
                look_Prm_Kurkodu.Properties.ValueMember = "Mkodlar_Kod";
                int width2 = 0;
                foreach (LookUpColumnInfo column in look_Prm_Kurkodu.Properties.Columns)
                    width2 += column.Width;
                look_Prm_Kurkodu.Properties.PopupWidth = width2 + 10;

                DataTable dtRecete = dbtools.SelectTable("select Rec_Genelkod,Rec_Ad from Cst_Recete WITH(NOLOCK) order by Rec_Genelkod");
                look_Prm_Bindirim.Properties.DataSource = dtRecete;
                look_Prm_Bindirim.Properties.DisplayMember = "Rec_Ad";
                look_Prm_Bindirim.Properties.ValueMember = "Rec_Genelkod";


                lookUpEdit_tipbox.Properties.DataSource = dtRecete;
                lookUpEdit_tipbox.Properties.DisplayMember = "Rec_Ad";
                lookUpEdit_tipbox.Properties.ValueMember = "Rec_Genelkod";

                look_Prm_Yuvarla.Properties.DataSource = dtRecete;
                look_Prm_Yuvarla.Properties.DisplayMember = "Rec_Ad";
                look_Prm_Yuvarla.Properties.ValueMember = "Rec_Genelkod";

                yuvarlamaRecete.Properties.DataSource = dtRecete;
                yuvarlamaRecete.Properties.DisplayMember = "Rec_Ad";
                yuvarlamaRecete.Properties.ValueMember = "Rec_Genelkod";

                raporyenile();

                gridyenile_Parametre();

                Terazi_Comport.Text = Param.Param_ComPort;
                Terazi_BaodRate.Text = Convert.ToString(Param.Param_BaudRate);
                Terazi_DataBits.Text = Convert.ToString(Param.Param_DataBits);
                Terazi_StopBits.Text = Param.Param_StopBits.ToString().Replace(",", ".");
                Terazi_Parity.Text = Convert.ToString(Param.Param_Parity);
                Terazi_FlowControl.Text = Param.Param_FlowControl;
                txt_Yukseklik.Text = Param.Param_bSizeH.ToString();
                txt_Genişlik.Text = Param.Param_bSizeW.ToString();
                chk_SatisArama.Checked = Param.Param_SatisArama;
                chk_HesapSor.Checked = Param.Param_HesapSor;
                chk_CariSor.Checked = Param.Param_CariSor;
                chk_CallerID.Checked = Param.Param_CallerID;
                chk_SiparisSayi.Checked = Param.Param_SiparisSayi;
                Param_PaketSiparisPayi.Checked = Param.Param_PaketSiparisPayi;
                Param_SubeGonder.Checked = Param.Param_SubeGonder;
                Param_LimitFolio.Checked = Param.Param_LimitFolio;
                Param_OzelMasaSiralama.Checked = Param.Param_OzelMasaSiralama;
                Param_HesapFisiDokum.Checked = Param.Param_HesapFisiDokum;
                Param_HspFontAlgilama.Checked = Param.Param_HspFontAlgilama;

                Param_AdisyonFolioAdi.Checked = Param.Param_AdisyonFolioAdi;
                Param_FullPos.Checked = Param.Param_FullPos;
                Param_CikisKapa.Checked = Param.Param_CikisKapa;
                Param_DirekAdisyonZor.Checked = Param.Param_DirekAdisyonZor;
                Param_DirekAdisyonPrSor.Checked = Param.Param_DirekAdisyonPrSor;
                Param_KGAlgilama.Checked = Param.Param_KGAlgilama;
                Param_ExtraFolioAcma.Checked = Param.Param_ExtraFolioAcma;
                Param_SiparisAna.Checked = Param.Param_SiparisAna;
                Param_iadeKontrol.Checked = Param.Param_iadeKontrol;
                Param_iadeLimit.EditValue = Param.Param_iadeLimit.ToString("n");
                Pos_HesapDkmRenk.Checked = Param.Pos_HesapDkmRenk;
                Param_AdisyonDegis.Checked = Param.Param_AdisyonDegis;
                Param_AdisyonIndAd.Checked = Param.Param_AdisyonIndAd;
                Param_SiparisTutar.Checked = Param.Param_SiparisTutar;
                Param_AnaEkranCiro.Checked = Param.Param_AnaEkranCiro;
                Param_MasaTakipCiro.Checked = Param.Param_MasaTakipCiro;
                Param_AcilisCekSil.Checked = Param.Param_AcilisCekSil;
                Param_CariAdSoyad.Checked = Param.Param_CariAdSoyad;
                Pos_SatirSil.Checked = Param.Param_SatirSil;
                Pos_SatirSilUser.Text = Param.Param_SatirSilUser;
                Param_MasaTakipMenu.Checked = Param.Param_MasaTakipMenu;
                Param_OdenmezAc.Checked = Param.Param_OdenmezAc;
                Param_ParaUstuIngenico.Checked = Param.Param_ParaUstuIngenico;

                Param_SatisTabloID.EditValue = Param.Param_SatisTabloID;
                Param_SatisTabloAktif.Checked = Param.Param_SatisTabloAktif;
                Param_SatisTabloGonderi.EditValue = Param.Param_SatisTabloGonderi;
                Param_AcilistaMenu.Checked = Param.Param_AcilistaMenu;
                Param_IngenicoSPR.Checked = Param.Param_IngenicoSPR;
                Param_SiparisFisFont.Checked = Param.Param_SiparisFisFont;
                Param_HizliSatisCekAc.Checked = Param.Param_HizliSatisCekAc;
                Param_KartfGBCheckOut.Checked = Param.Param_KartfGBCheckOut;
                Param_YeniHesapDkm.Checked = Param.Param_YeniHesapDkm;
                Param_YeniSiparisDkm.Checked = Param.Param_YeniSiparisDkm;
                Param_OdaKrediCompOdenmez.Checked = Param.Param_OdaKrediCompOdenmez;
                Param_KurTransfer.Checked = Param.Param_KurTransfer;
                Param_CallCenterPaket.Checked = Param.Param_CallCenterPaket;
                Param_PaketDipTotal.Checked = Param.Param_PaketDipTotal;
                Param_HesapKapamaAds.Checked = Param.Param_HesapKapamaAds;
                Param_HesapDkmAciklama.Checked = Param.Param_HesapDkmAciklama;

                Param_AndroGeriYazdir.Checked = Param.Param_AndroGeriYazdir;
                Param_PaketKucukEkran.Checked = Param.Param_PaketKucukEkran;
                Param_GetirTest.Checked = Param.Param_GetirTest;
                Param_GetirOtomatikOnay.Checked = Param.Param_GetirOtomatikOnay;
                Param_SatisCikisButton.Checked = Param.Param_SatisCikisButton;
                Param_nfcBarkodAktif.Checked = Param.Param_nfcBarkodAktif;
                Param_ParcaliMasaAktif.Checked = Param.Param_ParcaliMasaAktif;
                yazdirilmamissiparis.Checked = Param.yazdirilmamissiparis;
                masamusait.Checked = Param.masamusait;
                masatakiphesappasif.Checked = Param.masatakiphesappasif;
                kisivegarsonbirkeresoraktif.Checked = Param.kisivegarsonbirkeresoraktif;
                satirsilfiscikmasinaktif.Checked = Param.satirsilfiscikmasinaktif;
                onburoikramsifiryazaktif.Checked = Param.onburoikramsifiryazaktif;
                cariindirimAktif.Checked = Param.cariindirimAktif;
                txtKartnoSayisi.Text = Param.kartnoSayisi.ToString();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "", "", ex);
            }
        }

        public static string MyClass = "Ayarlar";

        #region *** navBarItem LinkClicked
        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            tabParametreler();
        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Print_Grup;

            DataTable dt = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = '08' order by Kodlar_Kod");

            look_PrintGrup_Ana.Properties.DataSource = dt;
            look_PrintGrup_Ana.Properties.DisplayMember = "Kodlar_Ad";
            look_PrintGrup_Ana.Properties.ValueMember = "Kodlar_Kod";

            Print_Aciklama_Yenile();


        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_PrintAyarlari;

            look_PrintAyar_Pr.Properties.DataSource = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad,Pkod_Printer from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '19' order by Pkod_Kod");
            look_PrintAyar_Pr.Properties.DisplayMember = "Pkod_Ad";
            look_PrintAyar_Pr.Properties.ValueMember = "Pkod_Kod";

            look_PrintAyar_dep.Properties.DataSource = dt2;
            look_PrintAyar_dep.Properties.DisplayMember = "Kodlar_Ad";
            look_PrintAyar_dep.Properties.ValueMember = "Kodlar_Kod";

            DataTable dt6 = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = '08'   order by Kodlar_Kod");

            dt6.Rows.Add("KSM", "Kasa Makbuz Printer");
            dt6.Rows.Add("PKT", "Paket Printer");
            dt6.Rows.Add("HES", "Hesap Printer");
            dt6.Rows.Add("ADI", "Adisyon Printer");
            dt6.Rows.Add("FAT", "Fatura Printer");
            dt6.Rows.Add("BKY", "ExtraFolio Printer");

            look_PrintAyar_AnaGrup.Properties.DataSource = dt6;
            look_PrintAyar_AnaGrup.Properties.DisplayMember = "Kodlar_Ad";
            look_PrintAyar_AnaGrup.Properties.ValueMember = "Kodlar_Kod";

            String pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                cmb_PrintAyar_Printer.Properties.Items.Add(pkInstalledPrinters);
                cmb_PrintAyar_AbuyerPr.Properties.Items.Add(pkInstalledPrinters);
                cmb_PrintAyar_AbuyerPr2.Properties.Items.Add(pkInstalledPrinters);
                cmb_PrintAyar_AbuyerPr3.Properties.Items.Add(pkInstalledPrinters);
                cmb_PrintAyar_AbuyerPr4.Properties.Items.Add(pkInstalledPrinters);
            }


        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Print_Hesap;


            Hesap_Yenile();
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Print_Adisyon;
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_OdemeKodlari;
            gridyenile_Odeme();

            DataTable dts = new DataTable();
            dts.Columns.Add("Kod", typeof(string));
            dts.Columns.Add("Ad", typeof(string));

            dts.Rows.Add("1", "Nakit");
            dts.Rows.Add("4", "Banka_Kartı");
            dts.Rows.Add("8", "YemekÇeki");
            dts.Rows.Add("0", "Indirim");

            Pkod_IWEPayment.Properties.DataSource = dts;
            Pkod_IWEPayment.Properties.DisplayMember = "Ad";
            Pkod_IWEPayment.Properties.ValueMember = "Kod";


            DataTable dtFisTipi = new DataTable();
            dtFisTipi.Columns.Add("Kod", typeof(string));
            dtFisTipi.Columns.Add("Ad", typeof(string));
            dtFisTipi.Rows.Add("V", "Hersey Dahil");
            dtFisTipi.Rows.Add("S", "Satis");
            dtFisTipi.Rows.Add("O", "Odenmez");
            dtFisTipi.Rows.Add("P", "Ikram");

            look_Odeme_FisTipi.Properties.DataSource = dtFisTipi;
            look_Odeme_FisTipi.Properties.DisplayMember = "Ad";
            look_Odeme_FisTipi.Properties.ValueMember = "Kod";


            if (Param.Tesis_Tipi == 0)
            {
                look_Odeme_OnbDepartman.Properties.DataSource = Fronttools.SelectTable("select Kodlar_Kod,Kodlar_Ad from Kodlar where Kodlar_Sinif = '01' and Kodlar_Ba = 'A'");
                look_Odeme_OnbDepartman.Properties.DisplayMember = "Kodlar_Ad";
                look_Odeme_OnbDepartman.Properties.ValueMember = "Kodlar_Kod";
            }

            Pkod_MuhasebeBorc.Properties.DataSource = Departman.MuhasebeKod_Getir(new DateTime(Param.Tarih.Year, 1, 1), new DateTime(Param.Tarih.Year, 12, 31), "0");
            Pkod_MuhasebeBorc.Properties.DisplayMember = "Plan_Ad";
            Pkod_MuhasebeBorc.Properties.ValueMember = "Plan_Kod";

            Pkod_MuhasebeAlacak.Properties.DataSource = Departman.MuhasebeKod_Getir(new DateTime(Param.Tarih.Year, 1, 1), new DateTime(Param.Tarih.Year, 12, 31), "0");
            Pkod_MuhasebeAlacak.Properties.DisplayMember = "Plan_Ad";
            Pkod_MuhasebeAlacak.Properties.ValueMember = "Plan_Kod";


            look_Ykkodu.Properties.DataSource = dbtools.SelectTable("select Convert(int,Pkod_Kod) as Pkod_Kod,Pkod_Ad from Pos_Kodlar where Pkod_Sinif = '91'");
            look_Ykkodu.Properties.DisplayMember = "Pkod_Ad";
            look_Ykkodu.Properties.ValueMember = "Pkod_Kod";

            Pkod_Dep.Properties.DataSource = dbtools.SelectTable("select Kodlar_Kod as Kodu,Kodlar_Ad as Adi from Stok_Kodlar where Kodlar_Sinif = 1");
            Pkod_Dep.Properties.DisplayMember = "Adi";
            Pkod_Dep.Properties.ValueMember = "Kodu";


            if (Departman.Kodlar_YS_Aktif)
            {
                DataTable dt = new DataTable();
                DataSet ds = iws.GetPaymentTypes();
                dt = ds.Tables[0];
                Pkod_YS_OdemeID.Properties.DataSource = dt;
                Pkod_YS_OdemeID.Properties.DisplayMember = "name";
                Pkod_YS_OdemeID.Properties.ValueMember = "Id";
            }
        }

        private void navBarItem9_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Entegre_Onb;

            look_EntOnb_Dep.Properties.DataSource = dt2;
            look_EntOnb_Dep.Properties.DisplayMember = "Kodlar_Ad";
            look_EntOnb_Dep.Properties.ValueMember = "Kodlar_Kod";

            look_EntOnbUrun.Properties.DataSource = dt3;
            look_EntOnbUrun.Properties.DisplayMember = "Kodlar_Ad";
            look_EntOnbUrun.Properties.ValueMember = "Kodlar_Kod";
        }

        private void navBarItem10_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Entegre_Cost;


            look_EntCost_Departman.Properties.DataSource = dt2;
            look_EntCost_Departman.Properties.DisplayMember = "Kodlar_Ad";
            look_EntCost_Departman.Properties.ValueMember = "Kodlar_Kod";


            look_EntCost_Grup.Properties.DataSource = dt3;
            look_EntCost_Grup.Properties.DisplayMember = "Kodlar_Ad";
            look_EntCost_Grup.Properties.ValueMember = "Kodlar_Kod";

            DataTable dt5 = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Kod +' - '+ Kodlar_Ad AS Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif ='01' and Kodlar_Anadepo = 'False' order by Kodlar_Kod");
            look_EntCost_Departman3.Properties.DataSource = dt5;
            look_EntCost_Departman3.Properties.DisplayMember = "Kodlar_Ad";
            look_EntCost_Departman3.Properties.ValueMember = "Kodlar_Kod";
        }

        private void navBarItem11_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Masa_Tanim;

            DataTable dt1 = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Kod +' - '+ Kodlar_Ad AS Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif ='01' and Kodlar_Anadepo = 'False' and Kodlar_Satis = 'True' order by Kodlar_Kod");
            look_MasaTan_Departman.Properties.DataSource = dt1;
            look_MasaTan_Departman.Properties.DisplayMember = "Kodlar_Ad";
            look_MasaTan_Departman.Properties.ValueMember = "Kodlar_Kod";


            txtParcaliDepartmanSec.Properties.DataSource = dt1;
            txtParcaliDepartmanSec.Properties.DisplayMember = "Kodlar_Ad";
            txtParcaliDepartmanSec.Properties.ValueMember = "Kodlar_Kod";


        }

        private void navBarItem12_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Masa_Konum;


            look_MasaKon_Departman.Properties.DataSource = dt2;
            look_MasaKon_Departman.Properties.DisplayMember = "Kodlar_Ad";
            look_MasaKon_Departman.Properties.ValueMember = "Kodlar_Kod";
        }

        private void navBarItem13_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Cari_Tanim;
            gridyenile_Cari();
            txt_Cari_Kod.Focus();
        }

        private void navBarItem14_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Cari_Hesap;

            date_CariHes.DateTime = DateTime.Now.Date;

            look_CariHes_Odeme.Properties.DataSource = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad,Pkod_Ozelkod from Pos_Kodlar where Pkod_Sinif = '11' and Pkod_Ozelkod <> '5' order by Pkod_Kod");
            look_CariHes_Odeme.Properties.DisplayMember = "Pkod_Ad";
            look_CariHes_Odeme.Properties.ValueMember = "Pkod_Kod";
        }

        private void navBarItem15_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Happy_Hour;


            look_HH_Departman5.Properties.DataSource = dt2;
            look_HH_Departman5.Properties.DisplayMember = "Kodlar_Ad";
            look_HH_Departman5.Properties.ValueMember = "Kodlar_Kod";
        }

        private void navBarItem16_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Kullanici_Ayarlari;
            Kul_comboyenile();
        }

        private void navBarItem17_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_MacPrint;

            String pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                cmb_MacPr_Printer.Properties.Items.Add(pkInstalledPrinters);
            }

            txt_MacPr_Mac.EditValue = dbtools.MacAdresi();


            DataTable dt6 = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = '08'   order by Kodlar_Kod");

            dt6.Rows.Add("KSM", "Kasa Makbuz Printer");
            dt6.Rows.Add("PKT", "Paket Printer");
            dt6.Rows.Add("HES", "Hesap Printer");
            dt6.Rows.Add("ADI", "Adisyon Printer");
            dt6.Rows.Add("FAT", "Fatura Printer");

            look_MacPr_Anagrup.Properties.DataSource = dt6;
            look_MacPr_Anagrup.Properties.DisplayMember = "Kodlar_Ad";
            look_MacPr_Anagrup.Properties.ValueMember = "Kodlar_Kod";

            gridyenile_MacPr();
        }

        private void navBarItem18_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Mail;
            gridyenile_Mail();
        }

        private void navBarItem19_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Pda;
            gridyenile_Pda();
        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Posta;

            look_Posta_Dep.Properties.DataSource = dt2;
            look_Posta_Dep.Properties.DisplayMember = "Kodlar_Ad";
            look_Posta_Dep.Properties.ValueMember = "Kodlar_Kod";

            gridyenile_Posta();
        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_PrinterTanim;

            String pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                cmb_Tanim_Pr.Properties.Items.Add(pkInstalledPrinters);
                cmb_Tanim_Ek1.Properties.Items.Add(pkInstalledPrinters);
                cmb_Tanim_Ek2.Properties.Items.Add(pkInstalledPrinters);
                cmb_Tanim_Ek3.Properties.Items.Add(pkInstalledPrinters);
            }

            gridyenile_PrintTanim();
        }


        private void navBarItem20_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            receteAcYeni();
        }

        public void receteAcYeni()
        {
            try
            {
                Rmosback.dbtools.conn = dbtools.conn;
                Rmosback.Classes.Constants.cnnBack = dbtools.conn.ConnectionString;
                Rmosback.Classes.Constants.cnnRmosMuh = Rmosmuh.conn.ConnectionString;

                Rmosback.dbtools.Kullanici_Kodu = User.U_BackUser;
                Rmosback.Classes.Constants.KullaniciKod = User.U_BackUser;

                Rmosback.Classes.Constants.MuhSirketDb = dbtools.database;

                Rmosback.Service.RmosMuh.SirketService sirketService = new Rmosback.Service.RmosMuh.SirketService();
                Rmosback.Classes.Constants.MuhSirketKod = sirketService.GetAll().Where(x => x.Sirket_Database.ToUpper() == dbtools.database.ToUpper()).FirstOrDefault().Sirket_Kod;


                Rmosback.Cst_Recete rec = new Rmosback.Cst_Recete();
                rec.ShowDialog();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "receteAc", "Kullanıcı Tanımlarından Back Kullanıcısını Seçin !", ex);
            }
        }

        public void stokParametreAcYeni()
        {
            try
            {
                Rmosback.dbtools.conn = dbtools.conn;
                Rmosback.dbtools.Kullanici_Kodu = User.U_BackUser;

                Rmosback.Classes.Constants.cnnBack = dbtools.conn.ConnectionString;
                Rmosback.Classes.Constants.cnnRmosMuh = Rmosmuh.conn.ConnectionString;

                Rmosback.Classes.Constants.KullaniciKod = User.U_BackUser;
                Rmosback.Classes.Constants.MuhSirketDb = dbtools.database;

                Rmosback.Service.RmosMuh.SirketService sirketService = new Rmosback.Service.RmosMuh.SirketService();
                //MessageBox.Show("Test1");
                Rmosback.Classes.Constants.MuhSirketKod = sirketService.GetAll().Where(x => x.Sirket_Database.ToUpper() == dbtools.database.ToUpper()).FirstOrDefault().Sirket_Kod;
                //MessageBox.Show("Test2");
                Rmosback.Kodlar kod = new Rmosback.Kodlar();
                kod.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA " + ex.Message);
            }
        }


        public void receteAc() // eski
        {
            try
            {
                Rmosback.dbtools.conn = dbtools.conn;
                Rmosback.Classes.Constants.cnnBack = dbtools.conn.ConnectionString;
                Rmosback.Classes.Constants.cnnRmosMuh = Rmosmuh.conn.ConnectionString;
                Rmosback.Service.RmosMuh.SirketService sirketService = new Rmosback.Service.RmosMuh.SirketService();
                Rmosback.Classes.Constants.MuhSirketKod = sirketService.GetAll().Where(x => x.Sirket_Database == dbtools.database).FirstOrDefault().Sirket_Kod;
                Rmosback.dbtools.Kullanici_Kodu = User.U_BackUser;
                Rmosback.Classes.Constants.KullaniciKod = User.U_BackUser;
                Rmosback.Cst_Recete rec = new Rmosback.Cst_Recete();
                rec.ShowDialog();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "receteAc", "Kullanıcı Tanımlarından Back Kullanıcısını Seçin !", ex);
            }
        }

        private void navBarItem23_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                string pos_user = "POS";
                string pos_sifre = "19830126";
                string pos_sirket = Departman.MKodlar_P_SirketKodu;
                string form = "Extra";

                Directory.SetCurrentDirectory(Param.Param_FrontPath);

                System.Diagnostics.ProcessStartInfo ps = new ProcessStartInfo();
                ps.Arguments = "\"" + pos_user + "\" \"" + pos_sifre + "\" \"" + pos_sirket + "\" \"" + form + "\"";
                ps.FileName = Param.Param_FrontPath + "\\" + "RmosFront.exe";
                System.Diagnostics.Process p = new Process();
                p.StartInfo = ps;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void navBarItem24_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                string pos_user = "POS";
                string pos_sifre = "19830126";
                string pos_sirket = Departman.MKodlar_P_SirketKodu;
                string form = "Folio";

                Directory.SetCurrentDirectory(Param.Param_FrontPath);

                System.Diagnostics.ProcessStartInfo ps = new ProcessStartInfo();
                ps.Arguments = "\"" + pos_user + "\" \"" + pos_sifre + "\" \"" + pos_sirket + "\" \"" + form + "\"";
                ps.FileName = Param.Param_FrontPath + "\\" + "RmosFront.exe";
                System.Diagnostics.Process p = new Process();
                p.StartInfo = ps;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void navBarItem21_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Kasagc;

            gridyenile_Kasagc();
        }

        private void navBarItem22_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_AciklamaItem;

            gridyenile_AciklamaItem();
        }

        private void navBarItem25_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_IlTanim;

            gridyenile_Il();
        }

        private void navBarItem26_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Ilce;

            look_Ilce_Il.Properties.DataSource = dbtools.SelectTable("select * from Pos_Adres where Adres_Sinif = '24' order by Adres_Ad");
            look_Ilce_Il.Properties.DisplayMember = "Adres_Ad";
            look_Ilce_Il.Properties.ValueMember = "Adres_Kod";
        }

        private void navBarItem27_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Mahalle;

            look_Mah_Il.Properties.DataSource = dbtools.SelectTable("select * from Pos_Adres where Adres_Sinif = '24' order by Adres_Ad");
            look_Mah_Il.Properties.DisplayMember = "Adres_Ad";
            look_Mah_Il.Properties.ValueMember = "Adres_Kod";
        }

        private void navBarItem28_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_Sube;

            gridyenile_Sube();
        }

        private void navBarItem29_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = tab_SubeAdres;

            look_SubeAdres_Sube.Properties.DataSource = dbtools.SelectTable("select * from Pos_kodlar where Pkod_Sinif = '27' order by Pkod_Kod");
            look_SubeAdres_Sube.Properties.DisplayMember = "Pkod_Ad";
            look_SubeAdres_Sube.Properties.ValueMember = "Pkod_Kod";

            gridyenile_SubeAdres();
        }
        #endregion

        #region Mail Parametreleri
        private void gridyenile_Mail()
        {
            DataTable dtMail = dbtools.SelectTable("select ISNULL(Mail_Gonder,0) as Mail_Gonder,Mail_Isim,Mail_Adres,Mail_Parola,Mail_Host,Mail_Port,isnull(Mail_SSL,0) as Mail_SSL, "
                            + " Mail_Alici1,Mail_Alici2,Mail_Alici3,Mail_Alici4,Mail_Alici5, "
                            + " isnull(Mail_Odeme_Tip,0) as Mail_Odeme_Tip,isnull(Mail_Servis_Paylari,0) as Mail_Servis_Paylari,isnull(Mail_Cari_Ozet,0) as Mail_Cari_Ozet, "
                            + " isnull(Mail_Odenmez_Ozet,0) as Mail_Odenmez_Ozet,isnull(Mail_Malz_Ozet,0) as Mail_Malz_Ozet,isnull(Mail_Ana_Ozet,0) as Mail_Ana_Ozet, "
                            + " isnull(Mail_Alt_Ozet,0) as Mail_Alt_Ozet,isnull(Mail_Iptal_Ozet,0) as Mail_Iptal_Ozet, "
                            + " Mail_Alici6,Mail_Alici7,Mail_Alici8,Mail_Alici9,Mail_Alici10 from Pos_Mail WITH(NOLOCK) where Mail_Id = 1");
            if (dtMail.Rows.Count > 0)
            {
                chk_Mailgonder.Checked = Convert.ToBoolean(dtMail.Rows[0]["Mail_Gonder"]);

                txt_Mail_Isim.Text = Convert.ToString(dtMail.Rows[0]["Mail_Isim"]);
                txt_Mail_Adres.Text = Convert.ToString(dtMail.Rows[0]["Mail_Adres"]);
                txt_Mail_Parola.Text = Convert.ToString(dtMail.Rows[0]["Mail_Parola"]);
                txt_Mail_Host.Text = Convert.ToString(dtMail.Rows[0]["Mail_Host"]);
                txt_Mail_Port.Text = Convert.ToString(dtMail.Rows[0]["Mail_Port"]);
                chk_Mail_SSL.Checked = Convert.ToBoolean(dtMail.Rows[0]["Mail_SSL"]);

                txt_Mail_Alici1.Text = Convert.ToString(dtMail.Rows[0]["Mail_Alici1"]);
                txt_Mail_Alici2.Text = Convert.ToString(dtMail.Rows[0]["Mail_Alici2"]);
                txt_Mail_Alici3.Text = Convert.ToString(dtMail.Rows[0]["Mail_Alici3"]);
                txt_Mail_Alici4.Text = Convert.ToString(dtMail.Rows[0]["Mail_Alici4"]);
                txt_Mail_Alici5.Text = Convert.ToString(dtMail.Rows[0]["Mail_Alici5"]);

                chk_Mail_Odeme.Checked = Convert.ToBoolean(dtMail.Rows[0]["Mail_Odeme_Tip"]);
                chk_Mail_Servis.Checked = Convert.ToBoolean(dtMail.Rows[0]["Mail_Servis_Paylari"]);
                chk_Mail_Cari.Checked = Convert.ToBoolean(dtMail.Rows[0]["Mail_Cari_Ozet"]);
                chk_Mail_Odenmez.Checked = Convert.ToBoolean(dtMail.Rows[0]["Mail_Odenmez_Ozet"]);
                chk_Mail_Malzeme.Checked = Convert.ToBoolean(dtMail.Rows[0]["Mail_Malz_Ozet"]);
                chk_Mail_Anagrup.Checked = Convert.ToBoolean(dtMail.Rows[0]["Mail_Ana_Ozet"]);
                chk_Mail_Altgrup.Checked = Convert.ToBoolean(dtMail.Rows[0]["Mail_Alt_Ozet"]);
                chk_Mail_Iptal.Checked = Convert.ToBoolean(dtMail.Rows[0]["Mail_Iptal_Ozet"]);

                txt_Mail_Alici6.Text = Convert.ToString(dtMail.Rows[0]["Mail_Alici6"]);
                txt_Mail_Alici7.Text = Convert.ToString(dtMail.Rows[0]["Mail_Alici7"]);
                txt_Mail_Alici8.Text = Convert.ToString(dtMail.Rows[0]["Mail_Alici8"]);
                txt_Mail_Alici9.Text = Convert.ToString(dtMail.Rows[0]["Mail_Alici9"]);
                txt_Mail_Alici10.Text = Convert.ToString(dtMail.Rows[0]["Mail_Alici10"]);
            }
        }

        private void btn_Mail_Kaydet_Click(object sender, EventArgs e)
        {
            int sayac = Convert.ToInt32(dbtools.DegerGetir("select count(*) from Pos_Mail WITH(NOLOCK) where Mail_Id = 1"));
            if (sayac > 0)
            {
                dbtools.execcmd("UPDATE    Pos_Mail SET Mail_Gonder = '" + Convert.ToBoolean(chk_Mailgonder.Checked) + "', Mail_Isim ='" + txt_Mail_Isim.Text + "', Mail_Adres ='" + txt_Mail_Adres.Text + "',  "
                                + "     Mail_Parola ='" + txt_Mail_Parola.Text + "', Mail_Host ='" + txt_Mail_Host.Text + "', Mail_Port ='" + txt_Mail_Port.Text + "', Mail_SSL ='" + Convert.ToBoolean(chk_Mail_SSL.Checked) + "', "
                                + "     Mail_Alici1 ='" + txt_Mail_Alici1.Text + "', Mail_Alici2 ='" + txt_Mail_Alici2.Text + "', Mail_Alici3 ='" + txt_Mail_Alici3.Text + "', "
                                + "     Mail_Alici4 ='" + txt_Mail_Alici4.Text + "', Mail_Alici5 ='" + txt_Mail_Alici5.Text + "', "
                                + "     Mail_Odeme_Tip = '" + Convert.ToBoolean(chk_Mail_Odeme.Checked) + "',Mail_Servis_Paylari = '" + Convert.ToBoolean(chk_Mail_Servis.Checked) + "', "
                                + "     Mail_Cari_Ozet = '" + Convert.ToBoolean(chk_Mail_Cari.Checked) + "',Mail_Odenmez_Ozet = '" + Convert.ToBoolean(chk_Mail_Odenmez.Checked) + "', "
                                + "     Mail_Malz_Ozet = '" + Convert.ToBoolean(chk_Mail_Malzeme.Checked) + "',Mail_Ana_Ozet = '" + Convert.ToBoolean(chk_Mail_Anagrup.Checked) + "', "
                                + "     Mail_Alt_Ozet = '" + Convert.ToBoolean(chk_Mail_Altgrup.Checked) + "',Mail_Iptal_Ozet = '" + Convert.ToBoolean(chk_Mail_Iptal.Checked) + "', "
                                + "     Mail_Alici6 = '" + txt_Mail_Alici6.Text + "', Mail_Alici7 = '" + txt_Mail_Alici7.Text + "', Mail_Alici8 = '" + txt_Mail_Alici8.Text + "', "
                                + "     Mail_Alici9 = '" + txt_Mail_Alici9.Text + "', Mail_Alici10 = '" + txt_Mail_Alici10.Text + "' "
                                + " WHERE Mail_Id = 1");
            }
            else
            {
                dbtools.execcmd("INSERT INTO Pos_Mail (Mail_Id, Mail_Gonder, Mail_Isim, Mail_Adres, Mail_Parola, Mail_Host, Mail_Port, Mail_SSL, "
                                + "                         Mail_Alici1, Mail_Alici2, Mail_Alici3, Mail_Alici4, Mail_Alici5, "
                                + "                         Mail_Odeme_Tip,Mail_Servis_Paylari,Mail_Cari_Ozet,Mail_Odenmez_Ozet, "
                                + "                         Mail_Malz_Ozet,Mail_Ana_Ozet,Mail_Alt_Ozet,Mail_Iptal_Ozet, "
                                + "                         Mail_Alici6, Mail_Alici7, Mail_Alici8, Mail_Alici9, Mail_Alici10) "
                                + " VALUES     (1,'" + Convert.ToBoolean(chk_Mailgonder.Checked) + "','" + txt_Mail_Isim.Text + "','" + txt_Mail_Adres.Text + "','" + txt_Mail_Parola.Text + "','" + txt_Mail_Host.Text + "','" + txt_Mail_Port.Text + "','" + Convert.ToBoolean(chk_Mail_SSL.Checked) + "', "
                                + " '" + txt_Mail_Alici1.Text + "','" + txt_Mail_Alici2.Text + "','" + txt_Mail_Alici3.Text + "','" + txt_Mail_Alici4.Text + "','" + txt_Mail_Alici5.Text + "', "
                                + " '" + Convert.ToBoolean(chk_Mail_Odeme.Checked) + "','" + Convert.ToBoolean(chk_Mail_Servis.Checked) + "','" + Convert.ToBoolean(chk_Mail_Cari.Checked) + "','" + Convert.ToBoolean(chk_Mail_Odenmez.Checked) + "', "
                                + " '" + Convert.ToBoolean(chk_Mail_Malzeme.Checked) + "','" + Convert.ToBoolean(chk_Mail_Anagrup.Checked) + "','" + Convert.ToBoolean(chk_Mail_Altgrup.Checked) + "','" + Convert.ToBoolean(chk_Mail_Iptal.Checked) + "', "
                                + " '" + txt_Mail_Alici6.Text + "','" + txt_Mail_Alici7.Text + "','" + txt_Mail_Alici8.Text + "','" + txt_Mail_Alici9.Text + "','" + txt_Mail_Alici10.Text + "')");
            }
        }

        private void btn_Mail_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Parametre Tanımları
        private void gridyenile_Parametre()
        {
            DataTable dt = dbtools.SelectTable("select isnull(Param_Onburo,0) as Param_Onburo2,isnull(Param_Cost,0) as Param_Cost2,isnull(Param_Muh,0) as Param_Muh2, "
                    + " isnull(Param_Tar_Nere,0) as Param_Tar_Nere2,isnull(Param_Kur_Nere,0) as Param_Kur_Nere2,isnull(Param_Limittip,0) as Param_Limittip2, "
                    + " isnull(Param_Balance,0) as Param_Balance2,isnull(Param_Fis_Dovizli,0) as Param_Fis_Dovizli2,isnull(Param_Tesistip,0) as Param_Tesistip2, "
                    + " isnull(Param_Tarih,getdate()) as Param_Tarih2,Param_Dovizkod,isnull(Param_Kur,0) as Param_Kur2,isnull(Param_Yansit,0) as Param_Yansit2, "
                    + " Param_Discount,isnull(Param_Kartla_Giris,0) as Param_Kartla_Giris2,isnull(Param_CalismaSekli,0) as Param_CalismaSekli2, "
                    + " Param_TesisAdi,isnull(Param_Miktar,9) as Param_Miktar2,Param_Doviz_Turu,isnull(Param_Miktar_Duzelt,0) as Param_Miktar_Duzelt2, "
                    + " isnull(Param_Masa_Refresh,0) as Param_Masa_Refresh2,isnull(Param_Yarim_Tam,0) as Param_Yarim_Tam2,isnull(Param_HH_Ind,0) as Param_HH_Ind2, "
                    + " isnull(Param_Fis_Urungrup,0) as Param_Fis_Urungrup2,Param_FullComp,isnull(Param_Rec_Bas,0) as Param_Rec_Bas,isnull(Param_Rec_Hane,0) as Param_Rec_Hane, "
                    + " isnull(Param_KG_Bas,0) as Param_KG_Bas, isnull(Param_KG_Hane,0) as Param_KG_Hane,isnull(Param_GR_Bas,0) as Param_GR_Bas,isnull(Param_GR_Hane,0) as Param_GR_Hane, "
                    + " isnull(Param_Masa_Geri,0) as Param_Masa_Geri2,Param_Rapor_Design,isnull(Param_Pda_Kartsor,0) as Param_Pda_Kartsor2,isnull(Param_Anagrup_Cikmasin,0) as Param_Anagrup_Cikmasin2, "
                    + " isnull(Param_Satis_YD,0) as Param_Satis_YD2, isnull(Param_Paket_YD ,0) as Param_Paket_YD2,isnull(Param_Dep_Fiyat,0) as Param_Dep_Fiyat2, "
                    + " isnull(Param_Masatr_Uyari,0) as Param_Masatr_Uyari2,Param_Bindirim,isnull(Param_Printer_Tanim,0) as Param_Printer_Tanim2,isnull(Param_Hesap_Disable,0) as Param_Hesap_Disable2,Param_Yuvarla, "
                    + " isnull(Param_Yuv_Sayi,0) as Param_Yuv_Sayi2,isnull(Param_Hsifir_Ikram,0) as Param_Hsifir_Ikram2,isnull(Param_Adispr_Uyari,0) as Param_Adispr_Uyari2,isnull(Param_Paketci_Sor,0) as Param_Paketci_Sor2, "
                    + " isnull(Param_Gunsonu_Aktar,0) as Param_Gunsonu_Aktar2,isnull(Param_Masa_Garson,0) as Param_Masa_Garson2,isnull(Param_Extre_Cikmasin,0) as Param_Extre_Cikmasin2,Param_Kart_Yoksay, "
                    + " isnull(Param_Adis_Doviz,0) as Param_Adis_Doviz2,isnull(Param_Tum_Paket,0) as Param_Tum_Paket2,isnull(Param_Siparis_Uyari,0) as Param_Siparis_Uyari2,ISNULL(Param_Hesap_Kilit,0) as Param_Hesap_Kilit2, "
                    + " isnull(Param_Masaacan_Garson,0) as Param_Masaacan_Garson2, "
                    + " Param_Adres1,Param_Adres2,Param_Adres3,Param_Adres4,Param_Adres5,Param_Fis_Aciklama,ISNULL(NULLIF(Param_Masa_Size,''),'90;45') as Param_Masa_Size, "
                    + " ISNULL(Param_Sonmasa,0) as Param_Sonmasa,ISNULL(Param_Sonmasa_Renk,'#FF4500') as Param_Sonmasa_Renk,ISNULL(Param_Paket_Form,0) as Param_Paket_Form2, "
                    + " ISNULL(Param_Paket_Kisi,0) as Param_Paket_Kisi2,isnull(Param_Hesap_DovizOzet,0) as Param_Hesap_DovizOzet2,ISNULL(Param_Hesap_DovizOzetToplam,0) as Param_Hesap_DovizOzetToplam2,Param_FrontPath, "
                    + " ISNULL(NULLIF(Param_SikKullanSize,''),'90;45') as Param_SikKullanSize,isnull(tipboxReceteKod,0) as tipboxReceteKod "
                    + " from Pos_Param where Param_Id = '1' ");

            DataTable dtMac = dbtools.SelectTable("SELECT  isnull(P_Tek,0) as P_Tek, P_Mac, P_Dep, ISNULL(P_Sabitkonum,0) as P_Sabitkonum, P_Sabitkonumkodu  FROM  Rmosmuh.dbo.P_Bilg WHERE P_Mac='" + dbtools.MacAdresi() + "'");

            if (dt.Rows.Count > 0)
            {
                chk_Prm_Onburo.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Onburo2"]);
                chk_Prm_Cost.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Cost2"]);
                chk_Prm_Muh.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Muh2"]);
                rdo_Prm_Tarih_Nereden.SelectedIndex = Convert.ToInt32(dt.Rows[0]["Param_Tar_Nere2"]);
                rdo_Prm_Kur_Nereden.SelectedIndex = Convert.ToInt32(dt.Rows[0]["Param_Kur_Nere2"]);
                rdo_Prm_Limit_Tipi.SelectedIndex = Convert.ToInt32(dt.Rows[0]["Param_Limittip2"]);
                rdo_Prm_Balance.SelectedIndex = Convert.ToInt32(dt.Rows[0]["Param_Balance2"]);
                rdo_Prm_Fis_Dovizli.SelectedIndex = Convert.ToInt32(dt.Rows[0]["Param_Fis_Dovizli2"]);
                rdo_Prm_Tesis_Tipi.SelectedIndex = Convert.ToInt32(dt.Rows[0]["Param_Tesistip2"]);
                date_Prm_Tarih.DateTime = Convert.ToDateTime(dt.Rows[0]["Param_Tarih2"]);
                look_Prm_Kurkodu.EditValue = Convert.ToString(dt.Rows[0]["Param_Dovizkod"]);
                txt_Prm_Kur.EditValue = Convert.ToDecimal(dt.Rows[0]["Param_Kur2"]);
                chk_Prm_Yansit.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Yansit2"]);
                txt_Prm_Discount.EditValue = Convert.ToString(dt.Rows[0]["Param_Discount"]);
                chk_Prm_Kul_Kart.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Kartla_Giris2"]);
                rdo_Prm_Calisma.SelectedIndex = Convert.ToInt32(dt.Rows[0]["Param_CalismaSekli2"]);
                txt_Prm_Tesis_Adi.EditValue = Convert.ToString(dt.Rows[0]["Param_TesisAdi"]);
                spn_Prm_Miktar.Value = Convert.ToInt32(dt.Rows[0]["Param_Miktar2"]);
                chk_Prm_MiktarDuzelt.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Miktar_Duzelt2"]);
                spin_Prm_Refresh.EditValue = Convert.ToInt32(dt.Rows[0]["Param_Masa_Refresh2"]);
                chk_Prm_YarimTam.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Yarim_Tam2"]);
                chk_Prm_HH.Checked = Convert.ToBoolean(dt.Rows[0]["Param_HH_Ind2"]);
                chk_Prm_UrunGrup.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Fis_Urungrup2"]);
                txt_Prm_Fullcomp.Text = Convert.ToString(dt.Rows[0]["Param_FullComp"]);

                spn_Prm_Recbas.EditValue = Convert.ToInt32(dt.Rows[0]["Param_Rec_Bas"]);
                spn_Prm_Rechane.EditValue = Convert.ToInt32(dt.Rows[0]["Param_Rec_Hane"]);
                spn_Prm_KGbas.EditValue = Convert.ToInt32(dt.Rows[0]["Param_KG_Bas"]);
                spn_Prm_KGhane.EditValue = Convert.ToInt32(dt.Rows[0]["Param_KG_Hane"]);
                spn_Prm_GRbas.EditValue = Convert.ToInt32(dt.Rows[0]["Param_GR_Bas"]);
                spn_Prm_GRhane.EditValue = Convert.ToInt32(dt.Rows[0]["Param_GR_Hane"]);

                chk_Prm_MasaGeri.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Masa_Geri2"]);
                look_Param_Rapor_Design.ItemIndex = look_Param_Rapor_Design.Properties.GetDataSourceRowIndex("Diz_Id", dt.Rows[0]["Param_Rapor_Design"]);
                //look_Param_Rapor_Design.EditValue = Convert.ToString(dt.Rows[0]["Param_Rapor_Design"]);
                chk_Prm_PdaKart.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Pda_Kartsor2"]);
                chk_Prm_Anagrup_Cikmasin.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Anagrup_Cikmasin2"]);

                chk_Prm_SatisYD_Aktif.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Satis_YD2"]);
                chk_Prm_PaketYD_Aktif.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Paket_YD2"]);
                chk_Prm_DepFiyat.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Dep_Fiyat2"]);
                chk_Prm_Masatr_Uyari.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Masatr_Uyari2"]);
                look_Prm_Bindirim.EditValue = Convert.ToString(dt.Rows[0]["Param_Bindirim"]);
                lookUpEdit_tipbox.EditValue = Convert.ToString(dt.Rows[0]["tipboxReceteKod"]);

                chk_Prm_PrinterTanim.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Printer_Tanim2"]);
                chk_Prm_HesapDisable.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Hesap_Disable2"]);
                look_Prm_Yuvarla.EditValue = Convert.ToString(dt.Rows[0]["Param_Yuvarla"]);
                spn_Prm_Ysayi.EditValue = Convert.ToInt32(dt.Rows[0]["Param_Yuv_Sayi2"]);
                chk_Prm_HsifirIkram.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Hsifir_Ikram2"]);
                chk_Prm_AdisyonprUyari.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Adispr_Uyari2"]);
                chk_Prm_Paketcisor.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Paketci_Sor2"]);
                chk_Prm_GunsonuAktar.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Gunsonu_Aktar2"]);
                chk_Prm_MasaGarson.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Masa_Garson2"]);
                chk_Prm_Extre_Cikmasin.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Extre_Cikmasin2"]);

                txt_Prm_KartYoksay.Text = Convert.ToString(dt.Rows[0]["Param_Kart_Yoksay"]);
                chk_Prm_AdisDoviz.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Adis_Doviz2"]);
                chk_Prm_Tum_Paket.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Tum_Paket2"]);
                chk_Prm_Siparis_Uyari.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Siparis_Uyari2"]);
                chk_Prm_Hesapkilit.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Hesap_Kilit2"]);
                chk_Prm_MasaacanGarson.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Masaacan_Garson2"]);

                txt_Adres1.Text = Convert.ToString(dt.Rows[0]["Param_Adres1"]);
                txt_Adres2.Text = Convert.ToString(dt.Rows[0]["Param_Adres2"]);
                txt_Adres3.Text = Convert.ToString(dt.Rows[0]["Param_Adres3"]);
                txt_Adres4.Text = Convert.ToString(dt.Rows[0]["Param_Adres4"]);
                txt_Adres5.Text = Convert.ToString(dt.Rows[0]["Param_Adres5"]);

                txt_Prm_FisAciklama.Text = Convert.ToString(dt.Rows[0]["Param_Fis_Aciklama"]);
                txt_Prm_MasaSize.Text = Convert.ToString(dt.Rows[0]["Param_Masa_Size"]);

                chk_Prm_Sonmasa.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Sonmasa"]);
                color_Prm_Sonmasa.Color = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dt.Rows[0]["Param_Sonmasa_Renk"]));
                chk_Prm_PaketForm.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Paket_Form2"]);
                chk_Prm_PaketKisi.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Paket_Kisi2"]);
                chk_Hesap_DovizOzet.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Hesap_DovizOzet2"]);
                chk_Hesap_DovizOzetToplam.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Hesap_DovizOzetToplam2"]);
                txt_Prm_FrontPath.Text = Convert.ToString(dt.Rows[0]["Param_FrontPath"]);
                txt_Prm_SikKullanSize.Text = Convert.ToString(dt.Rows[0]["Param_SikKullanSize"]);




            }
            if (dtMac.Rows.Count > 0)
            {
                chk_Prm_TekDep.Checked = Convert.ToBoolean(dtMac.Rows[0]["P_Tek"]);
                txt_Prm_TekDep.Text = Convert.ToString(dtMac.Rows[0]["P_Dep"]);
                chk_Prm_Konum.Checked = Convert.ToBoolean(dtMac.Rows[0]["P_Sabitkonum"]);
                txt_Prm_Konum.Text = Convert.ToString(dtMac.Rows[0]["P_Sabitkonumkodu"]);
            }
        }

        private void raporyenile()
        {
            look_Param_Rapor_Design.Properties.DataSource = dbtools.Dizayn_Getir(User.P_Kod, "Raporlar", "");
            look_Param_Rapor_Design.Properties.DisplayMember = "Diz_Rapor";
            look_Param_Rapor_Design.Properties.ValueMember = "Diz_Id";
        }



        public void yuvarlaKaydet()
        {
            try
            {

                string dep = yuvarlamaDepartman.EditValue.ToString();
                decimal fiyat = Convert.ToDecimal(yuvarlamaFiyat.Text);
                string recete = yuvarlamaRecete.EditValue.ToString();


                bool depvarmi = false;
                foreach (var item in Main.ayarlar.yuvarlaModels)
                {
                    if (item.yuvarlamaDepartman.Equals(dep))
                    {
                        depvarmi = true;
                        item.yuvarlamaDepartman = dep;
                        item.yuvarlamaFiyat = fiyat;
                        item.yuvarlamaRecete = recete;
                    }
                }

                if (depvarmi == false)
                {
                    YuvarlaModel model = new YuvarlaModel();
                    model.yuvarlamaDepartman = dep;
                    model.yuvarlamaFiyat = fiyat;
                    model.yuvarlamaRecete = recete;
                    Main.ayarlar.yuvarlaModels.Add(model);
                }
                string newJson = JsonConvert.SerializeObject(Main.ayarlar.yuvarlaModels).Replace("'", "''");

                string query = "update ayarlar set ayarlar_value='" + newJson + "' where ayarlar_key='yuvarlama'";
                dbtools.execcmd(query);

            }
            catch (Exception ex)
            {
                //RHMesaj.MyMessageError(MyClass, "yuvarlaKaydet", "", ex);
            }
        }

        public void otoIndirimKaydet()
        {
            try
            {

                string dep = lookUpOtoIndirimDep.EditValue.ToString();
                bool aktif = checkOtoIndirimAktif.Checked;


                bool depvarmi = false;
                foreach (var item in Main.ayarlar.indirimModels)
                {
                    if (item.depKod.Equals(dep))
                    {
                        depvarmi = true;
                        item.depKod = dep;
                        item.aktif = aktif;
                    }
                }

                if (depvarmi == false)
                {
                    IndirimModel model = new IndirimModel();
                    model.depKod = dep;
                    model.aktif = aktif;
                    Main.ayarlar.indirimModels.Add(model);
                }
                string newJson = JsonConvert.SerializeObject(Main.ayarlar.indirimModels).Replace("'", "''");

                string query = "update ayarlar set ayarlar_value='" + newJson + "' where ayarlar_key='otomatikIndirim'";
                dbtools.execcmd(query);

            }
            catch (Exception ex)
            {
                //RHMesaj.MyMessageError(MyClass, "yuvarlaKaydet", "", ex);
            }
        }


        private void btn_Prm_Kaydet_Click(object sender, EventArgs e)
        {
            yuvarlaKaydet();
            otoIndirimKaydet();

            DataTable dt = dbtools.SelectTable("select * from Pos_Param where Param_Id = '1'");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Param (Param_Id, Param_TesisAdi,Param_Onburo,Param_Cost,Param_Muh,Param_CalismaSekli,Param_Tar_Nere,Param_Kur_Nere,Param_Limittip,Param_Balance,Param_Fis_Dovizli,"
                    + " Param_Tesistip,Param_Tarih,Param_Dovizkod,Param_Kur,Param_Yansit,Param_Discount,Param_Miktar,Param_Kartla_Giris,Param_Miktar_Duzelt,Param_Masa_Refresh,Param_Yarim_Tam,Param_HH_Ind,Param_Fis_Urungrup,Param_FullComp, "
                    + " Param_Rec_Bas,Param_Rec_Hane,Param_KG_Bas,Param_KG_Hane,Param_GR_Bas,Param_GR_Hane,Param_Masa_Geri,Param_Rapor_Design,Param_Pda_Kartsor,Param_Anagrup_Cikmasin, "
                    + " Param_Satis_YD,Param_Paket_YD,Param_Dep_Fiyat,Param_Masatr_Uyari,Param_Bindirim,Param_Printer_Tanim, "
                    + " Param_Hesap_Disable,Param_Yuvarla,Param_Yuv_Sayi,Param_Hsifir_Ikram,Param_Adispr_Uyari,Param_Paketci_Sor,Param_Gunsonu_Aktar, "
                    + " Param_Masa_Garson,Param_Extre_Cikmasin,Param_Kart_Yoksay,Param_Adis_Doviz,Param_Tum_Paket,Param_Siparis_Uyari,Param_Hesap_Kilit,Param_Masaacan_Garson, "
                    + " Param_Adres1,Param_Adres2,Param_Adres3,Param_Adres4,Param_Adres5,Param_Fis_Aciklama,Param_Masa_Size,Param_Sonmasa,Param_Sonmasa_Renk, "
                    + " Param_Paket_Form,Param_Paket_Kisi,Param_Hesap_DovizOzet,Param_Hesap_DovizOzetToplam,Param_FrontPath,Param_SikKullanSize,"
                    + " Param_ComPort,Param_BaudRate,Param_DataBits,Param_Parity,Param_StopBits,Param_FlowControl,Param_bSizeW,Param_bSizeH,Param_SatisArama,"
                    + " Param_HesapSor,Param_CariSor,Param_CallerID,Param_SiparisSayi,Param_AutoUpdate,Param_PaketSiparisPayi,Param_SubeGonder,Param_LimitFolio,Param_DirekLimitUyari, "
                    + " Param_OzelMasaSiralama,Param_HesapFisiDokum,Param_HspFontAlgilama,Param_AdisyonFolioAdi,Param_FullPos,Param_CikisKapa,Param_DirekAdisyonZor,Param_DirekAdisyonPrSor,Param_KGAlgilama,Param_ExtraFolioAcma,Param_SiparisAna,Param_iadeKontrol,Param_iadeLimit, Pos_HesapDkmRenk, Param_AdisyonDegis, Param_AdisyonIndAd,Param_SiparisTutar,Param_AnaEkranCiro,Param_MasaTakipCiro,Param_AcilisCekSil,Param_CariAdSoyad,Param_OdenmezAc, "
                    + " Param_SatirSil,Param_SatirSilUser,Param_MasaTakipMenu,Param_ParaUstuIngenico,Param_SatisTabloGonderi, Param_SatisTabloID, Param_SatisTabloAktif, Param_AcilistaMenu,Param_IngenicoSPR,Param_SiparisFisFont,Param_HizliSatisCekAc,Param_KartfGBCheckOut, "
                    + " Param_YeniHesapDkm,Param_YeniSiparisDkm,Param_OdaKrediCompOdenmez,Param_KurTransfer,Param_CallCenterPaket, "
                    + " Param_PaketDipTotal,Param_HesapKapamaAds,Param_HesapDkmAciklama,Param_OzelMasaRengi,Param_RezMasaRengi,Param_AndroGeriYazdir,Param_PaketKucukEkran,Param_GetirTest,Param_GetirOtomatikOnay,Param_SatisCikisButton,Param_nfcBarkodAktif,Param_ParcaliMasaAktif,yazdirilmamissiparis,masamusait,masatakiphesappasif,kisivegarsonbirkeresoraktif,satirsilfiscikmasinaktif,onburoikramsifiryazaktif,kartnoSayisi,cariindirimAktif,tipboxReceteKod )"

                    + " VALUES ( "
                    + " '1', '" + txt_Prm_Tesis_Adi.Text + "', '" + Convert.ToBoolean(chk_Prm_Onburo.Checked) + "', '" + Convert.ToBoolean(chk_Prm_Cost.Checked) + "', '" + Convert.ToBoolean(chk_Prm_Muh.Checked) + "','" + rdo_Prm_Calisma.SelectedIndex + "','" + rdo_Prm_Tarih_Nereden.SelectedIndex + "', "
                    + " '" + rdo_Prm_Kur_Nereden.SelectedIndex + "', '" + rdo_Prm_Limit_Tipi.SelectedIndex + "', '" + rdo_Prm_Balance.SelectedIndex + "','" + rdo_Prm_Fis_Dovizli.SelectedIndex + "','" + rdo_Prm_Tesis_Tipi.SelectedIndex + "', "
                    + " '" + date_Prm_Tarih.DateTime.Date + "','" + Convert.ToString(look_Prm_Kurkodu.EditValue) + "','" + txt_Prm_Kur.Text.Replace(",", ".") + "','" + Convert.ToBoolean(chk_Prm_Yansit.Checked) + "', "
                    + " '" + txt_Prm_Discount.EditValue + "','" + spn_Prm_Miktar.Value + "','" + Convert.ToBoolean(chk_Prm_Kul_Kart.Checked) + "', "
                    + " '" + Convert.ToBoolean(chk_Prm_MiktarDuzelt.Checked) + "','" + spin_Prm_Refresh.EditValue + "', "
                    + " '" + Convert.ToBoolean(chk_Prm_YarimTam.Checked) + "','" + Convert.ToBoolean(chk_Prm_HH.Checked) + "','" + Convert.ToBoolean(chk_Prm_UrunGrup.Checked) + "','" + txt_Prm_Fullcomp.Text + "', "
                    + " '" + Convert.ToInt32(spn_Prm_Recbas.EditValue).ToString() + "','" + Convert.ToInt32(spn_Prm_Rechane.EditValue).ToString() + "','" + Convert.ToInt32(spn_Prm_KGbas.EditValue).ToString() + "','" + Convert.ToInt32(spn_Prm_KGhane.EditValue).ToString() + "', "
                    + " '" + Convert.ToInt32(spn_Prm_GRbas.EditValue).ToString() + "','" + Convert.ToInt32(spn_Prm_GRhane.EditValue).ToString() + "','" + Convert.ToBoolean(chk_Prm_MasaGeri.Checked) + "','" + Convert.ToString(look_Param_Rapor_Design.EditValue) + "','" + Convert.ToBoolean(chk_Prm_PdaKart.Checked) + "','" + Convert.ToBoolean(chk_Prm_Anagrup_Cikmasin.Checked) + "', "
                    + " '" + Convert.ToBoolean(chk_Prm_SatisYD_Aktif.Checked) + "','" + Convert.ToBoolean(chk_Prm_PaketYD_Aktif.Checked) + "','" + Convert.ToBoolean(chk_Prm_DepFiyat.Checked) + "','" + Convert.ToBoolean(chk_Prm_Masatr_Uyari.Checked) + "','" + Convert.ToString(look_Prm_Bindirim.EditValue) + "','" + Convert.ToBoolean(chk_Prm_PrinterTanim.Checked) + "', "
                    + " '" + Convert.ToBoolean(chk_Prm_HesapDisable.Checked) + "','" + Convert.ToString(look_Prm_Yuvarla.EditValue) + "','" + Convert.ToInt32(spn_Prm_Ysayi.EditValue) + "','" + Convert.ToBoolean(chk_Prm_HsifirIkram.Checked) + "','" + Convert.ToBoolean(chk_Prm_AdisyonprUyari.Checked) + "','" + Convert.ToBoolean(chk_Prm_Paketcisor.Checked) + "','" + Convert.ToBoolean(chk_Prm_GunsonuAktar.Checked) + "', "
                    + " '" + Convert.ToBoolean(chk_Prm_MasaGarson.Checked) + "','" + Convert.ToBoolean(chk_Prm_Extre_Cikmasin.Checked) + "','" + Convert.ToString(txt_Prm_KartYoksay.Text) + "','" + Convert.ToBoolean(chk_Prm_AdisDoviz.Checked) + "','" + Convert.ToBoolean(chk_Prm_Tum_Paket.Checked) + "','" + Convert.ToBoolean(chk_Prm_Siparis_Uyari.Checked) + "','" + Convert.ToBoolean(chk_Prm_Hesapkilit.Checked) + "', "
                    + " '" + Convert.ToBoolean(chk_Prm_MasaacanGarson.Checked) + "','" + txt_Adres1.Text + "','" + txt_Adres2.Text + "','" + txt_Adres3.Text + "','" + txt_Adres4.Text + "','" + txt_Adres5.Text + "','" + txt_Prm_FisAciklama.Text + "','" + txt_Prm_MasaSize.Text + "','" + Convert.ToBoolean(chk_Prm_Sonmasa.Checked) + "','" + System.Drawing.ColorTranslator.ToHtml(color_Prm_Sonmasa.Color) + "', "
                    + " '" + Convert.ToBoolean(chk_Prm_PaketForm.Checked) + "','" + Convert.ToBoolean(chk_Prm_PaketKisi.Checked) + "','" + Convert.ToBoolean(chk_Hesap_DovizOzet.Checked) + "','" + Convert.ToBoolean(chk_Hesap_DovizOzetToplam.Checked) + "','" + txt_Prm_FrontPath.Text + "','" + txt_Prm_SikKullanSize.Text + "', "
                    + " '" + Convert.ToString(Terazi_Comport.Text) + "','" + Convert.ToInt32(Terazi_BaodRate.Text) + "','" + Convert.ToInt32(Terazi_DataBits.Text) + "','" + Convert.ToString(Terazi_Parity.Text) + "', "
                    + " '" + Convert.ToDecimal(Terazi_StopBits.Text) + "','" + Convert.ToString(Terazi_FlowControl.Text) + "', "
                    + " '" + Convert.ToInt32(txt_Genişlik.Text) + "','" + Convert.ToInt32(txt_Yukseklik.Text) + "','" + Convert.ToBoolean(chk_SatisArama.Checked) + "','" + Convert.ToBoolean(chk_HesapSor.Checked) + "', "
                    + " '" + Convert.ToBoolean(chk_CariSor.Checked) + "','" + Convert.ToBoolean(chk_CallerID.Checked) + "','" + Convert.ToBoolean(chk_SiparisSayi.Checked) + "','" + Convert.ToBoolean(chk_AutoUpdate.Checked) + "', "
                    + " '" + Convert.ToBoolean(Param_PaketSiparisPayi.Checked) + "', '" + Convert.ToBoolean(Param_SubeGonder.Checked) + "', '" + Param_LimitFolio.Checked + "', "
                    + " '" + Param_DirekLimitUyari.Checked + "', '" + Param_OzelMasaSiralama.Checked + "', '" + Param_HesapFisiDokum.Checked + "', '" + Param_HspFontAlgilama.Checked + "', '" + Param_AdisyonFolioAdi.Checked + "', "
                    + " '" + Param_FullPos.Checked + "', '" + Param_CikisKapa.Checked + "','" + Param_DirekAdisyonZor.Checked + "','" + Param_DirekAdisyonPrSor.Checked + "', "
                    + " '" + Param_KGAlgilama.Checked + "','" + Param_ExtraFolioAcma.Checked + "','" + Param_SiparisAna.Checked + "','" + Param_iadeKontrol.Checked + "','" + Param_iadeLimit.EditValue.ToString().Replace(",", ".") + "','" + Pos_HesapDkmRenk.Checked + "','" + Param_AdisyonDegis.Checked + "', "
                    + " '" + Param_AdisyonIndAd.Checked + "','" + Param_SiparisTutar.Checked + "','" + Param_AnaEkranCiro.Checked + "','" + Param_MasaTakipCiro.Checked + "','" + Param_AcilisCekSil.Checked + "', "
                    + " '" + Param_CariAdSoyad.Checked + "','" + Param_OdenmezAc.Checked + "','" + Pos_SatirSil.Checked + "','" + Pos_SatirSilUser.Text + "','" + Param_MasaTakipMenu.Checked + "', "
                    + " '" + Param_ParaUstuIngenico.Checked + "','" + Param_SatisTabloGonderi.EditValue + "','" + Param_SatisTabloID.EditValue + "','" + Param_SatisTabloAktif.Checked + "','" + Param_AcilistaMenu.Checked + "','" + Param_IngenicoSPR.Checked + "','" + Param_SiparisFisFont.Checked + "', "
                    + " '" + Param_HizliSatisCekAc.Checked + "','" + Param_KartfGBCheckOut.Checked + "','" + Param_YeniHesapDkm.Checked + "','" + Param_YeniSiparisDkm.Checked + "','" + Param_OdaKrediCompOdenmez.Checked + "','" + Param_KurTransfer.Checked + "', "
                    + " '" + Param_CallCenterPaket.Checked + "','" + Param_PaketDipTotal.Checked + "','" + Param_HesapKapamaAds.Checked + "','" + Param_HesapDkmAciklama.Checked + "','" + System.Drawing.ColorTranslator.ToHtml(Pkod_OzelMasaRengi.Color) + "','" + System.Drawing.ColorTranslator.ToHtml(Pkod_RezMasaRengi.Color) + "','" + Param_AndroGeriYazdir.Checked + "','" + Param_PaketKucukEkran.Checked + "','" + Param_GetirTest.Checked + "','" + Param_GetirOtomatikOnay.Checked + "','" + Param_SatisCikisButton.Checked + "','" + Param_nfcBarkodAktif.Checked + "','" + Param_ParcaliMasaAktif.Checked + "','" + yazdirilmamissiparis.Checked + "','" + masamusait.Checked + "','" + masatakiphesappasif.Checked + "','" + kisivegarsonbirkeresoraktif.Checked + "','" + satirsilfiscikmasinaktif.Checked + "','" + onburoikramsifiryazaktif.Checked + "','" + txtKartnoSayisi.Text + "','" + cariindirimAktif.Checked + "','" + lookUpEdit_tipbox.EditValue + "'  )");

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Genel, Log.Log_Islem.Kaydet, "Genel Parametreler Kaydedildi", String.Empty, "1");
            }
            else
            {
                dbtools.execcmd("Update Pos_Param set Param_TesisAdi = '" + txt_Prm_Tesis_Adi.EditValue + "',Param_Onburo='" + Convert.ToBoolean(chk_Prm_Onburo.Checked) + "',Param_Cost ='" + Convert.ToBoolean(chk_Prm_Cost.Checked) + "',Param_Muh = '" + Convert.ToBoolean(chk_Prm_Muh.Checked) + "',Param_CalismaSekli = '" + rdo_Prm_Calisma.SelectedIndex + "', "
                + " Param_Tar_Nere = '" + rdo_Prm_Tarih_Nereden.SelectedIndex + "',Param_Kur_Nere = '" + rdo_Prm_Kur_Nereden.SelectedIndex + "',Param_Limittip = '" + rdo_Prm_Limit_Tipi.SelectedIndex + "',Param_Balance = '" + rdo_Prm_Balance.SelectedIndex + "', "
                + " Param_Fis_Dovizli ='" + rdo_Prm_Fis_Dovizli.SelectedIndex + "',Param_Tesistip='" + rdo_Prm_Tesis_Tipi.SelectedIndex + "',Param_Tarih='" + date_Prm_Tarih.DateTime.Date + "',Param_Dovizkod='" + Convert.ToString(look_Prm_Kurkodu.EditValue) + "', "
                + " Param_Kur='" + txt_Prm_Kur.Text.Replace(",", ".") + "',Param_Yansit='" + Convert.ToBoolean(chk_Prm_Yansit.Checked) + "',Param_Discount='" + txt_Prm_Discount.EditValue + "', "
                + " Param_Miktar = '" + spn_Prm_Miktar.Value + "',Param_Kartla_Giris='" + Convert.ToBoolean(chk_Prm_Kul_Kart.Checked) + "', "
                + " Param_Miktar_Duzelt = '" + Convert.ToBoolean(chk_Prm_MiktarDuzelt.Checked) + "', Param_Masa_Refresh = '" + spin_Prm_Refresh.EditValue + "',Param_Yarim_Tam = '" + Convert.ToBoolean(chk_Prm_YarimTam.Checked) + "', "
                + " Param_HH_Ind = '" + Convert.ToBoolean(chk_Prm_HH.Checked) + "', Param_Fis_Urungrup = '" + Convert.ToBoolean(chk_Prm_UrunGrup.Checked) + "',Param_FullComp = '" + txt_Prm_Fullcomp.Text + "', "
                + " Param_Rec_Bas = '" + Convert.ToInt32(spn_Prm_Recbas.EditValue).ToString() + "',Param_Rec_Hane = '" + Convert.ToInt32(spn_Prm_Rechane.EditValue).ToString() + "', "
                + " Param_KG_Bas = '" + Convert.ToInt32(spn_Prm_KGbas.EditValue).ToString() + "',Param_KG_Hane = '" + Convert.ToInt32(spn_Prm_KGhane.EditValue).ToString() + "', "
                + " Param_GR_Bas = '" + Convert.ToInt32(spn_Prm_GRbas.EditValue).ToString() + "',Param_GR_Hane = '" + Convert.ToInt32(spn_Prm_GRhane.EditValue).ToString() + "', "
                + " Param_Masa_Geri = '" + Convert.ToBoolean(chk_Prm_MasaGeri.Checked) + "',Param_Rapor_Design = '" + Convert.ToString(look_Param_Rapor_Design.EditValue) + "',Param_Pda_Kartsor = '" + Convert.ToBoolean(chk_Prm_PdaKart.Checked) + "',Param_Anagrup_Cikmasin = '" + Convert.ToBoolean(chk_Prm_Anagrup_Cikmasin.Checked) + "', "
                + " Param_Satis_YD = '" + Convert.ToBoolean(chk_Prm_SatisYD_Aktif.Checked) + "',Param_Paket_YD = '" + Convert.ToBoolean(chk_Prm_PaketYD_Aktif.Checked) + "',Param_Dep_Fiyat = '" + Convert.ToBoolean(chk_Prm_DepFiyat.Checked) + "',Param_Masatr_Uyari = '" + Convert.ToBoolean(chk_Prm_Masatr_Uyari.Checked) + "', "
                + " Param_Bindirim = '" + Convert.ToString(look_Prm_Bindirim.EditValue) + "',Param_Printer_Tanim = '" + Convert.ToBoolean(chk_Prm_PrinterTanim.Checked) + "',Param_Hesap_Disable = '" + Convert.ToBoolean(chk_Prm_HesapDisable.Checked) + "',Param_Yuvarla = '" + Convert.ToString(look_Prm_Yuvarla.EditValue) + "', "
                + " Param_Yuv_Sayi = '" + Convert.ToInt32(spn_Prm_Ysayi.EditValue) + "',Param_Hsifir_Ikram = '" + Convert.ToBoolean(chk_Prm_HsifirIkram.Checked) + "',Param_Adispr_Uyari = '" + Convert.ToBoolean(chk_Prm_AdisyonprUyari.Checked) + "',Param_Paketci_Sor = '" + Convert.ToBoolean(chk_Prm_Paketcisor.Checked) + "', "
                + " Param_Gunsonu_Aktar = '" + Convert.ToBoolean(chk_Prm_GunsonuAktar.Checked) + "',Param_Masa_Garson = '" + Convert.ToBoolean(chk_Prm_MasaGarson.Checked) + "',Param_Extre_Cikmasin = '" + Convert.ToBoolean(chk_Prm_Extre_Cikmasin.Checked) + "',Param_Kart_Yoksay = N'" + Convert.ToString(txt_Prm_KartYoksay.Text) + "', "
                + " Param_Adis_Doviz = '" + Convert.ToBoolean(chk_Prm_AdisDoviz.Checked) + "',Param_Tum_Paket = '" + Convert.ToBoolean(chk_Prm_Tum_Paket.Checked) + "',Param_Siparis_Uyari = '" + Convert.ToBoolean(chk_Prm_Siparis_Uyari.Checked) + "',Param_Hesap_Kilit = '" + Convert.ToBoolean(chk_Prm_Hesapkilit.Checked) + "', "
                + " Param_Masaacan_Garson = '" + Convert.ToBoolean(chk_Prm_MasaacanGarson.Checked) + "', "
                + " Param_Adres1 = '" + txt_Adres1.Text + "',Param_Adres2 = '" + txt_Adres2.Text + "',Param_Adres3 = '" + txt_Adres3.Text + "',Param_Adres4 = '" + txt_Adres4.Text + "',Param_Adres5 = '" + txt_Adres5.Text + "',Param_Fis_Aciklama = '" + txt_Prm_FisAciklama.Text + "', "
                + " Param_Masa_Size = '" + txt_Prm_MasaSize.Text + "',Param_Sonmasa = '" + Convert.ToBoolean(chk_Prm_Sonmasa.Checked) + "',Param_Sonmasa_Renk = '" + System.Drawing.ColorTranslator.ToHtml(color_Prm_Sonmasa.Color) + "',Param_Paket_Form = '" + Convert.ToBoolean(chk_Prm_PaketForm.Checked) + "', "
                + " Param_Paket_Kisi = '" + chk_Prm_PaketKisi.Checked + "',Param_Hesap_DovizOzet = '" + Convert.ToBoolean(chk_Hesap_DovizOzet.Checked) + "',Param_Hesap_DovizOzetToplam = '" + Convert.ToBoolean(chk_Hesap_DovizOzetToplam.Checked) + "',Param_FrontPath = '" + txt_Prm_FrontPath.Text + "', "
                + " Param_SikKullanSize = '" + txt_Prm_SikKullanSize.Text + "', Param_ComPort = '" + Convert.ToString(Terazi_Comport.Text) + "', Param_BaudRate ='" + Terazi_BaodRate.Text + "', "
                + " Param_DataBits = '" + Terazi_DataBits.Text + "', Param_Parity = '" + Terazi_Parity.Text + "', Param_StopBits = '" + Terazi_StopBits.Text + "', Param_FlowControl = '" + Terazi_FlowControl.Text + "', "
                + " Param_bSizeW = '" + Convert.ToInt32(txt_Genişlik.Text) + "', Param_bSizeH = '" + Convert.ToInt32(txt_Yukseklik.EditValue) + "', Param_SatisArama = '" + Convert.ToBoolean(chk_SatisArama.Checked) + "', "
                + " Param_HesapSor = '" + Convert.ToBoolean(chk_HesapSor.Checked) + "', Param_CariSor = '" + Convert.ToBoolean(chk_CariSor.Checked) + "', Param_CallerID = '" + Convert.ToBoolean(chk_CallerID.Checked) + "', "
                + " Param_SiparisSayi = '" + Convert.ToBoolean(chk_SiparisSayi.Checked) + "', Param_AutoUpdate = '" + Convert.ToBoolean(chk_AutoUpdate.Checked) + "', Param_PaketSiparisPayi = '" + Convert.ToBoolean(Param_PaketSiparisPayi.Checked) + "', "
                + " Param_SubeGonder = '" + Convert.ToBoolean(Param_SubeGonder.Checked) + "', Param_LimitFolio = '" + Param_LimitFolio.Checked + "', Param_DirekLimitUyari = '" + Param_DirekLimitUyari.Checked + "', "
                + " Param_OzelMasaSiralama = '" + Param_OzelMasaSiralama.Checked + "', Param_HesapFisiDokum = '" + Param_HesapFisiDokum.Checked + "', Param_HspFontAlgilama = '" + Param_HspFontAlgilama.Checked + "', "
                + " Param_AdisyonFolioAdi = '" + Param_AdisyonFolioAdi.Checked + "', Param_FullPos = '" + Param_FullPos.Checked + "', Param_CikisKapa = '" + Param_CikisKapa.Checked + "', "
                + " Param_DirekAdisyonZor = '" + Param_DirekAdisyonZor.Checked + "',Param_DirekAdisyonPrSor = '" + Param_DirekAdisyonPrSor.Checked + "', Param_KGAlgilama = '" + Param_KGAlgilama.Checked + "', "
                + " Param_ExtraFolioAcma = '" + Param_ExtraFolioAcma.Checked + "', Param_SiparisAna = '" + Param_SiparisAna.Checked + "', Param_iadeKontrol= '" + Param_iadeKontrol.Checked + "', Param_iadeLimit ='" + Param_iadeLimit.EditValue.ToString().Replace(",", ".") + "', "
                + " Pos_HesapDkmRenk = '" + Pos_HesapDkmRenk.Checked + "', Param_AdisyonDegis = '" + Param_AdisyonDegis.Checked + "',Param_AdisyonIndAd = '" + Param_AdisyonIndAd.Checked + "',Param_SiparisTutar = '" + Param_SiparisTutar.Checked + "',  "
                + " Param_AnaEkranCiro = '" + Param_AnaEkranCiro.Checked + "',Param_MasaTakipCiro = '" + Param_MasaTakipCiro.Checked + "', Param_AcilisCekSil= '" + Param_AcilisCekSil.Checked + "', Param_CariAdSoyad = '" + Param_CariAdSoyad.Checked + "', "
                + " Param_OdenmezAc = '" + Param_OdenmezAc.Checked + "',Param_SatirSil ='" + Pos_SatirSil.Checked + "', Param_SatirSilUser ='" + Pos_SatirSilUser.Text + "', Param_MasaTakipMenu ='" + Param_MasaTakipMenu.Checked + "', Param_ParaUstuIngenico = '" + Param_ParaUstuIngenico.Checked + "', "
                + " Param_SatisTabloID = '" + Param_SatisTabloID.EditValue + "', Param_SatisTabloGonderi = '" + Param_SatisTabloGonderi.EditValue + "',Param_SatisTabloAktif = '" + Param_SatisTabloAktif.Checked + "',Param_AcilistaMenu ='" + Param_AcilistaMenu.Checked + "',Param_IngenicoSPR = '" + Param_IngenicoSPR.Checked + "',Param_SiparisFisFont ='" + Param_SiparisFisFont.Checked + "', "
                + " Param_HizliSatisCekAc = '" + Param_HizliSatisCekAc.Checked + "', Param_KartfGBCheckOut = '" + Param_KartfGBCheckOut.Checked + "',Param_YeniHesapDkm = '" + Param_YeniHesapDkm.Checked + "',  "
                + " Param_YeniSiparisDkm = '" + Param_YeniSiparisDkm.Checked + "', Param_OdaKrediCompOdenmez = '" + Param_OdaKrediCompOdenmez.Checked + "', "
                + " Param_KurTransfer = '" + Param_KurTransfer.Checked + "',Param_CallCenterPaket = '" + Param_CallCenterPaket.Checked + "', "
                + " Param_PaketDipTotal = '" + Param_PaketDipTotal.Checked + "', Param_HesapKapamaAds = '" + Param_HesapKapamaAds.Checked + "',Param_HesapDkmAciklama = '" + Param_HesapDkmAciklama.Checked + "',Param_OzelMasaRengi = '" + System.Drawing.ColorTranslator.ToHtml(Pkod_OzelMasaRengi.Color) + "' , Param_RezMasaRengi = '" + System.Drawing.ColorTranslator.ToHtml(Pkod_RezMasaRengi.Color) + "',Param_AndroGeriYazdir = '" + Param_AndroGeriYazdir.Checked + "',Param_PaketKucukEkran = '" + Param_PaketKucukEkran.Checked + "',Param_GetirTest = '" + Param_GetirTest.Checked + "',Param_GetirOtomatikOnay = '" + Param_GetirOtomatikOnay.Checked + "',Param_SatisCikisButton = '" + Param_SatisCikisButton.Checked + "',Param_nfcBarkodAktif = '" + Param_nfcBarkodAktif.Checked + "',Param_ParcaliMasaAktif = '" + Param_ParcaliMasaAktif.Checked + "',yazdirilmamissiparis = '" + yazdirilmamissiparis.Checked + "',masamusait = '" + masamusait.Checked + "',masatakiphesappasif = '" + masatakiphesappasif.Checked + "',kisivegarsonbirkeresoraktif = '" + kisivegarsonbirkeresoraktif.Checked + "',satirsilfiscikmasinaktif = '" + satirsilfiscikmasinaktif.Checked + "',onburoikramsifiryazaktif = '" + onburoikramsifiryazaktif.Checked + "',kartnoSayisi = '" + txtKartnoSayisi.Text + "',cariindirimAktif = '" + cariindirimAktif.Checked + "',tipboxReceteKod = '" + lookUpEdit_tipbox.EditValue.ToString() + "'   "
                + " where Param_Id = '1' ");

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Genel, Log.Log_Islem.Duzelt, "Genel Parametrelerde Duzeltme Islemi Yapıldı", String.Empty, "1");

            }

            DataTable dtMac = dbtools.SelectTable("SELECT  *  FROM  Rmosmuh.dbo.P_Bilg WHERE P_Mac='" + dbtools.MacAdresi() + "'");
            if (dtMac.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Rmosmuh.dbo.P_Bilg  (P_Tek, P_Mac, P_Dep, P_Sabitkonum, P_Sabitkonumkodu)   VALUES ('" + Convert.ToBoolean(chk_Prm_TekDep.Checked) + "','" + dbtools.MacAdresi() + "','" + txt_Prm_TekDep.Text + "','" + Convert.ToBoolean(chk_Prm_Konum.Checked) + "','" + Convert.ToString(txt_Prm_Konum.Text) + "')");
            }
            else
            {
                dbtools.execcmd("UPDATE   Rmosmuh.dbo.P_Bilg SET  P_Tek ='" + Convert.ToBoolean(chk_Prm_TekDep.Checked) + "', P_Mac ='" + dbtools.MacAdresi() + "', P_Dep ='" + txt_Prm_TekDep.Text + "', "
                        + " P_Sabitkonum = '" + Convert.ToBoolean(chk_Prm_Konum.Checked) + "', P_Sabitkonumkodu = '" + Convert.ToString(txt_Prm_Konum.Text) + "'  WHERE P_Mac ='" + dbtools.MacAdresi() + "' ");
            }

            Param.Param_Yukle();

            RHMesaj.MyMessage("Güncellendi !");
        }

        private void btn_Prm_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdo_Prm_Kur_Nereden_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdo_Prm_Kur_Nereden.SelectedIndex == 1)
            {
                groupControl3.Visible = true;
            }
            else
            {
                groupControl3.Visible = false;
            }
        }

        private void chk_Prm_Yansit_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_Prm_Yansit.Checked)
            {
                txt_Prm_Discount.Visible = false;
            }
            else
            {
                txt_Prm_Discount.Visible = true;
            }
        }

        private void chk_Prm_Konum_CheckedChanged(object sender, EventArgs e)
        {
            txt_Prm_Konum.Visible = chk_Prm_Konum.Checked;
        }

        private void chk_Prm_TekDep_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_Prm_TekDep.Checked)
            {
                txt_Prm_TekDep.Visible = false;
            }
            else
            {
                txt_Prm_TekDep.Visible = true;
            }
        }
        private void rdo_Prm_Tarih_Nereden_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdo_Prm_Tarih_Nereden.SelectedIndex == 1 && User.P_Kod.ToUpper() == "RMOS")
            {
                date_Prm_Tarih.Properties.ReadOnly = false;
            }
            else
            {
                date_Prm_Tarih.Properties.ReadOnly = true;
            }
        }

        private void look_Param_Rapor_Design_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                look_Param_Rapor_Design.EditValue = null;
            }
        }

        private void look_Prm_Bindirim_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                look_Prm_Bindirim.EditValue = null;
            }
        }

        private void look_Prm_Yuvarla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                look_Prm_Yuvarla.EditValue = null;
            }
        }
        #endregion

        #region Printer Grup Ayarları
        int Id = 0;



        private void btn_PrintGrup_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(look_PrintGrup_Ana.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Lütfen Ana Grup Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToString(look_PrintGrup_Alt.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Lütfen Alt Grup Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Sinif = '15' and Pkod_Id = '" + Id + "'");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("insert into Pos_Kodlar (Pkod_Ustgrup, Pkod_Altgrup,Pkod_Ad,Pkod_Sinif,Pkod_AciklamaY,Pkod_AciklamaG) " +
                    "values ('" + Convert.ToString(look_PrintGrup_Ana.EditValue) + "','" + Convert.ToString(look_PrintGrup_Alt.EditValue) + "','" +
                    txt_PrintGrup_Aciklama.Text + "', '15', '" + Pkod_AciklamaY.EditValue + "','" + Pkod_AciklamaG.EditValue + "')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_PrintGrup, Log.Log_Islem.Kaydet, "Ana Grup :" + Convert.ToString(look_PrintGrup_Ana.EditValue) + " Alt Grup :" + Convert.ToString(look_PrintGrup_Alt.EditValue) + " Aciklama :" + txt_PrintGrup_Aciklama.Text, String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Ad = '" + txt_PrintGrup_Aciklama.Text + "', Pkod_AciklamaY = '" + Pkod_AciklamaY.EditValue + "', Pkod_AciklamaG = '" + Pkod_AciklamaG.EditValue + "'  where Pkod_Sinif = '15' and Pkod_Id = '" + Id + "' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_PrintGrup, Log.Log_Islem.Duzelt, "Ana Grup :" + Convert.ToString(look_PrintGrup_Ana.EditValue) + " Alt Grup :" + Convert.ToString(look_PrintGrup_Alt.EditValue) + " Aciklama :" + txt_PrintGrup_Aciklama.Text, String.Empty, String.Empty);
            }
        }

        private void gridyenile_PrintGrup()
        {
            gridColumn1.FieldName = "Pkod_Id";
            gridColumn2.FieldName = "Pkod_Ad";
            gridColumn142.FieldName = "Pkod_AciklamaY";
            gridColumn143.FieldName = "Pkod_AciklamaG";

            DataTable dt = dbtools.SelectTable("select Pkod_Id, Pkod_Ad, ISNULL(Pkod_AciklamaY,30) as Pkod_AciklamaY, ISNULL(Pkod_AciklamaG,80) as Pkod_AciklamaG from Pos_Kodlar where Pkod_Sinif = '15' and Pkod_Ustgrup = '" + Convert.ToString(look_PrintGrup_Ana.EditValue) + "' and Pkod_Altgrup = '" + Convert.ToString(look_PrintGrup_Alt.EditValue) + "' ");
            grd_PrintGrup.DataSource = dt;

            txt_PrintGrup_Aciklama.Text = "";
            Pkod_AciklamaY.Text = "30";
            Pkod_AciklamaG.Text = "80";
            txt_PrintGrup_Aciklama.Focus();
            Id = 0;
        }

        private void btn_PrintGrup_Sil_Click(object sender, EventArgs e)
        {
            if (Id != 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Seçili Kaydı Silmek İstediğinize Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Kodlar where Pkod_Sinif = '15' and Pkod_Id = '" + Id + "' ");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_PrintGrup, Log.Log_Islem.Sil, "Ana Grup :" + Convert.ToString(look_PrintGrup_Ana.EditValue) + " Alt Grup :" + Convert.ToString(look_PrintGrup_Alt.EditValue) + " Aciklama :" + txt_PrintGrup_Aciklama.Text, String.Empty, String.Empty);
                    gridyenile_PrintGrup();
                }
            }
            else
            {
                MessageBox.Show(res_man.GetString("Silinecek Kaydı Seçiniz...'"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                Id = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Pkod_Id"));
                txt_PrintGrup_Aciklama.EditValue = gridView1.GetFocusedRowCellValue("Pkod_Ad");
                Pkod_AciklamaY.EditValue = Convert.ToString(gridView1.GetFocusedRowCellValue("Pkod_AciklamaY")) == "" ? 30 : Convert.ToInt32(gridView1.GetFocusedRowCellValue("Pkod_AciklamaY"));
                Pkod_AciklamaG.EditValue = Convert.ToString(gridView1.GetFocusedRowCellValue("Pkod_AciklamaG")) == "" ? 80 : Convert.ToInt32(gridView1.GetFocusedRowCellValue("Pkod_AciklamaG"));
            }
        }

        private void look_PrintGrup_Ana_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = '09'  and Kodlar_Anagrup = '" + Convert.ToString(look_PrintGrup_Ana.EditValue) + "'  order by Kodlar_Kod");

            look_PrintGrup_Alt.Properties.DataSource = dt;
            look_PrintGrup_Alt.Properties.DisplayMember = "Kodlar_Ad";
            look_PrintGrup_Alt.Properties.ValueMember = "Kodlar_Kod";

            gridyenile_PrintGrup();


        }

        private void btn_PrintGrup_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void look_PrintGrup_Alt_EditValueChanged(object sender, EventArgs e)
        {
            gridyenile_PrintGrup();
        }

        private void Print_Aciklama_Yenile()
        {
            gridControl2.DataSource = dbtools.SelectTable("select convert(bit,0) as sec,Pkod_Kod,Pkod_Ad from Pos_Kodlar where Pkod_Sinif = '23' order by Pkod_Kod");
        }

        private void btn_PrintGrup_Aktar_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(look_PrintGrup_Ana.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Lütfen Ana Grup Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToString(look_PrintGrup_Alt.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Lütfen Alt Grup Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < gridView15.RowCount; i++)
            {
                if (Convert.ToBoolean(gridView15.GetRowCellValue(i, "sec")))
                {
                    txt_PrintGrup_Aciklama.Text = Convert.ToString(gridView15.GetRowCellValue(i, "Pkod_Ad"));
                    Id = Convert.ToInt32(dbtools.DegerGetir("select ISNULL((select Pkod_Id from Pos_Kodlar where Pkod_Sinif = '15' and Pkod_Ustgrup = '" + Convert.ToString(look_PrintGrup_Ana.EditValue) + "' and Pkod_Altgrup = '" + Convert.ToString(look_PrintGrup_Alt.EditValue) + "' and Pkod_Ad = '" + Convert.ToString(gridView15.GetRowCellValue(i, "Pkod_Ad")) + "'),0)"));
                    btn_PrintGrup_Kaydet_Click(null, null);
                    Application.DoEvents();
                }
            }
            txt_PrintGrup_Aciklama.Text = "";
            Print_Aciklama_Yenile();
            gridyenile_PrintGrup();

        }
        #endregion

        #region Print Ayarları
        private void btn_PrintAyar_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(look_PrintAyar_dep.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Departman Bos Geçilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (Convert.ToString(look_PrintAyar_AnaGrup.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Ana Grup Boş Geçilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + look_PrintAyar_dep.EditValue + "' and Pkod_Ustgrup= '" + look_PrintAyar_AnaGrup.EditValue + "' and Pkod_Altgrup = '" + look_PrintAyar_AltGrup.EditValue + "' and  Pkod_Sinif = '16' and ISNULL(Pkod_Posta,'') = '" + Convert.ToString(look_PrintAyar_Posta.EditValue) + "' and Pkod_Ad = '" + Convert.ToString(cmb_PrintAyar_Printer.Text) + "' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod, Pkod_Ustgrup,Pkod_Altgrup,Pkod_Ad,Pkod_Sinif,Pkod_Satir,Pkod_Printer,Pkod_Ip,Pkod_Port,Pkod_Ekran,Pkod_Posta,Pkod_AbuyerPr,Pkod_AbuyerPr2,Pkod_AbuyerPr3,Pkod_AbuyerPr4,Pkod_AbuyerIP,Pkod_AbuyerPort) VALUES ('" + look_PrintAyar_dep.EditValue + "','" + look_PrintAyar_AnaGrup.EditValue + "','" + look_PrintAyar_AltGrup.EditValue + "','" + cmb_PrintAyar_Printer.EditValue + "','16','" + spn_PrintAyar_BosSatir.Value + "','" + Convert.ToString(look_PrintAyar_Pr.EditValue) + "','" + txt_PrintAyar_Ip.Text + "'," + Convert.ToInt32(spn_PrintAyar_Port.EditValue) + ",'" + txt_PrintAyar_Ekran.Text + "','" + Convert.ToString(look_PrintAyar_Posta.EditValue) + "','" + cmb_PrintAyar_AbuyerPr.EditValue + "','" + cmb_PrintAyar_AbuyerPr2.EditValue + "','" + cmb_PrintAyar_AbuyerPr3.EditValue + "','" + cmb_PrintAyar_AbuyerPr4.EditValue + "','" + Pkod_AbuyerIP.Text + "','" + Pkod_AbuyerPort.Text + "')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_PrintAyar, Log.Log_Islem.Kaydet, "Departman :" + look_PrintAyar_dep.EditValue.ToString() + " Ana Grup :" + look_PrintAyar_AnaGrup.EditValue.ToString() + " Alt Grup :" + Convert.ToString(look_PrintAyar_AltGrup.EditValue) + " Printer :" + cmb_PrintAyar_Printer.EditValue + " - " + Convert.ToString(look_PrintAyar_Pr.EditValue) + " Bos Satir :" + spn_PrintAyar_BosSatir.Value.ToString(), String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_Ad =  '" + cmb_PrintAyar_Printer.EditValue + "', Pkod_Satir = '" + spn_PrintAyar_BosSatir.Value + "',Pkod_Printer = '" + Convert.ToString(look_PrintAyar_Pr.EditValue) + "',Pkod_Ip = '" + txt_PrintAyar_Ip.Text + "',Pkod_Port = '" + Convert.ToInt32(spn_PrintAyar_Port.EditValue) + "',Pkod_Ekran = '" + txt_PrintAyar_Ekran.Text + "',Pkod_AbuyerPr = '" + cmb_PrintAyar_AbuyerPr.EditValue + "' ,Pkod_AbuyerPr2 = '" + cmb_PrintAyar_AbuyerPr2.EditValue + "',Pkod_AbuyerPr3 = '" + cmb_PrintAyar_AbuyerPr3.EditValue + "' ,Pkod_AbuyerPr4 = '" + cmb_PrintAyar_AbuyerPr4.EditValue + "',Pkod_AbuyerIP = '" + Pkod_AbuyerIP.Text + "',Pkod_AbuyerPort = '" + Pkod_AbuyerPort.EditValue + "'  where Pkod_Kod = '" + look_PrintAyar_dep.EditValue + "' and Pkod_Ustgrup= '" + look_PrintAyar_AnaGrup.EditValue + "' and Pkod_Altgrup = '" + look_PrintAyar_AltGrup.EditValue + "' and Pkod_Sinif = '16' and ISNULL(Pkod_Posta,'') = '" + Convert.ToString(look_PrintAyar_Posta.EditValue) + "' and Pkod_Ad = '" + cmb_PrintAyar_Printer.Text + "' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_PrintAyar, Log.Log_Islem.Duzelt, "Departman :" + look_PrintAyar_dep.EditValue.ToString() + " Ana Grup :" + look_PrintAyar_AnaGrup.EditValue.ToString() + " Alt Grup :" + Convert.ToString(look_PrintAyar_AltGrup.EditValue) + " Printer :" + cmb_PrintAyar_Printer.EditValue + " - " + Convert.ToString(look_PrintAyar_Pr.EditValue) + " Bos Satir :" + spn_PrintAyar_BosSatir.Value.ToString(), String.Empty, String.Empty);
            }

            Print_Yenile();
        }

        private void look_PrintAyar_AnaGrup_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(look_PrintAyar_AnaGrup.EditValue) == "HES" || Convert.ToString(look_PrintAyar_AnaGrup.EditValue) == "ADI" || Convert.ToString(look_PrintAyar_AnaGrup.EditValue) == "FAT" || Convert.ToString(look_PrintAyar_AnaGrup.EditValue) == "PKT" || Convert.ToString(look_PrintAyar_AnaGrup.EditValue) == "KSM")
            {
                look_PrintAyar_AltGrup.Visible = false;
                look_PrintAyar_AltGrup.EditValue = null;
                return;
            }
            else
            {
                look_PrintAyar_AltGrup.Visible = true;
            }

            DataTable dt = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = '09'  and Kodlar_Anagrup = '" + Convert.ToString(look_PrintAyar_AnaGrup.EditValue) + "'  order by Kodlar_Kod");

            look_PrintAyar_AltGrup.Properties.DataSource = dt;
            look_PrintAyar_AltGrup.Properties.DisplayMember = "Kodlar_Ad";
            look_PrintAyar_AltGrup.Properties.ValueMember = "Kodlar_Kod";

        }

        private void Print_Yenile()
        {
            gridColumn19.FieldName = "Kodlar_Ad";
            gridColumn20.FieldName = "Pkod_Ad";
            gridColumn21.FieldName = "Pkod_Altgrup";
            gridColumn22.FieldName = "Pkod_Satir";
            gridColumn24.FieldName = "Pkod_Ustgrup";
            gridColumn57.FieldName = "AnaGrup";
            gridColumn58.FieldName = "AltGrup";
            gridColumn73.FieldName = "Pkod_Printer";
            gridColumn78.FieldName = "Pkod_Ip";
            gridColumn79.FieldName = "Pkod_Port";
            gridColumn88.FieldName = "Pkod_Ekran";
            gridColumn60.FieldName = "Pkod_Posta";
            gridColumn105.FieldName = "Posta_Ad";
            gridColumn106.FieldName = "Pkod_AbuyerPr";
            gridColumn146.FieldName = "Pkod_AbuyerPr2";
            gridColumn147.FieldName = "Pkod_AbuyerPr3";
            gridColumn148.FieldName = "Pkod_AbuyerPr4";

            //DataTable dt = dbtools.SelectTable("SELECT Pkod_Kod, Alt.Kodlar_Ad as AltGrup,alt.Kodlar_Kod as Pkod_Altgrup, "
            //                    + " CASE WHEN Pkod_Ustgrup = 'HES' THEN 'Hesap Printer' WHEN Pkod_Ustgrup = 'ADI' THEN 'Adisyon Printer' WHEN Pkod_Ustgrup = 'FAT' THEN 'Fatura Printer' WHEN Pkod_Ustgrup = 'PKT' THEN 'Paket Printer' WHEN Pkod_Ustgrup = 'KSM' THEN 'Kasa Makbuz Printer' else Ana.Kodlar_Ad end as AnaGrup, Pkod_Ad, Pkod_Satir,Pkod_Ustgrup,Pkod_Printer, "
            //                    + " Pkod_Ip,isnull(Pkod_Port,0) as Pkod_Port,Pkod_Ekran,ISNULL(Pkod_Posta,'') as Pkod_Posta "
            //                    + " FROM Pos_Kodlar "
            //                    + " left join Stok_Kodlar as Ana on Pkod_Ustgrup = Ana.Kodlar_Kod and Ana.Kodlar_Sinif = '08' "
            //                    + " "
            //                    + " left join Stok_Kodlar as Alt on Pkod_Altgrup = Alt.Kodlar_Kod and Alt.Kodlar_Sinif = '09' and Alt.Kodlar_Anagrup = ana.Kodlar_Kod where Pkod_Sinif = '16' and Pkod_Kod = '" + look_PrintAyar_dep.EditValue + "' ");

            DataTable dt = dbtools.SelectTable("SELECT pr.Pkod_Id,pr.Pkod_Kod, Alt.Kodlar_Ad as AltGrup,alt.Kodlar_Kod as Pkod_Altgrup, "
                                + " CASE WHEN pr.Pkod_Ustgrup = 'HES' THEN 'Hesap Printer' WHEN pr.Pkod_Ustgrup = 'ADI' THEN 'Adisyon Printer' WHEN pr.Pkod_Ustgrup = 'FAT' THEN 'Fatura Printer' WHEN pr.Pkod_Ustgrup = 'PKT' THEN 'Paket Printer' WHEN pr.Pkod_Ustgrup = 'KSM' THEN 'Kasa Makbuz Printer' else Ana.Kodlar_Ad end as AnaGrup, pr.Pkod_Ad, pr.Pkod_Satir,pr.Pkod_Ustgrup,pr.Pkod_Printer,  "
                                + " pr.Pkod_Ip,isnull(pr.Pkod_Port,0) as Pkod_Port,pr.Pkod_Ekran,ISNULL(pr.Pkod_Posta,'') as Pkod_Posta ,posta.Pkod_Ad as Posta_Ad,pr.Pkod_AbuyerPr "
                                + " ,pr.Pkod_AbuyerPr2,pr.Pkod_AbuyerPr3,pr.Pkod_AbuyerPr4,ISNULL(pr.Pkod_AbuyerIP,'') as Pkod_AbuyerIP, ISNULL(pr.Pkod_AbuyerPort,'') as Pkod_AbuyerPort "
                                + " FROM Pos_Kodlar as pr "
                                + " left join Stok_Kodlar as Ana on Pkod_Ustgrup = Ana.Kodlar_Kod and Ana.Kodlar_Sinif = '08'  "
                                + " left join Stok_Kodlar as Alt on Pkod_Altgrup = Alt.Kodlar_Kod and Alt.Kodlar_Sinif = '09' and Alt.Kodlar_Anagrup = ana.Kodlar_Kod  "
                                + " left join Pos_Kodlar as posta on posta.Pkod_Kod = pr.Pkod_Posta and posta.Pkod_Sinif = '18' and posta.Pkod_Dep = pr.Pkod_Kod "
                                + " where pr.Pkod_Sinif = '16' and pr.Pkod_Kod = '" + look_PrintAyar_dep.EditValue + "' ");

            grd_PrintAyar.DataSource = dt;
        }

        private void btn_PrintAyar_Sil_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Kodlar where Pkod_Kod = '" + look_PrintAyar_dep.EditValue + "' and Pkod_Ustgrup= '" + look_PrintAyar_AnaGrup.EditValue + "' and Pkod_Altgrup = '" + look_PrintAyar_AltGrup.EditValue + "' and Pkod_Sinif = '16'  and ISNULL(Pkod_Posta,'') = '" + Convert.ToString(look_PrintAyar_Posta.EditValue) + "' ");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_PrintAyar, Log.Log_Islem.Sil, "Departman :" + look_PrintAyar_dep.EditValue.ToString() + " Ana Grup :" + look_PrintAyar_AnaGrup.EditValue.ToString() + " Alt Grup :" + look_PrintAyar_AltGrup.EditValue.ToString() + " Printer :" + cmb_PrintAyar_Printer.EditValue + " Bos Satir :" + spn_PrintAyar_BosSatir.Value.ToString(), String.Empty, String.Empty);
                    Print_Yenile();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HATA " + ex.Message);
            }

        }

        private void gridView5_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView5.RowCount > 0)
            {
                look_PrintAyar_AltGrup.EditValue = gridView5.GetFocusedRowCellValue("Pkod_Altgrup").ToString();
                cmb_PrintAyar_Printer.EditValue = gridView5.GetFocusedRowCellValue("Pkod_Ad");
                spn_PrintAyar_BosSatir.Value = Convert.ToInt32(gridView5.GetFocusedRowCellValue("Pkod_Satir"));
                look_PrintAyar_AnaGrup.EditValue = gridView5.GetFocusedRowCellValue("Pkod_Ustgrup").ToString();
                look_PrintAyar_Pr.EditValue = gridView5.GetFocusedRowCellValue("Pkod_Printer").ToString();
                txt_PrintAyar_Ip.Text = Convert.ToString(gridView5.GetFocusedRowCellValue("Pkod_Ip"));
                spn_PrintAyar_Port.EditValue = Convert.ToInt32(gridView5.GetFocusedRowCellValue("Pkod_Port"));
                txt_PrintAyar_Ekran.Text = Convert.ToString(gridView5.GetFocusedRowCellValue("Pkod_Ekran"));
                look_PrintAyar_Posta.EditValue = Convert.ToString(gridView5.GetFocusedRowCellValue("Pkod_Posta"));
                cmb_PrintAyar_AbuyerPr.EditValue = gridView5.GetFocusedRowCellValue("Pkod_AbuyerPr");
                cmb_PrintAyar_AbuyerPr2.EditValue = gridView5.GetFocusedRowCellValue("Pkod_AbuyerPr2");
                cmb_PrintAyar_AbuyerPr3.EditValue = gridView5.GetFocusedRowCellValue("Pkod_AbuyerPr3");
                cmb_PrintAyar_AbuyerPr4.EditValue = gridView5.GetFocusedRowCellValue("Pkod_AbuyerPr4");
                Pkod_AbuyerIP.Text = Convert.ToString(gridView5.GetFocusedRowCellValue("Pkod_AbuyerIP"));
                Pkod_AbuyerPort.Text = Convert.ToString(gridView5.GetFocusedRowCellValue("Pkod_AbuyerPort"));
            }
        }

        private void btn_PrintAyar_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            Print_Yenile();

            look_PrintAyar_Posta.Properties.DataSource = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Sinif = '18' and Pkod_Dep = '" + Convert.ToString(look_PrintAyar_dep.EditValue) + "' order by Pkod_Kod");
            look_PrintAyar_Posta.Properties.DisplayMember = "Pkod_Ad";
            look_PrintAyar_Posta.Properties.ValueMember = "Pkod_Kod";
        }

        private void look_PrintAyar_Posta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                look_PrintAyar_Posta.EditValue = null;
            }
        }

        private void cmb_PrintAyar_AbuyerPr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_PrintAyar_AbuyerPr.EditValue = null;
            }
        }
        #endregion

        #region ADISYON - FATURA
        private void btn_Print_Adisyon_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'ADISYON'");

            if (dt.Rows.Count > 0)
            {
                DevExpress.XtraReports.UI.XtraReport myReport = xtraDizayn.BuildReport(Convert.ToString(dt.Rows[0]["Rapor_Id"]));
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream(Convert.ToString(dt.Rows[0]["Rapor_Id"]), null, myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Adisyon, Log.Log_Islem.Duzelt, "Adisyon Dizaynı Değiştirildi.", String.Empty, String.Empty);
                }
            }
            else
            {
                Print.Adisyon myReport = new Print.Adisyon();
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream("0", "ADISYON", myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Adisyon, Log.Log_Islem.Kaydet, "Adisyon Dizaynı Kaydedildi.", String.Empty, String.Empty);
                }
            }
        }

        private void btn_Print_Fatura_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'POS_FATURA'");

            if (dt.Rows.Count > 0)
            {
                DevExpress.XtraReports.UI.XtraReport myReport = xtraDizayn.BuildReport(Convert.ToString(dt.Rows[0]["Rapor_Id"]));
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream(Convert.ToString(dt.Rows[0]["Rapor_Id"]), null, myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Fatura, Log.Log_Islem.Duzelt, "Fatura Dizaynı Duzeltildi.", String.Empty, String.Empty);
                }
            }
            else
            {
                Print.Fatura myReport = new Print.Fatura();
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream("0", "POS_FATURA", myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Fatura, Log.Log_Islem.Kaydet, "Fatura Dizaynı Duzeltildi.", String.Empty, String.Empty);
                }
            }
        }
        #endregion

        #region Odeme Kodları
        private void btn_Odeme_Kaydet_Click(object sender, EventArgs e)
        {
            if (txt_Odeme_Kod.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Kod Bos Gecilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (txt_Odeme_Ad.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Ad Bos Gecilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + txt_Odeme_Kod.EditValue + "' and Pkod_Sinif = '11' ");
            if (dt.Rows.Count < 1)
            { // System.Drawing.ColorTranslator.ToHtml(clr_Masakon_Bos.Color)
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod,Pkod_Ad,Pkod_Sinif,Pkod_Kasagiris,Pkod_Kasacikis,Pkod_Tekoda,Pkod_Odano,Pkod_Ozelkod,Pkod_Fatura,Pkod_OnBuroKapatma,Pkod_OnBuroKapatma_Departman,Pkod_FisTipi,Pkod_PaketNot,Pkod_DirekBakiye,Pkod_MuhasebeAktif,Pkod_MuhasebeBorc,Pkod_MuhasebeAlacak,Pkod_AdisyonPr,Pkod_YKasaid,Pkod_banka,Pkod_YS_OdemeID,Pkod_OdemeBtnRenk,Pkod_IWEPayment,Pkod_Sira,Pkod_Dep,Pkod_otoKur,hesapDokTutarSifir,saatAralikDurdur,Pkod_E_Adisyon) VALUES ( "
                    + " '" + txt_Odeme_Kod.EditValue + "','" + txt_Odeme_Ad.EditValue + "','11','" + Convert.ToBoolean(chk_Odeme_Kasa_Giris.Checked) + "', "
                    + " '" + Convert.ToBoolean(chk_Odeme_Kasa_Cikis.Checked) + "', '" + Convert.ToBoolean(Chk_Odeme_TekOda.Checked) + "','" + txt_Odeme_Odano.EditValue + "','" + rdo_Odeme_OzelKod.SelectedIndex + "', '" + Convert.ToBoolean(chk_Odeme_Fatura.Checked) + "', "
                    + " '" + Convert.ToBoolean(chk_Prm_OnBuroKapatma.Checked) + "', '" + look_Odeme_OnbDepartman.EditValue + "','" + look_Odeme_FisTipi.EditValue + "','" + Convert.ToBoolean(chk_Odeme_paketNot.Checked) + "','" + Pkod_DirekBakiye.Checked + "', "
                    + " '" + Pkod_MuhasebeAktif.Checked + "', '" + Pkod_MuhasebeBorc.EditValue + "', '" + Pkod_MuhasebeAlacak.EditValue + "','" + Pkod_AdisyonPr.Checked + "', "
                    + " '" + look_Ykkodu.EditValue + "','" + look_Bankakodu.EditValue + "', '" + Pkod_YS_OdemeID.EditValue + "','" + System.Drawing.ColorTranslator.ToHtml(Pkod_OdemeBtnRenk.Color) + "','" + Pkod_IWEPayment.EditValue + "','" + odeme_Sira.EditValue + "','" + Pkod_Dep.EditValue + "','" + lookUpOtoKurSec.EditValue + "','" + hesapDokTutarSifir.Checked + "','" + txtsaatAralikDurdur.Text + "','" + Pkod_E_Adisyon.Checked + "' )");

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Kaydet, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Kod='" + txt_Odeme_Kod.EditValue + "',Pkod_Ad='" + txt_Odeme_Ad.EditValue + "', "
                + " Pkod_Kasagiris='" + Convert.ToBoolean(chk_Odeme_Kasa_Giris.Checked) + "',Pkod_Kasacikis='" + Convert.ToBoolean(chk_Odeme_Kasa_Cikis.Checked) + "', "
                + " Pkod_Tekoda='" + Convert.ToBoolean(Chk_Odeme_TekOda.Checked) + "',Pkod_Odano= '" + txt_Odeme_Odano.EditValue + "', Pkod_Ozelkod = '" + rdo_Odeme_OzelKod.SelectedIndex + "', "
                + " Pkod_Fatura = '" + Convert.ToBoolean(chk_Odeme_Fatura.Checked) + "', Pkod_OnBuroKapatma = '" + Convert.ToBoolean(chk_Prm_OnBuroKapatma.Checked) + "', "
                + " Pkod_OnBuroKapatma_Departman = '" + look_Odeme_OnbDepartman.EditValue + "',Pkod_FisTipi = '" + look_Odeme_FisTipi.EditValue + "',Pkod_PaketNot = '" + Convert.ToDecimal(chk_Odeme_paketNot.Checked) + "', "
                + " Pkod_DirekBakiye = '" + Pkod_DirekBakiye.Checked + "',Pkod_MuhasebeAktif ='" + Pkod_MuhasebeAktif.Checked + "', Pkod_MuhasebeBorc = '" + Pkod_MuhasebeBorc.EditValue + "', Pkod_MuhasebeAlacak ='" + Pkod_MuhasebeAlacak.EditValue + "', "
                + " Pkod_AdisyonPr = '" + Pkod_AdisyonPr.Checked + "', Pkod_YKasaid = '" + look_Ykkodu.EditValue + "', Pkod_banka = '" + look_Bankakodu.EditValue + "', "
                + " Pkod_YS_OdemeID = '" + Pkod_YS_OdemeID.EditValue + "', Pkod_OdemeBtnRenk = '" + System.Drawing.ColorTranslator.ToHtml(Pkod_OdemeBtnRenk.Color) + "', Pkod_IWEPayment = '" + Pkod_IWEPayment.EditValue + "', "
                + " Pkod_Sira = '" + odeme_Sira.EditValue + "', Pkod_Dep= '" + Pkod_Dep.EditValue + "', Pkod_otoKur= '" + lookUpOtoKurSec.EditValue + "', hesapDokTutarSifir= '" + hesapDokTutarSifir.Checked + "', saatAralikDurdur= '" + txtsaatAralikDurdur.Text + "', "
                + " Pkod_E_Adisyon = '" + Pkod_E_Adisyon.Checked + "' "
                + " where Pkod_Kod = '" + txt_Odeme_Kod.EditValue + "' and Pkod_Sinif = '11'");

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Duzelt, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Duzeltildi", String.Empty, String.Empty);
            }
            gridyenile_Odeme();
        }

        private void gridyenile_Odeme()
        {
            gridColumn3.FieldName = "Pkod_Kod";
            gridColumn4.FieldName = "Pkod_Ad";
            gridColumn5.FieldName = "Pkod_Sorgu";
            gridColumn6.FieldName = "Pkod_Kasagiris";
            gridColumn7.FieldName = "Pkod_Kasacikis";
            gridColumn8.FieldName = "Pkod_Indirim";
            gridColumn9.FieldName = "Pkod_Tekoda";
            gridColumn10.FieldName = "Pkod_Odano";
            gridColumn14.FieldName = "Pkod_Ozelkod";
            gridColumn26.FieldName = "Fatura";
            gridColumn27.FieldName = "Pkod_FisTipi";
            gridColumn27.FieldName = "Pkod_PaketNot2";
            gridColumn138.FieldName = "Pkod_DirekBakiye";
            gridColumn139.FieldName = "Pkod_MuhasebeAktif";
            gridColumn140.FieldName = "Pkod_MuhasebeBorc";
            gridColumn141.FieldName = "Pkod_MuhasebeAlacak";
            gridColumn145.FieldName = "Pkod_AdisyonPr";
            gridColumn161.FieldName = "Pkod_YS_OdemeID";
            gridColumn175.FieldName = "Pkod_E_Adisyon";

            DataTable dt = dbtools.SelectTable("select *, isnull(Pkod_Fatura,0) as Fatura,ISNULL(Pkod_PaketNot,0) as Pkod_PaketNot2, ISNULL(Pkod_DirekBakiye,0) as Pkod_DirekBakiye, ISNULL(Pkod_AdisyonPr,0) as Pkod_AdisyonPr,ISNULL(Pkod_YS_OdemeID,'') as Pkod_YS_OdemeID,Pkod_Dep,isnull(Pkod_E_Adisyon,0) as Pkod_E_Adisyon from Pos_Kodlar where Pkod_Sinif = '11' order by Pkod_Kod");
            grd_Odeme.DataSource = dt;

            txt_Odeme_Kod.EditValue = "";
            txt_Odeme_Ad.EditValue = "";
            look_Odeme_FisTipi.EditValue = null;
            chk_Odeme_paketNot.Checked = false;
            Pkod_YS_OdemeID.EditValue = "";
            look_Bankakodu.EditValue = "";
            look_Ykkodu.EditValue = "";
            Pkod_IWEPayment.EditValue = "";
            odeme_Sira.EditValue = 0;
            Pkod_Dep.EditValue = null;
            lookUpOtoKurSec.EditValue = null;

        }

        private void btn_Odeme_Sil_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Kodlar where Pkod_Kod = '" + txt_Odeme_Kod.EditValue + "' and Pkod_Sinif = '11'");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Sil, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Silindi.", String.Empty, String.Empty);
                    gridyenile_Odeme();
                }
            }
        }

        private void btn_Odeme_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                txt_Odeme_Kod.EditValue = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_Kod"));
                txt_Odeme_Ad.EditValue = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_Ad"));
                chk_Odeme_Kasa_Giris.Checked = Convert.ToBoolean(gridView2.GetFocusedRowCellValue("Pkod_Kasagiris"));
                chk_Odeme_Kasa_Cikis.Checked = Convert.ToBoolean(gridView2.GetFocusedRowCellValue("Pkod_Kasacikis"));
                Chk_Odeme_TekOda.Checked = Convert.ToBoolean(gridView2.GetFocusedRowCellValue("Pkod_Tekoda"));
                txt_Odeme_Odano.EditValue = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_Odano"));
                rdo_Odeme_OzelKod.SelectedIndex = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Pkod_Ozelkod"));
                chk_Odeme_Fatura.Checked = Convert.ToBoolean(gridView2.GetFocusedRowCellValue("Fatura"));
                chk_Prm_OnBuroKapatma.Checked = Convert.ToBoolean(gridView2.GetFocusedRowCellValue("Pkod_OnBuroKapatma"));
                look_Odeme_OnbDepartman.EditValue = gridView2.GetFocusedRowCellValue("Pkod_OnBuroKapatma_Departman");
                look_Odeme_FisTipi.EditValue = gridView2.GetFocusedRowCellValue("Pkod_FisTipi");
                chk_Odeme_paketNot.Checked = Convert.ToBoolean(gridView2.GetFocusedRowCellValue("Pkod_PaketNot2"));
                Pkod_DirekBakiye.Checked = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_DirekBakiye")) == "" ? false : Convert.ToBoolean(gridView2.GetFocusedRowCellValue("Pkod_DirekBakiye"));
                Pkod_MuhasebeAktif.Checked = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_MuhasebeAktif")) == "" ? false : Convert.ToBoolean(gridView2.GetFocusedRowCellValue("Pkod_MuhasebeAktif"));
                Pkod_MuhasebeBorc.EditValue = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_MuhasebeBorc"));
                Pkod_MuhasebeAlacak.EditValue = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_MuhasebeAlacak"));
                Pkod_AdisyonPr.Checked = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_AdisyonPr")) == "" ? false : Convert.ToBoolean(gridView2.GetFocusedRowCellValue("Pkod_AdisyonPr"));

                Pkod_E_Adisyon.Checked = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_E_Adisyon")) == "" ? false : Convert.ToBoolean(gridView2.GetFocusedRowCellValue("Pkod_E_Adisyon"));

                look_Ykkodu.EditValue = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Pkod_YKasaid"));
                look_Bankakodu.EditValue = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Pkod_banka"));


                string deger = gridView2.GetFocusedRowCellValue("Pkod_YS_OdemeID").ToString();
                Pkod_YS_OdemeID.EditValue = deger;

                Pkod_OdemeBtnRenk.Color = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_OdemeBtnRenk")));
                Pkod_IWEPayment.EditValue = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_IWEPayment"));

                odeme_Sira.EditValue = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_Sira")) == "" ? "0" : Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_Sira"));

                Pkod_Dep.EditValue = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_Dep"));

                lookUpOtoKurSec.EditValue = Convert.ToString(gridView2.GetFocusedRowCellValue("Pkod_otoKur"));


                hesapDokTutarSifir.Checked = Convert.ToString(gridView2.GetFocusedRowCellValue("hesapDokTutarSifir")) == "" ? false : Convert.ToBoolean(gridView2.GetFocusedRowCellValue("hesapDokTutarSifir"));

                txtsaatAralikDurdur.EditValue = Convert.ToString(gridView2.GetFocusedRowCellValue("saatAralikDurdur"));

            }
        }
        #endregion

        #region Önbüro Entegre
        private void btn_EntegreOnb_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(look_EntOnb_Dep.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Departman Bos Geçilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (Convert.ToString(look_EntOnbUrun.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Urun Grubu Bos Geçilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + look_EntOnb_Dep.EditValue + "' and Pkod_Urungrup = '" + look_EntOnbUrun.EditValue + "' and  Pkod_Sinif = '12'");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod, Pkod_Urungrup,Pkod_OnburoKod,Pkod_Sinif) VALUES ('" + look_EntOnb_Dep.EditValue + "','" + look_EntOnbUrun.EditValue + "','" + txt_EntOnb_OnburoKodu.EditValue + "','12')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OnbEntegre, Log.Log_Islem.Kaydet, "Departman :" + look_EntOnb_Dep.EditValue + " UrunGrup :" + look_EntOnbUrun.EditValue + " OnburoKodu :" + txt_EntOnb_OnburoKodu.EditValue + " Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_OnburoKod =  '" + txt_EntOnb_OnburoKodu.EditValue + "' where Pkod_Kod = '" + look_EntOnb_Dep.EditValue + "' and Pkod_Urungrup = '" + look_EntOnbUrun.EditValue + "' and Pkod_Sinif = '12'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OnbEntegre, Log.Log_Islem.Duzelt, "Departman :" + look_EntOnb_Dep.EditValue + " UrunGrup :" + look_EntOnbUrun.EditValue + " OnburoKodu :" + txt_EntOnb_OnburoKodu.EditValue + " Duzeltildi", String.Empty, String.Empty);
            }

            Entegre_Yenile();
        }

        private void Entegre_Yenile()
        {
            gridColumn11.FieldName = "Kodlar_Ad";
            gridColumn12.FieldName = "Pkod_OnburoKod";
            gridColumn13.FieldName = "Pkod_Urungrup";

            DataTable dt = dbtools.SelectTable("SELECT Pkod_Kod,Kodlar_Ad,Pkod_Urungrup,Pkod_OnburoKod FROM Pos_Kodlar left join "
                + "Stok_Kodlar on Pkod_Urungrup = Kodlar_Kod and Kodlar_Sinif = '10' where Pkod_Sinif = '12' and Pkod_Kod = '" + look_EntOnb_Dep.EditValue + "' ");
            grd_EntegreOnb.DataSource = dt;
        }

        private void btn_EntegreOnb_Sil_Click(object sender, EventArgs e)
        {
            DialogResult c;
            c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (c == DialogResult.Yes)
            {
                dbtools.execcmd("delete from Pos_Kodlar where Pkod_Kod = '" + look_EntOnb_Dep.EditValue + "' and Pkod_Urungrup = '" + look_EntOnbUrun.EditValue + "' and Pkod_Sinif = '12'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OnbEntegre, Log.Log_Islem.Sil, "Departman :" + look_EntOnb_Dep.EditValue + " UrunGrup :" + look_EntOnbUrun.EditValue + " OnburoKodu :" + txt_EntOnb_OnburoKodu.EditValue + " Silindi", String.Empty, String.Empty);
                Entegre_Yenile();
            }
        }

        private void gridView3_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView3.RowCount > 0)
            {
                look_EntOnbUrun.EditValue = gridView3.GetFocusedRowCellValue("Pkod_Urungrup").ToString();
                txt_EntOnb_OnburoKodu.EditValue = gridView3.GetFocusedRowCellValue("Pkod_OnburoKod");
            }
        }

        private void btn_EntegreOnb_cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void look_EntOnb_Dep_EditValueChanged(object sender, EventArgs e)
        {
            Entegre_Yenile();
        }
        #endregion

        #region Cost Entegre
        private void btn_EntCost_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(look_EntCost_Departman) == "")
            {
                MessageBox.Show(res_man.GetString("Departman Bos Geçilemez...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (Convert.ToString(look_EntCost_Grup) == "")
            {
                MessageBox.Show(res_man.GetString("Ana Grup Bos Geçilemez...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + look_EntCost_Departman.EditValue + "' and Pkod_Urungrup = '" + look_EntCost_Grup.EditValue + "' and  Pkod_Sinif = '13'");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod, Pkod_Urungrup,Pkod_OnburoKod,Pkod_Sinif) VALUES ('" + look_EntCost_Departman.EditValue + "','" + look_EntCost_Grup.EditValue + "','" + look_EntCost_Departman3.EditValue + "','13')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CostEntegre, Log.Log_Islem.Kaydet, "Departman :" + look_EntCost_Departman.EditValue + " UrunGrup :" + look_EntCost_Grup.EditValue + " OnburoKodu :" + look_EntCost_Departman3.EditValue + " Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_OnburoKod =  '" + look_EntCost_Departman3.EditValue + "' where Pkod_Kod = '" + look_EntCost_Departman.EditValue + "' and Pkod_Urungrup = '" + look_EntCost_Grup.EditValue + "' and Pkod_Sinif = '13'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CostEntegre, Log.Log_Islem.Duzelt, "Departman :" + look_EntCost_Departman.EditValue + " UrunGrup :" + look_EntCost_Grup.EditValue + " OnburoKodu :" + look_EntCost_Departman3.EditValue + " Duzeltildi", String.Empty, String.Empty);
            }
            Entegre_Yenile2();
        }

        private void Entegre_Yenile2()
        {
            gridColumn15.FieldName = "Kodlar_Ad";
            gridColumn16.FieldName = "Pkod_OnburoKod";
            gridColumn17.FieldName = "Pkod_Urungrup";

            DataTable dt = dbtools.SelectTable("SELECT Pkod_Kod,Kodlar_Ad,Pkod_Urungrup,Pkod_OnburoKod FROM Pos_Kodlar left join "
                + "Stok_Kodlar on Pkod_Urungrup = Kodlar_Kod and Kodlar_Sinif = '10' where Pkod_Sinif = '13' and Pkod_Kod = '" + look_EntCost_Departman.EditValue + "' ");
            grd_EntCost.DataSource = dt;
        }

        private void btn_EntCost_Sil_Click(object sender, EventArgs e)
        {
            DialogResult c;
            c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (c == DialogResult.Yes)
            {
                dbtools.execcmd("delete from Pos_Kodlar where Pkod_Kod = '" + look_EntCost_Departman.EditValue + "' and Pkod_Urungrup = '" + look_EntCost_Grup.EditValue + "' and Pkod_Sinif = '13'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CostEntegre, Log.Log_Islem.Sil, "Departman :" + look_EntCost_Departman.EditValue + " UrunGrup :" + look_EntCost_Grup.EditValue + " OnburoKodu :" + look_EntCost_Departman3.EditValue + " Silindi", String.Empty, String.Empty);
                Entegre_Yenile2();
            }
        }

        private void gridView4_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView4.RowCount > 0)
            {
                look_EntCost_Grup.EditValue = gridView4.GetFocusedRowCellValue("Pkod_Urungrup").ToString();
                look_EntCost_Departman3.EditValue = gridView4.GetFocusedRowCellValue("Pkod_OnburoKod");
            }
        }

        private void look_EntCost_Departman_EditValueChanged(object sender, EventArgs e)
        {
            Entegre_Yenile2();
        }
        #endregion


        public void parcaliMasaEkle()
        {
            try
            {
                DataTable dt = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Convert.ToString(txtParcaliDepartmanSec.EditValue) + "' and ISNULL(Masa_Parcali,0)<>1");

                int sayi = Convert.ToInt32(txtParcaliMasaSayisi.Text);
                foreach (DataRow item in dt.Rows)
                {
                    string masano = item["Masa_No"].ToString() + "_";
                    for (int i = 1; i < sayi + 1; i++)
                    {
                        string yeniMasaNo = masano + i;

                        string varmi = dbtools.DegerGetir("select count(Masa_No) as toplam  from Pos_Masa where Masa_No ='" + yeniMasaNo + "'");

                        if (Convert.ToInt32(varmi) < 1)
                        {
                            string query = @"INSERT INTO [dbo].[Pos_Masa]
                                                          ([Masa_Depart]
                                                          ,[Masa_No]
                                                          ,[Masa_Ad]
                                                          ,[Masa_Konum]
                                                          ,[Masa_Durum]
                                                          ,[Masa_Paket]
                                                          ,[Masa_Kartno]
                                                          ,[Masa_Posta]
                                                          ,[Masa_Parcali])
                                                    VALUES
                                                          ('" + txtParcaliDepartmanSec.EditValue + @"', 
                                                          '" + yeniMasaNo + @"', 
                                                          '" + yeniMasaNo + @"', 
                                                          '" + item["Masa_Konum"] + @"', 
                                                          '0',
                                                          '" + item["Masa_Paket"] + @"',
                                                          '" + item["Masa_Kartno"] + @"',
                                                          '" + item["Masa_Posta"] + @"',
                                                          '1'
                                                          )";

                            dbtools.execcmd(query);
                        }


                    }
                }


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "parcaliMasaEkle", "", ex);
            }
        }

        #region Masa Tanımları
        private void btn_MasaTan_Kaydet_Click(object sender, EventArgs e)
        {

            if (txtParcaliMasaAktif.Checked)
            {
                string varmi = dbtools.DegerGetir("select count(*) as toplam from Pos_Param where ISNULL(Param_ParcaliMasaAktif,0)=1");
                if (varmi.Equals("0"))
                {
                    RHMesaj.MyMessageInformation("Lütfen parçalı masayı aktif yaptıktan sonra ekleyiniz\nParametreler->Genel Parametre 3 -> Parçalı Masa Aktif");
                    return;
                }

                if (Convert.ToString(txtParcaliDepartmanSec.EditValue) == "")
                {
                    MessageBox.Show(res_man.GetString("Departman Boş Geçilemez...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                parcaliMasaEkle();
                gridyenile_MasaTanim();
                RHMesaj.MyMessage("Parçalı masalar eklendi...");
                return;
            }


            if (Convert.ToString(look_MasaTan_Departman.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Departman Boş Geçilemez...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txt_Masa_No1.Text == "")
            {
                MessageBox.Show(res_man.GetString("Masa Numarı Boş Geçilemez...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }



            if (checkEdit5.Checked == false)
            {
                if (txt_Masa_Ad.Text.Length < 1) txt_Masa_Ad.Text = txt_Masa_No1.Text;

                DataTable dt = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Convert.ToString(look_MasaTan_Departman.EditValue) + "' and Masa_No = '" + txt_Masa_No1.EditValue + "' ");
                if (dt.Rows.Count < 1)
                {
                    dbtools.execcmd("INSERT INTO Pos_Masa (Masa_Depart, Masa_No, Masa_Ad, Masa_Konum, Masa_Durum, Masa_Paket, Masa_Posta, Masa_Hayali) VALUES "
                    + " ('" + Convert.ToString(look_MasaTan_Departman.EditValue) + "', '" + txt_Masa_No1.EditValue + "', '" + txt_Masa_Ad.EditValue + "', '" + Convert.ToString(look_Konum.EditValue) + "','" + rdo_Masa_Durum.SelectedIndex + "','" + Convert.ToBoolean(chk_Masa_Paket.Checked) + "','" + Convert.ToString(look_Masa_Posta.EditValue) + "','" + Convert.ToBoolean(chk_Masa_Hayal.Checked) + "')");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MasaTanim, Log.Log_Islem.Kaydet, "Masa No :" + txt_Masa_No1.EditValue + " Masa Ad :" + txt_Masa_Ad.EditValue + " Kaydedildi.", String.Empty, String.Empty);
                }
                else
                {
                    dbtools.execcmd("update Pos_Masa set Masa_Ad = '" + txt_Masa_Ad.EditValue + "', Masa_Konum = '" + Convert.ToString(look_Konum.EditValue) + "', Masa_Durum = '" + rdo_Masa_Durum.SelectedIndex + "', Masa_Paket = '" + Convert.ToBoolean(chk_Masa_Paket.Checked) + "',Masa_Posta = '" + Convert.ToString(look_Masa_Posta.EditValue) + "', Masa_Hayali = '" + Convert.ToBoolean(chk_Masa_Hayal.Checked) + "' "
                    + " where Masa_Depart = '" + Convert.ToString(look_MasaTan_Departman.EditValue) + "' and Masa_No = '" + txt_Masa_No1.EditValue + "' ");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MasaTanim, Log.Log_Islem.Duzelt, "Masa No :" + txt_Masa_No1.EditValue + " Masa Ad :" + txt_Masa_Ad.EditValue + " Duzeltildi.", String.Empty, String.Empty);
                }

                gridyenile_MasaTanim();
            }
            else
            {

                try
                {
                    int lenght = txt_Masa_No2.Text.Length;

                    if (txt_Masa_No1.Text.Length < 1 || txt_Masa_No2.Text.Length < 1)
                    {
                        MessageBox.Show(res_man.GetString("Baslangıc ve Bitis Değerlerini Giriniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        for (int i = Convert.ToInt32(txt_Masa_No1.Text); i <= Convert.ToInt32(txt_Masa_No2.Text); i++)
                        {

                            DataTable dt = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Convert.ToString(look_MasaTan_Departman.EditValue) + "' and Masa_No = '" + txt_Masa_Onek.Text + i.ToString().PadLeft(lenght, "0"[0]) + "' ");
                            if (dt.Rows.Count == 0)
                            {
                                dbtools.execcmd("INSERT INTO Pos_Masa (Masa_Depart, Masa_No, Masa_Ad, Masa_Konum, Masa_Durum, Masa_Paket, Masa_Posta, Masa_Hayali) VALUES "
                                + " ('" + Convert.ToString(look_MasaTan_Departman.EditValue) + "', '" + txt_Masa_Onek.Text + i.ToString().PadLeft(lenght, "0"[0]) + "', '" + txt_Masaad_Onek.Text + i.ToString().PadLeft(lenght, "0"[0]) + "', '" + Convert.ToString(look_Konum.EditValue) + "','" + rdo_Masa_Durum.SelectedIndex + "','" + Convert.ToBoolean(chk_Masa_Paket.Checked) + "','" + Convert.ToString(look_Masa_Posta.EditValue) + "','" + Convert.ToBoolean(chk_Masa_Hayal.Checked) + "')");

                            }
                        }
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MasaTanim, Log.Log_Islem.Kaydet, "Toplu Tanıtım Bas:" + txt_Masa_Onek.Text + txt_Masa_No1.Text + " Bitis:" + txt_Masa_Onek.Text + txt_Masa_No2.Text + " Kaydedildi", String.Empty, String.Empty);
                        gridyenile_MasaTanim();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(res_man.GetString("Başlangıc Bitiş Değerleri Sayısal Olmalıdır...!!!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void gridyenile_MasaTanim()
        {
            int myRowHandle, myTopRowHandle;
            myTopRowHandle = gridView1.TopRowIndex;
            myRowHandle = gridView1.FocusedRowHandle;

            gridColumn30.FieldName = "Masa_No";
            gridColumn31.FieldName = "Masa_Ad";
            gridColumn32.FieldName = "Konum";
            gridColumn33.FieldName = "Masa_Durum";
            gridColumn34.FieldName = "Masa_Paket";
            gridColumn35.FieldName = "Masa_Konum";
            gridColumn77.FieldName = "Masa_Posta";
            gridColumn95.FieldName = "Masa_Hayali";
            gridColumn99.FieldName = "sec";
            gridColumn100.FieldName = "Masa_Id";

            DataTable dt = dbtools.SelectTable("SELECT convert(bit,0) as sec,Masa_Id,Masa_No, Masa_Ad, Pkod_Ad as Konum, Masa_Durum, Masa_Paket, Masa_Konum, Masa_Posta, ISNULL(Masa_Hayali,0) as Masa_Hayali "
                + " FROM Pos_Masa WITH(NOLOCK) LEFT JOIN Pos_Kodlar WITH(NOLOCK) ON Masa_Konum = Pkod_Konumkod and Pkod_Sinif = '04' and Pkod_Kod = '" + look_MasaTan_Departman.EditValue + "' where Masa_Depart = '" + Convert.ToString(look_MasaTan_Departman.EditValue) + "' order by Masa_No");

            grd_MasaTanim.DataSource = dt;


            gridView1.TopRowIndex = myTopRowHandle;
            gridView1.FocusedRowHandle = myRowHandle;
            gridView1.SelectRow(myRowHandle);

            txt_Masa_No1.Text = "";
            txt_Masa_No2.Text = "";
            txt_Masa_Ad.Text = "";
            look_Konum.EditValue = null;
            chk_Masa_Paket.Checked = false;
            look_Masa_Posta.EditValue = null;
            chk_Masa_Hayal.Checked = false;
        }

        private void btn_MasaTan_Sil_Click(object sender, EventArgs e)
        {
            DialogResult c;
            c = MessageBox.Show(res_man.GetString("Seçili Kaydı Silmek İstediğinize Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (c == DialogResult.Yes)
            {
                dbtools.execcmd("delete from Pos_Masa where Masa_No = '" + txt_Masa_No1.EditValue + "' and Masa_Depart = '" + Convert.ToString(look_MasaTan_Departman.EditValue) + "'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MasaTanim, Log.Log_Islem.Sil, "Masa No :" + txt_Masa_No1.EditValue + " Masa Ad :" + txt_Masa_Ad.EditValue + " Silindi.", String.Empty, String.Empty);
                gridyenile_MasaTanim();
            }
        }

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit5.Checked == true)
            {
                txt_Masa_No2.Visible = true;
                txt_Masa_Onek.Visible = true;
                txt_Masaad_Onek.Visible = true;
                txt_Masa_No1.Focus();
            }
            else
            {
                txt_Masa_No2.Visible = false;
                txt_Masa_Onek.Visible = false;
                txt_Masaad_Onek.Visible = false;
            }
        }

        private void gridView7_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView7.RowCount > 0)
            {
                txt_Masa_No1.EditValue = Convert.ToString(gridView7.GetFocusedRowCellValue("Masa_No"));
                txt_Masa_Ad.EditValue = Convert.ToString(gridView7.GetFocusedRowCellValue("Masa_Ad"));
                look_Konum.EditValue = Convert.ToString(gridView7.GetFocusedRowCellValue("Masa_Konum"));
                rdo_Masa_Durum.SelectedIndex = Convert.ToInt32(gridView7.GetFocusedRowCellValue("Masa_Durum"));
                chk_Masa_Paket.Checked = Convert.ToBoolean(gridView7.GetFocusedRowCellValue("Masa_Paket"));
                look_Masa_Posta.EditValue = Convert.ToString(gridView7.GetFocusedRowCellValue("Masa_Posta"));
                chk_Masa_Hayal.Checked = Convert.ToBoolean(gridView7.GetFocusedRowCellValue("Masa_Hayali"));
            }
        }

        private void look_Departman_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt2 = dbtools.SelectTable("SELECT Pkod_Konumkod,Pkod_Ad FROM Pos_Kodlar where Pkod_Sinif = '14' and Pkod_Kod = '" + look_MasaTan_Departman.EditValue + "' ");
            look_Konum.Properties.DataSource = dt2;
            look_Konum.Properties.DisplayMember = "Pkod_Ad";
            look_Konum.Properties.ValueMember = "Pkod_Konumkod";

            gridyenile_MasaTanim();
        }

        private void look_Konum_EditValueChanged(object sender, EventArgs e)
        {
            look_Masa_Posta.Properties.DataSource = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Sinif = '18' and Pkod_Dep = '" + Convert.ToString(look_MasaTan_Departman.EditValue) + "' and Pkod_Konumkod = '" + Convert.ToString(look_Konum.EditValue) + "'");
            look_Masa_Posta.Properties.DisplayMember = "Pkod_Ad";
            look_Masa_Posta.Properties.ValueMember = "Pkod_Kod";
        }

        private void btn_MasaTan_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_MasaTan_TSil_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(res_man.GetString("Seçili Masaları Silmek İstediğinize Emin Misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (gridView7.RowCount > 0)
            {
                for (int i = 0; i < gridView7.RowCount; i++)
                {
                    if (Convert.ToBoolean(gridView7.GetRowCellValue(i, "sec")))
                    {
                        int id = Convert.ToInt32(gridView7.GetRowCellValue(i, "Masa_Id"));

                        dbtools.execcmd("delete from Pos_Masa where Masa_Id = '" + id + "'");
                    }
                }
                gridyenile_MasaTanim();
            }
        }
        #endregion

        #region Masa Konumları
        private void btn_MasaKon_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(look_MasaKon_Departman) == "")
            {
                MessageBox.Show(res_man.GetString("Departman Bos Geçilemez...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (txt_MasaKon_KonKod.Text == "")
            {
                MessageBox.Show(res_man.GetString("Konum Kod Boş Geçilemez...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + look_MasaKon_Departman.EditValue + "' and Pkod_Konumkod = '" + txt_MasaKon_KonKod.EditValue + "' and  Pkod_Sinif = '14'");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod, Pkod_Konumkod, Pkod_Ad, Pkod_Sinif,Pkod_Sira,Pkod_Bosrenk,Pkod_Dolurenk,Pkod_Hesaprenk,Pkod_AndroBosrenk,Pkod_AndroDolurenk,Pkod_AndroHesaprenk) VALUES ('" + look_MasaKon_Departman.EditValue + "','" + txt_MasaKon_KonKod.EditValue + "','" + txt_MasaKon_KonAd.EditValue + "', '14','" + Convert.ToInt32(spn_Masakon_Sira.EditValue) + "','" + System.Drawing.ColorTranslator.ToHtml(clr_Masakon_Bos.Color) + "','" + System.Drawing.ColorTranslator.ToHtml(clr_Masakon_Dolu.Color) + "','" + System.Drawing.ColorTranslator.ToHtml(clr_Masakon_Hesap.Color) + "','" + System.Drawing.ColorTranslator.ToHtml(Pkod_AndroBosrenk.Color) + "','" + System.Drawing.ColorTranslator.ToHtml(Pkod_AndroDolurenk.Color) + "','" + System.Drawing.ColorTranslator.ToHtml(Pkod_AndroHesaprenk.Color) + "')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MasaKonum, Log.Log_Islem.Kaydet, "Departman :" + look_MasaKon_Departman.EditValue + " Kod :" + txt_MasaKon_KonKod.EditValue + " Konum Ad:" + txt_MasaKon_KonAd.EditValue + " Kaydedildi.", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_Ad =  '" + txt_MasaKon_KonAd.EditValue + "',Pkod_Sira = '" + Convert.ToInt32(spn_Masakon_Sira.EditValue) + "',Pkod_Bosrenk = '" + System.Drawing.ColorTranslator.ToHtml(clr_Masakon_Bos.Color) + "',Pkod_Dolurenk = '" + System.Drawing.ColorTranslator.ToHtml(clr_Masakon_Dolu.Color) + "',Pkod_Hesaprenk = '" + System.Drawing.ColorTranslator.ToHtml(clr_Masakon_Hesap.Color) + "',Pkod_AndroBosrenk = '" + System.Drawing.ColorTranslator.ToHtml(Pkod_AndroBosrenk.Color) + "',Pkod_AndroDolurenk = '" + System.Drawing.ColorTranslator.ToHtml(Pkod_AndroDolurenk.Color) + "',Pkod_AndroHesaprenk = '" + System.Drawing.ColorTranslator.ToHtml(Pkod_AndroHesaprenk.Color) + "' where Pkod_Kod = '" + look_MasaKon_Departman.EditValue + "' and Pkod_Konumkod = '" + txt_MasaKon_KonKod.EditValue + "' and  Pkod_Sinif = '14'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MasaKonum, Log.Log_Islem.Duzelt, "Departman :" + look_MasaKon_Departman.EditValue + " Kod :" + txt_MasaKon_KonKod.EditValue + " Konum Ad:" + txt_MasaKon_KonAd.EditValue + "Duzeltildi.", String.Empty, String.Empty);
            }

            Konum_Yenile();
        }

        private void Konum_Yenile()
        {
            gridColumn18.FieldName = "Pkod_Konumkod";
            gridColumn23.FieldName = "Pkod_Ad";
            gridColumn91.FieldName = "Pkod_Sira";
            gridColumn92.FieldName = "Pkod_Bosrenk";
            gridColumn93.FieldName = "Pkod_Dolurenk";
            gridColumn94.FieldName = "Pkod_Hesaprenk";

            DataTable dt = dbtools.SelectTable("SELECT Pkod_Konumkod,Pkod_Ad,ISNULL(Pkod_Sira,0) as Pkod_Sira,ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk ,ISNULL(Pkod_AndroBosrenk,'#00FF00') as Pkod_AndroBosrenk,ISNULL(Pkod_AndroDolurenk,'#FF4500') as Pkod_AndroDolurenk,ISNULL(Pkod_AndroHesaprenk,'#BA55D3') as Pkod_AndroHesaprenk FROM Pos_Kodlar where Pkod_Sinif = '14' and Pkod_Kod = '" + look_MasaKon_Departman.EditValue + "' ");
            grd_MasaKon.DataSource = dt;
            txt_MasaKon_KonKod.Text = "";
            txt_MasaKon_KonAd.Text = "";
            txt_MasaKon_KonKod.Focus();
            spn_Masakon_Sira.EditValue = 0;
        }

        private void btn_MasaKon_Sil_Click(object sender, EventArgs e)
        {
            if (gridView6.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Kodlar where Pkod_Konumkod = '" + txt_MasaKon_KonKod.EditValue + "' and Pkod_Kod = '" + look_MasaKon_Departman.EditValue + "' and Pkod_Sinif = '14'");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MasaKonum, Log.Log_Islem.Sil, "Departman :" + look_MasaKon_Departman.EditValue + " Kod :" + txt_MasaKon_KonKod.EditValue + " Konum Ad:" + txt_MasaKon_KonAd.EditValue + " Silindi.", String.Empty, String.Empty);
                    Konum_Yenile();
                }
            }
        }

        private void gridView6_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView6.RowCount > 0)
            {
                txt_MasaKon_KonKod.EditValue = gridView6.GetFocusedRowCellValue("Pkod_Konumkod");
                txt_MasaKon_KonAd.EditValue = gridView6.GetFocusedRowCellValue("Pkod_Ad");
                spn_Masakon_Sira.EditValue = Convert.ToInt32(gridView6.GetFocusedRowCellValue("Pkod_Sira"));
                clr_Masakon_Bos.Color = ColorTranslator.FromHtml(Convert.ToString(gridView6.GetFocusedRowCellValue("Pkod_Bosrenk")));
                clr_Masakon_Dolu.Color = ColorTranslator.FromHtml(Convert.ToString(gridView6.GetFocusedRowCellValue("Pkod_Dolurenk")));
                clr_Masakon_Hesap.Color = ColorTranslator.FromHtml(Convert.ToString(gridView6.GetFocusedRowCellValue("Pkod_Hesaprenk")));
                Pkod_AndroBosrenk.Color = ColorTranslator.FromHtml(Convert.ToString(gridView6.GetFocusedRowCellValue("Pkod_AndroBosrenk")));
                Pkod_AndroDolurenk.Color = ColorTranslator.FromHtml(Convert.ToString(gridView6.GetFocusedRowCellValue("Pkod_AndroDolurenk")));
                Pkod_AndroHesaprenk.Color = ColorTranslator.FromHtml(Convert.ToString(gridView6.GetFocusedRowCellValue("Pkod_AndroHesaprenk")));
            }
        }

        private void btn_MasaKon_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void look_MasaKon_Departman_EditValueChanged(object sender, EventArgs e)
        {
            Konum_Yenile();
        }
        #endregion

        #region Cari Tanımları
        private void btn_Cari_Kaydet_Click(object sender, EventArgs e)
        {
            if (txt_Cari_Kod.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("Cari Kodu Boş Geçilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (txt_Cari_Ad.Text.Length > 0 && txt_Cari_Soyad.Text.Length > 0)
                {
                    DataTable dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Kod = '" + txt_Cari_Kod.EditValue + "' ");
                    if (dt.Rows.Count < 1)
                    {
                        dbtools.execcmd("INSERT INTO dbo.Pos_Cari (Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Adres1,Cari_Adres2,Cari_Adres3,Cari_Funvan, "
                            + " Cari_Fadres1,Cari_Fadres2,Cari_Vergidarie,Cari_Vergino,Cari_Mail,Cari_Kart) VALUES ( '" + txt_Cari_Kod.EditValue + "','" + txt_Cari_Ad.EditValue + "','" + txt_Cari_Soyad.EditValue + "','" + txt_Cari_Telefon.EditValue + "', "
                            + " '" + txt_Cari_Adres1.EditValue + "','" + txt_Cari_Adres2.EditValue + "','" + txt_Cari_Adres3.EditValue + "','" + txt_Cari_F_Unvan.EditValue + "','" + txt_Cari_F_Adres1.EditValue + "','" + txt_Cari_F_Adres2.EditValue + "','" + txt_Cari_F_Vergidaire.EditValue + "', "
                            + " '" + txt_Cari_F_Vergino.EditValue + "','" + txt_Cari_F_Mail.EditValue + "', '" + txt_Cari_Kart_No.EditValue + "' )");
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Kaydet, txt_Cari_Kod.EditValue + " Kod ile Cari Kaydedildi.", String.Empty, String.Empty);
                    }
                    else
                    {
                        dbtools.execcmd("update dbo.Pos_Cari set Cari_Kod='" + txt_Cari_Kod.EditValue + "', Cari_Ad='" + txt_Cari_Ad.EditValue + "', Cari_Soyad='" + txt_Cari_Soyad.EditValue + "', Cari_Tel='" + txt_Cari_Telefon.EditValue + "', "
                        + " Cari_Adres1='" + txt_Cari_Adres1.EditValue + "', Cari_Adres2='" + txt_Cari_Adres2.EditValue + "', Cari_Adres3='" + txt_Cari_Adres3.EditValue + "',Cari_Funvan='" + txt_Cari_F_Unvan.EditValue + "', Cari_Fadres1='" + txt_Cari_F_Adres1.EditValue + "', "
                        + " Cari_Fadres2='" + txt_Cari_F_Adres2.EditValue + "', Cari_Vergidarie='" + txt_Cari_F_Vergidaire.EditValue + "', Cari_Vergino='" + txt_Cari_F_Vergino.EditValue + "',Cari_Mail='" + txt_Cari_F_Mail.EditValue + "', Cari_Kart = '" + txt_Cari_Kart_No.EditValue + "' where  Cari_Kod = '" + txt_Cari_Kod.EditValue + "'  ");
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Duzelt, txt_Cari_Kod.EditValue + " Kod ile Cari Duzeltildi.", String.Empty, String.Empty);
                    }

                    gridyenile_Cari();
                }
                else
                {
                    MessageBox.Show(res_man.GetString("Ad ve Soyad Boş Geçilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btn_Cari_Sil_Click(object sender, EventArgs e)
        {
            if (txt_Cari_Kod.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Silinecek Kaydı Seçiniz...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Seçili Kaydı Silmek İstediğinize Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Cari where Cari_Kod = '" + txt_Cari_Kod.EditValue + "' ");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Sil, txt_Cari_Kod.EditValue + " Kod ile Cari Silindi.", String.Empty, String.Empty);
                    gridyenile_Cari();
                }
            }
        }

        private void gridyenile_Cari()
        {
            gridColumn25.FieldName = "Cari_Kod";
            gridColumn28.FieldName = "Cari_Ad";
            gridColumn29.FieldName = "Cari_Soyad";
            gridColumn36.FieldName = "Cari_Tel";
            gridColumn38.FieldName = "Cari_Adres1";
            gridColumn39.FieldName = "Cari_Adres2";
            gridColumn40.FieldName = "Cari_Adres3";
            gridColumn41.FieldName = "Cari_Funvan";
            gridColumn42.FieldName = "Cari_Fadres1";
            gridColumn43.FieldName = "Cari_Fadres2";
            gridColumn44.FieldName = "Cari_Vergidarie";
            gridColumn45.FieldName = "Cari_Vergino";
            gridColumn46.FieldName = "Cari_Mail";
            gridColumn37.FieldName = "Cari_Kart";

            DataTable dt = dbtools.SelectTable("select * from Pos_Cari order by Cari_Kod");
            grd_Cari.DataSource = dt;

            txt_Cari_Kod.EditValue = "";
            txt_Cari_Ad.EditValue = "";
            txt_Cari_Soyad.EditValue = "";
            txt_Cari_Telefon.EditValue = "";
            txt_Cari_Adres1.EditValue = "";
            txt_Cari_Adres2.EditValue = "";
            txt_Cari_Adres3.EditValue = "";
            txt_Cari_F_Unvan.EditValue = "";
            txt_Cari_F_Adres1.EditValue = "";
            txt_Cari_F_Adres2.EditValue = "";
            txt_Cari_F_Vergidaire.EditValue = "";
            txt_Cari_F_Vergino.EditValue = "";
            txt_Cari_F_Mail.EditValue = "";
            txt_Cari_Kart_No.EditValue = "";
            txt_Cari_Kod.Focus();
        }

        private void gridView8_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView8.RowCount > 0)
            {
                txt_Cari_Kod.EditValue = gridView8.GetFocusedRowCellValue("Cari_Kod").ToString();
                txt_Cari_Ad.EditValue = gridView8.GetFocusedRowCellValue("Cari_Ad").ToString();
                txt_Cari_Soyad.EditValue = gridView8.GetFocusedRowCellValue("Cari_Soyad").ToString();
                txt_Cari_Telefon.EditValue = gridView8.GetFocusedRowCellValue("Cari_Tel").ToString();
                txt_Cari_Adres1.EditValue = gridView8.GetFocusedRowCellValue("Cari_Adres1").ToString();
                txt_Cari_Adres2.EditValue = gridView8.GetFocusedRowCellValue("Cari_Adres2").ToString();
                txt_Cari_Adres3.EditValue = gridView8.GetFocusedRowCellValue("Cari_Adres3").ToString();
                txt_Cari_F_Unvan.EditValue = gridView8.GetFocusedRowCellValue("Cari_Funvan").ToString();
                txt_Cari_F_Adres1.EditValue = gridView8.GetFocusedRowCellValue("Cari_Fadres1").ToString();
                txt_Cari_F_Adres2.EditValue = gridView8.GetFocusedRowCellValue("Cari_Fadres2").ToString();
                txt_Cari_F_Vergidaire.EditValue = gridView8.GetFocusedRowCellValue("Cari_Vergidarie").ToString();
                txt_Cari_F_Vergino.EditValue = gridView8.GetFocusedRowCellValue("Cari_Vergino").ToString();
                txt_Cari_F_Mail.EditValue = gridView8.GetFocusedRowCellValue("Cari_Mail").ToString();
                txt_Cari_Kart_No.EditValue = gridView8.GetFocusedRowCellValue("Cari_Kart").ToString();
            }
        }

        private void btn_Cari_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Cari Hesaplar
        int Chrk_Id = 0;
        private void btn_CariHes_Kaydet_Click(object sender, EventArgs e)
        {
            if (txt_CariHes_Kodu.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Cari Kodu Boş Geçilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToString(look_CariHes_Odeme.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Ödeme Kodu Boş Geçilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dbtools.execcmd("INSERT INTO Pos_Carihrk (Chrk_Cek ,Chrk_Cari ,Chrk_Tarih,Chrk_Depart,Chrk_Odeme,Chrk_Borc,Chrk_Alacak) values ("
                   + " 0, '" + txt_CariHes_Kodu.EditValue + "', '" + date_CariHes.DateTime.Date + "','','" + Convert.ToString(look_CariHes_Odeme.EditValue) + "', 0, '" + txt_CariHes_Odeme.Text.Replace(",", ".") + "' )");
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariHesap, Log.Log_Islem.Kaydet, txt_CariHes_Kodu.EditValue + " kodlu cari Hesaba " + Convert.ToString(look_CariHes_Odeme.EditValue) + " " + Convert.ToString(look_CariHes_Odeme.Text) + " ile " + txt_CariHes_Odeme.Text.Replace(",", ".") + " tutarı Eklendi", String.Empty, String.Empty);
            gridyenile_CariHesap();
        }

        private void gridyenile_CariHesap()
        {
            Chrk_Id = 0;
            gridColumn47.FieldName = "Chrk_Tarih";
            gridColumn48.FieldName = "Kodlar_Ad";
            gridColumn49.FieldName = "Chrk_Cek";
            gridColumn50.FieldName = "Pkod_Ad";
            gridColumn51.FieldName = "Chrk_Borc";
            gridColumn52.FieldName = "Chrk_Alacak";
            gridColumn53.FieldName = "Chrk_Id";
            gridColumn74.FieldName = "Pkod_Kod";

            DataTable dt = dbtools.SelectTable("SELECT Chrk_Id ,Chrk_Cek,Chrk_Cari,Chrk_Tarih,Kodlar_Ad,Chrk_Borc,Chrk_Alacak,Pkod_Kod,Pkod_Ad "
        + " FROM Pos_Carihrk left join Stok_Kodlar on Chrk_Depart = Kodlar_Kod and Kodlar_Sinif = '01' left join "
        + " Pos_Kodlar on Chrk_Odeme = Pkod_Kod and Pkod_Sinif = '11' where Chrk_Cari = '" + txt_CariHes_Kodu.EditValue + "' ");

            grd_CariHesap.DataSource = dt;

            Bakiye.Text = Convert.ToString(Convert.ToDecimal(gridColumn51.SummaryText) - Convert.ToDecimal(gridColumn52.SummaryText));
        }

        private void btn_CariHep_Print_Click(object sender, EventArgs e)
        {
            PrintingSystem printingSystem1 = new PrintingSystem();
            PrintableComponentLink printableComponentLink1 = new PrintableComponentLink();
            printingSystem1.Links.AddRange(new object[] { printableComponentLink1 });
            printableComponentLink1.Component = grd_CariHesap;
            printableComponentLink1.Landscape = false;
            printableComponentLink1.Margins = new System.Drawing.Printing.Margins(50, 50, 50, 50);
            string leftColumn = "Cari Hesap Dökümü";
            string rightColumn = lbl_CariHes_Bilgi.Text + "                    " + "Bakiye : " + Bakiye.Text;
            PageHeaderFooter phf = printableComponentLink1.PageHeaderFooter as PageHeaderFooter;
            phf.Header.Content.Clear();
            phf.Header.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            phf.Header.Content.AddRange(new string[] { leftColumn, rightColumn });
            phf.Header.LineAlignment = BrickAlignment.Far;
            printableComponentLink1.ShowPreview();
        }

        private void btn_CariHep_Sil_Click(object sender, EventArgs e)
        {
            if (Chrk_Id > 0)
            {
                dbtools.execcmd("delete from Pos_Carihrk where Chrk_Id = '" + Chrk_Id.ToString() + "'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariHesap, Log.Log_Islem.Sil, txt_CariHes_Kodu.EditValue + " kodlu cari Hesaptan " + Convert.ToString(look_CariHes_Odeme.EditValue) + " " + Convert.ToString(look_CariHes_Odeme.Text) + " ile " + txt_CariHes_Odeme.Text.Replace(",", ".") + " tutarı Silindi", String.Empty, String.Empty);
                gridyenile_CariHesap();
            }
        }

        private void btn_CariAra_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Cari_Ad,Cari_Soyad from Pos_Cari WITH(NOLOCK) where Cari_Kod = '" + txt_CariHes_Kodu.Text + "'");
            if (dt.Rows.Count > 0)
            {
                lbl_CariHes_Bilgi.Text = Convert.ToString(dt.Rows[0]["Cari_Ad"]) + " " + Convert.ToString(dt.Rows[0]["Cari_Soyad"]);
                gridyenile_CariHesap();
            }
            else
            {
                txt_CariHes_Kodu.Text = "";
                gridyenile_CariHesap();
            }
        }

        private void gridView9_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView9.RowCount > 0)
            {
                if (Convert.ToDecimal(gridView9.GetFocusedRowCellValue("Chrk_Borc")) == 0)
                {
                    Chrk_Id = Convert.ToInt32(gridView9.GetFocusedRowCellValue("Chrk_Id"));
                    date_CariHes.DateTime = Convert.ToDateTime(gridView9.GetFocusedRowCellValue("Chrk_Tarih"));
                    look_CariHes_Odeme.EditValue = Convert.ToString(gridView9.GetFocusedRowCellValue("Pkod_Kod"));
                    txt_CariHes_Odeme.Text = Convert.ToDecimal(gridView9.GetFocusedRowCellValue("Chrk_Alacak")).ToString();
                }
                else
                {
                    Chrk_Id = 0;
                    date_CariHes.DateTime = DateTime.Now;
                    look_CariHes_Odeme.EditValue = null;
                    txt_CariHes_Odeme.Text = "0";
                }
            }
        }

        private void btn_CariHep_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Happy Hour
        private void btn_HH_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(look_HH_Departman5.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Departman Bos Geçilemez...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + look_HH_Departman5.EditValue + "' and  Pkod_Sinif = '20' AND CONVERT(TIME(0),Pkod_Hh_Bas) = '" + time_HH_1.EditValue + "' AND CONVERT(TIME(0),Pkod_Hh_Bit) = '" + time_HH_2.EditValue + "' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod, Pkod_Hh_Bas, Pkod_Hh_Bit,Pkod_Hh_Oran, Pkod_Sinif) VALUES ('" + look_HH_Departman5.EditValue + "','" + time_HH_1.EditValue + "','" + time_HH_2.EditValue + "','" + spn_HH_Oran.EditValue + "', '20')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HH, Log.Log_Islem.Kaydet, look_HH_Departman5.EditValue + " Departmanına " + time_HH_1.EditValue + " - " + time_HH_2.EditValue + " Arasında " + spn_HH_Oran.EditValue + " Oranı Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_Hh_Oran =  '" + spn_HH_Oran.Value + "' where Pkod_Kod = '" + look_HH_Departman5.EditValue + "' and  Pkod_Sinif = '20' AND CONVERT(TIME(0),Pkod_Hh_Bas) = '" + time_HH_1.EditValue + "' AND CONVERT(TIME(0),Pkod_Hh_Bit) = '" + time_HH_2.EditValue + "' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HH, Log.Log_Islem.Duzelt, look_HH_Departman5.EditValue + " Departmanına " + time_HH_1.EditValue + " - " + time_HH_2.EditValue + " Arasında " + spn_HH_Oran.EditValue + " Oranı Duzeltildi", String.Empty, String.Empty);
            }

            HH_Yenile();
        }

        private void HH_Yenile()
        {
            gridColumn54.FieldName = "Pkod_Hh_Bas";
            gridColumn55.FieldName = "Pkod_Hh_Bit";
            gridColumn56.FieldName = "Pkod_Hh_Oran";

            DataTable dt = dbtools.SelectTable("SELECT Pkod_Hh_Bas, Pkod_Hh_Bit,Pkod_Hh_Oran FROM Pos_Kodlar "
                + " where Pkod_Sinif = '20' and Pkod_Kod = '" + Convert.ToString(look_HH_Departman5.EditValue) + "' ");

            grd_HappyHour.DataSource = dt;
        }

        private void look_Departman5_EditValueChanged(object sender, EventArgs e)
        {
            HH_Yenile();
        }

        private void btn_HH_Sil_Click(object sender, EventArgs e)
        {
            if (gridView10.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Kodlar where Pkod_Kod = '" + look_HH_Departman5.EditValue + "' and  Pkod_Sinif = '20' AND CONVERT(TIME(0),Pkod_Hh_Bas) = '" + time_HH_1.EditValue + "' AND CONVERT(TIME(0),Pkod_Hh_Bit) = '" + time_HH_2.EditValue + "'");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HH, Log.Log_Islem.Sil, look_HH_Departman5.EditValue + " Departmanına " + time_HH_1.EditValue + " - " + time_HH_2.EditValue + " Arasında " + spn_HH_Oran.EditValue + " Oranı Silindi", String.Empty, String.Empty);
                    HH_Yenile();
                }
            }
        }

        private void gridView10_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView10.RowCount > 0)
            {
                time_HH_1.EditValue = gridView10.GetFocusedRowCellValue("Pkod_Hh_Bas");
                time_HH_2.EditValue = gridView10.GetFocusedRowCellValue("Pkod_Hh_Bit");
                spn_HH_Oran.Value = Convert.ToInt32(gridView10.GetFocusedRowCellValue("Pkod_Hh_Oran"));
            }
        }

        private void btn_HH_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Kullanıcı Kodları
        private void btn_Kul_Kaydet_Click(object sender, EventArgs e)
        {

            try
            {
                if (txt_Kul_kod.Text.Length < 1)
                {
                    MessageBox.Show(res_man.GetString("Kullanıcı Kodu Bos Geçilemez...!!!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_Kul_kod.Focus();
                    return;
                }

                #region XZ Rapor Ayarları
                DataTable dtXZ = dbtools.SelectTable("select * from Rmosmuh.dbo.Pos_User_XZ with(nolock) where P_Kod = '" + txt_Kul_kod.Text + "'");
                if (dtXZ.Rows.Count < 1)
                {
                    dbtools.execcmd("insert into Rmosmuh.dbo.Pos_User_XZ( "
                       + " P_Kod ,"
                       + " Odeme ,"
                       + " Servis ,"
                       + " Cari ,"
                       + " Odenmez ,"
                       + " Malzeme ,"
                       + " Anagrup ,"
                       + " Altgrup ,"
                       + " Iptal ,"
                       + " PaketServis ,"
                       + " IndirimMasa ,"
                       + " YiyecekIcecek ,"
                       + " MasaKonum ,"
                       + " GarsonOzet ,"
                       + " GarsonTahsil ,"
                       + " SifirTutar ,"
                       + " OzetKasa ,"
                       + " ExtKasaRapor ,"
                       + " ExtKasaDetay ,"
                       + " SiparisIptal ,"
                       + " GonderilmemisSiparisIptal ,"
                       + " SiparisDuzelt,xzraporyazici,hesapyazici )"
                       + " Values( "
                       + " '" + txt_Kul_kod.Text + "', "
                       + " '" + chk_Odeme.Checked + "', "
                       + " '" + chk_Servis.Checked + "', "
                       + " '" + chk_Cari.Checked + "', "
                       + " '" + chk_Odenmez.Checked + "', "
                       + " '" + chk_Malzeme.Checked + "', "
                       + " '" + chk_Anagrup.Checked + "', "
                       + " '" + chk_Altgrup.Checked + "', "
                       + " '" + chk_Iptal.Checked + "', "
                       + " '" + chk_PaketServis.Checked + "', "
                       + " '" + chk_IndirimMasa.Checked + "', "
                       + " '" + chk_YiyecekIcecek.Checked + "', "
                       + " '" + chk_MasaKonum.Checked + "', "
                       + " '" + chk_GarsonOzet.Checked + "', "
                       + " '" + chk_GarsonTahsil.Checked + "', "
                       + " '" + chk_SifirTutar.Checked + "', "
                       + " '" + chk_OzetKasa.Checked + "', "
                       + " '" + chk_ExtKasaRapor.Checked + "', "
                       + " '" + chk_ExtKasaDetay.Checked + "', "
                       + " '" + chk_SiparisIptal.Checked + "', "
                       + " '" + chk_GonderilmemisSiparisIptal.Checked + "', "
                       + " '" + chk_SiparisDuzelt.Checked + "', "
                       + " '" + lookUpEditYazici.EditValue + "', "
                       + " '" + lookUpEditHesapDokYazici.EditValue + "'" +
                       ") ");
                }
                else
                {

                    string sorgu = "update Rmosmuh.dbo.Pos_User_XZ set "
                       + " Odeme =                            '" + chk_Odeme.Checked + "', "
                       + " Servis =                           '" + chk_Servis.Checked + "', "
                       + " Cari =                             '" + chk_Cari.Checked + "', "
                       + " Odenmez =                          '" + chk_Odenmez.Checked + "', "
                       + " Malzeme =                          '" + chk_Malzeme.Checked + "', "
                       + " Anagrup =                          '" + chk_Anagrup.Checked + "', "
                       + " Altgrup =                          '" + chk_Altgrup.Checked + "', "
                       + " Iptal =                            '" + chk_Iptal.Checked + "', "
                       + " PaketServis =                      '" + chk_PaketServis.Checked + "', "
                       + " IndirimMasa =                      '" + chk_IndirimMasa.Checked + "', "
                       + " YiyecekIcecek =                    '" + chk_YiyecekIcecek.Checked + "', "
                       + " MasaKonum =                        '" + chk_MasaKonum.Checked + "', "
                       + " GarsonOzet =                       '" + chk_GarsonOzet.Checked + "', "
                       + " GarsonTahsil =                     '" + chk_GarsonTahsil.Checked + "', "
                       + " SifirTutar =                       '" + chk_SifirTutar.Checked + "', "
                       + " OzetKasa =                         '" + chk_OzetKasa.Checked + "', "
                       + " ExtKasaRapor =                     '" + chk_ExtKasaRapor.Checked + "', "
                       + " ExtKasaDetay =                     '" + chk_ExtKasaDetay.Checked + "', "
                       + " SiparisIptal =                     '" + chk_SiparisIptal.Checked + "', "
                       + " GonderilmemisSiparisIptal =        '" + chk_GonderilmemisSiparisIptal.Checked + "', "
                       + " SiparisDuzelt =                    '" + chk_SiparisDuzelt.Checked + "', "
                       + " xzraporyazici =                    '" + lookUpEditYazici.EditValue + "', "
                       + " hesapyazici =                    '" + lookUpEditHesapDokYazici.EditValue + "'  "
                       + " where P_Kod = '" + txt_Kul_kod.Text + "' ";
                    dbtools.execcmd(sorgu);
                }
                #endregion

                if (txt_Kul_Kart.Text.Length > 0)
                {
                    DataTable dtKart = dbtools.SelectTable("select * from Rmosmuh.dbo.Pos_User with(nolock) where P_Kart = '" + txt_Kul_Kart.Text + "'");
                    if (dtKart.Rows.Count > 1)
                    {
                        MessageBox.Show(res_man.GetString("Aynı Kart Numarası Başka Kullanıcıda Tanımlı. Kart Numarasını Değiştiriniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }

                string backUser = "";
                if (U_BackUser.EditValue != null)
                {
                    backUser = U_BackUser.EditValue.ToString();
                }

                string dil = "";

                if (Pos_dil.EditValue != null)
                {
                    dil = Pos_dil.EditValue.ToString();
                }

                DataTable dt = dbtools.SelectTable("select * from Rmosmuh.dbo.Pos_User with(nolock) where P_Kod = '" + txt_Kul_kod.Text + "'");
                if (dt.Rows.Count < 1)
                {
                    dbtools.execcmd("INSERT INTO Rmosmuh.dbo.Pos_User (P_Kod,P_Sifre,P_Ad,P_Soyad,P_Kart,P_Kulturu, "
                        + " G_Miktarduzelt, G_Tutarduzelt, G_Satirsil, G_Indirim_Satis, G_Hesapdokumu, G_Odemeal, G_Odemesil, G_Indirim_Hesap, G_Yazdirkapat, G_Yazdirmadankapat,G_Bindirim, "
                        + " M_Masatakip, M_Satis, M_Masatransfer, M_Malzemetransfer, M_Ozelmasa,M_Odakontrol, M_Masakilitle, M_Hesapkapatma, M_SatisRelogin, "
                        + " M_HesapTr, "
                        + " D_Direksatis, "
                        + " R_Raporlar, R_Detay, R_XZ, R_Mahsupkes, R_Fisiptal,R_Fisiptalgecmis, "
                        + " A_Ayarlar, A_Parametre, A_Print, A_Odeme, A_Entegre, A_Masa, A_Cari, A_HH, A_Kullanici,A_Kasa, "
                        + " P_Gunsonu, P_Departman, "
                        + " Pda_Masatakip, "
                        + " Pda_Satis, Pda_Satirsil, Pda_Miktarduzelt, "
                        + " Pda_Hesap, Pda_Masatr, Pda_Malztr, Pda_Ozelmasa, Pda_Odakontrol, "
                        + " Pda_Direksatis, "
                        + " K_Kasa, "
                        + " And_Satis, And_Satirsil, And_Miktarduzelt, And_Tutarduzelt, "
                        + " And_Hesap, And_Ozelmasa, And_MasaTr, And_Giris, "
                        + " P_Posta,P_Indirim_Yuzde,P_Bindirim_Yuzde,P_Sabit_Masa,M_MasaAc,M_BaskaMasa,G_Satirsil_Y,M_GarsonDegistir, "
                        + " G_Zayi,G_Ikram,M_KisiSayisi,R_MasaGeri,M_SiparisTekrar,Pda_HesapDok,H_HizliSatis,R_TopluIsle,And_HesapDokum,And_HesapOdeme,And_MalzTransfer,S_Sp_Sil,ExtraFolio, "
                        + " And_Yarim,And_Tam,And_Bucuk,And_Duble,Pos_SubeTrf,Pos_AdisyonPr,Pos_OdemeDegistir,And_SatisSiparisBtn,Pos_ArtiEksi_Aktif,Pos_MasaAnlikDurum,Pos_MasaUrunSil,Pos_IWERep,Pos_KartF_CheckOut,Pos_SatirSilYetkili,Pos_MasaDirekS,Pos_MasaPaketS, "
                        + " Pos_YS_YetkiReddet,Pos_YarimDubleAlan,Pos_ReceteTanimlama,Pos_FixMenu,Pos_HesapArti,User_AP,Pos_OdaKontrol,Pos_HesapFisIptal,Pos_KartTanimSil,U_BackUser,chk_K_KasaRapor,Pos_KartTanimDuzelt,Pos_KartTanimTransfer,Pos_KartTanimBakiyeTransfer,Pos_dil,Pos_Eksileme,Pos_XZdepartman,Pos_KartfIndirimAktif,Pos_ServisPayiDuzelt) VALUES( "
                        + " '" + txt_Kul_kod.Text + "','" + txt_Kul_sifre.EditValue + "','" + txt_Kul_ad.EditValue + "','" + txt_Kul_soyad.EditValue + "','" + txt_Kul_Kart.EditValue + "','" + cmb_Kulturu.SelectedIndex.ToString() + "', "
                        + " '" + Convert.ToBoolean(chk_G_Miktarduzelt.Checked) + "','" + Convert.ToBoolean(chk_G_Tutarduzelt.Checked) + "','" + Convert.ToBoolean(chk_G_Satirsil.Checked) + "','" + Convert.ToBoolean(chk_G_Indirimsatis.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_G_Hesapdokum.Checked) + "','" + Convert.ToBoolean(chk_G_Odemeal.Checked) + "','" + Convert.ToBoolean(chk_G_Odemesil.Checked) + "','" + Convert.ToBoolean(chk_G_Indirimhesap.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_G_Yazdirkapat.Checked) + "','" + Convert.ToBoolean(chk_G_Yazdirmadankapat.Checked) + "','" + Convert.ToBoolean(chk_G_Bindirim.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_M_Masatakip.Checked) + "','" + Convert.ToBoolean(chk_M_Satis.Checked) + "','" + Convert.ToBoolean(chk_M_MasaTransfer.Checked) + "','" + Convert.ToBoolean(chk_M_MalzemeTransfer.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_M_Ozelmasa.Checked) + "','" + Convert.ToBoolean(chk_M_Odakontrol.Checked) + "','" + Convert.ToBoolean(chk_M_MasaKilitle.Checked) + "','" + Convert.ToBoolean(chk_M_HesapKapatma.Checked) + "','" + Convert.ToBoolean(chk_M_SatisRelogin.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_M_HesapTr.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_D_DirekSatis.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_R_Raporlar.Checked) + "','" + Convert.ToBoolean(chk_R_Detay.Checked) + "','" + Convert.ToBoolean(chk_R_XZ.Checked) + "','" + Convert.ToBoolean(chk_R_MahsupKes.Checked) + "','" + Convert.ToBoolean(chk_R_Fisiptal.Checked) + "','" + Convert.ToBoolean(chk_R_Fisiptalgecmis.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_A_Ayarlar.Checked) + "','" + Convert.ToBoolean(chk_A_Parametreler.Checked) + "','" + Convert.ToBoolean(chk_A_Print.Checked) + "','" + Convert.ToBoolean(chk_A_Odeme.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_A_Entegre.Checked) + "','" + Convert.ToBoolean(chk_A_Masa.Checked) + "','" + Convert.ToBoolean(chk_A_Cari.Checked) + "','" + Convert.ToBoolean(chk_A_HH.Checked) + "','" + Convert.ToBoolean(chk_A_Kullanici.Checked) + "','" + Convert.ToBoolean(chk_A_Kasa.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_GunSonu.Checked) + "', '" + Convert.ToString(chkCmb_Departman.EditValue) + "', "
                        + " '" + Convert.ToBoolean(chk_Pda_Masatakip.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_Pda_Satis.Checked) + "','" + Convert.ToBoolean(chk_Pda_Satirsil.Checked) + "','" + Convert.ToBoolean(chk_Pda_Miktarduzelt.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_Pda_Hesap.Checked) + "','" + Convert.ToBoolean(chk_Pda_Masatr.Checked) + "','" + Convert.ToBoolean(chk_Pda_Malztr.Checked) + "','" + Convert.ToBoolean(chk_Pda_Ozelmasa.Checked) + "','" + Convert.ToBoolean(chk_Pda_Ozelmasa.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_Pda_Direksatis.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_K_Kasa.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_And_Satis.Checked) + "','" + Convert.ToBoolean(chk_And_Satirsil.Checked) + "','" + Convert.ToBoolean(chk_And_MiktarD.Checked) + "','" + Convert.ToBoolean(chk_And_TutarD.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_And_Hesap.Checked) + "','" + Convert.ToBoolean(chk_And_Ozelmasa.Checked) + "','" + Convert.ToBoolean(chk_And_MasaTr.Checked) + "','" + Convert.ToBoolean(chk_And_Giris.Checked) + "', "
                        + " '" + Convert.ToString(chkUser_Posta.EditValue) + "','" + Convert.ToInt32(spn_Kul_Indirim.Value) + "','" + Convert.ToInt32(spn_Kul_Bindirim.Value) + "','" + Convert.ToString(txt_Kul_Masa.Text) + "','" + Convert.ToBoolean(chk_Kul_MasaAc.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_M_Baskamasa.Checked) + "','" + Convert.ToBoolean(chk_G_SatirSilY.Checked) + "','" + chk_Kul_GarsonDegistir.Checked + "', "
                        + " '" + Convert.ToBoolean(chk_Kul_Zayi.Checked) + "','" + Convert.ToBoolean(chk_Kul_Ikram.Checked) + "','" + Convert.ToBoolean(chk_Kul_KisiSayisi.Checked) + "','" + Convert.ToBoolean(chk_Kul_MasaGeri.Checked) + "','" + Convert.ToBoolean(chk_Kul_SiparisTekrar.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_Pda_HesapDok.Checked) + "','" + Convert.ToBoolean(chk_H_HizliSatis.Checked) + "','" + Convert.ToBoolean(chk_R_TopluIsle.Checked) + "','" + Convert.ToBoolean(chk_And_HesapDokum.Checked) + "','" + Convert.ToBoolean(chk_And_HesapOdeme.Checked) + "','" + Convert.ToBoolean(chk_And_MalzTransfer.Checked) + "', "
                        + " '" + Convert.ToBoolean(chk_SpSil.Checked) + @"','" + chk_ExtraFolio.Checked + @"', "
                        + " '" + And_Yarim.Checked + "','" + And_Tam.Checked + "','" + And_Bucuk.Checked + "','" + And_Duble.Checked + "','" + Pos_SubeTrf.Checked + "', "
                        + " '" + Pos_AdisyonPr.Checked + "','" + chk_OdemeTipi.Checked + "','" + And_SatisSiparisBtn.Checked + "','" + Pos_ArtiEksi_Aktif.Checked + "','" + Pos_MasaAnlikDurum.Checked + "','" + Pos_MasaUrunSil.Checked + "','" + Pos_IWERep.Checked + "','" + Pos_KartF_CheckOut.Checked + "','" + Pos_SatirSilYetkili.Checked + "', "
                        + " '" + Pos_MasaDirekS.Checked + "','" + Pos_MasaPaketS.Checked + "','" + Pos_YS_YetkiReddet.Checked + "','" + Pos_YarimDubleAlan.Checked + "','" + Pos_ReceteTanimlama.Checked + "','" + Pos_FixMenu.Checked + "', "
                        + " '" + Pos_HesapArti.Checked + "','" + User_AP.Checked + "','" + Pos_OdaKontrol.Checked + "','" + Pos_HesapFisIptal.Checked + "','" + Pos_KartTanimSil.Checked + "','" + backUser + "','" + chk_K_KasaRapor.Checked + "','" + Pos_KartTanimDuzelt.Checked + "','" + Pos_KartTanimTransfer.Checked + "','" + Pos_KartTanimBakiyeTransfer.Checked + "','" + dil + "','" + Pos_Eksileme.Checked + "','" + Pos_XZdepartman.Checked + "','" + Pos_KartfIndirimAktif.Checked + "','" + Pos_ServisPayiDuzelt.Checked + "' ) ");



                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Kullanici, Log.Log_Islem.Kaydet, txt_Kul_kod.Text + " Kodlu " + txt_Kul_ad.EditValue + " " + txt_Kul_soyad.EditValue + "Kullanıcı Kaydedildi", String.Empty, String.Empty);
                    Kul_temizle();
                }
                else
                {
                    dbtools.execcmd("update Rmosmuh.dbo.Pos_User set P_Sifre ='" + txt_Kul_sifre.EditValue + "',P_Ad ='" + txt_Kul_ad.EditValue + "',P_Soyad='" + txt_Kul_soyad.EditValue + "',P_Kart='" + txt_Kul_Kart.EditValue + "',P_Kulturu = '" + cmb_Kulturu.SelectedIndex.ToString() + "', "
                        + " G_Miktarduzelt ='" + Convert.ToBoolean(chk_G_Miktarduzelt.Checked) + "', G_Tutarduzelt ='" + Convert.ToBoolean(chk_G_Tutarduzelt.Checked) + "', G_Satirsil ='" + Convert.ToBoolean(chk_G_Satirsil.Checked) + "', "
                        + " G_Indirim_Satis ='" + Convert.ToBoolean(chk_G_Indirimsatis.Checked) + "', G_Hesapdokumu ='" + Convert.ToBoolean(chk_G_Hesapdokum.Checked) + "', G_Odemeal ='" + Convert.ToBoolean(chk_G_Odemeal.Checked) + "', "
                        + " G_Odemesil ='" + Convert.ToBoolean(chk_G_Odemesil.Checked) + "', G_Indirim_Hesap ='" + Convert.ToBoolean(chk_G_Indirimhesap.Checked) + "', G_Yazdirkapat ='" + Convert.ToBoolean(chk_G_Yazdirkapat.Checked) + "', "
                        + " G_Yazdirmadankapat ='" + Convert.ToBoolean(chk_G_Yazdirmadankapat.Checked) + "',G_Bindirim = '" + Convert.ToBoolean(chk_G_Bindirim.Checked) + "', "
                        + " M_Masatakip ='" + Convert.ToBoolean(chk_M_Masatakip.Checked) + "', M_Satis ='" + Convert.ToBoolean(chk_M_Satis.Checked) + "', M_Masatransfer ='" + Convert.ToBoolean(chk_M_MasaTransfer.Checked) + "', "
                        + " M_Malzemetransfer ='" + Convert.ToBoolean(chk_M_MalzemeTransfer.Checked) + "', M_Ozelmasa ='" + Convert.ToBoolean(chk_M_Ozelmasa.Checked) + "',M_Odakontrol = '" + Convert.ToBoolean(chk_M_Odakontrol.Checked) + "', M_Masakilitle ='" + Convert.ToBoolean(chk_M_MasaKilitle.Checked) + "', "
                        + " M_Hesapkapatma ='" + Convert.ToBoolean(chk_M_HesapKapatma.Checked) + "',M_SatisRelogin = '" + Convert.ToBoolean(chk_M_SatisRelogin.Checked) + "',M_HesapTr = '" + Convert.ToBoolean(chk_M_HesapTr.Checked) + "', "
                        + " D_Direksatis ='" + Convert.ToBoolean(chk_D_DirekSatis.Checked) + "', "
                        + " R_Raporlar ='" + Convert.ToBoolean(chk_R_Raporlar.Checked) + "', R_Detay ='" + Convert.ToBoolean(chk_R_Detay.Checked) + "', R_XZ ='" + Convert.ToBoolean(chk_R_XZ.Checked) + "', "
                        + " R_Mahsupkes ='" + Convert.ToBoolean(chk_R_MahsupKes.Checked) + "', R_Fisiptal ='" + Convert.ToBoolean(chk_R_Fisiptal.Checked) + "',R_Fisiptalgecmis = '" + Convert.ToBoolean(chk_R_Fisiptalgecmis.Checked) + "', "
                        + " A_Ayarlar ='" + Convert.ToBoolean(chk_A_Ayarlar.Checked) + "', A_Parametre ='" + Convert.ToBoolean(chk_A_Parametreler.Checked) + "', A_Print ='" + Convert.ToBoolean(chk_A_Print.Checked) + "', "
                        + " A_Odeme ='" + Convert.ToBoolean(chk_A_Odeme.Checked) + "', A_Entegre ='" + Convert.ToBoolean(chk_A_Entegre.Checked) + "', A_Masa ='" + Convert.ToBoolean(chk_A_Masa.Checked) + "', "
                        + " A_Cari ='" + Convert.ToBoolean(chk_A_Cari.Checked) + "', A_HH ='" + Convert.ToBoolean(chk_A_HH.Checked) + "', A_Kullanici ='" + Convert.ToBoolean(chk_A_Kullanici.Checked) + "',A_Kasa = '" + Convert.ToBoolean(chk_A_Kasa.Checked) + "', "
                        + " P_Gunsonu = '" + Convert.ToBoolean(chk_GunSonu.Checked) + "', P_Departman = '" + Convert.ToString(chkCmb_Departman.EditValue) + "', "
                        + " Pda_Masatakip = '" + Convert.ToBoolean(chk_Pda_Masatakip.Checked) + "', "
                        + " Pda_Satis = '" + Convert.ToBoolean(chk_Pda_Satis.Checked) + "',Pda_Satirsil = '" + Convert.ToBoolean(chk_Pda_Satirsil.Checked) + "',Pda_Miktarduzelt = '" + Convert.ToBoolean(chk_Pda_Miktarduzelt.Checked) + "', "
                        + " Pda_Hesap = '" + Convert.ToBoolean(chk_Pda_Hesap.Checked) + "',Pda_Masatr = '" + Convert.ToBoolean(chk_Pda_Masatr.Checked) + "',Pda_Malztr = '" + Convert.ToBoolean(chk_Pda_Malztr.Checked) + "',Pda_Ozelmasa = '" + Convert.ToBoolean(chk_Pda_Ozelmasa.Checked) + "',Pda_Odakontrol = '" + Convert.ToBoolean(chk_Pda_Odakontrol.Checked) + "', "
                        + " Pda_Direksatis = '" + Convert.ToBoolean(chk_Pda_Direksatis.Checked) + "', "
                        + " K_Kasa = '" + Convert.ToBoolean(chk_K_Kasa.Checked) + "', "
                        + " And_Satis = '" + Convert.ToBoolean(chk_And_Satis.Checked) + "',And_Satirsil = '" + Convert.ToBoolean(chk_And_Satirsil.Checked) + "',And_Miktarduzelt = '" + Convert.ToBoolean(chk_And_MiktarD.Checked) + "',And_Tutarduzelt = '" + Convert.ToBoolean(chk_And_TutarD.Checked) + "', "
                        + " And_Hesap = '" + Convert.ToBoolean(chk_And_Hesap.Checked) + "',And_Ozelmasa = '" + Convert.ToBoolean(chk_And_Ozelmasa.Checked) + "',And_MasaTr = '" + Convert.ToBoolean(chk_And_MasaTr.Checked) + "',And_Giris = '" + Convert.ToBoolean(chk_And_Giris.Checked) + "', "
                        + " P_Posta = '" + Convert.ToString(chkUser_Posta.EditValue) + "',P_Indirim_Yuzde = '" + Convert.ToInt32(spn_Kul_Indirim.Value) + "',P_Bindirim_Yuzde = '" + Convert.ToInt32(spn_Kul_Bindirim.Value) + "',P_Sabit_Masa = '" + Convert.ToString(txt_Kul_Masa.Text) + "',M_MasaAc = '" + Convert.ToBoolean(chk_Kul_MasaAc.Checked) + "', "
                        + " M_BaskaMasa = '" + Convert.ToBoolean(chk_M_Baskamasa.Checked) + "',G_Satirsil_Y = '" + Convert.ToBoolean(chk_G_SatirSilY.Checked) + "',M_GarsonDegistir = '" + chk_Kul_GarsonDegistir.Checked + "',G_Zayi = '" + Convert.ToBoolean(chk_Kul_Zayi.Checked) + "',G_Ikram = '" + Convert.ToBoolean(chk_Kul_Ikram.Checked) + "', "
                        + " M_KisiSayisi = '" + Convert.ToBoolean(chk_Kul_KisiSayisi.Checked) + "',R_MasaGeri = '" + Convert.ToBoolean(chk_Kul_MasaGeri.Checked) + "',M_SiparisTekrar = '" + Convert.ToBoolean(chk_Kul_SiparisTekrar.Checked) + "',Pda_HesapDok = '" + Convert.ToBoolean(chk_Pda_HesapDok.Checked) + "',H_HizliSatis = '" + Convert.ToBoolean(chk_H_HizliSatis.Checked) + "', "
                        + " R_TopluIsle = '" + Convert.ToBoolean(chk_R_TopluIsle.Checked) + "',And_HesapDokum = '" + Convert.ToBoolean(chk_And_HesapDokum.Checked) + "',And_HesapOdeme = '" + Convert.ToBoolean(chk_And_HesapOdeme.Checked) + "',And_MalzTransfer = '" + Convert.ToBoolean(chk_And_MalzTransfer.Checked) + @"', "
                        + " S_Sp_Sil = '" + Convert.ToBoolean(chk_SpSil.Checked) + "',ExtraFolio = '" + chk_ExtraFolio.Checked + "', And_Yarim= '" + And_Yarim.Checked + "', And_Tam= '" + And_Tam.Checked + "', "
                        + " And_Bucuk = '" + And_Bucuk.Checked + "', And_Duble = '" + And_Duble.Checked + "', Pos_SubeTrf = '" + Pos_SubeTrf.Checked + "', Pos_AdisyonPr = '" + Pos_AdisyonPr.Checked + "', "
                        + " Pos_OdemeDegistir = '" + chk_OdemeTipi.Checked + "',And_SatisSiparisBtn = '" + And_SatisSiparisBtn.Checked + "', Pos_ArtiEksi_Aktif = '" + Pos_ArtiEksi_Aktif.Checked + "', "
                        + " Pos_MasaAnlikDurum = '" + Pos_MasaAnlikDurum.Checked + "',Pos_MasaUrunSil = '" + Pos_MasaUrunSil.Checked + "', Pos_IWERep= '" + Pos_IWERep.Checked + "', Pos_KartF_CheckOut ='" + Pos_KartF_CheckOut.Checked + "', "
                        + " Pos_SatirSilYetkili = '" + Pos_SatirSilYetkili.Checked + "', Pos_MasaDirekS ='" + Pos_MasaDirekS.Checked + "', Pos_MasaPaketS = '" + Pos_MasaPaketS.Checked + "', "
                        + " Pos_YS_YetkiReddet = '" + Pos_YS_YetkiReddet.Checked + "', Pos_YarimDubleAlan ='" + Pos_YarimDubleAlan.Checked + "', Pos_ReceteTanimlama= '" + Pos_ReceteTanimlama.Checked + "', Pos_FixMenu = '" + Pos_FixMenu.Checked + "', "
                        + " Pos_HesapArti = '" + Pos_HesapArti.Checked + "', User_AP = '" + User_AP.Checked + "',Pos_OdaKontrol = '" + Pos_OdaKontrol.Checked + "',Pos_HesapFisIptal = '" + Pos_HesapFisIptal.Checked + "',Pos_KartTanimSil = '" + Pos_KartTanimSil.Checked + "',U_BackUser = '" + backUser + "',chk_K_KasaRapor = '" + chk_K_KasaRapor.Checked + "',Pos_KartTanimDuzelt = '" + Pos_KartTanimDuzelt.Checked + "',Pos_KartTanimBakiyeTransfer = '" + Pos_KartTanimBakiyeTransfer.Checked + "',Pos_KartTanimTransfer = '" + Pos_KartTanimTransfer.Checked + "',Pos_dil = '" + dil + "',Pos_Eksileme = '" + Pos_Eksileme.Checked + "',Pos_XZdepartman = '" + Pos_XZdepartman.Checked + "',Pos_KartfIndirimAktif = '" + Pos_KartfIndirimAktif.Checked + "',Pos_ServisPayiDuzelt = '" + Pos_ServisPayiDuzelt.Checked + "' "
                        + " where P_Kod = '" + txt_Kul_kod.Text + "' ");






                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Kullanici, Log.Log_Islem.Duzelt, txt_Kul_kod.Text + " Kodlu " + txt_Kul_ad.EditValue + " " + txt_Kul_soyad.EditValue + "Kullanıcı Duzeltildi", String.Empty, String.Empty);
                    Kul_temizle();
                }

                User.Yetki_Yukle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Kul_Sil_Click(object sender, EventArgs e)
        {

            if (txt_Kul_kod.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Silinecek Kaydı Seciniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult sor;
                sor = MessageBox.Show(res_man.GetString("Silmek İstediğinize Emin misiniz...???"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (sor == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Rmosmuh.dbo.Pos_User where P_Kod = '" + txt_Kul_kod.Text + "'");
                    dbtools.execcmd("delete from Rmosmuh.dbo.Pos_User_XZ where P_Kod = '" + txt_Kul_kod.Text + "'");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Kullanici, Log.Log_Islem.Sil, txt_Kul_kod.Text + " Kodlu " + txt_Kul_ad.EditValue + " " + txt_Kul_soyad.EditValue + "Kullanıcı Silindi", String.Empty, String.Empty);
                    Kul_temizle();
                }
            }
        }

        private void text_kod_Leave(object sender, EventArgs e)
        {
            string Filtre = "";
            if (User.P_Kulturu != 3)
            {
                Filtre = " and P_Kulturu <> 3 ";
            }


            DataTable dt = dbtools.SelectTable("select P_Kod  ,P_Sifre, P_Ad ,P_Soyad ,P_Kart ,isnull(P_Kulturu,-1) as P_Kulturu, "
                    + " ISNULL(G_Miktarduzelt,0) AS  G_Miktarduzelt,ISNULL(G_Tutarduzelt,0) AS G_Tutarduzelt,ISNULL(G_Satirsil,0) AS G_Satirsil, "
                    + " ISNULL(G_Indirim_Satis,0) AS G_Indirim_Satis,ISNULL(G_Hesapdokumu,0) AS G_Hesapdokumu,ISNULL(G_Odemeal,0) AS G_Odemeal , "
                    + " ISNULL(G_Odemesil,0) AS G_Odemesil ,ISNULL(G_Indirim_Hesap,0) AS G_Indirim_Hesap ,ISNULL(G_Yazdirkapat,0) AS G_Yazdirkapat , "
                    + " ISNULL(G_Yazdirmadankapat,0) AS G_Yazdirmadankapat ,ISNULL(G_Bindirim,0) as G_Bindirim, "
                    + " ISNULL(M_Masatakip,0) AS M_Masatakip ,ISNULL(M_Satis,0) AS M_Satis ,ISNULL(M_Masatransfer,0) AS M_Masatransfer ,ISNULL(M_Malzemetransfer,0) AS M_Malzemetransfer , "
                    + " ISNULL(M_Ozelmasa,0) AS M_Ozelmasa, ISNULL(M_Odakontrol,0) AS M_Odakontrol ,ISNULL(M_Masakilitle,0) AS M_Masakilitle,ISNULL(M_Hesapkapatma,0) AS M_Hesapkapatma , "
                    + " ISNULL(M_SatisRelogin,0) AS M_SatisRelogin,isnull(M_HesapTr,0) as M_HesapTr, "
                    + " ISNULL(D_Direksatis,0) AS D_Direksatis, "
                    + " ISNULL(R_Raporlar,0) AS R_Raporlar ,ISNULL(R_Detay,0) AS R_Detay ,ISNULL(R_XZ,0) AS R_XZ ,ISNULL(R_Mahsupkes,0) AS R_Mahsupkes ,ISNULL(R_Fisiptal,0) AS R_Fisiptal ,ISNULL(R_Fisiptalgecmis,0) as R_Fisiptalgecmis, "
                    + " ISNULL(A_Ayarlar,0) AS A_Ayarlar ,ISNULL(A_Parametre,0) AS A_Parametre ,ISNULL(A_Print,0) AS A_Print ,ISNULL(A_Odeme,0) AS A_Odeme ,ISNULL(A_Entegre,0) AS A_Entegre , "
                    + " ISNULL(A_Masa,0) AS A_Masa ,ISNULL(A_Cari,0) AS A_Cari ,ISNULL(A_HH,0) AS A_HH ,ISNULL(A_Kullanici,0) AS A_Kullanici , ISNULL(A_Kasa,0) as A_Kasa, "
                    + " ISNULL(P_Gunsonu,0) AS P_Gunsonu, P_Departman, "
                    + " ISNULL(Pda_Masatakip,0) as Pda_Masatakip, "
                    + " ISNULL(Pda_Satis,0) as Pda_Satis, ISNULL(Pda_Satirsil,0) as Pda_Satirsil, ISNULL(Pda_Miktarduzelt,0) as Pda_Miktarduzelt, "
                    + " ISNULL(Pda_Hesap,0) as Pda_Hesap, ISNULL(Pda_Masatr,0) as Pda_Masatr, ISNULL(Pda_Malztr,0) as Pda_Malztr, ISNULL(Pda_Ozelmasa,0) as Pda_Ozelmasa, ISNULL(Pda_Odakontrol,0) as Pda_Odakontrol, "
                    + " ISNULL(Pda_Direksatis,0) as Pda_Direksatis, "
                    + " ISNULL(K_Kasa,0) as K_Kasa,ISNULL(Pos_OdaKontrol,0) as Pos_OdaKontrol, "
                    + " ISNULL(And_Satis,0) as And_Satis,ISNULL(And_Satirsil,0) as And_Satirsil,ISNULL(And_Miktarduzelt,0) as And_Miktarduzelt,ISNULL(And_Tutarduzelt,0) as And_Tutarduzelt, "
                    + " ISNULL(And_Hesap,0) as And_Hesap,ISNULL(And_Ozelmasa,0) as And_Ozelmasa,ISNULL(And_MasaTr,0) as And_MasaTr,ISNULL(And_Giris,0) as And_Giris, "
                    + " P_Posta,ISNULL(P_Indirim_Yuzde,100) as P_Indirim_Yuzde,ISNULL(P_Bindirim_Yuzde,100) as P_Bindirim_Yuzde,P_Sabit_Masa,ISNULL(M_MasaAc,0) as M_MasaAc,ISNULL(M_BaskaMasa,0) as M_BaskaMasa, "
                    + " ISNULL(G_Satirsil_Y,0) as G_Satirsil_Y,ISNULL(M_GarsonDegistir,0) as M_GarsonDegistir,ISNULL(G_Zayi,0) as G_Zayi,ISNULL(G_Ikram,0) as G_Ikram, ISNULL(M_KisiSayisi,0) as M_KisiSayisi, "
                    + " ISNULL(R_MasaGeri,0) as R_MasaGeri,ISNULL(M_SiparisTekrar,0) as M_SiparisTekrar,ISNULL(Pda_HesapDok,0) as Pda_HesapDok,ISNULL(H_HizliSatis,0) as H_HizliSatis,ISNULL(R_TopluIsle,0) as R_TopluIsle, "
                    + " ISNULL(And_HesapDokum,0) as And_HesapDokum,ISNULL(And_HesapOdeme,0) as And_HesapOdeme,ISNULL(And_MalzTransfer,0) as And_MalzTransfer, "
                    + " ISNULL(S_Sp_Sil,0) as S_Sp_Sil, ISNULL(And_Yarim,0) as And_Yarim , ISNULL(And_Tam,0) as And_Tam , ISNULL(And_Bucuk,0) as And_Bucuk , ISNULL(And_Duble,0) as And_Duble, "
                    + " ISNULL(Pos_SubeTrf,0) as  Pos_SubeTrf,ISNULL(Pos_AdisyonPr,0) as  Pos_AdisyonPr, ISNULL(Pos_OdemeDegistir,0) as Pos_OdemeDegistir, ISNULL(ExtraFolio,0) as ExtraFolio, ISNULL(And_SatisSiparisBtn,0) as And_SatisSiparisBtn, "
                    + " ISNULL(Pos_MasaAnlikDurum,0) as Pos_MasaAnlikDurum, ISNULL(Pos_ArtiEksi_Aktif,0) as Pos_ArtiEksi_Aktif, ISNULL(Pos_MasaUrunSil,0) as Pos_MasaUrunSil,ISNULL(Pos_IWERep,0) as Pos_IWERep, ISNULL(Pos_KartF_CheckOut,0) as Pos_KartF_CheckOut, ISNULL(Pos_SatirSilYetkili,0) as Pos_SatirSilYetkili, "
                    + " ISNULL(Pos_MasaPaketS,0) as Pos_MasaPaketS,ISNULL(Pos_MasaDirekS,0) as Pos_MasaDirekS, ISNULL(Pos_YS_YetkiReddet,0) as Pos_YS_YetkiReddet, ISNULL(Pos_YarimDubleAlan,1) as Pos_YarimDubleAlan, ISNULL(Pos_ReceteTanimlama,1) as Pos_ReceteTanimlama,  "
                    + " ISNULL(Pos_FixMenu,0) as Pos_FixMenu, ISNULL(Pos_HesapFisIptal,0) as Pos_HesapFisIptal, ISNULL(Pos_KartTanimSil,0) as Pos_KartTanimSil,ISNULL(Pos_HesapArti,0) as Pos_HesapArti, ISNULL(User_AP,1) as User_AP,U_BackUser,ISNULL(chk_K_KasaRapor,0) as chk_K_KasaRapor,ISNULL(Pos_KartTanimDuzelt,0) as Pos_KartTanimDuzelt,ISNULL(Pos_KartTanimTransfer,0) as Pos_KartTanimTransfer,ISNULL(Pos_KartTanimBakiyeTransfer,0) as Pos_KartTanimBakiyeTransfer,isnull(Pos_dil,'tr-TR') as Pos_dil, ISNULL(Pos_Eksileme,0) as Pos_Eksileme, ISNULL(Pos_XZdepartman,0) as Pos_XZdepartman, ISNULL(Pos_KartfIndirimAktif,0) as Pos_KartfIndirimAktif, ISNULL(Pos_ServisPayiDuzelt,0) as Pos_ServisPayiDuzelt "
                    + " from Rmosmuh.dbo.Pos_User with(nolock) where P_Kod = '" + txt_Kul_kod.Text + "' " + Filtre + " ");

            if (dt.Rows.Count > 0)
            {
                txt_Kul_kod.Text = dt.Rows[0]["P_Kod"].ToString();
                txt_Kul_sifre.Text = dt.Rows[0]["P_Sifre"].ToString();
                txt_Kul_ad.Text = dt.Rows[0]["P_Ad"].ToString();
                txt_Kul_soyad.Text = dt.Rows[0]["P_Soyad"].ToString();
                txt_Kul_Kart.EditValue = dt.Rows[0]["P_Kart"].ToString();
                cmb_Kulturu.SelectedIndex = Convert.ToInt32(dt.Rows[0]["P_Kulturu"]);

                chk_G_Miktarduzelt.Checked = Convert.ToBoolean(dt.Rows[0]["G_Miktarduzelt"]);
                chk_G_Tutarduzelt.Checked = Convert.ToBoolean(dt.Rows[0]["G_Tutarduzelt"]);
                chk_G_Satirsil.Checked = Convert.ToBoolean(dt.Rows[0]["G_Satirsil"]);
                chk_G_Indirimsatis.Checked = Convert.ToBoolean(dt.Rows[0]["G_Indirim_Satis"]);
                chk_G_Hesapdokum.Checked = Convert.ToBoolean(dt.Rows[0]["G_Hesapdokumu"]);
                chk_G_Odemeal.Checked = Convert.ToBoolean(dt.Rows[0]["G_Odemeal"]);
                chk_G_Odemesil.Checked = Convert.ToBoolean(dt.Rows[0]["G_Odemesil"]);
                chk_G_Indirimhesap.Checked = Convert.ToBoolean(dt.Rows[0]["G_Indirim_Hesap"]);
                chk_G_Yazdirkapat.Checked = Convert.ToBoolean(dt.Rows[0]["G_Yazdirkapat"]);
                chk_G_Yazdirmadankapat.Checked = Convert.ToBoolean(dt.Rows[0]["G_Yazdirmadankapat"]);
                chk_G_Bindirim.Checked = Convert.ToBoolean(dt.Rows[0]["G_Bindirim"]);

                chk_M_Masatakip.Checked = Convert.ToBoolean(dt.Rows[0]["M_Masatakip"]);
                chk_M_Satis.Checked = Convert.ToBoolean(dt.Rows[0]["M_Satis"]);
                chk_M_MasaTransfer.Checked = Convert.ToBoolean(dt.Rows[0]["M_Masatransfer"]);
                chk_M_MalzemeTransfer.Checked = Convert.ToBoolean(dt.Rows[0]["M_Malzemetransfer"]);
                chk_M_Ozelmasa.Checked = Convert.ToBoolean(dt.Rows[0]["M_Ozelmasa"]);
                chk_M_Odakontrol.Checked = Convert.ToBoolean(dt.Rows[0]["M_Odakontrol"]);
                chk_M_MasaKilitle.Checked = Convert.ToBoolean(dt.Rows[0]["M_Masakilitle"]);
                chk_M_HesapKapatma.Checked = Convert.ToBoolean(dt.Rows[0]["M_Hesapkapatma"]);
                chk_M_SatisRelogin.Checked = Convert.ToBoolean(dt.Rows[0]["M_SatisRelogin"]);
                chk_M_HesapTr.Checked = Convert.ToBoolean(dt.Rows[0]["M_HesapTr"]);

                chk_D_DirekSatis.Checked = Convert.ToBoolean(dt.Rows[0]["D_Direksatis"]);

                chk_R_Raporlar.Checked = Convert.ToBoolean(dt.Rows[0]["R_Raporlar"]);
                chk_R_Detay.Checked = Convert.ToBoolean(dt.Rows[0]["R_Detay"]);
                chk_R_XZ.Checked = Convert.ToBoolean(dt.Rows[0]["R_XZ"]);
                chk_R_MahsupKes.Checked = Convert.ToBoolean(dt.Rows[0]["R_Mahsupkes"]);
                chk_R_Fisiptal.Checked = Convert.ToBoolean(dt.Rows[0]["R_Fisiptal"]);
                chk_R_Fisiptalgecmis.Checked = Convert.ToBoolean(dt.Rows[0]["R_Fisiptalgecmis"]);

                chk_A_Ayarlar.Checked = Convert.ToBoolean(dt.Rows[0]["A_Ayarlar"]);
                chk_A_Parametreler.Checked = Convert.ToBoolean(dt.Rows[0]["A_Parametre"]);
                chk_A_Print.Checked = Convert.ToBoolean(dt.Rows[0]["A_Print"]);
                chk_A_Odeme.Checked = Convert.ToBoolean(dt.Rows[0]["A_Odeme"]);
                chk_A_Entegre.Checked = Convert.ToBoolean(dt.Rows[0]["A_Entegre"]);
                chk_A_Masa.Checked = Convert.ToBoolean(dt.Rows[0]["A_Masa"]);
                chk_A_Cari.Checked = Convert.ToBoolean(dt.Rows[0]["A_Cari"]);
                chk_A_HH.Checked = Convert.ToBoolean(dt.Rows[0]["A_HH"]);
                chk_A_Kullanici.Checked = Convert.ToBoolean(dt.Rows[0]["A_Kullanici"]);
                chk_A_Kasa.Checked = Convert.ToBoolean(dt.Rows[0]["A_Kasa"]);

                chk_GunSonu.Checked = Convert.ToBoolean(dt.Rows[0]["P_Gunsonu"]);
                chkCmb_Departman.SetEditValue(Convert.ToString(dt.Rows[0]["P_Departman"]));


                chk_Pda_Masatakip.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_Masatakip"]);

                chk_Pda_Satis.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_Satis"]);
                chk_Pda_Satirsil.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_Satirsil"]);
                chk_Pda_Miktarduzelt.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_Miktarduzelt"]);

                chk_Pda_Hesap.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_Hesap"]);
                chk_Pda_Masatr.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_Masatr"]);
                chk_Pda_Malztr.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_Malztr"]);
                chk_Pda_Ozelmasa.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_Ozelmasa"]);
                chk_Pda_Odakontrol.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_Odakontrol"]);

                chk_Pda_Direksatis.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_Direksatis"]);

                chk_K_Kasa.Checked = Convert.ToBoolean(dt.Rows[0]["K_Kasa"]);

                chk_And_Satis.Checked = Convert.ToBoolean(dt.Rows[0]["And_Satis"]);
                chk_And_Satirsil.Checked = Convert.ToBoolean(dt.Rows[0]["And_Satirsil"]);
                chk_And_MiktarD.Checked = Convert.ToBoolean(dt.Rows[0]["And_Miktarduzelt"]);
                chk_And_TutarD.Checked = Convert.ToBoolean(dt.Rows[0]["And_Tutarduzelt"]);
                chk_And_Hesap.Checked = Convert.ToBoolean(dt.Rows[0]["And_Hesap"]);
                chk_And_Ozelmasa.Checked = Convert.ToBoolean(dt.Rows[0]["And_Ozelmasa"]);
                chk_And_MasaTr.Checked = Convert.ToBoolean(dt.Rows[0]["And_MasaTr"]);
                chk_And_Giris.Checked = Convert.ToBoolean(dt.Rows[0]["And_Giris"]);

                chkUser_Posta.SetEditValue(Convert.ToString(dt.Rows[0]["P_Posta"]));
                spn_Kul_Indirim.Value = Convert.ToInt32(dt.Rows[0]["P_Indirim_Yuzde"]);
                spn_Kul_Bindirim.Value = Convert.ToInt32(dt.Rows[0]["P_Bindirim_Yuzde"]);
                txt_Kul_Masa.Text = Convert.ToString(dt.Rows[0]["P_Sabit_Masa"]);
                chk_Kul_MasaAc.Checked = Convert.ToBoolean(dt.Rows[0]["M_MasaAc"]);
                chk_M_Baskamasa.Checked = Convert.ToBoolean(dt.Rows[0]["M_BaskaMasa"]);

                chk_G_SatirSilY.Checked = Convert.ToBoolean(dt.Rows[0]["G_Satirsil_Y"]);
                chk_Kul_GarsonDegistir.Checked = Convert.ToBoolean(dt.Rows[0]["M_GarsonDegistir"]);
                chk_Kul_Zayi.Checked = Convert.ToBoolean(dt.Rows[0]["G_Zayi"]);
                chk_Kul_Ikram.Checked = Convert.ToBoolean(dt.Rows[0]["G_Ikram"]);

                chk_Kul_KisiSayisi.Checked = Convert.ToBoolean(dt.Rows[0]["M_KisiSayisi"]);
                chk_Kul_MasaGeri.Checked = Convert.ToBoolean(dt.Rows[0]["R_MasaGeri"]);

                chk_Kul_SiparisTekrar.Checked = Convert.ToBoolean(dt.Rows[0]["M_SiparisTekrar"]);
                chk_Pda_HesapDok.Checked = Convert.ToBoolean(dt.Rows[0]["Pda_HesapDok"]);
                chk_H_HizliSatis.Checked = Convert.ToBoolean(dt.Rows[0]["H_HizliSatis"]);
                chk_R_TopluIsle.Checked = Convert.ToBoolean(dt.Rows[0]["R_TopluIsle"]);

                chk_And_HesapDokum.Checked = Convert.ToBoolean(dt.Rows[0]["And_HesapDokum"]);
                chk_And_HesapOdeme.Checked = Convert.ToBoolean(dt.Rows[0]["And_HesapOdeme"]);
                chk_And_MalzTransfer.Checked = Convert.ToBoolean(dt.Rows[0]["And_MalzTransfer"]);
                chk_SpSil.Checked = Convert.ToBoolean(dt.Rows[0]["S_Sp_Sil"]);

                And_Yarim.Checked = Convert.ToBoolean(dt.Rows[0]["And_Yarim"]);
                And_Tam.Checked = Convert.ToBoolean(dt.Rows[0]["And_Tam"]);
                And_Bucuk.Checked = Convert.ToBoolean(dt.Rows[0]["And_Bucuk"]);
                And_Duble.Checked = Convert.ToBoolean(dt.Rows[0]["And_Duble"]);
                Pos_SubeTrf.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_SubeTrf"]);
                Pos_AdisyonPr.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_AdisyonPr"]);
                chk_OdemeTipi.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_OdemeDegistir"]);
                Pos_Eksileme.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_Eksileme"]);
                Pos_XZdepartman.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_XZdepartman"]);

                chk_ExtraFolio.Checked = Convert.ToBoolean(dt.Rows[0]["ExtraFolio"]);
                And_SatisSiparisBtn.Checked = Convert.ToBoolean(dt.Rows[0]["And_SatisSiparisBtn"]);
                Pos_MasaAnlikDurum.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_MasaAnlikDurum"]);
                Pos_ArtiEksi_Aktif.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_ArtiEksi_Aktif"]);
                Pos_MasaUrunSil.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_MasaUrunSil"]);
                Pos_IWERep.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_IWERep"]);
                Pos_KartF_CheckOut.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_KartF_CheckOut"]);
                Pos_SatirSilYetkili.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_SatirSilYetkili"]);
                Pos_MasaPaketS.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_MasaPaketS"]);
                Pos_MasaDirekS.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_MasaDirekS"]);
                Pos_YS_YetkiReddet.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_YS_YetkiReddet"]);
                Pos_YarimDubleAlan.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_YarimDubleAlan"]);
                Pos_ReceteTanimlama.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_ReceteTanimlama"]);
                Pos_FixMenu.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_FixMenu"]);
                Pos_HesapArti.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_HesapArti"]);
                User_AP.Checked = Convert.ToBoolean(dt.Rows[0]["User_AP"]);
                Pos_OdaKontrol.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_OdaKontrol"]);
                Pos_HesapFisIptal.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_HesapFisIptal"]);

                Pos_KartTanimSil.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_KartTanimSil"]);
                U_BackUser.EditValue = dt.Rows[0]["U_BackUser"];
                Pos_dil.EditValue = dt.Rows[0]["Pos_dil"];

                chk_K_KasaRapor.Checked = Convert.ToBoolean(dt.Rows[0]["chk_K_KasaRapor"]);

                Pos_KartTanimDuzelt.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_KartTanimDuzelt"]);
                Pos_KartTanimTransfer.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_KartTanimTransfer"]);
                Pos_KartTanimBakiyeTransfer.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_KartTanimBakiyeTransfer"]);
                Pos_KartfIndirimAktif.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_KartfIndirimAktif"]);
                Pos_ServisPayiDuzelt.Checked = Convert.ToBoolean(dt.Rows[0]["Pos_ServisPayiDuzelt"]);







            }

            DataTable dtXZ = dbtools.SelectTable("SELECT 0,Id,P_Kod,ISNULL(Odeme,0) as Odeme,ISNULL(Servis,0) as Servis,ISNULL(Cari,0) as Cari,ISNULL(Odenmez,0) as Odenmez,ISNULL(Malzeme,0) as Malzeme,ISNULL(Anagrup,0) as Anagrup,ISNULL(Altgrup,0) as Altgrup,ISNULL(Iptal,0) as Iptal,ISNULL(PaketServis,0) as PaketServis,ISNULL(IndirimMasa,0) as IndirimMasa,ISNULL(YiyecekIcecek,0) as YiyecekIcecek,ISNULL(MasaKonum,0) as MasaKonum,ISNULL(GarsonOzet,0) as GarsonOzet,ISNULL(GarsonTahsil,0) as GarsonTahsil,ISNULL(SifirTutar,0) as SifirTutar,ISNULL(OzetKasa,0) as OzetKasa,ISNULL(ExtKasaRapor,0) as ExtKasaRapor,ISNULL(ExtKasaDetay,0) as ExtKasaDetay,ISNULL(SiparisIptal,0) as SiparisIptal,ISNULL(GonderilmemisSiparisIptal,0) as GonderilmemisSiparisIptal,ISNULL(SiparisDuzelt,0) as SiparisDuzelt,ISNULL(xzraporyazici,0) as xzraporyazici,hesapyazici FROM Rmosmuh.dbo.Pos_User_XZ where P_Kod = '" + txt_Kul_kod.Text + "' ");
            if (dtXZ.Rows.Count > 0)
            {
                chk_Odeme.Checked = Convert.ToBoolean(dtXZ.Rows[0]["Odeme"]);
                chk_Servis.Checked = Convert.ToBoolean(dtXZ.Rows[0]["Servis"]);
                chk_Cari.Checked = Convert.ToBoolean(dtXZ.Rows[0]["Cari"]);
                chk_Odenmez.Checked = Convert.ToBoolean(dtXZ.Rows[0]["Odenmez"]);
                chk_Malzeme.Checked = Convert.ToBoolean(dtXZ.Rows[0]["Malzeme"]);
                chk_Anagrup.Checked = Convert.ToBoolean(dtXZ.Rows[0]["Anagrup"]);
                chk_Altgrup.Checked = Convert.ToBoolean(dtXZ.Rows[0]["Altgrup"]);
                chk_Iptal.Checked = Convert.ToBoolean(dtXZ.Rows[0]["Iptal"]);
                chk_PaketServis.Checked = Convert.ToBoolean(dtXZ.Rows[0]["PaketServis"]);
                chk_IndirimMasa.Checked = Convert.ToBoolean(dtXZ.Rows[0]["IndirimMasa"]);
                chk_YiyecekIcecek.Checked = Convert.ToBoolean(dtXZ.Rows[0]["YiyecekIcecek"]);
                chk_MasaKonum.Checked = Convert.ToBoolean(dtXZ.Rows[0]["MasaKonum"]);
                chk_GarsonOzet.Checked = Convert.ToBoolean(dtXZ.Rows[0]["GarsonOzet"]);
                chk_GarsonTahsil.Checked = Convert.ToBoolean(dtXZ.Rows[0]["GarsonTahsil"]);
                chk_SifirTutar.Checked = Convert.ToBoolean(dtXZ.Rows[0]["SifirTutar"]);
                chk_OzetKasa.Checked = Convert.ToBoolean(dtXZ.Rows[0]["OzetKasa"]);
                chk_ExtKasaRapor.Checked = Convert.ToBoolean(dtXZ.Rows[0]["ExtKasaRapor"]);
                chk_ExtKasaDetay.Checked = Convert.ToBoolean(dtXZ.Rows[0]["ExtKasaDetay"]);
                chk_SiparisIptal.Checked = Convert.ToBoolean(dtXZ.Rows[0]["SiparisIptal"]);
                chk_GonderilmemisSiparisIptal.Checked = Convert.ToBoolean(dtXZ.Rows[0]["GonderilmemisSiparisIptal"]);
                chk_SiparisDuzelt.Checked = Convert.ToBoolean(dtXZ.Rows[0]["SiparisDuzelt"]);

                if (!dtXZ.Rows[0]["xzraporyazici"].ToString().Equals("0"))
                {
                    lookUpEditYazici.EditValue = dtXZ.Rows[0]["xzraporyazici"].ToString();

                }
                lookUpEditHesapDokYazici.EditValue = dtXZ.Rows[0]["hesapyazici"].ToString();
            }
        }

        private void Kul_temizle()
        {
            Kul_comboyenile();
            txt_Kul_kod.SelectedIndex = -1;
            txt_Kul_sifre.Text = "";
            txt_Kul_ad.Text = "";
            txt_Kul_soyad.Text = "";
            txt_Kul_Kart.EditValue = "";
            cmb_Kulturu.SelectedIndex = -1;
            chkCmb_Departman.SetEditValue(null);
            U_BackUser.EditValue = null;
            Haklari_Al();
        }

        private void Haklari_Al()
        {
            chk_G_Miktarduzelt.Checked = false;
            chk_G_Tutarduzelt.Checked = false;
            chk_G_Satirsil.Checked = false;
            chk_G_Indirimsatis.Checked = false;
            chk_G_Hesapdokum.Checked = false;
            chk_G_Odemeal.Checked = false;
            chk_G_Odemesil.Checked = false;
            chk_G_Indirimhesap.Checked = false;
            chk_G_Yazdirkapat.Checked = false;
            chk_G_Yazdirmadankapat.Checked = false;
            chk_G_Bindirim.Checked = false;

            chk_M_Masatakip.Checked = false;
            chk_M_Satis.Checked = false;
            chk_M_MasaTransfer.Checked = false;
            chk_M_MalzemeTransfer.Checked = false;
            chk_M_Ozelmasa.Checked = false;
            chk_M_Odakontrol.Checked = false;
            chk_M_MasaKilitle.Checked = false;
            chk_M_HesapKapatma.Checked = false;
            chk_M_SatisRelogin.Checked = false;
            chk_M_HesapTr.Checked = false;

            chk_D_DirekSatis.Checked = false;

            chk_R_Raporlar.Checked = false;
            chk_R_Detay.Checked = false;
            chk_R_XZ.Checked = false;
            chk_R_MahsupKes.Checked = false;
            chk_R_Fisiptal.Checked = false;
            chk_R_Fisiptalgecmis.Checked = false;

            chk_A_Ayarlar.Checked = false;
            chk_A_Parametreler.Checked = false;
            chk_A_Print.Checked = false;
            chk_A_Odeme.Checked = false;
            chk_A_Entegre.Checked = false;
            chk_A_Masa.Checked = false;
            chk_A_Cari.Checked = false;
            chk_A_HH.Checked = false;
            chk_A_Kullanici.Checked = false;
            chk_A_Kasa.Checked = false;

            chk_GunSonu.Checked = false;

            chk_Pda_Masatakip.Checked = false;

            chk_Pda_Satis.Checked = false;
            chk_Pda_Satirsil.Checked = false;
            chk_Pda_Miktarduzelt.Checked = false;

            chk_Pda_Hesap.Checked = false;
            chk_Pda_Masatr.Checked = false;
            chk_Pda_Malztr.Checked = false;
            chk_Pda_Ozelmasa.Checked = false;
            chk_Pda_Odakontrol.Checked = false;

            chk_Pda_Direksatis.Checked = false;

            chk_K_Kasa.Checked = false;

            chk_And_Satis.Checked = false;
            chk_And_Satirsil.Checked = false;
            chk_And_MiktarD.Checked = false;
            chk_And_TutarD.Checked = false;
            chk_And_Hesap.Checked = false;
            chk_And_Ozelmasa.Checked = false;
            chk_And_MasaTr.Checked = false;
            chk_And_Giris.Checked = false;

            chkUser_Posta.SetEditValue(null);
            spn_Kul_Indirim.Value = 0;
            spn_Kul_Bindirim.Value = 0;

            txt_Kul_Masa.Text = "";
            chk_Kul_MasaAc.Checked = false;
            chk_M_Baskamasa.Checked = false;

            chk_G_SatirSilY.Checked = false;


            chk_Odeme.Checked = false;
            chk_Servis.Checked = false;
            chk_Cari.Checked = false;
            chk_Odenmez.Checked = false;
            chk_Malzeme.Checked = false;
            chk_Anagrup.Checked = false;
            chk_Altgrup.Checked = false;
            chk_Iptal.Checked = false;
            chk_PaketServis.Checked = false;
            chk_IndirimMasa.Checked = false;
            chk_YiyecekIcecek.Checked = false;
            chk_MasaKonum.Checked = false;
            chk_GarsonOzet.Checked = false;
            chk_GarsonTahsil.Checked = false;
            chk_SifirTutar.Checked = false;
            chk_OzetKasa.Checked = false;
            chk_ExtKasaRapor.Checked = false;
            chk_ExtKasaDetay.Checked = false;
            chk_SiparisIptal.Checked = false;
            chk_GonderilmemisSiparisIptal.Checked = false;
            chk_SiparisDuzelt.Checked = false;

            chk_Kul_GarsonDegistir.Checked = false;
            chk_Kul_Zayi.Checked = false;
            chk_Kul_Ikram.Checked = false;

            chk_Kul_KisiSayisi.Checked = false;
            chk_Kul_MasaGeri.Checked = false;

            chk_Kul_SiparisTekrar.Checked = false;
            chk_Pda_HesapDok.Checked = false;
            chk_H_HizliSatis.Checked = false;
            chk_R_TopluIsle.Checked = false;

            chk_And_HesapDokum.Checked = false;
            chk_And_HesapOdeme.Checked = false;
            chk_And_MalzTransfer.Checked = false;


            And_Yarim.Checked = false;
            And_Tam.Checked = false;
            And_Bucuk.Checked = false;
            And_Duble.Checked = false;
            Pos_SubeTrf.Checked = false;
            Pos_AdisyonPr.Checked = false;

            Pos_MasaAnlikDurum.Checked = false;
            Pos_ArtiEksi_Aktif.Checked = false;
            Pos_MasaUrunSil.Checked = false;
            Pos_IWERep.Checked = false;
            Pos_KartF_CheckOut.Checked = false;
            Pos_SatirSilYetkili.Checked = false;
            Pos_MasaDirekS.Checked = false;
            Pos_MasaPaketS.Checked = false;
            Pos_YS_YetkiReddet.Checked = false;
            Pos_YarimDubleAlan.Checked = false;
            Pos_ReceteTanimlama.Checked = false;
            Pos_FixMenu.Checked = false;
            Pos_HesapArti.Checked = false;
            Pos_OdaKontrol.Checked = false;
            Pos_HesapFisIptal.Checked = false;
            Pos_KartTanimSil.Checked = false;
            chk_K_KasaRapor.Checked = false;
            Pos_KartTanimDuzelt.Checked = false;
            Pos_KartTanimTransfer.Checked = false;
            Pos_KartTanimBakiyeTransfer.Checked = false;
            Pos_KartfIndirimAktif.Checked = false;
            Pos_ServisPayiDuzelt.Checked = false;

        }

        private void Haklari_Ver()
        {
            Pos_MasaAnlikDurum.Checked = true;
            Pos_ArtiEksi_Aktif.Checked = true;
            chk_G_Miktarduzelt.Checked = true;
            chk_G_Tutarduzelt.Checked = true;
            chk_G_Satirsil.Checked = true;
            chk_G_Indirimsatis.Checked = true;
            chk_G_Hesapdokum.Checked = true;
            chk_G_Odemeal.Checked = true;
            chk_G_Odemesil.Checked = true;
            chk_G_Indirimhesap.Checked = true;
            chk_G_Yazdirkapat.Checked = true;
            chk_G_Yazdirmadankapat.Checked = true;
            chk_G_Bindirim.Checked = true;

            chk_M_Masatakip.Checked = true;
            chk_M_Satis.Checked = true;
            chk_M_MasaTransfer.Checked = true;
            chk_M_MalzemeTransfer.Checked = true;
            chk_M_Ozelmasa.Checked = true;
            chk_M_Odakontrol.Checked = true;
            chk_M_MasaKilitle.Checked = true;
            chk_M_HesapKapatma.Checked = true;
            chk_M_SatisRelogin.Checked = false;
            chk_M_HesapTr.Checked = true;

            chk_D_DirekSatis.Checked = true;

            chk_R_Raporlar.Checked = true;
            chk_R_Detay.Checked = true;
            chk_R_XZ.Checked = true;
            chk_R_MahsupKes.Checked = true;
            chk_R_Fisiptal.Checked = true;
            chk_R_Fisiptalgecmis.Checked = true;

            chk_A_Ayarlar.Checked = true;
            chk_A_Parametreler.Checked = true;
            chk_A_Print.Checked = true;
            chk_A_Odeme.Checked = true;
            chk_A_Entegre.Checked = true;
            chk_A_Masa.Checked = true;
            chk_A_Cari.Checked = true;
            chk_A_HH.Checked = true;
            chk_A_Kullanici.Checked = true;
            chk_A_Kasa.Checked = true;

            chk_GunSonu.Checked = true;

            chk_Pda_Masatakip.Checked = true;

            chk_Pda_Satis.Checked = true;
            chk_Pda_Satirsil.Checked = true;
            chk_Pda_Miktarduzelt.Checked = true;

            chk_Pda_Hesap.Checked = true;
            chk_Pda_Masatr.Checked = true;
            chk_Pda_Malztr.Checked = true;
            chk_Pda_Ozelmasa.Checked = true;
            chk_Pda_Odakontrol.Checked = true;

            chk_Pda_Direksatis.Checked = true;

            chk_K_Kasa.Checked = true;

            chk_And_Satis.Checked = true;
            chk_And_Satirsil.Checked = true;
            chk_And_MiktarD.Checked = true;
            chk_And_TutarD.Checked = true;
            chk_And_Hesap.Checked = true;
            chk_And_Ozelmasa.Checked = true;
            chk_And_MasaTr.Checked = true;
            chk_And_Giris.Checked = true;

            chkUser_Posta.SetEditValue(null);
            spn_Kul_Indirim.Value = 100;
            spn_Kul_Bindirim.Value = 100;
            txt_Kul_Masa.Text = "";
            chk_Kul_MasaAc.Checked = true;
            chk_M_Baskamasa.Checked = true;

            chk_G_SatirSilY.Checked = true;


            chk_Odeme.Checked = true;
            chk_Servis.Checked = true;
            chk_Cari.Checked = true;
            chk_Odenmez.Checked = true;
            chk_Malzeme.Checked = true;
            chk_Anagrup.Checked = true;
            chk_Altgrup.Checked = true;
            chk_Iptal.Checked = true;
            chk_PaketServis.Checked = true;
            chk_IndirimMasa.Checked = true;
            chk_YiyecekIcecek.Checked = true;
            chk_MasaKonum.Checked = true;
            chk_GarsonOzet.Checked = true;
            chk_GarsonTahsil.Checked = true;
            chk_SifirTutar.Checked = true;
            chk_OzetKasa.Checked = true;
            chk_ExtKasaRapor.Checked = true;
            chk_ExtKasaDetay.Checked = true;
            chk_SiparisIptal.Checked = true;
            chk_GonderilmemisSiparisIptal.Checked = true;
            chk_SiparisDuzelt.Checked = true;

            chk_Kul_GarsonDegistir.Checked = true;
            chk_Kul_Zayi.Checked = true;
            chk_Kul_Ikram.Checked = true;

            chk_Kul_KisiSayisi.Checked = true;
            chk_Kul_MasaGeri.Checked = true;

            chk_Kul_SiparisTekrar.Checked = true;
            chk_Pda_HesapDok.Checked = true;
            chk_H_HizliSatis.Checked = true;
            chk_R_TopluIsle.Checked = true;

            chk_And_HesapDokum.Checked = true;
            chk_And_HesapOdeme.Checked = true;
            chk_And_MalzTransfer.Checked = true;

            And_Yarim.Checked = true;
            And_Tam.Checked = true;
            And_Bucuk.Checked = true;
            And_Duble.Checked = true;
            Pos_SubeTrf.Checked = true;
            Pos_AdisyonPr.Checked = false;
            And_SatisSiparisBtn.Checked = true;
            Pos_MasaUrunSil.Checked = true;
            //Pos_IWERep.Checked = true;
            Pos_KartF_CheckOut.Checked = true;
            Pos_MasaDirekS.Checked = true;
            Pos_MasaPaketS.Checked = true;
            Pos_YarimDubleAlan.Checked = true;
            Pos_ReceteTanimlama.Checked = true;
            User_AP.Checked = true;
            Pos_OdaKontrol.Checked = true;
            Pos_HesapFisIptal.Checked = true;
            Pos_KartTanimSil.Checked = true;
            chk_K_KasaRapor.Checked = true;

            Pos_KartTanimDuzelt.Checked = true;
            Pos_KartTanimTransfer.Checked = true;
            Pos_KartTanimBakiyeTransfer.Checked = true;
            Pos_KartfIndirimAktif.Checked = true;
            Pos_ServisPayiDuzelt.Checked = true;
            //Pos_FixMenu.Checked = true;
            //Pos_HesapArti.Checked = true;




        }

        private void btn_Kul_TumYetki_Click(object sender, EventArgs e)
        {
            Haklari_Ver();
        }

        private void btn_Kul_HaklariAl_Click(object sender, EventArgs e)
        {
            Haklari_Al();
        }

        private void Kul_comboyenile()
        {
            look_Kul_Kod.EditValue = null;

            string filtre = "";

            if (User.P_Kulturu != 3)
            {
                filtre = " where P_Kulturu <> 3 ";
                cmb_Kulturu.Properties.Items.Remove("Yönetici");
            }

            txt_Kul_kod.Properties.Items.Clear();
            DataTable dt = dbtools.SelectTable("select P_Kod,P_Ad +' '+P_Soyad as P_Ad from Rmosmuh.dbo.Pos_User with(nolock) " + filtre + " order by P_Kod");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt_Kul_kod.Properties.Items.Add(dt.Rows[i]["P_Kod"]);
            }
            look_Kul_Kod.Properties.DataSource = dt;
            look_Kul_Kod.Properties.DisplayMember = "P_Ad";
            look_Kul_Kod.Properties.ValueMember = "P_Kod";


            DataTable dtDep = dbtools.SelectTable("select Kodlar_Kod,Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = '01' and Kodlar_Satis = 1 order by Kodlar_Kod");
            chkCmb_Departman.Properties.DataSource = dtDep;
            chkCmb_Departman.Properties.DisplayMember = "Kodlar_Ad";
            chkCmb_Departman.Properties.ValueMember = "Kodlar_Kod";

            DataTable dtPosta = dbtools.SelectTable("select posta.Pkod_Id,ISNULL(posta.Pkod_Kod,'') + ' - ' + ISNULL(posta.Pkod_Ad,'') + ' - ' + ISNULL(konum.Pkod_Ad,'')  + ' - ' + ISNULL(dep.Kodlar_Ad,'') as Ad "
                                + " from Pos_Kodlar as posta "
                                + " left join Stok_Kodlar as dep on posta.Pkod_Dep = dep.Kodlar_Kod and dep.Kodlar_Sinif = '01' "
                                + " left join Pos_Kodlar as konum on posta.Pkod_Konumkod = konum.Pkod_Konumkod and posta.Pkod_Dep = konum.Pkod_Kod and konum.Pkod_Sinif = '14' "
                                + " where posta.Pkod_Sinif = '18' ");
            chkUser_Posta.Properties.DataSource = dtPosta;
            chkUser_Posta.Properties.DisplayMember = "Ad";
            chkUser_Posta.Properties.ValueMember = "Pkod_Id";
        }

        private void look_Kul_Kod_EditValueChanged(object sender, EventArgs e)
        {
            txt_Kul_kod.Text = Convert.ToString(look_Kul_Kod.EditValue);
            text_kod_Leave(null, null);
        }

        private void btn_Kul_Cıkıs_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Mac Printer Ayarları
        private void gridyenile_MacPr()
        {
            gridColumn59.FieldName = "Pkod_Mac";
            gridColumn61.FieldName = "Pkod_Ad";
            gridColumn62.FieldName = "Pkod_Satir";
            gridColumn101.FieldName = "Pkod_Ustgrup";
            gridColumn102.FieldName = "Pkod_Altgrup";
            gridColumn103.FieldName = "ana";
            gridColumn104.FieldName = "alt";

            DataTable dt = dbtools.SelectTable("select Pkod_Mac,Pkod_Ustgrup,Pkod_Altgrup,Pkod_Ad,Pkod_Satir,ISNULL(Ana.Kodlar_Ad,Pkod_Ustgrup) as ana,Alt.Kodlar_Ad as alt from Pos_Kodlar "
                    + " left join Stok_Kodlar as Ana on Pkod_Ustgrup = Ana.Kodlar_Kod and Ana.Kodlar_Sinif = '08' "
                    + " left join Stok_Kodlar as Alt on Pkod_Altgrup = Alt.Kodlar_Kod and Alt.Kodlar_Sinif = '09' and Alt.Kodlar_Anagrup = ana.Kodlar_Kod  "
                    + " where Pkod_Sinif = '21' ");
            grd_MacPr.DataSource = dt;
        }

        private void btn_MacPr_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(look_MacPr_Anagrup.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("Grup İsmi Boş Geçilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Mac = '" + txt_MacPr_Mac.EditValue + "' and Pkod_Ustgrup = '" + look_MacPr_Anagrup.EditValue + "' and Pkod_Altgrup = '" + look_MacPr_Altgrup.EditValue + "' and  Pkod_Sinif = '21'");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Mac, Pkod_Ustgrup,Pkod_Altgrup,Pkod_Ad,Pkod_Sinif,Pkod_Satir) VALUES ('" + txt_MacPr_Mac.EditValue + "','" + look_MacPr_Anagrup.EditValue + "','" + look_MacPr_Altgrup.EditValue + "' ,'" + cmb_MacPr_Printer.EditValue + "','21','" + spn_MacPr_BosSatir.Value.ToString() + "')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MacPrint, Log.Log_Islem.Kaydet, "Mac :" + txt_MacPr_Mac.EditValue + " Printer :" + cmb_MacPr_Printer.EditValue + " Bos Satir: " + spn_MacPr_BosSatir.Value.ToString() + " Ana:" + look_MacPr_Anagrup.EditValue + " Alt:" + look_MacPr_Altgrup.EditValue + "  Eklendi.", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_Ad =  '" + cmb_MacPr_Printer.EditValue + "', Pkod_Satir = '" + spn_MacPr_BosSatir.Value + "' where Pkod_Mac = '" + txt_MacPr_Mac.EditValue + "' and Pkod_Ustgrup = '" + look_MacPr_Anagrup.EditValue + "' and Pkod_Altgrup = '" + look_MacPr_Altgrup.EditValue + "' and  Pkod_Sinif = '21'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MacPrint, Log.Log_Islem.Duzelt, "Mac :" + txt_MacPr_Mac.EditValue + " Printer :" + cmb_MacPr_Printer.EditValue + " Bos Satir: " + spn_MacPr_BosSatir.Value.ToString() + " Ana:" + look_MacPr_Anagrup.EditValue + " Alt:" + look_MacPr_Altgrup.EditValue + " Duzeltildi.", String.Empty, String.Empty);
            }
            gridyenile_MacPr();
        }

        private void btn_MacPr_Sil_Click(object sender, EventArgs e)
        {
            DialogResult c;
            c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (c == DialogResult.Yes)
            {
                dbtools.execcmd("delete from Pos_Kodlar where  Pkod_Mac = '" + txt_MacPr_Mac.EditValue + "' and Pkod_Ustgrup = '" + look_MacPr_Anagrup.EditValue + "' and Pkod_Altgrup = '" + look_MacPr_Altgrup.EditValue + "' and  Pkod_Sinif = '21'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MacPrint, Log.Log_Islem.Sil, "Mac :" + txt_MacPr_Mac.EditValue + " Printer :" + cmb_MacPr_Printer.EditValue + " Bos Satir: " + spn_MacPr_BosSatir.Value.ToString() + " Ana:" + look_MacPr_Anagrup.EditValue + " Alt:" + look_MacPr_Altgrup.EditValue + " Silindi.", String.Empty, String.Empty);
                gridyenile_MacPr();
            }
        }

        private void btn_MacPr_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView11_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView11.RowCount > 0)
            {
                look_MacPr_Anagrup.EditValue = Convert.ToString(gridView11.GetFocusedRowCellValue("Pkod_Ustgrup"));
                look_MacPr_Altgrup.EditValue = Convert.ToString(gridView11.GetFocusedRowCellValue("Pkod_Altgrup"));
                cmb_MacPr_Printer.EditValue = Convert.ToString(gridView11.GetFocusedRowCellValue("Pkod_Ad"));
                spn_MacPr_BosSatir.EditValue = Convert.ToInt32(gridView11.GetFocusedRowCellValue("Pkod_Satir"));
            }
        }

        private void look_MacPr_Anagrup_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(look_MacPr_Anagrup.EditValue) == "HES" || Convert.ToString(look_MacPr_Anagrup.EditValue) == "ADI" || Convert.ToString(look_MacPr_Anagrup.EditValue) == "FAT" || Convert.ToString(look_MacPr_Anagrup.EditValue) == "PKT" || Convert.ToString(look_MacPr_Anagrup.EditValue) == "KSM")
            {
                look_MacPr_Altgrup.Visible = false;
                look_MacPr_Altgrup.EditValue = null;
                return;
            }
            else
            {
                look_PrintAyar_AltGrup.Visible = true;
            }

            DataTable dt = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif = '09'  and Kodlar_Anagrup = '" + Convert.ToString(look_MacPr_Anagrup.EditValue) + "'  order by Kodlar_Kod");

            look_MacPr_Altgrup.Properties.DataSource = dt;
            look_MacPr_Altgrup.Properties.DisplayMember = "Kodlar_Ad";
            look_MacPr_Altgrup.Properties.ValueMember = "Kodlar_Kod";
        }
        #endregion

        #region  Fisi Çıktı Ayarı

        private void btn_Hesap_Font_Click(object sender, EventArgs e)
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
            Font fnt = (Font)converter.ConvertFromString(txt_Hesap_Font.Text);
            fontDialog1.Font = fnt;

            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_Hesap_Font.Text = converter.ConvertToString(fontDialog1.Font);
            }
        }

        private void btn_Iptal_Font_Click(object sender, EventArgs e)
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
            Font fnt = (Font)converter.ConvertFromString(txt_Iptal_Font.Text);
            fontDialog1.Font = fnt;

            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_Iptal_Font.Text = converter.ConvertToString(fontDialog1.Font);
            }
        }

        private void btn_Siparis_Font_Click(object sender, EventArgs e)
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
            Font fnt = (Font)converter.ConvertFromString(txt_Siparis_Font.Text);
            fontDialog1.Font = fnt;

            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_Siparis_Font.Text = converter.ConvertToString(fontDialog1.Font);
            }
        }

        private void btn_Mars_Font_Click(object sender, EventArgs e)
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
            Font fnt = (Font)converter.ConvertFromString(txt_Mars_Font.Text);
            fontDialog1.Font = fnt;

            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_Mars_Font.Text = converter.ConvertToString(fontDialog1.Font);
            }
        }

        private void btn_Paket_Font_Click(object sender, EventArgs e)
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
            Font fnt = (Font)converter.ConvertFromString(txt_Paket_Font.Text);
            fontDialog1.Font = fnt;

            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_Paket_Font.Text = converter.ConvertToString(fontDialog1.Font);
            }
        }

        private void btn_Kasa_Font_Click(object sender, EventArgs e)
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
            Font fnt = (Font)converter.ConvertFromString(txt_Kasa_Font.Text);
            fontDialog1.Font = fnt;

            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_Kasa_Font.Text = converter.ConvertToString(fontDialog1.Font);
            }
        }

        private void btn_XZ_Font_Click(object sender, EventArgs e)
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
            Font fnt = (Font)converter.ConvertFromString(txt_XZ_Font.Text);
            fontDialog1.Font = fnt;

            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_XZ_Font.Text = converter.ConvertToString(fontDialog1.Font);
            }
        }

        private void btn_Cari_Font_Click(object sender, EventArgs e)
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
            Font fnt = (Font)converter.ConvertFromString(txt_Cari_Font.Text);
            fontDialog1.Font = fnt;

            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_Cari_Font.Text = converter.ConvertToString(fontDialog1.Font);
            }
        }

        public void abuyerCiktiKaydet()
        {
            try
            {
                if (Convert.ToInt32(dbtools.DegerGetir("select count(*) from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'ABUYERSAYI' ")) > 0)
                {
                    dbtools.execcmd("update Pos_Kodlar set Pkod_Ciktisayisi = '" + Convert.ToInt32(spinEditAbuyerCiktiSayisi.EditValue) + "' where Pkod_Sinif = '17' and Pkod_Kod = 'ABUYERSAYI'");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Duzelt, "abuyer çıktı sayısı" + txt_Hesap_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spinEditAbuyerCiktiSayisi.EditValue).ToString(), String.Empty, String.Empty);
                }
                else
                {
                    dbtools.execcmd(" insert into Pos_Kodlar(Pkod_Kod,Pkod_Sinif,Pkod_Ciktisayisi) "
                            + " values ('ABUYERSAYI','17','" + Convert.ToInt32(spinEditAbuyerCiktiSayisi.EditValue) + "') ");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, "abuyer çıktı sayısı" + txt_Hesap_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spinEditAbuyerCiktiSayisi.EditValue).ToString(), String.Empty, String.Empty);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btn_Hesap_Kaydet_Click(object sender, EventArgs e)
        {
            abuyerCiktiKaydet();
            //Hesap
            if (Convert.ToInt32(dbtools.DegerGetir("select count(*) from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'HESAP' ")) > 0)
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Font = '" + txt_Hesap_Font.Text + "',Pkod_Ciktisayisi = '" + Convert.ToInt32(spn_Hesap_Ciktisayisi.EditValue) + "' where Pkod_Sinif = '17' and Pkod_Kod = 'HESAP'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Duzelt, "Hesap Font" + txt_Hesap_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Hesap_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd(" insert into Pos_Kodlar(Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi) "
                        + " values ('HESAP','17','" + txt_Hesap_Font.Text + "','" + Convert.ToInt32(spn_Hesap_Ciktisayisi.EditValue) + "') ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, "Hesap Font" + txt_Hesap_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Hesap_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }

            //Iptal
            if (Convert.ToInt32(dbtools.DegerGetir("select count(*) from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'IPTAL' ")) > 0)
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Font = '" + txt_Iptal_Font.Text + "',Pkod_Ciktisayisi = '" + Convert.ToInt32(spn_Iptal_Ciktisayisi.EditValue) + "' where Pkod_Sinif = '17' and Pkod_Kod = 'IPTAL'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_IptalFis, Log.Log_Islem.Duzelt, "Iptal Font" + txt_Iptal_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Iptal_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd(" insert into Pos_Kodlar(Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi) "
                        + " values ('IPTAL','17','" + txt_Iptal_Font.Text + "','" + Convert.ToInt32(spn_Iptal_Ciktisayisi.EditValue) + "') ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_IptalFis, Log.Log_Islem.Kaydet, "Iptal Font" + txt_Iptal_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Iptal_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }

            //Siparis
            if (Convert.ToInt32(dbtools.DegerGetir("select count(*) from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'SIPARIS' ")) > 0)
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Font = '" + txt_Siparis_Font.Text + "',Pkod_Ciktisayisi = '" + Convert.ToInt32(spn_Siparis_Ciktisayisi.EditValue) + "' where Pkod_Sinif = '17' and Pkod_Kod = 'SIPARIS'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "Siparis Font" + txt_Siparis_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Siparis_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd(" insert into Pos_Kodlar(Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi) "
                        + " values ('SIPARIS','17','" + txt_Siparis_Font.Text + "','" + Convert.ToInt32(spn_Siparis_Ciktisayisi.EditValue) + "') ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "Siparis Font" + txt_Siparis_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Siparis_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }

            //Mars
            if (Convert.ToInt32(dbtools.DegerGetir("select count(*) from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'MARS' ")) > 0)
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Font = '" + txt_Mars_Font.Text + "',Pkod_Ciktisayisi = '" + Convert.ToInt32(spn_Mars_Ciktisayisi.EditValue) + "' where Pkod_Sinif = '17' and Pkod_Kod = 'MARS'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "Mars Font" + txt_Mars_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Mars_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd(" insert into Pos_Kodlar(Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi) "
                        + " values ('MARS','17','" + txt_Mars_Font.Text + "','" + Convert.ToInt32(spn_Mars_Ciktisayisi.EditValue) + "') ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "Mars Font" + txt_Mars_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Mars_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }

            //Paket
            if (Convert.ToInt32(dbtools.DegerGetir("select count(*) from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'PAKET' ")) > 0)
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Font = '" + txt_Paket_Font.Text + "',Pkod_Ciktisayisi = '" + Convert.ToInt32(spn_Paket_Ciktisayisi.EditValue) + "' where Pkod_Sinif = '17' and Pkod_Kod = 'PAKET'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "Paket Font" + txt_Paket_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Paket_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd(" insert into Pos_Kodlar(Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi) "
                        + " values ('PAKET','17','" + txt_Paket_Font.Text + "','" + Convert.ToInt32(spn_Paket_Ciktisayisi.EditValue) + "') ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "Paket Font" + txt_Paket_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Paket_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }

            //Kasa Makbuz
            if (Convert.ToInt32(dbtools.DegerGetir("select count(*) from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'KASAMAKBUZ' ")) > 0)
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Font = '" + txt_Kasa_Font.Text + "',Pkod_Ciktisayisi = '" + Convert.ToInt32(spn_Kasa_Ciktisayisi.EditValue) + "' where Pkod_Sinif = '17' and Pkod_Kod = 'KASAMAKBUZ'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "Kasa Font" + txt_Kasa_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Kasa_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd(" insert into Pos_Kodlar(Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi) "
                        + " values ('KASAMAKBUZ','17','" + txt_Kasa_Font.Text + "','" + Convert.ToInt32(spn_Kasa_Ciktisayisi.EditValue) + "') ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "Kasa Font" + txt_Kasa_Font.Text + " Ciktisayisi :" + Convert.ToInt32(spn_Kasa_Ciktisayisi.EditValue).ToString(), String.Empty, String.Empty);
            }

            //XZ
            if (Convert.ToInt32(dbtools.DegerGetir("select count(*) from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'XZRAPOR' ")) > 0)
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Font = '" + txt_XZ_Font.Text + "' where Pkod_Sinif = '17' and Pkod_Kod = 'XZRAPOR'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "XZ Font" + txt_XZ_Font.Text, String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd(" insert into Pos_Kodlar(Pkod_Kod,Pkod_Sinif,Pkod_Font) "
                        + " values ('XZRAPOR','17','" + txt_XZ_Font.Text + "') ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "XZ Font" + txt_XZ_Font.Text, String.Empty, String.Empty);
            }

            //CARI HESAP
            if (Convert.ToInt32(dbtools.DegerGetir("select count(*) from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'CARIHESAP' ")) > 0)
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Font = '" + txt_Cari_Font.Text + "' where Pkod_Sinif = '17' and Pkod_Kod = 'CARIHESAP'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "CARIHESAP Font" + txt_Cari_Font.Text, String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd(" insert into Pos_Kodlar(Pkod_Kod,Pkod_Sinif,Pkod_Font) "
                        + " values ('CARIHESAP','17','" + txt_Cari_Font.Text + "') ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "CARIHESAP Font" + txt_Cari_Font.Text, String.Empty, String.Empty);
            }
        }

        private void Hesap_Yenile()
        {
            DataTable dtHesap = dbtools.SelectTable("select Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'HESAP'");
            if (dtHesap.Rows.Count > 0)
            {
                txt_Hesap_Font.Text = Convert.ToString(dtHesap.Rows[0]["Pkod_Font"]);
                spn_Hesap_Ciktisayisi.EditValue = Convert.ToInt32(dtHesap.Rows[0]["Pkod_Ciktisayisi"]);
            }
            else
            {
                txt_Hesap_Font.Text = "";
                spn_Hesap_Ciktisayisi.EditValue = 1;
            }

            DataTable dtIptal = dbtools.SelectTable("select Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'IPTAL'");
            if (dtIptal.Rows.Count > 0)
            {
                txt_Iptal_Font.Text = Convert.ToString(dtIptal.Rows[0]["Pkod_Font"]);
                spn_Iptal_Ciktisayisi.EditValue = Convert.ToInt32(dtIptal.Rows[0]["Pkod_Ciktisayisi"]);
            }
            else
            {
                txt_Iptal_Font.Text = "";
                spn_Iptal_Ciktisayisi.EditValue = 1;
            }

            DataTable dtSiparis = dbtools.SelectTable("select Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'SIPARIS'");
            if (dtSiparis.Rows.Count > 0)
            {
                txt_Siparis_Font.Text = Convert.ToString(dtSiparis.Rows[0]["Pkod_Font"]);
                spn_Siparis_Ciktisayisi.EditValue = Convert.ToInt32(dtSiparis.Rows[0]["Pkod_Ciktisayisi"]);
            }
            else
            {
                txt_Siparis_Font.Text = "";
                spn_Siparis_Ciktisayisi.EditValue = 1;
            }

            DataTable dtMars = dbtools.SelectTable("select Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'MARS'");
            if (dtMars.Rows.Count > 0)
            {
                txt_Mars_Font.Text = Convert.ToString(dtMars.Rows[0]["Pkod_Font"]);
                spn_Mars_Ciktisayisi.EditValue = Convert.ToInt32(dtMars.Rows[0]["Pkod_Ciktisayisi"]);
            }
            else
            {
                txt_Mars_Font.Text = "";
                spn_Mars_Ciktisayisi.EditValue = 1;
            }

            DataTable dtPaket = dbtools.SelectTable("select Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'PAKET'");
            if (dtPaket.Rows.Count > 0)
            {
                txt_Paket_Font.Text = Convert.ToString(dtPaket.Rows[0]["Pkod_Font"]);
                spn_Paket_Ciktisayisi.EditValue = Convert.ToInt32(dtPaket.Rows[0]["Pkod_Ciktisayisi"]);
            }
            else
            {
                txt_Paket_Font.Text = "";
                spn_Paket_Ciktisayisi.EditValue = 1;
            }

            DataTable dtKasa = dbtools.SelectTable("select Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'KASAMAKBUZ'");
            if (dtKasa.Rows.Count > 0)
            {
                txt_Kasa_Font.Text = Convert.ToString(dtKasa.Rows[0]["Pkod_Font"]);
                spn_Kasa_Ciktisayisi.EditValue = Convert.ToInt32(dtKasa.Rows[0]["Pkod_Ciktisayisi"]);
            }
            else
            {
                txt_Kasa_Font.Text = "";
                spn_Kasa_Ciktisayisi.EditValue = 1;
            }

            DataTable dtAbuyer = dbtools.SelectTable("select Pkod_Kod,Pkod_Sinif,Pkod_Font,Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'ABUYERSAYI'");
            if (dtAbuyer.Rows.Count > 0)
            {
                spinEditAbuyerCiktiSayisi.EditValue = Convert.ToInt32(dtAbuyer.Rows[0]["Pkod_Ciktisayisi"]);
            }
            else
            {
                spinEditAbuyerCiktiSayisi.EditValue = 1;
            }

            DataTable dtZX = dbtools.SelectTable("select Pkod_Kod,Pkod_Sinif,Pkod_Font from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'XZRAPOR'");
            if (dtZX.Rows.Count > 0)
            {
                txt_XZ_Font.Text = Convert.ToString(dtZX.Rows[0]["Pkod_Font"]);
            }
            else
            {
                txt_XZ_Font.Text = "";
            }

            DataTable dtCARIHESAP = dbtools.SelectTable("select Pkod_Kod,Pkod_Sinif,Pkod_Font from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'CARIHESAP'");
            if (dtCARIHESAP.Rows.Count > 0)
            {
                txt_Cari_Font.Text = Convert.ToString(dtCARIHESAP.Rows[0]["Pkod_Font"]);
            }
            else
            {
                txt_Cari_Font.Text = "";
            }
        }


        #endregion

        #region Pda Parametreleri
        private void gridyenile_Pda()
        {
            DataTable dt = dbtools.SelectTable("SELECT isnull(Param_Pda_Height,300) as Param_Pda_Height,isnull(Param_Pda_Width,250) as Param_Pda_Width,isnull(Param_Pda_Fullscreen,0) as Param_Pda_Fullscreen  FROM Pos_Param WITH(NOLOCK) WHERE Param_Id = '1'");

            if (dt.Rows.Count > 0)
            {
                spn_Pda_Height.EditValue = Convert.ToInt32(dt.Rows[0]["Param_Pda_Height"]);
                spn_Pda_Width.EditValue = Convert.ToInt32(dt.Rows[0]["Param_Pda_Width"]);
                chk_Pda_FullScreeen.Checked = Convert.ToBoolean(dt.Rows[0]["Param_Pda_Fullscreen"]);
            }
        }

        private void btn_Pda_Kaydet_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select * from Pos_Param where Param_Id = '1'");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Param(Param_Id,Param_Pda_Height,Param_Pda_Width,Param_Pda_Fullscreen) "
                    + " VALUES('1','" + Convert.ToInt32(spn_Pda_Height.EditValue).ToString() + "','" + Convert.ToInt32(spn_Pda_Width.EditValue).ToString() + "','" + Convert.ToBoolean(chk_Pda_FullScreeen.Checked) + "')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Pda, Log.Log_Islem.Kaydet, "Pda Parametrelerde Kayıt Islemi Yapıldı", String.Empty, "1");
            }
            else
            {
                dbtools.execcmd("UPDATE Pos_Param set Param_Pda_Height = '" + Convert.ToInt32(spn_Pda_Height.EditValue).ToString() + "',Param_Pda_Width = '" + Convert.ToInt32(spn_Pda_Width.EditValue).ToString() + "', "
                    + " Param_Pda_Fullscreen = '" + Convert.ToBoolean(chk_Pda_FullScreeen.Checked) + "' "
                    + " WHERE Param_Id = '1' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Pda, Log.Log_Islem.Duzelt, "Pda Parametrelerde Duzeltme Islemi Yapıldı", String.Empty, "1");
            }
        }

        private void btn_Pda_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Posta Ayarları
        private void gridyenile_Posta()
        {
            gridColumn63.FieldName = "Pkod_Kod";
            gridColumn64.FieldName = "Pkod_Ad";
            gridColumn65.FieldName = "Pkod_Dep";
            gridColumn66.FieldName = "Pkod_Konumkod";

            grd_Posta.DataSource = dbtools.SelectTable("select Pkod_Konumkod,Pkod_Ad,Pkod_Kod,Pkod_Dep,Pkod_Sira from Pos_Kodlar where Pkod_Sinif = '18' and Pkod_Dep = '" + Convert.ToString(look_Posta_Dep.EditValue) + "' order by Pkod_Kod");

            txt_Posta_Kod.Text = "";
            txt_Posta_Ad.Text = "";
        }

        private void look_Posta_Dep_EditValueChanged(object sender, EventArgs e)
        {
            look_Posta_Konum.Properties.DataSource = dbtools.SelectTable("select Pkod_Konumkod,Pkod_Ad from Pos_Kodlar where Pkod_Sinif = '14' and Pkod_Kod = '" + Convert.ToString(look_Posta_Dep.EditValue) + "'");
            look_Posta_Konum.Properties.DisplayMember = "Pkod_Ad";
            look_Posta_Konum.Properties.ValueMember = "Pkod_Konumkod";

            gridyenile_Posta();
        }

        private void btn_Posta_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(look_Posta_Dep.EditValue) == "" || Convert.ToString(look_Posta_Konum.EditValue) == "" || Convert.ToString(txt_Posta_Kod.Text) == "" || Convert.ToString(txt_Posta_Ad.Text) == "")
            {
                MessageBox.Show(res_man.GetString("Tüm Alanları Doldurunuz...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + txt_Posta_Kod.Text + "' and  Pkod_Sinif = '18' AND Pkod_Dep = '" + Convert.ToString(look_Posta_Dep.EditValue) + "' AND Pkod_Konumkod = '" + Convert.ToString(look_Posta_Konum.EditValue) + "'");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod, Pkod_Ad, Pkod_Dep,Pkod_Konumkod, Pkod_Sinif,Pkod_Sira) VALUES ('" + txt_Posta_Kod.Text + "','" + txt_Posta_Ad.Text + "','" + Convert.ToString(look_Posta_Dep.EditValue) + "','" + Convert.ToString(look_Posta_Konum.EditValue) + "', '18','" + spinEditPostaSira.Text + "')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Posta, Log.Log_Islem.Kaydet, look_Posta_Dep.EditValue + " Departmanında " + Convert.ToString(look_Posta_Konum.EditValue) + " Konumunda " + txt_Posta_Kod.Text + " - " + txt_Posta_Ad.Text + " Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_Ad =  '" + txt_Posta_Ad.Text + "',Pkod_Sira =  '" + spinEditPostaSira.Text + "' where Pkod_Kod = '" + txt_Posta_Kod.Text + "' and  Pkod_Sinif = '18' AND Pkod_Dep = '" + Convert.ToString(look_Posta_Dep.EditValue) + "' AND Pkod_Konumkod = '" + Convert.ToString(look_Posta_Konum.EditValue) + "'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Posta, Log.Log_Islem.Duzelt, look_Posta_Dep.EditValue + " Departmanında " + Convert.ToString(look_Posta_Konum.EditValue) + " Konumunda " + txt_Posta_Kod.Text + " - " + txt_Posta_Ad.Text + " Düzeltildi", String.Empty, String.Empty);
            }

            gridyenile_Posta();
        }

        private void btn_Posta_Sil_Click(object sender, EventArgs e)
        {
            if (gridView12.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Kodlar where Pkod_Kod = '" + txt_Posta_Kod.Text + "' and  Pkod_Sinif = '18' AND Pkod_Dep = '" + Convert.ToString(look_Posta_Dep.EditValue) + "' AND Pkod_Konumkod = '" + Convert.ToString(look_Posta_Konum.EditValue) + "'");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Posta, Log.Log_Islem.Sil, look_Posta_Dep.EditValue + " Departmanında " + Convert.ToString(look_Posta_Konum.EditValue) + " Konumunda " + txt_Posta_Kod.Text + " - " + txt_Posta_Ad.Text + " Silindi", String.Empty, String.Empty);
                    gridyenile_Posta();
                }
            }
        }

        private void gridView12_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView12.RowCount > 0)
            {
                txt_Posta_Kod.EditValue = gridView12.GetFocusedRowCellValue("Pkod_Kod");
                txt_Posta_Ad.EditValue = gridView12.GetFocusedRowCellValue("Pkod_Ad");
                look_Posta_Dep.EditValue = gridView12.GetFocusedRowCellValue("Pkod_Dep");
                look_Posta_Konum.EditValue = gridView12.GetFocusedRowCellValue("Pkod_Konumkod");
                spinEditPostaSira.Text = gridView12.GetFocusedRowCellValue("Pkod_Sira").ToString();


            }
        }

        private void btn_Posta_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Printer Tanımları
        private void gridyenile_PrintTanim()
        {
            gridColumn67.FieldName = "Pkod_Kod";
            gridColumn68.FieldName = "Pkod_Ad";
            gridColumn69.FieldName = "Pkod_Printer";
            gridColumn70.FieldName = "Pkod_Ek_Pr1";
            gridColumn71.FieldName = "Pkod_Ek_Pr2";
            gridColumn72.FieldName = "Pkod_Ek_Pr3";
            gridColumn80.FieldName = "Pkod_Ip";
            gridColumn81.FieldName = "Pkod_Port";
            gridColumn82.FieldName = "Pkod_Ek1_Ip";
            gridColumn83.FieldName = "Pkod_Ek1_Port";
            gridColumn84.FieldName = "Pkod_Ek2_Ip";
            gridColumn85.FieldName = "Pkod_Ek2_Port";
            gridColumn86.FieldName = "Pkod_Ek3_Ip";
            gridColumn87.FieldName = "Pkod_Ek3_Port";

            grd_Tanim.DataSource = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad,Pkod_Sinif,Pkod_Printer,Pkod_Ek_Pr1,Pkod_Ek_Pr2,Pkod_Ek_Pr3,Pkod_Ip,ISNULL(Pkod_Port,0) as Pkod_Port, "
                        + " Pkod_Ek1_Ip,ISNULL(Pkod_Ek1_Port,0)  as Pkod_Ek1_Port, "
                        + " Pkod_Ek2_Ip,ISNULL(Pkod_Ek2_Port,0)  as Pkod_Ek2_Port, "
                        + " Pkod_Ek3_Ip,ISNULL(Pkod_Ek3_Port,0)  as Pkod_Ek3_Port from Pos_Kodlar where Pkod_Sinif = '19' order by Pkod_Kod");

            txt_Tanim_Kod.EditValue = null;
            txt_Tanim_Ad.EditValue = null;
            cmb_Tanim_Pr.EditValue = null;
            cmb_Tanim_Ek1.EditValue = null;
            cmb_Tanim_Ek2.EditValue = null;
            cmb_Tanim_Ek3.EditValue = null;
            txt_Tanim_Ip.Text = String.Empty;
            spn_Tanim_Port.EditValue = 0;
            txt_TanimEk1_Ip.Text = String.Empty;
            spn_TanimEk1_Port.EditValue = 0;
            txt_TanimEk2_Ip.Text = String.Empty;
            spn_TanimEk2_Port.EditValue = 0;
            txt_TanimEk3_Ip.Text = String.Empty;
            spn_TanimEk3_Port.EditValue = 0;
        }

        private void gridView13_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView13.RowCount > 0)
            {
                txt_Tanim_Kod.EditValue = gridView13.GetFocusedRowCellValue("Pkod_Kod");
                txt_Tanim_Ad.EditValue = gridView13.GetFocusedRowCellValue("Pkod_Ad");
                cmb_Tanim_Pr.EditValue = gridView13.GetFocusedRowCellValue("Pkod_Printer");
                cmb_Tanim_Ek1.EditValue = gridView13.GetFocusedRowCellValue("Pkod_Ek_Pr1");
                cmb_Tanim_Ek2.EditValue = gridView13.GetFocusedRowCellValue("Pkod_Ek_Pr2");
                cmb_Tanim_Ek3.EditValue = gridView13.GetFocusedRowCellValue("Pkod_Ek_Pr3");
                txt_Tanim_Ip.Text = Convert.ToString(gridView13.GetFocusedRowCellValue("Pkod_Ip"));
                spn_Tanim_Port.EditValue = Convert.ToInt32(gridView13.GetFocusedRowCellValue("Pkod_Port"));
                txt_TanimEk1_Ip.Text = Convert.ToString(gridView13.GetFocusedRowCellValue("Pkod_Ek1_Ip"));
                spn_TanimEk1_Port.EditValue = Convert.ToInt32(gridView13.GetFocusedRowCellValue("Pkod_Ek1_Port"));
                txt_TanimEk2_Ip.Text = Convert.ToString(gridView13.GetFocusedRowCellValue("Pkod_Ek2_Ip"));
                spn_TanimEk2_Port.EditValue = Convert.ToInt32(gridView13.GetFocusedRowCellValue("Pkod_Ek2_Port"));
                txt_TanimEk3_Ip.Text = Convert.ToString(gridView13.GetFocusedRowCellValue("Pkod_Ek3_Ip"));
                spn_TanimEk3_Port.EditValue = Convert.ToInt32(gridView13.GetFocusedRowCellValue("Pkod_Ek3_Port"));
            }
        }

        private void btn_Tanim_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(txt_Tanim_Ad.EditValue) == "" || Convert.ToString(txt_Tanim_Kod.EditValue) == "" || Convert.ToString(cmb_Tanim_Pr.EditValue) == "")
            {
                MessageBox.Show(res_man.GetString("İlgili Alanları Doldurunuz...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + txt_Tanim_Kod.Text + "' and  Pkod_Sinif = '19' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod, Pkod_Ad, Pkod_Sinif, Pkod_Printer,Pkod_Ek_Pr1, Pkod_Ek_Pr2, Pkod_Ek_Pr3, "
                    + " Pkod_Ip,Pkod_Port,Pkod_Ek1_Ip,Pkod_Ek1_Port,Pkod_Ek2_Ip,Pkod_Ek2_Port,Pkod_Ek3_Ip,Pkod_Ek3_Port) "
                    + " VALUES ('" + txt_Tanim_Kod.Text + "','" + txt_Tanim_Ad.Text + "','19','" + Convert.ToString(cmb_Tanim_Pr.EditValue) + "','" + Convert.ToString(cmb_Tanim_Ek1.EditValue) + "','" + Convert.ToString(cmb_Tanim_Ek2.EditValue) + "','" + Convert.ToString(cmb_Tanim_Ek3.EditValue) + "', "
                    + " '" + txt_Tanim_Ip.Text + "'," + Convert.ToInt32(spn_Tanim_Port.EditValue) + ",'" + txt_TanimEk1_Ip.Text + "','" + Convert.ToInt32(spn_TanimEk1_Port.EditValue) + "','" + txt_TanimEk2_Ip.Text + "','" + Convert.ToInt32(spn_TanimEk2_Port.EditValue) + "','" + txt_TanimEk3_Ip.Text + "','" + Convert.ToInt32(spn_TanimEk3_Port.EditValue) + "')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_PrinterTanim, Log.Log_Islem.Kaydet, txt_Tanim_Kod.Text + " - " + txt_Tanim_Ad.Text + " Printer : " + Convert.ToString(cmb_Tanim_Pr.EditValue) + " Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_Ad =  '" + txt_Tanim_Ad.Text + "',Pkod_Printer = '" + Convert.ToString(cmb_Tanim_Pr.EditValue) + "',Pkod_Ek_Pr1 = '" + Convert.ToString(cmb_Tanim_Ek1.EditValue) + "',Pkod_Ek_Pr2 = '" + Convert.ToString(cmb_Tanim_Ek2.EditValue) + "',Pkod_Ek_Pr3 = '" + Convert.ToString(cmb_Tanim_Ek3.EditValue) + "', "
                    + " Pkod_Ip = '" + txt_Tanim_Ip.Text + "',Pkod_Port = '" + Convert.ToInt32(spn_Tanim_Port.EditValue) + "', "
                    + " Pkod_Ek1_Ip = '" + txt_TanimEk1_Ip.Text + "',Pkod_Ek1_Port = '" + Convert.ToInt32(spn_TanimEk1_Port.EditValue) + "', "
                    + " Pkod_Ek2_Ip = '" + txt_TanimEk2_Ip.Text + "',Pkod_Ek2_Port = '" + Convert.ToInt32(spn_TanimEk2_Port.EditValue) + "', "
                    + " Pkod_Ek3_Ip = '" + txt_TanimEk3_Ip.Text + "',Pkod_Ek3_Port = '" + Convert.ToInt32(spn_TanimEk3_Port.EditValue) + "' "
                    + " where Pkod_Kod = '" + txt_Tanim_Kod.Text + "' and  Pkod_Sinif = '19' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_PrinterTanim, Log.Log_Islem.Duzelt, txt_Tanim_Kod.Text + " - " + txt_Tanim_Ad.Text + " Printer : " + Convert.ToString(cmb_Tanim_Pr.EditValue) + " Düzeltildi", String.Empty, String.Empty);
            }

            gridyenile_PrintTanim();
        }

        private void btn_Tanim_Sil_Click(object sender, EventArgs e)
        {
            if (gridView13.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Kodlar where Pkod_Kod = '" + txt_Tanim_Kod.Text + "' and  Pkod_Sinif = '19'");

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_PrinterTanim, Log.Log_Islem.Sil, txt_Tanim_Kod.Text + " - " + txt_Tanim_Ad.Text + " Printer : " + Convert.ToString(cmb_Tanim_Pr.EditValue) + " Silindi", String.Empty, String.Empty);

                    gridyenile_PrintTanim();
                }
            }
        }

        private void btn_Tanim_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmb_Tanim_Ek1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_Tanim_Ek1.EditValue = null;
            }
        }

        private void cmb_Tanim_Ek2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_Tanim_Ek2.EditValue = null;
            }
        }

        private void cmb_Tanim_Ek3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_Tanim_Ek3.EditValue = null;
            }
        }

        private void cmb_Tanim_Pr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_Tanim_Pr.EditValue = null;
            }
        }
        #endregion

        #region Kasa Giriş Çıkış Kodları
        private void gridyenile_Kasagc()
        {
            gridColumn75.FieldName = "Pkod_Kod";
            gridColumn76.FieldName = "Pkod_Ad";

            string filter = rdo_Kasagc.SelectedIndex == 0 ? " and Pkod_Kasagiris = 1 " : " and Pkod_Kasacikis = 1 ";

            grd_Kasagc.DataSource = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad from Pos_Kodlar where Pkod_Sinif = '22' " + filter + " order by Pkod_Kod");

            txt_Kasagc_Kod.Text = "";
            txt_Kasagc_Ad.Text = "";
        }

        private void btn_Kasagc_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(txt_Kasagc_Kod.Text) == "" || Convert.ToString(txt_Kasagc_Ad.Text) == "")
            {
                MessageBox.Show(res_man.GetString("Tüm Alanları Doldurunuz...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            string filter = rdo_Kasagc.SelectedIndex == 0 ? " and Pkod_Kasagiris = 1 " : " and Pkod_Kasacikis = 1 ";
            bool KasaGiris = rdo_Kasagc.SelectedIndex == 0 ? true : false;
            bool KasaCikis = rdo_Kasagc.SelectedIndex == 1 ? true : false;
            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + txt_Kasagc_Kod.Text + "' and  Pkod_Sinif = '22' " + filter);
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod, Pkod_Ad, Pkod_Kasagiris,Pkod_Kasacikis, Pkod_Sinif) VALUES ('" + txt_Kasagc_Kod.Text + "','" + txt_Kasagc_Ad.Text + "','" + Convert.ToBoolean(KasaGiris) + "','" + Convert.ToBoolean(KasaCikis) + "', '22')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Kasagc, Log.Log_Islem.Kaydet, txt_Kasagc_Kod.Text + " - " + txt_Kasagc_Ad.Text + " Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_Ad =  '" + txt_Kasagc_Ad.Text + "' where Pkod_Kod = '" + txt_Kasagc_Kod.Text + "' and  Pkod_Sinif = '22' AND Pkod_Kasagiris = '" + Convert.ToBoolean(KasaGiris) + "' AND Pkod_Kasacikis = '" + Convert.ToBoolean(KasaCikis) + "'");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Kasagc, Log.Log_Islem.Duzelt, txt_Kasagc_Kod.Text + " - " + txt_Kasagc_Ad.Text + " Düzeltildi", String.Empty, String.Empty);
            }

            gridyenile_Kasagc();
        }

        private void rdo_Kasagc_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridyenile_Kasagc();
        }

        private void txt_Kasagc_Sil_Click(object sender, EventArgs e)
        {
            string filter = rdo_Kasagc.SelectedIndex == 0 ? " and Pkod_Kasagiris = 1 " : " and Pkod_Kasacikis = 1 ";
            if (gridView14.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Kodlar where Pkod_Kod = '" + txt_Kasagc_Kod.Text + "' and  Pkod_Sinif = '22' " + filter);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_Kasagc, Log.Log_Islem.Sil, txt_Kasagc_Kod.Text + " - " + txt_Kasagc_Ad.Text + " Silindi", String.Empty, String.Empty);
                    gridyenile_Kasagc();
                }
            }
        }

        private void gridView14_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView14.RowCount > 0)
            {
                txt_Kasagc_Kod.EditValue = gridView14.GetFocusedRowCellValue("Pkod_Kod");
                txt_Kasagc_Ad.EditValue = gridView14.GetFocusedRowCellValue("Pkod_Ad");
            }
        }

        private void txt_Kasagc_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Grup Açıklama Item
        private void gridyenile_AciklamaItem()
        {
            gridColumn96.FieldName = "Pkod_Kod";
            gridColumn97.FieldName = "Pkod_Ad";


            grd_AciklamaItem.DataSource = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad from Pos_Kodlar where Pkod_Sinif = '23' order by Pkod_Kod");

            txt_AciklamaItem_Kod.Text = "";
            txt_AciklamaItem_Ad.Text = "";
        }

        private void btn_AciklamaItem_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(txt_AciklamaItem_Kod.Text) == "" || Convert.ToString(txt_AciklamaItem_Ad.Text) == "")
            {
                MessageBox.Show(res_man.GetString("Tüm Alanları Doldurunuz...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }


            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + txt_AciklamaItem_Kod.Text + "' and  Pkod_Sinif = '23' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod, Pkod_Ad, Pkod_Sinif) VALUES ('" + txt_AciklamaItem_Kod.Text + "','" + txt_AciklamaItem_Ad.Text + "', '23')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_AciklamaItem, Log.Log_Islem.Kaydet, txt_AciklamaItem_Kod.Text + " - " + txt_AciklamaItem_Ad.Text + " Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_Ad =  '" + txt_AciklamaItem_Ad.Text + "' where Pkod_Kod = '" + txt_AciklamaItem_Kod.Text + "' and  Pkod_Sinif = '23' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_AciklamaItem, Log.Log_Islem.Duzelt, txt_AciklamaItem_Kod.Text + " - " + txt_AciklamaItem_Ad.Text + " Düzeltildi", String.Empty, String.Empty);
            }

            gridyenile_AciklamaItem();
        }

        private void btn_AciklamaItem_Sil_Click(object sender, EventArgs e)
        {
            if (gridView16.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Kodlar where Pkod_Kod = '" + txt_AciklamaItem_Kod.Text + "' and  Pkod_Sinif = '23' ");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_AciklamaItem, Log.Log_Islem.Sil, txt_AciklamaItem_Kod.Text + " - " + txt_AciklamaItem_Ad.Text + " Silindi", String.Empty, String.Empty);
                    gridyenile_AciklamaItem();
                }
            }
        }

        private void gridView16_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView16.RowCount > 0)
            {
                txt_AciklamaItem_Kod.EditValue = gridView16.GetFocusedRowCellValue("Pkod_Kod");
                txt_AciklamaItem_Ad.EditValue = gridView16.GetFocusedRowCellValue("Pkod_Ad");
            }
        }


        private void btn_AciklamaItem_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region İl Tanımlamaları

        private void gridyenile_Il()
        {
            gridColumn107.FieldName = "Adres_Kod";
            gridColumn108.FieldName = "Adres_Ad";


            gridControl17.DataSource = dbtools.SelectTable("select Adres_Kod,Adres_Ad from Pos_Adres where Adres_Sinif = '24' order by Adres_Ad");

            txt_Il_Kod.Text = "";
            txt_Il_Ad.Text = "";
        }
        private void gridView17_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView17.RowCount > 0)
            {
                txt_Il_Kod.EditValue = gridView17.GetFocusedRowCellValue("Adres_Kod");
                txt_Il_Ad.EditValue = gridView17.GetFocusedRowCellValue("Adres_Ad");
            }
        }

        private void btn_Il_Kaydet_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(txt_Il_Kod.Text) == "" || Convert.ToString(txt_Il_Ad.Text) == "")
            {
                MessageBox.Show(res_man.GetString("Tüm Alanları Doldurunuz...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }


            DataTable dt = dbtools.SelectTable("select * from Pos_Adres where Adres_Kod = '" + txt_Il_Kod.Text + "' and  Adres_Sinif = '24' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Adres (Adres_Kod, Adres_Ad, Adres_Sinif) VALUES ('" + txt_Il_Kod.Text + "','" + txt_Il_Ad.Text + "', '24')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_IlTanim, Log.Log_Islem.Kaydet, txt_Il_Kod.Text + " - " + txt_Il_Ad.Text + " Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Adres set Adres_Ad =  '" + txt_Il_Ad.Text + "' where Adres_Kod = '" + txt_Il_Kod.Text + "' and  Adres_Sinif = '24' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_IlTanim, Log.Log_Islem.Duzelt, txt_Il_Kod.Text + " - " + txt_Il_Ad.Text + " Düzeltildi", String.Empty, String.Empty);
            }

            gridyenile_Il();
        }

        private void btn_Il_Sil_Click(object sender, EventArgs e)
        {
            if (gridView17.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Adres where Adres_Kod = '" + txt_Il_Kod.Text + "' and  Adres_Sinif = '24' ");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_IlTanim, Log.Log_Islem.Sil, txt_Il_Kod.Text + " - " + txt_Il_Ad.Text + " Silindi", String.Empty, String.Empty);
                    gridyenile_Il();
                }
            }
        }

        private void btn_Il_Default_Click(object sender, EventArgs e)
        {
            try
            {
                dbtools.execcmd("exec Pos_AdresDefault @Il = 1, @Ilce = 0, @Mahalle = 0");
                dbtools.execcmd("exec Pos_AdresDefault @Il = 0, @Ilce = 1, @Mahalle = 0");
                dbtools.execcmd("exec Pos_AdresDefault @Il = 0, @Ilce = 0, @Mahalle = 1");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Il_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Ilce Tanimlamaları

        private void look_Ilce_Il_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(look_Ilce_Il.EditValue))) return;

            gridColumn109.FieldName = "Adres_Kod";
            gridColumn110.FieldName = "Adres_Ad";
            gridColumn111.FieldName = "ilKod";
            gridColumn112.FieldName = "ilAd";

            gridControl18.DataSource = dbtools.SelectTable(@"
select ilce.Adres_Kod, ilce.Adres_Ad, ilce.Adres_UstGrup as ilKod, il.Adres_Ad as ilAd 
from Pos_Adres as ilce
left join Pos_Adres as il on il.Adres_Kod = ilce.Adres_UstGrup and il.Adres_Sinif = '24'
where ilce.Adres_Sinif = '25' and il.Adres_Kod = '" + Convert.ToString(look_Ilce_Il.EditValue) + @"'
order by ilce.Adres_Ad");

            txt_Ilce_Kod.Text = "";
            txt_Ilce_Ad.Text = "";

        }
        private void btn_Ilce_Kaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(look_Ilce_Il.EditValue)) || Convert.ToString(txt_Ilce_Kod.Text) == "" || Convert.ToString(txt_Ilce_Ad.Text) == "")
            {
                MessageBox.Show(res_man.GetString("Tüm Alanları Doldurunuz...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }


            DataTable dt = dbtools.SelectTable("select * from Pos_Adres where Adres_Kod = '" + txt_Ilce_Kod.Text + "' and  Adres_Sinif = '25' and Adres_UstGrup = '" + Convert.ToString(look_Ilce_Il.EditValue) + "' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Adres (Adres_Kod, Adres_Ad, Adres_Sinif,Adres_UstGrup) VALUES ('" + txt_Ilce_Kod.Text + "','" + txt_Ilce_Ad.Text + "', '25', '" + Convert.ToString(look_Ilce_Il.EditValue) + "')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_IlceTanim, Log.Log_Islem.Kaydet, txt_Ilce_Kod.Text + " - " + txt_Ilce_Ad.Text + " Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Adres set Adres_Ad =  '" + txt_Ilce_Ad.Text + "' where Adres_Kod = '" + txt_Ilce_Kod.Text + "' and  Adres_Sinif = '25' and Adres_UstGrup = '" + Convert.ToString(look_Ilce_Il.EditValue) + "' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_IlceTanim, Log.Log_Islem.Duzelt, txt_Ilce_Kod.Text + " - " + txt_Ilce_Ad.Text + " Düzeltildi", String.Empty, String.Empty);
            }

            look_Ilce_Il_Leave(null, null);
        }

        private void btn_Ilce_Sil_Click(object sender, EventArgs e)
        {
            if (gridView18.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Adres where Adres_Kod = '" + txt_Ilce_Kod.Text + "' and  Adres_Sinif = '25' and Adres_UstGrup = '" + Convert.ToString(look_Ilce_Il.EditValue) + "' ");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_IlceTanim, Log.Log_Islem.Sil, txt_Ilce_Kod.Text + " - " + txt_Ilce_Ad.Text + " Silindi", String.Empty, String.Empty);
                    look_Ilce_Il_Leave(null, null);
                }
            }
        }

        private void btn_Ilce_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView18_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView18.RowCount > 0)
            {
                txt_Ilce_Kod.EditValue = gridView18.GetFocusedRowCellValue("Adres_Kod");
                txt_Ilce_Ad.EditValue = gridView18.GetFocusedRowCellValue("Adres_Ad");
            }
        }
        #endregion

        #region Mahalle Tanimlamaları
        private void look_Mah_Il_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(look_Mah_Il.EditValue))) return;

            look_Mah_Ilce.Properties.DataSource = dbtools.SelectTable("select * from Pos_Adres where Adres_UstGrup = '" + Convert.ToString(look_Mah_Il.EditValue) + "' and Adres_Sinif = '25' order by Adres_Ad");
            look_Mah_Ilce.Properties.DisplayMember = "Adres_Ad";
            look_Mah_Ilce.Properties.ValueMember = "Adres_Kod";
        }

        private void look_Mah_Ilce_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(look_Mah_Ilce.EditValue))) return;

            gridColumn113.FieldName = "Adres_Kod";
            gridColumn114.FieldName = "Adres_Ad";
            gridColumn115.FieldName = "ilKod";
            gridColumn116.FieldName = "ilAd";
            gridColumn117.FieldName = "ilceAd";
            gridColumn118.FieldName = "ilceAd";

            gridControl19.DataSource = dbtools.SelectTable(@"
select mah.Adres_Kod,mah.Adres_Ad,mah.Adres_UstGrup as ilKod,il.Adres_Ad as ilAd,mah.Adres_AltGrup as ilceKod,ilce.Adres_Ad as ilceAd 
from Pos_Adres as mah
left join Pos_Adres as il on il.Adres_Kod = mah.Adres_UstGrup and il.Adres_Sinif = '24'
left join Pos_Adres as ilce on ilce.Adres_Kod = mah.Adres_AltGrup and ilce.Adres_Sinif = '25'
where mah.Adres_Sinif = '26' and mah.Adres_UstGrup = '" + Convert.ToString(look_Mah_Il.EditValue) + "' and mah.Adres_AltGrup = '" + Convert.ToString(look_Mah_Ilce.EditValue) + @"'
order by mah.Adres_Ad");

            txt_Mah_Kod.Text = "";
            txt_Mah_Ad.Text = "";
        }

        private void btn_Mah_Kaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(look_Mah_Il.EditValue)) || string.IsNullOrEmpty(Convert.ToString(look_Mah_Ilce.EditValue)) || Convert.ToString(txt_Mah_Kod.Text) == "" || Convert.ToString(txt_Mah_Ad.Text) == "")
            {
                MessageBox.Show(res_man.GetString("Tüm Alanları Doldurunuz...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }


            DataTable dt = dbtools.SelectTable("select * from Pos_Adres where Adres_Kod = '" + txt_Mah_Kod.Text + "' and  Adres_Sinif = '26' and Adres_UstGrup = '" + Convert.ToString(look_Mah_Il.EditValue) + "' and Adres_AltGrup = '" + Convert.ToString(look_Mah_Ilce.EditValue) + "' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Adres (Adres_Kod, Adres_Ad, Adres_Sinif,Adres_UstGrup,Adres_AltGrup) VALUES ('" + txt_Mah_Kod.Text + "','" + txt_Mah_Ad.Text + "', '26', '" + Convert.ToString(look_Mah_Il.EditValue) + "','" + Convert.ToString(look_Mah_Ilce.EditValue) + "')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MahalleTanim, Log.Log_Islem.Kaydet, txt_Mah_Kod.Text + " - " + txt_Mah_Ad.Text + " Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Adres set Adres_Ad =  '" + txt_Mah_Ad.Text + "' where Adres_Kod = '" + txt_Mah_Kod.Text + "' and  Adres_Sinif = '26' and Adres_UstGrup = '" + Convert.ToString(look_Mah_Il.EditValue) + "' and Adres_AltGrup = '" + Convert.ToString(look_Mah_Ilce.EditValue) + "' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MahalleTanim, Log.Log_Islem.Duzelt, txt_Mah_Kod.Text + " - " + txt_Mah_Ad.Text + " Düzeltildi", String.Empty, String.Empty);
            }

            look_Mah_Ilce_Leave(null, null);
        }

        private void btn_Mah_Sil_Click(object sender, EventArgs e)
        {
            if (gridView19.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Adres where Adres_Kod = '" + txt_Mah_Kod.Text + "' and  Adres_Sinif = '26' and Adres_UstGrup = '" + Convert.ToString(look_Mah_Il.EditValue) + "' and Adres_AltGrup = '" + Convert.ToString(look_Mah_Ilce.EditValue) + "'  ");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_MahalleTanim, Log.Log_Islem.Sil, txt_Mah_Kod.Text + " - " + txt_Mah_Ad.Text + " Silindi", String.Empty, String.Empty);
                    look_Mah_Ilce_Leave(null, null);
                }
            }
        }

        private void btn_MAh_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView19_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView19.RowCount > 0)
            {
                txt_Mah_Kod.EditValue = gridView19.GetFocusedRowCellValue("Adres_Kod");
                txt_Mah_Ad.EditValue = gridView19.GetFocusedRowCellValue("Adres_Ad");
            }
        }
        #endregion

        #region Sube Tanimlamları
        private void gridyenile_Sube()
        {
            gridColumn119.FieldName = "Pkod_Kod";
            gridColumn120.FieldName = "Pkod_Ad";

            DataTable dt = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad from Pos_Kodlar with(nolock) where Pkod_Sinif = '11'");
            if (dt.Rows.Count > 0)
            {
                look_KapatmaKodu.Properties.DataSource = dt;
                look_KapatmaKodu.Properties.DisplayMember = "Pkod_Ad";
                look_KapatmaKodu.Properties.ValueMember = "Pkod_Kod";
            }



            gridControl20.DataSource = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Sinif = '27' order by Pkod_Kod");



            txt_Sube_Kod.Text = String.Empty;
            txt_Sube_Ad.Text = String.Empty;
            txt_Sube_Server.Text = String.Empty;
            txt_Sube_Database.Text = String.Empty;
            txt_Sube_User.Text = String.Empty;
            txt_Sube_Password.Text = String.Empty;
            txt_Sube_MAC.Text = String.Empty;
            Pkod_LinkServer.Text = String.Empty;
            look_KapatmaKodu.EditValue = null;
            txt_KapatmaHesabi.Text = String.Empty;

        }

        private void btn_Sube_Sil_Click(object sender, EventArgs e)
        {
            if (gridView20.RowCount > 0)
            {
                DialogResult c;
                c = MessageBox.Show(res_man.GetString("Secili Satırı Silmek İstediğineze Emin misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (c == DialogResult.Yes)
                {
                    dbtools.execcmd("delete from Pos_Kodlar where Pkod_Kod = '" + txt_Sube_Kod.Text + "' and  Pkod_Sinif = '27' ");
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SubeTanim, Log.Log_Islem.Sil, txt_Sube_Kod.Text + " - " + txt_Sube_Ad.Text + " Silindi", String.Empty, String.Empty);
                    gridyenile_Sube();
                }
            }
        }

        private void btn_Sube_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView20_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView20.RowCount > 0)
            {
                txt_Sube_Kod.EditValue = gridView20.GetFocusedRowCellValue("Pkod_Kod");
                txt_Sube_Ad.EditValue = gridView20.GetFocusedRowCellValue("Pkod_Ad");
                txt_Sube_Server.EditValue = gridView20.GetFocusedRowCellValue("Pkod_Server");
                txt_Sube_Database.EditValue = gridView20.GetFocusedRowCellValue("Pkod_Database");
                txt_Sube_User.EditValue = gridView20.GetFocusedRowCellValue("Pkod_User");
                txt_Sube_Password.EditValue = gridView20.GetFocusedRowCellValue("Pkod_Password");
                rdb_MerkezSube.EditValue = gridView20.GetFocusedRowCellValue("Pkod_MerkezSube");
                txt_Sube_MAC.EditValue = gridView20.GetFocusedRowCellValue("Pkod_SubeMac");
                look_KapatmaKodu.EditValue = gridView20.GetFocusedRowCellValue("Pkod_KapatmaKodu");
                txt_KapatmaHesabi.EditValue = gridView20.GetFocusedRowCellValue("Pkod_KapatmaHesabi");
                Pkod_LinkServer.EditValue = Convert.ToString(gridView20.GetFocusedRowCellValue("Pkod_LinkServer"));


            }
        }

        private void btn_Sube_Kaydet_Click(object sender, EventArgs e)
        {

            if (Convert.ToString(txt_Sube_Kod.Text) == "" || Convert.ToString(txt_Sube_Ad.Text) == "")
            {
                MessageBox.Show(res_man.GetString("Tüm Alanları Doldurunuz...!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }


            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + txt_Sube_Kod.Text + "' and  Pkod_Sinif = '27' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod, Pkod_Ad, Pkod_Sinif, Pkod_Server,Pkod_Database,Pkod_User,Pkod_Password,Pkod_MerkezSube,Pkod_SubeMac,Pkod_KapatmaKodu,Pkod_KapatmaHesabi,Pkod_LinkServer) VALUES ('" + txt_Sube_Kod.Text + "','" + txt_Sube_Ad.Text + "', '27','" + txt_Sube_Server.Text + "','" + txt_Sube_Database.Text + "','" + txt_Sube_User.Text + "','" + txt_Sube_Password.Text + "','" + rdb_MerkezSube.EditValue + "','" + txt_Sube_MAC.Text + "','" + look_KapatmaKodu.EditValue + "','" + txt_KapatmaHesabi.Text + "','" + Pkod_LinkServer.Text + "')");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SubeTanim, Log.Log_Islem.Kaydet, txt_Sube_Kod.Text + " - " + txt_Sube_Ad.Text + " Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("Update Pos_Kodlar set Pkod_Ad =  '" + txt_Sube_Ad.Text + "',Pkod_Server = '" + txt_Sube_Server.Text + "',Pkod_Database = '" + txt_Sube_Database.Text + "',Pkod_User = '" + txt_Sube_User.Text + "',Pkod_Password = '" + txt_Sube_Password.Text + "', Pkod_MerkezSube = '" + rdb_MerkezSube.EditValue + "',Pkod_SubeMac = '" + txt_Sube_MAC.Text + "', Pkod_KapatmaKodu = '" + look_KapatmaKodu.EditValue + "', Pkod_KapatmaHesabi = '" + txt_KapatmaHesabi.Text + "', Pkod_LinkServer= '" + Pkod_LinkServer.Text + "'  where Pkod_Kod = '" + txt_Sube_Kod.Text + "' and  Pkod_Sinif = '27' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SubeTanim, Log.Log_Islem.Duzelt, txt_Sube_Kod.Text + " - " + txt_Sube_Ad.Text + " Düzeltildi", String.Empty, String.Empty);
            }

            gridyenile_Sube();
        }
        #endregion

        #region Sube Adres Tanımlama
        private void btn_SubeAdres_Kaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(look_SubeAdres_Sube.EditValue))) return;

            foreach (var item in gridView21.GetSelectedRows())
            {
                int mahId = Convert.ToInt32(gridView21.GetRowCellValue(item, "mahalleId"));

                dbtools.execcmd("update Pos_Adres set Adres_Sube = '" + Convert.ToString(look_SubeAdres_Sube.EditValue) + "' where Adres_Id = '" + mahId + "'");
            }

            gridyenile_SubeAdres();
        }

        private void navBarItem31_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Pos_Plu_Terazi ppt = new Pos_Plu_Terazi();
            ppt.Show();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            //ManagementClass manager = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //foreach (ManagementObject obj in manager.GetInstances())
            //{
            //    if ((bool)obj["IPEnabled"])
            //    {
            //        txt_Sube_MAC.EditValue = obj["MacAddress"].ToString();
            //    }
            //}

            string mac = NetworkInterface
               .GetAllNetworkInterfaces()
               .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
               .Select(nic => nic.GetPhysicalAddress().ToString())
               .FirstOrDefault();

            string mac2 = "";
            for (int i = 0; i < mac.Length; i++)
            {
                if (i != 0 && i % 2 == 0)
                {
                    mac2 = mac2 + ":";
                }

                mac2 = mac2 + mac[i];

            }

            txt_Sube_MAC.EditValue = mac2;

        }

        private void navBarItem32_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

            stokParametreAcYeni();

        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Param_iadeKontrol_CheckedChanged(object sender, EventArgs e)
        {
            Param_iadeLimit.Enabled = Param_iadeKontrol.Checked;
        }

        private void navBarItem33_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = OdemeTipleri;
            IngenicoYenile(Convert.ToString(navBarItem33.Tag));
        }

        private void IngenicoYenile(string Tag)
        {
            if (Tag == "91")
            {
                gridControl1.DataSource = dbtools.SelectTable("Select Pkod_Kod,Pkod_Ad from Pos_Kodlar Where Pkod_Sinif = '" + Tag + "'");
            }
            if (Tag == "92")
            {
                gridControl3.DataSource = dbtools.SelectTable("Select Pkod_Kod,Pkod_Ad from Pos_Kodlar Where Pkod_Sinif = '" + Tag + "'");
            }
            if (Tag == "93")
            {
                gridControl4.DataSource = dbtools.SelectTable("Select Pkod_Kod,Pkod_Ad from Pos_Kodlar Where Pkod_Sinif = '" + Tag + "'");
            }
            if (Tag == "94")
            {
                gridControl5.DataSource = dbtools.SelectTable("Select Pkod_Kod,Pkod_Ad from Pos_Kodlar Where Pkod_Sinif = '" + Tag + "'");
            }

        }

        private void gridView22_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            textEdit161.Text = Convert.ToString(gridView22.GetFocusedRowCellValue("Pkod_Kod"));
            textEdit159.Text = Convert.ToString(gridView22.GetFocusedRowCellValue("Pkod_Ad"));
        }

        private void gridView23_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            textEdit165.Text = Convert.ToString(gridView23.GetFocusedRowCellValue("Pkod_Kod"));
            textEdit163.Text = Convert.ToString(gridView23.GetFocusedRowCellValue("Pkod_Ad"));
        }


        private void gridView24_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            textEdit169.Text = Convert.ToString(gridView24.GetFocusedRowCellValue("Pkod_Kod"));
            textEdit167.Text = Convert.ToString(gridView24.GetFocusedRowCellValue("Pkod_Ad"));
        }


        private void gridView25_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            textEdit173.Text = Convert.ToString(gridView25.GetFocusedRowCellValue("Pkod_Kod"));
            textEdit171.Text = Convert.ToString(gridView25.GetFocusedRowCellValue("Pkod_Ad"));
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (textEdit161.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Kod Bos Gecilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (textEdit159.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Ad Bos Gecilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + textEdit161.EditValue + "' and Pkod_Sinif = '91' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod,Pkod_Ad,Pkod_Sinif) VALUES ( "
                    + " '" + textEdit161.EditValue + "','" + textEdit159.EditValue + "','91')");

                //Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Kaydet, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Kod='" + textEdit161.EditValue + "',Pkod_Ad='" + textEdit159.EditValue + "' "

                + " where Pkod_Kod = '" + textEdit161.EditValue + "' and Pkod_Sinif = '91'");

                //Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Duzelt, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Duzeltildi", String.Empty, String.Empty);
            }
            IngenicoYenile(Convert.ToString(navBarItem33.Tag));
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("Delete from Pos_Kodlar Where Pkod_Kod = '" + textEdit161.Text + "' and Pkod_Sinif = '91'");
            IngenicoYenile(Convert.ToString(navBarItem33.Tag));
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("Delete from Pos_Kodlar Where Pkod_Kod = '" + textEdit165.Text + "' and Pkod_Sinif = '92'");
            IngenicoYenile(Convert.ToString(navBarItem34.Tag));
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (textEdit165.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Kod Bos Gecilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (textEdit163.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Ad Bos Gecilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + textEdit165.EditValue + "' and Pkod_Sinif = '92' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod,Pkod_Ad,Pkod_Sinif) VALUES ( "
                    + " '" + textEdit165.EditValue + "','" + textEdit163.EditValue + "','92')");

                //Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Kaydet, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Kod='" + textEdit165.EditValue + "',Pkod_Ad='" + textEdit163.EditValue + "' "

                + " where Pkod_Kod = '" + textEdit165.EditValue + "' and Pkod_Sinif = '92'");

                //Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Duzelt, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Duzeltildi", String.Empty, String.Empty);
            }
            IngenicoYenile(Convert.ToString(navBarItem34.Tag));
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("Delete from Pos_Kodlar Where Pkod_Kod = '" + textEdit169.Text + "' and Pkod_Sinif = '93'");
            IngenicoYenile(Convert.ToString(navBarItem35.Tag));
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (textEdit169.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Kod Bos Gecilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (textEdit167.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Ad Bos Gecilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + textEdit169.EditValue + "' and Pkod_Sinif = '93' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod,Pkod_Ad,Pkod_Sinif) VALUES ( "
                    + " '" + textEdit169.EditValue + "','" + textEdit167.EditValue + "','93')");

                //Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Kaydet, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Kod='" + textEdit169.EditValue + "',Pkod_Ad='" + textEdit167.EditValue + "' "

                + " where Pkod_Kod = '" + textEdit169.EditValue + "' and Pkod_Sinif = '93'");

                //Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Duzelt, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Duzeltildi", String.Empty, String.Empty);
            }
            IngenicoYenile(Convert.ToString(navBarItem35.Tag));
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("Delete from Pos_Kodlar Where Pkod_Kod = '" + textEdit173.Text + "' and Pkod_Sinif = '94'");
            IngenicoYenile(Convert.ToString(navBarItem36.Tag));
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            if (textEdit173.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Kod Bos Gecilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (textEdit171.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Ad Bos Gecilemez...?"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Kod = '" + textEdit173.EditValue + "' and Pkod_Sinif = '94' ");
            if (dt.Rows.Count < 1)
            {
                dbtools.execcmd("INSERT INTO Pos_Kodlar (Pkod_Kod,Pkod_Ad,Pkod_Sinif) VALUES ( "
                    + " '" + textEdit173.EditValue + "','" + textEdit171.EditValue + "','94')");

                //Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Kaydet, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Kaydedildi", String.Empty, String.Empty);
            }
            else
            {
                dbtools.execcmd("update Pos_Kodlar set Pkod_Kod='" + textEdit173.EditValue + "',Pkod_Ad='" + textEdit171.EditValue + "' "

                + " where Pkod_Kod = '" + textEdit173.EditValue + "' and Pkod_Sinif = '94'");

                //Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_OdemeKodu, Log.Log_Islem.Duzelt, txt_Odeme_Kod.EditValue + " Kodlu " + txt_Odeme_Ad.EditValue + " Adlı Odeme Kodu Duzeltildi", String.Empty, String.Empty);
            }
            IngenicoYenile(Convert.ToString(navBarItem36.Tag));
        }

        private void navBarItem34_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = DepartmanKisimlari;
            IngenicoYenile(Convert.ToString(navBarItem34.Tag));
        }

        private void navBarItem35_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = Bankalar;
            IngenicoYenile(Convert.ToString(navBarItem35.Tag));
        }

        private void navBarItem36_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPage = YemekCekleri;
            IngenicoYenile(Convert.ToString(navBarItem36.Tag));
        }

        private void DepartmanKisim()
        {
            // Button[] idDepartmenButtons = { m_btnK_017, m_btnK_018, m_btnK_019, m_btnK_020, m_btnK_021, m_btnK_022, m_btnK_023, m_btnK_024 };

            UInt32 RetCode = 0;
            byte indexOfTaxRates = 0;
            byte indexOfDepartments = 0;
            int numberOfTotalRecordsReceived = 0;
            int numberOfTotalTaxRates = 0;
            int numberOfTotalDepartments = 0;
            ST_TAX_RATE[] stTaxRates = new ST_TAX_RATE[8];
            ST_DEPARTMENT[] stDepartments = new ST_DEPARTMENT[12];

            for (int i = 0; i < stDepartments.Length; i++)
            {
                stDepartments[i] = new ST_DEPARTMENT();
            }

            do
            {
                RetCode = Json_GMPSmartDLL.FP3_GetTaxRates_Ex(IngenicoConn.CurrentInterface, indexOfTaxRates, ref numberOfTotalTaxRates, ref numberOfTotalRecordsReceived, ref stTaxRates, 8 - indexOfTaxRates);

                if (RetCode != 0)
                    return;
                //return RetCode;

                indexOfTaxRates += (byte)numberOfTotalRecordsReceived;

            } while (8 - indexOfTaxRates != 0);

            do
            {
                RetCode = Json_GMPSmartDLL.FP3_GetDepartments_Ex(IngenicoConn.CurrentInterface, indexOfDepartments, ref numberOfTotalDepartments, ref numberOfTotalRecordsReceived, ref stDepartments, 12 - indexOfDepartments);

                if (RetCode != 0)
                    return;

                indexOfDepartments += (byte)numberOfTotalRecordsReceived;

            } while (12 - indexOfDepartments != 0);

            for (int i = 0; i < indexOfTaxRates; i++)
            {
                if (i > 7)
                    continue;
                // idDepartmenButtons[i].Text = String.Format("{0}" + System.Environment.NewLine + "%{1}.{2}", stDepartments[i].szDeptName, stTaxRates[stDepartments[i].u8TaxIndex].taxRate / 100, stTaxRates[stDepartments[i].u8TaxIndex].taxRate % 100);

                cmb_Dep.Items.Add(String.Format("{0}" + System.Environment.NewLine + "%{1}.{2}", stDepartments[i].szDeptName, stTaxRates[stDepartments[i].u8TaxIndex].taxRate / 100, stTaxRates[stDepartments[i].u8TaxIndex].taxRate % 100));
            }

            if (!TransactionTaxRateList.ContainsKey(IngenicoConn.CurrentInterface))
                TransactionTaxRateList.Add(IngenicoConn.CurrentInterface, stTaxRates);

            if (!TransactionDepartmentList.ContainsKey(IngenicoConn.CurrentInterface))
                TransactionDepartmentList.Add(IngenicoConn.CurrentInterface, stDepartments);

            //return RetCode;

        }

        static public Dictionary<UInt32, ST_TAX_RATE[]> TransactionTaxRateList = new Dictionary<uint, ST_TAX_RATE[]>();
        static public Dictionary<UInt32, ST_EXCHANGE[]> TransactionExchangeList = new Dictionary<uint, ST_EXCHANGE[]>();
        static public Dictionary<UInt32, ST_DEPARTMENT[]> TransactionDepartmentList = new Dictionary<uint, ST_DEPARTMENT[]>();

        private void BankaListesiAl()
        {
            if (IngenicoConn.m_tvEcho.Nodes.Count < 1)
            {
                MessageBox.Show(res_man.GetString("Aktarım Yapmadan Once Eslestirme Yapılması Gerekir"));
                return;
            }

            listBox1.Items.Clear();

            uint sonuc = 0;
            byte numberOfTotalRecords = 0;
            byte numberOfTotalRecordsReceived = 0;
            ST_PAYMENT_APPLICATION_INFO[] stPaymentApplicationInfo = new ST_PAYMENT_APPLICATION_INFO[24];
            UInt32 amount = 0;

            UInt16 currencyOfPayment = (UInt16)ECurrency.CURRENCY_TL;//(UInt16)0; //m_comboBoxCurrency.SelectedIndex;

            UInt32 retcode = Json_GMPSmartDLL.FP3_GetPaymentApplicationInfo(IngenicoConn.CurrentInterface, ref numberOfTotalRecords, ref numberOfTotalRecordsReceived, ref stPaymentApplicationInfo, 24);

            if (retcode != Defines.TRAN_RESULT_OK)
                return;
            //HandleErrorCode(retcode);
            else if (numberOfTotalRecordsReceived == 0)
                MessageBox.Show(res_man.GetString("KrediKartı Odeme Uygulaması Bulunamadı"), "KrediKarti Kontrol", MessageBoxButtons.OK);
            else
            {
                ST_PAYMENT_APPLICATION_INFO[] stPaymentApplicationInfo2;

                stPaymentApplicationInfo2 = new ST_PAYMENT_APPLICATION_INFO[24];
                Array.Copy(stPaymentApplicationInfo, stPaymentApplicationInfo2, stPaymentApplicationInfo.Length);
                for (int i = 0; i < numberOfTotalRecordsReceived; i++)
                {

                    string str = "";
                    str += GMP_Tools.GetStringFromBytes(stPaymentApplicationInfo[i].name) +
                        " [" + stPaymentApplicationInfo[i].u16BKMId.ToString() + "] " +
                        " [" + stPaymentApplicationInfo[i].u16AppId.ToString("X2") + "] " +
                        " [" + stPaymentApplicationInfo[i].Status.ToString() + "] " +
                        " [" + stPaymentApplicationInfo[i].Priority.ToString() + "]";//+
                    //" [" + getAppTypeName(stPaymentApplicationInfo[i].AppType) + "]";

                    listBox1.Items.Add(str);
                }
            }
        }
        private void YemekCekiListesiAl()
        {
            listBox2 = new ListBox();
            if (IngenicoConn.m_tvEcho.Nodes.Count < 1)
            {
                MessageBox.Show(res_man.GetString("Aktarım Yapmadan Once Eslestirme Yapılması Gerekir"));
                return;
            }

            listBox2.Items.Clear();
            byte NumberOfTotalRecord = 24;
            byte NumberOfTotalRecordReceived = 0;

            ST_PAYMENT_APPLICATION_INFO[] StPaymentApplicationInfo = new ST_PAYMENT_APPLICATION_INFO[24];
            for (int i = 0; i < StPaymentApplicationInfo.Length; i++)
            {
                StPaymentApplicationInfo[i] = new ST_PAYMENT_APPLICATION_INFO();
            }

            uint retcode = Json_GMPSmartDLL.FP3_GetVasApplicationInfo(IngenicoConn.CurrentInterface, ref NumberOfTotalRecord, ref NumberOfTotalRecordReceived, ref StPaymentApplicationInfo, (ushort)EVasType.TLV_OKC_ASSIST_VAS_TYPE_YEMEKCEKI);

            if (retcode != Defines.TRAN_RESULT_OK)
                return;
            //HandleErrorCode(retcode);

            else if (NumberOfTotalRecordReceived == 0)
                MessageBox.Show(res_man.GetString("ÖKC Üzerinde Yemek Ceki Ödeme Uygulanaması Bulunamadı"), "HATA", MessageBoxButtons.OK);
            else
            {
                /*
                PaymentAppForm paf = new PaymentAppForm(NumberOfTotalRecordReceived, StPaymentApplicationInfo);
                DialogResult dr = paf.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                    return;
                */

                ST_PAYMENT_APPLICATION_INFO[] stPaymentApplicationInfo2;

                stPaymentApplicationInfo2 = new ST_PAYMENT_APPLICATION_INFO[24];
                Array.Copy(StPaymentApplicationInfo, stPaymentApplicationInfo2, StPaymentApplicationInfo.Length);
                for (int i = 0; i < NumberOfTotalRecordReceived; i++)
                {

                    string str = "";
                    str += GMP_Tools.GetStringFromBytes(StPaymentApplicationInfo[i].name) +
                        " [" + StPaymentApplicationInfo[i].u16BKMId.ToString() + "] " +
                        " [" + StPaymentApplicationInfo[i].u16AppId.ToString("X2") + "] " +
                        " [" + StPaymentApplicationInfo[i].Status.ToString() + "] " +
                        " [" + StPaymentApplicationInfo[i].Priority.ToString() + "]";//+
                    //" [" + getAppTypeName(stPaymentApplicationInfo[i].AppType) + "]";

                    listBox2.Items.Add(str);
                }
            }

        }

        ListBox listBox1 = new ListBox();
        ListBox listBox2 = new ListBox();
        ComboBox cmb_Dep = new ComboBox();
        private void simpleButton16_Click(object sender, EventArgs e)
        {
            DepartmanKisim();
            BankaListesiAl();
            YemekCekiListesiAl();


            if (MessageBox.Show(res_man.GetString("Yazarkasa Eslestirmesiyle Gelen Departman(Kısım) VB. Bilgileri RMOS ULTIMATE POS'a Aktarılacaktır \r\n Devam Etmek Istiyor musunuz ?"), "Aktarım Kontrol", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            else
            {
                dbtools.execcmd("delete from Pos_Kodlar where(Pkod_Sinif = '91' or Pkod_Sinif = '92' or Pkod_Sinif = '93' or Pkod_Sinif = '94')");
            }

            List<PKod> kodlar = new List<PKod>();

            PKod pkod = new PKod();
            pkod.Kod = "1";
            pkod.Ad = "PAYMENT_CASH_TL";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "3";
            pkod.Ad = "PAYMENT_BANK_CARD";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "4";
            pkod.Ad = "PAYMENT_YEMEKCEKI";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "5";
            pkod.Ad = "PAYMENT_MOBILE";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "6";
            pkod.Ad = "PAYMENT_HEDIYE_CEKI";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "7";
            pkod.Ad = "PAYMENT_IKRAM";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "8";
            pkod.Ad = "PAYMENT_ODEMESIZ";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "12";
            pkod.Ad = "PAYMENT_BANKA_TRANSFERI";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "13";
            pkod.Ad = "PAYMENT_CEK";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "14";
            pkod.Ad = "PAYMENT_ACIK_HESAP";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "15";
            pkod.Ad = "PAYMENT_DIGER";
            pkod.Sinif = "91";
            kodlar.Add(pkod);

            pkod = new PKod();
            pkod.Kod = "21";
            pkod.Ad = "INDIRIM";
            pkod.Sinif = "91";
            kodlar.Add(pkod);



            int say = 0;
            foreach (var item in cmb_Dep.Items)
            {
                PKod pk1 = new PKod();
                string str = item.ToString();
                str = str.Replace("\r\n", " | ");

                pk1.Ad = str;
                pk1.Sinif = "92";
                pk1.Kod = say.ToString();
                pk1.kdvOran = 0;
                kodlar.Add(pk1);

                say++;
            }




            say = 0;
            foreach (var item in listBox1.Items)
            {
                PKod pk1 = new PKod();

                string str = item.ToString();
                str = str.Replace("\r\n", " | ");
                pk1.Ad = str;
                pk1.Sinif = "93";
                pk1.Kod = say.ToString();
                kodlar.Add(pk1);

                say++;
            }


            say = 0;
            foreach (var item in listBox2.Items)
            {
                PKod pk1 = new PKod();

                string str = item.ToString();
                str = str.Replace("\r\n", " | ");
                pk1.Ad = str;
                pk1.Sinif = "94";
                pk1.Kod = say.ToString();
                kodlar.Add(pk1);
                say++;
            }

            try
            {
                SqlConnection conn = new SqlConnection(dbtools.connstr);
                conn.Open();


                foreach (var Pkod in kodlar)
                {
                    SqlCommand cmd = new SqlCommand("spymz_Poskod", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Pkod_Kod", SqlDbType.NVarChar).Value = Pkod.Kod;
                    cmd.Parameters.Add("@Pkod_Ad", SqlDbType.NVarChar).Value = Pkod.Ad;
                    cmd.Parameters.Add("@Pkod_Sinif", SqlDbType.NVarChar).Value = Pkod.Sinif;
                    cmd.ExecuteNonQuery();

                }
                conn.Close();
                MessageBox.Show(res_man.GetString("Aktarim Bitmistir \r\n Kod Sinifları \r\n 91:Odeme Sekilleri \r\n 92:Departman Kısımları \r\n 93:Bankalar \r\n 94:Yemek Cekleri"));

            }
            catch (Exception ex)
            {
                MessageBox.Show(res_man.GetString("Sql'e Aktarim Yapılırken Hata Olustu:") + ex.ToString());
            }

        }

        private void rdo_Odeme_OzelKod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdo_Odeme_OzelKod.SelectedIndex == 7)
            {

                look_Bankakodu.Properties.DataSource = dbtools.SelectTable("select Convert(int,Pkod_Kod) as Pkod_Kod ,Pkod_Ad from Pos_Kodlar where Pkod_Sinif = '94'");
                look_Bankakodu.Properties.DisplayMember = "Pkod_Ad";
                look_Bankakodu.Properties.ValueMember = "Pkod_Kod";
            }
            else
            {

                look_Bankakodu.Properties.DataSource = dbtools.SelectTable("select Convert(int,Pkod_Kod) as Pkod_Kod,Pkod_Ad from Pos_Kodlar where Pkod_Sinif = '93'");
                look_Bankakodu.Properties.DisplayMember = "Pkod_Ad";
                look_Bankakodu.Properties.ValueMember = "Pkod_Kod";
            }
        }

        private void Param_SatisTabloAktif_CheckedChanged(object sender, EventArgs e)
        {
            Param_SatisTabloGonderi.Enabled = Param_SatisTabloAktif.Checked;
        }

        private void Ayarlar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F12)
            {
                Param_SatisTabloID.Enabled = true;
            }

            if (User.P_Kulturu == 3)
            {
                if (e.Control && e.KeyCode == Keys.P)
                {
                    txt_Kul_Kart.Properties.PasswordChar = '\0';
                }
            }

            if (e.KeyCode == Keys.F5)
            {
                adminForms admin = new adminForms();
                admin.ShowDialog();
            }
        }

        private void tab_Masa_Konum_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton17_Click(object sender, EventArgs e)
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

                try
                {
                    System.Threading.Thread.Sleep(1 * 1000);
                    Application.DoEvents();


                    MailMessage ePosta = new MailMessage();
                    ePosta.From = new MailAddress(Mail_Adres, Mail_Isim);
                    if (Mail_Alici1.Length > 0) ePosta.To.Add(Mail_Alici1);

                    ePosta.Priority = MailPriority.Normal;
                    ePosta.Subject = Param.Tarih + " Tarihli Test e-Mail";
                    //ePosta.Attachments.Add(att);
                    //ePosta.Attachments.Add(att2);
                    //ePosta.Attachments.Add(att3);

                    string mailbody = Param.Tarih + " Tarihli Test e-Mail";

                    ePosta.IsBodyHtml = true;
                    ePosta.Body = mailbody;

                    SmtpClient ss = new SmtpClient(Mail_Host, Convert.ToInt32(Mail_Port));
                    ss.EnableSsl = Mail_SSL;
                    ss.DeliveryMethod = SmtpDeliveryMethod.Network;
                    ss.UseDefaultCredentials = false;
                    ss.Credentials = new NetworkCredential(Mail_Adres, Mail_Parola);
                    ss.Send(ePosta);

                    MessageBox.Show(res_man.GetString("Test Maili Gönderildi..."));

                }
                catch (Exception ex)
                {
                    MessageBox.Show(res_man.GetString("Mail Gönderilemedi...") + "\n" + ex.Message);
                    return;
                }
            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'HESAP'");

            if (dt.Rows.Count > 0)
            {
                DevExpress.XtraReports.UI.XtraReport myReport = xtraDizayn.BuildReport(Convert.ToString(dt.Rows[0]["Rapor_Id"]));
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream(Convert.ToString(dt.Rows[0]["Rapor_Id"]), null, myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Duzelt, "Hesap Dizaynı Değiştirildi.", String.Empty, String.Empty);
                }
            }
            else
            {
                Print.Hesap myReport = new Print.Hesap();
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream("0", "HESAP", myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, "Hesap Dizaynı Kaydedildi.", String.Empty, String.Empty);
                }
            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'SIPARISFISI'");

            if (dt.Rows.Count > 0)
            {
                DevExpress.XtraReports.UI.XtraReport myReport = xtraDizayn.BuildReport(Convert.ToString(dt.Rows[0]["Rapor_Id"]));
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream(Convert.ToString(dt.Rows[0]["Rapor_Id"]), null, myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "Sipariş Dizaynı Değiştirildi.", String.Empty, String.Empty);
                }
            }
            else
            {
                Print.Siparis myReport = new Print.Siparis();
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream("0", "SIPARISFISI", myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "Sipariş Dizaynı Kaydedildi.", String.Empty, String.Empty);
                }
            }
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'IPTALFISI'");

            if (dt.Rows.Count > 0)
            {
                DevExpress.XtraReports.UI.XtraReport myReport = xtraDizayn.BuildReport(Convert.ToString(dt.Rows[0]["Rapor_Id"]));
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream(Convert.ToString(dt.Rows[0]["Rapor_Id"]), null, myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "İptal Dizaynı Değiştirildi.", String.Empty, String.Empty);
                }
            }
            else
            {
                Print.IptalFisi myReport = new Print.IptalFisi();
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream("0", "IPTALFISI", myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "İptal Dizaynı Kaydedildi.", String.Empty, String.Empty);
                }
            }
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'ZAYIFISI'");

            if (dt.Rows.Count > 0)
            {
                DevExpress.XtraReports.UI.XtraReport myReport = xtraDizayn.BuildReport(Convert.ToString(dt.Rows[0]["Rapor_Id"]));
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream(Convert.ToString(dt.Rows[0]["Rapor_Id"]), null, myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "Zayi Dizaynı Değiştirildi.", String.Empty, String.Empty);
                }
            }
            else
            {
                Print.Zayi myReport = new Print.Zayi();
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream("0", "ZAYIFISI", myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "Zayi Dizaynı Kaydedildi.", String.Empty, String.Empty);
                }
            }
        }

        private void simpleButton23_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'PAKETFISI'");

            if (dt.Rows.Count > 0)
            {
                DevExpress.XtraReports.UI.XtraReport myReport = xtraDizayn.BuildReport(Convert.ToString(dt.Rows[0]["Rapor_Id"]));
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream(Convert.ToString(dt.Rows[0]["Rapor_Id"]), null, myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "Paket Dizaynı Değiştirildi.", String.Empty, String.Empty);
                }
            }
            else
            {
                Print.Paket myReport = new Print.Paket();
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream("0", "PAKETFISI", myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "Paket Dizaynı Kaydedildi.", String.Empty, String.Empty);
                }
            }
        }

        private void Pos_OdaKontrol_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btn_Print_Adisyon_Sil_Click(object sender, EventArgs e)
        {
            string dizaynAd = "ADISYON";

            Print.Adisyon myReport = new Print.Adisyon();

            xtraDizayn.SaveReportStream("0", dizaynAd, myReport);
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, dizaynAd + " Dizaynı Kaydedildi.", String.Empty, String.Empty);

            dizaynSil(dizaynAd);


        }

        private void btn_Print_Fatura_Sil_Click(object sender, EventArgs e)
        {
            string dizaynAd = "POS_FATURA";

            Print.Fatura myReport = new Print.Fatura();

            xtraDizayn.SaveReportStream("0", dizaynAd, myReport);
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, dizaynAd + " Dizaynı Kaydedildi.", String.Empty, String.Empty);

            dizaynSil(dizaynAd);
        }

        private void btn_Print_Hesap_Sil_Click(object sender, EventArgs e)
        {
            string dizaynAd = "HESAP";
            Print.Hesap myReport = new Print.Hesap();

            xtraDizayn.SaveReportStream("0", dizaynAd, myReport);
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, dizaynAd + " Dizaynı Kaydedildi.", String.Empty, String.Empty);

            dizaynSil(dizaynAd);

        }

        private void btn_Print_Siparis_Sil_Click(object sender, EventArgs e)
        {

            string dizaynAd = "SIPARISFISI";
            Print.Siparis myReport = new Print.Siparis();

            xtraDizayn.SaveReportStream("0", dizaynAd, myReport);
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, dizaynAd + " Dizaynı Kaydedildi.", String.Empty, String.Empty);

            dizaynSil(dizaynAd);

        }

        private void btn_Print_Iptal_Sil_Click(object sender, EventArgs e)
        {

            string dizaynAd = "IPTALFISI";
            Print.IptalFisi myReport = new Print.IptalFisi();


            xtraDizayn.SaveReportStream("0", dizaynAd, myReport);
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, dizaynAd + " Dizaynı Kaydedildi.", String.Empty, String.Empty);

            dizaynSil(dizaynAd);


        }

        private void btn_Print_Zayi_Sil_Click(object sender, EventArgs e)
        {

            string dizaynAd = "ZAYIFISI";
            Print.Zayi myReport = new Print.Zayi();


            xtraDizayn.SaveReportStream("0", dizaynAd, myReport);
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, dizaynAd + " Dizaynı Kaydedildi.", String.Empty, String.Empty);

            dizaynSil(dizaynAd);
        }

        private void btn_Print_Paket_Sil_Click(object sender, EventArgs e)
        {

            string dizaynAd = "PAKETFISI";
            Print.Paket myReport = new Print.Paket();

            xtraDizayn.SaveReportStream("0", dizaynAd, myReport);
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, dizaynAd + " Dizaynı Kaydedildi.", String.Empty, String.Empty);

            dizaynSil(dizaynAd);
        }

        public void dizaynSil(string dizaynAd)
        {
            dbtools.execcmd("delete from Rapor_Dizayn where Rapor_Kod='" + dizaynAd + "'");
            MessageBox.Show(dizaynAd + " Silindi ve Yenisi Kaydedildi");
        }

        private void btn_EntCost_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSqlStopStart_Click(object sender, EventArgs e)
        {
            Klavye2 klavye = new Klavye2();
            klavye.ShowDialog();

            if (klavye.yazi.Equals("111"))
            {
                SqlStopStart();
                RHMesaj.alertMesaj("SQL YENİDEN BAŞLADI ", 3);
            }
            else
            {
                RHMesaj.MyMessageInformation("Şifre Yanlış ! ");
            }

        }

        public void SqlStopStart()
        {
            try
            {
                string path = "SqlStopStart.bat";
                if (!File.Exists(path))
                {
                    string sqlInstanceName = getInstanceName();
                    File.WriteAllText("SqlStopStart.bat", "net stop " + sqlInstanceName + " && net start " + sqlInstanceName + "");
                }
                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo(path);
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.Verb = "runas";
                p.StartInfo = psi;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA " + ex.Message);

            }
        }

        public string getInstanceName()
        {
            string query = @"DECLARE @GetInstances TABLE
( Value nvarchar(100),
 InstanceNames nvarchar(100),
 Data nvarchar(100))

Insert into @GetInstances
EXECUTE xp_regread
  @rootkey = 'HKEY_LOCAL_MACHINE',
  @key = 'SOFTWARE\Microsoft\Microsoft SQL Server',
  @value_name = 'InstalledInstances'

Select InstanceNames from @GetInstances ";

            return dbtools.DegerGetir(query);
        }

        private void btnPaketKucukEkran_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            tabParametreler();
        }

        private void btnUrunSizeKaydet_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("update Stok_Kodlar set Kodlar_Size='" + txtUrunSize.Text + "' where Kodlar_Sinif='09'");
        }

        private void txtParcaliDepartmanSec_EditValueChanged(object sender, EventArgs e)
        {
            look_MasaTan_Departman.EditValue = txtParcaliDepartmanSec.EditValue;
        }



        private void btnUrunPrinterKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                string departman = lookUpEditUrunPrinterDepartman.EditValue.ToString();
                string yazici = comboBoxEditUrunPrinter.EditValue.ToString();


                bool depvarmi = false;
                foreach (var item in Main.ayarlar.urunPrintModels)
                {
                    if (item.departman.Equals(departman))
                    {
                        depvarmi = true;
                        item.departman = departman;
                        item.yazici = yazici;
                    }
                }

                if (depvarmi == false)
                {
                    UrunPrintModel urun = new UrunPrintModel();
                    urun.yazici = yazici;
                    urun.departman = departman;
                    Main.ayarlar.urunPrintModels.Add(urun);
                }

                string newJson = JsonConvert.SerializeObject(Main.ayarlar.urunPrintModels).Replace("'", "''");

                string query = "update ayarlar set ayarlar_value='" + newJson + "' where ayarlar_key='urun_printer'";
                dbtools.execcmd(query);

                RHMesaj.MyMessage("KAYDEDİLDİ...");
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnUrunPrinterKaydet_Click", "", ex);
            }
        }

        private void btnAyarlarYenile_Click(object sender, EventArgs e)
        {

        }
        public void MyGridDoldurAyarlar()
        {
            gridControl_ayarlar.DataSource = dbtools.SelectTable("select * from ayarlar");
            Main.ayarlar.yenile();
        }
        private void gridView_ayarlar_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                // ayarlar_key ayarlar_value
                GridView view = sender as GridView;
                if (view == null) return;

                int index = e.RowHandle;
                if (index < 0)
                {
                    return;
                }

                string oldvalue = gridView_ayarlar.ActiveEditor.OldEditValue.ToString();
                string newvalue = gridView_ayarlar.ActiveEditor.EditValue.ToString().Replace("'", "''");


                string ayarlar_key = gridView_ayarlar.GetRowCellValue(index, gridView_ayarlar.Columns["ayarlar_key"]).ToString();
                string mesaj = "\"" + ayarlar_key + "\" Alanını \"" + oldvalue + "\" -> \"" + newvalue + "\" Olarak Güncellemek İster misin ?";


                if (RHMesaj.MyMessageConfirmation(mesaj))
                {
                    dbtools.execcmd("update ayarlar set ayarlar_value='" + newvalue + "' where ayarlar_key='" + ayarlar_key + "'");
                    RHMesaj.alertMesaj(ayarlar_key + " GÜNCELLENDİ...");
                }
                else
                {
                    RHMesaj.alertMesaj("İPTAL EDİLDİ...");
                }

                MyGridDoldurAyarlar();
                gridView_ayarlar.FocusedRowHandle = index;

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "gridView_ayarlar_CellValueChanged", "", ex);
            }
        }

        private void xtraTabControl3_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            MyGridDoldurAyarlar();
        }


        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            string pageText = xtraTabControl1.SelectedTabPage.Text;
            switch (pageText)
            {
                case "Parametre":
                    MyGridDoldurAyarlar();
                    break;
                case "Mac Print Ayarı":
                    lookUpEditUrunPrinterDepartman.EditValue = Departman.Dep_Kodu;
                    comboBoxEditUrunPrinter.EditValue = Main.ayarlar.getYazici();
                    break;

            }
        }

        private void xtraTabControl5_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl5.SelectedTabPage.Text.Equals("ÜRÜN PRINTER AYARLARI"))
            {
                lookUpEditUrunPrinterDepartman.EditValue = Departman.Dep_Kodu;
                comboBoxEditUrunPrinter.EditValue = Main.ayarlar.getYazici();
            }
        }

        private void btnUrunDizayn_Click(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'URUN'");

            if (dt.Rows.Count > 0)
            {
                DevExpress.XtraReports.UI.XtraReport myReport = xtraDizayn.BuildReport(Convert.ToString(dt.Rows[0]["Rapor_Id"]));
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream(Convert.ToString(dt.Rows[0]["Rapor_Id"]), null, myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Duzelt, "URUN Dizaynı Değiştirildi.", String.Empty, String.Empty);
                }
            }
            else
            {
                RaporUrun myReport = new RaporUrun();
                myReport.ShowDesignerDialog();
                if (MessageBox.Show(res_man.GetString("Rapor Dataya Kaydedilsin mi...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    xtraDizayn.SaveReportStream("0", "URUN", myReport);
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_SiparisFis, Log.Log_Islem.Kaydet, "URUN Dizaynı Kaydedildi.", String.Empty, String.Empty);
                }
            }

        }

        private void btnUrunDizaynSil_Click(object sender, EventArgs e)
        {
            string dizaynAd = "URUN";
            RaporUrun myReport = new RaporUrun();

            xtraDizayn.SaveReportStream("0", dizaynAd, myReport);
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_HesapFis, Log.Log_Islem.Kaydet, dizaynAd + " Dizaynı Kaydedildi.", String.Empty, String.Empty);

            dizaynSil(dizaynAd);
        }

        private void lookUpEditUrunPrinterDepartman_EditValueChanged(object sender, EventArgs e)
        {
            comboBoxEditUrunPrinter.EditValue = Main.ayarlar.getYazici(lookUpEditUrunPrinterDepartman.EditValue.ToString());
        }

        private void yuvarlamaDepartman_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                YuvarlaModel model = Main.ayarlar.getYuvarlama(yuvarlamaDepartman.EditValue.ToString());
                if (model != null)
                {
                    yuvarlamaFiyat.Text = model.yuvarlamaFiyat + "";
                    yuvarlamaRecete.EditValue = model.yuvarlamaRecete;
                }
                else
                {
                    yuvarlamaFiyat.EditValue = 0;
                    yuvarlamaFiyat.Text = "0";
                    yuvarlamaRecete.EditValue = null;
                }
            }
            catch (Exception ex)
            {
                RHMesaj.alertMesaj(ex.Message);
            }

        }

        private void lookUpOtoIndirimDep_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                IndirimModel model = Main.ayarlar.getIndirimModel(lookUpOtoIndirimDep.EditValue.ToString());
                if (model != null)
                {
                    checkOtoIndirimAktif.Checked = model.aktif;
                }
                else
                {
                    checkOtoIndirimAktif.Checked = false;
                }
            }
            catch (Exception ex)
            {
                RHMesaj.alertMesaj(ex.Message);
            }

        }

        private void Pos_ServisPayiDuzelt_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_G_Tutarduzelt.Checked)
            {
                chk_G_Tutarduzelt.Checked = false;
            }

        }

        private void chk_G_Tutarduzelt_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_G_Tutarduzelt.Checked)
            {
                Pos_ServisPayiDuzelt.Checked = false;
                Console.WriteLine("olay oldu");
            }
        }

        private void gridyenile_SubeAdres()
        {
            gridControl21.DataSource = dbtools.SelectTable(@"
                            select sube.Pkod_Kod as subeKod, sube.Pkod_Ad as subeAd, il.Adres_Kod as ilKod, il.Adres_Ad as idAd, 
                            ilce.Adres_Kod as ilceKod, ilce.Adres_Ad as ilceAd, mahalle.Adres_Id as mahalleId, mahalle.Adres_Kod as mahalleKod, mahalle.Adres_Ad as mahalleAd
                            from Pos_Adres as mahalle
                            left join Pos_Adres as ilce on mahalle.Adres_AltGrup = ilce.Adres_Kod and ilce.Adres_Sinif = '25'
                            left join Pos_Adres as il on mahalle.Adres_UstGrup = il.Adres_Kod and il.Adres_Sinif = '24'
                            left join Pos_Kodlar as sube on mahalle.Adres_Sube = sube.Pkod_Kod and sube.Pkod_Sinif = '27'
                            where mahalle.Adres_Sinif = '26' ");

            gridView21.BestFitColumns();
        }
        #endregion
    }
}
