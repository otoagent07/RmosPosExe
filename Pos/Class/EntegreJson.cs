using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Class
{
    public class EntegreJson
    {
        public string tip { get; set; } 
        public string fisno { get; set; }
        public string printTip { get; set; }// siparis paket hesap
        public string odemeTip { get; set; }// siparis paket hesap

        public bool hesapYazsinmi { get; set; } = false;
        public bool siparistePaketYazsinmi { get; set; } = false;


    }
}
