using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    public class PanelSetting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
