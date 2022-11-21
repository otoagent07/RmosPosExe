using Newtonsoft.Json;
using Pos.Class;
using Pos.Controllers;
using Pos.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Trendyol
{
    public class TrendyolApi
    {
        HttpClient client;
        public TrendyolApi()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            client = new HttpClient(handler);
            ayarlar = entegreAyarlar.listele();
        }

        public OrderReqAccept getOrderReq(string packageId)
        {
            OrderReqAccept accept = new OrderReqAccept();
            accept.storeId = ayarlar.trendyolStoreId;
            accept.supplierId = ayarlar.trendyolSupplierId;
            accept.apiKey = ayarlar.trendyolApiKey;
            accept.apiSecret = ayarlar.trendyolApiSecret;
            accept.preparationTime = "10";
            accept.packageId = packageId;
            accept.recDep = Departman.Dep_Kodu;

            return accept;
        }

        public CancelOrderReq getCancelReq(string packageId, int reasonId, List<string> itemList)
        {
            CancelOrderReq cancel = new CancelOrderReq();
            cancel.storeId = ayarlar.trendyolStoreId;
            cancel.supplierId = ayarlar.trendyolSupplierId;
            cancel.apiKey = ayarlar.trendyolApiKey;
            cancel.apiSecret = ayarlar.trendyolApiSecret;
            cancel.packageId = packageId;
            cancel.reasonId = reasonId;
            cancel.recDep = Departman.Dep_Kodu;

            cancel.itemIdList = itemList;
            return cancel;
        }

        public TumuTrendyolAktifReq getActive()
        {
            TumuTrendyolAktifReq aktifReq = new TumuTrendyolAktifReq();
            aktifReq.storeId = ayarlar.trendyolStoreId;
            aktifReq.supplierId = ayarlar.trendyolSupplierId;
            aktifReq.apiKey = ayarlar.trendyolApiKey;
            aktifReq.apiSecret = ayarlar.trendyolApiSecret;
            aktifReq.recDep = Departman.Dep_Kodu;
            aktifReq.durum = true;
            return aktifReq;
        }


        EntegreAyarlarController entegreAyarlar = new EntegreAyarlarController();
        public entegreAyarlar ayarlar = new entegreAyarlar();
        public void apiSifirla()
        {
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            client = new HttpClient(handler);
            ayarlar = entegreAyarlar.listele();
        }
        public string requestPost(string url, Dictionary<string, string> dict)
        {
            apiSifirla();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            var Content = new FormUrlEncodedContent(dict);
            HttpResponseMessage responseOtel = client.PostAsync(url, Content).Result;
            string result = responseOtel.Content.ReadAsStringAsync().Result;
            return result;
        }

        public string requestPostJson(string url, object model)
        {
            apiSifirla();

            string json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseOtel = client.PostAsync(url, stringContent).Result;
            string result = responseOtel.Content.ReadAsStringAsync().Result;
            return result;
        }

        public string requestGet(string url)
        {
            apiSifirla();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            HttpResponseMessage responseOtel = client.GetAsync(url).Result;
            string result = responseOtel.Content.ReadAsStringAsync().Result;
            result = System.Net.WebUtility.HtmlDecode(result);

            return result;
        }

        public string requestGetBase64(string apiKey, int carId)
        {
            string url = "resimurl";

            HttpResponseMessage response = client.GetAsync(url).Result;
            var result = response.Content.ReadAsByteArrayAsync().Result;
            return Convert.ToBase64String(result);
        }


        public string kes(string txt, string ilk, string son)
        {
            try
            {
                string parcali1 = (txt.Split(new string[] { ilk }, StringSplitOptions.None))[1];
                string parcali2 = (parcali1.Split(new string[] { son }, StringSplitOptions.None))[0];
                return parcali2.Trim();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> kesList(string txt, string ilk, string son)
        {
            try
            {
                List<string> list = new List<string>();
                string[] parcali1 = (txt.Split(new string[] { ilk }, StringSplitOptions.None));
                for (int i = 1; i < parcali1.Length; i++)
                {
                    string parcali2 = (parcali1[i].Split(new string[] { son }, StringSplitOptions.None))[0];
                    list.Add(parcali2.Trim());

                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public GenelModel siparisAccept1(string packageId)
        {
            try
            {
                string url = ayarlar.trendyolApiLink + "api/Trendyol/putAcceptOrder";
                string json = requestPostJson(url, getOrderReq(packageId));
                GenelModel model = JsonConvert.DeserializeObject<GenelModel>(json);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public GenelModel siparisEndOfOrder2(string packageId)
        {
            try
            {
                string url = ayarlar.trendyolApiLink + "api/Trendyol/putEndOrder";
                string json = requestPostJson(url, getOrderReq(packageId));
                GenelModel model = JsonConvert.DeserializeObject<GenelModel>(json);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public GenelModel siparisOrderOnWay3(string packageId)
        {
            try
            {
                string url = ayarlar.trendyolApiLink + "api/Trendyol/putOrderOnWay";
                string json = requestPostJson(url, getOrderReq(packageId));
                GenelModel model = JsonConvert.DeserializeObject<GenelModel>(json);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public GenelModel siparisOrderDelivery4(string packageId)
        {
            try
            {
                string url = ayarlar.trendyolApiLink + "api/Trendyol/putOrderDelivery";
                string json = requestPostJson(url, getOrderReq(packageId));
                GenelModel model = JsonConvert.DeserializeObject<GenelModel>(json);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public GenelModel siparisCancel(string packageId, int reasonId, List<string> itemList)
        {
            try
            {
                string url = ayarlar.trendyolApiLink + "api/Trendyol/putOrderCancel";
                string json = requestPostJson(url, getCancelReq(packageId, reasonId, itemList));
                GenelModel model = JsonConvert.DeserializeObject<GenelModel>(json);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public GenelModel siparisGetir()
        {
            try
            {
                string url = ayarlar.trendyolApiLink + "api/Trendyol/getActive";
                string json = requestPostJson(url, getActive());
                GenelModel model = JsonConvert.DeserializeObject<GenelModel>(json);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public GenelModel menuKaydet()
        {
            try
            {
                if (ayarlar==null)
                {
                    RHMesaj.MyMessageInformation(Departman.Dep_Adi + " Departmanın Ayarları Yapılmamış!!!");
                    return null;
                }
                string url = ayarlar.trendyolApiLink + "api/Trendyol/getMenuKaydet";
                string json = requestPostJson(url, getActive());
                GenelModel model = JsonConvert.DeserializeObject<GenelModel>(json);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public bool eslesmeyenUrunVarmi(string siparisId)
        {
            bool kontrol = false;
            try
            {
                int count = Convert.ToInt32(dbtools.DegerGetir("select count(*) as toplam from entegreSiparisUrunler where siparisId='" + siparisId + "' and recId='" + RestoranTip.eslesmeyenRecId + "'"));
                if (count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageInformation("HATA ! \nTrendyolApi.eslesmeyenUrunVarmi()\n" + ex.Message);
            }

            return kontrol;
        }


        public void urunleriEslestir(string siparisId)
        {
            try
            {
                if (eslesmeyenUrunVarmi(siparisId))
                {
                    string query = @"select m.id,m.recId,m.recAd,m.recFiyat,s.id as siparisUrunId from entegreSiparisUrunler s
left join entegreMenu m on m.entegreId=s.entegreMenuId and m.entegreMenuMasterId=s.entegreMenuMasterId
where siparisId='" + siparisId + "'";
                    DataTable dataTable = dbtools.SelectTableR(query);

                    foreach (DataRow item in dataTable.Rows)
                    {
                        string fiyat = item["recFiyat"].ToString().Replace(",", ".");
                        string sorgu = "update entegreSiparisUrunler set recAd='" + item["recAd"].ToString() + "',recFiyat='" + fiyat + "',recId='" + item["recId"].ToString() + "' where id='" + item["siparisUrunId"].ToString() + "'";

                        dbtools.execcmdR(sorgu);
                    }
                }
                
            }
            catch (Exception ex)
            {
                RHMesaj.MyMessageInformation("HATA ! \nTrendyolApi.urunleriEslestir()\n" + ex.Message);
            }
        }


       

    }
}
