using Pospos.Domain.Entities;
using Pospos.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pospos.Business.Factories
{
    public class BankFactory
    {
        private readonly BankService _bankService;
        private readonly InstallmentService _installmentService;
        public BankFactory(BankService bankService, InstallmentService installmentService)
        {
            _bankService = bankService;
            _installmentService = installmentService;
        }

        public async Task<string> GetBackground(string binCode)
        {
            var bank = await _bankService.GetBankByBinCode(binCode);
            if(bank.Status)
            {
                if(!string.IsNullOrWhiteSpace(bank.data?.CardPictureUrl))
                {
                    return $"/Content/BankCardImages{bank.data.CardPictureUrl}";
                }
            }
            return "/Content/images/card_sample.png";
        }

        //public async Task<InstallmentModel> InstallmentTable(string binCode)
        //{
        //    InstallmentModel model = new InstallmentModel();
        //    string reqJsonData = string.Empty;
        //    try
        //    {
        //        CreditCardBankBin bankBin = _creditCardBankBinService.GetBankBinByBinCode(binCode);

        //        if (bankBin == null)
        //        {
        //            model.HasBank = false;
        //            return PartialView("InstallmentTable", model);
        //        }
        //        PaymentProject paymentProject = _paymentProjectService.GetPaymentProjectByCode(request.ProjectCode);
        //        PaymentProjectType projectType = _paymentProjectTypeService.GetPaymentProjectType(paymentProject.Id, PaymentTypeCode.Kart);
        //        CreditCardPos bankVpos = _paymentProjectPosService.GetPaymentProjectPos(paymentProject.Id, PosType.DDD, bankBin.CreditCardBankId);

        //        request.OrderTotal = projectType.UsingPercentage == true ?
        //            (request.OrderTotal + request.OrderTotal * (projectType.AdditionalFee) / 100) :
        //            projectType.AdditionalFee + request.OrderTotal;

        //        var bank = bankBin.CreditCardBank;
        //        if (bankVpos != null)
        //        {
        //            var installmentList = _creditCardInstallmentService.GetCreditCardInstallmentsByPosId(paymentProject.Id, bankVpos.Id, request.MaxInstallment);
        //            if (installmentList != null)
        //            {
        //                model.Amount = Math.Round(request.OrderTotal, 2);
        //                foreach (var installment in installmentList)
        //                {
        //                    decimal installmentAmount = (model.Amount * (1 + installment.TermDiffrence)) / installment.InstallmentCount;
        //                    decimal termTotalAmount = (model.Amount * (1 + installment.TermDiffrence));

        //                    installment.InstallmentAmount = Math.Round(installmentAmount, 2);
        //                    installment.TotalTermAmount = Math.Round(termTotalAmount, 2);
        //                }
        //                installmentList = installmentList.Where(p => p.InstallmentCount <= request.MaxInstallment).ToList();
        //                model.CreditCardBank = bank;
        //                model.CreditCardInstallments = installmentList;
        //                model.HasBank = true;
        //            }
        //        }
        //        else
        //        {
        //            model.CreditCardBank = bank;
        //            model.Amount = Math.Round(request.OrderTotal, 2);
        //            model.CreditCardInstallments = new List<CreditCardInstallment>();
        //            model.HasBank = true;
        //        }
        //        model.PaymentProjectId = paymentProject.Id;
        //        model.Culture = request.LanguageCulture;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.AddLog(ex, enc);
        //    }

        //    return PartialView("InstallmentTable", model);
        //}

        public async Task<IEnumerable<Bank>> GetAll()
        {
            var result = await _bankService.GetAll();

            return result.data;
        }

        public async Task<Bank> GetById(int Id)
        {
            var result = await _bankService.GetById(Id);

            return result.data;
        }
    }
}
