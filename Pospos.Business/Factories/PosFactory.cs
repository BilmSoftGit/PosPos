using Pospos.Domain.Entities;
using Pospos.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Business.Factories
{
    public class PosFactory
    {
        private readonly PosService _posService;

        public PosFactory(PosService posService)
        {
            this._posService = posService;
        }

        public async Task<IEnumerable<Pos>> GetAll()
        {
            var result = await _posService.GetAll();

            return result.data;
        }

        public async Task<Pos> GetById(int Id)
        {
            var result = await _posService.GetById(Id);

            return result.data;
        }
    }
}
