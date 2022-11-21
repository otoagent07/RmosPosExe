using Pos.Class;
using System;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Fihrist : DevExpress.XtraEditors.XtraForm
    {
        public int F_Id { get; set; }

        public Fihrist()
        {
            InitializeComponent();
        }

        private void Fihrist_Load(object sender, EventArgs e)
        {
            gridyenile();
        }

        private void gridyenile()
        {
            grd_Fihrist.DataSource = dbtools.SelectTable("select * from Pos_Fihrist order by F_Id desc");

            F_Id = 0;
            txt_Ad.Text = String.Empty;
            txt_Soyad.Text = String.Empty;
            txt_Tel1.Text = String.Empty;
            txt_Tel2.Text = String.Empty;
            txt_Adres.Text = String.Empty;
            txt_AdresTarif.Text = String.Empty;
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                F_Id = Convert.ToInt32(gridView1.GetFocusedRowCellValue("F_Id"));
                txt_Ad.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("F_Ad"));
                txt_Soyad.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("F_Soyad"));
                txt_Tel1.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("F_Tel1"));
                txt_Tel2.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("F_Tel2"));
                txt_Adres.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("F_Adres"));
                txt_AdresTarif.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("F_AdresTarif"));
            }
        }

        private void btn_Kaydet_Click(object sender, EventArgs e)
        {
            dxValidationProvider1.Validate();

            if (dxValidationProvider1.GetInvalidControls().Count > 0)
            {
                return;
            }
            
            dbtools.execcmd("INSERT INTO Pos_Fihrist(F_Ad, F_Soyad, F_Tel1, F_Tel2, F_Adres, F_AdresTarif) "
                + " VALUES     ('" + txt_Ad.Text + "','" + txt_Soyad.Text + "','" + txt_Tel1.Text + "','" + txt_Tel2.Text + "','" + txt_Adres.Text + "','" + txt_AdresTarif.Text + "')");
            gridyenile();
        }

        private void btn_Duzelt_Click(object sender, EventArgs e)
        {
            dxValidationProvider1.Validate();

            if (dxValidationProvider1.GetInvalidControls().Count > 0)
            {
                return;
            }

            if (F_Id != 0)
            {
                dbtools.execcmd("UPDATE    Pos_Fihrist "
                    + " SET  F_Ad ='" + txt_Ad.Text + "', F_Soyad ='" + txt_Soyad.Text + "', F_Tel1 ='" + txt_Tel1.Text + "', F_Tel2 ='" + txt_Tel2.Text + "', F_Adres ='" + txt_Adres.Text + "', F_AdresTarif ='" + txt_AdresTarif.Text + "' "
                    + " WHERE F_Id = '" + F_Id + "'");
                gridyenile();
            }
            else
            {
               MessageBox.Show(res_man.GetString("Düzeltilecek Kaydı Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void btn_Sil_Click(object sender, EventArgs e)
        {
            
            if (F_Id != 0)
            {
                if (MessageBox.Show(res_man.GetString("Seçili Kaydı Silmek İdtediğinize Emin Misiniz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                dbtools.execcmd("DELETE FROM    Pos_Fihrist  WHERE F_Id = '" + F_Id + "'");
                gridyenile();
            }
            else
            {
               MessageBox.Show(res_man.GetString("Silinecek Kaydı Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            if (F_Id > 0)
            {
                //Fishrist
                FisPr pr = new FisPr();
                pr.Fihrist_Adres(F_Id);
            }
        }



    }
}