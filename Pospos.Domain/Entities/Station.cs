using Pospos.Core.Attributes;
using Pospos.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Domain.Entities
{
    [TableName("Station")]
    public class Station : DetailedBaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SuccessUrl { get; set; }
        public string ErrorUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
