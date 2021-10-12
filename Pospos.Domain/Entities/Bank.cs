using Pospos.Core.Attributes;
using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    [TableName("Bank")]
    public class Bank : DetailedBaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string CardPictureUrl { get; set; }
        public int SortOrder { get; set; }
        public string SmallLogo { get; set; }
        public string BigLogo { get; set; }
        public string BackGroundColor { get; set; }
        public string FontColor { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}
