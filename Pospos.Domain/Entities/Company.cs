using Pospos.Core.Attributes;
using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    [TableName("Company")]
    public class Company : BaseEntity
    {
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string Address { get; set; }
        public bool IsApproved { get; set; }

    }
}
