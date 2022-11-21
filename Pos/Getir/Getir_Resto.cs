using DevExpress.XtraEditors;
using Pos.Getir.Class;
using System;
using System.Data;
using System.Drawing;

namespace Pos.Getir
{
    public partial class Getir_Resto : DevExpress.XtraEditors.XtraForm
    {
        public Getir_Resto()
        {
            InitializeComponent();
        }

        private void btnLoad()
        {
            DataTable dt = new DataTable("RestoDurum");

            dt.Columns.Add("tip", typeof(int));
            dt.Columns.Add("adi", typeof(string));
            dt.Columns.Add("link", typeof(string));


            dt.Rows.Add(0, "RESTORAN AÇIK", "/status/open");
            dt.Rows.Add(0, "RESTORAN KAPALI", "/status/close");
            dt.Rows.Add(1, "KURYE VAR", "/courier/enable");
            dt.Rows.Add(1, "KURYE YOK", "/courier/disable");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SimpleButton btn_Urun = new SimpleButton();
                    btn_Urun.Size = new Size(250, 90);
                    btn_Urun.TabIndex = 0;
                    btn_Urun.TabStop = false;
                    btn_Urun.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    btn_Urun.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                    btn_Urun.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Urun.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    btn_Urun.Appearance.Options.UseBackColor = false;
                    btn_Urun.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                    btn_Urun.Tag = Convert.ToString(dt.Rows[i]["link"]) + ";" + Convert.ToString(dt.Rows[i]["tip"]);
                    btn_Urun.Text = Convert.ToString(dt.Rows[i]["adi"]);

                    btn_Urun.Click += new EventHandler(btn_Urun_Click);
                    flowLayoutPanel1.Controls.Add(btn_Urun);
                }
            }
        }

        GetirApi getirApi = new GetirApi();
        void btn_Urun_Click(object sender, EventArgs e)
        {
            SimpleButton btn_Urun = (SimpleButton)sender;
            string[] veri = btn_Urun.Tag.ToString().Split(';');
            if (veri[1] == "0")
            {
                string result = getirApi.putRequestS(GetirStatik.requestRestoran + veri[0], GetirToken.apitoken);
                this.Close();
            }
            else
            {
                GetirOnay result = getirApi.postRequestS<GetirOnay>(GetirStatik.requestRestoran + veri[0], GetirToken.apitoken);
                if (result.result)
                {
                    this.Close();
                }
            }            
        }
        private void Getir_Resto_Load(object sender, EventArgs e)
        {
            btnLoad();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}