using Pospos.Core.Common;
using Pospos.Core.Modules;
using Pospos.Data.Repositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Service
{
    public class PosService : BaseService
    {
        private readonly PosRepository  _posRepository;

        public PosService(LogManager logger, PosRepository posRepository) : base(logger)
        {
            this._posRepository = posRepository;
        }

        public async Task<ResponseBase<IEnumerable<Pos>>> GetAll()
        {
            return await ExecuteAsync(() => _posRepository.GetAll(),
                "PosService-GetAll",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetAll",
                true);
        }

        public async Task<ResponseBase<Pos>> GetById(int Id)
        {
            return await ExecuteAsync(() => _posRepository.GetById(Id),
                "PosService-GetById",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetById",
                true);
        }
    }
}
