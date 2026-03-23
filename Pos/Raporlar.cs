using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSplashScreen;
using Pos.Class;
using Pos.Forms;
using Pos.Models;
using Pos.Print;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
namespace Pos
{
    // 21.09.2022 Dil için -> Formdan ->  Engilis united states seçildi -> kaydet ve defaulta geri çekmeyi unutma
    public partial class Raporlar : DevExpress.XtraEditors.XtraForm
    {
        public Raporlar()
        {
            if (Param.Param_FullPos == true)
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
            InitializeComponent();
        }

        public bool merkezAktifmi = false;
        private void Raporlar_Load(object sender, EventArgs e)
        {

            string q2 = $@"select
                                       Pkod_SubeMac as [SubeMac],
                                        Pkod_Server as [Server],
                                        Pkod_Database as [Database],
                                        Pkod_User as [User],
                                        Pkod_Password as [Password],Pkod_Kod,Pkod_MerkezSube,Pkod_Ad
                                        from pos_kodlar
                                        where
                                        Pkod_Sinif = 27
                                       and
                                        Pkod_MerkezSube = 'S'";

            var dt2 = dbtools.SelectTable(q2);

            lookUpEditSubeCon.Properties.DataSource = dt2;
            lookUpEditSubeCon.Properties.DisplayMember = "Pkod_Ad";
            lookUpEditSubeCon.Properties.ValueMember = "Pkod_Kod";

            merkezAktifmi = Param.merkezaktif;
            loadyukle();

            radioGroupMerkezSube.Visible = User.merkezsubeaktif;

            tab_Log.PageVisible = User.logRaporGor;
            if (User.Pos_AdisyonPr)
            {
                barSubItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btn_XZ2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                tab_Log.PageVisible = false;
            }




            //gridyenile();


            if (User.Pos_AcikmasalariGizle)
            {
                gridView11.OptionsView.ShowFooter = false;
                bandedGridView1.OptionsView.ShowFooter = false;
                gridView3.OptionsView.ShowFooter = false;
                gridView7.OptionsView.ShowFooter = false;
                gridView13.OptionsView.ShowFooter = false;
                tab_muhasebe.PageVisible = false;
                tab_AylikGenelRapor.PageVisible = false;
            }



            btnOnburoIadeYap.Visible = User.onburoRaporIade;
        }

