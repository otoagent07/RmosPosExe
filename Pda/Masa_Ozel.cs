using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Pda
{
    public partial class Masa_Ozel : DevExpress.XtraEditors.XtraForm
    {
        public string Ozel_Masa = String.Empty;

        public Masa_Ozel()
        {
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            Ozel_Masa = txt_Ozelmasa.Text;
            this.Close();
        }
    }
}