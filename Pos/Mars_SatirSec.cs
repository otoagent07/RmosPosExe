using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pos
{
    public partial class Mars_SatirSec : DevExpress.XtraEditors.XtraForm
    {
        public Mars_SatirSec()
        {
            InitializeComponent();
        }

        private void Mars_SatirSec_Load(object sender, EventArgs e)
        {
            gridyenile();
        }

        private void gridyenile()
        {
            gridControl1.DataSource = dbtools.SelectTable("select convert(bit,1) as sec, Rsat_Id,Rsat_Recete,Rec_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_Mars "
                    + " from Cst_Recete_Satis "
                    + " left join Cst_Recete on Rsat_Recete = Rec_Genelkod "
                    + " where Rsat_Ba = 'B' and Rsat_Fisno = '" + txt_Fisno.Text + "' and ISNULL(Rsat_Mars,0) = 0");
        }


        public bool cikis = false;
        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            cikis = true;
            this.Close();
        }

        private void btn_Marsla_Click(object sender, EventArgs e)
        {

            DataTable dataTable = dbtools.SelectTableR("select Rsat_Id from Cst_Recete_Satis where Rsat_Fisno = '" + txt_Fisno.Text + "' and Rsat_Mars = 1 ");

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (Convert.ToBoolean(gridView1.GetRowCellValue(i, "sec")))
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Mars = 1 where Rsat_Id = '" + Convert.ToInt32(gridView1.GetRowCellValue(i, "Rsat_Id")).ToString() + "'");
                }

            }


            FisPr pr = new FisPr();
            string sonuc = pr.MarsPr(Convert.ToInt32(txt_Fisno.Text), dataTable);
            if (sonuc != "OK")
            {
                MessageBox.Show(sonuc);
            }

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (Convert.ToBoolean(gridView1.GetRowCellValue(i, "sec")))
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr = 1 where Rsat_Id = '" + Convert.ToInt32(gridView1.GetRowCellValue(i, "Rsat_Id")).ToString() + "'");
                }

                dbtools.execcmd("update Cst_Recete_Satis set rezevePrintCiktimi=1 where Rsat_Id = '" + Convert.ToInt32(gridView1.GetRowCellValue(i, "Rsat_Id")).ToString() + "'");

            }



            this.Close();


        }

    }
}