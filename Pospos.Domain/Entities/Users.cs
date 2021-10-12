using Pospos.Core.Attributes;
using Pospos.Core.Common;
using System;

namespace Pospos.Domain.Entities
{
    [TableName("Users")]
    public class Users : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EMailAddress { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpireDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime PasswordExpireDate { get; set; }
        public bool IsApproved { get; set; }
        public bool? EMailApproved { get; set; }
        public bool? PhoneApproved { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
        public DateTime? EMailTokenExpireDate { get; set; }
        public DateTime? PhoneTokenExpireDate { get; set; }
        public string EMailToken { get; set; }
        public string PhoneToken { get; set; }
    }
}
