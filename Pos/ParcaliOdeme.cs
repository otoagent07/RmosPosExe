using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos
{
    public partial class ParcaliOdeme : Form
    {
        public int Split = 0;
        public static string MyClass = "ParcaliOdeme";
        public string fisno = "0";
        public string anamasano = "";
        public string altmasano = "";
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
                string query = "select Masa_Id,Masa_No,Masa_Ad,Masa_Durum,Masa_Parcali from Pos_Masa where Masa_No like '" + anamasano + "[_]%' and Masa_Durum='0'";
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
        }
        private void ParcaliOdeme_Load(object sender, EventArgs e)
        {
            ekrandakiButtonlariListele();
            anaListele();
            lookKapatmaYukle();



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

        public void lookKapatmaYukle()
        {
            try
            {
                DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");

                if (dt == null || dt.Rows.Count < 1)
                {
                    dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43");
                }

                look_Kapatma.Properties.DataSource = dt;

                look_Kapatma.Properties.DisplayMember = "Pkod_Ad";
                look_Kapatma.Properties.ValueMember = "Pkod_Kod";
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
                RHMesaj.MyMessageError(MyClass, "btnAltMasa1_Click", "",ex);
            }
        }

        public void altmasayenile()
        {
            try
            {
                string query = @"select Cst_Recete_Satis.*,Cst_Recete.Rec_Ad from Cst_Recete_Satis 
left join Cst_Recete on Rec_Genelkod=Rsat_Recete
where Rsat_Durum='A' and Rsat_Masa='" + altmasano + "'";
                DataTable dataTable = dbtools.SelectTableR(query);
                gridControlAlt.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "altmasayenile", "",ex);
            }
        }

        private void btnAnadanAlta_Click(object sender, EventArgs e)
        {
            try
            {
                /*Rsat_Id Rsat_Fisno Rsat_Masa Rsat_Tarih Rsat_Acilis Rsat_Ba Kasiyer Rsat_Miktar Rsat_Emiktar Rec_Ad Rsat_Tutar Rsat_Doviz MasaKonumAdi Rsat_UrunTahsilat Rsat_Recete Rsat_UrunBazliHspAdet*/


                if (altmasano=="")
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
            }

        }


        public void aktarim(DataRow neredenRow,string neredenMasa,string nereyeMasa,int fisno)
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
                    var nesne2 = db.Cst_Recete_Satis.Where(x => x.Rsat_Masa == nereyeMasa & x.Rsat_Durum == "A" & x.Rsat_Recete== recete).FirstOrDefault();
                    if (nesne2==null)
                    {
                        //alt ürün ekleme
                        nesne.Rsat_Masa = nereyeMasa;
                        nesne.Rsat_Fisno = fisno;
                        nesne.Rsat_Miktar = aktarimMiktar;
                        nesne.Rsat_Tutar = aktarimRsattutar;
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
                RHMesaj.MyMessageError(MyClass,"","",ex);
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


                string neredenMasa = altmasano ;
                string nereyeMasa = anamasano;

                aktarim(neredenRow, neredenMasa, nereyeMasa,Convert.ToInt32(fisno));


            }
            catch (Exception ex)
            {

                RHMesaj.MyMessageError(MyClass, "btnAlttanAnaya_Click", "", ex);
            }
            finally
            {
                anaListele();
                altmasayenile();
            }
        }
    }
}
