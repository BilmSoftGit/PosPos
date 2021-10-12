using Pospos.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Domain.Entities
{
    public class PaymentError : BaseEntity
    {
        public string PaymentType { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string Comment { get; set; }
        public string Culture { get; set; }
    }
}
