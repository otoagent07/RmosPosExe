using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Data;
using System.Windows.Forms;

namespace Pos
{
    public partial class Dep_Secim : DevExpress.XtraEditors.XtraForm
    {
        public DataTable dtDep;

        public Dep_Secim()
        {
            InitializeComponent();
        }

        private void Dep_Secim_Load(object sender, EventArgs e)
        {
            this.BringToFront();

            if (dtDep.Rows.Count > 0)
            {
                for (int i = 0; i < dtDep.Rows.Count; i++)
                {
                    SimpleButton btn = new SimpleButton();
                    btn.Name = i.ToString();
                    btn.Size = new System.Drawing.Size(216, 58);
                    btn.TabIndex = 0;
                    btn.Text = Convert.ToString(dtDep.Rows[i]["Kodlar_Ad"]);
                    btn.Tag = Convert.ToString(dtDep.Rows[i]["Kodlar_Kod"]);
                    btn.TabStop = false;
                    btn.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
                    btn.Click += new EventHandler(tikla_click);
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }
        public string sonuc = string.Empty;
        void tikla_click(object sender, EventArgs e)
        {
            Control mybutton = (Control)sender;
            Departman.Dep_Kodu = Convert.ToString(mybutton.Tag);

            sonuc = Departman.Dep_Param_Yukle();

            //Departman Parametreleri ve Önbüro adresi yüklendi mi?
            if (sonuc == "OK")
            {
                Param.Param_Yukle();
                FisPr.Param_Yukle();
            }
            else
            {
                MessageBox.Show(sonuc + "\n" + "Departman Parametreleri Yüklenemedi...");
            }

            this.Close();
            if (User.otoDirekSatis)
            {
                Program.main.direktsatisac();
            }


        }
    }
}