using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pospos.AdminUI.Helpers;
using Pospos.AdminUI.Models;
using Pospos.Business.Factories;
using Pospos.Core.Common;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Pospos.AdminUI.Controllers
{
    [SessionExpire]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CacheFactory _cacheFactory;

        public HomeController(ILogger<HomeController> logger, CacheFactory cacheFactory)
        {
            this._logger = logger;
            this._cacheFactory = cacheFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ViewGraphicsSummary))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
