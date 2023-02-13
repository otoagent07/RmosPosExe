using Newtonsoft.Json;
using Pos.Class;
using Pos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Controllers
{
    public class AyarlarController  //değerlere ulaşmak için ->  Main.ayarlar.paketOtoKapat
    {
        public string urun_printer = "",yuvarlama="", satisEkranGenislik="228";
        public bool paketOtoKapat = false;

        public string otomatikIndirim = "";

        DataTable dataTable = new DataTable();

        public List<UrunPrintModel> urunPrintModels = new List<UrunPrintModel>();

        public List<YuvarlaModel> yuvarlaModels = new List<YuvarlaModel>();

        public List<IndirimModel> indirimModels= new List<IndirimModel>();

        public AyarlarController()
        {
            yenile();

        }

        public void yenile()
        {
            dataTable = dbtools.SelectTable("select * from ayarlar");
            if (dataTable==null)
            {
                return;
            }
            foreach (DataRow item in dataTable.Rows)
            {
                switch (item["ayarlar_key"].ToString())
                {
                    case "urun_printer":
                        urun_printer = item["ayarlar_value"].ToString();
                        break;
                    case "paketOtoKapat":
                        int deger = Convert.ToInt32(item["ayarlar_value"].ToString());
                        paketOtoKapat = Convert.ToBoolean(deger);
                        break;
                    case "yuvarlama":
                        yuvarlama = item["ayarlar_value"].ToString();
                        break;
                    case "otomatikIndirim":
                        otomatikIndirim = item["ayarlar_value"].ToString();
                        break;
                    case "satisEkranGenislik":
                        satisEkranGenislik = item["ayarlar_value"].ToString();
                        break;

                }
            }

            urunPrintModels = new List<UrunPrintModel>();
            if (!urun_printer.Equals("")) 
            {
                urunPrintModels = JsonConvert.DeserializeObject<List<UrunPrintModel>>(urun_printer);
            }

            yuvarlaModels = new List<YuvarlaModel>();
            if (!yuvarlama.Equals(""))
            {
                yuvarlaModels = JsonConvert.DeserializeObject<List<YuvarlaModel>>(yuvarlama);
            }

            indirimModels = new List<IndirimModel>();
            if (!otomatikIndirim.Equals(""))
            {
                indirimModels = JsonConvert.DeserializeObject<List<IndirimModel>>(otomatikIndirim);
            }
        }

        public string getYazici(string depkod = "")
        {
            if (depkod == "") depkod = Departman.Dep_Kodu;
            foreach (var item in urunPrintModels)
            {
                if (item.departman.Equals(depkod))
                {
                    return item.yazici;
                }
            }

            return "";
        }
        public YuvarlaModel getYuvarlama(string depkod = "")
        {
            try
            {
                if (depkod == "") depkod = Departman.Dep_Kodu;
                foreach (var item in yuvarlaModels)
                {
                    if (item.yuvarlamaDepartman.Equals(depkod))
                    {
                        return item;
                    }
                }
            }
            catch(Exception ex)
            {

            }
         

            return null;
        }

        public IndirimModel getIndirimModel(string depkod = "")
        {
            if (depkod == "") depkod = Departman.Dep_Kodu;
            foreach (var item in indirimModels)
            {
                if (item.depKod.Equals(depkod))
                {
                    return item;
                }
            }
            return null;
        }


        public string getYuvarlamaRecete(string depkod = "")
        {
            if (depkod == "") depkod = Departman.Dep_Kodu;
            foreach (var item in yuvarlaModels)
            {
                if (item.yuvarlamaFiyat==0)
                {
                    return "";
                }
                if (item.yuvarlamaDepartman.Equals(depkod))
                {
                    return item.yuvarlamaRecete;
                }
            }

            return "";
        }

    }
}
