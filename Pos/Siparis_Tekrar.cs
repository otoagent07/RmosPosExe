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
    public partial class Siparis_Tekrar : DevExpress.XtraEditors.XtraForm
    {
        public Siparis_Tekrar()
        {
            InitializeComponent();
        }

        private void Siparis_Tekrar_Load(object sender, EventArgs e)
        {
            gridyenile();
        }

        private void gridyenile()
        {
            gridColumn1.FieldName = "Saat";

            gridControl1.DataSource = dbtools.SelectTable("select distinct CONVERT(varchar(5),Rsat_Satissaat,108) as Saat from Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and Rsat_Ba = 'B'");
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                gridyenile2(Convert.ToString(gridView1.GetFocusedRowCellValue("Saat")));
            }
        }

        private void gridyenile2(string saat)
        {
            gridColumn2.FieldName = "Rec_Ad";
            gridColumn3.FieldName = "Rsat_Emiktar";
            gridColumn4.FieldName = "Rsat_Miktar";
            gridColumn5.FieldName = "Rsat_Id";
            gridColumn6.FieldName = "Garson";

            gridControl2.DataSource = dbtools.SelectTable(@"select Rsat_Id,Rec_Ad,case when Rsat_Emiktar = 'T' then '' else Rsat_Emiktar end as Rsat_Emiktar,Rsat_Miktar,P_Ad + ' ' + P_Soyad as Garson
from Cst_Recete_Satis 
left join Cst_Recete on Rsat_Recete = Rec_Genelkod
left join Rmosmuh.dbo.Pos_User on P_Kod = Rsat_Garson
where Rsat_Fisno = '" + Convert.ToInt32(this.Tag) + "' and CONVERT(varchar(5),Rsat_Satissaat,108) = '" + saat + "'");
        }

        private void btn_Gonder_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                string ids = "";
                for (int i = 0; i < gridView2.RowCount; i++)
                {
                    ids += Convert.ToString(gridView2.GetRowCellValue(i, "Rsat_Id")) + ",";
                }


                dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr = 0 where Rsat_Id in (" + ids.Substring(0, ids.Length - 1) + ")");
                FisPr pr = new FisPr();
                string sonuc = pr.SiparisPr(Convert.ToInt32(this.Tag), false,0);
                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc);
                }

                dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr = 1 where Rsat_Id in (" + ids.Substring(0, ids.Length - 1) + ")");

            }
        }



    }
}