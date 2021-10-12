using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class SettingRepository : CustomRepository
    {
        public SettingRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<Setting> GetById(int Id)
        {
            return await GetByIdAsync<Setting>(Id);
        }

        public async Task<bool> Update(Setting entity)
        {
            return await UpdateAsync<Setting>(entity);
        }

        public async Task<Setting> Insert(Setting entity)
        {
            return await InsertAsync(entity);
        }
        public async Task<IEnumerable<Setting>> GetAll()
        {
            return await QeryAsync<Setting>("SELECT * FROM [Setting] with (NOLOCK)", null);
        }

        public async Task<IEnumerable<Setting>> Search(string key,string culture)
        {
            DynamicParameters param = new DynamicParameters();
            string query = "SELECT * FROM [Setting] with (NOLOCK) with (NOLOCK) where 1=1";
            if(!string.IsNullOrWhiteSpace(key))
            {
                param.Add("key", key);
                query += " and [Key]=@key";
            }

            if (!string.IsNullOrWhiteSpace(culture))
            {
                param.Add("culture", culture);
                query += " and [Culture]=@culture";
            }
            return await QeryAsync<Setting>(query, param);
        }
    }
}
