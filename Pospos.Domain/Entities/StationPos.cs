using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    public class StationPos : DetailedBaseEntity
    {
        public int StationId { get; set; }
        public int PosId { get; set; }
        public bool IsActive { get; set; }
    }
}
