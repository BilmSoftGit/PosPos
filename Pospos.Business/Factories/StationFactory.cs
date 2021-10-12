using Pospos.Domain.Entities;
using Pospos.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Business.Factories
{
    public class StationFactory
    {
        private readonly StationService _stationService;

        public StationFactory(StationService stationService)
        {
            this._stationService = stationService;
        }

        public async Task<IEnumerable<Station>> GetAll()
        {
            var result = await _stationService.GetAll();

            return result.data;
        }

        public async Task<Station> GetById(int Id)
        {
            var result = await _stationService.GetById(Id);

            return result.data;
        }
    }
}
