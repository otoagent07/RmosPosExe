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
    public partial class Detay_Iptal : DevExpress.XtraEditors.XtraForm
    {
        public Detay_Iptal()
        {
            InitializeComponent();
        }

        private void Detay_Iptal_Load(object sender, EventArgs e)
        {
            gridyenile();
        }

        private void gridyenile()
        {
            gridColumn1.FieldName = "Rec_Ad";
            gridColumn2.FieldName = "Rsat_Miktar";
            gridColumn4.FieldName = "Rsat_Tutar";
            gridColumn6.FieldName = "Rsat_Ba";

            gridControl1.DataSource = dbtools.SelectTable("declare @Fisno int = " + Convert.ToInt32(spn_Fisno.EditValue).ToString()
+ "   select Rec_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_Ba "
+ " from Cst_Satis_Ipt as ipt "
+ " left join Cst_Recete as rec on ipt.Rsat_Recete = rec.Rec_Genelkod  "
+ " where ipt.Rsat_Ba = 'B' and Rsat_Fisno = @Fisno "
+ " union all "
+ " select Pkod_Ad,Rsat_Miktar,Rsat_Tutar,Rsat_Ba "
+ " from Cst_Satis_Ipt as ipt "
+ " left join Pos_Kodlar as odeme on ipt.Rsat_Kapatma = odeme.Pkod_Kod and Pkod_Sinif = '11' "
+ " where ipt.Rsat_Ba = 'A' and Rsat_Fisno = @Fisno");
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}