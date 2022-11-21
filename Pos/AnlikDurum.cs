using Pos.Class;
using System;
using System.Data;
using System.Reflection;
using System.Resources;

namespace Pos
{
    public partial class AnlikDurum : DevExpress.XtraEditors.XtraForm
    {
        public AnlikDurum()
        {
            InitializeComponent();
        }


        string AcikCekler = "", KapaliCekler = "", ServisCekler = "", KuverCekler = "", ToplamCekler = "";

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FisPr a = new FisPr();
            a.AnlikPr(gdc_Anlik);
        }

        int AcikSayi = 0, KapaliSayi = 0, ToplamSayi = 0, ServisSayisi = 0, KuverSayisi = 0, ParamServisSayisi = 0;
        decimal AcikTutar = 0, KapaliTutar = 0, ToplamTutar = 0, ServisTutar = 0, KuverTutar = 0, ParamServisTutar = 0;

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void AnlikDurum_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable("AnlikDurum");
            dt.Columns.Add("Durum", typeof(string));
            dt.Columns.Add("Sayisi", typeof(decimal));
            dt.Columns.Add("Tutar", typeof(decimal));

            DataTable dtAcik = dbtools.SelectTable("Exec Pos_Sorgu @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Tarih1 = '" + Param.Tarih + "', @Sorgu_Tipi = '38'");

            if (dtAcik.Rows.Count > 0)
            {
                AcikCekler = res_man.GetString("Acik Çekler Sayısı ve Tutarı");
                AcikSayi = Convert.ToInt32(dtAcik.Rows[0]["Sayisi"]);
                AcikTutar = Convert.ToString(dtAcik.Rows[0]["Tutar"]) == "" ? 0 : Convert.ToDecimal(dtAcik.Rows[0]["Tutar"]);
            }
            else
            {
                AcikCekler = "";
                AcikSayi = 0;
                AcikTutar = 0;
            }

            DataTable dtKapali = dbtools.SelectTable("Exec Pos_Sorgu @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Tarih1 = '" + Param.Tarih + "', @Sorgu_Tipi = '37'");
            if (dtKapali.Rows.Count > 0)
            {
                KapaliCekler = res_man.GetString("Kapalı Çekler Sayısı ve Tutarı");
                KapaliSayi = Convert.ToInt32(dtKapali.Rows[0]["Sayisi"]);
                KapaliTutar = Convert.ToString(dtKapali.Rows[0]["Tutar"]) == "" ? 0 : Convert.ToDecimal(dtKapali.Rows[0]["Tutar"]);
            }
            else
            {
                KapaliCekler = "";
                KapaliSayi = 0;
                KapaliTutar = 0;
            }


            ToplamCekler = res_man.GetString("Toplam Çekler Sayısı ve Tutarı");
            ToplamSayi = AcikSayi + KapaliSayi;
            ToplamTutar = AcikTutar + KapaliTutar;

