using Pospos.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Domain.Entities
{
    public class Setting : BaseEntity
    {
        public int StationId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Culture { get; set; }
    }
}
