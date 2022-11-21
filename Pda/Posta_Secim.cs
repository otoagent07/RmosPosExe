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
    public partial class Posta_Secim : DevExpress.XtraEditors.XtraForm
    {
        public string Posta { get; set; }
        public Posta_Secim()
        {
            InitializeComponent();
        }

        private void Posta_Secim_Load(object sender, EventArgs e)
        {
            DataTable dt = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad from Pos_kodlar where Pkod_Sinif = '18' and Pkod_Dep = '" + Departman.Dep_Kodu + "'");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleButton btn = new SimpleButton();
                    btn.Name = i.ToString();
                    btn.Size = new System.Drawing.Size(100, 30);
                    btn.TabIndex = 0;
                    btn.Text = Convert.ToString(dt.Rows[i]["Pkod_Ad"]);
                    btn.Tag = Convert.ToString(dt.Rows[i]["Pkod_Kod"]);
                    btn.TabStop = false;
                    btn.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    btn.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn.Click += new EventHandler(btn_Click);
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            Control mybutton = (Control)sender;
            Posta = Convert.ToString(mybutton.Tag);
            
            this.Close();
        }

        private void btn_TumMasalar_Click(object sender, EventArgs e)
        {
            Posta = "";
            this.Close();
        }


    }
}