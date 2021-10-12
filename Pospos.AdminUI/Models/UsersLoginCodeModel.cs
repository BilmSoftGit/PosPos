using Pospos.Domain.Entities;
using System;

namespace Pospos.AdminUI.Models
{
    public class UsersLoginCodeModel : UsersLoginCode
    {
        public DateTime NewResendStartDate { get; set; }
        public int Timeout { get; set; }
    }
}
