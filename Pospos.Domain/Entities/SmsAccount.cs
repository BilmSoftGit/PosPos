using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    public class SmsAccount : BaseEntity
    {
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Orginator { get; set; }
        public string AccountNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ShortNumber { get; set; }
        public string Isunicode { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
        public int? Operator { get; set; }
        public bool IsActive { get; set; }
    }
}
