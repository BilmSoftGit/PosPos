using System.Threading.Tasks;

namespace Pospos.AdminUI.Helpers
{
    public interface IRecaptchaValidator
    {
        Task<bool> IsRecaptchaValid(string token);
    }
}
