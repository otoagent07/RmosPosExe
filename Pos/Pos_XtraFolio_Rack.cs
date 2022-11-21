using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pos
{
    public partial class Pos_XtraFolio_Rack : DevExpress.XtraEditors.XtraForm
    {
        public Pos_XtraFolio_Rack()
        {
            InitializeComponent();
        }

        public string Filtre = "";

        private void Load_Room()
        {
            flowLayoutPanel1.Controls.Clear();

            Color Normal = Color.ForestGreen, CompRenk = Color.DeepSkyBlue;

            DataTable dtOda = Fronttools.SelectTable(@"select Rez_Id,Rez_Adi_1,Rez_Odano,Rez_Adi_2,Rez_Giris_tarihi,Rez_Cikis_tarihi,Rez_limit_uyari_eh,
                                                    ISNULL(Rez_Toplam_kisi,0) as Rez_Toplam_kisi , Rez_Odeme as CompKodu
                                                    from dbo.Rez 
                                                    where Rez_R_I_H = 'I' and Rez_Master_detay <> 'D' " + Filtre + @"
                                                    order by Rez_Odano");



            string OdaSize = "110;60";
            Size s = new Size(Convert.ToInt32(OdaSize.Split(';')[0]), Convert.ToInt32(OdaSize.Split(';')[1]));

            for (int i = 0; i < dtOda.Rows.Count; i++)
            {
                SimpleButton btnOda = new SimpleButton();
                btnOda.Size = s;
                btnOda.TabIndex = 0;
                btnOda.TabStop = false;
                btnOda.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                btnOda.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                btnOda.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                btnOda.Appearance.Options.UseBackColor = true;

                btnOda.Tag = Convert.ToString(dtOda.Rows[i]["Rez_Odano"]);

                //Normal Oda
                if (Convert.ToString(dtOda.Rows[i]["CompKodu"]) != Param.Fullcomp_Kodu)
                {
                    btnOda.Appearance.BackColor = Normal;
                    btnOda.Text = Convert.ToString(dtOda.Rows[i]["Rez_Odano"]);
                }

                //Comp Oda
                if (Convert.ToString(dtOda.Rows[i]["CompKodu"]) == Param.Fullcomp_Kodu)
                {
                    btnOda.Appearance.BackColor = CompRenk;
                    btnOda.Text = Convert.ToString(dtOda.Rows[i]["Rez_Odano"]) + "\n" + "Full Comp";
                }

                btnOda.Appearance.BorderColor = btnOda.Appearance.BackColor;
                btnOda.Click += new EventHandler(btnOda_Click);
                flowLayoutPanel1.Controls.Add(btnOda);

            }

        }

        public DataTable dt = new DataTable();
        void btnOda_Click(object sender, EventArgs e)
        {
            SimpleButton myButton = (SimpleButton)sender;
            foreach (SimpleButton btn in flowLayoutPanel1.Controls)
            {
                btn.Appearance.BackColor = btn.Appearance.BorderColor;
            }
            myButton.Appearance.BackColor = Color.White;

            dt = new DataTable();
            dt = Fronttools.SelectTable(@"select Rez_Id,Rez_Adi_1,Rez_Odano,Rez_Adi_2,Rez_Giris_tarihi,Rez_Cikis_tarihi,Rez_limit_uyari_eh,
                                                    ISNULL(Rez_Toplam_kisi,0) as Rez_Toplam_kisi , Rez_Odeme as CompKodu
                                                    from dbo.Rez 
                                                    where Rez_R_I_H = 'I' and Rez_Master_detay <> 'D' and Rez_Odano = '" + myButton.Tag + @"'
                                                    " + Filtre + @"
                                                    order by Rez_Odano");

            if (dt.Rows.Count > 0)
            {
                txt_Odano.Text = Convert.ToString(dt.Rows[0]["Rez_Odano"]);
                txt_FolioID.Text = Convert.ToString(dt.Rows[0]["Rez_Id"]);
                txt_Adsoyad.Text = Convert.ToString(dt.Rows[0]["Rez_Adi_1"]) + " " + Convert.ToString(dt.Rows[0]["Rez_Adi_2"]);
                date_Giris.EditValue = Convert.ToDateTime(dt.Rows[0]["Rez_Giris_tarihi"]).ToString("dd.MM.yyyy");
                date_Cikis.EditValue = Convert.ToDateTime(dt.Rows[0]["Rez_Cikis_tarihi"]).ToString("dd.MM.yyyy");
                txt_Kisisayisi.Text = Convert.ToString(dt.Rows[0]["Rez_Toplam_kisi"]);
            }

        }

        private void Pos_XtraFolio_Rack_Load(object sender, EventArgs e)
        {
            Load_Room();
        }

        public bool Durum = false;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
            Durum = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
            Durum = true;
        }
    }
}