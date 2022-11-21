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

namespace Pos.YemekSepeti
{
    public partial class YS_Yardim : DevExpress.XtraEditors.XtraForm
    {
        public YS_Yardim()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();

            //DataSet ds = YS_AuthHeader.soap.GetRestaurantList(YS_AuthHeader.ah);

            //gridControl1.DataSource = ds;
        }
    }
}