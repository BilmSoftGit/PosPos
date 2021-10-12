using Pospos.Core.Common;
using Pospos.Core.Helpers;
using Pospos.Core.Modules;
using Pospos.Data.Repositories;
using Pospos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Service
{
    public class InstallmentService : BaseService
    {
        private readonly Utility _utility;
        private readonly InstallmentRepository _repository;
        public InstallmentService(LogManager logger, InstallmentRepository repository, Utility utility) : base(logger)
        {
            _repository = repository;
            _utility = utility;
        }

        public async Task<ResponseBase<Installment>> GetById(int Id)
        {
            return await ExecuteAsync(() => _repository.GetById(Id),
                "InstallmentService-GetById",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetById",
                true,
                new { Id = Id});
        }

        public async Task<ResponseBase<bool>> Update(Installment entity)
        {
            entity.UpdatedDate = DateTime.Now;
            //entity.UpdateUserId = 0;
            return await ExecuteAsync(() => _repository.Update(entity),
                "InstallmentService-Update",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "Update",
                true,
                entity);
        }

        public async Task<ResponseBase<Installment>> Insert(Installment entity)
        {
            entity.InsertDate = DateTime.Now;
            //entity.InsertedUserId = 0;
            return await ExecuteAsync(() => _repository.Insert(entity),
                "InstallmentService-Update",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "Update",
                true,
                entity);
        }

        public async Task<ResponseBase<IEnumerable<Installment>>> GetAll()
        {
            return await ExecuteAsync(() => _repository.GetAll(),
                "InstallmentService-GetAll",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetAll",
                true);
        }

        public async Task<ResponseBase<IEnumerable<Installment>>> GetByBinCode(string binCode,int stationId = 1)
        {
            return await ExecuteAsync(() => _repository.GetByBinCode(binCode, stationId),
                "InstallmentService-GetByBinCode",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetAllByBank",
                true,
                new { binCode = binCode, stationId= stationId });
        }

    }
}
