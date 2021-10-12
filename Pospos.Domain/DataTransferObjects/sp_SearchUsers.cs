using System;
using System.ComponentModel;

namespace Pospos.Domain.DataTransferObjects
{
    public class sp_SearchUsers
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EMailAddress { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
        public DateTime PasswordExpireDate { get; set; }
        public bool IsApproved { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }

        #region DB ilişkisinde görmezden gelinecek özellikler

        [DescriptionAttribute("ignore")]
        public int TotalRowCount { get; set; }

        #endregion
    }
}
