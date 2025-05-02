using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using Pos.Print;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Pos.Class
{
    public class FisPr
    {
        public static string Siparis_Font { get; set; }
        public static int Siparis_Ciktisayisi { get; set; }

        public static string Hesap_Font { get; set; }
        public static int Hesap_Ciktisayisi { get; set; }

        public static string Iptal_Font { get; set; }
        public static int Iptal_Ciktisayisi { get; set; }

        public static string Mars_Font { get; set; }
        public static int Mars_Ciktisayisi { get; set; }

        public static string Paket_Font { get; set; }
        public static int Paket_Ciktisayisi { get; set; }

        public static string Kasa_Font { get; set; }
        public static int Kasa_Ciktisayisi { get; set; }

        public static string Cari_Font { get; set; }

        List<string> Liste;

        private Font printFont;

        float yPos = 0;
        int count = 0;
        float leftMargin = 1;
        float topMargin = 1;


        public bool kisiyeSatis = false;
        public string kisiyeSatisAdSoyad = "";
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {

            yPos = 0;
            count = 0;
            leftMargin = 1;
            topMargin = 1;

            for (int i = 0; i < Liste.Count; i++)
            {
                Font mFont = printFont;

                //bool hicbiri = false;

                //if (Liste[i].StartsWith("TOPLAM"))
                //{
                //    mFont = new Font(printFont.FontFamily, printFont.Size + 2, FontStyle.Bold);
                //    hicbiri = true;
                //}

                //if (Liste[i].StartsWith("  *"))
                //{
                //    mFont = new Font(printFont.FontFamily, printFont.Size + 2, FontStyle.Bold);
                //    hicbiri = true;
                //}

                //if (Liste[i].StartsWith("#"))
                //{
                //    Liste[i] = Liste[i].Replace("#", "");
                //    mFont = new Font(printFont.FontFamily, printFont.Size + 2, FontStyle.Bold);
                //    hicbiri = true;
                //}

                //if (Liste[i].StartsWith("-#"))
                //{
                //    Liste[i] = Liste[i].Replace("-#", "");
                //    mFont = new Font(printFont.FontFamily, printFont.Size + 4, FontStyle.Bold);
                //    hicbiri = true;
                //}

                //if (hicbiri==false)
                //{
                //    mFont = new Font(printFont.FontFamily, 8, FontStyle.Bold);
                //}



                mFont = new Font(printFont.FontFamily, 8, FontStyle.Bold);

                if (!Param.Param_YeniHesapDkm)
                {
                    mFont = printFont;
                }

                yPos = topMargin + (count *
                  printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(Liste[i], mFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;

            }

            //System.Drawing.Image img = System.Drawing.Image.FromFile("C:\\Foto\\Foto.jpg");
            //ev.Graphics.DrawImage(img, leftMargin, yPos);
        }

        public static void Param_Yukle()
        {
            try
            {
                DataTable dtHesap = dbtools.SelectTable("select Pkod_Kod,Pkod_Font,isnull(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar with(nolock) where Pkod_Sinif = '17' and Pkod_Kod = 'HESAP' ");
                if (dtHesap.Rows.Count > 0)
                {
                    Hesap_Font = Convert.ToString(dtHesap.Rows[0]["Pkod_Font"]);
                    Hesap_Ciktisayisi = Convert.ToInt32(dtHesap.Rows[0]["Pkod_Ciktisayisi"]);
                }

                if (Hesap_Ciktisayisi == 0)
                {
                    RHMesaj.MyMessageInformation("Hesap_Ciktisayisi=0\nFiş font ayarlarından bir kere kaydet tuşuna basın !!");
                }

                DataTable dtIptal = dbtools.SelectTable("select Pkod_Kod, Pkod_Font,isnull(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar with(nolock) where Pkod_Sinif = '17' and Pkod_Kod = 'IPTAL' ");
                if (dtIptal.Rows.Count > 0)
                {
                    Iptal_Font = Convert.ToString(dtIptal.Rows[0]["Pkod_Font"]);
                    Iptal_Ciktisayisi = Convert.ToInt32(dtIptal.Rows[0]["Pkod_Ciktisayisi"]);
                }


                DataTable dtSiparis = dbtools.SelectTable("select Pkod_Kod, Pkod_Font,isnull(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar with(nolock) where Pkod_Sinif = '17' and Pkod_Kod = 'SIPARIS' ");
                if (dtSiparis.Rows.Count > 0)
                {
                    Siparis_Font = Convert.ToString(dtSiparis.Rows[0]["Pkod_Font"]);
                    Siparis_Ciktisayisi = Convert.ToInt32(dtSiparis.Rows[0]["Pkod_Ciktisayisi"]);
                }

                DataTable dtMars = dbtools.SelectTable("select Pkod_Kod, Pkod_Font,isnull(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar with(nolock) where Pkod_Sinif = '17' and Pkod_Kod = 'MARS' ");
                if (dtMars.Rows.Count > 0)
                {
                    Mars_Font = Convert.ToString(dtMars.Rows[0]["Pkod_Font"]);
                    Mars_Ciktisayisi = Convert.ToInt32(dtMars.Rows[0]["Pkod_Ciktisayisi"]);
                }

                DataTable dtPaket = dbtools.SelectTable("select Pkod_Kod, Pkod_Font,isnull(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar with(nolock) where Pkod_Sinif = '17' and Pkod_Kod = 'PAKET' ");
                if (dtPaket.Rows.Count > 0)
                {
                    Paket_Font = Convert.ToString(dtPaket.Rows[0]["Pkod_Font"]);
                    Paket_Ciktisayisi = Convert.ToInt32(dtPaket.Rows[0]["Pkod_Ciktisayisi"]);
                }

                DataTable dtKasa = dbtools.SelectTable("select Pkod_Kod, Pkod_Font,isnull(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar with(nolock) where Pkod_Sinif = '17' and Pkod_Kod = 'KASAMAKBUZ' ");
                if (dtKasa.Rows.Count > 0)
                {
                    Kasa_Font = Convert.ToString(dtKasa.Rows[0]["Pkod_Font"]);
                    Kasa_Ciktisayisi = Convert.ToInt32(dtKasa.Rows[0]["Pkod_Ciktisayisi"]);
                }

                DataTable dtCari = dbtools.SelectTable("select Pkod_Kod, Pkod_Font,isnull(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar with(nolock) where Pkod_Sinif = '17' and Pkod_Kod = 'CARIHESAP' ");
                if (dtCari.Rows.Count > 0)
                {
                    Cari_Font = Convert.ToString(dtCari.Rows[0]["Pkod_Font"]);
                }
            }
            catch (Exception ex)
            {

            }

        }
        public string SiparisPr(int Fisno, bool Mars, int Split, string abuyerBaslik = "   * * * ABUYER FISI * * *   ", string kartDetay1 = "", string kartdetay2 = "", bool hizliSatis = false, string garsonsor = "")
        {
            List<string> siparis = new List<string>();
            DataTable dtPrinter = SiparisPrinterBul(Fisno, Split, false);

            decimal bakiye = 0;

            bool isMac_Printer = false;
            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                {
                    isMac_Printer = true;
                }
            }

            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtPrinter.Rows[i]["Printer"]);
                int bosSatir = Convert.ToInt32(dtPrinter.Rows[i]["Pkod_Satir"]);

                if (printer == "" && Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) == "") continue;

                if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                {
                    printer = Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]);
                }

                DataTable dtSiparis = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 1);
                com.Parameters.AddWithValue("@Printer", printer);
                com.Parameters.AddWithValue("@Mars", Mars);
                com.Parameters.AddWithValue("@Split", Split);
                com.Parameters.AddWithValue("@MacAdres", isMac_Printer ? dbtools.MacAdresi() : "");
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtSiparis);

                if (dtSiparis.Rows.Count > 0)
                {



                    if (Param.Param_SiparisAna)
                    {
                        DataView dv = dtSiparis.DefaultView;
                        dv.Sort = "AnaGrupAdi desc";
                        dtSiparis = dv.ToTable();
                    }

                    if (con.State != ConnectionState.Closed) con.Close();

                    if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtSiparis.Rows[0]["Pkod_Printer"]);

                    string paketAciklama = "";
                    string masaNo = Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"]);
                    DataTable dt = dbtools.SelectTable("select ISNULL(Masa_Paket,0) as Masa_Paket from Pos_Masa where Masa_No = '" + masaNo + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(dt.Rows[0]["Masa_Paket"]))
                        {
                            paketAciklama = " (PAKET) ";
                        }
                    }


                    TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    for (int k = 0; k < Siparis_Ciktisayisi; k++)
                    {
                        //siparis.Add("Printer : " + printer);
                        //siparis.Add("Ek-1 : " + Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr1"]));
                        //siparis.Add("Ek-2 : " + Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr2"]));
                        //siparis.Add("Ek-3 : " + Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr3"]));



                        if (!hizliSatis)
                        {
                            if (Param.Param_SiparisFisFont)
                            {
                                siparis.Add(".");
                                siparis.Add("-# * * * FIS NO : " + Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]) + " * * *");
                            }
                            siparis.Add(".");
                            siparis.Add("   * * * SIPARIS FISI * * *  ");
                            siparis.Add(".");

                            siparis.Add("#Masa :" + (Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"])).PadRight(7, ' ') + "  #Kisi :" + Convert.ToString(dtSiparis.Rows[0]["Rsat_Kisi"]));
                            siparis.Add(".");
                            siparis.Add("#Konumu :" + (Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                            siparis.Add(".");
                            if (!string.IsNullOrEmpty(paketAciklama)) siparis.Add(paketAciklama);
                            siparis.Add(".");
                            siparis.Add("#Departman : " + Departman.Dep_Adi);
                            siparis.Add(".");
                            siparis.Add("Tarih:" + Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                            siparis.Add("Cekno:" + Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                            siparis.Add(".");


                            if (garsonsor.Equals(""))
                            {
                                siparis.Add("Grson:" + Convert.ToString(dtSiparis.Rows[0]["Garson"]));//.PadRight(10, ' '));// + " Grsn2:" + Convert.ToString(dtSiparis.Rows[0]["Garson2"]).PadRight(5, ' '));
                            }
                            else
                            {
                                siparis.Add("Grson:" + garsonsor);
                            }




                            siparis.Add(".");
                            siparis.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                        }
                        else
                        {

                            siparis.Add(".");
                            siparis.Add("   * * * SIPARIS FISI * * *  ");
                            siparis.Add(".");
                            if (!string.IsNullOrEmpty(paketAciklama)) siparis.Add(paketAciklama);
                            siparis.Add("#Konumu :" + (Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                            siparis.Add(".");
                            siparis.Add("Departman : " + Departman.Dep_Adi);
                            siparis.Add("Tarih:" + Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                            siparis.Add("#Cekno:" + Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                            siparis.Add(".");
                            siparis.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));

                        }
                        string YeniAnaGrup = string.Empty;
                        string EskiAnaGrup = string.Empty;
                        for (int j = 0; j < dtSiparis.Rows.Count; j++)
                        {
                            if (Param.Param_SiparisAna == false)
                            {
                                siparis.Add("".PadLeft(36, '-'));

                                siparis.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtSiparis.Rows[j]["Rsat_Miktar"])).PadRight(4, ' ') + " " + Convert.ToString(dtSiparis.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtSiparis.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));

                                if (Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]) != String.Empty)
                                    siparis.Add(Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]));
                            }
                            else
                            {
                                YeniAnaGrup = Convert.ToString(dtSiparis.Rows[j]["AnaGrupAdi"]);

                                if (YeniAnaGrup != EskiAnaGrup)
                                {
                                    siparis.Add("".PadLeft(36, '-'));

                                    siparis.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtSiparis.Rows[j]["Rsat_Miktar"])).PadRight(4, ' ') + " " + Convert.ToString(dtSiparis.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtSiparis.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));

                                    if (Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]) != String.Empty)
                                        siparis.Add(Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]));

                                    EskiAnaGrup = YeniAnaGrup;
                                }
                                else
                                {
                                    siparis.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtSiparis.Rows[j]["Rsat_Miktar"])).PadRight(4, ' ') + " " + Convert.ToString(dtSiparis.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtSiparis.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));

                                    if (Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]) != String.Empty) siparis.Add(Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]));

                                    EskiAnaGrup = YeniAnaGrup;
                                }
                            }



                            if (Param.Param_SiparisTutar)
                            {
                                bakiye += Convert.ToDecimal(dtSiparis.Rows[j]["Rsat_Tutar"]);

                            }
                        }


                        siparis.Add("".PadLeft(36, '-'));

                        if (Param.Param_SiparisTutar)
                        {
                            siparis.Add("------------------------");
                            siparis.Add(" TOPLAM : " + bakiye.ToString("n2"));
                            siparis.Add("------------------------");
                        }


                        for (int j = 0; j < bosSatir; j++)
                        {
                            siparis.Add(".");
                        }

                        //siparis.Add(Convert.ToString((char) 27) + "@" + Convert.ToString((char) 29) + "V" + (char)1);

                        try
                        {
                            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                            Font fnt;

                            if (Param.Param_HspFontAlgilama == true)
                            {
                                fnt = null;
                                if (fnt == null)
                                {
                                    fnt = new Font("Arial", 8);
                                }
                            }
                            else
                            {
                                fnt = (Font)converter.ConvertFromString(Siparis_Font);
                            }

                            printFont = fnt;

                            Liste = siparis;

                            PrintDocument pd = new PrintDocument();
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.PrinterSettings.PrinterName = printer;
                            pd.Print();

                            if (Param.Param_Printer_Tanim && Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr1"]) != "")
                            {
                                PrintDocument pd1 = new PrintDocument();
                                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                                pd.PrinterSettings.PrinterName = Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr1"]);
                                pd.Print();
                            }
                            if (Param.Param_Printer_Tanim && Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr2"]) != "")
                            {
                                PrintDocument pd2 = new PrintDocument();
                                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                                pd.PrinterSettings.PrinterName = Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr2"]);
                                pd.Print();
                            }
                            if (Param.Param_Printer_Tanim && Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr3"]) != "")
                            {
                                PrintDocument pd3 = new PrintDocument();
                                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                                pd.PrinterSettings.PrinterName = Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr3"]);
                                pd.Print();
                            }

                            siparis.Clear();
                        }
                        catch (Exception err)
                        {
                            return err.Message;
                        }
                    }
                }

                //AbuyerPr2(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
                //AbuyerPr3(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
                //AbuyerPr4(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            }

            AbuyerPr(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            return "OK";
        }



        public string newSiparisPr(int Fisno, bool Mars, int Split, string abuyerBaslik = "   * * * ABUYER FISI * * *   ", string kartDetay1 = "", string kartdetay2 = "", bool hizliSatis = false, string garsonsor = "", string fisBaslik = "", string kisiyeSatis = "", bool tumsiparisiTekrarGonder = false,bool direkSatis=false)
        {

            try
            {
                if (tumsiparisiTekrarGonder)
                {
                    dbtools.execcmdR("update Cst_Recete_Satis set Rsat_SiparisPr=0 where Rsat_Fisno='" + Fisno + "' ");
                }

                string sirano = StatikSinif.getSira(Fisno.ToString());


                //List<string> siparis = new List<string>();
                DataTable dtPrinter = SiparisPrinterBul(Fisno, Split, false);

                //decimal bakiye = 0;

                bool isMac_Printer = false;
                for (int i = 0; i < dtPrinter.Rows.Count; i++)
                {
                    if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                    {
                        isMac_Printer = true;
                        break;
                    }
                }

                DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'SIPARISFISI'");
                if (dtDizayn.Rows.Count < 1)
                {
                    return "Sipariş Dizaynı Yapılmamış...";
                }


                for (int i = 0; i < dtPrinter.Rows.Count; i++)
                {
                    string printer = Convert.ToString(dtPrinter.Rows[i]["Printer"]);
                    int bosSatir = Convert.ToInt32(dtPrinter.Rows[i]["Pkod_Satir"]);

                    if (printer == "" && Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) == "") continue;


                    // !! yeni siparis printer çıkarmıyorsa açtık. ERAMAX İÇİN YOKSA KALDRI YORUM SATIRINDAN
                    if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                    {
                        printer = Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]);
                    }


                    int raportip = 1;

                    if (direkSatis)
                    {
                        raportip = 32;
                    }

                    DataTable dtSiparis = new DataTable();
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis";
                    com.Parameters.AddWithValue("@Fisno", Fisno);
                    com.Parameters.AddWithValue("@Rapor_Tipi", raportip);
                    com.Parameters.AddWithValue("@Printer", printer);
                    com.Parameters.AddWithValue("@Mars", Mars);
                    com.Parameters.AddWithValue("@Split", Split);
                    com.Parameters.AddWithValue("@MacAdres", isMac_Printer ? dbtools.MacAdresi() : "");
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(dtSiparis);



                    if (dtSiparis.Rows.Count > 0)
                    {
                        foreach (DataRow boslaridoldur in dtSiparis.Rows)
                        {
                            string yaziciismi = boslaridoldur["urunbazliprinter"].ToString();
                            if (yaziciismi == "")
                            {
                                boslaridoldur["urunbazliprinter"] = printer;
                            }
                        }


                        var yazicilar = dtSiparis.AsEnumerable()
              .GroupBy(r => new { Col1 = r["urunbazliprinter"] })
              .Select(g => g.OrderBy(r => r["urunbazliprinter"]).First())
              .CopyToDataTable();



                        foreach (DataRow itemYazici in yazicilar.Rows) // 2 kere doncek
                        {
                            string yaziciismi = itemYazici["urunbazliprinter"].ToString();



                            var yazilacaklar = dtSiparis.Select("urunbazliprinter='" + yaziciismi + "'").CopyToDataTable();


                            foreach (DataRow item in dtSiparis.Rows)
                            {
                                item["Rsat_Miktar"] = item["Rsat_Miktar"].ToString().Replace(",0000", "").Replace(",000", "").Replace(",00", "");
                            }


                            Print.Siparis siparis = new Print.Siparis();
                            xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), siparis);
                            siparis.PrinterName = yaziciismi == "" ? printer : yaziciismi;
                            siparis.DataSource = yazilacaklar;//dtSiparis;

                            if (Param.Param_SiparisAna)
                            {
                                DataView dv = dtSiparis.DefaultView;
                                dv.Sort = "AnaGrupAdi desc";
                                dtSiparis = dv.ToTable();
                            }

                            if (con.State != ConnectionState.Closed) con.Close();

                            if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtSiparis.Rows[0]["Pkod_Printer"]);


                            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


                            siparis.xr_MasaNo.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"]);

                            if (kisiyeSatis != "")
                            {
                                siparis.xr_MasaNo.Text = siparis.xr_MasaNo.Text + "[" + kisiyeSatis + "]";
                            }

                            siparis.xr_Konum.Text = Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"]);
                            siparis.xr_KisiSayisi.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Kisi"]);
                            siparis.xr_Tarih.Text = Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToShortDateString();
                            siparis.xr_Acilis.Text = Convert.ToString(timeSpan);
                            siparis.txtDepartman.Text = Departman.Dep_Adi;
                            siparis.txtSiraNo.Text = sirano;

                            if (garsonsor.Equals(""))
                            {
                                siparis.xr_Garson.Text = Convert.ToString(dtSiparis.Rows[0]["Garson"]);
                            }
                            else
                            {
                                siparis.xr_Garson.Text = garsonsor;
                            }

                            siparis.xr_Cek.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]);

                            siparis.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
                            siparis.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));

                            if (fisBaslik != "")
                            {
                                siparis.xr_Baslik.Text = fisBaslik;
                            }

                            if (siparis.PrinterName != "Microsoft Print to PDF" && siparis.PrinterName != "") // 
                            {
                                siparis.Print();


                            }

                        }
                    }

                }

                AbuyerPr(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            }
            catch (Exception ex)
            {

                return "HATA Yazdırılamadı !\n" + ex.Message;
            }

            return "OK";
        }

        public string newSiparisPr_Tumsiparis(int Fisno, bool Mars, int Split, string abuyerBaslik = "   * * * ABUYER FISI * * *   ", string kartDetay1 = "", string kartdetay2 = "", bool hizliSatis = false, string garsonsor = "", string fisBaslik = "", string kisiyeSatis = "", bool tumsiparisiTekrarGonder = false)
        {

            try
            {
                if (tumsiparisiTekrarGonder)
                {
                    dbtools.execcmdR("update Cst_Recete_Satis set Rsat_SiparisPr=0 where Rsat_Fisno='" + Fisno + "' ");
                }

                string sirano = StatikSinif.getSira(Fisno.ToString());


                //List<string> siparis = new List<string>();
                DataTable dtPrinter = SiparisPrinterBul(Fisno, Split, false);

                //decimal bakiye = 0;

                bool isMac_Printer = false;
                for (int i = 0; i < dtPrinter.Rows.Count; i++)
                {
                    if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                    {
                        isMac_Printer = true;
                        break;
                    }
                }

                DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'SIPARISFISI'");
                if (dtDizayn.Rows.Count < 1)
                {
                    return "Sipariş Dizaynı Yapılmamış...";
                }

                //tümsiparis 1 olan tüm siparişlerin listesini getirdim
                DataTable dtSiparis2 = new DataTable();
                SqlConnection con2 = dbtools.conn;
                if (con2.State == ConnectionState.Closed) con2.Open();
                SqlCommand com2 = new SqlCommand();
                com2.Connection = con2;
                com2.CommandType = CommandType.StoredProcedure;
                com2.CommandTimeout = 0;
                com2.CommandText = "Pos_Satis";
                com2.Parameters.AddWithValue("@Fisno", Fisno);
                com2.Parameters.AddWithValue("@Rapor_Tipi", 32);
                com2.Parameters.AddWithValue("@Printer", "");
                com2.Parameters.AddWithValue("@Mars", Mars);
                com2.Parameters.AddWithValue("@Split", Split);
                com2.Parameters.AddWithValue("@MacAdres", isMac_Printer ? dbtools.MacAdresi() : "");
                SqlDataAdapter da2 = new SqlDataAdapter(com2);
                da2.Fill(dtSiparis2);


                for (int i = 0; i < dtPrinter.Rows.Count; i++)
                {
                    string printer = Convert.ToString(dtPrinter.Rows[i]["Printer"]);
                    int bosSatir = Convert.ToInt32(dtPrinter.Rows[i]["Pkod_Satir"]);

                    if (printer == "" && Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) == "") continue;


                    // !! yeni siparis printer çıkarmıyorsa açtık. ERAMAX İÇİN YOKSA KALDRI YORUM SATIRINDAN
                    if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                    {
                        printer = Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]);
                    }


                    DataTable dtSiparis = new DataTable();
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis";
                    com.Parameters.AddWithValue("@Fisno", Fisno);
                    com.Parameters.AddWithValue("@Rapor_Tipi", 1);
                    com.Parameters.AddWithValue("@Printer", printer);
                    com.Parameters.AddWithValue("@Mars", Mars);
                    com.Parameters.AddWithValue("@Split", Split);
                    com.Parameters.AddWithValue("@MacAdres", isMac_Printer ? dbtools.MacAdresi() : "");
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(dtSiparis);


                    DataTable resultTable = dtSiparis.Clone();


                    // DataTable'ların birleştirilmesi
                    var query = from t1 in dtSiparis.AsEnumerable()
                                select resultTable.LoadDataRow(
                                    t1.ItemArray, // İlk tablonun verilerini ekle
                                    false);
                    query.CopyToDataTable(resultTable, LoadOption.PreserveChanges);

                    foreach (DataRow item in dtSiparis.Rows)
                    {
                        int rsatid = Convert.ToInt32(item["Rsat_Id"].ToString());
                        bool cik = false;
                        foreach (DataRow item2 in dtSiparis2.Rows)
                        {
                            int rsatid2 = Convert.ToInt32(item2["Rsat_Id"].ToString());
                            if (rsatid == rsatid2)
                            {
                                var query2 = from t2 in dtSiparis2.AsEnumerable()
                                             select resultTable.LoadDataRow(
                                                 t2.ItemArray, // İkinci tablonun eksik verilerini ekle
                                                 false);
                                query2.CopyToDataTable(resultTable, LoadOption.PreserveChanges);
                                goto etiket2;
                            }

                        }
                    }

                etiket2:


                    var distinctRows = resultTable.AsEnumerable()
    .GroupBy(row => row.Field<int>("Rsat_Id"))
    .Select(group => group.First()) // Aynı Rsat_Id'ye sahip olan ilk satırı alıyoruz
    .CopyToDataTable();
                    dtSiparis = distinctRows;
                    if (dtSiparis.Rows.Count > 0)
                    {

                        string yaziciismi = printer.ToString();




                        foreach (DataRow item in dtSiparis.Rows)
                        {
                            item["Rsat_Miktar"] = item["Rsat_Miktar"].ToString().Replace(",0000", "").Replace(",000", "").Replace(",00", "");
                        }


                        Print.Siparis siparis = new Print.Siparis();
                        xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), siparis);
                        siparis.PrinterName = yaziciismi == "" ? printer : yaziciismi;
                        siparis.DataSource = dtSiparis;//dtSiparis;

                        if (Param.Param_SiparisAna)
                        {
                            DataView dv = dtSiparis.DefaultView;
                            dv.Sort = "AnaGrupAdi desc";
                            dtSiparis = dv.ToTable();
                        }

                        if (con.State != ConnectionState.Closed) con.Close();

                        if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtSiparis.Rows[0]["Pkod_Printer"]);


                        TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


                        siparis.xr_MasaNo.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"]);

                        if (kisiyeSatis != "")
                        {
                            siparis.xr_MasaNo.Text = siparis.xr_MasaNo.Text + "[" + kisiyeSatis + "]";
                        }

                        siparis.xr_Konum.Text = Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"]);
                        siparis.xr_KisiSayisi.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Kisi"]);
                        siparis.xr_Tarih.Text = Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToShortDateString();
                        siparis.xr_Acilis.Text = Convert.ToString(timeSpan);
                        siparis.txtDepartman.Text = Departman.Dep_Adi;
                        siparis.txtSiraNo.Text = sirano;

                        if (garsonsor.Equals(""))
                        {
                            siparis.xr_Garson.Text = Convert.ToString(dtSiparis.Rows[0]["Garson"]);
                        }
                        else
                        {
                            siparis.xr_Garson.Text = garsonsor;
                        }

                        siparis.xr_Cek.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]);

                        siparis.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
                        siparis.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));

                        if (fisBaslik != "")
                        {
                            siparis.xr_Baslik.Text = fisBaslik;
                        }

                        if (siparis.PrinterName != "Microsoft Print to PDF" && siparis.PrinterName != "") // 
                        {
                            siparis.Print();
                        }


                    }

                }

                AbuyerPr(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            }
            catch (Exception ex)
            {

                return "HATA Yazdırılamadı !\n" + ex.Message;
            }

            return "OK";
        }
        public string newSiparisPrF2(int Fisno, bool Mars, int Split, string abuyerBaslik = "   * * * ABUYER FISI * * *   ", string kartDetay1 = "", string kartdetay2 = "", bool hizliSatis = false, string garsonsor = "", string fisBaslik = "", string kisiyeSatis = "", bool tumsiparisiTekrarGonder = false)
        {

            try
            {
                int raporTip = 1;
                string tekrarSiparisPrintName = "";
                if (tumsiparisiTekrarGonder)
                {
                    dbtools.execcmdR("update Cst_Recete_Satis set Rsat_SiparisPr=0 where Rsat_Fisno='" + Fisno + "' ");
                    raporTip = 31;
                    tekrarSiparisPrintName = dbtools.DegerGetir("select isnull(siparisTekrarPrintName,'') as siparisTekrarPrintName from Pos_Param");
                }

                string sirano = StatikSinif.getSira(Fisno.ToString());


                //List<string> siparis = new List<string>();
                DataTable dtPrinter = SiparisPrinterBul(Fisno, Split, false);

                //decimal bakiye = 0;

                bool isMac_Printer = false;
                for (int i = 0; i < dtPrinter.Rows.Count; i++)
                {
                    if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                    {
                        isMac_Printer = true;
                        break;
                    }
                }

                DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'SIPARISFISI'");
                if (dtDizayn.Rows.Count < 1)
                {
                    return "Sipariş Dizaynı Yapılmamış...";
                }

                if (tumsiparisiTekrarGonder == true && tekrarSiparisPrintName != "")
                {
                    dtPrinter.Rows[0]["Printer"] = tekrarSiparisPrintName;
                }

                for (int i = 0; i < dtPrinter.Rows.Count; i++)
                {
                    string printer = Convert.ToString(dtPrinter.Rows[i]["Printer"]);
                    int bosSatir = Convert.ToInt32(dtPrinter.Rows[i]["Pkod_Satir"]);

                    if (printer == "" && Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) == "") continue;


                    // !! yeni siparis printer çıkarmıyorsa açtık. ERAMAX İÇİN YOKSA KALDRI YORUM SATIRINDAN
                    if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                    {
                        printer = Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]);
                    }


                    DataTable dtSiparis = new DataTable();
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis";
                    com.Parameters.AddWithValue("@Fisno", Fisno);
                    com.Parameters.AddWithValue("@Rapor_Tipi", raporTip);
                    com.Parameters.AddWithValue("@Printer", printer);
                    com.Parameters.AddWithValue("@Mars", Mars);
                    com.Parameters.AddWithValue("@Split", Split);
                    com.Parameters.AddWithValue("@MacAdres", isMac_Printer ? dbtools.MacAdresi() : "");
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(dtSiparis);



                    if (dtSiparis.Rows.Count > 0)
                    {




                        foreach (DataRow item in dtSiparis.Rows)
                        {
                            item["Rsat_Miktar"] = item["Rsat_Miktar"].ToString().Replace(",0000", "").Replace(",000", "").Replace(",00", "");
                        }


                        Print.Siparis siparis = new Print.Siparis();
                        xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), siparis);
                        siparis.PrinterName = tekrarSiparisPrintName;
                        siparis.DataSource = dtSiparis;//dtSiparis;

                        if (Param.Param_SiparisAna)
                        {
                            DataView dv = dtSiparis.DefaultView;
                            dv.Sort = "AnaGrupAdi desc";
                            dtSiparis = dv.ToTable();
                        }

                        if (con.State != ConnectionState.Closed) con.Close();

                        if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtSiparis.Rows[0]["Pkod_Printer"]);


                        TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


                        siparis.xr_MasaNo.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"]);

                        if (kisiyeSatis != "")
                        {
                            siparis.xr_MasaNo.Text = siparis.xr_MasaNo.Text + "[" + kisiyeSatis + "]";
                        }

                        siparis.xr_Konum.Text = Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"]);
                        siparis.xr_KisiSayisi.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Kisi"]);
                        siparis.xr_Tarih.Text = Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToShortDateString();
                        siparis.xr_Acilis.Text = Convert.ToString(timeSpan);
                        siparis.txtDepartman.Text = Departman.Dep_Adi;
                        siparis.txtSiraNo.Text = sirano;

                        if (garsonsor.Equals(""))
                        {
                            siparis.xr_Garson.Text = Convert.ToString(dtSiparis.Rows[0]["Garson"]);
                        }
                        else
                        {
                            siparis.xr_Garson.Text = garsonsor;
                        }

                        siparis.xr_Cek.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]);

                        siparis.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
                        siparis.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));

                        if (fisBaslik != "")
                        {
                            siparis.xr_Baslik.Text = fisBaslik;
                        }

                        if (siparis.PrinterName != "Microsoft Print to PDF" && siparis.PrinterName != "") // 
                        {
                            siparis.Print();
                        }

                    }

                    if (tumsiparisiTekrarGonder)
                    {
                        break;
                    }
                }



                AbuyerPr(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);

            }
            catch (Exception ex)
            {

                return "HATA Yazdırılamadı !\n" + ex.Message;
            }



            return "OK";
        }

        public string newSiparisPrYedek(int Fisno, bool Mars, int Split, string abuyerBaslik = "   * * * ABUYER FISI * * *   ", string kartDetay1 = "", string kartdetay2 = "", bool hizliSatis = false, string garsonsor = "")
        {
            //List<string> siparis = new List<string>();
            DataTable dtPrinter = SiparisPrinterBul(Fisno, Split, false);

            //decimal bakiye = 0;

            bool isMac_Printer = false;
            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                {
                    isMac_Printer = true;
                    break;
                }
            }

            DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'SIPARISFISI'");
            if (dtDizayn.Rows.Count < 1)
            {
                return "Sipariş Dizaynı Yapılmamış...";
            }

            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtPrinter.Rows[i]["Printer"]);
                int bosSatir = Convert.ToInt32(dtPrinter.Rows[i]["Pkod_Satir"]);

                if (printer == "" && Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) == "") continue;


                // !! yeni siparis printer çıkarmıyorsa açtık. ERAMAX İÇİN YOKSA KALDRI YORUM SATIRINDAN
                if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                {
                    printer = Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]);
                }


                DataTable dtSiparis = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 1);
                com.Parameters.AddWithValue("@Printer", printer);
                com.Parameters.AddWithValue("@Mars", Mars);
                com.Parameters.AddWithValue("@Split", Split);
                com.Parameters.AddWithValue("@MacAdres", isMac_Printer ? dbtools.MacAdresi() : "");
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtSiparis);

                if (dtSiparis.Rows.Count > 0)
                {

                    foreach (DataRow item in dtSiparis.Rows)
                    {
                        item["Rsat_Miktar"] = item["Rsat_Miktar"].ToString().Replace(",0000", "").Replace(",000", "").Replace(",00", "");
                    }


                    Print.Siparis siparis = new Print.Siparis();
                    xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), siparis);
                    siparis.PrinterName = printer;
                    siparis.DataSource = dtSiparis;

                    if (Param.Param_SiparisAna)
                    {
                        DataView dv = dtSiparis.DefaultView;
                        dv.Sort = "AnaGrupAdi desc";
                        dtSiparis = dv.ToTable();
                    }

                    if (con.State != ConnectionState.Closed) con.Close();

                    if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtSiparis.Rows[0]["Pkod_Printer"]);

                    //string paketAciklama = "";
                    //string masaNo = Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"]);
                    //DataTable dt = dbtools.SelectTable("select ISNULL(Masa_Paket,0) as Masa_Paket from Pos_Masa where Masa_No = '" + masaNo + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                    //if (dt.Rows.Count > 0)
                    //{
                    //    if (Convert.ToBoolean(dt.Rows[0]["Masa_Paket"]))
                    //    {
                    //        paketAciklama = " (PAKET) ";
                    //    }
                    //}


                    TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                    //for (int k = 0; k < Siparis_Ciktisayisi; k++)
                    //{
                    siparis.xr_MasaNo.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"]);
                    siparis.xr_Konum.Text = Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"]);
                    siparis.xr_KisiSayisi.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Kisi"]);
                    siparis.xr_Tarih.Text = Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToShortDateString();
                    siparis.xr_Acilis.Text = Convert.ToString(timeSpan);
                    siparis.txtDepartman.Text = Departman.Dep_Adi;

                    if (garsonsor.Equals(""))
                    {
                        siparis.xr_Garson.Text = Convert.ToString(dtSiparis.Rows[0]["Garson"]);
                    }
                    else
                    {
                        siparis.xr_Garson.Text = garsonsor;
                    }

                    siparis.xr_Cek.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]);

                    siparis.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
                    siparis.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));



                    //if (Param.Param_SiparisTutar)
                    //{
                    //    //bakiye += Convert.ToDecimal(dtSiparis.Rows[j]["Rsat_Tutar"]);
                    //}
                    //}


                    //if (Param.Param_SiparisTutar)
                    //{
                    //    //siparis.Add("------------------------");
                    //    //siparis.Add(" TOPLAM : " + bakiye.ToString("n2"));
                    //    //siparis.Add("------------------------");
                    //}


                    siparis.Print();
                }

                AbuyerPr(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            }



            return "OK";
        }

        public string YS_SiparisPr(int Fisno, bool Mars, int Split, string abuyerBaslik = "   * * * ABUYER FISI * * *   ", string kartDetay1 = "", string kartdetay2 = "", bool hizliSatis = false)
        {
            List<string> siparis = new List<string>();
            DataTable dtPrinter = SiparisPrinterBul(Fisno, Split, false);

            decimal bakiye = 0;

            bool isMac_Printer = false;
            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                {
                    isMac_Printer = true;
                }
            }


            //bool hic = false;


            //foreach (DataRow item in dtPrinter.Rows)
            //{
            //    string printer = item["Printer"].ToString();
            //    string macPrinter =item["Mac_Printer"].ToString();
            //    if (printer != "" || macPrinter != "")
            //    {
            //        hic = true;
            //    }
            //}

            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtPrinter.Rows[i]["Printer"]);
                int bosSatir = Convert.ToInt32(dtPrinter.Rows[i]["Pkod_Satir"]);

                if (printer == "" && Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) == "") continue;

                if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                {
                    printer = Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]);
                }

                DataTable dtSiparis = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 1);
                com.Parameters.AddWithValue("@Printer", printer);
                com.Parameters.AddWithValue("@Mars", Mars);
                com.Parameters.AddWithValue("@Split", Split);
                com.Parameters.AddWithValue("@MacAdres", isMac_Printer ? dbtools.MacAdresi() : "");
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtSiparis);

                if (Param.Param_SiparisAna)
                {
                    DataView dv = dtSiparis.DefaultView;
                    dv.Sort = "AnaGrupAdi desc";
                    dtSiparis = dv.ToTable();
                }

                if (con.State != ConnectionState.Closed) con.Close();

                if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtSiparis.Rows[0]["Pkod_Printer"]);

                string paketAciklama = "";
                string masaNo = Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"]);
                DataTable dt = dbtools.SelectTable("select ISNULL(Masa_Paket,0) as Masa_Paket from Pos_Masa where Masa_No = '" + masaNo + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["Masa_Paket"]))
                    {
                        paketAciklama = " (PAKET) ";
                    }
                }


                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                for (int k = 0; k < Siparis_Ciktisayisi; k++)
                {
                    //siparis.Add("Printer : " + printer);
                    //siparis.Add("Ek-1 : " + Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr1"]));
                    //siparis.Add("Ek-2 : " + Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr2"]));
                    //siparis.Add("Ek-3 : " + Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr3"]));



                    if (!hizliSatis)
                    {
                        siparis.Add(".");
                        siparis.Add("   * * * SIPARIS FISI * * *  ");
                        siparis.Add("#Masa :" + (Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"])).PadRight(7, ' ') + "  #Kisi :" + Convert.ToString(dtSiparis.Rows[0]["Rsat_Kisi"]));
                        siparis.Add(".");
                        siparis.Add("#Konumu :" + (Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                        siparis.Add(".");
                        if (!string.IsNullOrEmpty(paketAciklama)) siparis.Add(paketAciklama);
                        siparis.Add(".");
                        siparis.Add("#Departman : " + Departman.Dep_Adi);
                        siparis.Add(".");
                        siparis.Add("Tarih:" + Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                        siparis.Add("Cekno:" + Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                        siparis.Add(".");
                        siparis.Add("Grson:" + Convert.ToString(dtSiparis.Rows[0]["Garson"]));//.PadRight(10, ' '));// + " Grsn2:" + Convert.ToString(dtSiparis.Rows[0]["Garson2"]).PadRight(5, ' '));
                        siparis.Add(".");
                        siparis.Add(".");
                        siparis.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                    }
                    else
                    {

                        siparis.Add(".");
                        siparis.Add("   * * * SIPARIS FISI * * *  ");
                        siparis.Add(".");
                        if (!string.IsNullOrEmpty(paketAciklama)) siparis.Add(paketAciklama);
                        siparis.Add("#Konumu :" + (Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                        siparis.Add(".");
                        siparis.Add("Departman : " + Departman.Dep_Adi);
                        siparis.Add("Tarih:" + Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                        siparis.Add("#Cekno:" + Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                        siparis.Add(".");
                        siparis.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));

                    }
                    string YeniAnaGrup = string.Empty;
                    string EskiAnaGrup = string.Empty;
                    for (int j = 0; j < dtSiparis.Rows.Count; j++)
                    {
                        if (Param.Param_SiparisAna == false)
                        {
                            siparis.Add("".PadLeft(36, '-'));

                            siparis.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtSiparis.Rows[j]["Rsat_Miktar"])).PadRight(4, ' ') + " " + Convert.ToString(dtSiparis.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtSiparis.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));

                            if (Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]) != String.Empty)
                            {
                                string[] db_Parcala;
                                db_Parcala = Convert.ToString(Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"])).Split('|');

                                foreach (string a in db_Parcala)
                                {
                                    siparis.Add("-- " + Convert.ToString(a));
                                }
                            }
                        }
                        else
                        {
                            YeniAnaGrup = Convert.ToString(dtSiparis.Rows[j]["AnaGrupAdi"]);

                            if (YeniAnaGrup != EskiAnaGrup)
                            {
                                siparis.Add("".PadLeft(36, '-'));

                                siparis.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtSiparis.Rows[j]["Rsat_Miktar"])).PadRight(4, ' ') + " " + Convert.ToString(dtSiparis.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtSiparis.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));

                                if (Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]) != String.Empty)
                                {
                                    string[] db_Parcala;
                                    db_Parcala = Convert.ToString(Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"])).Split('|');

                                    foreach (string a in db_Parcala)
                                    {
                                        siparis.Add("-- " + Convert.ToString(a));
                                    }
                                }

                                EskiAnaGrup = YeniAnaGrup;
                            }
                            else
                            {
                                siparis.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtSiparis.Rows[j]["Rsat_Miktar"])).PadRight(4, ' ') + " " + Convert.ToString(dtSiparis.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtSiparis.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));

                                if (Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]) != String.Empty)
                                {
                                    string[] db_Parcala;
                                    db_Parcala = Convert.ToString(Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"])).Split('|');

                                    foreach (string a in db_Parcala)
                                    {
                                        siparis.Add("-- " + Convert.ToString(a));
                                    }
                                }

                                EskiAnaGrup = YeniAnaGrup;
                            }
                        }



                        if (Param.Param_SiparisTutar)
                        {
                            bakiye += Convert.ToDecimal(dtSiparis.Rows[j]["Rsat_Tutar"]);

                        }
                    }


                    siparis.Add("".PadLeft(36, '-'));

                    if (Param.Param_SiparisTutar)
                    {
                        siparis.Add("------------------------");
                        siparis.Add(" TOPLAM : " + bakiye.ToString("n2"));
                        siparis.Add("------------------------");
                    }


                    for (int j = 0; j < bosSatir; j++)
                    {
                        siparis.Add(".");
                    }

                    //siparis.Add(Convert.ToString((char) 27) + "@" + Convert.ToString((char) 29) + "V" + (char)1);

                    try
                    {
                        System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                        Font fnt;

                        if (Param.Param_HspFontAlgilama == true)
                        {
                            fnt = null;
                            if (fnt == null)
                            {
                                fnt = new Font("Arial", 8);
                            }
                        }
                        else
                        {
                            fnt = (Font)converter.ConvertFromString(Siparis_Font);
                        }



                        printFont = fnt;

                        Liste = siparis;

                        PrintDocument pd = new PrintDocument();
                        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                        pd.PrinterSettings.PrinterName = printer;
                        pd.Print();

                        if (Param.Param_Printer_Tanim && Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr1"]) != "")
                        {
                            PrintDocument pd1 = new PrintDocument();
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.PrinterSettings.PrinterName = Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr1"]);
                            pd.Print();
                        }
                        if (Param.Param_Printer_Tanim && Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr2"]) != "")
                        {
                            PrintDocument pd2 = new PrintDocument();
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.PrinterSettings.PrinterName = Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr2"]);
                            pd.Print();
                        }
                        if (Param.Param_Printer_Tanim && Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr3"]) != "")
                        {
                            PrintDocument pd3 = new PrintDocument();
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.PrinterSettings.PrinterName = Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr3"]);
                            pd.Print();
                        }

                        siparis.Clear();
                    }
                    catch (Exception err)
                    {
                        return err.Message;
                    }
                }
            }

            AbuyerPr(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            //AbuyerPr2(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            //AbuyerPr3(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            //AbuyerPr4(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            return "OK";
        }

        public string PaketPr(int Fisno, string Baslik, string siparisFisi = "", string GetirYemek_Order_ID = "",string parcaliAdres="")
        {

            try
            {
                string printer = String.Empty;
                int bosSatir = 0;
                string filter = "";
                string posta = Masa_Posta_bul(Fisno);
                if (Departman.Kodlar_Pr_Posta)
                {
                    filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
                }
                else
                {
                    filter = " and ISNULL(Pkod_Posta,'') = ''  ";
                }
                DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'PKT' " + filter);
                if (dtPrinter.Rows.Count > 0)
                {
                    printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                    bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
                }
                //DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'PKT'");
                //if (dtPrinter.Rows.Count > 0)
                //{
                //    printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                //    bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
                //}

                DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'PKT' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
                if (dtMacPrinter.Rows.Count > 0)
                {
                    printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
                }


                DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'PAKETFISI'");
                if (dtDizayn.Rows.Count < 1)
                {
                    return "Paket Dizaynı Yapılmamış...";
                }

                DateTime Tarih = Convert.ToDateTime(dbtools.DegerGetir("SELECT ISNULL((Select top(1) Rsat_Tarih From Cst_Recete_Satis Where Rsat_Fisno = '" + Fisno + "') , getdate())"));

                DataTable dtPaket = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Tarih1", Tarih);
                com.Parameters.AddWithValue("@Rapor_Tipi", 5);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtPaket);
                if (con.State == ConnectionState.Open) con.Close();

                int dtSatirsay = dtPaket.Rows.Count;

                Print.Paket paket = new Print.Paket();
                xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), paket);
                paket.PrinterName = printer;


                if (!GetirYemek_Order_ID.Equals("")) // bu kısım sırf adres ve tel için yapıldı
                {
                    string query = @"select top 1
GOrder_Client_clientPhoneNumber,
GOrder_Client_contactPhoneNumber,
GOrder_Client_DeliveryAddress_address,
GOrder_Client_DeliveryAddress_aptNo,
GOrder_Client_DeliveryAddress_floor,
GOrder_Client_DeliveryAddress_doorNo,
GOrder_Client_DeliveryAddress_description
from GetirYemek_Order where ID='" + GetirYemek_Order_ID + "'";
                    DataTable dt = dbtools.SelectTableR(query);

                    foreach (DataRow item in dtPaket.Rows)
                    {
                        dtPaket.Rows[0]["Cari_Tel"] = dt.Rows[0]["GOrder_Client_clientPhoneNumber"].ToString() + "\n" + dt.Rows[0]["GOrder_Client_contactPhoneNumber"].ToString();

                        string adres = dt.Rows[0]["GOrder_Client_DeliveryAddress_address"] + " Apt No: " + dt.Rows[0]["GOrder_Client_DeliveryAddress_aptNo"] + ", Kat: " + dt.Rows[0]["GOrder_Client_DeliveryAddress_floor"] + ", Daire No: " + dt.Rows[0]["GOrder_Client_DeliveryAddress_doorNo"] + "\nAdres Açıklama : " + dt.Rows[0]["GOrder_Client_DeliveryAddress_description"];

                        dtPaket.Rows[0]["Cari_Adres1"] = adres;
                    }

                }

                if (Baslik.Contains("GETİRYEMEK"))
                {
                    foreach (DataRow item in dtPaket.Rows)
                    {

                        string tutar = item["Rsat_Tutar"].ToString();
                        int tutar1 = Convert.ToInt32(Convert.ToDouble(tutar));
                        if (tutar1 == 0)
                        {
                            item["Rsat_Aciklama"] = "";
                        }
                    }
                }

                if (siparisFisi.Equals(""))
                {
                    paket.DataSource = dtPaket;
                }
                else
                {
                    DataTable copyDataTable = dtPaket.Clone();

                    for (int i = 0; i < copyDataTable.Columns.Count; i++)
                    {
                        copyDataTable.Columns[i].DataType = typeof(string);
                    }


                    foreach (DataRow item in dtPaket.Rows)
                    {
                        copyDataTable.ImportRow(item);

                        break;
                    }
                    foreach (DataRow item in copyDataTable.Rows)
                    {
                        item["Rec_Ad"] = siparisFisi;
                        item["Rsat_Aciklama"] = "";
                    }
                    paket.xr_Miktar.Visible = false;
                    paket.xr_Tutar.Visible = false;

                    paket.xrLabel18.Visible = false;
                    paket.xrLabel19.Visible = false;
                    paket.xr_Toplam.Visible = false;
                    paket.xrLine1.Visible = false;


                    paket.xr_Urun.WidthF = 266;
                    paket.xr_Urun.WordWrap = true;
                    paket.DataSource = copyDataTable;
                }


                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                if (dtPaket.Rows.Count > 0)
                {
                    //for (int k = 0; k < Paket_Ciktisayisi; k++)
                    //{
                    paket.xr_Baslik.Text = Baslik;
                    paket.xr_MasaNo.Text = Convert.ToString(dtPaket.Rows[0]["Rsat_Masa"]);
                    paket.xr_Konum.Text = Convert.ToString(dtPaket.Rows[0]["MasaKonumAdi"]);
                    paket.xr_Tarih.Text = Convert.ToDateTime(dtPaket.Rows[0]["Rsat_Tarih"]).ToShortDateString();

                    string acilis = dtPaket.Rows[0]["Rsat_Acilis"].ToString();

                    if (acilis.Trim() == "" && dtPaket.Rows.Count > 1)
                    {

                        paket.xr_Acilis.Text = Convert.ToDateTime(dtPaket.Rows[1]["Rsat_Acilis"].ToString()).ToString("HH:mm:ss");
                    }
                    else
                    {
                        paket.xr_Acilis.Text = acilis;

                    }

                    paket.xr_Cek.Text = Convert.ToString(dtPaket.Rows[0]["Rsat_Fisno"]);
                    paket.xr_CariAdSoyad.Text = Convert.ToString(dtPaket.Rows[0]["Cari_Ad"]) + " " + Convert.ToString(dtPaket.Rows[0]["Cari_Soyad"]);
                    paket.xr_Tel.Text = Convert.ToString(dtPaket.Rows[0]["Cari_Tel"]);

                    paket.xr_Tel.Text = Convert.ToString(dtPaket.Rows[0]["Cari_Tel"]);
                    paket.txtDeger1.Text = Convert.ToString(dtPaket.Rows[0]["deger1"]);



                    paket.xr_Adres.Text = (Convert.ToString(dtPaket.Rows[0]["Cari_Adres1"]) + "\n" + Convert.ToString(dtPaket.Rows[0]["Cari_Adres2"]) + "\n" + Convert.ToString(dtPaket.Rows[0]["Cari_Adres3"]) + "\n"
                    + Convert.ToString(dtPaket.Rows[0]["Cari_Mahalle"]) + "\n" + Convert.ToString(dtPaket.Rows[0]["Cari_Ilce"]) + " - " + Convert.ToString(dtPaket.Rows[0]["Cari_Il"]));


                    //if (parcaliAdres!="")
                    //{
                    //    paket.xr_Adres.Text = parcaliAdres;
                    //}

                    // 05.03.2025 emre atalay istedi
                    string aktifAdres = dbtools.DegerGetir("select top 1 isnull(adressecenek,1) as adressecenek from Pos_Cari where Cari_Kod='"+ dtPaket.Rows[0]["Cari_Kod"] + "'");

                    switch (aktifAdres)
                    {
                        case "1":
                            paket.xr_Adres.Text = Convert.ToString(dtPaket.Rows[0]["Cari_Adres1"]);

                            break;
                        case "2":
                            paket.xr_Adres.Text = Convert.ToString(dtPaket.Rows[0]["Cari_Adres2"]);

                            break;
                        case "3":
                            paket.xr_Adres.Text = Convert.ToString(dtPaket.Rows[0]["Cari_Adres3"]);

                            break;
                        default:
                            paket.xr_Adres.Text = Convert.ToString(dtPaket.Rows[0]["Cari_Adres1"]);
                            break;
                    }



                   paket.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]".ToString().Replace("|", "\n")));
                    paket.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
                    paket.xr_Tutar.Text = "[Rsat_Tutar]";

                    paket.txtSirano.Text = "[sirano]";



                    decimal UrunToplam = 0;
                    for (int i = 0; i < dtPaket.Rows.Count; i++)
                    {
                        UrunToplam += Convert.ToDecimal(dtPaket.Rows[i]["Rsat_Tutar"]);
                    }


                    paket.xr_Toplam.Text = UrunToplam.ToString("n2");

                    paket.xr_Not.Text = Convert.ToString(dtPaket.Rows[0]["Rsat_Not"]);


                    paket.xr_Paketci.Text = Convert.ToString(dtPaket.Rows[0]["Paketci"]);

                    //7.06.2024 İLERİ TARİH TEXTİ YAPILDI -RMOS SEPET...

                    string paketqr = $@"select top 1 isnull(deger1,'') as deger1 from Cst_Recete_Satis where Rsat_Fisno='{Fisno}' and (deger1 is not null and deger1<>'')";

                    string ileriTarih = dbtools.DegerGetir(paketqr);

                    paket.txtIleriTarih.Text = ileriTarih;


                    DataTable lat1 = dbtools.SelectTableR("select top 1 isnull(latitude,'') as latitude,isnull(longitude,'') as longitude from Cst_Recete_Satis where Rsat_Fisno='" + Fisno + "' and latitude<>'' and latitude is not null");

                    if (lat1!=null && lat1.Rows.Count>0)
                    {
                        string latitude = lat1.Rows[0][0].ToString();
                        string longitude = lat1.Rows[0][1].ToString();

                        paket.txtQr.Text = $"https://www.google.com/maps?q={latitude},{longitude}";
                    }
                    else
                    {
                        paket.txtQr.Visible = false;
                        paket.PageFooter.HeightF = (float)128;
                    }
      

                    for (int i = 0; i < Paket_Ciktisayisi; i++)
                    {
                        paket.Print();
                    }

                }

                return "OK";
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "PaketPr", "", ex);
                return "OK";
            }

        }

        public static string MyClass = "FisPr";

        public string PaketPrTrendyol(int Fisno, string Baslik)
        {

            try
            {
                string printer = String.Empty;
                int bosSatir = 0;
                string filter = "";
                string posta = Masa_Posta_bul(Fisno);
                if (Departman.Kodlar_Pr_Posta)
                {
                    filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
                }
                else
                {
                    filter = " and ISNULL(Pkod_Posta,'') = ''  ";
                }
                DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'PKT' " + filter);
                if (dtPrinter.Rows.Count > 0)
                {
                    printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                    bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
                }
                //DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'PKT'");
                //if (dtPrinter.Rows.Count > 0)
                //{
                //    printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                //    bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
                //}

                DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'PKT' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
                if (dtMacPrinter.Rows.Count > 0)
                {
                    printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
                }


                DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'PAKETFISI'");
                if (dtDizayn.Rows.Count < 1)
                {
                    return "Paket Dizaynı Yapılmamış...";
                }

                DateTime Tarih = Convert.ToDateTime(dbtools.DegerGetir("SELECT ISNULL((Select top(1) Rsat_Tarih From Cst_Recete_Satis Where Rsat_Fisno = '" + Fisno + "') , getdate())"));

                DataTable dtPaket = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Tarih1", Tarih);
                com.Parameters.AddWithValue("@Rapor_Tipi", 5);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtPaket);
                if (con.State == ConnectionState.Open) con.Close();

                int dtSatirsay = dtPaket.Rows.Count;

                Print.Paket paket = new Print.Paket();

                string raporId = Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]);
                xtraDizayn.LoadReportStream(raporId, paket);


                if (printer.Trim() == "")
                {
                    RHMesaj.MyMessageInformation("Yazıcı ismini bulamadı -> FisPr.cs->PaketPrTrendyol()\nMicrosoft XPS Document Writer -> olarak ayarlandı! ");
                    printer = "Microsoft XPS Document Writer";
                }
                paket.PrinterName = printer;



                paket.DataSource = dtPaket;




                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                if (dtPaket.Rows.Count > 0)
                {
                    //for (int k = 0; k < Paket_Ciktisayisi; k++)
                    //{
                    paket.xr_Baslik.Text = Baslik;
                    paket.xr_MasaNo.Text = Convert.ToString(dtPaket.Rows[0]["Rsat_Masa"]);
                    paket.xr_Konum.Text = Convert.ToString(dtPaket.Rows[0]["MasaKonumAdi"]);
                    paket.xr_Tarih.Text = Convert.ToDateTime(dtPaket.Rows[0]["Rsat_Tarih"]).ToShortDateString();

                    string acilis = dtPaket.Rows[0]["Rsat_Acilis"].ToString();

                    if (acilis.Trim() == "" && dtPaket.Rows.Count > 1)
                    {

                        paket.xr_Acilis.Text = Convert.ToDateTime(dtPaket.Rows[1]["Rsat_Acilis"].ToString()).ToString("HH:mm:ss");
                    }
                    else
                    {
                        paket.xr_Acilis.Text = acilis;

                    }

                    paket.xr_Cek.Text = Convert.ToString(dtPaket.Rows[0]["Rsat_Fisno"]);
                    paket.xr_CariAdSoyad.Text = Convert.ToString(dtPaket.Rows[0]["Cari_Ad"]) + " " + Convert.ToString(dtPaket.Rows[0]["Cari_Soyad"]);
                    paket.xr_Tel.Text = Convert.ToString(dtPaket.Rows[0]["Cari_Tel"]);

                    paket.txtDeger1.Text = Convert.ToString(dtPaket.Rows[0]["deger1"]);



                    paket.xr_Adres.Text = (Convert.ToString(dtPaket.Rows[0]["Cari_Adres1"]) + "\n" + Convert.ToString(dtPaket.Rows[0]["Cari_Adres2"]) + "\n" + Convert.ToString(dtPaket.Rows[0]["Cari_Adres3"]) + "\n"
                    + Convert.ToString(dtPaket.Rows[0]["Cari_Mahalle"]) + "\n" + Convert.ToString(dtPaket.Rows[0]["Cari_Ilce"]) + " - " + Convert.ToString(dtPaket.Rows[0]["Cari_Il"]));



                    //paket.xr_Urun.Text = "[Rsat_EntegreAd]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]".ToString().Replace("|", "\n")));

                    paket.xr_Urun.Text = "[Rsat_EntegreAd]";

                    paket.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
                    paket.xr_Tutar.Text = "[Rsat_Tutar]";



                    //decimal UrunToplam = 0;
                    //for (int i = 0; i < dtPaket.Rows.Count; i++)
                    //{
                    //    UrunToplam += Convert.ToDecimal(dtPaket.Rows[i]["Rsat_Tutar"]);
                    //}


                    paket.xr_Toplam.Text = "[Rsat_EntegreToplamFiyat]";

                    paket.xr_Not.Text = Convert.ToString(dtPaket.Rows[0]["Rsat_Not"]);
                    paket.xr_Paketci.Text = Convert.ToString(dtPaket.Rows[0]["Paketci"]);

                    DataTable lat1 = dbtools.SelectTableR("select top 1 isnull(latitude,'') as latitude,isnull(longitude,'') as longitude from Cst_Recete_Satis where Rsat_Fisno='"+Fisno+"' and latitude<>'' and latitude is not null");

                    string latitude = lat1.Rows[0][0].ToString();
                    string longitude = lat1.Rows[0][1].ToString();

                    paket.txtQr.Text = $"https://www.google.com/maps?q={latitude},{longitude}";

                    for (int i = 0; i < Paket_Ciktisayisi; i++)
                    {


                        paket.Print();
                    }

                }

                return "OK";
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "PaketPr", "", ex);
                return "OK";
            }

        }


        public string ZayiPr(int Fisno, bool Mars, int Split, string abuyerBaslik = "   * * * ZAYI FISI * * *   ", string kartDetay1 = "", string kartdetay2 = "", bool hizliSatis = false)
        {
            List<string> siparis = new List<string>();
            DataTable dtPrinter = ZayiPrinterBul(Fisno, Split, false);


            bool isMac_Printer = false;
            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                {
                    isMac_Printer = true;
                }
            }

            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtPrinter.Rows[i]["Printer"]);
                int bosSatir = Convert.ToInt32(dtPrinter.Rows[i]["Pkod_Satir"]);

                if (printer == "" && Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) == "") continue;

                if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                {
                    printer = Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]);
                }

                DataTable dtSiparis = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 13);
                com.Parameters.AddWithValue("@Printer", printer);
                com.Parameters.AddWithValue("@Mars", Mars);
                com.Parameters.AddWithValue("@Split", Split);
                com.Parameters.AddWithValue("@MacAdres", isMac_Printer ? dbtools.MacAdresi() : "");
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtSiparis);

                if (con.State != ConnectionState.Closed) con.Close();

                if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtSiparis.Rows[i]["Pkod_Printer"]);

                string paketAciklama = "";
                string masaNo = Convert.ToString(dtSiparis.Rows[i]["Rsat_Masa"]);
                DataTable dt = dbtools.SelectTable("select ISNULL(Masa_Paket,0) as Masa_Paket from Pos_Masa where Masa_No = '" + masaNo + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["Masa_Paket"]))
                    {
                        paketAciklama = " (PAKET) ";
                    }
                }


                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                for (int k = 0; k < Siparis_Ciktisayisi; k++)
                {
                    //siparis.Add("Printer : " + printer);
                    //siparis.Add("Ek-1 : " + Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr1"]));
                    //siparis.Add("Ek-2 : " + Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr2"]));
                    //siparis.Add("Ek-3 : " + Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr3"]));
                    if (!hizliSatis)
                    {
                        siparis.Add(".");
                        siparis.Add("   * * * ZAYI FISI * * *  ");
                        siparis.Add("#Masa :" + (Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"])).PadRight(7, ' ') + "  #Kisi :" + Convert.ToString(dtSiparis.Rows[0]["Rsat_Kisi"]));
                        siparis.Add(".");
                        siparis.Add("#Konumu :" + (Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                        siparis.Add(".");
                        if (!string.IsNullOrEmpty(paketAciklama)) siparis.Add(paketAciklama);
                        siparis.Add(".");
                        siparis.Add("#Departman : " + Departman.Dep_Adi);
                        siparis.Add(".");
                        siparis.Add("Tarih:" + Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                        siparis.Add("Cekno:" + Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                        siparis.Add(".");
                        siparis.Add("Grson:" + Convert.ToString(dtSiparis.Rows[0]["Garson"]));//.PadRight(10, ' '));// + " Grsn2:" + Convert.ToString(dtSiparis.Rows[0]["Garson2"]).PadRight(5, ' '));
                        siparis.Add(".");
                        siparis.Add(".");
                        siparis.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                    }
                    else
                    {
                        siparis.Add(".");
                        siparis.Add("   * * * ZAYI FISI * * *  ");
                        siparis.Add(".");
                        if (!string.IsNullOrEmpty(paketAciklama)) siparis.Add(paketAciklama);
                        siparis.Add("#Konumu :" + (Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                        siparis.Add(".");
                        siparis.Add("Departman : " + Departman.Dep_Adi);
                        siparis.Add("Tarih:" + Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                        siparis.Add("#Cekno:" + Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                        siparis.Add(".");
                        siparis.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                    }
                    for (int j = 0; j < dtSiparis.Rows.Count; j++)
                    {
                        siparis.Add("".PadLeft(36, '-'));

                        siparis.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtSiparis.Rows[j]["Rsat_Miktar"])).PadRight(4, ' ') + " " + Convert.ToString(dtSiparis.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtSiparis.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));

                        if (Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]) != String.Empty) siparis.Add(Convert.ToString(dtSiparis.Rows[j]["Rsat_Aciklama"]));
                    }
                    siparis.Add("".PadLeft(36, '-'));

                    for (int j = 0; j < bosSatir; j++)
                    {
                        siparis.Add(".");
                    }

                    //siparis.Add(Convert.ToString((char) 27) + "@" + Convert.ToString((char) 29) + "V" + (char)1);

                    try
                    {
                        System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                        Font fnt = (Font)converter.ConvertFromString(Siparis_Font);

                        if (Param.Param_HspFontAlgilama == true)
                        {
                            fnt = null;
                            if (fnt == null)
                            {
                                fnt = new Font("Arial", 8);
                            }
                        }

                        printFont = fnt;

                        Liste = siparis;

                        PrintDocument pd = new PrintDocument();
                        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                        pd.PrinterSettings.PrinterName = printer;
                        pd.Print();

                        if (Param.Param_Printer_Tanim && Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr1"]) != "")
                        {
                            PrintDocument pd1 = new PrintDocument();
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.PrinterSettings.PrinterName = Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr1"]);
                            pd.Print();
                        }
                        if (Param.Param_Printer_Tanim && Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr2"]) != "")
                        {
                            PrintDocument pd2 = new PrintDocument();
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.PrinterSettings.PrinterName = Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr2"]);
                            pd.Print();
                        }
                        if (Param.Param_Printer_Tanim && Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr3"]) != "")
                        {
                            PrintDocument pd3 = new PrintDocument();
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.PrinterSettings.PrinterName = Convert.ToString(dtSiparis.Rows[0]["Pkod_Ek_Pr3"]);
                            pd.Print();
                        }

                        siparis.Clear();
                    }
                    catch (Exception err)
                    {
                        return err.Message;
                    }
                }
            }

            AbuyerPr(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            //AbuyerPr2(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            //AbuyerPr3(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            //AbuyerPr4(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            return "OK";
        }
        public string newZayiPr(int Fisno, bool Mars, int Split, string abuyerBaslik = "   * * * ZAYI FISI * * *   ", string kartDetay1 = "", string kartdetay2 = "", bool hizliSatis = false)
        {

            DataTable dtPrinter = ZayiPrinterBul(Fisno, Split, false);


            bool isMac_Printer = false;
            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                {
                    isMac_Printer = true;
                }
            }


            DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'ZAYIFISI'");
            if (dtDizayn.Rows.Count < 1)
            {
                return "Zayi Fişi Dizaynı Yapılmamış...";
            }

            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtPrinter.Rows[i]["Printer"]);
                int bosSatir = Convert.ToInt32(dtPrinter.Rows[i]["Pkod_Satir"]);

                if (printer == "" && Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) == "") continue;

                if (Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]) != "")
                {
                    printer = Convert.ToString(dtPrinter.Rows[i]["Mac_Printer"]);
                }

                DataTable dtSiparis = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 13);
                com.Parameters.AddWithValue("@Printer", printer);
                com.Parameters.AddWithValue("@Mars", Mars);
                com.Parameters.AddWithValue("@Split", Split);
                com.Parameters.AddWithValue("@MacAdres", isMac_Printer ? dbtools.MacAdresi() : "");
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtSiparis);

                if (con.State != ConnectionState.Closed) con.Close();

                if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtSiparis.Rows[0]["Pkod_Printer"]);


                //string masaNo = Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"]);
                //DataTable dt = dbtools.SelectTable("select ISNULL(Masa_Paket,0) as Masa_Paket from Pos_Masa where Masa_No = '" + masaNo + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                //if (dt.Rows.Count > 0)
                //{
                //    if (Convert.ToBoolean(dt.Rows[0]["Masa_Paket"]))
                //    {
                //        string paketAciklama = " (PAKET) ";
                //    }
                //}


                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                for (int k = 0; k < Siparis_Ciktisayisi; k++)
                {

                    Print.Zayi Zayi = new Print.Zayi();
                    xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), Zayi);
                    Zayi.PrinterName = printer;
                    Zayi.DataSource = dtSiparis;

                    Zayi.xr_MasaNo.Text = (Convert.ToString(dtSiparis.Rows[0]["Rsat_Masa"]));
                    //Zayi.xr_ #Kisi :" + Convert.ToString(dtSiparis.Rows[0]["Rsat_Kisi"]));

                    Zayi.xr_Konum.Text = (Convert.ToString(dtSiparis.Rows[0]["MasaKonumAdi"]));

                    //if (!string.IsNullOrEmpty(paketAciklama)) siparis.Add(paketAciklama);
                    //siparis.Add(".");
                    Zayi.xr_OtelAdi.Text = Departman.Dep_Adi;

                    Zayi.xr_Tarih.Text = Convert.ToDateTime(dtSiparis.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy");
                    Zayi.xr_Acilis.Text = Convert.ToString(timeSpan);
                    Zayi.xr_Cek.Text = Convert.ToString(dtSiparis.Rows[0]["Rsat_Fisno"]);
                    Zayi.xr_Garson.Text = Convert.ToString(dtSiparis.Rows[0]["Garson"]);

                    for (int j = 0; j < dtSiparis.Rows.Count; j++)
                    {
                        Zayi.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
                        Zayi.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));

                        Zayi.Print();
                    }
                }
            }

            AbuyerPr(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            //AbuyerPr2(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            //AbuyerPr3(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            //AbuyerPr4(Fisno, Mars, Split, abuyerBaslik, kartDetay1, kartdetay2, hizliSatis);
            return "OK";
        }



        public string AbuyerPr33(int Fisno, bool Mars, int Split, string baslik, string kartDetay1, string kartdetay2, bool hizliSatis)
        {
            List<string> abuyer = new List<string>();
            DataTable dtPrinter = AbuyerPrinterBul(Fisno, Split, true);

            int abuyerCiktisayisi = 1;
            try
            {
                abuyerCiktisayisi = Convert.ToInt32(dbtools.DegerGetir("select ISNULL(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif='17' and Pkod_Kod='ABUYERSAYI'"));
            }
            catch (Exception ex)
            {


            }

            string printer = "";
            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                for (int a = 0; a < dtPrinter.Columns.Count; a++)
                {
                    printer = Convert.ToString(dtPrinter.Rows[i][a]);

                    if (printer == "")
                    {
                        continue;
                    }


                    DataTable dtAbuyer = new DataTable();
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;

                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis";
                    com.Parameters.AddWithValue("@Fisno", Fisno);
                    com.Parameters.AddWithValue("@Rapor_Tipi", 9);
                    com.Parameters.AddWithValue("@Printer", printer);
                    com.Parameters.AddWithValue("@Mars", Mars);
                    com.Parameters.AddWithValue("@Split", Split);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(dtAbuyer);

                    if (con.State != ConnectionState.Closed) con.Close();

                    if (dtAbuyer.Rows.Count <= 0)
                    {
                        continue;
                    }

                    if (Param.Param_Printer_Tanim)
                    {
                        printer = Convert.ToString(dtAbuyer.Rows[0]["Pkod_Printer"]);
                    }

                    foreach (DataRow boslaridoldur in dtAbuyer.Rows)
                    {
                        string yaziciismi = boslaridoldur["Rec_AbuyerPR"].ToString();
                        if (yaziciismi == "")
                        {
                            boslaridoldur["Rec_AbuyerPR"] = printer;
                        }
                    }


                    var yazicilar = dtAbuyer.AsEnumerable()
          .GroupBy(r => new { Col1 = r["Rec_AbuyerPR"] })
          .Select(g => g.OrderBy(r => r["Rec_AbuyerPR"]).First())
          .CopyToDataTable();



                    foreach (DataRow itemYazici in yazicilar.Rows) // 2 kere doncek
                    {
                        string yaziciismi = itemYazici["Rec_AbuyerPR"].ToString();



                        var yazilacaklar = dtAbuyer.Select("Rec_AbuyerPR='" + yaziciismi + "'").CopyToDataTable();






                        TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);



                        abuyer.Add(".");
                        //abuyer.Add("   * * * ABUYER FISI * * *  ");
                        if (!hizliSatis)
                        {
                            abuyer.Add(baslik);
                            abuyer.Add(" ");
                            abuyer.Add("Departman : " + Departman.Dep_Adi);
                            abuyer.Add(".");
                            abuyer.Add("#Masa :" + Convert.ToString(yazilacaklar.Rows[0]["Rsat_Masa"]).PadRight(10, ' ') + "  #Kisi :" + Convert.ToString(yazilacaklar.Rows[0]["Rsat_Kisi"]).PadRight(10, ' '));
                            abuyer.Add("#Konumu :" + (Convert.ToString(yazilacaklar.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                            abuyer.Add(".");
                            abuyer.Add("Tarih:" + Convert.ToDateTime(yazilacaklar.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                            abuyer.Add(".");
                            abuyer.Add("Cekno:" + Convert.ToString(yazilacaklar.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                            abuyer.Add(".");
                            abuyer.Add("Grson:" + Convert.ToString(yazilacaklar.Rows[0]["Garson"]));//.PadRight(10, ' '));// + " Grsn2:" + Convert.ToString(dtSiparis.Rows[0]["Garson2"]).PadRight(5, ' '));
                            abuyer.Add(".");
                            abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                        }
                        else
                        {
                            abuyer.Add(".");
                            abuyer.Add("   * * * SIPARIS FISI * * *  ");
                            abuyer.Add(".");
                            abuyer.Add("Departman : " + Departman.Dep_Adi);
                            abuyer.Add("Tarih:" + Convert.ToDateTime(yazilacaklar.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                            abuyer.Add("#Cekno:" + Convert.ToString(yazilacaklar.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                            abuyer.Add(".");
                            abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                        }
                        for (int j = 0; j < yazilacaklar.Rows.Count; j++)
                        {
                            abuyer.Add("".PadLeft(36, '-'));
                            if (Param.Param_SiparisSayi == false)
                            {
                                abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(yazilacaklar.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                            }
                            else
                            {
                                abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(yazilacaklar.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                            }
                            //abuyer.Add(" " + Convert.ToInt32(yazilacaklar.Rows[j]["Rsat_Miktar"]).ToString().PadRight(5, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rsat_Emiktar"]).PadRight(2, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                            if (Convert.ToString(yazilacaklar.Rows[j]["Rsat_Aciklama"]) != String.Empty) abuyer.Add(Convert.ToString(yazilacaklar.Rows[j]["Rsat_Aciklama"]));


                            string recAd = yazilacaklar.Rows[j]["Rec_Ad"].ToString();
                            string rsatId = yazilacaklar.Rows[j]["Rsat_Id"].ToString();

                            if (!recAd.Contains("**RZV**"))
                            {
                                dbtools.execcmd(@"update Cst_Recete_Satis set Rsat_AbuyerPr = 1,Rsat_AbuyerPr2 = 1,Rsat_AbuyerPr3 = 1,Rsat_AbuyerPr4 = 1 where Rsat_Id = '" + rsatId + "'");

                            }
                        }
                        abuyer.Add("".PadLeft(36, '-'));

                        abuyer.Add(".");
                        if (kartDetay1 != "") abuyer.Add(kartDetay1);
                        if (kartdetay2 != "") abuyer.Add(kartdetay2);

                        for (int j = 0; j < 5; j++)
                        {
                            abuyer.Add(".");
                        }

                        //siparis.Add(Convert.ToString((char) 27) + "@" + Convert.ToString((char) 29) + "V" + (char)1);

                        try
                        {
                            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                            Font fnt = (Font)converter.ConvertFromString(Siparis_Font);

                            if (fnt == null)
                            {
                                fnt = new Font("Arial", 8);
                            }

                            Liste = abuyer;
                            printFont = fnt;
                            PrintDocument pd = new PrintDocument();
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.PrinterSettings.PrinterName = yaziciismi;//printer;

                            for (int j = 0; j < abuyerCiktisayisi; j++)
                            {
                                pd.Print();
                            }

                            abuyer.Clear();
                        }
                        catch (Exception err)
                        {
                            return err.Message;
                        }
                    }
                }
            }

            //dbtools.execcmd(@"update Cst_Recete_Satis set Rsat_AbuyerPr = 1,Rsat_AbuyerPr2 = 1,Rsat_AbuyerPr3 = 1,Rsat_AbuyerPr4 = 1 where Rsat_Fisno = '" + Fisno + "'");

            return "OK";
        }

        public string AbuyerPrYedek1(int Fisno, bool Mars, int Split, string baslik, string kartDetay1, string kartdetay2, bool hizliSatis)
        {
            List<string> abuyer = new List<string>();
            DataTable dtPrinter = AbuyerPrinterBul(Fisno, Split, true);

            int abuyerCiktisayisi = 1;
            try
            {
                abuyerCiktisayisi = Convert.ToInt32(dbtools.DegerGetir("select ISNULL(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif='17' and Pkod_Kod='ABUYERSAYI'"));
            }
            catch (Exception ex)
            {


            }

            string printer = "";
            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                for (int a = 0; a < dtPrinter.Columns.Count; a++)
                {
                    printer = Convert.ToString(dtPrinter.Rows[i][a]);

                    if (printer == "")
                    {
                        continue;
                    }


                    DataTable dtAbuyer = new DataTable();
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;

                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis";
                    com.Parameters.AddWithValue("@Fisno", Fisno);
                    com.Parameters.AddWithValue("@Rapor_Tipi", 9);
                    com.Parameters.AddWithValue("@Printer", printer);
                    com.Parameters.AddWithValue("@Mars", Mars);
                    com.Parameters.AddWithValue("@Split", Split);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(dtAbuyer);

                    if (con.State != ConnectionState.Closed) con.Close();

                    if (dtAbuyer.Rows.Count <= 0)
                    {
                        continue;
                    }

                    if (Param.Param_Printer_Tanim)
                    {
                        printer = Convert.ToString(dtAbuyer.Rows[0]["Pkod_Printer"]);
                    }

                    foreach (DataRow boslaridoldur in dtAbuyer.Rows)
                    {
                        string yaziciismi = boslaridoldur["Rec_AbuyerPR"].ToString();
                        if (yaziciismi == "")
                        {
                            boslaridoldur["Rec_AbuyerPR"] = printer;
                        }
                    }


                    var yazicilar = dtAbuyer.AsEnumerable()
          .GroupBy(r => new { Col1 = r["Rec_AbuyerPR"] })
          .Select(g => g.OrderBy(r => r["Rec_AbuyerPR"]).First())
          .CopyToDataTable();



                    foreach (DataRow itemYazici in yazicilar.Rows) // 2 kere doncek
                    {
                        string yaziciismi = itemYazici["Rec_AbuyerPR"].ToString();



                        var yazilacaklar = dtAbuyer.Select("Rec_AbuyerPR='" + yaziciismi + "'").CopyToDataTable();






                        TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);



                        abuyer.Add(".");
                        //abuyer.Add("   * * * ABUYER FISI * * *  ");
                        if (!hizliSatis)
                        {
                            abuyer.Add(baslik);
                            abuyer.Add(" ");
                            abuyer.Add("Departman : " + Departman.Dep_Adi);
                            abuyer.Add(".");
                            abuyer.Add("#Masa :" + Convert.ToString(yazilacaklar.Rows[0]["Rsat_Masa"]).PadRight(10, ' ') + "  #Kisi :" + Convert.ToString(yazilacaklar.Rows[0]["Rsat_Kisi"]).PadRight(10, ' '));
                            abuyer.Add("#Konumu :" + (Convert.ToString(yazilacaklar.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                            abuyer.Add(".");
                            abuyer.Add("Tarih:" + Convert.ToDateTime(yazilacaklar.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                            abuyer.Add(".");
                            abuyer.Add("Cekno:" + Convert.ToString(yazilacaklar.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                            abuyer.Add(".");
                            abuyer.Add("Grson:" + Convert.ToString(yazilacaklar.Rows[0]["Garson"]));//.PadRight(10, ' '));// + " Grsn2:" + Convert.ToString(dtSiparis.Rows[0]["Garson2"]).PadRight(5, ' '));
                            abuyer.Add(".");
                            abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                        }
                        else
                        {
                            abuyer.Add(".");
                            abuyer.Add("   * * * SIPARIS FISI * * *  ");
                            abuyer.Add(".");
                            abuyer.Add("Departman : " + Departman.Dep_Adi);
                            abuyer.Add("Tarih:" + Convert.ToDateTime(yazilacaklar.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                            abuyer.Add("#Cekno:" + Convert.ToString(yazilacaklar.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                            abuyer.Add(".");
                            abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                        }
                        for (int j = 0; j < yazilacaklar.Rows.Count; j++)
                        {
                            abuyer.Add("".PadLeft(36, '-'));
                            if (Param.Param_SiparisSayi == false)
                            {
                                abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(yazilacaklar.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                            }
                            else
                            {
                                abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(yazilacaklar.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                            }
                            //abuyer.Add(" " + Convert.ToInt32(yazilacaklar.Rows[j]["Rsat_Miktar"]).ToString().PadRight(5, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rsat_Emiktar"]).PadRight(2, ' ') + Convert.ToString(yazilacaklar.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                            if (Convert.ToString(yazilacaklar.Rows[j]["Rsat_Aciklama"]) != String.Empty) abuyer.Add(Convert.ToString(yazilacaklar.Rows[j]["Rsat_Aciklama"]));
                        }
                        abuyer.Add("".PadLeft(36, '-'));

                        abuyer.Add(".");
                        if (kartDetay1 != "") abuyer.Add(kartDetay1);
                        if (kartdetay2 != "") abuyer.Add(kartdetay2);

                        for (int j = 0; j < 5; j++)
                        {
                            abuyer.Add(".");
                        }

                        //siparis.Add(Convert.ToString((char) 27) + "@" + Convert.ToString((char) 29) + "V" + (char)1);

                        try
                        {
                            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                            Font fnt = (Font)converter.ConvertFromString(Siparis_Font);

                            if (fnt == null)
                            {
                                fnt = new Font("Arial", 8);
                            }

                            Liste = abuyer;
                            printFont = fnt;
                            PrintDocument pd = new PrintDocument();
                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                            pd.PrinterSettings.PrinterName = yaziciismi;//printer;

                            for (int j = 0; j < abuyerCiktisayisi; j++)
                            {
                                pd.Print();
                            }

                            abuyer.Clear();
                        }
                        catch (Exception err)
                        {
                            return err.Message;
                        }
                    }
                }
            }

            dbtools.execcmd(@"update Cst_Recete_Satis set Rsat_AbuyerPr = 1,Rsat_AbuyerPr2 = 1,Rsat_AbuyerPr3 = 1,Rsat_AbuyerPr4 = 1 where Rsat_Fisno = '" + Fisno + "'");

            return "OK";
        }
        public string AbuyerPrYedek(int Fisno, bool Mars, int Split, string baslik, string kartDetay1, string kartdetay2, bool hizliSatis)
        {
            List<string> abuyer = new List<string>();
            DataTable dtPrinter = AbuyerPrinterBul(Fisno, Split, true);

            string printer = "";
            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                for (int a = 0; a < dtPrinter.Columns.Count; a++)
                {
                    printer = Convert.ToString(dtPrinter.Rows[i][a]);

                    if (printer == "")
                    {
                        continue;
                    }


                    DataTable dtAbuyer = new DataTable();
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;

                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis";
                    com.Parameters.AddWithValue("@Fisno", Fisno);
                    com.Parameters.AddWithValue("@Rapor_Tipi", 9);
                    com.Parameters.AddWithValue("@Printer", printer);
                    com.Parameters.AddWithValue("@Mars", Mars);
                    com.Parameters.AddWithValue("@Split", Split);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(dtAbuyer);

                    if (con.State != ConnectionState.Closed) con.Close();

                    if (dtAbuyer.Rows.Count <= 0)
                    {
                        continue;
                    }

                    if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtAbuyer.Rows[0]["Pkod_Printer"]);

                    TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);



                    abuyer.Add(".");
                    //abuyer.Add("   * * * ABUYER FISI * * *  ");
                    if (!hizliSatis)
                    {
                        abuyer.Add(baslik);
                        abuyer.Add(" ");
                        abuyer.Add("Departman : " + Departman.Dep_Adi);
                        abuyer.Add(".");
                        abuyer.Add("#Masa :" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Masa"]).PadRight(10, ' ') + "  #Kisi :" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Kisi"]).PadRight(10, ' '));
                        abuyer.Add("#Konumu :" + (Convert.ToString(dtAbuyer.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                        abuyer.Add(".");
                        abuyer.Add("Tarih:" + Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                        abuyer.Add(".");
                        abuyer.Add("Cekno:" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                        abuyer.Add(".");
                        abuyer.Add("Grson:" + Convert.ToString(dtAbuyer.Rows[0]["Garson"]));//.PadRight(10, ' '));// + " Grsn2:" + Convert.ToString(dtSiparis.Rows[0]["Garson2"]).PadRight(5, ' '));
                        abuyer.Add(".");
                        abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                    }
                    else
                    {
                        abuyer.Add(".");
                        abuyer.Add("   * * * SIPARIS FISI * * *  ");
                        abuyer.Add(".");
                        abuyer.Add("Departman : " + Departman.Dep_Adi);
                        abuyer.Add("Tarih:" + Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                        abuyer.Add("#Cekno:" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                        abuyer.Add(".");
                        abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                    }
                    for (int j = 0; j < dtAbuyer.Rows.Count; j++)
                    {
                        abuyer.Add("".PadLeft(36, '-'));
                        if (Param.Param_SiparisSayi == false)
                        {
                            abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtAbuyer.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                        }
                        else
                        {
                            abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtAbuyer.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                        }
                        //abuyer.Add(" " + Convert.ToInt32(dtAbuyer.Rows[j]["Rsat_Miktar"]).ToString().PadRight(5, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(2, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                        if (Convert.ToString(dtAbuyer.Rows[j]["Rsat_Aciklama"]) != String.Empty) abuyer.Add(Convert.ToString(dtAbuyer.Rows[j]["Rsat_Aciklama"]));
                    }
                    abuyer.Add("".PadLeft(36, '-'));

                    abuyer.Add(".");
                    if (kartDetay1 != "") abuyer.Add(kartDetay1);
                    if (kartdetay2 != "") abuyer.Add(kartdetay2);

                    for (int j = 0; j < 5; j++)
                    {
                        abuyer.Add(".");
                    }

                    //siparis.Add(Convert.ToString((char) 27) + "@" + Convert.ToString((char) 29) + "V" + (char)1);

                    try
                    {
                        System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                        Font fnt = (Font)converter.ConvertFromString(Siparis_Font);

                        if (fnt == null)
                        {
                            fnt = new Font("Arial", 8);
                        }

                        Liste = abuyer;
                        printFont = fnt;
                        PrintDocument pd = new PrintDocument();
                        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                        pd.PrinterSettings.PrinterName = printer;
                        pd.Print();

                        abuyer.Clear();
                    }
                    catch (Exception err)
                    {
                        return err.Message;
                    }

                }
            }

            dbtools.execcmd(@"update Cst_Recete_Satis set Rsat_AbuyerPr = 1,Rsat_AbuyerPr2 = 1,Rsat_AbuyerPr3 = 1,Rsat_AbuyerPr4 = 1 where Rsat_Fisno = '" + Fisno + "'");

            return "OK";
        }
        public string AbuyerPr2(int Fisno, bool Mars, int Split, string baslik, string kartDetay1, string kartdetay2, bool hizliSatis)
        {
            List<string> abuyer = new List<string>();
            DataTable dtPrinter = AbuyerPrinterBul(Fisno, Split, true);


            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtPrinter.Rows[i]["Printer2"]);

                if (printer == "")
                {
                    continue;
                }

                DataTable dtAbuyer = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;

                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 9);
                com.Parameters.AddWithValue("@Printer", printer);
                com.Parameters.AddWithValue("@Mars", Mars);
                com.Parameters.AddWithValue("@Split", Split);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtAbuyer);

                if (con.State != ConnectionState.Closed) con.Close();

                if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtAbuyer.Rows[0]["Pkod_Printer"]);

                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                if (dtAbuyer.Rows.Count <= 0)
                {
                    continue;
                }

                abuyer.Add(".");
                //abuyer.Add("   * * * ABUYER FISI * * *  ");
                if (!hizliSatis)
                {
                    abuyer.Add(baslik);
                    abuyer.Add(" ");
                    abuyer.Add("Departman : " + Departman.Dep_Adi);
                    abuyer.Add(".");
                    abuyer.Add("#Masa :" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Masa"]).PadRight(10, ' ') + "  #Kisi :" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Kisi"]).PadRight(10, ' '));
                    abuyer.Add("#Konumu :" + (Convert.ToString(dtAbuyer.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                    abuyer.Add(".");
                    abuyer.Add("Tarih:" + Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                    abuyer.Add(".");
                    abuyer.Add("Cekno:" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                    abuyer.Add(".");
                    abuyer.Add("Grson:" + Convert.ToString(dtAbuyer.Rows[0]["Garson"]));//.PadRight(10, ' '));// + " Grsn2:" + Convert.ToString(dtSiparis.Rows[0]["Garson2"]).PadRight(5, ' '));
                    abuyer.Add(".");
                    abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                }
                else
                {
                    abuyer.Add(".");
                    abuyer.Add("   * * * SIPARIS FISI * * *  ");
                    abuyer.Add(".");
                    abuyer.Add("Departman : " + Departman.Dep_Adi);
                    abuyer.Add("Tarih:" + Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                    abuyer.Add("#Cekno:" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                    abuyer.Add(".");
                    abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                }
                for (int j = 0; j < dtAbuyer.Rows.Count; j++)
                {
                    abuyer.Add("".PadLeft(36, '-'));
                    if (Param.Param_SiparisSayi == false)
                    {
                        abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtAbuyer.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                    }
                    else
                    {
                        abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtAbuyer.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                    }
                    //abuyer.Add(" " + Convert.ToInt32(dtAbuyer.Rows[j]["Rsat_Miktar"]).ToString().PadRight(5, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(2, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                    if (Convert.ToString(dtAbuyer.Rows[j]["Rsat_Aciklama"]) != String.Empty) abuyer.Add(Convert.ToString(dtAbuyer.Rows[j]["Rsat_Aciklama"]));
                }
                abuyer.Add("".PadLeft(36, '-'));

                abuyer.Add(".");
                if (kartDetay1 != "") abuyer.Add(kartDetay1);
                if (kartdetay2 != "") abuyer.Add(kartdetay2);

                for (int j = 0; j < 5; j++)
                {
                    abuyer.Add(".");
                }

                //siparis.Add(Convert.ToString((char) 27) + "@" + Convert.ToString((char) 29) + "V" + (char)1);

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Siparis_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = abuyer;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    abuyer.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }


            }
            return "OK";
        }



        public string AbuyerPr3(int Fisno, bool Mars, int Split, string baslik, string kartDetay1, string kartdetay2, bool hizliSatis)
        {
            List<string> abuyer = new List<string>();
            DataTable dtPrinter = AbuyerPrinterBul(Fisno, Split, true);


            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtPrinter.Rows[i]["Printer3"]);

                if (printer == "")
                {
                    continue;
                }

                DataTable dtAbuyer = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;

                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 9);
                com.Parameters.AddWithValue("@Printer", printer);
                com.Parameters.AddWithValue("@Mars", Mars);
                com.Parameters.AddWithValue("@Split", Split);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtAbuyer);

                if (con.State != ConnectionState.Closed) con.Close();

                if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtAbuyer.Rows[0]["Pkod_Printer"]);

                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                if (dtAbuyer.Rows.Count <= 0)
                {
                    continue;
                }

                abuyer.Add(".");
                //abuyer.Add("   * * * ABUYER FISI * * *  ");
                if (!hizliSatis)
                {
                    abuyer.Add(baslik);
                    abuyer.Add(" ");
                    abuyer.Add("Departman : " + Departman.Dep_Adi);
                    abuyer.Add(".");
                    abuyer.Add("#Masa :" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Masa"]).PadRight(10, ' ') + "  #Kisi :" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Kisi"]).PadRight(10, ' '));
                    abuyer.Add("#Konumu :" + (Convert.ToString(dtAbuyer.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                    abuyer.Add(".");
                    abuyer.Add("Tarih:" + Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                    abuyer.Add(".");
                    abuyer.Add("Cekno:" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                    abuyer.Add(".");
                    abuyer.Add("Grson:" + Convert.ToString(dtAbuyer.Rows[0]["Garson"]));//.PadRight(10, ' '));// + " Grsn2:" + Convert.ToString(dtSiparis.Rows[0]["Garson2"]).PadRight(5, ' '));
                    abuyer.Add(".");
                    abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                }
                else
                {
                    abuyer.Add(".");
                    abuyer.Add("   * * * SIPARIS FISI * * *  ");
                    abuyer.Add(".");
                    abuyer.Add("Departman : " + Departman.Dep_Adi);
                    abuyer.Add("Tarih:" + Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                    abuyer.Add("#Cekno:" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                    abuyer.Add(".");
                    abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                }
                for (int j = 0; j < dtAbuyer.Rows.Count; j++)
                {
                    abuyer.Add("".PadLeft(36, '-'));
                    if (Param.Param_SiparisSayi == false)
                    {
                        abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtAbuyer.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                    }
                    else
                    {
                        abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtAbuyer.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                    }
                    //abuyer.Add(" " + Convert.ToInt32(dtAbuyer.Rows[j]["Rsat_Miktar"]).ToString().PadRight(5, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(2, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                    if (Convert.ToString(dtAbuyer.Rows[j]["Rsat_Aciklama"]) != String.Empty) abuyer.Add(Convert.ToString(dtAbuyer.Rows[j]["Rsat_Aciklama"]));
                }
                abuyer.Add("".PadLeft(36, '-'));

                abuyer.Add(".");
                if (kartDetay1 != "") abuyer.Add(kartDetay1);
                if (kartdetay2 != "") abuyer.Add(kartdetay2);

                for (int j = 0; j < 5; j++)
                {
                    abuyer.Add(".");
                }

                //siparis.Add(Convert.ToString((char) 27) + "@" + Convert.ToString((char) 29) + "V" + (char)1);

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Siparis_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = abuyer;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    abuyer.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }


            }
            return "OK";
        }
        public string AbuyerPr4(int Fisno, bool Mars, int Split, string baslik, string kartDetay1, string kartdetay2, bool hizliSatis)
        {
            List<string> abuyer = new List<string>();
            DataTable dtPrinter = AbuyerPrinterBul(Fisno, Split, true);


            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtPrinter.Rows[i]["Printer4"]);

                if (printer == "")
                {
                    continue;
                }

                DataTable dtAbuyer = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;

                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 9);
                com.Parameters.AddWithValue("@Printer", printer);
                com.Parameters.AddWithValue("@Mars", Mars);
                com.Parameters.AddWithValue("@Split", Split);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtAbuyer);

                if (con.State != ConnectionState.Closed) con.Close();

                if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtAbuyer.Rows[0]["Pkod_Printer"]);

                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                if (dtAbuyer.Rows.Count <= 0)
                {
                    continue;
                }

                abuyer.Add(".");
                //abuyer.Add("   * * * ABUYER FISI * * *  ");
                if (!hizliSatis)
                {
                    abuyer.Add(baslik);
                    abuyer.Add(" ");
                    abuyer.Add("Departman : " + Departman.Dep_Adi);
                    abuyer.Add(".");
                    abuyer.Add("#Masa :" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Masa"]).PadRight(10, ' ') + "  #Kisi :" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Kisi"]).PadRight(10, ' '));
                    abuyer.Add("#Konumu :" + (Convert.ToString(dtAbuyer.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                    abuyer.Add(".");
                    abuyer.Add("Tarih:" + Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                    abuyer.Add(".");
                    abuyer.Add("Cekno:" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                    abuyer.Add(".");
                    abuyer.Add("Grson:" + Convert.ToString(dtAbuyer.Rows[0]["Garson"]));//.PadRight(10, ' '));// + " Grsn2:" + Convert.ToString(dtSiparis.Rows[0]["Garson2"]).PadRight(5, ' '));
                    abuyer.Add(".");
                    abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                }
                else
                {
                    abuyer.Add(".");
                    abuyer.Add("   * * * SIPARIS FISI * * *  ");
                    abuyer.Add(".");
                    abuyer.Add("Departman : " + Departman.Dep_Adi);
                    abuyer.Add("Tarih:" + Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                    abuyer.Add("#Cekno:" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                    abuyer.Add(".");
                    abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
                }
                for (int j = 0; j < dtAbuyer.Rows.Count; j++)
                {
                    abuyer.Add("".PadLeft(36, '-'));
                    if (Param.Param_SiparisSayi == false)
                    {
                        abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtAbuyer.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                    }
                    else
                    {
                        abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtAbuyer.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                    }
                    //abuyer.Add(" " + Convert.ToInt32(dtAbuyer.Rows[j]["Rsat_Miktar"]).ToString().PadRight(5, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(2, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                    if (Convert.ToString(dtAbuyer.Rows[j]["Rsat_Aciklama"]) != String.Empty) abuyer.Add(Convert.ToString(dtAbuyer.Rows[j]["Rsat_Aciklama"]));
                }
                abuyer.Add("".PadLeft(36, '-'));

                abuyer.Add(".");
                if (kartDetay1 != "") abuyer.Add(kartDetay1);
                if (kartdetay2 != "") abuyer.Add(kartdetay2);

                for (int j = 0; j < 5; j++)
                {
                    abuyer.Add(".");
                }

                //siparis.Add(Convert.ToString((char) 27) + "@" + Convert.ToString((char) 29) + "V" + (char)1);

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Siparis_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = abuyer;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    abuyer.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }


            }
            return "OK";
        }
        public string SiparisNotPr(int Fisno, string notStr, string printer)
        {
            List<string> not = new List<string>();

            DataTable dtDetay = dbtools.SelectTable("select top 1 Rsat_Tarih,Rsat_Fisno,Rsat_Masa,Rsat_Garson from Cst_Recete_Satis WITH(NOLOCK) WHERE Rsat_Ba = 'B' and Rsat_Fisno = " + Fisno);

            not.Add("");
            not.Add("  * * * SIPARIS NOTU * * *  ");
            not.Add("");
            not.Add("#Departman : " + Departman.Dep_Adi);
            not.Add("Tarih:" + Convert.ToDateTime(dtDetay.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' '));
            not.Add("#Cekno:" + Convert.ToString(dtDetay.Rows[0]["Rsat_Fisno"]).PadRight(10, ' ') + " #Masa :" + Convert.ToString(dtDetay.Rows[0]["Rsat_Masa"]).PadRight(10, ' '));
            not.Add("Grson:" + User.Isim_Getir(Convert.ToString(dtDetay.Rows[0]["Rsat_Garson"])));
            not.Add("");

            string[] notArray = notStr.Split('\n');
            not.AddRange(notArray);


            not.Add(".");
            not.Add(".");
            not.Add("Gönderim Tarih:" + Convert.ToDateTime(dbtools.DegerGetir("select getdate()")).ToString("dd.MM.yyyy HH:ss:mm"));
            not.Add("Gönderen      :" + User.P_Ad + " " + User.P_Soyad);
            not.Add(".");
            not.Add(".");
            not.Add(".");


            try
            {
                System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                Font fnt = (Font)converter.ConvertFromString(Siparis_Font);

                if (fnt == null)
                {
                    fnt = new Font("Arial", 8);
                }

                Liste = not;
                printFont = fnt;
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.PrinterSettings.PrinterName = printer;
                pd.Print();

                not.Clear();
            }
            catch (Exception err)
            {
                return err.Message;
            }

            return "OK";
        }
        public string MarsPr(int Fisno, DataTable dataTable)
        {
            List<string> mars = new List<string>();

            DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'MARS'");
            if (dtDizayn.Rows.Count < 1)
            {
                return "Marş Dizaynı Yapılmamış...";
            }


            bool birkere = true;
            DataTable dtPrinter = SiparisPrinterBul(Fisno, 0, false);
            for (int i = 0; i < dtPrinter.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtPrinter.Rows[i]["Printer"]);
                int bosSatir = Convert.ToInt32(dtPrinter.Rows[i]["Pkod_Satir"]);

                if (printer == "") continue;

                DataTable dtMars = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 1);
                com.Parameters.AddWithValue("@Printer", printer);
                com.Parameters.AddWithValue("@Mars", 1);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dtMars);


                DataTable copyDataTable = dtMars.Clone();

                foreach (DataRow item in dtMars.Rows)
                {
                    bool varmi = false;
                    foreach (DataRow item2 in dataTable.Rows)
                    {
                        if (item["Rsat_Id"].ToString().Equals(item2["Rsat_Id"].ToString()))
                        {
                            varmi = true;
                        }
                    }

                    if (varmi == false)
                    {
                        copyDataTable.ImportRow(item);
                    }

                }

                dtMars = copyDataTable;


                // 31.01.2025 emre atalayın isteği üzerine eklendi
                try
                {
                    //string marslananlar = $"SELECT Rsat_Id FROM Cst_Recete_Satis where Rsat_Fisno={Fisno} and rezevePrintCiktimi=1 and Rsat_Mars is null";
                    //DataTable dtMarslananlar = dbtools.SelectTableR(marslananlar);
                    //HashSet<int> marslananIds = new HashSet<int>(dtMarslananlar.AsEnumerable()
                    //    .Select(row => row.Field<int>("Rsat_Id")));
                    //dtMars = dtMars.AsEnumerable()
                    //    .Where(row => !marslananIds.Contains(row.Field<int>("Rsat_Id")))
                    //    .CopyToDataTable();

                    string marslananlar = $"SELECT Rsat_Id FROM Cst_Recete_Satis where Rsat_Fisno={Fisno} and rezevePrintCiktimi=1 and Rsat_Mars is null";
                    DataTable dtMarslananlar = dbtools.SelectTableR(marslananlar);

                    HashSet<int> marslananIds = new HashSet<int>(
                        dtMarslananlar.AsEnumerable().Select(row => row.Field<int>("Rsat_Id"))
                    );

                    var filteredRows = dtMars.AsEnumerable()
                        .Where(row => !marslananIds.Contains(row.Field<int>("Rsat_Id")));

                    if (filteredRows.Any()) // Eğer en az bir satır varsa
                    {
                        dtMars = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        dtMars = dtMars.Clone(); // Boş ama yapısı korunan bir DataTable oluştur
                    }

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }

                if (dtMars == null || dtMars.Rows.Count == 0)
                {
                    return "OK";
                }




                if (con.State != ConnectionState.Closed) con.Close();

                if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtMars.Rows[0]["Pkod_Printer"]);

                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                for (int k = 0; k < Mars_Ciktisayisi; k++)
                {
                   
                    try
                    {
                        Mars marsReport = new Mars();


                       

                        xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), marsReport);
                        marsReport.PrinterName = printer;
                        marsReport.DataSource = dtMars;
                        marsReport.xr_MasaNo.Text = Convert.ToString(dtMars.Rows[0]["Rsat_Masa"]);
                        marsReport.xr_Konum.Text = Convert.ToString(dtMars.Rows[0]["MasaKonumAdi"]);
                        marsReport.xr_KisiSayisi.Text = Convert.ToString(dtMars.Rows[0]["Rsat_Kisi"]);
                        marsReport.xr_Tarih.Text = Convert.ToDateTime(dtMars.Rows[0]["Rsat_Tarih"]).ToShortDateString();
                        marsReport.xr_Acilis.Text = Convert.ToString(timeSpan);
                        marsReport.txtDepartman.Text = Departman.Dep_Adi;
                        marsReport.xr_Garson.Text = dtMars.Rows[0]["Garson"].ToString();
                        marsReport.xr_Cek.Text = Convert.ToString(dtMars.Rows[0]["Rsat_Fisno"]);
                        marsReport.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
                        marsReport.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));
                        //mars.txtSiraNo.Text = sirano;

                        if ( marsReport.PrinterName != "") //  marsReport.PrinterName != "Microsoft Print to PDF" &&
                        {
                            marsReport.Print();
                        }

                        // aşağıya dokunma
                        int sayi = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis where Rsat_Fisno = '" + Fisno + "' and ISNULL(Rsat_Mars,0) = 1 and ISNULL(Rsat_AbuyerPr,0) = 1"));

                        string baslik = "   * * * ABUYER FISI * * *   ";
                        if (sayi > 0) baslik = "   * * * ABUYER FISI - MARS * * *   ";

                        if (birkere)
                        {
                            birkere = false;
                            AbuyerPr(Fisno, true, 0, baslik, "", "", false);
                        }



                        dbtools.execcmdR("update Pos_Log set Log_Yazdirilmis='E'  where Log_FisNo='" + Fisno + "'");

                    }
                    catch (Exception err)
                    {
                        return err.Message;
                    }
                }

            }
            return "OK";
        }


        //public string MarsPr(int Fisno, DataTable dataTable)
        //{
        //    List<string> mars = new List<string>();

        //    bool birkere = true;
        //    DataTable dtPrinter = SiparisPrinterBul(Fisno, 0, false);
        //    for (int i = 0; i < dtPrinter.Rows.Count; i++)
        //    {
        //        string printer = Convert.ToString(dtPrinter.Rows[i]["Printer"]);
        //        int bosSatir = Convert.ToInt32(dtPrinter.Rows[i]["Pkod_Satir"]);

        //        if (printer == "") continue;

        //        DataTable dtMars = new DataTable();
        //        SqlConnection con = dbtools.conn;
        //        if (con.State == ConnectionState.Closed) con.Open();
        //        SqlCommand com = new SqlCommand();
        //        com.Connection = con;
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.CommandTimeout = 0;
        //        com.CommandText = "Pos_Satis";
        //        com.Parameters.AddWithValue("@Fisno", Fisno);
        //        com.Parameters.AddWithValue("@Rapor_Tipi", 1);
        //        com.Parameters.AddWithValue("@Printer", printer);
        //        com.Parameters.AddWithValue("@Mars", 1);
        //        SqlDataAdapter da = new SqlDataAdapter(com);
        //        da.Fill(dtMars);


        //        DataTable copyDataTable = dtMars.Clone();

        //        foreach (DataRow item in dtMars.Rows)
        //        {
        //            bool varmi = false;
        //            foreach (DataRow item2 in dataTable.Rows)
        //            {
        //                if (item["Rsat_Id"].ToString().Equals(item2["Rsat_Id"].ToString()))
        //                {
        //                    varmi = true;
        //                }
        //            }

        //            if (varmi == false)
        //            {
        //                copyDataTable.ImportRow(item);
        //            }

        //        }

        //        dtMars = copyDataTable;


        //        if (con.State != ConnectionState.Closed) con.Close();

        //        if (Param.Param_Printer_Tanim) printer = Convert.ToString(dtMars.Rows[0]["Pkod_Printer"]);

        //        TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        for (int k = 0; k < Mars_Ciktisayisi; k++)
        //        {
        //            mars.Add(".");
        //            mars.Add("   * * * MARS FISI * * *  ");
        //            mars.Add(".");
        //            mars.Add("#Cekno:" + Convert.ToString(dtMars.Rows[0]["Rsat_Fisno"]).PadRight(10, ' ') + " #Masa :" + Convert.ToString(dtMars.Rows[0]["Rsat_Masa"]).PadRight(7, ' '));
        //            mars.Add("Grson:" + Convert.ToString(dtMars.Rows[0]["Garson"]).PadRight(10, ' '));
        //            mars.Add("Tarih:" + Convert.ToDateTime(dbtools.DegerGetir("select getdate()")).ToString("dd.MM.yyyy HH:mm:ss"));
        //            mars.Add(".");
        //            mars.Add("MIKTAR" + "     " + "URUN".PadRight(15, ' '));
        //            for (int j = 0; j < dtMars.Rows.Count; j++)
        //            {
        //                mars.Add("#------------------------");
        //                if (Param.Param_SiparisSayi == false)
        //                {
        //                    mars.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtMars.Rows[j]["Rsat_Miktar"])).ToString().PadRight(5, ' ') + Convert.ToString(dtMars.Rows[j]["Rsat_Emiktar"]).PadRight(2, ' ') + Convert.ToString(dtMars.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
        //                }
        //                else
        //                {
        //                    mars.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtMars.Rows[j]["Rsat_Miktar"])).ToString().PadRight(5, ' ') + Convert.ToString(dtMars.Rows[j]["Rsat_Emiktar"]).PadRight(2, ' ') + Convert.ToString(dtMars.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
        //                }

        //                //mars.Add(" " + Convert.ToInt32(dtMars.Rows[j]["Rsat_Miktar"]).ToString().PadRight(3, ' ') + Convert.ToString(dtMars.Rows[j]["Rsat_Emiktar"]).PadRight(2, ' ') + Convert.ToString(dtMars.Rows[j]["Rec_Ad"]).PadRight(23, ' ').Substring(0, 23));
        //                if (Convert.ToString(dtMars.Rows[j]["Rsat_Aciklama"]) != String.Empty) mars.Add(Convert.ToString(dtMars.Rows[j]["Rsat_Aciklama"]));
        //            }
        //            mars.Add("#------------------------");

        //            for (int j = 0; j < bosSatir; j++)
        //            {
        //                mars.Add(".");
        //            }

        //            try
        //            {
        //                System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
        //                Font fnt = (Font)converter.ConvertFromString(Mars_Font);

        //                if (fnt == null)
        //                {
        //                    fnt = new Font("Arial", 8);
        //                }

        //                Liste = mars;
        //                printFont = fnt;
        //                PrintDocument pd = new PrintDocument();
        //                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
        //                pd.PrinterSettings.PrinterName = printer;
        //                pd.Print();

        //                if (Param.Param_Printer_Tanim && Convert.ToString(dtMars.Rows[0]["Pkod_Ek_Pr1"]) != "")
        //                {
        //                    PrintDocument pd1 = new PrintDocument();
        //                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
        //                    pd.PrinterSettings.PrinterName = Convert.ToString(dtMars.Rows[0]["Pkod_Ek_Pr1"]);
        //                    pd.Print();
        //                }
        //                if (Param.Param_Printer_Tanim && Convert.ToString(dtMars.Rows[0]["Pkod_Ek_Pr2"]) != "")
        //                {
        //                    PrintDocument pd2 = new PrintDocument();
        //                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
        //                    pd.PrinterSettings.PrinterName = Convert.ToString(dtMars.Rows[0]["Pkod_Ek_Pr2"]);
        //                    pd.Print();
        //                }
        //                if (Param.Param_Printer_Tanim && Convert.ToString(dtMars.Rows[0]["Pkod_Ek_Pr3"]) != "")
        //                {
        //                    PrintDocument pd3 = new PrintDocument();
        //                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
        //                    pd.PrinterSettings.PrinterName = Convert.ToString(dtMars.Rows[0]["Pkod_Ek_Pr3"]);
        //                    pd.Print();
        //                }

        //                mars.Clear();

        //                int sayi = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis where Rsat_Fisno = '" + Fisno + "' and ISNULL(Rsat_Mars,0) = 1 and ISNULL(Rsat_AbuyerPr,0) = 1"));

        //                string baslik = "   * * * ABUYER FISI * * *   ";
        //                if (sayi > 0) baslik = "   * * * ABUYER FISI - MARS * * *   ";

        //                if (birkere)
        //                {
        //                    birkere = false;
        //                    AbuyerPr(Fisno, true, 0, baslik, "", "", false);
        //                }



        //                dbtools.execcmdR("update Pos_Log set Log_Yazdirilmis='E'  where Log_FisNo='" + Fisno + "'");

        //            }
        //            catch (Exception err)
        //            {
        //                return err.Message;
        //            }
        //        }

        //    }
        //    return "OK";
        //}

        public string newUrunBazliHesapDokum(bool hesapDokum, int Fisno, int Split, string Baslik, string Recete)
        {
            string printer = String.Empty;
            int bosSatir = 0;
            string filter = "";
            string posta = Masa_Posta_bul(Fisno);
            decimal B = 0, A = 0, Bakiye = 0;
            if (Departman.Kodlar_Pr_Posta)
            {
                filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
            }
            else
            {
                filter = " and ISNULL(Pkod_Posta,'') = ''  ";
            }
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES' " + filter);
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }

            DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'HESAP'");
            if (dtDizayn.Rows.Count < 1)
            {
                return "Hesap Dizaynı Yapılmamış...";
            }

            DataSet ds = new DataSet();
            DataTable dtHesap = new DataTable();
            DataTable dtOdeme = new DataTable();
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Split", Split);
            com.Parameters.AddWithValue("@Rapor_Tipi", 7);
            com.Parameters.AddWithValue("@Recete", Recete);
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(ds);
            dtHesap = ds.Tables[0];
            dtOdeme = ds.Tables[1];
            if (con.State == ConnectionState.Open) con.Close();

            int dtSatirsay = dtHesap.Rows.Count;

            Print.Hesap hsp = new Print.Hesap();
            xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), hsp);
            hsp.PrinterName = printer;
            hsp.DataSource = dtHesap;

            DateTime sqlTarih = Convert.ToDateTime(dbtools.DegerGetir("select getdate()"));
            hsp.xr_OtelAdi.Text = Param.Tesis_Adi;
            hsp.xr_Baslik.Text = Baslik;
            hsp.xr_OtelAdi2.Text = Departman.Sube_Ad;
            hsp.xr_Departman.Text = Departman.Dep_Adi;
            hsp.xr_MasaNo.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Masa"]);
            hsp.xr_Konum.Text = Convert.ToString(dtHesap.Rows[0]["MasaKonumAdi"]);
            hsp.xr_KisiSayisi.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Kisi"]);
            hsp.xr_Kuver.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Kuver"]);
            hsp.xr_Tarih.Text = Convert.ToDateTime(dtHesap.Rows[0]["Rsat_Tarih"]).ToShortDateString();
            hsp.xr_Acilis.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Acilis"]);
            hsp.xr_Kapanis.Text = sqlTarih.TimeOfDay.ToString();
            hsp.xr_Cek.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Fisno"]);
            hsp.xr_Odano.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Odano"]);
            hsp.xr_Rez.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]);
            if (Param.Tesis_Tipi == 0)
            {
                hsp.xr_Kart.Text = Fronttools.KartNo(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]));
                hsp.xr_GC.Text = Fronttools.GirisCikisTarih(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]));
            }
            hsp.xr_Kasiyer.Text = Convert.ToString(dtHesap.Rows[0]["Kasiyer"]);
            hsp.xr_Adisyon.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Adisyon"]);


            hsp.xr_Urun.Text = "[Rec_Ad]".ToString();
            hsp.xr_Miktar.Text = String.Format("{0:0.##}", ("[Rsat_Miktar]"));
            hsp.xr_Tutar.Text = String.Format("{0:0.00}", ("[Rsat_Tutar]"));

            hsp.xr_Emiktar.Text = "[Rsat_Emiktar]";

            decimal UrunToplam = 0;
            for (int i = 0; i < dtHesap.Rows.Count; i++)
            {
                if (dtHesap.Rows[i]["Rsat_Emiktar"].Equals("B"))
                {
                    dtHesap.Rows[i]["Rsat_Emiktar"] = "1BCK";
                }
                else if (dtHesap.Rows[i]["Rsat_Emiktar"].Equals("D"))
                {
                    dtHesap.Rows[i]["Rsat_Emiktar"] = "DBL";
                }
                else if (dtHesap.Rows[i]["Rsat_Emiktar"].Equals("Y"))
                {
                    dtHesap.Rows[i]["Rsat_Emiktar"] = "YRM";
                }

                UrunToplam += Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]);
            }
            B = UrunToplam;

            hsp.xr_UrunToplam.Text = UrunToplam.ToString("n2");
            hsp.xr_KalanToplam.Text = UrunToplam.ToString("n2");

            decimal odemeToplam = 0;
            if (dtOdeme.Rows.Count > 0)
            {
                //Odeme Tablosu
                for (int i = 0; i < dtOdeme.Rows.Count; i++)
                {
                    XRTableRow row = new XRTableRow();

                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = Convert.ToString(dtOdeme.Rows[i]["Rec_Ad"]);
                    cell1.WidthF = hsp.table_Odeme.WidthF * 30 / 100;
                    cell1.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                    row.Cells.Add(cell1);

                    XRTableCell cell2 = new XRTableCell();
                    cell2.Text = Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]).ToString("n2");
                    cell2.WidthF = hsp.table_Odeme.WidthF * 20 / 100;
                    cell2.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                    row.Cells.Add(cell2);

                    //if (Param.Tesis_Tipi == 0)
                    //{
                    //    XRTableCell cell3 = new XRTableCell();
                    //    cell3.Text = Fronttools.IsimSoyisim(Convert.ToInt32(dtOdeme.Rows[i]["Rsat_Folio"]));
                    //    cell3.WidthF = hsp.table_Odeme.WidthF * 30 / 100;
                    //    row.Cells.Add(cell3);

                    //    XRTableCell cell4 = new XRTableCell();
                    //    cell4.Text = Convert.ToInt32(dtOdeme.Rows[i]["Rsat_Folio"]).ToString();
                    //    cell4.WidthF = hsp.table_Odeme.WidthF * 25 / 100;
                    //    row.Cells.Add(cell4);
                    //}
                    odemeToplam += Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]);

                    hsp.table_Odeme.Rows.Add(row);
                }
            }
            A = odemeToplam;

            hsp.xr_KalanToplam.Text = (Convert.ToDecimal(hsp.xr_UrunToplam.Text) - odemeToplam).ToString("n2");
            hsp.xr_Not.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Not"]);

            Bakiye = B - A;
            Bakiye = Bakiye / Param.Doviz_Kuru;
            B = B / Param.Doviz_Kuru;
            Bakiye = Bakiye < Convert.ToDecimal(0.05) ? 0 : Bakiye;

            if (Param.Param_Hesap_DovizOzet)
            {
                DataTable dtDovizDagilim = new DataTable();


                decimal dagilimTutar = Param.Param_Hesap_DovizOzetToplam == true ? B : Bakiye;

                if (Param.Kurlar_Nerden == 0)
                {
                    if (Param.Tesis_Tipi == 0)
                    {
                        dtDovizDagilim = Fis_Islem.Doviz_DagilimFront(dagilimTutar);
                    }
                }
                else
                {
                    dtDovizDagilim = Fis_Islem.Doviz_Dagilim(dagilimTutar);
                }

                if (dtDovizDagilim.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDovizDagilim.Rows.Count; i++)
                    {
                        XRTableRow row = new XRTableRow();

                        XRTableCell cell1 = new XRTableCell();
                        cell1.Text = Convert.ToString(dtDovizDagilim.Rows[i]["Mkodlar_Ad"]);
                        cell1.WidthF = hsp.table_Doviz.WidthF * 30 / 100;
                        cell1.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                        row.Cells.Add(cell1);

                        XRTableCell cell2 = new XRTableCell();
                        cell2.Text = Convert.ToDecimal(dtDovizDagilim.Rows[i]["Doviz"]).ToString("n2");
                        cell2.WidthF = hsp.table_Doviz.WidthF * 20 / 100;
                        cell2.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                        row.Cells.Add(cell2);


                        hsp.table_Doviz.Rows.Add(row);
                    }
                }
            }

            DataTable dtUrungrup = new DataTable();
            dtUrungrup = UrunGrupBul(Fisno);
            for (int j = 0; j < dtUrungrup.Rows.Count; j++)
            {
                XRTableRow row = new XRTableRow();

                XRTableCell cell1 = new XRTableCell();
                cell1.Text = Convert.ToString(dtUrungrup.Rows[j]["Kodlar_Ad"]);
                cell1.WidthF = hsp.table_UrunGrup.WidthF * 60 / 100;
                cell1.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                row.Cells.Add(cell1);

                XRTableCell cell2 = new XRTableCell();
                cell2.Text = Convert.ToDecimal(dtUrungrup.Rows[j]["Tutar"]).ToString("n2");
                cell2.WidthF = hsp.table_UrunGrup.WidthF * 40 / 100;
                cell2.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                row.Cells.Add(cell2);


                hsp.table_UrunGrup.Rows.Add(row);
            }


            if (Convert.ToString(dtHesap.Rows[0]["Rsat_MusTipi"]) != "U" && Param.Tesis_Tipi == 0)
            {
                if (Param.Fiste_Balance == 0)
                {
                    //int folio = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]) == "" ? 0 : Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]);

                    //hsp.Add("Bakiye :" + Fronttools.BalanceBul(folio, Kart_No).ToString("N2"));

                    //hsp.Add("");
                }


                if (Param.Tesis_Tipi == 0)
                {
                    hsp.xr_FolioAdSoyad.Text = (Fronttools.IsimSoyisim(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"])));
                }
            }
            else
            {
                hsp.xr_FolioAdSoyad.Text = (Convert.ToString(dtHesap.Rows[0]["Rsat_Uye_Ad"]));
            }


            bakiyeYaz(hsp, dtHesap);


            hsp.Print();


            return "OK";
        }

        public void bakiyeYaz(Print.Hesap hsp, DataTable dtHesap)
        {
            if (Param.Tesis_Tipi == 0)
            {
                try
                {
                    hsp.xr_FolioAdSoyad.Text = (Fronttools.IsimSoyisim(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"])));

                    if (Departman.Kodlar_AndPos_NFC == true)
                    {
                        hsp.xr_FolioAdSoyad.Text = (Fronttools.CardFIsim(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));

                        hsp.xr_Bakiye.Text = (Fronttools.NFCBakiye(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]), Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"]))).ToString();
                    }

                    string kalanBakiye = "";
                    if (MasaTakip.hes != null)
                    {
                        kalanBakiye = MasaTakip.hes.lbl_Bilgi.Text;
                    }
                    int index = kalanBakiye.LastIndexOf("=");
                    if (index == -1)
                    {
                        hsp.xr_Bakiye.Text = "";
                        hsp.xrLabel42.Visible = false;
                        hsp.xrLabel43.Visible = false;
                    }
                    else
                    {
                        hsp.xr_Bakiye.Text = kalanBakiye.Substring(index, kalanBakiye.Length - index).Replace("=", "");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("HATA ! " + ex.Message);
                }


            }
        }


     
        public string newHesapDokum(bool hesapDokum, int Fisno, int Split, string Baslik, bool sifirli = false, bool parcalimi = false, string parcamasano = "")
        {
            try
            {
                string sirano = StatikSinif.getSira(Fisno.ToString());
                string printer = String.Empty;
                int bosSatir = 0;
                string filter = "";
                string posta = Masa_Posta_bul(Fisno);
                decimal B = 0, A = 0, Bakiye = 0;
                if (Departman.Kodlar_Pr_Posta)
                {
                    filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
                }
                else
                {
                    filter = " and ISNULL(Pkod_Posta,'') = ''  ";
                }

                string query = "select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES' " + filter;
                DataTable dtPrinter = dbtools.SelectTable(query);

                if (dtPrinter.Rows.Count == 0)
                {
                    query = "select distinct Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES' ";

                    dtPrinter = dbtools.SelectTable(query);
                }




                if (dtPrinter.Rows.Count > 0)
                {
                    printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                    bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
                }

                DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
                if (dtMacPrinter.Rows.Count > 0)
                {
                    printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
                }

                DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'HESAP'");
                if (dtDizayn.Rows.Count < 1)
                {
                    return "Hesap Dizaynı Yapılmamış...";
                }

                DataSet ds = new DataSet();
                DataTable dtHesap = new DataTable();
                DataTable dtOdeme = new DataTable();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Split", Split);
                com.Parameters.AddWithValue("@parcalimasano", parcamasano);
                com.Parameters.AddWithValue("@Rapor_Tipi", parcalimi == true ? 30 : 7);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);

                dtHesap = ds.Tables[0];
                dtOdeme = ds.Tables[1];
                if (con.State == ConnectionState.Open) con.Close();

                int dtSatirsay = dtHesap.Rows.Count;




                Print.Hesap hsp = new Print.Hesap();


                if (kisiyeSatis)
                {
                    dtHesap = dtHesap.Select("kisiyeSatisAdSoyad='" + kisiyeSatisAdSoyad + "'").CopyToDataTable();
                }

                xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), hsp);

                if (Param.hesapFisQr)
                {
                    string gunsonutar = Param.Tarih.ToString("yyyy-MM-dd");
                    hsp.txtQr.Text = gunsonutar.Replace("-", "");
                    hsp.txtQr.Visible = true;
                    hsp.ReportFooter.HeightF = (float)388.3324;
                    hsp.txtQr.SizeF = new SizeF((float)139.29, (float)125.98);
                }else if (Param.hesapFisQrFisno)
                {
                    hsp.txtQr.Text = Fisno+"";
                    hsp.txtQr.Visible = true;
                    hsp.ReportFooter.HeightF = (float)388.3324;
                    hsp.txtQr.SizeF = new SizeF((float)139.29, (float)125.98);
                }


                hsp.PrinterName = printer;
                hsp.DataSource = dtHesap;

                DateTime sqlTarih = Convert.ToDateTime(dbtools.DegerGetir("select getdate()"));
                hsp.xr_OtelAdi.Text = Param.Tesis_Adi;
                hsp.xr_Baslik.Text = Baslik;
                hsp.xr_OtelAdi2.Text = Departman.Sube_Ad;
                hsp.xr_Departman.Text = Departman.Dep_Adi;
                hsp.txtSiraNo.Text = sirano;

                string masaAd = Convert.ToString(dtHesap.Rows[0]["MasaAdi"]);
                if (masaAd.Contains(" -"))
                {
                    string bol1 = masaAd.Split(new string[] { " -" }, StringSplitOptions.None)[0].Trim();
                    string bol2 = masaAd.Split(new string[] { " -" }, StringSplitOptions.None)[1].Trim();

                    if (bol1 == bol2)
                    {
                        masaAd = masaAd.Replace("- " + bol1, "").Trim();
                    }
                }

                if (masaAd.Trim().Equals(""))
                {
                    masaAd = Convert.ToString(dtHesap.Rows[0]["Rsat_Masa"]);
                }


                hsp.txtToplamIkram.Text = dbtools.DegerGetir("select sum(Rsat_Fiyat) as Rsat_Fiyat from Cst_Recete_Satis where  Rsat_Ikram='1' and Rsat_Fisno='" + Fisno + "'");


                if (kisiyeSatis)
                {
                    masaAd = masaAd + "[" + kisiyeSatisAdSoyad + "]";
                }

                hsp.xr_MasaNo.Text = masaAd;



                hsp.xr_Konum.Text = Convert.ToString(dtHesap.Rows[0]["MasaKonumAdi"]);
                hsp.xr_KisiSayisi.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Kisi"]);
                hsp.xr_Kuver.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Kuver"]);
                hsp.xr_Tarih.Text = Convert.ToDateTime(dtHesap.Rows[0]["Rsat_Tarih"]).ToShortDateString();
                hsp.xr_Acilis.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Acilis"]);
                hsp.xr_Kapanis.Text = sqlTarih.TimeOfDay.ToString();
                hsp.xr_Cek.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Fisno"]);
                hsp.xr_Odano.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Odano"]);
                hsp.xr_Rez.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]);
                if (Param.Tesis_Tipi == 0)
                {
                    hsp.xr_Kart.Text = "";
                    hsp.xr_GC.Text = "";
                    try
                    {
                        hsp.xr_Kart.Text = Fronttools.KartNo(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]));
                        hsp.xr_GC.Text = Fronttools.GirisCikisTarih(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]));
                    }
                    catch (Exception ex)
                    {

                    }

                }
                hsp.xr_Kasiyer.Text = Convert.ToString(dtHesap.Rows[0]["Kasiyer"]);
                hsp.xr_Adisyon.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Adisyon"]);


                if (Param.Param_HesapDkmAciklama)
                {
                    hsp.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));
                }
                else
                {
                    hsp.xr_Urun.Text = "[Rec_Ad]";
                }

                hsp.xr_Miktar.Text = String.Format("{0:0.##}", ("[Rsat_Miktar]"));
                //hsp.xr_Miktar.Text = String.Format("{0:0.##}", ("[Rsat_Miktar]"));
                //hsp.xr_Miktar.Text = hsp.xr_Miktar.Text.Replace(",0000", "");

                hsp.xr_Emiktar.Text = "[Rsat_Emiktar]";

                string dovizIcon = "";
                switch (Param.Doviz_Adi1.ToUpper())
                {
                    case "EURO":
                        dovizIcon = " €";
                        break;
                    case "EUR":
                        dovizIcon = " €";
                        break;
                    case "GBP":
                        dovizIcon = " £";
                        break;
                    case "RUBLE":
                        dovizIcon = " ₽";
                        break;
                    case "USD":
                        dovizIcon = " $";
                        break;
                    case "DOLAR":
                        dovizIcon = " $";
                        break;
                    case "POUND":
                        dovizIcon = " £";
                        break;
                    default:
                        dovizIcon = " ₺";
                        break;
                }

                if (Param.Fis_Dovizli == 0)
                {
                    hsp.xr_Tutar.Text = String.Format("{0:0.00}", ("[Rsat_Tutar]"));
                }
                else
                {
                    hsp.xr_Tutar.Text = String.Format("{0:0.00}", ("[Rsat_Doviztutar]"));
                }


                hsp.xr_Tutar.Text += dovizIcon;


                if (Param.Param_Hsifir_Ikram)
                {
                    dtHesap.ConvertColumnType("Rsat_Tutar", typeof(string));
                }


                decimal UrunToplam = 0, UrunToplamTr = 0;
                for (int i = 0; i < dtHesap.Rows.Count; i++)
                {

                    decimal tutar = Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"].ToString());
                    decimal dovizTutar = Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Doviztutar"].ToString());

                    if (Param.Param_Hsifir_Ikram && tutar == 0)
                    {
                        dtHesap.Rows[i]["Rsat_Tutar"] = "*IKRAM*";
                    }

                    string zayimi = dbtools.DegerGetir("select ISNULL(Rsat_Zayi,0) as Rsat_Zayi  from Cst_Recete_Satis where Rsat_Id='" + dtHesap.Rows[i]["Rsat_Id"] + "'");

                    if (zayimi.Equals("True"))
                    {
                        dtHesap.Rows[i]["Rsat_Tutar"] = "*ZAYİ*";
                    }

                    if (dtHesap.Rows[i]["Rsat_Emiktar"].Equals("B"))
                    {
                        dtHesap.Rows[i]["Rsat_Emiktar"] = "1BCK";
                    }
                    else if (dtHesap.Rows[i]["Rsat_Emiktar"].Equals("D"))
                    {
                        dtHesap.Rows[i]["Rsat_Emiktar"] = "DBL";
                    }
                    else if (dtHesap.Rows[i]["Rsat_Emiktar"].Equals("Y"))
                    {
                        dtHesap.Rows[i]["Rsat_Emiktar"] = "YRM";
                    }


                    if (Param.Fis_Dovizli == 0)
                    {
                        UrunToplam += tutar;
                    }
                    else
                    {
                        UrunToplam += dovizTutar;
                    }

                    B += tutar;
                    UrunToplamTr += tutar;
                }


                decimal odemeToplam = 0, odemeToplamTr = 0;
                if (dtOdeme.Rows.Count > 0)
                {
                    //Odeme Tablosu
                    for (int i = 0; i < dtOdeme.Rows.Count; i++)
                    {
                        XRTableRow row = new XRTableRow();

                        XRTableCell cell1 = new XRTableCell();
                        cell1.Text = Convert.ToString(dtOdeme.Rows[i]["Rec_Ad"]);
                        cell1.WidthF = hsp.table_Odeme.WidthF * 30 / 100;
                        cell1.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                        row.Cells.Add(cell1);

                        XRTableCell cell2 = new XRTableCell();
                        if (Param.Fis_Dovizli == 0)
                        {
                            cell2.Text = Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]).ToString("n2");

                            if (sifirli)
                            {
                                cell2.Text = "0";
                            }
                        }
                        else
                        {
                            cell2.Text = Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Doviztutar"]).ToString("n2");
                            if (sifirli)
                            {
                                cell2.Text = "0";
                            }
                        }
                        if (Param.Fis_Dovizli != 0)
                        {
                            switch (dtOdeme.Rows[i]["DovizAdi"].ToString().ToUpper())
                            {
                                case "EURO":
                                    dovizIcon = " €";
                                    break;
                                case "EUR":
                                    dovizIcon = " €";
                                    break;
                                case "GBP":
                                    dovizIcon = " £";
                                    break;
                                case "RUBLE":
                                    dovizIcon = " ₽";
                                    break;
                                case "USD":
                                    dovizIcon = " $";
                                    break;
                                case "DOLAR":
                                    dovizIcon = " $";
                                    break;
                                default:
                                    //dovizIcon = " ₺";
                                    break;
                            }
                        }
                        cell2.Text += dovizIcon;


                        cell2.WidthF = hsp.table_Odeme.WidthF * 20 / 100;
                        cell2.Font = new System.Drawing.Font("Calibri", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                        cell2.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
                        row.Cells.Add(cell2);

                        //if (Param.Tesis_Tipi == 0)
                        //{
                        //    XRTableCell cell3 = new XRTableCell();
                        //    cell3.Text = Fronttools.IsimSoyisim(Convert.ToInt32(dtOdeme.Rows[i]["Rsat_Folio"]));
                        //    cell3.WidthF = hsp.table_Odeme.WidthF * 30 / 100;
                        //    row.Cells.Add(cell3);

                        //    XRTableCell cell4 = new XRTableCell();
                        //    cell4.Text = Convert.ToInt32(dtOdeme.Rows[i]["Rsat_Folio"]).ToString();
                        //    cell4.WidthF = hsp.table_Odeme.WidthF * 25 / 100;
                        //    row.Cells.Add(cell4);
                        //}
                        if (Param.Fis_Dovizli == 0)
                        {
                            odemeToplam += Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]);
                        }
                        else
                        {
                            odemeToplam += Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Doviztutar"]);
                        }

                        odemeToplamTr += Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]);

                        hsp.table_Odeme.Rows.Add(row);

                        A = Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]);
                    }
                }



                hsp.xr_UrunToplam.Text = UrunToplam.ToString("n2");
                hsp.xr_UrunToplamTr.Text = UrunToplamTr.ToString("n2");

                hsp.xr_KalanToplam.Text = UrunToplam.ToString("n2");
                hsp.xr_KalanToplamTr.Text = UrunToplamTr.ToString("n2");

                if (dtOdeme.Rows.Count > 0)
                {
                    hsp.xr_KalanToplam.Text = (UrunToplam - odemeToplam).ToString("n2");
                    hsp.xr_KalanToplamTr.Text = (UrunToplamTr - odemeToplamTr).ToString("n2");
                }

                // buraya yaz


                hsp.xr_Not.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Not"]); // new hesapdöküm

                Bakiye = B - A;
                Bakiye = Bakiye / Param.Doviz_Kuru;
                B = B / Param.Doviz_Kuru;
                Bakiye = Bakiye < Convert.ToDecimal(0.05) ? 0 : Bakiye;

                if (Param.Param_Hesap_DovizOzet)
                {
                    DataTable dtDovizDagilim = new DataTable();

                    decimal dagilimTutar = Param.Param_Hesap_DovizOzetToplam == true ? Bakiye : B; // ramo yaptı
                                                                                                   //decimal dagilimTutar = Param.Param_Hesap_DovizOzetToplam == true ? B : Bakiye; 

                    if (Param.Fis_Dovizli == 1)
                    {
                        dagilimTutar = Convert.ToDecimal(hsp.xr_KalanToplam.Text); // 07.05.2024 tarihinde ramo ekledi
                    }
                    else
                    {
                        dagilimTutar = Convert.ToDecimal(hsp.xr_KalanToplamTr.Text); // 07.05.2024 tarihinde ramo ekledi
                    }
                    if (Param.Kurlar_Nerden == 0)
                    {
                        dtDovizDagilim = Fis_Islem.Doviz_DagilimFront(dagilimTutar);
                    }
                    else
                    {
                        if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1) // tesis 1 ise posdur .. 0 ise önbüro
                        {

                            dagilimTutar = Convert.ToDecimal(hsp.xr_UrunToplam.Text);
                            dtDovizDagilim = Fis_Islem.Doviz_Dagilim(dagilimTutar);
                        }
                        else
                        {
                            dtDovizDagilim = Fis_Islem.Doviz_Dagilim(dagilimTutar);
                        }
                    }
                    if (dtDovizDagilim.Rows.Count > 0 && kisiyeSatis == false)
                    {
                        for (int i = 0; i < dtDovizDagilim.Rows.Count; i++)
                        {
                            XRTableRow row = new XRTableRow();

                            XRTableCell cell1 = new XRTableCell();
                            cell1.Text = Convert.ToString(dtDovizDagilim.Rows[i]["Mkodlar_Ad"]);
                            cell1.WidthF = hsp.table_Doviz.WidthF * 30 / 100;
                            cell1.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                            row.Cells.Add(cell1);

                            XRTableCell cell2 = new XRTableCell();

                            cell2.Text = Convert.ToDecimal(dtDovizDagilim.Rows[i]["Doviz"]).ToString("n2");
                            cell2.WidthF = hsp.table_Doviz.WidthF * 20 / 100;
                            cell2.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                            cell2.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
                            row.Cells.Add(cell2);


                            hsp.table_Doviz.Rows.Add(row);
                        }
                    }
                }

                hsp.xr_KalanToplam.Text += dovizIcon;
                hsp.xr_UrunToplam.Text += dovizIcon;


                hsp.xr_UrunToplamTr.Text += " ₺";
                hsp.xr_KalanToplamTr.Text += " ₺";




                DataTable dtUrungrup = new DataTable();
                dtUrungrup = UrunGrupBul(Fisno);
                if (kisiyeSatis == false)
                    for (int j = 0; j < dtUrungrup.Rows.Count; j++)
                    {
                        XRTableRow row = new XRTableRow();

                        XRTableCell cell1 = new XRTableCell();
                        cell1.Text = Convert.ToString(dtUrungrup.Rows[j]["Kodlar_Ad"]);
                        cell1.WidthF = hsp.table_UrunGrup.WidthF * 60 / 100;
                        cell1.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                        row.Cells.Add(cell1);

                        XRTableCell cell2 = new XRTableCell();
                        if (Param.Fis_Dovizli == 0)
                        {
                            cell2.Text = Convert.ToDecimal(dtUrungrup.Rows[j]["Tutar"]).ToString("n2");
                        }
                        else
                        {
                            cell2.Text = Convert.ToDecimal(dtUrungrup.Rows[j]["Doviztutar"]).ToString("n2");
                        }

                        cell2.WidthF = hsp.table_UrunGrup.WidthF * 40 / 100;
                        cell2.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                        cell2.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
                        row.Cells.Add(cell2);


                        hsp.table_UrunGrup.Rows.Add(row);

                    }


                if (Convert.ToString(dtHesap.Rows[0]["Rsat_MusTipi"]) != "U" && Param.Tesis_Tipi == 0)
                {
                    if (Param.Fiste_Balance == 0)
                    {
                        //int folio = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]) == "" ? 0 : Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]);

                        //hsp.Add("Bakiye :" + Fronttools.BalanceBul(folio, Kart_No).ToString("N2"));

                        //hsp.Add("");
                    }


                    bakiyeYaz(hsp, dtHesap);

                }
                else
                {
                    hsp.xr_FolioAdSoyad.Text = (Convert.ToString(dtHesap.Rows[0]["Rsat_Uye_Ad"]));
                }

                if (hsp.xr_FolioAdSoyad.Text.Equals("") && dtOdeme.Rows.Count > 0)
                {
                    string text = dtOdeme.Rows[0]["Cari"].ToString();
                    hsp.xr_FolioAdSoyad.Text = text;

                    if (text.Contains(" "))
                    {
                        hsp.xr_FolioAdSoyad.Text = text.Substring(text.IndexOf(" ")).Trim();
                    }
                }

                //if (Departman.Kodlar_AndPos_NFC == true)
                //{
                //    hsp.xr_FolioAdSoyad.Text = (Fronttools.CardFIsim(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));

                //    hsp.xr_Bakiye.Text = (Fronttools.NFCBakiye(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]), Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"]))).ToString();
                //}


                if (sifirli) // hesap döküm tutarları sıfırlar. mehmet şahin istedi
                {
                    hsp.xr_UrunToplam.Text = "0";
                    hsp.xr_UrunToplamTr.Text = "0";
                    hsp.xr_KalanToplam.Text = "0";
                    hsp.xr_KalanToplamTr.Text = "0";
                    hsp.txtToplamIkram.Text = "0";
                    hsp.txtToplamIkram.Text = "0";
                    hsp.xr_Tutar.Text = "0";
                    SetTextWatermark(hsp);

                }

                string yazici = dbtools.DegerGetir("select top 1 isnull(hesapyazici,0) as hesapyazici from Rmosmuh.dbo.Pos_User_XZ where P_Kod='" + User.P_Kod + "'");

                if (Param.servispayFooterda)
                {
                    servisfarklisatirda(dtHesap, hsp, dovizIcon);
                }

                try
                {
                    string toplamtutarim = hsp.xr_KalanToplamTr.Text.Replace(" ₺", "");
                    decimal toplamTutar = Convert.ToDecimal(toplamtutarim);
                    hsp.txt2kisi.Text = (toplamTutar / 2).ToString("N2");
                    hsp.txt3kisi.Text = (toplamTutar / 3).ToString("N2");
                    hsp.txt4kisi.Text = (toplamTutar / 4).ToString("N2");
                    hsp.txt5kisi.Text = (toplamTutar / 5).ToString("N2");
                }
                catch (Exception ex)
                {

                }


                try
                {
                    string cariKod = dtHesap.Rows[0]["Rsat_Cari"].ToString();
                    string bakiye = hsp.xr_Bakiye.Text;
                    if (bakiye== "Bakiye")
                    {
                        var cari1 = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 23, @Cari = '" + cariKod + "' ");

                        decimal toplamBorc = 0;
                        decimal toplamAlacak = 0;
                        decimal bakiye1 = 0;

                        // Her satırı döngü ile işleyelim
                        foreach (DataRow row in cari1.Rows)
                        {
                            // Borç ve alacak sütunlarını doğru isimlendirmeyi unutmayın
                            decimal borc = Convert.ToDecimal(row["Chrk_Borc"]);
                            decimal alacak = Convert.ToDecimal(row["Chrk_Alacak"]);

                            toplamBorc += borc;
                            toplamAlacak += alacak;
                        }

                        bakiye1 = toplamBorc - toplamAlacak;
                        if (bakiye1 != 0)
                        {
                            hsp.xr_Bakiye.Text ="[" + bakiye1 + "]";
                        }

                    }
                }
                catch (Exception ex)
                {

                }
               

                if (dtPrinter.Rows.Count > 0 && dtMacPrinter.Rows.Count == 0)
                {
                    for (int i = 0; i < dtPrinter.Rows.Count; i++)
                    {
                        if (Convert.ToString(dtPrinter.Rows[i]["Pkod_Ad"]) == "")
                        {
                            continue;
                        }

                        printer = Convert.ToString(dtPrinter.Rows[i]["Pkod_Ad"]);
                        hsp.PrinterName = printer;

                        if (yazici.Length > 2)
                        {
                            hsp.PrinterName = yazici;
                        }


                        for (int k = 0; k < Hesap_Ciktisayisi; k++)
                        {
                            hsp.Print();
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < dtMacPrinter.Rows.Count; i++)
                    {

                        if (Convert.ToString(dtMacPrinter.Rows[i]["Pkod_Ad"]) == "")
                        {
                            continue;
                        }
                        printer = Convert.ToString(dtMacPrinter.Rows[i]["Pkod_Ad"]);
                        hsp.PrinterName = printer;

                        if (yazici.Length > 2)
                        {
                            hsp.PrinterName = yazici;
                        }

                        for (int k = 0; k < Hesap_Ciktisayisi; k++)
                        {
                            hsp.Print();
                        }

                    }
                }



                dbtools.execcmdR("update Pos_Log set Log_HesapDokumu='E',Log_Yazdirilmis='E'  where Log_FisNo='" + Fisno + "'");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return "OK";
        }

        public void servisfarklisatirda(DataTable dtHesap, Print.Hesap hsp, string dovizIcon)
        {
            // servis payını ayrı yazma 15.04.2024
            decimal servisToplam = 0;
            try
            {
                string queryServis = $"select top 1 isnull(Kodlar_Servis_Recete,'') as servisPayiRecKod from Stok_Kodlar where Kodlar_Sinif = '01' and Kodlar_Kod = '{Departman.Dep_Kodu}'";

                string serviskod = dbtools.DegerGetir(queryServis);

                if (serviskod != "")
                {
                    DataRow[] rowsToDelete = dtHesap.Select("Rec_Genelkod = '" + serviskod + "'");

                    // Her bir bulunan satırı sil
                    if (rowsToDelete != null && rowsToDelete.Length > 0)
                    {
                        servisToplam = Convert.ToDecimal(rowsToDelete[0]["Rsat_Tutar"].ToString());

                        foreach (DataRow row in rowsToDelete)
                        {
                            dtHesap.Rows.Remove(row);
                        }

                        // Değişiklikleri uygula
                        dtHesap.AcceptChanges();
                    }

                }

                hsp.txtServiceToplam.Text = servisToplam + dovizIcon;
            }
            catch (Exception ex)
            {
                MessageBox.Show("servisfarklisatirda() Hata 125 !\n " + ex.Message);
            }
        }

        public void SetTextWatermark(XtraReport report)
        {
            Watermark textWatermark = new Watermark();
            textWatermark.Text = "NO PAYMENT";
            textWatermark.TextDirection = DirectionMode.ForwardDiagonal;
            textWatermark.Font = new Font(textWatermark.Font.Name, 40, FontStyle.Bold);
            textWatermark.ForeColor = Color.Black;
            textWatermark.TextTransparency = 150;
            textWatermark.ShowBehind = false;
            textWatermark.PageRange = "1,3-5";
            report.Watermark.CopyFrom(textWatermark);
        }

        public decimal getKurKarsilik(string dovizKodu, GridView gridView2)
        {
            try
            {
                for (int i = 0; i < gridView2.RowCount; i++)
                {
                    if (dovizKodu == Convert.ToString(gridView2.GetRowCellValue(i, "Mkodlar_Ad")))
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

        public string newHesapDokum2(bool hesapDokum, int Fisno, int Split, string Baslik, GridView gridView2)
        {

            string printer = String.Empty;
            int bosSatir = 0;
            string filter = "";
            string posta = Masa_Posta_bul(Fisno);
            decimal B = 0, A = 0, Bakiye = 0;
            if (Departman.Kodlar_Pr_Posta)
            {
                filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
            }
            else
            {
                filter = " and ISNULL(Pkod_Posta,'') = ''  ";
            }

            string query = "select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES' " + filter;
            DataTable dtPrinter = dbtools.SelectTable(query);

            if (dtPrinter.Rows.Count == 0)
            {
                query = "select distinct Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES' ";

                dtPrinter = dbtools.SelectTable(query);
            }




            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }

            DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'HESAP'");
            if (dtDizayn.Rows.Count < 1)
            {
                return "Hesap Dizaynı Yapılmamış...";
            }

            DataSet ds = new DataSet();
            DataTable dtHesap = new DataTable();
            DataTable dtOdeme = new DataTable();
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Split", Split);
            com.Parameters.AddWithValue("@Rapor_Tipi", 7);
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(ds);

            dtHesap = ds.Tables[0];
            dtOdeme = ds.Tables[1];
            if (con.State == ConnectionState.Open) con.Close();

            int dtSatirsay = dtHesap.Rows.Count;





            Print.Hesap hsp = new Print.Hesap();
            xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), hsp);
            hsp.PrinterName = printer;
            hsp.DataSource = dtHesap;

            DateTime sqlTarih = Convert.ToDateTime(dbtools.DegerGetir("select getdate()"));
            hsp.xr_OtelAdi.Text = Param.Tesis_Adi;
            hsp.xr_Baslik.Text = Baslik;
            hsp.xr_OtelAdi2.Text = Departman.Sube_Ad;
            hsp.xr_Departman.Text = Departman.Dep_Adi;

            string masaAd = Convert.ToString(dtHesap.Rows[0]["MasaAdi"]);
            if (masaAd.Trim().Equals(""))
            {
                masaAd = Convert.ToString(dtHesap.Rows[0]["Rsat_Masa"]);
            }


            hsp.xr_MasaNo.Text = masaAd;


            hsp.xr_Konum.Text = Convert.ToString(dtHesap.Rows[0]["MasaKonumAdi"]);
            hsp.xr_KisiSayisi.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Kisi"]);
            hsp.xr_Kuver.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Kuver"]);
            hsp.xr_Tarih.Text = Convert.ToDateTime(dtHesap.Rows[0]["Rsat_Tarih"]).ToShortDateString();
            hsp.xr_Acilis.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Acilis"]);
            hsp.xr_Kapanis.Text = sqlTarih.TimeOfDay.ToString();
            hsp.xr_Cek.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Fisno"]);
            hsp.xr_Odano.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Odano"]);
            hsp.xr_Rez.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]);
            if (Param.Tesis_Tipi == 0)
            {
                hsp.xr_Kart.Text = "";
                hsp.xr_GC.Text = "";
                try
                {
                    hsp.xr_Kart.Text = Fronttools.KartNo(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]));
                    hsp.xr_GC.Text = Fronttools.GirisCikisTarih(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]));
                }
                catch (Exception ex)
                {

                }

            }
            hsp.xr_Kasiyer.Text = Convert.ToString(dtHesap.Rows[0]["Kasiyer"]);
            hsp.xr_Adisyon.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Adisyon"]);





            if (Param.Param_HesapDkmAciklama)
            {
                hsp.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));
            }
            else
            {
                hsp.xr_Urun.Text = "[Rec_Ad]";
            }

            hsp.xr_Miktar.Text = String.Format("{0:0.##}", ("[Rsat_Miktar]"));

            hsp.xr_Emiktar.Text = "[Rsat_Emiktar]";

            string dovizIcon = "";
            switch (Param.Doviz_Adi1.ToUpper())
            {
                case "EURO":
                    dovizIcon = " €";
                    break;
                case "EUR":
                    dovizIcon = " €";
                    break;
                case "GBP":
                    dovizIcon = " £";
                    break;
                case "RUBLE":
                    dovizIcon = " ₽";
                    break;
                case "USD":
                    dovizIcon = " $";
                    break;
                case "DOLAR":
                    dovizIcon = " $";
                    break;
                default:
                    dovizIcon = " ₺";
                    break;
            }

            if (Param.Fis_Dovizli == 0)
            {
                hsp.xr_Tutar.Text = String.Format("{0:0.00}", ("[Rsat_Tutar]"));
            }
            else
            {
                hsp.xr_Tutar.Text = String.Format("{0:0.00}", ("[Rsat_Doviztutar]"));
            }


            hsp.xr_Tutar.Text += dovizIcon;


            if (Param.Param_Hsifir_Ikram)
            {
                dtHesap.ConvertColumnType("Rsat_Tutar", typeof(string));
            }


            decimal UrunToplam = 0, UrunToplamTr = 0;
            for (int i = 0; i < dtHesap.Rows.Count; i++)
            {

                decimal tutar = Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"].ToString());
                decimal dovizTutar = Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Doviztutar"].ToString());

                if (Param.Param_Hsifir_Ikram && tutar == 0)
                {
                    dtHesap.Rows[i]["Rsat_Tutar"] = "*IKRAM*";
                }

                if (dtHesap.Rows[i]["Rsat_Emiktar"].Equals("B"))
                {
                    dtHesap.Rows[i]["Rsat_Emiktar"] = "1BCK";
                }
                else if (dtHesap.Rows[i]["Rsat_Emiktar"].Equals("D"))
                {
                    dtHesap.Rows[i]["Rsat_Emiktar"] = "DBL";
                }
                else if (dtHesap.Rows[i]["Rsat_Emiktar"].Equals("Y"))
                {
                    dtHesap.Rows[i]["Rsat_Emiktar"] = "YRM";
                }


                if (Param.Fis_Dovizli == 0)
                {
                    UrunToplam += tutar;
                }
                else
                {
                    UrunToplam += dovizTutar;
                }

                B += tutar;
                UrunToplamTr += tutar;
            }


            decimal odemeToplam = 0, odemeToplamTr = 0;
            if (dtOdeme.Rows.Count > 0)
            {
                //Odeme Tablosu
                for (int i = 0; i < dtOdeme.Rows.Count; i++)
                {
                    if (dtOdeme.Rows[i]["Rsat_Indkodu"].ToString() != "")
                    {
                        dtOdeme.Rows[i]["DovizAdi"] = Param.Doviz_Adi1;
                    }
                    XRTableRow row = new XRTableRow();

                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = Convert.ToString(dtOdeme.Rows[i]["Rec_Ad"]);
                    cell1.WidthF = hsp.table_Odeme.WidthF * 30 / 100;
                    cell1.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                    row.Cells.Add(cell1);

                    XRTableCell cell2 = new XRTableCell();
                    if (Param.Fis_Dovizli == 0)
                    {
                        cell2.Text = Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]).ToString("n2");
                    }
                    else
                    {
                        cell2.Text = Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Doviztutar"]).ToString("n2");

                    }

                    switch (dtOdeme.Rows[i]["DovizAdi"].ToString().ToUpper())
                    {
                        case "EURO":
                            dovizIcon = " €";
                            break;
                        case "EUR":
                            dovizIcon = " €";
                            break;
                        case "GBP":
                            dovizIcon = " £";
                            break;
                        case "RUBLE":
                            dovizIcon = " ₽";
                            break;
                        case "USD":
                            dovizIcon = " $";
                            break;
                        case "DOLAR":
                            dovizIcon = " $";
                            break;
                        default:
                            dovizIcon = " ₺";
                            break;
                    }

                    cell2.Text += dovizIcon;



                    cell2.WidthF = hsp.table_Odeme.WidthF * 20 / 100;
                    cell2.Font = new System.Drawing.Font("Calibri", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                    cell2.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
                    row.Cells.Add(cell2);

                    //if (Param.Tesis_Tipi == 0)
                    //{
                    //    XRTableCell cell3 = new XRTableCell();
                    //    cell3.Text = Fronttools.IsimSoyisim(Convert.ToInt32(dtOdeme.Rows[i]["Rsat_Folio"]));
                    //    cell3.WidthF = hsp.table_Odeme.WidthF * 30 / 100;
                    //    row.Cells.Add(cell3);

                    //    XRTableCell cell4 = new XRTableCell();
                    //    cell4.Text = Convert.ToInt32(dtOdeme.Rows[i]["Rsat_Folio"]).ToString();
                    //    cell4.WidthF = hsp.table_Odeme.WidthF * 25 / 100;
                    //    row.Cells.Add(cell4);
                    //}
                    if (Param.Fis_Dovizli == 0)
                    {
                        odemeToplam += Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]);
                    }
                    else
                    {
                        //odemeToplam += Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Doviztutar"]);
                        decimal dovizTutar = Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_DovizTutar"]);

                        decimal dovizTLKarsilik = getKurKarsilik(Param.Doviz_Adi1, gridView2);
                        decimal girilenTLKarsilik = getKurKarsilik(dtOdeme.Rows[i]["DovizAdi"].ToString(), gridView2);
                        decimal tlKarsilik = girilenTLKarsilik * dovizTutar;
                        odemeToplam += tlKarsilik / dovizTLKarsilik;

                    }





                    odemeToplamTr += Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]);
                    hsp.table_Odeme.Rows.Add(row);

                    A = Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]);
                }
            }



            hsp.xr_UrunToplam.Text = UrunToplam.ToString("n2");
            hsp.xr_UrunToplamTr.Text = UrunToplamTr.ToString("n2");

            hsp.xr_KalanToplam.Text = UrunToplam.ToString("n2");
            hsp.xr_KalanToplamTr.Text = UrunToplamTr.ToString("n2");

            if (dtOdeme.Rows.Count > 0)
            {
                hsp.xr_KalanToplam.Text = (UrunToplam - odemeToplam).ToString("n2");
                hsp.xr_KalanToplamTr.Text = (UrunToplamTr - odemeToplamTr).ToString("n2");
            }

            // buraya yaz


            hsp.xr_Not.Text = Convert.ToString(dtHesap.Rows[0]["Rsat_Not"]); // new hesapdöküm

            Bakiye = B - A;
            Bakiye = Bakiye / Param.Doviz_Kuru;
            B = B / Param.Doviz_Kuru;
            Bakiye = Bakiye < Convert.ToDecimal(0.05) ? 0 : Bakiye;

            if (Param.Param_Hesap_DovizOzet)
            {
                DataTable dtDovizDagilim = new DataTable();

                decimal dagilimTutar = Param.Param_Hesap_DovizOzetToplam == true ? Bakiye : B; // ramo yaptı
                //decimal dagilimTutar = Param.Param_Hesap_DovizOzetToplam == true ? B : Bakiye; 

                if (Param.Kurlar_Nerden == 0)
                {
                    dtDovizDagilim = Fis_Islem.Doviz_DagilimFront(dagilimTutar);
                }
                else
                {
                    if (Param.Calisma_Sekli == 1 && Param.Tesis_Tipi == 1) // tesis 1 ise posdur .. 0 ise önbüro
                    {

                        dagilimTutar = Convert.ToDecimal(hsp.xr_UrunToplam.Text);
                        dtDovizDagilim = Fis_Islem.Doviz_Dagilim(dagilimTutar);
                    }
                    else
                    {
                        dtDovizDagilim = Fis_Islem.Doviz_Dagilim(dagilimTutar);
                    }
                }
                if (dtDovizDagilim.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDovizDagilim.Rows.Count; i++)
                    {
                        XRTableRow row = new XRTableRow();

                        XRTableCell cell1 = new XRTableCell();
                        cell1.Text = Convert.ToString(dtDovizDagilim.Rows[i]["Mkodlar_Ad"]);
                        cell1.WidthF = hsp.table_Doviz.WidthF * 30 / 100;
                        cell1.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                        row.Cells.Add(cell1);

                        XRTableCell cell2 = new XRTableCell();

                        cell2.Text = Convert.ToDecimal(dtDovizDagilim.Rows[i]["Doviz"]).ToString("n2");
                        cell2.WidthF = hsp.table_Doviz.WidthF * 20 / 100;
                        cell2.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                        cell2.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
                        row.Cells.Add(cell2);


                        hsp.table_Doviz.Rows.Add(row);
                    }
                }
            }
            string dovizIconKalan = "";

            switch (Param.Doviz_Adi1.ToString().ToUpper())
            {
                case "EURO":
                    dovizIconKalan = " €";
                    break;
                case "EUR":
                    dovizIcon = " €";
                    break;
                case "GBP":
                    dovizIconKalan = " £";
                    break;
                case "RUBLE":
                    dovizIconKalan = " ₽";
                    break;
                case "USD":
                    dovizIconKalan = " $";
                    break;
                case "DOLAR":
                    dovizIconKalan = " $";
                    break;
                default:
                    dovizIconKalan = " ₺";
                    break;
            }


            hsp.xr_KalanToplam.Text += dovizIconKalan;
            hsp.xr_UrunToplam.Text += dovizIconKalan;


            hsp.xr_UrunToplamTr.Text += " ₺";
            hsp.xr_KalanToplamTr.Text += " ₺";




            DataTable dtUrungrup = new DataTable();
            dtUrungrup = UrunGrupBul(Fisno);
            for (int j = 0; j < dtUrungrup.Rows.Count; j++)
            {
                XRTableRow row = new XRTableRow();

                XRTableCell cell1 = new XRTableCell();
                cell1.Text = Convert.ToString(dtUrungrup.Rows[j]["Kodlar_Ad"]);
                cell1.WidthF = hsp.table_UrunGrup.WidthF * 60 / 100;
                cell1.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                row.Cells.Add(cell1);

                XRTableCell cell2 = new XRTableCell();
                if (Param.Fis_Dovizli == 0)
                {
                    cell2.Text = Convert.ToDecimal(dtUrungrup.Rows[j]["Tutar"]).ToString("n2");
                }
                else
                {
                    cell2.Text = Convert.ToDecimal(dtUrungrup.Rows[j]["Doviztutar"]).ToString("n2");
                }

                cell2.WidthF = hsp.table_UrunGrup.WidthF * 40 / 100;
                cell2.Font = new System.Drawing.Font("Calibri", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                cell2.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
                row.Cells.Add(cell2);


                hsp.table_UrunGrup.Rows.Add(row);

            }


            if (Convert.ToString(dtHesap.Rows[0]["Rsat_MusTipi"]) != "U" && Param.Tesis_Tipi == 0)
            {
                if (Param.Fiste_Balance == 0)
                {
                    //int folio = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]) == "" ? 0 : Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]);

                    //hsp.Add("Bakiye :" + Fronttools.BalanceBul(folio, Kart_No).ToString("N2"));

                    //hsp.Add("");
                }


                bakiyeYaz(hsp, dtHesap);

            }
            else
            {
                hsp.xr_FolioAdSoyad.Text = (Convert.ToString(dtHesap.Rows[0]["Rsat_Uye_Ad"]));
            }

            if (hsp.xr_FolioAdSoyad.Text.Equals("") && dtOdeme.Rows.Count > 0)
            {
                string text = dtOdeme.Rows[0]["Cari"].ToString();
                hsp.xr_FolioAdSoyad.Text = text;

                if (text.Contains(" "))
                {
                    hsp.xr_FolioAdSoyad.Text = text.Substring(text.IndexOf(" ")).Trim();
                }
            }

            //if (Departman.Kodlar_AndPos_NFC == true)
            //{
            //    hsp.xr_FolioAdSoyad.Text = (Fronttools.CardFIsim(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));

            //    hsp.xr_Bakiye.Text = (Fronttools.NFCBakiye(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]), Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"]))).ToString();
            //}


            if (dtPrinter.Rows.Count > 0 && dtMacPrinter.Rows.Count == 0)
            {
                for (int i = 0; i < dtPrinter.Rows.Count; i++)
                {
                    if (Convert.ToString(dtPrinter.Rows[i]["Pkod_Ad"]) == "")
                    {
                        continue;
                    }

                    printer = Convert.ToString(dtPrinter.Rows[i]["Pkod_Ad"]);
                    hsp.PrinterName = printer;

                    for (int k = 0; k < Hesap_Ciktisayisi; k++)
                    {
                        hsp.Print();
                    }
                }

            }
            else
            {
                for (int i = 0; i < dtMacPrinter.Rows.Count; i++)
                {

                    if (Convert.ToString(dtMacPrinter.Rows[i]["Pkod_Ad"]) == "")
                    {
                        continue;
                    }
                    printer = Convert.ToString(dtMacPrinter.Rows[i]["Pkod_Ad"]);
                    hsp.PrinterName = printer;

                    for (int k = 0; k < Hesap_Ciktisayisi; k++)
                    {
                        hsp.Print();
                    }

                }
            }


            return "OK";
        }

        public string HesapDokum(bool hesapDokum, int Fisno, int Split)
        {
            //and (ISNULL(pr.Pkod_Posta,'') = ISNULL(@Posta,'') or ISNULL(pr.Pkod_Posta,'') = '')

            List<string> hesap = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            string filter = "";
            string posta = Masa_Posta_bul(Fisno);
            if (Departman.Kodlar_Pr_Posta)
            {
                filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
            }
            else
            {
                filter = " and ISNULL(Pkod_Posta,'') = ''  ";
            }
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES' " + filter);
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }


            DataSet ds = new DataSet();
            DataTable dtHesap = new DataTable();
            DataTable dtOdeme = new DataTable();
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Split", Split);
            com.Parameters.AddWithValue("@Rapor_Tipi", 7);
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(ds);
            dtHesap = ds.Tables[0];
            dtOdeme = ds.Tables[1];
            if (con.State == ConnectionState.Open) con.Close();

            int dtSatirsay = dtHesap.Rows.Count;


            for (int k = 0; k < Hesap_Ciktisayisi; k++)
            {
                decimal B = 0, A = 0, Bakiye = 0;

                if (printer == "") continue;


                hesap.Add("");
                if (hesapDokum) hesap.Add("  * * HESAP DOKUMU * *  ");
                else hesap.Add("  * * HESAP KAPATMA FISI * *  ");
                hesap.Add("");
                hesap.Add("#" + Param.Tesis_Adi.PadRight(40, ' ').Substring(0, 40));
                if (Param.Tesis_Tipi == 0)
                {
                    hesap.Add(Departman.Sube_Ad.PadRight(40, ' ').Substring(0, 40));
                    hesap.Add(Departman.Dep_Adi.PadRight(40, ' ').Substring(0, 40));
                }
                hesap.Add("");
                if (Param.Param_Adres1 != "") hesap.Add(Param.Param_Adres1);
                if (Param.Param_Adres2 != "") hesap.Add(Param.Param_Adres2);
                if (Param.Param_Adres3 != "") hesap.Add(Param.Param_Adres3);
                if (Param.Param_Adres4 != "") hesap.Add(Param.Param_Adres4);
                if (Param.Param_Adres5 != "") hesap.Add(Param.Param_Adres5);
                hesap.Add("");

                string masaAd = Convert.ToString(dtHesap.Rows[0]["MasaAdi"]);
                if (masaAd.Trim().Equals(""))
                {
                    masaAd = Convert.ToString(dtHesap.Rows[0]["Rsat_Masa"]);
                }
                hesap.Add("Masa : " + masaAd.PadRight(5, ' ') + " Kişi : " + Convert.ToString(dtHesap.Rows[0]["Rsat_Kisi"]).PadRight(5, ' '));
                //hesap.Add("Masa : " + Convert.ToString(dtHesap.Rows[0]["MasaAdi"]).PadRight(5, ' ') + " Kişi : " + Convert.ToString(dtHesap.Rows[0]["Rsat_Kisi"]).PadRight(5, ' '));
                hesap.Add("Kuver  :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Kuver"]).ToString().PadRight(6, ' '));
                hesap.Add("Tarih  :" + Convert.ToDateTime(dtHesap.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' '));
                hesap.Add("Acilis Saati :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Acilis"]).PadRight(15, ' '));
                //hesap.Add("Kapanis Saat :" + DateTime.Now.ToString("HH:ss:mm"));
                DateTime sqlTarih = Convert.ToDateTime(dbtools.DegerGetir("select getdate()"));
                hesap.Add("Kapanis Saat :" + sqlTarih.TimeOfDay.ToString());
                hesap.Add("Cek    :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Fisno"]).PadRight(14, ' '));
                if (Param.Tesis_Tipi == 0)
                {
                    int folio = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]) == "" ? 0 : Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]);

                    hesap.Add("Oda :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Odano"]).PadRight(16, ' ') + "Rezno:" + Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]).PadRight(14, ' '));
                    hesap.Add("Kart No:" + Fronttools.KartNo(folio));
                    hesap.Add("G-C: " + Fronttools.GirisCikisTarih(folio).ToString().PadRight(15, ' '));
                }
                hesap.Add("Kasiyer:" + Convert.ToString(dtHesap.Rows[0]["Kasiyer"]));
                hesap.Add("Adisyon:" + Convert.ToString(dtHesap.Rows[0]["Rsat_Adisyon"]).PadRight(11, ' '));
                hesap.Add("");
                hesap.Add("MIKTAR".PadLeft(6, ' ') + "URUN".PadLeft(10, ' ') + "TUTAR".PadLeft(15, ' '));

                //siparis.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));

                hesap.Add("".PadRight(40, '-'));
                //Satılan Ürünler

                int urunbosluk = 0;
                for (int i = 0; i < dtHesap.Rows.Count; i++)
                {
                    urunbosluk = 0;
                    //miktarbosluk = 0;
                    //tutarbosluk = 0;
                    //string ikram = Param.Param_Hsifir_Ikram && Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]) == 0 ? "*IKRAM*" : "";

                    string emiktar = string.Empty;
                    string miktar = string.Empty;
                    string rec_ad = string.Empty;
                    //string kdv = string.Empty;


                    emiktar = Convert.ToString(dtHesap.Rows[i]["Rsat_Emiktar"]) == "" ? " " : Convert.ToString(" " + String.Format("{0:0.##}", dtHesap.Rows[i]["Rsat_Emiktar"]));
                    miktar = Convert.ToString(" " + String.Format("{0:0.##}", dtHesap.Rows[i]["Rsat_Miktar"])).PadLeft(0, ' ');
                    rec_ad = Convert.ToString(dtHesap.Rows[i]["Rec_Ad"]);



                    rec_ad = (rec_ad) + " " + emiktar;


                    string tutar = "0";
                    if (Param.Fis_Dovizli == 1) //Dövizli
                    {
                        tutar = Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Doviztutar"]).ToString();
                    }
                    else
                    {
                        tutar = Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]).ToString();
                        if (Param.Param_Hsifir_Ikram && Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]) == 0)
                        {
                            tutar = "  *IKRAM*";
                        }
                    }
                    urunbosluk = Math.Abs(18 - rec_ad.Length);
                    //miktarbosluk = Math.Abs(10 - miktar.Length);
                    //tutarbosluk = Math.Abs(10 - tutar.Length);



                    hesap.Add(miktar.PadRight(5, ' ') + rec_ad.PadRight(20, ' ').Substring(0, 19) + "   " + tutar.PadLeft(5, ' '));

                    if (Param.Param_HesapDkmAciklama)
                    {
                        if (Convert.ToString(dtHesap.Rows[i]["Rsat_Aciklama"]) != String.Empty) hesap.Add(Convert.ToString(dtHesap.Rows[i]["Rsat_Aciklama"]));
                    }



                    B += Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]);
                }

                decimal toplamTutarYeni = 0;

                hesap.Add("".PadRight(40, '-'));
                if (dtOdeme.Rows.Count > 0)
                {
                    toplamTutarYeni = Convert.ToDecimal((B / Param.Doviz_Kuru).ToString("N2"));
                    hesap.Add("TOPLAM : ".PadRight(17, ' ') + (B / Param.Doviz_Kuru).ToString("N2").PadLeft(10, ' '));
                    hesap.Add("".PadRight(40, '-'));
                }

                //Ödemeler
                for (int i = 0; i < dtOdeme.Rows.Count; i++)
                {
                    //string miktar = Convert.ToString(dtOdeme.Rows[i]["Rsat_Miktar"]).PadLeft(5, ' ');
                    string rec_ad = Convert.ToString(dtOdeme.Rows[i]["Rec_Ad"]).PadRight(25, ' ');

                    if (Param.Fis_Dovizli == 1) //Dövizli
                    {
                        hesap.Add(rec_ad + Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Doviztutar"]).ToString().PadLeft(7, ' '));
                    }
                    else // TL
                    {
                        hesap.Add(rec_ad + Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]).ToString().PadLeft(7, ' '));
                    }
                    if (Convert.ToString(dtOdeme.Rows[i]["Cari"]) != "") hesap.Add(Convert.ToString(dtOdeme.Rows[i]["Cari"]));
                    A += Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]);


                }

                if (dtOdeme.Rows.Count > 0) hesap.Add("".PadRight(40, '-'));
                Bakiye = B - A;
                Bakiye = Bakiye / Param.Doviz_Kuru;
                B = B / Param.Doviz_Kuru;
                Bakiye = Bakiye < Convert.ToDecimal(0.05) ? 0 : Bakiye;
                //hesap.Add("TOPLAM : ".PadRight(6, ' ') + " " + Convert.ToString(Param.Doviz_Adi ?? "").PadLeft(5,' ') + Bakiye.ToString().PadLeft(10, ' '));
                hesap.Add("#" + "TOPLAM : ".PadRight(11, ' ') + " " + Bakiye.ToString("N2").PadLeft(10, ' '));
                hesap.Add("".PadRight(40, '-'));
                //Not
                hesap.Add("NOT : " + Convert.ToString(dtHesap.Rows[0]["Rsat_Not"]));
                hesap.Add("".PadRight(40, '-'));
                // Döviz Dağılımları
                if (Param.Param_Hesap_DovizOzet)
                {
                    DataTable dtDovizDagilim = new DataTable();

                    //if (Param.Tesis_Tipi == 0)
                    //{
                    //    dtDovizDagilim = Fis_Islem.Doviz_DagilimFront(Bakiye);
                    //}
                    //else
                    //{
                    //    dtDovizDagilim = Fis_Islem.Doviz_Dagilim(Bakiye);
                    //}

                    decimal dagilimTutar = Param.Param_Hesap_DovizOzetToplam == true ? B : Bakiye;

                    if (dagilimTutar == 0)
                    {
                        dagilimTutar = toplamTutarYeni;
                    }

                    if (Param.Kurlar_Nerden == 0)
                    {
                        dtDovizDagilim = Fis_Islem.Doviz_DagilimFront(dagilimTutar);
                    }
                    else
                    {
                        dtDovizDagilim = Fis_Islem.Doviz_Dagilim(dagilimTutar);
                    }

                    if (dtDovizDagilim.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDovizDagilim.Rows.Count; i++)
                        {
                            hesap.Add(Convert.ToString(dtDovizDagilim.Rows[i]["Mkodlar_Ad"]).PadRight(15, ' ') + "  " + Convert.ToString(dtDovizDagilim.Rows[i]["Doviz"]));
                            //  (" + Convert.ToString(dtDovizDagilim.Rows[i]["Kur"]) + ") 
                        }
                        hesap.Add("".PadRight(40, '-'));
                    }
                }


                if (Param.Fis_Urungrup)
                {
                    hesap.Add("");

                    DataTable dtUrungrup = new DataTable();
                    dtUrungrup = UrunGrupBul(Fisno);

                    for (int j = 0; j < dtUrungrup.Rows.Count; j++)
                    {
                        if (Param.Fis_Dovizli == 1)         //Dövizli
                        {
                            hesap.Add(Convert.ToString(dtUrungrup.Rows[j]["Kodlar_Ad"]).PadRight(15, ' ').Substring(0, 14) + Convert.ToDecimal(dtUrungrup.Rows[j]["Doviztutar"]).ToString().PadRight(7, ' '));
                        }
                        else
                        {
                            hesap.Add(String.Format("{0,-10}", (dtUrungrup.Rows[j]["Kodlar_Ad"])) + " " + Convert.ToDecimal(dtUrungrup.Rows[j]["Tutar"]).ToString("n2"));
                        }
                    }


                    hesap.Add("");



                }

                //hesap.Add("Teşekkür Ederiz...");
                //hesap.Add("Afiyet Olsun. Yine Bekleriz...");
                hesap.AddRange(Param.Param_Fis_Aciklama.Split('\n'));
                hesap.Add("");
                //hesap.Add("Rmos System     www.rmosyazilim.com");

                if (Convert.ToString(dtHesap.Rows[0]["Rsat_MusTipi"]) != "U" && Param.Tesis_Tipi == 0)
                {
                    if (Param.Fiste_Balance == 0)
                    {
                        int folio = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]) == "" ? 0 : Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]);

                        hesap.Add("Bakiye :" + Fronttools.BalanceBul(folio, Kart_No).ToString("N2"));

                        hesap.Add("");
                    }

                    hesap.Add("Teşekkür Ederiz...");
                    hesap.Add("");

                    if (Param.Tesis_Tipi == 0)
                    {
                        int folio = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]) == "" ? 0 : Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]);
                        hesap.Add(Fronttools.IsimSoyisim(folio));
                    }
                }
                else
                {
                    hesap.Add(Convert.ToString(dtHesap.Rows[0]["Rsat_Uye_Ad"]));
                }

                if (Departman.Kodlar_AndPos_NFC == true)
                {
                    hesap.Add("İsim Soyisim : " + Fronttools.CardFIsim(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));
                    hesap.Add(".");
                    hesap.Add("Bakiye : " + Fronttools.NFCBakiye(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]), Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));
                }



                if (Param.Tesis_Tipi == 0)
                {
                    hesap.Add("İmza : ");
                    hesap.Add(".");
                    hesap.Add(".");
                }






                for (int j = 0; j < bosSatir; j++)
                {
                    hesap.Add("");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                    if (Param.Param_HspFontAlgilama == true)
                    {
                        fnt = null;
                        if (fnt == null)
                        {
                            fnt = new Font("Arial", 8, FontStyle.Bold);
                        }
                    }
                    printFont = fnt;

                    Liste = hesap;




                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;



                    string yazici = dbtools.DegerGetir("select top 1 isnull(hesapyazici,0) as hesapyazici from Rmosmuh.dbo.Pos_User_XZ where P_Kod='" + User.P_Kod + "'");


                    if (yazici.Length > 2)
                    {
                        pd.PrinterSettings.PrinterName = yazici;
                    }

                    pd.Print();

                    hesap.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";
        }
        public string KartDokum(bool hesapDokum, int KartID, int Split)
        {
            //and (ISNULL(pr.Pkod_Posta,'') = ISNULL(@Posta,'') or ISNULL(pr.Pkod_Posta,'') = '')

            List<string> hesap = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            string filter = "";
            //string posta = Masa_Posta_bul(Fisno);
            //if (Departman.Kodlar_Pr_Posta)
            //{
            //    filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
            //}
            //else
            //{
            //    filter = " and ISNULL(Pkod_Posta,'') = ''  ";
            //}
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES' " + filter);
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }


            DataSet ds = new DataSet();
            DataTable dtHesap = new DataTable();
            DataTable dtOdeme = new DataTable();
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@KartId", KartID);
            com.Parameters.AddWithValue("@Split", Split);
            com.Parameters.AddWithValue("@Rapor_Tipi", 15);
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(ds);
            dtHesap = ds.Tables[0];
            dtOdeme = ds.Tables[1];
            if (con.State == ConnectionState.Open) con.Close();

            int dtSatirsay = dtHesap.Rows.Count;

            for (int k = 0; k < Hesap_Ciktisayisi; k++)
            {
                decimal B = 0, A = 0, Bakiye = 0;


                hesap.Add("");
                if (hesapDokum) hesap.Add("  * * KART DOKUMU * *  ");
                else hesap.Add("  * * KART DOKUMU * *  ");
                hesap.Add("");
                hesap.Add("#" + Param.Tesis_Adi.PadRight(40, ' ').Substring(0, 40));
                if (Param.Tesis_Tipi == 0)
                {
                    hesap.Add(Departman.Sube_Ad.PadRight(40, ' ').Substring(0, 40));
                    hesap.Add(Departman.Dep_Adi.PadRight(40, ' ').Substring(0, 40));
                }
                hesap.Add("");
                if (Param.Param_Adres1 != "") hesap.Add(Param.Param_Adres1);
                if (Param.Param_Adres2 != "") hesap.Add(Param.Param_Adres2);
                if (Param.Param_Adres3 != "") hesap.Add(Param.Param_Adres3);
                if (Param.Param_Adres4 != "") hesap.Add(Param.Param_Adres4);
                if (Param.Param_Adres5 != "") hesap.Add(Param.Param_Adres5);
                hesap.Add("");
                hesap.Add("Masa : " + Convert.ToString(dtHesap.Rows[0]["MasaAdi"]).PadRight(5, ' ') + " Kişi : " + Convert.ToString(dtHesap.Rows[0]["Rsat_Kisi"]).PadRight(5, ' '));
                hesap.Add("Kuver  :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Kuver"]).ToString().PadRight(6, ' '));
                hesap.Add("Tarih  :" + Convert.ToDateTime(dtHesap.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' '));
                hesap.Add("Acilis Saati :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Acilis"]).PadRight(15, ' '));
                //hesap.Add("Kapanis Saat :" + DateTime.Now.ToString("HH:ss:mm"));
                DateTime sqlTarih = Convert.ToDateTime(dbtools.DegerGetir("select getdate()"));
                hesap.Add("Kapanis Saat :" + sqlTarih.TimeOfDay.ToString());
                hesap.Add("Cek    :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Fisno"]).PadRight(14, ' '));
                if (Param.Tesis_Tipi == 0)
                {
                    int folio = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]) == "" ? 0 : Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]);

                    hesap.Add("Oda :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Odano"]).PadRight(16, ' ') + "Rezno:" + Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]).PadRight(14, ' '));
                    hesap.Add("Kart No:" + Fronttools.KartNo(folio));
                    hesap.Add("G-C: " + Fronttools.GirisCikisTarih(folio).ToString().PadRight(15, ' '));
                }
                hesap.Add("Kasiyer:" + Convert.ToString(dtHesap.Rows[0]["Kasiyer"]));
                hesap.Add("Adisyon:" + Convert.ToString(dtHesap.Rows[0]["Rsat_Adisyon"]).PadRight(11, ' '));
                hesap.Add("");
                hesap.Add("URUN".PadRight(25, ' ') + "MIKTAR".PadLeft(5, ' ') + "TUTAR".PadLeft(10, ' '));
                hesap.Add("".PadRight(40, '-'));
                //Satılan Ürünler

                int urunbosluk = 0;
                for (int i = 0; i < dtHesap.Rows.Count; i++)
                {
                    urunbosluk = 0;

                    //string ikram = Param.Param_Hsifir_Ikram && Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]) == 0 ? "*IKRAM*" : "";

                    string emiktar = string.Empty;
                    string miktar = string.Empty;
                    string rec_ad = string.Empty;
                    //string kdv = string.Empty;


                    emiktar = Convert.ToString(dtHesap.Rows[i]["Rsat_Emiktar"]) == "" ? " " : Convert.ToString(" " + String.Format("{0:0.##}", dtHesap.Rows[i]["Rsat_Emiktar"]));
                    miktar = Convert.ToString(" " + String.Format("{0:0.##}", dtHesap.Rows[i]["Rsat_Miktar"])).PadLeft(5, ' ');
                    rec_ad = Convert.ToString(dtHesap.Rows[i]["Rec_Ad"]);
                    //kdv = Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Kdvoran"]).ToString("n2");

                    //string emiktar = Convert.ToString(dtHesap.Rows[i]["Rsat_Emiktar"]) == "" ? " " : Convert.ToString(dtHesap.Rows[i]["Rsat_Emiktar"]);
                    //string miktar = Convert.ToString(dtHesap.Rows[i]["Rsat_Miktar"]).PadLeft(5, ' ');
                    //string rec_ad = Convert.ToString(dtHesap.Rows[i]["Rec_Ad"]);

                    rec_ad = (rec_ad).PadRight(22, ' ') + emiktar;
                    string tutar = "0";
                    if (Param.Fis_Dovizli == 1) //Dövizli
                    {
                        tutar = Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Doviztutar"]).ToString();
                    }
                    else
                    {
                        tutar = Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]).ToString();
                        if (Param.Param_Hsifir_Ikram && Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]) == 0)
                        {
                            tutar = "  *IKRAM*";
                        }
                    }
                    urunbosluk = Math.Abs(25 - rec_ad.Length);


                    hesap.Add(rec_ad.PadRight(urunbosluk, ' ') + miktar.PadRight(12, ' ') + tutar);

                    B += Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]);
                }
                hesap.Add("".PadRight(40, '-'));
                if (dtOdeme.Rows.Count > 0)
                {
                    hesap.Add("TOPLAM : ".PadRight(20, ' ') + (B / Param.Doviz_Kuru).ToString("N2").PadLeft(10, ' '));
                    hesap.Add("".PadRight(40, '-'));
                }

                //Ödemeler
                for (int i = 0; i < dtOdeme.Rows.Count; i++)
                {
                    //string miktar = Convert.ToString(dtOdeme.Rows[i]["Rsat_Miktar"]).PadLeft(5, ' ');
                    string rec_ad = Convert.ToString(dtOdeme.Rows[i]["Rec_Ad"]).PadRight(25, ' ');

                    if (Param.Fis_Dovizli == 1) //Dövizli
                    {
                        hesap.Add(rec_ad + Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Doviztutar"]).ToString().PadLeft(10, ' '));
                    }
                    else // TL
                    {
                        hesap.Add(rec_ad + Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]).ToString().PadLeft(10, ' '));
                    }
                    if (Convert.ToString(dtOdeme.Rows[i]["Cari"]) != "") hesap.Add(Convert.ToString(dtOdeme.Rows[i]["Cari"]));
                    A += Convert.ToDecimal(dtOdeme.Rows[i]["Rsat_Tutar"]);
                }
                if (dtOdeme.Rows.Count > 0) hesap.Add("".PadRight(40, '-'));
                Bakiye = B - A;
                Bakiye = Bakiye / Param.Doviz_Kuru;
                B = B / Param.Doviz_Kuru;
                Bakiye = Bakiye < Convert.ToDecimal(0.05) ? 0 : Bakiye;
                //hesap.Add("TOPLAM : ".PadRight(6, ' ') + " " + Convert.ToString(Param.Doviz_Adi ?? "").PadLeft(5,' ') + Bakiye.ToString().PadLeft(10, ' '));
                hesap.Add("TOPLAM : ".PadRight(11, ' ') + " " + Bakiye.ToString("N2").PadLeft(10, ' '));
                hesap.Add("".PadRight(40, '-'));
                //Not
                hesap.Add("NOT : " + Convert.ToString(dtHesap.Rows[0]["Rsat_Not"]));
                hesap.Add("".PadRight(40, '-'));
                // Döviz Dağılımları
                if (Param.Param_Hesap_DovizOzet)
                {
                    DataTable dtDovizDagilim = new DataTable();

                    //if (Param.Tesis_Tipi == 0)
                    //{
                    //    dtDovizDagilim = Fis_Islem.Doviz_DagilimFront(Bakiye);
                    //}
                    //else
                    //{
                    //    dtDovizDagilim = Fis_Islem.Doviz_Dagilim(Bakiye);
                    //}

                    decimal dagilimTutar = Param.Param_Hesap_DovizOzetToplam == true ? B : Bakiye;

                    if (Param.Kurlar_Nerden == 0)
                    {
                        dtDovizDagilim = Fis_Islem.Doviz_DagilimFront(dagilimTutar);
                    }
                    else
                    {
                        dtDovizDagilim = Fis_Islem.Doviz_Dagilim(dagilimTutar);
                    }

                    if (dtDovizDagilim.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDovizDagilim.Rows.Count; i++)
                        {
                            hesap.Add(Convert.ToString(dtDovizDagilim.Rows[i]["Mkodlar_Ad"]).PadRight(15, ' ') + "  " + Convert.ToString(dtDovizDagilim.Rows[i]["Doviz"]));
                            //  (" + Convert.ToString(dtDovizDagilim.Rows[i]["Kur"]) + ") 
                        }
                        hesap.Add("".PadRight(40, '-'));
                    }
                }




                //hesap.Add("Teşekkür Ederiz...");
                //hesap.Add("Afiyet Olsun. Yine Bekleriz...");
                hesap.AddRange(Param.Param_Fis_Aciklama.Split('\n'));
                hesap.Add("");
                //hesap.Add("Rmos System     www.rmosyazilim.com");

                if (Convert.ToString(dtHesap.Rows[0]["Rsat_MusTipi"]) != "U" && Param.Tesis_Tipi == 0)
                {
                    if (Param.Fiste_Balance == 0)
                    {
                        int folio = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]) == "" ? 0 : Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]);

                        hesap.Add("Bakiye :" + Fronttools.BalanceBul(folio, Kart_No).ToString("N2"));

                        hesap.Add("");
                    }

                    hesap.Add("Teşekkür Ederiz...");
                    hesap.Add("");

                    if (Param.Tesis_Tipi == 0)
                    {
                        int folio = Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]) == "" ? 0 : Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]);
                        hesap.Add(Fronttools.IsimSoyisim(folio));
                    }
                }
                else
                {
                    hesap.Add(Convert.ToString(dtHesap.Rows[0]["Rsat_Uye_Ad"]));
                }

                if (Departman.Kodlar_AndPos_NFC == true)
                {
                    hesap.Add("İsim Soyisim : " + Fronttools.CardFIsim(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));
                    hesap.Add(".");
                    hesap.Add("Bakiye : " + Fronttools.NFCBakiye(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]), Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));
                }



                if (Param.Tesis_Tipi == 0)
                {
                    hesap.Add("İmza : ");
                    hesap.Add(".");
                    hesap.Add(".");
                    hesap.Add(".");
                }

                for (int j = 0; j < bosSatir; j++)
                {
                    hesap.Add("");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                    if (Param.Param_HspFontAlgilama == true)
                    {
                        fnt = null;
                        if (fnt == null)
                        {
                            fnt = new Font("Arial", 8);
                        }
                    }
                    printFont = fnt;

                    Liste = hesap;

                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    hesap.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";
        }

        string Kart_No = String.Empty;
        private string HesapPr(int Fisno)
        {
            List<string> hesap = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }



            DataTable dtHesap = new DataTable();
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Rapor_Tipi", 2);
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(dtHesap);
            if (con.State == ConnectionState.Open) con.Close();


            for (int k = 0; k < Hesap_Ciktisayisi; k++)
            {
                decimal bakiye = 0;

                hesap.Add(".");
                hesap.Add("  * * * HESAP FISI * * *  ");
                hesap.Add(".");
                hesap.Add(Param.Tesis_Adi);
                hesap.Add(".");
                hesap.Add("Tarh:" + Convert.ToDateTime(dtHesap.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(10, ' ') + " Cek :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Fisno"]).PadRight(8, ' '));
                hesap.Add("Oda :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Odano"]).PadRight(10, ' ') + "Rezno:" + Convert.ToString(dtHesap.Rows[0]["Rsat_Folio"]).PadRight(8, ' '));
                hesap.Add("G-C: " + Fronttools.GirisCikisTarih(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"])).ToString().PadRight(8, ' '));
                hesap.Add("Masa:" + Convert.ToString(dtHesap.Rows[0]["Rsat_Masa"]).PadRight(10, ' ') + "Kasyr:" + Convert.ToString(dtHesap.Rows[0]["Kasiyer"]).PadRight(8, ' ').Substring(0, 7));
                hesap.Add("Adisyon:" + Convert.ToString(dtHesap.Rows[0]["Rsat_Adisyon"]).PadRight(11, ' '));
                hesap.Add(".");
                hesap.Add("URUN" + "        " + "MIKTAR" + "   TUTAR");
                hesap.Add("------------------------");
                for (int i = 0; i < dtHesap.Rows.Count; i++)
                {


                    if (Param.Fis_Dovizli == 1)         //Dövizli
                    {
                        hesap.Add(Convert.ToString(dtHesap.Rows[i]["Rec_Ad"]).PadRight(17, ' ').Substring(0, 16) + " " + Convert.ToString(" " + String.Format("{0:0.##}", dtHesap.Rows[i]["Rsat_Miktar"])).PadLeft(4, ' ') + Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Doviztutar"]).ToString().PadLeft(7, ' '));
                    }
                    else            // TL
                    {
                        hesap.Add(Convert.ToString(dtHesap.Rows[i]["Rec_Ad"]).PadRight(17, ' ').Substring(0, 16) + " " + Convert.ToString(" " + String.Format("{0:0.##}", dtHesap.Rows[i]["Rsat_Miktar"])).PadLeft(4, ' ') + Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]).ToString().PadLeft(7, ' '));
                    }




                    //hesap.Add("------------------------");

                    bakiye += Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]);
                }
                hesap.Add("------------------------");
                hesap.Add(" TOPLAM : " + bakiye.ToString() + " " + Param.Doviz_Adi);
                hesap.Add("------------------------");


                hesap.Add(".");
                hesap.Add("Teşekkür Ederiz...");
                hesap.Add(".");

                for (int j = 0; j < bosSatir; j++)
                {
                    hesap.Add(".");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = hesap;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    hesap.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }
            return "OK";
        }

        public string PaketciPr(DateTime Tarih, string Paketci)
        {
            string printer = String.Empty;
            int bosSatir = 0;
            string filter = "";

            List<string> PaketList = new List<string>();

            //string posta = Masa_Posta_bul(Fisno);
            //if (Departman.Kodlar_Pr_Posta)
            //{
            //    filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
            //}
            //else
            //{
            //    filter = " and ISNULL(Pkod_Posta,'') = ''  ";
            //}
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'PKT' " + filter);
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }
            //DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'PKT'");
            //if (dtPrinter.Rows.Count > 0)
            //{
            //    printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
            //    bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            //}

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'PKT' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }


            DataTable dtIptal = dbtools.SelectTable("Exec Pos_Satis @Tarih1 = '" + Tarih.ToString("yyyy-MM-dd") + "', @Paketci = '" + Paketci + "', @Rapor_Tipi = 22");

            if (dtIptal.Rows.Count > 0)
            {
                PaketList.Add("* * * PAKETCI OZET * * *");
                PaketList.Add("");
                PaketList.Add("Tarh:" + Convert.ToDateTime(dtIptal.Rows[0]["Tarih"]).ToString("dd.MM.yyyy").PadRight(10, ' '));
                PaketList.Add("");
                PaketList.Add("Paketci Ad Soyad:" + Convert.ToString(dtIptal.Rows[0]["Paketci"]).PadRight(10, ' '));
                PaketList.Add("");
                PaketList.Add("FISNO  MASANO  TUTAR        ");
                PaketList.Add("----------------------------");

                decimal Toplam = 0;
                for (int i = 0; i < dtIptal.Rows.Count; i++)
                {
                    PaketList.Add(Convert.ToString(dtIptal.Rows[i]["Rsat_Fisno"]).ToString().PadRight(8, ' ') + Convert.ToString(dtIptal.Rows[i]["MasaNo"]).ToString().PadRight(6, ' ') + Convert.ToString(dtIptal.Rows[i]["Tutar"]).PadRight(3, ' ') + "    " + Convert.ToString(dtIptal.Rows[i]["OdemeTipi"]).PadRight(5, ' '));
                    Toplam += Convert.ToDecimal(dtIptal.Rows[i]["Tutar"]);
                }

                PaketList.Add("--------------------------------");
                PaketList.Add("");

                PaketList.Add("TOPLAM PAKET : " + Convert.ToInt32(dtIptal.Rows.Count).ToString());
                PaketList.Add("TOPLAM TUTAR : " + Toplam.ToString("n2"));
                PaketList.Add("");
                PaketList.Add("");


                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 7);
                    }

                    Liste = PaketList;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    PaketList.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }

            }


            return "OK";
        }

        public string IptalPr(int SatirId, decimal Miktar)
        {
            List<string> iptal = new List<string>();

            //DataTable dtIptal = dbtools.SelectTable(" declare @Param_Printer_Tanim bit = '" + Param.Param_Printer_Tanim + "' "
            //            + " select Rsat_Tarih,Rsat_Fisno,Rsat_Masa,Rsat_Miktar,Rec_Ad,Rsat_Aciklama,pr.Pkod_Satir, case when @Param_Printer_Tanim = 0 then pr.Pkod_Ad else tanim.Pkod_Printer end as Printer "
            //            + " from Cst_Recete_Satis with(nolock) "
            //            + " left join Cst_Recete on Rec_Genelkod = Rsat_Recete "
            //            + " left join Pos_Kodlar as pr on Rec_Anagrup = pr.Pkod_Ustgrup and Rec_Altgrup = pr.Pkod_Altgrup and pr.Pkod_Sinif = '16'  and pr.Pkod_Kod = Rsat_Departman "
            //            + " left join Pos_Kodlar as tanim on  tanim.Pkod_Sinif = '19' and tanim.Pkod_Kod = pr.Pkod_Printer  "
            //            + " where Rsat_Id = '" + SatirId + "' and ISNULL(Rsat_SiparisPr,0) = 1 and ISNULL(Rec_SiparisCikmasin,0) = 0 ");

            DataTable dtIptal = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 15, @Rsat_Id = '" + SatirId + "', @MacAdres = '" + dbtools.MacAdresi() + "'");

            if (dtIptal.Rows.Count < 1)
            {
                return "OK";
            }

            for (int i = 0; i < dtIptal.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtIptal.Rows[i]["Printer"]);
                string abuyerPr = Convert.ToString(dtIptal.Rows[i]["Pkod_AbuyerPr"]);
                int bosSatir = Convert.ToInt32(dtIptal.Rows[i]["Pkod_Satir"]);

                if (Convert.ToString(dtIptal.Rows[0]["Mac_Printer"]) != "")
                {
                    printer = Convert.ToString(dtIptal.Rows[i]["Mac_Printer"]);
                    bosSatir = Convert.ToInt32(dtIptal.Rows[i]["Mac_Satir"]);
                }


                for (int k = 0; k < Iptal_Ciktisayisi; k++)
                {

                    iptal.Add(".");
                    iptal.Add("  * * * IPTAL FIS * * *  ");
                    iptal.Add("Masa: " + Convert.ToString(dtIptal.Rows[0]["Rsat_Masa"]).PadRight(5, ' ') + " Adi: " + Convert.ToString(dtIptal.Rows[0]["Masa_Ad"]).PadRight(5, ' '));
                    iptal.Add("Tarh:" + Convert.ToDateTime(dtIptal.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(10, ' ') + "Cekno:" + Convert.ToString(dtIptal.Rows[0]["Rsat_Fisno"]).PadRight(8, ' '));
                    iptal.Add("Saat:" + Convert.ToDateTime(dtIptal.Rows[0]["IptalTarih"]).ToString("HH:mm:ss"));
                    iptal.Add("Kullanıcı:" + User.P_Ad + " " + User.P_Soyad);

                    iptal.Add("MIKTAR   URUN                   ");
                    iptal.Add("--------------------------------");
                    iptal.Add("#" + Miktar.ToString().PadRight(6, ' ') + Convert.ToString(dtIptal.Rows[0]["Rec_Ad"]).PadLeft(17, ' ').Substring(0, 17));
                    iptal.Add(Convert.ToString(dtIptal.Rows[0]["Rsat_Aciklama"]));
                    iptal.Add("--------------------------------");
                }

                iptal.Add(".");

                if (Departman.Kodlar_AndPos_NFC == true)
                {
                    DataTable dtHesap = new DataTable();
                    iptal.Add("İsim Soyisim : " + Fronttools.CardFIsim(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));
                    iptal.Add(".");
                    iptal.Add("Bakiye : " + Fronttools.NFCBakiye(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]), Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));
                }


                for (int j = 0; j < bosSatir; j++)
                {
                    iptal.Add(".");
                }


                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Iptal_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = iptal;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    if (printer != "")
                    {
                        pd.PrinterSettings.PrinterName = printer;
                        pd.Print();
                    }

                    if (abuyerPr != "")
                    {
                        pd.PrinterSettings.PrinterName = abuyerPr;
                        pd.Print();
                    }

                    iptal.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";
        }

        public string newIptalPr(int SatirId, decimal Miktar)
        {
            yazdirilmismi = false;

            string mac = dbtools.MacAdresi();
            string q = "exec Pos_Sorgu @Sorgu_Tipi = 15, @Rsat_Id = '" + SatirId + "', @MacAdres = '" + dbtools.MacAdresi() + "'";
            DataTable dtIptal = dbtools.SelectTable(q);

            DataTable dtCopy = dtIptal.Clone();

            foreach (DataRow drtableOld in dtIptal.Rows)
            {
                dtCopy.ImportRow(drtableOld);
                break;
            }


            foreach (DataRow item in dtIptal.Rows)
            {
                yazdirilmismi = true;
                string printer = Convert.ToString(item["Printer"]);
                string abuyerPr = Convert.ToString(item["Pkod_AbuyerPr"]);
                int bosSatir = Convert.ToInt32(item["Pkod_Satir"]);


                if (Convert.ToString(item["Mac_Printer"]) != "")
                {
                    printer = Convert.ToString(item["Mac_Printer"]);
                }

                DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'IPTALFISI'");
                if (dtDizayn.Rows.Count < 1)
                {
                    return "İptal Fişi Dizaynı Yapılmamış...";
                }

                Print.IptalFisi iptal = new Print.IptalFisi();
                xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), iptal);

                iptal.DataSource = dtCopy;


                iptal.xr_MasaNo.Text = Convert.ToString(item["Rsat_Masa"]);//+ " Adi: " + Convert.ToString(item["Masa_Ad"]).PadRight(5, ' '));
                iptal.xr_Tarih.Text = Convert.ToDateTime(item["Rsat_Tarih"]).ToString("dd.MM.yyyy");
                iptal.xr_Cek.Text = Convert.ToString(item["Rsat_Fisno"]);
                iptal.xr_Acilis.Text = Convert.ToDateTime(item["IptalTarih"]).ToString("HH:mm:ss");
                iptal.xr_Garson.Text = User.P_Ad + " " + User.P_Soyad;

                string miktarim = Miktar.ToString().Replace(",0000", "").Replace(",000", "").Replace(",00", "");

                iptal.xr_Miktar.Text = miktarim + " " + "[Rsat_Emiktar]";
                //iptal.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
                iptal.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));



                if (Convert.ToString(item["Mac_Printer"]) != "")
                {
                    printer = Convert.ToString(item["Mac_Printer"]);
                    bosSatir = Convert.ToInt32(item["Mac_Satir"]);
                }

                string reckod = dbtools.DegerGetir("select isnull(Rsat_Recete,'') as Rsat_Recete from Cst_Recete_Satis where Rsat_Id='" + SatirId + "'");
                string yaziciismi = dbtools.DegerGetir("select top 1 isnull(Rec_Printer,'') as Rec_Printer from Cst_Recete where Rec_Genelkod='" + reckod + "'");


                iptal.PrinterName = printer;

                if (yaziciismi != null && yaziciismi != "")
                {
                    iptal.PrinterName = yaziciismi;
                }

                if (iptal.PrinterName != "Microsoft Print to PDF" && iptal.PrinterName != "")
                {
                    iptal.Print();
                }


                string ipyalYaziciAd = AbuyerPrIptalFis(SatirId);
                if (ipyalYaziciAd != null && ipyalYaziciAd != "")
                {
                    iptal = new Print.IptalFisi();
                    xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), iptal);

                    iptal.DataSource = dtCopy;


                    iptal.xr_MasaNo.Text = Convert.ToString(item["Rsat_Masa"]);//+ " Adi: " + Convert.ToString(item["Masa_Ad"]).PadRight(5, ' '));
                    iptal.xr_Tarih.Text = Convert.ToDateTime(item["Rsat_Tarih"]).ToString("dd.MM.yyyy");
                    iptal.xr_Cek.Text = Convert.ToString(item["Rsat_Fisno"]);
                    iptal.xr_Acilis.Text = Convert.ToDateTime(item["IptalTarih"]).ToString("HH:mm:ss");
                    iptal.xr_Garson.Text = User.P_Ad + " " + User.P_Soyad;

                    miktarim = Miktar.ToString().Replace(",0000", "").Replace(",000", "").Replace(",00", "");

                    iptal.xr_Miktar.Text = miktarim + " " + "[Rsat_Emiktar]";
                    //iptal.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
                    iptal.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));



                    iptal.PrinterName = ipyalYaziciAd;


                    if (iptal.PrinterName != "Microsoft Print to PDF" && iptal.PrinterName != "")
                    {
                        iptal.Print();
                    }


                }



            }





            return "OK";
        }

        public bool yazdirilmismi { get; set; } = false;

        //public string newIptalPr(int SatirId, decimal Miktar) // orji ramazan düzeltmeden önceki hali
        //{
        //    DataTable dtIptal = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 15, @Rsat_Id = '" + SatirId + "', @MacAdres = '" + dbtools.MacAdresi() + "'");

        //    if (dtIptal.Rows.Count < 1)
        //    {
        //        return "OK";
        //    }

        //    string printer = Convert.ToString(dtIptal.Rows[0]["Printer"]);
        //    string abuyerPr = Convert.ToString(dtIptal.Rows[0]["Pkod_AbuyerPr"]);
        //    int bosSatir = Convert.ToInt32(dtIptal.Rows[0]["Pkod_Satir"]);


        //    if (Convert.ToString(dtIptal.Rows[0]["Mac_Printer"]) != "")
        //    {
        //        printer = Convert.ToString(dtIptal.Rows[0]["Mac_Printer"]);
        //    }

        //    DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'IPTALFISI'");
        //    if (dtDizayn.Rows.Count < 1)
        //    {
        //        return "İptal Fişi Dizaynı Yapılmamış...";
        //    }

        //    Print.IptalFisi iptal = new Print.IptalFisi();
        //    xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), iptal);

        //    iptal.DataSource = dtIptal;


        //    iptal.xr_MasaNo.Text = Convert.ToString(dtIptal.Rows[0]["Rsat_Masa"]);//+ " Adi: " + Convert.ToString(dtIptal.Rows[0]["Masa_Ad"]).PadRight(5, ' '));
        //    iptal.xr_Tarih.Text = Convert.ToDateTime(dtIptal.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy");
        //    iptal.xr_Cek.Text = Convert.ToString(dtIptal.Rows[0]["Rsat_Fisno"]);
        //    iptal.xr_Acilis.Text = Convert.ToDateTime(dtIptal.Rows[0]["IptalTarih"]).ToString("HH:mm:ss");
        //    iptal.xr_Garson.Text = User.P_Ad + " " + User.P_Soyad;


        //    for (int i = 0; i < dtIptal.Rows.Count; i++)
        //    {
        //        iptal.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";
        //        iptal.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));

        //        // printer = Convert.ToString(dtIptal.Rows[i]["Printer"]);
        //        //string abuyerPr = Convert.ToString(dtIptal.Rows[i]["Pkod_AbuyerPr"]);
        //        //int bosSatir = Convert.ToInt32(dtIptal.Rows[i]["Pkod_Satir"]);

        //        if (Convert.ToString(dtIptal.Rows[0]["Mac_Printer"]) != "")
        //        {
        //            printer = Convert.ToString(dtIptal.Rows[i]["Mac_Printer"]);
        //            bosSatir = Convert.ToInt32(dtIptal.Rows[i]["Mac_Satir"]);
        //        }

        //        iptal.PrinterName = printer;
        //        iptal.Print();
        //    }

        //    return "OK";
        //}

        public string IptalPrNFC(int Fisno)
        {
            List<string> iptal = new List<string>();



            string printer = String.Empty;
            int bosSatir = 0;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and (Pkod_Ustgrup = 'BKY' or Pkod_Ustgrup = 'HES')");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and (Pkod_Ustgrup = 'BKY' or Pkod_Ustgrup = 'HES') and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }


            DataTable dtIptal = dbtools.SelectTable("exec Pos_Satis @Rapor_Tipi = 16, @Fisno = '" + Fisno + "'");

            if (dtIptal.Rows.Count < 1)
            {
                return "OK";
            }



            for (int k = 0; k < Iptal_Ciktisayisi; k++)
            {


                iptal.Add("* * * IPTAL FIS * * *");
                iptal.Add("");
                iptal.Add("Masa: " + Convert.ToString(dtIptal.Rows[0]["Rsat_Masa"]).PadRight(5, ' ') + " Adi: " + Convert.ToString(dtIptal.Rows[0]["Masa_Ad"]).PadRight(5, ' '));
                iptal.Add("Tarh:" + Convert.ToDateTime(dtIptal.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(10, ' ') + "Cekno:" + Convert.ToString(dtIptal.Rows[0]["Rsat_Fisno"]).PadRight(8, ' '));
                iptal.Add("Saat:" + Convert.ToDateTime(dtIptal.Rows[0]["Rsat_Iptal_Zaman"]).ToString("HH:mm:ss"));
                iptal.Add("Kullanıcı:" + User.P_Ad + " " + User.P_Soyad);
                iptal.Add("");
                iptal.Add("MIKTAR       URUN               ");
                iptal.Add("--------------------------------");

                for (int i = 0; i < dtIptal.Rows.Count; i++)
                {
                    iptal.Add(Convert.ToString(dtIptal.Rows[i]["Rsat_Miktar"]).ToString().PadRight(6, ' ') + Convert.ToString(dtIptal.Rows[i]["Rsat_Departman"]).PadLeft(17, ' ').Substring(0, 17));
                }


                iptal.Add("");
                iptal.Add("");
                iptal.Add("");
                iptal.Add(Convert.ToString(dtIptal.Rows[0]["Rsat_IptalNot"]));
                iptal.Add("--------------------------------");
            }

            iptal.Add(".");

            //if (Departman.Kodlar_AndPos_NFC == true)
            //{
            //    DataTable dtHesap = new DataTable();
            //    iptal.Add("İsim Soyisim : " + Fronttools.CardFIsim(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));
            //    iptal.Add(".");
            //    iptal.Add("Bakiye : " + Fronttools.NFCBakiye(Convert.ToInt32(dtHesap.Rows[0]["Rsat_Folio"]), Convert.ToInt32(dtHesap.Rows[0]["Rsat_Kart_ID"])));
            //}


            for (int j = 0; j < bosSatir; j++)
            {
                iptal.Add(".");
            }


            try
            {
                System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                Font fnt = (Font)converter.ConvertFromString(Iptal_Font);

                if (fnt == null)
                {
                    fnt = new Font("Arial", 8);
                }

                Liste = iptal;
                printFont = fnt;
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.PrinterSettings.PrinterName = printer;
                pd.Print();



                iptal.Clear();
            }
            catch (Exception err)
            {
                return err.Message;
            }

            return "OK";
        }


        public string Paket_KapaliPr(DataTable dtPaket)
        {
            List<string> paket = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'PKT'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }


            int dtSatirsay = dtPaket.Rows.Count;

            for (int k = 0; k < Paket_Ciktisayisi; k++)
            {
                decimal toplam = 0;


                paket.Add("------------------------");
                paket.Add("  * * * PAKET RAPOR * * *  ");
                paket.Add("------------------------");
                paket.Add(Param.Tesis_Adi.PadRight(40, ' ').Substring(0, 40));
                paket.Add("Tarih:" + Convert.ToDateTime(dtPaket.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(10, ' '));
                paket.Add("------------------------");

                paket.Add("CARI" + "        " + "FISNO" + "   TUTAR");
                paket.Add("------------------------");
                for (int i = 0; i < dtPaket.Rows.Count; i++)
                {
                    paket.Add(Convert.ToString(dtPaket.Rows[i]["Cari_AdSoyad"]).PadRight(17, ' ').Substring(0, 16) + " " + Convert.ToString(dtPaket.Rows[i]["Rsat_Fisno"]).PadLeft(4, ' ') + Convert.ToDecimal(dtPaket.Rows[i]["Tutar"]).ToString().PadLeft(7, ' '));
                    paket.Add(Convert.ToString(dtPaket.Rows[i]["Rsat_Acilis"]) + " - " + Convert.ToString(dtPaket.Rows[i]["Rsat_Kapanis"]));
                    paket.Add("------------------------");
                    toplam += Convert.ToDecimal(dtPaket.Rows[i]["Tutar"]);
                }
                paket.Add("------------------------");
                paket.Add(" TOPLAM : " + toplam.ToString());
                paket.Add("------------------------");

                paket.Add(".");
                paket.Add(".");
                paket.Add(".");

                for (int j = 0; j < bosSatir; j++)
                {
                    paket.Add(".");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Paket_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = paket;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    paket.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";
        }

        public string KasaMakbuzPr(int MakbuzId)
        {
            List<string> makbuz = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'KSM'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }



            DataTable dtKasaMakbuz = dbtools.SelectTable("select Pkasa_User_Ad,Pkasa_User_Soyad,Pkod_Ad,Pkasa_Tarih,Pkasa_Tutar,Pkasa_Aciklama "
                                + " from Pos_Kasahrk WITH(NOLOCK) "
                                + " left join Pos_Kodlar on Pkod_Kod = Pkasa_Kod and Pkod_Sinif = '22' "
                                + " where (Pkod_Kasagiris = case when Pkasa_GC = 'G' then 1 else 0 end) "
                                + " and Pkasa_Id = '" + MakbuzId + "'");


            for (int k = 0; k < Kasa_Ciktisayisi; k++)
            {
                makbuz.Add("------------------------");
                makbuz.Add("  * * * MAKBUZ FISI * * *  ");
                makbuz.Add("------------------------");

                makbuz.Add("   " + User.P_Ad + " " + User.P_Soyad);
                makbuz.Add(Convert.ToString(dtKasaMakbuz.Rows[0]["Pkod_Ad"]) + " FORMU");
                makbuz.Add("------------------------");
                makbuz.Add("Tutar " + Convert.ToDecimal(dtKasaMakbuz.Rows[0]["Pkasa_Tutar"]).ToString("N2"));
                makbuz.Add("------------------------");
                makbuz.Add("Tahsil Eden");
                makbuz.Add("Adi    : ");
                makbuz.Add("Soyadi : ");
                makbuz.Add("İmzası : ............");
                makbuz.Add(".");
                makbuz.Add("Ödeme Yapan");
                makbuz.Add("Adi    : ");
                makbuz.Add("Soyadi : ");
                makbuz.Add("İmzası : ............");

                makbuz.Add(".");
                makbuz.Add(".");
                makbuz.Add(".");

                for (int j = 0; j < bosSatir; j++)
                {
                    makbuz.Add(".");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Kasa_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = makbuz;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    makbuz.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";
        }

        public string KasaGunlukOzet(DateTime basTar, DateTime bitTar)
        {
            List<string> ozet = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'KSM'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            //string sql = "select Pkod_Kod,Pkod_Ad,SUM(Rsat_Tutar) AS Rsat_Tutar,SUM(Rsat_Doviztutar) AS Rsat_Doviztutar,Mkodlar_Ad as Dovizad  "
            //    + " from Cst_Recete_Satis WITH(NOLOCK) "
            //    + " LEFT JOIN Pos_Kodlar ON Rsat_Kapatma = Pkod_Kod and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' "
            //    + " LEFT JOIN Muh_Kodlar ON Mkodlar_Kod = Rsat_Dovizkodu and Mkodlar_sinif = '02' "
            //    + " WHERE Rsat_Ba = 'A' and convert(date,Rsat_Tarih) = '" + Param.Tarih + "' and #AAA "
            //    + " group by Pkod_Kod,Pkod_Ad,Rsat_Dovizkodu,Mkodlar_Ad "
            //    + " order by Pkod_Kod";

            string sql = "select Pkod_Kod,Pkod_Ad,SUM(Rsat_Tutar) AS Rsat_Tutar,SUM(Rsat_Doviztutar) AS Rsat_Doviztutar,Mkodlar_Ad as Dovizad  "
               + " from Cst_Recete_Satis WITH(NOLOCK) "
               + " LEFT JOIN Pos_Kodlar ON Rsat_Kapatma = Pkod_Kod and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' "
               + " LEFT JOIN Muh_Kodlar ON Mkodlar_Kod = Rsat_Dovizkodu and Mkodlar_sinif = '02' "
               + " WHERE Rsat_Ba = 'A' and convert(date,Rsat_Tarih) between '" + basTar + "' and '" + bitTar + "' and #AAA "
               + " group by Pkod_Kod,Pkod_Ad,Rsat_Dovizkodu,Mkodlar_Ad "
               + " order by Pkod_Kod";

            DataTable dt1 = dbtools.SelectTable(sql.Replace("#AAA", "Pkod_Kasagiris = 1"));
            DataTable dt2 = dbtools.SelectTable(sql.Replace("#AAA", "Pkod_Kasacikis = 1"));

            string sql2 = "select Pkasa_Kod,Pkod_Ad,SUM(Pkasa_Tutar) as Pkasa_Tutar "
                + " from Pos_Kasahrk WITH(NOLOCK) "
                + " LEFT JOIN Pos_Kodlar ON Pkasa_Kod = Pkod_Kod and Pkod_Sinif = '22' "
                + " where convert(date,Pkasa_Tarih) = '" + Param.Tarih + "'  and #AAA "
                + " group by Pkasa_Kod,Pkod_Ad";

            DataTable dt4 = dbtools.SelectTable(sql2.Replace("#AAA", " Pkasa_GC = 'G' and Pkod_Kasagiris = 1 "));
            DataTable dt3 = dbtools.SelectTable(sql2.Replace("#AAA", " Pkasa_GC = 'C' and Pkod_Kasacikis = 1 "));

            decimal tutar1 = 0, tutar2 = 0, tutar3 = 0, tutar4 = 0;

            for (int k = 0; k < Kasa_Ciktisayisi; k++)
            {
                ozet.Add("------------------------");
                ozet.Add("  GUNLUK OZET KASA RAPORU  ");
                ozet.Add("------------------------");
                ozet.Add("");
                ozet.Add("Tarih :" + Param.Tarih.ToString("dd.MM.yyyy"));
                ozet.Add("");
                if (dt1.Rows.Count > 0)
                {
                    ozet.Add("Kasa Girisleri");
                    ozet.Add("------------------------");
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        string ad = Convert.ToString(dt1.Rows[i]["Pkod_Ad"]);
                        decimal tutar = Convert.ToDecimal(dt1.Rows[i]["Rsat_Tutar"]);
                        string doviz = Convert.ToString(dt1.Rows[i]["Dovizad"]);
                        decimal dtutar = Convert.ToDecimal(dt1.Rows[i]["Rsat_Doviztutar"]);

                        ozet.Add(ad.PadRight(15, ' ') + tutar.ToString().PadLeft(8, ' ') + doviz.PadLeft(5, ' ') + dtutar.ToString().PadLeft(8, ' '));
                        tutar1 += tutar;
                    }
                }
                ozet.Add("");

                if (dt2.Rows.Count > 0)
                {
                    ozet.Add("Kasa Cikislari");
                    ozet.Add("------------------------");
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        string ad = Convert.ToString(dt2.Rows[i]["Pkod_Ad"]);
                        decimal tutar = Convert.ToDecimal(dt2.Rows[i]["Rsat_Tutar"]);
                        string doviz = Convert.ToString(dt2.Rows[i]["Dovizad"]);
                        decimal dtutar = Convert.ToDecimal(dt2.Rows[i]["Rsat_Doviztutar"]);

                        ozet.Add(ad.PadRight(15, ' ') + tutar.ToString().PadLeft(8, ' ') + doviz.PadLeft(5, ' ') + dtutar.ToString().PadLeft(8, ' '));
                        tutar2 += tutar;
                    }
                }
                ozet.Add("");

                if (dt4.Rows.Count > 0)
                {
                    ozet.Add("Extra Kasa Girisleri");
                    ozet.Add("------------------------");
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        string ad = Convert.ToString(dt4.Rows[i]["Pkod_Ad"]);
                        decimal tutar = Convert.ToDecimal(dt4.Rows[i]["Pkasa_Tutar"]);

                        ozet.Add(ad.PadRight(15, ' ') + tutar.ToString().PadLeft(8, ' '));
                        tutar4 += tutar;
                    }
                }
                ozet.Add("");
                if (dt3.Rows.Count > 0)
                {
                    ozet.Add("Extra Kasa Cikislari");
                    ozet.Add("------------------------");
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        string ad = Convert.ToString(dt3.Rows[i]["Pkod_Ad"]);
                        decimal tutar = Convert.ToDecimal(dt3.Rows[i]["Pkasa_Tutar"]);

                        ozet.Add(ad.PadRight(15, ' ') + tutar.ToString().PadLeft(8, ' '));
                        tutar3 += tutar;
                    }
                }

                ozet.Add("");
                ozet.Add("");
                ozet.Add("Toplam Giriş : " + (tutar1 + tutar4).ToString("N2"));
                ozet.Add("Toplam Cikis : " + (tutar2 + tutar3).ToString("N2"));
                ozet.Add("");
                ozet.Add("Genel Kasa Durumu : " + ((tutar1 + tutar4) - (tutar2 + tutar3)).ToString("N2"));


                ozet.Add(".");
                ozet.Add(".");
                ozet.Add(".");

                for (int j = 0; j < bosSatir; j++)
                {
                    ozet.Add(".");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Kasa_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = ozet;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();


                    ozet.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";
        }



        public void KasaGunlukOzetYeni(DateTime basTar, DateTime bitTar, string filtreDep = "", string filtreDep2 = "", string filtreDep3 = "")
        {
            try
            {
                string printer = String.Empty;
                DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'KSM'");
                if (dtPrinter.Rows.Count > 0)
                {
                    printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                }

                string sql = "select Rsat_Departman,Pkod_Kod,Pkod_Ad,SUM(Rsat_Tutar) AS Rsat_Tutar,SUM(Rsat_Doviztutar) AS Rsat_Doviztutar,Mkodlar_Ad as Dovizad  "
                   + " from Cst_Recete_Satis WITH(NOLOCK) "
                   + " LEFT JOIN Pos_Kodlar ON Rsat_Kapatma = Pkod_Kod and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' "
                   + " LEFT JOIN Muh_Kodlar ON Mkodlar_Kod = Rsat_Dovizkodu and Mkodlar_sinif = '02' "
                   + " WHERE Rsat_Ba = 'A' " + filtreDep + " and convert(date,Rsat_Tarih) between '" + basTar + "' and '" + bitTar + "' and #AAA "
                   + " group by Rsat_Departman,Pkod_Kod,Pkod_Ad,Rsat_Dovizkodu,Mkodlar_Ad "
                   + " order by Pkod_Kod";

                DataTable dt1 = dbtools.SelectTable(sql.Replace("#AAA", "Pkod_Kasagiris = 1"));
                DataTable dt2 = dbtools.SelectTable(sql.Replace("#AAA", "Pkod_Kasacikis = 1"));

                string sql2 = "select Pkasa_dep,Pkasa_Kod,Pkod_Ad,SUM(Pkasa_Tutar) as Pkasa_Tutar "
                    + " from Pos_Kasahrk WITH(NOLOCK) "
                    + " LEFT JOIN Pos_Kodlar ON Pkasa_Kod = Pkod_Kod and Pkod_Sinif = '22' "
                    + " where convert(date,Pkasa_Tarih) = '" + Param.Tarih + "'  " + filtreDep2 + " and #AAA "
                    + " group by Pkasa_dep,Pkasa_Kod,Pkod_Ad";

                DataTable dt4 = dbtools.SelectTable(sql2.Replace("#AAA", " Pkasa_GC = 'G' and Pkod_Kasagiris = 1 "));
                DataTable dt3 = dbtools.SelectTable(sql2.Replace("#AAA", " Pkasa_GC = 'C' and Pkod_Kasacikis = 1 "));



                //CARİ KASA GİRİŞİ VE ÇIKIŞI
                string sql3 = @"
select Chrk_Depart,Pkod_Kod,Pkod_Ad,SUM(Chrk_Alacak) AS Tutar
from Pos_Carihrk WITH(NOLOCK) 
LEFT JOIN Pos_Kodlar ON Chrk_Odeme = Pkod_Kod and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' 
WHERE Chrk_Alacak > 0 and convert(date,Chrk_Tarih) >= '" + basTar + "' " + filtreDep3 + " and convert(date,Chrk_Tarih) <= '" + bitTar + @"'   and #AAA 
group by Chrk_Depart,Pkod_Kod,Pkod_Ad
order by Pkod_Kod";
                //Chrk_Tarih
                DataTable dtCariGiris = dbtools.SelectTable(sql3.Replace("#AAA", "Pkod_Kasagiris = 1"));
                DataTable dtCariCikis = dbtools.SelectTable(sql3.Replace("#AAA", "Pkod_Kasacikis = 1"));



                decimal tutar1 = 0, tutar2 = 0, tutar3 = 0, tutar4 = 0;

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("odeme", typeof(string));
                dataTable.Columns.Add("tl", typeof(string));
                dataTable.Columns.Add("tur", typeof(string));
                dataTable.Columns.Add("doviz", typeof(string));

                DataRow dataRow = null;
                for (int k = 0; k < Kasa_Ciktisayisi; k++)
                {
                    if (dt1.Rows.Count > 0)
                    {
                        dataRow = dataTable.NewRow();
                        dataRow["tl"] = "KASA.GİR.";
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        //dataRow = dataTable.NewRow();
                        //dataRow["odeme"] = "ÖDEME";
                        //dataRow["tl"] = "TL";
                        //dataRow["tur"] = "TÜR";
                        //dataRow["doviz"] = "DÖVİZ";
                        //dataTable.Rows.Add(dataRow);

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            string ad = Convert.ToString(dt1.Rows[i]["Pkod_Ad"]);
                            decimal tutar = Convert.ToDecimal(dt1.Rows[i]["Rsat_Tutar"]);
                            string doviz = Convert.ToString(dt1.Rows[i]["Dovizad"]);
                            decimal dtutar = Convert.ToDecimal(dt1.Rows[i]["Rsat_Doviztutar"]);

                            if (doviz.Equals(""))
                            {
                                doviz = Param.Doviz_Adi1;
                            }

                            dataRow = dataTable.NewRow();
                            dataRow["odeme"] = ad;
                            dataRow["tl"] = tutar;
                            dataRow["tur"] = doviz;
                            dataRow["doviz"] = dtutar;
                            dataTable.Rows.Add(dataRow);

                            tutar1 += tutar;
                        }
                    }




                    if (dt2.Rows.Count > 0)
                    {
                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataRow["tl"] = "KASA.ÇIK.";
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        //dataRow = dataTable.NewRow();
                        //dataRow["odeme"] = "ÖDEME";
                        //dataRow["tl"] = "TL";
                        //dataRow["tur"] = "TÜR";
                        //dataRow["doviz"] = "DÖVİZ";
                        //dataTable.Rows.Add(dataRow);

                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            string ad = Convert.ToString(dt2.Rows[i]["Pkod_Ad"]);
                            decimal tutar = Convert.ToDecimal(dt2.Rows[i]["Rsat_Tutar"]);
                            string doviz = Convert.ToString(dt2.Rows[i]["Dovizad"]);
                            decimal dtutar = Convert.ToDecimal(dt2.Rows[i]["Rsat_Doviztutar"]);

                            dataRow = dataTable.NewRow();
                            dataRow["odeme"] = ad;
                            dataRow["tl"] = tutar;
                            dataRow["tur"] = doviz;
                            dataRow["doviz"] = dtutar;
                            dataTable.Rows.Add(dataRow);

                            tutar2 += tutar;
                        }
                    }

                    // CARİ GİRİŞ
                    if (dtCariGiris.Rows.Count > 0)
                    {
                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataRow["tl"] = "CARİ.GİR.";
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        for (int i = 0; i < dtCariGiris.Rows.Count; i++)
                        {
                            string ad = Convert.ToString(dtCariGiris.Rows[i]["Pkod_Ad"]);
                            decimal tutar = Convert.ToDecimal(dtCariGiris.Rows[i]["Tutar"]);

                            dataRow = dataTable.NewRow();
                            dataRow["odeme"] = ad;
                            dataRow["tl"] = tutar;
                            dataTable.Rows.Add(dataRow);

                            tutar4 += tutar;
                        }
                    }

                    // CARİ ÇIKIŞ

                    if (dtCariCikis.Rows.Count > 0)
                    {
                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataRow["tl"] = "CARİ.ÇIK.";
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        for (int i = 0; i < dtCariCikis.Rows.Count; i++)
                        {
                            string ad = Convert.ToString(dtCariCikis.Rows[i]["Pkod_Ad"]);
                            decimal tutar = Convert.ToDecimal(dtCariCikis.Rows[i]["Tutar"]);

                            dataRow = dataTable.NewRow();
                            dataRow["odeme"] = ad;
                            dataRow["tl"] = tutar;
                            dataTable.Rows.Add(dataRow);

                            tutar4 += tutar;
                        }
                    }




                    if (dt4.Rows.Count > 0)
                    {
                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataRow["tl"] = "E.KASA.G.";
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        //dataRow = dataTable.NewRow();
                        //dataRow["odeme"] = "ÖDEME";
                        //dataRow["tl"] = "TL";
                        //dataRow["tur"] = "TÜR";
                        //dataRow["doviz"] = "DÖVİZ";
                        //dataTable.Rows.Add(dataRow);

                        for (int i = 0; i < dt4.Rows.Count; i++)
                        {
                            string ad = Convert.ToString(dt4.Rows[i]["Pkod_Ad"]);
                            decimal tutar = Convert.ToDecimal(dt4.Rows[i]["Pkasa_Tutar"]);

                            dataRow = dataTable.NewRow();
                            dataRow["odeme"] = ad;
                            dataRow["tl"] = tutar;
                            dataTable.Rows.Add(dataRow);

                            tutar4 += tutar;
                        }
                    }



                    if (dt3.Rows.Count > 0)
                    {
                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataRow["tl"] = "E.KASA.Ç.";
                        dataTable.Rows.Add(dataRow);

                        dataRow = dataTable.NewRow();
                        dataTable.Rows.Add(dataRow);

                        //dataRow = dataTable.NewRow();
                        //dataRow["odeme"] = "ÖDEME";
                        //dataRow["tl"] = "TL";
                        //dataRow["tur"] = "TÜR";
                        //dataRow["doviz"] = "DÖVİZ";
                        //dataTable.Rows.Add(dataRow);

                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            string ad = Convert.ToString(dt3.Rows[i]["Pkod_Ad"]);
                            decimal tutar = Convert.ToDecimal(dt3.Rows[i]["Pkasa_Tutar"]);

                            dataRow = dataTable.NewRow();
                            dataRow["odeme"] = ad;
                            dataRow["tl"] = tutar;
                            dataTable.Rows.Add(dataRow);

                            tutar3 += tutar;
                        }
                    }

                    garsonTahsilatYaz(dataTable, basTar.ToString("yyyy-MM-dd"), bitTar.ToString("yyyy-MM-dd"), filtreDep);

                    GenelKasaPrint genelKasaPrint = new GenelKasaPrint();
                    genelKasaPrint.DataSource = dataTable;
                    genelKasaPrint.txtTarih.Text = Param.Tarih.ToString("dd.MM.yyyy");
                    genelKasaPrint.txtToplamGiris.Text = (tutar1 + tutar4).ToString("N2");
                    genelKasaPrint.txtToplamCikis.Text = (tutar2 + tutar3).ToString("N2");
                    genelKasaPrint.txtGenelKasaDurum.Text = ((tutar1 + tutar4) - (tutar2 + tutar3)).ToString("N2");
                    genelKasaPrint.PrinterName = printer;
                    genelKasaPrint.Print();

                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "KasaGunlukOzetYeni", "", ex);
            }

        }

        public void garsonTahsilatYaz(DataTable dataTable, string bastar, string bittar, string filtreDep)
        {

            try
            {
                DataRow dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);

                string query = @"        select  
        (LEFT(P_Ad,1) + '.' + P_Soyad+ '  ' +Pkod_Ad) as ad , SUM(Rsat_Tutar) as  Rsat_Tutar ,
        Rsat_Dovizkodu,SUM(Rsat_Doviztutar) as  Rsat_Doviztutar,Rsat_Departman
		from Cst_Recete_Satis 
		left join Rmosmuh.dbo.Pos_User on Rsat_Garson = P_Kod
		left join Pos_Kodlar on Pkod_Sinif = '11' and Pkod_Kod = Rsat_Kapatma
		where 
		convert(date,Rsat_Tarih) >= '" + bastar + @"' and convert(date,Rsat_Tarih) <= '" + bittar + @"' " + filtreDep + @" and
		Rsat_Ba = 'A' and P_Ad is not null
		group by LEFT(P_Ad,1) + '.' + P_Soyad+ '  ' +Pkod_Ad,Rsat_Dovizkodu,Rsat_Departman order by LEFT(P_Ad,1) + '.' + P_Soyad+ '  ' +Pkod_Ad";

                DataTable dt3 = dbtools.SelectTableR(query);

                if (dt3 != null && dt3.Rows.Count > 0)
                {

                    dataRow = dataTable.NewRow();
                    dataRow["tl"] = "G.TAHS.";
                    dataTable.Rows.Add(dataRow);

                    dataRow = dataTable.NewRow();
                    dataTable.Rows.Add(dataRow);

                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        string ad = Convert.ToString(dt3.Rows[i]["ad"]);
                        decimal tutar = Convert.ToDecimal(dt3.Rows[i]["Rsat_Tutar"].ToString());
                        string tur = dt3.Rows[i]["Rsat_Dovizkodu"].ToString();
                        decimal doviz = Convert.ToDecimal(dt3.Rows[i]["Rsat_Doviztutar"].ToString());

                        dataRow = dataTable.NewRow();
                        dataRow["odeme"] = ad;
                        dataRow["tl"] = tutar;
                        dataRow["tur"] = tur;
                        dataRow["doviz"] = doviz;
                        dataTable.Rows.Add(dataRow);

                    }
                }

                dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);
            }
            catch (Exception ex)
            {

            }
        }
        public string KasaGunlukOzetString(DateTime basTar, DateTime bitTar)
        {
            List<string> ozet = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'KSM'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            //string sql = "select Pkod_Kod,Pkod_Ad,SUM(Rsat_Tutar) AS Rsat_Tutar,SUM(Rsat_Doviztutar) AS Rsat_Doviztutar,Mkodlar_Ad as Dovizad  "
            //    + " from Cst_Recete_Satis WITH(NOLOCK) "
            //    + " LEFT JOIN Pos_Kodlar ON Rsat_Kapatma = Pkod_Kod and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' "
            //    + " LEFT JOIN Muh_Kodlar ON Mkodlar_Kod = Rsat_Dovizkodu and Mkodlar_sinif = '02' "
            //    + " WHERE Rsat_Ba = 'A' and convert(date,Rsat_Tarih) = '" + Param.Tarih + "' and #AAA "
            //    + " group by Pkod_Kod,Pkod_Ad,Rsat_Dovizkodu,Mkodlar_Ad "
            //    + " order by Pkod_Kod";

            string sql = "select Pkod_Kod,Pkod_Ad,SUM(Rsat_Tutar) AS Rsat_Tutar,SUM(Rsat_Doviztutar) AS Rsat_Doviztutar,Mkodlar_Ad as Dovizad  "
               + " from Cst_Recete_Satis WITH(NOLOCK) "
               + " LEFT JOIN Pos_Kodlar ON Rsat_Kapatma = Pkod_Kod and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' "
               + " LEFT JOIN Muh_Kodlar ON Mkodlar_Kod = Rsat_Dovizkodu and Mkodlar_sinif = '02' "
               + " WHERE Rsat_Ba = 'A' and convert(date,Rsat_Tarih) between '" + basTar + "' and '" + bitTar + "' and #AAA "
               + " group by Pkod_Kod,Pkod_Ad,Rsat_Dovizkodu,Mkodlar_Ad "
               + " order by Pkod_Kod";

            DataTable dt1 = dbtools.SelectTable(sql.Replace("#AAA", "Pkod_Kasagiris = 1"));
            DataTable dt2 = dbtools.SelectTable(sql.Replace("#AAA", "Pkod_Kasacikis = 1"));

            string sql2 = "select Pkasa_Kod,Pkod_Ad,SUM(Pkasa_Tutar) as Pkasa_Tutar "
                + " from Pos_Kasahrk WITH(NOLOCK) "
                + " LEFT JOIN Pos_Kodlar ON Pkasa_Kod = Pkod_Kod and Pkod_Sinif = '22' "
                + " where convert(date,Pkasa_Tarih) = '" + Param.Tarih + "'  and #AAA "
                + " group by Pkasa_Kod,Pkod_Ad";

            DataTable dt4 = dbtools.SelectTable(sql2.Replace("#AAA", " Pkasa_GC = 'G' and Pkod_Kasagiris = 1 "));
            DataTable dt3 = dbtools.SelectTable(sql2.Replace("#AAA", " Pkasa_GC = 'C' and Pkod_Kasacikis = 1 "));

            decimal tutar1 = 0, tutar2 = 0, tutar3 = 0, tutar4 = 0;



            if (dt1.Rows.Count > 0)
            {
                ozet.Add("Kasa Girisleri");
                ozet.Add("------------------------");
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string ad = Convert.ToString(dt1.Rows[i]["Pkod_Ad"]);
                    decimal tutar = Convert.ToDecimal(dt1.Rows[i]["Rsat_Tutar"]);
                    string doviz = Convert.ToString(dt1.Rows[i]["Dovizad"]);
                    decimal dtutar = Convert.ToDecimal(dt1.Rows[i]["Rsat_Doviztutar"]);

                    int boslukSayisi = 45 - (ad.Length * 2) - 25;

                    for (int k = 0; k < boslukSayisi; k++)
                    {
                        ad = ad + " ";
                    }

                    ad = ad + ":";

                    if (doviz.Equals(""))
                    {
                        doviz = "TL";
                    }

                    ozet.Add(ad + "  " + tutar.ToString() + "  " + doviz + "  " + dtutar.ToString());
                    // ozet.Add(ad.PadRight(15, ' ') + tutar.ToString().PadLeft(8, ' ') + doviz.PadLeft(5, ' ') + dtutar.ToString().PadLeft(8, ' '));
                    tutar1 += tutar;
                }
            }
            ozet.Add("");

            if (dt2.Rows.Count > 0)
            {
                ozet.Add("Kasa Cikislari");
                ozet.Add("------------------------");
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    string ad = Convert.ToString(dt2.Rows[i]["Pkod_Ad"]);
                    decimal tutar = Convert.ToDecimal(dt2.Rows[i]["Rsat_Tutar"]);
                    string doviz = Convert.ToString(dt2.Rows[i]["Dovizad"]);
                    decimal dtutar = Convert.ToDecimal(dt2.Rows[i]["Rsat_Doviztutar"]);

                    int boslukSayisi = 45 - (ad.Length * 2) - 25;

                    for (int k = 0; k < boslukSayisi; k++)
                    {
                        ad = ad + " ";
                    }

                    ad = ad + ":";

                    if (doviz.Equals(""))
                    {
                        doviz = "TL";
                    }
                    ozet.Add(ad + "  " + tutar.ToString() + "  " + doviz + "  " + dtutar.ToString());
                    //ozet.Add(ad.PadRight(15, ' ') + tutar.ToString().PadLeft(8, ' ') + doviz.PadLeft(5, ' ') + dtutar.ToString().PadLeft(8, ' '));
                    tutar2 += tutar;
                }
            }
            ozet.Add("");

            if (dt4.Rows.Count > 0)
            {
                ozet.Add("Extra Kasa Girisleri");
                ozet.Add("------------------------");
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    string ad = Convert.ToString(dt4.Rows[i]["Pkod_Ad"]);
                    decimal tutar = Convert.ToDecimal(dt4.Rows[i]["Pkasa_Tutar"]);

                    ozet.Add(ad.PadRight(15, ' ') + tutar.ToString().PadLeft(8, ' '));
                    tutar4 += tutar;
                }
            }
            ozet.Add("");
            if (dt3.Rows.Count > 0)
            {
                ozet.Add("Extra Kasa Cikislari");
                ozet.Add("------------------------");
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    string ad = Convert.ToString(dt3.Rows[i]["Pkod_Ad"]);
                    decimal tutar = Convert.ToDecimal(dt3.Rows[i]["Pkasa_Tutar"]);

                    ozet.Add(ad.PadRight(15, ' ') + tutar.ToString().PadLeft(8, ' '));
                    tutar3 += tutar;
                }
            }

            ozet.Add("");
            ozet.Add("");
            ozet.Add("Toplam Giriş : " + (tutar1 + tutar4).ToString("N2"));
            ozet.Add("Toplam Cikis : " + (tutar2 + tutar3).ToString("N2"));
            ozet.Add("");
            ozet.Add("Genel Kasa Durumu : " + ((tutar1 + tutar4) - (tutar2 + tutar3)).ToString("N2"));


            ozet.Add(".");
            ozet.Add(".");
            ozet.Add(".");

            for (int j = 0; j < bosSatir; j++)
            {
                ozet.Add(".");
            }




            string bastarih = basTar.ToString("yyyy-MM-dd");
            string bittarih = bitTar.ToString("yyyy-MM-dd");
            DataTable cariOzet = dbtools.SelectTableR($@"            select 
	        Cari_Kod as CARI_KOD,
	        Cari_Ad as ADI,
	        Cari_Soyad as SOYAD,
	        Cari_Tel as TELEFON,
	        Chrk_Tarih as TARIH ,
	        kodlar.Pkod_Ad as ODEME_TURU,
	        ISNULL(SUM(Chrk_Alacak),0) as TAHSILAT
	        from Pos_Cari WITH(NOLOCK)
	        left join Pos_Carihrk WITH(NOLOCK) on Cari_Kod = Chrk_Cari
	        LEFT OUTER JOIN Pos_Kodlar AS Kodlar WITH(NOLOCK) ON Chrk_Odeme = Kodlar.Pkod_Kod and Pkod_Sinif = '11'
	        where Chrk_Borc = 0 and Chrk_Alacak > 0
	        and (CONVERT(date,Chrk_Tarih) >= CONVERT(date,'{bastarih}'))
	        AND	(CONVERT(date,Chrk_Tarih) <= CONVERT(date,'{bittarih}'))
	        group by Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Chrk_Tarih,kodlar.Pkod_Ad 
	        order by Cari_Kod");

            if (cariOzet != null && cariOzet.Rows.Count > 0)
            {
                // Genişlik değerlerini belirleyin
                int adSoyadWidth = 20; // Ad ve Soyad için genişlik
                int odemeTuruWidth = 15; // Ödeme türü için genişlik
                int tahsilatWidth = 10; // Tahsilat için genişlik

                // Başlık kısmını formatla
                string header = $"{"Cari Ad-Soyad".PadRight(adSoyadWidth)}{"Ödeme Türü".PadRight(odemeTuruWidth)}{"TAHSİLAT".PadRight(tahsilatWidth)}";
                ozet.Add(header);

                foreach (DataRow row in cariOzet.Rows)
                {
                    // Veri kısımlarını formatla
                    string adSoyad = $"{row["ADI"]} {row["SOYAD"]}".PadRight(adSoyadWidth);
                    string odemeTuru = $"{row["ODEME_TURU"]}".PadRight(odemeTuruWidth);
                    string tahsilat = $"{row["TAHSILAT"]}".PadLeft(tahsilatWidth + 10);

                    ozet.Add($"{adSoyad.ToLower()}{odemeTuru}{tahsilat}");
                }
            }


            cariOzet = dbtools.SelectTableR($@"            select 
	         kodlar.Pkod_Ad as ODEME_TURU,
	        ISNULL(SUM(Chrk_Alacak),0) as TAHSILAT
	        from Pos_Cari WITH(NOLOCK)
	        left join Pos_Carihrk WITH(NOLOCK) on Cari_Kod = Chrk_Cari
	        LEFT OUTER JOIN Pos_Kodlar AS Kodlar WITH(NOLOCK) ON Chrk_Odeme = Kodlar.Pkod_Kod and Pkod_Sinif = '11'
	        where Chrk_Borc = 0 and Chrk_Alacak > 0
	        and (CONVERT(date,Chrk_Tarih) >= CONVERT(date,'{bastarih}'))
	        AND	(CONVERT(date,Chrk_Tarih) <= CONVERT(date,'{bittarih}'))
	        group by kodlar.Pkod_Ad 
	        ");

            ozet.Add("");
            ozet.Add("");
            ozet.Add("");
            if (cariOzet != null && cariOzet.Rows.Count > 0)
            {
                // Genişlik değerlerini belirleyin
                int odemeTuruWidth = 15; // Ödeme türü için genişlik
                int tahsilatWidth = 10; // Tahsilat için genişlik

                // Başlık kısmını formatla
                string header = $"{"Cari Toplam    ".PadRight(odemeTuruWidth)}{"TAHSİLAT".PadRight(tahsilatWidth)}";
                ozet.Add(header);

                foreach (DataRow row in cariOzet.Rows)
                {
                    // Veri kısımlarını formatla
                    string odemeTuru = $"{row["ODEME_TURU"]}".PadRight(odemeTuruWidth);
                    string tahsilat = $"{row["TAHSILAT"]}".PadLeft(tahsilatWidth + 10);

                    ozet.Add($"{odemeTuru}{tahsilat}");
                }
            }

            string ozetString = String.Join("\n", ozet.ToArray());

            return ozetString;
        }

        public DataTable UrunGrupBul(int Fisno)
        {
            DataTable dtUrungrup = new DataTable();
            SqlConnection con2 = dbtools.conn;
            if (con2.State == ConnectionState.Closed) con2.Open();
            SqlCommand com2 = new SqlCommand();
            com2.Connection = con2;
            com2.CommandType = CommandType.StoredProcedure;
            com2.CommandTimeout = 0;
            com2.CommandText = "Pos_Satis";
            com2.Parameters.AddWithValue("@Fisno", Fisno);
            com2.Parameters.AddWithValue("@Rapor_Tipi", 3);
            SqlDataAdapter da2 = new SqlDataAdapter(com2);
            da2.Fill(dtUrungrup);
            if (con2.State == ConnectionState.Open) con2.Close();

            return dtUrungrup;
        }

        public string MasaTr(int Fisno, string eskiMasano, string yeniMasano)
        {
            List<string> masatr = new List<string>();

            masatr.Add(".");
            masatr.Add("  * * * MASA TRANSFER * * *   ");
            masatr.Add(".");
            masatr.Add("Departman : " + Departman.Dep_Adi);
            masatr.Add("--------------------------------");
            masatr.Add("Eski Masa : " + eskiMasano);
            masatr.Add("--------------------------------");
            masatr.Add("--------------------------------");
            masatr.Add("YENI MASA : " + yeniMasano);
            for (int j = 0; j < 5; j++)
            {
                masatr.Add(".");
            }

            DataTable dtPr = SiparisPrinterBul(Fisno, 0, false);

            try
            {
                for (int i = 0; i < dtPr.Rows.Count; i++)
                {
                    string printer = Convert.ToString(dtPr.Rows[i]["Printer"]);

                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Siparis_Font);
                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }
                    Liste = masatr;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();
                }
            }
            catch (Exception err)
            {
                return err.Message;
            }

            return "OK";
        }

        public DataTable SiparisPrinterBul(int Fisno, int Split, bool SiparisTumYazici)
        {
            string query = "exec Pos_Sorgu @Sorgu_Tipi = 13, @Fisno = '" + Fisno + "', @Split = '" + Split + "', @MacAdres = '" + dbtools.MacAdresi() + "',@SiparisTumYazici = '" + SiparisTumYazici + "' ";
            DataTable dataTable = dbtools.SelectTable(query);

            return dataTable;
        }

        public DataTable ZayiPrinterBul(int Fisno, int Split, bool SiparisTumYazici)
        {
            return dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 28, @Fisno = '" + Fisno + "', @Split = '" + Split + "', @MacAdres = '" + dbtools.MacAdresi() + "',@SiparisTumYazici = '" + SiparisTumYazici + "' ");
        }

        public DataTable AbuyerPrinterBul(int Fisno, int Split, bool SiparisTumYazici)
        {
            string query = "exec Pos_Sorgu @Sorgu_Tipi = 24, @Fisno = '" + Fisno + "', @Split = '" + Split + "', @MacAdres = '" + dbtools.MacAdresi() + "',@SiparisTumYazici = '" + SiparisTumYazici + "' ";
            return dbtools.SelectTable(query);
        }

        public string AbuyerPrinterBulYeni(int Fisno)
        {
            DataTable dataTable = dbtools.SelectTableR("select  konumposta,ustgrup,altgrup from Cst_Recete_Satis where Rsat_Fisno='" + Fisno + "' and konumposta is not null group by konumposta,ustgrup,altgrup");

            string yaziciAd = "";
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow item in dataTable.Rows)
                {
                    string konumposta = item["konumposta"].ToString();
                    string ustgrup = item["ustgrup"].ToString();
                    string altgrup = item["altgrup"].ToString();

                    string query = "select isnull(Pkod_AbuyerPr,'') as Pkod_AbuyerPr from Pos_Kodlar where Pkod_Sinif='16' and Pkod_Posta='" + konumposta + "' and Pkod_Ustgrup='" + ustgrup + "' and Pkod_Altgrup='" + altgrup + "'";
                    yaziciAd = dbtools.DegerGetir(query); // iki tane yazıcı ismi geliyor


                }
            }

            return yaziciAd;
        }

        public string AbuyerPr(int Fisno, bool Mars, int Split, string baslik, string kartDetay1, string kartdetay2, bool hizliSatis)
        {

            try
            {

                int marsim = Mars == true ? 1 : 0;

                DataTable dataTable = dbtools.SelectTableR("select  konumposta,ustgrup,altgrup from Cst_Recete_Satis where Rsat_Fisno='" + Fisno + "' and konumposta is not null group by konumposta,ustgrup,altgrup");

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow item in dataTable.Rows)
                    {
                        string konumposta = item["konumposta"].ToString();
                        string ustgrup = item["ustgrup"].ToString();
                        string altgrup = item["altgrup"].ToString();

                        string query = "select top 1 isnull(Pkod_AbuyerPr,'') as p1,isnull(Pkod_AbuyerPr2,'') as p2 ,isnull(Pkod_AbuyerPr3,'') as p3 ,isnull(Pkod_AbuyerPr4,'') as p4 from Pos_Kodlar where Pkod_Sinif='16' and Pkod_Posta='" + konumposta + "' and Pkod_Ustgrup='" + ustgrup + "' and Pkod_Altgrup='" + altgrup + "'";
                        DataTable data = dbtools.SelectTableR(query); // iki tane yazıcı ismi geliyor

                        if (data == null || data.Rows.Count < 1) return "OK";

                        string abuyerPrintName_1 = data.Rows[0]["p1"].ToString();
                        string abuyerPrintName_2 = data.Rows[0]["p2"].ToString();
                        string abuyerPrintName_3 = data.Rows[0]["p3"].ToString();
                        string abuyerPrintName_4 = data.Rows[0]["p4"].ToString();

                        // önceden @Rapor_Tipi = 9
                        string q2 = @"select ISNULL(Rsat_SiparisPr,0) as Rsat_SiparisPr,Rsat_Id,Rsat_Tarih,CONVERT(varchar,Rsat_Acilis,108) as Rsat_Acilis,Rsat_Fisno,
			isnull(Garson.P_Ad,'') + ' ' + ISNULL(Garson.P_Soyad,'') as Garson,isnull(Garson2.P_Ad,'') +' '+ ISNULL(Garson2.P_Soyad,'') as Garson2,
case when ISNULL(Rsat_OzelMasaAdi,'') = '' then Masa_Ad else (Rsat_OzelMasaAdi + ' - ' + Masa_Ad) end as Rsat_Masa
,Rsat_Miktar,
case when ISNULL(Rsat_Mars,0) = 0 and " + marsim + @" = 1 then '**RZV** ' else '' end + case when ISNULL(Rsat_Yapma,0) = 1 then '[YAPMA] ' else '' end + Rec_Ad + ' ' + ISNULL(Rsat_Joker,'') as Rec_Ad,
ISNULL(Rsat_Aciklama,'') AS Rsat_Aciklama,'' as Pkod_Printer,
case Rsat_Emiktar when 'T' then '' when 'B' then '1BCK' When 'D' THEN 'DBL' When 'Y' THEN 'YRM' When 'A'  then '' else '' end as Rsat_Emiktar,
isnull(Rsat_Tarih,getdate()) as Rsat_Tarih2,isnull(Rsat_Kisi,0) as Rsat_Kisi,MasKonum.Pkod_Ad as MasaKonumAdi,ISNULL(Rsat_SiraAciklama,'') as Rsat_SiraAciklama,konumposta,ustgrup,altgrup,poskod.Pkod_AbuyerPr,isnull(rez.sirano,'') as sirano,isnull(rez.Rsat_Not,'') as Rsat_Not
from Cst_Recete_Satis rez
left join Cst_Recete as rec on rec.Rec_Genelkod=rez.Rsat_Recete
left join Pos_Masa as masa on masa.Masa_No = Rsat_Masa and masa.Masa_Depart = Rsat_Departman
left join pos_kodlar as MasKonum on MasKonum.Pkod_Sinif = '14' and MasKonum.Pkod_Konumkod = masa.Masa_Konum and MasKonum.Pkod_Kod = masa.Masa_Depart 
left join Rmosmuh.dbo.Pos_User as Garson on Garson.P_Kod = Rsat_Garson
left join Rmosmuh.dbo.Pos_User as Garson2 on Garson2.P_Kod = Rsat_Garson2
left join pos_kodlar as poskod on poskod.Pkod_Sinif='16' and poskod.Pkod_Posta=rez.konumposta and poskod.Pkod_Ustgrup=rez.ustgrup and poskod.Pkod_Altgrup=rez.altgrup

where  Rsat_Fisno='" + Fisno + @"' and rec.Rec_SiparisCikmasin=0 and Rsat_SiparisPr=0 and isnull(poskod.Pkod_AbuyerPr,'')<>''";

                        DataTable dtAbuyer = dbtools.SelectTableR(q2);

                        yazdirAbuyerNew(dtAbuyer, abuyerPrintName_1, Fisno, Mars, Split, baslik, kartDetay1, kartdetay2, hizliSatis, abuyerPrintName_2, abuyerPrintName_3, abuyerPrintName_4);

                        break; // abuyer 1 tane olur dedik

                    }
                }




                //dbtools.execcmd(@"update Cst_Recete_Satis set Rsat_AbuyerPr = 1,Rsat_AbuyerPr2 = 1,Rsat_AbuyerPr3 = 1,Rsat_AbuyerPr4 = 1 where Rsat_Fisno = '" + Fisno + "'");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "OK";
        }

        public string AbuyerPrIptalFis(int rsatId)
        {

            try
            {
                DataTable dataTable = dbtools.SelectTableR("select  konumposta,ustgrup,altgrup from Cst_Recete_Satis where Rsat_Id='" + rsatId + "' and konumposta is not null group by konumposta,ustgrup,altgrup");

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow item in dataTable.Rows)
                    {
                        string konumposta = item["konumposta"].ToString();
                        string ustgrup = item["ustgrup"].ToString();
                        string altgrup = item["altgrup"].ToString();

                        string query = "select top 1 isnull(Pkod_AbuyerPr,'') as Pkod_AbuyerPr from Pos_Kodlar where Pkod_Sinif='16' and Pkod_Posta='" + konumposta + "' and Pkod_Ustgrup='" + ustgrup + "' and Pkod_Altgrup='" + altgrup + "'";
                        string yaziciAd = dbtools.DegerGetir(query); // iki tane yazıcı ismi geliyor

                        return yaziciAd;
                    }
                }

            }
            catch (Exception ex)
            {
                return "";
            }

            return "";
        }


        public void yazdirAbuyerNew(DataTable dtAbuyer, string printer, int Fisno, bool Mars, int Split, string baslik, string kartDetay1, string kartdetay2, bool hizliSatis,string abuyerPrintName_2,string abuyerPrintName_3,string abuyerPrintName_4)
        {
            int abuyerCiktisayisi = 1;

            bool abuyerDizayn = true;
            try
            {
                DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'ABUYERR'");

                try
                {
                    abuyerCiktisayisi = Convert.ToInt32(dbtools.DegerGetir("select ISNULL(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif='17' and Pkod_Kod='ABUYERSAYI'"));


                    if (dtDizayn.Rows.Count < 1)
                    {
                        abuyerDizayn = false; // dizayn yoktur
                    }

                }
                catch (Exception ex)
                {

                }

                List<string> abuyer = new List<string>();
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                if (!hizliSatis)
                {
                    string baslikk = baslik;
                    string depAd = Departman.Dep_Adi;
                    string masano = dtAbuyer.Rows[0]["Rsat_Masa"].ToString();
                    string kisiSayisi = dtAbuyer.Rows[0]["Rsat_Kisi"].ToString();
                    string konum = dtAbuyer.Rows[0]["MasaKonumAdi"].ToString();
                    string tarih = Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy");
                    string saat = Convert.ToString(timeSpan);
                    string cekno = dtAbuyer.Rows[0]["Rsat_Fisno"].ToString();
                    string garson = dtAbuyer.Rows[0]["Garson"].ToString();
                    string sirano = dtAbuyer.Rows[0]["sirano"].ToString();
                    string Rsat_Acilis = dtAbuyer.Rows[0]["Rsat_Acilis"].ToString();
                    string not = dtAbuyer.Rows[0]["Rsat_Not"].ToString();

                    Abuyer abuyer1 = new Abuyer();

                    if (abuyerDizayn)
                    {
                        xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), abuyer1);
                    }

                    abuyer1.DataSource = dtAbuyer;
                    abuyer1.PrinterName = printer;

                    abuyer1.xr_MasaNo.Text = masano;
                    abuyer1.xr_Konum.Text = konum;
                    abuyer1.xr_KisiSayisi.Text = kisiSayisi;
                    abuyer1.xr_Tarih.Text = tarih;
                    abuyer1.xr_Acilis.Text = Rsat_Acilis;
                    abuyer1.txtDepartman.Text = Departman.Dep_Adi;
                    abuyer1.txtSiraNo.Text = sirano;
                    abuyer1.xr_Cek.Text = cekno;
                    abuyer1.xr_Garson.Text = garson;
                    abuyer1.xrRsat_Not.Text = not;

                    abuyer1.xr_Urun.Text = "[Rec_Ad]" + ("[Rsat_Aciklama]" == "" ? "" : ("\n" + "[Rsat_Aciklama]"));

                    abuyer1.xr_Miktar.Text = "[Rsat_Miktar]" + " " + "[Rsat_Emiktar]";

                    for (int i = 0; i < abuyerCiktisayisi; i++)
                    {
                        if (abuyer1.PrinterName != "Microsoft Print to PDF" && abuyer1.PrinterName != "") // 
                        {
                            abuyer1.CreateDocument();
                            abuyer1.Print();
                        }

                        if (abuyerPrintName_2!="" && abuyerPrintName_2!= "Microsoft Print to PDF")
                        {
                            abuyer1.PrinterName = abuyerPrintName_2;
                            abuyer1.CreateDocument();
                            abuyer1.Print();
                        }

                        if (abuyerPrintName_3 != "" && abuyerPrintName_3 != "Microsoft Print to PDF")
                        {
                            abuyer1.PrinterName = abuyerPrintName_3;
                            abuyer1.CreateDocument();
                            abuyer1.Print();
                        }


                        if (abuyerPrintName_4 != "" && abuyerPrintName_4 != "Microsoft Print to PDF")
                        {
                            abuyer1.PrinterName = abuyerPrintName_4;
                            abuyer1.CreateDocument();
                            abuyer1.Print();
                        }

                    }

                }


            }
            catch (Exception ex)
            {

                //MessageBox.Show("ABUYER FİŞİ HATA\n" + ex.Message);

            }


        }


        public void yazdirAbuyerNew_eski(DataTable dtAbuyer, string printer, int Fisno, bool Mars, int Split, string baslik, string kartDetay1, string kartdetay2, bool hizliSatis)
        {
            int abuyerCiktisayisi = 1;
            try
            {
                abuyerCiktisayisi = Convert.ToInt32(dbtools.DegerGetir("select ISNULL(Pkod_Ciktisayisi,1) as Pkod_Ciktisayisi from Pos_Kodlar where Pkod_Sinif='17' and Pkod_Kod='ABUYERSAYI'"));
            }
            catch (Exception ex) { }

            List<string> abuyer = new List<string>();
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            abuyer.Add(".");
            //abuyer.Add("   * * * ABUYER FISI * * *  ");
            if (!hizliSatis)
            {
                abuyer.Add(baslik);
                abuyer.Add(" ");
                abuyer.Add("Departman : " + Departman.Dep_Adi);
                abuyer.Add(".");
                abuyer.Add("#Masa :" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Masa"]).PadRight(10, ' ') + "  #Kisi :" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Kisi"]).PadRight(10, ' '));
                abuyer.Add("#Konumu :" + (Convert.ToString(dtAbuyer.Rows[0]["MasaKonumAdi"])).PadRight(7, ' '));
                abuyer.Add(".");
                abuyer.Add("Tarih:" + Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                abuyer.Add(".");
                abuyer.Add("Cekno:" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                abuyer.Add(".");
                abuyer.Add("Grson:" + Convert.ToString(dtAbuyer.Rows[0]["Garson"]));//.PadRight(10, ' '));// + " Grsn2:" + Convert.ToString(dtSiparis.Rows[0]["Garson2"]).PadRight(5, ' '));
                abuyer.Add(".");
                abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
            }
            else
            {
                abuyer.Add(".");
                abuyer.Add("   * * * SIPARIS FISI * * *  ");
                abuyer.Add(".");
                abuyer.Add("Departman : " + Departman.Dep_Adi);
                abuyer.Add("Tarih:" + Convert.ToDateTime(dtAbuyer.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' ') + " Saat:" + Convert.ToString(timeSpan).PadRight(10, ' '));
                abuyer.Add("#Cekno:" + Convert.ToString(dtAbuyer.Rows[0]["Rsat_Fisno"]).PadRight(10, ' '));
                abuyer.Add(".");
                abuyer.Add("MIKTAR".PadRight(13, ' ') + "URUN".PadRight(15, ' '));
            }
            for (int j = 0; j < dtAbuyer.Rows.Count; j++)
            {
                abuyer.Add("".PadLeft(36, '-'));
                if (Param.Param_SiparisSayi == false)
                {
                    abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtAbuyer.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                }
                else
                {
                    abuyer.Add(" " + String.Format("{0:0.##}", Convert.ToDecimal(dtAbuyer.Rows[j]["Rsat_Miktar"])).ToString().PadRight(4, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(8, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                }
                //abuyer.Add(" " + Convert.ToInt32(dtAbuyer.Rows[j]["Rsat_Miktar"]).ToString().PadRight(5, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rsat_Emiktar"]).PadRight(2, ' ') + Convert.ToString(dtAbuyer.Rows[j]["Rec_Ad"]).PadRight(29, ' ').Substring(0, 29));
                if (Convert.ToString(dtAbuyer.Rows[j]["Rsat_Aciklama"]) != String.Empty) abuyer.Add(Convert.ToString(dtAbuyer.Rows[j]["Rsat_Aciklama"]));


                string recAd = dtAbuyer.Rows[j]["Rec_Ad"].ToString();
                string rsatId = dtAbuyer.Rows[j]["Rsat_Id"].ToString();

                if (!recAd.Contains("**RZV**"))
                {
                    dbtools.execcmd(@"update Cst_Recete_Satis set Rsat_AbuyerPr = 1,Rsat_AbuyerPr2 = 1,Rsat_AbuyerPr3 = 1,Rsat_AbuyerPr4 = 1 where Rsat_Id = '" + rsatId + "'");

                }
            }
            abuyer.Add("".PadLeft(36, '-'));

            abuyer.Add(".");
            if (kartDetay1 != "") abuyer.Add(kartDetay1);
            if (kartdetay2 != "") abuyer.Add(kartdetay2);

            for (int j = 0; j < 5; j++)
            {
                abuyer.Add(".");
            }

            //siparis.Add(Convert.ToString((char) 27) + "@" + Convert.ToString((char) 29) + "V" + (char)1);

            try
            {
                System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                Font fnt = (Font)converter.ConvertFromString(Siparis_Font);

                if (fnt == null)
                {
                    fnt = new Font("Arial", 8);
                }

                Liste = abuyer;
                printFont = fnt;
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.PrinterSettings.PrinterName = printer;//printer;

                for (int j = 0; j < abuyerCiktisayisi; j++)
                {
                    pd.Print();
                }

                abuyer.Clear();
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        public string CariHesapPr(string CariKod, DataTable data = null)
        {
            List<string> cari = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }

            DataTable dt = new DataTable();

            if (data == null)
            {
                dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 23, @Cari = '" + CariKod + "' ");

            }
            else
            {
                dt = data;
            }

            cari.Add("");
            cari.Add("#" + Param.Tesis_Adi);
            cari.Add("");
            cari.Add("#Cari : " + Convert.ToString(dt.Rows[0]["Cari_Ad"]) + " " + Convert.ToString(dt.Rows[0]["Cari_Soyad"]));

            cari.Add("");
            decimal bakiye = 0;
            cari.Add("Tarih".PadRight(20, ' ') + "Borc".PadLeft(10, ' ') + "Alacak".PadLeft(10, ' '));
            cari.Add("-".PadRight(40, '-'));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string tarih = Convert.ToDateTime(dt.Rows[i]["Chrk_Tarih"]).ToString("dd.MM.yyyy");
                decimal borc = Convert.ToDecimal(dt.Rows[i]["Chrk_Borc"]);
                decimal alacak = Convert.ToDecimal(dt.Rows[i]["Chrk_Alacak"]);
                string odemeSekli = Convert.ToString(dt.Rows[i]["Pkod_Ad"]);

                bakiye += borc - alacak;

                cari.Add(tarih.PadRight(10, ' ') + odemeSekli.PadLeft(10, ' ') + borc.ToString("N2").PadLeft(10, ' ') + alacak.ToString("N2").PadLeft(10, ' '));
            }

            cari.Add("-".PadRight(40, '-'));
            cari.Add("#Bakiye: " + bakiye.ToString("N2"));
            cari.Add("-".PadRight(40, '-'));
            cari.Add("Düzenleme Tarih = " + DateTime.Now.ToString("dd.MM.yyyy"));


            for (int j = 0; j < bosSatir; j++)
            {
                cari.Add("");
            }

            try
            {
                System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                Font fnt = (Font)converter.ConvertFromString(Cari_Font);

                if (fnt == null)
                {
                    fnt = new Font("Arial", 8);
                }

                Liste = cari;
                printFont = fnt;
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.PrinterSettings.PrinterName = printer;
                pd.Print();

                cari.Clear();
            }
            catch (Exception err)
            {
                return err.Message;
            }


            return "OK";
        }

        public string Fihrist_Adres(int F_Id)
        {
            List<string> fihrist = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Urungrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }



            DataTable dtFihrist = dbtools.SelectTable("select F_Id, F_Ad, F_Soyad, F_Tel1, F_Tel2, F_Adres, F_AdresTarif from Pos_Fihrist where F_Id = " + F_Id);

            int dtSatirsay = dtFihrist.Rows.Count;

            for (int k = 0; k < Hesap_Ciktisayisi; k++)
            {
                fihrist.Add(".");
                fihrist.Add("  * * * FIHRIST ADRES * * *  ");
                fihrist.Add(".");
                fihrist.Add("Ad: " + Convert.ToString(dtFihrist.Rows[0]["F_Ad"]));
                fihrist.Add("Soyad: " + Convert.ToString(dtFihrist.Rows[0]["F_Soyad"]));
                fihrist.Add("Tel1: " + Convert.ToString(dtFihrist.Rows[0]["F_Tel1"]));
                fihrist.Add("Tel2: " + Convert.ToString(dtFihrist.Rows[0]["F_Tel2"]));

                fihrist.Add("#Adres: ");
                fihrist.AddRange(Convert.ToString(dtFihrist.Rows[0]["F_Adres"]).Split('\n'));
                fihrist.Add("#Tarif: ");
                fihrist.AddRange(Convert.ToString(dtFihrist.Rows[0]["F_AdresTarif"]).Split('\n'));
                fihrist.Add(".");

                for (int j = 0; j < bosSatir; j++)
                {
                    fihrist.Add(".");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = fihrist;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    fihrist.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";
        }

        public string Cari_Adres(string Cari_Kod)
        {

            List<string> fihrist = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Urungrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }



            DataTable dtFihrist = dbtools.SelectTable("select Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Tel2,Cari_Adres1,Cari_Adres2,Cari_Adres3 from Pos_Cari where Cari_Kod = '" + Cari_Kod + "'");

            int dtSatirsay = dtFihrist.Rows.Count;

            for (int k = 0; k < Hesap_Ciktisayisi; k++)
            {
                fihrist.Add(".");
                fihrist.Add("  * * * Cari ADRES * * *  ");
                fihrist.Add(".");
                fihrist.Add("Ad: " + Convert.ToString(dtFihrist.Rows[0]["Cari_Ad"]));
                fihrist.Add("Soyad: " + Convert.ToString(dtFihrist.Rows[0]["Cari_Soyad"]));
                fihrist.Add("Tel1: " + Convert.ToString(dtFihrist.Rows[0]["Cari_Tel"]));
                fihrist.Add("Tel2: " + Convert.ToString(dtFihrist.Rows[0]["Cari_Tel2"]));
                fihrist.Add("Adres: " + Convert.ToString(dtFihrist.Rows[0]["Cari_Adres1"]));
                fihrist.Add(Convert.ToString(dtFihrist.Rows[0]["Cari_Adres2"]));
                fihrist.Add(Convert.ToString(dtFihrist.Rows[0]["Cari_Adres3"]));
                fihrist.Add(".");

                for (int j = 0; j < bosSatir; j++)
                {
                    fihrist.Add(".");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = fihrist;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    fihrist.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";

        }

        public string TestPrint()
        {
            List<string> hesap = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Urungrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }


            hesap.Add("[LAYOUT]");
            hesap.Add("<T>Adisyon");
            hesap.Add("<L00>Tarih:{TICKET DATE}");
            hesap.Add("<L00>Saat:{TIME}");
            hesap.Add("<L00>Masa:{ENTITY NAME:Masa}");
            hesap.Add("<L00>Adisyon No:{TICKET NO}");
            hesap.Add("<F>-");
            hesap.Add("{ORDERS}");
            hesap.Add("");
            hesap.Add("[ORDERS]");
            hesap.Add("<L00>- {QUANTITY} {NAME}");
            hesap.Add("{ORDER TAGS}");
            hesap.Add("");
            hesap.Add("[ORDERS:İade]");
            hesap.Add("<J00>- {QUANTITY} {NAME}|**İade**");
            hesap.Add("{ORDER TAGS}");
            hesap.Add("");
            hesap.Add("[ORDER TAGS]");
            hesap.Add("-- Format for order tags");
            hesap.Add("<L00>     * {ORDER TAG NAME}");


            try
            {
                System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                if (fnt == null)
                {
                    fnt = new Font("Arial", 8);
                }

                Liste = hesap;
                printFont = fnt;
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.PrinterSettings.PrinterName = printer;
                pd.Print();

                hesap.Clear();
            }
            catch (Exception err)
            {
                return err.Message;
            }


            return "OK";
        }

        private string Masa_Posta_bul(int Fisno)
        {
            return dbtools.DegerGetir("select top 1 Masa_Posta from Pos_Masa left join Cst_Recete_Satis on Rsat_Masa = Masa_No and Rsat_Departman = Masa_Depart and Rsat_Ba = 'B' where Rsat_Fisno = " + Fisno.ToString());
        }

        public string ExtraFolioPr(string KartNo, int FolioID, string Baslik, decimal Ucret, string Tip, bool AltYazi, string Kurkodu)
        {
            //and (ISNULL(pr.Pkod_Posta,'') = ISNULL(@Posta,'') or ISNULL(pr.Pkod_Posta,'') = '')

            List<string> hesap = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            //string filter = "";
            //string posta = Masa_Posta_bul(Fisno);
            //if (Departman.Kodlar_Pr_Posta)
            //{
            //    filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
            //}
            //else
            //{
            //    filter = " and ISNULL(Pkod_Posta,'') = ''  ";
            //}

            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and (Pkod_Ustgrup = 'BKY' or Pkod_Ustgrup = 'HES') ");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and (Pkod_Ustgrup = 'BKY' or Pkod_Ustgrup = 'HES') and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }


            DataSet ds = new DataSet();
            DataTable dtBilgi = new DataTable();
            DataTable dtBky = new DataTable();


            dtBilgi = Fronttools.SelectTable("select * from KartF where CardF_R_I_H = 'I' and CardF_RezID = '" + FolioID + "' and CardF_No = '" + KartNo + "'");
            dtBky = Fronttools.SelectTable(@"select	ISNULL(SUM(case when Kumhrk_Ba = 'B' then Kumhrk_Tutar else (-1) * Kumhrk_Tutar end),0) as TL_Bakiye, 
ISNULL(SUM(case when Kumhrk_Ba = 'B' then Kumhrk_Doviz_tutar else (-1) * Kumhrk_Doviz_tutar end), 0) as Doviz_Bakiye
from Kumhrk
where Kumhrk_Re = 'E'  and(Kumhrk_Rez_id = (select case when Rez_Master_detay = 'M' then Rez_Id else Rez_Master_id end from Rez where Rez_Id = '" + FolioID + @"') And(ISNULL(Kumhrk_Kartno, '') = '" + KartNo + "'))");

            int dtSatirsay = dtBky.Rows.Count;

            for (int k = 0; k < Hesap_Ciktisayisi; k++)
            {
                //decimal B = 0, A = 0, Bakiye = 0;


                hesap.Add("");
                hesap.Add("");
                hesap.Add("  * * " + Baslik + " * *  ");
                hesap.Add("");
                hesap.Add("");
                hesap.Add("#" + Param.Tesis_Adi.PadRight(40, ' ').Substring(0, 40));
                hesap.Add("");
                //if (Param.Tesis_Tipi == 0)
                //{
                //    hesap.Add(Departman.Sube_Ad.PadRight(40, ' ').Substring(0, 40));
                //    hesap.Add("");
                //    hesap.Add(Departman.Dep_Adi.PadRight(40, ' ').Substring(0, 40));
                //}
                //hesap.Add("");
                //hesap.Add("");
                hesap.Add("Tarih  :" + Convert.ToDateTime(Param.Tarih).ToString("dd.MM.yyyy").PadRight(14, ' '));
                hesap.Add("");
                DateTime sqlTarih = Convert.ToDateTime(dbtools.DegerGetir("select getdate()"));
                hesap.Add("Saat :" + Convert.ToString(sqlTarih.ToShortTimeString()).PadRight(15, ' '));
                hesap.Add("");
                hesap.Add("Kasiyer:" + Convert.ToString(User.P_Ad) + " " + Convert.ToString(User.P_Soyad));
                hesap.Add("");
                hesap.Add("Kart Numarası :" + Convert.ToString(dtBilgi.Rows[0]["CardF_No"]).PadRight(14, ' '));
                hesap.Add("");
                hesap.Add("Adı Soyadı :" + (Convert.ToString(dtBilgi.Rows[0]["CardF_Ad"]) + " " + Convert.ToString(dtBilgi.Rows[0]["CardF_Soyad"])).PadRight(14, ' '));
                hesap.Add("");
                hesap.Add(Tip + " : " + Ucret.ToString("N2") + " " + Fronttools.DovizAdi(Kurkodu));
                hesap.Add("");

                if (dtBky.Rows.Count > 0)
                {
                    if (Param.Fis_Dovizli == 0)
                    {
                        hesap.Add("TOPLAM : ".PadRight(1, ' ') + " " + Math.Abs(Ucret).ToString("N2").PadLeft(2, ' ') + " " + Fronttools.DovizAdi(Kurkodu));
                    }
                    else
                    {
                        hesap.Add("TOPLAM : ".PadRight(1, ' ') + " " + Math.Abs(Ucret).ToString("N2").PadLeft(2, ' ') + " " + Fronttools.DovizAdi(Kurkodu));
                    }
                }

                hesap.Add("---------------------");

                if (Param.Param_Hesap_DovizOzet)
                {
                    DataTable dtDovizDagilim = new DataTable();

                    decimal dagilimTutar = Ucret;

                    if (Param.Kurlar_Nerden == 0)
                    {
                        dtDovizDagilim = Fis_Islem.Doviz_DagilimFront2(dagilimTutar, Kurkodu);
                    }
                    else
                    {
                        dtDovizDagilim = Fis_Islem.Doviz_Dagilim(dagilimTutar);
                    }

                    if (dtDovizDagilim.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDovizDagilim.Rows.Count; i++)
                        {

                            hesap.Add(Convert.ToString(dtDovizDagilim.Rows[i]["Mkodlar_Ad"]).PadRight(15, ' ') + "  " + Convert.ToString(dtDovizDagilim.Rows[i]["Doviz"]));
                            //  (" + Convert.ToString(dtDovizDagilim.Rows[i]["Kur"]) + ") 

                        }
                        hesap.Add("".PadRight(40, '-'));
                    }
                }
                hesap.Add("---------------------");
                hesap.Add("");
                hesap.Add("Teşekkür Ederiz...");
                hesap.Add("");

                if (Param.Tesis_Tipi == 0)
                {
                    hesap.Add("İmza : ");
                    hesap.Add(".");
                    hesap.Add(".");
                    hesap.Add(".");
                }


                if (AltYazi == true)
                {
                    hesap.Add(Convert.ToString(dtBilgi.Rows[0]["CardF_No"]) + " Numaralı");
                    hesap.Add("Hitit Kartını teslim aldım.");
                    hesap.Add("Tesisteki tüm harcamaları (Market Hariç)");
                    hesap.Add("bu kartla yapacağım.");
                    hesap.Add("Kartı kaybetmem durumunda" + "#" + "10 TL");
                    hesap.Add("Kart bedeli ödemek zorunda olduğumu");
                    hesap.Add("karta nakit ödeme yüklenmesi durumunda; ");
                    hesap.Add("kartta kalan bakiye yi nakit olarak alabileceğimi,");
                    hesap.Add("Kredi kartı ile ödeme yüklenmesi ");
                    hesap.Add("durumunda kartta kalan bakiye,");
                    hesap.Add("ödeme yapılan kredi kartına ");
                    hesap.Add("#" + "5(Beş) iş günü içerisinde masraflar");
                    hesap.Add("tarafıma ait olmak üzere işletme ");
                    hesap.Add("tarafından iade edileceği ");
                    hesap.Add("bilgisi tarafıma bildirildi.");
                    hesap.Add("Bu hususları kabul ettiğimi bildiririm.");
                    hesap.Add("");
                    hesap.Add("");
                    hesap.Add("");
                    hesap.Add("");
                    hesap.Add("İMZA");
                    hesap.Add("Konaklayan / Kartı teslim alan");
                    hesap.Add("");
                    hesap.Add("");
                    hesap.Add("");
                    hesap.Add("");
                }

                for (int j = 0; j < bosSatir; j++)
                {
                    hesap.Add(" ");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = hesap;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    hesap.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";
        }

        public string ExtraFolioDetayDokum(int KartID, int FolioID)
        {
            List<string> hesap = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            string filter = "";
            //string posta = Masa_Posta_bul(Fisno);
            //if (Departman.Kodlar_Pr_Posta)
            //{
            //    filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
            //}
            //else
            //{
            //    filter = " and ISNULL(Pkod_Posta,'') = ''  ";
            //}
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and (Pkod_Ustgrup = 'BKY' or Pkod_Ustgrup = 'HES') " + filter);
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and (Pkod_Ustgrup = 'BKY' or Pkod_Ustgrup = 'HES') and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }


            DataSet ds = new DataSet();
            DataTable dtHesap = new DataTable();
            DataTable dtOdeme = new DataTable();
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@KartID", KartID);
            com.Parameters.AddWithValue("@FolioID", FolioID);
            com.Parameters.AddWithValue("@Rapor_Tipi", 27);
            com.Parameters.AddWithValue("@Departman", Departman.Dep_Kodu);
            SqlDataAdapter da = new SqlDataAdapter(com);
            dtHesap = new DataTable();
            da.Fill(dtHesap);


            //dtHesap = ds.Tables[0];
            //dtOdeme = ds.Tables[1];
            if (con.State == ConnectionState.Open) con.Close();

            int dtSatirsay = dtHesap.Rows.Count;

            hesap.Add("");
            hesap.Add("  * * HARCAMA DETAYI * *  ");
            hesap.Add("");
            for (int k = 0; k < Hesap_Ciktisayisi; k++)
            {
                //decimal 0B = 0, A = 0;


                hesap.Add("");
                hesap.Add("  * * HARCAMA DETAYI * *  ");
                hesap.Add("");
                hesap.Add("#" + Param.Tesis_Adi.PadRight(40, ' ').Substring(0, 40));
                if (Param.Tesis_Tipi == 0)
                {
                    hesap.Add(Departman.Sube_Ad.PadRight(40, ' ').Substring(0, 40));
                    hesap.Add(Departman.Dep_Adi.PadRight(40, ' ').Substring(0, 40));
                }
                hesap.Add("");
                if (Param.Param_Adres1 != "") hesap.Add(Param.Param_Adres1);
                if (Param.Param_Adres2 != "") hesap.Add(Param.Param_Adres2);
                if (Param.Param_Adres3 != "") hesap.Add(Param.Param_Adres3);
                if (Param.Param_Adres4 != "") hesap.Add(Param.Param_Adres4);
                if (Param.Param_Adres5 != "") hesap.Add(Param.Param_Adres5);
                hesap.Add("");
                //hesap.Add("Masa : " + Convert.ToString(dtHesap.Rows[0]["MasaAdi"]).PadRight(5, ' ') + " Kişi : " + Convert.ToString(dtHesap.Rows[0]["Rsat_Kisi"]).PadRight(5, ' '));
                //hesap.Add("Kuver  :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Kuver"]).ToString().PadRight(6, ' '));
                hesap.Add("Tarih  :" + Convert.ToDateTime(dtHesap.Rows[0]["Tarih"]).ToString("dd.MM.yyyy").PadRight(14, ' '));
                //hesap.Add("Acilis Saati :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Acilis"]).PadRight(15, ' '));
                //hesap.Add("Kapanis Saat :" + DateTime.Now.ToString("HH:ss:mm"));
                DateTime sqlTarih = Convert.ToDateTime(dbtools.DegerGetir("select getdate()"));
                //hesap.Add("Kapanis Saat :" + sqlTarih.TimeOfDay.ToString());
                //hesap.Add("Cek    :" + Convert.ToString(dtHesap.Rows[0]["Rsat_Fisno"]).PadRight(14, ' '));
                hesap.Add("Kart No  :" + Convert.ToString(dtHesap.Rows[0]["Kart_No"]).PadRight(14, ' '));


                hesap.Add("");
                hesap.Add("URUN".PadRight(40, ' ') + "MİKTAR".PadLeft(10, ' ') + "TUTAR".PadLeft(10, ' '));
                hesap.Add("---------------------------------------------------------------");
                hesap.Add("");
                for (int i = 0; i < dtHesap.Rows.Count; i++)
                {
                    if (Convert.ToString(dtHesap.Rows[i]["Tutar"]) != "")
                    {
                        int Bosluk = 50;
                        int Adi = Convert.ToString(dtHesap.Rows[i]["Departman"]).Length;
                        int Miktar = Convert.ToString(dtHesap.Rows[i]["Miktar"]).Length;
                        hesap.Add(Convert.ToString(dtHesap.Rows[i]["Departman"]).PadRight(Bosluk - Adi, ' ') + Convert.ToDecimal(dtHesap.Rows[i]["Miktar"]).ToString("n2").PadRight(20 - Miktar, ' ') + Convert.ToDecimal(dtHesap.Rows[i]["Tutar"]).ToString("n2").PadRight(14, ' '));
                    }
                    else { hesap.Add(""); }
                }

                hesap.Add("");
                hesap.Add("");

                if (Departman.Kodlar_AndPos_NFC == true)
                {
                    hesap.Add("İsim Soyisim : " + Fronttools.CardFIsim(KartID));
                    hesap.Add("Bakiye : " + Fronttools.NFCBakiye(FolioID, KartID));
                }

                hesap.Add("");


                for (int j = 0; j < bosSatir; j++)
                {
                    hesap.Add("");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                    if (Param.Param_HspFontAlgilama == true)
                    {
                        fnt = null;
                        if (fnt == null)
                        {
                            fnt = new Font("Arial", 8);
                        }
                    }
                    printFont = fnt;

                    Liste = hesap;

                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    hesap.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";
        }

        public string ExtraFolioDokumPr(string KartNo, int FolioID, DateTime Tarih1, DateTime Tarih2)
        {
            //and (ISNULL(pr.Pkod_Posta,'') = ISNULL(@Posta,'') or ISNULL(pr.Pkod_Posta,'') = '')

            List<string> hesap = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;

            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and (Pkod_Ustgrup = 'BKY' or Pkod_Ustgrup = 'HES') ");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }


            DataSet ds = new DataSet();
            DataTable dtHesap = new DataTable();
            DataTable dtOdeme = new DataTable();
            DataTable dtBilgi = new DataTable();


            dtBilgi = Fronttools.SelectTable("select top(1) * from KartF where CardF_R_I_H = 'H' and CardF_RezID = '" + FolioID + "' and CardF_No = '" + KartNo + "' order by ID DESC");

            //DataTable dtHesap = new DataTable();
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Tarih1", Tarih1);
            com.Parameters.AddWithValue("@Tarih2", Tarih2);
            com.Parameters.AddWithValue("@Split", 0);
            com.Parameters.AddWithValue("@Rapor_Tipi", 11);
            com.Parameters.AddWithValue("@KartNo", KartNo);

            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(dtHesap);
            if (con.State == ConnectionState.Open) con.Close();


            decimal kdv18 = 0, kdv8 = 0;


            if (dtHesap.Rows.Count > 0)
            {
                for (int k = 0; k < Hesap_Ciktisayisi; k++)
                {
                    decimal bakiye = 0;

                    hesap.Add(".");
                    hesap.Add("  * * * HAREKET FİŞİ * * *  ");
                    hesap.Add(".");
                    hesap.Add(Param.Tesis_Adi);
                    hesap.Add(".");
                    hesap.Add("Tarh:" + Convert.ToDateTime(dtHesap.Rows[0]["Rsat_Tarih"]).ToString("dd.MM.yyyy").PadRight(10, ' '));
                    hesap.Add("");
                    hesap.Add("Kart Numarası :" + Convert.ToString(dtBilgi.Rows[0]["CardF_No"]).PadRight(14, ' '));
                    hesap.Add("");
                    hesap.Add("Adı Soyadı :" + (Convert.ToString(dtBilgi.Rows[0]["CardF_Ad"]) + " " + Convert.ToString(dtBilgi.Rows[0]["CardF_Soyad"])).PadRight(14, ' '));
                    hesap.Add(".");
                    hesap.Add("URUN".PadRight(17, ' ').Substring(0, 16) + "KDV".PadRight(5, ' ') + "TUTAR".PadRight(7, ' '));
                    hesap.Add("------------------------");
                    for (int i = 0; i < dtHesap.Rows.Count; i++)
                    {
                        if (Param.Fis_Dovizli == 1)         //Dövizli
                        {
                            hesap.Add(Convert.ToString(dtHesap.Rows[i]["Rec_Ad"]).PadRight(17, ' ').Substring(0, 16) + " " + Convert.ToString(" " + String.Format("{0:0.##}", dtHesap.Rows[i]["Rsat_Kdvoran"])).PadLeft(4, ' ') + Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Doviztutar"]).ToString().PadLeft(7, ' '));
                        }
                        else            // TL
                        {
                            hesap.Add(Convert.ToString(dtHesap.Rows[i]["Rec_Ad"]).PadRight(17, ' ').Substring(0, 16) + " " + Convert.ToString(" " + String.Format("{0:0.##}", dtHesap.Rows[i]["Rsat_Kdvoran"])).PadLeft(4, ' ') + Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]).ToString().PadLeft(7, ' '));
                        }

                        //hesap.Add("------------------------");

                        bakiye += Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]);

                        if (Convert.ToString(dtHesap.Rows[i]["Rsat_Ba"]) == "B" && Convert.ToInt32(dtHesap.Rows[i]["Rsat_Kdvoran"]) == 18)
                        {
                            kdv18 += Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]);
                        }
                        else if (Convert.ToString(dtHesap.Rows[i]["Rsat_Ba"]) == "B" && Convert.ToInt32(dtHesap.Rows[i]["Rsat_Kdvoran"]) == 8)
                        {
                            kdv8 += Convert.ToDecimal(dtHesap.Rows[i]["Rsat_Tutar"]);
                        }
                    }
                    hesap.Add("------------------------");
                    hesap.Add(" TOPLAM : " + bakiye.ToString() + " " + Param.Doviz_Adi);
                    hesap.Add("------------------------");
                    hesap.Add(".");

                    hesap.Add("KDV %18 : ".PadRight(5, ' ') + kdv18.ToString("N2"));
                    if (kdv8 > 0)
                    {
                        hesap.Add("KDV %8 : ".PadRight(5, ' ') + kdv8.ToString("N2"));
                    }
                    hesap.Add(".");
                    hesap.Add("Teşekkür Ederiz...");
                    hesap.Add(".");

                    for (int j = 0; j < bosSatir; j++)
                    {
                        hesap.Add(".");
                    }

                    try
                    {
                        System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                        Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                        if (fnt == null)
                        {
                            fnt = new Font("Arial", 8);
                        }

                        Liste = hesap;
                        printFont = fnt;
                        PrintDocument pd = new PrintDocument();
                        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                        pd.PrinterSettings.PrinterName = printer;
                        pd.Print();

                        hesap.Clear();
                    }
                    catch (Exception err)
                    {
                        return err.Message;
                    }
                }
            }
            return "OK";
        }

        public string AnlikPr(GridControl gdc)
        {
            List<string> hesap = new List<string>();

            string printer = String.Empty;
            int bosSatir = 0;
            //string filter = "";
            //string posta = Masa_Posta_bul(Fisno);
            //if (Departman.Kodlar_Pr_Posta)
            //{
            //    filter = " and ISNULL(Pkod_Posta,'') = ISNULL('" + posta + "','')  ";
            //}
            //else
            //{
            //    filter = " and ISNULL(Pkod_Posta,'') = ''  ";
            //}

            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad,Pkod_Satir from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'HES' ");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                bosSatir = Convert.ToInt32(dtPrinter.Rows[0]["Pkod_Satir"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'HES' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }



            DataTable dt = (DataTable)gdc.DataSource;


            for (int k = 0; k < Hesap_Ciktisayisi; k++)
            {
                //decimal B = 0, A = 0, Bakiye = 0;


                hesap.Add("");
                hesap.Add("");
                hesap.Add("  * * ANLIK DURUM RAPORU * *  ");
                hesap.Add("");
                hesap.Add("");
                hesap.Add("#" + Param.Tesis_Adi.PadRight(40, ' ').Substring(0, 40));
                hesap.Add("");
                if (Param.Tesis_Tipi == 0)
                {
                    hesap.Add(Departman.Sube_Ad.PadRight(40, ' ').Substring(0, 40));
                    hesap.Add("");
                    hesap.Add(Departman.Dep_Adi.PadRight(40, ' ').Substring(0, 40));
                }
                hesap.Add("");
                hesap.Add("");
                hesap.Add("Tarih  :" + Convert.ToDateTime(Param.Tarih).ToString("dd.MM.yyyy").PadRight(14, ' '));
                hesap.Add("");
                DateTime sqlTarih = Convert.ToDateTime(dbtools.DegerGetir("select getdate()"));
                hesap.Add("Saat :" + Convert.ToString(sqlTarih.ToShortTimeString()).PadRight(15, ' '));
                hesap.Add("");



                hesap.Add(dt.Rows[0][0].ToString().PadRight(33, ' ') + "    " + dt.Rows[0][1].ToString() + "    " + dt.Rows[0][2].ToString());
                hesap.Add("");
                hesap.Add(dt.Rows[1][0].ToString().PadRight(32, ' ') + "    " + dt.Rows[1][1].ToString() + "    " + dt.Rows[1][2].ToString());
                hesap.Add("");
                hesap.Add(dt.Rows[2][0].ToString().PadRight(10, ' ') + "    " + dt.Rows[2][1].ToString() + "    " + dt.Rows[2][2].ToString());
                hesap.Add("");
                hesap.Add(dt.Rows[3][0].ToString().PadRight(33, ' ') + "    " + dt.Rows[3][1].ToString() + "    " + dt.Rows[3][2].ToString());
                hesap.Add("");
                hesap.Add(dt.Rows[4][0].ToString().PadRight(36, ' ') + "    " + dt.Rows[4][1].ToString() + "    " + dt.Rows[4][2].ToString());

                hesap.Add("-----------------------------");

                hesap.Add("");
                hesap.Add("  * * ÖDEME RAPORU * *  ");
                hesap.Add("");

                DataTable Odeme = dbtools.SelectTable("Exec Pos_Satis_Rapor @Departman = '" + Departman.Dep_Kodu + "', @Tarih1 = '" + Param.Tarih + "', @Rapor_Tipi = 28");

                for (int i = 0; i < Odeme.Rows.Count; i++)
                {
                    hesap.Add(Odeme.Rows[i]["KapamaAd"].ToString().PadRight(35 - Odeme.Rows[i]["KapamaAd"].ToString().Length) + "    " + Odeme.Rows[i]["Rsat_Tutar"].ToString());
                }

                hesap.Add("-----------------------------");

                hesap.Add("");

                DataTable dtKisiSayisi = dbtools.SelectTable("exec Pos_Satis_Rapor @Rapor_Tipi=21,@Tarih1='" + Param.Tarih + "',@Tarih2='" + Param.Tarih + "',@Departman=N'" + Departman.Dep_Kodu + "',@Kullanici=N'" + User.P_Kod + "'");
                if (dtKisiSayisi.Rows.Count > 1)
                {
                    DataRow[] dr = dtKisiSayisi.Select("Tarih = '" + Param.Tarih.ToString("yyyy-MM-dd") + "'");
                    DataTable dts = dr.CopyToDataTable();

                    for (int i = 0; i < dts.Rows.Count; i++)
                    {
                        for (int j = 0; j < dts.Columns.Count; j++)
                        {
                            if (Convert.ToString(dts.Columns[j].Caption) == "MasaSayi")
                            {
                                hesap.Add("Masa Sayisi : ".PadRight(25, ' ') + dts.Rows[i]["MasaSayi"]);
                                hesap.Add("");
                            }

                            if (Convert.ToString(dts.Columns[j].Caption) == "MasaOrtalamasi")
                            {
                                hesap.Add("Masa Ortalamasi : ".PadRight(21, ' ') + dts.Rows[i]["MasaOrtalamasi"]);
                                hesap.Add("");
                            }

                            if (Convert.ToString(dts.Columns[j].Caption) == "KisiSayisi")
                            {
                                hesap.Add("Kişi Sayısı : ".PadRight(28, ' ') + dts.Rows[i]["KisiSayisi"]);
                                hesap.Add("");
                            }

                            if (Convert.ToString(dts.Columns[j].Caption) == "KisiOrtalamasi")
                            {
                                hesap.Add("Kişi Ortalaması : ".PadRight(24, ' ') + dts.Rows[i]["KisiOrtalamasi"]);
                                hesap.Add("");
                            }
                        }
                    }
                }





                hesap.Add("");
                hesap.Add("");
                hesap.Add("");
                hesap.Add("Teşekkür Ederiz...");
                hesap.Add("");


                for (int j = 0; j < bosSatir; j++)
                {
                    hesap.Add(" ");
                }

                try
                {
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(Hesap_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Arial", 8);
                    }

                    Liste = hesap;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrinterSettings.PrinterName = printer;
                    pd.Print();

                    hesap.Clear();
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }

            return "OK";

        }
    }
}
