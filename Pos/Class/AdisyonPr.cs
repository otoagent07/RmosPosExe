using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Pos.Print;
using DevExpress.XtraReports.UI;

namespace Pos.Class
{
    public class AdisyonPr
    {
       
        public string Adisyon_Yaz(int Fisno, bool Detay = false)
        {
            try
            {
                //Printer Seçimi
                string printer = String.Empty;
                DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'ADI'");
                if (dtPrinter.Rows.Count > 0)
                {
                    printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
                }

                DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'ADI' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
                if (dtMacPrinter.Rows.Count > 0)
                {
                    printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
                }

                if (printer == String.Empty)
                {
                    return "Adisyon İçin Printer Seçilmemiş";
                }

                //Dizayn Yuklenmesi
                DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'ADISYON'");
                if (dtDizayn.Rows.Count < 1)
                {
                    return "Adisyon Dizaynı Yapılmamış...";
                }

                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 4);
                com.Parameters.AddWithValue("@Detay", Detay);
                com.Parameters.AddWithValue("@AdisyonCesit", Param.Param_AdisyonDegis);


                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds);

                DataTable dt_Fis = new DataTable();
                dt_Fis = ds.Tables[0];
                DataTable dt_Odeme = new DataTable();
                dt_Odeme = ds.Tables[1];

                if (dt_Fis != null && dt_Fis.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_Fis.Rows)
                    {

                        string emiktar = row["Rsat_Emiktar"].ToString();
                        if (emiktar.Equals("T"))
                        {
                            continue;
                        }

                        if (row["Rsat_Emiktar"] != null && !row["Rsat_Emiktar"].ToString().Equals(""))
                        {
                            row["Rec_Ad"] = row["Rec_Ad"] + "[" + row["Rsat_Emiktar"] + "]";
                        }
                    }
                }

                if (!Detay)
                {
                    //Toplama Göre Boş Satır
                    for (int i = 0; i < Convert.ToInt32(dt_Fis.Rows[0]["Rsat_AdisPrSayac"]); i++)
                    {
                        DataRow row = dt_Fis.NewRow();
                        dt_Fis.Rows.InsertAt(row, 1);
                    }
                }

