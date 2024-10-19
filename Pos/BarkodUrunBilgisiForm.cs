using Pos.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos
{
    public partial class BarkodUrunBilgisiForm : Form
    {
        public BarkodUrunBilgisiForm()
        {
            InitializeComponent();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BarkodUrunBilgisiForm_Load(object sender, EventArgs e)
        {
            txtAd.Text = "";
            txtTutar.Text = "";
            txtBarkod.Text = "";


            barkodfocus();
        }

        void barkodfocus()
        {
            txtBarkod.Text = "";
            txtBarkod.Focus();
            txtBarkod.Select();
        }
        private void txtBarkod_Leave(object sender, EventArgs e)
        {
            DataTable dtR = dbtools.SelectTable("select top 1 Rec_Genelkod,Rec_Ad,Rec_Fiyat from Cst_Recete WITH(NOLOCK)  where Rec_Barkod = '" + txtBarkod.Text + "'");

            if (dtR==null || dtR.Rows.Count<1)
            {
                txtAd.Text = "ÜRÜN BULUNAMADI!";
                txtTutar.Text = "ÜRÜN YOK!";
                barkodfocus();
                return;
            }

            txtAd.Text = dtR.Rows[0]["Rec_Ad"].ToString();
            txtTutar.Text = dtR.Rows[0]["Rec_Fiyat"].ToString();
            barkodfocus();

        }
    }
}
