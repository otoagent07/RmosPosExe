using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pos
{
    public partial class MasaSecForm : Form
    {
        string Masa_No = String.Empty;
        string Ozel_Masa = String.Empty;
        int Split = 0;
        string Splitad = "";
        bool Masa_Paket = false;
        string Masa_Konum = "";
        string Masa_OzelAdi = string.Empty;
        int Masa_Durum = 0;
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        public MasaSecForm()
        {
            InitializeComponent();
        }
        private void MasaSecForm_Load(object sender, EventArgs e)
        {
            MasaYenileIlkAcilis(0);
        }
        private void MasaYenileIlkAcilis(int Acik, string filtre = "") // orjinali hiç değişmedi aa
        {
            try
            {
                StatikSinif.masaKilitAc();
                Cursor.Current = Cursors.WaitCursor;
                flp_Masa.Controls.Clear();
                Masa_No = String.Empty;
                Masa_Paket = false;
                Split = 0;
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
                            + " ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk, (Max(Pos_Rez.Rez_Adi) + ' ' +  Max(Pos_Rez.Rez_Soyadi)) as RezMasaAdi,(Convert(nvarchar(max),DATEDIFF(MINUTE,MIN(Rsat_Acilis),Convert(time,Getdate()))) + ' DK') as dak "
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
                                 + " ISNULL(Pkod_Bosrenk,'#00FF00') as Pkod_Bosrenk,ISNULL(Pkod_Dolurenk,'#FF4500') as Pkod_Dolurenk,ISNULL(Pkod_Hesaprenk,'#BA55D3') as Pkod_Hesaprenk, (Max(Pos_Rez.Rez_Adi) + ' ' +  Max(Pos_Rez.Rez_Soyadi)) as RezMasaAdi,(Convert(nvarchar(max),DATEDIFF(MINUTE,MIN(Rsat_Acilis),Convert(time,Getdate()))) + ' DK') as dak "
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
                                if (Param.Doviz_Kodu.Contains("USD"))
                                {
                                    TL = TL + " $";
                                }
                                else
                                {
                                    TL = TL + " €"; // €  £
                                }
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
                    btnMasa.Text += "\n" + Convert.ToString(dtMasa.Rows[i]["dak"]);
                    btnMasa.Click += new EventHandler(btnMasa_Click);
                    btnMasa.Name = btnMasa.Tag.ToString();
                    flp_Masa.Controls.Add(btnMasa);
                }
                //Split_Ayarla();
                DataTable dtBilgi = dbtools.SelectTable("select COUNT(Masa_Durum) as Sayi,MIN(Masa_Durum) as Durum from Pos_Masa WITH(NOLOCK) where Masa_Depart = '" + Departman.Dep_Kodu + "' group by Masa_Durum order by Masa_Durum");


                if (Param.ikinciEkranAktif==false)
                {
                    Main.satislistesi_ikinci_ekran.Listele(0);
                }
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
        public static string MyClass = "MasaSecForm";
        private void btnMasa_Click(Object sender, EventArgs e)
        {
            SimpleButton myButton = (SimpleButton)sender;
            Masa_No = myButton.Tag.ToString();
            DataTable dtMasa = dbtools.SelectTable("Select isnull(Masa_Paket,0) as Masa_Paket,isnull(Masa_Ozel,'') as Masa_Ozel, isnull(Masa_Durum,0) as Masa_Durum From Pos_Masa WITH(NOLOCK) where Masa_Depart = '" + Departman.Dep_Kodu + "' and Masa_No = '" + Masa_No + "'");
            if (dtMasa.Rows.Count > 0)
            {
                Masa_Paket = Convert.ToBoolean(dtMasa.Rows[0]["Masa_Paket"]);
                Masa_OzelAdi = Convert.ToString(dtMasa.Rows[0]["Masa_Ozel"]);
                Masa_Durum = Convert.ToInt32(dtMasa.Rows[0]["Masa_Durum"]);
            }

            int fisno = Convert.ToInt32(dbtools.DegerGetir("exec Pos_Sorgu @Sorgu_Tipi = 4, @Dep_Kodu = '" + Departman.Dep_Kodu + "',@Masano = '" + Masa_No + "'"));

            if (Param.ikinciEkranAktif==false)
            {
                Main.satislistesi_ikinci_ekran.Listele(fisno);
            }


            // satış ekranı yenileme
            MasaTakip.satis.Tag = "M";
            if (Masa_No == String.Empty)
            {
                MessageBox.Show(res_man.GetString("Masa Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            MasaTakip.satis.Masa_No = Masa_No;
            MasaTakip.satis.Masa_Paket = Masa_Paket;
            MasaTakip.satis.Ozel_Masa = Ozel_Masa;
            MasaTakip.satis.Split = Split;
            MasaTakip.satis.Splitad = Splitad;
            MasaTakip.satis.Masa_Durum = Masa_Durum;// 1

            MasaTakip.satis.load(urunleriYenile:false);
            MasaTakip.satis.shownAc();

            this.Close();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
