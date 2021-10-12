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
    public class BankService : BaseService
    {
        private readonly Utility _utility;
        private readonly BankRepository _repository;
        public BankService(LogManager logger, BankRepository repository, Utility utility) : base(logger)
        {
            _repository = repository;
            _utility = utility;
        }

        public async Task<ResponseBase<Bank>> GetById(int Id)
        {
            return await ExecuteAsync(() => _repository.GetById(Id),
                "BankService-GetById",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetById",
                true,
                new { Id = Id});
        }

        public async Task<ResponseBase<bool>> Update(Bank entity)
        {
            entity.UpdatedDate = DateTime.Now;
            //entity.UpdateUserId = 0;
            return await ExecuteAsync(() => _repository.Update(entity),
                "BankService-Update",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "Update",
                true,
                entity);
        }

        public async Task<ResponseBase<Bank>> Insert(Bank entity)
        {
            entity.InsertDate = DateTime.Now;
            //entity.InsertedUserId = 0;
            return await ExecuteAsync(() => _repository.Insert(entity),
                "BankService-Update",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "Update",
                true,
                entity);
        }

        public async Task<ResponseBase<IEnumerable<Bank>>> GetAll()
        {
            return await ExecuteAsync(() => _repository.GetAll(),
                "BankService-GetAll",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetAll",
                true);
        }

        public async Task<ResponseBase<Bank>> GetBankByBinCode(string binCode)
        {
            return await ExecuteAsync(() => _repository.GetByBinNumber(binCode),
                "BankService-GetByBinNumber",
                ProcessTpes.Sql,
                ProcessGroup.BankDefinition,
                "GetAllByBank",
                true,
                new { binCode = binCode });
        }

    }
}
