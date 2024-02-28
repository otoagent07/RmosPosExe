using DevExpress.XtraEditors;
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
    public partial class KisiyeSatisSec : Form
    {
        int fisno = -1;
        public KisiyeSatisSec(int fisno)
        {
            InitializeComponent();
            this.fisno = fisno;
        }

        public bool iptal = false;
        private void btnIptal_Click(object sender, EventArgs e)
        {
            iptal = true;
            this.Close();
        }

        private void KisiyeSatisSec_Load(object sender, EventArgs e)
        {
            DataTable dataTable = dbtools.SelectTableR($"select isnull(kisiyeSatisAdSoyad,'') as kisiyeSatisAdSoyad from Cst_Recete_Satis  where Rsat_Fisno={fisno} group by kisiyeSatisAdSoyad");

            foreach (DataRow item in dataTable.Rows)
            {
                MyFlowDoldur(item["kisiyeSatisAdSoyad"].ToString());
            }
          

        }
        public void MyFlowDoldur(string MasaIsmi)
        {
            try
            {
                SimpleButton button = new SimpleButton();
                button.Height = 70;
                button.Width = 135;
                button.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
                button.Text = MasaIsmi;
                button.Tag = MasaIsmi;
                button.Name = MasaIsmi;
            
                button.Click += Button_Click;
                flowLayoutPanel1.Controls.Add(button);
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA! MyFlowDoldur()" + ex.Message);
            }
        }

        public string sayac = "1";
        public string adsoyad = "";
        public string adsoyadTam = "";

        private void Button_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            string isim = control.Name;
            adsoyadTam = isim;
            if (isim!="" && isim.Contains("-"))
            {
                sayac = isim.Split('-')[0].ToString();
                adsoyad = isim.Split('-')[1].ToString();
            }

            this.Close();
        }



    }
}
