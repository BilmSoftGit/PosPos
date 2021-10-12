using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class CommonRepository : CustomRepository
    {
        public CommonRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<IEnumerable<City>> GetCities()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await this.QeryAsync<City>("SELECT  * FROM [City] ORDER BY Name ASC", parameters);
        }

        public async Task<IEnumerable<District>> GetDistricts(int CityId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CityId", CityId);

            return await this.QeryAsync<District>("SELECT  * FROM [District] WHERE CityId = @CityId ORDER BY Name ASC", parameters);
        }

        public async Task<EMailAccount> GetEMailAccountBySystemName(string SystemName)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SystemName", SystemName);

            return await this.QeryFirstAsync<EMailAccount>("SELECT  * FROM [EMailAccount] WHERE SystemName = @SystemName AND IsActive = 1", parameters);
        }

        public async Task<PanelSetting> GetSetting(string Key)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Key", Key);

            return await this.QeryFirstAsync<PanelSetting>("SELECT * FROM [PanelSetting] WHERE [Key] = @Key", parameters);
        }

        public async Task<SmsAccount> GetSmsAccount(string SystemName)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SystemName", SystemName);

            return await this.QeryFirstAsync<SmsAccount>("SELECT  * FROM [SmsAccount] WHERE SystemName = @SystemName", parameters);
        }
    }
}
