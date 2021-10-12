using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Pospos.AdminUI.ViewComponents
{
    public class PaymentCancelViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
