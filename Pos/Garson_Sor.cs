using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Data;
using System.Drawing;

namespace Pos
{
    public partial class Garson_Sor : DevExpress.XtraEditors.XtraForm
    {
        public string Garson_Kod = String.Empty;
        public string Garson_AdSoyad = String.Empty;
        public bool cikisbuttonaktif = true;
        public string fisno = "-1";
        public Garson_Sor(bool cikisbuttonaktif=true)
        {
            InitializeComponent();
            this.cikisbuttonaktif = cikisbuttonaktif;
        }

        private void Garson_Sor_Load(object sender, EventArgs e)
        {
            if (cikisbuttonaktif==false)
            {
                btnCikis.Visible = false;
            }
            this.BringToFront();
            if (Convert.ToString(this.Tag) == "PAKET")
            {
                lbl_Baslik.Text = "Paketci Seçiniz...";
            }
            else
            {
                lbl_Baslik.Text = "Garson Seçiniz...";
            }
            Garson_Yenile();
        }

        private void Garson_Yenile()
        {
            string P_Kulturu = "1";
            if (Convert.ToString(this.Tag) == "PAKET")
            {
                P_Kulturu = "2";
            }
            DataTable dtGarson = dbtools.SelectTable("select P_Kod,isnull(P_Ad,'') + ' ' + isnull(P_Soyad,'') as Adsoyad from Rmosmuh.dbo.Pos_User WITH(NOLOCK) where P_Kulturu = '" + P_Kulturu.ToString() + "' and ISNULL(User_AP,1) = 1 order by P_Ad");

            for (int i = 0; i < dtGarson.Rows.Count; i++)
            {
                SimpleButton btnGarson = new SimpleButton();
                btnGarson.Size = new Size(175, 60);
                btnGarson.TabIndex = 0;
                btnGarson.TabStop = false;
                btnGarson.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                btnGarson.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                //btnGarson.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;

                btnGarson.Appearance.BackColor = Color.DarkGreen;

                if (i%2==0)
                {
                    btnGarson.Appearance.BackColor = Color.DeepPink;
                }
               

                btnGarson.Text = Convert.ToString(dtGarson.Rows[i]["Adsoyad"]);
                btnGarson.Tag = Convert.ToString(dtGarson.Rows[i]["P_Kod"]);

                btnGarson.Click += new EventHandler(btnGarson_Click);
                flp_Garson.Controls.Add(btnGarson);
            }
        }

        void btnGarson_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            Garson_Kod = btn.Tag.ToString();
            this.Close();
        }

        public bool cikis = false;
        private void btnCikis_Click(object sender, EventArgs e)
        {
            cikis = true;
            this.Close();
        }

        private void btnPaketciSil_Click(object sender, EventArgs e)
        {
            string q = "update Cst_Recete_Satis set Rsat_Paketci='',paketAtamaTarih=null where Rsat_Fisno='" + fisno+"'";
            dbtools.execcmdR(q);
            this.Close();
        }
    }
}