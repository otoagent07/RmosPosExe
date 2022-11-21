using System.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System;

namespace Pos.Print
{
    public partial class DynamicReport : DevExpress.XtraReports.UI.XtraReport
    {
        DevExpress.XtraGrid.Views.Grid.GridView dgv;
        int genislik;
        int baslikYukseklik;
        int detayYukseklik;
        private string p;
        private string p_2;
        private DevExpress.XtraGrid.Views.Base.BaseView baseView;

        public DynamicReport(string Baslik, string OtelAdi, DevExpress.XtraGrid.Views.Grid.GridView grid)
        {
            InitializeComponent();
            dgv = grid;
            xr_Baslik.Text = Baslik;
            txt_OtelAdi.Text = OtelAdi;
        }

        public DynamicReport(string p, string p_2, DevExpress.XtraGrid.Views.Base.BaseView baseView)
        {
            this.p = p;
            this.p_2 = p_2;
            this.baseView = baseView;
        }

        private void BasliklariOlustur()
        {
            Point pnt = new Point(0, 80);
            //genislik = 1130 / dgv.VisibleColumns.Count;
            genislik = 790 / dgv.VisibleColumns.Count;
            baslikYukseklik = 23;

            for (int column = 0; column < dgv.VisibleColumns.Count; column++)
            {
                XRLabel label = new XRLabel();

                label.BackColor = Color.Transparent;
                label.Font = new Font("Verdana", 10, FontStyle.Bold);
                label.ForeColor = Color.Black;
                label.TextAlignment = TextAlignment.MiddleCenter;
                label.Borders = BorderSide.Bottom;

                label.SizeF = new SizeF(genislik, baslikYukseklik);
                label.Text = dgv.VisibleColumns[column].Caption;
                label.LocationF = pnt;
                label.WordWrap = false;
                label.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);

                pnt.X += genislik;

                PageHeader.Controls.Add(label);
            }

        }

        private void satirlariOlustur()
        {
            Point pnt = new Point(0, 20);
            detayYukseklik = 20;

            for (int row = 0; row < dgv.DataRowCount; row++)
            {
                for (int column = 0; column < dgv.VisibleColumns.Count; column++)
                {
                    XRLabel label = new XRLabel();

                    label.Font = new Font("Verdana", 9, FontStyle.Bold);
                    label.Borders = BorderSide.None;
                    label.ForeColor = Color.Black;
                    label.SizeF = new SizeF(genislik, detayYukseklik);
                    label.TextAlignment = TextAlignment.MiddleCenter;


                    if (Convert.ToString(dgv.VisibleColumns[column].FieldName) != "")
                    {
                        label.Text = Convert.ToString(dgv.GetRowCellDisplayText(row, dgv.VisibleColumns[column].FieldName));
                    }

                    label.LocationF = pnt;
                    label.WordWrap = false;
                    label.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);

                    pnt.X += genislik;

                    Detail.Controls.Add(label);
                }
                pnt.X = 0;
                pnt.Y += detayYukseklik;
            }
        }    

        private void DynamicReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            BasliklariOlustur();
            satirlariOlustur();
            xr_Date.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        }
    }
}