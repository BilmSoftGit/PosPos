using Microsoft.AspNetCore.Mvc;
using Pospos.Core.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pospos.Api.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi =true)]
        protected async Task<T> PushValidationErrors<T>() where T : BaseApiResponse
        {
            var resp = (T)Activator.CreateInstance(typeof(T));
            var errors = ModelState.Values.Select(x => x.Errors);
            if(errors.FirstOrDefault() != null)
            {
                resp.ErrorList = errors.SelectMany(x => x).Select(e => e.ErrorMessage).ToList();
            }
            return resp;
        }
    }
}
