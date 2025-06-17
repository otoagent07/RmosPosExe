using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Models
{
    public class SatisYapModel
    {
        public string satisFisno { get; set; }
        public int kartId { get; set; }
        public string Kart_No { get; set; }
        public string Rsat_Ba { get; set; }
        public string Urun_Kodu { get; set; }
        public string Rsat_Durum { get; set; }
        public string Rsat_Kapatma { get; set; }
        public decimal Rsat_Tutar { get; set; }
        public decimal Rec_Fiyat { get; set; }
        public decimal Rec_Dovifiyat { get; set; }
        public decimal Rsat_Net { get; set; }
        public decimal Rsat_Kdv { get; set; }
        public decimal Rec_Kdv { get; set; }
    }
}
