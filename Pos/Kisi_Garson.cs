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
    public partial class Kisi_Garson : DevExpress.XtraEditors.XtraForm
    {
        public decimal Kisi = 0;
        public string Garson_Kodu = String.Empty;
        public bool Iptal = false;

        Klavye1 kisi;
        Garson_Sor garson;
        bool Cik_1 = true, Cik_2 = true;

        public Kisi_Garson()
        {
            InitializeComponent();
        }

        private void Kisi_Garson_Load(object sender, EventArgs e)
        {
            this.BringToFront();

            bool kisi1 = (Convert.ToString(this.Tag) != "D" && Departman.Kisi_Sor);
            bool kisi2 = (Convert.ToString(this.Tag) == "D" && !Departman.Kodlar_Kisisorma_Pda);

            if (kisi1 || kisi2)
            {
                kisi = new Klavye1();
                kisi.Tag = "KISISAYISI";
                kisi.TopLevel = false;
                kisi.AutoScroll = true;
                kisi.WindowState = FormWindowState.Maximized;
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
                if (!Departman.Garson_Sor)
                {
                    this.Width = kisi.Width + 20;
                    this.StartPosition = FormStartPosition.CenterScreen;
                }
                splitContainerControl1.Panel1.Controls.Add(kisi);
                kisi.Show();
                Cik_1 = false;
            }

            if (Departman.Garson_Sor)
            {
                garson = new Garson_Sor();
                garson.Tag = "GARSON";
                garson.TopLevel = false;
                garson.AutoScroll = true;
                garson.WindowState = FormWindowState.Maximized;
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel2;
                splitContainerControl1.Panel2.Controls.Add(garson);
                garson.Show();
                Cik_2 = false;
            }

            if (Departman.Garson_Sor && (kisi1 || kisi2))
            {
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
            }
        }

        private void splitContainerControl1_Panel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            Kisi = kisi.sayi;

            Cik_1 = true;
            if (Cik_1 && Cik_2)
            {
                this.Close();
            }
        }

        private void splitContainerControl1_Panel2_ControlRemoved(object sender, ControlEventArgs e)
        {
            Garson_Kodu = garson.Garson_Kod;

            Cik_2 = true;
            if (Cik_1 && Cik_2)
            {
                this.Close();
            }
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            Iptal = true;
            this.Close();
        }



    }
}