using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pos.Class;

namespace Pos
{
    public partial class GarsonDegistir : DevExpress.XtraEditors.XtraForm
    {
        public string eskiGarsonKod;
        string yeniGarsonKod = "";
        public string Masa_No;

        public GarsonDegistir()
        {
            InitializeComponent();
        }

        private void GarsonDegistir_Load(object sender, EventArgs e)
        {
            lbl_Eskigarson.Text = eskiGarsonKod + " - " + User.Isim_Getir(eskiGarsonKod);
            gridyenile();
            garsonyenile();

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (Convert.ToString(gridView1.GetRowCellValue(i, "Rsat_Masa")) == Masa_No)
                {
                    gridView1.SetRowCellValue(i, "sec", true);
                }
            }
        }

        private void gridyenile()
        {
            gridColumn1.FieldName = "Rsat_Masa";
            gridColumn2.FieldName = "Rsat_Garson";
            gridColumn3.FieldName = "sec";
            gridColumn4.FieldName = "Rsat_Fisno";

            gridControl1.DataSource = dbtools.SelectTable("select distinct convert(bit,0) as sec, Rsat_Masa,P_Ad + ' ' + P_Soyad as Rsat_Garson,Rsat_Fisno  "
+ " from Cst_Recete_Satis WITH(NOLOCK)  left join Rmosmuh.dbo.Pos_User on Rsat_Garson = P_Kod "
+ " where Rsat_Departman = '" + Departman.Dep_Kodu + "' and Rsat_Ba = 'B' and Rsat_Durum = 'A' and NULLIF(Rsat_Masa,'') is not null  "
+ " order by Rsat_Masa");
        }

        private void garsonyenile()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dt = dbtools.SelectTable("select P_Kod,P_Ad,P_Soyad,P_Kulturu from Rmosmuh.dbo.Pos_User where P_Kulturu <> '2' and P_Kod <> '" + eskiGarsonKod + "'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SimpleButton btnGarson = new SimpleButton();
                btnGarson.Size = new Size(90, 45);
                btnGarson.TabIndex = 0;
                btnGarson.TabStop = false;
                btnGarson.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
                btnGarson.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                btnGarson.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                btnGarson.Appearance.Options.UseBackColor = true;

                btnGarson.Tag = Convert.ToString(dt.Rows[i]["P_Kod"]);

                btnGarson.Text = Convert.ToString(dt.Rows[i]["P_Kod"]) + "\n" + Convert.ToString(dt.Rows[i]["P_Ad"]) + " " + Convert.ToString(dt.Rows[i]["P_Soyad"]);

                btnGarson.Appearance.BackColor = Color.Gainsboro;
                btnGarson.Appearance.BorderColor = btnGarson.Appearance.BackColor;
                btnGarson.Click += new EventHandler(btnGarson_Click);
                flowLayoutPanel1.Controls.Add(btnGarson);
            }
        }

        void btnGarson_Click(object sender, EventArgs e)
        {
            SimpleButton myButton = (SimpleButton)sender;
            foreach (SimpleButton btn in flowLayoutPanel1.Controls)
            {
                btn.Appearance.BackColor = btn.Appearance.BorderColor;
            }
            myButton.Appearance.BackColor = Color.White;

            yeniGarsonKod = Convert.ToString(myButton.Tag);
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void btn_Degstir_Click(object sender, EventArgs e)
        {

            if (Convert.ToString(yeniGarsonKod) == "")
            {
                MessageBox.Show(res_man.GetString("Garson Seçiniz..."));
                return;
            }

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (Convert.ToBoolean(gridView1.GetRowCellValue(i, "sec")))
                {
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Garson = '" + yeniGarsonKod + "' where Rsat_Fisno = '" + Convert.ToInt32(gridView1.GetRowCellValue(i, "Rsat_Fisno")) + "'");
                }
            }

            this.Close();
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            gridView1.SetFocusedRowCellValue("sec", !Convert.ToBoolean(gridView1.GetFocusedRowCellValue("sec")));
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    gridView1.SetRowCellValue(i, "sec", true);
                }
            }
        }


    }
}