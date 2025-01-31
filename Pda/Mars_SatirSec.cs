using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pda
{
    public partial class Mars_SatirSec : DevExpress.XtraEditors.XtraForm
    {
        public Mars_SatirSec()
        {
            InitializeComponent();
        }

        private void Mars_SatirSec_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;

            gridyenile();
        }

        private void gridyenile()
        {
            gridControl1.DataSource = dbtools.SelectTable("select convert(bit,1) as sec, Rsat_Id,Rsat_Recete,Rec_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_Mars "
                    + " from Cst_Recete_Satis "
                    + " left join Cst_Recete on Rsat_Recete = Rec_Genelkod "
                    + " where Rsat_Ba = 'B' and Rsat_Fisno = '" + this.Tag.ToString() + "' and ISNULL(Rsat_Mars,0) = 0");
        }


        // burası küçük el terminali
        private void btn_Mars_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (Convert.ToBoolean(gridView1.GetRowCellValue(i, "sec")))
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Mars = 1 where Rsat_Id = '" + Convert.ToInt32(gridView1.GetRowCellValue(i, "Rsat_Id")).ToString() + "'");
                }
            }

            FisPr pr = new FisPr();
            string sonuc = pr.MarsPr(Convert.ToInt32(this.Tag),new DataTable());
            if (sonuc != "OK")
            {
                MessageBox.Show(sonuc);
            }

            this.Close();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}