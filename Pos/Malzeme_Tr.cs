using Pos.Class;
using System;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Malzeme_Tr : DevExpress.XtraEditors.XtraForm
    {
        public int kaynakFisno;

        public Malzeme_Tr()
        {
            InitializeComponent();
        }

        private void Malzeme_Tr_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            gridyenile_Konumlar();
            gridyenile_Masa();
            gridyenile_KaynakUrun();
            txt_MasaOzel.Text = dbtools.DegerGetir("Select ISNULL(Masa_Ozel,'') as Masa_Ozel From Pos_Masa where Masa_No = '" + txt_Masano.Text + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridyenile_KaynakUrun()
        {
            gridColumn3.FieldName = "Rsat_Id";
            gridColumn4.FieldName = "Rec_Ad";
            gridColumn5.FieldName = "Rsat_Miktar";
            gridColumn6.FieldName = "Rsat_Tutar";
            gridColumn14.FieldName = "Rsat_Recete";
            gridColumn15.FieldName = "Rsat_SiparisPr";

            string query = "select Rsat_Id,Rsat_Recete,Rec_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_SiparisPr,Rsat_Emiktar "
                                                          + "  from Cst_Recete_Satis "
                                                          + "      left join Cst_Recete on Rec_Genelkod = Rsat_Recete "
                                                          + "  where Rsat_Fisno = '" + kaynakFisno + "' and Rsat_Ba = 'B' ";

            //string query = "select Rsat_Id,Rsat_Recete,Rec_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_SiparisPr,Rsat_Emiktar "
            //                                              + "  from Cst_Recete_Satis "
            //                                              + "      left join Cst_Recete on Rec_Genelkod = Rsat_Recete "
            //                                              + "  where Rsat_Fisno = '" + kaynakFisno + "' and Rsat_Ba = 'B' and Rsat_Masa = '" + txt_Masano.Text + "'";

            DataTable dt = dbtools.SelectTable(query);

            gridControl3.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                // Masa Kapat
                dbtools.execcmd("update Pos_Masa set Masa_Durum = '0' where Masa_no = '" + txt_Masano.Text + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                dbtools.execcmd("delete from cst_recete_satis where Rsat_Fisno = '" + kaynakFisno + "' and Rsat_Ba = 'A'");
            }
        }

        private void gridyenile_KaynakUrun_Yukari(int Fisno, string Masa)
        {
            gridColumn7.FieldName = "Rsat_Id";
            gridColumn10.FieldName = "Rec_Ad";
            gridColumn11.FieldName = "Rsat_Miktar";
            gridColumn12.FieldName = "Rsat_Tutar";
            gridColumn19.FieldName = "Rsat_Recete";
            gridColumn18.FieldName = "Rsat_Masa";

            DataTable dt = dbtools.SelectTable("select Rsat_Id,Rsat_Recete,Rec_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_Masa,Rsat_Emiktar"
                                                          + "  from Cst_Recete_Satis "
                                                          + "      left join Cst_Recete on Rec_Genelkod = Rsat_Recete "
                                                          + "  where Rsat_Fisno = '" + Fisno + "' and Rsat_Ba = 'B' and Rsat_Masa = '" + Masa + "'");

            gridControl4.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                // Masa Kapat
                dbtools.execcmd("update Pos_Masa set Masa_Durum = '0' where Masa_no = '" + Masa + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                dbtools.execcmd("delete from cst_recete_satis where Rsat_Fisno = '" + Fisno + "' and Rsat_Ba = 'A'");
            }
        }

        private void gridyenile_Masa()
        {
            string filtre = String.Empty;

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Kod")) != "")
            {
                filtre = " and Masa_Konum = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Kod")) + "' ";
            }

            gridControl2.DataSource = dbtools.SelectTable("select Masa_No, Masa_Ad, Masa_Durum,ISNULL(Masa_Ozel,'') as Masa_Ozel from Pos_Masa with(nolock) where Masa_Paket = 0 and Masa_Depart = '" + Departman.Dep_Kodu + "' " + filtre + "  and Masa_No not like '%[_]%' order by Masa_No");
        }

        private void gridyenile_Konumlar()
        {
            DataTable dt = dbtools.SelectTable("SELECT Pkod_Konumkod, Pkod_Ad FROM Pos_Kodlar with(nolock)  WHERE Pkod_Sinif = '14' AND Pkod_Kod = '" + Departman.Dep_Kodu + "' order by Pkod_Konumkod ");

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Kod", typeof(string));
            dt2.Columns.Add("Ad", typeof(string));
            dt2.Rows.Add("", "Tüm Masalar");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt2.Rows.Add(dt.Rows[i]["Pkod_Konumkod"], dt.Rows[i]["Pkod_Ad"]);
            }
            gridControl1.DataSource = dt2;
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            gridyenile_Masa();
        }

        private void gridView2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            gridyenile_HedefUrun();
        }

        private void gridyenile_HedefUrun()
        {
            gridColumn7.FieldName = "Rsat_Id";
            gridColumn10.FieldName = "Rec_Ad";
            gridColumn11.FieldName = "Rsat_Miktar";
            gridColumn12.FieldName = "Rsat_Tutar";

            gridControl4.DataSource = dbtools.SelectTable("select Rsat_Id,Rec_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_Recete,Rsat_Masa,Rsat_Emiktar "
                                                          + "  from Cst_Recete_Satis "
                                                          + "      left join Cst_Recete on Rec_Genelkod = Rsat_Recete "
                                                          + "  where Rsat_Ba = 'B' and Rsat_Masa = '" + gridView2.GetFocusedRowCellValue("Masa_No").ToString() + "' and Rsat_Durum='A' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");
        }

        private void gridyenile_HedefUrun_Yukari(string Masa)
        {
            gridColumn7.FieldName = "Rsat_Id";
            gridColumn10.FieldName = "Rec_Ad";
            gridColumn11.FieldName = "Rsat_Miktar";
            gridColumn12.FieldName = "Rsat_Tutar";

            gridControl3.DataSource = dbtools.SelectTable("select Rsat_Id,Rec_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_Recete,Rsat_Masa,Rsat_Emiktar"
                                                          + "  from Cst_Recete_Satis "
                                                          + "      left join Cst_Recete on Rec_Genelkod = Rsat_Recete "
                                                          + "  where Rsat_Ba = 'B' and Rsat_Masa = '" + Masa + "' and Rsat_Durum='A' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");
        }
        int Fisno = 0;

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void Asagi()
        {
            for (int i = 0; i < gridView3.GetSelectedRows().Length; i++)
            {
                int index = gridView3.GetSelectedRows()[i];

                int hedefFisno = 0;

                if (Convert.ToString(dbtools.DegerGetir("select Masa_Durum from Pos_Masa where Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'")) != "0")
                {
                    try
                    {
                        hedefFisno = Convert.ToInt32(dbtools.DegerGetir("select top 1 Rsat_Fisno from Cst_Recete_Satis where Rsat_Masa = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "' and (Rsat_Durum = 'A' and Rsat_Ba = 'B')"));
                    }
                    catch (Exception ex)
                    {
                        hedefFisno = Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));
                    }

                }
                else
                {
                    hedefFisno = Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));
                }


                //string ozelMasaAd = dbtools.DegerGetir("select Masa_Ozel from Pos_Masa where Masa_No = (select  top 1 Rsat_Masa from Cst_Recete_Satis where Rsat_Fisno='" + hedefFisno + "')");
                string ozelMasaAd = dbtools.DegerGetir("select  top 1 Rsat_OzelMasaAdi from Cst_Recete_Satis where Rsat_Fisno='" + hedefFisno + "'");
                string masaNo = dbtools.DegerGetir("select  top 1 Rsat_Masa from Cst_Recete_Satis where Rsat_Fisno='" + hedefFisno + "'");


                if (Convert.ToDecimal(gridView3.GetRowCellValue(index, "Rsat_Miktar")) > 1)
                {
                    Klavye1 klv = new Klavye1();
                    klv.Tag = "MALZEMETR";
                    klv.UrunAdi = Convert.ToString(gridView3.GetRowCellValue(index, "Rec_Ad"));
                    klv.txt_Sayi.Text = Convert.ToDecimal(gridView3.GetRowCellValue(index, "Rsat_Miktar")).ToString();
                    klv.ShowDialog();
                    decimal deger = klv.sayi;

                    if (klv.Cikis)
                    {
                        return;
                    }

                    if (deger <= 0)
                    {
                        return;
                    }

                    decimal toplamMiktar = Convert.ToDecimal(gridView3.GetRowCellValue(index, "Rsat_Miktar"));
                    if (deger > toplamMiktar)
                    {
                        MessageBox.Show(res_man.GetString("Hatalı giriş..."));
                        return;
                    }


                    Fis_Islem.Satir_Sil(Convert.ToInt32(gridView3.GetRowCellValue(index, "Rsat_Id")), deger);



                    decimal recFiyat = Convert.ToDecimal(gridView3.GetRowCellValue(index, "Rsat_Tutar").ToString());

                    recFiyat = (recFiyat / toplamMiktar);





                    Satis s = new Satis();
                    s.Tag = "M";
                    s.malzemeTr = true;
                    s.bartxt_FisNo.EditValue = hedefFisno;
                    s.Miktar = deger;
                    s.eMiktar = Convert.ToString(gridView3.GetFocusedRowCellValue("Rsat_Emiktar"));
                    s.Masa_No = Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No"));
                    s.Rsat_SiparisPr = true;// Convert.ToBoolean(gridView3.GetFocusedRowCellValue("Rsat_SiparisPr"));
                    s.Rsat_AbuyerPr = true;

                    s.Urun_Sat(Convert.ToString(gridView3.GetRowCellValue(index, "Rsat_Recete")), false,recFiyat);

                    string rsat_id = Convert.ToInt32(gridView3.GetRowCellValue(index, "Rsat_Id")).ToString();

                    if (deger == Convert.ToDecimal(gridView3.GetRowCellValue(index, "Rsat_Miktar")))
                    {
                        dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + rsat_id + "'");
                    }

                    string hapyTutar = dbtools.DegerGetir("select top 1 (Rsat_Tutar*Rsat_Indoran/100) as Rsat_Tutar from  Cst_Recete_Satis where Rsat_Fisno = '" + kaynakFisno + "'  and Rsat_Ba = 'B' and Rsat_Indkodu='HAPPYHOUR' ");

                    if (hapyTutar != "")
                    {
                        hapyTutar = hapyTutar.Replace(",", ".");

                        string query = "update Cst_Recete_Satis set Rsat_Tutar = " + hapyTutar + "  where Rsat_Fisno = '" + kaynakFisno + "'  and Rsat_Ba = 'A' and Rsat_Indkodu='HAPPYHOUR'";

                        dbtools.execcmd(query);
                    }

                    // özel masa kayboluyordu onu düzelttim 
                    dbtools.execcmd("update Pos_Masa set Masa_Ozel='" + ozelMasaAd + "' where Masa_No='" + masaNo + "' and Masa_No<>'" + ozelMasaAd + "'");


                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Malz_Transfer, Log.Log_Islem.Duzelt, Convert.ToString(gridView3.GetRowCellValue(index, "Rec_Ad")) + " ürünü " + deger.ToString() + " miktarı " + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + " NL masaya transfer oldu." + "Eski Masa :" + txt_Masano.Text, hedefFisno.ToString(), "");

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Malz_Transfer, Log.Log_Islem.Duzelt, Convert.ToString(gridView3.GetRowCellValue(index, "Rec_Ad")) + " ürünü " + deger.ToString() + " miktarı " + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + " NL masaya transfer oldu." + "Eski Masa :" + txt_Masano.Text, Main.masa_takip.bartxt_FisNo.EditValue.ToString(), "");



                }
                else
                {
                    DataTable dtInd = dbtools.SelectTable("select Rsat_Fisno,Rsat_Indsatirid,Rsat_Indsatirid2 from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView3.GetRowCellValue(index, "Rsat_Id")) + "' ");


                    if (dtInd.Rows.Count > 0)
                    {
                        //İndirimlerin Silinmesi
                        //dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid"]) + "'");
                        //dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid2"]) + "'");

                        //İndirimlerin güncellenmesi Rambo
                        dbtools.execcmd("update Cst_Recete_Satis set Rsat_Fisno= '" + hedefFisno + "' where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid"]) + "'");
                        dbtools.execcmd("update Cst_Recete_Satis set Rsat_Fisno= '" + hedefFisno + "' where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid2"]) + "'");
                    }

                    string query = "select Masa_Durum from Pos_Masa where Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'";
                    string deger = dbtools.DegerGetir(query);

                    if (deger != "0" || deger == "0") // || deger == "0" sonradan eklendi
                    {

                        Fisno = hedefFisno;

                        query = "update Cst_Recete_Satis set Rsat_Masa = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "',Rsat_Fisno = '" + hedefFisno + "' where Rsat_Id = '" + Convert.ToInt32(gridView3.GetRowCellValue(index, "Rsat_Id")) + "'";
                        dbtools.execcmd(query);
                    }
                    else
                    {
                        Satis s = new Satis();
                        s.Tag = "M";
                        s.malzemeTr = true;
                        s.bartxt_FisNo.EditValue = hedefFisno;
                        Fisno = hedefFisno;
                        s.Miktar = 1;
                        s.eMiktar = Convert.ToString(gridView3.GetFocusedRowCellValue("Rsat_Emiktar"));
                        s.Masa_No = Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No"));
                        s.Rsat_SiparisPr = true;// Convert.ToBoolean(gridView3.GetFocusedRowCellValue("Rsat_SiparisPr"));
                        s.Rsat_AbuyerPr = true;

                        decimal recFiyat = Convert.ToDecimal(gridView3.GetRowCellValue(index, "Rsat_Tutar").ToString());

                        s.Urun_Sat(Convert.ToString(gridView3.GetRowCellValue(index, "Rsat_Recete")), false,recFiyat);

                        dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView3.GetRowCellValue(index, "Rsat_Id")).ToString() + "'");
                    }



                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Malz_Transfer, Log.Log_Islem.Duzelt, Convert.ToString(gridView3.GetRowCellValue(index, "Rec_Ad")) + " ürünü 1 miktarı " + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + " NL masaya transfer oldu." + "Eski Masa :" + txt_Masano.Text, hedefFisno.ToString(), "");


                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Malz_Transfer, Log.Log_Islem.Duzelt, Convert.ToString(gridView3.GetRowCellValue(index, "Rec_Ad")) + " ürünü 1 miktarı " + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + " NL masaya transfer oldu." + "Eski Masa :" + txt_Masano.Text, Main.masa_takip.bartxt_FisNo.EditValue.ToString(), "");

                }



                dbtools.execcmd("update Pos_Masa set Masa_Durum = 1 where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "'");
            }

            gridyenile_KaynakUrun();
            gridyenile_HedefUrun();


        }

        private void Yukari()
        {
            for (int i = 0; i < gridView4.GetSelectedRows().Length; i++)
            {
                int index = gridView4.GetSelectedRows()[i];

                if (Convert.ToDecimal(gridView4.GetRowCellValue(index, "Rsat_Miktar")) > 1)
                {
                    Klavye1 klv = new Klavye1();
                    klv.Tag = "MALZEMETR";
                    klv.txt_Sayi.Text = Convert.ToDecimal(gridView4.GetRowCellValue(index, "Rsat_Miktar")).ToString();
                    klv.UrunAdi = Convert.ToString(gridView4.GetRowCellValue(index, "Rec_Ad"));
                    klv.ShowDialog();
                    decimal deger = klv.sayi;

                    if (klv.Cikis)
                    {
                        return;
                    }

                    if (deger <= 0)
                    {
                        return;
                    }

                    decimal toplamMiktar = Convert.ToDecimal(gridView4.GetRowCellValue(index, "Rsat_Miktar"));
                    if (deger > toplamMiktar)
                    {
                        MessageBox.Show(res_man.GetString("Hatalı giriş..."));
                        return;
                    }

                    Fis_Islem.Satir_Sil(Convert.ToInt32(gridView4.GetRowCellValue(index, "Rsat_Id")), deger);


                    decimal recFiyat = Convert.ToDecimal(gridView4.GetRowCellValue(index, "Rsat_Tutar").ToString());

                    recFiyat = (recFiyat / toplamMiktar);

                    int hedefFisno = 0;

                    if (Convert.ToString(dbtools.DegerGetir("select Masa_Durum from Pos_Masa where Masa_No = '" + Convert.ToString(txt_Masano.Text) + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'")) != "0")
                    {
                        hedefFisno = Convert.ToInt32(dbtools.DegerGetir("select top 1 Rsat_Fisno from Cst_Recete_Satis where Rsat_Masa = '" + txt_Masano.Text.ToString() + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "' and (Rsat_Durum = 'A' and Rsat_Ba = 'B')"));
                    }
                    else
                    {
                        hedefFisno = Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));
                    }


                    //string ozelMasaAd = dbtools.DegerGetir("select Masa_Ozel from Pos_Masa where Masa_No = (select  top 1 Rsat_Masa from Cst_Recete_Satis where Rsat_Fisno='" + hedefFisno + "')");
                    string ozelMasaAd = dbtools.DegerGetir("select  top 1 Rsat_OzelMasaAdi from Cst_Recete_Satis where Rsat_Fisno='" + hedefFisno + "'");
                    string masaNo = dbtools.DegerGetir("select  top 1 Rsat_Masa from Cst_Recete_Satis where Rsat_Fisno='" + hedefFisno + "'");

                    Satis s = new Satis();
                    s.Tag = "M";
                    s.malzemeTr = true;
                    s.bartxt_FisNo.EditValue = hedefFisno;
                    kaynakFisno = hedefFisno;
                    s.Miktar = deger;
                    s.eMiktar = Convert.ToString(gridView4.GetRowCellValue(index, "Rsat_Emiktar"));
                    s.Masa_No = txt_Masano.Text.ToString();
                    s.Rsat_SiparisPr = true;// Convert.ToBoolean(gridView4.GetFocusedRowCellValue("Rsat_SiparisPr"));
                    s.Rsat_AbuyerPr = true;
                    s.Urun_Sat(Convert.ToString(gridView4.GetRowCellValue(index, "Rsat_Recete")), false,recFiyat);


                    if (deger == Convert.ToDecimal(gridView4.GetRowCellValue(index, "Rsat_Miktar")))
                    {
                        dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView4.GetRowCellValue(index, "Rsat_Id")).ToString() + "'");
                    }
                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Malz_Transfer, Log.Log_Islem.Duzelt, Convert.ToString(gridView4.GetRowCellValue(index, "Rec_Ad")) + " ürünü " + deger.ToString() + " miktarı " + txt_Masano.Text.ToString() + " NL masaya transfer oldu." + "Eski Masa :" + txt_Masano.Text, hedefFisno.ToString(), "");

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Malz_Transfer, Log.Log_Islem.Duzelt, Convert.ToString(gridView4.GetRowCellValue(index, "Rec_Ad")) + " ürünü " + deger.ToString() + " miktarı " + txt_Masano.Text.ToString() + " NL masaya transfer oldu." + "Eski Masa :" + txt_Masano.Text, Main.masa_takip.bartxt_FisNo.EditValue.ToString(), "");


                    dbtools.execcmd("update Pos_Masa set Masa_Ozel='" + ozelMasaAd + "' where Masa_No='" + masaNo + "' and Masa_No<>'" + ozelMasaAd + "'");
                }
                else
                {
                    DataTable dtInd = dbtools.SelectTable("select Rsat_Fisno,Rsat_Indsatirid,Rsat_Indsatirid2 from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView4.GetRowCellValue(index, "Rsat_Id")) + "' ");
                    if (dtInd.Rows.Count > 0)
                    {
                        //İndirimlerin Silinmesi
                        dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid"]) + "'");
                        dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToString(dtInd.Rows[0]["Rsat_Indsatirid2"]) + "'");
                    }

                    int hedefFisno = 0;


                    string query = "select Masa_Durum from Pos_Masa where Masa_No = '" + Convert.ToString(txt_Masano.Text) + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'";
                    string deger = dbtools.DegerGetir(query);

                    if (deger != "0" || deger == "0") // || deger == "0" sonradan eklendi
                    {
                        try
                        {
                            hedefFisno = Convert.ToInt32(dbtools.DegerGetir("select top 1 Rsat_Fisno from Cst_Recete_Satis where Rsat_Masa = '" + txt_Masano.Text.ToString() + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "' and (Rsat_Durum = 'A' and Rsat_Ba = 'B')"));
                        }
                        catch (Exception ex)
                        {
                            hedefFisno = Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));
                        }

                        Fisno = hedefFisno;
                        dbtools.execcmd("update Cst_Recete_Satis set Rsat_Masa = '" + txt_Masano.Text.ToString() + "',Rsat_Fisno = '" + hedefFisno + "' where Rsat_Id = '" + Convert.ToInt32(gridView4.GetRowCellValue(index, "Rsat_Id")) + "'");

                        if (Departman.Kodlar_ServisPayi)
                        {
                            dbtools.execcmd("Exec Pos_ServisPayi @Fisno = '" + Fisno + "', @MasaNo ='" + Convert.ToString(txt_Masano.Text) + "'");
                        }
                    }
                    else
                    {
                        hedefFisno = Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));
                        Satis s = new Satis();
                        s.Tag = "M";
                        s.malzemeTr = true;
                        s.bartxt_FisNo.EditValue = hedefFisno;
                        kaynakFisno = hedefFisno;
                        Fisno = hedefFisno;
                        s.Miktar = 1;
                        s.eMiktar = Convert.ToString(gridView4.GetRowCellValue(index, "Rsat_Emiktar"));
                        s.Masa_No = txt_Masano.Text.ToString();
                        s.Rsat_SiparisPr = true;// Convert.ToBoolean(gridView4.GetFocusedRowCellValue("Rsat_SiparisPr"));
                        s.Rsat_AbuyerPr = true;


                        string fiyat = "0";
                        try { fiyat = gridView3.GetRowCellValue(index, "Rsat_Tutar").ToString(); } catch (Exception ex) { }
                        decimal recFiyat = Convert.ToDecimal(fiyat);

                        s.Urun_Sat(Convert.ToString(gridView4.GetRowCellValue(index, "Rsat_Recete")), false,recFiyat);


                        dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Id = '" + Convert.ToInt32(gridView4.GetRowCellValue(index, "Rsat_Id")).ToString() + "'");
                    }

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Malz_Transfer, Log.Log_Islem.Duzelt, Convert.ToString(gridView4.GetRowCellValue(index, "Rec_Ad")) + " ürünü 1 miktarı " + txt_Masano.Text.ToString() + " NL masaya transfer oldu." + "Eski Masa :" + txt_Masano.Text, hedefFisno.ToString(), "");

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Malz_Transfer, Log.Log_Islem.Duzelt, Convert.ToString(gridView4.GetRowCellValue(index, "Rec_Ad")) + " ürünü 1 miktarı " + txt_Masano.Text.ToString() + " NL masaya transfer oldu." + "Eski Masa :" + txt_Masano.Text, Main.masa_takip.bartxt_FisNo.EditValue.ToString(), "");


                    kaynakFisno = hedefFisno;
                }

                dbtools.execcmd("update Pos_Masa set Masa_Durum = 1 where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Convert.ToString(txt_Masano.Text) + "'");


            }




        }

        public static string MyClass = "Malzeme_Tr";
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string hedefMasaNo2 = Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No"));

                if (StatikSinif.masaMusaitmi(hedefMasaNo2)==false)
                {
                    return;
                }

                Asagi();


                gridyenile_KaynakUrun();
                gridyenile_HedefUrun();


                if (Departman.Kodlar_ServisPayi)
                {
                    if (Convert.ToString(dbtools.DegerGetir("select Masa_Durum from Pos_Masa where Masa_No = '" + txt_Masano.Text + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'")) != "0")
                    {

                        string query = "select top 1 ISNULL(Rsat_Fisno,0) from Cst_Recete_Satis where Rsat_Masa = '" + txt_Masano.Text + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "' and (Rsat_Durum = 'A' and Rsat_Ba = 'B')";
                        int hedefFisno = Convert.ToInt32(dbtools.DegerGetir(query));

                        if (hedefFisno != 0)
                        {
                            dbtools.execcmd("Exec Pos_ServisPayi @Fisno = '" + hedefFisno + "', @MasaNo ='" + txt_Masano.Text + "'");
                        }
                    }
                }

                if (Departman.Kodlar_ServisPayi)
                {
                    if (Convert.ToString(dbtools.DegerGetir("select Masa_Durum from Pos_Masa where Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'")) != "0")
                    {

                        string hedefMasaNo = Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No"));

                        string query = "select top 1 ISNULL(Rsat_Fisno,0) from Cst_Recete_Satis where Rsat_Masa = '" + hedefMasaNo + "' and Rsat_Durum = 'A' and Rsat_Ba = 'B' and Rsat_Fisno is not null";
                        int hedefFisno = Convert.ToInt32(dbtools.DegerGetir(query));

                        dbtools.execcmd("Exec Pos_ServisPayi @Fisno = '" + hedefFisno + "', @MasaNo ='" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "'");
                    }
                }


                gridyenile_KaynakUrun();
                gridyenile_HedefUrun();

                if (gridView3.RowCount == 0)
                {
                    dbtools.execcmd("update Pos_Masa set Masa_Ozel = '' where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Convert.ToString(txt_Masano.Text) + "'");

                    //dbtools.execcmd("update Pos_Masa set Masa_Ozel = case when ISNULL(Masa_Ozel,'') != '' then '" + txt_MasaOzel.Text + "' end where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "'");

                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_OzelMasaAdi =  '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_Ozel")) + "' where Rsat_Fisno = '" + Fisno + "' and Rsat_Departman ='" + Departman.Dep_Kodu + "'");
                }


            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "simpleButton1_Click", "", ex);
            }

        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView2.FocusedRowHandle < 0)
                {
                    MessageBox.Show(res_man.GetString("Masa Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }



                Yukari();

                gridyenile_KaynakUrun();
                gridyenile_HedefUrun();


                if (Departman.Kodlar_ServisPayi)
                {
                    if (Convert.ToString(dbtools.DegerGetir("select Masa_Durum from Pos_Masa where Masa_No = '" + txt_Masano.Text + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'")) != "0")
                    {
                        int hedefFisno = Convert.ToInt32(dbtools.DegerGetir("select top 1 ISNULL(Rsat_Fisno,0) from Cst_Recete_Satis where Rsat_Masa = '" + txt_Masano.Text + "' and Rsat_Durum = 'A' and Rsat_Ba = 'B' "));

                        if (hedefFisno != 0)
                        {
                            dbtools.execcmd("Exec Pos_ServisPayi @Fisno = '" + hedefFisno + "', @MasaNo ='" + txt_Masano.Text + "'");
                        }
                    }
                }

                if (Departman.Kodlar_ServisPayi)
                {
                    if (Convert.ToString(dbtools.DegerGetir("select Masa_Durum from Pos_Masa where Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'")) != "0")
                    {
                        string query = "select top 1 ISNULL(Rsat_Fisno,0) from Cst_Recete_Satis where Rsat_Masa = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' and Rsat_Durum = 'A' and Rsat_Ba = 'B' and Rsat_Fisno is not null";

                        string deger = dbtools.DegerGetir(query);
                        if (!deger.Equals(""))
                        {
                            int hedefFisno = Convert.ToInt32(deger);

                            dbtools.execcmd("Exec Pos_ServisPayi @Fisno = '" + hedefFisno + "', @MasaNo ='" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "'");
                        }



                    }
                }


                gridyenile_KaynakUrun();
                gridyenile_HedefUrun();

                if (gridView4.RowCount == 0)
                {
                    dbtools.execcmd("update Pos_Masa set Masa_Ozel= '' , Masa_Durum = 0 where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "'");

                    dbtools.execcmd("update Pos_Masa set Masa_Ozel = case when ISNULL(Masa_Ozel,'') != '' then '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' end where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Convert.ToString(txt_Masano.Text) + "'");

                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_OzelMasaAdi = Case when ISNULL(Rsat_OzelMasaAdi,'') != '' then '" + Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No")) + "' end where Rsat_Fisno = '" + Fisno + "' and Rsat_Departman ='" + Departman.Dep_Kodu + "'");
                }

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "simpleButton2_Click", "", ex);
            }


        }

        private void gridControl2_Click(object sender, EventArgs e)
        {
            simpleButton1.Enabled = true;
            simpleButton2.Enabled = true;
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            simpleButton1.Enabled = false;
            simpleButton2.Enabled = false;
        }

        private void Malzeme_Tr_Shown(object sender, EventArgs e)
        {
            StatikSinif.masaKilitle(txt_Masano.Text);
        }
    }
}