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

namespace Pos
{
    public partial class TarihDegistir : DevExpress.XtraEditors.XtraForm
    {
        public int Fisno { get; set; }

        public TarihDegistir()
        {
            InitializeComponent();
        }

        private void TarihDegistir_Load(object sender, EventArgs e)
        {
            dateEdit1.DateTime = Param.Tarih;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (Fisno > 0)
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Tarih = '" + dateEdit1.DateTime.Date + "' where Rsat_Fisno = '" + Fisno + "'");
                this.Close();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}