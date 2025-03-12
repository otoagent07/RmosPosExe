using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Models
{
    public class PrintJson
    {
        public int id { get; set; }
        public string tip { get; set; } = "-1";
        public string fisno { get; set; } = "-1";
        public string printTip { get; set; } = "-1"; // siparis paket hesap 
        public string odemeTip { get; set; } = "-1";  // siparis paket hesap

        public bool hesapYazsinmi { get; set; } = false;

        public bool siparistePaketYazsinmi { get; set; } = false;

        public bool isPrint { get; set; } = false;
    }
}
