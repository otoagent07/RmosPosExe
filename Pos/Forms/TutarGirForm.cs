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
    public partial class TutarGirForm : Form
    {
        public TutarGirForm()
        {
            InitializeComponent();
        }

      public decimal tutar = 0;
      public bool vazgec = false;
        private void TutarGirForm_Load(object sender, EventArgs e)
        {
            txtTutar.Select();
            txtTutar.Focus();
        }

        private void btnIadeYap_Click(object sender, EventArgs e)
        {
            tutar = Convert.ToDecimal(txtTutar.Text);
            this.Close();
        }

        private void btnVazgec_Click(object sender, EventArgs e)
        {
            vazgec = true;
            this.Close();
        }
    }
}
