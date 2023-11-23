using DevExpress.XtraEditors;
using Pos.Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Media;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;


namespace Pos.YemekSepeti
{
    public partial class YS_SiparisGeldi : DevExpress.XtraEditors.XtraForm
    {
        public YS_SiparisGeldi()
        {
            InitializeComponent();
        }

        IntegrationWebService1.Integration iws = new IntegrationWebService1.Integration(); 
        private bool IsRestaurantOpen()
        {
            try
            {
                bool s = iws.IsRestaurantOpen(YS_RestoInfo.YS_CatalogName, YS_RestoInfo.YS_CatagoryName);

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private void Temizle(Control Ctrl)
        {
            foreach (Control item in Ctrl.Controls)
            {
                if (item is LabelControl)
                    if (((LabelControl)item).Enabled)
                        ((LabelControl)item).Text = string.Empty;

                if (item is MemoEdit)
                    if (((MemoEdit)item).Enabled)
                        ((MemoEdit)item).Text = string.Empty;

                if (item is ListBoxControl)
                    if (((ListBoxControl)item).Enabled)
                        ((ListBoxControl)item).Items.Clear();

                if (item.Controls.Count > 0)
                    Temizle(item);
            }
        }
        private void YS_SiparisGeldi_Load(object sender, EventArgs e)
        {
            lbl_AdSoyad.Text = string.Empty;
            lbl_OrderID.Text = string.Empty;
            lbl_OdemeTipi.Text = string.Empty;
            lbl_Tutar.Text = string.Empty;

            iws.AuthHeaderValue = new IntegrationWebService1.AuthHeader();
            iws.AuthHeaderValue.UserName = YS_AuthHeader.ah.UserName;
            iws.AuthHeaderValue.Password = YS_AuthHeader.ah.Password;

            if (IsRestaurantOpen())
            {
                iws.UpdateRestaurantStateAsync(YS_RestoInfo.YS_CatalogName, YS_RestoInfo.YS_CatagoryName, IntegrationWebService1.RestaurantStates.Open);
            }

            timer1.Enabled = true;
        }
        private void Beep()
        {
            SoundPlayer player = new SoundPlayer();

            //string path = Properties.Resources.tada; // Çalmasini istediginiz ses dosyasinin yolu

            player.Stream = Properties.Resources.Yemeksepeti;

            player.Play();
        }

        DataSet dsOrder = new DataSet();

        decimal Indirim = 0;
        private void SiparisKontrolEt()
        {
            if (iws.GetMessageV2() != null)
            {
                Beep();
                timer1.Enabled = false;
                this.Visible = true;
                notifyIcon1.Visible = false;
                dsOrder = XMLtoDataSet.ConvertXMLToDataSet(iws.GetMessageV2());

                
                if (dsOrder != null)
                {
                    foreach (DataRow order in dsOrder.Tables["Order"].Rows)
                    {
                        lbl_OrderID.Text = Convert.ToString(order["Id"]);
                        lbl_AdSoyad.Text = Convert.ToString(order["CustomerName"]).Replace("Joker", "").Replace("-", "").Trim();
                        lbl_OdemeTipi.Text = Convert.ToString(dbtools.DegerGetir("select Pkod_Ad from Pos_Kodlar where pkod_Sinif = 11 and Pkod_YS_OdemeID = '" + Convert.ToString(order["PaymentMethodId"]) + "'"));
                        mem_Adres.Text = Convert.ToString(order["Address"]) + "\n" + Convert.ToString(order["AddressDescription"]) + " \n" + Convert.ToString(order["Region"]) + " - " + Convert.ToString(order["City"]);
                        mem_Not.Text = Convert.ToString(order["OrderNote"]);
                        lbl_Tutar.Text = Convert.ToString(order["OrderTotal"]);
                        Indirim = Math.Abs(Convert.ToDecimal(order["ChangeInTotal"].ToString().Replace(".", ",")));
                    }
                    OrderKaydet(dsOrder);
                    ProductKaydet(dsOrder);

                    for (int i = 0; i < dsOrder.Tables.Count; i++)
                    {
                        if (dsOrder.Tables[i].TableName == "option")
                        {
                            OptionKaydet(dsOrder);
                        }
                        if (dsOrder.Tables[i].TableName == "promotion")
                        {
                            PromotionKaydet(dsOrder);
                        }
                    }

                        CariOlustur(dsOrder);
                }
            }
        }
        int Fisno = 0;
        private void OrderKaydet(DataSet Order)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                foreach (DataRow order in Order.Tables["Order"].Rows)
                {
                    SqlCommand cmd = new SqlCommand("YS_Order_Kaydet", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Order_Id", order["Id"]);
                    cmd.Parameters.AddWithValue("@MessageId", order["MessageId"]);
                    cmd.Parameters.AddWithValue("@RestaurantName", order["RestaurantName"]);
                    cmd.Parameters.AddWithValue("@RestaurantCatalog", order["RestaurantCatalog"]);
                    cmd.Parameters.AddWithValue("@RestaurantCategory", order["RestaurantCategory"]);
                    cmd.Parameters.AddWithValue("@CustomerName", order["CustomerName"].ToString().Replace("Joker", "").Replace("-", "").Trim());
                    cmd.Parameters.AddWithValue("@CustomerId", order["CustomerId"]);
                    cmd.Parameters.AddWithValue("@CustomerType", order["CustomerType"]);
                    cmd.Parameters.AddWithValue("@PaymentNote", order["PaymentNote"]);
                    cmd.Parameters.AddWithValue("@OrderTotal", order["OrderTotal"]);
                    cmd.Parameters.AddWithValue("@CustomerPhone", order["CustomerPhone"]);
                    cmd.Parameters.AddWithValue("@CustomerPhone2", order["CustomerPhone2"]);
                    cmd.Parameters.AddWithValue("@PromoCode", order["PromoCode"]);
                    cmd.Parameters.AddWithValue("@City", order["City"]);
                    cmd.Parameters.AddWithValue("@Region", order["Region"]);
                    cmd.Parameters.AddWithValue("@Organization", order["Organization"]);
                    cmd.Parameters.AddWithValue("@Address", order["Address"]);
                    cmd.Parameters.AddWithValue("@AddressDescription", order["AddressDescription"]);
                    cmd.Parameters.AddWithValue("@AddressId", order["AddressId"]);
                    cmd.Parameters.AddWithValue("@PaymentMethodId", order["PaymentMethodId"]);
                    cmd.Parameters.AddWithValue("@DeliveryTime", order["DeliveryTime"]);
                    cmd.Parameters.AddWithValue("@ChangeInTotal", order["ChangeInTotal"]);
                    cmd.Parameters.AddWithValue("@Currency", order["Currency"]);
                    cmd.Parameters.AddWithValue("@OrderNote", order["OrderNote"]);
                    cmd.Parameters.AddWithValue("@Pos_Kullanici", User.P_Kod);

                    cmd.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
                return;
            }
        }
        private void ProductKaydet(DataSet Product)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                foreach (DataRow product in Product.Tables["product"].Rows)
                {
                    SqlCommand cmd = new SqlCommand("YS_Product_Kaydet", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Product_Id", product["id"]);
                    cmd.Parameters.AddWithValue("@Order_Id", lbl_OrderID.Text);
                    cmd.Parameters.AddWithValue("@Name", product["Name"]);
                    cmd.Parameters.AddWithValue("@Price", product["Price"]);
                    cmd.Parameters.AddWithValue("@ListPrice", product["ListPrice"]);
                    cmd.Parameters.AddWithValue("@Quantity", product["Quantity"]);
                    cmd.Parameters.AddWithValue("@Options", product["Options"]);
                    cmd.Parameters.AddWithValue("@OptionIds", product["OptionIds"]);
                    cmd.Parameters.AddWithValue("@OrderIndex", product["OrderIndex"]);
                    cmd.Parameters.AddWithValue("@ParentIndex", product["ParentIndex"]);
                    cmd.Parameters.AddWithValue("@PromoParentIndex", product["PromoParentIndex"]);
                    cmd.Parameters.AddWithValue("@ProductOptionId", product["ProductOptionId"]);
                    cmd.Parameters.AddWithValue("@Pos_Kullanici", User.P_Kod);

                    cmd.ExecuteNonQuery();

                    list_Siparis.Items.Add("Miktar : " + product["Quantity"] + "   Ürün Adı : " + product["Name"] + "   Fiyat : " + product["ListPrice"] + (product["Options"] == "" ? "" : ("   Porsiyon : " + product["Options"])));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
                return;
            }
        }
        private void OptionKaydet(DataSet Option)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                foreach (DataRow option in Option.Tables["option"].Rows)
                {
                    SqlCommand cmd = new SqlCommand("YS_Option_Kaydet", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Order_Id", lbl_OrderID.Text);
                    cmd.Parameters.AddWithValue("@Id", option["Id"]);
                    cmd.Parameters.AddWithValue("@Name", option["Name"]);
                    cmd.Parameters.AddWithValue("@ProductOrderId", option["ProductOrderId"]);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
                return;
            }
        }
        private void PromotionKaydet(DataSet Promotion)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                foreach (DataRow promotion in Promotion.Tables["promotion"].Rows)
                {
                    SqlCommand cmd = new SqlCommand("YS_Promotion_Kaydet", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Order_Id", lbl_OrderID.Text);
                    cmd.Parameters.AddWithValue("@Id", promotion["Id"]);
                    cmd.Parameters.AddWithValue("@DefinitionName", promotion["DefinitionName"]);
                    cmd.Parameters.AddWithValue("@ProductOrderId", promotion["ProductOrderId"]);

                    cmd.ExecuteNonQuery();

                    //list_Promosyon.Items.Add("Bilgi : " + promotion["DefinitionName"]);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
                return;
            }

        }
        private void CariOlustur(DataSet Cari)
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                foreach (DataRow cari in Cari.Tables["Order"].Rows)
                {
                    SqlCommand cmd = new SqlCommand("YS_Cari_Kaydet", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    string[] words = cari["CustomerName"].ToString().Replace("Joker","").Replace("-","").Trim().Split(' ');

                    cmd.Parameters.AddWithValue("@Cari_Ad", words[0]);
                    cmd.Parameters.AddWithValue("@Cari_Soyad", words[1]);
                    cmd.Parameters.AddWithValue("@Cari_Tel", cari["CustomerPhone"]);
                    cmd.Parameters.AddWithValue("@Cari_Adres1", cari["Address"] + "\nAdres Açıklama : " + cari["AddressDescription"] + "\n" + Convert.ToString(cari["Region"]) + " - " + Convert.ToString(cari["City"]));
                    cmd.Parameters.AddWithValue("@Cari_Tel2", cari["CustomerPhone2"]);
                    cmd.Parameters.AddWithValue("@Cari_Tip", "Y");
                    cmd.Parameters.AddWithValue("@Cari_Limit", 0);
                    cmd.Parameters.AddWithValue("@Cari_LimitTutar", 0);
                    cmd.Parameters.AddWithValue("@Cari_Il", cari["City"]);
                    cmd.Parameters.AddWithValue("@Cari_Ilce", cari["Region"]);
                    cmd.Parameters.AddWithValue("@Cari_Mahalle", cari["AddressDescription"]);
                    cmd.Parameters.AddWithValue("@Cari_Aktif", 1);
                    cmd.Parameters.AddWithValue("@Cari_YS_AddressId", cari["AddressId"]);
                    cmd.Parameters.AddWithValue("@Cari_YS_CustomerID", cari["CustomerId"]);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        CariKod = Convert.ToString(dt.Rows[0][0]);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
                return;
            }
        }
        private void SiparisKabulEt()
        {
            foreach (DataRow order in dsOrder.Tables["order"].Rows)
            {
                iws.MessageSuccessfulV2(Convert.ToString(order["MessageId"]));
            }
            dbtools.execcmd("Update YS_Order Set OrderStatus = '" + IntegrationWebService1.OrderStates.Accepted + "' where Order_Id = '" + lbl_OrderID.Text + "'");
            iws.UpdateOrderAsync(Convert.ToDecimal(lbl_OrderID.Text), IntegrationWebService1.OrderStates.Accepted, "");

            //return "OK";
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FisnoAL();
            Urun_Sat(dsOrder, Fisno, Indirim);
        }
        private void Siparis_Gonder(bool Mars, int Fis)
        {
            FisPr pr = new FisPr();
           
            string sonucSiparis = pr.YS_SiparisPr(Fis, Mars, 0);
            
            //FisPr pr = new FisPr();
            string sonucPaket = pr.PaketPr(Fisno, " * * * YEMEKSEPETİ PAKET FİSİ * * * ");

            if ( sonucPaket == "OK") //sonucSiparis == "OK" ||
            {     
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr = 1 where Rsat_Fisno = '" + Fis + "' ");
            }

            //this.Close();
        }
        private int FisnoAL()
        {
            Fisno = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
            StatikSinif.siranoarttir();
            return Fisno;
        }
        public void Urun_Sat(DataSet Product, int Fis, decimal Joker)
        {
            Recete_Islem rec = new Recete_Islem();
            string Rsat_Odano = String.Empty, Rsat_Adisyon,
                 Rsat_Cari = String.Empty, Rsat_Paketci = String.Empty, Rsat_Indkodu = String.Empty, Rsat_Garson2, Rsat_Uye_Kart_Turu = String.Empty, Rsat_Pansiyon = String.Empty, Rsat_MusTipi = String.Empty, Rsat_Uye_Ad = String.Empty, Rsat_Onbdep = String.Empty, Rsat_KartNo = String.Empty;
            int Rsat_Folio = 0, Rsat_Kisi, Rsat_Uye_Id = 0;
            string Not = "";
            decimal Rsat_Indoran = 0;

            DataTable dtMasa = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1 and Masa_Durum = 0 order by Masa_No");

            if (dtMasa.Rows.Count < 1)
            {
                MessageBox.Show(res_man.GetString("Boş Paket Masanız Bulunmamaktadır."));
                return;
            }

            foreach (DataRow order in Product.Tables["order"].Rows)
            {
                DataTable dt = dbtools.SelectTable("Select * From Pos_Cari Where Cari_YS_CustomerID = '" + order["CustomerId"] + "' and Cari_YS_AddressID = '" + order["AddressId"] + "'");
                if (dt.Rows.Count > 0)
                {
                    Rsat_Cari = Convert.ToString(dt.Rows[0]["Cari_Kod"]);
                    Rsat_MusTipi = Convert.ToString(dt.Rows[0]["Cari_Tip"]);
                    Rsat_Uye_Ad = Convert.ToString(dt.Rows[0]["Cari_Ad"]) + " " + Convert.ToString(dt.Rows[0]["Cari_Soyad"]);
                    Not = order["PaymentNote"].ToString();
                }
            }
            string Masa_No = Convert.ToString(dtMasa.Rows[0]["Masa_No"]);


            foreach (DataRow product in Product.Tables["product"].Rows)
            {
                //for (int i = 0; i < r.order.product.Count; i++)
                //{

                DataTable dtRecete = dbtools.SelectTable("exec Pos_Sorgu @Sorgu_Tipi = 39, @Dep_Kodu = '" + Departman.Dep_Kodu + "',@YS_UrunID = '" + product["id"] + "'");

                if (dtRecete.Rows.Count == 0)
                {
                    MessageBox.Show("SİPARİŞ ALIMI DURDURULACAKTIR...\nİŞLEME DEVAM EDEBİLMEK İÇİN ÜRÜN TANIMLAMA EKRANI AÇILACAKTIR...\nLÜTFEN ÜRÜN EŞLEŞTİRMESİ YAPINIZ..\nÜRÜN İŞLEMİ TAMAMLANDIKTAN SONRA SİPARİŞLERİNİZ ONAYLAYABİLİRSİNİZ..", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    //if (MessageBox.Show("RESTORAN DURUMU YOĞUN DURUMUNA ALINSIN MI?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                    //    string msg = iws.UpdateRestaurantState(YS_RestoInfo.YS_CatalogName, YS_RestoInfo.YS_CatagoryName, IntegrationWebService1.RestaurantStates.HugeDemand);
                    //    dbtools.execcmd("Update YS_Restaurant set YS_OpenClosed = 'YOĞUN' where YS_CatagoryName = '" + YS_RestoInfo.YS_CatagoryName + "'");
                    //}


                    YS_Panel m = new YS_Panel();
                    m.xtraTabControl1.SelectedTabPage = m.xtraTabPage2;
                    m.ShowDialog();

                    //FisIptal(Fis.ToString(), "YemekSepeti Ürünü Eşleştirilmemiş..");
                    ////string Durum = iws.UpdateOrder(Convert.ToDecimal(lbl_OrderID.Text), IntegrationWebService1.OrderStates.Cancelled, "Yoğunluk Sebebi ile Özür Dileriz.");
                    //timer1.Enabled = false;
                    //this.Visible = false;
                    //notifyIcon1.Visible = true;
                    //return;
                    return;
                }

                string Rec_Ad = Convert.ToString(dtRecete.Rows[0]["Rec_Ad"]);
                decimal Rec_Fiyat = 0;
                decimal Rec_Dovifiyat = 0;
                string Rsat_Emiktar = "";

                if (Convert.ToString(product["Options"]) == "")
                {
                    Rsat_Emiktar = "T";
                }
                else if (Convert.ToString(product["Options"]) == "1.5 Porsiyon")
                {
                    Rsat_Emiktar = "B";
                }
                else if (Convert.ToString(product["Options"]) == "2 Porsiyon")
                {
                    Rsat_Emiktar = "D";
                }


                //if (!Param.Param_Paket_YD)
                //{
                Rec_Fiyat = Convert.ToDecimal(product["ListPrice"].ToString().Replace(".", ","));
                Rec_Dovifiyat = Convert.ToDecimal(product["ListPrice"].ToString().Replace(".", ","));
                //}
                //else
                //{
                //    Rec_Fiyat = Convert.ToDecimal(dtRecete.Rows[0]["Rec_Fiyat"]);
                //    Rec_Dovifiyat = Convert.ToDecimal(dtRecete.Rows[0]["Rec_Fiyat"]);
                //}

                decimal Rec_Kdv = rec.Kdv_Bul(Convert.ToString(dtRecete.Rows[0]["Rec_Genelkod"]));
                string Rec_Dovizkodu = Convert.ToString(dtRecete.Rows[0]["Rec_Dovizkodu"]);
                Rsat_Onbdep = Convert.ToString(dtRecete.Rows[0]["Pkod_OnburoKod"]);

                decimal Rsat_Kdv, Rsat_Net, Rsat_Tutar;


                if (Param.Calisma_Sekli == 1) //Dövizli Çalışma Şekli
                {
                    Rsat_Tutar = Rec_Dovifiyat * Param.Doviz_Kuru;
                    Rsat_Net = ((Rsat_Tutar * 100) / (100 + Rec_Kdv));
                    Rsat_Kdv = (Rsat_Tutar - Rsat_Net);
                }
                else        // TL Çalışma
                {
                    Rsat_Tutar = Rec_Fiyat;
                    Rsat_Net = ((Rsat_Tutar * 100) / (100 + Rec_Kdv));
                    Rsat_Kdv = Rsat_Tutar - Rsat_Net;
                }

                Rsat_Tutar = Rsat_Tutar;// * Miktar;
                Rsat_Net = Rsat_Net;// * Miktar;
                Rsat_Kdv = Rsat_Kdv;// * Miktar;
                Rec_Dovifiyat = Rec_Dovifiyat;// * Miktar;

                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.CommandText = "Pos_YS_Ekle";

                com.Parameters.AddWithValue("@Rsat_Fisno", Convert.ToInt32(Fis));
                com.Parameters.AddWithValue("@Rsat_Tarih", Param.Tarih);
                com.Parameters.AddWithValue("@Rsat_Departman", Departman.Dep_Kodu);
                com.Parameters.AddWithValue("@Rsat_Recete", Convert.ToString(dtRecete.Rows[0]["Rec_Genelkod"]));
                com.Parameters.AddWithValue("@Rsat_Kdvoran", Rec_Kdv.ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Miktar", (product["Quantity"]).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Fiyat", (Rsat_Tutar).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Net", (Rsat_Net).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Kdv", (Rsat_Kdv).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Tutar", (Rsat_Tutar).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Dovizkodu", Param.Doviz_Kodu);
                com.Parameters.AddWithValue("@Rsat_Doviztutar", (Rec_Dovifiyat).ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Odano", Rsat_Odano);
                com.Parameters.AddWithValue("@Rsat_Folio", Rsat_Folio);
                com.Parameters.AddWithValue("@Rsat_Adisyon", DBNull.Value);
                com.Parameters.AddWithValue("@Rsat_Masa", Masa_No);
                com.Parameters.AddWithValue("@Rsat_Garson", User.P_Kod);
                com.Parameters.AddWithValue("@Rsat_Kisi", 1);
                com.Parameters.AddWithValue("@Rsat_Cari", Rsat_Cari);
                com.Parameters.AddWithValue("@Rsat_Split", 0);
                com.Parameters.AddWithValue("@Rsat_Aciklama", product["Options"]);
                com.Parameters.AddWithValue("@Rsat_Paketci", Rsat_Paketci);
                com.Parameters.AddWithValue("@Rsat_Emiktar", Rsat_Emiktar);
                com.Parameters.AddWithValue("@Rsat_Garson2", User.P_Kod);
                com.Parameters.AddWithValue("@Rsat_Uye_Kart_Turu", Rsat_Uye_Kart_Turu);
                com.Parameters.AddWithValue("@Rsat_Pansiyon", Rsat_Pansiyon);
                com.Parameters.AddWithValue("@Rsat_MusTipi", Rsat_MusTipi);
                com.Parameters.AddWithValue("@Rsat_Uye_Id", Rsat_Uye_Id);
                com.Parameters.AddWithValue("@Rsat_Uye_Ad", Rsat_Uye_Ad);
                com.Parameters.AddWithValue("@Rsat_Indkodu", Rsat_Indkodu);
                com.Parameters.AddWithValue("@Rsat_Indoran", Rsat_Indoran.ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Onbdep", Rsat_Onbdep);
                com.Parameters.AddWithValue("@Rsat_Dovizkur", Param.Doviz_Kuru.ToString().Replace(",", "."));
                com.Parameters.AddWithValue("@Rsat_Not",    Not + " " + "(Ödeme : " + lbl_OdemeTipi.Text + ") " + mem_Not.Text);
                com.Parameters.AddWithValue("@Rsat_Pda", Convert.ToBoolean(false));
                com.Parameters.AddWithValue("@Rsat_Splitad", "");
                com.Parameters.AddWithValue("@Rsat_SiparisPr", 0);  
                com.Parameters.AddWithValue("@Rsat_Yapma", 0);
                com.Parameters.AddWithValue("@Rsat_AbuyerPr", 0);
                com.Parameters.AddWithValue("@Rsat_AbuyerPr2", 0);
                com.Parameters.AddWithValue("@Rsat_AbuyerPr3", 0);
                com.Parameters.AddWithValue("@Rsat_AbuyerPr4", 0);
                com.Parameters.AddWithValue("@Rsat_Sube", "");
                com.Parameters.AddWithValue("@Rsat_OzelMasaAdi", Masa_No);
                com.Parameters.AddWithValue("@PaketFiyatTipi", "S");
                com.Parameters.AddWithValue("@Rsat_Duzeltme", 0);

                if (Departman.Kodlar_AndPos_NFC == true) com.Parameters.AddWithValue("@Rsat_Kart_ID", 0);
                if (Departman.Kodlar_AndPos_NFC == true) com.Parameters.AddWithValue("@Rsat_Kartno", "");
                if (Departman.Kodlar_Ingenico_IWE == true) com.Parameters.AddWithValue("@Rsat_Ingenico_Status", 1);

                com.ExecuteNonQuery();
                con.Close();


                //if (Ozel_Masa != String.Empty) dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 11, @Dep_Kodu = '" + Departman.Dep_Kodu + "', @Masano = '" + Masa_No + "', @Ozel_Masa = '" + Ozel_Masa + "'");

                //Log.Log_Kaydet(Log.Log_Program.Pos, Log.Log_Bolum.Satis, Log.Log_Islem.Kaydet, Departman.Dep_Adi + " Urun:" + Urun_Kodu + "-" + Rec_Ad + " Miktar:" + Miktar.ToString() + " Tutar:" + Rsat_Tutar.ToString("N2"), Convert.ToInt32(bartxt_FisNo.EditValue).ToString(), "");

                //Miktar_Duzenle();
                //chk_Yapma.Checked = false;
                //gridyenile();
                //txt_EkNot.Text = "";
                //Aciklama = String.Empty;

                //txt_Barkod.Focus();
                //MiktarDuzeltme = 0;

                //Miktar = 1;

                //if (Convert.ToString(this.Tag) ==  "H") txt_Barkod.Focus();

            }


            if (Joker > 0)
            {
                Fis_Islem.Manuel_Indirim(Convert.ToInt32(Fis), "T", Joker, Joker, 0, 0);
            }



            Siparis_Gonder(false, Fisno);
            OnlineOdemeKapat(dsOrder, Fisno, Joker);
            SiparisKabulEt();
            dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_YSDurum = '" + IntegrationWebService1.OrderStates.Accepted + "', Rsat_YSOrderID = '" + lbl_OrderID.Text + "'  Where Rsat_Fisno = '" + Fis + "'");
            timer1.Enabled = true;
            this.Visible = false;
            notifyIcon1.Visible = true;
            Temizle(this);
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
        private string SiparisReddet()
        {
            string iptalSebep = "";
            Klavye2 klavye = new Klavye2();
            klavye.Tag = "FISIPTAL";
            klavye.ShowDialog();
            iptalSebep = klavye.yazi == null ? "" : klavye.yazi;

            if (iptalSebep.Length < 1)
            {
                return "";
            }

            string Durum = "";
            foreach (DataRow order in dsOrder.Tables["order"].Rows)
            {
                iws.MessageSuccessfulV2(Convert.ToString(order["MessageId"]));
                dbtools.execcmd("Update YS_Order Set IptalReddetNedeni = '" + iptalSebep + "' where Order_Id = '" + Convert.ToDecimal(lbl_OrderID.Text) + "'");
                dbtools.execcmd("Update YS_Order Set OrderStatus = '" + IntegrationWebService1.OrderStates.Rejected + "' where Order_Id = '" + lbl_OrderID.Text + "'");
            }
            Durum = iws.UpdateOrder(Convert.ToDecimal(lbl_OrderID.Text), IntegrationWebService1.OrderStates.Rejected, iptalSebep);
            return Durum;
        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmLogin a = new frmLogin();
            a.Durum = "YS";
            a.Button_Cikis.Enabled = false;
            a.ShowDialog();

            if (a.DonenDeger == false)
            {
                MessageBox.Show("Yetkili Parolasını Yanlış Girildi...\nÜrün Reddedilemedi.", res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (SiparisReddet() == "OK")
            {
                Temizle(this);
                timer1.Enabled = true;
                this.Visible = false;
                notifyIcon1.Visible = true;
            }
        }
        string CariKod = "";
        private void OnlineOdemeKapat(DataSet Order, int Fis, decimal Indirim)
        {
            string OnlineOdemeID = "";

            foreach (DataRow order in Order.Tables["order"].Rows)
            {
                OnlineOdemeID = Convert.ToString(order["PaymentMethodId"]);
            }
            if (OnlineOdemeID == "4")
            {
                string KapatmaKodu = Convert.ToString(dbtools.DegerGetir("Select Pkod_Kod From Pos_Kodlar Where Pkod_Sinif = 11 and Pkod_YS_OdemeID = '" + OnlineOdemeID + "'"));

                decimal FisTutar = Convert.ToDecimal(dbtools.DegerGetir("Select SUM(Rsat_Tutar) From Cst_Recete_Satis Where Rsat_BA= 'B' and Rsat_Fisno = '" + Fis + "'"));

                if (Param.Tesis_Tipi == 1)
                {
                    Fis_Islem.Odeme_Al(Convert.ToInt32(Fis), (FisTutar - Indirim), (FisTutar - Indirim), KapatmaKodu, "Y", "", 0, CariKod, 0, Param.Doviz_Kodu, false);
                    Fis_Islem.Satis_Tip(Convert.ToInt32(Fis), KapatmaKodu, "");
                }
            }
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Temizle(this);
            this.Visible = false;
            notifyIcon1.Visible = true;
            this.ShowInTaskbar = true;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipTitle = "YemekSepeti";
            notifyIcon1.BalloonTipText = "YemekSepeti Çalışıyor...";
            notifyIcon1.ShowBalloonTip(50);
            timer1.Enabled = true;
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            notifyIcon1.Visible = false;
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://www.google.com"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CheckForInternetConnection())
            {
                SiparisKontrolEt();
            }
        }
    }
}