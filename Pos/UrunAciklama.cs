using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Data;

namespace Pos
{
    public partial class UrunAciklama : DevExpress.XtraEditors.XtraForm
    {
        public UrunAciklama()
        {
            InitializeComponent();
        }

        public string receteKodu { get; set; }

        private void AltUrun_Yenile()
        {
            flp_UrunAciklama.Controls.Clear();

            DataTable dt = dbtools.SelectTable(@"Select
            Pkod_Id, 
            Pkod_Ad,
            ISNULL(Pkod_AciklamaY,50) as Pkod_AciklamaY, 
            ISNULL(Pkod_AciklamaG,150) as Pkod_AciklamaG 
            from Cst_Recete
            left join Pos_Kodlar on Convert(nvarchar(max),Pkod_Id) in (SELECT fieldvalue FROM dbo.stringArray(Rec_AciklamaGrup,','))
            Where Rec_Genelkod = '" + receteKodu + "'");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CheckButton chk_Aciklama = new CheckButton();
                    chk_Aciklama.Size = new System.Drawing.Size(Convert.ToInt32(dt.Rows[i]["Pkod_AciklamaG"]), Convert.ToInt32(dt.Rows[i]["Pkod_AciklamaY"]));
                    chk_Aciklama.TabIndex = 0;
                    chk_Aciklama.TabStop = false;
                    chk_Aciklama.Checked = false;
                    chk_Aciklama.LookAndFeel.UseDefaultLookAndFeel = false;
                    chk_Aciklama.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    chk_Aciklama.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    chk_Aciklama.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    chk_Aciklama.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    //chk_Aciklama.Appearance.Options.UseBackColor = true;
                    chk_Aciklama.LookAndFeel.SkinName = "iMaginary";
                    chk_Aciklama.Text = Convert.ToString(dt.Rows[i]["Pkod_Ad"]);
                    chk_Aciklama.Tag = Convert.ToString(dt.Rows[i]["Pkod_Id"]);

                    chk_Aciklama.CheckedChanged += new EventHandler(chk_Aciklama_Click);
                    flp_UrunAciklama.Controls.Add(chk_Aciklama);
                }
            }
        }

        public string Aciklama = "";
        void chk_Aciklama_Click(object sender, EventArgs e)
        {
            CheckButton btn_Urun = (CheckButton)sender;
            btn_Urun.Checked = true;
            Aciklama += "  " +btn_Urun.Text.ToString();
            lbl_Aciklama.Text += "  " + btn_Urun.Text;
        }

        private void UrunAciklama_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            lbl_Aciklama.Text = "";
            AltUrun_Yenile();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            Aciklama = "";
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            AltUrun_Yenile();
            Aciklama = "";
            lbl_Aciklama.Text = "";
        }
    }
}