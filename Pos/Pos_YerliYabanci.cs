using System;

namespace Pos
{
    public partial class Pos_YerliYabanci : DevExpress.XtraEditors.XtraForm
    {
        public Pos_YerliYabanci()
        {
            InitializeComponent();
        }

        public string YO = "";
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            YO = "Y";
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            YO = "O";
            this.Close();
        }
    }
}