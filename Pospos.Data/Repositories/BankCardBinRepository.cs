using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class BankCardBinRepository : CustomRepository
    {
        public BankCardBinRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<BankCardBin> GetById(int Id)
        {
            return await GetByIdAsync<BankCardBin>(Id);
        }

        public async Task<bool> Update(BankCardBin entity)
        {
            return await UpdateAsync<BankCardBin>(entity);
        }

        public async Task<BankCardBin> Insert(BankCardBin entity)
        {
            return await InsertAsync(entity);
        }
        public async Task<IEnumerable<BankCardBin>> GetAll()
        {
            return await QeryAsync<BankCardBin>("SELECT * FROM [BankCardBin] with (NOLOCK) where IsDeleted = 0 order by BinCode ASC", null);
        }

        public async Task<IEnumerable<BankCardBin>> GetAllByBank(int bankId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("bankId", bankId);
            return await QeryAsync<BankCardBin>("SELECT * FROM [BankCardBin] with (NOLOCK) where BankId = @bankId and IsDeleted = 0 order by BinCode ASC", parameters);
        }
    }
}
