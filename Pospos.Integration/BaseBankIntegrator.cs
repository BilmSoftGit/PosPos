using System;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pospos.Integration
{
    public class BaseBankIntegrator
    {
        protected string GetSha1(string sha512Data)
        {
            SHA512 sha = new SHA512Managed();
            //SHA1 sha = new SHA1CryptoServiceProvider();
            string hashedPassword = sha512Data;
            byte[] hashbytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(hashedPassword);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            return GetHexaDecimal(inputbytes);
        }
        protected bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        protected string GetHexaDecimal(byte[] bytes)
        {
            StringBuilder s = new StringBuilder();
            int length = bytes.Length;
            for (int n = 0; n <= length - 1; n++)
            {
                s.Append(String.Format("{0,2:x}", bytes[n]).Replace(" ", "0"));
            }
            return s.ToString();
        }

        //private PaymentProcess CreatePaymentProcess(CreditCardPos vPos, OOSPaymentRequest model, string bankOrder, decimal rewardPoint = 0)
        //{
        //    PaymentProcess process = new PaymentProcess();
        //    try
        //    {
        //        IPaymentProcessService paymentProcessService = DependencyResolver.Current.GetService<IPaymentProcessService>();
        //        IUnitOfWork unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();

        //        string maskedCard = string.Empty;

        //        if (vPos.PaymentTypeId != (int)PaymentTypeCode.GarantiPay)
        //        {
        //            maskedCard = (model.CreditCard.CardNumber1 + model.CreditCard.CardNumber2).Substring(0, 6) + "******" + model.CreditCard.CardNumber4;
        //        }
        //        process.CreditCardBankId = vPos.CreditCardBankId;
        //        process.CreditCardPosId = vPos.Id;
        //        process.PaymentTypeId = vPos.PaymentTypeId;
        //        process.CustomerId = model.CustomerId.ToString();
        //        process.UserName = model.Email;
        //        process.CardNumber = maskedCard;
        //        process.IsApproved = false;
        //        process.PaymentProjectCode = model.ProjectCode;
        //        process.InstalmentCount = model.CreditCard.Installment.ToString();
        //        process.CardHolder = model.CreditCard.CardholderName;
        //        process.CurrencyCode = "949";
        //        process.ClientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //        process.Browser = Utils.GetBrowser();
        //        process.TransDate = DateTime.Now;
        //        process.BankOrder = bankOrder;
        //        process.ProjectOrderGuid = model.OrderGuid.ToString();
        //        process.TotalAmount = model.OrderTotal.ToString();
        //        process.StoreType = vPos.StoreType;
        //        process.ChargeType = vPos.ChargeType;
        //        process.LanguageCulture = model.LanguageCulture;
        //        process.RewardAmount = rewardPoint.ToString();
        //        process.CampaignProcessId = model.CampaignProcessId;

        //        paymentProcessService.Add(process);
        //        unitOfWork.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        AddExceptionLog(ex);
        //    }

        //    return process;
        //}

        //private PaymentProcess CreatePaymentProcess(CreditCardPos vPos, OOSPaymentRequest model, string bankOrder, Dictionary<int, decimal> termDiffrence, decimal rewardPoint = 0)
        //{
        //    PaymentProcess process = new PaymentProcess();
        //    try
        //    {
        //        IPaymentProcessService paymentProcessService = DependencyResolver.Current.GetService<IPaymentProcessService>();
        //        IUnitOfWork unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();

        //        string maskedCard = string.Empty;

        //        if (vPos.PaymentTypeId != (int)PaymentTypeCode.GarantiPay)
        //        {
        //            maskedCard = (model.CreditCard.CardNumber1 + model.CreditCard.CardNumber2).Substring(0, 6) + "******" + model.CreditCard.CardNumber4;
        //        }
        //        decimal orderTotal = Convert.ToDecimal(model.OrderTotal);
        //        decimal vadeFarkiOran = termDiffrence[model.CreditCard.Installment];
        //        decimal vadeliToplamTutar = (orderTotal) * (1 + vadeFarkiOran);
        //        string amount = Math.Round(vadeliToplamTutar, 2).ToString();

        //        process.CreditCardBankId = vPos.CreditCardBankId;
        //        process.CreditCardPosId = vPos.Id;
        //        process.PaymentTypeId = vPos.PaymentTypeId;
        //        process.CustomerId = model.CustomerId.ToString();
        //        process.UserName = model.Email;
        //        process.CardNumber = maskedCard;
        //        process.IsApproved = false;
        //        process.PaymentProjectCode = model.ProjectCode;
        //        process.InstalmentCount = model.CreditCard.Installment.ToString();
        //        process.CardHolder = model.CreditCard.CardholderName;
        //        process.CurrencyCode = "949";
        //        process.ClientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //        process.Browser = Utils.GetBrowser();
        //        process.TransDate = DateTime.Now;
        //        process.BankOrder = bankOrder;
        //        process.ProjectOrderGuid = model.OrderGuid.ToString();
        //        process.TotalAmount = amount;
        //        process.StoreType = vPos.StoreType;
        //        process.ChargeType = vPos.ChargeType;
        //        process.LanguageCulture = model.LanguageCulture;
        //        process.RewardAmount = rewardPoint.ToString();
        //        process.CampaignProcessId = model.CampaignProcessId;

        //        if (model.ProcessRewardId != null && model.ProcessRewardId > 0)
        //        {
        //            process.ProcessRewardId = model.ProcessRewardId;
        //        }

        //        paymentProcessService.Add(process);
        //        unitOfWork.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        AddExceptionLog(ex);
        //    }

        //    return process;
        //}
    }
}
