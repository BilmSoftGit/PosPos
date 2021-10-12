using System;

namespace Pospos.Domain.DataTransferObjects
{
    public class sp_GetTopUsersWithUserId
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
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
    }
}
