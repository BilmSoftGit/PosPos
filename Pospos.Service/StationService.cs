using Pospos.Core.Common;
using Pospos.Core.Modules;
using Pospos.Data.Repositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Service
{
    public class StationService : BaseService
    {
        private readonly StationRepository _stationRepository;

        public StationService(LogManager logger, StationRepository stationRepository) : base(logger)
        {
            this._stationRepository = stationRepository;
        }

        public async Task<ResponseBase<IEnumerable<Station>>> GetAll()
        {
            return await ExecuteAsync(() => _stationRepository.GetAll(),
                "StationService-GetAll",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetAll",
                true);
        }

        public async Task<ResponseBase<Station>> GetById(int Id)
        {
            return await ExecuteAsync(() => _stationRepository.GetById(Id),
                "StationService-GetById",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetById",
                true);
        }
    }
}
