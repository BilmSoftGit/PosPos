using Microsoft.AspNetCore.Http;

namespace Pospos.AdminUI.Helpers
{
    public class HtmlRequestUrl
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        string UrlPath = string.Empty;

        public HtmlRequestUrl(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public string GetMainMenuControl(string ThisPath)
        {
            this.UrlPath = _httpContextAccessor.HttpContext.Request.Path.ToString();

            return UrlPath.ToLower() == ThisPath.ToLower() || UrlPath.ToLower().Contains(ThisPath.ToLower()) ? "menu-open" : string.Empty;
        }

        public string GetBottomMenuControl(string ThisPath)
        {
            this.UrlPath = _httpContextAccessor.HttpContext.Request.Path.ToString();

            return UrlPath.ToLower() == ThisPath.ToLower() || UrlPath.ToLower().Contains(ThisPath.ToLower() + "/") ? "active" : string.Empty;
        }

        public string GetActiveMenuControl(string ThisPath)
        {
            this.UrlPath = _httpContextAccessor.HttpContext.Request.Path.ToString();

            return UrlPath.ToLower() == ThisPath.ToLower() ? "active" : string.Empty;
        }
    }
}
