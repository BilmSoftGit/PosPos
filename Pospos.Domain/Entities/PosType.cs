using Pospos.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Domain.Entities
{
    public class PosType : DetailedBaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
