using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Pospos.Core.Common;
using Pospos.Core.Modules;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pospos.Api.Filters
{
    public class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        private readonly LogManager _logger;
        public ExceptionHandlerFilterAttribute(LogManager logger)
        {
            _logger = logger;
        }

        public async override Task OnExceptionAsync(ExceptionContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            _logger.InternalError(new Core.Common.LogObject()
            {
                FirstLevelCategory = controllerActionDescriptor.MethodInfo.ReflectedType.Name,
                Request = await FormatRequest(context.HttpContext.Request),
                Response = context.Exception,
                ErrorObject = context.Exception,
                SecondLevelCategory = controllerActionDescriptor.MethodInfo.Name,
                ThirdLevelCategory = context.Exception.GetType().Name,
                Description = $"{controllerActionDescriptor.MethodInfo.ReflectedType.Name}_{controllerActionDescriptor.MethodInfo.Name}"
            });

            context.ExceptionHandled = true;
            BaseApiResponse resp = new BaseApiResponse();
            resp.ErrorList.Add("Internall Error...");
            context.Result = new JsonResult(resp)
            {
                StatusCode = (int?)HttpStatusCode.OK
            };
            base.OnException(context);
        }

        private async Task<string> FormatRequest(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            request.EnableBuffering();
            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;
            return bodyAsText;
        }
    }
}
