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
    public class BankCardBinService : BaseService
    {
        private readonly Utility _utility;
        private readonly BankCardBinRepository _repository;
        public BankCardBinService(LogManager logger, BankCardBinRepository repository, Utility utility) : base(logger)
        {
            _repository = repository;
            _utility = utility;
        }

        public async Task<ResponseBase<BankCardBin>> GetById(int Id)
        {
            return await ExecuteAsync(() => _repository.GetById(Id),
                "BankCardBinService-GetById",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetById",
                true,
                new { Id = Id});
        }

        public async Task<ResponseBase<bool>> Update(BankCardBin entity)
        {
            entity.UpdatedDate = DateTime.Now;
            //entity.UpdateUserId = 0;
            return await ExecuteAsync(() => _repository.Update(entity),
                "BankCardBinService-Update",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "Update",
                true,
                entity);
        }

        public async Task<ResponseBase<BankCardBin>> Insert(BankCardBin entity)
        {
            entity.InsertDate = DateTime.Now;
            //entity.InsertedUserId = 0;
            return await ExecuteAsync(() => _repository.Insert(entity),
                "BankCardBinService-Update",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "Update",
                true,
                entity);
        }

        public async Task<ResponseBase<IEnumerable<BankCardBin>>> GetAll()
        {
            return await ExecuteAsync(() => _repository.GetAll(),
                "BankCardBinService-GetAll",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetAll",
                true);
        }

        public async Task<ResponseBase<IEnumerable<BankCardBin>>> GetAllByBank(int bankId)
        {
            return await ExecuteAsync(() => _repository.GetAllByBank(bankId),
                "BankCardBinService-GetAllByBank",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetAllByBank",
                true,
                new { bankId = bankId });
        }

    }
}
