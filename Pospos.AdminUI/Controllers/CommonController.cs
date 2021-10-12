using Microsoft.AspNetCore.Mvc;
using Pospos.Business.Factories;
using Pospos.Core.Common;
using Pospos.Core.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace Pospos.AdminUI.Controllers
{
    public class CommonController : Controller
    {
        private readonly CommonFactory _commonFactory;
        private readonly CacheFactory _cacheFactory;

        public CommonController(CommonFactory commonFactory, CacheFactory cacheFactory)
        {
            this._commonFactory = commonFactory;
            this._cacheFactory = cacheFactory;
        }

        [Route("get-districts")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetDistricts(int cityId)
        {
            var result = await _commonFactory.GetDisctricts(cityId);

            var data = new Select2ViewData();

            data.results = result.Select(x => new Select2InData { id = x.Id, text = x.Name }).ToList();

            return Json(new { items = data.results });
        }

        private string[] whiteList =
        {
            "",
            "/",
            "/payments/list",
            "/payments/bank/list",
            "/payments/canceled-payments",
            "/users",
            "/users/roles",
            "/users/role-permissions",
            "/member-businesses",
            "/member-businesses/create-update",
            "/users/create-update"
        };

        public async Task<IActionResult> SecurityReturnUrl(string returnUrl)
        {
            if (!whiteList.Contains(returnUrl.ToLower()))
                return BadRequest();
            else
                return Redirect(returnUrl);
        }

        [Route("ClearCache")]
        public async Task<IActionResult> ClearCache(string ReturnUrl)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ClearCache))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            await _cacheFactory.ClearCache();

            if (!string.IsNullOrWhiteSpace(ReturnUrl))
                await SecurityReturnUrl(ReturnUrl);

            return Redirect("/");
        }
    }
}
