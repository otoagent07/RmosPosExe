using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using Pos.Class;
using Pos.Controllers;
using Pos.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.ServiceProcess;
using System.Windows.Forms;

namespace Pos
{
    public partial class MasaTakip : DevExpress.XtraEditors.XtraForm
    {
        string Masa_No = String.Empty;
        string Ozel_Masa = String.Empty;
        int Split = 0;
        string Splitad = "";
        bool Masa_Paket = false;
        string Masa_Konum = "";
        string Masa_OzelAdi = string.Empty;

        public MasaTakip()
        {
            InitializeComponent();
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void Menu()
        {
            if (!Param.Param_MasaTakipMenu)
            {
                groupControl1.Visible = true;
                bar_Menu.Visible = false;
            }
            else
            {
                groupControl1.Visible = false;
                bar_Menu.Visible = true;
            }
        }


        bool birKere = false;
        private void MasaTakip_Load(object sender, EventArgs e)
        {
            try
            {
                yaziciKapaliysaAc();
                masaDurumUpdate();
                this.BringToFront();
                Menu();
                gridyenile_Konumlar();
                MasaYenileIlkAcilis(0);
                Kapatma_Yenile();
                Bilgileri_Doldur();
                Param.Param_Yukle();

                btnHesapDokum.Enabled = User.G_Hesapdokumu;
                btnHesapDokumEski.Enabled = User.G_Hesapdokumu;




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }


        private void Bilgileri_Doldur()
        {
            barspn_Refresh.EditValue = Param.Masa_Refresh;


            if (Langs.Default.Dil == "tr-TR")
            {
                bartxt_Kul.Caption = "Kullanıcı : " + User.P_Ad + " " + User.P_Soyad;
                bartxt_Dep.Caption = "Departman : " + Departman.Dep_Adi;
            }
            else
            {
                bartxt_Kul.Caption = "User : " + User.P_Ad + " " + User.P_Soyad;
                bartxt_Dep.Caption = "Department : " + Departman.Dep_Adi;

            }





            btn_Satis.Enabled = User.M_Satis;
            btn_MasaTransfer.Enabled = User.M_Masatransfer;
            btn_OzelMasa.Enabled = User.M_Ozelmasa;
            btn_HesapBak.Enabled = User.M_Hesapkapatma;
            flp_Kapatma.Enabled = User.M_Hesapkapatma;
            btn_MalzemeTransfer.Enabled = User.M_Malzemetransfer;
            btn_Rapor.Enabled = User.R_Raporlar;

            bar_Satis.Enabled = User.M_Satis;
            bar_MasaTransfer.Enabled = User.M_Masatransfer;
            bar_OzelMasa.Enabled = User.M_Ozelmasa;
            bar_HesapBak.Enabled = User.M_Hesapkapatma;
            bar_MalzemeTransfer.Enabled = User.M_Malzemetransfer;
            bar_Rapor.Enabled = User.R_Raporlar;

            if (!User.Pos_OdaKontrol)
            {
                bar_OdaKontrol.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }


            if (!User.Pos_MasaPaketS)
            {
                bar_PaketSatis.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

            if (!User.Pos_MasaDirekS)
            {
                bar_Direksatis.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

        }




        private void Kapatma_Yenile()
        {

            DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");



            if (dt == null || dt.Rows.Count < 1)
            {
                dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43");
            }



            if (dt.Rows.Count < 8)
            {
                panelControl2.Size = new System.Drawing.Size(741, 65);
            }
            else
            {
                panelControl2.Size = new System.Drawing.Size(741, 125);
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string backcolor = Convert.ToString(dt.Rows[i]["Pkod_OdemeBtnRenk"]);
                    Color color = new Color();
                    if (backcolor != "")
                    {
                        color = System.Drawing.ColorTranslator.FromHtml(backcolor);
                    }

                    SimpleButton btn_Kapatma = new SimpleButton();
                    btn_Kapatma.Size = new System.Drawing.Size(90, 50);
                    btn_Kapatma.TabIndex = 0;
                    btn_Kapatma.TabStop = false;
                    btn_Kapatma.Font = new System.Drawing.Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    btn_Kapatma.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btn_Kapatma.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Kapatma.Appearance.Options.UseBackColor = true;
                    btn_Kapatma.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                    btn_Kapatma.Appearance.BackColor = color;

                    btn_Kapatma.Text = Convert.ToString(dt.Rows[i]["Pkod_Ad"]);
                    btn_Kapatma.Tag = Convert.ToString(dt.Rows[i]["Pkod_Kod"]);

                    btn_Kapatma.Click += new EventHandler(btn_Kapatma_Click);
                    flp_Kapatma.Controls.Add(btn_Kapatma);
                }
            }

        }


        void btn_Kapatma_Click(object sender, EventArgs e)
        {
            if (bartxt_FisNo.EditValue == null || bartxt_FisNo.EditValue.ToString() == "0") return;
            if (StatikSinif.masaMusaitmi(Masa_No) == false) return;

            if (Masa_No == String.Empty)
            {
                MessageBox.Show(res_man.GetString("Masa Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Param.Param_Masaacan_Garson && !User.M_BaskaMasa)
            {
                string garson = dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Garson from Cst_Recete_Satis where Rsat_Ba = 'B' and Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue).ToString() + "' order by Rsat_Id),'')");
                if (garson != User.P_Kod && garson != "")
                {
                    MessageBox.Show(res_man.GetString("Masayı ") + garson + " - " + User.Isim_Getir(garson) + res_man.GetString(" Açmıştır.Başkası Satış Yapamaz...!!!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            SimpleButton btn = (SimpleButton)sender;

            timer1.Enabled = false;

            string kapatmaKod = btn.Tag.ToString();

            string masaDurum = dbtools.DegerGetir("select top 1 Masa_Durum from Pos_Masa where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");


            Hesap hes = new Hesap();
            hes.ozelKod2 = 0; // sonradan eklendi rambo
            hes.Tag = bartxt_FisNo.EditValue;
            hes.Masa_No = Masa_No;
            hes.look_Kapatma.EditValue = kapatmaKod;




            hes.Split = Split;
            hes.CariKodu = dbtools.DegerGetir("select ISNULL(Rsat_Cari,'') as Rsat_Cari From Cst_Recete_Satis where Rsat_Fisno = '" + bartxt_FisNo.EditValue + "' group by Rsat_Cari");


            string fistip = dbtools.DegerGetir(" select Pkod_FisTipi from Pos_Kodlar  where Pkod_Tekoda='1' and Pkod_Kod='" + kapatmaKod + "'");
            if (fistip.Equals("P") && hes.CariKodu.Equals(""))
            {
                hes.CariKodu = dbtools.DegerGetir(" select Cari_Kod from Pos_Cari where Cari_Kod=(select Pkod_Odano from Pos_Kodlar  where Pkod_Tekoda='1' and Pkod_FisTipi='P')");

                hes.ozelKod2 = -1;

            }

            if (Param.Param_Hesap_Disable) hes.look_Kapatma.Enabled = false;
            hes.ShowDialog();



            if (masaDurum == "2")
            {
                string masaDurumYeni = dbtools.DegerGetir("select top 1 Masa_Durum from Pos_Masa where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                if (masaDurumYeni == "1")
                {
                    dbtools.execcmdR("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                }
            }

            MasaYenile(0);

            timer1.Enabled = true;
        }

        private void gridyenile_Konumlar()
        {

            DataTable dt = dbtools.SelectTable("SELECT Pkod_Konumkod, Pkod_Ad FROM Pos_Kodlar WHERE Pkod_Sinif = '14' AND Pkod_Kod = '" + Departman.Dep_Kodu + "' order by Pkod_Sira,Pkod_Konumkod ");

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Kod", typeof(string));
            dt2.Columns.Add("Ad", typeof(string));
            dt2.Rows.Add("", "Tüm Masalar");

            int index = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt2.Rows.Add(dt.Rows[i]["Pkod_Konumkod"], dt.Rows[i]["Pkod_Ad"]);

                if (Convert.ToString(dt.Rows[i]["Pkod_Konumkod"]) == Param.P_Sabitkonumkodu)
                {
                    index = i;
                }
            }
            gridControl1.DataSource = dt2;

            if (Param.P_Sabitkonum)
            {
                Masa_Konum = Param.P_Sabitkonumkodu;
                gridView1.FocusedRowHandle = index + 1;
                gridView1.ShowEditor();
            }

        }
        /*
        private void gridyenile_Konumlar()
        {

            string query = "SELECT Pkod_Konumkod, Pkod_Ad FROM Pos_Kodlar WHERE Pkod_Sinif = '14' AND Pkod_Kod = '" + Departman.Dep_Kodu + "' order by Pkod_Ad,Pkod_Sira,Pkod_Konumkod ";

            DataTable dt = dbtools.SelectTable(query);

            DataTable dt2 = new DataTable();

            dt2.Columns.Add("Kod", typeof(string));
            dt2.Columns.Add("Ad", typeof(string));

            dt2.Rows.Add("", "Tüm Masalar");

            int index = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt2.Rows.Add(dt.Rows[i]["Pkod_Konumkod"], dt.Rows[i]["Pkod_Ad"]);

                if (Convert.ToString(dt.Rows[i]["Pkod_Konumkod"]) == Param.P_Sabitkonumkodu)
                {
                    index = i;
                }
            }
            gridControl1.DataSource = dt2;

            if (Param.P_Sabitkonum)
            {
                Masa_Konum = Param.P_Sabitkonumkodu;
                gridView1.FocusedRowHandle = index + 1;
                gridView1.ShowEditor();
            }

            //gridView1.Columns["Sira"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

        }*/

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //private void ButtonName_MouseHover(object sender, EventArgs e)
        //{
        //    SimpleButton clickedButton = (SimpleButton)sender;
        //    string MasaNo = clickedButton.Tag.ToString();

        //    int Fisno = Convert.ToInt32(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 4, @Dep_Kodu = '" + Departman.Dep_Kodu + "',@Masano = '" + MasaNo + "'"));

        //    decimal Bakiye = 0;
        //    int Kuver = 0;

        //    if (Fisno != 0)
        //    {
        //        Bakiye = Convert.ToDecimal(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 21, @Fisno = '" + Fisno + "'"));

        //        Kuver = Convert.ToString(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 34, @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Fisno = '" + Fisno + "'")) == "" ? 0 : Convert.ToInt32(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 34, @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Fisno = '" + Fisno + "'"));
        //    }

        //    System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
        //    ToolTip1.AutoPopDelay = 10000;
        //    ToolTip1.ToolTipTitle = (MasaNo + " Nolu Masa Bilgileri...");
        //    ToolTip1.SetToolTip(clickedButton, "KUVER SAYISI = " + Kuver + "\n" + "BAKİYE = " + Bakiye.ToString("n2"));

        //}

        public void masaDurumUpdate()
        {
            dbtools.execcmdR(@"update Pos_Masa set Masa_Durum='1' where Masa_Id in(
select masa.Masa_Id from Pos_Masa masa
left join Cst_Recete_Satis satis on masa.Masa_No=satis.Rsat_Masa and masa.Masa_Depart=satis.Rsat_Departman
where Rsat_Durum='A' and masa.Masa_Durum<>'2' group by masa.Masa_Id
)");
        }

        public void yaziciKapaliysaAc()
        {
            try
            {
                return;// bunun için ayrı servis yazdım. buradan yavaşlatır diye kapattım
                ServiceController ser = new ServiceController();
                ser.ServiceName = "Spooler";
                if (ser.Status != ServiceControllerStatus.Running)
                {
                    string path = "YaziciStart.bat";
                    if (!File.Exists(path))
                    {
                        File.WriteAllText("YaziciStart.bat", "net start Spooler");
                    }
                    Process p = new Process();
                    ProcessStartInfo psi = new ProcessStartInfo(path);
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    psi.Verb = "runas";
                    p.StartInfo = psi;
                    p.Start();

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void MasaYenile(int Acik, string filtre = "")
        {
            try
            {
                yaziciKapaliysaAc();
                masaDurumUpdate();


                StatikSinif.masaKilitAc();

                Cursor.Current = Cursors.WaitCursor;

                barspn_Refresh.EditValue = Param.Masa_Refresh;
                Masa_No = String.Empty;
                Masa_Paket = false;
                Split = 0;
                cbtn_0.Checked = true;
                //lbl_KisiSayisi.Text = String.Empty;

                Color bos = Color.Lime, dolu = Color.OrangeRed, hesap = Color.MediumOrchid;

                Color rezMasaRengi = Color.LavenderBlush;
                string KonumFilter = String.Empty;
                if (Masa_Konum != String.Empty)
                {
                    KonumFilter = " and Masa_Konum = '" + Masa_Konum + "' ";

                    //DataTable dtRenk = dbtools.SelectTable("select ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk from Pos_Kodlar where Pkod_Sinif ='14' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Konumkod = '" + Masa_Konum + "'");
                    //bos = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtRenk.Rows[0]["Pkod_Bosrenk"]));
                    //dolu = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtRenk.Rows[0]["Pkod_Dolurenk"]));
                    //hesap = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtRenk.Rows[0]["Pkod_Hesaprenk"]));
                }

                string paketFilter = "";
                if (Param.Param_Tum_Paket && Masa_Konum == "")
                {
                    paketFilter = " and Masa_Paket = 0 ";
                }

                string siralama = " order by ISNULL(Pkod_Sira,-1), Masa_No ";

                if (Param.Param_OzelMasaSiralama == true)
                {
                    siralama = " order by Masa_Ozel Desc,ISNULL(Pkod_Sira,-1), Masa_No ";
                }

                string Masalar = "";
                if (Acik == 1)
                {
                    Masalar = " and Masa_Durum <> 0 ";
                }

                string toplamTutarQuery = @" 
    (select sum(Rsat_Tutar) from (select Rsat_Id,Rsat_Recete,
	case when ISNULL(Rsat_Zayi,0) = 0 then '' else '<u>ZAYI</u> ' end + '<b>'+Rec_Ad+ ' ' + ISNULL(Rsat_Joker,'') +'</b><br><i>' + ISNULL(Rsat_Aciklama,'') + '</i>' as Rec_Ad,
	Rec_Ad as Rec_Ad2,Rsat_Miktar,
		case when ISNULL(Rsat_Emiktar,'T') = 'T' then '' else Rsat_Emiktar end as Rsat_Emiktar,
			Rsat_Tutar,Rsat_Doviztutar,ISNULL(Rsat_Aciklama, '') as Rsat_Aciklama  ,
			Rsat_Ba,
			Rsat_Not,Rsat_Adisyon,ISNULL(Rsat_Split,0) as Rsat_Split,
			case when ISNULL(Rsat_SiparisPr,0) = 1 then 'true' else 'false' end as Rsat_SiparisPr,
			ISNULL(Rsat_Zayi,0) as Rsat_Zayi,Rsat_Acilis,Po.P_Ad as Rsat_Garson,
			ISNULL(Rec_Miktar_Gr,0) as Rec_Miktar_Gr,
			Rsat_Dovizkodu,
			ISNULL(Rsat_SiparisPr,0) as Rsat_SiparisPr2, 
			ISNULL(Rsat_Mars,0) as Rsat_Mars,
			ISNULL(Rsat_SiraAciklama,'') as Rsat_SiraAciklama
			,ISNULL(Rsat_Kisi ,0) as Rsat_Kisi,ISNULL(Rec_Fiyat,0) as Rec_Fiyat
		from Cst_Recete_Satis satis
			left join Cst_Recete on Rsat_Recete = Rec_Genelkod
			left join Rmosmuh.dbo.Pos_User as po on po.P_Kod = Rsat_Garson
		where satis.Rsat_Fisno = satis3.Rsat_Fisno and satis.Rsat_Ba = 'B' and satis.Rsat_Durum = 'A' 
			--and (ISNULL(Rsat_Split,0) = @Split or @Split = 0)
		union all
		
		select MIN(Rsat_Id),'',Pkod_Ad,Pkod_Ad,0,'',
		sum(Rsat_Tutar) * -1,sum(Rsat_Doviztutar) * -1,'',min(Rsat_Ba),
		'','',0,'false',0,Null,null,0,null,0,0,'',0,0
		from Cst_Recete_Satis satis2
		left join Pos_Kodlar on Rsat_Kapatma = Pkod_Kod and Pkod_Sinif = '11'
		where satis2.Rsat_Fisno = satis3.Rsat_Fisno and Rsat_Ba = 'A' and Rsat_Durum = 'A' 
			--and (ISNULL(Rsat_Split,0) = @Split or @Split = 0) 
		group by Pkod_Ad
		) as tablo)
		as Rsat_Tutar ,";


                if (Param.Param_Masa_Garson)
                {
                    toplamTutarQuery = "'0' as Rsat_Tutar,";
                }

                string query = "select Rsat_Fisno, " + toplamTutarQuery + " Masa_No, ISNULL(Masa_Ozel,'') as Masa_Ozel , "
                            + " MIN(ISNULL(Rsat_MusTipi,'')) as Rsat_MusTipi,ISNULL(Masa_Durum,0) as Masa_Durum,"
                            + " MAX(CASE WHEN ISNULL(Rsat_Hesap_Kilit,0)= 1 THEN 1 ELSE 0 END) as Rsat_Hesap_Kilit,"
                            + " Masa_Ad,"
                            + " isnull(Masa_Paket,0) as Masa_Paket, "
                            + " case when Masa_Durum <> 0 then MIN(ISNULL(Kasiyer.P_Ad,'')) + ' ' +  MIN(ISNULL(Kasiyer.P_Soyad,'')) else '' end as Garson, "
                            + " case when Masa_Durum <> 0 then MIN(ISNULL(Garson.P_Ad,'')) + ' ' +  MIN(ISNULL(Garson.P_Soyad,'')) else '' end as Garson2, "
                            + " COUNT(Rez_Id) as RezSayisi, "
                            + " ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk, (Max(Pos_Rez.Rez_Adi) + ' ' +  Max(Pos_Rez.Rez_Soyadi)) as RezMasaAdi "
                            + " from Pos_Masa WITH(NOLOCK) "
                            + " left join Cst_Recete_Satis as satis3 WITH(NOLOCK) on Masa_No = Rsat_Masa and Rsat_Durum = 'A'  and Rsat_Departman = Masa_Depart  "
                            //+ "     left join Rmosmuh.dbo.Pos_User as Kasiyer on Rsat_Garson = Kasiyer.P_Kod "
                            + " left join Rmosmuh.dbo.Pos_User as Kasiyer on Kasiyer.P_Kod = (Select Top(1) Rsat_Garson From Cst_Recete_Satis as s Where Rsat_Durum = 'A' and s.Rsat_Departman = '" + Departman.Dep_Kodu + "' and s.Rsat_Masa = Masa_No and s.Rsat_Fisno = satis3.Rsat_Fisno order by 1 desc ) "
                            + " left join Rmosmuh.dbo.Pos_User as Garson on Rsat_Garson2 = Garson.P_Kod "
                            + " left join Pos_Rez on Rez_Dep = '" + Departman.Dep_Kodu + "' and Rez_Masano = Masa_No and Rez_Tarih = '" + Param.Tarih + "' "
                            + " left join Pos_Kodlar as renk on Masa_Konum = Pkod_Konumkod and Pkod_Kod = Masa_Depart and Pkod_Sinif ='14' "
                            + " where Masa_Depart = '" + Departman.Dep_Kodu + "' and (ISNULL(Masa_Parcali,0)<>1 or Masa_Durum=1) and ISNULL(Masa_Hayali,0) = 0  " + KonumFilter + paketFilter + Masalar
                            + " group by Rsat_Fisno,Masa_No,Masa_Ad,Masa_Ozel,Masa_Durum,Masa_Paket,Pkod_Bosrenk ,Pkod_Dolurenk, Pkod_Hesaprenk, Pkod_Sira "
                            + siralama;

                if (filtre != "")
                {
                    query = "select Rsat_Fisno, " + toplamTutarQuery + " Masa_No, ISNULL(Masa_Ozel,'') as Masa_Ozel , "
                                 + " MIN(ISNULL(Rsat_MusTipi,'')) as Rsat_MusTipi,ISNULL(Masa_Durum,0) as Masa_Durum,"
                                 + " MAX(CASE WHEN ISNULL(Rsat_Hesap_Kilit,0)= 1 THEN 1 ELSE 0 END) as Rsat_Hesap_Kilit,"
                                 + " Masa_Ad,"
                                 + " isnull(Masa_Paket,0) as Masa_Paket, "
                                 + " case when Masa_Durum <> 0 then MIN(ISNULL(Kasiyer.P_Ad,'')) + ' ' +  MIN(ISNULL(Kasiyer.P_Soyad,'')) else '' end as Garson, "
                                 + " case when Masa_Durum <> 0 then MIN(ISNULL(Garson.P_Ad,'')) + ' ' +  MIN(ISNULL(Garson.P_Soyad,'')) else '' end as Garson2, "
                                 + " COUNT(Rez_Id) as RezSayisi, "
                                 + " ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk, (Max(Pos_Rez.Rez_Adi) + ' ' +  Max(Pos_Rez.Rez_Soyadi)) as RezMasaAdi "
                                 + " from Pos_Masa WITH(NOLOCK) "
                                 + " left join Cst_Recete_Satis as satis3 WITH(NOLOCK) on Masa_No = Rsat_Masa and Rsat_Durum = 'A'  and Rsat_Departman = Masa_Depart  "
                                 //+ "     left join Rmosmuh.dbo.Pos_User as Kasiyer on Rsat_Garson = Kasiyer.P_Kod "
                                 + " left join Rmosmuh.dbo.Pos_User as Kasiyer on Kasiyer.P_Kod = (Select Top(1) Rsat_Garson From Cst_Recete_Satis as s Where Rsat_Durum = 'A' and s.Rsat_Departman = '" + Departman.Dep_Kodu + "' and s.Rsat_Masa = Masa_No and s.Rsat_Fisno = satis3.Rsat_Fisno order by 1 desc ) "
                                 + " left join Rmosmuh.dbo.Pos_User as Garson on Rsat_Garson2 = Garson.P_Kod "
                                 + " left join Pos_Rez on Rez_Dep = '" + Departman.Dep_Kodu + "' and Rez_Masano = Masa_No and Rez_Tarih = '" + Param.Tarih + "' "
                                 + " left join Pos_Kodlar as renk on Masa_Konum = Pkod_Konumkod and Pkod_Kod = Masa_Depart and Pkod_Sinif ='14' "
                                 + " where Masa_Depart = '" + Departman.Dep_Kodu + "' " +
                                 " " + filtre +
                                 " and (ISNULL(Masa_Parcali,0)<>1 or Masa_Durum=1) and ISNULL(Masa_Hayali,0) = 0  " + KonumFilter + paketFilter + Masalar
                                 + " group by Rsat_Fisno,Masa_No,Masa_Ad,Masa_Ozel,Masa_Durum,Masa_Paket,Pkod_Bosrenk ,Pkod_Dolurenk, Pkod_Hesaprenk, Pkod_Sira "
                                 + siralama;
                }

                if (Param.Calisma_Sekli == 1)
                {
                    query = query.Replace("as Rsat_Tutar", "as Rsat_Doviztutar");
                    query = query.Replace("sum(Rsat_Tutar)", "sum(Rsat_Doviztutar)");
                }

                DataTable dtMasa = dbtools.SelectTable(query);


                if (dtMasa.Rows.Count < 1)
                {
                    //MessageBox.Show("Bu Departmana Ait Masa Tanımı Yapılmamış", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string sonMasa = "";
                if (Param.Param_Sonmasa)
                {
                    sonMasa = dbtools.DegerGetir("select top 1 Rsat_Masa from Cst_Recete_Satis where Rsat_Tarih = '" + Param.Tarih + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "' and Rsat_Durum = 'A' order by Rsat_Satissaat desc");
                }

                string masaSize = Param.Param_Masa_Size == "" ? "90;45" : Param.Param_Masa_Size;
                Size s = new Size(Convert.ToInt32(masaSize.Split(';')[0]), Convert.ToInt32(masaSize.Split(';')[1]));

                //for (int i = 0; i < dtMasa.Rows.Count; i++)
                //{



                if (dtMasa.Rows.Count != flp_Masa.Controls.Count)
                {
                    MasaYenileIlkAcilis(Acik, filtre);
                    return;
                }

                int i = -1;
                foreach (SimpleButton btnMasa in flp_Masa.Controls)
                {
                    i++;

                    bos = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtMasa.Rows[i]["Pkod_Bosrenk"]));
                    dolu = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtMasa.Rows[i]["Pkod_Dolurenk"]));
                    hesap = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtMasa.Rows[i]["Pkod_Hesaprenk"]));
                    bool kilit = Convert.ToBoolean(dtMasa.Rows[i]["Rsat_Hesap_Kilit"]);

                    btnMasa.Appearance.Options.UseBackColor = false;

                    btnMasa.Tag = Convert.ToString(dtMasa.Rows[i]["Masa_No"]);

                    //Boş Masa
                    if (Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "0")
                    {
                        btnMasa.Appearance.BackColor = bos;
                        btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                    }

                    //Dolu Masa
                    if (Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "1")
                    {
                        btnMasa.Appearance.BackColor = dolu;
                        btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                    }

                    if (Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) != String.Empty)
                    {
                        btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                        //if (Param.Pos_HesapDkmRenk == false)
                        //{
                        btnMasa.Appearance.BackColor = Param.Param_OzelMasaRengi;
                        //}
                        //else
                        //{
                        //    btnMasa.Appearance.BackColor = hesap;
                        //}

                    }



                    if (!kilit && Convert.ToInt32(dtMasa.Rows[i]["Masa_Durum"]) == 2)
                    {
                        if (Param.Pos_HesapDkmRenk == false)
                        {
                            btnMasa.Appearance.BackColor = dolu;
                            btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) == "" ? Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]) : Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                        }
                        else
                        {
                            btnMasa.Appearance.BackColor = hesap;
                            btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) == "" ? Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]) : Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                        }
                    }

                    //Beklemede Masalar
                    if (kilit && Convert.ToInt32(dtMasa.Rows[i]["Masa_Durum"]) == 2)
                    {
                        btnMasa.Appearance.BackColor = hesap;
                        btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) == "" ? Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]) : Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                    }

                    if (Convert.ToInt32(dtMasa.Rows[i]["RezSayisi"]) > 0)
                    {
                        btnMasa.Appearance.BackColor = Param.Param_RezMasaRengi;
                        btnMasa.Text += "\nREZERVE(" + Convert.ToInt32(dtMasa.Rows[i]["RezSayisi"]) + ")\n" + Convert.ToString(dtMasa.Rows[i]["RezMasaAdi"]);
                    }

                    //Pda Acılan Masalar
                    //if (Convert.ToBoolean(dtMasa.Rows[i]["Rsat_Pda"]) == true)
                    //{
                    //    btnMasa.Appearance.ForeColor = Color.AliceBlue;
                    //    btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                    //}

                    //Özel Masaların Adı - Rengi 


                    //Garson Adı Eklenmesi Masaya

                    if (Param.Param_Masa_Garson && Convert.ToString(dtMasa.Rows[i]["Garson"]) != "" && !Departman.Garson_Sor)
                    {
                        btnMasa.Text += "\n" + Convert.ToString(dtMasa.Rows[i]["Garson"]);
                    }

                    if (Convert.ToString(dtMasa.Rows[i]["Garson2"]) != "" && Departman.Garson_Sor)
                    {
                        btnMasa.Text += "\n" + Convert.ToString(dtMasa.Rows[i]["Garson2"]);
                    }

                    string TL = "";
                    if (Param.Param_Masa_Garson == false)
                    {
                        if (Param.Calisma_Sekli == 1)
                        {
                            TL = dtMasa.Rows[i]["Rsat_Doviztutar"].ToString().Trim();
                            if (!TL.Equals(""))
                            {
                                TL = TL + " €"; // €  £
                            }
                        }
                        else
                        {
                            TL = dtMasa.Rows[i]["Rsat_Tutar"].ToString().Trim();
                            if (!TL.Equals(""))
                            {
                                TL = TL + " ₺";
                            }
                        }

                        btnMasa.Text += "\n" + TL;

                    }


                    //Paket Masaları
                    if (Convert.ToBoolean(dtMasa.Rows[i]["Masa_Paket"]) && Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "0")
                    {
                        btnMasa.Appearance.BackColor = Color.Gainsboro;
                    }

                    if (sonMasa != "" && sonMasa == Convert.ToString(dtMasa.Rows[i]["Masa_No"]))
                    {
                        btnMasa.Appearance.BackColor = Param.Param_Sonmasa_Renk;
                    }


                    if (Convert.ToInt32(dtMasa.Rows[i]["Masa_Durum"]) == 2) // !! burada kaldım
                    {
                        btnMasa.Appearance.BackColor = hesap;
                    }


                    btnMasa.Appearance.BorderColor = btnMasa.Appearance.BackColor;
                    //btnMasa.Click += new EventHandler(btnMasa_Click);
                    //btnMasa.DoubleClick += new EventHandler(btnMasa_DoubleClick);

                    //flp_Masa.Controls.Add(btnMasa);
                    //btnMasa.MouseHover += ButtonName_MouseHover;

                    // !!!!!

                }

