using Pospos.Core.Attributes;
using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    [TableName("RolesPermissions")]
    public class RolesPermissions : BaseEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
