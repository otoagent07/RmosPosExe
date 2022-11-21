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
    public partial class MasaTakip : DevExpress.XtraEditors.XtraForm
    {
        string Masa_No = String.Empty;
        string Ozel_Masa = String.Empty;
        public string Posta { get; set; }

        public MasaTakip()
        {
            InitializeComponent();
        }

        private void MasaTakip_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;

            btn_Satis.Enabled = User.Pda_Satis;
            btn_Hesap.Enabled = User.Pda_Hesap;
            btn_MasaTr.Enabled = User.Pda_Masatr;
            btn_MalzTr.Enabled = User.Pda_Malztr;
            btn_OzelMasa.Enabled = User.Pda_Ozelmasa;
            btn_OdaKontrol.Enabled = User.Pda_Odakontrol;
            btn_Mars.Enabled = Departman.Kodlar_Mars;
            btn_KisiSayisi.Enabled = User.M_KisiSayisi;
            btn_HesapDokum.Enabled = User.Pda_HesapDok;

            MasaYenile();

            barspn_Refresh.EditValue = Param.Masa_Refresh;

            flowLayoutPanel1.ClientSize = flowLayoutPanel1.Size;
        }

        private void MasaYenile()
        {
            flowLayoutPanel1.Controls.Clear();
            barspn_Refresh.EditValue = Param.Masa_Refresh;
            Masa_No = String.Empty;
            Ozel_Masa = String.Empty;

            string filterPosta = "";
            if (Convert.ToString(Posta) != "")
            {
                filterPosta = " and ISNULL(Masa_Posta,'') = '" + Posta + "'";
            }


            DataTable dtMasa = dbtools.SelectTable("select Masa_No, Masa_Ozel,MIN(Rsat_MusTipi) as Rsat_MusTipi,Masa_Durum, "
                               + "     case when min(Rsat_MusTipi) = 'O' then Masa_Ad + '\nOda: ' + isnull(min(Rsat_Odano),'') "
                               + "     when MIN(Rsat_Mustipi) = 'U' then Masa_Ad + '\nUye: ' + isnull(MIN(Rsat_Uye_Ad),'') "
                               + "     when MIN(Rsat_MusTipi) = 'C' then Masa_Ad + '\nCari: ' + isnull(MIN(Rsat_Cari),'') "
                               + "     else Masa_Ad end as Masa_Ad2,Masa_Ad "
                               + " from Pos_Masa "
                               + "     left join Cst_Recete_Satis on Masa_No = Rsat_Masa and Rsat_Durum = 'A' "
                               + " where Masa_Depart = '" + Departman.Dep_Kodu + "'  " + filterPosta
                               + " group by Masa_No,Masa_Ad,Masa_Ozel,Masa_Durum");

            if (dtMasa.Rows.Count < 1)
            {
                MessageBox.Show("Bu Departmana Ait Masa Tanımı Yapılmamış", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < dtMasa.Rows.Count; i++)
            {
                SimpleButton btnMasa = new SimpleButton();
                btnMasa.Size = new Size(55, 30);
                btnMasa.TabIndex = 0;
                btnMasa.TabStop = false;
                btnMasa.Font = new Font("Tahoma", 6.5F, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                btnMasa.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
                btnMasa.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Character;
                btnMasa.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                btnMasa.Appearance.Options.UseBackColor = true;
                btnMasa.Padding = new System.Windows.Forms.Padding(0);

                btnMasa.Tag = Convert.ToString(dtMasa.Rows[i]["Masa_No"]);

                //Boş Masa
                if (Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "0")
                {
                    btnMasa.Appearance.BackColor = Color.Lime;
                    btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                }

                //Dolu Masa
                if (Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "1")
                {
                    btnMasa.Appearance.BackColor = Color.OrangeRed;
                    btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                }

                //Beklemede Masalar
                if (Convert.ToString(dtMasa.Rows[i]["Masa_Durum"]) == "2")
                {
                    btnMasa.Appearance.BackColor = Color.MediumOrchid;
                    btnMasa.Text = Convert.ToString(dtMasa.Rows[i]["Masa_Ad"]);
                }

                //Sabitlenmiş Masaların Masa_Ad Renkleri
                //if (Convert.ToString(dtMasa.Rows[i]["Rsat_MusTipi"]) != String.Empty)
                //{
                //    switch (Convert.ToString(dtMasa.Rows[i]["Rsat_MusTipi"]))
                //    {
                //        case "O": btnMasa.ForeColor = Color.LightGoldenrodYellow;
                //            break;
                //        case "U": btnMasa.ForeColor = Color.LightBlue;
                //            break;
                //        case "C": btnMasa.ForeColor = Color.LightSalmon;
                //            break;
                //        default: btnMasa.ForeColor = Color.Black;
                //            break;
                //    }
                //}

                //Özel Masaların Adı - Rengi 
                if (Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]) != String.Empty)
                {
                    string ozel = Convert.ToString(dtMasa.Rows[i]["Masa_Ozel"]);
                    btnMasa.Text = ozel.Length > 4 ? ozel.Substring(0, 4) : ozel;
                    btnMasa.Appearance.BackColor = Color.DarkOrange;
                }

                btnMasa.Appearance.BorderColor = btnMasa.Appearance.BackColor;
                btnMasa.Click += new EventHandler(btnMasa_Click);
                flowLayoutPanel1.Controls.Add(btnMasa);
            }

            bartxt_FisNo.EditValue = 0;
        }

        void btnMasa_Click(object sender, EventArgs e)
        {
            barspn_Refresh.EditValue = Param.Masa_Refresh;

            SimpleButton myButton = (SimpleButton)sender;
            foreach (SimpleButton btn in flowLayoutPanel1.Controls)
            {
                btn.Appearance.BackColor = btn.Appearance.BorderColor;
            }
            myButton.Appearance.BackColor = Color.White;

            Masa_No = myButton.Tag.ToString();

            bartxt_FisNo.EditValue = Convert.ToInt32(dbtools.DegerGetir("select ISNULL((select TOP 1 isnull(Rsat_Fisno,0) from Cst_Recete_Satis where Rsat_Durum = 'A' and Rsat_Masa = '" + Masa_No + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "'),0)"));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            barspn_Refresh.EditValue = Convert.ToInt32(barspn_Refresh.EditValue) - 1;
            if (Convert.ToInt32(barspn_Refresh.EditValue) == 0)
            {
                MasaYenile();
            }
        }

        private void btn_Cikis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btn_Yenile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MasaYenile();
        }

        private void btn_Satis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Satis_Yap();
        }

        private void Satis_Yap()
        {
            timer1.Enabled = false;

            if (Masa_No == String.Empty)
            {
                MessageBox.Show("Masa Seçiniz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Param.Param_Masaacan_Garson && !User.M_BaskaMasa)
            {
                string garson = dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Garson from Cst_Recete_Satis where Rsat_Ba = 'B' and Rsat_Fisno = '" + bartxt_FisNo.EditValue + "' order by Rsat_Id),'')");
                if (garson != User.P_Kod && garson != "")
                {
                    MessageBox.Show("Masayı " + garson + " - " + User.Isim_Getir(garson) + " Açmıştır.\nBaşkası Satış Yapamaz...!!!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if (Param.Param_Pda_Kartsor)
            {
                DataTable dtt = dbtools.SelectTable("select Masa_Ozel from Pos_Masa where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                if (dtt.Rows.Count > 0)
                {
                    if (Convert.ToString(dtt.Rows[0]["Masa_Ozel"]) == "")
                    {
                        Kart_Oku kartno = new Kart_Oku();
                        kartno.ShowDialog();

                        kartno.Ozel_Masa_Ad = kartno.Ozel_Masa_Ad == null ? "" : kartno.Ozel_Masa_Ad;

                        if (kartno.Ozel_Masa_Ad != "")
                        {
                            Ozel_Masa = kartno.Ozel_Masa_Ad;
                        }
                    }
                } 
            }


            Urun ur = new Urun();
            ur.Ozel_Masa = Ozel_Masa;
            ur.Masa_No = Masa_No;
            ur.Tag = bartxt_FisNo.EditValue;
            ur.ShowDialog();
            ur.Satis_Tip = "M";
            MasaYenile();

            timer1.Enabled = true;
        }

        private void btn_Hesap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Masa_No == String.Empty)
            {
                MessageBox.Show("Masa Seçiniz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Convert.ToInt32(dbtools.DegerGetir("select COUNT(*) from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Durum = 'A' and Rsat_Ba = 'B' and Rsat_Masa = '" + Masa_No + "' and Rsat_Departman = '" + Departman.Dep_Kodu + "' ")) < 1)
            {
                return;
            }

            timer1.Enabled = false;

            if (Param.Tesis_Tipi == 0)
            {
                Odeme_Tip tip = new Odeme_Tip();
                tip.Fis_No = Convert.ToInt32(bartxt_FisNo.EditValue);
                tip.ShowDialog();

                if (Convert.ToString(tip.Satis_Tip) == "")
                {
                    //MessageBox.Show("Ödeme Tipi Seçiniz...");
                }
                else
                {
                    Odeme_Al odeme = new Odeme_Al();
                    odeme.Odeme_Kodu = tip.Satis_Tip;
                    odeme.Tag = Convert.ToInt32(bartxt_FisNo.EditValue);
                    odeme.Masa_No = Masa_No;
                    odeme.Satis_Tip = "M";
                    odeme.ShowDialog();
                }
            }
            else
            {
                Hesap hes = new Hesap();
                hes.Tag = bartxt_FisNo.EditValue;
                hes.Masa_No = Masa_No;
                hes.ShowDialog();
            } 

            MasaYenile();
            timer1.Enabled = true;
        }

        private void btn_OzelMasa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            timer1.Enabled = false;

            Masa_Ozel ozel = new Pda.Masa_Ozel();
            ozel.ShowDialog();
            Ozel_Masa = ozel.Ozel_Masa;
            Satis_Yap();

            timer1.Enabled = true;
        }

        private void btn_OdaKontrol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            timer1.Enabled = false;

            Oda_Kontrol kontrol = new Oda_Kontrol();
            kontrol.ShowDialog();

            timer1.Enabled = true;
        }

        private void btn_MasaTr_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Convert.ToInt32(bartxt_FisNo.EditValue) > 0 && Masa_No != String.Empty)
            {
                timer1.Enabled = false;

                Masa_Tr transfer = new Masa_Tr();
                transfer.Tag = Convert.ToInt32(bartxt_FisNo.EditValue).ToString();
                transfer.txt_Masano.Text = Masa_No;
                transfer.ShowDialog();
                MasaYenile();

                timer1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Masa Bilgisi Bulunamadı...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_MalzTr_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Convert.ToInt32(bartxt_FisNo.EditValue) > 0 && Masa_No != String.Empty)
            {
                timer1.Enabled = false;

                Malz_Tr tr = new Malz_Tr();
                tr.txt_Masano.EditValue = Masa_No;
                tr.Kaynak_Fisno = Convert.ToInt32(bartxt_FisNo.EditValue);
                tr.ShowDialog();
                MasaYenile();

                timer1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Masa Bilgisi Bulunamadı...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_HesapDokum_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Convert.ToInt32(bartxt_FisNo.EditValue) > 0 && Masa_No != String.Empty)
            {
                if (Departman.Kodlar_Hesap_Adisyon)
                {
                    AdisyonPr adisyon = new AdisyonPr();
                    string cevap = adisyon.Adisyon_Yaz(Convert.ToInt32(bartxt_FisNo.EditValue));
                    if (cevap != "OK")
                    {
                        MessageBox.Show(cevap);
                        return;
                    }
                    adisyon.Adisyon_Sayac_Arttir(Convert.ToInt32(bartxt_FisNo.EditValue));
                }
                else
                {
                    FisPr pr = new FisPr();
                    pr.HesapDokum(true,Convert.ToInt32(bartxt_FisNo.EditValue),0);
                }

                dbtools.execcmd("update Pos_Masa set Masa_Durum = '2' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");

                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Ingenico_Status='1' where Rsat_Fisno='" + Convert.ToInt32(bartxt_FisNo.EditValue) + "'");

                if (Param.Param_Hesap_Kilit)
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Hesap_Kilit = 1 where Rsat_Fisno = '" + Convert.ToInt32(bartxt_FisNo.EditValue) + "'");
                }
            }
            else
            {
                MessageBox.Show("Masa Bilgisi Bulunamadı...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Mars_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Convert.ToInt32(bartxt_FisNo.EditValue) > 0)
            {
                Mars_SatirSec m = new Mars_SatirSec();
                m.Tag = Convert.ToInt32(bartxt_FisNo.EditValue);
                m.ShowDialog();
            }
        }

        private void btn_KisiSayisi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int Fisno = Convert.ToInt32(bartxt_FisNo.EditValue);

            DataTable dtKisi = dbtools.SelectTable("select Rsat_Kisi from Cst_Recete_Satis WITH(NOLOCK) where Rsat_Fisno = '" + Fisno + "'");
            if (dtKisi.Rows.Count <= 0)
            {
                return;
            }

            Klavye1 klavye = new Klavye1();
            klavye.Tag = "KISISAYISI";
            klavye.txt_Sayi.Text = Convert.ToInt32(dtKisi.Rows[0]["Rsat_Kisi"]).ToString();
            klavye.ShowDialog();
            int Kisi_Sayisi = Convert.ToInt32(klavye.sayi);

            Fis_Islem.Kisi_Sayisi(Fisno, Kisi_Sayisi);

            Log.Log_Kaydet(Log.Log_Program.Pda, Log.Log_Bolum.Kisi_Sayisi, Log.Log_Islem.Duzelt, "Fis No:" + Fisno.ToString() + " Kisi Sayısı Duzeltildi.Eski Kisi: " + Convert.ToInt32(dtKisi.Rows[0]["Rsat_Kisi"]).ToString() + " Yeni Kisi: " + Kisi_Sayisi.ToString(), Fisno.ToString(), "");
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SatisList s = new SatisList();
                s.Fis_No = Convert.ToInt32(bartxt_FisNo.EditValue);
                s.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}