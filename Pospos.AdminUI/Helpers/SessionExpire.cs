using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Pospos.Core.Helpers;
using System;

namespace Pospos.AdminUI.Helpers
{
    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = filterContext.HttpContext.Request.Path.ToString() + filterContext.HttpContext.Request.QueryString.Value;

            ////Development ortamında (lokalde) session'ı en yetkili kullanıcı için set ediyoruz...
            //if (filterContext.HttpContext.Request.Host.ToString().ToLower().Contains("localhost:"))
            //{
            //    LoginSessionData session2 = new LoginSessionData();
            //    session2.IsActive = true;
            //    session2.CreateDate = DateTime.Now;
            //    session2.NameSurname = string.Format("Erkan Domurcuk");
            //    session2.UserId = 4;
            //    SessionExtensions.Set<LoginSessionData>(filterContext.HttpContext.Session, "UserData", session2);
            //}

            LoginSessionData session = SessionExtensions.Get<LoginSessionData>(filterContext.HttpContext.Session, "UserData");

            if (session == null || session.UserId <= 0)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "action", "Login" },
                    { "controller", "Account" },
                    { "returnUrl", url }
                });
                return;
            }

            if (!session.IsActive && !filterContext.HttpContext.Request.Path.ToString().ToLower().Contains("change-password"))
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "action", "ChangePassword" },
                    { "controller", "Account" }
                });
                return;
            }
        }
    }
}
