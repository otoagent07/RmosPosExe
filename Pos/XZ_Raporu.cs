using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Drawing.Printing;
using Pos.Class;
using System.Resources;
using System.Reflection;
using DevExpress.XtraGrid;
using System.IO;
using System.Diagnostics;
using DevExpress.XtraPrinting;

namespace Pos
{
    public partial class XZ_Raporu : DevExpress.XtraEditors.XtraForm
    {
        public XZ_Raporu()
        {
            InitializeComponent();
        }

        public string setDepartman = null;
        private void Xz_Raporu_Load(object sender, EventArgs e)
        {
            string query = @"SELECT Pkod_Ad FROM Pos_Kodlar  
where Pkod_Sinif = '16' and Pkod_Ustgrup = 'HES'
group by Pkod_Ad";

            DataTable dataTable = dbtools.SelectTableR(query);
            lookUpEditYazici.Properties.DataSource = dataTable;
            lookUpEditYazici.Properties.ValueMember = "Pkod_Ad";
            lookUpEditYazici.Properties.DisplayMember = "Pkod_Ad";


            string yazici = dbtools.DegerGetir("select top 1 isnull(xzraporyazici,0) as xzraporyazici from Rmosmuh.dbo.Pos_User_XZ where P_Kod='" + User.P_Kod + "'");

            if (yazici.Equals("0"))
            {
                lookUpEditYazici.EditValue = dataTable.Rows[0][0].ToString();

            }
            else
            {
                lookUpEditYazici.EditValue = yazici;

            }

            this.BringToFront();
            dateTarih.DateTime = Param.Tarih.Date;

            rdo_X_Z.SelectedIndex = 1;


            DataTable dtGarson = dbtools.SelectTable("select P_Kod, P_Ad+' '+P_Soyad as P_Ad from Rmosmuh.dbo.Pos_User order by P_Kod");
            if (dtGarson.Rows.Count > 0)
            {
                look_Garson.Properties.DataSource = dtGarson;
                look_Garson.Properties.ValueMember = "P_Kod";
                look_Garson.Properties.DisplayMember = "P_Ad";
                look_Garson.ItemIndex = 0;
            }

            string filter = "";
            if (User.P_Departman != "")
            {
                filter = " AND Kodlar_Kod IN ('" + User.P_Departman.Replace(", ", "','") + "')";
            }

            DataTable dt_Dep = dbtools.SelectTable("select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar with(nolock) where Kodlar_Sinif ='01' and Kodlar_Anadepo = 'False' and Kodlar_Satis = 1 " + filter + " order by Kodlar_Kod");
            cmb_Departman.Properties.DataSource = dt_Dep;
            cmb_Departman.Properties.DisplayMember = "Kodlar_Ad";
            cmb_Departman.Properties.ValueMember = "Kodlar_Kod";
            cmb_Departman.SetEditValue(setDepartman);



            chk_Odeme.Checked = User.XZ_Odeme;
            chk_Servis.Checked = User.XZ_Servis;
            chk_Cari.Checked = User.XZ_Cari;
            chk_Odenmez.Checked = User.XZ_Odenmez;
            chk_Malzeme.Checked = User.XZ_Malzeme;
            chk_Anagrup.Checked = User.XZ_Anagrup;
            chk_Altgrup.Checked = User.XZ_Altgrup;
            chk_Iptal.Checked = User.XZ_Iptal;
            chk_PaketServis.Checked = User.XZ_PaketServis;
            chk_IndirimMasa.Checked = User.XZ_IndirimMasa;
            chk_YiyecekIcecek.Checked = User.XZ_YiyecekIcecek;
            chk_MasaKonum.Checked = User.XZ_MasaKonum;
            chk_GarsonOzet.Checked = User.XZ_GarsonOzet;
            chk_GarsonTahsil.Checked = User.XZ_GarsonTahsil;
            chk_SifirTutar.Checked = User.XZ_SifirTutar;
            chk_OzetKasa.Checked = User.XZ_OzetKasa;
            chk_ExtKasaRapor.Checked = User.XZ_ExtKasaRapor;
            chk_ExtKasaDetay.Checked = User.XZ_ExtKasaDetay;
            chk_SiparisIptal.Checked = User.XZ_SiparisIptal;
            chk_GonderilmemisSiparisIptal.Checked = User.XZ_GonderilmemisSiparisIptal;
            chk_SiparisDuzelt.Checked = User.XZ_SiparisDuzelt;


        }

