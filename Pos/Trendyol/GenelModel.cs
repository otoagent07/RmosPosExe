using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Trendyol
{
    public class GenelModel
    {
        public bool success { get; set; } = true;
        public string mesaj { get; set; } = "";
        public object data { get; set; } = null;
    }
}
