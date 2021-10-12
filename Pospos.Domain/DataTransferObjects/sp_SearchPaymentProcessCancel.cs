using System;
using System.ComponentModel;

namespace Pospos.Domain.DataTransferObjects
{
    public class sp_SearchPaymentProcessCancel
    {
        public int Id { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? BankId { get; set; }
        public int? PosId { get; set; }
        public bool IsApproved { get; set; }
        public string CustomerId { get; set; }
        public string UserName { get; set; }
        public string Culture { get; set; }
        public int? StationId { get; set; }
        public string ErrorCode { get; set; }
        public string TimeStamp { get; set; }
        public string Signature { get; set; }
        public string PosMessage { get; set; }
        public string PosRef { get; set; }
        public string XId { get; set; }
        public string Md { get; set; }
        public string InstalmentCount { get; set; }
        public string TotalAmount { get; set; }
        public string CardHolder { get; set; }
        public string StoreType { get; set; }
        public string ChargeType { get; set; }
        public string CurrencyCode { get; set; }
        public string CardNumber { get; set; }
        public string AuthCode { get; set; }
        public string HostRefNum { get; set; }
        public string ProcReturnCode { get; set; }
        public string TransId { get; set; }
        public string HostMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string HandleMessage { get; set; }
        public string Browser { get; set; }
        public string ClientIp { get; set; }
        public string MdStatus { get; set; }
        public string BankOrder { get; set; }
        public string StationTransactionToken { get; set; }
        public DateTime? TransDate { get; set; }
        public string RefNo { get; set; }
        public string Alias { get; set; }
        public string CardName { get; set; }
        public string OrderRef { get; set; }
        public string Rrn { get; set; }
        public string BankMerchantId { get; set; }
        public string ClientId { get; set; }
        public string TerminalBank { get; set; }
        public string Cavv { get; set; }
        public string Hash { get; set; }
        public string PaymentTypeName { get; set; }
        public string BankName { get; set; }
        public string StationName { get; set; }
        public int PaymentProcessId { get; set; }

        #region DB ilişkisinde görmezden gelinecek özellikler

        [DescriptionAttribute("ignore")]
        public int TotalRowCount { get; set; }

        #endregion
    }
}
