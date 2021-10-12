using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class PosTypeRepository : CustomRepository
    {
        public PosTypeRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<PosType> GetById(int Id)
        {
            return await GetByIdAsync<PosType>(Id);
        }

        public async Task<bool> Update(PosType entity)
        {
            return await UpdateAsync<PosType>(entity);
        }

        public async Task<PosType> Insert(PosType entity)
        {
            return await InsertAsync(entity);
        }
        public async Task<IEnumerable<PosType>> GetAll()
        {
            return await QeryAsync<PosType>("SELECT * FROM [PosType] with (NOLOCK) where IsDeleted = 0", null);
        }
    }
}
