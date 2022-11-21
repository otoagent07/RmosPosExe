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
using System.IO;

namespace Pos
{
    public partial class Pos_Plu_Terazi : DevExpress.XtraEditors.XtraForm
    {
        public Pos_Plu_Terazi()
        {
            InitializeComponent();
        }

        private void Doldur()
        {
            DataTable dt = dbtools.SelectTable("set dateformat dmy ; select Kodlar_Kod, Kodlar_Ad from Stok_Kodlar where Kodlar_Sinif ='01' and Kodlar_Anadepo = 'False' and Kodlar_Satis = 'True' order by Kodlar_Kod");
            if (dt.Rows.Count > 0)
            {
                checkedComboBoxEdit1.Properties.DataSource = dt;
                checkedComboBoxEdit1.Properties.DisplayMember = "Kodlar_Ad";
                checkedComboBoxEdit1.Properties.ValueMember = "Kodlar_Kod";
                checkedComboBoxEdit1.CheckAll();
            }
        }

        private void Pos_Plu_Terazi_Load(object sender, EventArgs e)
        {
            Doldur();
        }

        private void AnaAraKodlar()
        {
            DataTable dt = dbtools.SelectTable("exec Cost_Recete_Liste @Liste_Tipi=1,@Departman='" + Convert.ToString(checkedComboBoxEdit1.EditValue) +"'");
            if (dt.Rows.Count > 0)
            {
                gridControl2.DataSource = dt;
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            gridControl2.DataSource = null;
            AnaAraKodlar();
        }

        private void txtOlustur()
        {
            StringBuilder s = new StringBuilder();

            int Bosluk = 30;
            string metinBosluk = Console.ReadLine();

            for (int i = 0; i < gridView2.GetSelectedRows().Length ; i++)
            {
                int index = gridView2.GetSelectedRows()[i];
                s.Append(1);
                for (int j = 0; j < Bosluk - 1; j++)
                {
                    metinBosluk += " ";
                    
                }
                s.Append(metinBosluk);
                metinBosluk = String.Empty;
                s.Append(Convert.ToString(gridView2.GetRowCellValue(index,"Rec_Genelkod")));
                for (int j = 0; j < Bosluk - Convert.ToString(gridView2.GetRowCellValue(index, "Rec_Genelkod")).Length; j++)
                {
                    metinBosluk += " ";
                }
                s.Append(metinBosluk);
                metinBosluk = String.Empty;
                s.Append(1);
                for (int j = 0; j < Bosluk - 1; j++)
                {
                    metinBosluk += " ";

                }
                s.Append(metinBosluk);
                metinBosluk = String.Empty;
                s.Append(Convert.ToString(gridView2.GetRowCellValue(index, "Rec_Genelkod")));
                for (int j = 0; j < Bosluk - Convert.ToString(gridView2.GetRowCellValue(index, "Rec_Genelkod")).Length; j++)
                {
                    metinBosluk += " ";
                }
                s.Append(metinBosluk);
                metinBosluk = String.Empty;
                s.Append(Convert.ToString(gridView2.GetRowCellValue(index, "Rec_Ad")));
                for (int j = 0; j < Bosluk - Convert.ToString(gridView2.GetRowCellValue(index, "Rec_Ad")).Length; j++)
                {
                    metinBosluk += " ";
                }
                s.Append(metinBosluk);
                metinBosluk = String.Empty;
                s.Append(Convert.ToString(gridView2.GetRowCellValue(index, "Rec_Fiyat")).Replace(",","").Replace(".",""));
                for (int j = 0; j < Bosluk - Convert.ToString(gridView2.GetRowCellValue(index, "Rec_Fiyat")).Length; j++)
                {
                    metinBosluk += " ";
                }
                s.Append(metinBosluk);
                metinBosluk = String.Empty;
                s.Append(Environment.NewLine);
            }

            StreamWriter sw = new StreamWriter(@"Barkod.TXT");
            sw.Write(s.ToString());
            sw.Flush();
            sw.Close();

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            txtOlustur();
        }

        private void checkedComboBoxEdit1_EditValueChanged(object sender, EventArgs e)
        {
            gridControl2.DataSource = null;
            AnaAraKodlar();
        }
    }
}