using Pospos.Core.Attributes;
using System;

namespace Pospos.Core.Common
{
    public class UserToken
    {
        [ClaimName("userId")]
        public string Id { get; set; }

        [ClaimName("Sid")]
        public DateTime ExpireDateTime { get; set; }

        [ClaimName("role")]
        public string RoleIds { get; set; }
    }
}
