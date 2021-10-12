using Pospos.Core.Helpers;
using Pospos.Domain.DataTransferObjects;
using Pospos.Domain.Entities;
using Pospos.Domain.Model;
using Pospos.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Business.Factories
{
    public class PaymentProcessFactory
    {
        private readonly PaymentProcessService _paymentProcessService;
        private readonly Utility _utility;
        private readonly SecurityHelper _security;
        public PaymentProcessFactory(PaymentProcessService paymentProcessService, Utility utility, SecurityHelper security)
        {
            this._paymentProcessService = paymentProcessService;
            _utility = utility;
            _security = security;
        }

        public async Task<IEnumerable<sp_SearchPaymentProcess>> SearchForDataTablePaymentProcess(int length, int page, string sortColumn, string sortColumnAscDesc, string search, string Username, string CardHolder, string CardNumber, int? BankId = null, int? PaymentTypeId = null, int? PosId = null, int? CompanyId = null, bool? IsApproved = null)
        {
            var result = await _paymentProcessService.SearchForDataTablePaymentProcess(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, PaymentTypeId, PosId, CompanyId, IsApproved);

            return result.data;
        }

        public async Task<IEnumerable<sp_SearchPaymentProcess>> SearchForDataTablePaymentProcessForExcel(int length, int page, string sortColumn, string sortColumnAscDesc, string search, string Username, string CardHolder, string CardNumber, int? BankId = null, int? PaymentTypeId = null, int? PosId = null, int? CompanyId = null, bool? IsApproved = null)
        {
            var result = await _paymentProcessService.SearchForDataTablePaymentProcessForExcel(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, PaymentTypeId, PosId, CompanyId, IsApproved);

            return result.data;
        }

        public async Task<PaymentProcess> GetById(int Id)
        {
            var result = await _paymentProcessService.GetById(Id);

            return result.data;
        }
        public async Task<PaymentProcess> GetByToken(string Token)
        {
            var result = await _paymentProcessService.GetByToken(Token);

            return result.data;
        }

        public async Task<bool> UpdatePaymentCancellationLevel(int Id, CancellationLevel CancellationLevel)
        {
            var entity = await _paymentProcessService.GetById(Id);
            if (entity.data != null)
            {
                entity.data.CancellationLevel = (int)CancellationLevel;
                return _paymentProcessService.Update(entity.data).Result.data;
            }

            return false;
        }

        public async Task<PaymentProcess> InsertPaymentProcess(PaymentTestRequestModel req)
        {
            PaymentProcess process = new PaymentProcess()
            {
                CardHolder = req.CardholderName,
                CardNumber = req.CardNumber1 + req.CardNumber2 + req.CardNumber3 + req.CardNumber4,
                InsertDate = DateTime.Now,
                InstalmentCount = "1",
                StationTransactionToken = _utility.GetOrderNumber(),
                CustomerId = "1",
                Cavv = req.CardCode,
                CardName = req.CreditCardType,
                ClientIp = _security.GetClientIpAddress(),
                Culture = "tr-TR",
                CurrencyCode = "TL",
                PosId = 7,
                BankId = 3,
                IsApproved = false,
                StationId = 1,
                TotalAmount = "100"
            };
            var result = await _paymentProcessService.Insert(process);

            return result.data;
        }

        public async Task<string> CreatePaymentToken(int Id)
        {
            string guid = Guid.NewGuid().ToString();

            var payment = await _paymentProcessService.GetById(Id);
            if (payment.Status)
            {
                payment.data.Token = guid;
                payment.data.TokenExpireDate = DateTime.Now.AddDays(1);
                var result = await _paymentProcessService.Update(payment.data);
                if (result.data)
                {
                    return guid;
                }
            }

            return string.Empty;
        }
    }
}