        private void Fis_Print(DataTable dt)
        {
            List<string> list;

            DataTable dt3 = dbtools.SelectTable("SELECT Pkod_Ad, Pkod_Satir FROM Pos_Kodlar  where Pkod_Sinif = '16' and Pkod_Ustgrup = 'HES' and Pkod_Kod = '" + Departman.Dep_Kodu + "' ");
            if (dt3.Rows.Count > 0 )
            {
                string Printer = dt3.Rows[0]["Pkod_Ad"].ToString();
                int Bos_Satir = Convert.ToInt32(dt3.Rows[0]["Pkod_Satir"].ToString());

                string tesis_Adi = Param.Tesis_Adi;

                list = new List<string>();

                if (rdo_X_Z.SelectedIndex == 0) list.Add("    ***  X RAPORU  ***");
                if (rdo_X_Z.SelectedIndex == 1) list.Add("    ***  Z RAPORU  ***");
                list.Add(". ");
                list.Add("    " + tesis_Adi);
                list.Add("    " + dateTarih.DateTime.ToShortDateString());
                if (rdo_X_Z.SelectedIndex == 0) list.Add("    " + "Garson: " + look_Garson.Text);
                list.Add("    " + DateTime.Now.ToShortTimeString());
                list.Add(".");
                list.Add("--------------------------------");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["Aciklama"]) == "---")
                    {
                        list.Add("--------------------------------");
                    }
                    else if (Convert.ToString(dt.Rows[i]["Aciklama"]).StartsWith("#"))
                    {
                        list.Add(Convert.ToString(dt.Rows[i]["Aciklama"]));
                    }
                    else
                    {
                        list.Add(Convert.ToString(dt.Rows[i]["Aciklama"]).PadRight(27, " "[0]).Substring(0, 27) + " " + Convert.ToString(dt.Rows[i]["Miktar"]).PadLeft(5, " "[0]) + " " + Convert.ToString(dt.Rows[i]["Tutar"]).PadLeft(8, " "[0]));
                    }
                }

                for (int x = 0; x < Bos_Satir + 5; x++)
                {
                    list.Add(".");
                }



