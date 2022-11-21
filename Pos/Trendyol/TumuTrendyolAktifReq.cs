using Pos.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Trendyol
{
    public class TumuTrendyolAktifReq
    {
        public Nullable<bool> durum { get; set; } = false;// eğer true ise  aktif siparişler gelcek

        public int? supplierId { get; set; } = 121754;
        public int? storeId { get; set; } = 635;

        public string apiKey { get; set; }
        public string apiSecret { get; set; }
        public string token { get; set; } = ""; // bu değer boş yollancak . sonra doldurulcak base64(apiKey:apiSecret) olarak
        public string recDep { get; set; } = Departman.Dep_Kodu;

    }
}
