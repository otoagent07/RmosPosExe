using DevExpress.XtraEditors;
using Newtonsoft.Json;
using Pos.Class;
using Pos.Getir.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Pos.Getir
{
    public partial class Getir_Panel : DevExpress.XtraEditors.XtraForm
    {
        public Getir_Panel()
        {
            InitializeComponent();
            //xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
        }

        private void Getir_Panel_Load(object sender, EventArgs e)
        {
            

            DataTable dt = dbtools.SelectTable("Select Rec_Genelkod,Rec_Ad from Cst_Recete");
            repositoryItemLookUpEdit_AnaMenu.DataSource = dt;
            repositoryItemLookUpEdit_AnaMenu.ValueMember = "Rec_Genelkod";
            repositoryItemLookUpEdit_AnaMenu.DisplayMember = "Rec_Ad";

            repositoryItemCheckedComboBoxEdit_AnaMenu.DataSource = dt;
            repositoryItemCheckedComboBoxEdit_AnaMenu.ValueMember = "Rec_Genelkod";
            repositoryItemCheckedComboBoxEdit_AnaMenu.DisplayMember = "Rec_Ad";

            repositoryItemSearchLookUpEdit_AnaMenu.DataSource = dt;
            repositoryItemSearchLookUpEdit_AnaMenu.ValueMember = "Rec_Genelkod";
            repositoryItemSearchLookUpEdit_AnaMenu.DisplayMember = "Rec_Ad";

            repositoryItemLookUpEdit_AltRecete.DataSource = dt;
            repositoryItemLookUpEdit_AltRecete.ValueMember = "Rec_Genelkod";
            repositoryItemLookUpEdit_AltRecete.DisplayMember = "Rec_Ad";

            repositoryItemCheckedComboBoxEdit_AltRecete.DataSource = dt;
            repositoryItemCheckedComboBoxEdit_AltRecete.ValueMember = "Rec_Genelkod";
            repositoryItemCheckedComboBoxEdit_AltRecete.DisplayMember = "Rec_Ad";

            repositoryItemSearchLookUpEdit_AltRecete.DataSource = dt;
            repositoryItemSearchLookUpEdit_AltRecete.ValueMember = "Rec_Genelkod";
            repositoryItemSearchLookUpEdit_AltRecete.DisplayMember = "Rec_Ad";


            repLook_Odeme.DataSource = dbtools.SelectTable("Select Pkod_Kod,Pkod_Ad From Pos_Kodlar Where Pkod_Sinif = 11");
            repLook_Odeme.ValueMember = "Pkod_Kod";
            repLook_Odeme.DisplayMember = "Pkod_Ad";

            //xtraTabControl1.SelectedTabPage = tab_Restoran;

            txt_appSecretKey.Text = Departman.Kodlar_Getir_appSecretKey;
            txt_restaurantSecretKey.Text = Departman.Kodlar_Getir_restaurantSecretKey;


            RestoGetir();
        }


        public DevExpress.XtraNavBar.NavBarItem nBar;

        public void nBar_Kontrol(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            nBar = (DevExpress.XtraNavBar.NavBarItem)sender;

            if (nBar.Tag == "1")
            {
                xtraTabControl1.SelectedTabPage = tab_Restoran;

                txt_appSecretKey.Text = Departman.Kodlar_Getir_appSecretKey;
                txt_restaurantSecretKey.Text = Departman.Kodlar_Getir_appSecretKey;

                RestoGetir();
            }
            else if (nBar.Tag == "2")
            {
                xtraTabControl1.SelectedTabPage = tab_Menu;
                anaReceteYenile();
            }
            else if (nBar.Tag == "3")
            {
                xtraTabControl1.SelectedTabPage = tab_Odeme;
                PaymentListele();
            }
        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {

            if (Convert.ToInt32(dbtools.DegerGetir("Select Count(*) From GetirYemek_Restaurant Where GetirResto_Departman = '" + Departman.Dep_Kodu + "'")) == 0)
            {
                dbtools.execcmd(@"INSERT INTO [dbo].[GetirYemek_Restaurant]
                               ([GetirResto_Departman]
                               ,[GetirResto_restaurantid]
                               ,[GetirResto_name]
                               ,[GetirResto_averagePreparationTime]
                               ,[GetirResto_status]
                               ,[GetirResto_isCourierAvailable]
                               ,[GetirResto_tokenKey]) 
                               VALUES('" + Departman.Dep_Kodu + @"',
                               '" + txt_RestoranID.Text + "','" + txt_name.Text + "','" + txt_averagePreparationTime.Text + "','"
                                  + txt_status.Text + "','" + chk_isCourierAvailable.Checked + "','" + mem_Token.Text + "' )");
            }
            else
            {
                dbtools.execcmd(@"UPDATE [dbo].[GetirYemek_Restaurant]
                                   SET [GetirResto_Departman] = '" + Departman.Dep_Kodu + @"'
                                      ,[GetirResto_restaurantid] = '" + txt_RestoranID.Text + @"'
                                      ,[GetirResto_name] = '" + txt_name.Text + @"'
                                      ,[GetirResto_averagePreparationTime] = '" + txt_averagePreparationTime.Text + @"'
                                      ,[GetirResto_status] = '" + txt_status.Text + @"'
                                      ,[GetirResto_isCourierAvailable] = '" + chk_isCourierAvailable.Checked + @"'
                                      ,[GetirResto_tokenKey] = '" + mem_Token.Text + @"' 
                                 WHERE GetirResto_Departman = '" + Departman.Dep_Kodu + @"' ");
            }

            GetirToken.apitoken = mem_Token.Text;
        }

        GetirApi api = new GetirApi();

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            RestoGetir();
        }

        private void RestoGetir()
        {
            GetirLoginResponse loginResponse = api.getToken(Departman.Kodlar_Getir_appSecretKey, Departman.Kodlar_Getir_restaurantSecretKey);
            if (loginResponse == null)
            {
                return;
            }
            mem_Token.Text = loginResponse.token;
            txt_RestoranID.Text = loginResponse.restaurantId;
            GetirToken.apitoken = mem_Token.Text;

            GetirRestaurantResponse getirRestaurantResponse = api.getRestaurant(loginResponse.token);
            if (getirRestaurantResponse != null)
            {
                txt_averagePreparationTime.Text = getirRestaurantResponse.averagePreparationTime.ToString();
                txt_status.Text = getirRestaurantResponse.status.ToString();
                chk_isCourierAvailable.Checked = getirRestaurantResponse.isCourierAvailable;
                txt_name.Text = getirRestaurantResponse.name;
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public string MyClass = "Getir_Panel";
        public string replaceTirnak(string text)
        {
            try
            {
                return text.Replace("'", "''");

            }
            catch (Exception ex)
            {
                return text;
            }
        }
        public void menuGetir()
        {
            try
            {
                string Json = JsonConvert.SerializeObject(api.getRestaurantMenu(GetirToken.apitoken), Formatting.Indented);
                GetirMenuResponse.Root getirMenuResponse = JsonConvert.DeserializeObject<GetirMenuResponse.Root>(Json);

                DataTable dtYoksay = dbtools.SelectTableR("select Product_id from GetirYemek_Menu_Product where product_yoksay='1'");

                dbtools.execcmd("Delete From GetirYemek_Menu_ProductCategory Delete From GetirYemek_Menu_Product Delete From GetirYemek_Menu_OptionCategory Delete From GetirYemek_Menu_Option  ");

                foreach (var productCategories in getirMenuResponse.productCategories)
                {
                    string query = @"INSERT INTO [dbo].[GetirYemek_Menu_ProductCategory]
                                   ([ProductCategory_id]
                                   ,[ProductCategory_name]
                                   ,[ProductCategory_restaurant]
                                   ,[ProductCategory_isApproved]
                                   ,[ProductCategory_weight]
                                   ,[ProductCategory_status]
                                   ,[ProductCategory_chainProductCategory])
                                   VALUES('" + productCategories.id + "','" + replaceTirnak( productCategories.name.tr) + "','" + replaceTirnak(productCategories.restaurant) + "','" + productCategories.isApproved + "','" + productCategories.weight + "','" + productCategories.status + "','" + replaceTirnak( productCategories.chainProductCategory) + "')";
                    dbtools.execcmd(query);

                    foreach (var products in productCategories.products)
                    {
                        dbtools.execcmd(@"INSERT INTO [dbo].[GetirYemek_Menu_Product]
                                   ([Product_id]
                                   ,[Product_productCategory]
                                   ,[Product_name]
                                   ,[Product_description_tr]
                                   ,[Product_price]
                                   ,[Product_stuckPrice]
                                   ,[Product_weight]
                                   ,[Product_status]
                                   ,[Product_isApproved]
                                   ,[Product_imageURL]
                                   ,[Product_wideImageURL]
                                   ,[Product_chainProduct])
                                   VALUES('" + products.id + "','" + replaceTirnak( products.productCategory) + "','" + replaceTirnak(products.name.tr)
                                        + "','" + replaceTirnak(products.description.tr) + "','" + products.price.ToString().Replace(",", ".") + "','" + products.struckPrice.ToString().Replace(",", ".") + "','" + products.weight + "','" + products.status + "','" + products.isApproved + "','" + replaceTirnak(products.imageURL) + "','" + replaceTirnak(products.wideImageURL) + "','" + replaceTirnak(products.chainProduct) + "')");


                        foreach (var optionCategories in products.optionCategories)
                        {
                            dbtools.execcmd(@"INSERT INTO [dbo].[GetirYemek_Menu_OptionCategory]
                           ([Product_ID]
                           ,[OptionCategory_id]
                           ,[OptionCategory_name]
                           ,[OptionCategory_minCount]
                           ,[OptionCategory_maxCount]
                           ,[OptionCategory_weight]
                           ,[OptionCategory_status])
                         VALUES('" + products.id + "','" + optionCategories.id + "','" + replaceTirnak(optionCategories.name.tr) + "', '" + optionCategories.minCount
                             + "','" + optionCategories.maxCount + "','" + optionCategories.weight + "','" + optionCategories.status + "')");


                            foreach (var options in optionCategories.options)
                            {
                                //    dbtools.execcmd(@"INSERT INTO [dbo].[GetirYemek_Menu_Option]
                                //           ([OptionCategory_ID]
                                //           ,[Option_id]
                                //           ,[Option_name]
                                //           ,[Option_type]
                                //           ,[Option_price]
                                //           ,[Option_weight]
                                //           ,[Option_status]
                                //           ,[Option_product])
                                // VALUES('" + optionCategories.id + "','" + options.id + "','" + options.name.tr + "','" + options.type
                                //+ "','" + options.price + "','" + options.weight + "','" + options.status + "','" + options.product + "')");

                                //string opsiyonId = options.product;
                                //if (opsiyonId==null)
                                //{
                                //    opsiyonId = options.id;
                                //}

                                string opsiyonId = options.id;

                                query = @"INSERT INTO [dbo].[GetirYemek_Menu_Option]
                                   ([OptionCategory_ID]
                                   ,[Option_id]
                                   ,[Option_name]
                                   ,[Option_type]
                                   ,[Option_price]
                                   ,[Option_weight]
                                   ,[Option_status]
                                   ,[Option_product])
                         VALUES('" + optionCategories.id + "','" + opsiyonId + "','" + replaceTirnak( options.name.tr )+ "','" + options.type
                        + "'," + options.price.ToString().Replace(",", ".") + ",'" + options.weight + "','" + options.status + "','" + replaceTirnak(options.product) + "')";
                                dbtools.execcmd(query);


                            }
                        }

                    }
                }

                foreach (DataRow item in dtYoksay.Rows)
                {
                    // select Product_id from GetirYemek_Menu_Product where product_yoksay='1'
                    string query = "update GetirYemek_Menu_Product set product_yoksay='1' where Product_id='" + item["Product_id"] + "'";
                    dbtools.execcmd(query);

                }
                anaReceteYenile();
            }
            catch(Exception ex)
            {
                RHMesaj.MyMessageError(MyClass, "menuGetir", "",ex);
            }
            
        }
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            menuGetir();
        }

        private void anaReceteYenile()
        {

            //string query = @"select 
            //    ProductCategory.ProductCategory_name as AnaGrupAdi,
            //    Product.Product_id as GetirMenu_id,
            //    Product.Product_name as GetirMenu_name,
            //    Product.Product_Price as GetirMenu_price,
            //    Rec_Genelkod as Rec_Genelkod,
            //    Rec_Fiyat as Rec_Fiyat
            //    from GetirYemek_Menu_ProductCategory as ProductCategory
            //    left join GetirYemek_Menu_Product as Product on ProductCategory.ProductCategory_id = Product.Product_productCategory                
            //    left join Cst_Recete on Product.Product_id = Rec_GetirMenuID 

            //    ";
            string query = @"select distinct 
                ProductCategory.ProductCategory_name as AnaGrupAdi,
                Product.Product_id as GetirMenu_id,
                Product.Product_name as GetirMenu_name,
                Product.Product_Price as GetirMenu_price,
                Rec_Genelkod as Rec_Genelkod,
                Rec_Fiyat as Rec_Fiyat,
                isnull(Product_yoksay,0) as Product_yoksay,
                Product.ID
                from GetirYemek_Menu_ProductCategory as ProductCategory
                left join GetirYemek_Menu_Product as Product on ProductCategory.ProductCategory_id = Product.Product_productCategory                
                left join Cst_Recete as recete on  recete.Rec_GetirMenuID like '%'+Product.Product_id+'%'

                ";

            DataTable dt = dbtools.SelectTable(query);

            gridControl8.DataSource = dt;

        }

        private void MenuKaydet()
        {
            try
            {
                for (int i = 0; i < gridView8.RowCount; i++)
                {
                    string Rec_Genelkod = Convert.ToString(gridView8.GetRowCellValue(i, "Rec_Genelkod"));
                    if (Rec_Genelkod.Length > 0)
                    {

                        string GetirMenu_id = Convert.ToString(gridView8.GetRowCellValue(i, "GetirMenu_id"));

                        dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = '" + GetirMenu_id + "' Where Rec_Genelkod = '" + Rec_Genelkod + "'");

                        for (int j = 0; j < gridView1.RowCount; j++)
                        {
                            if (Convert.ToString(gridView1.GetRowCellValue(j, "Rec_Genelkod")).Length > 0)
                            {
                                dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = '" + Convert.ToString(gridView1.GetRowCellValue(j, "Option_id")) + "' Where Rec_Genelkod = '" + Convert.ToString(gridView1.GetRowCellValue(j, "Rec_Genelkod")) + "'");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata : " + ex.Message);
                return;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            MenuKaydet();
            anaReceteYenile();
        }

        private void PaymentListele()
        {
            gdc_Odeme.DataSource = dbtools.SelectTable(@"select 
                        GetirPayment_id, GetirPayment_name, Pkod_Ad,Pkod_Kod
                        from GetirYemek_Payment
                        left join Pos_Kodlar on Pkod_Sinif = 11 and Pkod_Getir_PaymentID = GetirPayment_id
                        ");
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            List<GetirPaymentResponse.Root> getirPaymentResponse = api.getPayment(GetirToken.apitoken);

            dbtools.execcmd("Delete From GetirYemek_Payment");

            foreach (var item in getirPaymentResponse)
            {
                dbtools.execcmd(@"INSERT INTO [dbo].[GetirYemek_Payment]
                           ([GetirPayment_id]
                           ,[GetirPayment_name]
                           ,[GetirPayment_icon]
                           ,[GetirPayment_paymentGroup]
                           ,[GetirPayment_deliveryTypes]
                           ,[GetirPayment_type])                   
                            VALUES('" + item.id + "','" + item.name.tr + "','" + item.icon
                            + "','" + item.paymentGroup + "','" + item.deliveryTypes[0] + "','" + item.type + "')");

            }
            PaymentListele();
        }

        private void OdemeKaydet()
        {
            try
            {
                for (int i = 0; i < gdv_Odeme.RowCount; i++)
                {
                    if (Convert.ToString(gdv_Odeme.GetRowCellValue(i, "Pkod_Kod")).Length > 0)
                    {
                        dbtools.execcmd("Update Pos_Kodlar set Pkod_Getir_PaymentID = '" + Convert.ToString(gdv_Odeme.GetRowCellValue(i, "GetirPayment_id")) + "' Where Pkod_Kod = '" + Convert.ToString(gdv_Odeme.GetRowCellValue(i, "Pkod_Kod")) + "' and Pkod_Sinif = 11");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata : " + ex.Message);
                return;
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            OdemeKaydet();
            PaymentListele();
        }

        private void OdemeSil()
        {
            if (Convert.ToString(gdv_Odeme.GetFocusedRowCellValue("Pkod_Getir_PaymentID")) != "")
            {
                dbtools.execcmd("Pos_Kodlar set Pkod_Getir_PaymentID = '' Where Pkod_Getir_PaymentID = '" + Convert.ToString(gdv_Odeme.GetFocusedRowCellValue("Pkod_Getir_PaymentID")) + "' and Pkod_Sinif = 11 ");
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            OdemeSil();
            PaymentListele();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReceteSil()
        {
            if (gridView8.FocusedRowHandle < 0) return;
            dbtools.execcmd("Update Cst_Recete Set Rec_GetirMenuID = NULL where Rec_Genelkod = '" + Convert.ToString(gridView8.GetFocusedRowCellValue("Rec_Genelkod")) + "'");
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            ReceteSil();
            anaReceteYenile();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void repositoryItemLookUpEdit1_Leave_1(object sender, EventArgs e)
        {
            //anaReceteKaydet(sender);




        }

      

        public void altReceteYenile()
        {
            if (gridView8.FocusedRowHandle < 0)
            {
                return;
            }

            gridControl1.DataSource = null;

            string query = @"select distinct
             Options.ID            
            ,Product.Product_id as GetirMenu_id
            ,Product.Product_name as GetirMenu_name
            ,OptionCategory.OptionCategory_name as AnaGrupAdi
            ,Options.Option_id as Option_id
            ,Options.Option_name as Option_name
            ,Options.Option_price as Option_price
            ,Rec_Genelkod as Rec_Genelkod,
            Rec_Fiyat as Rec_Fiyat
            from GetirYemek_Menu_Option as Options
            left join GetirYemek_Menu_OptionCategory as OptionCategory on Options.optioncategory_ID = OptionCategory.optioncategory_id
            left join GetirYemek_Menu_Product as Product on OptionCategory.Product_ID = Product.Product_id
            left join GetirYemek_Menu_ProductCategory as ProductCategory on ProductCategory.ProductCategory_id = Product.Product_productCategory
            left join Cst_Recete as recete on  recete.Rec_GetirMenuID like '%'+Options.Option_id+'%'
            Where Product.Product_id  = '" + gridView8.GetFocusedRowCellValue("GetirMenu_id") + "'";
            
            //left join Cst_Recete on Rec_GetirMenuID = Options.Option_id

            gridControl1.DataSource = dbtools.SelectTable(query);
        }

        private void repositoryItemLookUpEdit3_Leave(object sender, EventArgs e)
        {
            //altReceteKaydet(sender);


        }

        private void btnAltUrunSil_Click(object sender, EventArgs e)
        {
                
            if (gridView1.FocusedRowHandle < 0)
            {
                RHMesaj.MyMessageInformation("Lütfen Satır Seçiniz !");
                return;
            }


            string Rec_Genelkod = gridView1.GetFocusedRowCellValue("Rec_Genelkod").ToString(); // ID

            if (Rec_Genelkod.Length > 0)
            {

                dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = NULL Where Rec_Genelkod = '" + Rec_Genelkod + "'");
                altReceteYenile();
            }

        }



        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == tab_Menu)
            {
                menuGetir();
            }
        }

        public string getirIdDuzenle(string str) //  ",2,4,3,12,25,2,4,3,6,2,2,2,"
        {
            str = str.Trim().TrimStart(',').TrimEnd(','); // baştaki ve sondaki virgülleri siler
            List<string> uniques = str.Split(',').Distinct().ToList(); // virgüle göre tekrar edenleri teke düşürür
            string newStr = string.Join(",", uniques); // listeyi stringe virgül koyarak birleştirir

            return newStr;
        }
        //public void anaReceteKaydet(object sender)
        //{
        //    string GetirMenu_id = Convert.ToString(gridView8.GetFocusedRowCellValue("GetirMenu_id"));
        //    //string Rec_GenelkodOld = ((sender as LookUpEdit).OldEditValue).ToString();
        //    string Rec_Genelkod = ((sender as LookUpEdit).EditValue).ToString();

        //    string Rec_GetirMenuID = dbtools.DegerGetir("select Rec_GetirMenuID from Cst_Recete Where Rec_Genelkod = '" + Rec_Genelkod + "'");

        //    Rec_GetirMenuID = Rec_GetirMenuID + "," + GetirMenu_id + ",";

        //    Rec_GetirMenuID = getirIdDuzenle(Rec_GetirMenuID);

        //    dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = '" + Rec_GetirMenuID + "' Where Rec_Genelkod = '" + Rec_Genelkod + "'");

        //  //  anaReceteYenile();

        //    if (Convert.ToString(gridView8.GetFocusedRowCellValue("Rec_Genelkod")) != "")
        //    {
        //        DataTable dt = dbtools.SelectTable("Select Rec_Fiyat,Rec_Paket_Tam From Cst_Recete Where Rec_Genelkod = '" + Convert.ToString(gridView8.GetFocusedRowCellValue("Rec_Genelkod")) + "'");

        //        if (dt.Rows.Count > 0)
        //        {
        //            gridView8.SetFocusedRowCellValue("Rec_Fiyat", dt.Rows[0][0]);
        //        }
        //    }

        //    RHMesaj.alertMesaj(Rec_GetirMenuID + " Olarak Güncellendi...", 2);
        //}



        //public void anaReceteKaydet(object sender)
        //{
        //    string GetirMenu_id = Convert.ToString(gridView8.GetFocusedRowCellValue("GetirMenu_id"));
        //    string Rec_GenelkodOld = ((sender as LookUpEdit).OldEditValue).ToString();
        //    string Rec_Genelkod = ((sender as LookUpEdit).EditValue).ToString();

        //    dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = NULL Where Rec_Genelkod = '" + Rec_GenelkodOld + "'");
        //    dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = '" + GetirMenu_id + "' Where Rec_Genelkod = '" + Rec_Genelkod + "'");

        //    anaReceteYenile();

        //    RHMesaj.alertMesaj(Rec_GenelkodOld + " -> " + Rec_Genelkod + " Olarak Güncellendi...", 2);
        //}

        //public void altReceteKaydet(object sender)
        //{
        //    try
        //    {
        //        string Option_id = Convert.ToString(gridView1.GetFocusedRowCellValue("Option_id"));

        //        string Rec_Genelkod = ((sender as LookUpEdit).EditValue).ToString();
        //        string Rec_GenelkodOld = ((sender as LookUpEdit).OldEditValue).ToString();

        //        dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = NULL Where Rec_Genelkod = '" + Rec_GenelkodOld + "'");
        //        dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = '" + Option_id + "' Where Rec_Genelkod = '" + Rec_Genelkod + "'");

        //        //altReceteYenile();

        //        if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")) != "")
        //        {
        //            DataTable dt = dbtools.SelectTable("Select Rec_Fiyat,Rec_Paket_Tam From Cst_Recete Where Rec_Genelkod = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")) + "'");

        //            if (dt.Rows.Count > 0)
        //            {
        //                gridView1.SetFocusedRowCellValue("Rec_Fiyat", dt.Rows[0][0]);
        //            }
        //        }

        //        RHMesaj.alertMesaj(Rec_GenelkodOld + " -> " + Rec_Genelkod + " Olarak Güncellendi...", 2);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

        private void repositoryItemLookUpEdit_AltRecete_EditValueChanged(object sender, EventArgs e)
        {


        }

        private void repositoryItemCheckedComboBoxEdit_AnaMenu_Leave(object sender, EventArgs e)
        {
            string GetirMenu_id = Convert.ToString(gridView8.GetFocusedRowCellValue("GetirMenu_id"));
            //string Rec_GenelkodOld = ((sender as LookUpEdit).OldEditValue).ToString();
            string Rec_Genelkod = ((sender as CheckedComboBoxEdit).EditValue).ToString();

            string Rec_GetirMenuID = dbtools.DegerGetir("select Rec_GetirMenuID from Cst_Recete Where Rec_Genelkod = '" + Rec_Genelkod + "'");

            Rec_GetirMenuID = Rec_GetirMenuID + "," + GetirMenu_id + ",";

            Rec_GetirMenuID = getirIdDuzenle(Rec_GetirMenuID);


            string[] splits = Rec_Genelkod.Split(',');
            foreach (var item in splits)
            {
                dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = '" + Rec_GetirMenuID + "' Where Rec_Genelkod = '" + item.Trim() + "'");

            }


            // anaReceteYenile();

            if (Convert.ToString(gridView8.GetFocusedRowCellValue("Rec_Genelkod")) != "")
            {
                DataTable dt = dbtools.SelectTable("Select Rec_Fiyat,Rec_Paket_Tam From Cst_Recete Where Rec_Genelkod = '" + Convert.ToString(gridView8.GetFocusedRowCellValue("Rec_Genelkod")) + "'");

                if (dt.Rows.Count > 0)
                {
                    gridView8.SetFocusedRowCellValue("Rec_Fiyat", dt.Rows[0][0]);
                }
            }

            //RHMesaj.alertMesaj(Rec_GetirMenuID + " Olarak Güncellendi...", 2);
        }

        private void repositoryItemCheckedComboBoxEdit_AltRecete_Leave(object sender, EventArgs e)
        {
            try
            {
                string Option_id = Convert.ToString(gridView1.GetFocusedRowCellValue("Option_id"));

                string Rec_Genelkod = ((sender as CheckedComboBoxEdit).EditValue).ToString();
                //string Rec_GenelkodOld = ((sender as LookUpEdit).OldEditValue).ToString();

                string Rec_GetirMenuID = dbtools.DegerGetir("select Rec_GetirMenuID from Cst_Recete Where Rec_Genelkod = '" + Rec_Genelkod + "'");
                Rec_GetirMenuID = Rec_GetirMenuID + "," + Option_id + ",";
                Rec_GetirMenuID = getirIdDuzenle(Rec_GetirMenuID);

                string[] splits = Rec_Genelkod.Split(',');
                foreach (var item in splits)
                {
                    dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = '" + Rec_GetirMenuID + "' Where Rec_Genelkod = '" + item.Trim() + "'");
                }

               // altReceteYenile();

                if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")) != "")
                {
                    DataTable dt = dbtools.SelectTable("Select Rec_Fiyat,Rec_Paket_Tam From Cst_Recete Where Rec_Genelkod = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")) + "'");

                    if (dt.Rows.Count > 0)
                    {
                        gridView1.SetFocusedRowCellValue("Rec_Fiyat", dt.Rows[0][0]);
                    }
                }

               // RHMesaj.alertMesaj(Rec_GetirMenuID + " Olarak Güncellendi...", 2);
            }
            catch (Exception ex)
            {

            }

        }

        private void btnAltReceteYenile_Click(object sender, EventArgs e)
        {
            altReceteYenile();
        }

        private void btnAnaReceteYenile_Click(object sender, EventArgs e)
        {
            anaReceteYenile();
            altReceteYenile();

        }

        private void gridView8_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridView8.FocusedRowHandle<0)
            {
                return;
            }
            altReceteYenile();
        }

       

        private void repositoryItemCheckEdit_yoksay_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            string id = gridView8.GetRowCellValue(gridView8.FocusedRowHandle, "ID").ToString();
            string query = "update GetirYemek_Menu_Product set Product_yoksay='" + checkEdit.Checked + "' where ID='" + id + "'";
            dbtools.execcmd(query);
        }

        private void repositoryItemSearchLookUpEdit_AltRecete_Leave(object sender, EventArgs e)
        {
            try
            {
                string Option_id = Convert.ToString(gridView1.GetFocusedRowCellValue("Option_id"));

                string Rec_Genelkod = ((sender as SearchLookUpEdit).EditValue).ToString();
                //string Rec_GenelkodOld = ((sender as LookUpEdit).OldEditValue).ToString();

                string Rec_GetirMenuID = dbtools.DegerGetir("select Rec_GetirMenuID from Cst_Recete Where Rec_Genelkod = '" + Rec_Genelkod + "'");
                Rec_GetirMenuID = Rec_GetirMenuID + "," + Option_id + ",";
                Rec_GetirMenuID = getirIdDuzenle(Rec_GetirMenuID);

                string[] splits = Rec_Genelkod.Split(',');
                foreach (var item in splits)
                {
                    dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = '" + Rec_GetirMenuID + "' Where Rec_Genelkod = '" + item.Trim() + "'");
                }

                // altReceteYenile();

                if (Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")) != "")
                {
                    DataTable dt = dbtools.SelectTable("Select Rec_Fiyat,Rec_Paket_Tam From Cst_Recete Where Rec_Genelkod = '" + Convert.ToString(gridView1.GetFocusedRowCellValue("Rec_Genelkod")) + "'");

                    if (dt.Rows.Count > 0)
                    {
                        gridView1.SetFocusedRowCellValue("Rec_Fiyat", dt.Rows[0][0]);
                    }
                }

                //RHMesaj.alertMesaj(Rec_GetirMenuID + " Olarak Güncellendi...", 2);
            }
            catch (Exception ex)
            {

            }
        }


        private void repositoryItemSearchLookUpEdit_AnaMenu_Leave(object sender, EventArgs e)
        {
            string GetirMenu_id = Convert.ToString(gridView8.GetFocusedRowCellValue("GetirMenu_id"));
            //string Rec_GenelkodOld = ((sender as LookUpEdit).OldEditValue).ToString();
            if ((sender as SearchLookUpEdit).EditValue==null) { return; }
            string Rec_Genelkod = ((sender as SearchLookUpEdit).EditValue).ToString();

            string Rec_GetirMenuID = dbtools.DegerGetir("select Rec_GetirMenuID from Cst_Recete Where Rec_Genelkod = '" + Rec_Genelkod + "'");

            Rec_GetirMenuID = Rec_GetirMenuID + "," + GetirMenu_id + ",";

            Rec_GetirMenuID = getirIdDuzenle(Rec_GetirMenuID);


            string[] splits = Rec_Genelkod.Split(',');
            foreach (var item in splits)
            {
                dbtools.execcmd("Update Cst_Recete set Rec_GetirMenuID = '" + Rec_GetirMenuID + "' Where Rec_Genelkod = '" + item.Trim() + "'");

            }


            // anaReceteYenile();

            if (Convert.ToString(gridView8.GetFocusedRowCellValue("Rec_Genelkod")) != "")
            {
                DataTable dt = dbtools.SelectTable("Select Rec_Fiyat,Rec_Paket_Tam From Cst_Recete Where Rec_Genelkod = '" + Convert.ToString(gridView8.GetFocusedRowCellValue("Rec_Genelkod")) + "'");

                if (dt.Rows.Count > 0)
                {
                    gridView8.SetFocusedRowCellValue("Rec_Fiyat", dt.Rows[0][0]);
                }
            }

            //RHMesaj.alertMesaj(Rec_GetirMenuID + " Olarak Güncellendi...", 2);
        }
    }
}