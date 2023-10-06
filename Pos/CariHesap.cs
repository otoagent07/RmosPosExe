using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;
using DevExpress.XtraPrinting;
using System.Data.SqlClient;
using DevExpress.XtraGrid;
using System.Drawing.Printing;
using System.Resources;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Diagnostics;
using Pos.Print;
using DevExpress.XtraReports.UI;
using System.Net.Mail;
using System.Net;

namespace Pos
{
    public partial class CariHesap : DevExpress.XtraEditors.XtraForm
    {
        public string Tel = "";
        public string CariKod = "";
        public bool AcikAdres = false;
        public bool BilgiCari = false;
        public CariHesap()
        {
            InitializeComponent();
        }

        TextEdit txt_Active;

        private void Cari_Load(object sender, EventArgs e)
        {
            dateEditRap3BasTar.EditValue = Param.Tarih;
            dateEditRap3BitTar.EditValue = Param.Tarih;

            dateEditCariBas.EditValue = Param.Tarih;
            dateEditCariBit.EditValue = Param.Tarih;

            date_CariHes.DateTime = Param.Tarih;


            look_CariHes_Odeme.Properties.DataSource = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad,Pkod_Ozelkod from Pos_Kodlar where Pkod_Sinif = '11' and Pkod_Ozelkod <> '5' order by Pkod_Kod");
            look_CariHes_Odeme.Properties.DisplayMember = "Pkod_Ad";
            look_CariHes_Odeme.Properties.ValueMember = "Pkod_Kod";

            look_CariMuh.Properties.DataSource = Departman.MuhasebeKod_Getir(new DateTime(Param.Tarih.Year, 1, 1), new DateTime(Param.Tarih.Year, 12, 31), "1");
            look_CariMuh.Properties.DisplayMember = "Plan_Ad";
            look_CariMuh.Properties.ValueMember = "Plan_Kod";

            DataTable dtCariTip = new DataTable();
            dtCariTip.Columns.Add("kod", typeof(string));
            dtCariTip.Columns.Add("ad", typeof(string));

            dtCariTip.Rows.Add("C", "Cari");
            dtCariTip.Rows.Add("O", "Odenmez");
            dtCariTip.Rows.Add("P", "Ikram");
            dtCariTip.Rows.Add("Y", "Yemek Sepeti");
            dtCariTip.Rows.Add("G", "Getir Yemek");

            look_Cari_Tip.Properties.DataSource = dtCariTip;
            look_Cari_Tip.Properties.DisplayMember = "ad";
            look_Cari_Tip.Properties.ValueMember = "kod";
            look_Cari_Tip.EditValue = "C";

            look_Cari_TipBakiye.Properties.DataSource = dtCariTip;
            look_Cari_TipBakiye.Properties.DisplayMember = "ad";
            look_Cari_TipBakiye.Properties.ValueMember = "kod";
            look_Cari_TipBakiye.EditValue = "C";

            look_Cari_Il.Properties.DataSource = dbtools.SelectTable("select * from Pos_Adres where Adres_Sinif = '24' order by Adres_Ad");
            look_Cari_Il.Properties.DisplayMember = "Adres_Ad";
            look_Cari_Il.Properties.ValueMember = "Adres_Kod";


            rap3Listele(true);

            if (Tel != "")
            {
                xtraTabControl1.SelectedTabPage = tab_Cari_Tanim;
                txt_Cari_Telefon.Text = Tel;
                //gridyenile_Cari();
            }
            if (!string.IsNullOrEmpty(CariKod))
            {
                xtraTabControl1.SelectedTabPage = tab_Cari_Tanim;
            }

            if (BilgiCari == true)
            {
                if (!string.IsNullOrEmpty(CariKod))
                {
                    xtraTabControl1.SelectedTabPage = tab_Cari_Tanim;
                    gridyenile_Cari();
                }
            }

            if (User.P_Kod.ToUpper() == "RMOS" || User.P_Kod.ToUpper() == "RMOSXYZ")
            {
                groupControl3.Visible = true;
                simpleButton17.Enabled = true;
            }

            dateTarih1.DateTime = Param.Tarih;
            dateTarih2.DateTime = Param.Tarih;

            cari2_Tarih1.DateTime = Param.Tarih;
            cari2_Tarih2.DateTime = Param.Tarih;

            if (AcikAdres)
            {
                txt_Cari_Adres1.Select();
                txt_Cari_Adres1.Focus();
                txt_Cari_Kod_Click(txt_Cari_Adres1, null);
            }
            else
            {
                txt_Cari_Kod.Focus();
            }
        }

