namespace Pospos.AdminUI.Models
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            isLoggedOut = false;
        }

        public LoginViewModel(bool _isLoggedOut, string _returnUrl)
        {
            isLoggedOut = _isLoggedOut;
            returnUrl = _returnUrl;
        }

        public bool isLoggedOut { get; set; } = false;
        public string returnUrl { get; set; }
    }
}
