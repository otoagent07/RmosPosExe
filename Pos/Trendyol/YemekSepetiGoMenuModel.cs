using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Trendyol
{
    public class YemekSepetiGoMenuModel
    {
        public string Menu { get; set; }
        public string F2 { get; set; }
        public string F3 { get; set; }

        [JsonProperty("Menu category")]
        public string MenuCategory { get; set; }
        public string F5 { get; set; }
        public string F6 { get; set; }
        public string F7 { get; set; }
        public string F8 { get; set; }
        public string F9 { get; set; }
        public string Product { get; set; }
        public string F11 { get; set; }
        public string F12 { get; set; }
        public string F13 { get; set; }
        public string F14 { get; set; }
        public string F15 { get; set; }
        public string F16 { get; set; }
        public string F17 { get; set; }
        public string F18 { get; set; }
        public string F19 { get; set; }
        public string F20 { get; set; }
        public string F21 { get; set; }
        public string F22 { get; set; }
    }
}
