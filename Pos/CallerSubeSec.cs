using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pos
{
    public partial class CallerSubeSec : DevExpress.XtraEditors.XtraForm
    {
        public string kod { get; set; }

        public CallerSubeSec()
        {
            InitializeComponent();
        }

        private void CallerSubeSec_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Sinif = '27' order by Pkod_Kod");
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            kod = Convert.ToString(gridView1.GetFocusedRowCellValue("Pkod_Kod"));

            this.Close();
        }
    }
}