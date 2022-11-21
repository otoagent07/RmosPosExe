using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;
using System.Resources;
using System.Reflection;

namespace Pos
{
    public partial class Pos_TeraziBarkodEkran : DevExpress.XtraEditors.XtraForm
    {
        public Pos_TeraziBarkodEkran()
        {
            InitializeComponent();
        }


        private void Pos_TeraziBarkodEkran_Load(object sender, EventArgs e)
        {
            txt_Barkod.Text = String.Empty;
            txt_Barkod.Focus();
        }
        public decimal urun_Miktar = 0;
      
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void BarkodOku()
        {
            urun_Miktar = 0;
            try
            {
                string barkod = txt_Barkod.Text;
                txt_Barkod.Text = String.Empty;
                if (barkod.Length > 0)
                {
                    string urun_Kodu = barkod.Substring(Param.Barkod_Recbas, Param.Barkod_Rechane);
                    int KG = Convert.ToInt32(barkod.Substring(Param.Barkod_KGbas, Param.Barkod_KGhane));
                    int GR = Convert.ToInt32(barkod.Substring(Param.Barkod_GRbas, Param.Barkod_GRhane));

                    urun_Miktar = KG + (Convert.ToDecimal(GR) / Convert.ToDecimal(Math.Pow(10, Param.Barkod_GRhane)));

                    DataTable dt = dbtools.SelectTable("select Rec_Genelkod,Rec_Ad from Cst_Recete WITH(NOLOCK) where Rec_Genelkod = '" + urun_Kodu + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txt_urunAdi.Text = Convert.ToString(dt.Rows[0]["Rec_Ad"]);
                        txt_UrunKodu.Text = Convert.ToString(dt.Rows[0]["Rec_Genelkod"]);
                        txt_UrunGR.EditValue = urun_Miktar + " / KG";
                    }
                    else
                    {
                        MessageBox.Show(res_man.GetString("Ürün Kodu Bulunamadı..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (urun_Miktar > Param.Max_Miktar)
                    {
                        if (MessageBox.Show(res_man.GetString("Maximum Miktarı Geçtiniz...")+ "\n" + res_man.GetString("Devam Etmek İstiyor Musunuz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                    }
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            Durum = true;
            this.Close();
        }

        public bool Durum = false;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Durum = false;
            txt_urunAdi.Text = String.Empty;
            txt_UrunKodu.Text = String.Empty;
            txt_UrunGR.EditValue = String.Empty;
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Durum = true;
            this.Close();
        }

        private void txt_Barkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BarkodOku();
            }
        }
    }
}