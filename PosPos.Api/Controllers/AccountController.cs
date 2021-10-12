using Microsoft.AspNetCore.Mvc;
using Pospos.Api.Models.Request;
using Pospos.Api.Models.Response;
using Pospos.Core.Common;
using Pospos.Data.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pospos.Api.Controllers
{
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly UserRepository _userRepository;

        public AccountController(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [Route("login")]
        [HttpPost]
        public async Task<LoginRespModel> Login(LoginReqModel request)
        {
            var result = await _userRepository.GetLogin(request.Username, request.Password);

            if (result != null)
                return new LoginRespModel { Success = true, Message = "İşlem başarılı", User = result };
            else
                return new LoginRespModel { Success = false, Message = "Kullanıcı adı ve/veya şifre geçersiz!", User = result };
        }
    }
}
