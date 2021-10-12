using Pospos.Core.Common;
using Pospos.Core.Modules;
using Pospos.Data.Repositories;
using Pospos.Domain.DataTransferObjects;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Service
{
    public class PaymentProcessCancelService : BaseService
    {
        private readonly PaymentProcessCancelRepository _repository;

        public PaymentProcessCancelService(LogManager logger, PaymentProcessCancelRepository repository) : base(logger)
        {
            this._repository = repository;
        }

        public async Task<ResponseBase<PaymentProcessCancel>> GetById(int Id)
        {
            return await ExecuteAsync(() => _repository.GetById(Id),
                "PaymentProcessCancelService-GetById",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetById",
                true,
                new { Id = Id });
        }

        public async Task<ResponseBase<IEnumerable<PaymentProcessCancel>>> GetAll()
        {
            return await ExecuteAsync(() => _repository.GetAll(),
                "PaymentProcessCancelService-GetAll",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetAll",
                true);
        }

        public async Task<ResponseBase<PaymentProcessCancel>> Insert(PaymentProcessCancel entity)
        {
            return await ExecuteAsync(() => _repository.Insert(entity),
                "PaymentProcessCancelService-Insert",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "Insert",
                true);
        }

        public async Task<ResponseBase<IEnumerable<sp_SearchPaymentProcessCancel>>> SearchForDataTablePaymentProcessCancel(int length, int page, string sortColumn, string sortColumnAscDesc, string search, string Username, string CardHolder, string CardNumber, int? BankId = null, int? PaymentTypeId = null, int? PosId = null, int? CompanyId = null)
        {
            return await ExecuteAsync(() => _repository.SearchForDataTablePaymentProcessCancel(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, PaymentTypeId, PosId, CompanyId),
                "PaymentProcessCancelService-SearchForDataTablePaymentProcessCancel",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "SearchForDataTablePaymentProcessCancel",
                true);
        }
    }
}
