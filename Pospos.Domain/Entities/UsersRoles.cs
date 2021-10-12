using Pospos.Core.Attributes;
using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    [TableName("UsersRoles")]
    public class UsersRoles : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
