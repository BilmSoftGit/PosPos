using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class InstallmentRepository : CustomRepository
    {
        public InstallmentRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<Installment> GetById(int Id)
        {
            return await GetByIdAsync<Installment>(Id);
        }

        public async Task<bool> Update(Installment entity)
        {
            return await UpdateAsync<Installment>(entity);
        }
        public async Task<Installment> Insert(Installment entity)
        {
            return await InsertAsync(entity);
        }

        public async Task<IEnumerable<Installment>> GetAll(int stationId = 1,int posId = 0)
        {
            DynamicParameters param = new DynamicParameters();
            string query = "SELECT * FROM [Installment] with (NOLOCK) where IsDeleted = @deleted";
            param.Add("deleted", 0);
            if(stationId > 0)
            {
                param.Add("stationId", stationId);
                query += " and StationId = @stationId";
            }

            if (posId > 0)
            {
                param.Add("posId", posId);
                query += " and PosId = @posId";
            }
            return await QeryAsync<Installment>(query, param);
        }

        public async Task<IEnumerable<Installment>> GetByBinCode(string binCode,int stationId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("stationId", stationId);
            param.Add("binCode", binCode);
            return await QeryAsync<Installment>(@"SELECT inst.[Id]
      ,inst.[StationId]
      ,inst.[PosId]
      ,inst.[InstallmentCount]
      ,inst.[TermDiffrence]
      ,inst.[IsActive]
      ,inst.[IsDeleted]
      ,inst.[CreatedByUserId]
      ,inst.[UpdatedByUserId]
      ,inst.[CreatedDate]
      ,inst.[UpdatedDate]
  FROM [PosPos].[dbo].[Installment] inst with (NOLOCK) 
  Inner Join dbo.BankCardBin bcbin with (NOLOCK) on bcbin.BinCode = @binCode
  Inner Join dbo.[Pos] pos with (NOLOCK) on pos.BankId = bcbin.BankId
  where inst.PosId = pos.Id and inst.IsDeleted = 0 and inst.IsActive = 1 and inst.StationId = @stationId
  and pos.IsActive = 1 and pos.IsDeleted = 0", param);
        }
    }
}
