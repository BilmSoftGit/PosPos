using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class BankRepository : CustomRepository
    {
        public BankRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<Bank> GetById(int Id)
        {
            return await GetByIdAsync<Bank>(Id);
        }

        public async Task<bool> Update(Bank entity)
        {
            return await UpdateAsync<Bank>(entity);
        }

        public async Task<Bank> Insert(Bank entity)
        {
            return await InsertAsync(entity);
        }
        public async Task<IEnumerable<Bank>> GetAll()
        {
            return await QeryAsync<Bank>("SELECT * FROM [Bank] with (NOLOCK) where IsDeleted = 0 order by SortOrder ASC", null);
        }

        public async Task<IEnumerable<Bank>> GetAllActive()
        {
            return await QeryAsync<Bank>("SELECT * FROM [Bank] with (NOLOCK) where IsDeleted = 0 and IsActive = 1 order by SortOrder ASC", null);
        }

        public async Task<Bank> GetByBinNumber(string binCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("binCode", binCode);

            return await QeryFirstAsync<Bank>("SELECT TOP 1 bank.* FROM [Bank] bank with (NOLOCK) Inner Join [BankCardBin] bbin with (NOLOCK) where bank.IsDeleted = 0 and bank.IsActive = 1 and bbin.IsDeleted = 0 and bbin.IsActive = 1 and bbin.BinCode = @binCode", null);
        }
    }
}
