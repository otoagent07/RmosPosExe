using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Ingenico
{
    public class SonucTicket
    {
        public byte ticketType { get; set; }
        public UInt16 ZNo { get; set; }
        public UInt16 FNo { get; set; }
        public UInt16 EJNo { get; set; }
        public string BkmID { get; set; }
    }
}
