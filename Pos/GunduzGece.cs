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
    public partial class GunduzGece : DevExpress.XtraEditors.XtraForm
    {
        public GunduzGece()
        {
            InitializeComponent();
        }

        public string DepKodu;

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("Update Stok_Kodlar Set Kodlar_GGFiyat = 0 Where Kodlar_Kod = '" + DepKodu + "'");
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            dbtools.execcmd("Update Stok_Kodlar Set Kodlar_GGFiyat = 1 Where Kodlar_Kod = '" + DepKodu + "'");
            this.Close();
        }
    }
}