using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class Paket_Servis : DevExpress.XtraEditors.XtraForm
    {
        public Cari pCari { get; set; }
        public bool AcikAdres = false;
        //public string CariTel = String.Empty;

        public Paket_Servis()
        {
            InitializeComponent();
            this.BringToFront();
        }
        bool canClose = false;
        private void Paket_Servis_Load(object sender, EventArgs e)
        {
            gridyenile1();
        }

        private void gridyenile1()
        {

            gv_Cari.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;
            gv_Cari.FocusedColumn = gv_Cari.Columns.ColumnByFieldName("Cari_Kod");
            gv_Cari.ShowEditor();

            grd_Cari.DataSource = dbtools.SelectTable(@"select  Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Adres1,Cari_Adres2,Cari_Adres3,
    ISNULL(Cari_Adres1, '') + ' ' + ISNULL(Cari_Adres2, '') + ' ' + ISNULL(Cari_Adres3, '') as Adres, Cari_Kart,
    Cari_Funvan, Cari_Fadres1, Cari_Fadres2, Cari_Vergidarie, Cari_Vergino, Cari_Mail, Cari_Il, Cari_Ilce, Cari_Mahalle,
    0 as Bakiye

    from Pos_Cari WITH(NOLOCK)   order by Cari_Kod

        ");
        }


        private void gridyenile2()
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis";
            com.Parameters.AddWithValue("@Rapor_Tipi", 5);
            com.Parameters.AddWithValue("@Cari", Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Kod")));
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gridControl1.DataSource = dt;
        }
        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void btn_Cari_Kaydet_Click(object sender, EventArgs e)
        {
            if (txt_Cari_Kod.Text.Length == 0)
            {
                MessageBox.Show(res_man.GetString("Cari Kodu Boş Geçilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (txt_Cari_Ad.Text.Length > 0)
                {
                    DataTable dt = dbtools.SelectTable("select * from Pos_Cari where Cari_Kod = '" + txt_Cari_Kod.EditValue + "' ");
                    if (dt.Rows.Count < 1)
                    {
                        dbtools.execcmd("INSERT INTO dbo.Pos_Cari (Cari_Kod,Cari_Ad,Cari_Soyad,Cari_Tel,Cari_Adres1,Cari_Adres2,Cari_Adres3,Cari_Kart) VALUES ( '" + txt_Cari_Kod.EditValue + "','" + txt_Cari_Ad.EditValue + "','" + txt_Cari_Soyad.EditValue + "','" + txt_Cari_Telefon.EditValue + "', "
                            + " '" + txt_Cari_Adres1.EditValue + "','" + txt_Cari_Adres2.EditValue + "','" + txt_Cari_Adres3.EditValue + "', '" + txt_Cari_Kart_No.EditValue + "' )");
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Kaydet, txt_Cari_Kod.EditValue + " Kod ile Cari Kaydedildi.", String.Empty, String.Empty);
                    }
                    else
                    {
                        dbtools.execcmd("update dbo.Pos_Cari set Cari_Kod='" + txt_Cari_Kod.EditValue + "', Cari_Ad='" + txt_Cari_Ad.EditValue + "', Cari_Soyad='" + txt_Cari_Soyad.EditValue + "', Cari_Tel='" + txt_Cari_Telefon.EditValue + "', "
                        + " Cari_Adres1='" + txt_Cari_Adres1.EditValue + "', Cari_Adres2='" + txt_Cari_Adres2.EditValue + "', Cari_Adres3='" + txt_Cari_Adres3.EditValue + "', Cari_Kart = '" + txt_Cari_Kart_No.EditValue + "' where  Cari_Kod = '" + txt_Cari_Kod.EditValue + "'  ");
                        Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Prm_CariTanim, Log.Log_Islem.Duzelt, txt_Cari_Kod.EditValue + " Kod ile Cari Duzeltildi.", String.Empty, String.Empty);
                    }

                    gridyenile1();

                    txt_Cari_Ad.EditValue = null;
                    txt_Cari_Soyad.EditValue = null;
                    txt_Cari_Telefon.EditValue = null;
                    txt_Cari_Adres1.EditValue = null;
                    txt_Cari_Adres2.EditValue = null;
                    txt_Cari_Adres3.EditValue = null;
                    txt_Cari_Kart_No.EditValue = null;
                }
                else
                {
                    MessageBox.Show(res_man.GetString("Ad Boş Geçilemez..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txt_Cari_Kod_Leave(object sender, EventArgs e)
        {
            Cari c = Cari.Cari_Getir(Convert.ToString(txt_Cari_Kod.EditValue));
            if (c != null)
            {
                txt_Cari_Ad.EditValue = c.Cari_Ad;
                txt_Cari_Soyad.EditValue = c.Cari_Soyad;
                txt_Cari_Telefon.EditValue = c.Cari_Tel;
                txt_Cari_Adres1.EditValue = c.Cari_Adres1;
                txt_Cari_Adres2.EditValue = c.Cari_Adres2;
                txt_Cari_Adres3.EditValue = c.Cari_Adres3;
                txt_Cari_Kart_No.EditValue = c.Cari_Kart;
            }
        }

        private void btn_Satis_Click(object sender, EventArgs e)
        {

            int seciliSatir = gv_Cari.FocusedRowHandle;
            if (seciliSatir== -2147483646)
            {
                if (gv_Cari.RowCount > 0)
                {
                    pCari = Cari.Cari_Getir(Convert.ToString(gv_Cari.GetRowCellValue(0, "Cari_Kod")));

                    this.Close();
                }
            }
            else
            {
                if (gv_Cari.RowCount > 0)
                {
                    pCari = Cari.Cari_Getir(Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Kod")));
                    this.Close();
                }
            }
         
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            canClose = true;
            this.Close();
        }

        private void btn_AcikAdres_Click(object sender, EventArgs e)
        {
            AcikAdres = true;
            this.Close();
        }

        private void gv_Cari_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0)
            //{
            //    return;
            //}

            txt_Cari_Kod.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Kod"));
            txt_Cari_Ad.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Ad"));
            txt_Cari_Soyad.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Soyad"));
            txt_Cari_Telefon.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Tel"));
            txt_Cari_Adres1.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Adres"));
            txt_Cari_Kart_No.Text = Convert.ToString(gv_Cari.GetFocusedRowCellValue("Cari_Kart"));

            if (chk_Listele.Checked)
            {
                gridyenile2();
            }
        }
    }
}