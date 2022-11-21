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

namespace Pos.frmMasa
{
    public partial class frmMasaTakip : DevExpress.XtraEditors.XtraForm
    {
        public frmMasaTakip()
        {
            InitializeComponent();
        }

        private void tileBar1_ItemClick(object sender, TileItemEventArgs e)
        {
            if (e.Item == tileBarItem18)
            {
                this.Close();
            }
        }
    }
}