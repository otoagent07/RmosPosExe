using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmosIngenicoGMP
{
    public class ST_E_IRSALIYE_BILGI
    {
        public string IrsaliyeNo; // max [16 + 1];
        public string ETTN;// max[36 + 1];
        public string CustomerTCKN_VKN;// max [11 + 1];
        public string CustomerName;// max [48 + 1];
        public string CustomerAdress;// max [96 + 1];
        public string DriverTCKN_VKN;// max [11 + 1];
        public string DriverName;// max [48 + 1];
        public string VehiclePlate; //mx [12 + 1];
        public UInt32 TransferDate;
    }
}
