using System;

namespace Pospos.Core.Helpers
{
    public class LoginSessionData
    {
        public int UserId { get; set; }
        public string NameSurname { get; set; }
        public string ProfilPhoto { get; set; }
        public DateTime CreateDate { get; set; }
        public string Token { get; set; }
        public int CompanyId { get; set; }

        //Şifresinin süresi olmuş ise bu alan false olarak gelecek. Şifresini değiştirene kadar da false olarak kalacak.
        public bool IsActive { get; set; } = true;
        public string Username { get; set; }
    }
}
