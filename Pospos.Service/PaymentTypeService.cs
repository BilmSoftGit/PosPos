using Pospos.Core.Common;
using Pospos.Core.Modules;
using Pospos.Data.Repositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Service
{
    public class PaymentTypeService : BaseService
    {
        private readonly PaymentTypeRepository _paymentTypeRepository;

        public PaymentTypeService(LogManager logger, PaymentTypeRepository paymentTypeRepository) : base(logger)
        {
            this._paymentTypeRepository = paymentTypeRepository;
        }

        public async Task<ResponseBase<IEnumerable<PaymentType>>> GetAll()
        {
            return await ExecuteAsync(() => _paymentTypeRepository.GetAll(),
                "PaymentProcessService-GetAll",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetAll",
                true);
        }

        public async Task<ResponseBase<PaymentType>> GetById(int Id)
        {
            return await ExecuteAsync(() => _paymentTypeRepository.GetById(Id),
                "PaymentProcessService-GetById",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetById",
                true);
        }
    }
}
