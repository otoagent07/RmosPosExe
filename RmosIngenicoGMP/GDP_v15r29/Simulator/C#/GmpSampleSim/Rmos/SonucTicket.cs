using System;

namespace RmosIngenicoGMP.Rmos
{
    public class SonucTicket
    {
        public byte ticketType { get; set; }
        public UInt16 ZNo { get; set; }
        public UInt16 FNo { get; set; }
        public UInt16 EJNo { get; set; }
    }
}
