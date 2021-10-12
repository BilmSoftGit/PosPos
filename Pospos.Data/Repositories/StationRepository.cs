using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class StationRepository : CustomRepository
    {
        public StationRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<Station> GetById(int Id)
        {
            return await GetByIdAsync<Station>(Id);
        }

        public async Task<bool> Update(Station entity)
        {
            return await UpdateAsync<Station>(entity);
        }

        public async Task<Station> Insert(Station entity)
        {
            return await InsertAsync(entity);
        }
        public async Task<IEnumerable<Station>> GetAll()
        {
            return await QeryAsync<Station>("SELECT * FROM [Station] with (NOLOCK) where IsDeleted = 0", null);
        }
    }
}
