using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.DataTransferObjects;
using Pospos.Domain.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class PaymentProcessCancelRepository : CustomRepository
    {
        public PaymentProcessCancelRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<PaymentProcessCancel> GetById(int Id)
        {
            return await GetByIdAsync<PaymentProcessCancel>(Id);
        }

        public async Task<bool> Update(PaymentProcessCancel entity)
        {
            return await UpdateAsync<PaymentProcessCancel>(entity);
        }

        public async Task<PaymentProcessCancel> Insert(PaymentProcessCancel entity)
        {
            return await InsertAsync(entity);
        }
        public async Task<IEnumerable<PaymentProcessCancel>> GetAll()
        {
            return await QeryAsync<PaymentProcessCancel>("SELECT * FROM [PaymentProcessCancel] with (NOLOCK) order by BinCode ASC", null);
        }

        public async Task<IEnumerable<BankCardBin>> GetAllByBank(int bankId)
        {
            DynamicParameters parameters = new DynamicParameters();

            return await QeryAsync<BankCardBin>("SELECT * FROM [PaymentProcessCancel] with (NOLOCK) order by Id ASC", parameters);
        }

        public async Task<IEnumerable<sp_SearchPaymentProcessCancel>> SearchForDataTablePaymentProcessCancel(int length, int page, string sortColumn, string sortColumnAscDesc, string search, string Username, string CardHolder, string CardNumber, int? BankId = null, int? PaymentTypeId = null, int? PosId = null, int? CompanyId = null)
        {
            DynamicParameters paramaters = new DynamicParameters();
            paramaters.Add("PageSize", length);
            paramaters.Add("CurrentPage", page);
            paramaters.Add("sortName", sortColumn);
            paramaters.Add("sortDirection", sortColumnAscDesc);
            paramaters.Add("search", search);

            if (!string.IsNullOrWhiteSpace(Username))
                paramaters.Add("username", Username);

            if (!string.IsNullOrWhiteSpace(CardHolder))
                paramaters.Add("cardholder", CardHolder);

            if (!string.IsNullOrWhiteSpace(CardNumber))
                paramaters.Add("cardnumber", CardNumber);

            if (BankId.GetValueOrDefault(0) > 0)
                paramaters.Add("bankid", BankId);

            if (PaymentTypeId.GetValueOrDefault(0) > 0)
                paramaters.Add("paymenttypeid", PaymentTypeId);

            if (PosId.GetValueOrDefault(0) > 0)
                paramaters.Add("posid", PosId);

            if (CompanyId.GetValueOrDefault(0) > 0)
                paramaters.Add("companyId", CompanyId);

            return await this.QeryAsync<sp_SearchPaymentProcessCancel>("sp_SearchPaymentProcessCancel", paramaters, CommandType.StoredProcedure);
        }
    }
}
