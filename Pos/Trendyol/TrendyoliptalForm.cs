using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos.Trendyol
{
    public partial class TrendyoliptalForm : Form
    {
        public TrendyoliptalForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancelId = ((SimpleButton)sender).Tag.ToString();
            this.Close();
        }

       public string cancelId = "";
        private void TrendyoliptalForm_Load(object sender, EventArgs e)
        {
            cancelId = "";
        }
    }
}
