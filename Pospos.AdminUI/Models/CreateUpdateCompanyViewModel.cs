using Pospos.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Pospos.AdminUI.Models
{
    public class CreateUpdateCompanyViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string CompanyName { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string Address { get; set; }
        public bool IsApproved { get; set; }
        public string CompanyCode { get; set; }
        public IEnumerable<City> Cities { get; set; }
    }
}
