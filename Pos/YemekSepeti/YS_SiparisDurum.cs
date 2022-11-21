using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pos.YemekSepeti
{
    public partial class YS_SiparisDurum : DevExpress.XtraEditors.XtraForm
    {
        public YS_SiparisDurum()
        {
            InitializeComponent();
        }

        public string YSOrderID = "";
        string YSMessageID = "";
        public string Fis = "";
        public string CariKod = "";
        string Kapatma = "";
        private void YS_SiparisDurum_Load(object sender, EventArgs e)
        {
            iws.AuthHeaderValue = new IntegrationWebService1.AuthHeader();
            iws.AuthHeaderValue.UserName = YS_AuthHeader.ah.UserName;
            iws.AuthHeaderValue.Password = YS_AuthHeader.ah.Password;

            OrderGetir(YSOrderID);
        }

        IntegrationWebService1.Integration iws = new IntegrationWebService1.Integration();
        private void OrderGetir(string OrderID)
        {
            DataTable dt = dbtools.SelectTable("select CustomerPhone,MessageID,OrderTotal,PaymentMethodId, (Address + ' ' + AddressDescription + ' ' + Organization + ' ' + Region + ' ' + City) as Address, CustomerName From YS_Order Where Order_Id = '" + OrderID + "'");
            if (dt.Rows.Count > 0)
            {
                lbl_AdSoyad.Text = Convert.ToString(dt.Rows[0]["CustomerName"]);
                lbl_OrderID.Text = OrderID;
                lbl_Tutar.Text = Convert.ToString(dt.Rows[0]["OrderTotal"]);
                lbl_Telefon.Text = Convert.ToString(dt.Rows[0]["CustomerPhone"]);
                YSMessageID = Convert.ToString(dt.Rows[0]["MessageID"]);
                mem_Adres.Text = Convert.ToString(dt.Rows[0]["Address"]);
                Kapatma = Convert.ToString(dt.Rows[0]["PaymentMethodId"]);
                lbl_OdemeTipi.Text = Convert.ToString(dbtools.DegerGetir("select Pkod_Ad from Pos_Kodlar where pkod_Sinif = 11 and Pkod_YS_OdemeID = '" + Kapatma + "'"));

            }
        }

        public string SiparisYolda(string OrderID, string MessageID)
        {
            iws.UpdateOrderAsync(Convert.ToDecimal(OrderID), IntegrationWebService1.OrderStates.OnDelivery, "");
            return "OK";
        }


        private string PaketciDegistir(string Fisno)
        {
            Garson_Sor pkt = new Garson_Sor();
            pkt.Tag = "PAKET";
            pkt.ShowDialog();
            return pkt.Garson_Kod;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {

            iws.UpdateOrderAsync(Convert.ToDecimal(YSOrderID), IntegrationWebService1.OrderStates.OnDelivery, "");

            dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_YSDurum = '" + IntegrationWebService1.OrderStates.OnDelivery + "',Rsat_Paketci = '" + PaketciDegistir(Fis) + "' Where Rsat_Fisno = '" + Fis + "'");
            this.Close();
        }



        private void simpleButton2_Click(object sender, EventArgs e)
        {
            iws.UpdateOrderAsync(Convert.ToDecimal(YSOrderID), IntegrationWebService1.OrderStates.Delivered, "");
            dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_YSDurum = '" + IntegrationWebService1.OrderStates.Delivered + "' Where Rsat_Fisno = '" + Fis + "'");
            OnlineOdemeKapat(Kapatma, Fis);


            HesapKapa2();
            //this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string iptalSebep = "";
            Klavye2 klavye = new Klavye2();
            klavye.Tag = "FISIPTAL";
            klavye.ShowDialog();
            iptalSebep = klavye.yazi == null ? "" : klavye.yazi;

            if (iptalSebep.Length < 1)
            {
                return;
            }

            dbtools.execcmd("Update YS_Order Set IptalReddetNedeni = '" + iptalSebep + "' , OrderStatus = '" + IntegrationWebService1.OrderStates.Cancelled + "' where Order_Id = '" + YSOrderID + "'");
            FisIptal(Fis, iptalSebep);
            iws.UpdateOrderAsync(Convert.ToDecimal(YSOrderID), IntegrationWebService1.OrderStates.Cancelled, iptalSebep);
            dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_YSDurum = '" + IntegrationWebService1.OrderStates.Cancelled + "' Where Rsat_Fisno = '" + Fis + "'");
            this.Close();
        }

        private void FisIptal(string Fisno, string Iptal)
        {
            Param.Param_Yukle();
            string iptalSebep = String.Empty;
            //string Fisno = xtraTabControl1.SelectedTabPage == tab_Genelrapor ? Convert.ToString(bandedGridView1.GetFocusedRowCellValue("Rsat_Fisno")) : Convert.ToString(gridView11.GetFocusedRowCellValue("Rsat_Fisno"));
            //DateTime Tarih = xtraTabControl1.SelectedTabPage == tab_Genelrapor ? Convert.ToDateTime(bandedGridView1.GetFocusedRowCellValue("Rsat_Tarih")) : Convert.ToDateTime(gridView11.GetFocusedRowCellValue("Rsat_Tarih"));

            //Klavye2 klavye = new Klavye2();
            //klavye.Tag = "FISIPTAL";
            //klavye.ShowDialog();

            iptalSebep = Iptal;

            if (iptalSebep.Length < 1)
            {
                return;
            }


            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Cek_Iptal";
            com.Parameters.AddWithValue("@Fis_No", Fisno);
            com.Parameters.AddWithValue("@Users", User.P_Kod);
            com.Parameters.AddWithValue("@Rsat_IptalNot", iptalSebep);
            com.Parameters.AddWithValue("@Onb_Sil", 0);
            com.ExecuteNonQuery();
            if (con.State == ConnectionState.Open) con.Close();

            Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Fis_Iptal, Log.Log_Islem.Sil, Fisno + " NL Fis Silindi", Fisno, String.Empty);


            FisPr fis = new FisPr();
            string sonuc = fis.IptalPrNFC(Convert.ToInt32(Fisno));

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void OnlineOdemeKapat(string YSKapatma, string Fis)
        {
            if (YSKapatma == "4")
            {
                //string KapatmaKodu = Convert.ToString(dbtools.DegerGetir("Select Pkod_Kod From Pos_Kodlar Where Pkod_Sinif = 11 and Pkod_YS_OdemeID = '" + YSKapatma + "'"));

                //decimal FisTutar = Convert.ToDecimal(dbtools.DegerGetir("Select SUM(Rsat_Tutar) From Cst_Recete_Satis Where Rsat_BA= 'B' and Rsat_Fisno = '" + Fis + "'"));

                if (Param.Tesis_Tipi == 1)
                {
                    //Fis_Islem.Odeme_Al(Convert.ToInt32(Fis), FisTutar, FisTutar, KapatmaKodu, "Y", "", 0, CariKod, 0, Param.Doviz_Kodu, false);
                    //Fis_Islem.Satis_Tip(Convert.ToInt32(Fis), KapatmaKodu, "");
                    string Masa_No = Convert.ToString(dbtools.DegerGetir("select Rsat_Masa From Cst_Recete_Satis Where Rsat_Fisno = '" + Fis + "' and Rsat_Ba = 'B' Group by Rsat_Masa"));
                    dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 16,@Masano = '" + Masa_No + "',@Dep_Kodu = '" + Departman.Dep_Kodu + "'");
                    dbtools.execcmd("update Cst_Recete_Satis set Rsat_Durum = 'K' where Rsat_Fisno = '" + Fis + "' ");
                }
            }
        }

        private void HesapKapa(string Fis, string KapatmaHesap)
        {
            string Masa_No = Convert.ToString(dbtools.DegerGetir("select Rsat_Masa From Cst_Recete_Satis Where Rsat_Fisno = '" + Fis + "' Group by Rsat_Masa"));
            Hesap hes = new Hesap();
            hes.Tag = Fis;
            hes.Masa_No = Masa_No;

            DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");

            if (dt == null || dt.Rows.Count < 1)
            {
                dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43");
            }

            hes.look_Kapatma.Properties.DataSource = dt;
            hes.look_Kapatma.Properties.DisplayMember = "Pkod_Ad";
            hes.look_Kapatma.Properties.ValueMember = "Pkod_Kod";

            hes.look_Kapatma.EditValue = KapatmaHesap=="" ? null : KapatmaHesap;
            hes.Split = 0;
            hes.Splitad = "";
            hes.ShowDialog();
            this.Close();

         
        }

        private void HesapKapa2()
        {
            if (Main.ayarlar.paketOtoKapat==false)
            {
                return;
            }
            string KapatmaHesap = Convert.ToString(dbtools.DegerGetir("Select Pkod_Kod From Pos_Kodlar Where Pkod_Sinif = 11 and Pkod_YS_OdemeID = '" + Kapatma + "'"));

            string Masa_No = Convert.ToString(dbtools.DegerGetir("select Rsat_Masa From Cst_Recete_Satis Where Rsat_Fisno = '" + Fis + "' Group by Rsat_Masa"));
            Hesap hes = new Hesap();
            hes.Tag = Fis;
            hes.Masa_No = Masa_No;


            DataTable dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43, @Dep_Kodu = '" + Departman.Dep_Kodu + "'");

            if (dt == null || dt.Rows.Count < 1)
            {
                dt = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 43");
            }


            hes.look_Kapatma.Properties.DataSource = dt;

            hes.look_Kapatma.Properties.DisplayMember = "Pkod_Ad";
            hes.look_Kapatma.Properties.ValueMember = "Pkod_Kod";

            hes.look_Kapatma.EditValue = KapatmaHesap == "" ? null : KapatmaHesap;
            hes.Split = 0;
            hes.Splitad = "";

            hes.Visible = false;
            hes.otoYazdirmadanKapat = true;
            hes.Show();

        }


        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string KapatmaKodu = Convert.ToString(dbtools.DegerGetir("Select Pkod_Kod From Pos_Kodlar Where Pkod_Sinif = 11 and Pkod_YS_OdemeID = '" + Kapatma + "'"));
            HesapKapa(Fis, KapatmaKodu);
        }
    }
}