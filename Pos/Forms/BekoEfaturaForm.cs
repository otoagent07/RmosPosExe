using Pos.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos.Forms
{
    public partial class BekoEfaturaForm : Form
    {

        public int fisno = -1;
        public BekoEfaturaForm(int fisno)
        {
            InitializeComponent();
            this.fisno = fisno;
        }

        int tip = 1; // 1 efatura , 2 earşiv , 3 fatura
        private void btnEfatura_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("update Cst_Recete_Satis set efaturaTip = 1 where Rsat_Fisno = '" + fisno + "'");
            this.Close();
        }

        private void btnEarsiv_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("update Cst_Recete_Satis set efaturaTip = 2 where Rsat_Fisno = '" + fisno + "'");
            this.Close();

        }

        private void btnFatura_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("update Cst_Recete_Satis set efaturaTip = 3 where Rsat_Fisno = '" + fisno + "'");
            this.Close();

        }
    }
}
