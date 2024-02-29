using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmosIngenicoGMP.Rmos
{
    public class Satislar
    {
        public int Fisno { get; set; }
        public DateTime Tarih { get; set; }
        public string BA { get; set; }
        public string Recete { get; set; }
        public string Adi { get; set; }
        public int Ykasaid { get; set; }
        public string Pkod_Ad { get; set; }
        public int Birim { get; set; }
        public string Kodlar_Ad { get; set; }
        public decimal kdvOran { get; set; }
        public decimal Tutar { get; set; }
        public decimal? Miktar { get; set; }
        public int odemeKodu { get; set; }
        public int banka { get; set; }
        public string OdemeAdi { get; set; }
        public string ybdAdi { get; set; }
    }
}
