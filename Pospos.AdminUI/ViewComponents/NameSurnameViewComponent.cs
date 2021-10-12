using Microsoft.AspNetCore.Mvc;
using Pospos.AdminUI.Helpers;
using Pospos.Core.Helpers;
using System.Threading.Tasks;

namespace Pospos.AdminUI.ViewComponents
{
    public class NameSurnameViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loginSessionData = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");
            if (loginSessionData != null)
            {
                ViewBag.NameSurname = loginSessionData.NameSurname;
            }

            return View();
        }
    }
}
