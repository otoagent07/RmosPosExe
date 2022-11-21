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
    public partial class PaketNot : DevExpress.XtraEditors.XtraForm
    {
        public int Fisno { get; set; }


        public PaketNot()
        {
            InitializeComponent();
        }

        private void PaketNot_Load(object sender, EventArgs e)
        {
            labelControl1.Text = Convert.ToString(dbtools.DegerGetir("select top 1 Rsat_Not from Cst_Recete_Satis where Rsat_Fisno = '" + Fisno + "' ")+"\n");

            AltUrun_Yenile();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            //string not = dbtools.DegerGetir("select top 1 Rsat_Not from Cst_Recete_Satis where Rsat_Fisno = '" + Fisno + "'");

            //not = labelControl1.Text + "\n" + not;

            dbtools.execcmd("update Cst_Recete_Satis set Rsat_Not = '" + labelControl1.Text + "' where Rsat_Fisno = '" + Fisno + "'");
            
            this.Close();
        }

        private void AltUrun_Yenile()
        {
            flowLayoutPanel1.Controls.Clear();

            DataTable dt = dbtools.SelectTable("select Pkod_Ad from Pos_Kodlar where Pkod_Sinif= '23' and Pkod_Kod like 'P%'");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleButton btn_Not = new SimpleButton();
                    btn_Not.Size = new System.Drawing.Size(100, 40);
                    btn_Not.TabIndex = 0;
                    btn_Not.TabStop = false;
                    btn_Not.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    btn_Not.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    btn_Not.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Not.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    btn_Not.Appearance.Options.UseBackColor = true;


                    btn_Not.Text = Convert.ToString(dt.Rows[i]["Pkod_Ad"]);

                    btn_Not.Click += Btn_Not_Click;
                    flowLayoutPanel1.Controls.Add(btn_Not);
                }
            }
        }

        private void Btn_Not_Click(object sender, EventArgs e)
        {
            SimpleButton btn_Not = (SimpleButton)sender;
            labelControl1.Text = labelControl1.Text + "[" + btn_Not.Text + "]";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            labelControl1.Text = "";
        }
    }
}