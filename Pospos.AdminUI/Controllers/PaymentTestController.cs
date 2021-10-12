using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pospos.AdminUI.Models;
using Pospos.Business.Factories;
using Pospos.Domain.Model;

namespace Pospos.AdminUI.Controllers
{
    public class PaymentTestController : Controller
    {
        private readonly BankFactory _bankFactory;
        private readonly PaymentProcessFactory _paymentProcessFactory;
        public PaymentTestController(BankFactory bankFactory, PaymentProcessFactory paymentProcessFactory)
        {
            _bankFactory = bankFactory;
            _paymentProcessFactory = paymentProcessFactory;
        }

        [Route("paymenttest")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> GetBackground(string binCode)
        {
            return await _bankFactory.GetBackground(binCode);
        }

        [HttpPost]
        public ActionResult InstallmentTable(string binCode)
        {
            //InstallmentModel model = new InstallmentModel();
            //string reqJsonData = string.Empty;
            //try
            //{
            //    CreditCardBankBin bankBin = _creditCardBankBinService.GetBankBinByBinCode(binCode);

            //    if (bankBin == null)
            //    {
            //        model.HasBank = false;
            //        return PartialView("InstallmentTable", model);
            //    }
            //    PaymentProject paymentProject = _paymentProjectService.GetPaymentProjectByCode(request.ProjectCode);
            //    PaymentProjectType projectType = _paymentProjectTypeService.GetPaymentProjectType(paymentProject.Id, PaymentTypeCode.Kart);
            //    CreditCardPos bankVpos = _paymentProjectPosService.GetPaymentProjectPos(paymentProject.Id, PosType.DDD, bankBin.CreditCardBankId);

            //    request.OrderTotal = projectType.UsingPercentage == true ?
            //        (request.OrderTotal + request.OrderTotal * (projectType.AdditionalFee) / 100) :
            //        projectType.AdditionalFee + request.OrderTotal;

            //    var bank = bankBin.CreditCardBank;
            //    if (bankVpos != null)
            //    {
            //        var installmentList = _creditCardInstallmentService.GetCreditCardInstallmentsByPosId(paymentProject.Id, bankVpos.Id, request.MaxInstallment);
            //        if (installmentList != null)
            //        {
            //            model.Amount = Math.Round(request.OrderTotal, 2);
            //            foreach (var installment in installmentList)
            //            {
            //                decimal installmentAmount = (model.Amount * (1 + installment.TermDiffrence)) / installment.InstallmentCount;
            //                decimal termTotalAmount = (model.Amount * (1 + installment.TermDiffrence));

            //                installment.InstallmentAmount = Math.Round(installmentAmount, 2);
            //                installment.TotalTermAmount = Math.Round(termTotalAmount, 2);
            //            }
            //            installmentList = installmentList.Where(p => p.InstallmentCount <= request.MaxInstallment).ToList();
            //            model.CreditCardBank = bank;
            //            model.CreditCardInstallments = installmentList;
            //            model.HasBank = true;
            //        }
            //    }
            //    else
            //    {
            //        model.CreditCardBank = bank;
            //        model.Amount = Math.Round(request.OrderTotal, 2);
            //        model.CreditCardInstallments = new List<CreditCardInstallment>();
            //        model.HasBank = true;
            //    }
            //    model.PaymentProjectId = paymentProject.Id;
            //    model.Culture = request.LanguageCulture;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.AddLog(ex, enc);
            //}
            return null;
           // return PartialView("InstallmentTable", model);
        }

        [HttpPost]
        public async Task<ActionResult> PayByCard(PaymentTestRequestModel req)
        {
           var resp = await _paymentProcessFactory.InsertPaymentProcess(req);

            return RedirectToAction("Index", "PaymentTest");
        }
    }
}