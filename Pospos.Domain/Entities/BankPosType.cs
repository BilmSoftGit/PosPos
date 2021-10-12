using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    public class BankPosType : DetailedBaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
