using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Trendyol
{
    public class OrderReqAccept
    {
        public int? supplierId { get; set; } = 121754;
        public int? storeId { get; set; } = 483;
        public string packageId { get; set; } = "";
        public string preparationTime { get; set; } = "";
        public string apiKey { get; set; } = "";
        public string apiSecret { get; set; } = "";
        public string Token { get; set; } = "";
        public string recDep { get; set; } = "";
    }
}
