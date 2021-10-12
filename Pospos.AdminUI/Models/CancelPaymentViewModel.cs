namespace Pospos.AdminUI.Models
{
    public class CancelPaymentViewModel
    {
        public int Id { get; set; }
        public bool FirstCancel { get; set; } = false;
        public bool SecondCancel { get; set; } = false;
        public bool EndCancel { get; set; } = false;
    }
}
