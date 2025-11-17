using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Arama : XtraForm
    {
        // naber mustafa
        public string KapatmaKodu { get; set; }
        public int Odeme_Ozelkod { get; set; }
        CheckButton chk_MusTipi = null;
        CheckButton chk_Filtre = null;
        public bool Cikis { get; set; } = false;
        public bool HizliSatis { get; set; } = false;
        public bool FormClose { get; set; } = false;
        public string Mus_tipi { get; set; }
        public string Oda_No { get; set; }
        public int Folio { get; set; }
        public int Master_Folio { get; set; }
        public string Pansiyon { get; set; }
        public string Odeme_Kodu { get; set; }
        public int Uye_Id { get; set; }
        public string Uye_Adsoyad { get; set; }
        public string Uye_Kartturu { get; set; }
        public string Cari_Kod { get; set; } = "";
        public string Kart_No { get; set; }
        public string Ind_Kodu { get; set; }
        public decimal Ind_Oran { get; set; }
        public decimal Cari_indirimOran { get; set; } = 0;
        public string Bilgi { get; set; } = string.Empty;

        public Arama()
        {
            InitializeComponent();
        }

        public bool otomatikSatis = false;
        public string kartnom = "";

        private void Arama_Load(object sender, EventArgs e)
        {

            this.BringToFront();

            if (Convert.ToString(this.Tag) == "D")
            {
                btn_Cikis.Visible = true;
            }

            if (Departman.Sorgu_Sekli == 0)
            {
                chk_OdaNo.Enabled = true;
                chk_KartNo.Enabled = false;
                chk_Ad.Enabled = false;
                chk_Soyad.Enabled = false;

                chk_OdaNo.Checked = true;
            }
            if (Departman.Sorgu_Sekli == 1)
            {
                chk_OdaNo.Enabled = false;
                chk_KartNo.Enabled = true;
                chk_Ad.Enabled = false;
                chk_Soyad.Enabled = false;

                chk_KartNo.Checked = true;
            }
            if (Departman.Sorgu_Sekli == 2)
            {
                chk_OdaNo.Enabled = true;
                chk_KartNo.Enabled = true;
                chk_Ad.Enabled = true;
                chk_Soyad.Enabled = true;
            }

            if (Odeme_Ozelkod == 5)
            {
                chk_OdaHesap.Enabled = false;
                chk_UyeHesap.Enabled = false;
                chk_CariHesap.Checked = true;
            }

            if (Param.Tesis_Tipi == 1)
            {
                chk_OdaHesap.Enabled = false;
                chk_UyeHesap.Enabled = false;
                chk_CariHesap.Checked = true;
            }

            txt_Arama.Focus();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            Cikis = true;
            FormClose = true;
            HizliSatis = false;
            this.Close();
        }

        private void Btn_klavye_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            txt_Arama.Text = txt_Arama.Text + btn.Text;
        }

        private void chk_Hesap_CheckedChanged(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;
            txt_Arama.Text = String.Empty;
            CheckButton chkbtn = (CheckButton)sender;

            if (chk_MusTipi == null)
            {
                chk_MusTipi = chkbtn;
            }

            if (chk_MusTipi == chk_OdaHesap)
            {
                chk_OdaHesap.Checked = true;
                chk_UyeHesap.Checked = false;
                chk_CariHesap.Checked = false;
            }
            if (chk_MusTipi == chk_UyeHesap)
            {
                chk_OdaHesap.Checked = false;
                chk_UyeHesap.Checked = true;
                chk_CariHesap.Checked = false;
                chk_KartNo.Checked = true;
            }
            if (chk_MusTipi == chk_CariHesap)
            {
                chk_OdaHesap.Checked = false;
                chk_UyeHesap.Checked = false;
                chk_CariHesap.Checked = true;

                chk_OdaNo.Text = "Cari Kod";
            }
            chk_MusTipi = null;
            txt_Arama.Focus();
        }

        private void chk_Filtre_CheckedChanged(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;

            txt_Arama.Text = String.Empty;

            CheckButton chkbtn = (CheckButton)sender;

            if (chk_Filtre == null)
            {
                chk_Filtre = chkbtn;
            }

            if (chk_Filtre == chk_OdaNo)
            {
                chk_OdaNo.Checked = true;
                chk_KartNo.Checked = false;
                chk_Ad.Checked = false;
                chk_Soyad.Checked = false;
            }
            if (chk_Filtre == chk_KartNo)
            {
                chk_OdaNo.Checked = false;
                chk_KartNo.Checked = true;
                chk_Ad.Checked = false;
                chk_Soyad.Checked = false;
            }
            if (chk_Filtre == chk_Ad)
            {
                chk_OdaNo.Checked = false;
                chk_KartNo.Checked = false;
                chk_Ad.Checked = true;
                chk_Soyad.Checked = false;
            }
            if (chk_Filtre == chk_Soyad)
            {
                chk_OdaNo.Checked = false;
                chk_KartNo.Checked = false;
                chk_Ad.Checked = false;
                chk_Soyad.Checked = true;
            }
            chk_Filtre = null;
            txt_Arama.Focus();
        }

        private void Back_Space_Click(object sender, EventArgs e)
        {
            if (txt_Arama.Text.Length > 0)
            {
                txt_Arama.Text = txt_Arama.Text.Substring(0, txt_Arama.Text.Length - 1);
            }
        }

        private void txt_Arama_TextChanged(object sender, EventArgs e)
        {
            if (Departman.Kodlar_AndPos_NFC == true)
            {
                string Kart = txt_Arama.Text.Replace(" ", "").Replace(":", "");
                txt_Arama.Text = Kart;
            }

            if (chk_KartNo.Checked)
            {
                if (txt_Arama.Text.Length >= Param.kartnoSayisi)
                {
                    Arama_Yap();
                }
            }
            else
            {
                Arama_Yap();
            }
            
        }

        private void Arama_Yap()
        {
            if (txt_Arama.Text.Length > 0)
            {
                if (chk_OdaHesap.Checked)
                {
                    Oda_Ara();
                }
                if (chk_UyeHesap.Checked)
                {
                    Uye_Ara();
                }
                if (chk_CariHesap.Checked)
                {
                    Cari_Ara();
                }
            }
        }
        public static string MyClass = "Arama";
        public void Oda_Ara()
        {
            try
            {
                Mus_tipi = "O";

                string ozelKapatmakodu = dbtools.DegerGetir("select Pkod_Ozelkod from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '11' and Pkod_Kod = '" + KapatmaKodu + "'");


                string Rez_Kartno = "Rez_Kartno";
                string dataKart = txt_Arama.Text;

                string[] ozelKarakter = Param.Param_Kart_Yoksay.Split('#');
                if (ozelKarakter.Length > 0 && Param.Param_Kart_Yoksay != "")
                {
                    for (int i = 0; i < ozelKarakter.Length; i++)
                    {
                        Rez_Kartno = "REPLACE(" + Rez_Kartno + ",'" + ozelKarakter[i] + "','')";
                        dataKart = dataKart.Replace(ozelKarakter[i], "");
                    }
                }

                string filtre = "";
                if (chk_OdaNo.Checked)
                {
                    filtre = " and Rez_Odano like N'" + txt_Arama.Text + "%' ";
                }
                if (chk_KartNo.Checked)
                {
                    Rez_Kartno = "CardF_No"; // 04.08.2022 de değiştirildi . özhan bey istedi
                    filtre = " and " + Rez_Kartno + " like N'" + dataKart + "%' ";
                }
                if (chk_Ad.Checked)
                {
                    filtre = " and Rez_Adi_1 like N'" + txt_Arama.Text + "%' ";
                }
                if (chk_Soyad.Checked)
                {
                    filtre = " and Rez_Adi_2 like N'" + txt_Arama.Text + "%' ";
                }

                if (ozelKapatmakodu == "2" || ozelKapatmakodu == "3") //Odenmez Ikram Kontrolü
                {
                    filtre += " and ISNULL(Kodlar_Comp_muaf_10,0) = 1 ";
                }

                if (Param.Param_Extre_Cikmasin)
                {
                    filtre += " and Rez_Master_detay <> 'D' ";
                }

                if (chk_KartNo.Checked)
                {
                    filtre += " and CardF_R_I_H='I' ";

                }

                gridColumn1.FieldName = "Rez_Id";
                gridColumn2.FieldName = "Rez_Odano";
                gridColumn3.FieldName = "Rez_Adi_1";
                gridColumn4.FieldName = "Rez_Adi_2";
                gridColumn5.FieldName = "Rez_Kartno";
                gridColumn6.FieldName = "Rez_Konaklama";
                gridColumn7.FieldName = "Rez_Giris_tarihi";
                gridColumn8.FieldName = "Rez_Cikis_tarihi";
                gridColumn9.FieldName = "Kisi";
                gridColumn10.FieldName = "Ac_Adi";
                gridColumn11.FieldName = "Rez_Odeme";
                gridColumn12.FieldName = "Rez_Master_Id";


                gridColumn1.Caption = "ID";
                gridColumn2.Caption = "Oda No";
                gridColumn3.Caption = "Adı";
                gridColumn4.Caption = "Soyadı";
                gridColumn5.Caption = "Kart No";
                gridColumn6.Caption = "Konaklama";
                gridColumn7.Caption = "Giriş Tarihi";
                gridColumn8.Caption = "Çıkış tarihi";
                gridColumn9.Caption = "Kisi";
                gridColumn10.Caption = "Acenta";
                gridColumn11.Caption = "Odeme";
                gridColumn12.Caption = "Master Id";

                gridColumn11.Visible = false;
                gridColumn12.Visible = false;

                gridControl1.DataSource = null;

                gridColumn14.FieldName = "CardF_Indirim";
                gridColumn14.Caption = "CardF_Indirim";
                gridColumn14.Visible = false;

                if (Departman.Kodlar_AndPos_NFC == true) //  && User.ExtraFolio == false
                {
                    gridColumn14.Visible = true;
                    string sorgu = @"select Rez_Id,
                                    Rez_Odano,
                                    Rez_Adi_1 ,
                                    Rez_Adi_2, 
                                    CardF_No as Rez_Kartno,
                                    Rez_Konaklama,
                                    Rez_Giris_tarihi,
                                    Rez_Cikis_tarihi, 
                                    convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,
                                    Ac_Adi,
                                    Rez_Odeme, 
                                    case when Rez_Master_detay = 'D' then Rez_Master_id else Rez_Id end as Rez_Master_Id,
                                    KartF.ID as ID,
                                    Kodlar_Ad,
                                    CardF_Indirim
                                    FROM Rez WITH(NOLOCK) 
                                    left join Acenta WITH(NOLOCK) on Rez_Macenta = Acenta.Ac_Kodu 
                                    left join Kodlar WITH(NOLOCK) on Kodlar_Sinif = '10' and Rez_Odeme = Kodlar_Kod 
                                    left join KartF on  CardF_RezID = Rez_Id WHERE  Rez_R_I_H = 'I' " + filtre;


                    gridControl1.DataSource = Fronttools.SelectTable(sorgu);

                }
                else
                {
                    filtre = "";
                    if (chk_OdaNo.Checked)
                    {
                        filtre = " and Rez_Odano like N'" + txt_Arama.Text + "%' ";
                    }
                    if (chk_KartNo.Checked)
                    {
                        filtre = " and (rez.Rez_Kartno like N'" + txt_Arama.Text + "%'  or rez.Rez_Kartno11 like N'" + txt_Arama.Text + "%' or rez.Rez_Kartno12 like N'" + txt_Arama.Text + "%' or rez.Rez_Kartno13 like N'" + txt_Arama.Text + "%' )";
                    }
                    if (chk_Ad.Checked)
                    {
                        filtre = " and Rez_Adi_1 like N'" + txt_Arama.Text + "%' ";
                    }
                    if (chk_Soyad.Checked)
                    {
                        filtre = " and Rez_Adi_2 like N'" + txt_Arama.Text + "%' ";
                    }

                    string sorgu = @"select Rez_Id,
                                    Rez_Odano,
                                    Rez_Adi_1 ,
                                    Rez_Adi_2, 
                                     rez.Rez_Kartno as Rez_Kartno,
                                    Rez_Konaklama,
                                    Rez_Giris_tarihi,
                                    Rez_Cikis_tarihi, 
                                    convert(nvarchar(3),Rez_Buyuk) +'+'+ convert(nvarchar(3),Rez_Kucuk) +'+'+ convert(nvarchar(3),Rez_Free) as Kisi,
                                    Ac_Adi,
                                    Rez_Odeme, 
                                    case when Rez_Master_detay = 'D' then Rez_Master_id else Rez_Id end as Rez_Master_Id,
                                    rez.Rez_Id as ID,
                                    Kodlar_Ad  
                                    FROM Rez WITH(NOLOCK) 
                                    left join Acenta WITH(NOLOCK) on Rez_Macenta = Acenta.Ac_Kodu 
                                    left join Kodlar WITH(NOLOCK) on Kodlar_Sinif = '10' and Rez_Odeme = Kodlar_Kod where rez.Rez_R_I_H='I' " + filtre;



                    gridControl1.DataSource = Fronttools.SelectTable(sorgu);

                }

                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Oda_Ara", "", ex);
            }

        }



        private void Uye_Ara()
        {
            Mus_tipi = "U";

            string Kimlik_Kart = "Kimlik_Kart";
            string dataKart = txt_Arama.Text;

            string[] ozelKarakter = Param.Param_Kart_Yoksay.Split('#');
            if (ozelKarakter.Length > 0 && Param.Param_Kart_Yoksay != "")
            {
                for (int i = 0; i < ozelKarakter.Length; i++)
                {
                    Kimlik_Kart = "REPLACE(" + Kimlik_Kart + ",'" + ozelKarakter[i] + "','')";
                    dataKart = dataKart.Replace(ozelKarakter[i], "");
                }
            }

            string filtre = "";
            if (chk_KartNo.Checked)
            {
                filtre = " and " + Kimlik_Kart + " like '" + dataKart + "%' ";
            }
            if (chk_Ad.Checked)
            {
                filtre = " and Kimlik_Ad like '" + txt_Arama.Text + "%' ";
            }
            if (chk_Soyad.Checked)
            {
                filtre = " and Kimlik_Soyad like '" + txt_Arama.Text + "%' ";
            }

            gridColumn1.FieldName = "Kimlik_Id";
            gridColumn2.FieldName = "Kimlik_Ad";
            gridColumn3.FieldName = "Kimlik_Soyad";
            gridColumn4.FieldName = "Kimlik_Kart";
            gridColumn5.FieldName = "Kart_Turu";
            gridColumn6.FieldName = "";
            gridColumn7.FieldName = "";
            gridColumn8.FieldName = "";
            gridColumn9.FieldName = "";
            gridColumn10.FieldName = "";
            gridColumn11.FieldName = "";
            gridColumn12.FieldName = "";


            gridColumn1.Caption = "ID";
            gridColumn2.Caption = "Adı";
            gridColumn3.Caption = "Soyadı";
            gridColumn4.Caption = "Kart No";
            gridColumn5.Caption = "Uyelik Turu";
            gridColumn6.Caption = "...";
            gridColumn7.Caption = "...";
            gridColumn8.Caption = "...";
            gridColumn9.Caption = "...";
            gridColumn10.Caption = "...";
            gridColumn11.Caption = "...";
            gridColumn12.Caption = "...";

            gridColumn1.Visible = false;
            gridColumn6.Visible = false;
            gridColumn7.Visible = false;
            gridColumn8.Visible = false;
            gridColumn9.Visible = false;
            gridColumn10.Visible = false;
            gridColumn11.Visible = false;
            gridColumn12.Visible = false;

            gridControl1.DataSource = Fronttools.SelectTable("select Kimlik_Id,Kimlik_Ad,Kimlik_Soyad,Kimlik_Kart,Kart_Turu from Previl with(nolock) where Len(Kart_Turu) > 0 " + filtre);
            gridView1.BestFitColumns();
        }

        private void Cari_Ara()
        {
            string tipFilter = "";
            if (Odeme_Ozelkod == 5)
            {
                tipFilter = " and ISNULL(Cari_Tip,'C') = 'C' ";
            }
            if (Odeme_Ozelkod == 2)
            {
                tipFilter = " and ISNULL(Cari_Tip,'C') = 'O' ";
            }
            if (Odeme_Ozelkod == 3)
            {
                tipFilter = " and ISNULL(Cari_Tip,'C') = 'P' ";
            }

            Mus_tipi = "C";
            string filtre = "";
            if (chk_OdaNo.Checked)
            {
                filtre = " and Cari_Kod like '" + txt_Arama.Text + "%' ";
            }
            if (chk_KartNo.Checked)
            {
                filtre = " and Cari_Kart like '" + txt_Arama.Text + "%' ";
            }
            if (chk_Ad.Checked)
            {
                filtre = " and Cari_Ad like '" + txt_Arama.Text + "%' ";
            }
            if (chk_Soyad.Checked)
            {
                filtre = " and Cari_Soyad like '" + txt_Arama.Text + "%' ";
            }

            gridColumn1.FieldName = "Cari_Kod";
            gridColumn2.FieldName = "Cari_Ad";
            gridColumn3.FieldName = "Cari_Soyad";
            gridColumn4.FieldName = "Cari_Tel";
            gridColumn5.FieldName = "Cari_Adres1";
            gridColumn6.FieldName = "Cari_Adres2";
            gridColumn7.FieldName = "Cari_Adres3";
            gridColumn8.FieldName = "Cari_Kart";
            gridColumn9.FieldName = "Cari_indirimOran";
            gridColumn10.FieldName = "";
            gridColumn11.FieldName = "";
            gridColumn12.FieldName = "";


            gridColumn1.Caption = "Cari Kod";
            gridColumn2.Caption = "Cari Ad";
            gridColumn3.Caption = "Cari Soyad";
            gridColumn4.Caption = "Cari Tel";
            gridColumn5.Caption = "Cari Adres1";
            gridColumn6.Caption = "Cari Adres2";
            gridColumn7.Caption = "Cari Adres3";
            gridColumn8.Caption = "Cari Kart";
            gridColumn9.Caption = "Cari_indirimOran";
            gridColumn10.Caption = "...";
            gridColumn11.Caption = "...";
            gridColumn12.Caption = "...";

            gridColumn1.Visible = true;
            gridColumn2.Visible = true;
            gridColumn3.Visible = true;
            gridColumn4.Visible = true;
            gridColumn5.Visible = true;
            gridColumn6.Visible = true;
            gridColumn7.Visible = true;
            gridColumn8.Visible = true;
            gridColumn9.Visible = true;
            gridColumn10.Visible = false;
            gridColumn11.Visible = false;
            gridColumn12.Visible = false;

            gridControl1.DataSource = dbtools.SelectTable("select Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Adres1,Cari_Adres2,Cari_Adres3,Cari_Kart,isnull(Cari_indirimOran,0) as Cari_indirimOran from Pos_Cari where ISNULL(Cari_Aktif,1) = 1 " + filtre + tipFilter);
            gridView1.BestFitColumns();
        }

        public string KartID = "";

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        public decimal CardF_Indirim = 0;

        public void tamam()
        {
            try
            {
                if (Mus_tipi == null || Convert.ToString(Mus_tipi) == "" || gridView1.RowCount == 0)
                {
                    MessageBox.Show(res_man.GetString("Lütfen Hesap Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                if (Mus_tipi == "O")
                {
                    Oda_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Odano"));
                    Folio = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rez_Id"));
                    Pansiyon = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Konaklama"));
                    Odeme_Kodu = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Odeme"));
                    Master_Folio = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rez_Master_Id"));
                    Kart_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Kartno"));
                    KartID = Convert.ToString(gridView1.GetFocusedRowCellValue("ID"));

                    string query = "declare @Ind nvarchar(200) =(select Rez_Uye_depind_kodu from " + Fronttools.DB_LinkServer + "." + Fronttools.DB_Database + ".dbo.Rez with(nolock) where Rez_Id = '" + Master_Folio.ToString() + "') "
                                                           + " select Ind_Kodu,Ind_Oran "
                                                           + " from Cst_Indirim with(nolock) "
                                                           + " where Ind_Kodu in (select FieldValue from StringArray(@Ind,',')) "
                                                           + " ORDER BY Ind_Oran desc";
                    DataTable dtInd = dbtools.SelectTable(query);
                    if (dtInd != null)
                    {
                        if (dtInd.Rows.Count > 0)
                        {
                            Ind_Oran = Convert.ToDecimal(dtInd.Rows[0]["Ind_Oran"]);
                            Ind_Kodu = Convert.ToString(dtInd.Rows[0]["Ind_Kodu"]);
                        }
                        else
                        {
                            Ind_Oran = 0;
                            Ind_Kodu = String.Empty;
                        }
                    }


                    Bilgi = Oda_No + "  " + Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Adi_1")) + " " + Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Adi_2"));
                }

                if (Mus_tipi == "U")
                {
                    Oda_No = "U" + Departman.Dep_Kodu;
                    Folio = Convert.ToInt32(Fronttools.DegerGetir("select isnull((select isnull(Rez_Id,0) from Rez with(nolock) where Rez_Odano = N'" + Oda_No + "' and Rez_R_I_H = 'I'),0) "));
                    Uye_Id = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Kimlik_Id"));
                    Uye_Adsoyad = Convert.ToString(Convert.ToString(gridView1.GetFocusedRowCellValue("Kimlik_Ad")) + " " + Convert.ToString(gridView1.GetFocusedRowCellValue("Kimlik_Soyad")));
                    Uye_Adsoyad = Uye_Adsoyad.Length > 100 ? Uye_Adsoyad.Substring(0, 99) : Uye_Adsoyad;
                    Uye_Kartturu = Convert.ToString(gridView1.GetFocusedRowCellValue("Kart_Turu"));

                    Ind_Kodu = Convert.ToString(gridView1.GetFocusedRowCellValue("Kart_Turu"));
                    Ind_Oran = Convert.ToDecimal(dbtools.DegerGetir("select isnull((select Ind_Oran from Cst_Indirim with(nolock)where Ind_Kodu = N'" + Convert.ToString(gridView1.GetFocusedRowCellValue("Kart_Turu")) + "'),0)"));

                    Bilgi = "Uye : " + Uye_Adsoyad;
                }

                if (Mus_tipi == "C")
                {
                    Cari_Kod = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
                    Cari_indirimOran = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Cari_indirimOran").ToString());
                    Bilgi = "Cari : " + Cari_Kod;
                    Uye_Adsoyad = Convert.ToString(Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Ad")) + " " + Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Soyad")));

                }

                FormClose = true;
                HizliSatis = true;

                try
                {
                    CardF_Indirim = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("CardF_Indirim"));
                }
                catch (Exception ex)
                {
                    CardF_Indirim = 0;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "tamam", "", ex);
            }
        }
        public void btn_OK_Click(object sender, EventArgs e)
        {
            tamam();
        }

        private void Arama_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FormClose == false)
            {
                //e.Cancel = true;
            }
        }

        private void btn_CariEkle_Click(object sender, EventArgs e)
        {
            CariHesap cari = new CariHesap();
            cari.Odeme_Ozelkod = Odeme_Ozelkod;
            cari.xtraTabControl1.SelectedTabPageIndex = 1;
            cari.ShowDialog();
        }

        private void txt_Arama_EditValueChanged(object sender, EventArgs e)
        {
            if (Departman.Kodlar_AndPos_NFC == true)
            {
                string Kart = txt_Arama.Text.Replace(" ", "").Replace(":", "");
                txt_Arama.Text = Kart;
            }
        }
        private void txt_Arama_Enter(object sender, EventArgs e)
        {
            if (Departman.Kodlar_AndPos_NFC == true)
            {
                string Kart = txt_Arama.Text.Replace(" ", "").Replace(":", "");
                txt_Arama.Text = Kart;
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            tamam();
        }

        private void Arama_Shown(object sender, EventArgs e)
        {
            if (otomatikSatis)
            {
                chk_KartNo.Checked = true;
                txt_Arama.Text = kartnom;
                btn_OK.PerformClick();
            }

            if (Param.Tesis_Tipi==1)
            {
                chk_Ad.Checked = true;
            }
        }
    }
}