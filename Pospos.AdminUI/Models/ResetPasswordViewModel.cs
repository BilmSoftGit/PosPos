namespace Pospos.AdminUI.Models
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        public string Token { get; set; }
        public bool SuccessToken { get; set; } = false;
    }
}