                Adisyon ads = new Adisyon();
                xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), ads);
                ads.PrinterName = printer;
                ads.DataSource = dt_Fis;


                //Header
                ads.lbl_Departman.Text += Convert.ToString(dt_Fis.Rows[0]["Departman"]);
                ads.lbl_Tarih.Text += Convert.ToDateTime(dt_Fis.Rows[0]["Rsat_Tarih"]).ToShortDateString();
                ads.lbl_Acilissaat.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Acilis"]);
                ads.lbl_Fisno.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Fisno"]);
                ads.lbl_Kasiyer.Text += Convert.ToString(dt_Fis.Rows[dt_Fis.Rows.Count - 2]["Kasiyer"]);
                ads.lbl_Garson.Text += Convert.ToString(dt_Fis.Rows[0]["Garson"]);
                ads.lbl_Masa.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Masa"]) + " - " + Convert.ToString(dt_Fis.Rows[0]["Masa_Ozel"]);
                ads.lbl_Kisi.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Kisi"]);
                //ads.xr_AdSoyad.Text += "";
                //ads.xr_Oda.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Odano"]); ;



                //Detail
                ads.lbl_Malzeme.Text = "[Rec_Ad]".ToString();
                ads.lbl_Kdvoran.Text = String.Format("{0:0.##}", ("[Rsat_Kdvoran]"));
                ads.lbl_Miktar.Text = String.Format("{0:0.##}", ("[Rsat_Miktar]"));
                ads.lbl_Tutar.Text = String.Format("{0:0.##}", ("[Rsat_Tutar]"));
                ads.lbl_AdisPr.Text = "[Rsat_AdisyonPr]".ToString();

                //Footer
                ads.lbl_Indirimtutar.Text = Convert.ToDecimal(dt_Fis.Rows[0]["Ind"]).ToString("n2");
                ads.lbl_Geneltoplam.Text = Convert.ToDecimal(dt_Fis.Rows[0]["Geneltoplam"]).ToString("n2");
                ads.lbl_Doviztoplam.Text = Convert.ToDecimal(dt_Fis.Rows[0]["Doviztoplam"]).ToString("n2") + " " + Param.Doviz_Adi;
                ads.lbl_Kapanissaat.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Kapanis"]);
                ads.lbl_Kasiyer2.Text += Convert.ToString(dt_Fis.Rows[dt_Fis.Rows.Count - 2]["Kasiyer"]);
                //ads.lbl_Bedel.Text = Convert.ToString(dt_Fis.Rows[0]["Bedel"]);
                ads.lbl_Not.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Not"]);


                //System.Threading.Thread.Sleep(500);


                if (dt_Odeme.Rows.Count > 0)
                {
                    //Odeme Tablosu
                    for (int i = 0; i < dt_Odeme.Rows.Count; i++)
                    {
                        XRTableRow row = new XRTableRow();

                        XRTableCell cell1 = new XRTableCell();
                        cell1.Text = Convert.ToString(dt_Odeme.Rows[i]["Pkod_Ad"]);
                        cell1.WidthF = ads.table_Odeme.WidthF * 30 / 100;
                        row.Cells.Add(cell1);

                        XRTableCell cell2 = new XRTableCell();
                        cell2.Text = Convert.ToDecimal(dt_Odeme.Rows[i]["Tutar"]).ToString("n2");
                        cell2.WidthF = ads.table_Odeme.WidthF * 20 / 100;
                        row.Cells.Add(cell2);

                        //if (Param.Tesis_Tipi == 0)
                        //{
                        //    XRTableCell cell3 = new XRTableCell();
                        //    cell3.Text = Fronttools.IsimSoyisim(Convert.ToInt32(dt_Odeme.Rows[i]["Rsat_Folio"]));
                        //    cell3.WidthF = ads.table_Odeme.WidthF * 30 / 100;
                        //    row.Cells.Add(cell3);

                        //    XRTableCell cell4 = new XRTableCell();
                        //    cell4.Text = Convert.ToInt32(dt_Odeme.Rows[i]["Rsat_Folio"]).ToString();
                        //    cell4.WidthF = ads.table_Odeme.WidthF * 25 / 100;
                        //    row.Cells.Add(cell4);
                        //}


                        ads.table_Odeme.Rows.Add(row);
                    }




                    //System.Threading.Thread.Sleep(500);

                    ads.lbl_Kapatma.Text = Convert.ToString(dt_Odeme.Rows[0]["Pkod_Ad"]);
                    ads.lbl_Kapatmatutar.Text = Convert.ToDecimal(dt_Odeme.Rows[0]["Tutar"]).ToString("n2");

                    if (Param.Param_AdisyonFolioAdi == false)
                    {
                        if (Param.Tesis_Tipi == 0)
                        {
                            ads.lbl_Kapatmaisim.Text = Fronttools.IsimSoyisim(Convert.ToInt32(dt_Odeme.Rows[0]["Rsat_Folio"]));
                            ads.lbl_Kapatmafolio.Text = Convert.ToInt32(dt_Odeme.Rows[0]["Rsat_Folio"]).ToString();
                        }
                    }

                    if (Param.Tesis_Tipi == 0 && Param.Fiste_Balance == 0)
                    {
                        ads.lbl_Bakiye.Text += Fronttools.BalanceBul(Fronttools.MasterFolioBul(Convert.ToInt32(dt_Odeme.Rows[0]["Rsat_Folio"])), Kart_No).ToString("N2");
                    }


                    if (dt_Odeme.Rows.Count == 2)
                    {
                        ads.lbl_Kapatma.Text += "/ " + Convert.ToString(dt_Odeme.Rows[1]["Pkod_Ad"]);
                        ads.lbl_Kapatmatutar.Text += "/ " + Convert.ToDecimal(dt_Odeme.Rows[1]["Tutar"]).ToString("n2");
                        if (Param.Tesis_Tipi == 0)
                        {
                            ads.lbl_Kapatmafolio.Text += "/ " + Convert.ToInt32(dt_Odeme.Rows[1]["Rsat_Folio"]).ToString();
                        }
                    }
                }
                //System.Threading.Thread.Sleep(500);

                DataTable dtUrungrup = new DataTable();
                dtUrungrup = UrunGrupBul(Fisno);
                for (int j = 0; j < dtUrungrup.Rows.Count; j++)
                {
                    XRTableRow row = new XRTableRow();

                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = Convert.ToString(dtUrungrup.Rows[j]["Kodlar_Ad"]);
                    cell1.WidthF = ads.table_UrunGrup.WidthF * 60 / 100;
                    row.Cells.Add(cell1);

                    XRTableCell cell2 = new XRTableCell();
                    cell2.Text = Convert.ToDecimal(dtUrungrup.Rows[j]["Tutar"]).ToString("n2");
                    cell2.WidthF = ads.table_UrunGrup.WidthF * 40 / 100;
                    row.Cells.Add(cell2);


                    ads.table_UrunGrup.Rows.Add(row);
                }

                DataTable dtOdemegrup = new DataTable();
                dtOdemegrup = OdemeGrupBul(Fisno);
                for (int j = 0; j < dtOdemegrup.Rows.Count; j++)
                {
                    XRTableRow row = new XRTableRow();

                    XRTableCell cell3 = new XRTableCell();
                    cell3.Text = Convert.ToString(dtOdemegrup.Rows[j]["Kodlar_Ad"]);
                    cell3.WidthF = ads.table_OdemeGrup.WidthF * 60 / 100;
                    row.Cells.Add(cell3);

                    XRTableCell cell4 = new XRTableCell();
                    cell4.Text = Convert.ToDecimal(dtOdemegrup.Rows[j]["Tutar"]).ToString("n2");
                    cell4.WidthF = ads.table_OdemeGrup.WidthF * 40 / 100;
                    row.Cells.Add(cell4);


                    ads.table_OdemeGrup.Rows.Add(row);
                }

                //Kdv Ayrımı
                DataTable dtKdvAyrim = new DataTable();
                dtKdvAyrim = KdvAyrim(Fisno);
                for (int j = 0; j < dtKdvAyrim.Rows.Count; j++)
                {
                    XRTableRow row = new XRTableRow();

                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = String.Format("{0:0.##}", dtKdvAyrim.Rows[j]["Rsat_Kdvoran"]);
                    cell1.WidthF = ads.table_KdvAyrim.WidthF * 60 / 100;
                    row.Cells.Add(cell1);

                    XRTableCell cell2 = new XRTableCell();
                    cell2.Text = String.Format("{0:0.##}", dtKdvAyrim.Rows[j]["Tutar"]);
                    cell2.WidthF = ads.table_KdvAyrim.WidthF * 40 / 100;
                    row.Cells.Add(cell2);


                    ads.table_KdvAyrim.Rows.Add(row);
                }

                //System.Threading.Thread.Sleep(500);

                //Görünüm
                string durum = Convert.ToString(dt_Fis.Rows[0]["Rsat_Durum"]);
                int prSayac = Convert.ToInt32(dt_Fis.Rows[0]["Rsat_AdisPrSayac"]);
                if (prSayac == 0 || Detay == true)
                {
                    ads.pnl_Header.Visible = true;
                }
                else
                {
                    ads.lbl_Departman.Text = " ";
                    ads.lbl_Tarih.Text = " ";
                    ads.lbl_Acilissaat.Text = " ";
                    ads.lbl_Kasiyer.Text = " ";
                    ads.lbl_Garson.Text = " ";
                    ads.lbl_Fisno.Text = " ";
                    ads.lbl_Masa.Text = " ";
                    ads.lbl_Kisi.Text = " ";
                    ads.xrLine1.Text = " ";
                    ads.lbl_Imza.Text = " ";
                    ads.lbl_Isim.Text = " ";
                    ads.lbl_Oda.Text = " ";
                    ads.xrLine2.Text = " ";


                    //ads.lbl_Departman.Visible = false;
                    //ads.lbl_Tarih.Visible = false;
                    //ads.lbl_Acilissaat.Visible = false;
                    //ads.lbl_Kasiyer.Visible = false;
                    //ads.lbl_Garson.Visible = false;
                    //ads.lbl_Fisno.Visible = false;
                    //ads.lbl_Masa.Visible = false;
                    //ads.lbl_Kisi.Visible = false;
                    //ads.xrLine1.Visible = false;
                    //ads.lbl_Imza.Visible = false;
                    //ads.lbl_Isim.Visible = false;
                    //ads.lbl_Oda.Visible = false;
                    //ads.xrLine2.Visible = false;
                    //ads.pnl_Header.HeightF = ads.PageHeader.HeightF;
                }

                if (durum == "A")
                {
                    ads.pnl_Footer.Visible = false;
                }
                else
                {
                    ads.pnl_Footer.Visible = true;
                }

                dbtools.execcmd("update Cst_Recete_Satis set Rsat_AdisyonPr = 1 where Rsat_Fisno ='" + Fisno.ToString() + "'  and  Rsat_Ba='B' ");

                //ads.ShowPreview();
                ads.Print();


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Adisyon_Yaz", "", ex);
            }

            return "OK";
        }

        // aşağıki metot 30.07.2021 e ait
        public string Adisyon_YazEski(int Fisno, bool Detay = false, bool Kapat = false)
        {
            //Printer Seçimi
            string printer = String.Empty;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'ADI'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'ADI' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }

            if (printer == String.Empty)
            {
                return "Adisyon İçin Printer Seçilmemiş";
            }

            //Dizayn Yuklenmesi
            DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'ADISYON'");
            if (dtDizayn.Rows.Count < 1)
            {
                return "Adisyon Dizaynı Yapılmamış...";
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Rapor_Tipi", 4);
            com.Parameters.AddWithValue("@Detay", Detay);
            com.Parameters.AddWithValue("@AdisyonCesit", Param.Param_AdisyonDegis);


            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable dt_Fis = new DataTable();
            dt_Fis = ds.Tables[0];
            DataTable dt_Odeme = new DataTable();
            dt_Odeme = ds.Tables[1];

            if (!Detay)
            {
                //Toplama Göre Boş Satır
                for (int i = 0; i < Convert.ToInt32(dt_Fis.Rows[0]["Rsat_AdisPrSayac"]); i++)
                {
                    DataRow row = dt_Fis.NewRow();
                    dt_Fis.Rows.InsertAt(row, 1);
                }
            }

            Adisyon ads = new Adisyon();
            xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), ads);
            ads.PrinterName = printer;
            ads.DataSource = dt_Fis;


            //Header
            ads.lbl_Departman.Text += Convert.ToString(dt_Fis.Rows[0]["Departman"]);
            ads.lbl_Tarih.Text += Convert.ToDateTime(dt_Fis.Rows[0]["Rsat_Tarih"]).ToShortDateString();
            ads.lbl_Acilissaat.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Acilis"]);
            ads.lbl_Fisno.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Fisno"]);
            ads.lbl_Kasiyer.Text += Convert.ToString(dt_Fis.Rows[dt_Fis.Rows.Count - 2]["Kasiyer"]);
            ads.lbl_Garson.Text += Convert.ToString(dt_Fis.Rows[0]["Garson"]);
            ads.lbl_Masa.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Masa"]) + " - " + Convert.ToString(dt_Fis.Rows[0]["Masa_Ozel"]);
            ads.lbl_Kisi.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Kisi"]);
            //ads.xr_AdSoyad.Text += "";
            //ads.xr_Oda.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Odano"]); ;



            //Detail
            ads.lbl_Malzeme.Text = "[Rec_Ad]".ToString();
            ads.lbl_Kdvoran.Text = String.Format("{0:0.##}", ("[Rsat_Kdvoran]"));
            ads.lbl_Miktar.Text = String.Format("{0:0.##}", ("[Rsat_Miktar]"));
            ads.lbl_Tutar.Text = String.Format("{0:0.##}", ("[Rsat_Tutar]"));
            ads.lbl_AdisPr.Text = "[Rsat_AdisyonPr]".ToString();

            //Footer
            ads.lbl_Indirimtutar.Text = Convert.ToDecimal(dt_Fis.Rows[0]["Ind"]).ToString("n2");
            ads.lbl_Geneltoplam.Text = Convert.ToDecimal(dt_Fis.Rows[0]["Geneltoplam"]).ToString("n2");
            ads.lbl_Doviztoplam.Text = Convert.ToDecimal(dt_Fis.Rows[0]["Doviztoplam"]).ToString("n2") + " " + Param.Doviz_Adi;
            ads.lbl_Kapanissaat.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Kapanis"]);
            ads.lbl_Kasiyer2.Text += Convert.ToString(dt_Fis.Rows[dt_Fis.Rows.Count - 2]["Kasiyer"]);
            //ads.lbl_Bedel.Text = Convert.ToString(dt_Fis.Rows[0]["Bedel"]);
            ads.lbl_Not.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Not"]);


            //System.Threading.Thread.Sleep(500);


            if (dt_Odeme.Rows.Count > 0)
            {
                //Odeme Tablosu
                for (int i = 0; i < dt_Odeme.Rows.Count; i++)
                {
                    XRTableRow row = new XRTableRow();

                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = Convert.ToString(dt_Odeme.Rows[i]["Pkod_Ad"]);
                    cell1.WidthF = ads.table_Odeme.WidthF * 30 / 100;
                    row.Cells.Add(cell1);

                    XRTableCell cell2 = new XRTableCell();
                    cell2.Text = Convert.ToDecimal(dt_Odeme.Rows[i]["Tutar"]).ToString("n2");
                    cell2.WidthF = ads.table_Odeme.WidthF * 20 / 100;
                    row.Cells.Add(cell2);

                    //if (Param.Tesis_Tipi == 0)
                    //{
                    //    XRTableCell cell3 = new XRTableCell();
                    //    cell3.Text = Fronttools.IsimSoyisim(Convert.ToInt32(dt_Odeme.Rows[i]["Rsat_Folio"]));
                    //    cell3.WidthF = ads.table_Odeme.WidthF * 30 / 100;
                    //    row.Cells.Add(cell3);

                    //    XRTableCell cell4 = new XRTableCell();
                    //    cell4.Text = Convert.ToInt32(dt_Odeme.Rows[i]["Rsat_Folio"]).ToString();
                    //    cell4.WidthF = ads.table_Odeme.WidthF * 25 / 100;
                    //    row.Cells.Add(cell4);
                    //}


                    ads.table_Odeme.Rows.Add(row);
                }




                //System.Threading.Thread.Sleep(500);

                ads.lbl_Kapatma.Text = Convert.ToString(dt_Odeme.Rows[0]["Pkod_Ad"]);
                ads.lbl_Kapatmatutar.Text = Convert.ToDecimal(dt_Odeme.Rows[0]["Tutar"]).ToString("n2");

                if (Param.Param_AdisyonFolioAdi == false)
                {
                    if (Param.Tesis_Tipi == 0)
                    {
                        ads.lbl_Kapatmaisim.Text = Fronttools.IsimSoyisim(Convert.ToInt32(dt_Odeme.Rows[0]["Rsat_Folio"]));
                        ads.lbl_Kapatmafolio.Text = Convert.ToInt32(dt_Odeme.Rows[0]["Rsat_Folio"]).ToString();
                    }
                }

                if (Param.Tesis_Tipi == 0 && Param.Fiste_Balance == 0)
                {
                    ads.lbl_Bakiye.Text += Fronttools.BalanceBul(Fronttools.MasterFolioBul(Convert.ToInt32(dt_Odeme.Rows[0]["Rsat_Folio"])), Kart_No).ToString("N2");
                }


                if (dt_Odeme.Rows.Count == 2)
                {
                    ads.lbl_Kapatma.Text += "/ " + Convert.ToString(dt_Odeme.Rows[1]["Pkod_Ad"]);
                    ads.lbl_Kapatmatutar.Text += "/ " + Convert.ToDecimal(dt_Odeme.Rows[1]["Tutar"]).ToString("n2");
                    if (Param.Tesis_Tipi == 0)
                    {
                        ads.lbl_Kapatmafolio.Text += "/ " + Convert.ToInt32(dt_Odeme.Rows[1]["Rsat_Folio"]).ToString();
                    }
                }
            }
            //System.Threading.Thread.Sleep(500);

            DataTable dtUrungrup = new DataTable();
            dtUrungrup = UrunGrupBul(Fisno);
            for (int j = 0; j < dtUrungrup.Rows.Count; j++)
            {
                XRTableRow row = new XRTableRow();

                XRTableCell cell1 = new XRTableCell();
                cell1.Text = Convert.ToString(dtUrungrup.Rows[j]["Kodlar_Ad"]);
                cell1.WidthF = ads.table_UrunGrup.WidthF * 60 / 100;
                row.Cells.Add(cell1);

                XRTableCell cell2 = new XRTableCell();
                cell2.Text = Convert.ToDecimal(dtUrungrup.Rows[j]["Tutar"]).ToString("n2");
                cell2.WidthF = ads.table_UrunGrup.WidthF * 40 / 100;
                row.Cells.Add(cell2);


                ads.table_UrunGrup.Rows.Add(row);
            }

            DataTable dtOdemegrup = new DataTable();
            dtOdemegrup = OdemeGrupBul(Fisno);
            for (int j = 0; j < dtOdemegrup.Rows.Count; j++)
            {
                XRTableRow row = new XRTableRow();

                XRTableCell cell3 = new XRTableCell();
                cell3.Text = Convert.ToString(dtOdemegrup.Rows[j]["Kodlar_Ad"]);
                cell3.WidthF = ads.table_OdemeGrup.WidthF * 60 / 100;
                row.Cells.Add(cell3);

                XRTableCell cell4 = new XRTableCell();
                cell4.Text = Convert.ToDecimal(dtOdemegrup.Rows[j]["Tutar"]).ToString("n2");
                cell4.WidthF = ads.table_OdemeGrup.WidthF * 40 / 100;
                row.Cells.Add(cell4);


                ads.table_OdemeGrup.Rows.Add(row);
            }

            //Kdv Ayrımı
            DataTable dtKdvAyrim = new DataTable();
            dtKdvAyrim = KdvAyrim(Fisno);
            for (int j = 0; j < dtKdvAyrim.Rows.Count; j++)
            {
                XRTableRow row = new XRTableRow();

                XRTableCell cell1 = new XRTableCell();
                cell1.Text = String.Format("{0:0.##}", dtKdvAyrim.Rows[j]["Rsat_Kdvoran"]);
                cell1.WidthF = ads.table_KdvAyrim.WidthF * 60 / 100;
                row.Cells.Add(cell1);

                XRTableCell cell2 = new XRTableCell();
                cell2.Text = String.Format("{0:0.##}", dtKdvAyrim.Rows[j]["Tutar"]);
                cell2.WidthF = ads.table_KdvAyrim.WidthF * 40 / 100;
                row.Cells.Add(cell2);


                ads.table_KdvAyrim.Rows.Add(row);
            }

            //System.Threading.Thread.Sleep(500);

            //Görünüm
            string durum = Convert.ToString(dt_Fis.Rows[0]["Rsat_Durum"]);
            int prSayac = Convert.ToInt32(dt_Fis.Rows[0]["Rsat_AdisPrSayac"]);
            if (prSayac == 0 || Detay == true)
            {
                ads.pnl_Header.Visible = true;
            }
            else
            {
                ads.lbl_Departman.Text = " ";
                ads.lbl_Tarih.Text = " ";
                ads.lbl_Acilissaat.Text = " ";
                ads.lbl_Kasiyer.Text = " ";
                ads.lbl_Garson.Text = " ";
                ads.lbl_Fisno.Text = " ";
                ads.lbl_Masa.Text = " ";
                ads.lbl_Kisi.Text = " ";
                ads.xrLine1.Text = " ";
                ads.lbl_Imza.Text = " ";
                ads.lbl_Isim.Text = " ";
                ads.lbl_Oda.Text = " ";
                ads.xrLine2.Text = " ";


                //ads.lbl_Departman.Visible = false;
                //ads.lbl_Tarih.Visible = false;
                //ads.lbl_Acilissaat.Visible = false;
                //ads.lbl_Kasiyer.Visible = false;
                //ads.lbl_Garson.Visible = false;
                //ads.lbl_Fisno.Visible = false;
                //ads.lbl_Masa.Visible = false;
                //ads.lbl_Kisi.Visible = false;
                //ads.xrLine1.Visible = false;
                //ads.lbl_Imza.Visible = false;
                //ads.lbl_Isim.Visible = false;
                //ads.lbl_Oda.Visible = false;
                //ads.xrLine2.Visible = false;
                //ads.pnl_Header.HeightF = ads.PageHeader.HeightF;
            }

            if (durum == "A")
            {
                ads.pnl_Footer.Visible = false;
            }
            else
            {
                ads.pnl_Footer.Visible = true;
            }

            dbtools.execcmd("update Cst_Recete_Satis set Rsat_AdisyonPr = 1 where Rsat_Fisno ='" + Fisno.ToString() + "'  and  Rsat_Ba='B' ");

            //ads.ShowPreview();
            ads.Print();



            return "OK";
        }
        public static string MyClass = "AdisyonPr";

        public string AdisyonKartID_Yaz(int KartID, bool Detay = false, bool Kapat = false)
        {
            //Printer Seçimi
            string printer = String.Empty;
            DataTable dtPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar with(nolock) where Pkod_Sinif = '16' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Ustgrup = 'ADI'");
            if (dtPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtPrinter.Rows[0]["Pkod_Ad"]);
            }

            DataTable dtMacPrinter = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '21' and Pkod_Ustgrup = 'ADI' and Pkod_Mac = '" + dbtools.MacAdresi() + "'");
            if (dtMacPrinter.Rows.Count > 0)
            {
                printer = Convert.ToString(dtMacPrinter.Rows[0]["Pkod_Ad"]);
            }

            if (printer == String.Empty)
            {
                return "Adisyon İçin Printer Seçilmemiş";
            }

            //Dizayn Yuklenmesi
            DataTable dtDizayn = dbtools.SelectTable("select Rapor_Id From Rapor_Dizayn where Rapor_Kod = 'ADISYON'");
            if (dtDizayn.Rows.Count < 1)
            {
                return "Adisyon Dizaynı Yapılmamış...";
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@KartId", KartID);
            com.Parameters.AddWithValue("@Rapor_Tipi", 44);
            com.Parameters.AddWithValue("@Detay", Detay);
            com.Parameters.AddWithValue("@AdisyonCesit", Param.Param_AdisyonDegis);


            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable dt_Fis = new DataTable();
            dt_Fis = ds.Tables[0];
            DataTable dt_Odeme = new DataTable();
            dt_Odeme = ds.Tables[1];

            if (!Detay)
            {
                //Toplama Göre Boş Satır
                for (int i = 0; i < Convert.ToInt32(dt_Fis.Rows[0]["Rsat_AdisPrSayac"]); i++)
                {
                    DataRow row = dt_Fis.NewRow();
                    dt_Fis.Rows.InsertAt(row, 1);
                }
            }

            Adisyon ads = new Adisyon();
            xtraDizayn.LoadReportStream(Convert.ToString(dtDizayn.Rows[0]["Rapor_Id"]), ads);
            ads.PrinterName = printer;
            ads.DataSource = dt_Fis;


            //Header
            ads.lbl_Departman.Text += Convert.ToString(dt_Fis.Rows[0]["Departman"]);
            ads.lbl_Tarih.Text += Convert.ToDateTime(dt_Fis.Rows[0]["Rsat_Tarih"]).ToShortDateString();
            ads.lbl_Acilissaat.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Acilis"]);
            ads.lbl_Fisno.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Fisno"]);
            ads.lbl_Kasiyer.Text += Convert.ToString(dt_Fis.Rows[dt_Fis.Rows.Count - 2]["Kasiyer"]);
            ads.lbl_Garson.Text += Convert.ToString(dt_Fis.Rows[0]["Garson"]);
            ads.lbl_Masa.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Masa"]) + " - " + Convert.ToString(dt_Fis.Rows[0]["Masa_Ozel"]);
            ads.lbl_Kisi.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Kisi"]);

            //Detail
            ads.lbl_Malzeme.Text = "[Rec_Ad]".ToString();
            ads.lbl_Kdvoran.Text = String.Format("{0:0.##}", ("[Rsat_Kdvoran]"));
            ads.lbl_Miktar.Text = String.Format("{0:0.##}", ("[Rsat_Miktar]"));
            ads.lbl_Tutar.Text = String.Format("{0:0.##}", ("[Rsat_Tutar]"));
            ads.lbl_AdisPr.Text = "[Rsat_AdisyonPr]".ToString();

            //Footer
            ads.lbl_Indirimtutar.Text = Convert.ToDecimal(dt_Fis.Rows[0]["Ind"]).ToString("n2");
            ads.lbl_Geneltoplam.Text = Convert.ToDecimal(dt_Fis.Rows[0]["Geneltoplam"]).ToString("n2");
            ads.lbl_Doviztoplam.Text = Convert.ToDecimal(dt_Fis.Rows[0]["Doviztoplam"]).ToString("n2") + " " + Param.Doviz_Adi;
            ads.lbl_Kapanissaat.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Kapanis"]);
            ads.lbl_Kasiyer2.Text += Convert.ToString(dt_Fis.Rows[dt_Fis.Rows.Count - 2]["Kasiyer"]);
            //ads.lbl_Bedel.Text = Convert.ToString(dt_Fis.Rows[0]["Bedel"]);
            ads.lbl_Not.Text += Convert.ToString(dt_Fis.Rows[0]["Rsat_Not"]);


            //System.Threading.Thread.Sleep(500);


            if (dt_Odeme.Rows.Count > 0)
            {
                //Odeme Tablosu
                for (int i = 0; i < dt_Odeme.Rows.Count; i++)
                {
                    XRTableRow row = new XRTableRow();

                    XRTableCell cell1 = new XRTableCell();
                    cell1.Text = Convert.ToString(dt_Odeme.Rows[i]["Pkod_Ad"]);
                    cell1.WidthF = ads.table_Odeme.WidthF * 30 / 100;
                    row.Cells.Add(cell1);

                    XRTableCell cell2 = new XRTableCell();
                    cell2.Text = Convert.ToDecimal(dt_Odeme.Rows[i]["Tutar"]).ToString("n2");
                    cell2.WidthF = ads.table_Odeme.WidthF * 20 / 100;
                    row.Cells.Add(cell2);

                    //if (Param.Tesis_Tipi == 0)
                    //{
                    //    XRTableCell cell3 = new XRTableCell();
                    //    cell3.Text = Fronttools.IsimSoyisim(Convert.ToInt32(dt_Odeme.Rows[i]["Rsat_Folio"]));
                    //    cell3.WidthF = ads.table_Odeme.WidthF * 30 / 100;
                    //    row.Cells.Add(cell3);

                    //    XRTableCell cell4 = new XRTableCell();
                    //    cell4.Text = Convert.ToInt32(dt_Odeme.Rows[i]["Rsat_Folio"]).ToString();
                    //    cell4.WidthF = ads.table_Odeme.WidthF * 25 / 100;
                    //    row.Cells.Add(cell4);
                    //}


                    ads.table_Odeme.Rows.Add(row);
                }




                //System.Threading.Thread.Sleep(500);

                ads.lbl_Kapatma.Text = Convert.ToString(dt_Odeme.Rows[0]["Pkod_Ad"]);
                ads.lbl_Kapatmatutar.Text = Convert.ToDecimal(dt_Odeme.Rows[0]["Tutar"]).ToString("n2");

                if (Param.Param_AdisyonFolioAdi == false)
                {
                    if (Param.Tesis_Tipi == 0)
                    {
                        ads.lbl_Kapatmaisim.Text = Fronttools.IsimSoyisim(Convert.ToInt32(dt_Odeme.Rows[0]["Rsat_Folio"]));
                        ads.lbl_Kapatmafolio.Text = Convert.ToInt32(dt_Odeme.Rows[0]["Rsat_Folio"]).ToString();
                    }
                }

                if (Param.Tesis_Tipi == 0 && Param.Fiste_Balance == 0)
                {
                    ads.lbl_Bakiye.Text += Fronttools.BalanceBul(Fronttools.MasterFolioBul(Convert.ToInt32(dt_Odeme.Rows[0]["Rsat_Folio"])), Kart_No).ToString("N2");
                }


                if (dt_Odeme.Rows.Count == 2)
                {
                    ads.lbl_Kapatma.Text += "/ " + Convert.ToString(dt_Odeme.Rows[1]["Pkod_Ad"]);
                    ads.lbl_Kapatmatutar.Text += "/ " + Convert.ToDecimal(dt_Odeme.Rows[1]["Tutar"]).ToString("n2");
                    if (Param.Tesis_Tipi == 0)
                    {
                        ads.lbl_Kapatmafolio.Text += "/ " + Convert.ToInt32(dt_Odeme.Rows[1]["Rsat_Folio"]).ToString();
                    }
                }
            }
            //System.Threading.Thread.Sleep(500);

            DataTable dtUrungrup = new DataTable();
            dtUrungrup = UrunGrupBulKartID(KartID);
            for (int j = 0; j < dtUrungrup.Rows.Count; j++)
            {
                XRTableRow row = new XRTableRow();

                XRTableCell cell1 = new XRTableCell();
                cell1.Text = Convert.ToString(dtUrungrup.Rows[j]["Kodlar_Ad"]);
                cell1.WidthF = ads.table_UrunGrup.WidthF * 60 / 100;
                row.Cells.Add(cell1);

                XRTableCell cell2 = new XRTableCell();
                cell2.Text = Convert.ToDecimal(dtUrungrup.Rows[j]["Tutar"]).ToString("n2");
                cell2.WidthF = ads.table_UrunGrup.WidthF * 40 / 100;
                row.Cells.Add(cell2);


                ads.table_UrunGrup.Rows.Add(row);
            }

            DataTable dtOdemegrup = new DataTable();
            dtOdemegrup = OdemeGrupBul(KartID);
            for (int j = 0; j < dtOdemegrup.Rows.Count; j++)
            {
                XRTableRow row = new XRTableRow();

                XRTableCell cell3 = new XRTableCell();
                cell3.Text = Convert.ToString(dtOdemegrup.Rows[j]["Kodlar_Ad"]);
                cell3.WidthF = ads.table_OdemeGrup.WidthF * 60 / 100;
                row.Cells.Add(cell3);

                XRTableCell cell4 = new XRTableCell();
                cell4.Text = Convert.ToDecimal(dtOdemegrup.Rows[j]["Tutar"]).ToString("n2");
                cell4.WidthF = ads.table_OdemeGrup.WidthF * 40 / 100;
                row.Cells.Add(cell4);


                ads.table_OdemeGrup.Rows.Add(row);
            }

            //Kdv Ayrımı
            DataTable dtKdvAyrim = new DataTable();
            dtKdvAyrim = KdvAyrimKartID(KartID);
            for (int j = 0; j < dtKdvAyrim.Rows.Count; j++)
            {
                XRTableRow row = new XRTableRow();

                XRTableCell cell1 = new XRTableCell();
                cell1.Text = String.Format("{0:0.##}", dtKdvAyrim.Rows[j]["Rsat_Kdvoran"]);
                cell1.WidthF = ads.table_KdvAyrim.WidthF * 60 / 100;
                row.Cells.Add(cell1);

                XRTableCell cell2 = new XRTableCell();
                cell2.Text = String.Format("{0:0.##}", dtKdvAyrim.Rows[j]["Tutar"]);
                cell2.WidthF = ads.table_KdvAyrim.WidthF * 40 / 100;
                row.Cells.Add(cell2);


                ads.table_KdvAyrim.Rows.Add(row);
            }

            //System.Threading.Thread.Sleep(500);

            //Görünüm
            string durum = Convert.ToString(dt_Fis.Rows[0]["Rsat_Durum"]);
            int prSayac = Convert.ToInt32(dt_Fis.Rows[0]["Rsat_AdisPrSayac"]);
            if (prSayac == 0 || Detay == true)
            {
                ads.pnl_Header.Visible = true;
            }
            else
            {
                ads.lbl_Departman.Text = " ";
                ads.lbl_Tarih.Text = " ";
                ads.lbl_Acilissaat.Text = " ";
                ads.lbl_Kasiyer.Text = " ";
                ads.lbl_Garson.Text = " ";
                ads.lbl_Fisno.Text = " ";
                ads.lbl_Masa.Text = " ";
                ads.lbl_Kisi.Text = " ";
                ads.xrLine1.Text = " ";
                ads.lbl_Imza.Text = " ";
                ads.lbl_Isim.Text = " ";
                ads.lbl_Oda.Text = " ";
                ads.xrLine2.Text = " ";


                //ads.lbl_Departman.Visible = false;
                //ads.lbl_Tarih.Visible = false;
                //ads.lbl_Acilissaat.Visible = false;
                //ads.lbl_Kasiyer.Visible = false;
                //ads.lbl_Garson.Visible = false;
                //ads.lbl_Fisno.Visible = false;
                //ads.lbl_Masa.Visible = false;
                //ads.lbl_Kisi.Visible = false;
                //ads.xrLine1.Visible = false;
                //ads.lbl_Imza.Visible = false;
                //ads.lbl_Isim.Visible = false;
                //ads.lbl_Oda.Visible = false;
                //ads.xrLine2.Visible = false;
                //ads.pnl_Header.HeightF = ads.PageHeader.HeightF;
            }

            if (durum == "A")
            {
                ads.pnl_Footer.Visible = false;
            }
            else
            {
                ads.pnl_Footer.Visible = true;
            }

            dbtools.execcmd("update Cst_Recete_Satis set Rsat_AdisyonPr = 1 where Rsat_Kart_ID ='" + KartID.ToString() + "'  and  Rsat_Ba='B' ");

            //ads.ShowPreview();
            ads.Print();



            return "OK";
        }

        private DataTable KdvAyrim(int Fisno)
        {
            DataTable dt = dbtools.SelectTable(@"declare @Fis_Tutar decimal(18,2) = (select SUM(Rsat_Tutar) FROM Cst_Recete_Satis WITH(NOLOCK) 
                where Rsat_Fisno = '" + Fisno.ToString() + "' and Rsat_Ba = 'B' and ISNULL(Rsat_Ikram,0) = 0 ) "
                        + " declare @Alacak_Tutar decimal(18,2) = (select SUM(Rsat_Tutar) FROM Cst_Recete_Satis as odeme WITH(NOLOCK) LEFT JOIN Pos_Kodlar as  kodlar WITH(NOLOCK) ON odeme.Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' where Rsat_Fisno = '" + Fisno.ToString() + "' and Pkod_Ozelkod <> '4' and Rsat_Ba = 'A') "
                        + " declare @Katsayi decimal(18,8) = (@Alacak_Tutar / @Fis_Tutar ) "
                        + " if ISNULL(@Katsayi,0) = 0 begin set @Katsayi = 1 end "
                        + " SELECT Rsat_Kdvoran,(SUM(Rsat_Kdv)) * @Katsayi as Tutar  "
                        + " FROM Cst_Recete_Satis WITH(NOLOCK) "
                        + " LEFT JOIN Cst_Recete WITH(NOLOCK) on Rec_Genelkod = Rsat_Recete "
                        + " LEFT JOIN Stok_Kodlar WITH(NOLOCK) on Rec_Urungrup = Kodlar_Kod AND Kodlar_Sinif = '10' "
                        + " WHERE Rsat_Fisno = '" + Fisno.ToString() + "' AND Rsat_Ba = 'B' and ISNULL(Rsat_Ikram,0) = 0 "
                        + " GROUP BY Rsat_Kdvoran "
                        + " ORDER BY Rsat_Kdvoran desc");

            return dt;
        }

        private DataTable KdvAyrimKartID(int KartID)
        {
            DataTable dt = dbtools.SelectTable(@"declare @Fis_Tutar decimal(18,2) = (select SUM(Rsat_Tutar) FROM Cst_Recete_Satis WITH(NOLOCK) 
                where Rsat_Kart_ID = '" + KartID.ToString() + "' and Rsat_Ba = 'B' and ISNULL(Rsat_Ikram,0) = 0 ) "
                        + " declare @Alacak_Tutar decimal(18,2) = (select SUM(Rsat_Tutar) FROM Cst_Recete_Satis as odeme WITH(NOLOCK) LEFT JOIN Pos_Kodlar as  kodlar WITH(NOLOCK) ON odeme.Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' where Rsat_Kart_ID = '" + KartID.ToString() + "' and Pkod_Ozelkod <> '4' and Rsat_Ba = 'A') "
                        + " declare @Katsayi decimal(18,8) = (@Alacak_Tutar / @Fis_Tutar ) "
                        + " if ISNULL(@Katsayi,0) = 0 begin set @Katsayi = 1 end "
                        + " SELECT Rsat_Kdvoran,(SUM(Rsat_Kdv)) * @Katsayi as Tutar  "
                        + " FROM Cst_Recete_Satis WITH(NOLOCK) "
                        + " LEFT JOIN Cst_Recete WITH(NOLOCK) on Rec_Genelkod = Rsat_Recete "
                        + " LEFT JOIN Stok_Kodlar WITH(NOLOCK) on Rec_Urungrup = Kodlar_Kod AND Kodlar_Sinif = '10' "
                        + " WHERE Rsat_Kart_ID = '" + KartID.ToString() + "' AND Rsat_Ba = 'B' and ISNULL(Rsat_Ikram,0) = 0 "
                        + " GROUP BY Rsat_Kdvoran "
                        + " ORDER BY Rsat_Kdvoran desc");

            return dt;
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

        public DataTable UrunGrupBulKartID(int KartID)
        {
            DataTable dtUrungrup = new DataTable();
            SqlConnection con2 = dbtools.conn;
            if (con2.State == ConnectionState.Closed) con2.Open();
            SqlCommand com2 = new SqlCommand();
            com2.Connection = con2;
            com2.CommandType = CommandType.StoredProcedure;
            com2.CommandTimeout = 0;
            com2.CommandText = "Pos_Satis";
            com2.Parameters.AddWithValue("@KartId", KartID);
            com2.Parameters.AddWithValue("@Rapor_Tipi", 33);
            SqlDataAdapter da2 = new SqlDataAdapter(com2);
            da2.Fill(dtUrungrup);
            if (con2.State == ConnectionState.Open) con2.Close();

            return dtUrungrup;
        }

        public DataTable OdemeGrupBul(int Fisno)
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
            com2.Parameters.AddWithValue("@Rapor_Tipi", 12);
            SqlDataAdapter da2 = new SqlDataAdapter(com2);
            da2.Fill(dtUrungrup);
            if (con2.State == ConnectionState.Open) con2.Close();

            return dtUrungrup;
        }

        public DataTable OdemeGrupBulKartID(int KartID)
        {
            DataTable dtUrungrup = new DataTable();
            SqlConnection con2 = dbtools.conn;
            if (con2.State == ConnectionState.Closed) con2.Open();
            SqlCommand com2 = new SqlCommand();
            com2.Connection = con2;
            com2.CommandType = CommandType.StoredProcedure;
            com2.CommandTimeout = 0;
            com2.CommandText = "Pos_Satis";
            com2.Parameters.AddWithValue("@KartId", KartID);
            com2.Parameters.AddWithValue("@Rapor_Tipi", 122);
            SqlDataAdapter da2 = new SqlDataAdapter(com2);
            da2.Fill(dtUrungrup);
            if (con2.State == ConnectionState.Open) con2.Close();

            return dtUrungrup;
        }

        string Kart_No = String.Empty;

        public bool Adisyon_Sayac_Arttir(int Fisno)
        {
            return dbtools.execcmd("update Cst_Recete_Satis set Rsat_AdisPrSayac = isnull(Rsat_AdisPrSayac,0) + 1 where Rsat_Fisno ='" + Fisno.ToString() + "'  and  Rsat_Ba='B' ");
        }

        public bool Adisyon_Sayac_Sifirla(int Fisno)
        {
            return dbtools.execcmd("update Cst_Recete_Satis set Rsat_AdisPrSayac = 0,Rsat_AdisyonPr = 0 where Rsat_Fisno ='" + Fisno.ToString() + "' ");
        }

        public string Adisyon_Yaz2(int Fisno)
        {

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Rapor_Tipi", 4);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);


            Microsoft.Reporting.WinForms.ReportViewer view = new Microsoft.Reporting.WinForms.ReportViewer();

            view.LocalReport.ReportPath = @"C:\Users\Öner\Desktop\Pos\Pos\Print\Report1.rdlc";

            //view.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("Table", ds.Tables[0]));

            view.Refresh();

            view.PrintDialog();

            return "OK";
        }

    }
}
