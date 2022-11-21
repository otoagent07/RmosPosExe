using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace Pos
{
    public partial class Uzak : DevExpress.XtraEditors.XtraForm
    {
        public Uzak()
        {
            InitializeComponent();
        }

        private void Uzak_Load(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string programDizini = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            string pro = programDizini + "\\Uzak\\TeamViewer.exe";
            System.Diagnostics.Process.Start(@"" + pro);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string programDizini = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            string pro = programDizini + "\\Uzak\\Ammyy.exe";
            System.Diagnostics.Process.Start(@"" + pro);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string programDizini = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            string pro = programDizini + "\\Uzak\\AnyDesk.exe";
            System.Diagnostics.Process.Start(@"" + pro);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string programDizini = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            string pro = programDizini + "\\Uzak\\Alpemix.exe";
            System.Diagnostics.Process.Start(@"" + pro);
        }

        
    }
}