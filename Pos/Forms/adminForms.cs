using Pos;
using System;
using System.Windows.Forms;

namespace RmosAcentex.Forms
{
    public partial class adminForms : Form
    {
        public adminForms()
        {
            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            if (txtSifre.Text.ToLower().Equals("19830126x"))
            {
                Program.main.bar_yemekSepeti.Enabled = true;
                Program.main.barButtonItem5.Enabled = true;
                Program.main.barButtonItem6.Enabled = true;
                if (Program.main.genelAyarlar!=null)
                {
                    Program.main.genelAyarlar.panelControl1.Visible = true;


                    Program.main.genelAyarlar.date_Prm_Tarih.Properties.ReadOnly = false;


                    RHMesaj.alertMesaj("Admin Girişi Başarılı");
                }
                    this.Close();

            }
            else
            {
                RHMesaj.alertMesaj("Şifre Yanlış!");
                txtSifre.Select();
                txtSifre.Focus();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void adminForms_Load(object sender, EventArgs e)
        {
            txtSifre.Select();
            txtSifre.Focus();
        }
    }
}
