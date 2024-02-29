using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmosIngenicoGMP
{
    public class ST_E_BILET
    {
        public string BelgeNo; // max 16
        public string ETTN; // max 36
        public string FilmAdi; // max 48
        public byte Tip;
        public uint SeansTarihSaat;
        public string Belediye; // max 48
        public string SinemaAdi; // max 48
        public string SalonAdi; // max 32
        public string KoltukNo; // max 48
        public UInt16 KdvOrani;
    }
}