                try
                {
                    string XZ_Font = dbtools.DegerGetir("select Pkod_Font from Pos_Kodlar where Pkod_Sinif = '17'  and Pkod_Kod = 'XZRAPOR'");
                    System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Font));
                    Font fnt = (Font)converter.ConvertFromString(XZ_Font);

                    if (fnt == null)
                    {
                        fnt = new Font("Lucida Console", 9);
                    }


                    stringToPrint = string.Join(Environment.NewLine, list.ToArray());

                    if (checkEditPdfKaydet.Checked)
                    {

                        printFont = fnt;
                        PrintDocument pd = new PrintDocument();
                        pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

                        Printer = lookUpEditYazici.EditValue.ToString();
                        pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                        pd.Print();

                        //DataTable dataTable = new DataTable();
                        //dataTable.Columns.Add("XZ RAPOR");
                        //DataRow dr = dataTable.NewRow();
                        //dr["XZ RAPOR"] = stringToPrint;
                        //dataTable.Rows.Add(dr);
                        //gridControl1.DataSource = dataTable;
                        //yazdir(gridControl1);
                    }
                    else
                    {
                        //Liste = list;
                        printFont = fnt;
                        PrintDocument pd = new PrintDocument();
                        pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

                        Printer = lookUpEditYazici.EditValue.ToString();
                        pd.PrinterSettings.PrinterName = Printer;
                        pd.Print();
                    }
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(res_man.GetString("Bu Departmana Ait Yazıcı Ayarlarını Kontrol Ediniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        string stringToPrint = "";
        //List<string> Liste;
        private Font printFont;
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
            //ev.Graphics.DrawString(Liste[i], printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
            //count++;




            float yPos = 0;
            int count = 0;
            float leftMargin = 1;
            float topMargin = 3;

            int charactersOnPage = 0;
            int linesPerPage = 0;
            e.Graphics.MeasureString(stringToPrint, printFont, e.PageBounds.Size, new StringFormat(), out charactersOnPage, out linesPerPage);


            string[] yazi = stringToPrint.Split('\n');
            for (int i = 0; i < yazi.Length; i++)
            {
                yPos = topMargin + (count * printFont.GetHeight(e.Graphics));
                e.Graphics.DrawString(yazi[i], printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }

            stringToPrint = stringToPrint.Substring(charactersOnPage);

            e.HasMorePages = (stringToPrint.Length > 0);
        }

        private void rdo_X_Z_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdo_X_Z.SelectedIndex == 0)
            {
                textEdit3.Visible = true;
                look_Garson.Visible = true;
                look_Garson.ItemIndex = 0;
            }
            else
            {
                textEdit3.Visible = false;
                look_Garson.Visible = false;
                look_Garson.ItemIndex = -1;
            }
        }

        public bool getSadeceDepXz()
        {
            try
            {
                string deger = dbtools.DegerGetir("SELECT top 1 isnull(Pos_XZdepartman,0) as Pos_XZdepartman  FROM RmosMuh.dbo.Pos_User where P_Kod='" + User.P_Kod + "'");
                return Convert.ToBoolean(deger);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void yazdir(GridControl gridControl)
        {
            
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Pdf File (.pdf)|*.pdf |Excel (2010) (.xlsx)|*.xlsx|Excel (2003)(.xls)|*.xls|RichText File (.rtf)|*.rtf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".xls":
                            gridControl.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gridControl.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            gridControl.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            gridControl.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            gridControl.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            gridControl.ExportToMht(exportFilePath);
                            break;
                        default:
                            break;
                    }

                    //string dosyaKonum = Path.GetDirectoryName(Application.ExecutablePath);

                    string basePath = Path.GetDirectoryName(exportFilePath);

                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(basePath)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
            }
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "Pos_XZ_Raporu";
            com.Parameters.AddWithValue("@Tarih1", dateTarih.DateTime.Date);
            if (rdo_X_Z.SelectedIndex == 0)
            {
                com.Parameters.AddWithValue("@Garson", Convert.ToString(look_Garson.EditValue));
            }
            com.Parameters.AddWithValue("@Odeme", Convert.ToBoolean(chk_Odeme.Checked));
            com.Parameters.AddWithValue("@Servis", Convert.ToBoolean(chk_Servis.Checked));
            com.Parameters.AddWithValue("@Cari", Convert.ToBoolean(chk_Cari.Checked));
            com.Parameters.AddWithValue("@Odenmez", Convert.ToBoolean(chk_Odenmez.Checked));
            com.Parameters.AddWithValue("@Mazleme", Convert.ToBoolean(chk_Malzeme.Checked));
            com.Parameters.AddWithValue("@Ana", Convert.ToBoolean(chk_Anagrup.Checked));
            com.Parameters.AddWithValue("@Ara", Convert.ToBoolean(chk_Altgrup.Checked));
            com.Parameters.AddWithValue("@Iptal", Convert.ToBoolean(chk_Iptal.Checked));
            com.Parameters.AddWithValue("@Yeni", 1);
            com.Parameters.AddWithValue("@PaketServis", chk_PaketServis.Checked);
            com.Parameters.AddWithValue("@IndirimMasa", chk_IndirimMasa.Checked);
            com.Parameters.AddWithValue("@YiyecekIcecek", chk_YiyecekIcecek.Checked);
            com.Parameters.AddWithValue("@MasaKonum", chk_MasaKonum.Checked);
            com.Parameters.AddWithValue("@GarsonOzet", chk_GarsonOzet.Checked);
            com.Parameters.AddWithValue("@GarsonTahsil", chk_GarsonTahsil.Checked);
            com.Parameters.AddWithValue("@SifirTutar", chk_SifirTutar.Checked);
            com.Parameters.AddWithValue("@OzetKasa", chk_OzetKasa.Checked);
            com.Parameters.AddWithValue("@ExtKasaRapor", chk_ExtKasaRapor.Checked);
            com.Parameters.AddWithValue("@ExtKasaDetay", chk_ExtKasaDetay.Checked);
            com.Parameters.AddWithValue("@SiparisIptal", chk_SiparisIptal.Checked);
            com.Parameters.AddWithValue("@GonderilmemisSiparisIptal", chk_GonderilmemisSiparisIptal.Checked);
            com.Parameters.AddWithValue("@SiparisDuzelt", chk_SiparisDuzelt.Checked);
            com.Parameters.AddWithValue("@CariTahsilat", chk_CariTahsilat.Checked);
            com.Parameters.AddWithValue("@servispayi", chk_servispayi.Checked);
            com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            sadeceDeptKasa(dt, dateTarih.DateTime.ToString("yyyy-MM-dd"));
            DataTable copyDataTable = dt.Clone();
            for (int i = 0; i < copyDataTable.Columns.Count; i++)
            {
                copyDataTable.Columns[i].DataType = typeof(string);
            }

            Console.WriteLine(dt.Rows.Count);

            int maxId = Convert.ToInt32(dt.Compute("max([Id])", string.Empty));
            maxId = maxId + 1;

            if (checkEditDepSatis.Checked)
            {
                string dep = cmb_Departman.EditValue.ToString();
                if (dep.Contains(","))
                {
                    dep = dep.Split(',')[0].Trim();
                }
                string tarih = dateTarih.DateTime.ToString("yyyy-MM-dd");
                string query = "exec Pos_Satis_Istatistik @Tarih1=N'" + tarih + "',@Tarih2=N'" + tarih + "' ,@Departman=" + dep + ",@depSatis=1";
                DataTable istatik = dbtools.SelectTableR(query);
                sadeceDept(istatik);


                foreach (DataRow item in dt.Rows)
                {
                   

                    if (item["Aciklama"].ToString().Contains("#Yazdırılmamış Sipariş-> Recete :"))
                    {
                        item["Aciklama"] = item["Aciklama"].ToString().Replace("#Yazdırılmamış Sipariş-> Recete :", "").Trim();
                        item["Aciklama"] = item["Aciklama"].ToString().Replace("Silindi", "").Trim();
                        item["Aciklama"] = item["Aciklama"].ToString().Replace("Miktar :", "").Trim();
                    }

                    string deger = item["Aciklama"].ToString();

                    if (deger.Trim().Equals("Ext Kasa Raporu"))
                    {
                        foreach (DataRow row in istatik.Rows)
                        {
                            DataRow dr = copyDataTable.NewRow();
                            dr["Id"] = maxId;
                            dr["Sira"] = row["Sira"];
                            dr["Aciklama"] = row["Aciklama"];
                            dr["Tutar"] = row["Tutar"];
                            maxId++;
                            copyDataTable.Rows.Add(dr);


                        }

                        copyDataTable.ImportRow(item);
                    }
                    else
                    {
                        copyDataTable.ImportRow(item);

                    }
                }
            }

            // dateTarih.DateTime Param.Param_Bindirim

            Console.WriteLine(copyDataTable.Rows.Count);

            Fis_Print(copyDataTable);

            this.Cursor = Cursors.Default;
        }

      

        public void sadeceDeptKasa(DataTable dt, string date)
        {
            try
            {
                bool depZorunlu = getSadeceDepXz();
                if (depZorunlu)
                {
                    string query = @"(select isnull(SUM(Rsat_Tutar),0) AS Rsat_Tutar   from Cst_Recete_Satis WITH(NOLOCK)  
LEFT JOIN Pos_Kodlar ON Rsat_Kapatma = Pkod_Kod and Pkod_Sinif = '11' 
and Pkod_Ozelkod <> '4'  LEFT JOIN Muh_Kodlar ON Mkodlar_Kod = Rsat_Dovizkodu 
and Mkodlar_sinif = '02'  WHERE Rsat_Ba = 'A'  and Rsat_Departman in("+Departman.Dep_Kodu+@")  
and convert(date,Rsat_Tarih) >= '" + date + @"' 
and convert(date,Rsat_Tarih) <= '" + date + @"' 
and Pkod_Kasagiris = 1) ";
                    string kasaGiris = dbtools.DegerGetir(query);

                    query = @"(select isnull(SUM(Rsat_Tutar),0) AS Rsat_Tutar   from Cst_Recete_Satis WITH(NOLOCK)  
LEFT JOIN Pos_Kodlar ON Rsat_Kapatma = Pkod_Kod and Pkod_Sinif = '11' 
and Pkod_Ozelkod <> '4'  LEFT JOIN Muh_Kodlar ON Mkodlar_Kod = Rsat_Dovizkodu 
and Mkodlar_sinif = '02'  WHERE Rsat_Ba = 'A'  and Rsat_Departman in(" + Departman.Dep_Kodu + @")  
and convert(date,Rsat_Tarih) >= '" + date + @"' 
and convert(date,Rsat_Tarih) <= '" + date + @"' 
and Pkod_Kasacikis = 1 ) ";

                    string kasaCikis = dbtools.DegerGetir(query);

                    query = @"select ISNULL(SUM(Pkasa_Tutar) ,0) as extraKasaGiris  from Pos_Kasahrk WITH(NOLOCK)  
LEFT JOIN Pos_Kodlar ON Pkasa_Kod = Pkod_Kod and Pkod_Sinif = '22'  
where convert(date,Pkasa_Tarih) >= '"+date+ @"'  
and Pkasa_dep in(" + Departman.Dep_Kodu + @")  
and convert(date,Pkasa_Tarih) <= '" + date + @"'  
and  Pkasa_GC = 'G' and Pkod_Kasagiris = 1 ";

                    string extraKasaGiris = dbtools.DegerGetir(query);

                    query = @"select ISNULL(SUM(Pkasa_Tutar) ,0) as extraKasaCikis  
from Pos_Kasahrk WITH(NOLOCK)  
LEFT JOIN Pos_Kodlar ON Pkasa_Kod = Pkod_Kod and Pkod_Sinif = '22'  
where convert(date,Pkasa_Tarih) >= '" + date + @"'  
and Pkasa_dep in(" + Departman.Dep_Kodu + @")  
and convert(date,Pkasa_Tarih) <= '" + date + @"'  
and  Pkasa_GC = 'C' and Pkod_Kasacikis = 1";

                    string extraKasaCikis = dbtools.DegerGetir(query);

                    query = @"select ISNULL(SUM(Chrk_Alacak) ,0) as cariGiris
from Pos_Carihrk WITH(NOLOCK) 
LEFT JOIN Pos_Kodlar ON Chrk_Odeme = Pkod_Kod and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' 
WHERE Chrk_Alacak > 0 and convert(date,Chrk_Tarih) >= '" + date + @"'  
and Chrk_Depart in(" + Departman.Dep_Kodu + @")    and convert(date,Chrk_Tarih) <= '" + date + @"'  
and Pkod_Kasagiris = 1 ";

                    string cariGiris = dbtools.DegerGetir(query);

                    query = @"select ISNULL(SUM(Chrk_Alacak) ,0) as cariCikis
from Pos_Carihrk WITH(NOLOCK) 
LEFT JOIN Pos_Kodlar ON Chrk_Odeme = Pkod_Kod and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' 
WHERE Chrk_Alacak > 0 and convert(date,Chrk_Tarih) >= '" + date + @"'   
and Chrk_Depart in(" + Departman.Dep_Kodu + @")  and convert(date,Chrk_Tarih) <= '" + date + @"'  
and Pkod_Kasacikis = 1";

                    string cariCikis = dbtools.DegerGetir(query);

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Aciklama"].ToString().Equals("Kasa Giris"))
                        {
                            row["Tutar"] = Convert.ToDecimal(kasaGiris)+ Convert.ToDecimal(extraKasaGiris) + Convert.ToDecimal(cariGiris);
                        }
                        else if (row["Aciklama"].ToString().Equals("Kasa Cikis"))
                        {
                            row["Tutar"] = Convert.ToDecimal(kasaCikis) + Convert.ToDecimal(extraKasaCikis) + Convert.ToDecimal(cariCikis);
                        }
                        else if (row["Aciklama"].ToString().Equals("Fark"))
                        {
                            decimal girisToplam = Convert.ToDecimal(kasaGiris) + Convert.ToDecimal(extraKasaGiris) + Convert.ToDecimal(cariGiris);
                            decimal cikisToplam = Convert.ToDecimal(kasaCikis) + Convert.ToDecimal(extraKasaCikis) + Convert.ToDecimal(cariCikis);
                            row["Tutar"] = girisToplam - cikisToplam;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        public void sadeceDept(DataTable istatik)
        {
            try
            {
                bool depZorunlu = getSadeceDepXz();
                if (depZorunlu)
                {
                    for (int i = istatik.Rows.Count - 2; i >= 1; i--)
                    {
                        DataRow dr = istatik.Rows[i];
                        if (dr["Aciklama"].ToString() != Departman.Dep_Adi)
                        {
                            dr.Delete();
                        }
                    }
                    istatik.AcceptChanges();
                    if (istatik.Rows.Count == 3)
                    {
                        istatik.Rows[2][2] = istatik.Rows[1][2];
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
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
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
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
        }



    }
}