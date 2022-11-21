using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pos
{
    public partial class KisiSayisiDegistir : DevExpress.XtraEditors.XtraForm
    {
        public KisiSayisiDegistir()
        {
            InitializeComponent();
        }

        public int KisiSayisi;

        public int Fisno { get; set; }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void KisiSayisiDegistir_Load(object sender, EventArgs e)
        {
            txt_KisiSayisi.Text = Convert.ToString(KisiSayisi);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (Fisno > 0)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Kisi = '" + txt_KisiSayisi.Text + "' where Rsat_Fisno = '" + Fisno + "'");
                this.Close();
            }
        }
    }
}