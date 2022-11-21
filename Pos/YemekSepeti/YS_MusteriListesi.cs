using Pos.Class;
using System;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos.YemekSepeti
{
    public partial class YS_MusteriListesi : DevExpress.XtraEditors.XtraForm
    {
        public YS_MusteriListesi()
        {
            InitializeComponent();
        }

        private void YS_MusteriListesi_Load(object sender, EventArgs e)
        {
            txt_Fisno.Text = Fisno;
            Listele();
        }

        private void Listele()
        {
            DataTable dt = dbtools.SelectTable("select top 10 * from YS_Order order by Order_Id desc ");
            if (dt.Rows.Count > 0)
            {
                gridControl1.DataSource = dt;
            }
        }

        public string Fisno;

        public bool Durum = false;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Durum = false;
            this.Close();
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                MessageBox.Show(res_man.GetString("Listeden Müşteriyi Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK);
                return;
            }

            dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_YSOrderID = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Order_Id")) + "' Where Rsat_Fisno = '" + Fisno + "'");
            this.Close();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            gridControl2.DataSource = dbtools.SelectTable("Select * From YS_Product Where Order_Id = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Order_Id")) + "'");
        }
    }
}