        #region Cari Tanımları
        private void btn_Cari_Kaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
            gridyenile_Cari();
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        private void Kaydet()
        {
            // Cari_indirimOran txtIndOran
            string indirimDec = txtIndOran.EditValue.ToString().Replace(",",".");
            if (Param.Param_CariAdSoyad == false)
            {
                if (txt_Cari_Ad.Text.Length > 0 && txt_Cari_Soyad.Text.Length > 0)
                {
                    if (txt_Cari_Kod.Text.Length == 0)
                    {
                        DialogResult c = MessageBox.Show(res_man.GetString("Cari Kodu Boş Geçilemez...") + "\n" + res_man.GetString("Sistem Yeni Kod Versin mi?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (c == System.Windows.Forms.DialogResult.No)
                        {
                            MessageBox.Show(res_man.GetString("Cari Kodu Giriniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    DataTable dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Kod = '" + txt_Cari_Kod.EditValue + "' ");
                    if (dt.Rows.Count < 1)
                    {
                        int id = Convert.ToInt32(dbtools.DegerGetir("INSERT INTO dbo.Pos_Cari (Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Adres1,Cari_Adres2,Cari_Adres3,Cari_Funvan,Cari_Funvan2, "
                            + " Cari_Fadres1,Cari_Fadres2,Cari_Vergidarie,Cari_Vergino,Cari_Mail,Cari_Kart,Cari_Tel2,Cari_Email,Cari_Tip,Cari_Limit,Cari_LimitTutar,Cari_Il,cari_Ilce,Cari_Mahalle,Cari_MuhasebeKodu,Cari_Aktif,Cari_DogumTar,Cari_indirimOran) VALUES ( '" + txt_Cari_Kod.EditValue + "','" + txt_Cari_Ad.EditValue + "','" + txt_Cari_Soyad.EditValue + "','" + txt_Cari_Telefon.EditValue + "', "
                            + " '" + txt_Cari_Adres1.EditValue + "','" + txt_Cari_Adres2.EditValue + "','" + txt_Cari_Adres3.EditValue + "','" + txt_Cari_F_Unvan.EditValue + "','" + txt_Cari_F_Unvan2.EditValue + "','" + txt_Cari_F_Adres1.EditValue + "','" + txt_Cari_F_Adres2.EditValue + "','" + txt_Cari_F_Vergidaire.EditValue + "', "
                            + " '" + txt_Cari_F_Vergino.EditValue + "','" + txt_Cari_F_Mail.EditValue + "', '" + txt_Cari_Kart_No.EditValue + "','" + txt_Cari_Telefon2.EditValue + "','" + txt_Cari_Email.EditValue + "','" + Convert.ToString(look_Cari_Tip.EditValue) + "', "
                            + " '" + Convert.ToBoolean(chk_Cari_limit.Checked) + "','" + spn_Cari_Limit.Value.ToString().Replace(",", ".") + "','" + Convert.ToString(look_Cari_Il.EditValue) + "','" + Convert.ToString(look_Cari_Ilce.EditValue) + "','" + Convert.ToString(look_Cari_Mahalle.EditValue) + "', "
                            + " '" + look_CariMuh.EditValue + "','" + Cari_Aktif.Checked + "','" + Cari_DogumTar.EditValue + "','"+ indirimDec+"')  select SCOPE_IDENTITY()"));

                        if (txt_Cari_Kod.Text.Length == 0)
                        {
                            dbtools.execcmd("update Pos_Cari set Cari_Kod = '" + id + "' where Cari_Id = '" + id + "'");
                        }


                        //CariKod = id.ToString(); // ONUR İÇİN BURADAKİ KOD İF'İN ALTINA TAŞINDI
                        //txt_Cari_Kod.Text = CariKod;// ONUR İÇİN BURADAKİ KOD İF'İN ALTINA TAŞINDI

                        if (Param.Param_OdenmezAc)
                        {
                            if (Convert.ToString(look_Cari_Tip.EditValue) != "C")
                            {
                                string query = @"INSERT INTO [dbo].[Cst_Odenmez]
                                                ([Ode_Kod]
                                                ,[Ode_Ad]
                                                 ,Ode_Doviz,Ode_Tl,  Ode_Indirim    )                                                      
                                               VALUES  (
                                                '" + (txt_Cari_Kod.Text.Length == 0 ? id.ToString() : txt_Cari_Kod.Text) + @"','" + txt_Cari_Ad.Text + " " + txt_Cari_Soyad.Text + "',0,0,0 )";
                                dbtools.execcmd(query);


                            }
                        }

                        CariKod = id.ToString();     // ONUR İÇİN KOD DEĞİŞTİRİLDİ NORMALDE BEN BURAYA DOKUNMAM AMA İSTEK GELDİ
                        txt_Cari_Kod.Text = CariKod; // ONUR İÇİN KOD DEĞİŞTİRİLDİ NORMALDE BEN BURAYA DOKUNMAM AMA İSTEK GELDİ

                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Kaydet, txt_Cari_Kod.EditValue + " Kod ile Cari Kaydedildi.", String.Empty, String.Empty);
                    }
                    else
                    {
                        dbtools.execcmd("update dbo.Pos_Cari set Cari_Kod='" + txt_Cari_Kod.EditValue + "', Cari_Ad='" + txt_Cari_Ad.EditValue + "', Cari_Soyad='" + txt_Cari_Soyad.EditValue + "', Cari_Tel='" + txt_Cari_Telefon.EditValue + "', "
                        + " Cari_Adres1='" + txt_Cari_Adres1.EditValue + "', Cari_Adres2='" + txt_Cari_Adres2.EditValue + "', Cari_Adres3='" + txt_Cari_Adres3.EditValue + "',Cari_Funvan='" + txt_Cari_F_Unvan.EditValue + "', Cari_Fadres1='" + txt_Cari_F_Adres1.EditValue + "', "
                        + " Cari_Fadres2='" + txt_Cari_F_Adres2.EditValue + "', Cari_Vergidarie='" + txt_Cari_F_Vergidaire.EditValue + "', Cari_Vergino='" + txt_Cari_F_Vergino.EditValue + "',Cari_Mail='" + txt_Cari_F_Mail.EditValue + "', Cari_Kart = '" + txt_Cari_Kart_No.EditValue + "', "
                        + " Cari_Tel2 = '" + txt_Cari_Telefon2.EditValue + "',Cari_Email = '" + txt_Cari_Email.EditValue + "',Cari_Tip = '" + Convert.ToString(look_Cari_Tip.EditValue) + "', "
                        + " Cari_Limit = '" + Convert.ToBoolean(chk_Cari_limit.Checked) + "',Cari_LimitTutar = '" + spn_Cari_Limit.Value.ToString().Replace(",", ".") + "',Cari_Il = '" + Convert.ToString(look_Cari_Il.EditValue) + "', "
                        + " Cari_Ilce = '" + Convert.ToString(look_Cari_Ilce.EditValue) + "',Cari_Mahalle = '" + Convert.ToString(look_Cari_Mahalle.EditValue) + "', "
                        + " Cari_MuhasebeKodu = '" + look_CariMuh.EditValue + "', Cari_Aktif = '" + Cari_Aktif.Checked + "',Cari_Funvan2='" + txt_Cari_F_Unvan2.EditValue + "', "
                        + " Cari_DogumTar = '" + Cari_DogumTar.DateTime.Date + "', Cari_indirimOran = '" + indirimDec + "' where  Cari_Kod = '" + txt_Cari_Kod.EditValue + "' ");

                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Duzelt, txt_Cari_Kod.EditValue + " Kod ile Cari Duzeltildi.", String.Empty, String.Empty);

                        CariKod = txt_Cari_Kod.Text;

                        if (Param.Param_OdenmezAc)
                        {
                            if (Convert.ToString(look_Cari_Tip.EditValue) != "C")
                            {
                                dbtools.execcmd(@"UPDATE [dbo].[Cst_Odenmez]
                                   SET [Ode_Kod] = '" + CariKod + @"'
                                      ,[Ode_Ad] = '" + txt_Cari_Ad.Text + " " + txt_Cari_Soyad.Text + @"'
                                        

                                 WHERE Ode_Kod = '" + CariKod + "'");

                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(res_man.GetString("Ad ve Soyad Boş Geçilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (txt_Cari_Kod.Text.Length == 0)
                {
                    DialogResult c = MessageBox.Show(res_man.GetString("Cari Kodu Boş Geçilemez...\nSistem Yeni Kod Versin mi?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (c == System.Windows.Forms.DialogResult.No)
                    {
                        MessageBox.Show(res_man.GetString("Cari Kodu Giriniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                DataTable dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Kod = '" + txt_Cari_Kod.EditValue + "' ");
                if (dt.Rows.Count < 1)
                {
                    int id = Convert.ToInt32(dbtools.DegerGetir("INSERT INTO dbo.Pos_Cari (Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Adres1,Cari_Adres2,Cari_Adres3,Cari_Funvan, "
                        + " Cari_Fadres1,Cari_Fadres2,Cari_Vergidarie,Cari_Vergino,Cari_Mail,Cari_Kart,Cari_Tel2,Cari_Email,Cari_Tip,Cari_Limit,Cari_LimitTutar,Cari_Il,cari_Ilce,Cari_Mahalle,Cari_MuhasebeKodu,Cari_Aktif,Cari_indirimOran) VALUES ( '" + txt_Cari_Kod.EditValue + "','" + txt_Cari_Ad.EditValue + "','" + txt_Cari_Soyad.EditValue + "','" + txt_Cari_Telefon.EditValue + "', "
                        + " '" + txt_Cari_Adres1.EditValue + "','" + txt_Cari_Adres2.EditValue + "','" + txt_Cari_Adres3.EditValue + "','" + txt_Cari_F_Unvan.EditValue + "','" + txt_Cari_F_Adres1.EditValue + "','" + txt_Cari_F_Adres2.EditValue + "','" + txt_Cari_F_Vergidaire.EditValue + "', "
                        + " '" + txt_Cari_F_Vergino.EditValue + "','" + txt_Cari_F_Mail.EditValue + "', '" + txt_Cari_Kart_No.EditValue + "','" + txt_Cari_Telefon2.EditValue + "','" + txt_Cari_Email.EditValue + "','" + Convert.ToString(look_Cari_Tip.EditValue) + "', "
                        + " '" + Convert.ToBoolean(chk_Cari_limit.Checked) + "','" + spn_Cari_Limit.Value.ToString().Replace(",", ".") + "','" + Convert.ToString(look_Cari_Il.EditValue) + "','" + Convert.ToString(look_Cari_Ilce.EditValue) + "','" + Convert.ToString(look_Cari_Mahalle.EditValue) + "', "
                        + " '" + look_CariMuh.EditValue + "','" + Cari_Aktif.Checked + "','"+ indirimDec+"')  select SCOPE_IDENTITY()"));

                    if (txt_Cari_Kod.Text.Length == 0)
                    {
                        dbtools.execcmd("update Pos_Cari set Cari_Kod = '" + id + "' where Cari_Id = '" + id + "'");
                    }

                    CariKod = id.ToString();
                    txt_Cari_Kod.Text = CariKod;

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Kaydet, txt_Cari_Kod.EditValue + " Kod ile Cari Kaydedildi.", String.Empty, String.Empty);
                }
                else
                {
                    dbtools.execcmd("update dbo.Pos_Cari set Cari_Kod='" + txt_Cari_Kod.EditValue + "', Cari_Ad='" + txt_Cari_Ad.EditValue + "', Cari_Soyad='" + txt_Cari_Soyad.EditValue + "', Cari_Tel='" + txt_Cari_Telefon.EditValue + "', "
                    + " Cari_Adres1='" + txt_Cari_Adres1.EditValue + "', Cari_Adres2='" + txt_Cari_Adres2.EditValue + "', Cari_Adres3='" + txt_Cari_Adres3.EditValue + "',Cari_Funvan='" + txt_Cari_F_Unvan.EditValue + "', Cari_Fadres1='" + txt_Cari_F_Adres1.EditValue + "', "
                    + " Cari_Fadres2='" + txt_Cari_F_Adres2.EditValue + "', Cari_Vergidarie='" + txt_Cari_F_Vergidaire.EditValue + "', Cari_Vergino='" + txt_Cari_F_Vergino.EditValue + "',Cari_Mail='" + txt_Cari_F_Mail.EditValue + "', Cari_Kart = '" + txt_Cari_Kart_No.EditValue + "', "
                    + " Cari_Tel2 = '" + txt_Cari_Telefon2.EditValue + "',Cari_Email = '" + txt_Cari_Email.EditValue + "',Cari_Tip = '" + Convert.ToString(look_Cari_Tip.EditValue) + "', "
                    + " Cari_Limit = '" + Convert.ToBoolean(chk_Cari_limit.Checked) + "',Cari_LimitTutar = '" + spn_Cari_Limit.Value.ToString().Replace(",", ".") + "',Cari_Il = '" + Convert.ToString(look_Cari_Il.EditValue) + "', "
                    + " Cari_Ilce = '" + Convert.ToString(look_Cari_Ilce.EditValue) + "',Cari_Mahalle = '" + Convert.ToString(look_Cari_Mahalle.EditValue) + "', "
                    + " Cari_MuhasebeKodu = '" + look_CariMuh.EditValue + "', Cari_Aktif = '" + Cari_Aktif.Checked+ "', Cari_indirimOran = '" + indirimDec + "' where  Cari_Kod = '" + txt_Cari_Kod.EditValue + "' ");

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Duzelt, txt_Cari_Kod.EditValue + " Kod ile Cari Duzeltildi.", String.Empty, String.Empty);

                    CariKod = txt_Cari_Kod.Text;
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
                DataTable dtHrk = dbtools.SelectTable("select * from Pos_Carihrk where Chrk_Cari = '" + txt_Cari_Kod.Text + "'");
                if (dtHrk.Rows.Count > 0)
                {
                    MessageBox.Show(res_man.GetString("Cari hareket Görmüş Silinemez!!"));
                    return;
                }

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
            gridColumn1.FieldName = "Cari_Tel2";
            gridColumn2.FieldName = "Cari_Email";
            gridColumn18.FieldName = "Cari_Tip";
            gridColumn32.FieldName = "Cari_Limit";
            gridColumn33.FieldName = "Cari_LimitTutar";
            gridColumn34.FieldName = "Cari_Il";
            gridColumn35.FieldName = "Cari_Ilce";
            gridColumn54.FieldName = "Cari_Mahalle";
            gridColumn55.FieldName = "Cari_MuhasebeKodu";

            string filtre = "";
            if (!string.IsNullOrEmpty(CariKod)) filtre = " Where Cari_Kod = '" + CariKod + "'";

            DataTable dt = dbtools.SelectTable(@"
        SELECT ISNULL(Cari_Id,0)            as Cari_Id
      , ISNULL(Cari_Kod, '')                as Cari_Kod
      , ISNULL(Cari_Ad, '')                 as Cari_Ad
      , ISNULL(Cari_Soyad, '')              as Cari_Soyad
      , ISNULL(Cari_Tel, '')                as Cari_Tel
      , ISNULL(Cari_Adres1, '')             as Cari_Adres1
      , ISNULL(Cari_Adres2, '')             as Cari_Adres2
      , ISNULL(Cari_Adres3, '')             as Cari_Adres3
      , ISNULL(Cari_Funvan, '')             as Cari_Funvan
      , ISNULL(Cari_Fadres1, '')            as Cari_Fadres1
      , ISNULL(Cari_Fadres2, '')            as Cari_Fadres2
      , ISNULL(Cari_Vergidarie, '')         as Cari_Vergidarie
      , ISNULL(Cari_Vergino, '')            as Cari_Vergino
      , ISNULL(Cari_Mail, '')               as Cari_Mail
      , ISNULL(Cari_Kart, '')               as Cari_Kart
      , ISNULL(Cari_Tel2, '')               as Cari_Tel2
      , ISNULL(Cari_Email, '')              as Cari_Email
      , ISNULL(Cari_Tip, 'C')               as Cari_Tip
      , ISNULL(Cari_Limit, 0)               as Cari_Limit
      , ISNULL(Cari_LimitTutar, 0)          as Cari_LimitTutar
      , ISNULL(Cari_Il, '')                 as Cari_Il
      , ISNULL(Cari_Ilce, '')               as Cari_Ilce
      , ISNULL(Cari_Mahalle, '')            as Cari_Mahalle
      , ISNULL(Cari_MuhasebeKodu, '')       as Cari_MuhasebeKodu
      , ISNULL(Cari_Aktif, 1)               as Cari_Aktif
      , ISNULL(Cari_YS_AddressId, 0)        as Cari_YS_AddressId
      , ISNULL(Cari_YS_CustomerID, '')      as Cari_YS_CustomerID
      , ISNULL(Cari_Funvan2, '')            as Cari_Funvan2
      , ISNULL(Cari_DogumTar, getdate())    as Cari_DogumTar
  FROM [dbo].[Pos_Cari] " + filtre + " order by Cari_Kod");
            grd_Cari.DataSource = dt;

            if (dt.Rows.Count == 1)
            {
                look_Cari_Tip.EditValue = Convert.ToString(dt.Rows[0]["Cari_Tip"]);
                txt_Cari_Kod.EditValue = Convert.ToString(dt.Rows[0]["Cari_Kod"]);
                txt_Cari_Ad.EditValue = Convert.ToString(dt.Rows[0]["Cari_Ad"]);
                txt_Cari_Soyad.EditValue = Convert.ToString(dt.Rows[0]["Cari_Soyad"]);
                txt_Cari_Telefon.EditValue = Convert.ToString(dt.Rows[0]["Cari_Tel"]);
                txt_Cari_Adres1.EditValue = Convert.ToString(dt.Rows[0]["Cari_Adres1"]);
                txt_Cari_Adres2.EditValue = Convert.ToString(dt.Rows[0]["Cari_Adres2"]);
                txt_Cari_Adres3.EditValue = Convert.ToString(dt.Rows[0]["Cari_Adres3"]);
                txt_Cari_F_Unvan.EditValue = Convert.ToString(dt.Rows[0]["Cari_Funvan"]);
                txt_Cari_F_Unvan2.EditValue = Convert.ToString(dt.Rows[0]["Cari_Fadres1"]);
                txt_Cari_F_Adres1.EditValue = Convert.ToString(dt.Rows[0]["Cari_Fadres2"]);
                txt_Cari_F_Adres2.EditValue = Convert.ToString(dt.Rows[0]["Cari_Vergidarie"]);
                txt_Cari_F_Vergidaire.EditValue = Convert.ToString(dt.Rows[0]["Cari_Vergidarie"]);
                txt_Cari_F_Vergino.EditValue = Convert.ToString(dt.Rows[0]["Cari_Vergino"]);
                txt_Cari_F_Mail.EditValue = Convert.ToString(dt.Rows[0]["Cari_Mail"]);
                txt_Cari_Kart_No.EditValue = Convert.ToString(dt.Rows[0]["Cari_Kart"]);
                txt_Cari_Telefon2.EditValue = Convert.ToString(dt.Rows[0]["Cari_Tel2"]);
                txt_Cari_Email.EditValue = Convert.ToString(dt.Rows[0]["Cari_Email"]);
                chk_Cari_limit.Checked = Convert.ToBoolean(dt.Rows[0]["Cari_Limit"]);
                spn_Cari_Limit.EditValue = Convert.ToString(dt.Rows[0]["Cari_LimitTutar"]);
                look_Cari_Il.EditValue = Convert.ToString(dt.Rows[0]["Cari_Il"]);
                look_Cari_Ilce.EditValue = Convert.ToString(dt.Rows[0]["Cari_Ilce"]);
                look_Cari_Mahalle.EditValue = Convert.ToString(dt.Rows[0]["Cari_Mahalle"]);
                look_CariMuh.EditValue = Convert.ToString(dt.Rows[0]["Cari_MuhasebeKodu"]);
                Cari_Aktif.Checked = Convert.ToBoolean(dt.Rows[0]["Cari_Aktif"]);
            }
            else
            {
                look_Cari_Tip.EditValue = "C";
                txt_Cari_Kod.EditValue = "";
                txt_Cari_Ad.EditValue = "";
                txt_Cari_Soyad.EditValue = "";
                txt_Cari_Telefon.EditValue = "";
                txt_Cari_Adres1.EditValue = "";
                txt_Cari_Adres2.EditValue = "";
                txt_Cari_Adres3.EditValue = "";
                txt_Cari_F_Unvan.EditValue = "";
                txt_Cari_F_Unvan2.EditValue = "";
                txt_Cari_F_Adres1.EditValue = "";
                txt_Cari_F_Adres2.EditValue = "";
                txt_Cari_F_Vergidaire.EditValue = "";
                txt_Cari_F_Vergino.EditValue = "";
                txt_Cari_F_Mail.EditValue = "";
                txt_Cari_Kart_No.EditValue = "";
                txt_Cari_Telefon2.EditValue = "";
                txt_Cari_Email.EditValue = "";
                chk_Cari_limit.Checked = false;
                spn_Cari_Limit.Value = 0;
                look_Cari_Il.EditValue = null;
                look_Cari_Ilce.EditValue = null;
                look_Cari_Mahalle.EditValue = null;
                look_CariMuh.EditValue = null;
                Cari_Aktif.Checked = true;
                //txt_Cari_Kod.Focus();
            }
        }
        private void btn_Cari_Cikis_Click(object sender, EventArgs e)
        {
            Cikis();
        }

        private void Cikis()
        {
            if (AcikAdres == true)
            {
                if (txt_Cari_Kod.Text.Length == 0)
                {
                    MessageBox.Show(res_man.GetString("Cari Kodu Boş Geçilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            CariKod = txt_Cari_Kod.Text;
            this.Close();
        }

        private void look_Cari_Il_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(look_Cari_Il.EditValue))) return;

            look_Cari_Ilce.Properties.DataSource = dbtools.SelectTable("select Adres_Ad,Adres_Kod from Pos_Adres where Adres_UstGrup = '" + Convert.ToString(look_Cari_Il.EditValue) + "' and Adres_Sinif = '25' order by Adres_Ad");
            look_Cari_Ilce.Properties.DisplayMember = "Adres_Ad";
            look_Cari_Ilce.Properties.ValueMember = "Adres_Kod";

            look_Cari_Ilce.EditValue = null;
            look_Cari_Mahalle.EditValue = null;
        }

        private void look_Cari_Ilce_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(look_Cari_Ilce.EditValue))) return;

            look_Cari_Mahalle.Properties.DataSource = dbtools.SelectTable("select Adres_Ad,Adres_Kod from Pos_Adres where Adres_UstGrup = '" + Convert.ToString(look_Cari_Il.EditValue) + "' and Adres_AltGrup = '" + Convert.ToString(look_Cari_Ilce.EditValue) + "' and Adres_Sinif = '26' order by Adres_Ad");
            look_Cari_Mahalle.Properties.DisplayMember = "Adres_Ad";
            look_Cari_Mahalle.Properties.ValueMember = "Adres_Kod";

            look_Cari_Mahalle.EditValue = null;
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
                   + " 0, '" + txt_CariHes_Kodu.EditValue + "', '" + date_CariHes.DateTime.Date + "','" + Departman.Dep_Kodu + "','" + Convert.ToString(look_CariHes_Odeme.EditValue) + "', 0, '" + txt_CariHes_Odeme.Text.Replace(",", ".") + "' )");
            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariHesap, Log.Log_Islem.Kaydet, txt_CariHes_Kodu.EditValue + " kodlu cari Hesaba " + Convert.ToString(look_CariHes_Odeme.EditValue) + " " + Convert.ToString(look_CariHes_Odeme.Text) + " ile " + txt_CariHes_Odeme.Text.Replace(",", ".") + " tutarı Eklendi", String.Empty, String.Empty);
            gridyenile_CariHesap();
        }

        DataTable dtCari1 = new DataTable();
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

            dtCari1 = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 23, @Cari = '" + txt_CariHes_Kodu.EditValue + "' ");

            grd_CariHesap.DataSource = dtCari1;

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
            Arama ara = new Arama();
            //ara.Odeme_Ozelkod = 5;
            ara.ShowDialog();
            gridControl7.DataSource = null;

            txt_CariHes_Kodu.Text = ara.Cari_Kod;
            DataTable dt = dbtools.SelectTable("select Cari_Ad,Cari_Soyad from Pos_Cari WITH(NOLOCK) where Cari_Kod = '" + ara.Cari_Kod + "'");
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
                    date_CariHes.DateTime = Param.Tarih;
                    look_CariHes_Odeme.EditValue = null;
                    txt_CariHes_Odeme.Text = "0";
                }

                if (Convert.ToString(gridView9.GetFocusedRowCellValue("Chrk_Cek")) != "")
                {
                    gridColumn56.FieldName = "Rec_Ad";
                    gridColumn57.FieldName = "Rsat_Miktar";
                    gridColumn58.FieldName = "Rsat_Emiktar";
                    gridColumn59.FieldName = "Rsat_Tutar";
                    gridColumn60.FieldName = "Rsat_Doviztutar";
                    gridColumn61.FieldName = "Rsat_Ba";

                    if (Param.Calisma_Sekli == 1)
                    {
                        gridColumn60.Visible = true;
                        gridColumn60.VisibleIndex = 4;
                    }
                    else
                    {
                        gridColumn59.Visible = true;
                        gridColumn59.VisibleIndex = 4;
                    }

                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 0;
                    com.CommandText = "Pos_Satis";
                    com.Parameters.AddWithValue("@Fisno", Convert.ToInt32(gridView9.GetFocusedRowCellValue("Chrk_Cek")));
                    com.Parameters.AddWithValue("@Rapor_Tipi", 2);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gridControl7.DataSource = dt;

                }
            }
        }

        private void btn_CariHep_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView9.RowCount > 0)
            {
                FisPr pr = new FisPr();
                pr.CariHesapPr(Convert.ToString(txt_CariHes_Kodu.EditValue),data: grd_CariHesap.DataSource as DataTable);
            }
        }
        #endregion

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == tab_Cari_Hesap)
            {
                date_CariHes.DateTime = Param.Tarih;

                look_CariHes_Odeme.Properties.DataSource = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad,Pkod_Ozelkod from Pos_Kodlar where Pkod_Sinif = '11' and Pkod_Ozelkod <> '5' order by Pkod_Kod");
                look_CariHes_Odeme.Properties.DisplayMember = "Pkod_Ad";
                look_CariHes_Odeme.Properties.ValueMember = "Pkod_Kod";
            }
            if (xtraTabControl1.SelectedTabPage == tab_Cari_Tanim)
            {
                if (AcikAdres)
                {
                    gridyenile_Cari();
                }

            }
            if (xtraTabControl1.SelectedTabPage == tab_CariIslem)
            {
                gridyenile_1();

                dateEdit1.DateTime = new DateTime(Param.Tarih.Year, Param.Tarih.Month, 1);
                dateEdit2.DateTime = new DateTime(Param.Tarih.Year, Param.Tarih.Month, 1).AddMonths(1).AddDays(-1);
            }

            if (xtraTabControl1.SelectedTabPage == tab_CariOzet)
            {
                gridyenile_2();

                dateEdit4.DateTime = new DateTime(Param.Tarih.Year, Param.Tarih.Month, 1);
                dateEdit3.DateTime = new DateTime(Param.Tarih.Year, Param.Tarih.Month, 1).AddMonths(1).AddDays(-1);
            }

            if (AcikAdres)
            {
                txt_Cari_Adres1.Select();
                txt_Cari_Adres1.Focus();
                txt_Cari_Kod_Click(txt_Cari_Adres1, null);
            }
            else
            {
                txt_Cari_Kod.Focus();
            }
        }

        #region Cari Özet
        private void gridyenile_2()
        {
            gridControl4.DataSource = dbtools.SelectTable("select Cari_Kod,Cari_Ad,Cari_Soyad from Pos_Cari order by Cari_Id desc");
        }

        private void gridView4_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView4.RowCount > 0)
            {
                gridColumn22.FieldName = "Rec_Ad";
                gridColumn23.FieldName = "Rsat_Miktar";
                gridColumn24.FieldName = "Rsat_Emiktar";
                gridColumn26.FieldName = "Rsat_Tutar";
                gridColumn27.FieldName = "Rsat_Doviztutar";
                gridColumn30.FieldName = "Rsat_Ba";
                gridColumn31.FieldName = "Rsat_Maliyet";

                if (Param.Calisma_Sekli == 1)
                {
                    gridColumn27.Visible = true;
                    gridColumn27.VisibleIndex = 4;
                }
                else
                {
                    gridColumn26.Visible = true;
                    gridColumn26.VisibleIndex = 4;
                }

                DataTable dt = dbtools.SelectTable(@"
select Rsat_Fisno,Rsat_Masa,Rsat_Tarih,
	sum(Rsat_Miktar) as Rsat_Miktar,case when Rsat_Emiktar = 'T' then '' else Rsat_Emiktar end as Rsat_Emiktar,
    Rec_Ad,sum(Rsat_Tutar) as Rsat_Tutar,sum(Rsat_Doviztutar) as Rsat_Doviztutar,sum(Rsat_Maliyet) as Rsat_Maliyet,
	Rsat_Cari
from Cst_Recete_Satis
	left join Cst_Recete on Rec_Genelkod = Rsat_Recete
where Rsat_Fisno in (select Rsat_Fisno from Cst_Recete_Satis where (Rsat_Tarih >= '" + dateEdit4.DateTime.Date + "' and Rsat_Tarih <= '" + dateEdit3.DateTime.Date + "') and Rsat_Ba = 'A' and Rsat_Cari = '" + Convert.ToString(gridView4.GetFocusedRowCellValue("Cari_Kod")) + @"') 
	and Rsat_Ba = 'B'
group by Rec_Ad,Rsat_Fisno,Rsat_Masa,Rsat_Tarih,Rsat_Emiktar,Rsat_Cari");

                gridControl5.DataSource = dt;
            }
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            string leftColumn = Convert.ToString(gridView4.GetFocusedRowCellValue("Cari_Ad")) + " " + Convert.ToString(gridView4.GetFocusedRowCellValue("Cari_Soyad"));
            string rightColumn = dateEdit4.DateTime.ToShortDateString() + "-" + dateEdit3.DateTime.ToShortDateString();


            PrintingSystem printingSystem1 = new PrintingSystem();
            PrintableComponentLink printableComponentLink1 = new PrintableComponentLink();
            printingSystem1.Links.AddRange(new object[] { printableComponentLink1 });
            printableComponentLink1.Component = gridControl5;
            printableComponentLink1.Landscape = false;
            printableComponentLink1.Margins = new System.Drawing.Printing.Margins(20, 20, 50, 20);

            PageHeaderFooter phf = printableComponentLink1.PageHeaderFooter as PageHeaderFooter;
            phf.Header.Content.Clear();
            phf.Header.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            phf.Header.Content.Add(leftColumn);
            phf.Header.Content.Add(rightColumn);
            phf.Header.LineAlignment = BrickAlignment.Far;
            printableComponentLink1.ShowPreview();
        }
        #endregion

        #region Cari İşlemler
        private void gridyenile_1()
        {
            gridControl1.DataSource = dbtools.SelectTable("select Cari_Kod,Cari_Ad,Cari_Soyad from Pos_Cari order by Cari_Id desc");
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                gridControl2.DataSource = dbtools.SelectTable("SELECT Chrk_Id ,Chrk_Cek,Chrk_Cari,Chrk_Tarih,Kodlar_Ad,Chrk_Borc,Chrk_Alacak,Pkod_Kod,Pkod_Ad "
    + " FROM Pos_Carihrk left join Stok_Kodlar on Chrk_Depart = Kodlar_Kod and Kodlar_Sinif = '01' left join  "
    + " Pos_Kodlar on Chrk_Odeme = Pkod_Kod and Pkod_Sinif = '11'  "
    + " WHERE Chrk_Cari = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod")) + "' "
    + " and (Chrk_Tarih >= '" + dateEdit1.DateTime.Date + "' and Chrk_Tarih <= '" + dateEdit2.DateTime.Date + "' ) "
    + " order by Chrk_Cek desc");

                txt_Borc.Text = gridColumn9.SummaryText;
                txt_Alacak.Text = gridColumn10.SummaryText;
                txt_Bakiye.Text = Convert.ToString(Convert.ToDecimal(gridColumn9.SummaryText) - Convert.ToDecimal(gridColumn10.SummaryText));

                gridControl3.DataSource = null;

            }
        }

        private void gridView2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                gridColumn12.FieldName = "Rec_Ad";
                gridColumn13.FieldName = "Rsat_Miktar";
                gridColumn14.FieldName = "Rsat_Emiktar";
                gridColumn15.FieldName = "Rsat_Tutar";
                gridColumn16.FieldName = "Rsat_Doviztutar";
                gridColumn17.FieldName = "Rsat_Ba";

                if (Param.Calisma_Sekli == 1)
                {
                    gridColumn16.Visible = true;
                    gridColumn16.VisibleIndex = 4;
                }
                else
                {
                    gridColumn15.Visible = true;
                    gridColumn15.VisibleIndex = 4;
                }

                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", Convert.ToInt32(gridView2.GetFocusedRowCellValue("Chrk_Cek")));
                com.Parameters.AddWithValue("@Rapor_Tipi", 2);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gridControl3.DataSource = dt;
            }
        }

