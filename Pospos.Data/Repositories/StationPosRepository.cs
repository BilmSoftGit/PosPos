using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class StationPosRepository : CustomRepository
    {
        public StationPosRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<StationPos> GetById(int Id)
        {
            return await GetByIdAsync<StationPos>(Id);
        }

        public async Task<bool> Update(StationPos entity)
        {
            return await UpdateAsync<StationPos>(entity);
        }

        public async Task<StationPos> Insert(StationPos entity)
        {
            return await InsertAsync(entity);
        }
        public async Task<IEnumerable<StationPos>> GetAll()
        {
            return await QeryAsync<StationPos>("SELECT * FROM [StationPos] with (NOLOCK) where IsDeleted = 0", null);
        }

        public async Task<IEnumerable<StationPos>> Search(int posId = 0,int stationId = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("deleted", 0);
            string query = "SELECT * FROM [StationPos] with (NOLOCK) where IsDeleted = @deleted";
            if(posId > 0)
            {
                query += " and PosId= @posId";
                param.Add("posId", posId);
            }
            if (stationId > 0)
            {
                query += " and StationId= @stationId";
                param.Add("stationId", stationId);
            }
            return await QeryAsync<StationPos>(query, param);
        }
    }
}
