using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using Pos.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos
{
    public partial class HesapKisiyeSatis : Form
    {
        int fisno = -1;
        public HesapKisiyeSatis(int fisno)
        {
            InitializeComponent();
            this.fisno = fisno;
        }

        private void HesapKisiyeSatis_Load(object sender, EventArgs e)
        {
            try
            {
                string sorgu = $"select isnull(kisiyeSatisAdSoyad,'')  as 'Ad Soyad',SUM(CASE WHEN Rsat_Ba = 'B' THEN Rsat_Fiyat ELSE 0 END) - SUM(CASE WHEN Rsat_Ba = 'A' THEN Rsat_Fiyat ELSE 0 END) AS fark from Cst_Recete_Satis where Rsat_Fisno='{fisno}' group by kisiyeSatisAdSoyad";


                DataTable dataTable = dbtools.SelectTableR(sorgu);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        dr["fark"] = dr["fark"].ToString().Replace(",0000", "");
                        dr["fark"] = dr["fark"].ToString().Replace(",000", "");
                        dr["fark"] = dr["fark"].ToString().Replace(",00", "");
                    }

                }

                string columname = "KİŞİYİ SEÇ";
                dataTable.Columns.Add(columname, typeof(string));

                string columname2 = "SATIŞLARI GÖSTER";
                dataTable.Columns.Add(columname2, typeof(string));

                string columname3 = "HESAP YAZDIR";
                dataTable.Columns.Add(columname3, typeof(string));

                gridControl1.DataSource = dataTable;
                gridView1.BestFitColumns();

                buttonEkle(columname);
                buttonEkle2(columname2);
                buttonEkle3(columname3);


                gridviewSumYaz(gridView1, "fark");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

     

        private void hesapyazdir_click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string adsoyad = gridView1.GetFocusedRowCellValue("Ad Soyad").ToString();
                FisPr pr = new FisPr();
                pr.kisiyeSatis = true;
                pr.kisiyeSatisAdSoyad = adsoyad;
                if (Param.Param_YeniHesapDkm)
                {
                    pr.newHesapDokum(true, fisno, 0, "* * * HESAP DÖKÜM FİŞİ * * *");
                }
                else
                {
                    pr.HesapDokum(true, Convert.ToInt32(this.Tag), 0);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        public void buttonEkle3(string columnName)
        {
            try
            {
                RepositoryItemButtonEdit repositoryButton = new RepositoryItemButtonEdit();
                repositoryButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                repositoryButton.Buttons[0].Caption = columnName;
                repositoryButton.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                repositoryButton.ButtonClick += hesapyazdir_click;
                gridView1.Columns[columnName].ColumnEdit = repositoryButton;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public void buttonEkle2(string columnName)
        {
            try
            {

                RepositoryItemButtonEdit repositoryButton = new RepositoryItemButtonEdit();
                repositoryButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                repositoryButton.Buttons[0].Caption = columnName;
                repositoryButton.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                repositoryButton.ButtonClick += satislarigoster_click;
                gridView1.Columns[columnName].ColumnEdit = repositoryButton;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public void gridviewSumYaz(GridView grid, string fieldname)
        {
            try
            {
                grid.OptionsView.ShowFooter = true;

                if (grid.Columns.Count > 0)
                {
                    grid.Columns[fieldname].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    grid.Columns[fieldname].SummaryItem.FieldName = fieldname;
                    grid.Columns[fieldname].SummaryItem.DisplayFormat = "{0:n2}";
                    grid.UpdateTotalSummary();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        private void satislarigoster_click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string adsoyad = gridView1.GetFocusedRowCellValue("Ad Soyad").ToString();

                string query = $@"select rec.Rec_Ad as 'Ürün Ad',Rsat_Miktar as Miktar,Rsat_Tutar as Toplam from Cst_Recete_Satis sat
left join Cst_Recete rec on rec.Rec_Genelkod=sat.Rsat_Recete
where Rsat_Fisno='{fisno}' and kisiyeSatisAdSoyad='{adsoyad}' and Rsat_Ba='B'  ";
                DataTable dataTable = dbtools.SelectTableR(query);


                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        dr["Miktar"] = dr["Miktar"].ToString().Replace(",0000", "");
                        dr["Miktar"] = dr["Miktar"].ToString().Replace(",000", "");
                        dr["Toplam"] = dr["Toplam"].ToString().Replace(",00", "");
                        dr["Toplam"] = dr["Toplam"].ToString().Replace(",000", "");
                        dr["Toplam"] = dr["Toplam"].ToString().Replace(",0000", "");
                    }

                }

                gridControlFis.DataSource = dataTable;

                gridViewFis.BestFitColumns();
                gridviewSumYaz(gridViewFis, "Toplam");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void buttonEkle(string columnName)
        {
            try
            {

                RepositoryItemButtonEdit repositoryButton = new RepositoryItemButtonEdit();
                repositoryButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                repositoryButton.Buttons[0].Caption = columnName;
                repositoryButton.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                repositoryButton.ButtonClick += RepositoryButton_ButtonClick;
                gridView1.Columns[columnName].ColumnEdit = repositoryButton;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public DataRow seciliRow = null;
        private void RepositoryButton_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            sec();
        }

        public void sec()
        {
            try
            {
                int[] selectedRows = gridView1.GetSelectedRows();

                if (selectedRows.Length != 1)
                {
                    return;
                }
                int selectedRowIndex = selectedRows[0];
                seciliRow = gridView1.GetDataRow(selectedRowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            sec();
        }
    }
}
