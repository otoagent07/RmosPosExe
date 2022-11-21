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
    public partial class Siparis_Not : DevExpress.XtraEditors.XtraForm
    {
        public int Fisno { get; set; }

        FisPr pr;

        public Siparis_Not()
        {
            InitializeComponent();

            this.BringToFront();
        }

        private void Siparis_Not_Load(object sender, EventArgs e)
        {
            pr = new FisPr();
            DataTable dtPr = pr.SiparisPrinterBul(Fisno, 0, true);

            DataTable dt = new DataTable();
            dt.Columns.Add("Printer", typeof(string));

            for (int i = 0; i < dtPr.Rows.Count; i++)
            {
                string printer = Convert.ToString(dtPr.Rows[i]["Printer"]);

                if (Convert.ToString(dtPr.Rows[i]["Mac_Printer"]) != "")
                {
                    printer = Convert.ToString(dtPr.Rows[i]["Mac_Printer"]);
                }

                dt.Rows.Add(printer);
            }

            gridColumn1.FieldName = "Printer";
            gridControl1.DataSource = dt;
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Gonder_Click(object sender, EventArgs e)
        {
            if (memoEdit1.Text == "")
            {
                return;
            }
            pr.SiparisNotPr(Fisno, memoEdit1.Text, Convert.ToString(gridView1.GetFocusedRowCellValue("Printer")));

        }
    }
}