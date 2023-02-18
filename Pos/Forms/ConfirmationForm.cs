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
    public partial class ConfirmationForm : Form
    {
        public ConfirmationForm(string mesaj)
        {
            InitializeComponent();
            textBox1.Text = mesaj;
            onay = false;
        }
        public bool onay = false;
        private void ConfirmationForm_Load(object sender, EventArgs e)
        {
            btnEvet.Select();
            btnEvet.Focus();
        }

        private void btnEvet_Click(object sender, EventArgs e)
        {
            onay = true;
            this.Close();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            onay = false;
            this.Close();

        }
    }
}
