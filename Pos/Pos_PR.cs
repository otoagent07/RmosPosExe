using DevExpress.Mvvm.Native;
using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Pos_PR : DevExpress.XtraEditors.XtraForm
    {
        public Pos_PR()
        {
            InitializeComponent();
        }

        public DataTable dtPr;

        public string Tip = "S";

        public string Fisno = String.Empty;

        private void Pos_PR_Load(object sender, EventArgs e)
        {
            this.BringToFront();

            dtPr = dbtools.SelectTable("select P_Kod , P_Ad + ' ' + P_Soyad as PR from RmosMuh.dbo.Pos_User where P_Kulturu = 4");

            if (dtPr.Rows.Count > 0)
            {
                for (int i = 0; i < dtPr.Rows.Count; i++)
                {
                    SimpleButton btn = new SimpleButton();
                    btn.Name = i.ToString();
                    btn.Size = new System.Drawing.Size(150, 60);
                    btn.TabIndex = 0;
                    btn.Text = Convert.ToString(dtPr.Rows[i]["PR"]);
                    btn.Tag = Convert.ToString(dtPr.Rows[i]["P_Kod"]);
                    btn.TabStop = false;
                    btn.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    btn.Click += new EventHandler(tikla_click);
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
            else
            {
                MessageBox.Show(res_man.GetString("Lütfen Kullanıcı Kodlarından PR Tanımlayınız..."));
                sonuc = "";
                this.Close();
            }

        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());

        public string sonuc = string.Empty;

        public string PRKodu = string.Empty;

        void tikla_click(object sender, EventArgs e)
        {
            Control mybutton = (Control)sender;
            PRKodu = Convert.ToString(mybutton.Tag);

            if (Tip == "S")
            {
                sonuc = "OK";
                this.Close();
            }
            else
            {
                dbtools.execcmd("update Cst_Recete_Satis Set Rsat_PR = '" + PRKodu + "' where Rsat_Fisno = '" + Fisno+  "'");
                this.Close();
            }
       
        }

        private void Pos_PR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && Keys.F4 == e.KeyCode)
            {
                return;
            }
        }
    }
}