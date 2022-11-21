using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Data;
using System.Windows.Forms;

namespace Pos
{
    public partial class ParaUstu : DevExpress.XtraEditors.XtraForm
    {
        public ParaUstu()
        {
            InitializeComponent();
        }

        public int Fisno;
        private void btn_Sayi_Click(object sender, EventArgs e)
        {
            PictureEdit btn = (PictureEdit)sender;
            txt_Sayi.Text = (Convert.ToDecimal(txt_Sayi.Text) + Convert.ToDecimal(btn.Tag)).ToString("n2");
            txt_ParaUstu.Text = (Convert.ToDecimal(txt_Sayi.Text) - Convert.ToDecimal(txt_ToplamTutar.Text)).ToString("n2");
        }

        private void ParaUstu_Load(object sender, EventArgs e)
        {

            txt_ToplamTutar.Text = Bakiye_bul_TL().ToString("n2");
            
        }

        private decimal Bakiye_bul_TL()
        {
            decimal bakiye = 0;
            DataTable dtBakiye = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 21,@Fisno = '" + Fisno + "', @Split = '" + 0 + "'");

            bakiye = Convert.ToDecimal(dtBakiye.Rows[0]["TLBakiye"]);

            bakiye = Math.Abs(bakiye) < Convert.ToDecimal(0.03) ? 0 : bakiye;
            return bakiye;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            txt_Sayi.Text = "0,00";
            txt_ParaUstu.Text = "0,00";
        }
    }
}