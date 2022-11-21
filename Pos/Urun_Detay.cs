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
    public partial class Urun_Detay : DevExpress.XtraEditors.XtraForm
    {
        public Urun_Detay()
        {
            InitializeComponent();
        }

        private void Urun_Detay_Load(object sender, EventArgs e)
        {
            txt_Filtre.Focus();
        }

        private void btn_Ara_Click(object sender, EventArgs e)
        {
            if (txt_Filtre.Text != "")
            {
                gridControl1.DataSource = dbtools.SelectTable("select Rec_Genelkod,Rec_Ad,Rec_Fiyat from Cst_Recete where Rec_Ad like '" + txt_Filtre.Text + "%'");
                gridView1.BestFitColumns();
                txt_Filtre.Text = "";
                txt_Filtre.Focus();
            }
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}