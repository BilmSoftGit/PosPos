using Pospos.Domain.Entities;
using System.Collections.Generic;

namespace Pospos.AdminUI.Models
{
    public class PaymentCancelListViewModel : BaseViewModel
    {
        public IEnumerable<Bank> Banks { get; set; }
        public IEnumerable<PaymentType> PaymentTypes { get; set; }
        public IEnumerable<Pos> Poses { get; set; }
    }
}
