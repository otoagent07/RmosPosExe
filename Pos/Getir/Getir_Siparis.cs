using Pos.Class;
using Pos.Getir.Class;
using System;
using System.Collections.Generic;

namespace Pos.Getir
{
    public partial class Getir_Siparis : DevExpress.XtraEditors.XtraForm
    {
        public Getir_Siparis()
        {
            InitializeComponent();
        }

        GetirApi getirApi = new GetirApi();

        private void Getir_Siparis_Load(object sender, EventArgs e)
        {

        }
        private void SiparisAl(string Token)
        {
            gridControl1.DataSource = null;
            List<GetirOrderResponse.Root> getirOrderResponse = getirApi.postRequestS<List<GetirOrderResponse.Root>>(GetirStatik.requestOrder, Token);
            

            foreach (var item in getirOrderResponse)
            {
                int Deger = Convert.ToInt32(dbtools.DegerGetir("Select Count(*) From GetirYemek_Order Where GOrder_id = '" + item.id + "'"));
                if (Deger == 0)
                {                 
                    string OrderID = dbtools.DegerGetir(@"INSERT INTO [dbo].[GetirYemek_Order]
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
                           ,[GOrder_Client_DeliveryAddress_id]
                           ,[GOrder_Client_DeliveryAddress_address]
                           ,[GOrder_Client_DeliveryAddress_aptNo]
                           ,[GOrder_Client_DeliveryAddress_floor]
                           ,[GOrder_Client_DeliveryAddress_doorNo]
                           ,[GOrder_Client_DeliveryAddress_description]
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
                           VALUES('" + item.id + "','" + item.status + "','" + item.isScheduled + "','" + item.confirmationId + "','" + item.clientNote + "','" + item.totalPrice + @"','" + item.checkoutDate + "','" + (item.scheduledDate == null ? DateTime.Now : item.scheduledDate) + "','" + item.deliveryType + "','" + item.doNotKnock + "','" + item.isEcoFriendly + "','" + item.paymentMethod + @"','" + (item.totalDiscountedPrice == null ? 0 : item.totalDiscountedPrice) + @"','" + item.client.id + @"','" + item.client.name + @"','" + item.client.clientPhoneNumber + @"','" + item.client.contactPhoneNumber + @"','" + (item.client.deliveryAddress.id == null ? "" : item.client.deliveryAddress.id) + @"','" + (item.client.deliveryAddress.address == null ? "" : item.client.deliveryAddress.address) + @"','" + (item.client.deliveryAddress.aptNo == null ? "" : item.client.deliveryAddress.aptNo) + @"','" + (item.client.deliveryAddress.floor == null ? "" : item.client.deliveryAddress.floor) + @"','" + (item.client.deliveryAddress.doorNo == null ? "" : item.client.deliveryAddress.doorNo) + @"','" + (item.client.deliveryAddress.description == null ? "" : item.client.deliveryAddress.description) + @"','" + item.client.location.lat + @"','" + item.client.location.lon + @"','" + item.courier.id + @"','" + item.courier.status + @"','" + item.courier.name + @"','" + item.courier.location.lat + @"','" + item.courier.location.lon + @"','" + item.restaurant.id + @"','" + item.paymentMethodText.en + @"','" + item.paymentMethodText.tr + @"') 
                             select SCOPE_IDENTITY()");

                    foreach (var product in item.products)
                    {
                        string ProductID = dbtools.DegerGetir(@"INSERT INTO [dbo].[GetirYemek_Product]
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
                       ,[GProducts_DisplayInfo_Option_en])
                        VALUES('" + OrderID + @"','" + product.id + @"','" + product.imageURL + @"','" + product.wideImageURL + @"','" + product.product + @"','" + product.count + @"','" + product.chainProduct + @"','" + product.name.tr + @"','" + product.name.en + @"','" + product.price + @"','" + product.optionPrice + @"','" + product.totalPrice + @"','" + product.totalOptionPrice + @"','" + product.totalPriceWithOption + @"','" + product.displayInfo.title.tr + @"','" + product.displayInfo.title.en + @"','" + "" + @"','" + "" + @"'
                        ) Select SCOPE_IDENTITY() ");


                        //foreach (var optionCategory in product.optionCategories)
                        //{
                        //    string optionCategoryID = dbtools.DegerGetir(@"INSERT INTO [dbo].[GetirYemek_OptionCategory]
                        //   ([GProduct_ID]
                        //   ,[GOptionCategory_optionCategory]
                        //   ,[GOptionCategory_name_tr]
                        //   ,[GOptionCategory_name_en])
                        //    VALUES('" + ProductID + @"','" + optionCategory.optionCategory + @"','" + optionCategory.name.tr + @"','" + optionCategory.name.en + @"') Select SCOPE_IDENTITY() ");

                        foreach (var optionCategory in product.optionCategories)
                        {
                            foreach (var options in optionCategory.options)
                            {
                                string optionsID = dbtools.DegerGetir(@"
                                INSERT INTO [dbo].[GetirYemek_Option]
                                           ([GOptionCategory_ID]
                                           ,[GOption_option]
                                           ,[GOption_name_tr]
                                           ,[GOption_name_en]
                                           ,[GOption_price]
                                           ,[GOption_tr]
                                           ,[GOption_en])
                                     VALUES('" + ProductID + @"','" + options.option + @"','" + options.name.tr + @"','" + options.name.en + @"','" + options.price + @"','" + options.tr + @"','" + options.en + @"') Select SCOPE_IDENTITY()");
                            }
                            break;
                        }
                    }
                    //}
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SiparisAl(GetirToken.apitoken);
        }
    }
}