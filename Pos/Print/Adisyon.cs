using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Pos.Class;

namespace Pos.Print
{
    public partial class Adisyon : DevExpress.XtraReports.UI.XtraReport
    {
        public bool Detay = false;

        public Adisyon()
        {
            InitializeComponent();
        }

        private void Satir_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //if (!Detay)
            //{
            //    XRLabel label = (XRLabel)sender;
            //    if (Convert.ToString(GetCurrentColumnValue("Rsat_AdisyonPr")) == "True")
            //    {
            //        label.Text = "";
            //    }
            //}
        }
    }
}
