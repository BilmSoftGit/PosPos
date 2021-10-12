using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class BankPosTypeRepository : CustomRepository
    {
        public BankPosTypeRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<BankPosType> GetById(int Id)
        {
            return await GetByIdAsync<BankPosType>(Id);
        }

        public async Task<bool> Updaten(BankPosType entity)
        {
            return await UpdateAsync<BankPosType>(entity);
        }
        public async Task<BankPosType> Insert(BankPosType entity)
        {
            return await InsertAsync(entity);
        }

        public async Task<IEnumerable<BankPosType>> GetAll()
        {
            return await QeryAsync<BankPosType>("SELECT * FROM [BankPosType] with (NOLOCK) where IsDeleted = 0 order by Name ASC", null);
        }
    }
}
