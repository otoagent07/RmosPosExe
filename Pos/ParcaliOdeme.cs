using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using Pos.Class;
using Pos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos
{
    public partial class ParcaliOdeme : Form
    {
        public int Split = 0;
        public static string MyClass = "ParcaliOdeme";
        public string fisno = "0";
        public string anamasano = "1";
        public string altmasano = "1";
        public ParcaliOdeme(string fisno, string anamasano)
        {
            InitializeComponent();
            this.fisno = fisno;
            this.Tag = fisno;
            this.anamasano = anamasano;
        }

        DataTable dtAna = new DataTable();
        DataTable dtAnaMasalari = new DataTable();

        List<SimpleButton> buttons = new List<SimpleButton>();
        public void anaListele()
        {
            try
            {
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_Satis";
                com.Parameters.AddWithValue("@Fisno", fisno);
                com.Parameters.AddWithValue("@Rapor_Tipi", 2);
                com.Parameters.AddWithValue("@Split", Split);
                SqlDataAdapter da = new SqlDataAdapter(com);
                dtAna = new DataTable();
                da.Fill(dtAna);
                /*Rsat_Id Rsat_Fisno Rsat_Masa Rsat_Tarih Rsat_Acilis Rsat_Ba Kasiyer Rsat_Miktar Rsat_Emiktar Rec_Ad Rsat_Tutar Rsat_Doviz MasaKonumAdi Rsat_UrunTahsilat Rsat_Recete Rsat_UrunBazliHspAdet*/


                gridControlAna.DataSource = dtAna;


                groupControlAna.Text = "ANA Fişno: " + fisno + " - Masa No: " + anamasano;
                string query = "select Masa_Id,Masa_No,Masa_Ad,Masa_Durum,Masa_Parcali from Pos_Masa where Masa_No like '" + anamasano + "[_]%' and Masa_Durum='0'";
                dtAnaMasalari = dbtools.SelectTableR(query);


                if (dtAnaMasalari != null && dtAnaMasalari.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAnaMasalari.Rows.Count; i++)
                    {
                        DataRow row = dtAnaMasalari.Rows[i];
                        buttons[i].Tag = row["Masa_No"].ToString();
                        buttons[i].Visible = true;
                        buttons[i].Text = row["Masa_Ad"].ToString();
                    }
                }
                else
                {
                    RHMesaj.MyMessageInformation("Lütfen Masa Tanımdan parçalı masa oluşturunuz");
                }

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "anaListele", "", ex);
            }
        }

        public void ekrandakiButtonlariListele()
        {
            buttons.Add(btnAltMasa1);
            buttons.Add(btnAltMasa2);
            buttons.Add(btnAltMasa3);
            buttons.Add(btnAltMasa4);
            buttons.Add(btnAltMasa5);
            buttons.Add(btnAltMasa6);
            buttons.Add(btnAltMasa7);
            buttons.Add(btnAltMasa8);
            buttons.Add(btnAltMasa9);
            buttons.Add(btnAltMasa10);
        }
        private void ParcaliOdeme_Load(object sender, EventArgs e)
        {
            ekrandakiButtonlariListele();
            anaListele();
            lookKapatmaYukle();



        }

        private void gridViewAna_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Rsat_Miktar")
            {

                e.DisplayText = e.DisplayText.Replace(",0000", "");
                e.DisplayText = e.DisplayText.Replace(",000", "");
                e.DisplayText = e.DisplayText.Replace(",00", "");
            }
        }

        public void lookKapatmaYukle()
        {
            try
            {
                DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");

                if (dt == null || dt.Rows.Count < 1)
                {
                    dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43");
                }

                look_Kapatma.Properties.DataSource = dt;

                look_Kapatma.Properties.DisplayMember = "Pkod_Ad";
                look_Kapatma.Properties.ValueMember = "Pkod_Kod";
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "lookKapatmaYukle", "", ex);
            }

        }
        private void btnAltMasa1_Click(object sender, EventArgs e)
        {
            foreach (SimpleButton item in buttons)
            {
                item.Appearance.BackColor = Color.Transparent; // varsayılan rengi
                item.Appearance.Options.UseBackColor = true;
            }

            SimpleButton tiklanan = (sender as SimpleButton);
            tiklanan.Appearance.BackColor = Color.Lime;


            //groupControlAlt.Text = "Alt Fişno: " + fisno + " - Masa No: " + tiklanan.Text;
            groupControlAlt.Text = "Alt Masa No: " + tiklanan.Text;

            altmasano = tiklanan.Tag.ToString();

        }

        private void btnAnadanAlta_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dataRow = gridViewAna.GetDataRow(gridViewAna.FocusedRowHandle);

                RmosMerkez21Entities db = new RmosMerkez21Entities(dbtools.server, dbtools.database, dbtools.users, dbtools.pwd);

                int rsat_id = Convert.ToInt32(dataRow["Rsat_Id"].ToString());
                var nesne = db.Cst_Recete_Satis.Where(x => x.Rsat_Id == rsat_id).FirstOrDefault();
               


                var hedeffisno= Convert.ToInt32(dbtools.DegerGetir("exec Cost_Fis_No"));
                var AMiktar = Convert.ToString(gridViewAna.GetFocusedRowCellValue("Rsat_Emiktar"));
                if (nesne!=null)
                {
                    nesne.Rsat_Id = 0;
                    nesne.Rsat_Masa = "";
                    nesne.Rsat_Fisno = 0;
                    nesne.Rsat_Miktar = 0;
                    nesne.Rsat_Tutar = 0;
                    nesne.Rsat_Net = 0;
                    nesne.Rsat_Kdv = 0;
                    nesne.Rsat_Maliyet = 0;
                    nesne.Rsat_Doviztutar = 0;
                    nesne.Rsat_Ind = 0;
                    db.Cst_Recete_Satis.Add(nesne);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                RHMesaj.MyMessageError(MyClass, "btnAnadanAlta_Click", "",ex);
            }

        }

        private void btnAlttanAnaya_Click(object sender, EventArgs e)
        {

        }
    }
}
