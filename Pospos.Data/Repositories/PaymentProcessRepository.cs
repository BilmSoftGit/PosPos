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
    public class PaymentProcessRepository : CustomRepository
    {
        public PaymentProcessRepository(MainConnectionManager connection) : base(connection)
        {

        }

        public async Task<PaymentProcess> GetById(int Id)
        {
            return await GetByIdAsync<PaymentProcess>(Id);
        }

        public async Task<PaymentProcess> GetByToken(string Token)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Token", Token);

            return await this.QeryFirstAsync<PaymentProcess>("SELECT * FROM [PaymentProcess] WHERE Token = @Token", parameters, CommandType.Text);
        }

        public async Task<bool> Update(PaymentProcess entity)
        {
            return await UpdateAsync<PaymentProcess>(entity);
        }
        public async Task<PaymentProcess> Insert(PaymentProcess entity)
        {
            return await InsertAsync(entity);
        }

        public async Task<IEnumerable<sp_SearchPaymentProcess>> SearchForDataTablePaymentProcess(int length, int page, string sortColumn, string sortColumnAscDesc, string search, string Username, string CardHolder, string CardNumber, int? BankId = null, int? PaymentTypeId = null, int? PosId = null, int? CompanyId = null, bool? IsApproved = null)
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

            if (IsApproved.HasValue)
                paramaters.Add("isapproved", IsApproved);

            return await this.QeryAsync<sp_SearchPaymentProcess>("sp_SearchPaymentProcess", paramaters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<sp_SearchPaymentProcess>> SearchForDataTablePaymentProcessForExcel(int length, int page, string sortColumn, string sortColumnAscDesc, string search, string Username, string CardHolder, string CardNumber, int? BankId = null, int? PaymentTypeId = null, int? PosId = null, int? CompanyId = null, bool? IsApproved = null)
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

            if (IsApproved.HasValue)
                paramaters.Add("isapproved", IsApproved);

            return await this.QeryAsync<sp_SearchPaymentProcess>("sp_SearchPaymentProcess_Excel", paramaters, CommandType.StoredProcedure);
        }
    }
}
