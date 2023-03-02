using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Pos.Print
{
    public partial class Hesap : DevExpress.XtraReports.UI.XtraReport
    {
        public Hesap()
        {
            InitializeComponent();
        }

        private void xr_Miktar_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string text = ((XRLabel)sender).Text;
            text = text.Replace(",0000", "");
            text = text.Replace(",000", "");
            text = text.Replace(",00", "");

            ((XRLabel)sender).Text = text;

        }
    }
}
