using Pos.Class;
using Pos.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos.Trendyol
{
    public partial class TrendyolOnayForm : Form
    {
        string siparisId = "", fisno = "";
        public TrendyolOnayForm(string siparisId,string fisno)
        {
            InitializeComponent();
            this.siparisId = siparisId;
            this.fisno = fisno;
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static string MyClass = "TrendyolOnayForm";
        
        private void btnTrendyolSiparisHazirlandi_Click(object sender, EventArgs e)
        {
            TrendyolController trendyolController = new TrendyolController(siparisId,fisno);
            trendyolController.hazirlandi();
            this.Close();
        }

        private void btnTrendyolSiparisYolaCikti_Click(object sender, EventArgs e)
        {
            TrendyolController trendyolController = new TrendyolController(siparisId, fisno);
            trendyolController.yolaCikti();
            this.Close();
        }

        private void btnTrendyolSiparisTeslimEdildi_Click(object sender, EventArgs e)
        {
            TrendyolController trendyolController = new TrendyolController(siparisId, fisno);
            trendyolController.teslimEdildi();
            this.Close();
        }

    }
}
