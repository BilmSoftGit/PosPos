using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class PaymentErrorRepository : CustomRepository
    {
        public PaymentErrorRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<PaymentError> GetById(int Id)
        {
            return await GetByIdAsync<PaymentError>(Id);
        }

        public async Task<bool> Updater(PaymentError entity)
        {
            return await UpdateAsync<PaymentError>(entity);
        }
        public async Task<PaymentError> Insert(PaymentError entity)
        {
            return await InsertAsync(entity);
        }

        public async Task<IEnumerable<PaymentError>> Get()
        {
            return await QeryAsync<PaymentError>("SELECT * FROM [PaymentError] with (NOLOCK) order by Code ASC", null);
        }

        public async Task<IEnumerable<Installment>> GetByPaymentType(string type)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("type", type);
            return await QeryAsync<Installment>("SELECT * FROM [Installment] with (NOLOCK) where PaymentType = @type", param);
        }
    }
}
