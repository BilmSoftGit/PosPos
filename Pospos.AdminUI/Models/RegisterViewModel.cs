using Pospos.Domain.Entities;
using System.Collections.Generic;

namespace Pospos.AdminUI.Models
{
    public class RegisterViewModel : BaseViewModel
    {
        public IEnumerable<City> Cities { get; set; }
    }
}
