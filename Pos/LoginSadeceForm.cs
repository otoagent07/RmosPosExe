using DevExpress.XtraEditors;
using Pos.Class;
using Pos.Forms;
using System;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class LoginSadeceForm : DevExpress.XtraEditors.XtraForm
    {

        public bool okey = false;
        public LoginSadeceForm()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Size = new System.Drawing.Size(469, 678);
        }


        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static string MyClass = "frmLogin";
        private void btn_Giris_Click(object sender, EventArgs e)
        {

            var dt_P_User = Rmosmuh.SelectTable("select * from Pos_User with(nolock) where P_Kart = '" + txt_Giris.Text + "' or  P_Kod = '" + txt_Giris.Text + "'");

            if (dt_P_User == null || dt_P_User.Rows.Count < 1)
            {
                MessageBox.Show("ŞİFRE YANLIŞ");
                return;
            }


            okey = true;
            this.Close();
        }





        private void btn_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            txt_Giris.Text += btn.Text;
            if (txt_Giris.Text == "19830126")
            {
                btn_Geri.Enabled = true;
            }
            else
            {
                btn_Geri.Enabled = false;
            }
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void btn_Sil_Click(object sender, EventArgs e)
        {
            txt_Giris.Text = String.Empty;

        }

        private void btn_Geri_Click(object sender, EventArgs e)
        {
            Language d = new Language();
            d.ShowDialog();
        }

        private void txt_Giris_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (txt_Giris.Text == "19830126")
            {
                btn_Geri.Enabled = true;
            }
            else
            {
                btn_Geri.Enabled = false;
            }
        }

        private bool _altF4Pressed = false;
        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                _altF4Pressed = true;
            }
        }
    }
}