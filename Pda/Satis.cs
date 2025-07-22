using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Pda
{
    public partial class Satis : DevExpress.XtraEditors.XtraForm
    {
        CheckButton chk_Miktar = null;
        public bool Cikis = false;

        public string Masa_No;
        public string Ana_Grup;
        public string Alt_Grup;
        public string Ozel_Masa;
        public string Satis_Tip;
        public int Fisno;
        public bool Rsat_SiparisPr = false;
        public bool Rsat_AbuyerPr = false;

        // Satış Satır Ayarları
        string Aciklama = String.Empty;
        public decimal Miktar = 1;
        public string Garson;
        public int Kisi_Sayisi;

        //Direk Satış Ayarları
        string D_Mus_tipi = string.Empty;
        string D_Oda_No = string.Empty;
        int D_Folio = 0;
        string D_Pansiyon = string.Empty;
        int D_Uye_Id = 0;
        string D_Uye_Adsoyad = string.Empty;
        string D_Uye_Kartturu = string.Empty;
        string D_Cari_Kod = string.Empty;
        string D_Ind_Kodu = string.Empty;
        decimal D_Ind_Oran = 0;
        //string D_Odeme = string.Empty;



        public Satis()
        {
            InitializeComponent();
        }

        private void Satis_Load(object sender, EventArgs e)
        {
            Param.Param_Yukle();

            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;
            this.Text = "Satıs" + "   Masa:" + Masa_No;

            btn_Duzelt.Enabled = User.Pda_Miktarduzelt;
            btn_Satirsil.Enabled = User.Pda_Satirsil;
            btn_Mars.Enabled = Departman.Kodlar_Mars;

            Bilgileri_Doldur();


            //Direk_Satis();

            gridyenile();

            Urun_Yenile();

            flp_Urun.ClientSize = flp_Urun.Size;

            int siparisSayac = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis where Rsat_Fisno = '" + Fisno + "' and ISNULL(Rsat_SiparisPr,0) = 0"));
            if (siparisSayac > 0)
            {
                btn_Kapat.Enabled = false;
            }
        }

        //LookUpEdit look_EMiktar = new LookUpEdit();
        private void Bilgileri_Doldur()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Kod", typeof(string));
            dt.Columns.Add("Ad", typeof(string));

            dt.Rows.Add("Y", "Yarım");
            dt.Rows.Add("T", "Tam");
            dt.Rows.Add("B", "Bir Bucuk");
            dt.Rows.Add("D", "Double");

            look_EMiktar.Properties.DataSource = dt;
            look_EMiktar.Properties.DisplayMember = "Ad";
            look_EMiktar.Properties.ValueMember = "Kod";


            DataTable dt2 = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '15' and Pkod_Ustgrup = '" + Ana_Grup + "' and Pkod_Altgrup = '" + Alt_Grup + "'");
            chklist_Aciklama.Properties.DataSource = dt2;
            chklist_Aciklama.Properties.DisplayMember = "Pkod_Ad";
            chklist_Aciklama.Properties.ValueMember = "Pkod_Ad";


            Fisno_Al();
        }

        private void Fisno_Al()
        {
            if (Satis_Tip == "D" && Fisno == 0)
            {
                Fisno = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
            }
            else
            {
                Fisno = Convert.ToInt32(dbtools.DegerGetir("select ISNULL((select TOP 1 isnull(Rsat_Fisno,0) from Cst_Recete_Satis WITH(NOLOCK) where  Rsat_Durum = 'A' and Rsat_Masa = '" + Masa_No + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'),0)"));
                if (Fisno == 0)
                {
                    Fisno = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
                }
            }
        }



        private void Urun_Yenile()
        {
            flp_Urun.Controls.Clear();

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Cost_Recete_Liste";
            com.Parameters.AddWithValue("@Rec_Anagrup", Ana_Grup);
            com.Parameters.AddWithValue("@Rec_Altgrup", Alt_Grup);
            com.Parameters.AddWithValue("@Liste_Tipi", 2);
            com.Parameters.AddWithValue("@Urun_Filtre", Convert.ToString(txt_Filtre.EditValue));
            com.Parameters.AddWithValue("@Urun_Kodu", Convert.ToString(txt_Filtre.EditValue));
            com.Parameters.AddWithValue("@Departman", Departman.Dep_Kodu);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleButton btn_Urun = new SimpleButton();
                    btn_Urun.Size = new System.Drawing.Size(65, 30);
                    btn_Urun.TabIndex = 0;
                    btn_Urun.TabStop = false;
                    btn_Urun.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    btn_Urun.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    btn_Urun.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Urun.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    btn_Urun.Appearance.Options.UseBackColor = true;
                    btn_Urun.Margin = new Padding(1, 1, 1, 1);


                    if (Param.Calisma_Sekli == 0) btn_Urun.Text = Convert.ToString(dt.Rows[i]["Rec_Ad"]) + "\n" + Convert.ToString(dt.Rows[i]["Rec_Fiyat"]);
                    if (Param.Calisma_Sekli == 1) btn_Urun.Text = Convert.ToString(dt.Rows[i]["Rec_Ad"]) + "\n" + Convert.ToString(dt.Rows[i]["Rec_Dovifiyat"]);
                    btn_Urun.Tag = Convert.ToString(dt.Rows[i]["Rec_Genelkod"]);

                    btn_Urun.Click += new EventHandler(btn_Urun_Click);
                    flp_Urun.Controls.Add(btn_Urun);
                }
            }
            //txt_Filtre.EditValue = null;
        }

        void btn_Urun_Click(object sender, EventArgs e)
        {


            //NotUpdate();

            SimpleButton btn_Urun = (SimpleButton)sender;
            //Urun_Sat(btn_Urun.Tag.ToString());

            if (btn_Urun.Text.StartsWith("(*) "))
            {
                Alt_Recete alt = new Alt_Recete();
                alt.ustReceteKodu = btn_Urun.Tag.ToString();
                alt.ustReceteAdi = btn_Urun.Text.Split('\n')[0].ToString();
                alt.ShowDialog();
                if (Convert.ToString(alt.altReceteKodu) != "")
                {
                    Urun_Sat(alt.altReceteKodu);
                }
            }
            else
            {
                Urun_Sat(btn_Urun.Tag.ToString());
            }

            Siparis_Kontrol();
        }

        private void Siparis_Kontrol()
        {
            if (Departman.Siparis == true)
            {
                //btn_Cikis.Enabled = false;
            }
        }

        public void Urun_Sat(string Urun_Kodu)
        {
            Recete_Islem rec = new Recete_Islem();
            string Rsat_Odano = String.Empty, Rsat_Adisyon, Rsat_Cari = String.Empty, Rsat_Paketci = String.Empty, Rsat_Indkodu = String.Empty, Rsat_Garson2, Rsat_Uye_Kart_Turu = String.Empty, Rsat_Pansiyon = String.Empty, Rsat_MusTipi = String.Empty, Rsat_Uye_Ad = String.Empty, Rsat_Onbdep = String.Empty;
            int Rsat_Folio = 0, Rsat_Kisi, Rsat_Split = 0, Rsat_Uye_Id = 0;
            decimal Rsat_Indoran = 0;

            Rsat_Adisyon = String.Empty;
            Rsat_Garson2 = Garson;
            Rsat_Split = 0;
            Rsat_Folio = 0;
            Rsat_Kisi = Kisi_Sayisi;


            //Recete Detayları
            DataTable dtRecete = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 2, @Dep_Kodu = '" + Departman.Dep_Kodu + "',@Rec_Genelkod = '" + Urun_Kodu + "'");

            string Rec_Ad = Convert.ToString(dtRecete.Rows[0]["Rec_Ad"]);
            decimal Rec_Fiyat = Convert.ToDecimal(dtRecete.Rows[0]["Rec_Fiyat"]);
            decimal Rec_Kdv = rec.Kdv_Bul(Urun_Kodu); //Convert.ToDecimal(dtRecete.Rows[0]["Rec_Kdv"]);
            decimal Rec_Dovifiyat = Convert.ToDecimal(dtRecete.Rows[0]["Rec_Dovifiyat"]);
            string Rec_Dovizkodu = Convert.ToString(dtRecete.Rows[0]["Rec_Dovizkodu"]);
            Rsat_Onbdep = Convert.ToString(dtRecete.Rows[0]["Pkod_OnburoKod"]);

            decimal Rsat_Kdv, Rsat_Net, Rsat_Tutar;

            if (Param.Calisma_Sekli == 1) //Dövizli Çalışma Şekli
            {
                Rsat_Tutar = Rec_Dovifiyat * Param.Doviz_Kuru;
                Rsat_Net = (Rsat_Tutar * 100) / (100 + Rec_Kdv);
                Rsat_Kdv = Rsat_Tutar - Rsat_Net;
            }
            else        // TL Çalışma
            {
                Rsat_Tutar = Rec_Fiyat;
                Rsat_Net = (Rsat_Tutar * 100) / (100 + Rec_Kdv);
                Rsat_Kdv = Rsat_Tutar - Rsat_Net;
            }

            Rsat_Tutar = Rsat_Tutar ;// * Miktar;
            Rsat_Net = Rsat_Net ;// * Miktar;
            Rsat_Kdv = Rsat_Kdv ;// * Miktar;
            Rec_Dovifiyat = Rec_Dovifiyat ;// * Miktar;



            //Sabitlenmiş Oda bilgileri (varsa)
            if (Convert.ToString(this.Tag) == "M")
            {
                DataTable dtSatis = dbtools.SelectTable("select top 1 Rsat_Odano, Rsat_Adisyon, Rsat_Cari, Rsat_Paketci, Rsat_Garson2, "
                                            + "     Rsat_Uye_Kart_Turu, Rsat_Pansiyon, Rsat_MusTipi, Rsat_Uye_Ad, Rsat_Indkodu, isnull(Rsat_Folio,0) as Rsat_Folio, isnull(Rsat_Kisi,0) as Rsat_Kisi, "
                                            + "     isnull(Rsat_Uye_Id,0) as Rsat_Uye_Id, isnull(Rsat_Indoran,0) as Rsat_Indoran "
                                            + " FROM Cst_Recete_Satis with(nolock) where Rsat_Fisno = '" + Fisno.ToString() + "' and Rsat_Ba = 'B' order by Rsat_Indkodu desc");

                if (dtSatis.Rows.Count > 0)
                {
                    Rsat_Odano = Convert.ToString(dtSatis.Rows[0]["Rsat_Odano"]);
                    Rsat_Adisyon = Convert.ToString(dtSatis.Rows[0]["Rsat_Adisyon"]);
                    Rsat_Cari = Convert.ToString(dtSatis.Rows[0]["Rsat_Cari"]);
                    Rsat_Paketci = Convert.ToString(dtSatis.Rows[0]["Rsat_Paketci"]);
                    Rsat_Garson2 = Convert.ToString(dtSatis.Rows[0]["Rsat_Garson2"]);
                    Rsat_Uye_Kart_Turu = Convert.ToString(dtSatis.Rows[0]["Rsat_Uye_Kart_Turu"]);
                    Rsat_Pansiyon = Convert.ToString(dtSatis.Rows[0]["Rsat_Pansiyon"]);
                    Rsat_MusTipi = Convert.ToString(dtSatis.Rows[0]["Rsat_MusTipi"]);
                    Rsat_Uye_Ad = Convert.ToString(dtSatis.Rows[0]["Rsat_Uye_Ad"]);
                    Rsat_Folio = Convert.ToInt32(dtSatis.Rows[0]["Rsat_Folio"]);
                    Rsat_Kisi = Convert.ToInt32(dtSatis.Rows[0]["Rsat_Kisi"]);
                    Rsat_Uye_Id = Convert.ToInt32(dtSatis.Rows[0]["Rsat_Uye_Id"]);
                    Rsat_Indkodu = Convert.ToString(dtSatis.Rows[0]["Rsat_Indkodu"]);
                    Rsat_Indoran = Convert.ToDecimal(dtSatis.Rows[0]["Rsat_Indoran"]);
                }

            }
            //Direk Satış Bilgileri
            if (Convert.ToString(this.Tag) == "D")
            {
                Rsat_Odano = D_Oda_No;
                Rsat_Adisyon = String.Empty;
                Rsat_Cari = D_Cari_Kod;
                Rsat_Paketci = String.Empty;
                Rsat_Garson2 = Garson;
                Rsat_Uye_Kart_Turu = D_Uye_Kartturu;
                Rsat_Pansiyon = D_Pansiyon;
                Rsat_MusTipi = D_Mus_tipi;
                Rsat_Uye_Ad = D_Uye_Adsoyad;
                Rsat_Folio = D_Folio;
                Rsat_Kisi = Kisi_Sayisi;
                Rsat_Uye_Id = D_Uye_Id;
                Rsat_Indkodu = D_Ind_Kodu;
                Rsat_Indoran = D_Ind_Oran;
            }


            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Ekle";

            com.Parameters.AddWithValue("@Rsat_Fisno", Fisno);
            com.Parameters.AddWithValue("@Rsat_Tarih", Param.Tarih);
            com.Parameters.AddWithValue("@Rsat_Departman", Departman.Dep_Kodu);
            com.Parameters.AddWithValue("@Rsat_Recete", Urun_Kodu);
            com.Parameters.AddWithValue("@Rsat_Kdvoran", Rec_Kdv.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Rsat_Miktar", Miktar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Rsat_Fiyat", Rsat_Tutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Rsat_Net", Rsat_Net.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Rsat_Kdv", Rsat_Kdv.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Rsat_Tutar", Rsat_Tutar.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Rsat_Dovizkodu", Param.Doviz_Kodu);
            com.Parameters.AddWithValue("@Rsat_Doviztutar", Rec_Dovifiyat.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Rsat_Odano", Rsat_Odano);
            com.Parameters.AddWithValue("@Rsat_Folio", Rsat_Folio);
            com.Parameters.AddWithValue("@Rsat_Adisyon", Rsat_Adisyon);
            com.Parameters.AddWithValue("@Rsat_Masa", Masa_No);
            com.Parameters.AddWithValue("@Rsat_Garson", User.P_Kod);
            com.Parameters.AddWithValue("@Rsat_Kisi", Rsat_Kisi);
            com.Parameters.AddWithValue("@Rsat_Cari", Rsat_Cari);
            com.Parameters.AddWithValue("@Rsat_Split", Rsat_Split);
            com.Parameters.AddWithValue("@Rsat_Aciklama", (Aciklama + " " + txt_Aciklama.Text).TrimEnd());
            com.Parameters.AddWithValue("@Rsat_Paketci", Rsat_Paketci);
            com.Parameters.AddWithValue("@Rsat_Emiktar", Convert.ToString(look_EMiktar.EditValue));
            com.Parameters.AddWithValue("@Rsat_Garson2", Rsat_Garson2);
            com.Parameters.AddWithValue("@Rsat_Uye_Kart_Turu", Rsat_Uye_Kart_Turu);
            com.Parameters.AddWithValue("@Rsat_Pansiyon", Rsat_Pansiyon);
            com.Parameters.AddWithValue("@Rsat_MusTipi", Rsat_MusTipi);
            com.Parameters.AddWithValue("@Rsat_Uye_Id", Rsat_Uye_Id);
            com.Parameters.AddWithValue("@Rsat_Uye_Ad", Rsat_Uye_Ad);
            com.Parameters.AddWithValue("@Rsat_Indkodu", Rsat_Indkodu);
            com.Parameters.AddWithValue("@Rsat_Indoran", Rsat_Indoran.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Rsat_Onbdep", Rsat_Onbdep);
            com.Parameters.AddWithValue("@Rsat_Dovizkur", Param.Doviz_Kuru.ToString().Replace(",", "."));
            com.Parameters.AddWithValue("@Rsat_Pda", Convert.ToBoolean(true));
            com.Parameters.AddWithValue("@Rsat_SiparisPr", Rsat_SiparisPr);
            com.Parameters.AddWithValue("@Rsat_Yapma", chk_Yapma.Checked);
            com.Parameters.AddWithValue("@Rsat_AbuyerPr", Rsat_AbuyerPr);

            com.ExecuteNonQuery();
            con.Close();


            chk_Yapma.Checked = false;
            txt_Filtre.EditValue = null;
            txt_Aciklama.Text = String.Empty;
            chklist_Aciklama.SetEditValue(null);
            Aciklama = String.Empty;
            if (Departman.Siparis)
            {
                btn_Kapat.Enabled = false;
            }

            if (Ozel_Masa != String.Empty) dbtools.execcmd("update Pos_Masa set Masa_Ozel = '" + Ozel_Masa + "' where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Masa_No + "' ");

            Log.Log_KaydetUrun(Log.Log_Program.Pda, Log.Log_Bolum.Satis, Log.Log_Islem.Kaydet, Departman.Dep_Adi + " Urun:" + Urun_Kodu + "-" + Rec_Ad + " Miktar:" + Miktar.ToString() + " Tutar:" + Rsat_Tutar.ToString("N2"), Fisno.ToString(), "",Log_Recete: Urun_Kodu, Log_Urun: Rec_Ad);

            gridyenile();
            btn_M_1.Checked = true;
        }

        private void gridyenile()
        {
            look_EMiktar.EditValue = null;

            gridColumn1.FieldName = "Rec_Ad";
            gridColumn2.FieldName = "Rsat_Miktar";
            gridColumn3.FieldName = "Rsat_Emiktar";
            gridColumn4.FieldName = "Rsat_Tutar";
            gridColumn5.FieldName = "Rsat_Doviztutar";
            gridColumn6.FieldName = "Rsat_Aciklama";
            gridColumn7.FieldName = "Rsat_Recete";
            gridColumn8.FieldName = "Rsat_Ba";
            gridColumn9.FieldName = "Rsat_Id";

            if (Param.Calisma_Sekli == 1)   //Dövizli
            {
                gridColumn5.Visible = true;
                gridColumn5.VisibleIndex = 3;
            }
            else
            {
                gridColumn4.Visible = true;
                gridColumn4.VisibleIndex = 3;
            }
            gridColumn6.VisibleIndex = 4;

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Fisno);
            com.Parameters.AddWithValue("@Rapor_Tipi", 0);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl1.DataSource = dt;
            gridView1.BestFitColumns();
        }

        private void btn_Kapat_Click(object sender, EventArgs e)
        {
            //NotUpdate();
            if (Satis_Tip == "D" && gridView1.RowCount > 0)
            {
                if (Param.Tesis_Tipi == 0)
                {
                    Odeme_Tip tip = new Odeme_Tip();
                    tip.Fis_No = Convert.ToInt32(Fisno);
                    tip.ShowDialog();

                    if (Convert.ToString(tip.Satis_Tip) == "")
                    {
                        MessageBox.Show("Ödeme Tipi Seçiniz...");
                    }
                    else
                    {
                        Odeme_Al odeme = new Odeme_Al();
                        odeme.Odeme_Kodu = tip.Satis_Tip;
                        odeme.Tag = Convert.ToInt32(Fisno);
                        odeme.Masa_No = "";
                        odeme.Satis_Tip = Satis_Tip;
                        odeme.ShowDialog();
                    }
                }
                else
                {
                    Hesap hes = new Hesap();
                    hes.Tag = Fisno;
                    hes.Masa_No = "";
                    hes.ShowDialog();
                }

                Cikis = true;
                this.Close();
            }
            else
            {
                Cikis = true;
                this.Close();
            }
        }

        private void btn_Geri_Click(object sender, EventArgs e)
        {

            //NotUpdate();
            Cikis = false;
            this.Close();
        }

        private void txt_Filtre_EditValueChanged(object sender, EventArgs e)
        {
            Urun_Yenile();
        }

        private void chklist_Aciklama_EditValueChanged(object sender, EventArgs e)
        {
            Aciklama = chklist_Aciklama.Text;
        }

        private void btn_Satirsil_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")) == String.Empty)
            {
                return;
            }

            if (MessageBox.Show("Seçili Satırı Silmek İstediğinize Emin Misiniz...?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            bool Rsat_SiparisPr = Convert.ToBoolean(dbtools.DegerGetir("select ISNULL((select ISNULL(Rsat_SiparisPr,0) from Cst_Recete_Satis WHERE Rsat_Id = " + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + "),0)"));

            if (Rsat_SiparisPr && !User.G_Satirsil_Y)
            {
                MessageBox.Show("Yazdırılmış Satır Silme Yetkiniz Yoktur...!!!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            decimal Sil_Miktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
            if (Sil_Miktar > 1 && Convert.ToInt32(dbtools.DegerGetir("SELECT COUNT(*) FROM Cst_Recete_Satis WITH(NOLOCK) WHERE Rsat_Id = " + Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")) + " AND ISNULL(Rsat_AdisyonPr,0) = 0")) > 0)
            {
                Klavye1 klv = new Klavye1();
                klv.Tag = "SATIRSIL";
                klv.txt_Sayi.Text = Sil_Miktar.ToString();
                klv.ShowDialog();
                if (Sil_Miktar < klv.sayi)
                {
                    MessageBox.Show("Hatalı Giriş...");
                    return;
                }
                Sil_Miktar = klv.sayi;
            }

            if (Departman.Siparis)
            {
                FisPr fis = new FisPr();
                string sonuc = fis.IptalPr(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), Sil_Miktar);
                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc);
                }
            }

            Log.Log_Kaydet(Log.Log_Program.Pda, Log.Log_Bolum.Satir_Sil, Log.Log_Islem.Sil, "Recete : " + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Miktar : " + Sil_Miktar + " Silindi", Fisno.ToString(), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")), Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")), Sil_Miktar);

            Fis_Islem.Satir_Sil(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), Sil_Miktar);


            int satirsay = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + Fisno.ToString() + "' and Rsat_Ba = 'B' "));
            if (satirsay == 0)
            {
                dbtools.execcmd("update Pos_Masa set Masa_Durum = 0, Masa_Ozel = '' where Masa_No = '" + Masa_No.ToString() + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
            }
            gridyenile();
        }

        private void btn_Duzelt_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Ba")) == "A")
            {
                MessageBox.Show("Alacak Satırı üzerinde İşlem Yapamazsınız....", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Klavye1 klv = new Klavye1();
            klv.Tag = "MIKTARDUZELT";
            klv.txt_Sayi.EditValue = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
            klv.ShowDialog();

            decimal eskiMiktar = Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));
            Miktar = klv.sayi - Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Rsat_Miktar"));

            if (klv.sayi == 0 || Miktar == 0)
            {
                return;
            }

            Urun_Sat(Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Recete")));

            Log.Log_Kaydet(Log.Log_Program.Pda, Log.Log_Bolum.Miktar_Duzelt, Log.Log_Islem.Duzelt, Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Ad")) + " Miktarı " + Convert.ToString(eskiMiktar) + " iken " + klv.sayi.ToString() + " ile Değişti", Fisno.ToString(), Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Id")));
            Miktar = 1;
        }

        private void btn_Gonder_Click(object sender, EventArgs e)
        {
            Siparis_Gonder(false);
        }

        private void Siparis_Gonder(bool Mars)
        {
            //NotUpdate();
            FisPr pr = new FisPr();
            string sonuc = "";
            //string sonuc = pr.SiparisPr(Fisno, Mars, 0);
            if (Param.Param_YeniSiparisDkm)
            {
                sonuc = pr.newSiparisPr(Convert.ToInt32(Fisno), Mars, 0);

            }
            else
            {
                sonuc = pr.SiparisPr(Convert.ToInt32(Fisno), Mars, 0);
            }

            if (sonuc != "OK")
            {
                MessageBox.Show(sonuc);
            }
            else
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr = 1 where Rsat_Fisno = '" + Fisno.ToString() + "' ");
            }

            btn_Kapat.Enabled = true;
            btn_Kapat_Click(null, null);
        }

        private void btn_Miktar_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton btn = (CheckButton)sender;

            if (chk_Miktar == null)
            {
                chk_Miktar = btn;
            }
            btn_M_1.Checked = chk_Miktar == btn_M_1 ? true : false;
            btn_M_2.Checked = chk_Miktar == btn_M_2 ? true : false;
            btn_M_3.Checked = chk_Miktar == btn_M_3 ? true : false;
            btn_M_4.Checked = chk_Miktar == btn_M_4 ? true : false;
            btn_M_5.Checked = chk_Miktar == btn_M_5 ? true : false;
            btn_M_6.Checked = chk_Miktar == btn_M_6 ? true : false;
            btn_M_7.Checked = chk_Miktar == btn_M_7 ? true : false;
            btn_M_8.Checked = chk_Miktar == btn_M_8 ? true : false;
            btn_M_9.Checked = chk_Miktar == btn_M_9 ? true : false;
            if (chk_Miktar != null)
            {
                Miktar = Convert.ToDecimal(chk_Miktar.Text);
            }
            if (chk_Miktar == btn)
            {
                chk_Miktar = null;
            }

        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //if (gridView1.RowCount > 0)
            //{
            //    int id = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id"));
            //    txt_Aciklama.Text = Fis_Islem.SiparisNotGetir(id);
            //}
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //int eskiHandle = e.PrevFocusedRowHandle;
            //string eskiNot = txt_Aciklama.Text;

            //if (eskiHandle >= 0)
            //{
            //    Fis_Islem.SiparisNotUpdate(Convert.ToInt32(gridView1.GetRowCellValue(eskiHandle, "Rsat_Id")), Aciklama+ " " + eskiNot);
            //    txt_Aciklama.Text = "";
            //    chklist_Aciklama.SetEditValue(null);
            //    Aciklama = "";
            //}
        }

        private void NotUpdate()
        {
            if (gridView1.RowCount > 0)
            {
                Fis_Islem.SiparisNotUpdate(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")), Aciklama + " " + txt_Aciklama.Text);
                txt_Aciklama.Text = "";
                chklist_Aciklama.SetEditValue(null);
                Aciklama = "";
            }
        }

        private void btn_Mars_Click(object sender, EventArgs e)
        {


            if (Convert.ToString(this.Tag) != "D" && Fisno > 0)
            {
                //Fis_Islem.Mars_Update(Fisno, Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Id")));
                //Siparis_Gonder(true);
                //this.Close();

                Mars_SatirSec mars = new Mars_SatirSec();
                mars.Tag = Convert.ToInt32(Fisno);
                mars.ShowDialog();
                Cikis = true;
                this.Close();
            }
        }






    }
}