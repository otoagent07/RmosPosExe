using System.Collections.Generic;

namespace Pos.Getir.Class
{
    public class GetirPaymentResponse
    {
        public class Root
        {
            public string id;
            public Name name;
            public string icon;
            public int paymentGroup;
            public List<int> deliveryTypes;
            public int type;
        }

    }
}
