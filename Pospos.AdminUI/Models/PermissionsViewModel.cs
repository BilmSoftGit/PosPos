using Pospos.Domain.Entities;
using System.Collections.Generic;

namespace Pospos.AdminUI.Models
{
    public class PermissionsViewModel : BaseViewModel
    {
        public int editRoleId { get; set; }
        public IEnumerable<Roles> Roles { get; set; }
        public IEnumerable<Permissions> Permissions { get; set; }
        public IEnumerable<RolesPermissions> RolesPermissions { get; set; }
    }
}
