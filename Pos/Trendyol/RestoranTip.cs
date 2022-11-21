using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Class
{
    public class RestoranTip
    {
        public static int yemeksepeti = 0;
        public static int getir = 1;
        public static int trendyol = 2;

        public static string onayBekliyor = "Onay Bekliyor"; //400
        public static string hazirlaniyor = "Hazırlanıyor"; //500
        public static string hazirlandi = "Hazırlandı";
        public static string yolaCikti = "Yola Çıktı"; //700
        public static string teslimEdildi = "Teslim Edildi";
        public static string iptalEdildi = "İptal Edildi";


        public static int onayBekliyorKod = 0;
        public static int hazirlaniyorKod = 1;
        public static int hazirlandiKod   = 2;
        public static int yolaCiktiKod    = 3;
        public static int teslimEdildiKod = 4;
        public static int iptalEdildiKod = 5;

        public static string restoranKuryesi = "STORE";
        public static string entegreKurye = "GO";

        public static string eslesmeyenRecId = "999888777";
    }
}
