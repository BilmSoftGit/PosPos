using Pospos.Core.Helpers;
using Pospos.Domain.DataTransferObjects;
using Pospos.Domain.Entities;
using Pospos.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Business.Factories
{
    public class PaymentProcessCancelFactory
    {
        private readonly PaymentProcessCancelService _paymentProcessCancelService;
        private readonly PaymentProcessService _paymentProcessService;
        private readonly PaymentProcessFactory _paymentProcessFactory;

        public PaymentProcessCancelFactory(PaymentProcessCancelService paymentProcessCancelService, PaymentProcessService paymentProcessService,
            PaymentProcessFactory paymentProcessFactory)
        {
            this._paymentProcessCancelService = paymentProcessCancelService;
            this._paymentProcessService = paymentProcessService;
            this._paymentProcessFactory = paymentProcessFactory;
        }

        public async Task<PaymentProcessCancel> GetById(int Id)
        {
            var result = await _paymentProcessCancelService.GetById(Id);

            return result.data;
        }

        public async Task<IEnumerable<PaymentProcessCancel>> GetAll(int Id)
        {
            var result = await _paymentProcessCancelService.GetAll();

            return result.data;
        }

        public async Task<PaymentProcessCancel> Insert(PaymentProcessCancel entity)
        {
            var result = await _paymentProcessCancelService.Insert(entity);

            return result.data;
        }

        public async Task<IEnumerable<sp_SearchPaymentProcessCancel>> SearchForDataTablePaymentProcessCancel(int length, int page, string sortColumn, string sortColumnAscDesc, string search, string Username, string CardHolder, string CardNumber, int? BankId = null, int? PaymentTypeId = null, int? PosId = null, int? CompanyId = null)
        {
            var result = await _paymentProcessCancelService.SearchForDataTablePaymentProcessCancel(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, PaymentTypeId, PosId, CompanyId);

            return result.data;
        }

        /// <summary>
        /// Tüm onaylar alındıktan sonra tam iptal işlemi için burası çalıştırılacak...
        /// </summary>
        /// <param name="Id">İptal edilecek PaymentProcess'in Id'si...</param>
        /// <returns></returns>
        public async Task<bool> CancelPayment(int Id)
        {
            var payment = await _paymentProcessService.GetById(Id);
            var data = payment.data;

            PaymentProcessCancel paymentProcessCancel = new PaymentProcessCancel
            {
                Alias = data.Alias,
                AuthCode = data.AuthCode,
                BankId = data.BankId,
                BankMerchantId = data.BankMerchantId,
                BankOrder = data.BankOrder,
                Browser = data.Browser,
                CardHolder = data.CardHolder,
                CardName = data.CardName,
                CardNumber = data.CardNumber,
                Cavv = data.Cavv,
                ChargeType = data.ChargeType,
                ClientId = data.ClientId,
                ClientIp = data.ClientIp,
                Culture = data.Culture,
                CurrencyCode = data.CurrencyCode,
                CustomerId = data.CustomerId,
                ErrorCode = data.ErrorCode,
                ErrorMessage = data.ErrorMessage,
                HandleMessage = data.HandleMessage,
                Hash = data.Hash,
                HostMessage = data.HostMessage,
                HostRefNum = data.HostRefNum,
                InsertDate = data.InsertDate,
                InstalmentCount = data.InstalmentCount,
                IsApproved = data.IsApproved,
                Md = data.Md,
                MdStatus = data.MdStatus,
                OrderRef = data.OrderRef,
                PaymentProcessId = data.Id,
                PaymentTypeId = data.PaymentTypeId,
                PosId = data.PosId,
                PosMessage = data.PosMessage,
                PosRef = data.PosRef,
                ProcReturnCode = data.ProcReturnCode,
                RefNo = data.RefNo,
                Rrn = data.Rrn,
                Signature = data.Signature,
                StationId = data.StationId,
                StationTransactionToken = data.StationTransactionToken,
                StoreType = data.StoreType,
                TerminalBank = data.TerminalBank,
                TimeStamp = data.TimeStamp,
                TotalAmount = data.TotalAmount,
                TransDate = DateTime.Now,
                TransId = data.TransId,
                UserName = data.UserName,
                XId = data.XId
            };

            var _paymentProcessCancel = await _paymentProcessCancelService.Insert(paymentProcessCancel);
            await _paymentProcessFactory.UpdatePaymentCancellationLevel(payment.data.Id, CancellationLevel.EndLevel);

            return _paymentProcessCancel.Status && _paymentProcessCancel.data != null;
        }
    }
}
