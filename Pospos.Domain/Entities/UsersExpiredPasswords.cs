using Pospos.Core.Attributes;
using Pospos.Core.Common;
using System;

namespace Pospos.Domain.Entities
{
    [TableName("UsersExpiredPasswords")]
    public class UsersExpiredPasswords : BaseEntity
    {
        public int UserId { get; set; }

        public string Password { get; set; }

        public DateTime ExpireDate { get; set; }

    }

}
