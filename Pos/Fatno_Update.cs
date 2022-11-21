using Pos.Class;
using System;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Fatno_Update : DevExpress.XtraEditors.XtraForm
    {
        public Fatno_Update()
        {
            this.BringToFront();
            InitializeComponent();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), System.Reflection.Assembly.GetExecutingAssembly());
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            dbtools.execcmd(" update Pos_Fatura set PFat_Fatno = '" + txt_YeniFatNo.Text + "' where PFat_Cekno = '" + txt_Fisno.Text + "'");
            MessageBox.Show(txt_Fisno.Text + " Nolu Fisin Fatura Numarası Değişti...", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void Fatno_Update_Load(object sender, EventArgs e)
        {

        }
    }
}