using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Klavye2 : DevExpress.XtraEditors.XtraForm
    {
        public string yazi = "";

        public Klavye2()
        {
            InitializeComponent();
            this.BringToFront();
        }

        int neredenGeldim = 0;
        public Klavye2(int neredenGeldim)
        {
            InitializeComponent();
            this.BringToFront();

            this.neredenGeldim = neredenGeldim;
            /*
             neredenGeldim 1 ise masatakip
             */
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            txt_Yazi.Text += btn.Text;
        }

        public int Fisno = 0;
        private void Back_Space_Click(object sender, EventArgs e)
        {
            if (txt_Yazi.Text.Length > 0)
            {
                txt_Yazi.Text = txt_Yazi.Text.Substring(0, txt_Yazi.Text.Length - 1);
            }
        }

        public bool cikis = false;

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //

        public void setOk()
        {
            cikis = true;
            yazi = txt_Yazi.Text;

            if (Param.Tesis_Tipi == 1)
            {
                if (Param.Param_CariSor == true)
                {
                    DataTable dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Ad like '" + yazi + "%' ");
                    if (dt.Rows.Count > 0)
                    {
                        Pos_CariList c = new Pos_CariList();
                        c.gridControl1.DataSource = dt;
                        c.ShowDialog();

                        if (c.Durum == true)
                        {
                            yazi = c.CariKodYazi;
                            if (Fisno != 0)
                            {

                                dbtools.execcmd("update Cst_Recete_Satis Set Rsat_Cari = '" + c.CariKodu + "' Where Rsat_Fisno = '" + Fisno + "'");
                                dbtools.execcmd("update Cst_Recete_Satis Set Rsat_Mustipi = '" + dbtools.DegerGetir("select Cari_Tip From Pos_Cari where Cari_Kod = '" + c.CariKodu + "'") + "' Where Rsat_Fisno = '" + Fisno + "'");
                            }

                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            if (Convert.ToString(this.Tag) == "FISIPTAL")
            {
                if (yazi.Length < 1)
                {
                    MessageBox.Show(res_man.GetString("İptal Sebebini Giriniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            this.Close();
        }


        private void btn_OK_Click(object sender, EventArgs e)
        {
            setOk();
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());


        public string baslik = "";
        private void Klavye2_Load(object sender, EventArgs e)
        {
            txtBaslik.Text = "";
            if (this.Tag != null && this.Tag.ToString().Equals("FISIPTAL"))
            {
                txtBaslik.Text = res_man.GetString("İPTAL NEDENİNİ GİRİNİZ...");
            }

            if (!baslik.Equals(""))
            {
                txtBaslik.Text = baslik;
            }
        }

        private void Klavye2_Shown(object sender, EventArgs e)
        {
            txt_Yazi.Select();

            txt_Yazi.Focus();
        }

        private void txt_Yazi_EditValueChanged(object sender, EventArgs e)
        {
            if (neredenGeldim == 1) // masaTakip
            {
                Main.masa_takip.setFilter(txt_Yazi.Text);
            }
        }



        private void txt_Yazi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                setOk();
            }
        }
    }
}