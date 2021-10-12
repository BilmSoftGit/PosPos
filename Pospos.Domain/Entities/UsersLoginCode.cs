using Pospos.Core.Attributes;
using Pospos.Core.Common;
using System;

namespace Pospos.Domain.Entities
{
    [TableName("UsersLoginCode")]
    public class UsersLoginCode : BaseEntity
    {
        public int? UserId { get; set; }
        public string Token { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? PassiveDate { get; set; }

    }
}
