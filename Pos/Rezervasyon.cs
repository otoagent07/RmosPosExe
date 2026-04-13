using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using Pos.Class;
using System;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Rezervasyon : DevExpress.XtraEditors.XtraForm
    {
        int Rez_Id = 0;
        public Rezervasyon()
        {
            InitializeComponent();
        }

        private void Rezervasyon_Load(object sender, EventArgs e)
        {

            dateEdit1.DateTime = Param.Tarih;
            dateTarih1.DateTime = Param.Tarih;
            dateTarih2.DateTime = Param.Tarih;

            look_Departman.Properties.DataSource = dbtools.SelectTable("select Kodlar_Kod,Kodlar_Ad from Stok_Kodlar WITH(NOLOCK) where Kodlar_Sinif = '01' and Kodlar_Satis = 1 order by Kodlar_Kod");
            look_Departman.Properties.DisplayMember = "Kodlar_Ad";
            look_Departman.Properties.ValueMember = "Kodlar_Kod";
        }

        #region Rezervasyon Kaydetme
        private void look_Departman_EditValueChanged(object sender, EventArgs e)
        {
            look_Masa.Properties.DataSource = dbtools.SelectTable("select Masa_No,Masa_Ad from Pos_Masa Where Masa_Depart = '" + Convert.ToString(look_Departman.EditValue) + "' and ISNULL(Masa_Paket,0) = 0 order by Masa_No");
            look_Masa.Properties.DisplayMember = "Masa_Ad";
            look_Masa.Properties.ValueMember = "Masa_No";
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            Rez_Id = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rez_Id"));
            dateEdit1.DateTime = Convert.ToDateTime(gridView1.GetFocusedRowCellValue("Rez_Tarih"));
            timeEdit1.EditValue = gridView1.GetFocusedRowCellValue("Rez_Saat");
            look_Departman.EditValue = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Dep"));
            txt_Adi.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Adi"));
            txt_Soyadi.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Soyadi"));
            spn_KisiSayisi.EditValue = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rez_Kisi"));
            spn_MasaSayisi.EditValue = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rez_MasaSayisi"));
            look_Masa.EditValue = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Masano"));
            txt_Telefon.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Tel"));
            txt_Email.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Email"));
            txt_Not.Text = Convert.ToString(gridView1.GetFocusedRowCellValue("Rez_Not"));


            int durum = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rez_Durum"));

            switch (durum)
            {
                case 0: radioButtonAktif.Checked = true; break;
                case 1: radioButtonGeldi.Checked = true; break;
                case 2: radioButtonGelmedi.Checked = true; break;
                case 3: radioButtonKaraListe.Checked = true; break;
                default:
                    radioButtonAktif.Checked = true;
                    break;
            }



        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            gridyenile();
        }

        private void btn_Kaydet_Click(object sender, EventArgs e)
        {
            Rez_Id = 0;
            dxValidationProvider1.Validate();

            if (dxValidationProvider1.GetInvalidControls().Count > 0)
            {
                return;
            }

            Rez r = new Rez();
            r.Rez_Id = Rez_Id;
            r.Rez_Tarih = dateEdit1.DateTime;
            r.Rez_Saat = new TimeSpan(timeEdit1.Time.Hour, timeEdit1.Time.Minute, timeEdit1.Time.Second);
            r.Rez_Dep = Convert.ToString(look_Departman.EditValue);
            r.Rez_Adi = txt_Adi.Text;
            r.Rez_Soyadi = txt_Soyadi.Text;
            r.Rez_Kisi = Convert.ToInt32(spn_KisiSayisi.EditValue);
            r.Rez_MasaSayisi = Convert.ToInt32(spn_MasaSayisi.EditValue);
            r.Rez_Masano = Convert.ToString(look_Masa.EditValue);
            r.Rez_Tel = txt_Telefon.Text;
            r.Rez_Email = txt_Email.Text;
            r.Rez_Not = txt_Not.Text;


            int durum = 0; // 0 aktif,1 geldi ,2 gelmedi,3 karaliste
            if (radioButtonAktif.Checked) durum = 0;
            else if (radioButtonGeldi.Checked) durum = 1;
            else if (radioButtonGelmedi.Checked) durum = 2;
            else if (radioButtonKaraListe.Checked) durum = 3;

            r.Rez_Durum = durum;

            r.Rez_Kaydet(r);
            gridyenile();

        }

        private void btn_Duzelt_Click(object sender, EventArgs e)
        {
            dxValidationProvider1.Validate();

            if (dxValidationProvider1.GetInvalidControls().Count > 0)
            {
                return;
            }
            if (Rez_Id != 0)
            {
                Rez r = new Rez();
                r.Rez_Id = Rez_Id;
                r.Rez_Tarih = dateEdit1.DateTime;
                r.Rez_Saat = new TimeSpan(timeEdit1.Time.Hour, timeEdit1.Time.Minute, timeEdit1.Time.Second);
                r.Rez_Dep = Convert.ToString(look_Departman.EditValue);
                r.Rez_Adi = txt_Adi.Text;
                r.Rez_Soyadi = txt_Soyadi.Text;
                r.Rez_Kisi = Convert.ToInt32(spn_KisiSayisi.EditValue);
                r.Rez_MasaSayisi = Convert.ToInt32(spn_MasaSayisi.EditValue);
                r.Rez_Masano = Convert.ToString(look_Masa.EditValue);
                r.Rez_Tel = txt_Telefon.Text;
                r.Rez_Email = txt_Email.Text;
                r.Rez_Not = txt_Not.Text;


                int durum = 0; // 0 aktif,1 geldi ,2 gelmedi,3 karaliste
                if (radioButtonAktif.Checked) durum = 0;
                else if (radioButtonGeldi.Checked) durum = 1;
                else if (radioButtonGelmedi.Checked) durum = 2;
                else if (radioButtonKaraListe.Checked) durum = 3;

                r.Rez_Durum = durum;


                r.Rez_Kaydet(r);
                gridyenile();
            }
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void btn_Sil_Click(object sender, EventArgs e)
        {
            if (Rez_Id != 0)
            {
                if (MessageBox.Show(res_man.GetString("Rezervasyon Silmek İstiyor Musunuz...?"), res_man.GetString("Uyarı"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Rez r = new Rez();

                    r.Rez_Sil(Rez_Id);

                    gridyenile();
                }
            }
        }

        private void gridyenile()
        {
            Rez r = new Rez();
            gridControl1.DataSource = r.Rez_Liste(0, dateEdit1.DateTime, dateEdit1.DateTime);

            Rez_Id = 0;
            timeEdit1.EditValue = "00:00:00";
            look_Departman.EditValue = null;
            txt_Adi.Text = String.Empty;
            txt_Soyadi.Text = String.Empty;
            spn_KisiSayisi.EditValue = 0;
            spn_MasaSayisi.EditValue = 0;
            look_Masa.EditValue = null;
            txt_Telefon.Text = String.Empty;
            txt_Email.Text = String.Empty;
            txt_Not.Text = String.Empty;
            radioButtonAktif.Checked = true;
        }
        #endregion

        #region Rezervasyon Raporu
        private void btn_Listele_Click(object sender, EventArgs e)
        {
            Rez r = new Rez();
            gridControl2.DataSource = r.Rez_Liste(0, dateTarih1.DateTime, dateTarih2.DateTime);
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            PrintingSystem printingSystem1 = new PrintingSystem();
            PrintableComponentLink printableComponentLink1 = new PrintableComponentLink();
            printingSystem1.Links.AddRange(new object[] { printableComponentLink1 });
            printableComponentLink1.Component = gridControl2;
            printableComponentLink1.Landscape = false;
            printableComponentLink1.Margins = new System.Drawing.Printing.Margins(50, 50, 50, 50);
            string leftColumn = "Rezervasyon Raporu";
            string rightColumn = dateTarih1.DateTime.ToLongDateString() + " - " + dateTarih2.DateTime.ToLongDateString();
            PageHeaderFooter phf = printableComponentLink1.PageHeaderFooter as PageHeaderFooter;
            phf.Header.Content.Clear();
            phf.Header.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            phf.Header.Content.AddRange(new string[] { leftColumn, rightColumn });
            phf.Header.LineAlignment = BrickAlignment.Far;
            printableComponentLink1.ShowPreview();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string fName = string.Empty;
            saveFileDialog1.Filter = "Excel Document (*.xls)|*.xls";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "Rezervasyon_Raporu.xls";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != null)
                {
                    XlsExportOptions opt = new XlsExportOptions();
                    opt.ShowGridLines = true;
                    gridControl2.ExportToXls(saveFileDialog1.FileName, opt);
                }
            }
        }

        #endregion

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.RowHandle >= 0 )
            {
                int durum = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "Rez_Durum")?.ToString());


                switch (durum)
                {
                   // case 0: e.Appearance.BackColor = Color.DeepPink; e.Appearance.ForeColor = Color.White; break;
                    case 1: e.Appearance.BackColor = Color.LimeGreen; e.Appearance.ForeColor = Color.White; break;
                    case 2: e.Appearance.BackColor = Color.Red; e.Appearance.ForeColor = Color.White; break;
                    case 3: e.Appearance.BackColor = Color.Gray; e.Appearance.ForeColor = Color.White; break;
                    default:
                        radioButtonAktif.Checked = true;
                        break;
                }



            }
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.RowHandle >= 0 )
            {
                int durum = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "Rez_Durum")?.ToString());


                switch (durum)
                {
                    // case 0: e.Appearance.BackColor = Color.DeepPink; e.Appearance.ForeColor = Color.White; break;
                    case 1: e.Appearance.BackColor = Color.LimeGreen; e.Appearance.ForeColor = Color.White; break;
                    case 2: e.Appearance.BackColor = Color.Red; e.Appearance.ForeColor = Color.White; break;
                    case 3: e.Appearance.BackColor = Color.Gray; e.Appearance.ForeColor = Color.White; break;
                    default:
                        radioButtonAktif.Checked = true;
                        break;
                }

            }
        }
    }
}