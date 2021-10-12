using Pospos.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Pospos.AdminUI.Models
{
    public class CreateUpdateUserViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
        public string EMailAddress { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
        public DateTime PasswordExpireDate { get; set; }
        public bool IsApproved { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<Roles> Roles { get; set; }
        public IEnumerable<Roles> SelectedRoles { get; set; }
        public IEnumerable<Permissions> Permissions { get; set; }
        public IEnumerable<Permissions> SelectedPermissions { get; set; }
    }
}
