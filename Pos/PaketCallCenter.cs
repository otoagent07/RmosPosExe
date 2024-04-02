using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraNavBar;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Pos.Class;
using Pos.Controllers;
using Pos.Forms;
using Pos.Getir;
using Pos.Getir.Class;
using Pos.Models;
using Pos.Trendyol;
using Pos.YemekSepeti;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Pos
{
    public partial class PaketCallCenter : DevExpress.XtraEditors.XtraForm
    {
        public PaketCallCenter()
        {
            InitializeComponent();
            //xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            xtraTabPage3.PageVisible = false;
        }

        public PaketCallCenter(bool getirAktif) // sadece true geliyor sıkıntı yok maksat buraya girsin ilerde false gönderilebilir
        {
            InitializeComponent();
            //xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

            if (getirAktif)
            {
                //xtraTabControl1.SelectedTabPage = xtraTabPage3;
            }
            else
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage4;
            }
        }

        // select top 1 isnull(Kodlar_Getir_AP,0) as Kodlar_Getir_AP from Stok_Kodlar where Kodlar_sinif=01 and Kodlar_Kod=''
        bool getirAktifmi = false;
        private void PaketCallCenter_Load(object sender, EventArgs e)
        {
            try
            {
                
                dateTarih1.EditValue = Param.Tarih.Date;
                dateTarih2.EditValue = Param.Tarih.Date;
                dateEdit1.EditValue = Param.Tarih.Date;

                look_Paketci.Properties.DataSource = dbtools.SelectTable("select P_Kod,P_Ad + ' ' + P_Soyad as Adsoyad from Rmosmuh.dbo.Pos_User where P_Kulturu = 2 order by P_Kod");
                look_Paketci.Properties.DisplayMember = "Adsoyad";
                look_Paketci.Properties.ValueMember = "P_Kod";

                DataTable dt = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_Sinif = '27' order by Pkod_Kod");
                if (dt.Rows.Count > 0)
                {
                    look_SubeListele.Properties.DataSource = dt;
                    look_SubeListele.Properties.DisplayMember = "Pkod_Ad";
                    look_SubeListele.Properties.ValueMember = "Pkod_Kod";
                }

                if (User.P_Kod.ToUpper() == "RMOS")
                {
                    btn_Gonder.Visible = true;
                }

                Kapatma_Yenile();

                gridView1.SablonYukle(this.Name, 1);
                gridyenile();


                if (Param.Param_CallCenterPaket)
                {
                    timer2.Enabled = true;
                    gridControl5.Visible = true;
                }

                timer1.Enabled = true;

                gridColumn8.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                simpleButton7.Enabled = Departman.Kodlar_YS_Aktif;

                getirYenile();


                //trendyolApi.siparisGetir();
                trendyolYenile();


                if (Departman.Kodlar_Getir_AP == false)
                {
                    timerGetirYenile.Enabled = false;
                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                }

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "PaketCallCenter_Load", "", ex);
            }
        }

        public void getirYenile()
        {
            if (Departman.Kodlar_Getir_AP && RestoGetir())
            {
                //RestoGetir();
                //xtraTabPage3.PageVisible = true;
                SiparisAl(GetirToken.apitoken);
                simpleButton7.Enabled = Departman.Kodlar_Getir_AP;
            }

        }

        private bool RestoGetir()
        {
            GetirLoginResponse loginResponse = getirApi.getToken(Departman.Kodlar_Getir_appSecretKey, Departman.Kodlar_Getir_restaurantSecretKey);
            if (loginResponse == null)
            {
                return false;
            }

            GetirToken.apitoken = loginResponse.token;
            return true;
        }
        public DataTable GetirListele()
        {
            string query = @"Select 
            GOrder.ID as ID,
            GOrder.GOrder_id as GOrder_id,
            GOrder.GOrder_confirmationId as GOrder_confirmationId,
            GOrder.GOrder_Client_name as GOrder_Client_name,
            GOrder.GOrder_Client_contactPhoneNumber,
            CONVERT(decimal(7,2),GOrder.GOrder_totalPrice) as GOrder_totalPrice,
            Convert(varchar,GOrder.GOrder_checkoutDate,121) as GOrder_checkoutDate,
            GOrder.GOrder_status ,
            Case GOrder.GOrder_status 
            When 325 then 'Ön Onay Bekliyor..İleri Tarihli Sipariş'
            when 350 then 'İleri tarihli sipariş, ön onay alındı'
            When 400 then 'Onay Bekleniyor'
            When 500 then 'Hazırlanıyor'
            When 550 then 'Sipariş hazırlandı'
            When 600 then 'Sipariş kuryeye teslim edildi'
            When 700 then 'Kurye yola çıktı'
            When 900 then 'Sipariş teslim edildi'
            When 1500 then 'Sipariş admin tarafından iptal edildi'
            When 1600 then 'Sipariş restoran tarafından iptal edildi' else 'Tanımlı Değil' end as GOrder_status_title,
            GOrder.GOrder_Courier_name as GOrder_Courier_name,
            GOrder.GOrder_PaymentMethodText_tr as GOrder_PaymentMethodText_tr
            From GetirYemek_Order as GOrder where GOrder.GOrder_status < 401
            ";
            DataTable dataTable = dbtools.SelectTable(query);
            gridControl3.DataSource = dataTable;

            if (dataTable != null && dataTable.Rows.Count < 1)
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage1;
            }

            return dataTable;
        }


        public string replaceVirgul(decimal sayi)
        {
            return sayi.ToString().Replace(",", ".");
        }
        public void SiparisAl(string Token)
        {
            List<GetirOrderResponse.Root> getirOrderResponse = getirApi.postOrderPeriodicUnapproved(Token);


            foreach (var item in getirOrderResponse)
            {
                if (item.status == 325)
                {
                    GetirOnay result = getirApi.getOrderVerifyScheduled(GetirToken.apitoken, item.id);
                    if (result.result)
                    {
                        continue;
                    }
                }
                else
                {
                    int Deger = Convert.ToInt32(dbtools.DegerGetir("Select Count(*) From GetirYemek_Order Where GOrder_id = '" + item.id + "'"));
                    if (Deger == 0)
                    {
                        item.totalDiscountedPrice = (item.totalDiscountedPrice == null ? 0 : item.totalDiscountedPrice);



                        decimal discountedPrice = item.totalPrice - item.totalDiscountedPrice;

                        if (item.totalDiscountedPrice == 0)
                        {
                            discountedPrice = 0;
                        }

                        string replaceDiscountedPrice = replaceVirgul(discountedPrice);

                        string query = @"INSERT INTO [dbo].[GetirYemek_Order]
                           ([GOrder_id]
                           ,[GOrder_status]
                           ,[GOrder_isScheduled]
                           ,[GOrder_confirmationId]
                           ,[GOrder_clientNote]
                           ,[GOrder_totalPrice]
                           ,[GOrder_checkoutDate] 
                           ,[GOrder_scheduledDate]
                           ,[GOrder_deliveryType]
                           ,[GOrder_doNotKnock]
                           ,[GOrder_isEcoFriendly]
                           ,[GOrder_paymentMethod]
                           ,[GOrder_totalDiscountedPrice]
                           ,[GOrder_Client_id]
                           ,[GOrder_Client_name]
                           ,[GOrder_Client_clientPhoneNumber]
                           ,[GOrder_Client_contactPhoneNumber]
                           ,[GOrder_Client_Location_lat]
                           ,[GOrder_Client_Location_lon]
                           ,[GOrder_Courier_id]
                           ,[GOrder_Courier_status]
                           ,[GOrder_Courier_name]
                           ,[GOrder_Courier_Location_lat]
                           ,[GOrder_Courier_Location_lon]
                           ,[GOrder_Restaurant_id]
                           ,[GOrder_PaymentMethodText_en]
                           ,[GOrder_PaymentMethodText_tr])
                           VALUES('" + item.id + "','" + item.status + "','" + item.isScheduled + "','" + item.confirmationId + "','" + item.clientNote + "','" + replaceVirgul(item.totalPrice) + @"','" + item.checkoutDate + "','" + (item.scheduledDate == null ? DateTime.Now : item.scheduledDate) + "','" + item.deliveryType + "','" + item.doNotKnock + "','" + item.isEcoFriendly + "','" + item.paymentMethod + @"','" + replaceDiscountedPrice + @"','" + item.client.id + @"','" + item.client.name + @"','" + item.client.clientPhoneNumber + @"','" + item.client.clientUnmaskedPhoneNumber + @"','" + item.client.location.lat + @"','" + item.client.location.lon + @"','" + item.courier.id + @"','" + item.courier.status + @"','" + item.courier.name + @"','" + item.courier.location.lat + @"','" + item.courier.location.lon + @"','" + item.restaurant.id + @"','" + item.paymentMethodText.en + @"','" + item.paymentMethodText.tr + @"') 
                             select SCOPE_IDENTITY()";
                        string OrderID = dbtools.DegerGetir(query);

                        if (item.client.deliveryAddress != null)
                        {
                            dbtools.DegerGetir(@"
                            UPDATE [dbo].[GetirYemek_Order]
                               SET [GOrder_Client_DeliveryAddress_id]		   = '" + item.client.deliveryAddress.id + @"'
                                  ,[GOrder_Client_DeliveryAddress_address]     = '" + item.client.deliveryAddress.address + @"'
                                  ,[GOrder_Client_DeliveryAddress_aptNo]       = '" + item.client.deliveryAddress.aptNo + @"'
                                  ,[GOrder_Client_DeliveryAddress_floor]       = '" + item.client.deliveryAddress.floor + @"'
                                  ,[GOrder_Client_DeliveryAddress_doorNo]      = '" + item.client.deliveryAddress.doorNo + @"'
                                  ,[GOrder_Client_DeliveryAddress_description] = '" + item.client.deliveryAddress.description + @"'
    
                             WHERE ID = '" + OrderID + "'");
                        }


                        string siparisFisi = "";
                        int sonFis = 0;
                        foreach (var product in item.products)
                        {
                            sonFis++;
                            string aciklama = "";
                            foreach (var itemAciklama in product.displayInfo.options.tr)
                            {
                                aciklama = aciklama + itemAciklama + "\n   ";
                            }

                            string boslukAyarla = product.displayInfo.title.tr + "  ";

                            int karakterSayisi = 65;
                            if (boslukAyarla.Length > 25)
                            {
                                karakterSayisi = boslukAyarla.Length + 2;
                            }

                            for (int i = 0; i < karakterSayisi - boslukAyarla.Length; i++) // boslukAyarla.Length
                            {
                                boslukAyarla = boslukAyarla + " ";
                            }

                            decimal priceWithOption = Math.Round(product.totalPriceWithOption, 2);

                            siparisFisi = siparisFisi + boslukAyarla + product.count + "      " + priceWithOption + "\n   " + aciklama + "Not: " + product.note + "\n\n";

                            if (sonFis == item.products.Count)
                            {
                                decimal indirim = item.totalPrice - item.totalDiscountedPrice;

                                decimal toplamTutar = item.totalDiscountedPrice;

                                if (item.totalDiscountedPrice == 0)
                                {
                                    indirim = 0;
                                    toplamTutar = item.totalPrice;
                                }

                                indirim = Math.Round(indirim, 2);

                                siparisFisi = siparisFisi
                                    + "INDIRIM                                               " + Math.Round(indirim, 2) + "\n"
                                    + "--------------------------------------------------------\n"
                                    + "TOPLAM                                               " + Math.Round(toplamTutar, 2);

                            }

                            query = @"INSERT INTO [dbo].[GetirYemek_Product]
                       ([GOrder_ID]
                       ,[GProducts_id]
                       ,[GProducts_imageURL]
                       ,[GProducts_wideImageURL]
                       ,[GProducts_product]
                       ,[GProducts_count]
                       ,[GProducts_chainProduct]
                       ,[GProducts_name_tr]
                       ,[GProducts_name_en]
                       ,[GProducts_price]
                       ,[GProducts_optionPrice]
                       ,[GProducts_totalPrice]
                       ,[GProducts_totalOptionPrice]
                       ,[GProducts_totalPriceWithOption]
                       ,[GProducts_DisplayInfo_Title_tr]
                       ,[GProducts_DisplayInfo_Title_en]
                       ,[GProducts_DisplayInfo_Option_tr]
                       ,[GProducts_DisplayInfo_Option_en]
                       ,[GProducts_Note]
                       ,[GProducts_SiparisFis],
                        GProducts_priceWithOption
                        )
                        VALUES('" + OrderID + @"','" + product.id + @"','" + product.imageURL + @"','" + product.wideImageURL + @"','" + product.product + @"','" + product.count + @"','" + product.chainProduct + @"','" + product.name.tr + @"','" + product.name.en + @"','" + replaceVirgul(product.price) + @"','" + replaceVirgul(product.optionPrice) + @"','" + replaceVirgul(product.totalPrice) + @"','" + replaceVirgul(product.totalOptionPrice) + @"','" + replaceVirgul(product.totalPriceWithOption) + @"','" + product.displayInfo.title.tr + @"','" + product.displayInfo.title.en + @"','" + "" + @"','" + "" + @"','" + product.note + @"','" + siparisFisi + @"','" + replaceVirgul(product.priceWithOption) + @"'
                        ) Select SCOPE_IDENTITY() ";

                            string yoksayQuery = "select ISNULL(product_yoksay,0) as product_yoksay from GetirYemek_Menu_Product where Product_id='" + product.product + "'";
                            bool yoksay = degerGetirBool(yoksayQuery);
                            bool yoksay2 = degerGetirBool(yoksayQuery);


                            string ProductID = dbtools.DegerGetir(query);
                            dbtools.execcmd("update GetirYemek_Product set GProducts_SiparisFis='" + siparisFisi.Replace("'", "''") + "' where GOrder_ID='" + OrderID + "'");



                            foreach (var optionCategory in product.optionCategories)
                            {
                                foreach (var options in optionCategory.options)
                                {
                                    string tercihAd = optionCategory.name.tr.ToLower();

                                    decimal pricess = options.price;

                                    if (pricess != 0 || yoksay || tercihAd.Contains("porsiyon tercihi")) //  
                                    {
                                        pricess = product.priceWithOption; //product.totalPriceWithOption
                                        yoksay = false;
                                    }


                                    if (product.displayInfo.title.tr.ToLower().Contains("menü"))
                                    {
                                        pricess = 0;
                                    }

                                    if (tercihAd.Contains("malzeme tercihi") || tercihAd.Contains("sos tercihi"))
                                    {
                                        pricess = 0;
                                    }

                                    query = @"
                                INSERT INTO [dbo].[GetirYemek_Option]
                                           ([GOptionCategory_ID]
                                           ,[GOption_option]
                                           ,[GOption_name_tr]
                                           ,[GOption_name_en]
                                           ,[GOption_price]
                                           ,[GOption_tr]
                                           ,[GOption_en])
                                     VALUES('" + ProductID + @"','" + options.option + @"','" + options.name.tr + @"','" + options.name.en + @"','"
                                        + replaceVirgul(pricess) + @"','" + options.tr + @"','" + options.en + @"') Select SCOPE_IDENTITY()";


                                    //replaceVirgul(options.price)
                                    yoksayQuery = "select ISNULL(product_yoksay,0) as product_yoksay from GetirYemek_Menu_Product where Product_id='" + options.product + "'";

                                    string optionsID = "";

                                    if (degerGetirBool(yoksayQuery) == false)
                                    {
                                        optionsID = dbtools.DegerGetir(query);
                                    }


                                    if (options.optionCategories != null)
                                    {
                                        foreach (var option2 in options.optionCategories)
                                        {
                                            foreach (var options3 in option2.options)
                                            {


                                                pricess = options3.price;
                                                if (pricess != 0 || yoksay2)
                                                {
                                                    pricess = product.priceWithOption; ;
                                                    yoksay2 = false;
                                                }

                                                if (product.displayInfo.title.tr.ToLower().Contains("menü"))
                                                {
                                                    pricess = 0;
                                                }

                                                query = @"
                                INSERT INTO [dbo].[GetirYemek_Option]
                                           ([GOptionCategory_ID]
                                           ,[GOption_option]
                                           ,[GOption_name_tr]
                                           ,[GOption_name_en]
                                           ,[GOption_price]
                                           ,[GOption_tr]
                                           ,[GOption_en])
                                     VALUES('" + ProductID + @"','" + options3.option + @"','" + options3.name.tr + @"','" + options3.name.en + @"','" + replaceVirgul(pricess) + @"','" + options3.tr + @"','" + options3.en + @"') Select SCOPE_IDENTITY()";

                                                optionsID = dbtools.DegerGetir(query);
                                            }

                                        }
                                    }

                                }
                                //break;
                            }
                        }
                    }
                }
            }
            GetirListele();

        }


        public bool degerGetirBool(string yoksayQuery)
        {
            string deger = dbtools.DegerGetir(yoksayQuery);
            if (deger.Equals("")) deger = "False";
            bool yoksay = Convert.ToBoolean(deger);
            return yoksay;
        }

        GetirApi getirApi = new GetirApi();

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            CallerCallCenter d = new CallerCallCenter();
            d.Tel = "05452109107";
            d.Show();
        }

        public void gridyenile()
        {
            string Durum = string.Empty;

            if (chk_Acik.Checked == true)
            {
                Durum += "A";
            }

            if (chk_Acik.Checked == true && chk_Kapali.Checked == true)
            {
                Durum += ",";
            }

            if (chk_Kapali.Checked == true)
            {
                Durum += "K";
            }

            string Sube = string.Empty;

            if (chk_Sube.Checked == true)
            {
                Sube = "and (Rsat_Sube in (SELECT fieldvalue FROM dbo.stringArray('" + Convert.ToString(look_SubeListele.EditValue) + "',',')))";
            }

            //string query2 = @"select 
            //case when MIN(Rsat_Durum) = 'A' then 'Acik' else 'Kapali' end as Rsat_Durum,
            //MAX(Rsat_Id) as Rsat_Id,
            //Rsat_Tarih,
            //Masa_No,
            //Masa_Ad,
            //Cst_Recete_Satis.Rsat_Fisno,
            //Cari_Kod,(Cari_Ad + ' ' + Cari_Soyad) as Cari_AdSoyad,
            //(SUM(Rsat_Tutar) ) as Tutar,P_Kod,P_Ad + ' ' + P_Soyad as P_AdSoyad ,Rsat_Sube,sube.Pkod_Ad as subeAd,Rsat_SubeDurum,
            //MAX(ISNULL(Cari_Adres1,'')) + ' ' + MAX(ISNULL(Cari_Adres2,'')) + ' ' + MAX(ISNULL(Cari_Adres3,'')) as Cari_Adres, Cari_Tip as Cari_Tip,
            //case  Cari_Tip when 'Y' then 'YEMEK SEPETİ' when 'G' Then 'GETİR YEMEK' when 'T' Then 'TRENDYOL' else 'PAKET' end as YSDurum,
            //case  Cari_Tip when 'Y' then (case MIN(ISNULL(Rsat_YSDurum,'')) when 'Accepted' then 'HAZIRLANIYOR' When 'OnDelivery' then 'SİPARİŞ YOLDA' when 'Delivered' THEN 'TESLİM EDİLDİ' When 'Cancelled' then 'İPTAL EDİLDİ' else '' end) When 'G' Then 
            //(Case MAX(ISNULL(Rsat_GetirDurum,''))
            //When 325 then 'Ön Onay Bekliyor..İleri Tarihli Sipariş'
            //when 350 then 'İleri tarihli sipariş, ön onay alındı'
            //When 400 then 'Onay Bekleniyor'
            //When 500 then 'Hazırlanıyor'
            //When 550 then 'Sipariş hazırlandı'
            //When 600 then 'Sipariş kuryeye teslim edildi'
            //When 700 then 'Kurye yola çıktı'
            //When 900 then 'Sipariş teslim edildi'
            //When 1500 then 'Sipariş admin tarafından iptal edildi'
            //When 1600 then 'Sipariş restoran tarafından iptal edildi' else 'Tanımlı Değil' end) end as Rsat_YSDurum,
            //case Cari_Tip when 'Y' then MIN(ISNULL(Rsat_YSOrderID,'')) when 'G' then MIN(ISNULL(Rsat_GetirOrderID,'')) else  '' end  as Rsat_YSOrderID,
            //Max(Rsat_Not) as Rsat_Not,MIN(Rsat_Acilis) as Rsat_Acilis,
            //case when ISNULL(GOrder_deliveryType,2) = 1 then 'GETİR KURYESİ' else 'RESTORAN KURYESİ' end as Kurye,
            //GOrder_deliveryType as GOrder_deliveryType
            //from Cst_Recete_Satis 
            //left join Pos_Masa on Rsat_Masa = Masa_No  and Masa_Depart = Rsat_Departman
            //left join Pos_Cari on Rsat_Cari = Cari_Kod 
            //left join Rmosmuh.dbo.Pos_User on P_Kod = Rsat_Paketci 
            //left join Pos_Kodlar as sube on sube.Pkod_Kod = Rsat_Sube and sube.Pkod_Sinif = '27'
            //Left Join GetirYemek_Order on GOrder_id = Rsat_GetirOrderID
            //where (Rsat_Durum in (SELECT fieldvalue FROM dbo.stringArray('" + Durum + @"',','))) 
            //and Convert(date,Rsat_Tarih) >= '" + dateTarih1.DateTime.Date.ToString("yyyy-MM-dd") + @"' and Convert(date,Rsat_Tarih) <= '" + dateTarih2.DateTime.Date.ToString("yyyy-MM-dd") + @"'
            //and Rsat_Ba = 'B' and Masa_Konum = 'P' and Rsat_Departman = '" + Departman.Dep_Kodu + @"' " + Sube + @"
            //group by Rsat_Tarih,Masa_No,Masa_Ad,Cst_Recete_Satis.Rsat_Fisno,Cari_Kod,Cari_Ad,
            //Cari_Soyad,Cari_Adres1,P_Kod,P_Ad,P_Soyad ,Rsat_Sube,sube.Pkod_Ad ,Rsat_SubeDurum,Cari_Tip,GOrder_deliveryType
            //order by Cst_Recete_Satis.Rsat_Fisno desc ";

            string query = StatikModel.getPaketSqlText(dateTarih1, dateTarih2, Departman.Dep_Kodu, Sube, Durum,sistemTar: checkEditSistemTarihi.Checked);
            gridControl1.DataSource = dbtools.SelectTableR(query);


            //gridView1.BestFitColumns();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                string Masa_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Masa_No"));

                Satis satis = new Satis();
                satis.Tag = "M";
                satis.Masa_No = Masa_No;
                satis.Masa_Paket = true;
                satis.Ozel_Masa = "";
                satis.Split = 0;
                satis.Splitad = "";
                satis.ShowDialog();
                satis.PaketFiyat = "P";
                gridyenile();
            }

        }

        ResourceManager res_man = new ResourceManager("Pos.Class.lang_" + (Langs.Default.Dil == "" ? "tr" : Langs.Default.Dil.Substring(0, 2)), Assembly.GetExecutingAssembly());
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DataTable dtMasa = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1 and Masa_Durum = 0 order by Masa_No");

            if (dtMasa.Rows.Count < 1)
            {
                MessageBox.Show(res_man.GetString("Boş Paket Masanız Bulunmamaktadır."));
                return;
            }

            string Masa_No = Convert.ToString(dtMasa.Rows[0]["Masa_No"]);

            Satis satis = new Satis();
            satis.Tag = "M";
            satis.Masa_No = Masa_No;
            satis.Masa_Paket = true;
            satis.Ozel_Masa = "";
            satis.Split = 0;
            satis.Splitad = "";
            satis.ShowDialog();
            satis.PaketFiyat = "P";



            gridyenile();
        }

        private void btn_Yenile_Click(object sender, EventArgs e)
        {
            gridyenile();
        }

        private void btn_Gonder_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                string SubeKod = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Sube"));
                string Fisno = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));

                SubeyeGonder(SubeKod, Fisno);
            }
        }

        public void SubeyeGonder(string SubeKod, string Fisno)
        {
            try
            {
                if (string.IsNullOrEmpty(SubeKod))
                {
                    MessageBox.Show(res_man.GetString("Sube Boş Geçilemez."));
                    return;
                }

                DataTable dtSube = dbtools.SelectTable("select * from Pos_Kodlar where PKod_Sinif= '27' and Pkod_Kod = '" + SubeKod + "'");
                if (dtSube.Rows.Count <= 0) return;

                string subeConnectionString = "Data Source='" + Convert.ToString(dtSube.Rows[0]["Pkod_Server"]) + "';Initial Catalog=" + Convert.ToString(dtSube.Rows[0]["Pkod_Database"]) + "; Persist Security Info=True;uid='" + Convert.ToString(dtSube.Rows[0]["Pkod_User"]) + "'; pwd='" + Convert.ToString(dtSube.Rows[0]["Pkod_Password"]) + "'";

                DataTable dtFis = dbtools.SelectTable("select * from Cst_Recete_Satis where Rsat_Fisno = '" + Fisno + "' and Rsat_BA = 'B'");
                string jsonData = JsonConvert.SerializeObject(dtFis, Formatting.Indented);

                DataTable dtCari = dbtools.SelectTable(@"select [Cari_Id]
                                                          ,ISNULL([Cari_Kod],'') as Cari_Kod
                                                          ,ISNULL([Cari_Ad],'') as Cari_Ad
                                                          ,ISNULL([Cari_Soyad],'') as Cari_Soyad
                                                          ,ISNULL([Cari_Tel],'') as Cari_Tel
                                                          ,ISNULL([Cari_Adres1],'') as Cari_Adres1
                                                          ,ISNULL([Cari_Adres2],'') as Cari_Adres2
                                                          ,ISNULL([Cari_Adres3],'') as Cari_Adres3
                                                          ,ISNULL([Cari_Funvan],'') as Cari_Funvan
                                                          ,ISNULL([Cari_Fadres1],'') as Cari_Fadres1
                                                          ,ISNULL([Cari_Fadres2],'') as Cari_Fadres2
                                                          ,ISNULL([Cari_Vergidarie],'') as Cari_Vergidarie
                                                          ,ISNULL([Cari_Vergino],'') as Cari_Vergino
                                                          ,ISNULL([Cari_Mail],'') as Cari_Mail
                                                          ,ISNULL([Cari_Kart],'') as Cari_Kart
                                                          ,ISNULL([Cari_Tel2],'') as Cari_Tel2
                                                          ,ISNULL([Cari_Email],'') as Cari_Email
                                                          ,ISNULL([Cari_Tip],'') as Cari_Tip
                                                          ,ISNULL([Cari_Limit],0)  as Cari_Limit
                                                          ,ISNULL([Cari_LimitTutar],0) as Cari_LimitTutar
                                                          ,ISNULL([Cari_Il],'') as Cari_Il
                                                          ,ISNULL([Cari_Ilce],'') as Cari_Ilce
                                                          ,ISNULL([Cari_Mahalle],'') as Cari_Mahalle
                                                          FROM [dbo].[Pos_Cari] where Cari_Kod = '" + Convert.ToString(dtFis.Rows[0]["Rsat_Cari"]) + "'");
                string jsonDataCari = JsonConvert.SerializeObject(dtCari, Formatting.Indented);


                SqlConnection con = new SqlConnection(subeConnectionString);
                SqlCommand cmd = new SqlCommand("insert into Pos_CallCenter(Center_Data,Center_Cari) values ('" + jsonData + "','" + jsonDataCari + "')", con);
                if (con.State != ConnectionState.Open) con.Open();
                cmd.ExecuteNonQuery();
                if (con.State != ConnectionState.Closed) con.Close();

                dbtools.execcmd("update Cst_Recete_satis set Rsat_SubeDurum = 'Gönderildi' where Rsat_Fisno = '" + Fisno + "'");

                MessageBox.Show(res_man.GetString("Siparişiniz Gönderildi."));

                gridyenile();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool Odeme_Al(string Sube, int Fisno, decimal Tutar, string Masano)
        {
            DataTable dtSubeMerkez = dbtools.SelectTable("Select Pkod_MerkezSube,Pkod_KapatmaKodu,Pkod_KapatmaHesabi From Pos_Kodlar Where Pkod_Sinif = '27' and Pkod_Kod = '" + Sube + "'");
            if (dtSubeMerkez.Rows.Count > 0)
            {
                if (Convert.ToString(dtSubeMerkez.Rows[0]["Pkod_MerkezSube"]) != "S")
                {
                    MessageBox.Show(res_man.GetString("Lütfen Şube Seçiniz.."), "Uyarı");
                    return false;
                }
                if (Convert.ToString(dtSubeMerkez.Rows[0]["Pkod_KapatmaKodu"]) == "")
                {
                    MessageBox.Show(res_man.GetString("Lütfen Kapatma Kodu Tanıtınız.."), "Uyarı");
                    return false;
                }
                if (Convert.ToString(dtSubeMerkez.Rows[0]["Pkod_KapatmaHesabi"]) == "")
                {
                    MessageBox.Show(res_man.GetString("Lütfen Kapatma Hesabı Tanıtınız.."), "Uyarı");
                    return false;
                }


                Fis_Islem.Odeme_Al(
                   Fisno,
                    Tutar,
                    Tutar,
                    Convert.ToString(dtSubeMerkez.Rows[0]["Pkod_KapatmaKodu"]),
                    "C",
                    "",
                    0,
                    Convert.ToString(dtSubeMerkez.Rows[0]["Pkod_KapatmaHesabi"]),
                    0,
                    Convert.ToString(Param.Doviz_Kodu), false);

                //Fis_Islem.Satis_Tip(Convert.ToInt32(this.Tag), Convert.ToString(look_Kapatma.EditValue), pansiyon_A);


                //Fis_Islem.Onburo_At(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Fisno")));
                Fis_Islem.Onburo_At(Fisno, "", 0);

                //Masa_No
                //dbtools.execcmd("update Pos_Masa set Masa_Durum = '0', Masa_Ozel = '' where Masa_No = '" + Masa_No + "' and Masa_Depart = '" + Departman.Dep_Kodu + "'");
                dbtools.execcmd("exec Pos_Sorgu @Sorgu_Tipi = 16,@Masano = '" + Masano + "',@Dep_Kodu = '" + Departman.Dep_Kodu + "'");

                return true;
            }

            return false;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                string SubeMerkez = dbtools.DegerGetir("Select Pkod_MerkezSube From Pos_Kodlar Where Pkod_Sinif = '27' and Pkod_Kod = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Sube")) + "'");
                if (SubeMerkez == "S")
                {
                    bool sir = Odeme_Al(Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Sube")), Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Fisno")), Convert.ToDecimal(gridView1.GetFocusedRowCellValue("Tutar")), Convert.ToString(gridView1.GetFocusedRowCellValue("Masa_No")));
                    if (sir == true)
                    {
                        gridyenile();
                        return;
                    }
                }

                string Fisno = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                string Masa_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Masa_No"));

                Hesap hes = new Hesap();
                hes.Tag = Fisno;
                hes.Masa_No = Masa_No;
                hes.Split = 0;
                hes.Splitad = "";
                hes.ShowDialog();
                gridyenile();
            }
        }

        private void btn_SubeDegis_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                string SubeKod = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Sube"));
                string Fisno = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));


                CallerSubeSec sube = new CallerSubeSec();
                sube.ShowDialog();
                if (string.IsNullOrEmpty(sube.kod)) return;

                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Sube = '" + sube.kod + "' where Rsat_Fisno = '" + Fisno + "'");

                gridyenile();
            }
        }

        private void PaketciDegistir(int Fis_No)
        {
            Garson_Sor pkt = new Garson_Sor();
            pkt.Tag = "PAKET";
            pkt.ShowDialog();
            string Paketci = pkt.Garson_Kod;

            if (Paketci != "")
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_Paketci = '" + Paketci + "' where Rsat_Fisno = '" + Fis_No + "'");
            }


        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                PaketciDegistir(Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Fisno")));
                gridyenile();
            }
        }

        private void btn_PaketPr_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                int Fisno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                string Masa_No = Convert.ToString(gridView1.GetFocusedRowCellValue("Masa_No"));

                FisPr pr = new FisPr();

                string baslik;
                string add = Convert.ToString(gridView1.GetFocusedRowCellValue("YSDurum"));

                bool trendyol = false;
                if (add == "PAKET")
                {
                    baslik = "* * * PAKET FİSİ * * * ";
                }
                else if (add == "GETİR YEMEK")
                {
                    baslik = "* * * GETİRYEMEK PAKET FİSİ * * * "; 
                }
                else if (add == "TRENDYOL")
                {
                    baslik = "* * * TRENDYOL PAKET FİSİ * * * ";
                    trendyol = true;
                }
                else
                {
                    baslik = "* * * YEMEKSEPETİ PAKET FİSİ * * * ";
                }

                string siparisFisi = gridView1.GetFocusedRowCellValue("ID").ToString();

                siparisFisi = dbtools.DegerGetir("select top 1 GProducts_SiparisFis from GetirYemek_Product where GOrder_ID='" + siparisFisi + "'");

                string sonuc = "";
                if (trendyol)
                {
                     sonuc = pr.PaketPrTrendyol(Fisno, baslik);
                }
                else
                {
                     sonuc = pr.PaketPr(Fisno, baslik, siparisFisi);
                }

                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc);
                }
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            look_SubeListele.Enabled = chk_Sube.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            gridyenile();
        }

        private void look_SubeListele_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string fisno = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));


            CariHesap cari = new CariHesap();
            cari.xtraTabControl1.SelectedTabPageIndex = 1;
            cari.CariKod = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
            cari.BilgiCari = true;
            cari.ShowDialog();

            string yenicari = cari.cariKodPaketGuncelle;
            if (yenicari != "")
            {
                dbtools.execcmdR($"update Cst_Recete_Satis set Rsat_Cari='{yenicari}' where Rsat_Fisno='{fisno}'");
            }

            gridyenile();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                return;
            }

            string fisno = gridView1.GetFocusedRowCellValue("Rsat_Fisno").ToString();
            string GOrder_confirmationId = gridView1.GetFocusedRowCellValue("GOrder_confirmationId").ToString();
            string sipariId = gridView1.GetFocusedRowCellValue("Rsat_EntegreId").ToString();


            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Tip")) == "Y")
            {
                YS_SiparisDurum y = new YS_SiparisDurum();
                y.YSOrderID = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_YSOrderID"));
                y.Fis = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                y.CariKod = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
                y.ShowDialog();

            }

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Tip")) == "G")
            {
                string durum = gridView1.GetFocusedRowCellValue("Rsat_YSDurum").ToString();
                if (durum.ToLower().Contains("sipariş teslim edildi"))
                {
                    return;
                }

                string saat = gridView1.GetFocusedRowCellValue("Rsat_Acilis").ToString();
                string gelenTar = DateTime.Now.ToString("yyyy-MM-dd ") + saat;
                DateTime gelenTarih = Convert.ToDateTime(gelenTar);


                DateTime bitisTarih = DateTime.Now; //  gelenTarih.AddMinutes(2)

                int farkDk = (int)(bitisTarih - gelenTarih).TotalMinutes;

                DateTime bitTarr = gelenTarih.AddMinutes(2);


                if (farkDk < 3)
                {
                    RHMesaj.MyMessageInformation("Lütfen " + bitTarr.ToString("HH:mm:ss") + " Zamandan Sonra Tıklayınız !");
                    return;
                }

                Getir_Durum y = new Getir_Durum(durum, fisno, GOrder_confirmationId);
                y.OrderID = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_YSOrderID"));
                y.deliveryType = Convert.ToInt32(gridView1.GetFocusedRowCellValue("GOrder_deliveryType"));

                //y.Fis = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                //y.CariKod = Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Kod"));
                y.ShowDialog();

            }

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Tip")) == "T")
            {
                TrendyolOnayForm trendyolOnayForm = new TrendyolOnayForm(sipariId, fisno);
                trendyolOnayForm.ShowDialog();
            }
            gridyenile();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            YS_RestoranDurum s = new YS_RestoranDurum();
            s.ShowDialog();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void müşteriNumarasıGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                return;
            }

            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Cari_Tip")) == "Y")
            {
                YS_MusteriListesi y = new YS_MusteriListesi();
                y.Fisno = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));
                y.ShowDialog();
            }
            gridyenile();
        }


        private void Kapatma_Yenile()
        {
            DataTable dt = dbtools.SelectTable("select Pkod_Kod,Pkod_Ad,ISNULL(Pkod_OdemeBtnRenk,'') as Pkod_OdemeBtnRenk from Pos_Kodlar with(nolock) where Pkod_Sinif = '11' and Pkod_Ozelkod <> '4' and Pkod_Ozelkod <> '8' order by Pkod_Sira ");

            //if (dt.Rows.Count < 8)
            //{
            //    panelControl2.Size = new System.Drawing.Size(741, 65);
            //}
            //else
            //{
            //    panelControl2.Size = new System.Drawing.Size(741, 125);
            //}


            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string backcolor = Convert.ToString(dt.Rows[i]["Pkod_OdemeBtnRenk"]);
                    Color color = new Color();
                    if (backcolor != "")
                    {
                        color = System.Drawing.ColorTranslator.FromHtml(backcolor);
                    }

                    SimpleButton btn_Kapatma = new SimpleButton();
                    btn_Kapatma.Size = new System.Drawing.Size(90, 50);
                    btn_Kapatma.TabIndex = 0;
                    btn_Kapatma.TabStop = false;
                    btn_Kapatma.Font = new System.Drawing.Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    btn_Kapatma.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
                    btn_Kapatma.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btn_Kapatma.Appearance.Options.UseBackColor = true;
                    btn_Kapatma.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                    btn_Kapatma.Appearance.BackColor = color;

                    btn_Kapatma.Text = Convert.ToString(dt.Rows[i]["Pkod_Ad"]);
                    btn_Kapatma.Tag = Convert.ToString(dt.Rows[i]["Pkod_Kod"]);

                    btn_Kapatma.Click += new EventHandler(btn_Kapatma_Click);
                    flp_Kapatma.Controls.Add(btn_Kapatma);
                }
            }

        }

        void btn_Kapatma_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno")) == "")
            {
                MessageBox.Show(res_man.GetString("Paket Sipariş Seçiniz..."), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Param.Param_Masaacan_Garson && !User.M_BaskaMasa)
            {
                string garson = dbtools.DegerGetir("select ISNULL((select top 1 Rsat_Garson from Cst_Recete_Satis where Rsat_Ba = 'B' and Rsat_Fisno = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno")) + "' order by Rsat_Id),'')");
                if (garson != User.P_Kod && garson != "")
                {
                    MessageBox.Show(res_man.GetString("Masayı ") + garson + " - " + User.Isim_Getir(garson) + res_man.GetString(" Açmıştır.Başkası Satış Yapamaz...!!!"), res_man.GetString("Uyarı"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            SimpleButton btn = (SimpleButton)sender;

            timer1.Enabled = false;

            Hesap hes = new Hesap();
            hes.Tag = Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno"));
            //hes.Masa_No = Masa_No;
            hes.look_Kapatma.EditValue = btn.Tag.ToString();
            hes.Split = 0;
            hes.CariKodu = dbtools.DegerGetir("select ISNULL(Rsat_Cari,'') as Rsat_Cari From Cst_Recete_Satis where Rsat_Fisno = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rsat_Fisno")) + "' group by Rsat_Cari");
            if (Param.Param_Hesap_Disable) hes.look_Kapatma.Enabled = false;
            hes.ShowDialog();
            //MasaYenile(0);
            gridyenile();
            timer1.Enabled = true;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridView1.SablonKaydet(this.Name, 1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridView1.SablonSil(this.Name, 1);
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Garson_Sor pkt = new Garson_Sor();
            pkt.Tag = "PAKET";
            pkt.ShowDialog();

            if (pkt.cikis)
            {
                return;
            }
            string Paketci = pkt.Garson_Kod;

            FisPr pr = new FisPr();


            string sonuc = pr.PaketciPr(Param.Tarih, Paketci);
            if (sonuc != "OK")
            {
                MessageBox.Show(sonuc);
            }


        }

        CheckButton chktip = null;
        private void Tip_Changed(object sender, EventArgs e)
        {

        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            gridyenile_2();
        }

        private void gridyenile_2()
        {
            SqlConnection con = dbtools.conn;
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 0;
            com.CommandText = "Pos_Satis_Rapor";
            com.Parameters.AddWithValue("@Rapor_Tipi", 15);
            com.Parameters.AddWithValue("@Tarih1", dateEdit1.DateTime.Date);
            com.Parameters.AddWithValue("@Tarih2", dateEdit1.DateTime.Date);
            com.Parameters.AddWithValue("@Acik", chk_Acik.Checked);
            com.Parameters.AddWithValue("@Kapali", chk_Kapali.Checked);
            if (chk_TekPaket.Checked) com.Parameters.AddWithValue("@Paketci", look_Paketci.EditValue);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            gridControl2.DataSource = dt;
            gridView2.BestFitColumns();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                PaketciDegistir(Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Fisno")));
                gridyenile_2();
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                Detay detay = new Detay();
                detay.spn_Fisno.EditValue = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Fisno"));
                //detay.btn_Fispr.Visible = false;
                //detay.btn_Adisyonpr.Visible = false;
                //detay.btn_Faturapr.Visible = false;
                detay.ShowDialog();
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            string leftColumn = "Paket Raporu";
            string rightColumn = dateEdit1.DateTime.ToLongDateString() + "-" + dateEdit1.DateTime.ToLongDateString();


            PrintingSystem printingSystem1 = new PrintingSystem();
            PrintableComponentLink printableComponentLink1 = new PrintableComponentLink();
            printingSystem1.Links.AddRange(new object[] { printableComponentLink1 });
            printableComponentLink1.Component = gridControl2;
            printableComponentLink1.Landscape = false;
            printableComponentLink1.Margins = new System.Drawing.Printing.Margins(20, 20, 50, 20);

            PageHeaderFooter phf = printableComponentLink1.PageHeaderFooter as PageHeaderFooter;
            phf.Header.Content.Clear();
            phf.Header.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            phf.Header.Content.AddRange(new string[] { leftColumn, rightColumn });
            phf.Header.LineAlignment = BrickAlignment.Far;
            printableComponentLink1.ShowPreview();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                FisPr pr = new FisPr();
                pr.PaketciPr(dateEdit1.DateTime.Date, Convert.ToString(look_Paketci.EditValue));
            }
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {


            if (gridView2.RowCount > 0)
            {
                int Fisno = Convert.ToInt32(gridView2.GetFocusedRowCellValue("Rsat_Fisno"));
                string Masa_No = Convert.ToString(gridView2.GetFocusedRowCellValue("Masa_No"));

                FisPr pr = new FisPr();
                string sonuc = pr.PaketPr(Fisno, " * * * PAKET FİSİ * * * ");
                if (sonuc != "OK")
                {
                    MessageBox.Show(sonuc);
                }
            }
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridView5.RowCount == 0) return;

                string jsonSatis = Convert.ToString(gridView5.GetFocusedRowCellValue("Center_Data"));
                string jsonCari = Convert.ToString(gridView5.GetFocusedRowCellValue("Center_Cari"));
                string id = Convert.ToString(gridView5.GetFocusedRowCellValue("Center_Id"));

                DataTable dtSatis = (DataTable)JsonConvert.DeserializeObject(jsonSatis, (typeof(DataTable)));
                DataTable dtCari = (DataTable)JsonConvert.DeserializeObject(jsonCari, (typeof(DataTable)));


                DataTable dtMasa = dbtools.SelectTable("select * from Pos_Masa where Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1 and Masa_Durum = 0 order by Masa_No");

                if (dtMasa.Rows.Count < 1)
                {
                    MessageBox.Show(res_man.GetString("Boş Paket Masanız Bulunmamaktadır."));
                    return;
                }


                #region Cari
                bool cari_kaydet = true;

                string cariKod = Convert.ToString(dtCari.Rows[0]["Cari_Kod"]);

                DataTable dt1 = dbtools.SelectTable(@"select [Cari_Id]
        ,ISNULL([Cari_Kod],'') as Cari_Kod
      ,ISNULL([Cari_Ad],'') as Cari_Ad
      ,ISNULL([Cari_Soyad],'') as Cari_Soyad
      ,ISNULL([Cari_Tel],'') as Cari_Tel
      ,ISNULL([Cari_Adres1],'') as Cari_Adres1
      ,ISNULL([Cari_Adres2],'') as Cari_Adres2
      ,ISNULL([Cari_Adres3],'') as Cari_Adres3
      ,ISNULL([Cari_Funvan],'') as Cari_Funvan
      ,ISNULL([Cari_Fadres1],'') as Cari_Fadres1
      ,ISNULL([Cari_Fadres2],'') as Cari_Fadres2
      ,ISNULL([Cari_Vergidarie],'') as Cari_Vergidarie
      ,ISNULL([Cari_Vergino],'') as Cari_Vergino
      ,ISNULL([Cari_Mail],'') as Cari_Mail
      ,ISNULL([Cari_Kart],'') as Cari_Kart
      ,ISNULL([Cari_Tel2],'') as Cari_Tel2
      ,ISNULL([Cari_Email],'') as Cari_Email
      ,ISNULL([Cari_Tip],'') as Cari_Tip
      ,ISNULL([Cari_Limit],0)  as Cari_Limit
      ,ISNULL([Cari_LimitTutar],0) as Cari_LimitTutar
      ,ISNULL([Cari_Il],'') as Cari_Il
      ,ISNULL([Cari_Ilce],'') as Cari_Ilce
      ,ISNULL([Cari_Mahalle],'') as Cari_Mahalle
       from Pos_Cari where Cari_Tel = '" + Convert.ToString(dtCari.Rows[0]["Cari_Tel"]) + "'");
                if (dt1.Rows.Count > 0)
                {
                    cariKod = Convert.ToString(dt1.Rows[0]["Cari_Kod"]);
                    cari_kaydet = false;
                }


                if (cari_kaydet)
                {

                    string colCari = "";
                    for (int i = 0; i < dtCari.Columns.Count; i++)
                    {
                        if (dtCari.Columns[i].ColumnName == "Cari_Id") continue;

                        colCari += dtCari.Columns[i].ColumnName + ",";
                    }
                    colCari = colCari.Substring(0, colCari.Length - 1);

                    string valueCari = "";
                    for (int j = 0; j < dtCari.Columns.Count; j++)
                    {
                        if (dtCari.Columns[j].ColumnName == "Cari_Id") continue;

                        if (dtCari.Columns[j].ColumnName == "Cari_LimitTutar")
                        {
                            valueCari += "'0',";
                            continue;
                        }

                        valueCari += "'" + Convert.ToString(dtCari.Rows[0][j]).Replace(",", ".") + "',";
                    }
                    valueCari = valueCari.Substring(0, valueCari.Length - 1);

                    cariKod = dbtools.DegerGetir(@"INSERT INTO [dbo].[Pos_Cari](" + colCari + @")VALUES(" + valueCari + @")
                        declare @id int = (select SCOPE_IDENTITY())
                        update Pos_Cari set Cari_Kod = @id where Cari_Id = @id
                        select @id");

                }
                #endregion

                #region Satıs
                string Masano = Convert.ToString(dtMasa.Rows[0]["Masa_No"]);
                int Fisno = Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
                StatikSinif.siranoarttir();

                string col = "";
                for (int i = 0; i < dtSatis.Columns.Count; i++)
                {
                    if (dtSatis.Columns[i].ColumnName == "Rsat_Id") continue;

                    col += dtSatis.Columns[i].ColumnName + ",";
                }
                col = col.Substring(0, col.Length - 1);

                string value = "";
                for (int i = 0; i < dtSatis.Rows.Count; i++)
                {
                    for (int j = 0; j < dtSatis.Columns.Count; j++)
                    {
                        if (dtSatis.Columns[j].ColumnName == "Rsat_Id") continue;

                        if (dtSatis.Columns[j].ColumnName == "Rsat_Masa")
                        {
                            value += "'" + Masano + "',";
                            continue;
                        }
                        if (dtSatis.Columns[j].ColumnName == "Rsat_Fisno")
                        {
                            value += "'" + Fisno + "',";
                            continue;
                        }
                        if (dtSatis.Columns[j].ColumnName == "Rsat_Durum")
                        {
                            value += "'A',";
                            continue;
                        }
                        if (dtSatis.Columns[j].ColumnName == "Rsat_Cari")
                        {
                            value += "'" + cariKod + "',";
                            continue;
                        }
                        if (dtSatis.Columns[j].ColumnName == "Rsat_Ind")
                        {
                            value += "0, ";
                            continue;
                        }
                        if (dtSatis.Columns[j].ColumnName == "Rsat_SiparisPr")
                        {
                            value += "'0',";
                            continue;
                        }

                        if (dtSatis.Columns[j].ColumnName == "Rsat_Sube")
                        {
                            value += "'" + Departman.Kodlar_PosSubeKod + "',";
                            continue;
                        }

                        value += "'" + Convert.ToString(dtSatis.Rows[i][j]).Replace(",", ".") + "',";
                    }
                    value = value.Substring(0, value.Length - 1);


                    string iki = value.Substring(0, value.Length - 2);
                    value = iki + "'0'";

                    dbtools.execcmd(@"INSERT INTO [dbo].[Cst_Recete_Satis](" + col + ") VALUES(" + value + ")");


                    dbtools.execcmd(@"update Pos_Masa set Masa_Durum = 1 where Masa_No = '" + Masano + @"' and Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1");

                    //col = String.Empty;
                    value = String.Empty;



                }
                #endregion

                dbtools.execcmd(@"Update Pos_CallCenter Set Center_Pasif = 1 Where Center_Id = '" + id + "'");
                //xtraTabControl1_SelectedPageChanged(null, null);

                FisPr pr = new FisPr();
                pr.SiparisPr(Fisno, false, 0);
                pr.PaketPr(Fisno, " * * * PAKET FİSİ * * * ");



                //DataTable dtMerkez = dbtools.SelectTable("select * from Pos_Kodlar where Pkod_MerkezSube = 'M' and  Pkod_Sinif = '27'  ");
                //if (dtMerkez.Rows.Count > 0)
                //{
                //    string merkezConnectionString = "Data Source='" + Convert.ToString(dtMerkez.Rows[0]["Pkod_Server"]) + "';Initial Catalog=" + Convert.ToString(dtMerkez.Rows[0]["Pkod_Database"]) + "; Persist Security Info=True;uid='" + Convert.ToString(dtMerkez.Rows[0]["Pkod_User"]) + "'; pwd='" + Convert.ToString(dtMerkez.Rows[0]["Pkod_Password"]) + "'";


                //    SqlConnection con = new SqlConnection(merkezConnectionString);
                //    SqlCommand cmd = new SqlCommand("update Cst_Recete_satis set Rsat_SubeDurum = 'Sipariş Hazırlanıyor' where Rsat_Fisno = '" + dtSatis.Rows[0]["Rsat_Fisno"] + "'", con);
                //    if (con.State != ConnectionState.Open) con.Open();
                //    cmd.ExecuteNonQuery();
                //    if (con.State != ConnectionState.Closed) con.Close();
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            gridControl5.DataSource = dbtools.SelectTable("select 'Yeni Sipariş' as Data,* from Pos_CallCenter where ISNULL(Center_Pasif,0) = 0");
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0)
            {
                return;
            }

            Kaydet(Convert.ToString(gridView3.GetFocusedRowCellValue("ID")));

            GetirListele();
            gridyenile();

        }

        string CariKod = "";
        public static Getir_Panel getir_Panel = null;
        int Fisno = 0;
        int FisnoAL()
        {
            StatikSinif.siranoarttir();
            return Convert.ToInt32(dbtools.DegerGetir("execute Cost_Fis_No"));
        }

        public void Urun_Sat(DataSet Product, int Fis, decimal Joker, string GetirYemek_Order_ID)
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

            foreach (DataRow order in Product.Tables[0].Rows)
            {
                DataTable dt = dbtools.SelectTable("Select * From Pos_Cari Where Cari_Getir_ID = '" + order["GOrder_Client_id"] + "'");
                if (dt.Rows.Count > 0)
                {
                    Rsat_Cari = Convert.ToString(dt.Rows[0]["Cari_Kod"]);
                    Rsat_MusTipi = Convert.ToString(dt.Rows[0]["Cari_Tip"]);
                    Rsat_Uye_Ad = Convert.ToString(dt.Rows[0]["Cari_Ad"]) + " " + Convert.ToString(dt.Rows[0]["Cari_Soyad"]);
                    //Not = order["GOrder_PaymentMethodText_tr"].ToString();
                }

                break;
            }

            string Masa_No = Convert.ToString(dtMasa.Rows[0]["Masa_No"]);//*****

            bool birkere = false; string total = "", indirim = "";
            foreach (DataRow product in Product.Tables[0].Rows)
            {

                string yoksayQuery = "select ISNULL(product_yoksay,0) as product_yoksay from GetirYemek_Menu_Product where Product_id='" + product["id"].ToString() + "'";


                //if (birkere)
                //{
                //    product["Total"] = total;
                //    product["Discount"] = indirim;
                //    birkere = false;
                //}

                if (degerGetirBool(yoksayQuery) == true)
                {
                    //total = product["Total"].ToString();
                    //indirim = product["Discount"].ToString();
                    //birkere = true;
                    continue;
                }



                //for (int i = 0; i < r.order.product.Count; i++)
                //{

                string query = "exec Pos_Sorgu @Sorgu_Tipi = 39, @Dep_Kodu = '" + Departman.Dep_Kodu + "',@YS_UrunID = '" + product["id"] + "'";
                DataTable dtRecete = dbtools.SelectTable(query);

                if (dtRecete.Rows.Count == 0)
                {

                    if (getir_Panel != null)
                    {
                        return;
                    }
                    MessageBox.Show("SİPARİŞ ALIMI DURDURULACAKTIR...\nİŞLEME DEVAM EDEBİLMEK İÇİN ÜRÜN TANIMLAMA EKRANI AÇILACAKTIR...\nLÜTFEN ÜRÜN EŞLEŞTİRMESİ YAPINIZ..\nÜRÜN İŞLEMİ TAMAMLANDIKTAN SONRA SİPARİŞLERİNİZ ONAYLAYABİLİRSİNİZ..", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    //if (MessageBox.Show("RESTORAN DURUMU YOĞUN DURUMUNA ALINSIN MI?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                    //    string msg = iws.UpdateRestaurantState(YS_RestoInfo.YS_CatalogName, YS_RestoInfo.YS_CatagoryName, IntegrationWebService1.RestaurantStates.HugeDemand);
                    //    dbtools.execcmd("Update YS_Restaurant set YS_OpenClosed = 'YOĞUN' where YS_CatagoryName = '" + YS_RestoInfo.YS_CatagoryName + "'");
                    //}

                    dbtools.execcmd("delete from Cst_Recete_Satis where Rsat_Fisno='" + Fis + "'");
                    getir_Panel = new Getir_Panel();
                    getir_Panel.navBarControl1.LinkSelectionMode = LinkSelectionModeType.OneInGroupAndAllowAutoSelect;
                    getir_Panel.navBarItem2.AllowAutoSelect = true;
                    getir_Panel.navBarControl1.SelectedLink = getir_Panel.navBarControl1.Groups[0].ItemLinks[1];
                    getir_Panel.xtraTabControl1.SelectedTabPage = getir_Panel.tab_Menu;
                    getir_Panel.menuGetir();
                    getir_Panel.ShowDialog();
                    getir_Panel = null;

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

                //if (Convert.ToString(product["Options"]) == "")
                //{
                Rsat_Emiktar = "T";
                //}
                //else if (Convert.ToString(product["Options"]) == "1.5 Porsiyon")
                //{
                //    Rsat_Emiktar = "B";
                //}
                //else if (Convert.ToString(product["Options"]) == "2 Porsiyon")
                //{
                //    Rsat_Emiktar = "D";
                //}


                //if (!Param.Param_Paket_YD)
                //{
                Rec_Fiyat = Convert.ToDecimal(product["Total"].ToString().Replace(".", ","));
                Rec_Dovifiyat = Convert.ToDecimal(product["Total"].ToString().Replace(".", ","));
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
                com.Parameters.AddWithValue("@Rsat_Miktar", (product["quantity"]).ToString().Replace(",", "."));
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
                com.Parameters.AddWithValue("@Rsat_Aciklama", product["GProducts_note"]);
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
                com.Parameters.AddWithValue("@Rsat_Not", (product["GOrder_clientNote"] == "" ? "" : "(Sipariş Notu : " + product["GOrder_clientNote"] + ")\n") + "(Ödeme : " + product["GOrder_PaymentMethodText_tr"] + ")");
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

            }

            if (Joker > 0)
            {
                Fis_Islem.Manuel_Indirim(Convert.ToInt32(Fis), "T", Joker, Joker, 0, 0);
            }

            // todo yap
            GetirOnay result = getirApi.getOrderVerify(GetirToken.apitoken, Convert.ToString(gridView3.GetFocusedRowCellValue("GOrder_id")));
            if (result.result == true)
            {
                string id = Convert.ToString(gridView3.GetFocusedRowCellValue("ID"));
                string GOrder_id = Convert.ToString(gridView3.GetFocusedRowCellValue("GOrder_id"));

                dbtools.execcmd("Update GetirYemek_Order Set GOrder_status = '500' Where ID = '" + id + "' update cst_Recete_Satis Set Rsat_GetirOrderID = '" + GOrder_id + "', Rsat_GetirDurum = '500' Where Rsat_Fisno = '" + Fisno + "'");



                string siparisFisi = Product.Tables[0].Rows[0]["GProducts_SiparisFis"].ToString();

                //foreach (DataRow item in Product.Tables[0].Rows)
                //{
                //    if (siparisFisi.Length< item["GProducts_SiparisFis"].ToString().Length)
                //    {
                //        siparisFisi = item["GProducts_SiparisFis"].ToString();
                //    }
                //}


                Siparis_Gonder(false, Fisno, siparisFisi, GetirYemek_Order_ID);

            }

            //OnlineOdemeKapat(dsOrder, Fisno, Joker);
            //SiparisKabulEt();
            //dbtools.execcmd("Update Cst_Recete_Satis Set Rsat_YSDurum = '" + IntegrationWebService1.OrderStates.Accepted + "', Rsat_YSOrderID = '" + lbl_OrderID.Text + "'  Where Rsat_Fisno = '" + Fis + "'");
            //timer1.Enabled = true;
            //this.Visible = false;
            //notifyIcon1.Visible = true;
            //Temizle(this);
        }



        // dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr='0' where Rsat_Fisno='" + Fis + "'");
        private void Siparis_Gonder(bool Mars, int Fis, string siparisFisi = "", string GetirYemek_Order_ID = "")
        {
            FisPr pr = new FisPr();

            string sonucSiparis = pr.newSiparisPr(Fis, Mars, 0);
            // string sonucSiparis = pr.YS_SiparisPr(Fis, Mars, 0);
            pr = new FisPr();

            dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr='0' where Rsat_Fisno='" + Fis + "'");

            string sonucPaket = pr.PaketPr(Fisno, " * * * GETİRYEMEK PAKET FİŞİ * * * ", siparisFisi, GetirYemek_Order_ID);

            if (sonucPaket == "OK") // sonucSiparis == "OK" || 
            {
                dbtools.execcmd("update Cst_Recete_Satis set Rsat_SiparisPr = 1 where Rsat_Fisno = '" + Fis + "' ");
            }


            //this.Close();
        }

        public void Kaydet(string ID)
        {
            DataTable dt = dbtools.SelectTable(@"Select * From GetirYemek_Order Where ID = '" + ID + "'");
            DataSet dsCari = new DataSet();
            dsCari = dt.DataSet;

            if (dt.Rows.Count > 0)
            {
                Fisno = FisnoAL();
                SqlConnection con = dbtools.conn;
                if (con.State == ConnectionState.Closed) con.Open();
                try
                {
                    foreach (DataRow cari in dsCari.Tables[0].Rows)
                    {
                        SqlCommand cmd = new SqlCommand("Getir_Cari_Kaydet", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        string[] words = cari["GOrder_Client_name"].ToString().Split(' ');


                        //string tel = cari["GOrder_Client_contactPhoneNumber"].ToString().Trim();
                        string tel = cari["GOrder_Client_clientPhoneNumber"].ToString().Trim() + "\n" + cari["GOrder_Client_contactPhoneNumber"].ToString().Trim();


                        //if (tel.Equals(""))
                        //{
                        //    tel = cari["GOrder_Client_clientPhoneNumber"].ToString().Trim();
                        //}

                        cmd.Parameters.AddWithValue("@Cari_Ad", words[0]);
                        cmd.Parameters.AddWithValue("@Cari_Soyad", words[1]);
                        cmd.Parameters.AddWithValue("@Cari_Tel", tel);

                        //cmd.Parameters.AddWithValue("@Cari_Adres1", cari["GOrder_Client_DeliveryAddress_address"] + " Apt No: " + cari["GOrder_Client_DeliveryAddress_aptNo"] + ", Daire No: " + Convert.ToString(cari["GOrder_Client_DeliveryAddress_floor"]) + ", Kat:" + Convert.ToString(cari["GOrder_Client_DeliveryAddress_doorNo"]) + "\nAdres Açıklama : " + Convert.ToString(cari["GOrder_Client_DeliveryAddress_description"]));

                        cmd.Parameters.AddWithValue("@Cari_Adres1", cari["GOrder_Client_DeliveryAddress_address"] + " Apt No: " + cari["GOrder_Client_DeliveryAddress_aptNo"] + ", Kat: " + Convert.ToString(cari["GOrder_Client_DeliveryAddress_floor"]) + ", Daire No: " + Convert.ToString(cari["GOrder_Client_DeliveryAddress_doorNo"]) + "\nAdres Açıklama : " + Convert.ToString(cari["GOrder_Client_DeliveryAddress_description"]));

                        cmd.Parameters.AddWithValue("@Cari_Tel2", "");
                        cmd.Parameters.AddWithValue("@Cari_Tip", "G");
                        cmd.Parameters.AddWithValue("@Cari_Limit", 0);
                        cmd.Parameters.AddWithValue("@Cari_LimitTutar", 0);
                        cmd.Parameters.AddWithValue("@Cari_Il", "");
                        cmd.Parameters.AddWithValue("@Cari_Ilce", "");
                        cmd.Parameters.AddWithValue("@Cari_Mahalle", "");
                        cmd.Parameters.AddWithValue("@Cari_Aktif", 1);
                        cmd.Parameters.AddWithValue("@Cari_Getir_AddressId", cari["GOrder_Client_DeliveryAddress_id"]);
                        cmd.Parameters.AddWithValue("@Cari_Getir_ID", cari["GOrder_Client_id"]);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dts = new DataTable();
                        da.Fill(dts);

                        if (dts.Rows.Count > 0)
                        {
                            CariKod = Convert.ToString(dts.Rows[0][0]);
                        }
                    }


                    string query = @"select 
                            GetirYemek_Product.ID as TableID, 
                            GProducts_product as id,
                            GProducts_count as quantity,
                            Convert(decimal(18,2),Replace(GProducts_price,',','.')) as Fiyat,
                            Convert(decimal(18,2),Replace(Orders.GOrder_totalDiscountedPrice,',','.')) as Discount,
                            Convert(decimal(18,2),Replace(GProducts_totalPriceWithOption,',','.')) as Total,
                            Orders.GOrder_Client_id as GOrder_Client_id,
                            Orders.GOrder_Client_DeliveryAddress_id as GOrder_Client_DeliveryAddress_id,
                            Orders.GOrder_PaymentMethodText_tr as GOrder_PaymentMethodText_tr,
                            Orders.GOrder_clientNote as GOrder_clientNote,
                            GProducts_note,
                            GProducts_SiparisFis
                            from  GetirYemek_Product 
                            left join GetirYemek_Order as Orders on GetirYemek_Product.GOrder_ID = Orders.ID

                            where Orders.ID = '" + ID + @"'

                            union all

                            select 
                            Options.ID as TableID, 
                            Options.GOption_option as id,
                            GProducts_count as quantity,
                            Convert(decimal(18,2),Replace(Options.GOption_price,',','.')) as Fiyat,
                            0 as Discount,
                            Convert(decimal(18,2),Replace(Options.GOption_price * GProducts_count,',','.')) as Total,
                            Orders.GOrder_Client_id as GOrder_Client_id,
                            Orders.GOrder_Client_DeliveryAddress_id as GOrder_Client_DeliveryAddress_id,
                            Orders.GOrder_PaymentMethodText_tr as GOrder_PaymentMethodText_tr,
                            Orders.GOrder_clientNote as GOrder_clientNote,
                            GProducts_note,
                            GProducts_SiparisFis
                            from  GetirYemek_Option as Options
                            left join GetirYemek_Product as Product on Product.ID = Options.GOptionCategory_ID
                            left join GetirYemek_Order as Orders on Product.GOrder_ID = Orders.ID
                            where Orders.ID = '" + ID + @"'";



                    DataTable dtProduct = dbtools.SelectTable(query);

                    if (dtProduct.Rows.Count > 0)
                    {
                        DataSet dsProduct = new DataSet();
                        dsProduct = dtProduct.DataSet;
                        Urun_Sat(dsProduct, Fisno, Convert.ToDecimal(dtProduct.Rows[0]["Discount"]), ID);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    dbtools.execcmd("Delete From Cst_Recete_Satis Where Rsat_Fisno = '" + Fisno + "'");
                    return;
                }
            }
        }

        private void gridView3_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0)
            {
                return;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            SiparisAl(GetirToken.apitoken);
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0)
            {
                return;
            }

            Getir_Iptal y = new Getir_Iptal();
            y.OrderID = Convert.ToString(gridView3.GetFocusedRowCellValue("GOrder_id"));
            y.ShowDialog();

            getirYenile();
        }

        private void simpleButton7_Click_1(object sender, EventArgs e)
        {
            Getir_Resto a = new Getir_Resto();
            a.ShowDialog();
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            getirYenile();
        }

        private void PaketCallCenter_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.main.paketCallCenter = null;

            timer1.Stop();
            timer1.Enabled = false;
            timer1.Dispose();

            timer2.Stop();
            timer2.Enabled = false;
            timer2.Dispose();

            timerGetirYenile.Stop();
            timerGetirYenile.Enabled = false;
            timerGetirYenile.Dispose();
        }

        int getirYenileSayac = 60;
        private void timerGetirYenile_Tick(object sender, EventArgs e)
        {
            getirYenileSayac--;
            txtGetirYenileSn.Text = getirYenileSayac + "";
            if (getirYenileSayac == 0)
            {
                getirYenile();
                getirYenileSayac = 60;
                // trendyolApi.siparisGetir();
                // trendyolYenile();

                yenile = true;
                trendyolYenile();
            }
        }

        private void btnGetirSil_Click(object sender, EventArgs e)
        {
            try
            {
                string ID = gridView3.GetFocusedRowCellValue("ID").ToString();

                string query = @"
delete from GetirYemek_Order where ID='" + ID + @"' 
delete from GetirYemek_Product where GOrder_ID= '" + ID + @"'
delete from GetirYemek_Option where GOptionCategory_ID=(select top 1 ID from GetirYemek_Product where GOrder_ID='" + ID + @"')";

                dbtools.execcmd(query);
                getirYenile();

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnGetirSil_Click", "", ex);
            }
        }

        public static string MyClass = "PaketCallCenter";

        private void PaketCallCenter_Shown(object sender, EventArgs e)
        {

        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Text.Equals("Getir"))
            {
                getirYenile();
            }
            else if (xtraTabControl1.SelectedTabPage.Text.Equals("Açık Paketler"))
            {
                gridyenile();
            }
        }

        public void gridviewCountYaz(GridView grid)
        {
            if (grid.Columns.Count > 0)
            {
                grid.Columns[0].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                grid.Columns[0].SummaryItem.FieldName = grid.Columns[0].FieldName;
                grid.Columns[0].SummaryItem.DisplayFormat = "{0:n0}";
                grid.UpdateTotalSummary();
            }
        }

        public void gridviewToplamYaz(GridView grid, string fieldName)
        {
            if (grid.Columns.Count > 0)
            {
                grid.Columns[fieldName].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grid.Columns[fieldName].SummaryItem.FieldName = fieldName;
                grid.Columns[fieldName].SummaryItem.DisplayFormat = "{0:n2}";
                grid.UpdateTotalSummary();
            }
        }

        bool yenile = true;
        public void trendyolYenile(string filtre = "")
        {
            try
            {
                if (yenile == false) return;
                string icindekiler = "'" + RestoranTip.onayBekliyorKod + "','" + RestoranTip.hazirlaniyorKod + "','" + RestoranTip.hazirlandiKod + "','" + RestoranTip.yolaCiktiKod + "'";
                string query = "SELECT * FROM entegreSiparis where tip='" + RestoranTip.trendyol + "' and durumKod in(" + icindekiler + ") " + filtre;

                DataTable dataTable = dbtools.SelectTableR(query);

                if (dataTable!=null && dataTable.Rows.Count>0)
                {
                    xtraTabControl1.SelectedTabPage = xtraTabPage4;
                }

                gridControlTrendyolSiparis.DataSource = dataTable;
                gridviewCountYaz(gridViewTrendyolSiparis);

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "trendyolYenile", "", ex);
            }
        }

        public DataTable trendyolYenileUrun(string siparisId)
        {
            DataTable dataTable = null;
            try
            {
                string query = "select *,fiyat*adet as toplamFiyat from entegreSiparisUrunler where siparisId='" + siparisId + "'";

                dataTable = dbtools.SelectTableR(query);

                gridControlTrendyolSiparisUrunler.DataSource = dataTable;
                gridviewCountYaz(gridViewTrendyolSiparisUrunler);
                gridviewToplamYaz(gridViewTrendyolSiparisUrunler, "toplamFiyat");

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "trendyolYenileUrun", "", ex);
            }

            return dataTable;
        }



        private void btnTrendyolSiparisYenile_Click(object sender, EventArgs e)
        {
            yenile = true;
            trendyolYenile();
        }
        TrendyolApi trendyolApi = new TrendyolApi();

        CariController cariController = new CariController();
        private void btnTrendyolSiparisOnay_Click(object sender, EventArgs e)
        {
            try
            {

                yenile = true;
                int focusId = gridViewTrendyolSiparis.FocusedRowHandle;
                if (focusId < 0)
                {
                    RHMesaj.MyMessageInformation("Lütfen sipariş seçiniz!");
                    return;
                }

                DataRow seciliSiparis = gridViewTrendyolSiparis.GetDataRow(focusId);
                if (seciliSiparis == null)
                {
                    RHMesaj.MyMessageInformation("Bir hata oldu tekrar dene!");
                    return;
                }

                string siparisId = seciliSiparis["siparisId"].ToString();
                string cariId = seciliSiparis["cariId"].ToString();

                trendyolApi.urunleriEslestir(siparisId);


                bool kontrol = trendyolApi.eslesmeyenUrunVarmi(siparisId);

                if (kontrol)
                {
                    string mesaj = "Eşleşmeyen ürün var!!!"+Environment.NewLine+"Eşleştirmek istiyor musun ?";

                    ConfirmationForm confirmationForm = new ConfirmationForm(mesaj);
                    confirmationForm.ShowDialog();


                    if (confirmationForm.onay)
                    {
                        EntegreMenu trendyolMenu = new EntegreMenu(2);
                        trendyolMenu.ShowDialog();
                        kontrol = trendyolApi.eslesmeyenUrunVarmi(siparisId);
                        if (kontrol)
                        {
                            RHMesaj.alertMesajSagUst("TRENDYOL", "EŞLEŞMEYEN ÜRÜN VAR", 5);
                        }
                    }
                    else
                    {
                        kontrol = true;

                    }

                }

                //if (kontrol) return; // 12.05.2023 de yorum satırı yapıldı

                cariController.kaydetBySiparisId(siparisId, cariId);

                string masaNo = dbtools.DegerGetir("select top 1 Masa_No from Pos_Masa where Masa_Depart = '" + Departman.Dep_Kodu + "' and ISNULL(Masa_Paket,0) = 1 and Masa_Durum = 0 order by Masa_No");

                if (masaNo.Equals(""))
                {
                    RHMesaj.MyMessageInformation("Boş Paket Masanız Bulunmamaktadır.");
                    return;
                }

                GenelModel model = trendyolApi.siparisAccept1(siparisId);
               
                //trendyolYenileUrun(siparisId);

                if (model.success == false)
                {
                    RHMesaj.MyMessageInformation("Bir hata oldu " + model.mesaj);
                }
                else
                {
                    // fisno ve masaNo oluştur . artık eşleşen ürünleri . trendyol fiyattaki gibi satıcaz
                    // burada kaldım


                    DataTable seciliSiparisUrunler = trendyolYenileUrun(siparisId);
                    if (seciliSiparisUrunler != null && seciliSiparisUrunler.Rows.Count > 0)
                    {
                        SatisController satis = new SatisController();
                        satis.kaydet(seciliSiparis, masaNo);
                        // satis.kaydet(seciliSiparis,seciliSiparisUrunler);
                    }


                    RHMesaj.alertMesaj("Sipariş Kabul edildi.");
                }


                trendyolYenile();
                gridViewTrendyolSiparis.FocusedRowHandle = focusId;

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnTrendyolSiparisOnay_Click", "", ex);
            }
        }





        private void btnTrendyolSiparisHazirlandi_Click(object sender, EventArgs e)
        {
            try
            {
                string siparisId = gridViewTrendyolSiparis.GetFocusedRowCellValue("siparisId").ToString();
                string fisno = gridViewTrendyolSiparis.GetFocusedRowCellValue("fisno").ToString();
                TrendyolController trendyolController = new TrendyolController(siparisId, fisno);
                trendyolController.hazirlandi();

                trendyolYenile();

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnTrendyolSiparisHazirlandi_Click", "", ex);
            }
        }

        private void btnTrendyolSiparisYolaCikti_Click(object sender, EventArgs e)
        {
            try
            {
                string siparisId = gridViewTrendyolSiparis.GetFocusedRowCellValue("siparisId").ToString();
                string fisno = gridViewTrendyolSiparis.GetFocusedRowCellValue("fisno").ToString();
                TrendyolController trendyolController = new TrendyolController(siparisId, fisno);
                trendyolController.yolaCikti();
                trendyolYenile();

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnTrendyolSiparisYolaCikti_Click", "", ex);
            }
        }

        private void btnTrendyolSiparisTeslimEdildi_Click(object sender, EventArgs e)
        {
            try
            {
                string siparisId = gridViewTrendyolSiparis.GetFocusedRowCellValue("siparisId").ToString();
                string fisno = gridViewTrendyolSiparis.GetFocusedRowCellValue("fisno").ToString();
                TrendyolController trendyolController = new TrendyolController(siparisId, fisno);
                trendyolController.teslimEdildi();

                trendyolYenile();

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnTrendyolSiparisTeslimEdildi_Click", "", ex);
            }
        }
        private void btnTrendyolSiparisIptal_Click(object sender, EventArgs e)
        {
            try
            {
                string siparisId = gridViewTrendyolSiparis.GetFocusedRowCellValue("siparisId").ToString();
                TrendyoliptalForm trendyoliptalForm = new TrendyoliptalForm();
                trendyoliptalForm.ShowDialog();
                string iptalId = trendyoliptalForm.cancelId;


                DataTable dataPaket = dbtools.SelectTableR("select packageItemId from entegreSiparisUrunler where siparisId='" + siparisId + "'");

                List<string> urunPaketIdleri = new List<string>();

                foreach (DataRow item in dataPaket.Rows)
                {
                    List<TrendyolUrunPaketIdList> model = JsonConvert.DeserializeObject<List<TrendyolUrunPaketIdList>>(item["packageItemId"].ToString());
                    foreach (var aa in model)
                    {
                        urunPaketIdleri.Add(aa.packageItemId);

                    }
                }


                if (!iptalId.Equals(""))
                {
                    GenelModel model = trendyolApi.siparisCancel(siparisId, Convert.ToInt32(iptalId), urunPaketIdleri);

                    if (model.success == false)
                    {
                        RHMesaj.MyMessageInformation("Bir hata oldu " + model.mesaj);
                    }
                    else
                    {
                        dbtools.execcmdR("update Cst_Recete_Satis set Rsat_EntegreDurumKod='" + RestoranTip.iptalEdildiKod + "' where Rsat_EntegreId='" + siparisId + "'");

                        RHMesaj.MyMessageInformation("Sipariş İptal Edildi.");
                    }

                    trendyolYenile();


                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnTrendyolSiparisIptal_Click", "", ex);
            }


        }

        private void btnTrendyolSiparisSil_Click(object sender, EventArgs e)
        {
            try
            {
                string siparisId = gridViewTrendyolSiparis.GetFocusedRowCellValue("siparisId").ToString();
                dbtools.execcmdR("delete from entegreSiparis where siparisId='" + siparisId + "'");
                dbtools.execcmdR("delete from entegreSiparisUrunler where siparisId='" + siparisId + "'");
                trendyolYenile();
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnTrendyolSiparisSil_Click", "", ex);
            }
        }

        private void btnTrendyolSiparisOnaylananListele_Click(object sender, EventArgs e)
        {
            try
            {
                yenile = false;

                string basTar = Convert.ToDateTime(dateTarih1.EditValue.ToString()).ToString("yyyy-MM-dd");
                string bittar = Convert.ToDateTime(dateTarih2.EditValue.ToString()).AddDays(1).ToString("yyyy-MM-dd");

                string query = "SELECT * FROM entegreSiparis where tip='" + RestoranTip.trendyol + "' and durumKod='" + RestoranTip.teslimEdildiKod + "' and guncellemeTarih between '" + basTar + "' and '" + bittar + "'";

                DataTable dataTable = dbtools.SelectTableR(query);

                gridControlTrendyolSiparis.DataSource = dataTable;
                gridviewCountYaz(gridViewTrendyolSiparis);

                RHMesaj.alertMesajSagUst(basTar + " / " + bittar, "Tarihli Onaylanan Listesi", 5);
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnTrendyolSiparisOnaylananListele_Click", "", ex);
            }
        }

        private void gridViewTrendyolSiparis_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridViewTrendyolSiparis.FocusedRowHandle < 0)
            {
                return;
            }

            string siparisId = gridViewTrendyolSiparis.GetFocusedRowCellValue("siparisId").ToString();
            trendyolYenileUrun(siparisId);
        }

        private void btnTrendyolSiparisIptalleriListele_Click(object sender, EventArgs e)
        {
            try
            {
                yenile = false;
                string basTar = Convert.ToDateTime(dateTarih1.EditValue.ToString()).ToString("yyyy-MM-dd");
                string bittar = Convert.ToDateTime(dateTarih2.EditValue.ToString()).AddDays(1).ToString("yyyy-MM-dd");

                string query = "SELECT * FROM entegreSiparis where tip='" + RestoranTip.trendyol + "' and durumKod='" + RestoranTip.iptalEdildiKod + "' and guncellemeTarih between '" + basTar + "' and '" + bittar + "'";

                DataTable dataTable = dbtools.SelectTableR(query);

                gridControlTrendyolSiparis.DataSource = dataTable;
                gridviewCountYaz(gridViewTrendyolSiparis);

                RHMesaj.alertMesajSagUst(basTar + " / " + bittar, "Tarihli İptal Listesi", 5);

            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "btnTrendyolSiparisOnaylananListele_Click", "", ex);
            }
        }

        public void loadingAc()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
        }

        public void loadingKapat()
        {
            SplashScreenManager.CloseForm(false);
        }

        private void btnTrendyolSiparisInternettenCek_Click(object sender, EventArgs e)
        {
            try
            {
                loadingAc();
                trendyolApi.siparisGetir();
                yenile = true;
                trendyolYenile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            loadingKapat();

        }

        private void btnTrendyolMenu_Click(object sender, EventArgs e)
        {
            EntegreMenu trendyolMenu = new EntegreMenu(2);
            trendyolMenu.ShowDialog();
        }

        private void gridViewTrendyolSiparis_KeyDown(object sender, KeyEventArgs e)
        {
            hucreKopyala(gridControlTrendyolSiparis, e);
        }

        public void hucreKopyala(GridControl gridControl, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    GridView view = gridControl.FocusedView as GridView;
                    Clipboard.SetText(view.GetFocusedDisplayText());
                    e.Handled = true;
                    RHMesaj.alertMesaj("\"" + view.GetFocusedDisplayText() + "\" KOPYALANDI");
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "hucreKopyala", "", ex);
            }
        }

        private void gridViewTrendyolSiparisUrunler_KeyDown(object sender, KeyEventArgs e)
        {
            hucreKopyala(gridControlTrendyolSiparisUrunler, e);
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                GridView View = sender as GridView;

                string kapatma = View.GetRowCellDisplayText(e.RowHandle, View.Columns["YSDurum"]);

                if (kapatma != null && kapatma == "PAKET" && e.Column.FieldName == "YSDurum")
                {
                    e.Appearance.BackColor = Color.Turquoise;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (kapatma != null && kapatma == "TRENDYOL" && e.Column.FieldName == "YSDurum")
                {
                    e.Appearance.BackColor = Color.Orange;
                    e.Appearance.ForeColor = Color.Black;
                }
            }

        }
    }
}