using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Ingenico
{
    public class IngenicoClass
    {
        public class ST_PAYMENT
        {
            public byte flags;
            public UInt32 dateOfPayment;
            public UInt32 typeOfPayment;                // EPaymentTypes
            public byte subtypeOfPayment;               // EPaymentSubtypes
            public UInt32 orgAmount;                    // Exp; Currency Amount
            public UInt16 orgAmountCurrencyCode;        // as defined in currecyTable from GIB
            public UInt32 payAmount;                    // always TL with precision 2
            public UInt16 payAmountCurrencyCode;        // always TL
            public UInt32 cashBackAmountInTL;           // Para üstü, her zaman TL with precision 2
            public UInt32 cashBackAmountInDoviz;        // Para Üstü, döviz satış ise döviz karşılığı
            public string paymentName;                  // Payment name written on the ticket */
            public string paymentInfo;                  // Payment sub message acording to the payment type */
            public ST_BANK_PAYMENT_INFO stBankPayment;  // Keeps all payment info related with bank


            public ST_PAYMENT()
            {
                paymentName = "";
                paymentInfo = "";
                stBankPayment = new ST_BANK_PAYMENT_INFO();
            }
        };
    }
}
