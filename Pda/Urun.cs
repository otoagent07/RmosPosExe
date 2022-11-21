using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pda
{
    public partial class Urun : DevExpress.XtraEditors.XtraForm
    {
        public string Masa_No = String.Empty;
        public string Ozel_Masa = String.Empty;
        public string Satis_Tip = String.Empty;// = "D";  //Direk Satış
        string Ana_Grup = String.Empty;
        string Alt_Grup = String.Empty;

        string Garson;
        int Kisi_Sayisi;

        public Urun()
        {
            InitializeComponent();
        }

        private void Urun_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;
            this.Text = "Urun" + "  - Masa:" + Masa_No;

            Kisi_Garson();

            if (Satis_Tip == "D")
            {
                btn_Geri.Enabled = false;
            }
            if (Param.Param_Anagrup_Cikmasin)
            {
                Alt_Yenile();
                flp_AnaGrup.Visible = false;
            }
            else
            {
                Ust_Yenile();
            }

            Siparis_Kontrol();
        }

        private void Kisi_Garson()
        {
            Garson = User.P_Kod;
            int kontrol = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + this.Tag.ToString() + "'"));
            if (kontrol > 0)
            {
                return;
            }

            if (Departman.Kisi_Sor && !(Satis_Tip == "D" && Departman.Kodlar_Kisisorma_Pda))
            {
                Klavye1 klv = new Klavye1();
                klv.Tag = "KISISOR";
                klv.ShowDialog();
                Kisi_Sayisi = Convert.ToInt32(klv.sayi);
            }

            if (Departman.Garson_Sor)
            {

            }
        }

        private void Siparis_Kontrol()
        {
            if (Departman.Siparis)
            {
                int sayac = Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis where Rsat_Fisno = '" + this.Tag + "' and Rsat_SiparisPr = 0"));
                if (sayac == 0)
                {
                    btn_Geri.Enabled = true;
                }
                else
                {
                    btn_Geri.Enabled = false;
                }
            }
        }

        private void Ust_Yenile()
        {
            flp_AltGrup.Controls.Clear();

            DataTable dt = dbtools.SelectTable("SELECT Kont_Anagrup, Stok_Kodlar.Kodlar_Ad as Ana_Grupad FROM  Pos_Grup WITH(NOLOCK) LEFT JOIN Stok_Kodlar ON Kont_Anagrup = Kodlar_Kod and Kodlar_Sinif = '08' "
            + " WHERE Kont_Departman = '" + Departman.Dep_Kodu + "' GROUP BY Kont_Anagrup,Stok_Kodlar.Kodlar_Ad ORDER BY Kont_Anagrup");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    SimpleButton btn_AnaGrup = new SimpleButton();
                    btn_AnaGrup.Size = new System.Drawing.Size(75, 25);
                    btn_AnaGrup.TabIndex = 0;
                    btn_AnaGrup.TabStop = false;
                    btn_AnaGrup.Font = new System.Drawing.Font("Tahoma", 7F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    btn_AnaGrup.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btn_AnaGrup.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_AnaGrup.Appearance.Options.UseBackColor = true;

                    btn_AnaGrup.Text = Convert.ToString(dt.Rows[i]["Ana_Grupad"]);
                    btn_AnaGrup.Tag = Convert.ToString(dt.Rows[i]["Kont_Anagrup"]);


                    btn_AnaGrup.Click += new EventHandler(btn_AnaGrup_Click);
                    flp_AnaGrup.Controls.Add(btn_AnaGrup);
                }
            }
        }

        void btn_AnaGrup_Click(object sender, EventArgs e)
        {
            flp_AltGrup.Controls.Clear();

            SimpleButton btn_AnaGrup = (SimpleButton)sender;
            Ana_Grup = btn_AnaGrup.Tag.ToString();

            Alt_Yenile();
        }

        private void Alt_Yenile()
        {

            string filter = "";
            string filter2 = "";
            if (!Param.Param_Anagrup_Cikmasin)
            {
                filter = "and Kont_Anagrup = '" + Ana_Grup + "'";
                filter2 = " and Rec_Anagrup = '" + Ana_Grup + "'";
            }
            DataTable dt = dbtools.SelectTable("SELECT top 1 Rdep_Departman as Kont_Departman,Rec_Anagrup as Kont_Anagrup,'SIKKULLAN' as Kont_Aragrup,'SIKKULLAN' as Kont_Aragrup2,'(*) SIK KULLANILAN' as Kodlar_Ad,-10000 as Kodlar_Sira,NULL  as Kodlar_Size,NULL as Kodlar_Font,NULL as Kodlar_Backcolor "
               + " FROM Cst_Recete_Dep WITH(NOLOCK)  "
               + "     left join Cst_Recete on Rec_Genelkod = Rdep_Recete "
               + " WHERE Rdep_Departman = '" + Departman.Dep_Kodu + "' " + filter2 + " and ISNULL(Rdep_SikKullanilan,0) = 1 "
               + " UNION ALL "
               + " SELECT Kont_Departman, Kont_Anagrup, Kont_Aragrup,  Kont_Anagrup +'#'+ Kont_Aragrup as Kont_Aragrup2, Ara_Grup.Kodlar_Ad,isnull(Ara_Grup.Kodlar_Sira,0),ISNULL(NULLIF(Ara_Grup.Kodlar_Size,''),'100;40') as Kodlar_Size,Ara_Grup.Kodlar_Font,Ara_Grup.Kodlar_Backcolor "
               + " FROM Pos_Grup "
               + " left join Stok_Kodlar as Ana_Grup ON Kont_Anagrup = Ana_Grup.Kodlar_Kod AND Ana_Grup.Kodlar_Sinif = '08' "
               + " left join Stok_Kodlar as Ara_Grup ON Kont_Aragrup = Ara_Grup.Kodlar_Kod AND Ara_Grup.Kodlar_Sinif = '09' and Ara_Grup.Kodlar_Anagrup =  Kont_Anagrup "
               + " WHERE Kont_Departman = '" + Departman.Dep_Kodu + "' " + filter + " ORDER BY Kodlar_Sira, Kodlar_Ad, Kont_Anagrup, Kont_Aragrup, Kont_Departman");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleButton btn_AltGrup = new SimpleButton();
                    btn_AltGrup.Size = new System.Drawing.Size(100, 25);
                    btn_AltGrup.TabIndex = 0;
                    btn_AltGrup.TabStop = false;
                    btn_AltGrup.Font = new System.Drawing.Font("Tahoma", 8F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162))); //Calibri
                    btn_AltGrup.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btn_AltGrup.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_AltGrup.Appearance.Options.UseBackColor = true;


                    btn_AltGrup.Text = Convert.ToString(dt.Rows[i]["Kodlar_Ad"]);
                    btn_AltGrup.Tag = Param.Param_Anagrup_Cikmasin == true ? Convert.ToString(dt.Rows[i]["Kont_Aragrup2"]) : Convert.ToString(dt.Rows[i]["Kont_Aragrup"]);

                    btn_AltGrup.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_AltGrup.Click += new EventHandler(btn_AltGrup_Click);
                    flp_AltGrup.Controls.Add(btn_AltGrup);
                }
            }
        }

        void btn_AltGrup_Click(object sender, EventArgs e)
        {
            SimpleButton btn_AltGrup = (SimpleButton)sender;

            if (Param.Param_Anagrup_Cikmasin && Convert.ToString(btn_AltGrup.Tag) != "SIKKULLAN")
            {
                string[] kod = btn_AltGrup.Tag.ToString().Split('#');

                Ana_Grup = kod[0];
                Alt_Grup = kod[1];
            }
            else
            {
                Alt_Grup = btn_AltGrup.Tag.ToString();
            }

            Satis sat = new Satis();
            sat.Ana_Grup = Ana_Grup;
            sat.Alt_Grup = Alt_Grup;
            sat.Ozel_Masa = Ozel_Masa;
            sat.Masa_No = Masa_No;
            sat.Satis_Tip = Satis_Tip;
            sat.Fisno = Convert.ToInt32(this.Tag);
            sat.Tag = "M";
            sat.Garson = Garson;
            sat.Kisi_Sayisi = Kisi_Sayisi;
            sat.ShowDialog();

            if (sat.Cikis)
            {
                this.Close();
            }
            else
            {
                Siparis_Kontrol();
            }
        }

        private void btn_Geri_Click(object sender, EventArgs e)
        {
            this.Close();
        }




    }
}