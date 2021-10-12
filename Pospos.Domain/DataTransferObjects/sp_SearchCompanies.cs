using System;
using System.ComponentModel;

namespace Pospos.Domain.DataTransferObjects
{
    public class sp_SearchCompanies
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public bool IsApproved { get; set; }

        #region DB ilişkisinde görmezden gelinecek özellikler

        [DescriptionAttribute("ignore")]
        public int TotalRowCount { get; set; }

        #endregion
    }
}
