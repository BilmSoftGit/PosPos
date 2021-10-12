using Pospos.Domain.Entities;

namespace Pospos.AdminUI.Models
{
    public class PaymentCanceledDetailViewModel
    {
        public PaymentProcessCancel PaymentProcessCancel { get; set; }
        public Bank Bank { get; set; }
        public PaymentType PaymentType { get; set; }
        public Pos Pos { get; set; }
        public Users User { get; set; }
        public Station Station { get; set; }
    }
}
