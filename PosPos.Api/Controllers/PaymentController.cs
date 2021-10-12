using Microsoft.AspNetCore.Mvc;
using Pospos.Api.Models.Request;
using Pospos.Api.Models.Response;
using Pospos.Business.Factories;
using Pospos.Core.Common;
using Pospos.Data.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pospos.Api.Controllers
{
    [ApiController]
    public class PaymentController : BaseApiController
    {
        private readonly BankFactory _bankFactory;
        public PaymentController(BankFactory bankFactory)
        {
            _bankFactory = bankFactory;
        }

        [Route("get_test_data")]
        [HttpGet]
        public async Task<BinTestRequest> gettestdata()
        {
            BinTestRequest resp = new BinTestRequest();
            resp.BinCode = "554960";
            return resp;
        }

        [Route("Get_BackGround")]
        [HttpPost]
        public async Task<ResponseBase<String>> Get_BackGround(BinTestRequest req)
        {
            ResponseBase<String> resp = new ResponseBase<string>();
            resp.data = await _bankFactory.GetBackground(req.BinCode);
            return resp;
        }
    }
}
