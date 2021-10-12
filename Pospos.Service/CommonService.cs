using Pospos.Core.Common;
using Pospos.Core.Modules;
using Pospos.Data.Repositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Service
{
    public class CommonService : BaseService
    {
        private readonly CommonRepository _commonRepository;
        public CommonService(LogManager logger, CommonRepository commonRepository) : base(logger)
        {
            _commonRepository = commonRepository;
        }

        public async Task<ResponseBase<IEnumerable<City>>> getAllCities()
        {
            return await ExecuteAsync(() => _commonRepository.GetCities(),
                "CommonService-getAllCities",
                "Mssql",
                "Common",
                "getAllCities",
                true);
        }

        public async Task<ResponseBase<IEnumerable<District>>> getAllDistricts(int CityId)
        {
            return await ExecuteAsync(() => _commonRepository.GetDistricts(CityId),
                "CommonService-getAllDistricts",
                "Mssql",
                "Common",
                "getAllDistricts",
                true);
        }

        public async Task<ResponseBase<EMailAccount>> GetEMailAccountBySystemName(string SystemName)
        {
            return await ExecuteAsync(() => _commonRepository.GetEMailAccountBySystemName(SystemName),
                "CommonService-GetEMailAccountBySystemName",
                "Mssql",
                "Common",
                "GetEMailAccountBySystemName",
                true);
        }

        public async Task<ResponseBase<PanelSetting>> GetSetting(string Key)
        {
            return await ExecuteAsync(() => _commonRepository.GetSetting(Key),
                "CommonService-GetSetting",
                "Mssql",
                "Common",
                "GetSetting",
                true);
        }

        public async Task<ResponseBase<SmsAccount>> GetSmsAccount(string SystemName)
        {
            return await ExecuteAsync(() => _commonRepository.GetSmsAccount(SystemName),
                "CommonService-GetSmsAccount",
                "Mssql",
                "Common",
                "GetSmsAccount",
                true);
        }
    }
}
