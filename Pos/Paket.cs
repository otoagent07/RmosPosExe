using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Newtonsoft.Json;
using Pos.Class;
using Pos.Controllers;
using Pos.Print;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Paket : DevExpress.XtraEditors.XtraForm
    {

        string Masa_No = "";
        //int Fisno = 0;


        public static Paket paketForm;

        public Paket()
        {
            if (Param.Param_FullPos == true)
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
            InitializeComponent();
        }

        private void Paket_Load(object sender, EventArgs e)
        {
            //Param.Param_Yukle();



            paketForm = this;
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            dateTarih1.EditValue = Param.Tarih;
            dateTarih2.EditValue = Param.Tarih;
            dateEdit1.EditValue = Param.Tarih;

            DataTable dt = dbtools.SelectTable("select P_Kod,(P_Ad + ' ' + P_Soyad) as Adsoyad from Rmosmuh.dbo.Pos_User where P_Kulturu = 2 order by P_Kod");
            if (dt.Rows.Count > 0)
            {
                look_Paketci.Properties.DataSource = dt;
                look_Paketci.Properties.DisplayMember = "Adsoyad";
                look_Paketci.Properties.ValueMember = "P_Kod";
            }


            if (Param.Param_PaketDipTotal)
            {
                gridColumn8.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            }

            xtraTabControl1_SelectedPageChanged(null, null);


            simpleButton10.Visible = User.G_Odemeal;

            string fileName = getDizaynPath();
            if (File.Exists(fileName))
            {
                gridView1.RestoreLayoutFromXml(fileName);
            }


            string fileName2 = getDizaynPath_kp();
            if (File.Exists(fileName2))
            {
                gridView2.RestoreLayoutFromXml(fileName2);
            }



            btnDirektSatis.Visible = User.D_Direksatis;


            labelSecili.Text = "";

            panel1.Visible = !Param.Param_StokAnlikAtmasin;

            btnOdemeTipiDegistir.Visible = User.Pos_OdemeDegistir;
        }



        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        CheckButton chktip = null;
        private void Tip_Changed(object sender, EventArgs e)
        {
            CheckButton chkbtn = (CheckButton)sender;
            if (chktip == null)
            {
                chktip = chkbtn;
            }

            if (chktip == chk_PaketSatis)
            {
                chk_PaketSatis.Checked = true;
                chk_CallerId.Checked = false;
                chk_KayitliTel.Checked = false;
                chk_KapaliPaket.Checked = false;

                xtraTabControl1.SelectedTabPage = tab_Paket;
            }
            if (chktip == chk_CallerId)
            {
                chk_PaketSatis.Checked = false;
                chk_CallerId.Checked = true;
                chk_KayitliTel.Checked = false;
                chk_KapaliPaket.Checked = false;

                xtraTabControl1.SelectedTabPage = tab_CallerId;
            }
            if (chktip == chk_KayitliTel)
            {
                chk_PaketSatis.Checked = false;
                chk_CallerId.Checked = false;
                chk_KayitliTel.Checked = true;
                chk_KapaliPaket.Checked = false;

                xtraTabControl1.SelectedTabPage = tab_KayitliTel;
            }
            if (chktip == chk_KapaliPaket)
            {
                chk_PaketSatis.Checked = false;
                chk_CallerId.Checked = false;
                chk_KayitliTel.Checked = false;
                chk_KapaliPaket.Checked = true;

                xtraTabControl1.SelectedTabPage = tab_KapaliPaket;
            }
            chktip = null;

            //if (chk_PaketSatis.Checked)
            //{
            //    gridyenile_1();
            //}
        }

        int sonsecili = 0;
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == tab_Paket)
            {
                Kapatma_Yenile();
                gridyenile_1();
            }
            if (xtraTabControl1.SelectedTabPage == tab_KayitliTel)
            {
                gridyenile_3();
            }
        }




        #region Paket Satis
        private void Kapatma_Yenile()
        {
            flp_Kapatma.Controls.Clear();
            DataTable dt = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad,isnull(Pkod_OdemeBtnRenk,'') as renk  from Pos_Kodlar with(nolock) where Pkod_OdemeAktif=1 and Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' and Pkod_Ozelkod <> '8' order by Pkod_Sira");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    SimpleButton btn_Kapatma = new SimpleButton();
                    btn_Kapatma.Size = new System.Drawing.Size(100, 50);
                    btn_Kapatma.TabIndex = 0;
                    btn_Kapatma.TabStop = false;
                    btn_Kapatma.Font = new System.Drawing.Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    btn_Kapatma.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btn_Kapatma.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Kapatma.Appearance.Options.UseBackColor = true;

                    btn_Kapatma.Text = Convert.ToString(dt.Rows[i]["Pkod_Ad"]);
                    btn_Kapatma.Tag = Convert.ToString(dt.Rows[i]["Pkod_Kod"]);
                    string renk = dt.Rows[i]["renk"].ToString();
                    if (renk != "")
                    {
                        btn_Kapatma.Appearance.BackColor = System.Drawing.ColorTranslator.FromHtml(renk);
                    }

                    btn_Kapatma.Click += new EventHandler(btn_Kapatma_Click);
                    flp_Kapatma.Controls.Add(btn_Kapatma);
                }
            }

        }

        void btn_Kapatma_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {

                if (gridView1.GetSelectedRows().Length == 0)
                {
                    RHMesaj.alertMesaj("Lütfen satırı işaretle");
                    return;
                }


                foreach (var rowHandle in gridView1.GetSelectedRows())
                {
                    var Fisno = gridView1.GetRowCellValue(rowHandle, "Rsat_Fisno").ToString();
                    Masa_No = gridView1.GetRowCellValue(rowHandle, "Masa_No").ToString();

                    SimpleButton btn = (SimpleButton)sender;

                    Hesap hes = new Hesap();
                    hes.Tag = Fisno;
                    hes.Masa_No = Masa_No;
                    hes.acaracmazyazdirmadankapat = Param.paketotohesapkapat;
                    hes.look_Kapatma.EditValue = btn.Tag.ToString();
                    if (Param.Param_Hesap_Disable) hes.look_Kapatma.Enabled = false;
                    hes.ShowDialog();
                }



            }

            gridyenile_1();

        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {

                Masa_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Masa_No"));

                Satis satis = new Satis();
                satis.Tag = "M";
                satis.Masa_No = Masa_No;
                satis.Masa_Paket = true;
                satis.Ozel_Masa = "";
                satis.Split = 0;
                satis.Splitad = "";
                satis.ShowDialog();
                gridyenile_1();
            }
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            yenipaket();
        }

        public Satis satis;
        public void yenipaket()
        {
            DataTable dtMasa = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1 and Masa_Durum = 0 order by Masa_No");

            if (dtMasa.Rows.Count < 1)
            {
                MessageBox.Show(res_man.GetString("Boş Paket Masanız Bulunmamaktadır."));
                return;
            }

            Masa_No = Convert.ToString(dtMasa.Rows[0]["Masa_No"]);

            satis = new Satis();
            satis.Tag = "M";
            satis.Masa_No = Masa_No;
            satis.Masa_Paket = true;
            satis.Ozel_Masa = "";
            satis.Split = 0;
            satis.Splitad = "";
            satis.ShowDialog();
            gridyenile_1();
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {

            if (gridView1.GetSelectedRows().Length == 0)
            {
                RHMesaj.alertMesaj("Lütfen check ile satırı işaretle");
                return;
            }




            Garson_Sor pkt = new Garson_Sor();
            pkt.Tag = "PAKET";
            pkt.fisno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Rsat_Fisno").ToString();
            pkt.ShowDialog();
            string Paketci = pkt.Garson_Kod;

            if (Paketci != "")
            {
                foreach (var rowHandle in gridView1.GetSelectedRows())
                {
                    var Rsat_Fisno = gridView1.GetRowCellValue(rowHandle, "Rsat_Fisno").ToString();
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Paketci = '" + Paketci + "'" +
                        ",paketAtamaTarih=getdate() " +
                        ",sepetDurum=1 " +
                        "where Rsat_Fisno = '" + Rsat_Fisno + "'");

                }

            }




            gridyenile_1();

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                string Fisno = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                Masa_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Masa_No"));

                Hesap hes = new Hesap();
                hes.Tag = Fisno;
                hes.Masa_No = Masa_No;
                hes.Split = 0;
                hes.Splitad = "";
                hes.ShowDialog();
                gridyenile_1();
            }
        }

        private void PaketciDegistir(int Fis_No)
        {
            Garson_Sor pkt = new Garson_Sor();
            pkt.Tag = "PAKET";
            pkt.ShowDialog();
            string Paketci = pkt.Garson_Kod;

            if (Paketci != "")
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Paketci = '" + Paketci + "' where Rsat_Fisno = '" + Fis_No + "'");
            }


        }


        private void btn_PaketPr_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                int Fisno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                Masa_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Masa_No"));

                FisPr pr = new FisPr();
                string sonuc = pr.PaketPr(Fisno, " * * * PAKET FİSİ * * * ");
                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc);
                }


                //dbtools.execcmd("update Cst_Recete_Satis set Rsat_Ingenico_Status='1' where Rsat_Fisno='" + Fisno + "'");

                string aciklama = "Fisno : " + Fisno + " . PAKET FİŞİ ALINDI. ";

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, aciklama, Convert.ToString(Fisno), "");


                dbtools.execcmdR($"update Cst_Recete_Satis set Rsat_Ingenico_Status=1 where Rsat_Fisno={Fisno}");


                dbtools.execcmd("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");


                if (Param.Param_Hesap_Kilit)
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Hesap_Kilit = 1 where Rsat_Fisno = '" + Fisno + "'");
                }

            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                int Fisno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));

                PaketNot not = new PaketNot();
                not.Fisno = Fisno;
                not.ShowDialog();
            }
        }

        private void btn_Yenile_Click(object sender, EventArgs e)
        {
            gridyenile_1();
        }
        private void gridyenile_1()
        {
            //            string query = @"
            //                select Rsat_Fisno,SUM(Rsat_Tutar) as Tutar
            //            into #Temp
            //            from Cst_Recete_Satis 
            //	            left join Pos_Masa on Rsat_Masa = Masa_No  and Masa_Depart = Rsat_Departman
            //	            left join Pos_Cari on Rsat_Cari = Cari_Kod 
            //	            left join Pos_Kodlar as OdemeKodu on OdemeKodu.Pkod_Kod = Rsat_Kapatma and OdemeKodu.Pkod_Sinif = '11'
            //          where Rsat_Ba = 'A'  and Rsat_Departman = '" + Departman.Dep_Kodu + @"' and OdemeKodu.Pkod_Ozelkod = '4'
            //            group by Rsat_Fisno


            //select  MAX(Rsat_Id) as Rsat_Id,Rsat_Tarih,Masa_No,Masa_Ad,Cst_Recete_Satis.Rsat_Fisno,MAX(Cari_Kod) as Cari_Kod,MAX(Cari_Ad) + ' ' + MAX(Cari_Soyad) as Cari_AdSoyad,
            //MAX(ISNULL(Cari_Adres1,'')) + ' ' + MAX(ISNULL(Cari_Adres2,'')) + ' ' + MAX(ISNULL(Cari_Adres3,'')) as Cari_Adres, 
            //	(SUM(Rsat_Tutar) - SUM(t.Tutar)) as Tutar, max(P_Kod) as P_Kod,max(P_Ad + ' ' + P_Soyad) as P_AdSoyad ,Rsat_Sube,sube.Pkod_Ad as subeAd,MAX(ISNULL(Rsat_SubeDurum,'')) as Rsat_SubeDurum
            //from Cst_Recete_Satis 
            //	left join Pos_Masa on Rsat_Masa = Masa_No  and Masa_Depart = Rsat_Departman
            //	left join Pos_Cari on Rsat_Cari = Cari_Kod 
            //	left join Rmosmuh.dbo.Pos_User on P_Kod = Rsat_Paketci 
            //	left join Pos_Kodlar as sube on sube.Pkod_Kod = Rsat_Sube and sube.Pkod_Sinif = '27'
            //    left join #Temp as t on t.Rsat_Fisno = Cst_Recete_Satis.Rsat_Fisno
            //where Rsat_Durum = 'A' and Masa_Konum = 'P' and Rsat_Departman = '" + Departman.Dep_Kodu + @"' 
            //and Rsat_Ba = 'B'
            //and Rsat_Tarih >= '" + dateTarih1.DateTime.Date + @"' and Rsat_Tarih <= '" + dateTarih2.DateTime.Date + @"'

            //group by Rsat_Tarih,Masa_No,Masa_Ad,Cst_Recete_Satis.Rsat_Fisno,Rsat_Sube,sube.Pkod_Ad ,t.Tutar
            //order by Cst_Recete_Satis.Rsat_Fisno desc

            //drop table #Temp

            //";


            switch (sonsecili)
            {
                case 0:
                    chk_PaketSatis.Checked = true;
                    chk_PaketciAtanmayanlar.Checked = false;
                    chk_PaketciAtananlar.Checked = false;

                    break;
                case 1:
                    chk_PaketciAtanmayanlar.Checked = true;
                    break;
                case 2:
                    chk_PaketciAtananlar.Checked = true;
                    break;
                default:
                    break;
            }


            string query = StatikModel.getPaketSqlText(dateTarih1, dateTarih2, Departman.Dep_Kodu, "", "A,K", true, atanmayanlar: chk_PaketciAtanmayanlar.Checked, atananlar: chk_PaketciAtananlar.Checked);

            var data = dbtools.SelectTableR(query);



            gridControl1.DataSource = data;
            //gridControl1.DataSource = dbtools.SelectTable(@"
            //          select  MAX(Rsat_Id) as Rsat_Id,Rsat_Tarih,Masa_No,Masa_Ad,Cst_Recete_Satis.Rsat_Fisno,MAX(Cari_Kod) as Cari_Kod,MAX(Cari_Ad) + ' ' + MAX(Cari_Soyad) as Cari_AdSoyad,
            //MAX(ISNULL(Cari_Adres1,'')) + ' ' + MAX(ISNULL(Cari_Adres2,'')) + ' ' + MAX(ISNULL(Cari_Adres3,'')) as Cari_Adres, 
            //	(SUM(Rsat_Tutar)) as Tutar, max(P_Kod) as P_Kod,max(P_Ad + ' ' + P_Soyad) as P_AdSoyad ,Rsat_Sube,sube.Pkod_Ad as subeAd,MAX(ISNULL(Rsat_SubeDurum,'')) as Rsat_SubeDurum
            //from Cst_Recete_Satis 
            //	left join Pos_Masa on Rsat_Masa = Masa_No  and Masa_Depart = Rsat_Departman
            //	left join Pos_Cari on Rsat_Cari = Cari_Kod 
            //	left join Rmosmuh.dbo.Pos_User on P_Kod = Rsat_Paketci 
            //	left join Pos_Kodlar as sube on sube.Pkod_Kod = Rsat_Sube and sube.Pkod_Sinif = '27'

            //where Rsat_Durum = 'A' and Masa_Konum = 'P' and Rsat_Departman = '" + Departman.Dep_Kodu + @"' 
            //and Rsat_Ba = 'B'
            //and Rsat_Tarih >= '" + dateTarih1.DateTime.Date + @"' and Rsat_Tarih <= '" + dateTarih2.DateTime.Date + @"'

            //group by Rsat_Tarih,Masa_No,Masa_Ad,Cst_Recete_Satis.Rsat_Fisno,Rsat_Sube,sube.Pkod_Ad 
            //order by Cst_Recete_Satis.Rsat_Fisno desc
            //");

            //gridView1.BestFitColumns();



            gridControl5.DataSource = dbtools.SelectTable("select 'Yeni Sipariş' as Data,* from Pos_CallCenter where ISNULL(Center_Pasif,0) = 0");

            son5call();

            seciliyaz();
        }

        public void son5call()
        {
            string query = $@"select top 10 Caller_Telno as tel,Caller_Carikod as kod,isnull((Cari_Ad+' '+Cari_Soyad),'') as adSoyad from Pos_CallerId poscari
left join Pos_Cari cari on cari.Cari_Kod=poscari.Caller_Carikod
order by Caller_Id desc";
            gridControlSon5Call.DataSource = dbtools.SelectTableR(query);
        }
        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridView5.RowCount == 0) return;

                string jsonSatis = Convert.ToString(gridView5.GetFocusedRowCellValue("Center_Data"));
                string jsonCari = Convert.ToString(gridView5.GetFocusedRowCellValue("Center_Cari"));
                string id = Convert.ToString(gridView5.GetFocusedRowCellValue("Center_Id"));

                DataTable dtSatis = (DataTable)JsonConvert.DeserializeObject(jsonSatis, (typeof(DataTable)));
                DataTable dtCari = (DataTable)JsonConvert.DeserializeObject(jsonCari, (typeof(DataTable)));


                DataTable dtMasa = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1 and Masa_Durum = 0 order by Masa_No");

                if (dtMasa.Rows.Count < 1)
                {
                    MessageBox.Show(res_man.GetString("Boş Paket Masanız Bulunmamaktadır."));
                    return;
                }


                #region Cari
                bool cari_kaydet = true;

                string cariKod = Convert.ToString(dtCari.Rows[0]["Cari_Kod"]);

                DataTable dt1 = dbtools.SelectTable(@"select [Cari_Id]
        ,ISNULL([Cari_Kod],'') as Cari_Kod
      ,ISNULL([Cari_Ad],'') as Cari_Ad
      ,ISNULL([Cari_Soyad],'') as Cari_Soyad
      ,ISNULL([Cari_Tel],'') as Cari_Tel
      ,ISNULL([Cari_Adres1],'') as Cari_Adres1
      ,ISNULL([Cari_Adres2],'') as Cari_Adres2
      ,ISNULL([Cari_Adres3],'') as Cari_Adres3
      ,ISNULL([Cari_Funvan],'') as Cari_Funvan
      ,ISNULL([Cari_Fadres1],'') as Cari_Fadres1
      ,ISNULL([Cari_Fadres2],'') as Cari_Fadres2
      ,ISNULL([Cari_Vergidarie],'') as Cari_Vergidarie
      ,ISNULL([Cari_Vergino],'') as Cari_Vergino
      ,ISNULL([Cari_Mail],'') as Cari_Mail
      ,ISNULL([Cari_Kart],'') as Cari_Kart
      ,ISNULL([Cari_Tel2],'') as Cari_Tel2
      ,ISNULL([Cari_Email],'') as Cari_Email
      ,ISNULL([Cari_Tip],'') as Cari_Tip
      ,ISNULL([Cari_Limit],0)  as Cari_Limit
      ,ISNULL([Cari_LimitTutar],0) as Cari_LimitTutar
      ,ISNULL([Cari_Il],'') as Cari_Il
      ,ISNULL([Cari_Ilce],'') as Cari_Ilce
      ,ISNULL([Cari_Mahalle],'') as Cari_Mahalle
       from Pos_Cari where Cari_Tel = '" + Convert.ToString(dtCari.Rows[0]["Cari_Tel"]) + "'");
                if (dt1.Rows.Count > 0)
                {
                    cariKod = Convert.ToString(dt1.Rows[0]["Cari_Kod"]);
                    cari_kaydet = false;
                }


                if (cari_kaydet)
                {

                    string colCari = "";
                    for (int i = 0; i < dtCari.Columns.Count; i++)
                    {
                        if (dtCari.Columns[i].ColumnName == "Cari_Id") continue;

                        colCari += dtCari.Columns[i].ColumnName + ",";
                    }
                    colCari = colCari.Substring(0, colCari.Length - 1);

                    string valueCari = "";
                    for (int j = 0; j < dtCari.Columns.Count; j++)
                    {
                        if (dtCari.Columns[j].ColumnName == "Cari_Id") continue;

                        if (dtCari.Columns[j].ColumnName == "Cari_LimitTutar")
                        {
                            valueCari += "'0',";
                            continue;
                        }

                        valueCari += "'" + Convert.ToString(dtCari.Rows[0][j]).Replace(",", ".") + "',";
                    }
                    valueCari = valueCari.Substring(0, valueCari.Length - 1);

                    cariKod = dbtools.DegerGetir(@"INSERT INTO [dbo].[Pos_Cari](" + colCari + @")VALUES(" + valueCari + @")
                        declare @id int = (select SCOPE_IDENTITY())
                        update Pos_Cari set Cari_Kod = @id where Cari_Id = @id
                        select @id");

                }
                #endregion

                #region Satıs
                string Masano = Convert.ToString(dtMasa.Rows[0]["Masa_No"]);
                int Fisno = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
                StatikSinif.siranoarttir();

                string col = "";
                for (int i = 0; i < dtSatis.Columns.Count; i++)
                {
                    if (dtSatis.Columns[i].ColumnName == "Rsat_Id") continue;

                    col += dtSatis.Columns[i].ColumnName + ",";
                }
                col = col.Substring(0, col.Length - 1);

                string value = "";
                for (int i = 0; i < dtSatis.Rows.Count; i++)
                {
                    value = ""; // her satırda sıfırla

                    for (int j = 0; j < dtSatis.Columns.Count; j++)
                    {
                        string colName = dtSatis.Columns[j].ColumnName;
                        string colValue = Convert.ToString(dtSatis.Rows[i][j]).Replace(",", "."); // virgülü noktaya çevir

                        if (colName == "Rsat_Id") continue;

                        if (colName == "Rsat_Masa")
                        {
                            value += "'" + Masano + "',";
                            continue;
                        }
                        if (colName == "Rsat_Fisno")
                        {
                            value += "'" + Fisno + "',";
                            continue;
                        }
                        if (colName == "Rsat_Durum")
                        {
                            value += "'A',";
                            continue;
                        }
                        if (colName == "Rsat_Cari")
                        {
                            value += "'" + cariKod + "',";
                            continue;
                        }
                        if (colName == "Rsat_Ind" || colName == "Rsat_HapyhourTutar" || colName == "Rsat_EntegreToplamFiyat")
                        {
                            value += "0,";
                            continue;
                        }
                        if (colName == "Rsat_SiparisPr")
                        {
                            value += "0,";
                            continue;
                        }
                        if (colName == "Rsat_Sube")
                        {
                            value += "'" + Departman.Kodlar_PosSubeKod + "',";
                            continue;
                        }

                        if (dtSatis.Columns[j].ColumnName == "Rsat_Departman")
                        {
                            value += "'" + Departman.Dep_Kodu + "',";
                            continue;
                        }

                        if (dtSatis.Columns[j].ColumnName == "Rsat_Indkodu")
                        {
                            value += "'',";
                            continue;
                        }


                        // Sütun tipi kontrolü (örn: bit, decimal, int gibi)
                        var colType = dtSatis.Columns[j].DataType;

                        if (colType == typeof(bool) || colType == typeof(Boolean))
                        {
                            // True/False yazılı string'ler varsa dönüştür
                            if (colValue.ToLower() == "false")
                                value += "0,";
                            else if (colValue.ToLower() == "true")
                                value += "1,";
                            else
                                value += string.IsNullOrWhiteSpace(colValue) ? "NULL," : colValue + ",";
                        }
                        else if (colType == typeof(int) || colType == typeof(decimal) || colType == typeof(double) || colType == typeof(float))
                        {
                            value += string.IsNullOrWhiteSpace(colValue) ? "NULL," : colValue + ",";
                        }
                        else
                        {
                            value += string.IsNullOrWhiteSpace(colValue) ? "NULL," : "'" + colValue + "',";
                        }
                    }

                    // Sondaki virgülü kaldır
                    value = value.TrimEnd(',');

                    string q = @"INSERT INTO [dbo].[Cst_Recete_Satis](" + col + ") VALUES(" + value + ")";
                    dbtools.execcmd(q);


                    dbtools.execcmd(@"update Pos_Masa set Masa_Durum = 1 where Masa_No = '" + Masano + @"' and Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1");

                    //col = String.Empty;
                    value = String.Empty;



                }
                #endregion

                dbtools.execcmd(@"Update Pos_CallCenter Set Center_Pasif = 1 Where Center_Id = '" + id + "'");
                xtraTabControl1_SelectedPageChanged(null, null);

                FisPr pr = new FisPr();
                //pr.SiparisPr(Fisno, false, 0);

                pr.newSiparisPr(Convert.ToInt32(Fisno), false, 0);


                pr.PaketPr(Fisno, " * * * PAKET FİSİ * * * ");



                //DataTable dtMerkez = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_MerkezSube = 'M' and  Pkod_Sinif = '27'  ");
                //if (dtMerkez.Rows.Count > 0)
                //{
                //    string merkezConnectionString = "Data Source='" + Convert.ToString(dtMerkez.Rows[0]["Pkod_Server"]) + "';Initial Catalog=" + Convert.ToString(dtMerkez.Rows[0]["Pkod_Database"]) + "; Persist Security Info=True;uid='" + Convert.ToString(dtMerkez.Rows[0]["Pkod_User"]) + "'; pwd='" + Convert.ToString(dtMerkez.Rows[0]["Pkod_Password"]) + "'";


                //    SqlConnection con = new SqlConnection(merkezConnectionString);
                //    SqlCommand cmd = new SqlCommand("update Cst_Recete_satis set Rsat_SubeDurum = 'Sipariş Hazırlanıyor' where Rsat_Fisno = '" + dtSatis.Rows[0]["Rsat_Fisno"] + "'", con);
                //    if (con.State != ConnectionState.Open) con.Open();
                //    cmd.ExecuteNonQuery();
                //    if (con.State != ConnectionState.Closed) con.Close();
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        //  private void gridView5_DoubleClick2(object sender, EventArgs e)
        //  {
        //      try
        //      {
        //          if (gridView5.RowCount == 0) return;

        //          string jsonSatis = Convert.ToString(gridView5.GetFocusedRowCellValue("Center_Data"));
        //          string jsonCari = Convert.ToString(gridView5.GetFocusedRowCellValue("Center_Cari"));
        //          string id = Convert.ToString(gridView5.GetFocusedRowCellValue("Center_Id"));

        //          DataTable dtSatis = (DataTable)JsonConvert.DeserializeObject(jsonSatis, (typeof(DataTable)));
        //          DataTable dtCari = (DataTable)JsonConvert.DeserializeObject(jsonCari, (typeof(DataTable)));


        //          DataTable dtMasa = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1 and Masa_Durum = 0 order by Masa_No");

        //          if (dtMasa.Rows.Count < 1)
        //          {
        //              MessageBox.Show(res_man.GetString("Boş Paket Masanız Bulunmamaktadır."));
        //              return;
        //          }


        //          #region Cari
        //          bool cari_kaydet = true;

        //          string cariKod = Convert.ToString(dtCari.Rows[0]["Cari_Kod"]);

        //          DataTable dt1 = dbtools.SelectTable(@"select [Cari_Id]
        //  ,ISNULL([Cari_Kod],'') as Cari_Kod
        //,ISNULL([Cari_Ad],'') as Cari_Ad
        //,ISNULL([Cari_Soyad],'') as Cari_Soyad
        //,ISNULL([Cari_Tel],'') as Cari_Tel
        //,ISNULL([Cari_Adres1],'') as Cari_Adres1
        //,ISNULL([Cari_Adres2],'') as Cari_Adres2
        //,ISNULL([Cari_Adres3],'') as Cari_Adres3
        //,ISNULL([Cari_Funvan],'') as Cari_Funvan
        //,ISNULL([Cari_Fadres1],'') as Cari_Fadres1
        //,ISNULL([Cari_Fadres2],'') as Cari_Fadres2
        //,ISNULL([Cari_Vergidarie],'') as Cari_Vergidarie
        //,ISNULL([Cari_Vergino],'') as Cari_Vergino
        //,ISNULL([Cari_Mail],'') as Cari_Mail
        //,ISNULL([Cari_Kart],'') as Cari_Kart
        //,ISNULL([Cari_Tel2],'') as Cari_Tel2
        //,ISNULL([Cari_Email],'') as Cari_Email
        //,ISNULL([Cari_Tip],'') as Cari_Tip
        //,ISNULL([Cari_Limit],0)  as Cari_Limit
        //,ISNULL([Cari_LimitTutar],0) as Cari_LimitTutar
        //,ISNULL([Cari_Il],'') as Cari_Il
        //,ISNULL([Cari_Ilce],'') as Cari_Ilce
        //,ISNULL([Cari_Mahalle],'') as Cari_Mahalle
        // from Pos_Cari where Cari_Tel = '" + Convert.ToString(dtCari.Rows[0]["Cari_Tel"]) + "'");
        //          if (dt1.Rows.Count > 0)
        //          {
        //              cariKod = Convert.ToString(dt1.Rows[0]["Cari_Kod"]);
        //              cari_kaydet = false;
        //          }


        //          if (cari_kaydet)
        //          {

        //              string colCari = "";
        //              for (int i = 0; i < dtCari.Columns.Count; i++)
        //              {
        //                  if (dtCari.Columns[i].ColumnName == "Cari_Id") continue;

        //                  colCari += dtCari.Columns[i].ColumnName + ",";
        //              }
        //              colCari = colCari.Substring(0, colCari.Length - 1);

        //              string valueCari = "";
        //              for (int j = 0; j < dtCari.Columns.Count; j++)
        //              {
        //                  if (dtCari.Columns[j].ColumnName == "Cari_Id") continue;

        //                  if (dtCari.Columns[j].ColumnName == "Cari_LimitTutar")
        //                  {
        //                      valueCari += "'0',";
        //                      continue;
        //                  }

        //                  valueCari += "'" + Convert.ToString(dtCari.Rows[0][j]).Replace(",", ".") + "',";
        //              }
        //              valueCari = valueCari.Substring(0, valueCari.Length - 1);

        //              cariKod = dbtools.DegerGetir(@"INSERT INTO [dbo].[Pos_Cari](" + colCari + @")VALUES(" + valueCari + @")
        //                  declare @id int = (select SCOPE_IDENTITY())
        //                  update Pos_Cari set Cari_Kod = @id where Cari_Id = @id
        //                  select @id");

        //          }
        //          #endregion

        //          #region Satıs
        //          string Masano = Convert.ToString(dtMasa.Rows[0]["Masa_No"]);
        //          int Fisno = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
        //          StatikSinif.siranoarttir();

        //          string col = "";
        //          for (int i = 0; i < dtSatis.Columns.Count; i++)
        //          {
        //              if (dtSatis.Columns[i].ColumnName == "Rsat_Id") continue;

        //              col += dtSatis.Columns[i].ColumnName + ",";
        //          }
        //          col = col.Substring(0, col.Length - 1);

        //          string value = "";
        //          for (int i = 0; i < dtSatis.Rows.Count; i++)
        //          {
        //              for (int j = 0; j < dtSatis.Columns.Count; j++)
        //              {
        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_Id") continue;

        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_Masa")
        //                  {
        //                      value += "'" + Masano + "',";
        //                      continue;
        //                  }
        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_Fisno")
        //                  {
        //                      value += "'" + Fisno + "',";
        //                      continue;
        //                  }
        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_Durum")
        //                  {
        //                      value += "'A',";
        //                      continue;
        //                  }
        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_Cari")
        //                  {
        //                      value += "'" + cariKod + "',";
        //                      continue;
        //                  }
        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_Ind")
        //                  {
        //                      value += "0, ";
        //                      continue;
        //                  }
        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_SiparisPr")
        //                  {
        //                      value += "'0',";
        //                      continue;
        //                  }

        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_Sube")
        //                  {
        //                      value += "'" + Departman.Kodlar_PosSubeKod + "',";
        //                      continue;
        //                  }


        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_HapyhourTutar")
        //                  {
        //                      value += "0,";
        //                      continue;
        //                  }

        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_EntegreToplamFiyat")
        //                  {
        //                      value += "0,";
        //                      continue;
        //                  }

        //                  if (dtSatis.Columns[j].ColumnName == "Rsat_Departman")
        //                  {
        //                      value += "'" + Departman.Dep_Kodu + "',";
        //                      continue;
        //                  }


        //                  value += "'" + Convert.ToString(dtSatis.Rows[i][j]).Replace(",", ".") + "',";
        //              }
        //              value = value.Substring(0, value.Length - 1);


        //              string q = @"INSERT INTO [dbo].[Cst_Recete_Satis](" + col + ")VALUES(" + value + ")";

        //              dbtools.execcmd(q);


        //              dbtools.execcmd(@"update Pos_Masa set Masa_Durum = 1 where Masa_No = '" + Masano + @"' and Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1");

        //              //col = String.Empty;
        //              value = String.Empty;



        //          }
        //          #endregion

        //          dbtools.execcmd(@"Update Pos_CallCenter Set Center_Pasif = 1 Where Center_Id = '" + id + "'");
        //          xtraTabControl1_SelectedPageChanged(null, null);

        //          FisPr pr = new FisPr();
        //          pr.SiparisPr(Fisno, false, 0);
        //          pr.PaketPr(Fisno, " * * * PAKET FİSİ * * * ");



        //          //DataTable dtMerkez = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_MerkezSube = 'M' and  Pkod_Sinif = '27'  ");
        //          //if (dtMerkez.Rows.Count > 0)
        //          //{
        //          //    string merkezConnectionString = "Data Source='" + Convert.ToString(dtMerkez.Rows[0]["Pkod_Server"]) + "';Initial Catalog=" + Convert.ToString(dtMerkez.Rows[0]["Pkod_Database"]) + "; Persist Security Info=True;uid='" + Convert.ToString(dtMerkez.Rows[0]["Pkod_User"]) + "'; pwd='" + Convert.ToString(dtMerkez.Rows[0]["Pkod_Password"]) + "'";


        //          //    SqlConnection con = new SqlConnection(merkezConnectionString);
        //          //    SqlCommand cmd = new SqlCommand("update Cst_Recete_satis set Rsat_SubeDurum = 'Sipariş Hazırlanıyor' where Rsat_Fisno = '" + dtSatis.Rows[0]["Rsat_Fisno"] + "'", con);
        //          //    if (con.State != ConnectionState.Open) con.Open();
        //          //    cmd.ExecuteNonQuery();
        //          //    if (con.State != ConnectionState.Closed) con.Close();
        //          //}

        //      }
        //      catch (Exception ex)
        //      {
        //          MessageBox.Show(ex.ToString());
        //      }
        //  }

        #endregion

        #region Kapalı Paket Raporu
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            gridyenile_2();
        }

        private void gridyenile_2()
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 15);
            com.Parameters.AddWithValue("@Tarih1", dateEdit1.DateTime.Date);
            com.Parameters.AddWithValue("@Tarih2", dateEdit1.DateTime.Date);
            com.Parameters.AddWithValue("@Acik", chk_Acik.Checked);
            com.Parameters.AddWithValue("@Kapali", chk_Kapali.Checked);
            if (chk_TekPaket.Checked) com.Parameters.AddWithValue("@Paketci", look_Paketci.EditValue);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            gridControl2.DataSource = dt;
            gridView2.BestFitColumns();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                PaketciDegistir(Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Fisno")));
                gridyenile_2();
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                Detay detay = new Detay();
                detay.spn_Fisno.EditValue = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Fisno"));
                //detay.btn_Fispr.Visible = false;
                //detay.btn_Adisyonpr.Visible = false;
                //detay.btn_Faturapr.Visible = false;
                detay.ShowDialog();
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            string leftColumn = "Paket Raporu";
            string rightColumn = dateEdit1.DateTime.ToLongDateString() + "-" + dateEdit1.DateTime.ToLongDateString();


            PrintingSystem printingSystem1 = new PrintingSystem();
            PrintableComponentLink printableComponentLink1 = new PrintableComponentLink();
            printingSystem1.Links.AddRange(new object[] { printableComponentLink1 });
            printableComponentLink1.Component = gridControl2;
            printableComponentLink1.Landscape = false;
            printableComponentLink1.Margins = new System.Drawing.Printing.Margins(20, 20, 50, 20);

            PageHeaderFooter phf = printableComponentLink1.PageHeaderFooter as PageHeaderFooter;
            phf.Header.Content.Clear();
            phf.Header.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            phf.Header.Content.AddRange(new string[] { leftColumn, rightColumn });
            phf.Header.LineAlignment = BrickAlignment.Far;
            printableComponentLink1.ShowPreview();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                FisPr pr = new FisPr();
                pr.PaketciPr(dateEdit1.DateTime.Date, Convert.ToString(look_Paketci.EditValue));
            }
        }
        #endregion

        #region Kayıtlı Telefon Numaraları
        private void gridyenile_3()
        {
            gridControl3.DataSource = dbtools.SelectTable("select Cari_Tel,Cari_Tel2,Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Adres1 + ' ' +Cari_Adres2 + ' ' +Cari_Adres3 as Adres from Pos_Cari where NULLIF(Cari_Tel,'') IS NOT NULL OR NULLIF(Cari_Tel2,'') IS NOT NULL");
            gridView3.BestFitColumns();
        }
        #endregion

        #region Caller Id
        private void simpleButton12_Click(object sender, EventArgs e)
        {
            string query = "select Caller_Tarih,Caller_Telno,Caller_Carikod,Cari_Ad + ' ' + Cari_Soyad as Adsoyad,Cari_Adres1 + ' ' + Cari_Adres2 + ' ' + Cari_Adres3 as Adres  "
                                    + " from Pos_CallerId "
                                    + " left join Pos_Cari on Caller_Carikod = Cari_Kod "
                                    + " where Convert(date,Caller_Tarih) = CONVERT(date,'" + dateEdit2.DateTime.Date + "')";
            gridControl4.DataSource = dbtools.SelectTable(query);
            gridView4.BestFitColumns();
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            gridyenile_1();
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            string fisno = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));


            CariHesap cari = new CariHesap();
            cari.xtraTabControl1.SelectedTabPageIndex = 1;
            cari.CariKod = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
            cari.BilgiCari = true;
            cari.ShowDialog();

            string yenicari = cari.cariKodPaketGuncelle;
            if (yenicari != "")
            {
                dbtools.execcmdR($"update Cst_Recete_Satis set Rsat_Cari='{yenicari}' where Rsat_Fisno='{fisno}'");
            }

            gridyenile_1();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {

            int fisno = -1;
            if (xtraTabControl1.SelectedTabPage == tab_Paket)
            {
                fisno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));

            }
            else if (xtraTabControl1.SelectedTabPage == tab_KapaliPaket)
            {
                fisno = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Fisno"));
            }

            if (fisno == -1)
            {
                return;
            }

            FisPr pr = new FisPr();
            if (Param.Param_YeniHesapDkm)
            {
                pr.newHesapDokum(true, fisno, 0, "* * * HESAP FİŞİ * * *");
            }
            else
            {
                pr.HesapDokum(true, fisno, 0);
            }

            string aciklama = "Fisno : " + fisno + " . HESAP DÖKÜM FİŞİ ALINDI. ";

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, aciklama, Convert.ToString(fisno), "");

            dbtools.execcmdR($"update Cst_Recete_Satis set Rsat_Ingenico_Status=1 where Rsat_Fisno={fisno}");


            dbtools.execcmd("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");


            if (Param.Param_Hesap_Kilit)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Hesap_Kilit = 1 where Rsat_Fisno = '" + fisno + "'");
            }
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {

            if (gridView2.RowCount > 0)
            {
                int Fisno = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Fisno"));
                Masa_No = Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No"));

                FisPr pr = new FisPr();
                string sonuc = pr.PaketPr(Fisno, " * * * PAKET FİSİ * * * ");
                string aciklama = "Fisno : " + Fisno + " . PAKET FİŞİ ALINDI. ";

                Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Hesap, Log.Log_Islem.Kaydet, aciklama, Convert.ToString(Fisno), "");
                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc);
                }
            }
        }

        private void Paket_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
            timer1.Dispose();
        }

        private void btnGridDizaynKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == tab_Paket)
                {
                    gridView1.SaveLayoutToXml(getDizaynPath());
                    MessageBox.Show("Grid Dizayn Kaydedildi");
                }

                if (xtraTabControl1.SelectedTabPage == tab_KapaliPaket)
                {
                    gridView2.SaveLayoutToXml(getDizaynPath_kp());
                    MessageBox.Show("Grid Dizayn Kaydedildi");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
            }
        }
        public string getDizaynPath()
        {
            string klasorAd = "GridDizaynPos";
            if (!Directory.Exists(klasorAd))
            {
                Directory.CreateDirectory(klasorAd);
            }
            return klasorAd + @"\" + User.P_Kod + "_Paket.xml";
        }

        public string getDizaynPath_kp()
        {
            string klasorAd = "GridDizaynPos";
            if (!Directory.Exists(klasorAd))
            {
                Directory.CreateDirectory(klasorAd);
            }
            return klasorAd + @"\" + User.P_Kod + "_Paket_kp.xml";
        }


        public static string MyClass = "Paket";
        private void btnGridDizaynSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RHMesaj.MyMessageConfirmation("\"" + User.P_Kod + "\" Kullanıcısına Ait Grid Dizayn'ı Silmek İstediğinize Emin misiniz ?"))
                {
                    return;
                }



                string path = getDizaynPath();


                if (xtraTabControl1.SelectedTabPage == tab_Paket)
                {

                }

                if (xtraTabControl1.SelectedTabPage == tab_KapaliPaket)
                {
                    path = getDizaynPath_kp();
                }

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

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                GridView View = sender as GridView;

                string kapatma = View.GetRowCellDisplayText(e.RowHandle, View.Columns["YSDurum"]);

                try
                {

                    GridView view = sender as GridView;

                    bool isSelected = gridView1.IsRowSelected(e.RowHandle);

                    if (isSelected == false)
                    {
                        string pkod = View.GetRowCellDisplayText(e.RowHandle, View.Columns["P_Kod"]);
                        if (pkod != "")
                        {
                            e.Appearance.BackColor = Color.Lime;
                            e.Appearance.ForeColor = Color.Black;
                        }
                    }

                }
                catch (Exception ex)
                {

                }

                if (e.Column.FieldName == "baslangicDakika")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                }

                if (kapatma != null && kapatma == "PAKET" && e.Column.FieldName == "YSDurum")
                {
                    e.Appearance.BackColor = Color.Turquoise;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (kapatma != null && kapatma == "TRENDYOL" && e.Column.FieldName == "YSDurum")
                {
                    e.Appearance.BackColor = Color.Orange;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (kapatma != null && kapatma == "GETİR YEMEK" && e.Column.FieldName == "YSDurum")
                {
                    e.Appearance.BackColor = Color.FromArgb(127, 109, 237);
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (kapatma != null && kapatma == "YEMEK SEPETİ" && e.Column.FieldName == "YSDurum")
                {
                    e.Appearance.BackColor = Color.DarkOrange;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        private void gridControlSon5Call_DoubleClick(object sender, EventArgs e)
        {
            int seciliSatir = gridViewSon5Call.FocusedRowHandle;
            if (seciliSatir < 0)
            {
                MessageBox.Show("Satır yok");
                return;
            }


            DataTable dtMasa = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1 and Masa_Durum = 0 order by Masa_No");

            if (dtMasa.Rows.Count < 1)
            {
                MessageBox.Show(res_man.GetString("Boş Paket Masanız Bulunmamaktadır."));
                return;
            }

            Masa_No = Convert.ToString(dtMasa.Rows[0]["Masa_No"]);

            string carikod = Convert.ToString(gridViewSon5Call.GetFocusedRowCellValue("kod"));
            string tel = Convert.ToString(gridViewSon5Call.GetFocusedRowCellValue("tel"));
            var pCari = Cari.Cari_Getir(carikod);

            Satis satis = new Satis();
            satis.Tag = "M";
            satis.Masa_No = Masa_No;
            satis.Masa_Paket = true;
            satis.Ozel_Masa = "";
            satis.Split = 0;
            satis.Splitad = "";
            satis.mCari = pCari;

            if (carikod == "")
            {
                satis.AcikAdres = true;
                satis.mCari = null;
                satis.CariTel = tel;
                satis.ShowDialog();
            }
            else
            {
                satis.AcikAdres = false;
                satis.ShowDialog();
            }

            gridyenile_1();

        }

        private void chk_PaketciAtanmayanlar_Click(object sender, EventArgs e)
        {
            sonsecili = 1;

            chk_PaketciAtananlar.Checked = false;
            chk_PaketciAtanmayanlar.Checked = true;

            chk_PaketSatis.Checked = false;
            chk_CallerId.Checked = false;
            chk_KayitliTel.Checked = false;
            chk_KapaliPaket.Checked = false;

            xtraTabControl1.SelectedTabPage = tab_Paket;

            gridyenile_1();



        }

        private void chk_PaketciAtananlar_Click(object sender, EventArgs e)
        {
            sonsecili = 2;

            chk_PaketciAtananlar.Checked = true;
            chk_PaketciAtanmayanlar.Checked = false;

            chk_PaketSatis.Checked = false;
            chk_CallerId.Checked = false;
            chk_KayitliTel.Checked = false;
            chk_KapaliPaket.Checked = false;

            xtraTabControl1.SelectedTabPage = tab_Paket;
            gridyenile_1();

        }

        private void chk_PaketSatis_Click(object sender, EventArgs e)
        {
            sonsecili = 0;
            gridyenile_1();

        }

        private void chk_PaketciAtanmayanlar_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chk_PaketciAtananlar_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle < 0) return; // Başlık satırına tıklamayı önle

            GridView view = sender as GridView;

            bool isSelected = gridView1.IsRowSelected(e.RowHandle);

            if (!isSelected)
            {
                // Satırı seç
                gridView1.SelectRow(e.RowHandle);
            }
            else
            {
                // Eğer zaten seçiliyse, tekrar tıklanınca seçimi kaldır
                gridView1.UnselectRow(e.RowHandle);
            }

            seciliyaz();
        }

        public void seciliyaz()
        {
            try
            {
                int sayi = gridView1.GetSelectedRows().Length;

                if (sayi == 0)
                {
                    labelSecili.Text = "";
                }
                else
                {
                    labelSecili.Text = sayi + " adet seçildi";
                }
            }
            catch (Exception ex)
            {

            }

        }


        private void btnDirektSatis_Click(object sender, EventArgs e)
        {

            Satis sat = new Satis();
            sat.Masa_No = User.P_Sabit_Masa;
            sat.Tag = "D";
            sat.ShowDialog();


            if (Param.ikinciEkranAktif == false)
            {
                Main.satislistesi_ikinci_ekran.Listele(0);
            }
        }

        private void btnKuryeOzet_Click(object sender, EventArgs e)
        {
            // 21.08.2025 basic kod ile test edebilirsin
            /*
             select isnull((u.P_Ad+' '+u.P_Soyad),'ATANMAMIŞ') as adsoyad ,count(distinct s.Rsat_Fisno) as adet,sum(s.Rsat_Tutar) as tutar from Cst_Recete_Satis s
left join Rmosmuh.dbo.Pos_User u on P_Kod = Rsat_Paketci
where 
Rsat_Ba='B' and 
s.Rsat_Tarih between '2025-08-20' and '2025-08-20'
group by (u.P_Ad+' '+u.P_Soyad)
             */


            string date = dateEdit1.DateTime.ToString("yyyy-MM-dd");

            string query = $@"declare @Tarih1  datetime 
set @Tarih1 = CONVERT(DATE,'{date}')
declare @Tarih2  datetime 
set @Tarih2 = CONVERT(DATE,'{date}')
declare @Indkapatma nvarchar(20) = (select Pkod_Kod from Pos_Kodlar WITH(NOLOCK) where Pkod_Sinif = '11' and Pkod_Ozelkod = '4')

select  
isnull(max(P_Ad + ' ' + P_Soyad),'ATANMAMIŞ') as 'adsoyad',
count(distinct Satis.Rsat_Fisno) as 'adet',
SUM(Satis.Rsat_Tutar) as 'tutar'
from Cst_Recete_Satis as Satis
--left join Pos_Masa on Rsat_Masa = Masa_No  -- çiftlediği için 21.08.2025 de yorum yaptık
left join Pos_Cari on Rsat_Cari = Cari_Kod
left join Rmosmuh.dbo.Pos_User on P_Kod = Rsat_Paketci
left join Cst_Recete_Satis  as Indirim on Satis.Rsat_Fisno = Indirim.Rsat_Fisno and Indirim.Rsat_Ba = 'A' and Indirim.Rsat_Kapatma = @Indkapatma
left join (SELECT Rsat_Fisno,Rsat_Kapatma,SUM(Rsat_Tutar) as Rsat_Tutar FROM Cst_Recete_Satis WITH(NOLOCK) WHERE (Rsat_Ba = 'A' and Rsat_Kapatma <> @Indkapatma)
GROUP BY Rsat_Fisno,Rsat_Kapatma,Rsat_Cari) AS Odeme ON Satis.Rsat_Fisno = Odeme.Rsat_Fisno
left join Pos_Kodlar AS Kodlar WITH(NOLOCK) ON Odeme.Rsat_Kapatma = Kodlar.Pkod_Kod and Kodlar.Pkod_Ozelkod <> '4' and Pkod_Sinif = '11'
left join Stok_Kodlar AS Departman WITH(NOLOCK) ON Satis.Rsat_Departman = Departman.Kodlar_Kod AND Departman.Kodlar_Sinif = '01' 
where (CONVERT(date,Satis.Rsat_Tarih) >= CONVERT(DATE,@Tarih1) OR @Tarih1 IS NULL)
AND (CONVERT(DATE,Satis.Rsat_Tarih) <= CONVERT(DATE,@Tarih2) or @Tarih2 is null) 
AND ((Satis.Rsat_Durum  = 'A' AND 0 = 1) OR (Satis.Rsat_Durum  = 'K' AND 1 = 1))
--AND Masa_Konum = 'P' 
and satis.Rsat_Ba = 'B'
group by (P_Ad + ' ' + P_Soyad)
order by count(distinct Satis.Rsat_Fisno) desc";


            var data = dbtools.SelectTableR(query);

            KuryeOzet kuryeOzet = new KuryeOzet();

            kuryeOzet.xr_Tarih.Text = dateEdit1.DateTime.ToString("dd.MM.yyyy");
            kuryeOzet.DataSource = data;


            string yazici = dbtools.DegerGetir("select top 1 isnull(xzraporyazici,0) as xzraporyazici from Rmosmuh.dbo.Pos_User_XZ where P_Kod='" + User.P_Kod + "'");

            if (yazici.Equals("0"))
            {
                kuryeOzet.ShowPreview();
            }
            else
            {
                kuryeOzet.PrinterName = yazici;
                kuryeOzet.Print();
            }



        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                GridView View = sender as GridView;

                string kapatma = View.GetRowCellDisplayText(e.RowHandle, View.Columns["YSDurum"]);



                if (kapatma != null && kapatma == "PAKET" && e.Column.FieldName == "YSDurum")
                {
                    e.Appearance.BackColor = Color.Turquoise;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (kapatma != null && kapatma == "TRENDYOL" && e.Column.FieldName == "YSDurum")
                {
                    e.Appearance.BackColor = Color.Orange;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (kapatma != null && kapatma == "GETİR YEMEK" && e.Column.FieldName == "YSDurum")
                {
                    e.Appearance.BackColor = Color.FromArgb(127, 109, 237);
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (kapatma != null && kapatma == "YEMEK SEPETİ" && e.Column.FieldName == "YSDurum")
                {
                    e.Appearance.BackColor = Color.DarkOrange;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        private void btnOdemeTipiDegistir_Click(object sender, EventArgs e)
        {
            try
            {
                int Fisno = 0;
                DateTime Tarih = Param.Tarih;

                Fisno = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Fisno"));
                Tarih = Convert.ToDateTime(gridView2.GetFocusedRowCellValue("Rsat_Tarih"));


                if (Tarih.Date != Param.Tarih.Date)
                {
                    MessageBox.Show(res_man.GetString("Geçmiş Tarihe Ait Ödeme Bilgileri Değiştirilemez..."), res_man.GetString("Uyarı"));
                    return;
                }

                if (Fisno == 0)
                {
                    MessageBox.Show(res_man.GetString("Fiş No 0 Olamaz....!"), res_man.GetString("Uyarı"));
                    return;
                }

                Hesap h = new Hesap();
                h.fisno = Fisno;
                h.Tag = Fisno;
                h.odemetipiDegistirdenmigeldi = true;
                h.tip = "O";
                h.ShowDialog();

                gridyenile_2();
            }
            catch (Exception ex)
            {

                RHMesaj.alertMesaj(ex.Message);
            }

        }
    }
}