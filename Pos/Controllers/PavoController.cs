using Newtonsoft.Json;
using Pos.Class;
using Pos.Models;
using Pos.PavoModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos.Controllers
{
    public class PavoController
    {


        public static string Kodlar_pavoUrl { get; set; } = "";
        public static string Kodlar_pavoSirketId { get; set; } = "";
        public static string Kodlar_Kod { get; set; } = "";
        public static bool aktif { get; set; } = false;

        public bool pavoDepartmanYukle()
        {
            try
            {

                string q1 = $@"select top 1 Kodlar_pavoUrl,Kodlar_pavoSirketId,Kodlar_Kod from Stok_Kodlar where Kodlar_Sinif='01' and Kodlar_Kod='{Departman.Dep_Kodu}' and Kodlar_Pavo=1";
                DataTable dataTable = dbtools.SelectTableR(q1);

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    aktif = false;

                    return false;
                }

                aktif = true;
                PavoController.Kodlar_pavoUrl = dataTable.Rows[0]["Kodlar_pavoUrl"].ToString();
                PavoController.Kodlar_pavoSirketId = dataTable.Rows[0]["Kodlar_pavoSirketId"].ToString();
                PavoController.Kodlar_Kod = dataTable.Rows[0]["Kodlar_Kod"].ToString();

            }
            catch (Exception ex)
            {
                RHMesaj.alertMesaj(ex.Message);
                LogHata(ex.Message);

            }

            return true;

        }

        public async void pavon86KabloluPairing() // cihaz eşleştirme yapılır
        {
            try
            {
                var varmi = pavoDepartmanYukle();

                if (varmi == false || aktif == false)
                {
                    return;
                }

                if (varmi == true && (PavoController.Kodlar_pavoUrl == "" || PavoController.Kodlar_pavoSirketId == ""))
                {
                    MessageBox.Show("Stok kodlar pavo ayarları kayıtlı değil");
                    return;
                }

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, PavoController.Kodlar_pavoUrl + $"/Pavo/Pairing?sirketId={PavoController.Kodlar_pavoSirketId}");
                var response = await client.SendAsync(request);
                string sonuc = response.Content.ReadAsStringAsync().Result;

                var model = JsonConvert.DeserializeObject<PavoResponse>(sonuc);
                if (model == null) { MessageBox.Show("Model is null"); return; }
                if (model.success == false)
                {
                    RHMesaj.alertMesaj("Pavo Başarısız\n" + model.message);
                    LogHata(sonuc);
                }


                Console.WriteLine("aaa2");
            }
            catch (Exception ex)
            {

                RHMesaj.alertMesaj(ex.Message);
                LogHata(ex.Message);
            }
        }



        public async void pavon86KablosuzPairing() // cihaz eşleştirme yapılır
        {
            try
            {

                PavoController pavoController = new PavoController();
                var varmi = pavoController.pavoDepartmanYukle();

                if (varmi == false || aktif == false)
                {
                    return;
                }

                if (varmi == true && (PavoController.Kodlar_pavoUrl == "" || PavoController.Kodlar_pavoSirketId == ""))
                {
                    MessageBox.Show("Stok kodlar pavo ayarları kayıtlı değil");
                    return;
                }

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, PavoController.Kodlar_pavoUrl + $"/Pavo/AuthenticateAsync?sirketId={PavoController.Kodlar_pavoSirketId}");
                var response = await client.SendAsync(request);
                string sonuc = response.Content.ReadAsStringAsync().Result;

                var model = JsonConvert.DeserializeObject<PavoResponse>(sonuc);
                if (model == null) { MessageBox.Show("Model is null"); return; }
                if (model.success == false)
                {
                    RHMesaj.alertMesaj("Pavo Başarısız\n" + model.message);
                    LogHata(sonuc);
                }

            }
            catch (Exception ex)
            {

                RHMesaj.alertMesaj(ex.Message);
                LogHata(ex.Message);
            }
        }



        public void sendPaymentLink(string fisno)
        {
            try
            {
                if (Kodlar_pavoUrl == "" || aktif == false)
                {
                    return;
                }


                try
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, Kodlar_pavoUrl + "/Pavo/PaymentLinkRequest");

                    var payment = new PavoReqModel
                    {
                        fisno = fisno,
                        depkod = Departman.Dep_Kodu,
                        paymentTypeId = 0,
                        sirketId = Kodlar_pavoSirketId
                    };

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(payment);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    request.Content = content;

                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    string sonuc = response.Content.ReadAsStringAsync().Result;
                    var model = JsonConvert.DeserializeObject<PavoResponse>(sonuc);
                    if (model.success == false)
                    {
                        LogHata(sonuc);
                    }
                }
                catch (Exception ex)
                {

                    RHMesaj.alertMesaj(ex.Message);
                    LogHata(ex.Message);
                }



            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                RHMesaj.alertMesaj(ex.Message);
                LogHata(ex.Message);

            }
        }


        public void pavoHesapKapat()
        {
            try
            {
                if (Kodlar_pavoUrl == "" || aktif == false)
                {
                    return;
                }
                string q = $@"select Rsat_Fisno from Cst_Recete_Satis  as s
left join Pos_Masa as m on m.Masa_No=s.Rsat_Masa
where  Rsat_Durum='A' and (m.Masa_Durum=1 or m.Masa_Durum=2) and PaymentLinkId !=''
group by Rsat_Fisno";
                DataTable dataTable = dbtools.SelectTableR(q);

                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow item in dataTable.Rows)
                        {
                            var client = new HttpClient();
                            var request = new HttpRequestMessage(HttpMethod.Post, Kodlar_pavoUrl + "/Pavo/CheckLinkRequest");

                            string fisno = item["Rsat_Fisno"].ToString();
                            var payment = new PavoReqModel
                            {
                                fisno = fisno,
                                depkod = Departman.Dep_Kodu,
                                paymentTypeId = 0,
                                sirketId = Kodlar_pavoSirketId
                            };

                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(payment);
                            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                            request.Content = content;

                            var response = client.SendAsync(request).Result;
                            response.EnsureSuccessStatusCode();
                            string sonuc = response.Content.ReadAsStringAsync().Result;
                            var model = JsonConvert.DeserializeObject<PavoResponse>(sonuc);
                            if (model.success == false)
                            {
                                LogHata(sonuc);
                            }
                        }
                    }
                });


            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                RHMesaj.alertMesaj(ex.Message);
                LogHata(ex.Message);
            }
        }


        public void LogHata(string hataMesaji)
        {
            try
            {
                string dosyaYolu = "Pavohatalar.txt";

                // 1️⃣ Şu anki tarih ve saat
                string zaman = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // 2️⃣ Yazılacak yeni kayıt
                string yeniKayit = $"{zaman} - {hataMesaji}{Environment.NewLine}{Environment.NewLine}";

                // 3️⃣ Eski içeriği oku
                string eskiIcerik = "";
                if (File.Exists(dosyaYolu))
                {
                    eskiIcerik = File.ReadAllText(dosyaYolu);
                }

                // 4️⃣ Yeni hata mesajını en üstte olacak şekilde birleştir
                string tumIcerik = yeniKayit + eskiIcerik;

                // 5️⃣ Dosyaya yaz
                File.WriteAllText(dosyaYolu, tumIcerik);
            }
            catch (Exception ex)
            {

            }

        }




    }
}
