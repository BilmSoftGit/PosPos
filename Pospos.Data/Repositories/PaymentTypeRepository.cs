using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class PaymentTypeRepository : CustomRepository
    {
        public PaymentTypeRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<PaymentType> GetById(int Id)
        {
            return await GetByIdAsync<PaymentType>(Id);
        }

        public async Task<bool> Update(PaymentType entity)
        {
            return await UpdateAsync<PaymentType>(entity);
        }

        public async Task<PaymentType> Insert(PaymentType entity)
        {
            return await InsertAsync(entity);
        }
        public async Task<IEnumerable<PaymentType>> GetAll()
        {
            return await QeryAsync<PaymentType>("SELECT * FROM [PaymentType] with (NOLOCK) where IsDelete = 0 order by Name asc", null);
        }
    }
}
