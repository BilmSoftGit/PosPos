using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Domain.Model
{
    public class PaymentTestRequestModel
    {
        public string CardholderName { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string CreditCardType { get; set; }
        public string CardNumber1 { get; set; }
        public string CardNumber2 { get; set; }
        public string CardNumber3 { get; set; }
        public string CardNumber4 { get; set; }
        public string CardCode { get; set; }
        public string Installment { get; set; }
    }
}
