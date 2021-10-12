using Pospos.Core.Common;
using Pospos.Core.Modules;
using Pospos.Data.Repositories;
using Pospos.Domain.DataTransferObjects;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Service
{
    public class PaymentProcessService : BaseService
    {
        private readonly PaymentProcessRepository _paymentProcessRepository;

        public PaymentProcessService(LogManager logger, PaymentProcessRepository paymentProcessRepository) : base(logger)
        {
            this._paymentProcessRepository = paymentProcessRepository;
        }

        public async Task<ResponseBase<IEnumerable<sp_SearchPaymentProcess>>> SearchForDataTablePaymentProcess(int length, int page, string sortColumn, string sortColumnAscDesc, string search, string Username, string CardHolder, string CardNumber, int? BankId = null, int? PaymentTypeId = null, int? PosId = null, int? CompanyId = null, bool? IsApproved = null)
        {
            return await ExecuteAsync(() => _paymentProcessRepository.SearchForDataTablePaymentProcess(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, PaymentTypeId, PosId, CompanyId, IsApproved),
                "PaymentProcessService-SearchForDataTablePaymentProcess",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "SearchForDataTablePaymentProcess",
                true);
        }

        public async Task<ResponseBase<IEnumerable<sp_SearchPaymentProcess>>> SearchForDataTablePaymentProcessForExcel(int length, int page, string sortColumn, string sortColumnAscDesc, string search, string Username, string CardHolder, string CardNumber, int? BankId = null, int? PaymentTypeId = null, int? PosId = null, int? CompanyId = null, bool? IsApproved = null)
        {
            return await ExecuteAsync(() => _paymentProcessRepository.SearchForDataTablePaymentProcessForExcel(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, PaymentTypeId, PosId, CompanyId, IsApproved),
                "PaymentProcessService-SearchForDataTablePaymentProcess",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "SearchForDataTablePaymentProcess",
                true);
        }

        public async Task<ResponseBase<PaymentProcess>> GetById(int Id)
        {
            return await ExecuteAsync(() => _paymentProcessRepository.GetById(Id),
                "PaymentProcessService-GetById",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetById",
                true);
        }
        public async Task<ResponseBase<PaymentProcess>> GetByToken(string Token)
        {
            return await ExecuteAsync(() => _paymentProcessRepository.GetByToken(Token),
                "PaymentProcessService-GetByToken",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetByToken",
                true);
        }
        public async Task<ResponseBase<bool>> Update(PaymentProcess entity)
        {
            return await ExecuteAsync(() => _paymentProcessRepository.Update(entity),
                "PaymentProcessService-Update",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "Update",
                true);
        }

        public async Task<ResponseBase<PaymentProcess>> Insert(PaymentProcess process)
        {
            return await ExecuteAsync(() => _paymentProcessRepository.Insert(process),
                "PaymentProcessService-Insert",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "Insert",
                true,
                process);
        }
    }
}