        public void loadyukle()
        {
            try
            {
                this.BringToFront();
                //CultureInfo culture = CultureInfo.CreateSpecificCulture("en-EN");
                //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;     
                // Set this culture as the default culture for all threads in this application. 
                //// Note: The following properties are supported in the .NET Framework 4.5+
                //CultureInfo.DefaultThreadCurrentCulture = culture;
                //CultureInfo.DefaultThreadCurrentUICulture = culture;
                dateTarih1.DateTime = Param.Tarih.Date;//.AddYears(-2);
                dateTarih2.DateTime = Param.Tarih.Date;
                DataTable Fis_Tip = new DataTable();
                Fis_Tip.Columns.Add("Kod", typeof(string));
                Fis_Tip.Columns.Add("Ad", typeof(string));
                Fis_Tip.Rows.Add("S", "Satis");
                Fis_Tip.Rows.Add("O", "Odenmez");
                Fis_Tip.Rows.Add("P", "Ikram");
                Fis_Tip.Rows.Add("V", "Hersey Dahil");
                Fis_Tip.Rows.Add("N", "Fiyat Analiz");
                cmb_Fistipi.Properties.DataSource = Fis_Tip;
                cmb_Fistipi.Properties.DisplayMember = "Ad";
                cmb_Fistipi.Properties.ValueMember = "Kod";
                cmb_Fistipi.SetEditValue("S,O,P,V,N");
                string filter = "";
                if (User.P_Departman != "" && merkezAktifmi == false)
                {
                    filter = " AND Kodlar_Kod IN ('" + User.P_Departman.Replace(", ", "','") + "')";
                }
                string q1 = "select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar with(nolock) where Kodlar_Sinif = '01' and Kodlar_Anadepo = 'False' and Kodlar_Satis = 1 " + filter + " order by Kodlar_Kod";
                DataTable dt_Dep = dbtools.SelectTable(q1);
                cmb_Departman.Properties.DataSource = dt_Dep;
                cmb_Departman.Properties.DisplayMember = "Kodlar_Ad";
                cmb_Departman.Properties.ValueMember = "Kodlar_Kod";
                //cmb_Departman.SetEditValue(Departman.Dep_Kodu);

                var allValues = string.Join(",", dt_Dep.AsEnumerable().Select(row => row["Kodlar_Kod"].ToString()));
                cmb_Departman.SetEditValue(allValues);


                DataTable dt_Ana = dbtools.SelectTable("SELECT Kodlar_Id ,Kodlar_Kod, Kodlar_Ad, Kodlar_Sinif  FROM Stok_Kodlar with(nolock) where Kodlar_Sinif = '08' order by Kodlar_Kod");
                cmb_Anagrup.Properties.DataSource = dt_Ana;
                cmb_Anagrup.Properties.DisplayMember = "Kodlar_Ad";
                cmb_Anagrup.Properties.ValueMember = "Kodlar_Kod";
                DataTable dt_Konum = dbtools.SelectTable("select Pkod_Id,Kodlar_Ad + ' - ' + Pkod_Ad as Ad from Pos_Kodlar left join Stok_Kodlar on Kodlar_Kod = Pkod_Kod and Kodlar_Sinif = '01' where pkod_sinif = '14'");
                cmb_Konum.Properties.DataSource = dt_Konum;
                cmb_Konum.Properties.DisplayMember = "Ad";
                cmb_Konum.Properties.ValueMember = "Pkod_Id";
                DataTable dt_Garson = dbtools.SelectTable(@"select P_Kod,(P_Ad + ' ' + P_Soyad) as AdSoyad,
CASE 
        WHEN P_Kulturu = 3 THEN 'YÖNETİCİ'
        WHEN P_Kulturu = 0 THEN 'KASİYER'
        WHEN P_Kulturu = 1 THEN 'GARSON'
        WHEN P_Kulturu = 2 THEN 'PAKETÇİ'
	    ELSE 'KULLANICI'
        end as 'Tip'
from RmosMuh.dbo.Pos_User where P_Kulturu <> 4 ORDER BY 
    CASE 
        WHEN P_Kulturu = 3 THEN 1
        WHEN P_Kulturu = 0 THEN 2
        WHEN P_Kulturu = 1 THEN 3
        WHEN P_Kulturu = 2 THEN 4
        ELSE 5
    END;");
                chk_GarsonKasiyer.Properties.DataSource = dt_Garson;
                chk_GarsonKasiyer.Properties.DisplayMember = "AdSoyad";
                chk_GarsonKasiyer.Properties.ValueMember = "P_Kod";


                if (Departman.Kodlar_PRSor)
                {
                    DataTable dt_PR = dbtools.SelectTable("select P_Kod,(P_Ad + ' ' + P_Soyad) as AdSoyad from RmosMuh.dbo.Pos_User where P_Kulturu = 4");
                    if (dt_PR.Rows.Count > 0)
                    {
                        chk_PR.Properties.DataSource = dt_PR;
                        chk_PR.Properties.DisplayMember = "AdSoyad";
                        chk_PR.Properties.ValueMember = "P_Kod";
                    }
                }
                chk_Ana.Checked = false;
                chk_Alt.Checked = false;
                btn_Detay.Visibility = User.R_Detay == false ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always;

                btn_XZ.Visibility = DevExpress.XtraBars.BarItemVisibility.Never; // btn_XZ2 olarak değiştirildi


                btn_XZ2.Visibility = User.R_XZ == false ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always;

                btn_Mahsupkes.Visibility = User.R_Mahsupkes == false ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always;
                btn_Fisiptal.Visibility = User.R_Fisiptal == false ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always;
                btn_MasaGeri.Visibility = Param.RaporMasa_Geri == false ? DevExpress.XtraBars.BarItemVisibility.Never :
                                                        User.R_MasaGeri == false ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always;
                btn_TopluIsleme.Visibility = User.R_TopluIsle == false ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always;
                btn_OdemeTipi.Visibility = User.Pos_OdemeDegistir == false ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always;
                raporyenile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cmb_Anagrup_EditValueChanged(object sender, EventArgs e)
        {
            chk_Alt.Checked = false;
            if (chk_Ana.Checked == true)
            {
                DataTable dt = dbtools.SelectTable("SELECT Kodlar_Id ,Kodlar_Kod, Kodlar_Ad, Kodlar_Sinif  FROM Stok_Kodlar with(nolock) where Kodlar_Sinif = '09' and Kodlar_Anagrup in (" + cmb_Anagrup.EditValue + ") order by Kodlar_Kod");
                cmb_Altgrup.Properties.DataSource = dt;
                cmb_Altgrup.Properties.DisplayMember = "Kodlar_Ad";
                cmb_Altgrup.Properties.ValueMember = "Kodlar_Kod";
            }
            else
            {
                DataTable bos = new DataTable();
                cmb_Altgrup.Properties.DataSource = bos;
            }
        }
        public void loadingAc()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
        }
        public void loadingKapat()
        {
            SplashScreenManager.CloseForm(false);
        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridyenile();
        }
        public DataTable getSatisList()
        {

            int paketharic = checkEditPaketHaricHepsi.Checked ? 0 : 1;

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 0);
            com.Parameters.AddWithValue("@paketharichepsi", paketharic);
            com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
            com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
            com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
            if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
            if (chk_Ana.Checked == true) com.Parameters.AddWithValue("@Ana_Grup", cmb_Anagrup.EditValue);
            if (chk_Alt.Checked == true) com.Parameters.AddWithValue("@Alt_Grup", cmb_Altgrup.EditValue);
            com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
            com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
            com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
            com.Parameters.AddWithValue("@Acik", chk_acik.Checked);
            com.Parameters.AddWithValue("@Kapali", chk_Kapali.Checked);
            com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
            if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
            com.Parameters.AddWithValue("@GarsonKasiyer", chk_GarsonKasiyer.EditValue);
            if (Convert.ToString(cmb_Konum.EditValue) != "") com.Parameters.AddWithValue("@Konum_Id", cmb_Konum.EditValue);
            if (Departman.Kodlar_PRSor) com.Parameters.AddWithValue("@PR", chk_PR.EditValue);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public DataTable servisPayiHesapla(DataTable dt)
        {
            try
            {
                dt.Columns.Add("Servis Payı", typeof(string));
                bool varmi = false;
                foreach (GridColumn item in gridView11.Columns)
                {
                    if (item.FieldName.Equals("Servis Payı"))
                    {
                        varmi = true;
                    }
                }
                if (varmi == false) gridView11.Columns.AddVisible("Servis Payı");
                foreach (DataRow item in dt.Rows)
                {
                    DataTable dataTable = dbtools.SelectTableR("select Rsat_Recete,Rsat_Fisno,Rsat_Tutar from Cst_Recete_Satis where Rsat_Fisno='" + item["Rsat_Fisno"].ToString() + "'");
                    foreach (DataRow rec in dataTable.Rows)
                    {
                        if (rec["Rsat_Recete"].ToString().Equals(Param.Param_Bindirim))
                        {
                            item["Servis Payı"] = rec["Rsat_Tutar"].ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "", "", ex);
            }
            return dt;
        }
        public void gridviewCountYaz(GridView grid)
        {
            if (grid.Columns.Count > 0)
            {
                grid.Columns["Servis Payı"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grid.Columns["Servis Payı"].SummaryItem.FieldName = "Servis Payı";
                grid.Columns["Servis Payı"].SummaryItem.DisplayFormat = "{0:n0}";
                grid.UpdateTotalSummary();
            }
        }
        private void gridyenile()
        {
            try
            {
                loadingAc();
                dateTarih1.SelectionStart = 0;
                dateTarih1.SelectionLength = 0;
                dateTarih2.SelectionStart = 0;
                dateTarih2.SelectionLength = 0;
                if (xtraTabControl1.SelectedTabPage == tab_Genelrapor)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 0);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    if (chk_Ana.Checked == true) com.Parameters.AddWithValue("@Ana_Grup", cmb_Anagrup.EditValue);
                    if (chk_Alt.Checked == true) com.Parameters.AddWithValue("@Alt_Grup", cmb_Altgrup.EditValue);
                    com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Acik", chk_acik.Checked);
                    com.Parameters.AddWithValue("@Kapali", chk_Kapali.Checked);
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    com.Parameters.AddWithValue("@GarsonKasiyer", chk_GarsonKasiyer.EditValue);
                    if (Convert.ToString(cmb_Konum.EditValue) != "") com.Parameters.AddWithValue("@Konum_Id", cmb_Konum.EditValue);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl1.DataSource = dt;
                    string fileName = getGenelCekDizaynPath();
                    if (File.Exists(fileName))
                    {
                        bandedGridView1.RestoreLayoutFromXml(fileName);
                    }
                    // XML restore işleminden sonra footer ayarını tekrar uygula

                    if (User.Pos_AcikmasalariGizle)
                    {
                        bandedGridView1.OptionsView.ShowFooter = false;

                    }
                }
                if (xtraTabControl1.SelectedTabPage == tab_Satis)
                {
                    DataTable dt = getSatisList();
                    List<string> adisyonFisler = new List<string>();
                    foreach (DataRow item in dt.Rows)
                    {
                        if (item["Rsat_AdisyonPR"].ToString().ToLower().Equals("true"))
                        {
                            adisyonFisler.Add(item["Rsat_Fisno"].ToString());
                        }
                    }
                    //if (adisyonFisler.Count>0)
                    //{
                    //    string newStr = string.Join(",", adisyonFisler);
                    //    string query = "update Cst_Recete_Satis set Rsat_AdisyonPr='1' from Cst_Recete_Satis where Rsat_Fisno in (" + newStr + ") and rsat_ba='B' ";
                    //    if (Departman.Adisyon==false)
                    //    {
                    //        dbtools.execcmd(query);
                    //    }
                    //}
                    HashSet<int> providers = new HashSet<int>();
                    foreach (var provider in dt.AsEnumerable()
                                               .Select(dr => dr.Field<int>("Rsat_Fisno")))
                    {
                        if (!providers.Add(provider))
                        {
                            dt = getSatisList();
                            break;
                        }
                    }
                    if (checkEditServisPay.Checked)
                    {
                        dt = servisPayiHesapla(dt);
                        gridControl11.DataSource = dt;
                        gridviewCountYaz(gridView11);
                    }
                    else
                    {
                        gridControl11.DataSource = dt;
                    }



                    if (checkEditDirektSatis.Checked)
                    {
                        try
                        {
                            if (dt.Rows.Count > 0)
                            {
                                var sonuc = dt.Select("Rsat_Masa=''");
                                if (sonuc != null && sonuc.Length > 0)
                                {
                                    dt = sonuc.CopyToDataTable();
                                    gridControl11.DataSource = dt;
                                }
                                else
                                {
                                    dt.Rows.Clear();
                                    gridControl11.DataSource = dt;
                                }


                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("direkt satış hatası \n" + ex.Message);
                        }
                    }

                    // Pos_AcikmasalariGizle parametresi kontrolü - Açık masaları gizle
                    //if (User.Pos_AcikmasalariGizle) // 21.01.2026 da yorum satırı yapıldı.
                    //{
                    //    try
                    //    {
                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            // Rsat_Durum = "Açık" olan satırları bul ve sil
                    //            for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    //            {
                    //                if (dt.Rows[i]["Rsat_Durum"] != null && dt.Rows[i]["Rsat_Durum"].ToString().Equals("Acik", StringComparison.OrdinalIgnoreCase))
                    //                {
                    //                    dt.Rows[i].Delete();
                    //                }
                    //            }
                    //            dt.AcceptChanges();
                    //            gridControl11.DataSource = dt;
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MessageBox.Show("Açık masaları gizleme hatası \n" + ex.Message);
                    //    }
                    //}



                    Rapor_Tipi.ItemIndex = Rapor_Tipi.Properties.GetDataSourceRowIndex("Diz_Id", Param.Param_Rapor_Design);




                    string fileName = getSatisDizaynPath2();
                    if (File.Exists(fileName))
                    {
                        gridView11.RestoreLayoutFromXml(fileName);
                    }

                }
                if (xtraTabControl1.SelectedTabPage == tab_Iptalcekraporu)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 1);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    if (chk_Ana.Checked == true) com.Parameters.AddWithValue("@Ana_Grup", cmb_Anagrup.EditValue);
                    if (chk_Alt.Checked == true) com.Parameters.AddWithValue("@Alt_Grup", cmb_Altgrup.EditValue);
                    com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl2.DataSource = dt;
                    string fileName = getIptalCekDizaynPath();
                    if (File.Exists(fileName))
                    {
                        gridView2.RestoreLayoutFromXml(fileName);
                    }
                }
                if (xtraTabControl1.SelectedTabPage == tab_Cekdetay)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 2);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    if (chk_Ana.Checked == true) com.Parameters.AddWithValue("@Ana_Grup", cmb_Anagrup.EditValue);
                    if (chk_Alt.Checked == true) com.Parameters.AddWithValue("@Alt_Grup", cmb_Altgrup.EditValue);
                    com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    if (chk_Detay.Checked == true) com.Parameters.AddWithValue("@DetayGrup", cmb_DetayGrup.EditValue);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl3.DataSource = dt;
                }
                if (xtraTabControl1.SelectedTabPage == tab_Receteozet)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 3);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    if (chk_Ana.Checked == true) com.Parameters.AddWithValue("@Ana_Grup", cmb_Anagrup.EditValue);
                    if (chk_Alt.Checked == true) com.Parameters.AddWithValue("@Alt_Grup", cmb_Altgrup.EditValue);
                    com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl4.DataSource = dt;
                }
                if (xtraTabControl1.SelectedTabPage == tab_Garsonsatis)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 4);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    if (chk_Ana.Checked == true) com.Parameters.AddWithValue("@Ana_Grup", cmb_Anagrup.EditValue);
                    if (chk_Alt.Checked == true) com.Parameters.AddWithValue("@Alt_Grup", cmb_Altgrup.EditValue);
                    com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    com.Parameters.AddWithValue("@GarsonKasiyer", chk_GarsonKasiyer.EditValue);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl5.DataSource = dt;
                }
                if (xtraTabControl1.SelectedTabPage == tab_Fatura)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 5);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    if (chk_Ana.Checked == true) com.Parameters.AddWithValue("@Ana_Grup", cmb_Anagrup.EditValue);
                    if (chk_Alt.Checked == true) com.Parameters.AddWithValue("@Alt_Grup", cmb_Altgrup.EditValue);
                    com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl6.DataSource = dt;
                }
                if (xtraTabControl1.SelectedTabPage == tab_AylikGenelRapor)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 21);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    //com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    //if (chk_Ana.Checked == true) com.Parameters.AddWithValue("@Ana_Grup", cmb_Anagrup.EditValue);
                    //if (chk_Alt.Checked == true) com.Parameters.AddWithValue("@Alt_Grup", cmb_Altgrup.EditValue);
                    //com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    //com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    //com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    //com.Parameters.AddWithValue("@Acik", chk_acik.Checked);
                    //com.Parameters.AddWithValue("@Kapali", chk_Kapali.Checked);
                    //if (Convert.ToString(cmb_Konum.EditValue) != "") com.Parameters.AddWithValue("@Konum_Id", cmb_Konum.EditValue);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl15.DataSource = dt;
                }
                if (xtraTabControl1.SelectedTabPage == tab_OdemeRaporu)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 16);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    if (Convert.ToString(cmb_Konum.EditValue) != "") com.Parameters.AddWithValue("@Konum_Id", cmb_Konum.EditValue);
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    con.Close();
                    //ds.Relations.Clear();
                    //ds.Relations.Add("R_C", ds.Tables["Table"].Columns["Rsat_Fisno"], ds.Tables["Table1"].Columns["Rsat_Fisno"]);
                    //gridControl7.LevelTree.Nodes.Add(ds.Relations["R_C"].RelationName, gridView7_1);
                    //gridControl7.DataSource = ds.Tables["Table"];
                    gridControl7.DataSource = ds.Tables[0];
                    gridView7.BestFitColumns();
                    for (int i = 0; i < gridView7.Columns.Count; i++)
                    {
                        gridView7.Columns[i].OptionsColumn.AllowFocus = false;
                        if (i > 4)
                        {
                            gridView7.Columns[i].Width = 75;
                            gridView7.Columns[i].AppearanceCell.BackColor = Color.Azure;
                            gridView7.Columns[i].AppearanceCell.Options.UseBackColor = true;
                            gridView7.Columns[i].Summary.Clear();
                            gridView7.Columns[i].Summary.Add(DevExpress.Data.SummaryItemType.Sum, gridView7.Columns[i].FieldName, "{0:n2}");
                        }
                        else
                        {
                            gridView7.Columns[i].Width = 90;
                        }
                    }
                }
                if (xtraTabControl1.SelectedTabPage == tab_Satisrapor)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 7);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    con.Close();
                    ds.Relations.Clear();
                    ds.Relations.Add("R_C", ds.Tables["Table"].Columns["Rsat_Fisno"], ds.Tables["Table1"].Columns["Rsat_Fisno"]);
                    gridControl8.LevelTree.Nodes.Add(ds.Relations["R_C"].RelationName, gridView8_1);
                    gridControl8.DataSource = ds.Tables["Table"];
                    for (int i = 0; i < gridView8.Columns.Count; i++)
                    {
                        gridView8.Columns[i].OptionsColumn.AllowFocus = false;
                    }
                }
                if (xtraTabControl1.SelectedTabPage == tab_Log)
                {
                    lograpor();
                }
                if (xtraTabControl1.SelectedTabPage == tab_Alacak)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 9);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl10.DataSource = dt;
                }
                if (xtraTabControl1.SelectedTabPage == tab_Uye)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 10);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    com.Parameters.AddWithValue("@OnburoDB", Fronttools.DB_Database);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl12.DataSource = dt;
                }
                if (xtraTabControl1.SelectedTabPage == tab_DepartmanOzet)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 13);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl13.DataSource = dt;
                }
                if (xtraTabControl1.SelectedTabPage == tab_Zayi)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 14);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                    com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                    com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
                    if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl14.DataSource = dt;
                    gridView14.BestFitColumns();
                }
                if (xtraTabControl1.SelectedTabPage == tab_Ikram)
                {
                    gridView16.Columns.Clear();
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis_Rapor";
                    com.Parameters.AddWithValue("@Rapor_Tipi", 29);
                    com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                    com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                    if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    gridControl16.DataSource = dt;
                    gridView16.BestFitColumns();

                    gridviewCountYaz(gridView16, "Tutar");

                }
                if (xtraTabControl1.SelectedTabPage == tab_reskullanim)
                {
                    reskullanimListele();
                }
                if (xtraTabControl1.SelectedTabPage == tab_muhasebe)
                {
                    muhasebeRapor(true);


                }
            }
            catch (Exception ex)
            {
                loadingKapat();
                RHMesaj.MyMessageError(MyClass, "gridyenile", "", ex);
            }
            loadingKapat();
        }

        public void lograpor()
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 8);
            com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
            com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
            com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
            if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            gridControl9.DataSource = dt;
            string fileName = getLogDizaynPath();
            if (File.Exists(fileName))
            {
                gridView9.RestoreLayoutFromXml(fileName);
            }
            else
            {
                gridView9.BestFitColumns();
            }
        }
        public void reskullanimListele()
        {
            gridControlResKullanim.DataSource = dbtools.SelectTableR(@"select 
case 
when Bufe_Tipi=0 then 'Kahvaltı'
when Bufe_Tipi=1 then 'Öğle'
when Bufe_Tipi=2 then 'Akşam'
end as Bufe_Tipi ,
Tarih,RezId,Master_RezId,Odano,KartNo,Pansiyon_Kodu from Pos_ResKullanim");
            gridViewResKullanim.BestFitColumns();
        }
        private void btn_Print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == tab_Genelrapor)
            {
                Rapor_Print("Genel Çek Raporu", gridControl1);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Satis)
            {
                Rapor_Print("Genel Çek Raporu", gridControl11);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Iptalcekraporu)
            {
                Rapor_Print("Iptal Çek Raporu", gridControl2);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Cekdetay)
            {
                Rapor_Print("Çek Detay Listesi", gridControl3);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Receteozet)
            {
                Rapor_Print("Recete Ozet Listesi", gridControl4);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Garsonsatis)
            {
                Rapor_Print("Garson Satıs Çizelgesi", gridControl5);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Fatura)
            {
                Rapor_Print("Pos Fatura", gridControl6);
            }
            if (xtraTabControl1.SelectedTabPage == tab_OdemeRaporu)
            {
                Rapor_Print("Ödeme Raporu", gridControl7);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Satisrapor)
            {
                Rapor_Print("Satıs Raporu", gridControl8);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Log)
            {
                Rapor_Print("Log Raporu", gridControl9);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Alacak)
            {
                Rapor_Print("Alacak Raporu", gridControl10);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Uye)
            {
                Rapor_Print("Uye Satis Raporu", gridControl12);
            }
            if (xtraTabControl1.SelectedTabPage == tab_DepartmanOzet)
            {
                Rapor_Print("Departman Özet Raporu", gridControl13);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Zayi)
            {
                Rapor_Print("Zayi Raporu", gridControl14);
            }
            if (xtraTabControl1.SelectedTabPage == tab_AylikGenelRapor)
            {
                Rapor_Print("Aylık Genel Rapor", gridControl15);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Ikram)
            {
                Rapor_Print("İkram Edilen Ürünler Raporu", gridControl16);
            }

            if (xtraTabControl1.SelectedTabPage == tab_muhasebe)
            {
                rapor.ShowPreview();
            }
        }
        private void Rapor_Print(string header, GridControl grid)
        {
            string leftColumn = header;
            string rightColumn = dateTarih1.DateTime.ToLongDateString() + "-" + dateTarih2.DateTime.ToLongDateString();
            PrintingSystem printingSystem1 = new PrintingSystem();
            PrintableComponentLink printableComponentLink1 = new PrintableComponentLink();
            printingSystem1.Links.AddRange(new object[] { printableComponentLink1 });
            printableComponentLink1.Component = grid;
            printableComponentLink1.Landscape = false;
            printableComponentLink1.Margins = new System.Drawing.Printing.Margins(20, 20, 50, 20);
            PageHeaderFooter phf = printableComponentLink1.PageHeaderFooter as PageHeaderFooter;
            phf.Header.Content.Clear();
            phf.Header.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            phf.Header.Content.AddRange(new string[] { leftColumn, rightColumn });
            phf.Header.LineAlignment = BrickAlignment.Far;
            printableComponentLink1.ShowPreview();
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void btn_Mahsupkes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dateTarih1.DateTime.Date != dateTarih2.DateTime.Date)
            {
                MessageBox.Show(res_man.GetString("Farklı Tarihler için Mahsup Oluşturulamaz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show(res_man.GetString("Nakit Hesaplar için Mahsup Kesmek İstiyor Musunuz ?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Pos_Mahsup(true, false);
            }
            if (MessageBox.Show(res_man.GetString("KK Hesaplar için Mahsup Kesmek İstiyor Musunuz ?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Pos_Mahsup(false, true);
            }
        }
        private void Pos_Gelir_Mahsup(bool Kdv)
        {
            if (Kdv == true)
            {
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "Pos_Gelir_Mahsub";
                com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                //com.Parameters.AddWithValue("@KDVAyrimi", Kdv);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                MessageBox.Show(res_man.GetString("Gelir Hesapları için Fis Oluştu.... Fis No : ") + Convert.ToString(dt.Rows[0][0]), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "Pos_Gelir_Mahsub_KdvSiz";
                com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
                com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
                com.Parameters.AddWithValue("@Satis_Tip", cmb_Fistipi.EditValue);
                if (cmb_Departman.EditValue != null) com.Parameters.AddWithValue("@Departman", cmb_Departman.EditValue);
                if (chk_Ana.Checked == true) com.Parameters.AddWithValue("@Ana_Grup", cmb_Anagrup.EditValue);
                if (chk_Alt.Checked == true) com.Parameters.AddWithValue("@Alt_Grup", cmb_Altgrup.EditValue);
                com.Parameters.AddWithValue("@Saat_Filtre", rdo_Saat.SelectedIndex);
                com.Parameters.AddWithValue("@Saat1", timeSaat1.Time.ToString("HH:mm:ss"));
                com.Parameters.AddWithValue("@Saat2", timeSaat2.Time.ToString("HH:mm:ss"));
                com.Parameters.AddWithValue("@Kapali", true);
                if (Convert.ToString(cmb_Konum.EditValue) != "") com.Parameters.AddWithValue("@Konum_Id", cmb_Konum.EditValue);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                MessageBox.Show(res_man.GetString("Gelir Hesapları için Fis Oluştu.... Fis No : ") + Convert.ToString(dt.Rows[0][0]), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Pos_Mahsup(bool Nakit, bool KK)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = "Pos_Fatura_Mahsup";
            com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
            com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
            com.Parameters.AddWithValue("@fis_user", User.P_Kod);
            com.Parameters.AddWithValue("@Nakit", Nakit);
            com.Parameters.AddWithValue("@KK", KK);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            if (Nakit)
            {
                MessageBox.Show(res_man.GetString("Nakit Hesaplar için Fis Oluştu.... Fis No : ") + Convert.ToString(dt.Rows[0][0]), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (KK)
            {
                MessageBox.Show(res_man.GetString("KK Hesaplar için Fis Oluştu.... Fis No : ") + Convert.ToString(dt.Rows[0][0]), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void fatNoDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fatno_Update fatno = new Fatno_Update();
            fatno.txt_Fisno.Text = Convert.ToString(gridView6.GetFocusedRowCellValue("PFat_Cekno"));
            fatno.txt_EskiFatNo.Text = Convert.ToString(gridView6.GetFocusedRowCellValue("PFat_Fatno"));
            fatno.ShowDialog();
            barButtonItem1_ItemClick(null, null);
        }
        private void FisIptal()
        {
            if (xtraTabControl1.SelectedTabPage == tab_Genelrapor || xtraTabControl1.SelectedTabPage == tab_Satis)
            {
                Param.Param_Yukle();
                string iptalSebep = String.Empty;
                string Fisno = xtraTabControl1.SelectedTabPage == tab_Genelrapor ? Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno")) : Convert.ToString(gridView11.GetFocusedRowCellValue("Rsat_Fisno"));
                DateTime Tarih = xtraTabControl1.SelectedTabPage == tab_Genelrapor ? Convert.ToDateTime(bandedGridView1.GetFocusedRowCellValue("Rsat_Tarih")) : Convert.ToDateTime(gridView11.GetFocusedRowCellValue("Rsat_Tarih"));
                if (User.P_Kod.ToUpper() != "RMOS")//!User.R_Fisiptalgecmis
                {
                    if (Tarih.Date != Param.Tarih.Date)
                    {
                        MessageBox.Show(res_man.GetString("Farklı Tarihteki Fişi İptal Edemezsiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                DataTable dataTable = dbtools.SelectTableR("select Pkod_Ad from Pos_Kodlar where Pkod_Sinif='23' and Pkod_Kod like 'FP%' -- ürün iptal");

                Klavye2 klavye = new Klavye2(data: dataTable);
                klavye.Tag = "FISIPTAL";
                klavye.ShowDialog();
                iptalSebep = klavye.yazi;
                if (iptalSebep.Length < 1)
                {
                    return;
                }

                iptalfisyazdir(Fisno, iptalSebep);

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


                merkezeiptalcekgonder(Fisno);
            }
            else
            {
                MessageBox.Show(res_man.GetString("Sadece Genel Çek Raporundan Fişi İptal Edebilirsiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public void iptalfisyazdir(string fisno, string neden)
        {

            if (Param.Satis_YarimTam == false)
            {
                return;
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

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Satir_Sil, Log.Log_Islem.Sil, yazdirilmissa + " Sipariş-> " + "Recete : " + Rec_Ad + " Miktar : " + miktar + " Silindi", Convert.ToString(fisno), Rsat_Id.ToString(), Rec_Ad, miktar, neden, Rsat_Tutar);

                //Fis_Islem.Satir_Sil(Rsat_Id, miktar);



            }

        }

        public void merkezeiptalcekgonder(string Fisno)
        {
            try
            {
                if (Param.Param_SatisTabloAktif && Param.Param_SatisTabloGonderi > 0)
                {

                    DataTable dataTable = dbtools.SelectTableR($"select * from Cst_Satis_Ipt where Rsat_Fisno='{Fisno}'");
                    if (dataTable == null || dataTable.Rows.Count == 0) return;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        int departman = Convert.ToInt32(row["Rsat_Departman"].ToString());
                        string fisno = row["Rsat_Fisno"]?.ToString() ?? "";
                        row["Rsat_Fisno"] = departman + fisno;
                    }


                    Sube2Merkez a = new Sube2Merkez(); // hedef veritabanı bağlantısı

                    using (SqlConnection destinationConnection = new SqlConnection(a.connstr))
                    {
                        destinationConnection.Open();

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                        {
                            bulkCopy.DestinationTableName = "Cst_Satis_Ipt"; // hedef tablo adı

                            // Kolon eşleştirmelerini belirt (eğer adlar birebirse bu adım gerekmez ama önerilir)
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            }

                            // Veriyi gönder
                            bulkCopy.WriteToServer(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata loglanabilir
                Console.WriteLine("Hata oluştu: " + ex.Message);
            }
        }
        private void btn_Detay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Detay();
        }
        private void Detay()
        {
            if (xtraTabControl1.SelectedTabPage == tab_Genelrapor || xtraTabControl1.SelectedTabPage == tab_Satis)
            {
                Detay detay = new Detay();
                if (xtraTabControl1.SelectedTabPage == tab_Genelrapor)
                {
                    detay.spn_Fisno.EditValue = Convert.ToInt32(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                }
                if (xtraTabControl1.SelectedTabPage == tab_Satis)
                {
                    detay.spn_Fisno.EditValue = Convert.ToInt32(gridView11.GetFocusedRowCellValue("Rsat_Fisno"));
                }
                detay.masano = gridView11.GetFocusedRowCellValue("Masa_No").ToString();


                detay.ShowDialog();
            }
            else if (xtraTabControl1.SelectedTabPage == tab_Iptalcekraporu)
            {
                Detay_Iptal ipt = new Detay_Iptal();
                ipt.spn_Fisno.EditValue = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Fisno"));
                ipt.ShowDialog();
            }
            else
            {
                MessageBox.Show(res_man.GetString("Sadece Genel Çek Raporundan Fişi Detayına Gidebilirsiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void btn_Cikis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void btn_MasaGeri_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }
        private void MasaGeriAl()
        {
            if (xtraTabControl1.SelectedTabPage == tab_Genelrapor)
            {
                if (Convert.ToDateTime(bandedGridView1.GetFocusedRowCellValue("Rsat_Tarih")) != Param.Tarih)
                {
                    MessageBox.Show(res_man.GetString("Farklı Tarihteki Masayı Geri Alamazsınız..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Durum")) == "Acik")
                {
                    MessageBox.Show(res_man.GetString("Açık Masayı Masayı Geri Alamazsınız..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //if (Param.Tesis_Tipi == 0)
                //{
                //    MessageBox.Show(res_man.GetString("Çalışma Sistemi Otel ise Masayı Geri Alamazsınız...", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                // 30.05.2023 de yorum yapıldı
                //string masaNo = Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Masa"));
                //if (masaNo == "")
                //{
                //    MessageBox.Show(res_man.GetString("Masa Bilgisi Bulunamadı..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //    return;
                //}
                if (MessageBox.Show(res_man.GetString("Seçili Masayı Geri Almak İstiyor Musunuz?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string masaNo = Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Masa"));
                if (masaNo == "")
                {
                    string fisno = Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                    masaNo = dbtools.DegerGetir("select top 1 Masa_No from Pos_Masa where Masa_Durum='0' and Masa_Depart='" + Departman.Dep_Kodu + "' order by Masa_Id desc");
                    string query = "update Cst_Recete_Satis set Rsat_Masa='" + masaNo + "',Rsat_OzelMasaAdi='" + masaNo + "' where Rsat_Fisno='" + fisno + "'";
                    dbtools.execcmdR(query);
                    dbtools.execcmdR("update Pos_Masa set Masa_Durum='1' where Masa_No='" + masaNo + "' and Masa_Depart='" + Departman.Dep_Kodu + "'");
                    MessageBox.Show("MASANO : " + masaNo + " Aktarılmıştır ");
                }
                DataTable dtDurum = dbtools.SelectTable("select Rsat_Durum from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Masa = '" + masaNo + "' and rsat_Departman = '" + Convert.ToString(bandedGridView1.GetFocusedRowCellValue("DepartmanKod")) + "' and Rsat_Durum = 'A' ");
                if (dtDurum.Rows.Count > 0)
                {
                    MessageBox.Show(masaNo + " NL Masa Şuan Dolu Masa Geri Alınamıyor...", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Durum = 'A' , Rsat_RecAP = 2 where Rsat_Fisno = '" + Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno")) + "'");
                string dep = Convert.ToString(dbtools.DegerGetir("select top 1 Rsat_Departman from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno")) + "'"));
                string ozelMasa = Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Masa_Ozel"));
                string masaAd = Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Masa_Ad"));
                if (ozelMasa.Equals(masaAd))
                {
                    ozelMasa = "";
                }
                dbtools.execcmd("update Pos_Masa set Masa_Durum = 1,Masa_Ozel='" + ozelMasa + "'  where Masa_No = '" + masaNo + "' and Masa_Depart = '" + dep + "' ");
                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Raporlar, Log.Log_Islem.Duzelt, "Masa Geri Alma MasaNo:" + masaNo + " Fisno:" + Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno")), Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno")), "");
                if (Param.Tesis_Tipi == 0)
                {
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Cek_Geri";
                    com.Parameters.AddWithValue("@Fis_No", Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno")));
                    com.Parameters.AddWithValue("@Users", User.P_Kod);
                    //com.Parameters.AddWithValue("@Rsat_IptalNot", iptalSebep);
                    com.Parameters.AddWithValue("@Onb_Sil", Convert.ToDateTime(bandedGridView1.GetFocusedRowCellValue("Rsat_Tarih")).Date != Param.Tarih.Date ? 0 : 1);
                    com.ExecuteNonQuery();
                    if (con.State == ConnectionState.Open) con.Close();
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Fis_Iptal, Log.Log_Islem.Sil, Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno")) + " NL Fis Cek Geri Alındı", Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno")), String.Empty);
                }
                gridyenile();
            }
            else
            {
                MessageBox.Show("Sadece Genel Çek Raporundan Masa Geri Alabilirsiniz...  YÖNLENİYOR...! MASAYI TEKRAR DAN GERİ ALMAYI DENEYİN..!", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                xtraTabControl1.SelectedTabPage = tab_Genelrapor;

            }
        }
        private void raporyenile()
        {
            Rapor_Tipi.Properties.DataSource = dbtools.Dizayn_Getir(User.P_Kod, this.Name, "");
            Rapor_Tipi.Properties.DisplayMember = "Diz_Rapor";
            Rapor_Tipi.Properties.ValueMember = "Diz_Id";
        }
        private void Rapor_Tipi_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = dbtools.Dizayn_Getir(User.P_Kod, this.Name, Convert.ToString(Rapor_Tipi.EditValue));
            MemoryStream memStream = new MemoryStream((byte[])dt.Rows[0]["Diz_XML"]);
            gridView11.RestoreLayoutFromStream(memStream);
        }
        private void raporDizaynıKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == tab_Genelrapor)
            {
                bandedGridView1.ActiveFilterString = null;
                Rapor_Dizayn rdiz = new Rapor_Dizayn(this.Name, ref bandedGridView1);
                rdiz.ShowDialog();
                raporyenile();
            }
            if (xtraTabControl1.SelectedTabPage == tab_Satis)
            {
                gridView11.ActiveFilterString = null;
                Rapor_Dizayn rdiz = new Rapor_Dizayn(this.Name, ref gridView11);
                rdiz.ShowDialog();
                raporyenile();
            }
        }
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdo_SatisIptal.SelectedIndex == 0)
            {
                xtraTabControl1.SelectedTabPage = tab_Satis;
                gridyenile();
            }
            if (rdo_SatisIptal.SelectedIndex == 1)
            {
                xtraTabControl1.SelectedTabPage = tab_Iptalcekraporu;
                gridyenile();
            }
        }
        private void btn_FisLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }
        private void gridView11_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.Name == "bandedGridColumn43")
            {
                // "Ciro : "  23.10.2024
                e.Info.DisplayText = (Convert.ToDecimal(bandedGridColumn49.SummaryItem.SummaryValue) - Convert.ToDecimal(bandedGridColumn50.SummaryItem.SummaryValue) - Convert.ToDecimal(bandedGridColumn61.SummaryItem.SummaryValue) - Convert.ToDecimal(bandedGridColumn62.SummaryItem.SummaryValue)).ToString("N2");
                e.Info.Appearance.ForeColor = Color.Red;
                //e.Info.Appearance.Font = new Font("Tahoma", 9, FontStyle.Bold);
            }
            if (e.Column.Name == "gridColumn120")
            {
                e.Info.DisplayText = "Onburo : " + (Convert.ToDecimal(bandedGridColumn49.SummaryItem.SummaryValue) - Convert.ToDecimal(bandedGridColumn50.SummaryItem.SummaryValue) - Convert.ToDecimal(bandedGridColumn61.SummaryItem.SummaryValue) - Convert.ToDecimal(bandedGridColumn63.SummaryItem.SummaryValue)).ToString("N2");
                e.Info.Appearance.ForeColor = Color.Red;
                //e.Info.Appearance.Font = new Font("Tahoma", 9, FontStyle.Bold);
            }
        }
        private void btn_TopluIsleme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FisTopluIsleme tt = new FisTopluIsleme();
            tt.ShowDialog();
        }
        private void tarihDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.R_Fisiptalgecmis)
            {
            }
            else
            {
                if (User.P_Kod.ToUpper() != "RMOS")
                {
                    MessageBox.Show(res_man.GetString("Bu İşlemi Yapmaya Yetkiniz Yoktur."));
                    return;
                }
            }
            TarihDegistir tarih = new TarihDegistir();
            tarih.Fisno = Convert.ToInt32(gridView11.GetFocusedRowCellValue("Rsat_Fisno"));
            tarih.ShowDialog();
            barButtonItem1_ItemClick(null, null);
        }
        public void Mail_Gonder(DateTime tarih, DateTime tarih2, GridControl gC, string RaporAdi)
        {
            DataTable dt = dbtools.SelectTable("select ISNULL(Mail_Gonder,0) as Mail_Gonder,Mail_Isim,Mail_Adres,Mail_Parola,Mail_Host,Mail_Port,Mail_SSL, "
                                     + " Mail_Alici1,Mail_Alici2,Mail_Alici3,Mail_Alici4,Mail_Alici5, "
                                     + " isnull(Mail_Odeme_Tip,0) as Mail_Odeme_Tip,isnull(Mail_Servis_Paylari,0) as Mail_Servis_Paylari,isnull(Mail_Cari_Ozet,0) as Mail_Cari_Ozet, "
                                     + " isnull(Mail_Odenmez_Ozet,0) as Mail_Odenmez_Ozet,isnull(Mail_Malz_Ozet,0) as Mail_Malz_Ozet,isnull(Mail_Ana_Ozet,0) as Mail_Ana_Ozet, "
                                     + " isnull(Mail_Alt_Ozet,0) as Mail_Alt_Ozet,isnull(Mail_Iptal_Ozet,0) as Mail_Iptal_Ozet, "
                                     + " Mail_Alici6,Mail_Alici7,Mail_Alici8,Mail_Alici9,Mail_Alici10 from Pos_Mail WITH(NOLOCK) where Mail_Id = 1");
            if (dt.Rows.Count > 0)
            {
                //bool Mail_Gonder = Convert.ToBoolean(dt.Rows[0]["Mail_Gonder"]);
                //if (Mail_Gonder == true)
                {
                    string Mail_Isim = Convert.ToString(dt.Rows[0]["Mail_Isim"]);
                    string Mail_Adres = Convert.ToString(dt.Rows[0]["Mail_Adres"]);
                    string Mail_Parola = Convert.ToString(dt.Rows[0]["Mail_Parola"]);
                    string Mail_Host = Convert.ToString(dt.Rows[0]["Mail_Host"]);
                    string Mail_Port = Convert.ToString(dt.Rows[0]["Mail_Port"]);
                    bool Mail_SSL = Convert.ToBoolean(dt.Rows[0]["Mail_SSL"]);
                    string Mail_Alici1 = Convert.ToString(dt.Rows[0]["Mail_Alici1"]);
                    string Mail_Alici2 = Convert.ToString(dt.Rows[0]["Mail_Alici2"]);
                    string Mail_Alici3 = Convert.ToString(dt.Rows[0]["Mail_Alici3"]);
                    string Mail_Alici4 = Convert.ToString(dt.Rows[0]["Mail_Alici4"]);
                    string Mail_Alici5 = Convert.ToString(dt.Rows[0]["Mail_Alici5"]);
                    string Mail_Alici6 = Convert.ToString(dt.Rows[0]["Mail_Alici6"]);
                    string Mail_Alici7 = Convert.ToString(dt.Rows[0]["Mail_Alici7"]);
                    string Mail_Alici8 = Convert.ToString(dt.Rows[0]["Mail_Alici8"]);
                    string Mail_Alici9 = Convert.ToString(dt.Rows[0]["Mail_Alici9"]);
                    string Mail_Alici10 = Convert.ToString(dt.Rows[0]["Mail_Alici10"]);
                    try
                    {
                        System.Threading.Thread.Sleep(1 * 1000);
                        Application.DoEvents();
                        DevExpress.XtraPrinting.PrintingSystemBase ps = new DevExpress.XtraPrinting.PrintingSystemBase();
                        DevExpress.XtraPrintingLinks.PrintableComponentLinkBase link = new DevExpress.XtraPrintingLinks.PrintableComponentLinkBase(ps);
                        link.Component = gC;
                        link.Landscape = true;
                        link.PaperKind = System.Drawing.Printing.PaperKind.A4;
                        link.CreateDocument();
                        //link.PrintingSystemBase.ExportToPdf(tarih.ToShortDateString() + " - " + tarih2.ToShortDateString() + " Tarih Arası " + RaporAdi + ".pdf");
                        MemoryStream mem = new MemoryStream();
                        link.PrintingSystemBase.ExportToPdf(mem);
                        mem.Seek(0, System.IO.SeekOrigin.Begin);
                        Attachment att = new Attachment(mem, tarih.ToShortDateString() + " - " + tarih2.ToShortDateString() + " Tarih Arası " + RaporAdi + ".pdf", "application/pdf");
                        //MemoryStream mem = new MemoryStream();
                        ////gunsonu.ExportToPdf(mem);
                        //mem.Seek(0, System.IO.SeekOrigin.Begin);
                        //Attachment att = new Attachment(tarih.ToShortDateString() + " - " + tarih2.ToShortDateString() + " Tarih Arası " + RaporAdi + ".pdf");
                        MailMessage ePosta = new MailMessage();
                        ePosta.From = new MailAddress(Mail_Adres, Mail_Isim);
                        if (Mail_Alici1.Length > 0) ePosta.To.Add(Mail_Alici1);
                        if (Mail_Alici2.Length > 0) ePosta.To.Add(Mail_Alici2);
                        if (Mail_Alici3.Length > 0) ePosta.To.Add(Mail_Alici3);
                        if (Mail_Alici4.Length > 0) ePosta.To.Add(Mail_Alici4);
                        if (Mail_Alici5.Length > 0) ePosta.To.Add(Mail_Alici5);
                        if (Mail_Alici6.Length > 0) ePosta.To.Add(Mail_Alici6);
                        if (Mail_Alici7.Length > 0) ePosta.To.Add(Mail_Alici7);
                        if (Mail_Alici8.Length > 0) ePosta.To.Add(Mail_Alici8);
                        if (Mail_Alici9.Length > 0) ePosta.To.Add(Mail_Alici9);
                        if (Mail_Alici10.Length > 0) ePosta.To.Add(Mail_Alici10);
                        ePosta.Priority = MailPriority.Normal;
                        ePosta.Subject = tarih.Date.ToLongDateString() + " - " + tarih2.ToShortDateString() + " Pos İki Tarih Arası " + RaporAdi + " Raporu";
                        ePosta.Attachments.Add(att);
                        //ePosta.Attachments.Add(att2);
                        //ePosta.Attachments.Add(att3);
                        string mailbody = Mail_Detay(tarih, tarih2, RaporAdi);
                        ePosta.IsBodyHtml = true;
                        ePosta.Body = mailbody;
                        SmtpClient ss = new SmtpClient(Mail_Host, Convert.ToInt32(Mail_Port));
                        ss.EnableSsl = Mail_SSL;
                        ss.DeliveryMethod = SmtpDeliveryMethod.Network;
                        ss.UseDefaultCredentials = false;
                        ss.Credentials = new NetworkCredential(Mail_Adres, Mail_Parola);
                        ss.Send(ePosta);
                        MessageBox.Show(res_man.GetString("Mail Başarıyla Gönderildi."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception err)
                    {
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Gun_Sonu, Log.Log_Islem.Kaydet, tarih.Date.ToString("dd.MM.yyyy") + " Mail Gönderim Sırasında Hata Oldu...", "", "");
                        MessageBox.Show(res_man.GetString("Mail Gönderilemedi...") + "\n" + err.Message, res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }
        private string Mail_Detay(DateTime tarih, DateTime tarih2, string RaporAdi)
        {
            return tarih.Date.ToLongDateString() + " - " + tarih2.Date.ToLongDateString() + " Tarihli Pos " + RaporAdi + " Ektedir.";
        }
        private void gridView1_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            PrintingSystemBase pb = e.PrintingSystem as PrintingSystemBase;
            pb.PageSettings.Landscape = true;
        }
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == tab_Genelrapor)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl1, tab_Genelrapor.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Satis)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl11, tab_Satis.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Iptalcekraporu)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl2, tab_Iptalcekraporu.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Cekdetay)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl3, tab_Cekdetay.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Receteozet)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl4, tab_Receteozet.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Garsonsatis)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl5, tab_Garsonsatis.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Fatura)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl6, tab_Fatura.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_OdemeRaporu)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl7, tab_OdemeRaporu.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Satisrapor)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl8, tab_Satisrapor.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Log)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl9, tab_Log.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Alacak)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl10, tab_Alacak.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Uye)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl12, tab_Uye.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_DepartmanOzet)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl13, tab_DepartmanOzet.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_Zayi)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl14, tab_Zayi.Text);
            }
            if (xtraTabControl1.SelectedTabPage == tab_AylikGenelRapor)
            {
                Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gridControl15, tab_AylikGenelRapor.Text);
            }
            //Mail_Gonder(dateTarih1.DateTime, dateTarih2.DateTime, gC, xtraTabControl1.SelectedTabPage.Text);
        }
        private void barButtonItem3_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show(res_man.GetString("Gelir Hesaplar için Mahsup Kesmek İstiyor Musunuz ?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Pos_Gelir_Mahsup(true);
            }
        }
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show(res_man.GetString("Gelir Hesaplar için Mahsup Kesmek İstiyor Musunuz ?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Pos_Gelir_Mahsup(false);
            }
        }
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MasaGeriAl();
        }
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int Fisno = 0;
            DateTime Tarih = Param.Tarih;
            if (xtraTabControl1.SelectedTabPage == tab_Genelrapor || xtraTabControl1.SelectedTabPage == tab_Satis)
            {
                if (xtraTabControl1.SelectedTabPage == tab_Genelrapor)
                {
                    Fisno = Convert.ToInt32(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                    Tarih = Convert.ToDateTime(bandedGridView1.GetFocusedRowCellValue("Rsat_Tarih"));
                }
                if (xtraTabControl1.SelectedTabPage == tab_Satis)
                {
                    Fisno = Convert.ToInt32(gridView11.GetFocusedRowCellValue("Rsat_Fisno"));
                    Tarih = Convert.ToDateTime(gridView11.GetFocusedRowCellValue("Rsat_Tarih"));
                }
                if (Tarih.Date != Param.Tarih.Date)
                {
                    MessageBox.Show(res_man.GetString("Geçmiş Tarihe Ait Ödeme Bilgileri Değiştirilemez..."), res_man.GetString("Uyarı"));
                    return;
                }
                /*
                if (Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Durum")) != "Acik")
                {
                    MessageBox.Show("Kapalı masanın ödeme tipini değiştiremezseniz!\nİlk önce masayı geri al yapınız!");
                    return;
                }*/
                Hesap h = new Hesap();
                h.fisno = Fisno;
                h.Tag = Fisno;
                h.odemetipiDegistirdenmigeldi = true;
                h.tip = "O";
                h.ShowDialog();

                gridyenile();
            }
        }
        private void barButtonItem5_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FisIptal();
        }
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XZ_Raporu xz = new XZ_Raporu();
            xz.setDepartman = Convert.ToString(cmb_Departman.EditValue);
            xz.ShowDialog();
        }
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == tab_Genelrapor || xtraTabControl1.SelectedTabPage == tab_Satis || xtraTabControl1.SelectedTabPage == tab_Log)
            {
                int Fisno = 0;
                if (xtraTabControl1.SelectedTabPage == tab_Genelrapor)
                {
                    Fisno = Convert.ToInt32(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                }
                if (xtraTabControl1.SelectedTabPage == tab_Satis)
                {
                    Fisno = Convert.ToInt32(gridView11.GetFocusedRowCellValue("Rsat_Fisno"));
                }
                if (xtraTabControl1.SelectedTabPage == tab_Log)
                {
                    Fisno = Convert.ToInt32(gridView9.GetFocusedRowCellValue("Fisno"));
                }
                if (Fisno > 0)
                {
                    Fis_Log log = new Fis_Log();
                    log.Fisno = Fisno;
                    log.ShowDialog();
                }
            }
        }
        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
            dbtools.coneskiyedon();

        }
        private void chk_Detay_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Detay.Checked == true)
            {
                DataTable dt = dbtools.SelectTable("SELECT Kodlar_Id ,Kodlar_Kod, Kodlar_Ad, Kodlar_Sinif  FROM Stok_Kodlar with(nolock) where Kodlar_Sinif = '11' order by Kodlar_Kod");
                cmb_DetayGrup.Properties.DataSource = dt;
                cmb_DetayGrup.Properties.DisplayMember = "Kodlar_Ad";
                cmb_DetayGrup.Properties.ValueMember = "Kodlar_Kod";
            }
            else
            {
                //DataTable bos = new DataTable();
                cmb_DetayGrup.Properties.DataSource = null;
            }
        }
        private void kişiSayısıDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KisiSayisiDegistir kisi = new KisiSayisiDegistir();
            kisi.Fisno = Convert.ToInt32(gridView11.GetFocusedRowCellValue("Rsat_Fisno"));
            kisi.KisiSayisi = Convert.ToInt32(gridView11.GetFocusedRowCellValue("Rsat_Kisi"));
            kisi.ShowDialog();
            barButtonItem1_ItemClick(null, null);
        }
        private void gridView11_DoubleClick(object sender, EventArgs e)
        {
            Detay();
        }
        private void bandedGridView1_DoubleClick(object sender, EventArgs e)
        {
            Detay();
        }
        private void faturaİptalEtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Faturayı İptal Etmek İstiyormusunuz?", res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dbtools.execcmd("Delete From Pos_Fatura where PFat_Cekno = '" + Convert.ToString(gridView6.GetFocusedRowCellValue("PFat_Cekno")) + "'");
            }
            barButtonItem1_ItemClick(null, null);
        }
        private void btnLogSatirSilRapor_Click(object sender, EventArgs e)
        {
            gridControl9.DataSource = null;
            gridView9.Columns.Clear();


            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 8);
            com.Parameters.AddWithValue("@UyeOzet", 1); // satır silden geldiğini fark etmesi için nasıl olsa bu alan kullanmıo
            com.Parameters.AddWithValue("@Tarih1", dateTarih1.DateTime.Date);
            com.Parameters.AddWithValue("@Tarih2", dateTarih2.DateTime.Date);
            com.Parameters.AddWithValue("@Kullanici", User.P_Kod);
            if (User.Pos_IWERep == true) com.Parameters.AddWithValue("@IWERep", 1);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            gridControl9.DataSource = dt;
            string fileName = getLogDizaynPath();
            if (File.Exists(fileName))
            {
                gridView9.RestoreLayoutFromXml(fileName);
            }
            gridView9.BestFitColumns();
            gridviewCountYaz(gridView9, "Tutar");
        }
        public static void gridviewCountYaz(GridView grid, string fieldName)
        {
            try
            {
                if (grid.Columns.Count > 0)
                {
                    grid.OptionsView.ShowFooter = true;
                    grid.Columns[fieldName].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    grid.Columns[fieldName].SummaryItem.FieldName = fieldName;
                    grid.Columns[fieldName].SummaryItem.DisplayFormat = "{0:n0}";
                }
            }
            catch (Exception)
            {
            }
        }
        public static string MyClass = "Raporlar";
        private void btnLogGridDizaynKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                gridView9.SaveLayoutToXml(getLogDizaynPath());
                MessageBox.Show("Grid Dizayn Kaydedildi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
            }
        }
        public string getLogDizaynPath()
        {
            string klasorAd = "GridDizaynPos";
            if (!Directory.Exists(klasorAd))
            {
                Directory.CreateDirectory(klasorAd);
            }
            return klasorAd + @"\" + User.P_Kod + "_Log.xml";
        }
        private void btnLogGridDizaynSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RHMesaj.MyMessageConfirmation("\"" + User.P_Kod + "\" Kullanıcısına Ait Grid Dizayn'ı Silmek İstediğinize Emin misiniz ?"))
                {
                    return;
                }
                string path = getLogDizaynPath();
                if (File.Exists(path))
                {
                    File.Delete(path);
                    RHMesaj.alertMesaj("Grid Dizayn Temizlendi");
                }
                else
                {
                    RHMesaj.alertMesaj("Grid Dizayn BULUNAMADI! \n " + path);
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnGridDizaynTemizle1_Click", "", ex);
            }
        }
        public string getIptalCekDizaynPath()
        {
            string klasorAd = "GridDizaynPos";
            if (!Directory.Exists(klasorAd))
            {
                Directory.CreateDirectory(klasorAd);
            }
            return klasorAd + @"\" + User.P_Kod + "_IptalCek.xml";
        }
        public string getGenelCekDizaynPath()
        {
            string klasorAd = "GridDizaynPos";
            if (!Directory.Exists(klasorAd))
            {
                Directory.CreateDirectory(klasorAd);
            }
            return klasorAd + @"\" + User.P_Kod + "_GenelCek.xml";
        }



       


        private void btnIptalGridDizanyKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                gridView2.SaveLayoutToXml(getIptalCekDizaynPath());
                MessageBox.Show("Grid Dizayn Kaydedildi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
            }
        }
        private void btnIptalGridDizanySil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RHMesaj.MyMessageConfirmation("\"" + User.P_Kod + "\" Kullanıcısına Ait Grid Dizayn'ı Silmek İstediğinize Emin misiniz ?"))
                {
                    return;
                }
                string path = getIptalCekDizaynPath();
                if (File.Exists(path))
                {
                    File.Delete(path);
                    RHMesaj.alertMesaj("Grid Dizayn Temizlendi");
                }
                else
                {
                    RHMesaj.alertMesaj("Grid Dizayn BULUNAMADI! \n " + path);
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnGridDizaynTemizle1_Click", "", ex);
            }
        }
        private void btnGenelCekDizaynKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                bandedGridView1.SaveLayoutToXml(getGenelCekDizaynPath());
                MessageBox.Show("Grid Dizayn Kaydedildi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
            }
        }
        private void btnGenelCekDizaynSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RHMesaj.MyMessageConfirmation("\"" + User.P_Kod + "\" Kullanıcısına Ait Grid Dizayn'ı Silmek İstediğinize Emin misiniz ?"))
                {
                    return;
                }
                string path = getGenelCekDizaynPath();
                if (File.Exists(path))
                {
                    File.Delete(path);
                    RHMesaj.alertMesaj("Grid Dizayn Temizlendi");
                }
                else
                {
                    RHMesaj.alertMesaj("Grid Dizayn BULUNAMADI! \n " + path);
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnGridDizaynTemizle1_Click", "", ex);
            }
        }
        private void Raporlar_Shown(object sender, EventArgs e)
        {
            gridyenile();
        }
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            gridyenile();
            //if (xtraTabControl1.SelectedTabPage == tab_Satis)
            //{
            //    gridyenile();
            //}
        }
        private void btnMuhListele_Click(object sender, EventArgs e)
        {
            muhasebeRapor(true);
        }
        MuhasebeRapor rapor = null;
        public MuhasebeRapor muhasebeRapor(bool mailGitsin = true)
        {
            rapor = new MuhasebeRapor();
            try
            {
                string tar1 = dateTarih1.DateTime.ToString("yyyy-MM-dd");
                string tar2 = dateTarih2.DateTime.ToString("yyyy-MM-dd");
                string tarih = Param.Tarih.ToString("yyyy-MM-dd");
                string query = @"declare @Fis_Tutar decimal(18,2) = (select SUM(Satis.Rsat_Tutar)   
FROM Cst_Recete_Satis as satis WITH(NOLOCK) 
LEFT JOIN Pos_Kodlar as  kodlar WITH(NOLOCK) ON Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' and Pkod_Ozelkod <> '4'
where  Rsat_Tarih between '" + tar1 + @"' and '" + tar2 + @"' AND Satis.Rsat_Ba = 'B' and Rsat_Satistip<>'O' )  declare @Katsayi decimal(18,8) = ((select SUM(ISNULL(Rsat_Tutar,0)) as Tutar from Cst_Recete_Satis 
WITH(NOLOCK)  LEFT JOIN Pos_Kodlar as  kodlar 
WITH(NOLOCK) ON Rsat_Kapatma = kodlar.Pkod_Kod and kodlar.Pkod_Sinif = '11' 
where Rsat_Tarih between '" + tar1 + @"' and '" + tar2 + @"' and Pkod_Ozelkod <> '4' and Rsat_Ba = 'A' and Rsat_Satistip<>'O' group by Pkod_Fatura) / @Fis_Tutar )  
if @Katsayi = 0 begin set @Katsayi = 1 end  SELECT MIN(convert(date,Rsat_Tarih)) as Rsat_Tarih,
Kodlar_Ad + ' BEDELI' as Aciklama,
MIN(Rsat_Kdvoran) as Rec_Kdv,      
((SUM(Rsat_Tutar) * 100 / (100 + MIN(Rsat_Kdvoran))) * MIN(Rsat_Kdvoran) / 100) * @Katsayi as Kdv,       
(SUM(Rsat_Tutar) * 100 / (100 + MIN(Rsat_Kdvoran))) * @Katsayi as Net,      
(SUM(Rsat_Tutar)) * @Katsayi as Tutar
FROM Cst_Recete_Satis WITH(NOLOCK)      
LEFT JOIN Cst_Recete WITH(NOLOCK) on Rec_Genelkod = Rsat_Recete      
LEFT JOIN Stok_Kodlar WITH(NOLOCK) on Rec_Urungrup = Kodlar_Kod AND Kodlar_Sinif = '10'  
WHERE Rsat_Tarih between '" + tar1 + @"' and '" + tar2 + @"' AND Rsat_Ba = 'B' and Rsat_Satistip<>'O'
GROUP BY Kodlar_Ad  ORDER BY Kodlar_Ad desc";
                DataTable dataTable = dbtools.SelectTableR(query);
                decimal brutToplam = 0;
                decimal netToplam = 0;
                if (dataTable != null && dataTable.Rows.Count > 0)
                    foreach (DataRow row in dataTable.Rows)
                    {
                        brutToplam += Convert.ToDecimal(row["Tutar"].ToString());
                        netToplam += Convert.ToDecimal(row["Net"].ToString());
                    }
                rapor.DataSource = dataTable;
                rapor.txtTarih.Text = DateTime.Now.ToString("dd.MM.yyyy");
                rapor.txtDepAd.Text = Departman.Dep_Adi;
                //rapor.txtBrutToplam.Text = brutToplam.ToString();
                //rapor.txtNetToplam.Text = netToplam.ToString();
                var istatis = dbtools.SelectTableR("exec Pos_Satis_Istatistik @Tarih1='" + tarih + "', @Tarih2='" + tarih + "',@TekDepartman='" + Departman.Dep_Kodu + "',@Tahsilat=1");
                string odemeler = "";
                if (istatis != null && istatis.Rows.Count > 0)
                {
                    bool ilksatir = false;
                    foreach (DataRow item in istatis.Rows)
                    {
                        if (ilksatir == false)
                        {
                            ilksatir = true;
                            continue;
                        }
                        string ad = item["Ad"].ToString();
                        string tutar = item["Tutar"].ToString();
                        int kalanbosluk = 15 - ad.Length;
                        string bosluk = "";
                        for (int i = 0; i < kalanbosluk; i++)
                        {
                            bosluk = bosluk + " ";
                        }
                        int sonbosluk = bosluk.Length;
                        odemeler = odemeler + ad + bosluk + tutar + "\n";
                    }
                }
                odemeler = odemeler + "\nBRÜT TOPLAM :  " + brutToplam + "\n" + "NET TOPLAM  :  " + netToplam;
                rapor.txtTumOdeme.Text = odemeler;
                string klasor = "CariRapor";
                if (!Directory.Exists(klasor))
                {
                    Directory.CreateDirectory(klasor);
                }
                string path = klasor + "\\" + DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss") + "_muh.pdf";
                gridControlMuh.DataSource = dataTable;
                rapor.ExportToPdf(path);
                //gridViewMuh.ExportToPdf(path);
                if (mailGitsin)
                {
                    // Mail_Gonder(path);
                }
                else
                {
                    rapor.ShowPreview();
                }

                gridviewSumYaz(gridViewMuh, "Kdv");
                gridviewSumYaz(gridViewMuh, "Net");
                gridviewSumYaz(gridViewMuh, "Tutar");
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError("Rapor_Sec", "muhasebeRapor", "", ex);
            }
            return rapor;
        }

        public void gridviewSumYaz(GridView grid, string fieldname)
        {
            grid.OptionsView.ShowFooter = true;

            if (grid.Columns.Count > 0)
            {
                grid.Columns[fieldname].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grid.Columns[fieldname].SummaryItem.FieldName = fieldname;
                grid.Columns[fieldname].SummaryItem.DisplayFormat = "{0:n4}";
                grid.UpdateTotalSummary();
            }
        }


        private void btnMuhExcel_Click(object sender, EventArgs e)
        {
            rapor.ShowPreview();
        }

        private void lookUpEditSubeCon_EditValueChanged(object sender, EventArgs e)
        {
            string depkod = lookUpEditSubeCon.EditValue.ToString();
            subeyukle(depkod);
        }

        public void subeyukle(string depkod)
        {
            try
            {
                string q1 = $@"select top 1
                                       Pkod_SubeMac as [SubeMac],
                                        Pkod_Server as [Server],
                                        Pkod_Database as [Database],
                                        Pkod_User as [User],
                                        Pkod_Password as [Password],Pkod_Kod
                                        from pos_kodlar
                                        where
                                        Pkod_Sinif = 27
                                        and
                                        Pkod_MerkezSube = 'S' and Pkod_Kod='{depkod}'";

                dbtools.coneskiyedon();

                DataTable dt = dbtools.SelectTableR(q1);

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Dep kod şube tanımda yok");
                    return;
                }
                var server = dt.Rows[0]["Server"].ToString();
                var database = dt.Rows[0]["Database"].ToString();
                var users = dt.Rows[0]["User"].ToString();
                var pwd = dt.Rows[0]["Password"].ToString();

                dbtools.conYenile(server, database, users, pwd);
                Departman.Dep_Param_Yukle();
                Param.Param_Yukle();
                FisPr.Param_Yukle();
                User.Yetki_Yukle();
                loadyukle();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void radioGroup1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (radioGroupMerkezSube.SelectedIndex == 0)
            {
                lookUpEditSubeCon.Visible = false;
                labelSubeCon.Visible = false;

                string depkod = Departman.Dep_Kodu;
                subeyukle(depkod);
            }
            else
            {
                lookUpEditSubeCon.Visible = true;
                labelSubeCon.Visible = true;


                if (lookUpEditSubeCon.EditValue == null) return;
                string depkod = lookUpEditSubeCon.EditValue.ToString();

                subeyukle(depkod);
            }

        }

        private void gridView6_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string efatNo = view.GetRowCellDisplayText(e.RowHandle, view.Columns["PFat_EFatno"]);
                if (!string.IsNullOrWhiteSpace(efatNo))
                {
                    // Doluysa kırmızı
                    e.Appearance.BackColor = Color.LightGreen;
                    e.Appearance.ForeColor = Color.White;
                }
                else
                {
                    // Boşsa yeşil
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                }
            }
        }

        private void btnYazdirilmisIpt_Click(object sender, EventArgs e)
        {
            string q1 = $@"select * 
                          from Pos_log WITH (NOLOCK) 
                           where Log_Islem='Sil' 
                            and Log_Bolum='Satir_Sil' 
                            and  Log_Yazdirilmis='E' 
                            and Log_Aciklama LIKE '%yazdırılmış%'
                            and Log_Pos_Tarih between '{dateTarih1.DateTime.ToString("yyyy-MM-dd")}' and '{dateTarih2.DateTime.ToString("yyyy-MM-dd")}'
                          order by Log_Pos_Tarih desc ";
            DataTable data = dbtools.SelectTableR(q1);

            gridControl9.DataSource = null;
            gridView9.Columns.Clear();

            gridControl9.DataSource = data;
        }

        private void btnTutarDuzeltmis_Click(object sender, EventArgs e)
        {
            string q1 = $@"select * 
from Pos_log WITH (NOLOCK) 
where Log_Islem='Duzelt' 
  and Log_Bolum='Tutar_Duzelt' 
                            and Log_Pos_Tarih between '{dateTarih1.DateTime.ToString("yyyy-MM-dd")}' and '{dateTarih2.DateTime.ToString("yyyy-MM-dd")}'
                          order by Log_Pos_Tarih desc ";
            DataTable data = dbtools.SelectTableR(q1);

            gridControl9.DataSource = null;
            gridView9.Columns.Clear();

            gridControl9.DataSource = data;
        }

        private void btnMasaTransf_Click(object sender, EventArgs e)
        {
            string q1 = $@"select * 
from Pos_log WITH (NOLOCK) 
where Log_Islem='Duzelt' 
  and Log_Bolum='Masa_Transfer' 
                            and Log_Pos_Tarih between '{dateTarih1.DateTime.ToString("yyyy-MM-dd")}' and '{dateTarih2.DateTime.ToString("yyyy-MM-dd")}'
                          order by Log_Pos_Tarih desc ";
            DataTable data = dbtools.SelectTableR(q1);

            gridControl9.DataSource = null;
            gridView9.Columns.Clear();

            gridControl9.DataSource = data;
        }

        private void btnUrunTransf_Click(object sender, EventArgs e)
        {
            string q1 = $@"select * 
from Pos_log WITH (NOLOCK) 
where Log_Islem='Duzelt' 
  and Log_Bolum='Malz_Transfer' 
                            and Log_Pos_Tarih between '{dateTarih1.DateTime.ToString("yyyy-MM-dd")}' and '{dateTarih2.DateTime.ToString("yyyy-MM-dd")}'
                          order by Log_Pos_Tarih desc ";
            DataTable data = dbtools.SelectTableR(q1);

            gridControl9.DataSource = null;
            gridView9.Columns.Clear();

            gridControl9.DataSource = data;
        }

        private void btnIndirimMasa_Click(object sender, EventArgs e)
        {
            string q1 = $@"select * 
from Pos_log WITH (NOLOCK) 
where Log_Islem='Kaydet' 
  and Log_Bolum='Indirim_Uygula' 
                            and Log_Pos_Tarih between '{dateTarih1.DateTime.ToString("yyyy-MM-dd")}' and '{dateTarih2.DateTime.ToString("yyyy-MM-dd")}'
                          order by Log_Pos_Tarih desc ";
            DataTable data = dbtools.SelectTableR(q1);

            gridControl9.DataSource = null;
            gridView9.Columns.Clear();

            gridControl9.DataSource = data;
        }

        private void btn_XZ2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XZ_Raporu xz = new XZ_Raporu();
            xz.setDepartman = Convert.ToString(cmb_Departman.EditValue);
            xz.look_Garson.EditValue = User.P_Kod;
            xz.ShowDialog();
        }

        private void gridView11_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                GridView View = sender as GridView;
                if (e.Column.FieldName == "bekoDurum" || e.Column.FieldName == "bekoAciklama" || e.Column.FieldName == "bekoId")
                {
                    string bekoDurum = View.GetRowCellDisplayText(e.RowHandle, View.Columns["bekoDurum"]);

                    switch (bekoDurum)
                    {
                        case "0":
                            break;
                        case "1": // hesap dökülmüş turuncu
                            e.Appearance.BackColor = Color.DarkOrange;
                            e.Appearance.ForeColor = Color.White;
                            break;
                        case "2": // hesap kapanmış
                            e.Appearance.BackColor = Color.DarkGreen;
                            e.Appearance.ForeColor = Color.White;
                            break;
                        case "3":
                            e.Appearance.BackColor = Color.Red;
                            e.Appearance.ForeColor = Color.White;
                            break;
                        default:
                            break;
                    }

                }
            }

        }

        private void btnOnburoIadeYap_Click(object sender, EventArgs e)
        {
            try
            {
                TutarGirForm tutarGirForm = new TutarGirForm();
                tutarGirForm.ShowDialog();


                if (tutarGirForm.tutar == 0 || tutarGirForm.vazgec == true)
                {
                    RHMesaj.alertMesaj("VAZGEÇİLDİ");
                    return;
                }

                var fisno = Convert.ToInt32(gridView11.GetFocusedRowCellValue("Rsat_Fisno"));
                var Rsat_Dovizbrut = Convert.ToDecimal(gridView11.GetFocusedRowCellValue("Rsat_Dovizbrut"));


                if (tutarGirForm.tutar> Rsat_Dovizbrut)
                {
                    MessageBox.Show("İADE TUTARI, TOPLAM TUTARDAN BÜYÜK OLAMAZ !");
                    return;
                }

                string frontDb = Fronttools.database.ToString();

                string girilenIadeTutar = tutarGirForm.tutar.ToString().Replace(",",".");

                string query = $@"INSERT INTO {frontDb}.dbo.Kumhrk
(
	Kumhrk_Tarih,
	Kumhrk_Rez_id,
	Kumhrk_Oda,
	Kumhrk_Dep_kodu,
	Kumhrk_Ba,
	Kumhrk_Bfd,
	Kumhrk_Me,
	Kumhrk_Re,
	Kumhrk_Tipi,
	Kumhrk_Doviz_kodu,
	Kumhrk_Kur,
	Kumhrk_Doviz_tutar,
	Kumhrk_Tutar,
	Kumhrk_Def_doviz,
	Kumhrk_Odenmez,
	Kumhrk_Eski_folio,
	Kumhrk_Zaman,
	Kumhrk_Kartno,
	Kumhrk_Kart_id,
	Kumhrk_Cekno,
	Kumhrk_Aciklama,
	Kumhrk_Safe_no,
	Kumhrk_Nakit_kredi,
	Kumhrk_Posting_kodu,
	Kumhrk_Pos_no,
	Kumhrk_Fatura_eh,
	Kumhrk_Fatura_Id,
	Kumhrk_Fatura_no,
	Kumhrk_Rezkira_id,
	Kumhrk_Kulanici_id,
	Kumhrk_Kulanici_kodu,
	Kumhrk_Sirket,
	Kumhrk_Otel,
	Kumhrk_Yil_kodu,
	Kumhrk_Tel_id,
	Kumhrk_Foltr_eh,
	Kumhrk_Eski_oda,
	Kumhrk_Sistem_tarihi,
	Kumhrk_Sistem_unv,
	Kumhrk_Fatura_kesildi_eh,
	Kumhrk_Fatura_bolum,
	Kumhrk_Pm_eh,
	Kumhrk_Islem_tarihi,
	Kumhrk_Yazarkasa_10,
	Kumhrk_Gunluk_aylik,
	Kum_Konver_oran,
	Kum_Konver,
	Kum_Konver_matrah,
	Kumhrk_Kdvyuzde,
	Kumhrk_Kdvsiz,
	Kumhrk_Doviz_kdvsiz
)
SELECT TOP 1
	k.Kumhrk_Tarih,
	k.Kumhrk_Rez_id,
	k.Kumhrk_Oda,
	k.Kumhrk_Dep_kodu,
	'B',
	'I',
	k.Kumhrk_Me,
	k.Kumhrk_Re,
	k.Kumhrk_Tipi,
	k.Kumhrk_Doviz_kodu,
	k.Kumhrk_Kur,
	-{girilenIadeTutar},
	({girilenIadeTutar}*k.Kumhrk_Kur)*-1,
	-{girilenIadeTutar},
	k.Kumhrk_Odenmez,
	k.Kumhrk_Eski_folio,
	k.Kumhrk_Zaman,
	k.Kumhrk_Kartno,
	k.Kumhrk_Kart_id,
	k.Kumhrk_Cekno,
	k.Kumhrk_Aciklama,
	k.Kumhrk_Safe_no,
	k.Kumhrk_Nakit_kredi,
	k.Kumhrk_Posting_kodu,
	k.Kumhrk_Pos_no,
	k.Kumhrk_Fatura_eh,
	k.Kumhrk_Fatura_Id,
	k.Kumhrk_Fatura_no,
	k.Kumhrk_Rezkira_id,
	k.Kumhrk_Kulanici_id,
	k.Kumhrk_Kulanici_kodu,
	k.Kumhrk_Sirket,
	k.Kumhrk_Otel,
	k.Kumhrk_Yil_kodu,
	k.Kumhrk_Tel_id,
	k.Kumhrk_Foltr_eh,
	k.Kumhrk_Eski_oda,
	k.Kumhrk_Sistem_tarihi,
	k.Kumhrk_Sistem_unv,
	k.Kumhrk_Fatura_kesildi_eh,
	k.Kumhrk_Fatura_bolum,
	k.Kumhrk_Pm_eh,
	k.Kumhrk_Islem_tarihi,
	k.Kumhrk_Yazarkasa_10,
	k.Kumhrk_Gunluk_aylik,
	k.Kum_Konver_oran,
	k.Kum_Konver,
	k.Kum_Konver_matrah,
	k.Kumhrk_Kdvyuzde,
	k.Kumhrk_Kdvsiz,
	k.Kumhrk_Doviz_kdvsiz
FROM {frontDb}.dbo.Kumhrk k
LEFT JOIN {frontDb}.dbo.rez r 
	ON r.Rez_Id = k.Kumhrk_Rez_id
WHERE 
	k.Kumhrk_Cekno = '{fisno}'
	AND k.Kumhrk_Oda = (
		SELECT TOP 1 Rsat_Odano 
		FROM Cst_Recete_Satis
		WHERE Rsat_Fisno = '{fisno}'
		AND Rsat_Odano <> ''
		AND Rsat_Odano IS NOT NULL
	)
	AND k.Kumhrk_Ba = 'B'
	AND r.Rez_R_I_H = 'I';";


                dbtools.execcmdR(query);

               var tltutar =  Param.Doviz_Kuru * tutarGirForm.tutar;
                var tltutarstr = tltutar.ToString().Replace(",",".");

                string indirimQuery = $@"exec Pos_Manuel_Indirim @Fisno={fisno},@Ind_Tip=N'T',@Ind_Tutar=N'{tltutarstr}',@Ind_Doviztutar=N'{girilenIadeTutar}',@Ind_Oran=N'0',@Ind_Turu=N'MANUEL',@Split=0,@Ind_User=N'{User.P_Kod}',@aciklama=N''";

                dbtools.execcmdR(indirimQuery);


                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.UrunIade, Log.Log_Islem.UrunIade, "Önbüro iade yapıldı Fisno:" +fisno+" ... Girilen iade tutarı : "+ girilenIadeTutar, "","");


                MessageBox.Show("IADE BAŞARILI ÖNBÜRODAN KONTROL EDİNİZ!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void satisRaporDizaynKaydet2_Click(object sender, EventArgs e)
        {
            try
            {
                gridView11.SaveLayoutToXml(getSatisDizaynPath2());
                MessageBox.Show("Grid Dizayn Kaydedildi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
            }
        }

        public string getSatisDizaynPath2()
        {
            string klasorAd = "GridDizaynPos";
            if (!Directory.Exists(klasorAd))
            {
                Directory.CreateDirectory(klasorAd);
            }
            return klasorAd + @"\" + User.P_Kod + "_Satis2.xml";
        }

        private void satisRaporDizaynTemizle2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RHMesaj.MyMessageConfirmation("\"" + User.P_Kod + "\" Kullanıcısına Ait Grid Dizayn'ı Silmek İstediğinize Emin misiniz ?"))
                {
                    return;
                }
                string path = getSatisDizaynPath2();
                if (File.Exists(path))
                {
                    File.Delete(path);
                    RHMesaj.alertMesaj("Grid Dizayn Temizlendi");
                }
                else
                {
                    RHMesaj.alertMesaj("Grid Dizayn BULUNAMADI! \n " + path);
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "satisRaporDizaynTemizle2_Click", "", ex);
            }
        }
    }
}