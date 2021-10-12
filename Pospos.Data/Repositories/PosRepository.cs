using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class PosRepository : CustomRepository
    {
        public PosRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<Pos> GetById(int Id)
        {
            return await GetByIdAsync<Pos>(Id);
        }

        public async Task<bool> Update(Pos entity)
        {
            return await UpdateAsync<Pos>(entity);
        }

        public async Task<Pos> Insert(Pos entity)
        {
            return await InsertAsync(entity);
        }
        public async Task<IEnumerable<Pos>> GetAll()
        {
            return await QeryAsync<Pos>("SELECT * FROM [Pos] with (NOLOCK) where IsDeleted = 0 order by CreditCardBankName asc", null);
        }

        public async Task<IEnumerable<Pos>> SearchAll(int bankId = 0,int posTypeId = 0,int bankTypeId = 0,bool isActive= true)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("isActive", isActive);
            string query = "SELECT* FROM [Pos] with(NOLOCK) where IsDeleted = 0 and IsActive = @isActive";
            if(bankId > 0)
            {
                query += " and BankId = @bankId";
                param.Add("bankId", bankId);
            }
            if (posTypeId > 0)
            {
                query += " and PosTypeId = @posTypeId";
                param.Add("posTypeId", posTypeId);
            }
            if (bankTypeId > 0)
            {
                query += " and BankTypeId = @bankTypeId";
                param.Add("bankTypeId", bankTypeId);
            }
            return await QeryAsync<Pos>(query, param);
        }
    }
}
