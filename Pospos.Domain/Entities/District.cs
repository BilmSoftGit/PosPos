using Pospos.Core.Attributes;
using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    [TableName("District")]
    public class District : BaseEntity
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public bool DefaultValue { get; set; }
    }
}