        private void btn_Fispr_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                FisPr pr = new FisPr();
                if (Param.Param_YeniHesapDkm)
                {
                    pr.newHesapDokum(true, Convert.ToInt32(gridView2.GetFocusedRowCellValue("Chrk_Cek")), 0, "* * * HESAP FİŞİ * * *");
                }
                else
                {
                    pr.HesapDokum(true, Convert.ToInt32(gridView2.GetFocusedRowCellValue("Chrk_Cek")), 0);
                }
            }
        }









        #endregion

        private void özelSilmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.P_Kod.ToUpper() != "RMOS")
            {
                MessageBox.Show(res_man.GetString("Bu İşlemi Yapmaya Yetkiniz Yoktur."));
                return;
            }
            if (MessageBox.Show(res_man.GetString("Seçili Satırı Silmek İstiyor Musunuz?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            Chrk_Id = Convert.ToInt32(gridView9.GetFocusedRowCellValue("Chrk_Id"));
            dbtools.execcmd("delete Pos_Carihrk where Chrk_Id = '" + Chrk_Id + "'");
            gridyenile_CariHesap();
        }

        private void KapanisAcilis()
        {
            string odemeKodCari = Convert.ToString(dbtools.DegerGetir("select pk.Pkod_Kod from Pos_Kodlar as pk where pk.Pkod_Sinif = '11' and pk.Pkod_Ozelkod = '5'"));

            if (odemeKodCari == "")
            {
                MessageBox.Show(res_man.GetString("Cari kodu tanımlı değil."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dtCari = dbtools.SelectTable(@"select  pc.Chrk_Cari as Chrk_Cari, (SUM(pc.Chrk_Borc) - SUM(pc.Chrk_Alacak)) as Bakiye from Pos_Carihrk pc 
                                where DATEPART(year, pc.Chrk_Tarih) <= '" + spinEdit1.EditValue + "'   group by pc.Chrk_Cari   having(SUM(pc.Chrk_Borc) - SUM(pc.Chrk_Alacak)) <> 0");

            if (dtCari.Rows.Count > 0)
            {
                for (int i = 0; i < dtCari.Rows.Count; i++)
                {
                    int Sonuc = Convert.ToInt32(dbtools.DegerGetir("Select Count(*) From Pos_Carihrk as pc  where DATEPART(year, pc.Chrk_Tarih) <= '" + spinEdit1.EditValue + @"' 
                                and pc.Chrk_Cari = '" + Convert.ToString(dtCari.Rows[i]["Chrk_Cari"]) + "'     and Convert(date,pc.Chrk_Tarih) = Convert(date,'" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31) + @"')
                                and pc.Chrk_Odeme = '" + odemeKodCari + "'"));

                    if (Sonuc == 0)
                    {
                        if (Convert.ToDecimal(dtCari.Rows[i]["Bakiye"]) > 0)
                        {
                            dbtools.execcmd(@"INSERT INTO [dbo].[Pos_Carihrk] ([Chrk_Cek],[Chrk_Cekid],[Chrk_Cari],[Chrk_Tarih],[Chrk_Depart],[Chrk_Odeme],[Chrk_Borc],[Chrk_Alacak])
                                              VALUES(0,null,'" + Convert.ToString(dtCari.Rows[i]["Chrk_Cari"]) + "','" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31) + "',null,'" + odemeKodCari + "',0,'" + Math.Abs(Convert.ToDecimal(dtCari.Rows[i]["Bakiye"])).ToString().Replace(",", ".")
                                              + "')");

                            dbtools.execcmd(@"INSERT INTO [dbo].[Pos_Carihrk] ([Chrk_Cek],[Chrk_Cekid],[Chrk_Cari],[Chrk_Tarih],[Chrk_Depart],[Chrk_Odeme],[Chrk_Borc],[Chrk_Alacak])
                                              VALUES(0,null,'" + Convert.ToString(dtCari.Rows[i]["Chrk_Cari"]) + "','" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31).AddDays(1) + "',null,'" + odemeKodCari + "','" + Math.Abs(Convert.ToDecimal(dtCari.Rows[i]["Bakiye"])).ToString().Replace(",", ".")
                                              + "',0)");
                        }
                        else
                        {

                            dbtools.execcmd(@"INSERT INTO [dbo].[Pos_Carihrk] ([Chrk_Cek],[Chrk_Cekid],[Chrk_Cari],[Chrk_Tarih],[Chrk_Depart],[Chrk_Odeme],[Chrk_Borc],[Chrk_Alacak])
                                              VALUES(0,null,'" + Convert.ToString(dtCari.Rows[i]["Chrk_Cari"]) + "','" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31) + "',null,'" + odemeKodCari + "','" + Math.Abs(Convert.ToDecimal(dtCari.Rows[i]["Bakiye"])).ToString().Replace(",", ".")
                                              + "',0)");

                            dbtools.execcmd(@"INSERT INTO [dbo].[Pos_Carihrk] ([Chrk_Cek],[Chrk_Cekid],[Chrk_Cari],[Chrk_Tarih],[Chrk_Depart],[Chrk_Odeme],[Chrk_Borc],[Chrk_Alacak])
                                              VALUES(0,null,'" + Convert.ToString(dtCari.Rows[i]["Chrk_Cari"]) + "','" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31).AddDays(1) + "',null,'" + odemeKodCari + "',0,'" + Math.Abs(Convert.ToDecimal(dtCari.Rows[i]["Bakiye"])).ToString().Replace(",", ".")
                                             + "')");

                        }
                    }
                    else
                    {
                        if (Convert.ToDecimal(dtCari.Rows[i]["Bakiye"]) > 0)
                        {
                            dbtools.execcmd(@"Update pc Set Chrk_Tarih = '" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31) + "', Chrk_Borc = 0, Chrk_Alacak = '" + Math.Abs(Convert.ToDecimal(dtCari.Rows[i]["Bakiye"])).ToString().Replace(",", ".") + "'  from Pos_Carihrk pc  where DATEPART(year, pc.Chrk_Tarih) <= '" + spinEdit1.EditValue + "'   and pc.Chrk_Cari = '" + Convert.ToString(dtCari.Rows[i]["Chrk_Cari"]) + "'   and Convert(date,pc.Chrk_Tarih) = Convert(date,'" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31) + "') and pc.Chrk_Odeme = '" + odemeKodCari + "'");


                            dbtools.execcmd(@"Update pc Set Chrk_Tarih = '" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31) + "', Chrk_Borc = '" + Math.Abs(Convert.ToDecimal(dtCari.Rows[i]["Bakiye"])).ToString().Replace(",", ".") + "', Chrk_Alacak = 0   from Pos_Carihrk pc  where DATEPART(year, pc.Chrk_Tarih) <= '" + spinEdit1.EditValue + "' and pc.Chrk_Cari = '" + Convert.ToString(dtCari.Rows[i]["Chrk_Cari"]) + "' and Convert(date,pc.Chrk_Tarih) = Convert(date,'" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31).AddDays(1) + "')  and pc.Chrk_Odeme = '" + odemeKodCari + "'");
                        }
                        else
                        {
                            dbtools.execcmd("Update pc Set Chrk_Tarih = @'" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31) + "', Chrk_Borc = '" + Math.Abs(Convert.ToDecimal(dtCari.Rows[i]["Bakiye"])).ToString().Replace(",", ".") + @"', Chrk_Alacak = 0
                                from Pos_Carihrk as pc where DATEPART(year, pc.Chrk_Tarih) <= '" + spinEdit1.EditValue + @"' 
                                and pc.Chrk_Cari = '" + Convert.ToString(dtCari.Rows[i]["Chrk_Cari"]) + @"'
                                and Convert(date,pc.Chrk_Tarih) = Convert(date,'" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31) + @"')
                                and pc.Chrk_Odeme = '" + odemeKodCari + "'");

                            dbtools.execcmd("Update pc Set Chrk_Tarih = @'" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31) + "', Chrk_Borc = 0, Chrk_Alacak = '" + Math.Abs(Convert.ToDecimal(dtCari.Rows[i]["Bakiye"])).ToString().Replace(",", ".") + @"'
                                from Pos_Carihrk as pc where DATEPART(year, pc.Chrk_Tarih) <= '" + spinEdit1.EditValue + @"' 
                                and pc.Chrk_Cari = '" + Convert.ToString(dtCari.Rows[i]["Chrk_Cari"]) + @"'
                                and Convert(date,pc.Chrk_Tarih) = Convert(date,'" + new DateTime(Convert.ToInt32(spinEdit1.EditValue), 12, 31).AddDays(1) + @"')
                                and pc.Chrk_Odeme = '" + odemeKodCari + "'");

                        }
                    }
                }
            }

            MessageBox.Show(res_man.GetString("Kapanış Açılış, yapıldı."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            KapanisAcilis();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            if (txt_Active == null)
            {
                MessageBox.Show(res_man.GetString("Yazılacak Alanı Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txt_Active.Text += btn.Text;
        }

        private void Back_Space_Click(object sender, EventArgs e)
        {
            if (txt_Active.Text.Length > 0)
            {
                txt_Active.Text = txt_Active.Text.Substring(0, txt_Active.Text.Length - 1);
            }
        }

        private void txt_Cari_Kod_Click(object sender, EventArgs e)
        {
            TextEdit txt = sender as TextEdit;
            txt_Active = txt;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridControl6.DataSource = null;
            gridView6.RefreshData();
            gridControl6.Refresh();
            gridControl6.DataSource = dbtools.SelectTable("Exec Pos_Sorgu @Sorgu_Tipi = 26, @Tarih1 = '" + dateTarih1.DateTime.Date + "', @Tarih2 = '" + dateTarih2.DateTime.Date + "'");

            gridControl8.DataSource = null;
            gridView10.RefreshData();
            gridControl8.Refresh();
            gridControl8.DataSource = dbtools.SelectTable("Exec Pos_Sorgu @Sorgu_Tipi = 27");
        }
        private void simpleButton4_Click(object sender, EventArgs e)
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
            gridColumn1.FieldName = "Cari_Tel2";
            gridColumn2.FieldName = "Cari_Email";
            gridColumn18.FieldName = "Cari_Tip";
            gridColumn32.FieldName = "Cari_Limit";
            gridColumn33.FieldName = "Cari_LimitTutar";
            gridColumn34.FieldName = "Cari_Il";
            gridColumn35.FieldName = "Cari_Ilce";
            gridColumn54.FieldName = "Cari_Mahalle";
            gridColumn55.FieldName = "Cari_MuhasebeKodu";
            gridColumn80.FieldName = "Cari_indirimOran";

            DataTable dt = dbtools.SelectTable(@"
        SELECT ISNULL(Cari_Id,0)            as Cari_Id
      , ISNULL(Cari_Kod, '')                as Cari_Kod
      , ISNULL(Cari_Ad, '')                 as Cari_Ad
      , ISNULL(Cari_Soyad, '')              as Cari_Soyad
      , ISNULL(Cari_Tel, '')                as Cari_Tel
      , ISNULL(Cari_Adres1, '')             as Cari_Adres1
      , ISNULL(Cari_Adres2, '')             as Cari_Adres2
      , ISNULL(Cari_Adres3, '')             as Cari_Adres3
      , ISNULL(Cari_Funvan, '')             as Cari_Funvan
      , ISNULL(Cari_Fadres1, '')            as Cari_Fadres1
      , ISNULL(Cari_Fadres2, '')            as Cari_Fadres2
      , ISNULL(Cari_Vergidarie, '')         as Cari_Vergidarie
      , ISNULL(Cari_Vergino, '')            as Cari_Vergino
      , ISNULL(Cari_Mail, '')               as Cari_Mail
      , ISNULL(Cari_Kart, '')               as Cari_Kart
      , ISNULL(Cari_Tel2, '')               as Cari_Tel2
      , ISNULL(Cari_Email, '')              as Cari_Email
      , ISNULL(Cari_Tip, 'C')               as Cari_Tip
      , ISNULL(Cari_Limit, 0)               as Cari_Limit
      , ISNULL(Cari_LimitTutar, 0)          as Cari_LimitTutar
      , ISNULL(Cari_Il, '')                 as Cari_Il
      , ISNULL(Cari_Ilce, '')               as Cari_Ilce
      , ISNULL(Cari_Mahalle, '')            as Cari_Mahalle
      , ISNULL(Cari_MuhasebeKodu, '')       as Cari_MuhasebeKodu
      , ISNULL(Cari_Aktif, 1)               as Cari_Aktif
      , ISNULL(Cari_YS_AddressId, 0)        as Cari_YS_AddressId
      , ISNULL(Cari_YS_CustomerID, '')      as Cari_YS_CustomerID
      , ISNULL(Cari_Funvan2, '')            as Cari_Funvan2
      , ISNULL(Cari_DogumTar, getdate())    as Cari_DogumTar
      , ISNULL(Cari_indirimOran,0)          as Cari_indirimOran
  FROM [dbo].[Pos_Cari] order by Cari_Kod");


            grd_Cari.DataSource = dt;

            look_Cari_Tip.EditValue = "C";
            txt_Cari_Kod.EditValue = "";
            txt_Cari_Ad.EditValue = "";
            txt_Cari_Soyad.EditValue = "";
            txt_Cari_Telefon.EditValue = "";
            txt_Cari_Adres1.EditValue = "";
            txt_Cari_Adres2.EditValue = "";
            txt_Cari_Adres3.EditValue = "";
            txt_Cari_F_Unvan.EditValue = "";
            txt_Cari_F_Unvan2.EditValue = "";
            txt_Cari_F_Adres1.EditValue = "";
            txt_Cari_F_Adres2.EditValue = "";
            txt_Cari_F_Vergidaire.EditValue = "";
            txt_Cari_F_Vergino.EditValue = "";
            txt_Cari_F_Mail.EditValue = "";
            txt_Cari_Kart_No.EditValue = "";
            txt_Cari_Telefon2.EditValue = "";
            txt_Cari_Email.EditValue = "";
            chk_Cari_limit.Checked = false;
            spn_Cari_Limit.Value = 0;
            look_Cari_Il.EditValue = null;
            look_Cari_Ilce.EditValue = null;
            look_Cari_Mahalle.EditValue = null;
            look_CariMuh.EditValue = null;

            if (AcikAdres)
            {
                txt_Cari_Adres1.Focus();
                txt_Cari_Kod_Click(txt_Cari_Adres1, null);
            }
            else
            {
                txt_Cari_Kod.Focus();
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(gridView9.GetFocusedRowCellValue("Chrk_Cek")) != "")
            {
                DialogResult c = DialogResult.Yes;
                if (Param.Param_Adispr_Uyari)
                {
                    c = MessageBox.Show(res_man.GetString("Adisyon Dökülecek... Devam Etmek İstiyor Musunuz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                if (c == System.Windows.Forms.DialogResult.Yes)
                {
                    AdisyonPr adisyon = new AdisyonPr();
                    string cevap = adisyon.Adisyon_Yaz(Convert.ToInt32(gridView9.GetFocusedRowCellValue("Chrk_Cek")));
                    if (cevap != "OK")
                    {
                        MessageBox.Show(cevap);
                        return;
                    }
                    adisyon.Adisyon_Sayac_Arttir(Convert.ToInt32(gridView9.GetFocusedRowCellValue("Chrk_Cek")));
                }
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Rapor_Print("Cari Hesap Döküm Raporu", gridControl6);
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


        private void Excel(DevExpress.XtraGrid.GridControl gc, string ExcelAdi)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string fName = string.Empty;
            saveFileDialog1.Filter = "Excel Document (*.xls)|*.xls";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = ExcelAdi + ".xls";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != null)
                {
                    DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.WYSIWYG;
                    XlsExportOptions opt = new XlsExportOptions();
                    opt.ShowGridLines = true;
                    gc.ExportToXls(saveFileDialog1.FileName, opt);
                }
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            Excel(gridControl6, "DepartmanCariTahsilat");
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            Rapor_Print("Cari Hesap Döküm Raporu", gridControl8);
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            Excel(gridControl8, "CarilerinTahsilatListesi");
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            Excel(grd_CariHesap, "CariHesapLİstesi");
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            Excel(gridControl2, "CariHesapHareketleri");
        }

        private void Fis_Print(DataTable dt)
        {
            List<string> list;

            DataTable dt3 = dbtools.SelectTable("SELECT Pkod_Ad, Pkod_Satir FROM Pos_Kodlar  where Pkod_Sinif = '16' and Pkod_Ustgrup = 'HES' and Pkod_Kod = '" + Departman.Dep_Kodu + "' ");
            if (dt3.Rows.Count > 0)
            {
                string Printer = dt3.Rows[0]["Pkod_Ad"].ToString();
                int Bos_Satir = Convert.ToInt32(dt3.Rows[0]["Pkod_Satir"].ToString());

                string tesis_Adi = Param.Tesis_Adi;

                list = new List<string>();

                //if (rdo_X_Z.SelectedIndex == 0) list.Add("    ***  X RAPORU  ***");
                //if (rdo_X_Z.SelectedIndex == 1) list.Add("    ***  Z RAPORU  ***");
                list.Add(". ");
                list.Add("    " + tesis_Adi);
                list.Add("    " + Param.Tarih.ToShortDateString());
                //if (rdo_X_Z.SelectedIndex == 0) list.Add("    " + "Garson: " + look_Garson.Text);
                list.Add("    " + DateTime.Now.ToShortTimeString());
                list.Add(".");
                list.Add("--------------------------------");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(Convert.ToString(dt.Rows[i]["ADI"]).PadRight(10, " "[0]).Substring(0, 10) + " " + Convert.ToString(dt.Rows[i]["SOYADI"]).PadLeft(5, " "[0]) + " " + Convert.ToString(dt.Rows[i]["BAKIYE"]).PadLeft(8, " "[0]));
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
                    //Liste = list;
                    printFont = fnt;
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                    pd.PrinterSettings.PrinterName = Printer;
                    pd.Print();
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
        private void simpleButton12_Click(object sender, EventArgs e)
        {
            Fis_Print((DataTable)gridControl8.DataSource);
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            Excel(gridControl9, "CariyeAtilanSatislar");
            Excel(gridControl10, "CariTahsilat");
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            gridView11.Columns.Clear();
            gridView11.BestFitColumns();
            gridControl9.DataSource = dbtools.SelectTable(@"declare @Indkapatma nvarchar(20) = (select Pkod_Kod from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '11' and Pkod_Ozelkod = '4')

            SELECT
            Convert(date,Rsat_Tarih) as TARIH,
            Rsat_Fisno as FIS_NO,
            MIN(Rsat_Odano) as ODA_NO,
            MIN(Cari_Kod) as CARI_KOD,
            (MAX(Cari.Cari_Ad) + ' ' + ISNULL(MAX(Cari.Cari_Soyad),'') + ' ' +MAX(Rsat_Cari)) as AD_SOYAD,
            SUM(Rsat_Tutar) as TUTAR
            FROM Cst_Recete_Satis as Satis WITH(NOLOCK)
            LEFT OUTER JOIN Pos_Cari AS Cari WITH(NOLOCK) ON Rsat_Cari = Cari.Cari_Kod
            WHERE 
            (CONVERT(date,Satis.Rsat_Tarih) >= CONVERT(date,'" + cari2_Tarih1.EditValue + @"'))
            AND
            (CONVERT(date,Satis.Rsat_Tarih) <= CONVERT(date,'" + cari2_Tarih2.EditValue + @"'))
            AND 
            Rsat_Ba = 'A' and Rsat_Kapatma <> @Indkapatma
            GROUP BY Rsat_Fisno,Rsat_Tarih
            order by Rsat_Fisno desc
            ");
            gridView11.Columns["TUTAR"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "TUTAR", "TOPLAM = {0}");

            gridView12.Columns.Clear();
            gridView12.BestFitColumns();
            gridControl10.DataSource = dbtools.SelectTable(@"
            select 
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
	        and (CONVERT(date,Chrk_Tarih) >= CONVERT(date,'" + cari2_Tarih1.DateTime.Date + @"'))
	        AND	(CONVERT(date,Chrk_Tarih) <= CONVERT(date,'" + cari2_Tarih2.DateTime.Date + @"'))
	        group by Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Chrk_Tarih,kodlar.Pkod_Ad 
	        order by Cari_Kod
                    ");


            gridView12.Columns["TAHSILAT"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "TAHSILAT", "TOPLAM = {0}");

        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            Kaydet();
            Cikis();
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            gridControl11.DataSource = null;

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Sorgu";
            com.Parameters.AddWithValue("@Sorgu_Tipi", 42);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl11.DataSource = dt;
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            Excel(gridControl11, "Cari Listesi");
        }

        private void gridView8_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (gridView8.RowCount > 0)
                {
                    look_Cari_Tip.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Tip"));
                    txt_Cari_Kod.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Kod"));
                    txt_Cari_Ad.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Ad"));
                    txt_Cari_Soyad.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Soyad"));
                    txt_Cari_Telefon.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Tel"));
                    txt_Cari_Adres1.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Adres1"));
                    txt_Cari_Adres2.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Adres2"));
                    txt_Cari_Adres3.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Adres3"));
                    txt_Cari_F_Unvan.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Funvan"));
                    txt_Cari_F_Unvan2.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Funvan2"));
                    txt_Cari_F_Adres1.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Fadres1"));
                    txt_Cari_F_Adres2.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Fadres2"));
                    txt_Cari_F_Vergidaire.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Vergidarie"));
                    txt_Cari_F_Vergino.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Vergino"));
                    txt_Cari_F_Mail.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Mail"));
                    txt_Cari_Kart_No.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Kart"));
                    txt_Cari_Telefon2.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Tel2"));
                    txt_Cari_Email.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Email"));
                    chk_Cari_limit.Checked = Convert.ToBoolean(gridView8.GetFocusedRowCellValue("Cari_Limit"));
                    spn_Cari_Limit.Value = Convert.ToDecimal(gridView8.GetFocusedRowCellValue("Cari_LimitTutar"));
                    look_Cari_Il.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Il"));
                    look_Cari_Ilce.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Ilce"));
                    look_Cari_Mahalle.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Mahalle"));
                    look_CariMuh.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_MuhasebeKodu"));
                    Cari_Aktif.Checked = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_Aktif")) == "" ? false : Convert.ToBoolean(gridView8.GetFocusedRowCellValue("Cari_Aktif"));
                    Cari_DogumTar.EditValue = Convert.ToString(gridView8.GetFocusedRowCellValue("Cari_DogumTar"));


                    txtIndOran.EditValue = gridView8.GetFocusedRowCellValue("Cari_indirimOran").ToString();

                }

                //if (AcikAdres)
                //{
                //    txt_Cari_Adres1.Focus();
                //    txt_Cari_Kod_Click(txt_Cari_Adres1, null);
                //}
                //else
                //{
                //    txt_Cari_Kod.Focus();
                //}
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "gridView8_RowClick", "",ex);
            }
            

        }
        public static string MyClass = "CariHesap";
        private void btnCariRap3Listele_Click(object sender, EventArgs e)
        {
            rap3Listele();
        }


        public DataTable rap3Listele(bool hepsi = false,bool sifirDahil=false)
        {
            try
            {
                string basTar = Convert.ToDateTime(dateEditRap3BasTar.EditValue).ToString("yyyy-MM-dd");
                string bitTar = Convert.ToDateTime(dateEditRap3BitTar.EditValue).ToString("yyyy-MM-dd");

                if (hepsi)
                {
                    basTar = "2000-01-01";
                    bitTar = "3000-01-01";
                }

                string sifir = "";
                if (sifirDahil==false)
                {
                    sifir = "having sum(Chrk_Borc-Chrk_Alacak)>0";
                }

                string tip = look_Cari_TipBakiye.EditValue.ToString();

                string query = @"select Chrk_Cari as CariId,Cari_Ad as Ad,Cari_Soyad as Soyad,sum(Chrk_Borc-Chrk_Alacak) as Bakiye from Pos_Carihrk as hrk 
left join Pos_Cari as cari on CONVERT(varchar(500), cari.Cari_Id)=hrk.Chrk_Cari 
where isnull(Cari_Tip,'C')='"+ tip + "' and Chrk_Tarih between '" + basTar + @"' and '" + bitTar + @"' 
group by Cari_Ad,Cari_Soyad,Chrk_Cari "+ sifir;

                DataTable dataTable = dbtools.SelectTableR(query);

                gridControlCariRap3.DataSource = dataTable;
                gridviewCountYaz(gridViewCariRap3);


                if (gridViewCariRap3.FocusedRowHandle > -1)
                {
                    gridViewCariRap3.FocusedRowHandle = -2147483646;
                    gridViewCariRap3.FocusedRowHandle = 0;
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnCariRap3Listele_Click", "", ex);
            }

            return null;
        }

        public void gridviewCountYaz(GridView grid)
        {
            if (grid.Columns.Count > 0)
            {
                grid.Columns[0].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                grid.Columns[0].SummaryItem.FieldName = grid.Columns[0].FieldName;
                grid.Columns[0].SummaryItem.DisplayFormat = "{0:n0}";
                grid.UpdateTotalSummary();
            }
        }

        private void btnExcelKaydetCariRapor3_Click(object sender, EventArgs e)
        {
            yazdir(gridControlCariRap3);
            
        }

        public void yazdir2(GridControl gridControl)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2010) (.xlsx)|*.xlsx|Excel (2003)(.xls)|*.xls|RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;
                    string dosyaAdi = new FileInfo(exportFilePath).Name.Replace(fileExtenstion,"")+"_detay";

                    //string dosyaKonum = Path.GetDirectoryName(Application.ExecutablePath);

                    string basePath = Path.GetDirectoryName(exportFilePath);
                    string base2 = basePath + "\\" + dosyaAdi + fileExtenstion;


                    switch (fileExtenstion)
                    {
                        case ".xls":
                            gridControl.ExportToXls(exportFilePath);
                            gridControlCariRap3Detay.ExportToXls(base2);

                            break;
                        case ".xlsx":
                            gridControl.ExportToXlsx(exportFilePath);
                            gridControlCariRap3Detay.ExportToXlsx(base2);

                            break;
                        case ".rtf":
                            gridControl.ExportToRtf(exportFilePath);
                            gridControlCariRap3Detay.ExportToRtf(base2);

                            break;
                        case ".pdf":
                            gridControl.ExportToPdf(exportFilePath);
                            gridControlCariRap3Detay.ExportToPdf(base2);

                            break;
                        case ".html":
                            gridControl.ExportToHtml(exportFilePath);
                            gridControlCariRap3Detay.ExportToHtml(base2);

                            break;
                        case ".mht":
                            gridControl.ExportToMht(exportFilePath);
                            gridControlCariRap3Detay.ExportToMht(base2);

                            break;
                        default:
                            break;
                    }
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(basePath)
                    {
                        UseShellExecute = true
                    };
                    p.Start();

                }
            }
        }

        public void yazdir(GridControl gridControl)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2010) (.xlsx)|*.xlsx|Excel (2003)(.xls)|*.xls|RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
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

        private void btnCariBakiyePrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt3 = dbtools.SelectTable("SELECT Pkod_Ad, Pkod_Satir FROM Pos_Kodlar  where Pkod_Sinif = '16' and Pkod_Ustgrup = 'HES' and Pkod_Kod = '" + Departman.Dep_Kodu + "' ");
                if (dt3 != null && dt3.Rows.Count > 0)
                {
                    string printerName = dt3.Rows[0]["Pkod_Ad"].ToString();

                    DataTable dataTable = gridControlCariRap3.DataSource as DataTable;

                    string toplamBakiye = gridViewCariRap3.Columns["Bakiye"].SummaryItem.SummaryValue.ToString();
                    // txtToplamBakiye

                    CariBakiyeKontrolRapor rapor = new CariBakiyeKontrolRapor();
                    rapor.txtToplamBakiye.Text = toplamBakiye;
                    rapor.DataSource = dataTable;
                    rapor.PrinterName = printerName;

                    rapor.txtTarih.Text = DateTime.Now.ToString("dd.MM.yyyy");
                    rapor.txtDepAd.Text = Departman.Dep_Adi;
                    rapor.Print();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void btnCariBakiyeMail_Click(object sender, EventArgs e)
        {
            try
            {
                string klasor = "CariRapor";
                if (!Directory.Exists(klasor))
                {
                    Directory.CreateDirectory(klasor);
                }

                string path = klasor+"\\"+DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")+".pdf";

                gridViewCariRap3.ExportToPdf(path);

                Mail_Gonder(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void Mail_Gonder(string fileName)
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

                        MailMessage ePosta = new MailMessage();

                        ePosta.Attachments.Add(new Attachment(fileName));

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
                        ePosta.Subject = "CARİ BAKİYE KONTROL RAPOR "+DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                        ePosta.IsBodyHtml = true;
                        ePosta.Body = ePosta.Subject;


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
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Gun_Sonu, Log.Log_Islem.Kaydet, DateTime.Now.ToString("dd.MM.yyyy") + " Mail Gönderim Sırasında Hata Oldu...", "", "");
                        MessageBox.Show(res_man.GetString("Mail Gönderilemedi...") + "\n" + err.Message, res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }

        private void btnCariRap3ListeleHepsi_Click(object sender, EventArgs e)
        {
            rap3Listele(sifirDahil:true);
        }

        private void gridViewCariRap3_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridViewCariRap3.FocusedRowHandle<0)
            {
                return;
            }
            //gridColumn22.FieldName = "Rec_Ad";
            //gridColumn23.FieldName = "Rsat_Miktar";
            //gridColumn24.FieldName = "Rsat_Emiktar";
            //gridColumn26.FieldName = "Rsat_Tutar";
            //gridColumn27.FieldName = "Rsat_Doviztutar";
            //gridColumn30.FieldName = "Rsat_Ba";
            //gridColumn31.FieldName = "Rsat_Maliyet";

            //if (Param.Calisma_Sekli == 1)
            //{
            //    gridColumn27.Visible = true;
            //    gridColumn27.VisibleIndex = 4;
            //}
            //else
            //{
            //    gridColumn26.Visible = true;
            //    gridColumn26.VisibleIndex = 4;
            //}


            string query = @"
select Rsat_Fisno,Rsat_Masa,Rsat_Tarih,
	sum(Rsat_Miktar) as Rsat_Miktar,case when Rsat_Emiktar = 'T' then '' else Rsat_Emiktar end as Rsat_Emiktar,
    Rec_Ad,sum(Rsat_Tutar) as Rsat_Tutar,sum(Rsat_Doviztutar) as Rsat_Doviztutar,sum(Rsat_Maliyet) as Rsat_Maliyet,
	Rsat_Cari
from Cst_Recete_Satis
	left join Cst_Recete on Rec_Genelkod = Rsat_Recete
where Rsat_Fisno in (select Rsat_Fisno from Cst_Recete_Satis where (Rsat_Tarih >= '" + dateEditRap3BasTar.DateTime.Date + "' and Rsat_Tarih <= '" + dateEditRap3BitTar.DateTime.Date + "') and Rsat_Ba = 'A' and Rsat_Cari = '" + Convert.ToString(gridViewCariRap3.GetFocusedRowCellValue("CariId")) + @"') 
	and Rsat_Ba = 'B'
group by Rec_Ad,Rsat_Fisno,Rsat_Masa,Rsat_Tarih,Rsat_Emiktar,Rsat_Cari";
                DataTable dt = dbtools.SelectTable(query);

                gridControlCariRap3Detay.DataSource = dt;
        }

        private void btnExcelKaydetCariRapor3Detay_Click(object sender, EventArgs e)
        {
            yazdir(gridControlCariRap3Detay);

        }

        private void btnFiltrele_Click(object sender, EventArgs e)
        {
            try
            {
                //var data = grd_CariHesap.DataSource as DataTable;


                //var yazilacaklar = data.Select("Chrk_Tarih between '" + dateEditCariBas.DateTime.ToString("yyyy-MM-dd") + "' and " + dateEditCariBit.DateTime.ToString("yyyy-MM-dd") + "").CopyToDataTable();


                var yazilacaklar = dtCari1.Select("Chrk_Tarih >= #"+ dateEditCariBas.DateTime.ToString("yyyy-MM-dd") + "#  and Chrk_Tarih<= #" + dateEditCariBit.DateTime.ToString("yyyy-MM-dd") + "#").CopyToDataTable();

                grd_CariHesap.DataSource = yazilacaklar;
                Bakiye.Text = Convert.ToString(Convert.ToDecimal(gridColumn51.SummaryText) - Convert.ToDecimal(gridColumn52.SummaryText));
            }
            catch(Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnFiltrele_Click", "",ex);
            }
           
        }
    }
}