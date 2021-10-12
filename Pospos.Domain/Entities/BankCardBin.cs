using Pospos.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Domain.Entities
{
    public class BankCardBin : DetailedBaseEntity
    {
        public int BankId { get; set; }
        public string BinCode { get; set; }
        public bool IsActive { get; set; }
    }
}
