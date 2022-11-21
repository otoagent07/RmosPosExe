using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using Pos.Class;
using System.Resources;
using System.Reflection;

namespace Pos
{
    public partial class Rapor_Dizayn : DevExpress.XtraEditors.XtraForm
    {
        public Rapor_Dizayn(string Form_Adi, ref DevExpress.XtraGrid.Views.Grid.GridView gridView1)
        {
            InitializeComponent();
            F_Adi = Form_Adi;
            grd = gridView1;
            gridView1.ActiveFilterString = null;
        }
        public Rapor_Dizayn(string Form_Adi, ref DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gridView1)
        {
            InitializeComponent();
            F_Adi = Form_Adi;
            bngrd = gridView1;
            gridView1.ActiveFilterString = null;
        }
        public Rapor_Dizayn(string Form_Adi, ref DevExpress.XtraPivotGrid.PivotGridControl gridView1)
        {
            InitializeComponent();
            F_Adi = Form_Adi;
            pivot = gridView1;
           
        }

        private void Rapor_Dizayn_Load(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        string F_Adi;
        DevExpress.XtraPivotGrid.PivotGridControl pivot;
        DevExpress.XtraGrid.Views.Grid.GridView grd;
        DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bngrd;

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textEdit1.Text.Length < 1)
            {
                MessageBox.Show(res_man.GetString("Rapor Adı Girmelisiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            DataTable dt = dbtools.SelectTable("SELECT Diz_Id,Diz_User,Diz_Form,Diz_Rapor,Diz_XML FROM User_Dizayn where Diz_Rapor = '" + textEdit1.EditValue + "' ");
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show(res_man.GetString("Bu isimde zaten bir rapor var..."), "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                if (textEdit1.Text.Length > 0)
                {

                    

                    MemoryStream GoruntuFile = new MemoryStream();
                    if (bngrd == null)
                    {
                        if (pivot == null)
                        {
                            grd.SaveLayoutToStream(GoruntuFile);
                        }
                        else
                        {
                            pivot.SaveLayoutToStream(GoruntuFile);
                        }
                    }
                    else
                    {
                        bngrd.SaveLayoutToStream(GoruntuFile);
                    }
                    BinaryReader br = new BinaryReader(GoruntuFile);
                    byte[] fileContent = new byte[GoruntuFile.Length];
                    fileContent = GoruntuFile.ToArray();

                
                    SqlConnection con = dbtools.conn;
                    if (con.State == ConnectionState.Closed) con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = ("INSERT INTO User_Dizayn(Diz_Form,Diz_Rapor,Diz_XML) VALUES (@Diz_Form,@Diz_Rapor,@Diz_XML)");
                    com.Parameters.AddWithValue("@Diz_Form",F_Adi);
                    com.Parameters.AddWithValue("@Diz_Rapor", textEdit1.EditValue);
                    com.Parameters.AddWithValue("@Diz_XML", fileContent);
                    com.ExecuteNonQuery();
                    con.Close(); 
                    br.Close();
                    GoruntuFile.Close();
                }
            }

            this.Close();
        }

        
    }
}
