using Newtonsoft.Json;
using Pos.Class;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace Pos.Getir.Class
{
    public class GetirApi
    {
        public GetirApi()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        public T getRequestS<T>(string link, string token = "")
        {
            string result = "";
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
                if (token != "") client.DefaultRequestHeaders.Add("token", token);
                HttpResponseMessage response = client.GetAsync(link).Result;
                var byteArray = response.Content.ReadAsByteArrayAsync().Result;
                result = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                jsonKaydet(result);
                if (result != "" && result.Substring(0, 1).Equals("[")) //listtir 
                {
                    List<GetirGenelError> errorList = JsonConvert.DeserializeObject<List<GetirGenelError>>(result);
                    if (errorList != null && errorList.Count != 0 && errorList[0].code != 0) // 0 ise hata yok demek
                    {
                        MessageBox.Show("Error code = " + errorList[0].code + "\nError message = " + errorList[0].message);
                        result = "";
                    }
                }
                else // list değildir -> result = result.Substring(1, result.Length - 2);
                {
                    GetirGenelError error = JsonConvert.DeserializeObject<GetirGenelError>(result);
                    if (error != null && error.code != 0) // 0 ise hata yok demek
                    {
                        MessageBox.Show("Error code = " + error.code + "\nError message = " + error.message);
                        result = "";
                    }
                }

                T outs = JsonConvert.DeserializeObject<T>(result);

                return outs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir Hata Oldu " + hataMesaj + ex.Message);
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        public void jsonKaydet(string result)
        {

            int toplam = Convert.ToInt32(dbtools.DegerGetir("select count(*) as toplam from GetirYemek_Json "));
            if (toplam>1000)
            {
                dbtools.execcmd("truncate table GetirYemek_Json");
            }
            dbtools.execcmd("insert into GetirYemek_Json(GetirYemek_Json_ad) values('" + result.Replace("'","''") + "')");

        }
        public T postRequestS<T>(string link, string token = "")
        {
            string result = "";
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
                if (token != "") client.DefaultRequestHeaders.Add("token", token);
                var stringContent = new StringContent("{}", Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(link, stringContent).Result;
                var byteArray = response.Content.ReadAsByteArrayAsync().Result;
                result = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

             /*   result = "[{\"id\":\"614331cbc9047eeef9136bf7\",\"status\":400,\"isScheduled\":false,\"confirmationId\":\"l263\",\"client\":{\"id\":\"601fb074b085f100068a18de\",\"name\":\"Murat S.\",\"location\":{\"lat\":36.889063413338455,\"lon\":30.73906313627958},\"clientPhoneNumber\":\"+90 (850) 346-9382 / 29339\",\"contactPhoneNumber\":\"+90 (850) 215-1500\",\"deliveryAddress\":{\"id\":\"60bf5377fd137e0006a5ee94\",\"address\":\"Mehmetçik Mah. - Mehmetçik, Aspendos Blv. 63d, 07300 Muratpaşa/Antalya, Türkiye\",\"aptNo\":\"63\",\"floor\":\"A\",\"doorNo\":\"\",\"city\":\"Antalya\",\"district\":\"Muratpaşa\",\"description\":\"Moda Life Mobilya\"}},\"courier\":{\"id\":\"5dc073faf4d28e09f0377cad\",\"status\":900,\"name\":\"RestoranKuryesi\",\"location\":{\"lat\":40.740522,\"lon\":28.623905}},\"products\":[{\"id\":\"614331cbc9047e3762136bfa\",\"imageURL\":\"https://cdn.getiryemek.com/products/1622412072784_500x375.jpeg\",\"wideImageURL\":\"https://cdn.getiryemek.com/products/1622412072784_1000x750.jpeg\",\"count\":1,\"product\":\"60a227e16ec6b70519911822\",\"chainProduct\":\"\",\"name\":{\"tr\":\"Ekmek Arası Et Döner\",\"en\":\"Ekmek Arası Et Döner\"},\"price\":26.9,\"optionPrice\":0,\"priceWithOption\":26.9,\"totalPrice\":26.9,\"totalOptionPrice\":0,\"totalPriceWithOption\":26.9,\"optionCategories\":[{\"optionCategory\":\"60a227e16ec6b7080b911823\",\"name\":{\"tr\":\"Malzeme Tercihi\",\"en\":\"Malzeme Tercihi\"},\"options\":[{\"option\":\"60a227e16ec6b7a97b911824\",\"name\":{\"tr\":\"İstemiyorum\",\"en\":\"İstemiyorum\"},\"price\":0}]},{\"optionCategory\":\"60a227e16ec6b7c6b5911826\",\"name\":{\"tr\":\"Porsiyon Tercihi\",\"en\":\"Porsiyon Tercihi\"},\"options\":[{\"option\":\"60a227e16ec6b766b9911827\",\"name\":{\"tr\":\"1 Porsiyon\",\"en\":\"1 Porsiyon\"},\"price\":0}]},{\"optionCategory\":\"60a227e16ec6b7df77911829\",\"name\":{\"tr\":\"Sos Tercihi\",\"en\":\"Sos Tercihi\"},\"options\":[{\"option\":\"60a227e16ec6b7f0e191182e\",\"name\":{\"tr\":\"Yoğurtlu Sarımsaklı Sos\",\"en\":\"Yoğurtlu Sarımsaklı Sos\"},\"price\":0}]}],\"displayInfo\":{\"title\":{\"tr\":\"Ekmek Arası Et Döner\",\"en\":\"Ekmek Arası Et Döner\"},\"options\":{\"tr\":[\"Malzeme Tercihi: İstemiyorum\",\"Porsiyon Tercihi: 1 Porsiyon\",\"Sos Tercihi: Yoğurtlu Sarımsaklı Sos\"],\"en\":[\"Malzeme Tercihi: İstemiyorum\",\"Porsiyon Tercihi: 1 Porsiyon\",\"Sos Tercihi: Yoğurtlu Sarımsaklı Sos\"]}},\"note\":\"\"},{\"id\":\"614331d9b5a706a627f39312\",\"imageURL\":\"\",\"wideImageURL\":\"\",\"count\":1,\"product\":\"60a227efadab316a5560e59e\",\"chainProduct\":\"\",\"name\":{\"tr\":\"Coca Cola (330 ml)\",\"en\":\"Coca Cola (330 ml)\"},\"price\":7,\"optionPrice\":0,\"priceWithOption\":7,\"totalPrice\":7,\"totalOptionPrice\":0,\"totalPriceWithOption\":7,\"optionCategories\":[],\"displayInfo\":{\"title\":{\"tr\":\"Coca Cola (330 ml)\",\"en\":\"Coca Cola (330 ml)\"},\"options\":{\"tr\":[],\"en\":[]}},\"note\":\"\"}],\"clientNote\":\"\",\"totalPrice\":33.9,\"totalDiscountedPrice\":20,\"checkoutDate\":\"2021-09-16T12:01:02.386Z\",\"deliveryType\":2,\"doNotKnock\":false,\"isEcoFriendly\":false,\"restaurant\":{\"id\":\"5ff7c82b37b18178b006c980\"},\"paymentMethod\":1,\"paymentMethodText\":{\"en\":\"Online Payment\",\"tr\":\"Online Ödeme\"}}]";
             */

                jsonKaydet(result);
                if (result != "" && result.Substring(0, 1).Equals("[")) //listtir 
                {
                    List<GetirGenelError> errorList = JsonConvert.DeserializeObject<List<GetirGenelError>>(result);
                    if (errorList != null && errorList.Count != 0 && errorList[0].code != 0) // 0 ise hata yok demek
                    {
                        string mesaj = "Error code = " + errorList[0].code + "\nError message = " + errorList[0].message;
                        if (errorList[0].code == 62)
                        {
                            RHMesaj.MyMessageInformation("Lütfen 1-2 Dk Bekleyiniz !\n" + mesaj);
                        }
                        else
                        {
                            MessageBox.Show(mesaj);
                        }
                    }
                }
                else // list değildir -> result = result.Substring(1, result.Length - 2);
                {
                    GetirGenelError error = JsonConvert.DeserializeObject<GetirGenelError>(result);

                    if (error != null && error.code != 0) // 0 ise hata yok demek
                    {
                        string mesaj = "Error code = " + error.code + "\nError message = " + error.message;

                        if (error.code == 62)
                        {
                            RHMesaj.MyMessageInformation("Lütfen 1-2 Dk Bekleyiniz !\n" + mesaj);
                        }
                        else
                        {
                            MessageBox.Show(mesaj);
                        }
                    }
                }

                T outs = JsonConvert.DeserializeObject<T>(result);

                return outs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir Hata Oldu " + hataMesaj + ex.Message);
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        public string hataMesaj = "\nİnternet Bağlantınızı Kontrol Ediniz !\n";

        public TOut postRequestS<TIn, TOut>(string link, TIn model)
        {
            string result = "";
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
                HttpResponseMessage response = client.PostAsync(link, stringContent).Result;
                var byteArray = response.Content.ReadAsByteArrayAsync().Result;
                result = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                jsonKaydet(result);
                if (result != "" && result.Substring(0, 1).Equals("[")) //listtir 
                {
                    List<GetirGenelError> errorList = JsonConvert.DeserializeObject<List<GetirGenelError>>(result);
                    if (errorList != null && errorList.Count != 0 && errorList[0].code != 0) // 0 ise hata yok demek
                    {
                        MessageBox.Show("Error code = " + errorList[0].code + "\nError message = " + errorList[0].message);
                    }
                }
                else // list değildir -> result = result.Substring(1, result.Length - 2);
                {
                    GetirGenelError error = JsonConvert.DeserializeObject<GetirGenelError>(result);
                    if (error != null && error.code != 0) // 0 ise hata yok demek
                    {
                        string ekle = "";
                        if (error.code==4)
                        {
                            ekle = " Base Url Farklı Olabilir. Test Kullanın[Param3] ! ";
                        }
                        MessageBox.Show("Error code = " + error.code + "\nError message = " + error.message+ " "+ekle);
                    }
                }

                TOut outs = JsonConvert.DeserializeObject<TOut>(result);

                return outs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir Hata Oldu " + hataMesaj + ex.Message);
                TOut outs = JsonConvert.DeserializeObject<TOut>(result); // result boş olursa nesne null doner
                return outs;
            }
        }
        public TOut postRequestS<TIn, TOut>(string link, string token, TIn model)
        {
            string result = "";
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                if (token != "") client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
                HttpResponseMessage response = client.PostAsync(link, stringContent).Result;
                var byteArray = response.Content.ReadAsByteArrayAsync().Result;
                result = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                jsonKaydet(result);
                if (result != "" && result.Substring(0, 1).Equals("[")) //listtir 
                {
                    List<GetirGenelError> errorList = JsonConvert.DeserializeObject<List<GetirGenelError>>(result);
                    if (errorList != null && errorList.Count != 0 && errorList[0].code != 0) // 0 ise hata yok demek
                    {
                        MessageBox.Show("Error code = " + errorList[0].code + "\nError message = " + errorList[0].message);
                    }
                }
                else // list değildir -> result = result.Substring(1, result.Length - 2);
                {
                    GetirGenelError error = JsonConvert.DeserializeObject<GetirGenelError>(result);
                    if (error != null && error.code != 0) // 0 ise hata yok demek
                    {
                        MessageBox.Show("Error code = " + error.code + "\nError message = " + error.message);
                    }
                }

                TOut outs = JsonConvert.DeserializeObject<TOut>(result);

                return outs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir Hata Oldu " + hataMesaj + ex.Message);
                TOut outs = JsonConvert.DeserializeObject<TOut>(result); // result boş olursa nesne null doner
                return outs;
            }
        }

        public string putRequestS(string link, string token = "")
        {
            string result = "";
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));
                if (token != "") client.DefaultRequestHeaders.Add("token", token);
                var stringContent = new StringContent("{}", Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(link, stringContent).Result;
                var byteArray = response.Content.ReadAsByteArrayAsync().Result;
                result = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                jsonKaydet(result);
                if (result != "" && result.Substring(0, 1).Equals("[")) //listtir 
                {
                    List<GetirGenelError> errorList = JsonConvert.DeserializeObject<List<GetirGenelError>>(result);
                    if (errorList != null && errorList.Count != 0 && errorList[0].code != 0) // 0 ise hata yok demek
                    {
                        MessageBox.Show("Error code = " + errorList[0].code + "\nError message = " + errorList[0].message);
                    }
                }
                else // list değildir -> result = result.Substring(1, result.Length - 2);
                {
                    GetirGenelError error = JsonConvert.DeserializeObject<GetirGenelError>(result);
                    if (error != null && error.code != 0) // 0 ise hata yok demek
                    {
                        MessageBox.Show("Error code = " + error.code + "\nError message = " + error.message);
                    }
                }

                string outs = result;

                return outs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir Hata Oldu " + hataMesaj + ex.Message);
                return result;
            }
        }


        public GetirLoginResponse getToken(string appSecretKey, string restaurantSecretKey)
        {
            try
            {
                GetirLoginRequest getirLoginRequest = new GetirLoginRequest();
                getirLoginRequest.appSecretKey = appSecretKey;
                getirLoginRequest.restaurantSecretKey = restaurantSecretKey;

                GetirLoginResponse response = postRequestS<GetirLoginRequest, GetirLoginResponse>(GetirStatik.requestLogin, getirLoginRequest);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }

        public GetirRestaurantResponse getRestaurant(string token)
        {
            try
            {
                GetirRestaurantResponse response = getRequestS<GetirRestaurantResponse>(GetirStatik.requestRestoran, token);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }

        public GetirMenuResponse.Root getRestaurantMenu(string token)
        {
            try
            {
                GetirMenuResponse.Root response = getRequestS<GetirMenuResponse.Root>(GetirStatik.requestMenu, token);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }

        public GetirMenuResponse getMenuRespose(string token)
        {
            try
            {
                GetirMenuResponse response = getRequestS<GetirMenuResponse>(GetirStatik.requestMenu, token);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }

        public List<GetirOrderResponse.Root> postFoodOrderActiveResponse(string token)
        {
            try
            {
                List<GetirOrderResponse.Root> response = postRequestS<List<GetirOrderResponse.Root>>(GetirStatik.requestOrder, token);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }

        public GetirOnay getOrderVerify(string token, string foodOrderId) // onay
        {
            try
            {
                GetirOnay response = postRequestS<GetirOnay>(GetirStatik.requestOrderBase + "/" + foodOrderId + "/verify", token);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }

        public GetirOnay getOrderVerifyScheduled(string token, string foodOrderId) // onay
        {
            try
            {
                GetirOnay response = postRequestS<GetirOnay>(GetirStatik.requestOrderBase + "/" + foodOrderId + "/verify-scheduled", token);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }
        public List<GetirOrderResponse.Root> postOrderPeriodicUnapproved(string token) // onay
        {
            try
            {
                List<GetirOrderResponse.Root> response = postRequestS<List<GetirOrderResponse.Root>>(GetirStatik.requestOrderBase + "/periodic/unapproved", token);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }

        public GetirOnay getOrderPrepare(string token, string foodOrderId) // onay
        {
            try
            {
                GetirOnay response = postRequestS<GetirOnay>(GetirStatik.requestOrderBase + "/" + foodOrderId + "/prepare", token);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }

        public GetirOnay getOrderPrepareDeliverHandover(string token, string foodOrderId, string Link) // onay
        {
            try
            {
                GetirOnay response = postRequestS<GetirOnay>(GetirStatik.requestOrderBase + "/" + foodOrderId + "/" + Link, token);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }

        public GetirCancel.PostCancelResponse PostFoodOrderCancel(string id, string Message, string token, string foodOrderId)
        {
            try
            {
                GetirCancel.PostCancelRequest cancel = new GetirCancel.PostCancelRequest();
                cancel.cancelNote = Message;
                cancel.cancelReasonId = id;

                GetirCancel.PostCancelResponse response = postRequestS<GetirCancel.PostCancelRequest, GetirCancel.PostCancelResponse>(GetirStatik.requestOrderBase + "/" + foodOrderId + "/cancel", token, cancel);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }

        public List<GetirPaymentResponse.Root> getPayment(string token)
        {
            try
            {
                List<GetirPaymentResponse.Root> response = getRequestS<List<GetirPaymentResponse.Root>>(GetirStatik.requestPayment, token);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA ! " + ex.Message);
                return null;
            }
        }
    }
}