            DataTable dtServis = dbtools.SelectTable(@"select
                                COUNT(Rsat_Recete) as Sayisi,
                                SUM(ISNULL(Rsat_Tutar,0)) as Tutar
                                from Cst_Recete_Satis
                                where Rsat_Tarih = '" + Param.Tarih + "' and Rsat_Recete = '" + Departman.Kodlar_Servis_Recete + @"' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");

            if (dtServis.Rows.Count > 0)
            {
                ServisCekler = res_man.GetString("Servis Payı Sayısı ve Tutarı");
                ServisSayisi = Convert.ToInt32(dtServis.Rows[0]["Sayisi"]);
                ServisTutar = Convert.ToString(dtServis.Rows[0]["Tutar"]) == "" ? 0 : Convert.ToDecimal(dtServis.Rows[0]["Tutar"]);

            }
            else
            {
                ServisSayisi = 0;
                ServisTutar = 0;
            }


            if (dtServis.Rows.Count == 0)
            {



                DataTable dtServisParam = dbtools.SelectTable(@"
                              
                                select
                                COUNT(Rsat_Recete) as Sayisi,
                                SUM(ISNULL(Rsat_Tutar,0)) as Tutar
                                from Cst_Recete_Satis
                                where Rsat_Tarih = '" + Param.Tarih + "' and Rsat_Recete = '" + Param.Param_Bindirim + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'");

                if (dtServisParam.Rows.Count > 0)
                {
                    ParamServisSayisi = Convert.ToInt32(dtServisParam.Rows[0]["Sayisi"]);
                    ParamServisTutar = Convert.ToString(dtServisParam.Rows[0]["Tutar"]) == "" ? 0 : Convert.ToDecimal(dtServisParam.Rows[0]["Tutar"]);

                }
                else
                {
                    ParamServisSayisi = 0;
                    ParamServisTutar = 0;
                }

            }
            DataTable dtKuver = dbtools.SelectTable(@"select
                                COUNT(Rsat_Recete) as Sayisi,
                                SUM(ISNULL(Rsat_Tutar,0)) as Tutar
                                from Cst_Recete_Satis
                                where Rsat_Tarih = '" + Param.Tarih + "' and Rsat_Recete = '" + Departman.Kodlar_Kuver_Recete + "'");

            if (dtKuver.Rows.Count > 0)
            {
                KuverCekler = res_man.GetString("Kuver Sayısı ve Tutarı ");
                KuverSayisi = Convert.ToInt32(dtKuver.Rows[0]["Sayisi"]);
                KuverTutar = Convert.ToString(dtKuver.Rows[0]["Tutar"]) == "" ? 0 : Convert.ToDecimal(dtKuver.Rows[0]["Tutar"]);
            }
            else
            {
                KuverSayisi = 0;
                KuverTutar = 0;
            }

            dt.Rows.Add(AcikCekler, dtAcik.Rows[0][1], dtAcik.Rows[0][2]);
            dt.Rows.Add(KapaliCekler, dtKapali.Rows[0][1], dtKapali.Rows[0][2]);
            dt.Rows.Add(ToplamCekler, ToplamSayi, ToplamTutar);



            {
                dt.Rows.Add(ServisCekler, ServisSayisi + ParamServisSayisi, ServisTutar + ParamServisTutar);
            }

            if (Departman.Kodlar_Kuver_Sat)
            {
                dt.Rows.Add(KuverCekler, dtKuver.Rows[0][0], dtKuver.Rows[0][1]);
            }
            dt.Rows.Add(null, null, null);

            DataTable Odeme = dbtools.SelectTable("Exec Pos_Satis_Rapor @Departman = '" + Departman.Dep_Kodu + "', @Tarih1 = '" + Param.Tarih + "', @Rapor_Tipi = 28");

            decimal Gelir = 0;

            for (int i = 0; i < Odeme.Rows.Count; i++)
            {
                dt.Rows.Add(Odeme.Rows[i]["KapamaAd"].ToString().PadRight(35 - Odeme.Rows[i]["KapamaAd"].ToString().Length), Odeme.Rows[i]["Rsat_Tutar"].ToString(), null);
                Gelir += Convert.ToDecimal(Odeme.Rows[i]["Rsat_Tutar"]);
            }

            dt.Rows.Add(null, null, null);

            DataTable dtKisiSayisi = dbtools.SelectTable("exec Pos_Satis_Rapor @Rapor_Tipi=21,@Tarih1='" + Param.Tarih + "',@Tarih2='" + Param.Tarih + "',@Departman=N'" + Departman.Dep_Kodu + "',@Kullanici=N'" + User.P_Kod + "'");

            if (dtKisiSayisi!=null)
            {
                if (dtKisiSayisi.Rows.Count > 1)
                {
                    DataRow[] dr = dtKisiSayisi.Select("Tarih = '" + Param.Tarih.ToString("yyyy-MM-dd") + "'");
                    DataTable dts = dr.CopyToDataTable();

                    for (int i = 0; i < dts.Rows.Count; i++)
                    {
                        for (int j = 0; j < dts.Columns.Count; j++)
                        {
                            if (Convert.ToString(dts.Columns[j].Caption) == "MasaSayi")
                            {
                                dt.Rows.Add("Masa Sayisi : ".PadRight(25, ' '), dts.Rows[i]["MasaSayi"], null);

                            }

                            if (Convert.ToString(dts.Columns[j].Caption) == "MasaOrtalamasi")
                            {
                                dt.Rows.Add("Masa Ortalamasi : ".PadRight(21, ' '), dts.Rows[i]["MasaOrtalamasi"], null);

                            }

                            if (Convert.ToString(dts.Columns[j].Caption) == "KisiSayisi")
                            {
                                dt.Rows.Add("Kişi Sayısı : ".PadRight(28, ' '), dts.Rows[i]["KisiSayisi"], null);

                            }

                            if (Convert.ToString(dts.Columns[j].Caption) == "KisiOrtalamasi")
                            {
                                dt.Rows.Add("Kişi Ortalaması : ".PadRight(24, ' '), dts.Rows[i]["KisiOrtalamasi"], null);
                                //hesap.Add("");
                            }
                        }
                    }
                }

            }


            dt.Rows.Add(null, null, null);

            dt.Rows.Add(null, null, null);

            //DataTable dtDep = (dbtools.SelectTable("exec Pos_Satis_Rapor @Rapor_Tipi = '13', @Departman = '" + Departman.Dep_Kodu + "', @Tarih1 = '" + Param.Tarih + "', @Tarih2 = '" + Param.Tarih + "'"));

            //if (dtDep.Rows.Count > 0)
            //{
            dt.Rows.Add("TOPLAM DEPARTMAN GELİRİ", Gelir, 0);
            //}

            gdc_Anlik.DataSource = dt;

        }
    }
}