using Pospos.Domain.Entities;
using Pospos.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Business.Factories
{
    public class PaymentTypeFactory
    {
        private readonly PaymentTypeService _paymentTypeService;

        public PaymentTypeFactory(PaymentTypeService paymentTypeService)
        {
            this._paymentTypeService = paymentTypeService;
        }

        public async Task<IEnumerable<PaymentType>> GetAll()
        {
            var result = await _paymentTypeService.GetAll();

            return result.data;
        }

        public async Task<PaymentType> GetById(int Id)
        {
            var result = await _paymentTypeService.GetById(Id);

            return result.data;
        }
    }
}
