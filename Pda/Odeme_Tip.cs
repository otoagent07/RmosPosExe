using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pda
{
    public partial class Odeme_Tip : DevExpress.XtraEditors.XtraForm
    {
        public int Fis_No;
        public string Satis_Tip;

        public Odeme_Tip()
        {
            InitializeComponent();
        }

        private void Odeme_Tip_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width, Param.Pda_Height);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;

            Kapatma_Yenile();
        }

        private void Kapatma_Yenile()
        {
            flp_Kapatma.Controls.Clear();
            DataTable dt = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad from Pos_Kodlar with(nolock) where Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' and Pkod_Ozelkod <> '8' order by Pkod_Kod ");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    SimpleButton btn_Kapatma = new SimpleButton();
                    btn_Kapatma.Size = new System.Drawing.Size(150, 30);
                    btn_Kapatma.TabIndex = 0;
                    btn_Kapatma.TabStop = false;
                    btn_Kapatma.Font = new System.Drawing.Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    btn_Kapatma.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btn_Kapatma.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Kapatma.Appearance.Options.UseBackColor = true;

                    btn_Kapatma.Text = Convert.ToString(dt.Rows[i]["Pkod_Ad"]);
                    btn_Kapatma.Tag = Convert.ToString(dt.Rows[i]["Pkod_Kod"]);


                    btn_Kapatma.Click += new EventHandler(btn_Kapatma_Click);
                    flp_Kapatma.Controls.Add(btn_Kapatma);
                }
            }
        }

        void btn_Kapatma_Click(object sender, EventArgs e)
        {
            SimpleButton btn_Kapatma = (SimpleButton)sender;
            Satis_Tip = Convert.ToString(btn_Kapatma.Tag);
            this.Close();



        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            Satis_Tip = "";
            this.Close();
        }

        

    }
}