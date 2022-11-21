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
    public partial class UyariForm : Form
    {
        string uyariMesaj = "";
        public UyariForm(string uyariMesaj)
        {
            InitializeComponent();
            this.uyariMesaj = uyariMesaj;
        }

        private void UyariForm_Load(object sender, EventArgs e)
        {
            txtUyariMesaj.Text = uyariMesaj;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
