using Pospos.Domain.Entities;
using System.Collections.Generic;

namespace Pospos.AdminUI.Models
{
    public class ApproveContactsViewModel : BaseViewModel
    {
        public int UserId { get; set; }
        public string SMSToken { get; set; }
        public string EMailToken { get; set; }
        public bool smsenable { get; set; }
        public bool emailenable { get; set; }

        public List<Users> kullanicilar { get; set; }
    }
}
