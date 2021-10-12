using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    public class Permissions : BaseEntity
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
    }
}
