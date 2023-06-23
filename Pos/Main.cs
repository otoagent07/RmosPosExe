using DevExpress.XtraBars.Alerter;
using DevExpress.XtraReports.UI;
using Pos.Class;
using Pos.Controllers;
using Pos.Entities;
using Pos.Forms;
using Pos.Getir;
using Pos.Getir.Class;
using Pos.Ingenico;
using Pos.Print;
using Pos.Trendyol;
using Pos.Update;
using Pos.YemekSepeti;
using RmosAcentex.Forms;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Media;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace Pos
{

    public partial class Main : DevExpress.XtraEditors.XtraForm
    {
        SimpleTcpClient client;
        public bool isLogin { get; set; }

        public static SatisListesi a = new SatisListesi();

        //int m_type = 0;

        public Main()
        {
            InitializeComponent();
        }

        public void dilYenile()
        {
            try
            {
                lbl_Tarih.Text = Param.Tarih.ToLongDateString();

                if (Langs.Default.Dil == "tr-TR")
                {
                    btnMasaTakip.Text = "Masa Takip";
                    btnDirekSatis.Text = "Direkt Satış";
                    btn_HizliSatis.Text = "Hızlı Satış";
                    btnRaporlar.Text = "Raporlar";
                    simpleButton1.Text = "Yönetim İşlemleri";
                    btn_Rez.Text = "Rezervasyon İşlemleri";
                    btnKasa.Text = "Kasa İşlemleri";
                    btnCari.Text = "Cari İşlemleri";
                    btn_ExtraTanimlama.Text = "Extra Kart Tanımlama";
                    btnGunSonu.Text = "Gün Sonu";
                    btnDepDegis.Text = "Departman Değiştir";
                    simpleButton3.Text = "Uzaktan Yardım";
                    btnAyarlar.Text = "Ayarlar";
                    simpleButton6.Text = "Kur Girişi";
                    barButtonItem1.Caption = "ÇIKIŞ";

                    labelControl1.Text = "Departman   : ";
                    labelControl2.Text = "Kullanıcı         : ";
                    labelControl5.Text = "Tarih   : ";
                    labelControl3.Text = "Aktif Kur        : ";
                    txt_FiyatSekli.Text = "Fiyat Şekli     : ";

                    groupControl1.Text = "Bilgi Paneli";
                    groupControl2.Text = "Kurlar";
                    gridView1.ViewCaption = "Kurlar";
                }
                else if (Langs.Default.Dil == "en-US")
                {
                    btnMasaTakip.Text = "Table Track";
                    btnDirekSatis.Text = "Direct Selling";
                    btn_HizliSatis.Text = "Fast Selling";
                    btnRaporlar.Text = "Reports";
                    simpleButton1.Text = "Management Operations";
                    btn_Rez.Text = "Reservation Procedures";
                    btnKasa.Text = "Cash Transactions";
                    btnCari.Text = "Accounts Transactions";
                    btn_ExtraTanimlama.Text = "Extra Card Identification";
                    btnGunSonu.Text = "End of the Day";
                    btnDepDegis.Text = "Change Department";
                    simpleButton3.Text = "Remote Assistance";
                    btnAyarlar.Text = "Settings";
                    simpleButton6.Text = "Currency Entry";
                    barButtonItem1.Caption = "CLOSE";

                    labelControl1.Text = "Department   : ";
                    labelControl2.Text = "User                 : ";
                    labelControl5.Text = "Date  : ";
                    labelControl3.Text = "Active Rate    : ";
                    txt_FiyatSekli.Text = "Price Form      : ";

                    groupControl1.Text = "Info Panel";
                    groupControl2.Text = "Exchange Rate";
                    gridView1.ViewCaption = "Exchange Rate";


                }
                else if (Langs.Default.Dil == "ru")
                {
                    btnMasaTakip.Text = "Отслеживание стола";
                    btnDirekSatis.Text = "Прямая продажа";
                    btn_HizliSatis.Text = "Быстрые продажи";
                    btnRaporlar.Text = "Отчеты";
                    simpleButton1.Text = "Административные операции";
                    btn_Rez.Text = "Операции по резервации";
                    btnKasa.Text = "Кассовые операции";
                    btnCari.Text = "Текущие операции";
                    btn_ExtraTanimlama.Text = "Определение доп. карты";
                    btnGunSonu.Text = "Конец дня";
                    btnDepDegis.Text = "Смена филиала";
                    simpleButton3.Text = "Удаленная помощь";
                    btnAyarlar.Text = "Настройки";
                    simpleButton6.Text = "Ввод курса";
                    barButtonItem1.Caption = "ВЫХОД";

                    labelControl1.Text = "Филиал    : ";
                    labelControl2.Text = "Пользователь    :";
                    labelControl5.Text = "Дата :";
                    labelControl3.Text = "Активный курс   :";
                    txt_FiyatSekli.Text = "Форма цены     : ";

                    groupControl1.Text = "Информационная панель";
                    groupControl2.Text = "Курсы";
                    gridView1.ViewCaption = "Курсы";


                }
            }
            catch (Exception ex)
            {

            }
        }

        public static bool direkGecis = false;
        DataTable dt_P_User;
        public Main(string[] args)
        {
            try
            {
                direkGecis = true;
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = args[i].Replace("!!!", "");
                }

                dbtools.server = args[0];
                dbtools.users = args[1];
                dbtools.pwd = args[2];
                dbtools.database = args[3];


                User.P_Kod = args[4];
                User.P_Sifre = args[5];

                DataTable dt = dbtools.SelectTable("select isnull(Param_Kartla_Giris,0) as Param_Kartla_Giris from Pos_Param where Param_Id = '1'");
                if (dt.Rows.Count > 0)
                {
                    bool Param_Kartla_Giris = Convert.ToBoolean(dt.Rows[0]["Param_Kartla_Giris"]);
                    if (Param_Kartla_Giris)
                    {
                        dt_P_User = Rmosmuh.SelectTable("select top 1 * from Pos_User with(nolock) where P_Kart = '" + User.P_Kod + "' or P_Kod='" + User.P_Kod + "'");
                        if (dt_P_User.Rows.Count > 0)
                        {
                            User.P_Kod = Convert.ToString(dt_P_User.Rows[0]["P_Kod"]);
                            User.P_Sifre = Convert.ToString(dt_P_User.Rows[0]["P_Sifre"]);
                            User.P_Ad = Convert.ToString(dt_P_User.Rows[0]["P_Ad"]);
                            User.P_Soyad = Convert.ToString(dt_P_User.Rows[0]["P_Soyad"]);
                            User.P_Kart = Convert.ToString(dt_P_User.Rows[0]["P_Kart"]);

                            User.Yetki_Yukle();
                        }
                    }
                    else
                    {
                        dt_P_User = Rmosmuh.SelectTable("select * from Pos_User with(nolock) where P_Kod = '" + User.P_Kod + "' and P_Sifre = '" + User.P_Sifre + "' ");
                        if (dt_P_User.Rows.Count > 0)
                        {
                            User.P_Kod = Convert.ToString(dt_P_User.Rows[0]["P_Kod"]);
                            User.P_Sifre = Convert.ToString(dt_P_User.Rows[0]["P_Sifre"]);
                            User.P_Ad = Convert.ToString(dt_P_User.Rows[0]["P_Ad"]);
                            User.P_Soyad = Convert.ToString(dt_P_User.Rows[0]["P_Soyad"]);
                            User.P_Kart = Convert.ToString(dt_P_User.Rows[0]["P_Kart"]);

                            User.Yetki_Yukle();

                        }
                    }
                }


                //MessageBox.Show(args[0]);
                InitializeComponent();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Main", "", ex);
            }

        }


        private string Mac()
        {
            try
            {
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
                return mac2;

                //ManagementClass manager = new ManagementClass("Win32_NetworkAdapterConfiguration");
                //foreach (ManagementObject obj in manager.GetInstances())
                //{
                //    if ((bool)obj["IPEnabled"])
                //    {


                //        string mac3 = obj["MacAddress"].ToString();
                //        return mac3;
                //    }
                //}
            }
            catch (Exception ex)
            {

            }

            return "";
        }

        string xMac = String.Empty;

        string xSubeMac = String.Empty;

        private void CekSil()
        {
            string query = "Select Rsat_Fisno,Rsat_Masa,Rsat_Departman From Cst_Recete_Satis Where Rsat_Tarih < '" + Param.Tarih + "' and Rsat_Durum = 'A' group by Rsat_Fisno,Rsat_Masa,Rsat_Departman";
            DataTable dtAcik = dbtools.SelectTable(query);
            if (dtAcik.Rows.Count > 0)
            {
                for (int i = 0; i < dtAcik.Rows.Count; i++)
                {
                    dbtools.execcmd("Delete From Cst_Recete_Satis Where Rsat_Fisno = '" + Convert.ToInt32(dtAcik.Rows[i]["Rsat_Fisno"]) + "'");

                    dbtools.execcmd("Update Pos_Masa Set Masa_Durum = 0,Masa_NFC = 0 where Masa_Depart = '" + Convert.ToString(dtAcik.Rows[i]["Rsat_Departman"]) + "' and Masa_No = '" + Convert.ToString(dtAcik.Rows[i]["Rsat_Masa"]) + "'");

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Menu, Log.Log_Islem.Sil, Convert.ToString(dtAcik.Rows[i]["Rsat_Fisno"]) + " NL Açık Fiş Silindi.. Yetkili Silme İşlemi. ", Convert.ToString(dtAcik.Rows[i]["Rsat_Fisno"]), "", "", 0);

                }
            }
        }

        //CultureInfo culture = new CultureInfo("tr-TR");
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        public frmLogin login;
        public static string MyClass = "Main";

        public  void departmanYukleNew()
        {
            try
            {
                isLogin = true;
                xMac = Mac();
                xSubeMac = SubeMac();
                Giris_Yap();
                if (Departman.Kodlar_Getir_AP && Param.Param_GetirTest)
                {
                    GetirStatik.baseUri = GetirStatik.baseUriTest;
                    GetirStatik.yenile();
                }

                if (Param.Tesis_Tipi == 1)
                {
                    string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";
                    DataTable dtKur = dbtools.SelectTable("Select MKodlar_Ad as Kodlar_Ad," + Departman.MKodlar_P_DovizTuru + " from Kurlar with(nolock) left join Muh_Kodlar with(nolock) on MKodlar_Kod = Kurlar_Kodu and MKodlar_Sinif = '02'  where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Tarih = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "' ");
                    if (dtKur.Rows.Count < 1)
                    {
                        MessageBox.Show(Param.Tarih.ToString("dd.MM.yyyy") + " Tarihli Kurlar Yoktur...Kur Girişinden Günlük Kurları Tanımlayabilirisiniz.", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        kurAc();
                    }
                }
                if (Param.Param_KurTransfer == true)
                {
                    KurTransfer();
                }
                Bilgi_Paneli();
                if (isLogin == true && Param.Param_CallerID == true)
                {
                    try
                    {
                        CallerId c = new CallerId();
                        c.Show();
                        c.Visible = false;
                        c.notifyIcon1.Visible = true;
                        c.ShowInTaskbar = true;
                        c.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                        c.notifyIcon1.BalloonTipTitle = "Caller Id";
                        c.notifyIcon1.BalloonTipText = "Caller Id Çalışıyor...!!!";
                        c.notifyIcon1.ShowBalloonTip(50);

                    }
                    catch (Exception ex)
                    {
                        RHMesaj.MyMessageInformation(ex.Message);
                    }
                }
                bool Update = Param.Param_AutoUpdate;
                if (Update == true)
                {
                    DialogResult a = MessageBox.Show("Program İçerisinde Yeni Güncelleme Mevcuttur.", "Güncelleme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (a == DialogResult.Yes)
                    {
                        Updater u = new Updater();
                        u.ShowDialog();
                    }
                }
                barButtonItem4.Enabled = User.Pos_SubeTrf;
                Main_Tarih.Text = Param.Tarih.ToString("dd.MM.yyyy") + "\n" + Param.Tarih.ToString("dddd");
                if (Param.Param_FullPos == true)
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                }
                IngenicoKullan();
                if (Param.Param_AnaEkranCiro)
                {
                    groupControl4.Visible = true;
                    Anlik();
                }

                if (Param.Param_AcilisCekSil)
                {
                    CekSil();
                }

                string Maxc = Mac();


                if (Departman.Kodlar_YS_Aktif && Maxc == Departman.Kodlar_YSMac)
                {
                    YS_SiparisGeldi a = new YS_SiparisGeldi();
                    a.timer1.Interval = (YS_RestoInfo.YS_ServiceTime * 1000);
                    a.Show();
                    a.Visible = false;
                    a.notifyIcon1.Visible = true;
                    a.ShowInTaskbar = true;
                    a.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    a.notifyIcon1.BalloonTipTitle = "YemekSepeti Servis";
                    a.notifyIcon1.BalloonTipText = "YemekSepeti Servisi Çalışıyor...";
                    a.notifyIcon1.ShowBalloonTip(50);

                }

                if (Param.Param_AcilistaMenu)
                {
                    Sube2Merkez a = new Sube2Merkez();
                    a.ReceteGetir();
                }

                if (Param.Param_SatisTabloAktif && Param.Param_SatisTabloGonderi > 0)
                {
                    timer_Sube2Merkez.Enabled = true;
                }

                if (User.ExtraFolio == true)
                {
                    if (Param.Param_KartfGBCheckOut && Param.Tesis_Tipi == 0)
                    {
                        Fronttools.execcmd("Update Kartf Set CardF_R_I_H = 'H' Where CardF_CikisTrh <= '" + Param.Tarih.AddDays(-1) + "'");
                    }
                }

                if (User.P_Ad.Equals("ALİ") && User.P_Soyad.Equals("VELİ"))
                {

                }
                else
                {
                    if (System.Windows.Forms.Screen.AllScreens.Length > 1)
                    {
                        Main.a.Location = Screen.AllScreens[1].Bounds.Location;
                        Main.a.Show();

                    }
                }


                this.Text = "RMOS Ultimate POS [" + dbtools.database + "] v0.3.42";



                if (Departman.Kodlar_Getir_AP && Maxc == Departman.Kodlar_YSMac)
                {
                    if (RestoGetir())
                    {
                        RHMesaj.toastMesaj("GetirYemek Servisi Çalışıyor...");
                        getirYenile();

                    }
                    else
                    {
                        RHMesaj.alertMesaj("Getir Yemek appSecretKey veya restaurantSecretKey hatalıdır !");
                    }
                }

                string trendyolDurum = dbtools.DegerGetir("select top 1 trendyolDurum from entegreAyarlar where recDep='" + Departman.Dep_Kodu + "'");

                if ((trendyolDurum.ToLower().Equals("true") || trendyolDurum.Equals("1")) && Maxc == Departman.Kodlar_YSMac)
                {
                    entegreAyarlarYenile();
                    trendyolYenile();

                    RHMesaj.toastMesaj("TRENDYOL Servisi Çalışıyor...");
                }
                else
                {
                    timerTrendyol.Enabled = false;
                    barButtonItem6.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void loginOlNew()
        {
            try
            {
                login = new frmLogin();
                login.ShowDialog();

                if (login.Cikis)
                {
                    Application.Exit();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loginOlNew()\n" + ex.Message);
                
            }
        }
        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                this.BringToFront();
                isLogin = false;
                if (direkGecis == false)
                {
                    login = new frmLogin();
                    login.ShowDialog();

                    if (login.Cikis)
                    {
                        Application.Exit();
                        return;
                    }
                }

                departmanYukleNew();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Main_Load", "", ex);
            }

        }

        //SimpleTcpClient client;

        // aşağısı 06.05.2023 tarihinde yedeği alında
        private void Main_LoadYeni(object sender, EventArgs e)
        {
            try
            {
                this.BringToFront();
                isLogin = false;
                if (direkGecis == false)
                {
                    login = new frmLogin();
                    login.ShowDialog();

                    if (login.Cikis)
                    {
                        Application.Exit();
                        return;
                    }
                }

                isLogin = true;
                xMac = Mac();
                xSubeMac = SubeMac();
                Giris_Yap();
                if (Departman.Kodlar_Getir_AP && Param.Param_GetirTest)
                {
                    GetirStatik.baseUri = GetirStatik.baseUriTest;
                    GetirStatik.yenile();
                }

                if (Param.Tesis_Tipi == 1)
                {
                    string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";
                    DataTable dtKur = dbtools.SelectTable("Select MKodlar_Ad as Kodlar_Ad," + Departman.MKodlar_P_DovizTuru + " from Kurlar with(nolock) left join Muh_Kodlar with(nolock) on MKodlar_Kod = Kurlar_Kodu and MKodlar_Sinif = '02'  where Kurlar_Cesit = '" + kur_cesit + "' and Kurlar_Tarih = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "' ");
                    if (dtKur.Rows.Count < 1)
                    {
                        MessageBox.Show(Param.Tarih.ToString("dd.MM.yyyy") + " Tarihli Kurlar Yoktur...Kur Girişinden Günlük Kurları Tanımlayabilirisiniz.", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        kurAc();
                    }
                }
                if (Param.Param_KurTransfer == true)
                {
                    KurTransfer();
                }
                Bilgi_Paneli();
                if (isLogin == true && Param.Param_CallerID == true)
                {
                    try
                    {
                        CallerId c = new CallerId();
                        c.Show();
                        c.Visible = false;
                        c.notifyIcon1.Visible = true;
                        c.ShowInTaskbar = true;
                        c.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                        c.notifyIcon1.BalloonTipTitle = "Caller Id";
                        c.notifyIcon1.BalloonTipText = "Caller Id Çalışıyor...!!!";
                        c.notifyIcon1.ShowBalloonTip(50);

                    }
                    catch (Exception ex)
                    {
                        RHMesaj.MyMessageInformation(ex.Message);
                    }
                }
                bool Update = Param.Param_AutoUpdate;
                if (Update == true)
                {
                    DialogResult a = MessageBox.Show("Program İçerisinde Yeni Güncelleme Mevcuttur.", "Güncelleme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (a == DialogResult.Yes)
                    {
                        Updater u = new Updater();
                        u.ShowDialog();
                    }
                }
                barButtonItem4.Enabled = User.Pos_SubeTrf;
                Main_Tarih.Text = Param.Tarih.ToString("dd.MM.yyyy") + "\n" + Param.Tarih.ToString("dddd");
                if (Param.Param_FullPos == true)
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                }
                IngenicoKullan();
                if (Param.Param_AnaEkranCiro)
                {
                    groupControl4.Visible = true;
                    Anlik();
                }

                if (Param.Param_AcilisCekSil)
                {
                    CekSil();
                }

                string Maxc = Mac();


                if (Departman.Kodlar_YS_Aktif && Maxc == Departman.Kodlar_YSMac)
                {
                    YS_SiparisGeldi a = new YS_SiparisGeldi();
                    a.timer1.Interval = (YS_RestoInfo.YS_ServiceTime * 1000);
                    a.Show();
                    a.Visible = false;
                    a.notifyIcon1.Visible = true;
                    a.ShowInTaskbar = true;
                    a.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    a.notifyIcon1.BalloonTipTitle = "YemekSepeti Servis";
                    a.notifyIcon1.BalloonTipText = "YemekSepeti Servisi Çalışıyor...";
                    a.notifyIcon1.ShowBalloonTip(50);

                }

                if (Param.Param_AcilistaMenu)
                {
                    Sube2Merkez a = new Sube2Merkez();
                    a.ReceteGetir();
                }

                if (Param.Param_SatisTabloAktif && Param.Param_SatisTabloGonderi > 0)
                {
                    timer_Sube2Merkez.Enabled = true;
                }

                if (User.ExtraFolio == true)
                {
                    if (Param.Param_KartfGBCheckOut && Param.Tesis_Tipi == 0)
                    {
                        Fronttools.execcmd("Update Kartf Set CardF_R_I_H = 'H' Where CardF_CikisTrh <= '" + Param.Tarih.AddDays(-1) + "'");
                    }
                }

                if (User.P_Ad.Equals("ALİ") && User.P_Soyad.Equals("VELİ"))
                {

                }
                else
                {
                    if (System.Windows.Forms.Screen.AllScreens.Length > 1)
                    {
                        Main.a.Location = Screen.AllScreens[1].Bounds.Location;
                        Main.a.Show();

                    }
                }


                this.Text += " [" + dbtools.database + "] v0.3.12";



                if (Departman.Kodlar_Getir_AP && Maxc == Departman.Kodlar_YSMac)
                {
                    if (RestoGetir())
                    {
                        RHMesaj.toastMesaj("GetirYemek Servisi Çalışıyor...");
                        getirYenile();

                    }
                    else
                    {
                        RHMesaj.alertMesaj("Getir Yemek appSecretKey veya restaurantSecretKey hatalıdır !");
                    }
                }

                string trendyolDurum = dbtools.DegerGetir("select top 1 trendyolDurum from entegreAyarlar where recDep='" + Departman.Dep_Kodu + "'");

                if (trendyolDurum.ToLower().Equals("true") || trendyolDurum.Equals("1"))
                {
                    entegreAyarlarYenile();
                    trendyolYenile();

                    RHMesaj.toastMesaj("TRENDYOL Servisi Çalışıyor...");
                }
                else
                {
                    timerTrendyol.Enabled = false;
                    barButtonItem6.Enabled = false;
                }


                //dbtools.execcmd(StatikSinif.getAlterQuery());

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Main_Load", "", ex);
            }

        }
        public void entegreAyarlarYenile()
        {
            TrendyolApi trendyolApi = new TrendyolApi();

            try
            {
                trendyolApi.requestGet(trendyolApi.ayarlar.trendyolApiLink);

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "entegreAyarlarYenile", trendyolApi.ayarlar.trendyolApiLink + "\nLinke ulaşılamıyor", ex);
            }
        }

        private void IngenicoKullan()
        {
            if (Departman.Kodlar_Ingenico)
            {
                if (Departman.Kodlar_IngenicoCon == 0)
                {
                    IngenicoConn a = new IngenicoConn();
                    a.EslestirmeYap(true);
                }
                else
                {
                    string programDizini = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                    string pro = programDizini + "\\IngenicoOKC\\RmosIngenicoGMP.exe";


                    Process[] processesByName = Process.GetProcessesByName("RmosIngenicoGMP");
                    if (processesByName.Length == 0)
                    {
                        Process.Start(@"" + pro);
                    }

                    //IngenicoOKC
                    //client = new SimpleTcpClient
                    //{
                    //    StringEncoder = Encoding.UTF8,
                    //    AutoTrimStrings = true
                    //};

                    ////client.DataReceived += client_DataReceived;

                    //client.Connect(Departman.Kodlar_Ingenico_IP, Convert.ToInt32(Departman.Kodlar_Ingenico_Port));
                    //if (!client.TcpClient.Connected)
                    //    client.Connect(Departman.Kodlar_Ingenico_IP, Convert.ToInt32(Departman.Kodlar_Ingenico_Port));

                    //client.WriteLine("ESLESTIR;");
                }
            }
        }

        private void KurTransfer()
        {
            if (Param.Kurlar_Nerden == 1)
            {
                int Count = Convert.ToInt32(dbtools.DegerGetir(@"select count(*) from kurlar where Kurlar_Tarih = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "'"));
                if (Count == 0)
                {
                    dbtools.execcmd(@"INSERT INTO [dbo].[Kurlar]
           ([Kurlar_Cesit]
           ,[Kurlar_Tarih]
           ,[Kurlar_Kodu]
           ,[Doviz_Alis]
           ,[Doviz_Satis]
           ,[Efektif_Alis]
           ,[Efektif_Satis])

                    SELECT [Kurlar_Cesit]
                          ,'" + Param.Tarih + @"'
                          ,[Kurlar_Kodu]
                          ,[Doviz_Alis]
                          ,[Doviz_Satis]
                          ,[Efektif_Alis]
                          ,[Efektif_Satis]
                      FROM [dbo].[Kurlar] Where Kurlar_Tarih = '" + Param.Tarih.AddDays(-1) + "'");
                }
            }
        }


        private void Anlik()
        {
            try
            {
                DataTable dt = dbtools.SelectTable("exec Pos_Satis_Rapor @Rapor_Tipi=21,@Tarih1='" + Param.Tarih + "',@Tarih2='" + Param.Tarih + "',@Departman=N'" + Departman.Dep_Kodu + "',@Kullanici=N'" + User.P_Kod + "'");
                if (dt.Rows.Count > 1)
                {
                    DataRow[] dr = dt.Select("Tarih = '" + Param.Tarih.ToString("yyyy-MM-dd") + "'");
                    DataTable dts = dr.CopyToDataTable();

                    gdc_Anlik.DataSource = dts;
                }
            }
            catch
            {

            }
        }


        private void Bilgi_Paneli()
        {
            try
            {
                lbl_Departman.Text = Departman.Dep_Adi;
                lbl_Kullanici.Text = User.P_Ad + " " + User.P_Soyad;
                lbl_Kur.Text = Param.Doviz_Kuru.ToString("n4");

                lbl_Tarih.Text = Param.Tarih.ToLongDateString();
                //lbl_Tarih.Text = Param.Tarih.ToLongDateString();

                Main_Tarih.Text = Param.Tarih.ToString("dd.MM.yyyy") + "\n" + Param.Tarih.ToString("dddd");
                //Main_Tarih.Text = Param.Tarih.Date + "\n" + Param.Tarih.Date.ToString("dddd");


                gridColumn1.FieldName = "Kodlar_Ad";
                gridColumn2.FieldName = Departman.MKodlar_P_DovizTuru;

                if (Param.Kurlar_Nerden == 0)
                {
                    string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";

                    gridControl1.DataSource = Fronttools.SelectTable("select Kodlar_Ad," + Departman.MKodlar_P_DovizTuru + " from Kurlar  left join Kodlar  on Kodlar_Kod = Kurlar_Kodu and Kodlar_Sinif = '02'  where Kurlar_Cesit = '" + kur_cesit + "' and Convert(date,Kurlar_Tarih,105) = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "' ");
                }
                else
                {
                    string kur_cesit = Departman.MKodlar_P_DovizCins == "1" ? "E" : "M";

                    gridControl1.DataSource = dbtools.SelectTable("select MKodlar_Ad as Kodlar_Ad," + Departman.MKodlar_P_DovizTuru + " from Kurlar  left join Muh_Kodlar on MKodlar_Kod = Kurlar_Kodu and MKodlar_Sinif = '02'  where Kurlar_Cesit = '" + kur_cesit + "' and Convert(date,Kurlar_Tarih,105)  = '" + Param.Tarih.Date.ToString("yyyy-MM-dd") + "' ");
                }


                btnMasaTakip.Enabled = User.M_Masatakip;
                btnDirekSatis.Enabled = User.D_Direksatis;
                //btnRaporlar.Enabled = User.R_Raporlar;
                btnAyarlar.Enabled = User.A_Ayarlar;
                btnGunSonu.Enabled = User.P_Gunsonu;
                btnKasa.Enabled = User.K_Kasa;
                btnCari.Enabled = User.A_Cari;
                btn_HizliSatis.Enabled = User.H_HizliSatis;
                btn_ExtraTanimlama.Enabled = Departman.Kodlar_AndPos_NFC;
                bar_yemekSepeti.Enabled = Departman.Kodlar_YS_Aktif;
                barButtonItem5.Enabled = Departman.Kodlar_Getir_AP;

                if (Departman.Kodlar_YS_Aktif)
                {
                    YS_RestoInfo.RestorantYukle(Departman.Dep_Kodu, "");
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("text/html"))
                {
                    RHMesaj.MyMessageInformation("İnternet Bağlantınızı Kontrol Ediniz ! " + ex.Message);
                }
                else
                {
                    // Giris_Yap();
                    //  Bilgi_Paneli();
                    //RHMesaj.MyMessageError(MyClass, "Bilgi_Paneli", " Departmanı otomatik seçmeyin !", ex);
                }
            }

        }

        private void Giris_Yap()
        {
            string sonuc = "OK", hata = "";

            //Departman Seçimi
            bool oto_dep = Departman.Dep_OtoSecim();
            if (!oto_dep)
            {
                string filter = "";
                if (User.P_Departman != "")
                {
                    filter = " AND Kodlar_Kod IN ('" + User.P_Departman.Replace(", ", "','") + "')";
                }

                DataTable dtDep = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif ='01' and Kodlar_Anadepo = 'False' and Kodlar_Satis = 'True' " + filter + " order by Kodlar_Id");


                if (dtDep.Rows.Count == 1)
                {
                    Departman.Dep_Kodu = Convert.ToString(dtDep.Rows[0]["Kodlar_Kod"]);
                    sonuc = Departman.Dep_Param_Yukle();
                    hata = "1";
                }
                else
                {
                    Dep_Secim dep = new Dep_Secim();
                    dep.dtDep = dtDep;
                    dep.ShowDialog();
                    sonuc = dep.sonuc;
                    hata = "2";
                }
            }

            if (Departman.Kodlar_GGAktif)
            {
                GunduzGece g = new GunduzGece();
                g.DepKodu = Departman.Dep_Kodu;
                g.ShowDialog();
                sonuc = Departman.Dep_Param_Yukle();
                hata = "3";

                txt_FiyatSekli.Visible = true;
                lbl_FiyatSekli.Visible = true;

                lbl_FiyatSekli.Text = (Departman.Kodlar_GGFiyat == false ? "GÜNDÜZ FİYATI" : "AKŞAM FİYATI");


            }

            //sonuc = Departman.Dep_Param_Yukle();

            //Departman Parametreleri ve Önbüro adresi yüklendi mi?
            if (sonuc == "OK")
            {
                Param.Param_Yukle();
                FisPr.Param_Yukle();
            }
            else
            {
                MessageBox.Show((res_man.GetString("Departman Parametreleri Yüklenemedi...") + hata + "\n" + dbtools.server + "\n" + dbtools.users + "\n" + dbtools.pwd + "\n" + dbtools.database));
            }

            //Tek Departman Kullnılacaksa Dep Değiştir Gizle
            if (Param.Tek_Dep)
            {
                btnDepDegis.Enabled = false;
            }

            if (User.P_Kod == "999")
            {
                btn_Update.Enabled = true;
            }

            //culture = new CultureInfo(User.Pos_Culture);
            //culture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            //culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
            //culture.DateTimeFormat.DateSeparator = ".";
            //culture.DateTimeFormat.ShortTimePattern = "HH:mm";
            //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            //Localize.ApplicationLanguage(culture.TwoLetterISOLanguageName);
        }

        public static MasaTakip masa_takip;
        private void btnMasaTakip_Click(object sender, EventArgs e)
        {
            masa_takip = new MasaTakip();
            masa_takip.ShowDialog();

            if (Param.Param_AnaEkranCiro)
            {
                groupControl4.Visible = true;
                Anlik();
            }
        }

        private void btnAyarlar_Click(object sender, EventArgs e)
        {
            Ayarlar ayar = new Ayarlar();
            ayar.ShowDialog();
        }

        int prmSayac = 0;

        int SiparisSayac = 0;



        private void timer1_Tick(object sender, EventArgs e)
        {
            //m_lblProcTime.Text = DateTime.Now.ToString("HH:mm:ss");
            //m_lblProcDate.Text = DateTime.Now.ToString("dd.MM.yy");

            if (Langs.Default.Dil == "tr-TR")
            {
                //bartxt_Saat.Caption = "Saat : " + DateTime.Now.ToLongTimeString();
                bartxt_Saat.Caption = "Saat : " + DateTime.Now.ToString("HH:mm:ss");
            }
            else
            {
                bartxt_Saat.Caption = "Time : " + DateTime.Now.ToString("HH:mm:ss");
            }

            prmSayac = prmSayac + 1;

            if (prmSayac == 60 && isLogin == true)
            {
                Param.Param_Yukle();
                Departman.Dep_Param_Yukle();
                Bilgi_Paneli();
                prmSayac = 0;
            }

            if (isLogin == true)
            {
                if (xMac == xSubeMac)
                {
                    //SiparisSayac = 0;
                    SiparisSayac++;
                    if (SiparisSayac == 15)
                    {
                        int Siparis = Convert.ToInt32(dbtools.DegerGetir("select ISNULL(Count(*),0) from Pos_CallCenter where ISNULL(Center_Pasif,0) = 0"));
                        //Siparis = 1;
                        if (Siparis > 0)
                        {

                            AlertInfo alertInfo = new AlertInfo("Sipariş Bilgisi", Siparis + " Yeni Siparişiniz Bulunmaktadır.");
                            alertInfo.HotTrackedText = Siparis + " Yeni Siparişiniz Bulunmaktadır.";


                            AlertControl control = new AlertControl();
                            control.FormLocation = AlertFormLocation.BottomRight;
                            control.AllowHotTrack = true;
                            control.Show(null, alertInfo);


                            Beep();
                        }
                        SiparisSayac = 0;

                    }
                }
            }
            //Anlik();
        }

        private void Beep()
        {
            SoundPlayer player = new SoundPlayer();

            //string path = Properties.Resources.tada; // Çalmasini istediginiz ses dosyasinin yolu

            player.Stream = Properties.Resources.Yemeksepeti;

            //player.Stream = Properties.Resources.tada;

            player.Play();

            paketCallCenterAc();

        }

        private string SubeMac()
        {
            string SubeMac = dbtools.DegerGetir("Select Top(1) ISNULL(Pkod_SubeMac,'') as Pkod_SubeMac From Pos_Kodlar Where Pkod_Sinif = '27' and Pkod_SubeMac is not null");

            return SubeMac;
        }

        private void btnDirekSatis_Click(object sender, EventArgs e)
        {
            Satis sat = new Satis();
            sat.Masa_No = User.P_Sabit_Masa;
            sat.Tag = "D";
            sat.ShowDialog();

            if (Param.Param_AnaEkranCiro)
            {
                groupControl4.Visible = true;
                Anlik();
            }
            Main.a.Listele(0);
        }

        private void btnRaporlar_Click(object sender, EventArgs e)
        {
            Rapor_Sec rap = new Rapor_Sec();
            rap.ShowDialog();

            if (Param.Param_AnaEkranCiro)
            {
                groupControl4.Visible = true;
                Anlik();
            }
        }


        private void btnGunSonu_Click(object sender, EventArgs e)
        {
            Gun_Sonu gun = new Gun_Sonu();
            gun.ShowDialog();
        }

        private void btnKasa_Click(object sender, EventArgs e)
        {
            Kasa_Islem kasa = new Kasa_Islem();
            kasa.ShowDialog();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            CariHesap c = new CariHesap();
            c.ShowDialog();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Application.Exit();

            if (Param.Param_CikisKapa == true)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "C:\\Windows\\system32\\shutdown.exe";
                psi.Arguments = "-f -s -t 0";
                Process.Start(psi);
            }

        }

        private void btn_Rez_Click(object sender, EventArgs e)
        {
            Rezervasyon rez = new Rezervasyon();
            rez.ShowDialog();
        }

        private void btn_Fihrist_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Fihrist f = new Fihrist();
            f.ShowDialog();
        }

        private void btn_HizliSatis_Click(object sender, EventArgs e)
        {

            Satis sat = new Satis();
            sat.Tag = "H";
            sat.ShowDialog();

            if (Param.Param_AnaEkranCiro)
            {
                groupControl4.Visible = true;
                Anlik();
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //PaketCallCenter c = new PaketCallCenter();
            //c.ShowDialog();
            paketCallCenterAc();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CallerCallCenter C = new CallerCallCenter();
                C.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void Main_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F2)
            {
                PaketCallCenter p = new PaketCallCenter();
                p.Show();
            }

            if (e.KeyCode == Keys.F3)
            {
                try
                {
                    CallerCallCenter cc = new CallerCallCenter();
                    cc.Tel = "05451112255";
                    cc.Show();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }

            if (e.KeyCode == Keys.F5)
            {
                adminForms admin = new adminForms();
                admin.ShowDialog();
            }

        }

        private void alertControl1_AlertClick(object sender, AlertClickEventArgs e)
        {
            Paket p = new Paket();
            p.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Pos_ExtraFolio p = new Pos_ExtraFolio();
            p.Show();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Pos_SubeMerkez p = new Pos_SubeMerkez();
            p.ShowDialog();
        }


        private void btnDepDegis_Click(object sender, EventArgs e)
        {
            //Giris_Yap();
            // Bilgi_Paneli();
            //reLogin();// 06.05.2023 tarihinde yorum yapıldı
            departmanYukleNew();
        }

        public void reLogin()
        {


            string server = dbtools.server + "!!!"; // buradan çağırdığını anlamak için
            string users = dbtools.users;
            string pwd = dbtools.pwd;
            string database = dbtools.database;

            string direkKullaniciAd = User.P_Kod;
            string direkKullaniciSifre = User.P_Sifre;

            string hepsi = server + " " + users + " " + pwd + " " + database + " " + direkKullaniciAd + " " + direkKullaniciSifre;
            string[] args = hepsi.Split(' ');

            //MessageBox.Show(hepsi);

            Application.Exit();

            var proc = System.Diagnostics.Process.Start(Application.ExecutablePath, hepsi);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Uzak uzak = new Uzak();
            uzak.Show();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                CallerId c = new CallerId();
                c.Show();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //frmLogin login = new frmLogin();
            //login.ShowDialog();
            //if (login.Cikis)
            //{
            //    Application.Exit();
            //}

            //Giris_Yap();

            //Bilgi_Paneli();

            //reLoginTam(); // 06.05.2023 tarihinde yorum yapıldı
            loginOlNew();
            departmanYukleNew();
        }

        public void reLoginTam()
        {
            Application.Exit();
            System.Diagnostics.Process.Start(Application.ExecutablePath);
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            kurAc();
        }

        public void kurAc()
        {
            Kurlar kur = new Kurlar();
            kur.ShowDialog();
        }

        public Ayarlar genelAyarlar = null;
        private void btnAyarlar_Click_1(object sender, EventArgs e)
        {
            genelAyarlar = new Ayarlar();
            genelAyarlar.ShowDialog();
        }

        private void bar_yemekSepeti_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            YS_Panel a = new YS_Panel();
            a.ShowDialog();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Updater u = new Updater();
            u.ShowDialog();
        }

        int SubeSaniye = 0;
        private void timer_Sube2Merkez_Tick(object sender, EventArgs e)
        {
            SubeSaniye++;
            if (SubeSaniye == (Param.Param_SatisTabloGonderi * 60))
            {
                timer_Sube2Merkez.Enabled = false;
                Sube2Merkez a = new Sube2Merkez();
                a.GonderSatis();
                timer_Sube2Merkez.Enabled = true;
                SubeSaniye = 0;
            }
        }


        /*
         POPUPDAN ONCE SİLDİKLERİM
        tg

EntegreMenu trendyolMenu = new EntegreMenu(2);
            trendyolMenu.ShowDialog();



yg

EntegreMenu trendyolMenu = new EntegreMenu(0);
            trendyolMenu.ShowDialog();


get
 Getir_Panel a = new Getir_Panel();
            a.Show();


yemek panel
YS_Panel a = new YS_Panel();
            a.ShowDialog();

         */
        private void barButtonItem5_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Getir_Panel a = new Getir_Panel();
            a.Show();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (Departman.Kodlar_Ingenico)
            {
                if (Departman.Kodlar_IngenicoCon == 0)
                {
                    IngenicoConn a = new IngenicoConn();
                    a.EslestirmeYap(true);
                }
                else
                {
                    //string programDizini = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                    //string pro = programDizini + "\\IngenicoOKC.exe";


                    //Process[] processesByName = Process.GetProcessesByName("IngenicoOKC");
                    //if (processesByName.Length == 0)
                    //{
                    //    Process.Start(@"" + pro);
                    //}

                }


                //if (txtStatus.Text == "OK")
                {

                    try
                    {
                        foreach (Process proc in Process.GetProcessesByName("RmosIngenicoGMP"))
                        {
                            proc.Kill();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }


            }
            //private void simpleButton1_Click_1(object sender, EventArgs e)
            //{
            //    Type[] typelist = GetTypesInNamespace(Assembly.GetExecutingAssembly(), GetThisNamespace());
            //    for (int i = 0; i < typelist.Length; i++)
            //    {
            //        try
            //        {
            //            var form = Activator.CreateInstance(Type.GetType(GetThisNamespace() + "." + typelist[i].Name)) as Form;
            //            ClearLabel(form, form.Name);
            //        }
            //        catch (Exception ex) // form olmayınca yani class olunca catche düşer
            //        {
            //            //Console.WriteLine(ex.Message);
            //        }
            //    }
            //    resx.Close();
            //}
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            YonetimIslemleri a = new YonetimIslemleri();
            a.ShowDialog();
        }



        private void timerGetir_Tick(object sender, EventArgs e)
        {
            getirYenile();
        }



        public PaketCallCenter paketCallCenter = null;
        public void getirYenile()
        {
            string Maxc = Mac();
            if (Departman.Kodlar_Getir_AP && Maxc == Departman.Kodlar_YSMac && RestoGetir())
            {

                List<GetirOrderResponse.Root> getirOrderResponse = getirApi.postOrderPeriodicUnapproved(GetirToken.apitoken);
                if (getirOrderResponse != null && getirOrderResponse.Count > 0)
                {
                    RHMesaj.toastMesajGetir("GETİR YEMEK SİPARİŞİNİZ VAR !\nSiparişi görmek için tıklayınız.");
                    Beep();


                    if (Param.Param_GetirOtomatikOnay && PaketCallCenter.getir_Panel == null)
                    {
                        PaketCallCenter paketCallCenter1 = new PaketCallCenter();
                        paketCallCenter1.SiparisAl(GetirToken.apitoken);

                        DataTable dataTable = paketCallCenter1.GetirListele();
                        foreach (DataRow item in dataTable.Rows)
                        {
                            paketCallCenter1.Kaydet(item["ID"].ToString());
                        }
                        paketCallCenter1.Dispose();

                        if (paketCallCenter != null)
                        {
                            paketCallCenter.getirYenile();
                        }
                    }
                }
            }
            else
            {
                timerGetir.Enabled = false;
            }
        }

        public void paketCallCenterAc()
        {
            if (paketCallCenter == null)
            {
                if (Program.main.toastNotificationsManager2.Notifications[0].Body.Contains("TRENDYOL"))
                {
                    paketCallCenter = new PaketCallCenter(false);
                }
                else // getir
                {
                    paketCallCenter = new PaketCallCenter(true);
                }

                paketCallCenter.ShowDialog();
            }
            else
            {
                paketCallCenter.BringToFront();

                if (Program.main.toastNotificationsManager2.Notifications[0].Body.Contains("TRENDYOL"))
                {
                    paketCallCenter.xtraTabControl1.SelectedTabPage = paketCallCenter.xtraTabPage4;
                }
                else // getir
                {
                    paketCallCenter.xtraTabControl1.SelectedTabPage = paketCallCenter.xtraTabPage3;
                }


            }
        }



        GetirApi getirApi = new GetirApi();
        private bool RestoGetir()
        {
            GetirLoginResponse loginResponse = getirApi.getToken(Departman.Kodlar_Getir_appSecretKey, Departman.Kodlar_Getir_restaurantSecretKey);
            if (loginResponse == null || loginResponse.restaurantId == null)
            {
                return false;
            }

            GetirToken.apitoken = loginResponse.token;
            return true;
        }

        private void alertControl2_AlertClick(object sender, AlertClickEventArgs e)
        {
            try
            {
                paketCallCenterAc();
                (Program.main.alertControl2.AlertFormList[0] as AlertForm).Close();
            }
            catch (Exception ex)
            {

            }

        }

        private void toastNotificationsManager2_Activated(object sender, DevExpress.XtraBars.ToastNotifications.ToastNotificationEventArgs e)
        {
            try
            {
                paketCallCenterAc();
                (Program.main.alertControl2.AlertFormList[0] as AlertForm).Close();
            }
            catch (Exception ex)
            {

            }
        }


        public static AyarlarController ayarlar = new AyarlarController();

        private void Main_Shown(object sender, EventArgs e)
        {
            dbtools.execcmdR("exec defaultParametre");

            StatikModel.wait_loadingKapat();



            string tar = Param.Tarih.ToString("yyyy-MM-dd");
            string tar2 = DateTime.Now.ToString("yyyy-MM-dd");
            if (!tar.Equals(tar2))
            {
                if (User.P_Kod != "1")
                {
                    tar = Convert.ToDateTime(tar).ToLongDateString();
                    tar2 = Convert.ToDateTime(tar2).ToLongDateString();

                    string en = res_man.GetString("Sistem Tarihi ile Pos Tarihi Aynı Değil Kontrol Ediniz!!!");
                    string mesaj = en + System.Environment.NewLine + res_man.GetString("SistemTar:") + tar2 + System.Environment.NewLine + res_man.GetString("PosTar:") + tar;

                    //string mesaj = en + System.Environment.NewLine + res_man.GetString("SistemTar: ") + tar2 + System.Environment.NewLine + res_man.GetString("PosTar: ") + tar;
                    ConfirmationForm confirmationForm = new ConfirmationForm(mesaj);
                    confirmationForm.ShowDialog();
                }

            }


            // receteAc("asdsad","adsasd",(decimal)65.22);
        }

        public void receteAc(string Rec_YS_UrunID, string Rec_Ad, decimal Rec_Fiyat)
        {
            RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);

            Cst_Recete recete = DeepClone<Cst_Recete>(db.Cst_Recete.SqlQuery("select top 1 * from Cst_Recete order by Rec_Genelkod desc").FirstOrDefault());
            recete.Rec_Id = 0;
            recete.Rec_Genelkod = (Convert.ToInt32(recete.Rec_Genelkod) + 1).ToString();

            recete.Rec_Kodu = recete.Rec_Genelkod.Substring(recete.Rec_Genelkod.Length - 3);

            recete.Rec_Ad = "YEMEK SEPETİ-" + Rec_Ad;
            recete.Rec_Fiyat = Rec_Fiyat;

            recete.Rec_Net = (Rec_Fiyat * 100) / (100 + 8);
            recete.Rec_Kdv = Rec_Fiyat - recete.Rec_Net;


            recete.Rec_YS_UrunID = Rec_YS_UrunID;
            db.Cst_Recete.Add(recete);
            db.SaveChanges();

        }

        public T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            EntegreMenu trendyolMenu = new EntegreMenu(2);
            trendyolMenu.ShowDialog();
        }

        private void timerTrendyol_Tick(object sender, EventArgs e)
        {
            trendyolYenile();
        }

        public void trendyolYenile()
        {
            try
            {

                string trendyolDurum = dbtools.DegerGetir("select top 1 trendyolDurum from entegreAyarlar where recDep='" + Departman.Dep_Kodu + "'");

                if (trendyolDurum.ToLower().Equals("true") || trendyolDurum.Equals("1"))
                {

                    //string icindekiler = "'" + RestoranTip.onayBekliyorKod + "','" + RestoranTip.hazirlaniyorKod + "','" + RestoranTip.hazirlandiKod + "','" + RestoranTip.yolaCiktiKod + "'";

                    string icindekiler = "'" + RestoranTip.onayBekliyorKod + "'";

                    string query = "SELECT * FROM entegreSiparis where tip='" + RestoranTip.trendyol + "' and recDep='" + Departman.Dep_Kodu + "' and durumKod in(" + icindekiler + ") ";

                    DataTable dataSiparis = dbtools.SelectTableR(query);

                    if (dataSiparis != null && dataSiparis.Rows.Count > 0)
                    {
                        RHMesaj.toastMesajGetir("TRENDYOL SİPARİŞİNİZ VAR !\nSiparişi görmek için tıklayınız.");
                        Beep();
                    }
                }
                else
                {
                    timerTrendyol.Enabled = false;
                    barButtonItem6.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "trendyolYenile", "", ex);
            }
        }

        private void btnYemekSepetiGo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            EntegreMenu trendyolMenu = new EntegreMenu(0);
            trendyolMenu.ShowDialog();

        }

        private void barStaticItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string yaziciAd = dbtools.DegerGetir("Select isnull(Kodlar_parakasa,'') as Kodlar_parakasa From Stok_Kodlar Where Kodlar_Sinif ='01' and Kodlar_Kod='" + Departman.Dep_Kodu + "'");

                if (yaziciAd=="")
                {
                    string mesaj = @"Lütfen Pos Departmandan Yazıcı Seçiniz
Yazıcı Özellikleri 
Cash Drawer #1 Before Printing 
ve
No Cut Seçili Olsun
İki Yazıcı İsmi Aynı İp Girebilirsiniz";
                    MessageBox.Show(mesaj);
                    return;
                }
                XtraReport1 report1 = new XtraReport1();
                report1.PrinterName = yaziciAd;
                report1.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

       
    }
}