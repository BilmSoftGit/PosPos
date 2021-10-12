using Pospos.Core.Attributes;
using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    [TableName("City")]
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public bool DefaultValue { get; set; }
    }
}
