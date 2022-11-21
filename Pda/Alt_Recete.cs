using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;
using System.Data.SqlClient;

namespace Pda
{
    public partial class Alt_Recete : DevExpress.XtraEditors.XtraForm
    {
        public string ustReceteKodu { get; set; }
        public string ustReceteAdi { get; set; }

        public string altReceteKodu { get; set; }

        public Alt_Recete()
        {
            InitializeComponent();
        }

        private void Alt_Recete_Load(object sender, EventArgs e)
        {
            this.Size = new Size(Param.Pda_Width - 30, Param.Pda_Height - 30);
            if (Param.Param_Pda_Fullscreen) this.WindowState = FormWindowState.Maximized;

            lbl_Baslik.Text = ustReceteAdi + " " + lbl_Baslik.Text;
            AltUrun_Yenile();
        }

        private void AltUrun_Yenile()
        {
            flp_Altrecete.Controls.Clear();

            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Cost_Recete_Liste";
            com.Parameters.AddWithValue("@Liste_Tipi", 4);
            com.Parameters.AddWithValue("@Departman", Departman.Dep_Kodu);
            com.Parameters.AddWithValue("@Ust_Recete", ustReceteKodu);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleButton btn_AltRecete = new SimpleButton();
                    btn_AltRecete.Size = new System.Drawing.Size(65, 30);
                    btn_AltRecete.TabIndex = 0;
                    btn_AltRecete.TabStop = false;
                    btn_AltRecete.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    btn_AltRecete.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    btn_AltRecete.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_AltRecete.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    btn_AltRecete.Appearance.Options.UseBackColor = true;


                    if (Param.Calisma_Sekli == 0) btn_AltRecete.Text = Convert.ToString(dt.Rows[i]["Rec_Ad"]) + "\n" + Convert.ToString(dt.Rows[i]["Rec_Fiyat"]);
                    if (Param.Calisma_Sekli == 1) btn_AltRecete.Text = Convert.ToString(dt.Rows[i]["Rec_Ad"]) + "\n" + Convert.ToString(dt.Rows[i]["Rec_Dovifiyat"]);
                    btn_AltRecete.Tag = Convert.ToString(dt.Rows[i]["Rec_Genelkod"]);

                    btn_AltRecete.Click += new EventHandler(btn_AltRecete_Click);
                    flp_Altrecete.Controls.Add(btn_AltRecete);
                }
            }
        }

        void btn_AltRecete_Click(object sender, EventArgs e)
        {
            SimpleButton btn_Urun = (SimpleButton)sender;
            altReceteKodu = btn_Urun.Tag.ToString();
            this.Close();
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            altReceteKodu = "";
            this.Close();
        }


    }
}