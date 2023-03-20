using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json;
using Pos.Class;
using Pos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos
{
    public partial class ParcaliOdeme : Form
    {

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        public int Split = 0;
        public static string MyClass = "ParcaliOdeme";
        public string fisno = "0";
        public string anamasano = "";
        public string altmasano = "";
        public string odemetip = "";
        public string hesapno = "";
        public ParcaliOdeme(string fisno, string anamasano)
        {
            InitializeComponent();
            this.fisno = fisno;
            this.Tag = fisno;
            this.anamasano = anamasano;
        }

        DataTable dtAna = new DataTable();
        DataTable dtAnaMasalari = new DataTable();

        List<SimpleButton> buttons = new List<SimpleButton>();
        List<SimpleButton> buttonsOdeme = new List<SimpleButton>();
        public void anaListele()
        {
            try
            {
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 2);
                com.Parameters.AddWithValue("@Split", Split);
                SqlDataAdapter da = new SqlDataAdapter(com);
                dtAna = new DataTable();
                da.Fill(dtAna);
                /*Rsat_Id Rsat_Fisno Rsat_Masa Rsat_Tarih Rsat_Acilis Rsat_Ba Kasiyer Rsat_Miktar Rsat_Emiktar Rec_Ad Rsat_Tutar Rsat_Doviz MasaKonumAdi Rsat_UrunTahsilat Rsat_Recete Rsat_UrunBazliHspAdet*/


                gridControlAna.DataSource = dtAna;


                groupControlAna.Text = "ANA Fişno: " + fisno + " - Masa No: " + anamasano;
                string query = "select Masa_Id,Masa_No,Masa_Ad,Masa_Durum,Masa_Parcali from Pos_Masa where Masa_No like '" + anamasano + "[_]%' "; // and Masa_Durum = '0'
                dtAnaMasalari = dbtools.SelectTableR(query);


                if (dtAnaMasalari != null && dtAnaMasalari.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAnaMasalari.Rows.Count; i++)
                    {
                        DataRow row = dtAnaMasalari.Rows[i];
                        buttons[i].Tag = row["Masa_No"].ToString();
                        buttons[i].Visible = true;
                        buttons[i].Text = row["Masa_Ad"].ToString();
                    }
                }
                else
                {
                    RHMesaj.MyMessageInformation("Lütfen Masa Tanımdan parçalı masa oluşturunuz");
                }

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "anaListele", "", ex);
            }
        }

        public void ekrandakiButtonlariListele()
        {
            buttons.Add(btnAltMasa1);
            buttons.Add(btnAltMasa2);
            buttons.Add(btnAltMasa3);
            buttons.Add(btnAltMasa4);
            buttons.Add(btnAltMasa5);
            buttons.Add(btnAltMasa6);
            buttons.Add(btnAltMasa7);
            buttons.Add(btnAltMasa8);
            buttons.Add(btnAltMasa9);
            buttons.Add(btnAltMasa10);


            buttonsOdeme.Add(btnOdeme1);
            buttonsOdeme.Add(btnOdeme2);
            buttonsOdeme.Add(btnOdeme3);
            buttonsOdeme.Add(btnOdeme4);
            buttonsOdeme.Add(btnOdeme5);
            buttonsOdeme.Add(btnOdeme6);
            buttonsOdeme.Add(btnOdeme7);
            buttonsOdeme.Add(btnOdeme8);
            buttonsOdeme.Add(btnOdeme9);
            buttonsOdeme.Add(btnOdeme10);
            buttonsOdeme.Add(btnOdeme11);
            buttonsOdeme.Add(btnOdeme12);
        }
        private void ParcaliOdeme_Load(object sender, EventArgs e)
        {
            ekrandakiButtonlariListele();
            anaListele();
            lookKapatmaYukle();

            btnYazdirKapat.Enabled = User.G_Yazdirkapat;
            btnYazdirmadanKapat.Enabled = User.G_Yazdirmadankapat;
            btnAraOdemeAl.Enabled = User.G_Odemeal;
            btnHesapDokum.Enabled = User.G_Hesapdokumu;
            btnOdemeSil.Enabled = User.G_Odemesil;
            btnIndirim.Enabled = User.G_Indirim_Hesap;
        }

        private void gridViewAna_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Rsat_Miktar")
            {

                e.DisplayText = e.DisplayText.Replace(",0000", "");
                e.DisplayText = e.DisplayText.Replace(",000", "");
                e.DisplayText = e.DisplayText.Replace(",00", "");
            }
        }

        DataTable dtOdemeKodlari = new DataTable();
        public void lookKapatmaYukle()
        {
            try
            {
                dtOdemeKodlari = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");

                if (dtOdemeKodlari == null || dtOdemeKodlari.Rows.Count < 1)
                {
                    dtOdemeKodlari = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43");
                }

                // Pkod_Ad Pkod_Kod
                for (int i = 0; i < dtOdemeKodlari.Rows.Count; i++)
                {
                    DataRow row = dtOdemeKodlari.Rows[i];
                    buttonsOdeme[i].Tag = row["Pkod_Kod"].ToString();
                    buttonsOdeme[i].Visible = true;
                    buttonsOdeme[i].Text = row["Pkod_Ad"].ToString();
                    string renk = row["Pkod_OdemeBtnRenk"].ToString();
                    if (renk != "")
                    {
                        buttonsOdeme[i].Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml(row["Pkod_OdemeBtnRenk"].ToString());
                    }

                }


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "lookKapatmaYukle", "", ex);
            }

        }


        private void btnAltMasa1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (SimpleButton item in buttons)
                {
                    item.Appearance.BackColor = Color.Transparent; // varsayılan rengi
                    item.Appearance.Options.UseBackColor = true;
                }

                SimpleButton tiklanan = (sender as SimpleButton);
                tiklanan.Appearance.BackColor = Color.Lime;


                //groupControlAlt.Text = "Alt Fişno: " + fisno + " - Masa No: " + tiklanan.Text;
                groupControlAlt.Text = "Alt Masa No: " + tiklanan.Text;

                altmasano = tiklanan.Tag.ToString();
                altmasayenile();



            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnAltMasa1_Click", "", ex);
            }
        }

        public void altmasayenile()
        {
            try
            {
                string query = @"select Cst_Recete_Satis.*, 
case when Cst_Recete.Rec_Ad is null 
then (select top 1 Pkod_Ad from Pos_Kodlar where Pkod_Sinif = '11'  and Pkod_Kod=Rsat_Kapatma)
else Cst_Recete.Rec_Ad end as 'Rec_Ad'
from Cst_Recete_Satis 
left join Cst_Recete on Rec_Genelkod=Rsat_Recete
where Rsat_Durum='A' and Rsat_Masa='" + altmasano + "' order by Rsat_Id";
                DataTable dataTable = dbtools.SelectTableR(query);

                foreach (DataRow item in dataTable.Rows)
                {
                    if (item["Rsat_Kapatma"] != null && item["Rsat_Kapatma"].ToString() != "")
                    {
                        item["Rsat_Tutar"] = Convert.ToDecimal(item["Rsat_Tutar"].ToString()) * -1;
                    }
                }

                gridControlAlt.DataSource = dataTable;


                txtOdemeTutar.Text = gridViewAlt.Columns["Rsat_Tutar"].SummaryItem.SummaryValue.ToString();

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "altmasayenile", "", ex);
            }
        }

        private void btnAnadanAlta_Click(object sender, EventArgs e)
        {
            try
            {
                /*Rsat_Id Rsat_Fisno Rsat_Masa Rsat_Tarih Rsat_Acilis Rsat_Ba Kasiyer Rsat_Miktar Rsat_Emiktar Rec_Ad Rsat_Tutar Rsat_Doviz MasaKonumAdi Rsat_UrunTahsilat Rsat_Recete Rsat_UrunBazliHspAdet*/


                if (altmasano == "")
                {
                    RHMesaj.MyMessageInformation("Lütfen masa seçiniz!");
                    return;
                }
                DataRow neredenRow = gridViewAna.GetDataRow(gridViewAna.FocusedRowHandle);


                string neredenMasa = anamasano;
                string nereyeMasa = altmasano;


                aktarim(neredenRow, neredenMasa, nereyeMasa, 0);


            }
            catch (Exception ex)
            {

                RHMesaj.MyMessageError(MyClass, "btnAnadanAlta_Click", "", ex);
            }
            finally
            {
                anaListele();
                altmasayenile();


                DataTable dataAna = gridControlAna.DataSource as DataTable;

                if (dataAna==null || dataAna.Rows.Count<1)
                {
                    string masakapatmaquery = "update Pos_Masa set Masa_Durum = 0, Masa_Ozel = '', Masa_NFC = 0 where Masa_No = '" + anamasano + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'";

                    dbtools.SelectTableR(masakapatmaquery);

                    string fisnoyaGoreQuery = @"update Cst_Recete_Satis set Rsat_Kapanis=convert(varchar(10), GETDATE(), 108),Rsat_Durum='K',Rsat_SistemDate = Getdate() where Rsat_Fisno='" + fisno + "'";

                    dbtools.SelectTableR(fisnoyaGoreQuery);
                }
                
            }

        }

        


        public void aktarim(DataRow neredenRow, string neredenMasa, string nereyeMasa, int fisno)
        {
            try
            {
                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);

                int rsat_id = Convert.ToInt32(neredenRow["Rsat_Id"].ToString());


                var nesne = db.Cst_Recete_Satis.Where(x => x.Rsat_Id == rsat_id).FirstOrDefault();



                var AMiktar = Convert.ToString(gridViewAna.GetFocusedRowCellValue("Rsat_Emiktar"));
                if (nesne != null)
                {
                    decimal rsattutar = Convert.ToDecimal(neredenRow["Rsat_Tutar"].ToString());
                    decimal rsatmiktar = Convert.ToDecimal(neredenRow["Rsat_Miktar"].ToString());
                    decimal kdvoran = Convert.ToDecimal(neredenRow["Rsat_Kdvoran"].ToString());
                    string urunad = neredenRow["Rec_Ad"].ToString();
                    string recete = neredenRow["Rsat_Recete"].ToString();


                    decimal aktarimMiktar = rsatmiktar;
                    if (rsatmiktar > 1)
                    {
                        Klavye1 klv = new Klavye1();
                        klv.Tag = "MALZEMETR";
                        klv.txt_Sayi.Text = rsatmiktar.ToString();
                        klv.UrunAdi = urunad;
                        klv.ShowDialog();
                        aktarimMiktar = klv.sayi;

                        if (klv.Cikis)
                        {
                            return;
                        }

                        if (aktarimMiktar <= 0)
                        {
                            return;
                        }
                    }

                    decimal aktarimmaliyet = Convert.ToDecimal(dbtools.DegerGetir("select top 1 isnull(SUM(Detay_Maliyet),0)*" + aktarimMiktar + " AS Maliyet from Cst_Recete_Detay where Detay_Recete='" + recete + "'"));

                    decimal birimFiyat = rsattutar / rsatmiktar;
                    decimal aktarimRsattutar = birimFiyat * aktarimMiktar;
                    decimal kalanmiktar = rsatmiktar - aktarimMiktar;

                    //ana ürün güncelleme
                    if (kalanmiktar == 0)
                    {
                        db.Cst_Recete_Satis.Remove(nesne);
                        db.SaveChanges();
                    }
                    else if (kalanmiktar > 0)
                    {
                        decimal anamaliyet = Convert.ToDecimal(dbtools.DegerGetir("select top 1 isnull(SUM(Detay_Maliyet),0)*" + kalanmiktar + " AS Maliyet from Cst_Recete_Detay where Detay_Recete='" + recete + "'"));
                        decimal aktarimAnaTutar = birimFiyat * kalanmiktar;
                        nesne.Rsat_Miktar = kalanmiktar;
                        nesne.Rsat_Tutar = aktarimAnaTutar;
                        nesne.Rsat_Fiyat = aktarimAnaTutar;
                        nesne.Rsat_Net = aktarimAnaTutar / ((100 + kdvoran) / 100);
                        nesne.Rsat_Kdv = (aktarimAnaTutar * kdvoran) / 100;
                        nesne.Rsat_Maliyet = anamaliyet;
                        nesne.Rsat_Doviztutar = aktarimAnaTutar;
                        db.Entry(nesne).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        RHMesaj.MyMessageInformation("TOPLAM MİKTARDAN FAZLA SATILAMAZ!");
                        return;
                    }

                    //alt ürün güncelleme -> aynı reçete varsa miktarını arttırcaz
                    var nesne2 = db.Cst_Recete_Satis.Where(x => x.Rsat_Masa == nereyeMasa & x.Rsat_Durum == "A" & x.Rsat_Recete == recete).FirstOrDefault();
                    if (nesne2 == null)
                    {
                        //alt ürün ekleme
                        nesne.Rsat_Masa = nereyeMasa;
                        nesne.Rsat_Fisno = fisno;
                        nesne.Rsat_Miktar = aktarimMiktar;
                        nesne.Rsat_Tutar = aktarimRsattutar;
                        nesne.Rsat_Fiyat = aktarimRsattutar;
                        nesne.Rsat_Net = aktarimRsattutar / ((100 + kdvoran) / 100);
                        nesne.Rsat_Kdv = (aktarimRsattutar * kdvoran) / 100;
                        nesne.Rsat_Maliyet = aktarimmaliyet;
                        nesne.Rsat_Doviztutar = aktarimRsattutar;
                        nesne.Rsat_Ind = 0;
                        nesne.Rsat_Durum = "A";
                        db.Cst_Recete_Satis.Add(nesne);
                        db.SaveChanges();
                    }
                    else
                    {
                        aktarimMiktar = aktarimMiktar + Convert.ToDecimal(nesne2.Rsat_Miktar);


                        aktarimmaliyet = Convert.ToDecimal(dbtools.DegerGetir("select top 1 isnull(SUM(Detay_Maliyet),0)*" + aktarimMiktar + " AS Maliyet from Cst_Recete_Detay where Detay_Recete='" + recete + "'"));

                        aktarimRsattutar = birimFiyat * aktarimMiktar;
                        //kalanmiktar = rsatmiktar - aktarimMiktar;


                        nesne2.Rsat_Masa = nereyeMasa;
                        nesne2.Rsat_Fisno = fisno;
                        nesne2.Rsat_Miktar = aktarimMiktar;
                        nesne2.Rsat_Tutar = aktarimRsattutar;
                        nesne2.Rsat_Fiyat = aktarimRsattutar;
                        nesne2.Rsat_Net = aktarimRsattutar / ((100 + kdvoran) / 100);
                        nesne2.Rsat_Kdv = (aktarimRsattutar * kdvoran) / 100;
                        nesne2.Rsat_Maliyet = aktarimmaliyet;
                        nesne2.Rsat_Doviztutar = aktarimRsattutar;
                        nesne2.Rsat_Ind = 0;
                        nesne2.Rsat_Durum = "A";
                        db.Entry(nesne2).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                    }


                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "", "", ex);
            }
        }


        private void btnAlttanAnaya_Click(object sender, EventArgs e)
        {
            try
            {
                /*Rsat_Id Rsat_Fisno Rsat_Masa Rsat_Tarih Rsat_Acilis Rsat_Ba Kasiyer Rsat_Miktar Rsat_Emiktar Rec_Ad Rsat_Tutar Rsat_Doviz MasaKonumAdi Rsat_UrunTahsilat Rsat_Recete Rsat_UrunBazliHspAdet*/


                if (altmasano == "")
                {
                    RHMesaj.MyMessageInformation("Lütfen masa seçiniz!");
                    return;
                }
                DataRow neredenRow = gridViewAlt.GetDataRow(gridViewAlt.FocusedRowHandle);


                string neredenMasa = altmasano;
                string nereyeMasa = anamasano;

                aktarim(neredenRow, neredenMasa, nereyeMasa, Convert.ToInt32(fisno));


            }
            catch (Exception ex)
            {

                RHMesaj.MyMessageError(MyClass, "btnAlttanAnaya_Click", "", ex);
            }
            finally
            {
                anaListele();
                altmasayenile();

                DataTable dt = gridControlAlt.DataSource as DataTable;

                if (dt == null || dt.Rows.Count < 1)
                {
                    string masakapatmaquery = "update Pos_Masa set Masa_Durum = 0, Masa_Ozel = '', Masa_NFC = 0 where Masa_No = '" + altmasano + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'";
                    dbtools.SelectTableR(masakapatmaquery);
                }
            }
        }

        private void btnOdeme1_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (SimpleButton item in buttonsOdeme)
                {
                    // Pkod_Ad Pkod_Kod
                    if (item.Tag == null) break;

                    bool kontrol = false;
                    string renk = "";
                    for (int i = 0; i < dtOdemeKodlari.Rows.Count; i++)
                    {
                        DataRow row = dtOdemeKodlari.Rows[i];
                        string kod = row["Pkod_Kod"].ToString();
                        renk = row["Pkod_OdemeBtnRenk"] == null ? "" : row["Pkod_OdemeBtnRenk"].ToString();
                        if (renk != "" && kod == item.Tag.ToString())
                        {
                            kontrol = true;
                            break;
                        }

                    }

                    if (kontrol)
                    {
                        item.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml(renk);
                    }
                    else
                    {
                        item.Appearance.BackColor = Color.Transparent; // varsayılan rengi
                        item.Appearance.Options.UseBackColor = true;
                    }

                }


                SimpleButton tiklanan = (sender as SimpleButton);

                tiklanan.Appearance.BackColor = Color.Lime;


                odemetip = tiklanan.Tag.ToString();



            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnOdeme1_Click", "", ex);
            }
        }

        private void btnOdemeTutar_Click(object sender, EventArgs e)
        {
            Klavye1 k = new Klavye1();
            k.Tag = "ODEMETUTAR";
            k.ShowDialog();

            if (k.Cikis == false)
            {
                txtOdemeTutar.EditValue = k.sayi;
            }
        }

        private void gridViewAlt_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Rsat_Miktar")
            {
                e.DisplayText = e.DisplayText.Replace(",0000", "");
                e.DisplayText = e.DisplayText.Replace(",000", "");
                e.DisplayText = e.DisplayText.Replace(",00", "");
            }


        }

        public DataTable getHesap(string ba) // a ise ödeme b ise satış
        {
            DataTable dtDeger = new DataTable();
            try
            {
                string query = @"select 
                                        sum(Rsat_Fiyat) as Rsat_Fiyat,
                                        max(Rsat_Kdvoran) as Rsat_Kdvoran,
                                        sum(Rsat_Net) as Rsat_Net,
                                        sum(Rsat_Kdv) as Rsat_Kdv,
                                        sum(Rsat_Tutar) as Rsat_Tutar,
                                        sum(Rsat_Doviztutar) as Rsat_Doviztutar
                                        from Cst_Recete_Satis 
                                        where Rsat_Masa='" + altmasano + "' and Rsat_Ba='"+ ba + "' and Rsat_Durum='A' and Rsat_Fisno='0' having sum(Rsat_Fiyat) is not null";

                 dtDeger = dbtools.SelectTableR(query);

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "getHesap", "",ex);
            }

            return dtDeger;

        }

        public void yazdirKapat(bool yazdirilsinmi, bool araodememi = false)
        {
            try
            {
                if (altmasano == "")
                {
                    RHMesaj.MyMessageInformation("Lütfen masa seçiniz!");
                    return;
                }

                DataTable dataTable = (gridControlAlt.DataSource as DataTable);
                if (dataTable == null || dataTable.Rows.Count < 1)
                {
                    RHMesaj.MyMessageInformation("Ürün Yok!");
                    return;
                }

                if (odemetip == "")
                {
                    RHMesaj.MyMessageInformation("Lütfen ödeme tipini seçiniz!");
                    return;
                }

                int rsatId = Convert.ToInt32(dataTable.Rows[0]["Rsat_Id"].ToString());

                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);
                var nesne = db.Cst_Recete_Satis.Where(x => x.Rsat_Id == rsatId).FirstOrDefault();
                if (nesne != null)
                {

                    DataTable dtOdeme = getHesap("A");
                    DataTable dtDeger = getHesap("B");

                    if (dtDeger == null || dtDeger.Rows.Count < 1)
                    {
                        RHMesaj.MyMessageInformation("Hiç satış bulunamadı!");
                        return;
                    }

                    if (dtOdeme!=null && dtOdeme.Rows.Count>0)
                    {
                        dtDeger.Rows[0]["Rsat_Fiyat"] = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Fiyat"].ToString()) - Convert.ToDecimal(dtOdeme.Rows[0]["Rsat_Fiyat"].ToString());

                        dtDeger.Rows[0]["Rsat_Kdvoran"] = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Kdvoran"].ToString()) - Convert.ToDecimal(dtOdeme.Rows[0]["Rsat_Kdvoran"].ToString());

                        dtDeger.Rows[0]["Rsat_Net"] = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Net"].ToString()) - Convert.ToDecimal(dtOdeme.Rows[0]["Rsat_Net"].ToString());

                        dtDeger.Rows[0]["Rsat_Kdv"] = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Kdv"].ToString()) - Convert.ToDecimal(dtOdeme.Rows[0]["Rsat_Kdv"].ToString());
                        dtDeger.Rows[0]["Rsat_Tutar"] = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Tutar"].ToString()) - Convert.ToDecimal(dtOdeme.Rows[0]["Rsat_Tutar"].ToString());
                        dtDeger.Rows[0]["Rsat_Doviztutar"] = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Doviztutar"].ToString()) - Convert.ToDecimal(dtOdeme.Rows[0]["Rsat_Doviztutar"].ToString());

                    }

                


                    if (dtDeger == null || dtDeger.Rows.Count < 1)
                    {
                        RHMesaj.MyMessageInformation("Hiç satış bulunamadı!");
                        return;
                    }

                
                    



                    int fisno = 0;
                    if (araodememi)
                    {
                        decimal fiyat = Convert.ToDecimal(txtOdemeTutar.Text);
                        nesne.Rsat_Fiyat = fiyat;
                        nesne.Rsat_Kdvoran = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Kdvoran"].ToString());

                        nesne.Rsat_Net = fiyat / (1 + (nesne.Rsat_Kdvoran / 100));
                        nesne.Rsat_Kdv = fiyat - (fiyat / (1 + (nesne.Rsat_Kdvoran / 100)));

                        nesne.Rsat_Tutar = fiyat;
                        nesne.Rsat_Doviztutar = fiyat;
                        nesne.Rsat_Fisno = 0;
                    }
                    else
                    {
                        fisno = Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));
                        nesne.Rsat_Fisno = fisno;

                        nesne.Rsat_Fiyat = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Fiyat"].ToString());
                        nesne.Rsat_Kdvoran = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Kdvoran"].ToString());
                        nesne.Rsat_Net = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Net"].ToString());
                        nesne.Rsat_Kdv = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Kdv"].ToString());
                        nesne.Rsat_Tutar = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Tutar"].ToString());
                        nesne.Rsat_Doviztutar = Convert.ToDecimal(dtDeger.Rows[0]["Rsat_Doviztutar"].ToString());

                    }


                    nesne.Rsat_Miktar = 1;
                    nesne.Rsat_Kapatma = odemetip;
                    nesne.Rsat_Ba = "A";
                    nesne.Rsat_Maliyet = 0;
                    nesne.Rsat_Recete = null;
                    nesne.Rsat_Ind = null;
                    nesne.Rsat_Adisyon = null;
                    nesne.Rsat_Kisi = null;
                    nesne.Rsat_Acilis = null;
                    nesne.Rsat_Aciklama = null;
                    nesne.Rsat_SiparisPr = true;
                    nesne.Rsat_AdisPr = false;
                    nesne.Rsat_Emiktar = null;
                    nesne.Rsat_AdisPrSayac = 0;
                    nesne.Rsat_Satir_Iptal = false;
                    nesne.Rsat_Uye_Id = null;
                    nesne.Rsat_Indoran = null;
                    nesne.Rsat_Onbdep = null;
                    nesne.Rsat_OzelMasaAdi = null;



                    /*
                 NOT EĞER TUTAR SIFIRSA YAZDIR KAPAT DİYİNCEDE SATIR ATIYOR HALBUKİ ARA ÖDEMELERLE TÜM TUTARI ALMIŞTI
                 SIFIR İSE SATIR ATMASIN(AŞAĞIDAKİ KOD İLE YAPILDI)
                 */
                    if (nesne.Rsat_Fiyat!=0 && nesne.Rsat_Tutar!=0)
                    {
                        db.Cst_Recete_Satis.Add(nesne);
                        db.SaveChanges();
                    }
                   


                    if (araodememi == false)
                    {
                        string fisnoAtaQuery = @"update Cst_Recete_Satis set Rsat_Fisno='" + fisno + "' where  Rsat_Masa='" + altmasano + "'  and Rsat_Durum='A' ";

                        dbtools.SelectTableR(fisnoAtaQuery);

                        string masakapatmaquery = "update Pos_Masa set Masa_Durum = 0, Masa_Ozel = '', Masa_NFC = 0 where Masa_No = '" + altmasano + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'";

                        dbtools.SelectTableR(masakapatmaquery);

                        string fisnoyaGoreQuery = @"update Cst_Recete_Satis set Rsat_Kapanis=convert(varchar(10), GETDATE(), 108),Rsat_Durum='K',Rsat_SistemDate = Getdate() where Rsat_Fisno='" + fisno + "'";

                        dbtools.SelectTableR(fisnoyaGoreQuery);


                        string aciklama = "Fiş Kapatma. Fisno:" + fisno + " Masano:" + altmasano + " Ödeme Şekli:" + odemetip + " Hesap No:" + hesapno;

                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, aciklama, fisno.ToString(), "");

                    }
                    else
                    {
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, "Rsat Id=" + nesne.Rsat_Id + " Fiş Ara Ödeme Alındı. Tutar:" + nesne.Rsat_Tutar.ToString() + " Kod: " + odemetip, nesne.Rsat_Id.ToString(), "");
                    }

                    if (yazdirilsinmi)
                    {
                        yaziciyaGonder(fisno);
                    }

                    //if (araodememi == false)
                    //{
                    //    this.Close();
                    //}
                }


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "yazdirKapat", "", ex);
            }
            finally
            {
                altmasayenile();

            }
        }

        private void btnYazdirKapat_Click(object sender, EventArgs e)
        {
            yazdirKapat(true);
        }


        public void yaziciyaGonder(int fisno)
        {
            try
            {
                FisPr pr = new FisPr();
                string cevap = pr.newHesapDokum(true, fisno, Split, "* * * HESAP KAPATMA FİŞİ * * *");
            }
            catch (Exception ex)
            {

                RHMesaj.MyMessageError(MyClass, "yaziciyaGonder", "", ex);
            }
        }

        private void btnYazdirmadanKapat_Click(object sender, EventArgs e)
        {
            yazdirKapat(false);

        }

        private void btnHesapDokum_Click(object sender, EventArgs e)
        {
            try
            {
                if (altmasano == "")
                {
                    RHMesaj.MyMessageInformation("Lütfen masa seçiniz!");
                    return;
                }

                DataTable dataTable = (gridControlAlt.DataSource as DataTable);
                if (dataTable == null || dataTable.Rows.Count < 1)
                {
                    RHMesaj.MyMessageInformation("Ürün Yok!");
                    return;
                }

                FisPr pr = new FisPr();
                string cevap = pr.newHesapDokum(true, 0, Split, "* * * HESAP KAPATMA FİŞİ * * *", parcalimi: true, parcamasano: altmasano);
            }
            catch (Exception ex)
            {

                RHMesaj.MyMessageError(MyClass, "btnHesapDokum_Click", "", ex);
            }



        }

        private void btnAraOdemeAl_Click(object sender, EventArgs e)
        {
            yazdirKapat(false, araodememi: true);
        }

        private void gridViewAlt_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                GridView View = sender as GridView;

                string kapatma = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Rsat_Kapatma"]);

                if (kapatma != null && kapatma != "")
                {
                    if (e.Column.FieldName == "Rec_Ad")
                    {
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.ForeColor = Color.White;
                    }
                }
            }
        }

        private void btnOdemeSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewAlt.FocusedRowHandle<1)
                {
                    MessageBox.Show("Lütfen ödeme satırını seçiniz !");
                    return;
                }

                if (Convert.ToString(gridViewAlt.GetFocusedRowCellValue("Rsat_Id")) == String.Empty || Convert.ToString(gridViewAlt.GetFocusedRowCellValue("Rsat_Ba")) == "B")
                {
                    MessageBox.Show(res_man.GetString("Sadece Ödeme Satırı Silinebilir..."));
                    return;
                }

                string rsatId = Convert.ToString(gridViewAlt.GetFocusedRowCellValue("Rsat_Id"));
                dbtools.execcmd(@"
                            delete from Pos_Carihrk
                            where Chrk_Id in (
                            select Chrk_Id from
                            Pos_Carihrk as chrk left join Cst_Recete_Satis as satis on 
                            chrk.Chrk_Cek = satis.Rsat_Fisno and chrk.Chrk_Cekid = satis.Rsat_Fisno and chrk.Chrk_Tarih = satis.Rsat_Tarih
                            and satis.Rsat_Departman = chrk.Chrk_Depart and satis.Rsat_Kapatma = Chrk_Odeme 
                            and satis.Rsat_Tutar = Chrk_Borc
                            where satis.Rsat_Id = '" + rsatId + "')");


                dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + rsatId + "'");

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Sil, "Ödeme Silme Fisno : " + Convert.ToString(this.Tag) + " Tutar:" + Convert.ToString(gridViewAlt.GetFocusedRowCellValue("Rsat_Tutar")), Convert.ToString(this.Tag), rsatId);

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnOdemeSil_Click", "",ex);

            }
            finally
            {
                altmasayenile();
            }
        }

        private void btnIndirim_Click(object sender, EventArgs e)
        {
            try
            {
                Indirim ind = new Indirim();
                ind.Tag = "I";
                ind.tutar = Convert.ToDecimal(gridColumn4.SummaryText);
                ind.ShowDialog();


                string neden = "";
                if (Departman.Kodlar_YazSipNedSor && ind.cikisyapti == false)
                {
                    Klavye2 klv = new Klavye2();
                    klv.ShowDialog();
                    neden = klv.yazi == null ? "" : klv.yazi;
                }


                decimal tutar = 0, doviztutar = 0, oran = 0;
                if (ind.indTipi == "T")
                {
                    
                        tutar = ind.indSayi;
                        doviztutar = tutar / Param.Doviz_Kuru;

                        decimal toplamTutar23 = Convert.ToDecimal(gridViewAlt.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                        decimal oran23 = (tutar / toplamTutar23) * 100;

                        if (oran23 > User.P_Indirim_Yuzde)
                        {
                            MessageBox.Show("Max Indirim Yuzdesini Aştınız..." + "\n" + "Max İndirim Yüzdeniz : %" + User.P_Indirim_Yuzde.ToString() + "\n" + "Şuan ki İndirim Oranı : %" + oran23.ToString("n2"));
                            return;
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
                    int fisno = Convert.ToInt32(this.Tag);
                    Fis_Islem.Manuel_Indirim(fisno, ind.indTipi, tutar, doviztutar, oran, Split, neden: neden);

                    decimal toplamTutar = Convert.ToDecimal(gridViewAlt.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                    if (oran > 0)
                    {
                        string aciklama = "İNDİRİM UYGULANDI . Fisno:" + fisno + " Masano:" + altmasano + " İNDİRİM ORANI : " + oran + " İNDİRİMSİZ TOPLAM TUTAR : " + toplamTutar;
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Indirim_Uygula, Log.Log_Islem.Kaydet, aciklama, fisno.ToString(), "", neden: neden);
                    }
                    else if (tutar > 0)
                    {
                        string aciklama = "İNDİRİM UYGULANDI . Fisno:" + fisno + " Masano:" + altmasano + " İNDİRİM TUTARI : " + tutar + " İNDİRİMSİZ TOPLAM TUTAR : " + toplamTutar;
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Indirim_Uygula, Log.Log_Islem.Kaydet, aciklama, fisno.ToString(), "", neden: neden);
                    }

                    //gridyenile();
                    //Bakiye_Kontrol();
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnIndirim_Click", "", ex);
            }
            finally
            {
                altmasayenile();
            }
        }
    }
}
