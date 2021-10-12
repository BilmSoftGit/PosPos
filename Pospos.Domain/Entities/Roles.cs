using Pospos.Core.Attributes;
using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    [TableName("Roles")]
    public class Roles : BaseEntity
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
        public bool IsSystemRole { get; set; }
    }
}
