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
    public partial class Pos_XtraFolio_OdemeTipi : DevExpress.XtraEditors.XtraForm
    {
        public Pos_XtraFolio_OdemeTipi()
        {
            InitializeComponent();
        }

        public DataTable dt = new DataTable();
        public int Kumhrk_Id;
        private void Pos_XtraFolio_OdemeTipi_Load(object sender, EventArgs e)
        {
            look_OdemeDep.Properties.DataSource = dt;
            look_OdemeDep.Properties.DisplayMember = "Kodlar_Ad";
            look_OdemeDep.Properties.ValueMember = "Kodlar_Kod";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Fronttools.execcmd("update Kumhrk set Kumhrk_Dep_kodu = '" + look_OdemeDep.EditValue + "' where Kumhrk_Id = '" + Kumhrk_Id + "'");
            this.Close();
        }
    }
}