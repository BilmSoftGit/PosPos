using Pospos.Core.Attributes;
using Pospos.Core.Common;

namespace Pospos.Domain.Entities
{
    [TableName("Pos")]
    public class Pos : DetailedBaseEntity
    {
        public int PosTypeId { get; set; }
        public int PaymentTypeId { get; set; }
        public int BankId { get; set; }
        public int BankTypeId { get; set; }
        public string ApiHost { get; set; }
        public string ApiUser { get; set; }
        public string ApiPassword { get; set; }
        public string StoreType { get; set; }
        public string CreditCardBankName { get; set; }
        public string Mode { get; set; }
        public string CurrencyCode { get; set; }
        public string Host3d { get; set; }
        public string HostXml { get; set; }
        public string ChargeType { get; set; }
        public string TerminalUser { get; set; }
        public string PosnetId { get; set; }
        public string TerminalId { get; set; }
        public string MerchantId { get; set; }
        public string ProvPassword { get; set; }
        public string ProvUser { get; set; }
        public string StoreKey { get; set; }
        public string SecurePaymentReturn { get; set; }
        public string ApiVersion { get; set; }
        public decimal PerProcessExtraAmount { get; set; }
        public decimal PerProcessDiscountRate { get; set; }
        public decimal DDDSwitchAmount{ get; set; }
        public bool DDDSecureFlag { get; set; }
        public bool DDDSwitchFlag { get; set; }
        public bool PointFlag { get; set; }
        public bool MCFlag { get; set; }
        public bool VISAFlag { get; set; }
        public bool AMEXFlag { get; set; }
        public bool IsDefault { get; set; }
        public bool IsProd { get; set; }
        public bool IsActive { get; set; }
    }
}