                //Split_Ayarla();
                gridControl2.DataSource = null;
                bartxt_FisNo.EditValue = 0;

                DataTable dtBilgi = dbtools.SelectTable("select COUNT(Masa_Durum) as Sayi,MIN(Masa_Durum) as Durum from Pos_Masa WITH(NOLOCK) where Masa_Depart = '" + Departman.Dep_Kodu + "'  group by Masa_Durum order by Masa_Durum");
                bartxt_BosMasa.EditValue = Convert.ToString(dtBilgi.Rows[0]["Sayi"]);
                bartxt_DoluMasa.EditValue = dtBilgi.Rows.Count > 1 ? Convert.ToString(dtBilgi.Rows[1]["Sayi"]) : "0";

                Main.a.Listele(0);

            }
            catch (Exception ex)
            {
                // RHMesaj.MyMessageError(MyClass, "MasaYenile", "", ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void MasaYenileIlkAcilis(int Acik, string filtre = "") // orjinali hiç değişmedi
        {
            try
            {
                StatikSinif.masaKilitAc();

                Cursor.Current = Cursors.WaitCursor;

                flp_Masa.Controls.Clear();
                barspn_Refresh.EditValue = Param.Masa_Refresh;
                Masa_No = String.Empty;
                Masa_Paket = false;
                Split = 0;
                cbtn_0.Checked = true;
                //lbl_KisiSayisi.Text = String.Empty;

                Color bos = Color.Lime, dolu = Color.OrangeRed, hesap = Color.MediumOrchid;

                Color rezMasaRengi = Color.LavenderBlush;
                string KonumFilter = String.Empty;
                if (Masa_Konum != String.Empty)
                {
                    KonumFilter = " and Masa_Konum = '" + Masa_Konum + "' ";

                    //DataTable dtRenk = dbtools.SelectTable("select ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk from Pos_Kodlar where Pkod_Sinif ='14' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Konumkod = '" + Masa_Konum + "'");
                    //bos = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtRenk.Rows[0]["Pkod_Bosrenk"]));
                    //dolu = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtRenk.Rows[0]["Pkod_Dolurenk"]));
                    //hesap = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtRenk.Rows[0]["Pkod_Hesaprenk"]));
                }

                string paketFilter = "";
                if (Param.Param_Tum_Paket && Masa_Konum == "")
                {
                    paketFilter = " and Masa_Paket = 0 ";
                }

                string siralama = " order by ISNULL(Pkod_Sira,-1), Masa_No ";

                if (Param.Param_OzelMasaSiralama == true)
                {
                    siralama = " order by Masa_Ozel Desc,ISNULL(Pkod_Sira,-1), Masa_No ";
                }

                string Masalar = "";
                if (Acik == 1)
                {
                    Masalar = " and Masa_Durum <> 0 ";
                }

                string toplamTutarQuery = @" 
    (select sum(Rsat_Tutar) from (select Rsat_Id,Rsat_Recete,
	case when ISNULL(Rsat_Zayi,0) = 0 then '' else '<u>ZAYI</u> ' end + '<b>'+Rec_Ad+ ' ' + ISNULL(Rsat_Joker,'') +'</b><br><i>' + ISNULL(Rsat_Aciklama,'') + '</i>' as Rec_Ad,
	Rec_Ad as Rec_Ad2,Rsat_Miktar,
		case when ISNULL(Rsat_Emiktar,'T') = 'T' then '' else Rsat_Emiktar end as Rsat_Emiktar,
			Rsat_Tutar,Rsat_Doviztutar,ISNULL(Rsat_Aciklama, '') as Rsat_Aciklama  ,
			Rsat_Ba,
			Rsat_Not,Rsat_Adisyon,ISNULL(Rsat_Split,0) as Rsat_Split,
			case when ISNULL(Rsat_SiparisPr,0) = 1 then 'true' else 'false' end as Rsat_SiparisPr,
			ISNULL(Rsat_Zayi,0) as Rsat_Zayi,Rsat_Acilis,Po.P_Ad as Rsat_Garson,
			ISNULL(Rec_Miktar_Gr,0) as Rec_Miktar_Gr,
			Rsat_Dovizkodu,
			ISNULL(Rsat_SiparisPr,0) as Rsat_SiparisPr2, 
			ISNULL(Rsat_Mars,0) as Rsat_Mars,
			ISNULL(Rsat_SiraAciklama,'') as Rsat_SiraAciklama
			,ISNULL(Rsat_Kisi ,0) as Rsat_Kisi,ISNULL(Rec_Fiyat,0) as Rec_Fiyat
		from Cst_Recete_Satis satis
			left join Cst_Recete on Rsat_Recete = Rec_Genelkod
			left join Rmosmuh.dbo.Pos_User as po on po.P_Kod = Rsat_Garson
		where   satis.Rsat_Fisno = satis3.Rsat_Fisno and satis.Rsat_Ba = 'B' and satis.Rsat_Durum = 'A' 
			--and (ISNULL(Rsat_Split,0) = @Split or @Split = 0)
		union all
		
		select MIN(Rsat_Id),'',Pkod_Ad,Pkod_Ad,0,'',
		sum(Rsat_Tutar) * -1,sum(Rsat_Doviztutar) * -1,'',min(Rsat_Ba),
		'','',0,'false',0,Null,null,0,null,0,0,'',0,0
		from Cst_Recete_Satis satis2
		left join Pos_Kodlar on Rsat_Kapatma = Pkod_Kod and Pkod_Sinif = '11'
		where satis2.Rsat_Fisno = satis3.Rsat_Fisno and Rsat_Ba = 'A' and Rsat_Durum = 'A' 
			--and (ISNULL(Rsat_Split,0) = @Split or @Split = 0) 
		group by Pkod_Ad
		) as tablo)
		as Rsat_Tutar ,";


                if (Param.Param_Masa_Garson)
                {
                    toplamTutarQuery = "'0' as Rsat_Tutar,";
                }

                string query = "select Rsat_Fisno, " + toplamTutarQuery + " Masa_No, ISNULL(Masa_Ozel,'') as Masa_Ozel , "
                            + " MIN(ISNULL(Rsat_MusTipi,'')) as Rsat_MusTipi,ISNULL(Masa_Durum,0) as Masa_Durum,"
                            + " MAX(CASE WHEN ISNULL(Rsat_Hesap_Kilit,0)= 1 THEN 1 ELSE 0 END) as Rsat_Hesap_Kilit,"
                            + " Masa_Ad,"
                            + " isnull(Masa_Paket,0) as Masa_Paket, "
                            + " case when Masa_Durum <> 0 then MIN(ISNULL(Kasiyer.P_Ad,'')) + ' ' +  MIN(ISNULL(Kasiyer.P_Soyad,'')) else '' end as Garson, "
                            + " case when Masa_Durum <> 0 then MIN(ISNULL(Garson.P_Ad,'')) + ' ' +  MIN(ISNULL(Garson.P_Soyad,'')) else '' end as Garson2, "
                            + " COUNT(Rez_Id) as RezSayisi, "
                            + " ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk, (Max(Pos_Rez.Rez_Adi) + ' ' +  Max(Pos_Rez.Rez_Soyadi)) as RezMasaAdi "
                            + " from Pos_Masa WITH(NOLOCK) "
                            + " left join Cst_Recete_Satis as satis3 WITH(NOLOCK) on Masa_No = Rsat_Masa and Rsat_Durum = 'A'  and Rsat_Departman = Masa_Depart  "
                            //+ "     left join Rmosmuh.dbo.Pos_User as Kasiyer on Rsat_Garson = Kasiyer.P_Kod "
                            + " left join Rmosmuh.dbo.Pos_User as Kasiyer on Kasiyer.P_Kod = (Select Top(1) Rsat_Garson From Cst_Recete_Satis as s Where Rsat_Durum = 'A' and s.Rsat_Departman = '" + Departman.Dep_Kodu + "' and s.Rsat_Masa = Masa_No and s.Rsat_Fisno = satis3.Rsat_Fisno order by 1 desc ) "
                            + " left join Rmosmuh.dbo.Pos_User as Garson on Rsat_Garson2 = Garson.P_Kod "
                            + " left join Pos_Rez on Rez_Dep = '" + Departman.Dep_Kodu + "' and Rez_Masano = Masa_No and Rez_Tarih = '" + Param.Tarih.ToString("yyyy-MM-dd") + "' "
                            + " left join Pos_Kodlar as renk on Masa_Konum = Pkod_Konumkod and Pkod_Kod = Masa_Depart and Pkod_Sinif ='14' "
                            + " where (ISNULL(Masa_Parcali,0)<>1 or Masa_Durum=1) and Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Hayali,0) = 0  " + KonumFilter + paketFilter + Masalar
                            + " group by Rsat_Fisno,Masa_No,Masa_Ad,Masa_Ozel,Masa_Durum,Masa_Paket,Pkod_Bosrenk ,Pkod_Dolurenk, Pkod_Hesaprenk, Pkod_Sira "
                            + siralama;

                if (filtre != "")
                {
                    query = "select Rsat_Fisno, " + toplamTutarQuery + " Masa_No, ISNULL(Masa_Ozel,'') as Masa_Ozel , "
                                 + " MIN(ISNULL(Rsat_MusTipi,'')) as Rsat_MusTipi,ISNULL(Masa_Durum,0) as Masa_Durum,"
                                 + " MAX(CASE WHEN ISNULL(Rsat_Hesap_Kilit,0)= 1 THEN 1 ELSE 0 END) as Rsat_Hesap_Kilit,"
                                 + " Masa_Ad,"
                                 + " isnull(Masa_Paket,0) as Masa_Paket, "
                                 + " case when Masa_Durum <> 0 then MIN(ISNULL(Kasiyer.P_Ad,'')) + ' ' +  MIN(ISNULL(Kasiyer.P_Soyad,'')) else '' end as Garson, "
                                 + " case when Masa_Durum <> 0 then MIN(ISNULL(Garson.P_Ad,'')) + ' ' +  MIN(ISNULL(Garson.P_Soyad,'')) else '' end as Garson2, "
                                 + " COUNT(Rez_Id) as RezSayisi, "
                                 + " ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk, (Max(Pos_Rez.Rez_Adi) + ' ' +  Max(Pos_Rez.Rez_Soyadi)) as RezMasaAdi "
                                 + " from Pos_Masa WITH(NOLOCK) "
                                 + " left join Cst_Recete_Satis as satis3 WITH(NOLOCK) on Masa_No = Rsat_Masa and Rsat_Durum = 'A'  and Rsat_Departman = Masa_Depart  "
                                 //+ "     left join Rmosmuh.dbo.Pos_User as Kasiyer on Rsat_Garson = Kasiyer.P_Kod "
                                 + " left join Rmosmuh.dbo.Pos_User as Kasiyer on Kasiyer.P_Kod = (Select Top(1) Rsat_Garson From Cst_Recete_Satis as s Where Rsat_Durum = 'A' and s.Rsat_Departman = '" + Departman.Dep_Kodu + "' and s.Rsat_Masa = Masa_No and s.Rsat_Fisno = satis3.Rsat_Fisno order by 1 desc ) "
                                 + " left join Rmosmuh.dbo.Pos_User as Garson on Rsat_Garson2 = Garson.P_Kod "
                                 + " left join Pos_Rez on Rez_Dep = '" + Departman.Dep_Kodu + "' and Rez_Masano = Masa_No and Rez_Tarih = '" + Param.Tarih.ToString("yyyy-MM-dd") + "' "
                                 + " left join Pos_Kodlar as renk on Masa_Konum = Pkod_Konumkod and Pkod_Kod = Masa_Depart and Pkod_Sinif ='14' "
                                 + " where (ISNULL(Masa_Parcali,0)<>1 or Masa_Durum=1) and Masa_Depart = '" + Departman.Dep_Kodu + "' " +
                                 " " + filtre +
                                 " and ISNULL(Masa_Hayali,0) = 0  " + KonumFilter + paketFilter + Masalar
                                 + " group by Rsat_Fisno,Masa_No,Masa_Ad,Masa_Ozel,Masa_Durum,Masa_Paket,Pkod_Bosrenk ,Pkod_Dolurenk, Pkod_Hesaprenk, Pkod_Sira "
                                 + siralama;
                }


                if (Param.Calisma_Sekli == 1)
                {
                    query = query.Replace("as Rsat_Tutar", "as Rsat_Doviztutar");
                    query = query.Replace("sum(Rsat_Tutar)", "sum(Rsat_Doviztutar)");
                }
                DataTable dtMasa = dbtools.SelectTableR(query);


                if (dtMasa.Rows.Count < 1)
                {
                    //MessageBox.Show("Bu Departmana Ait Masa Tanımı Yapılmamış", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string sonMasa = "";
                if (Param.Param_Sonmasa)
                {
                    sonMasa = dbtools.DegerGetir("select top 1 Rsat_Masa from Cst_Recete_Satis where Rsat_Tarih = '" + Param.Tarih + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "' and Rsat_Durum = 'A' order by Rsat_Satissaat desc");
                }

                string masaSize = Param.Param_Masa_Size == "" ? "90;45" : Param.Param_Masa_Size;
                Size s = new Size(Convert.ToInt32(masaSize.Split(';')[0]), Convert.ToInt32(masaSize.Split(';')[1]));

                for (int i = 0; i < dtMasa.Rows.Count; i++)
                {
                    bos = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtMasa.Rows[i]["Pkod_Bosrenk"]));
                    dolu = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtMasa.Rows[i]["Pkod_Dolurenk"]));
                    hesap = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtMasa.Rows[i]["Pkod_Hesaprenk"]));
                    bool kilit = Convert.ToBoolean(dtMasa.Rows[i]["Rsat_Hesap_Kilit"]);



                    SimpleButton btnMasa = new SimpleButton();
                    btnMasa.Size = s;
                    btnMasa.TabIndex = 0;
                    btnMasa.TabStop = false;
                    btnMasa.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                    btnMasa.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btnMasa.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                    btnMasa.Appearance.Options.UseBackColor = false;

                    btnMasa.Tag = Convert.ToString(dtMasa.Rows[i]["Masa_No"]);



                    //Boş Masa
                    if (Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "0")
                    {
                        btnMasa.Appearance.BackColor = bos;
                        btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                    }

                    //Dolu Masa
                    if (Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "1")
                    {
                        btnMasa.Appearance.BackColor = dolu;
                        btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                    }

                    if (Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) != String.Empty)
                    {
                        btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                        //if (Param.Pos_HesapDkmRenk == false)
                        //{
                        btnMasa.Appearance.BackColor = Param.Param_OzelMasaRengi;
                        //}
                        //else
                        //{
                        //    btnMasa.Appearance.BackColor = hesap;
                        //}

                    }



                    if (!kilit && Convert.ToInt32(dtMasa.Rows[i]["Masa_Durum"]) == 2)
                    {
                        if (Param.Pos_HesapDkmRenk == false)
                        {
                            btnMasa.Appearance.BackColor = dolu;
                            btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) == "" ? Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]) : Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                        }
                        else
                        {
                            btnMasa.Appearance.BackColor = hesap;
                            btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) == "" ? Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]) : Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                        }
                    }

                    //Beklemede Masalar
                    if (kilit && Convert.ToInt32(dtMasa.Rows[i]["Masa_Durum"]) == 2)
                    {
                        btnMasa.Appearance.BackColor = hesap;
                        btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) == "" ? Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]) : Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                    }

                    if (Convert.ToInt32(dtMasa.Rows[i]["RezSayisi"]) > 0)
                    {
                        btnMasa.Appearance.BackColor = Param.Param_RezMasaRengi;
                        btnMasa.Text += "\nREZERVE(" + Convert.ToInt32(dtMasa.Rows[i]["RezSayisi"]) + ")\n" + Convert.ToString(dtMasa.Rows[i]["RezMasaAdi"]);
                    }

                    //Pda Acılan Masalar
                    //if (Convert.ToBoolean(dtMasa.Rows[i]["Rsat_Pda"]) == true)
                    //{
                    //    btnMasa.Appearance.ForeColor = Color.AliceBlue;
                    //    btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                    //}

                    //Özel Masaların Adı - Rengi 


                    //Garson Adı Eklenmesi Masaya

                    if (Param.Param_Masa_Garson && Convert.ToString(dtMasa.Rows[i]["Garson"]) != "" && !Departman.Garson_Sor)
                    {
                        btnMasa.Text += "\n" + Convert.ToString(dtMasa.Rows[i]["Garson"]);
                    }

                    if (Convert.ToString(dtMasa.Rows[i]["Garson2"]) != "" && Departman.Garson_Sor)
                    {
                        btnMasa.Text += "\n" + Convert.ToString(dtMasa.Rows[i]["Garson2"]);
                    }

                    string TL = "";

                    if (Param.Param_Masa_Garson == false)
                    {

                        if (Param.Calisma_Sekli == 1)
                        {
                            TL = dtMasa.Rows[i]["Rsat_Doviztutar"].ToString().Trim();
                            if (!TL.Equals(""))
                            {
                                TL = TL + " €";
                            }
                        }
                        else
                        {
                            TL = dtMasa.Rows[i]["Rsat_Tutar"].ToString().Trim();
                            if (!TL.Equals(""))
                            {
                                TL = TL + " ₺";
                            }
                        }

                        btnMasa.Text += "\n" + TL;
                    }


                    //Paket Masaları
                    if (Convert.ToBoolean(dtMasa.Rows[i]["Masa_Paket"]) && Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "0")
                    {
                        btnMasa.Appearance.BackColor = Color.Gainsboro;
                    }

                    if (sonMasa != "" && sonMasa == Convert.ToString(dtMasa.Rows[i]["Masa_No"]))
                    {
                        btnMasa.Appearance.BackColor = Param.Param_Sonmasa_Renk;
                    }


                    if (Convert.ToInt32(dtMasa.Rows[i]["Masa_Durum"]) == 2) // !! burada kaldım
                    {
                        btnMasa.Appearance.BackColor = hesap;
                    }


                    btnMasa.Appearance.BorderColor = btnMasa.Appearance.BackColor;
                    btnMasa.Click += new EventHandler(btnMasa_Click);
                    btnMasa.DoubleClick += new EventHandler(btnMasa_DoubleClick);

                    flp_Masa.Controls.Add(btnMasa);
                    //btnMasa.MouseHover += ButtonName_MouseHover;

                    // !!!!!

                }

                //Split_Ayarla();
                gridControl2.DataSource = null;
                bartxt_FisNo.EditValue = 0;

                DataTable dtBilgi = dbtools.SelectTable("select COUNT(Masa_Durum) as Sayi,MIN(Masa_Durum) as Durum from Pos_Masa WITH(NOLOCK) where Masa_Depart = '" + Departman.Dep_Kodu + "' group by Masa_Durum order by Masa_Durum");
                bartxt_BosMasa.EditValue = Convert.ToString(dtBilgi.Rows[0]["Sayi"]);
                bartxt_DoluMasa.EditValue = dtBilgi.Rows.Count > 1 ? Convert.ToString(dtBilgi.Rows[1]["Sayi"]) : "0";

                Main.a.Listele(0);
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "MasaYenileIlkAcilis", "", ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }


        }

        /*
        private void MasaYenile(int Acik, string filtre = "")
        {
            flp_Masa.Controls.Clear();
            barspn_Refresh.EditValue = Param.Masa_Refresh;
            Masa_No = String.Empty;
            Masa_Paket = false;
            Split = 0;
            cbtn_0.Checked = true;
            //lbl_KisiSayisi.Text = String.Empty;

            Color bos = Color.Lime, dolu = Color.OrangeRed, hesap = Color.MediumOrchid;

            Color rezMasaRengi = Color.LavenderBlush;
            string KonumFilter = String.Empty;
            if (Masa_Konum != String.Empty)
            {
                KonumFilter = " and Masa_Konum = '" + Masa_Konum + "' ";

                //DataTable dtRenk = dbtools.SelectTable("select ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk from Pos_Kodlar where Pkod_Sinif ='14' and Pkod_Kod = '" + Departman.Dep_Kodu + "' and Pkod_Konumkod = '" + Masa_Konum + "'");
                //bos = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtRenk.Rows[0]["Pkod_Bosrenk"]));
                //dolu = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtRenk.Rows[0]["Pkod_Dolurenk"]));
                //hesap = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtRenk.Rows[0]["Pkod_Hesaprenk"]));
            }

            string paketFilter = "";
            if (Param.Param_Tum_Paket && Masa_Konum == "")
            {
                paketFilter = " and Masa_Paket = 0 ";
            }

            string siralama = " order by ISNULL(Pkod_Sira,-1), Masa_No ";

            if (Param.Param_OzelMasaSiralama == true)
            {
                siralama = " order by Masa_Ozel Desc,ISNULL(Pkod_Sira,-1), Masa_No ";
            }

            string Masalar = "";
            if (Acik == 1)
            {
                Masalar = " and Masa_Durum <> 0 ";
            }

            string query = "select Masa_No, ISNULL(Masa_Ozel,'') as Masa_Ozel , "
                            + " MIN(ISNULL(Rsat_MusTipi,'')) as Rsat_MusTipi,ISNULL(Masa_Durum,0) as Masa_Durum,"
                            + " MAX(CASE WHEN ISNULL(Rsat_Hesap_Kilit,0)= 1 THEN 1 ELSE 0 END) as Rsat_Hesap_Kilit,"
                            + " Masa_Ad,"
                            + " isnull(Masa_Paket,0) as Masa_Paket, "
                            + " case when Masa_Durum <> 0 then MIN(ISNULL(Kasiyer.P_Ad,'')) + ' ' +  MIN(ISNULL(Kasiyer.P_Soyad,'')) else '' end as Garson, "
                            + " case when Masa_Durum <> 0 then MIN(ISNULL(Garson.P_Ad,'')) + ' ' +  MIN(ISNULL(Garson.P_Soyad,'')) else '' end as Garson2, "
                            + " COUNT(Rez_Id) as RezSayisi, "
                            + " ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk, (Max(Pos_Rez.Rez_Adi) + ' ' +  Max(Pos_Rez.Rez_Soyadi)) as RezMasaAdi "
                            + " from Pos_Masa WITH(NOLOCK) "
                            + " left join Cst_Recete_Satis WITH(NOLOCK) on Masa_No = Rsat_Masa and Rsat_Durum = 'A'  and Rsat_Departman = Masa_Depart  "
                            //+ "     left join Rmosmuh.dbo.Pos_User as Kasiyer on Rsat_Garson = Kasiyer.P_Kod "
                            + " left join Rmosmuh.dbo.Pos_User as Kasiyer on Kasiyer.P_Kod = (Select Top(1) Rsat_Garson From Cst_Recete_Satis as s Where Rsat_Durum = 'A' and s.Rsat_Departman = '" + Departman.Dep_Kodu + "' and s.Rsat_Masa = Masa_No and s.Rsat_Fisno = Cst_Recete_Satis.Rsat_Fisno order by 1 desc ) "
                            + " left join Rmosmuh.dbo.Pos_User as Garson on Rsat_Garson2 = Garson.P_Kod "
                            + " left join Pos_Rez on Rez_Dep = '" + Departman.Dep_Kodu + "' and Rez_Masano = Masa_No and Rez_Tarih = '" + Param.Tarih + "' "
                            + " left join Pos_Kodlar as renk on Masa_Konum = Pkod_Konumkod and Pkod_Kod = Masa_Depart and Pkod_Sinif ='14' "
                            + " where Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Hayali,0) = 0  " + KonumFilter + paketFilter + Masalar
                            + " group by Masa_No,Masa_Ad,Masa_Ozel,Masa_Durum,Masa_Paket,Pkod_Bosrenk ,Pkod_Dolurenk, Pkod_Hesaprenk, Pkod_Sira "
                            + siralama;

            if (filtre != "")
            {
                query = "select Masa_No, ISNULL(Masa_Ozel,'') as Masa_Ozel , "
                             + " MIN(ISNULL(Rsat_MusTipi,'')) as Rsat_MusTipi,ISNULL(Masa_Durum,0) as Masa_Durum,"
                             + " MAX(CASE WHEN ISNULL(Rsat_Hesap_Kilit,0)= 1 THEN 1 ELSE 0 END) as Rsat_Hesap_Kilit,"
                             + " Masa_Ad,"
                             + " isnull(Masa_Paket,0) as Masa_Paket, "
                             + " case when Masa_Durum <> 0 then MIN(ISNULL(Kasiyer.P_Ad,'')) + ' ' +  MIN(ISNULL(Kasiyer.P_Soyad,'')) else '' end as Garson, "
                             + " case when Masa_Durum <> 0 then MIN(ISNULL(Garson.P_Ad,'')) + ' ' +  MIN(ISNULL(Garson.P_Soyad,'')) else '' end as Garson2, "
                             + " COUNT(Rez_Id) as RezSayisi, "
                             + " ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk, (Max(Pos_Rez.Rez_Adi) + ' ' +  Max(Pos_Rez.Rez_Soyadi)) as RezMasaAdi "
                             + " from Pos_Masa WITH(NOLOCK) "
                             + " left join Cst_Recete_Satis WITH(NOLOCK) on Masa_No = Rsat_Masa and Rsat_Durum = 'A'  and Rsat_Departman = Masa_Depart  "
                             //+ "     left join Rmosmuh.dbo.Pos_User as Kasiyer on Rsat_Garson = Kasiyer.P_Kod "
                             + " left join Rmosmuh.dbo.Pos_User as Kasiyer on Kasiyer.P_Kod = (Select Top(1) Rsat_Garson From Cst_Recete_Satis as s Where Rsat_Durum = 'A' and s.Rsat_Departman = '" + Departman.Dep_Kodu + "' and s.Rsat_Masa = Masa_No and s.Rsat_Fisno = Cst_Recete_Satis.Rsat_Fisno order by 1 desc ) "
                             + " left join Rmosmuh.dbo.Pos_User as Garson on Rsat_Garson2 = Garson.P_Kod "
                             + " left join Pos_Rez on Rez_Dep = '" + Departman.Dep_Kodu + "' and Rez_Masano = Masa_No and Rez_Tarih = '" + Param.Tarih + "' "
                             + " left join Pos_Kodlar as renk on Masa_Konum = Pkod_Konumkod and Pkod_Kod = Masa_Depart and Pkod_Sinif ='14' "
                             + " where Masa_Depart = '" + Departman.Dep_Kodu + "' " +
                             " " + filtre +
                             " and ISNULL(Masa_Hayali,0) = 0  " + KonumFilter + paketFilter + Masalar
                             + " group by Masa_No,Masa_Ad,Masa_Ozel,Masa_Durum,Masa_Paket,Pkod_Bosrenk ,Pkod_Dolurenk, Pkod_Hesaprenk, Pkod_Sira "
                             + siralama;
            }

            DataTable dtMasa = dbtools.SelectTable(query);

            if (dtMasa.Rows.Count < 1)
            {
                //MessageBox.Show("Bu Departmana Ait Masa Tanımı Yapılmamış", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sonMasa = "";
            if (Param.Param_Sonmasa)
            {
                sonMasa = dbtools.DegerGetir("select top 1 Rsat_Masa from Cst_Recete_Satis where Rsat_Tarih = '" + Param.Tarih + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "' and Rsat_Durum = 'A' order by Rsat_Satissaat desc");
            }

            string masaSize = Param.Param_Masa_Size == "" ? "90;45" : Param.Param_Masa_Size;
            Size s = new Size(Convert.ToInt32(masaSize.Split(';')[0]), Convert.ToInt32(masaSize.Split(';')[1]));

            for (int i = 0; i < dtMasa.Rows.Count; i++)
            {
                bos = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtMasa.Rows[i]["Pkod_Bosrenk"]));
                dolu = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtMasa.Rows[i]["Pkod_Dolurenk"]));
                hesap = System.Drawing.ColorTranslator.FromHtml(Convert.ToString(dtMasa.Rows[i]["Pkod_Hesaprenk"]));
                bool kilit = Convert.ToBoolean(dtMasa.Rows[i]["Rsat_Hesap_Kilit"]);


                SimpleButton btnMasa = new SimpleButton();
                btnMasa.Size = s;
                btnMasa.TabIndex = 0;
                btnMasa.TabStop = false;
                btnMasa.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                btnMasa.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                btnMasa.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                btnMasa.Appearance.Options.UseBackColor = false;

                btnMasa.Tag = Convert.ToString(dtMasa.Rows[i]["Masa_No"]);



                //Boş Masa
                if (Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "0")
                {
                    btnMasa.Appearance.BackColor = bos;
                    btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                }

                //Dolu Masa
                if (Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "1")
                {
                    btnMasa.Appearance.BackColor = dolu;
                    btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                }

                if (Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) != String.Empty)
                {
                    btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                    //if (Param.Pos_HesapDkmRenk == false)
                    //{
                    btnMasa.Appearance.BackColor = Param.Param_OzelMasaRengi;
                    //}
                    //else
                    //{
                    //    btnMasa.Appearance.BackColor = hesap;
                    //}

                }



                if (!kilit && Convert.ToInt32(dtMasa.Rows[i]["Masa_Durum"]) == 2)
                {
                    if (Param.Pos_HesapDkmRenk == false)
                    {
                        btnMasa.Appearance.BackColor = dolu;
                        btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) == "" ? Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]) : Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                    }
                    else
                    {
                        btnMasa.Appearance.BackColor = hesap;
                        btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) == "" ? Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]) : Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                    }
                }

                //Beklemede Masalar
                if (kilit && Convert.ToInt32(dtMasa.Rows[i]["Masa_Durum"]) == 2)
                {
                    btnMasa.Appearance.BackColor = hesap;
                    btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) == "" ? Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]) : Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                }

                if (Convert.ToInt32(dtMasa.Rows[i]["RezSayisi"]) > 0)
                {
                    btnMasa.Appearance.BackColor = Param.Param_RezMasaRengi;
                    btnMasa.Text += "\nREZERVE(" + Convert.ToInt32(dtMasa.Rows[i]["RezSayisi"]) + ")\n" + Convert.ToString(dtMasa.Rows[i]["RezMasaAdi"]);
                }

                //Pda Acılan Masalar
                //if (Convert.ToBoolean(dtMasa.Rows[i]["Rsat_Pda"]) == true)
                //{
                //    btnMasa.Appearance.ForeColor = Color.AliceBlue;
                //    btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                //}

                //Özel Masaların Adı - Rengi 


                //Garson Adı Eklenmesi Masaya

                if (Param.Param_Masa_Garson && Convert.ToString(dtMasa.Rows[i]["Garson"]) != "" && !Departman.Garson_Sor)
                {
                    btnMasa.Text += "\n" + Convert.ToString(dtMasa.Rows[i]["Garson"]);
                }

                if (Convert.ToString(dtMasa.Rows[i]["Garson2"]) != "" && Departman.Garson_Sor)
                {
                    btnMasa.Text += "\n" + Convert.ToString(dtMasa.Rows[i]["Garson2"]);
                }


                //Paket Masaları
                if (Convert.ToBoolean(dtMasa.Rows[i]["Masa_Paket"]) && Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "0")
                {
                    btnMasa.Appearance.BackColor = Color.Gainsboro;
                }

                if (sonMasa != "" && sonMasa == Convert.ToString(dtMasa.Rows[i]["Masa_No"]))
                {
                    btnMasa.Appearance.BackColor = Param.Param_Sonmasa_Renk;
                }


                if (Convert.ToInt32(dtMasa.Rows[i]["Masa_Durum"]) == 2) // !! burada kaldım
                {
                    btnMasa.Appearance.BackColor = hesap;
                }


                btnMasa.Appearance.BorderColor = btnMasa.Appearance.BackColor;
                btnMasa.Click += new EventHandler(btnMasa_Click);
                btnMasa.DoubleClick += new EventHandler(btnMasa_DoubleClick);
                flp_Masa.Controls.Add(btnMasa);
                //btnMasa.MouseHover += ButtonName_MouseHover;

                // !!!!!

            }

            //Split_Ayarla();
            gridControl2.DataSource = null;
            bartxt_FisNo.EditValue = 0;

            DataTable dtBilgi = dbtools.SelectTable("select COUNT(Masa_Durum) as Sayi,MIN(Masa_Durum) as Durum from Pos_Masa WITH(NOLOCK) where Masa_Depart = '" + Departman.Dep_Kodu + "' group by Masa_Durum order by Masa_Durum");
            bartxt_BosMasa.EditValue = Convert.ToString(dtBilgi.Rows[0]["Sayi"]);
            bartxt_DoluMasa.EditValue = dtBilgi.Rows.Count > 1 ? Convert.ToString(dtBilgi.Rows[1]["Sayi"]) : "0";

            Main.a.Listele(0);

        }*/

        private void btnMasa_DoubleClick(Object sender, EventArgs e)
        {
            satisGit();
        }

        int Masa_Durum = 0;
        void btnMasa_Click(object sender, EventArgs e)
        {
            try
            {
                barspn_Refresh.EditValue = Param.Masa_Refresh;

                SimpleButton myButton = (SimpleButton)sender;
                foreach (SimpleButton btn in flp_Masa.Controls)
                {
                    btn.Appearance.BackColor = btn.Appearance.BorderColor;
                }
                myButton.Appearance.BackColor = Color.White;

                Masa_No = myButton.Tag.ToString();

                DataTable dtMasa = dbtools.SelectTable("Select isnull(Masa_Paket,0) as Masa_Paket,isnull(Masa_Ozel,'') as Masa_Ozel, isnull(Masa_Durum,0) as Masa_Durum From Pos_Masa WITH(NOLOCK) where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Masa_No + "'");
                if (dtMasa.Rows.Count > 0)
                {
                    Masa_Paket = Convert.ToBoolean(dtMasa.Rows[0]["Masa_Paket"]);
                    Masa_OzelAdi = Convert.ToString(dtMasa.Rows[0]["Masa_Ozel"]);
                    Masa_Durum = Convert.ToInt32(dtMasa.Rows[0]["Masa_Durum"]);
                }

                //Masa_Paket = Convert.ToBoolean(dbtools.DegerGetir("select isnull(Masa_Paket,0) from Pos_Masa WITH(NOLOCK) where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Masa_No + "'"));
                //Masa_OzelAdi = dbtools.DegerGetir("select isnull(Masa_Ozel,null) from Pos_Masa WITH(NOLOCK) where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Masa_No + "'");

                bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 4, @Dep_Kodu = '" + Departman.Dep_Kodu + "',@Masano = '" + Masa_No + "'"));

                Main.a.Listele(Convert.ToInt32(bartxt_FisNo.EditValue));


                //Masa_Durum = Convert.ToInt32(dbtools.DegerGetir("select isnull(Masa_Durum,0) from Pos_Masa WITH(NOLOCK) where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Masa_No + "'"));
                bar_Sure.EditValue = Convert.ToString(dbtools.DegerGetir("select Convert(nvarchar(max),Convert(time(0),MIN(Rsat_Acilis))) + ' - ' + Convert(nvarchar(max),DATEDIFF(MINUTE,MIN(Rsat_Acilis),Convert(time,Getdate()))) + ' Dk ' from Cst_Recete_Satis where Rsat_Fisno = '" + bartxt_FisNo.EditValue + "'"));

                if (Masa_Paket == true && User.P_Kulturu == 1)
                {
                    Masa_Konum = "";
                    MasaYenile(0);
                    return;
                }


                if (Departman.Kodlar_Kuver_Sat == true)
                {
                    DataTable dt = dbtools.SelectTable("Exec Pos_Sorgu @Sorgu_Tipi = '33', @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Fisno = '" + bartxt_FisNo.EditValue + "'");
                    if (dt.Rows.Count > 0)
                    {
                        //bar_Sure.EditValue = Convert.ToString(dt.Rows[0]["Sayi"]);
                        bar_KuverTutar.EditValue = Convert.ToDecimal(dt.Rows[0]["Tutar"]).ToString("n2");
                    }
                }


                //lbl_KisiSayisi.Text = Convert.ToString(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 25, @Dep_Kodu = '" + Departman.Dep_Kodu + "',@Masano = '" + Masa_No + "', @Fisno = '" + bartxt_FisNo.EditValue + "'"));




                //Split_Ayarla();


                gridyenile();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnMasa_Click", "", ex);
            }

        }

        private void Split_Ayarla()
        {
            cbtn_0.ForeColor = Color.Black;
            cbtn_1.ForeColor = Color.Black;
            cbtn_2.ForeColor = Color.Black;
            cbtn_3.ForeColor = Color.Black;
            cbtn_4.ForeColor = Color.Black;
            cbtn_5.ForeColor = Color.Black;
            cbtn_6.ForeColor = Color.Black;
            cbtn_7.ForeColor = Color.Black;
            cbtn_8.ForeColor = Color.Black;
            cbtn_9.ForeColor = Color.Black;

            cbtn_0.Tag = "";
            cbtn_1.Tag = "";
            cbtn_2.Tag = "";
            cbtn_3.Tag = "";
            cbtn_4.Tag = "";
            cbtn_5.Tag = "";
            cbtn_6.Tag = "";
            cbtn_7.Tag = "";
            cbtn_8.Tag = "";
            cbtn_9.Tag = "";


            DataTable dtSplit = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 20,@Masano = '" + Masa_No + "', @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Fisno = '" + Convert.ToString(bartxt_FisNo.EditValue) + "'");
            for (int i = 0; i < dtSplit.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 0) { cbtn_0.ForeColor = Color.Red; cbtn_0.Tag = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 1) { cbtn_1.ForeColor = Color.Red; cbtn_1.Tag = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 2) { cbtn_2.ForeColor = Color.Red; cbtn_2.Tag = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 3) { cbtn_3.ForeColor = Color.Red; cbtn_3.Tag = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 4) { cbtn_4.ForeColor = Color.Red; cbtn_4.Tag = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 5) { cbtn_5.ForeColor = Color.Red; cbtn_5.Tag = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 6) { cbtn_6.ForeColor = Color.Red; cbtn_6.Tag = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 7) { cbtn_7.ForeColor = Color.Red; cbtn_7.Tag = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 8) { cbtn_8.ForeColor = Color.Red; cbtn_8.Tag = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
                if (Convert.ToInt32(dtSplit.Rows[i]["Rsat_Split"]) == 9) { cbtn_9.ForeColor = Color.Red; cbtn_9.Tag = Convert.ToString(dtSplit.Rows[i]["Rsat_Splitad"]); }
            }
        }

        public bool parcalimi() // false ise parçalı değildir. true ise parçalıdır
        {
            string varmi = dbtools.DegerGetir("select count(*) as toplam from Pos_Masa where Masa_Depart='" + Departman.Dep_Kodu + "' and Masa_Durum='1' and Masa_No like '" + Masa_No + "[_]%'");

            if (varmi.Equals("0"))
            {
                return false; // parcali değiltir
            }

            return true;
        }

        private void gridyenile()
        {
            if (!Masa_No.Contains("_") && parcalimi())
            {
                string query = @"select distinct Rsat_Fisno from Cst_Recete_Satis 
where  Rsat_Durum = 'A' and 
Rsat_Masa in(select Masa_No from Pos_Masa where Masa_Depart='" + Departman.Dep_Kodu + @"' and Masa_Durum='1' and Masa_No like '" + Masa_No + @"[_]%' ) 
and Rsat_Departman = '" + Departman.Dep_Kodu + "'";

                DataTable dt2 = dbtools.SelectTable(query);


                DataTable dtParcaliHepsi = new DataTable();


                foreach (DataRow item in dt2.Rows)
                {
                    string fisno = item["Rsat_Fisno"].ToString();
                    DataTable dtParcali = gridyenileParcaliMasa(fisno);
                    dtParcaliHepsi.Merge(dtParcali);
                }

                gridControl2.DataSource = dtParcaliHepsi;
                gridView2.BestFitColumns();

                return;
            }

            gridColumn3.FieldName = "Rec_Ad";
            gridColumn4.FieldName = "Rsat_Miktar";
            gridColumn5.FieldName = "Rsat_Emiktar";
            gridColumn6.FieldName = "Rsat_Tutar";
            gridColumn7.FieldName = "Rsat_Doviztutar";
            gridColumn8.FieldName = "Rsat_Ba";
            gridColumn9.FieldName = "Rsat_Garson";

            if (Param.Calisma_Sekli == 1)
            {
                gridColumn7.Visible = true;
            }
            else
            {
                gridColumn6.Visible = true;
            }

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", Convert.ToInt32(bartxt_FisNo.EditValue));
            com.Parameters.AddWithValue("@Rapor_Tipi", 0);
            com.Parameters.AddWithValue("@Split", Split);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            toplamTutar = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {

                    toplamTutar += Convert.ToDecimal(item["Rsat_Tutar"].ToString());

                    item["Rsat_Miktar"] = item["Rsat_Miktar"].ToString().Replace(",0000", "");


                    item["Rsat_Tutar"] = item["Rsat_Tutar"].ToString().Replace(",00", "");


                }
            }

            DataTable dtCloned = dt.Clone();
            dtCloned.Columns["Rsat_Tutar"].DataType = typeof(string);
            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }


            gridControl2.DataSource = dtCloned;
            gridView2.BestFitColumns();
        }

        decimal toplamTutar = 0;
        private DataTable gridyenileParcaliMasa(string fisno)
        {
            gridColumn3.FieldName = "Rec_Ad";
            gridColumn4.FieldName = "Rsat_Miktar";
            gridColumn5.FieldName = "Rsat_Emiktar";
            gridColumn6.FieldName = "Rsat_Tutar";
            gridColumn7.FieldName = "Rsat_Doviztutar";
            gridColumn8.FieldName = "Rsat_Ba";
            gridColumn9.FieldName = "Rsat_Garson";

            if (Param.Calisma_Sekli == 1)
            {
                gridColumn7.Visible = true;
            }
            else
            {
                gridColumn6.Visible = true;
            }


            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Fisno", fisno);
            com.Parameters.AddWithValue("@Rapor_Tipi", 0);
            com.Parameters.AddWithValue("@Split", Split);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);



            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    item["Rsat_Miktar"] = item["Rsat_Miktar"].ToString().Replace(",0000", "");
                    item["Rsat_Tutar"] = item["Rsat_Tutar"].ToString().Replace(",00", "");
                }
            }



            DataTable dtCloned = dt.Clone();
            dtCloned.Columns["Rsat_Tutar"].DataType = typeof(string);
            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }

            return dtCloned;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            barspn_Refresh.EditValue = Convert.ToInt32(barspn_Refresh.EditValue) - 1;
            if (Convert.ToInt32(barspn_Refresh.EditValue) == 0)
            {
                MasaYenile(0);
                Param.Param_Yukle();
            }
        }

        private void btn_Satis_Click(object sender, EventArgs e)
        {
            Satis();
        }

        private void Satis()
        {
            if (Param.Param_Masaacan_Garson && !User.M_BaskaMasa)
            {
                string garson = dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Garson from Cst_Recete_Satis where Rsat_Ba = 'B' and Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue).ToString() + "' order by Rsat_Id),'')");
                if (garson != User.P_Kod && garson != "")
                {
                    MessageBox.Show(res_man.GetString("Masayı ") + garson + " - " + User.Isim_Getir(garson) + res_man.GetString(" Açmıştır.Başkası Satış Yapamaz...!!!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            timer1.Enabled = false;

            Satis_Yap();

            timer1.Enabled = true;
        }

        private void Satis_Yap()
        {
            int fisno = Convert.ToInt32(bartxt_FisNo.EditValue);
            if (Param.Param_Hesap_Kilit)
            {
                if (fisno > 0)
                {
                    bool Rsat_Hesap_Kilit = Convert.ToBoolean(dbtools.DegerGetir("select top 1 ISNULL(Rsat_Hesap_Kilit,0) from Cst_Recete_Satis where Rsat_Fisno = " + fisno));
                    if (Rsat_Hesap_Kilit)
                    {
                        MessageBox.Show(res_man.GetString("Masa Satışa Kapalı..."), "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }

            if (Param.Param_Masaacan_Garson && !User.M_BaskaMasa)
            {
                string garson = dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Garson from Cst_Recete_Satis where Rsat_Ba = 'B' and Rsat_Fisno = '" + fisno + "' order by Rsat_Id),'')");
                if (garson != User.P_Kod && garson != "")
                {
                    MessageBox.Show(res_man.GetString("Masayı ") + garson + " - " + User.Isim_Getir(garson) + res_man.GetString(" Açmıştır.Başkası Satış Yapamaz...!!!"), "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }


            Satis satis = new Satis();
            satis.Tag = "M";
            if (Masa_No == String.Empty)
            {
                MessageBox.Show(res_man.GetString("Masa Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            satis.Masa_No = Masa_No;
            satis.Masa_Paket = Masa_Paket;
            satis.Ozel_Masa = Ozel_Masa;
            satis.Split = Split;
            satis.Splitad = Splitad;
            satis.Masa_Durum = Masa_Durum;// 1

            satis.ShowDialog();


            MasaYenile(0);

            if (User.M_SatisRelogin)
            {
                Relogin();
            }
        }

        //private void btn_OdaKilitle_Click(object sender, EventArgs e)
        //{
        //    if (Masa_No == String.Empty || gridView2.RowCount == 0)
        //    {
        //       MessageBox.Show(res_man.GetString("Masa Bilgisi Bulunamadı...", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    timer1.Enabled = false;
        //    Arama ara = new Arama();
        //    ara.Tag = "K";
        //    ara.ShowDialog();

        //    if (ara.Mus_tipi != String.Empty)
        //    {
        //        dbtools.execcmd("update Cst_Recete_Satis set Rsat_MusTipi = '" + ara.Mus_tipi + "',Rsat_Odano = '" + ara.Oda_No + "',Rsat_Folio = '" + ara.Folio + "',Rsat_Pansiyon = '" + ara.Pansiyon + "',Rsat_Uye_Id = '" + ara.Uye_Id + "', "
        //                                + " Rsat_Uye_Ad = '" + ara.Uye_Adsoyad + "',Rsat_Uye_Kart_Turu = '" + ara.Uye_Kartturu + "',Rsat_Cari = '" + ara.Cari_Kod + "',Rsat_Indkodu = '" + ara.Ind_Kodu + "',Rsat_Indoran = '" + ara.Ind_Oran.ToString().Replace(",", ".") + "' "
        //                                + " where Rsat_Fisno = '" + Convert.ToString(bartxt_FisNo.EditValue) + "' and Rsat_Masa = '" + Masa_No + "'");
        //        dbtools.execcmd("exec Pos_Satis_Induyg @Fisno = " + Convert.ToString(bartxt_FisNo.EditValue));
        //        MasaYenile(0);
        //    }
        //    timer1.Enabled = true;
        //}

        private void btn_HesapBak_Click(object sender, EventArgs e)
        {
            HesapBak();
        }

        private void HesapBak()
        {

            if (isYazdirilmamisSiparis())
            {
                return;
            }

            if (Masa_No == String.Empty || gridView2.RowCount == 0)
            {
                MessageBox.Show(res_man.GetString("Masa Bilgisi Bulunamadı..."), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (StatikSinif.masaMusaitmi(Masa_No) == false) return;

            if (Param.Param_Masaacan_Garson && !User.M_BaskaMasa)
            {
                string garson = dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Garson from Cst_Recete_Satis where Rsat_Ba = 'B' and Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue).ToString() + "' order by Rsat_Id),'')");
                if (garson != User.P_Kod && garson != "")
                {
                    MessageBox.Show(res_man.GetString("Masayı ") + garson + " - " + User.Isim_Getir(garson) + res_man.GetString(" Açmıştır.Başkası Satış Yapamaz...!!!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            timer1.Enabled = false;

            string masaDurum = dbtools.DegerGetir("select top 1 Masa_Durum from Pos_Masa where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

            hes = new Hesap();
            hes.Tag = bartxt_FisNo.EditValue;
            hes.Masa_No = Masa_No;
            hes.Split = Split;
            hes.Splitad = Splitad;
            hes.ShowDialog();

            if (masaDurum == "2")
            {
                string masaDurumYeni = dbtools.DegerGetir("select top 1 Masa_Durum from Pos_Masa where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

                if (masaDurumYeni == "1")
                {
                    dbtools.execcmdR("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                }
            }



            MasaYenile(0);



            if (User.M_SatisRelogin)
            {
                Relogin();
            }

            timer1.Enabled = true;
        }

        public static Hesap hes;
        private void btn_MasaTransfer_Click(object sender, EventArgs e)
        {
            MasaTransfer();
        }

        private void MasaTransfer()
        {
            if (StatikSinif.masaMusaitmi(Masa_No) == false) return;

            if (Convert.ToInt32(bartxt_FisNo.EditValue) > 0 && Masa_No != String.Empty)
            {
                if (Param.Param_Masaacan_Garson && !User.M_BaskaMasa)
                {
                    string garson = dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Garson from Cst_Recete_Satis where Rsat_Ba = 'B' and Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue).ToString() + "' order by Rsat_Id),'')");
                    if (garson != User.P_Kod && garson != "")
                    {
                        MessageBox.Show(res_man.GetString("Masayı ") + garson + " - " + User.Isim_Getir(garson) + res_man.GetString(" Açmıştır.Başkası Satış Yapamaz...!!!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }

                timer1.Enabled = false;

                Masa_Tr transfer = new Masa_Tr();
                transfer.Tag = Convert.ToInt32(bartxt_FisNo.EditValue).ToString();
                transfer.txt_Masano.Text = Masa_No;
                transfer.OzelMasaAdi = Masa_OzelAdi;
                transfer.ShowDialog();
                MasaYenile(0);

                if (User.M_SatisRelogin)
                {
                    Relogin();
                }

                timer1.Enabled = true;
            }
            else
            {
                MessageBox.Show(res_man.GetString("Masa Bilgisi Bulunamadı..."), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_OzelMasa_Click(object sender, EventArgs e)
        {
            OzelMasa();
        }

        private void OzelMasa()
        {
            if (Masa_No == String.Empty)
            {
                MessageBox.Show(res_man.GetString("Masa Seçiniz..."), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Param.Param_Masaacan_Garson && !User.M_BaskaMasa)
            {
                string garson = dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Garson from Cst_Recete_Satis where Rsat_Ba = 'B' and Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue).ToString() + "' order by Rsat_Id),'')");
                if (garson != User.P_Kod && garson != "")
                {
                    MessageBox.Show(res_man.GetString("Masayı ") + garson + " - " + User.Isim_Getir(garson) + res_man.GetString(" Açmıştır.Başkası Satış Yapamaz...!!!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            timer1.Enabled = false;
            Klavye2 klavye = new Klavye2();
            klavye.Fisno = Convert.ToInt32(bartxt_FisNo.EditValue);

            string fisno = bartxt_FisNo.EditValue.ToString();

            klavye.txt_Yazi.Text = Masa_OzelAdi;
            klavye.ShowDialog();

            if (klavye.cikis == true)
            {
                Ozel_Masa = klavye.yazi;


                bool yenile = false;
                if (MessageBox.Show("Satış Ekranına Geçiş Yapılsın mı?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    dbtools.execcmd("update Pos_Masa set Masa_Ozel = '" + Ozel_Masa + "', Masa_Durum = 1 where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Ozel_Masa, Log.Log_Islem.Kaydet, Masa_No + " NOLU Masa Özel Masa Yapıldı " + Ozel_Masa, Convert.ToString(bartxt_FisNo.EditValue), String.Empty);

                    Satis_Yap();
                }
                else
                {
                    if (Convert.ToInt32(bartxt_FisNo.EditValue) == 0)
                    {
                        dbtools.execcmd("update Pos_Masa set Masa_Ozel = NULL, Masa_Durum = 0 where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                    }
                    else
                    {
                        dbtools.execcmd("update Pos_Masa set Masa_Ozel = '" + Ozel_Masa + "', Masa_Durum = 1 where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Ozel_Masa, Log.Log_Islem.Kaydet, Masa_No + " NOLU Masa Özel Masa Yapıldı " + Ozel_Masa, Convert.ToString(bartxt_FisNo.EditValue), String.Empty);
                    }


                    yenile = true;
                }

                dbtools.execcmd("update Cst_Recete_Satis set Rsat_OzelMasaAdi = '" + Ozel_Masa + "' where Rsat_Fisno = '" + fisno + "' and Rsat_Departman ='" + Departman.Dep_Kodu + "'");

                Ozel_Masa = String.Empty;

                if (yenile) MasaYenile(0);
            }
            timer1.Enabled = true;
        }

        public void satisOzelMasaAdDegistir(string fisno, string ozelMasaAd)
        {
            try
            {
                dbtools.execcmdR("update Cst_Recete_Satis set Rsat_OzelMasaAdi='" + ozelMasaAd + "' where Rsat_Fisno='" + fisno + "'");
            }
            catch (Exception ex)
            {

            }
        }
        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {

            Masa_Konum = Convert.ToString(gridView1.GetFocusedRowCellValue("Kod"));

            if (User.P_Kulturu == 1 && Masa_Konum == "P")
            {
                return;
            }

            if (Param.Param_Paket_Form && Masa_Konum == "P")
            {
                Paket p = new Paket();
                p.ShowDialog();
                gridView1.FocusedRowHandle = 0;// SONRADAN EKLENDİ RAMAZAN MENDİ BEY TÜM MASALARA GİTSİN DEDİ
                Masa_Konum = Convert.ToString(gridView1.GetFocusedRowCellValue("Kod")); // SONRADAN EKLENDİ RAMAZAN MENDİ BEY TÜM MASALARA GİTSİN DEDİ
            }

            MasaYenileIlkAcilis(0);

        }

        private void btn_MasaIslem_Click(object sender, EventArgs e)
        {
            MasaIslemler();
        }

        private void MasaIslemler()
        {
            //if (Convert.ToInt32(bartxt_FisNo.EditValue) > 0 && Masa_No != String.Empty)
            //{

            if (Param.Param_Masaacan_Garson && !User.M_BaskaMasa)
            {
                string garson = dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Garson from Cst_Recete_Satis where Rsat_Ba = 'B' and Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue).ToString() + "' order by Rsat_Id),'')");
                if (garson != User.P_Kod && garson != "")
                {
                    MessageBox.Show(res_man.GetString("Masayı ") + garson + " - " + User.Isim_Getir(garson) + res_man.GetString(" Açmıştır.Başkası Satış Yapamaz...!!!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }


            timer1.Enabled = false;
            Masa_Islem masa = new Masa_Islem();
            masa.Masa_No = Masa_No;
            masa.Masa_Paket = Masa_Paket;
            masa.Fisno = Convert.ToInt32(bartxt_FisNo.EditValue);
            masa.ShowDialog();

            if (User.M_SatisRelogin)
            {
                Relogin();
            }
            MasaYenile(0);
            timer1.Enabled = true;




            //}
            //else
            //{
            //   MessageBox.Show(res_man.GetString("Masa Bilgisi Bulunamadı...", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void btn_Relogin_Click(object sender, EventArgs e)
        {
            Relogin();
        }

        private void Relogin()
        {
            frmLogin login = new frmLogin();
            login.ShowDialog();
            if (login.Cikis)
            {
                Application.Exit();
                return;
            }
            Bilgileri_Doldur();
        }

        CheckButton btnSecilen;
        private void Split_CheckedChanged(object sender, EventArgs e)
        {
            if (btnSecilen == null)
            {
                btnSecilen = (CheckButton)sender;

                cbtn_0.Checked = false;
                cbtn_1.Checked = false;
                cbtn_2.Checked = false;
                cbtn_3.Checked = false;
                cbtn_4.Checked = false;
                cbtn_5.Checked = false;
                cbtn_6.Checked = false;
                cbtn_7.Checked = false;
                cbtn_8.Checked = false;
                cbtn_9.Checked = false;

                cbtn_0.ForeColor = Color.Black;
                cbtn_1.ForeColor = Color.Black;
                cbtn_2.ForeColor = Color.Black;
                cbtn_3.ForeColor = Color.Black;
                cbtn_4.ForeColor = Color.Black;
                cbtn_5.ForeColor = Color.Black;
                cbtn_6.ForeColor = Color.Black;
                cbtn_7.ForeColor = Color.Black;
                cbtn_8.ForeColor = Color.Black;
                cbtn_9.ForeColor = Color.Black;

                Split = Convert.ToInt32(btnSecilen.Text == "Tümü" ? "0" : btnSecilen.Text);
                Splitad = Convert.ToString(btnSecilen.Tag);
                btnSecilen.Checked = true;
                btnSecilen.ForeColor = Color.Red;

                btnSecilen = null;

                //Split_Ayarla();
                gridyenile();
            }

        }

        private void btn_SplitBol_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(bartxt_FisNo.EditValue) > 0)
            {
                timer1.Enabled = false;

                Split_Bol bol = new Split_Bol();
                bol.Fisno = Convert.ToInt32(bartxt_FisNo.EditValue);
                bol.Masa_No = Masa_No;
                bol.ShowDialog();

                timer1.Enabled = true;
            }
            else
            {
                MessageBox.Show(res_man.GetString("Masa Bilgisi Bulunamadı..."), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btn_Rapor_Click(object sender, EventArgs e)
        {
            Raporlar r = new Raporlar();
            r.ShowDialog();
            MasaYenile(0);
        }

        private void btn_MalzemeTransfer_Click(object sender, EventArgs e)
        {
            MalzemeTransfer();
        }

        private void MalzemeTransfer()
        {
            if (StatikSinif.masaMusaitmi(Masa_No) == false) return;

            if (Convert.ToInt32(bartxt_FisNo.EditValue) > 0 && Masa_No != String.Empty)
            {
                if (Param.Param_Masaacan_Garson && !User.M_BaskaMasa)
                {
                    string garson = dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Garson from Cst_Recete_Satis where Rsat_Ba = 'B' and Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue).ToString() + "' order by Rsat_Id),'')");
                    if (garson != User.P_Kod && garson != "")
                    {
                        MessageBox.Show(res_man.GetString("Masayı ") + garson + " - " + User.Isim_Getir(garson) + res_man.GetString(" Açmıştır.Başkası Satış Yapamaz...!!!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }

                timer1.Enabled = false;
                Malzeme_Tr malz = new Malzeme_Tr();
                malz.txt_Masano.EditValue = Masa_No;
                malz.kaynakFisno = Convert.ToInt32(bartxt_FisNo.EditValue);
                malz.ShowDialog();
                MasaYenile(0);
                if (User.M_SatisRelogin)
                {
                    Relogin();
                }
                timer1.Enabled = true;
            }
            else
            {
                MessageBox.Show(res_man.GetString("Masa Bilgisi Bulunamadı..."), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void MasaTakip_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.F)
            {
                Urun_Detay detay = new Urun_Detay();
                detay.Show();
            }

            if (User.Pos_MasaAnlikDurum)
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    AnlikDurum detay = new AnlikDurum();
                    detay.Show();
                }
            }
        }

        private void gridView2_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (e.Column == gridColumn3)
            {
                e.Info.DisplayText = res_man.GetString("Toplam : ") + Convert.ToDecimal(gridColumn6.SummaryItem.SummaryValue).ToString("N2");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            KisiSayisiDegistir();

        }

        private void KisiSayisiDegistir()
        {
            timer1.Enabled = false;
            DataTable dtKisi = dbtools.SelectTable("select Rsat_Kisi from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + bartxt_FisNo.EditValue + "'");
            if (dtKisi.Rows.Count <= 0)
            {
                return;
            }

            Klavye1 klavye = new Klavye1();
            klavye.Tag = "KISISAYISI";
            klavye.txt_Sayi.Text = Convert.ToInt32(dtKisi.Rows[0]["Rsat_Kisi"]).ToString();
            klavye.ShowDialog();
            int Kisi_Sayisi = Convert.ToInt32(klavye.sayi);

            Fis_Islem.Kisi_Sayisi(Convert.ToInt32(bartxt_FisNo.EditValue), Kisi_Sayisi);

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Kisi_Sayisi, Log.Log_Islem.Duzelt, "Fis No:" + bartxt_FisNo.EditValue.ToString() + " Kisi Sayısı Duzeltildi.Eski Kisi: " + Convert.ToInt32(dtKisi.Rows[0]["Rsat_Kisi"]).ToString() + " Yeni Kisi: " + Kisi_Sayisi.ToString(), bartxt_FisNo.EditValue.ToString(), "");

            MasaYenile(0);
            timer1.Enabled = true;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Yenile_DoubleClick(object sender, EventArgs e)
        {
            MasaYenile(1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MasaYenile(0);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MasaYenile(1);
        }

        private void btn_Yenile_Click(object sender, EventArgs e)
        {
            MasaYenile(0);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Relogin();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Raporlar r = new Raporlar();
            r.ShowDialog();
            MasaYenile(0);
            Main.a.Listele(0);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MasaYenile(0);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OzelMasa();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MasaTransfer();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MasaIslemler();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            KisiSayisiDegistir();
        }

        private void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            



            HesapBak();
            Main.a.Listele(0);

            txt_Filtre.Text = "";
        }

        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            satisGit();
        }

        public void satisGit()
        {
            if (StatikSinif.masaMusaitmi(Masa_No) == false) return;

            string parcaliAktif = dbtools.DegerGetir("select count(*) as toplam from Pos_Param where ISNULL(Param_ParcaliMasaAktif,0)=1");

            if (parcaliAktif.Equals("1"))
            {
                string query = "select count(*) as toplam from Pos_Masa where Masa_Depart='" + Departman.Dep_Kodu + "' and Masa_Durum='1' and Masa_No like '" + Masa_No + "[_]%' ";
                string parcalimi = dbtools.DegerGetir(query);

                if (!parcalimi.Equals("0"))
                {
                    RHMesaj.MyMessageInformation("Masa parçalanmıştır. Ana masaya giremezsiniz!");
                    return;
                }
            }



            Satis();
            Main.a.Listele(0);
            txt_Filtre.Text = "";
        }

        private void bar_PaketSatis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PaketCallCenter c = new PaketCallCenter();
            c.ShowDialog();
        }

        private void bar_Direksatis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Satis sat = new Satis();
            sat.Masa_No = User.P_Sabit_Masa;
            sat.Tag = "D";
            sat.ShowDialog();
        }

        private void bar_MalzemeTransfer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MalzemeTransfer();
        }

        private void barButtonItem3_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OzelMasa();
        }



        private void barButtonItem3_ItemClick_2(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Oda_Kontrol oda = new Oda_Kontrol();
            oda.Show();
        }

        private void txt_Filtre_EditValueChanged(object sender, EventArgs e)
        {
            string text = txt_Filtre.Text;
            if (text.Length == 0)
            {
                MasaYenile(0, "");
                return;
            }
            else if (text.Length > 1)
            {
                string filtre = "and ( Masa_Ad like '%" + text + "%' or  Masa_Ozel like '%" + text + "%' )";
                MasaYenile(0, filtre);
            }
        }

        private void btnFilterClear_Click(object sender, EventArgs e)
        {
            txt_Filtre.Text = "";
        }

        private void txt_Filtre_Click(object sender, EventArgs e)
        {
            Klavye2 klavye = new Klavye2(1);
            klavye.ShowDialog();
            txt_Filtre.Text = klavye.yazi;
        }

        public void setFilter(string text)
        {
            txt_Filtre.Text = text;
        }

        private void MasaTakip_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.masa_takip = null;
            timer1.Stop();
            timer1.Enabled = false;
        }


        public static string MyClass = "MasaTakip";

        AyarlarController ayarlar = new AyarlarController();
        private void Yuvarlama()
        {
            try
            {
                YuvarlaModel model = ayarlar.getYuvarlama(Departman.Dep_Kodu);
                if (model != null && model.yuvarlamaFiyat > 0)
                {

                    if (toplamTutar % 1 == 0 || toplamTutar % 1 == (decimal)0.5)
                    {
                        return;
                    }

                    string fisno = bartxt_FisNo.EditValue.ToString();

                    dbtools.execcmd("Delete From Cst_Recete_Satis where Rsat_Fisno = '" + fisno + "' and Rsat_Recete = '" + model.yuvarlamaRecete + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");


                    Satis s = new Satis();
                    s.Tag = "M";
                    s.bartxt_FisNo.EditValue = fisno;
                    s.Miktar = 1;
                    s.Masa_No = Masa_No;
                    decimal artik = toplamTutar % (model.yuvarlamaFiyat / 100);
                    s.Yuv_Tutar = (model.yuvarlamaFiyat / 100) - artik;
                    s.Urun_Sat(model.yuvarlamaRecete, siparisPr: true);
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "Yuvarlama", "", ex);
            }
        }

        public void otoIndirimYuvarlama()
        {
            try
            {
                AyarlarController ayarlar = new AyarlarController();
                IndirimModel model = ayarlar.getIndirimModel();
                if (model.aktif == false)
                {
                    return;
                }

                //decimal toplamTutar = Convert.ToDecimal(gridView1.Columns["Rsat_Tutar"].SummaryItem.SummaryValue);

                decimal virguldensonrakiSayi = toplamTutar % 1;

                if (virguldensonrakiSayi > 0)
                {
                    int fisno = Convert.ToInt32(bartxt_FisNo.EditValue.ToString());

                    Fis_Islem.Manuel_Indirim(fisno, "T", virguldensonrakiSayi, virguldensonrakiSayi, 0, 0);

                    string aciklama = "OTOMATİK İNDİRİM UYGULANDI . Fisno:" + fisno + " Masano:" + Masa_No + " İNDİRİM TUTARI : " + toplamTutar + " İNDİRİMSİZ TOPLAM TUTAR : " + toplamTutar;


                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Indirim_Uygula, Log.Log_Islem.Kaydet, aciklama, fisno.ToString(), "");

                    gridyenile();
                }

            }
            catch (Exception ex)
            {
                // RHMesaj.MyMessageError(MyClass, "otoIndirimYuvarlama", "", ex);
            }
        }

        //private void Yuvarlama()
        //{
        //    try
        //    {

        //        if (Param.Param_Yuvarla != "" && Convert.ToDecimal(Param.Param_Yuv_Sayi) > 0)
        //        {
        //            dbtools.execcmd("Delete From Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Recete = '" + Param.Param_Yuvarla + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");

        //            Satis s = new Satis();
        //            s.Tag = "M";
        //            s.bartxt_FisNo.EditValue = Convert.ToInt32(this.Tag);
        //            s.Miktar = 1;
        //            s.Masa_No = Masa_No;
        //            decimal artik = toplamTutar % (Convert.ToDecimal(Param.Param_Yuv_Sayi) / 100);
        //            s.Yuv_Tutar = (Convert.ToDecimal(Param.Param_Yuv_Sayi) / 100) - artik;
        //            s.Urun_Sat(Param.Param_Yuvarla);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        RHMesaj.MyMessageError(MyClass, "Yuvarlama", "", ex);
        //    }
        //}


        public bool isYazdirilmamisSiparis()
        {
            try
            {
                string deger = dbtools.DegerGetir("select count(Rsat_SiparisPr) as toplam from Cst_Recete_Satis where Rsat_Fisno='" + bartxt_FisNo.EditValue.ToString() + "' and (Rsat_SiparisPr='0' and isnull(Rsat_Mars,0)=0) and Rsat_Ba='B'");

                if (Convert.ToInt32(deger) > 0)
                {
                    MessageBox.Show("Yazdırılmamış sipariş vardır!");
                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA " + ex.Message);
            }

            return false;

        }

        public void HesapDokum() // buraya bak
        {
            try
            {

                if (isYazdirilmamisSiparis())
                {
                    return;
                }

                if (StatikSinif.masaMusaitmi(Masa_No) == false) return;

                Yuvarlama();
                otoIndirimYuvarlama();

                int fisno = Convert.ToInt32(bartxt_FisNo.EditValue.ToString());
                FisPr pr = new FisPr();
                if (Param.Param_YeniHesapDkm)
                {
                    pr.newHesapDokum(true, fisno, Split, "* * * HESAP DÖKÜM FİŞİ * * *");
                }
                else
                {
                    pr.HesapDokum(true, fisno, Split);
                }

                dbtools.execcmd("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Ingenico_Status='1' where Rsat_Fisno='" + fisno + "'");

                if (Param.Param_Hesap_Kilit)
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Hesap_Kilit = 1 where Rsat_Fisno = '" + fisno + "'");
                }

                if (User.M_SatisRelogin)
                {
                    Relogin();
                }

                MasaYenile(0);
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "HesapDokum", "", ex);
            }
        }
        private void btnHesapDokum_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HesapDokum();

        }

        private void btnHesapDokumEski_Click(object sender, EventArgs e)
        {
            HesapDokum();
        }

        private void bar_Yenile_ItemDoubleClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MasaYenile(1);
        }

        private void btnMasaKilitAc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int Fisno = Convert.ToInt32(bartxt_FisNo.EditValue.ToString());
            if (Fisno > 0)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Hesap_Kilit = 0 where Rsat_Fisno = '" + Fisno + "'");
                dbtools.execcmd("update Pos_Masa set Masa_Durum = '1' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "' ");
                MasaYenile(0);
            }
        }

        private void btnParcaliMasa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            parcaliMasaIlk();
        }

        public void parcaliMasaIlk()
        {
            if (Convert.ToInt32(bartxt_FisNo.EditValue) != 0)
            {
                RHMesaj.MyMessageInformation("DOLU MASA PARÇALANAMAZ!");
                return;
            }

            string parcaliAktif = dbtools.DegerGetir("select count(*) as toplam from Pos_Param where ISNULL(Param_ParcaliMasaAktif,0)=1");

            if (parcaliAktif.Equals("1"))
            {
                if (Masa_No == String.Empty)
                {
                    MessageBox.Show(res_man.GetString("Masa Seçiniz..."), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Masa_No.Contains("_"))
                {
                    MessageBox.Show("Parçalı masa parçalanamaz!");
                    return;
                }

                parcaliMasaYap();

            }
            else
            {
                RHMesaj.MyMessageInformation("Parçalı masa aktif değil\nParametreler -> Genel Parametre 3 -> Parçalı Masa Aktif");
            }
        }


        public void parcaliMasaYap()
        {
            try
            {
                timer1.Enabled = false;
                int kisiSayisi = getKisiSayisi() + 1;
                for (int i = 1; i < kisiSayisi; i++) // kişi sayısı kadar dön
                {
                    parcaliMasaYap1(i);
                }

                MasaYenile(0);

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "parcaliMasaYap", "", ex);
            }
            finally
            {
                timer1.Enabled = true;

            }
        }

        public void parcaliMasaYap1(int sayi)
        {
            try
            {
                Klavye2 klavye = new Klavye2();
                klavye.Fisno = Convert.ToInt32(bartxt_FisNo.EditValue);
                klavye.txt_Yazi.Text = Masa_OzelAdi;
                klavye.baslik = sayi + ". Kişinin ismini giriniz";
                klavye.ShowDialog();
                if (klavye.cikis == true)
                {

                    string yeniMasaNo = Masa_No + "_" + sayi;

                    Ozel_Masa = yeniMasaNo + " " + klavye.yazi;


                    dbtools.execcmd("update Pos_Masa set Masa_Ozel = '" + Ozel_Masa + "', Masa_Durum = 1 where Masa_Parcali='1' and Masa_No = '" + yeniMasaNo + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

                    dbtools.execcmd("update Pos_Masa set Masa_Durum = 1 where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

                    Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Ozel_Masa, Log.Log_Islem.Kaydet, yeniMasaNo + " NOLU Masa Parçalı Özel Masa Yapıldı " + Ozel_Masa, Convert.ToString(bartxt_FisNo.EditValue), String.Empty);

                    Ozel_Masa = String.Empty;

                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "parcaliMasaYap1", "", ex);
            }
        }

        public int getKisiSayisi()
        {
            int kisiSayisi = 0;
            Kisi_Garson sor = new Kisi_Garson();
            sor.Tag = this.Tag;
            sor.ShowDialog();
            kisiSayisi = Convert.ToInt32(sor.Kisi);
            //if (sor.Iptal)
            //{
            //    this.Close();
            //}

            return kisiSayisi;
        }

    }
}