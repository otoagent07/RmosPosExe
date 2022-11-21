using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Trendyol
{
    public class TrendyolUrunPaketIdList
    {
        public string packageItemId { get; set; }
        public object lineItemId { get; set; }
        public bool isCancelled { get; set; }
        public object coupon { get; set; }
        public List<object> promotions { get; set; }
    }
}
