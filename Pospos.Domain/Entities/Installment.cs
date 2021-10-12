using Pospos.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Domain.Entities
{
    public class Installment : DetailedBaseEntity
    {
        public int StationId { get; set; }
        public int PosId { get; set; }
        public int InstallmentCount { get; set; }
        public decimal TermDifference { get; set; }
        public bool IsActive { get; set; }
    }
}
