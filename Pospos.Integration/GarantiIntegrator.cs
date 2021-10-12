using Pospos.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Pospos.Integration
{
    public class GarantiIntegrator : BaseBankIntegrator
    {
        private readonly SecurityHelper _securityHelper;
        private readonly Utility _utility;
        public GarantiIntegrator(SecurityHelper securityHelper, Utility utility) :base()
        {
            _securityHelper = securityHelper;
            _utility = utility;
        }

       

        //private ProcessPaymentResult doPayment3D(CreditCardPos vPos, Dictionary<int, decimal> termDiffrence, OOSPaymentRequest model, decimal rewardPoint = 0)
        //{
        //    decimal orderTotal = Convert.ToDecimal(model.OrderTotal);
        //    ProcessPaymentResult result = new ProcessPaymentResult();

        //    string strOrderId = _utility.GetOrderNumber();
        //    PaymentProcess process = CreatePaymentProcess(vPos, model, strOrderId, termDiffrence, rewardPoint);

        //    string rewardAmount = model.CreditCard.MoneyPoint;
        //    string strMode = vPos.Mode.Clear();
        //    string strApiVersion = vPos.ApiVersion.Clear();
        //    string strTerminalProvUserId = vPos.ProvUser.Clear();
        //    string strType = vPos.ChargeType.Clear();
        //    decimal vadeFarkiOran = termDiffrence[model.CreditCard.Installment];
        //    decimal vadeliToplamTutar = (orderTotal) * (1 + vadeFarkiOran);
        //    decimal subtotal = Math.Round(vadeliToplamTutar, 2);
        //    string strAmount = subtotal.ToString().Replace(",", "").Replace(".", "");
        //    string strCurrencyCode = vPos.CurrencyCode.Clear();
        //    string strInstallmentCount = Convert.ToInt16(model.CreditCard.Installment) == 1 ? "" : model.CreditCard.Installment.ToString();
        //    string strTerminalUserId = model.ProjectCode;
        //    string strCustomeripaddress = vPos.Mode == "TEST" ? "127.0.0.1" : _securityHelper.GetClientIpAddress();
        //    string strTerminalId = vPos.TerminalId.Clear();
        //    string _strTerminalID = "0" + vPos.TerminalId.Clear();
        //    string strTerminalMerchantId = vPos.MerchantId.Clear();
        //    string strProvisionPassword = vPos.ProvPassword.Clear();
        //    string strStoreKey = vPos.StoreKey.Clear();
        //    string strSuccessUrl = vPos.SecurePaymentReturn + "?process=" + process.Id;
        //    string strErrorUrl = vPos.SecurePaymentReturn + "?process=" + process.Id;
        //    string securityData = GetSha1(strProvisionPassword + _strTerminalID).ToUpper();
        //    string hashData = GetSha1(strTerminalId + strOrderId + strAmount + strSuccessUrl + strErrorUrl + strType + strInstallmentCount + strStoreKey + securityData).ToUpper();
        //    string strsecure3Dsecuritylevel = vPos.StoreType;
        //    //string cardHolder = model.CreditCard.CardholderName;
        //    //string smpCode = model.ProjectCode;
        //    string ccv = model.CreditCard.CardCode;
        //    string pan = model.CreditCard.CardNumber;
        //    string ecomPaymentCardExpDateYear = model.CreditCard.ExpireYear.Substring(model.CreditCard.ExpireYear.Length - 2, 2);
        //    string ecomPaymentCardExpDateMonth = model.CreditCard.ExpireMonth;

        //    if (ecomPaymentCardExpDateMonth.Length == 1)
        //    {
        //        ecomPaymentCardExpDateMonth = "0" + ecomPaymentCardExpDateMonth;
        //    }
        //    else
        //    {
        //        ecomPaymentCardExpDateMonth = ecomPaymentCardExpDateMonth;
        //    }
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("secure3dsecuritylevel="); sb.Append(strsecure3Dsecuritylevel);
        //    sb.Append("&cardnumber="); sb.Append(pan);
        //    sb.Append("&cardexpiredatemonth="); sb.Append(ecomPaymentCardExpDateMonth);
        //    sb.Append("&cardexpiredateyear="); sb.Append(ecomPaymentCardExpDateYear);
        //    sb.Append("&cardcvv2="); sb.Append(ccv);
        //    sb.Append("&mode="); sb.Append(strMode);
        //    sb.Append("&apiversion="); sb.Append(strApiVersion);
        //    sb.Append("&terminalprovuserid="); sb.Append(strTerminalProvUserId);
        //    sb.Append("&terminaluserid="); sb.Append(strTerminalUserId);
        //    sb.Append("&terminalmerchantid="); sb.Append(strTerminalMerchantId);
        //    sb.Append("&txntype="); sb.Append(strType);
        //    sb.Append("&txnamount="); sb.Append(strAmount);
        //    sb.Append("&txncurrencycode="); sb.Append(strCurrencyCode);
        //    sb.Append("&txninstallmentcount="); sb.Append(strInstallmentCount);
        //    sb.Append("&orderid="); sb.Append(strOrderId);
        //    sb.Append("&terminalid="); sb.Append(strTerminalId);
        //    sb.Append("&successurl="); sb.Append(strSuccessUrl);
        //    sb.Append("&errorurl="); sb.Append(strErrorUrl);
        //    sb.Append("&customeripaddress="); sb.Append(strCustomeripaddress);
        //    sb.Append("&secure3dhash="); sb.Append(hashData);

        //    try
        //    {
        //        HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(vPos.Host3d);
        //        string proxy = null;
        //        byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());

        //        hwr.Method = "POST";
        //        hwr.ContentType = "application/x-www-form-urlencoded";
        //        hwr.ContentLength = buffer.Length;
        //        hwr.AllowAutoRedirect = false;
        //        hwr.MaximumAutomaticRedirections = 250;
        //        hwr.Timeout = 100000;

        //        hwr.Proxy = new WebProxy(proxy, true);
        //        hwr.Credentials = CredentialCache.DefaultCredentials;
        //        ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
        //        hwr.CookieContainer = new CookieContainer();

        //        Stream datagonder = hwr.GetRequestStream();
        //        datagonder.Write(buffer, 0, buffer.Length);
        //        datagonder.Flush();
        //        datagonder.Close();

        //        HttpWebResponse hwresOdeme = (HttpWebResponse)hwr.GetResponse();
        //        string uriString = hwresOdeme.Headers["Location"];

        //        Stream donenveri = hwresOdeme.GetResponseStream();
        //        StreamReader srOdeme = new StreamReader(donenveri);
        //        string strSonuc = srOdeme.ReadToEnd();

        //        hwresOdeme.Close();
        //        result.SecureOdemeGuvenlikSayfasiString = strSonuc;

        //        #region Request Log
        //        GarantiRequest garantiRequest = new GarantiRequest
        //        {
        //            Secure3Dsecuritylevel = strsecure3Dsecuritylevel,
        //            CardNumber = (model.CreditCard.CardNumber1 + model.CreditCard.CardNumber2).Substring(0, 6) + "******" + model.CreditCard.CardNumber4,
        //            Mode = strMode,
        //            ApiVersion = strApiVersion,
        //            TerminalProvUserId = strTerminalProvUserId,
        //            TerminalUserId = strTerminalUserId,
        //            TerminalMerchantId = strTerminalMerchantId,
        //            TxnType = strType,
        //            TxnAmount = strAmount,
        //            TxnCurrencyCode = strCurrencyCode,
        //            TxnInstallmentCount = strInstallmentCount,
        //            OrderId = strOrderId,
        //            TerminalId = strTerminalId,
        //            SuccessUrl = strSuccessUrl,
        //            ErrorUrl = strErrorUrl,
        //            CustomerIpAddress = strCustomeripaddress,
        //            Secure3Dhash = hashData
        //        };
        //        AddLog(new ProjectProcess
        //        {
        //            ClientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
        //            Data = JsonConvert.SerializeObject(garantiRequest),
        //            Content = strSonuc,
        //            Text = "Garanti 3D Request",
        //            ProjectCode = model.ProjectCode,
        //            CustomerId = model.CustomerId,
        //            UserName = model.Email,
        //            TransDate = DateTime.Now
        //        });
        //        #endregion
        //    }

        //    catch (WebException ex)
        //    {
        //        AddExceptionLog(ex);
        //        result.Errors.Add("Bilgi doğrulama sırasında bir hata ile karşılaşıldı: " + ex.Message);
        //    }

        //    return result;
        //}

        //public PaymentProcess CancelAndReturnPayment(PaymentProcess paymentProcess, ProcessType type, string amount = null)
        //{
        //    CreditCardPos vPos = paymentProcess.CreditCardPos;
        //    PaymentProcess process = new PaymentProcess();

        //    const string strProvUserID = "PROVRFN";
        //    const string strMotoInd = "N";
        //    string strAmount = string.Empty;
        //    string resultXml = string.Empty;
        //    string data = string.Empty;
        //    string strType = vPos.ChargeType;
        //    string strMode = vPos.Mode;
        //    string strVersion = vPos.ApiVersion;
        //    string strTerminalID = vPos.TerminalId;
        //    string _strTerminalID = "0" + vPos.TerminalId;
        //    string strProvisionPassword = vPos.ProvPassword;
        //    string strUserID = vPos.ProvUser;
        //    string strMerchantID = vPos.MerchantId;
        //    string strIPAddress = _securityHelper.GetClientIpAddress();
        //    string strOrderID = paymentProcess.BankOrder;
        //    string strOriginalRetrefNum = paymentProcess.HostRefNum;

        //    switch (type)
        //    {
        //        case ProcessType.Cancel:
        //            strType = "Void";
        //            strAmount = paymentProcess.TotalAmount.Replace(",", "").Replace(".", "");
        //            break;
        //        case ProcessType.Refund:
        //            strType = "refund";
        //            strAmount = paymentProcess.TotalAmount.Replace(",", "").Replace(".", "");
        //            process.ChargeType = "Refund";
        //            break;
        //        case ProcessType.PartialRefund:
        //            strType = "refund";
        //            strAmount = amount.Replace(",", "").Replace(".", "");
        //            process.ChargeType = "Refund";
        //            break;
        //        default:
        //            break;
        //    }
        //    #region Post string

        //    string securityData = GetSha1(strProvisionPassword + _strTerminalID).ToUpper();
        //    string hashData = GetSha1(strOrderID + strTerminalID + strAmount + securityData).ToUpper();

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //    sb.Append("<GVPSRequest>");
        //    sb.Append("<Mode>"); sb.Append(strMode); sb.Append("</Mode>");
        //    sb.Append("<Version>"); sb.Append(strVersion); sb.Append("</Version>");
        //    sb.Append("<Terminal>");
        //    sb.Append("<ProvUserID>"); sb.Append(strProvUserID); sb.Append("</ProvUserID>");
        //    sb.Append("<HashData>"); sb.Append(hashData); sb.Append("</HashData>");
        //    sb.Append("<UserID>"); sb.Append(strUserID); sb.Append("</UserID>");
        //    sb.Append("<ID>"); sb.Append(strTerminalID); sb.Append("</ID>");
        //    sb.Append("<MerchantID>"); sb.Append(strMerchantID); sb.Append("</MerchantID>");
        //    sb.Append("</Terminal>");
        //    sb.Append("<Customer>");
        //    sb.Append("<IPAddress>"); sb.Append(strIPAddress); sb.Append("</IPAddress>");
        //    sb.Append("</Customer>");
        //    sb.Append("<Order>");
        //    sb.Append("<OrderID>"); sb.Append(strOrderID); sb.Append("</OrderID>");
        //    sb.Append("</Order>");
        //    sb.Append("<Transaction>");
        //    sb.Append("<Type>"); sb.Append(strType); sb.Append("</Type>");
        //    sb.Append("<Amount>"); sb.Append(strAmount); sb.Append("</Amount>");
        //    sb.Append("<MotoInd>"); sb.Append(strMotoInd); sb.Append("</MotoInd>");
        //    sb.Append("<OriginalRetrefNum>"); sb.Append(strOriginalRetrefNum); sb.Append("</OriginalRetrefNum>");
        //    sb.Append("</Transaction>");
        //    sb.Append("</GVPSRequest>");
        //    #endregion

        //    data = "data=" + sb.ToString();

        //    WebRequest _WebRequest = WebRequest.Create(vPos.HostXml);
        //    _WebRequest.Method = "POST";

        //    byte[] byteArray = Encoding.UTF8.GetBytes(data);
        //    _WebRequest.ContentType = "application/x-www-form-urlencoded";
        //    _WebRequest.ContentLength = byteArray.Length;

        //    Stream dataStream = _WebRequest.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse _WebResponse = _WebRequest.GetResponse();
        //    dataStream = _WebResponse.GetResponseStream();

        //    StreamReader reader = new StreamReader(dataStream);
        //    resultXml = reader.ReadToEnd();

        //    System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
        //    xDoc.LoadXml(resultXml);

        //    #region Payment Process result
        //    XmlElement xEReasonCode = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/ReasonCode") as XmlElement;
        //    XmlElement xEErrorMsg = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/ErrorMsg") as XmlElement;
        //    XmlElement xESysErrMsg = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/SysErrMsg") as XmlElement;
        //    XmlElement xERetrefNum = xDoc.SelectSingleNode("//GVPSResponse/Transaction/RetrefNum") as XmlElement;
        //    XmlElement xEAuthCode = xDoc.SelectSingleNode("//GVPSResponse/Transaction/AuthCode") as XmlElement;
        //    XmlElement xEClientIp = xDoc.SelectSingleNode("//GVPSResponse/Customer/IPAddress") as XmlElement;

        //    process.CardHolder = paymentProcess.CardHolder;
        //    process.PaymentProjectCode = paymentProcess.PaymentProjectCode;
        //    process.CardNumber = paymentProcess.CardNumber;
        //    process.HostRefNum = xERetrefNum.InnerText;
        //    process.HostMessage = xESysErrMsg.InnerText;
        //    process.ErrorMessage = xEErrorMsg.InnerText;
        //    process.ProcReturnCode = xEReasonCode.InnerText;
        //    process.AuthCode = xEAuthCode.InnerText;
        //    process.BankOrder = strOrderID;
        //    process.TransDate = DateTime.Now;
        //    process.Browser = "";// Utils.GetBrowser();
        //    //process.MdStatus = mdstatus;
        //    process.InstalmentCount = paymentProcess.InstalmentCount;
        //    process.ClientIp = xEClientIp.InnerText;
        //    process.CurrencyCode = paymentProcess.CurrencyCode;
        //    process.CreditCardBankId = vPos.CreditCardBankId;
        //    process.PaymentTypeId = vPos.PaymentTypeId;
        //    process.TransId = null;
        //    process.ChargeType = strType;
        //    process.StoreType = paymentProcess.StoreType;
        //    process.TotalAmount = !string.IsNullOrEmpty(amount) ? amount : paymentProcess.TotalAmount;
        //    #endregion

        //    if (resultXml.Contains("<ReasonCode>00</ReasonCode>") && resultXml.Contains("<Message>Approved</Message>"))
        //    {
        //        process.IsApproved = true;//3D işlem onayı alındı."
        //    }
        //    else
        //    {
        //        process.IsApproved = false;//3D işlem onayı alınamadı
        //    }

        //    return process;
        //}

        //public void doPaymentBonusFlash(CreditCardPos vPos, OOSPaymentRequest request, List<CreditCardInstallment> installments)
        //{
        //    ProcessPaymentResult result = new ProcessPaymentResult();
        //    string orderId = Utils.CreateOrder();

        //    PaymentProcess process = CreatePaymentProcess(vPos, request, orderId);

        //    string txnAmount = Math.Round(request.OrderTotal, 2).ToString().Replace(",", "").Replace(".", "");
        //    string securityLevel = "CUSTOM_PAY";
        //    string txnType = "gpdatarequest";
        //    string txnSubType = vPos.ChargeType;
        //    string companyName = vPos.PaymentProjectCompany.Name;
        //    string bnsUseFlag = "Y";
        //    string fbbUseFlag = "N";
        //    string chequeUseflag = "N";
        //    string mileUseflag = "N";
        //    string addcampaignInstallment = "N";
        //    string garantiPay = "Y";
        //    string terminalUserId = vPos.TerminalUser;
        //    string txnTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        //    string txnMotoInd = "N";
        //    string mobilInd = "N";
        //    string lang = "tr";
        //    string refreshTime = "3";
        //    string userId = request.ProjectCode;
        //    string txnInstallmentCount = string.Empty;
        //    //string mode = vPos.Mode;
        //    string mode = "PROD";
        //    string apiVersion = vPos.ApiVersion;
        //    string terminalProvUserId = vPos.ProvUser;
        //    string strType = vPos.ChargeType.Clear();
        //    string txnCurrencyCode = vPos.CurrencyCode;
        //    string customerIpAddress = Utils.GetUserIP();
        //    string customerEmailAdress = request.Email;
        //    string terminalId = vPos.TerminalId;
        //    string _terminalID = "0" + vPos.TerminalId.Clear();
        //    string terminalMerchantId = vPos.MerchantId;
        //    string provisionPassword = vPos.ProvPassword.Clear();
        //    string storeKey = vPos.StoreKey.Clear();
        //    string successUrl = vPos.SecurePaymentReturn + "?process=" + process.Id;
        //    string errorUrl = vPos.SecurePaymentReturn + "?process=" + process.Id;
        //    string totallnstallmentcount = "0";

        //    if (installments != null)
        //    {
        //        totallnstallmentcount = installments.Where(p => p.InstallmentCount > 1).Count().ToString();
        //    }

        //    string securityData = GetSha1(provisionPassword + _terminalID).ToUpper();
        //    string secure3DHash = GetSha1(terminalId + orderId + txnAmount + successUrl + errorUrl + txnType + storeKey + securityData).ToUpper();

        //    RemotePost remotepost = new RemotePost();
        //    //remotepost.Url = "http://www.garantipos.com.tr/Admin/post.asp";
        //    remotepost.Url = vPos.Host3d;
        //    remotepost.Add("secure3dsecuritylevel", securityLevel);
        //    remotepost.Add("txntype", txnType);
        //    remotepost.Add("txnsubtype", txnSubType);
        //    remotepost.Add("orderid", orderId);
        //    remotepost.Add("txnamount", txnAmount);
        //    remotepost.Add("companyname", companyName);
        //    remotepost.Add("garantipay", garantiPay);
        //    remotepost.Add("bnsuseflag", bnsUseFlag);
        //    remotepost.Add("fbbuseflag", fbbUseFlag);
        //    remotepost.Add("chequeuseflag", chequeUseflag);
        //    remotepost.Add("mileuseflag", mileUseflag);
        //    remotepost.Add("addcampaigninstallment", addcampaignInstallment);
        //    remotepost.Add("totallinstallmentcount", totallnstallmentcount);
        //    remotepost.Add("cardnumber", "");
        //    remotepost.Add("cardexpiredatemonth", "");
        //    remotepost.Add("cardexpiredateyear", "");
        //    remotepost.Add("mode", mode);
        //    remotepost.Add("cardcvv2", "");
        //    remotepost.Add("apiversion", apiVersion);
        //    remotepost.Add("terminalprovuserid", terminalProvUserId);
        //    remotepost.Add("terminaluserid", terminalUserId);
        //    remotepost.Add("terminalid", terminalId);
        //    remotepost.Add("terminalmerchantid", terminalMerchantId);
        //    remotepost.Add("customeripaddress", customerIpAddress);
        //    remotepost.Add("customeremailaddress", customerEmailAdress);
        //    remotepost.Add("txncurrencycode", txnCurrencyCode);
        //    remotepost.Add("txninstallmentcount", txnInstallmentCount);//boş gönderilmeli
        //    remotepost.Add("successurl", successUrl);
        //    remotepost.Add("errorurl", errorUrl);
        //    remotepost.Add("secure3dhash", secure3DHash);
        //    remotepost.Add("txnmotoind", txnMotoInd);
        //    remotepost.Add("mobilind", mobilInd);
        //    remotepost.Add("txntimestamp", txnTimeStamp);
        //    remotepost.Add("lang", lang);
        //    remotepost.Add("refreshtime", refreshTime);

        //    foreach (CreditCardInstallment installment in installments)
        //    {
        //        if (installment.InstallmentCount > 1)
        //        {
        //            remotepost.Add("installmentnumber" +
        //                (installment.InstallmentCount - 1), installment.InstallmentCount.ToString().Replace(",", "").Replace(".", ""));
        //            remotepost.Add("installmentamount" +
        //                (installment.InstallmentCount - 1), installment.InstallmentAmount.ToString().Replace(",", "").Replace(".", ""));
        //        }
        //    }

        //    remotepost.Post();
        //}

        //public PaymentProcess GetBonusFlashResult(CreditCardPos vPos, PaymentProcess result)
        //{
        //    #region Request parameter
        //    NameValueCollection requestForm = HttpContext.Current.Request.Form;
        //    string mdStatus = requestForm.Get("mdstatus");
        //    string mdErrorMessage = requestForm.Get("mderrormessage");
        //    string errMsg = requestForm.Get("errmsg");
        //    string clientId = requestForm.Get("clientid");
        //    string gpResponse = requestForm.Get("response");
        //    string procReturnCode = requestForm.Get("procreturncode");
        //    string customerIpAddress = requestForm.Get("customeripaddress");
        //    string txnMotoInd = requestForm.Get("txnmotoind");
        //    string terminalMerchantId = requestForm.Get("terminalmerchantid");
        //    string txntType = requestForm.Get("txntype");
        //    string refreshTime = requestForm.Get("refreshtime");
        //    string lang = requestForm.Get("lang");
        //    string garantiPay = requestForm.Get("garantipay");
        //    string mode = requestForm.Get("mode");
        //    string txnAmount = requestForm.Get("txnamount");
        //    string chequeUseFlag = requestForm.Get("chequeuseflag");
        //    string terminalId = requestForm.Get("terminalid");
        //    string customerEmailAddress = requestForm.Get("customeremailaddress");
        //    string secure3dHash = requestForm.Get("secure3dhash");
        //    string terminalProvUserId = requestForm.Get("terminalprovuserid");
        //    string txnCurrencyCode = requestForm.Get("txncurrencycode");
        //    string errorUrl = requestForm.Get("errorurl");
        //    string totallInstallmentCount = requestForm.Get("totallinstallmentcount");
        //    string txnSubType = requestForm.Get("txnsubtype");
        //    string companyName = requestForm.Get("companyname");
        //    string orderId = requestForm.Get("orderid");
        //    string successUrl = requestForm.Get("successurl");
        //    string secure3dSecurityLevel = requestForm.Get("secure3dsecuritylevel");
        //    string txnInstallmentCount = requestForm.Get("txninstallmentcount");
        //    string terminalUserId = requestForm.Get("terminaluserid");
        //    string bnsUseFlag = requestForm.Get("bnsuseflag");
        //    string addCampaignInstallment = requestForm.Get("addcampaigninstallment");
        //    string mobilInd = requestForm.Get("mobilind");
        //    string txnTimeStamp = requestForm.Get("txntimestamp");
        //    string mileUseFlag = requestForm.Get("mileuseflag");
        //    string apiVersion = requestForm.Get("apiversion");
        //    string hashParams = requestForm.Get("hashparams");
        //    string hashParamsVal = requestForm.Get("hashparamsval");
        //    string hashParam = requestForm.Get("hash");
        //    string gpHashData = requestForm.Get("gphashdata");

        //    string timeDelay = requestForm.Get("timedelay");
        //    string authCode = requestForm.Get("authcode");
        //    string hostRefNum = requestForm.Get("hostrefnum");
        //    string maskedPan = requestForm.Get("maskedPan");
        //    string rnd = requestForm.Get("rnd");
        //    string gpInstallment = requestForm.Get("gpinstallment");
        //    string gpInstallmentAmount = requestForm.Get("gpinstallmentamount");
        //    string usedBonusAmount = requestForm.Get("usedbonusamount");
        //    string usedFbbAmount = requestForm.Get("usedfbbamount");
        //    string cardRef = requestForm.Get("cardref");
        //    string fbbUseFlag = requestForm.Get("fbbuseflag");

        //    string mdStatusText = string.Empty;
        //    string paramsval = string.Empty;
        //    string storeKey = vPos.StoreKey;

        //    result.CardNumber = maskedPan;
        //    result.AuthCode = authCode;
        //    #endregion

        //    #region Response Log
        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    requestForm.CopyTo(dict);

        //    int customerId = 0;
        //    int.TryParse(result.CustomerId, out customerId);
        //    string response = JsonConvert.SerializeObject(dict);
        //    ProjectProcess projectProcess = new ProjectProcess
        //    {
        //        ClientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
        //        Data = response,
        //        Text = "Bonus Flash Response",
        //        ProjectCode = result.PaymentProjectCode,
        //        CustomerId = customerId,
        //        UserName = result.UserName,
        //        TransDate = DateTime.Now
        //    };

        //    AddLog(projectProcess);
        //    #endregion

        //    result.AuthCode = authCode;
        //    result.ProcReturnCode = procReturnCode;
        //    result.TransId = string.Empty;
        //    result.HostRefNum = hostRefNum;
        //    //result.ErrorMessage = mdErrorMsg;
        //    result.HostMessage = errMsg;
        //    result.MdStatus = mdStatus;
        //    result.CardNumber = maskedPan;
        //    result.IsApproved = true;

        //    #region Garanti pay doğrulama            

        //    if (!procReturnCode.Contains("00"))
        //    {
        //        result.IsApproved = false;
        //        result.HandleMessage = GetGarantiErrorMessage(procReturnCode, result.IsApproved, result.LanguageCulture);
        //        return result;
        //    }

        //    authCode = authCode ?? string.Empty;
        //    procReturnCode = procReturnCode ?? string.Empty;
        //    gpInstallmentAmount = gpInstallmentAmount ?? string.Empty;
        //    gpInstallment = gpInstallment ?? string.Empty;

        //    result.InstalmentCount = gpInstallment;

        //    if (!string.IsNullOrEmpty(gpInstallment) && gpInstallment.Contains("0"))
        //    {
        //        result.InstalmentCount = "1";
        //    }

        //    string garantipayhashdata = Base64EncodedSha1(vPos.TerminalId + result.BankOrder + authCode + procReturnCode + gpInstallmentAmount + gpInstallment + storeKey);

        //    if (!string.IsNullOrEmpty(gpHashData) && !gpHashData.Equals(garantipayhashdata))
        //    {
        //        result.IsApproved = false;
        //        result.HandleMessage = "Garanti Pay için güvenlik doğrulaması yapılamadı.";
        //        return result;
        //    }
        //    #endregion

        //    if (!string.IsNullOrEmpty(gpInstallmentAmount))
        //    {
        //        result.TotalAmount = gpInstallmentAmount;
        //    }

        //    return result;
        //}

        //public ProcessPaymentResult doPayment3D(CreditCardPos vPos, Dictionary<int, decimal> termDiffrence, OOSPaymentRequest model, decimal rewardPoint = 0)
        //{
        //    decimal orderTotal = Convert.ToDecimal(model.OrderTotal);
        //    ProcessPaymentResult result = new ProcessPaymentResult();

        //    string strOrderId = Utils.CreateOrder();
        //    PaymentProcess process = CreatePaymentProcess(vPos, model, strOrderId, termDiffrence, rewardPoint);

        //    string rewardAmount = model.CreditCard.MoneyPoint;
        //    string strMode = vPos.Mode.Clear();
        //    string strApiVersion = vPos.ApiVersion.Clear();
        //    string strTerminalProvUserId = vPos.ProvUser.Clear();
        //    string strType = vPos.ChargeType.Clear();
        //    decimal vadeFarkiOran = termDiffrence[model.CreditCard.Installment];
        //    decimal vadeliToplamTutar = (orderTotal) * (1 + vadeFarkiOran);
        //    decimal subtotal = Math.Round(vadeliToplamTutar, 2);
        //    string strAmount = subtotal.ToString().Replace(",", "").Replace(".", "");
        //    string strCurrencyCode = vPos.CurrencyCode.Clear();
        //    string strInstallmentCount = Convert.ToInt16(model.CreditCard.Installment) == 1 ? "" : model.CreditCard.Installment.ToString();
        //    string strTerminalUserId = model.ProjectCode;
        //    string strCustomeripaddress = vPos.Mode == "TEST" ? "127.0.0.1" : HttpContext.Current.Request.UserHostAddress;
        //    string strTerminalId = vPos.TerminalId.Clear();
        //    string _strTerminalID = "0" + vPos.TerminalId.Clear();
        //    string strTerminalMerchantId = vPos.MerchantId.Clear();
        //    string strProvisionPassword = vPos.ProvPassword.Clear();
        //    string strStoreKey = vPos.StoreKey.Clear();
        //    string strSuccessUrl = vPos.SecurePaymentReturn + "?process=" + process.Id;
        //    string strErrorUrl = vPos.SecurePaymentReturn + "?process=" + process.Id;
        //    string securityData = GetSha1(strProvisionPassword + _strTerminalID).ToUpper();
        //    string hashData = GetSha1(strTerminalId + strOrderId + strAmount + strSuccessUrl + strErrorUrl + strType + strInstallmentCount + strStoreKey + securityData).ToUpper();
        //    string strsecure3Dsecuritylevel = vPos.StoreType;
        //    //string cardHolder = model.CreditCard.CardholderName;
        //    //string smpCode = model.ProjectCode;
        //    string ccv = model.CreditCard.CardCode;
        //    string pan = model.CreditCard.CardNumber;
        //    string ecomPaymentCardExpDateYear = model.CreditCard.ExpireYear.Substring(model.CreditCard.ExpireYear.Length - 2, 2);
        //    string ecomPaymentCardExpDateMonth = model.CreditCard.ExpireMonth;

        //    if (ecomPaymentCardExpDateMonth.Length == 1)
        //    {
        //        ecomPaymentCardExpDateMonth = "0" + ecomPaymentCardExpDateMonth;
        //    }
        //    else
        //    {
        //        ecomPaymentCardExpDateMonth = ecomPaymentCardExpDateMonth;
        //    }
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("secure3dsecuritylevel="); sb.Append(strsecure3Dsecuritylevel);
        //    sb.Append("&cardnumber="); sb.Append(pan);
        //    sb.Append("&cardexpiredatemonth="); sb.Append(ecomPaymentCardExpDateMonth);
        //    sb.Append("&cardexpiredateyear="); sb.Append(ecomPaymentCardExpDateYear);
        //    sb.Append("&cardcvv2="); sb.Append(ccv);
        //    sb.Append("&mode="); sb.Append(strMode);
        //    sb.Append("&apiversion="); sb.Append(strApiVersion);
        //    sb.Append("&terminalprovuserid="); sb.Append(strTerminalProvUserId);
        //    sb.Append("&terminaluserid="); sb.Append(strTerminalUserId);
        //    sb.Append("&terminalmerchantid="); sb.Append(strTerminalMerchantId);
        //    sb.Append("&txntype="); sb.Append(strType);
        //    sb.Append("&txnamount="); sb.Append(strAmount);
        //    sb.Append("&txncurrencycode="); sb.Append(strCurrencyCode);
        //    sb.Append("&txninstallmentcount="); sb.Append(strInstallmentCount);
        //    sb.Append("&orderid="); sb.Append(strOrderId);
        //    sb.Append("&terminalid="); sb.Append(strTerminalId);
        //    sb.Append("&successurl="); sb.Append(strSuccessUrl);
        //    sb.Append("&errorurl="); sb.Append(strErrorUrl);
        //    sb.Append("&customeripaddress="); sb.Append(strCustomeripaddress);
        //    sb.Append("&secure3dhash="); sb.Append(hashData);

        //    try
        //    {
        //        HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(vPos.Host3d);
        //        string proxy = null;
        //        byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());

        //        hwr.Method = "POST";
        //        hwr.ContentType = "application/x-www-form-urlencoded";
        //        hwr.ContentLength = buffer.Length;
        //        hwr.AllowAutoRedirect = false;
        //        hwr.MaximumAutomaticRedirections = 250;
        //        hwr.Timeout = 100000;

        //        hwr.Proxy = new WebProxy(proxy, true);
        //        hwr.Credentials = CredentialCache.DefaultCredentials;
        //        ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
        //        hwr.CookieContainer = new CookieContainer();

        //        Stream datagonder = hwr.GetRequestStream();
        //        datagonder.Write(buffer, 0, buffer.Length);
        //        datagonder.Flush();
        //        datagonder.Close();

        //        HttpWebResponse hwresOdeme = (HttpWebResponse)hwr.GetResponse();
        //        string uriString = hwresOdeme.Headers["Location"];

        //        Stream donenveri = hwresOdeme.GetResponseStream();
        //        StreamReader srOdeme = new StreamReader(donenveri);
        //        string strSonuc = srOdeme.ReadToEnd();

        //        hwresOdeme.Close();
        //        result.SecureOdemeGuvenlikSayfasiString = strSonuc;

        //        #region Request Log
        //        GarantiRequest garantiRequest = new GarantiRequest
        //        {
        //            Secure3Dsecuritylevel = strsecure3Dsecuritylevel,
        //            CardNumber = (model.CreditCard.CardNumber1 + model.CreditCard.CardNumber2).Substring(0, 6) + "******" + model.CreditCard.CardNumber4,
        //            Mode = strMode,
        //            ApiVersion = strApiVersion,
        //            TerminalProvUserId = strTerminalProvUserId,
        //            TerminalUserId = strTerminalUserId,
        //            TerminalMerchantId = strTerminalMerchantId,
        //            TxnType = strType,
        //            TxnAmount = strAmount,
        //            TxnCurrencyCode = strCurrencyCode,
        //            TxnInstallmentCount = strInstallmentCount,
        //            OrderId = strOrderId,
        //            TerminalId = strTerminalId,
        //            SuccessUrl = strSuccessUrl,
        //            ErrorUrl = strErrorUrl,
        //            CustomerIpAddress = strCustomeripaddress,
        //            Secure3Dhash = hashData
        //        };
        //        AddLog(new ProjectProcess
        //        {
        //            ClientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
        //            Data = JsonConvert.SerializeObject(garantiRequest),
        //            Content = strSonuc,
        //            Text = "Garanti 3D Request",
        //            ProjectCode = model.ProjectCode,
        //            CustomerId = model.CustomerId,
        //            UserName = model.Email,
        //            TransDate = DateTime.Now
        //        });
        //        #endregion
        //    }

        //    catch (WebException ex)
        //    {
        //        AddExceptionLog(ex);
        //        result.Errors.Add("Bilgi doğrulama sırasında bir hata ile karşılaşıldı: " + ex.Message);
        //    }

        //    return result;
        //}

        //public PaymentProcess Get3DResult(CreditCardPos vPos, PaymentProcess result)
        //{
        //    #region Request parameter
        //    NameValueCollection requestForm = HttpContext.Current.Request.Form;
        //    string storekey = vPos.StoreKey.Clear();
        //    string hashparams = requestForm.Get("HASHPARAMS");
        //    string hashparamsval = requestForm.Get("HASHPARAMSVAL");
        //    string hashparam = requestForm.Get("HASH");
        //    string secure3DSecurityLevel = requestForm.Get("secure3dsecuritylevel");
        //    string paramsval = "";
        //    string taksitval = requestForm.Get("taksit");
        //    string ecomPaymentCardExpDateMonth = requestForm.Get("Ecom_Payment_Card_ExpDate_Month");
        //    string ecomPaymentCardExpDateYear = requestForm.Get("Ecom_Payment_Card_ExpDate_Year");
        //    string clientidval = requestForm.Get("clientid");
        //    string expiresval = ecomPaymentCardExpDateMonth + "/" + ecomPaymentCardExpDateYear;
        //    string cv2val = requestForm.Get("cv2");
        //    string totalval = requestForm.Get("amount");
        //    string numberval = requestForm.Get("md");
        //    string strMode = requestForm.Get("mode");
        //    string strApiVersion = requestForm.Get("apiversion");
        //    string strTerminalProvUserID = requestForm.Get("terminalprovuserid");
        //    string strType = requestForm.Get("txntype");
        //    string strAmount = requestForm.Get("txnamount");
        //    string strCurrencyCode = requestForm.Get("txncurrencycode");
        //    string strInstallmentCount = requestForm.Get("txninstallmentcount");
        //    string strTerminalUserID = requestForm.Get("terminaluserid");
        //    string strOrderID = requestForm.Get("oid");
        //    string strCustomeripaddress = requestForm.Get("customeripaddress");
        //    string strcustomeremailaddress = requestForm.Get("customeremailaddress");
        //    string strTerminalID = requestForm.Get("clientid");
        //    string strTerminalMerchantID = requestForm.Get("terminalmerchantid");
        //    string strSuccessURL = requestForm.Get("successurl");
        //    string strErrorURL = requestForm.Get("errorurl");
        //    string _authcode = requestForm.Get("cavv");
        //    string _sclevel = requestForm.Get("eci");
        //    string _strTxnID = requestForm.Get("xid");
        //    string _strmd = requestForm.Get("md");
        //    string strAuthenticationCode = requestForm.Get("cavv");
        //    string strSecurityLevel = requestForm.Get("eci");
        //    string strTxnID = requestForm.Get("xid");
        //    string strMD = requestForm.Get("md");
        //    string strMDStatus = requestForm.Get("mdstatus");
        //    string strMDStatusText = requestForm.Get("mderrormessage");
        //    string payersecuritylevelval = requestForm.Get("eci");
        //    string payertxnidval = requestForm.Get("xid");
        //    string payerauthenticationcodeval = requestForm.Get("cavv");
        //    string mdstatus = requestForm.Get("mdStatus");
        //    string procReturnCode = requestForm.Get("procreturncode");

        //    #endregion

        //    #region Response Log
        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    requestForm.CopyTo(dict);

        //    int customerId = 0;
        //    int.TryParse(result.CustomerId, out customerId);
        //    string response = JsonConvert.SerializeObject(dict);
        //    ProjectProcess projectProcess = new ProjectProcess
        //    {
        //        ClientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
        //        Data = response,
        //        Text = "Garanti 3D Response",
        //        ProjectCode = result.PaymentProjectCode,
        //        CustomerId = customerId,
        //        UserName = result.UserName,
        //        TransDate = DateTime.Now
        //    };

        //    AddLog(projectProcess);
        //    #endregion

        //    if (!string.IsNullOrEmpty(mdstatus) && mdstatus.Contains("7") &&
        //        !string.IsNullOrEmpty(procReturnCode) && procReturnCode.Contains("99")) //3D Onayı alınamadı.
        //    {
        //        result.IsApproved = false;//3D işlem onayı alınamadı
        //        result.HandleMessage = "Garanti Bankası 3D sistem hatası.Lütfen daha sonra tekrar deneyiniz.";
        //        result.HostRefNum = "";
        //        result.HostMessage = strMDStatusText;
        //        result.ErrorMessage = "";
        //        result.ProcReturnCode = "";
        //        result.AuthCode = _authcode;
        //        result.MdStatus = mdstatus;
        //        result.TransId = null;
        //        result.ChargeType = strType;
        //        result.StoreType = secure3DSecurityLevel;

        //        return result;
        //    }
        //    int index1 = 0, index2 = 0;
        //    try
        //    {
        //        do
        //        {
        //            index2 = hashparams.IndexOf(":", index1);
        //            String val = requestForm.Get(hashparams.Substring(index1, index2 - index1)) == null ? "" : requestForm.Get(hashparams.Substring(index1, index2 - index1));
        //            paramsval += val;
        //            index1 = index2 + 1;
        //        }
        //        while (index1 < hashparams.Length);
        //    }
        //    catch (Exception ex)
        //    {
        //        //LogHelper.Log(ex);
        //        AddExceptionLog(ex);

        //        result.IsApproved = false;
        //        result.ErrorMessage = ex.Message;
        //        result.HandleMessage = "işlem bankaya gönderilirken hata oluştu. Lütfen tekrar deneyiniz.";

        //        return result;
        //    }

        //    String hashval = paramsval + storekey;
        //    SHA1 sha = new SHA1CryptoServiceProvider();
        //    byte[] hashbytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(hashval);
        //    byte[] inputbytes = sha.ComputeHash(hashbytes);
        //    String hash = Convert.ToBase64String(inputbytes);

        //    if (!paramsval.Equals(hashparamsval) || !hash.Equals(hashparam))
        //    {
        //        result.IsApproved = false;
        //        result.ErrorMessage = "Gönderilen hash değeri ile dönen hash değeri farklı";
        //    }
        //    string _strTerminalID = "0" + strTerminalID;
        //    string strProvisionPassword = vPos.ProvPassword.Clear();
        //    string strCardholderPresentCode = "13";
        //    string strMotoInd = "N";
        //    string SecurityData = GetSha1(strProvisionPassword + _strTerminalID).ToUpper();
        //    string HashData = GetSha1(strOrderID + strTerminalID + strAmount + SecurityData).ToUpper();
        //    string rewardType = "BNS";
        //    decimal rewardAmount = 0;

        //    decimal.TryParse(result.RewardAmount, out rewardAmount);
        //    string strRewardAmount = Math.Round(rewardAmount, 2).ToString().Replace(",", "").Replace(".", "");

        //    // mdStatus 3d işlemin sonucu ile ilgili bilgi verir. 1,2,3,4 başarılı, 5,6,7,8,9,0 başarısızdır.
        //    // 3D parametreler
        //    if (mdstatus.Equals("1") || mdstatus.Equals("2") || mdstatus.Equals("3") || mdstatus.Equals("4")) //3D Onayı alınmıştır.
        //    {
        //        #region GVPS XML
        //        StringBuilder gvpsXml = new StringBuilder();
        //        gvpsXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //        gvpsXml.Append("<GVPSRequest>");
        //        gvpsXml.Append("<Mode>");
        //        gvpsXml.Append(strMode);
        //        gvpsXml.Append("</Mode>");
        //        gvpsXml.Append("<Version>");
        //        gvpsXml.Append(strApiVersion);
        //        gvpsXml.Append("</Version><Terminal><ProvUserID>");
        //        gvpsXml.Append(strTerminalProvUserID);
        //        gvpsXml.Append("</ProvUserID><HashData>");
        //        gvpsXml.Append(HashData);
        //        gvpsXml.Append("</HashData><UserID>");
        //        gvpsXml.Append(strTerminalUserID);
        //        gvpsXml.Append("</UserID><ID>");
        //        gvpsXml.Append(strTerminalID);
        //        gvpsXml.Append("</ID><MerchantID>");
        //        gvpsXml.Append(strTerminalMerchantID);
        //        gvpsXml.Append("</MerchantID></Terminal>");
        //        gvpsXml.Append("<Customer><IPAddress>");
        //        gvpsXml.Append("127.0.0.1");
        //        gvpsXml.Append("</IPAddress><EmailAddress></EmailAddress></Customer>");
        //        gvpsXml.Append("<Card><Number></Number><ExpireDate></ExpireDate><CVV2></CVV2></Card>");
        //        gvpsXml.Append("<Order><OrderID>");
        //        gvpsXml.Append(strOrderID);
        //        gvpsXml.Append("</OrderID><GroupID></GroupID><Description></Description></Order>");
        //        gvpsXml.Append("<Transaction>");
        //        gvpsXml.Append("<Type>");
        //        gvpsXml.Append(strType);
        //        gvpsXml.Append("</Type><InstallmentCnt>");
        //        gvpsXml.Append(strInstallmentCount);
        //        gvpsXml.Append("</InstallmentCnt><Amount>");
        //        gvpsXml.Append(strAmount);
        //        gvpsXml.Append("</Amount><CurrencyCode>");
        //        gvpsXml.Append(strCurrencyCode);
        //        gvpsXml.Append("</CurrencyCode><CardholderPresentCode>");
        //        gvpsXml.Append(strCardholderPresentCode);
        //        gvpsXml.Append("</CardholderPresentCode><MotoInd>");
        //        gvpsXml.Append(strMotoInd);
        //        gvpsXml.Append("</MotoInd>");
        //        gvpsXml.Append("<Secure3D><AuthenticationCode>");
        //        gvpsXml.Append(strAuthenticationCode);
        //        gvpsXml.Append("</AuthenticationCode><SecurityLevel>");
        //        gvpsXml.Append(strSecurityLevel);
        //        gvpsXml.Append("</SecurityLevel><TxnID>");
        //        gvpsXml.Append(strTxnID);
        //        gvpsXml.Append("</TxnID><Md>");
        //        gvpsXml.Append(strMD);
        //        gvpsXml.Append("</Md></Secure3D>");
        //        if (rewardAmount > 0)
        //        {
        //            gvpsXml.Append("<RewardList>");
        //            gvpsXml.Append("<Reward>");
        //            gvpsXml.Append("<Type>"); gvpsXml.Append(rewardType); ; gvpsXml.Append("</Type>");
        //            gvpsXml.Append("<UsedAmount>"); gvpsXml.Append(strRewardAmount); ; gvpsXml.Append("</UsedAmount>");
        //            gvpsXml.Append("</Reward>");
        //            gvpsXml.Append("</RewardList>");
        //        }
        //        gvpsXml.Append("</Transaction>");
        //        gvpsXml.Append("</GVPSRequest>");
        //        #endregion

        //        #region Xml Request
        //        ProjectProcess xmlProcess = new ProjectProcess
        //        {
        //            ClientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
        //            Data = gvpsXml.ToString(),
        //            Text = "Garanti xml request",
        //            ProjectCode = result.PaymentProjectCode,
        //            CustomerId = customerId,
        //            UserName = result.UserName,
        //            TransDate = DateTime.Now
        //        };

        //        AddLog(xmlProcess);
        //        #endregion

        //        string gelenXml = "";
        //        try
        //        {
        //            string data = "data=" + gvpsXml;

        //            WebRequest webRequest = WebRequest.Create(vPos.HostXml.Clear());
        //            webRequest.Method = "POST";

        //            byte[] byteArray = Encoding.UTF8.GetBytes(data);
        //            webRequest.ContentType = "application/x-www-form-urlencoded";
        //            webRequest.ContentLength = byteArray.Length;

        //            Stream dataStream = webRequest.GetRequestStream();
        //            dataStream.Write(byteArray, 0, byteArray.Length);
        //            dataStream.Close();

        //            WebResponse webResponse = webRequest.GetResponse();
        //            dataStream = webResponse.GetResponseStream();

        //            StreamReader reader = new StreamReader(dataStream);
        //            gelenXml = reader.ReadToEnd();

        //            XmlDocument xDoc = new XmlDocument();
        //            xDoc.LoadXml(gelenXml);

        //            #region Payment Process result
        //            XmlElement xEReasonCode = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/ReasonCode") as XmlElement;
        //            XmlElement xEErrorMsg = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/ErrorMsg") as XmlElement;
        //            XmlElement xESysErrMsg = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/SysErrMsg") as XmlElement;
        //            XmlElement xERetrefNum = xDoc.SelectSingleNode("//GVPSResponse/Transaction/RetrefNum") as XmlElement;
        //            XmlElement xEAuthCode = xDoc.SelectSingleNode("//GVPSResponse/Transaction/AuthCode") as XmlElement;
        //            //XmlElement xEClientIp = xDoc.SelectSingleNode("//GVPSResponse/Customer/IPAddress") as XmlElement;

        //            if (xERetrefNum != null) result.HostRefNum = xERetrefNum.InnerText;
        //            if (xESysErrMsg != null) result.HostMessage = xESysErrMsg.InnerText;
        //            if (xEErrorMsg != null) result.ErrorMessage = xEErrorMsg.InnerText;
        //            if (xEReasonCode != null) result.ProcReturnCode = xEReasonCode.InnerText;
        //            if (xEAuthCode != null) result.AuthCode = xEAuthCode.InnerText;
        //            result.MdStatus = mdstatus;
        //            result.TransId = null;
        //            #endregion

        //            if (gelenXml.Contains("<ReasonCode>00</ReasonCode>") && gelenXml.Contains("<Message>Approved</Message>"))
        //            {
        //                result.IsApproved = true;//3D işlem onayı alındı."
        //            }
        //            else
        //            {
        //                result.IsApproved = false;//3D işlem onayı alınamadı
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            AddExceptionLog(ex);

        //            result.IsApproved = false;
        //            result.ErrorMessage = "işlem bankaya gönderilirken hata oluştu = > " + ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        result.IsApproved = false;//3D işlem onayı alınamadı
        //        result.HandleMessage = "3D işlem onayı alınamadı";
        //        result.HostRefNum = "";
        //        result.HostMessage = strMDStatusText;
        //        result.ErrorMessage = "";
        //        result.ProcReturnCode = "";
        //        result.AuthCode = _authcode;
        //        result.MdStatus = mdstatus;
        //        result.TransId = null;
        //        result.ChargeType = strType;
        //        result.StoreType = secure3DSecurityLevel;

        //        return result;
        //    }
        //    result.HandleMessage = GetGarantiErrorMessage(result.ProcReturnCode, result.IsApproved, result.LanguageCulture);

        //    return result;
        //}

        //public PaymentProcess doPaymentXML(CreditCardPos vPos, CreditCardBank bank, Dictionary<int, decimal> termDiffrence, PaymentInfoModel paymentInfoModel)
        //{
        //    PaymentProcess process = new PaymentProcess();
        //    DateTime now = DateTime.Now;

        //    string resultXml = string.Empty;
        //    string data = string.Empty;
        //    decimal orderTotal = Convert.ToDecimal(paymentInfoModel.OrderTotal);
        //    string strMode = vPos.Mode;
        //    string strVersion = vPos.ApiVersion;
        //    string strTerminalID = vPos.TerminalId;
        //    string _strTerminalID = "0" + vPos.TerminalId;
        //    string strProvUserID = vPos.ProvUser;
        //    string strProvisionPassword = vPos.ProvPassword;
        //    string strUserID = vPos.TerminalUser;
        //    string strMerchantID = vPos.MerchantId;
        //    string strIPAddress = _securityHelper.GetClientIpAddress();
        //    string strEmailAddress = "default@none.mail";
        //    string strOrderID = now.Day.ToString() + now.Month.ToString() + now.Year.ToString() + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
        //    string strNumber = paymentInfoModel.CardNumber;
        //    string creditCardExpireMonth = "";
        //    if (paymentInfoModel.ExpireMonth.ToString().Length == 1)
        //    {
        //        creditCardExpireMonth = "0" + paymentInfoModel.ExpireMonth.ToString();
        //    }
        //    else
        //    {
        //        creditCardExpireMonth = paymentInfoModel.ExpireMonth.ToString();
        //    }
        //    string strExpireDate = creditCardExpireMonth + paymentInfoModel.ExpireYear.ToString().Substring(2);
        //    string strCVV2 = paymentInfoModel.CardCode;
        //    decimal vadeFarkiOran = termDiffrence[int.Parse(paymentInfoModel.Installment)];
        //    decimal vadeliToplamTutar = (orderTotal) * (1 + vadeFarkiOran);
        //    decimal subtotal = Math.Round(vadeliToplamTutar, 2);
        //    string strAmount = subtotal.ToString().Replace(",", "").Replace(".", "");
        //    string strType = vPos.ChargeType;
        //    string strCurrencyCode = vPos.CurrencyCode;
        //    string strCardholderPresentCode = "0";
        //    string strMotoInd = "N";
        //    string strInstallmentCount = paymentInfoModel.Installment == "1" ? "" : paymentInfoModel.Installment;
        //    string strHostAddress = vPos.HostXml;

        //    string securityData = GetSha1(strProvisionPassword + _strTerminalID).ToUpper();
        //    string hashData = GetSha1(strOrderID + strTerminalID + strNumber + strAmount + securityData).ToUpper();

        //    #region Post string
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //    sb.Append("<GVPSRequest>");
        //    sb.Append("<Mode>"); sb.Append(strMode); sb.Append("</Mode>");
        //    sb.Append("<Version>"); sb.Append(strVersion); sb.Append("</Version>");
        //    sb.Append("<Terminal>");
        //    sb.Append("<ProvUserID>"); sb.Append(strProvUserID); sb.Append("</ProvUserID>");
        //    sb.Append("<HashData>"); sb.Append(hashData); sb.Append("</HashData>");
        //    sb.Append("<UserID>"); sb.Append(strUserID); sb.Append("</UserID>");
        //    sb.Append("<ID>"); sb.Append(strTerminalID); sb.Append("</ID>");
        //    sb.Append("<MerchantID>"); sb.Append(strMerchantID); sb.Append("</MerchantID>");
        //    sb.Append("</Terminal>");
        //    sb.Append("<Order>");
        //    sb.Append("<OrderID>"); sb.Append(strOrderID); sb.Append("</OrderID>");
        //    sb.Append("</Order>");
        //    sb.Append("<Customer>");
        //    sb.Append("<IPAddress>"); sb.Append(strIPAddress); sb.Append("</IPAddress>");
        //    sb.Append("<EmailAddress>"); sb.Append(strEmailAddress); sb.Append("</EmailAddress>");
        //    sb.Append("</Customer>");
        //    sb.Append("<Card>");
        //    sb.Append("<Number>"); sb.Append(strNumber); sb.Append("</Number>");
        //    sb.Append("<ExpireDate>"); sb.Append(strExpireDate); sb.Append("</ExpireDate>");
        //    sb.Append("<CVV2>"); sb.Append(strCVV2); sb.Append("</CVV2>");
        //    sb.Append("</Card>");
        //    sb.Append("<Transaction>");
        //    sb.Append("<Type>"); sb.Append(strType); sb.Append("</Type>");
        //    sb.Append("<InstallmentCnt>"); sb.Append(strInstallmentCount); sb.Append("</InstallmentCnt>");
        //    sb.Append("<Amount>"); sb.Append(strAmount); sb.Append("</Amount>");
        //    sb.Append("<CurrencyCode>"); sb.Append(strCurrencyCode); sb.Append("</CurrencyCode>");
        //    sb.Append("<CardholderPresentCode>"); sb.Append(strCardholderPresentCode); sb.Append("</CardholderPresentCode>");
        //    sb.Append("<MotoInd>"); sb.Append(strMotoInd); sb.Append("</MotoInd>");
        //    sb.Append("</Transaction>");
        //    sb.Append("</GVPSRequest>");
        //    #endregion

        //    try
        //    {
        //        data = "data=" + sb.ToString();

        //        WebRequest _WebRequest = WebRequest.Create(vPos.HostXml);
        //        _WebRequest.Method = "POST";

        //        byte[] byteArray = Encoding.UTF8.GetBytes(data);
        //        _WebRequest.ContentType = "application/x-www-form-urlencoded";
        //        _WebRequest.ContentLength = byteArray.Length;

        //        Stream dataStream = _WebRequest.GetRequestStream();
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        dataStream.Close();

        //        WebResponse _WebResponse = _WebRequest.GetResponse();
        //        dataStream = _WebResponse.GetResponseStream();

        //        StreamReader reader = new StreamReader(dataStream);
        //        resultXml = reader.ReadToEnd();

        //        System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
        //        xDoc.LoadXml(resultXml);

        //        #region Payment Process result
        //        XmlElement xEReasonCode = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/ReasonCode") as XmlElement;
        //        XmlElement xEErrorMsg = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/ErrorMsg") as XmlElement;
        //        XmlElement xESysErrMsg = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/SysErrMsg") as XmlElement;
        //        XmlElement xERetrefNum = xDoc.SelectSingleNode("//GVPSResponse/Transaction/RetrefNum") as XmlElement;
        //        XmlElement xEAuthCode = xDoc.SelectSingleNode("//GVPSResponse/Transaction/AuthCode") as XmlElement;
        //        XmlElement xEClientIp = xDoc.SelectSingleNode("//GVPSResponse/Customer/IPAddress") as XmlElement;

        //        process.CardHolder = paymentInfoModel.CardholderName;
        //        process.PaymentProjectCode = paymentInfoModel.ProjectCode;
        //        process.CardNumber = paymentInfoModel.CardNumber.Substring(0, 6).PadRight(paymentInfoModel.CardNumber.Length, '*');
        //        process.HostRefNum = xERetrefNum.InnerText;
        //        process.HostMessage = xESysErrMsg.InnerText;
        //        process.ErrorMessage = xEErrorMsg.InnerText;
        //        process.ProcReturnCode = xEReasonCode.InnerText;
        //        process.AuthCode = xEAuthCode.InnerText;
        //        process.BankOrder = strOrderID;
        //        process.TransDate = DateTime.Now;
        //        process.Browser = "";// Utils.GetBrowser();
        //        process.StoreType = vPos.StoreType;
        //        process.InstalmentCount = paymentInfoModel.Installment;
        //        process.ClientIp = xEClientIp.InnerText;
        //        process.CurrencyCode = vPos.CurrencyCode;
        //        process.CreditCardBankId = vPos.CreditCardBankId;
        //        process.CreditCardPosId = vPos.Id;
        //        process.PaymentTypeId = vPos.PaymentTypeId;
        //        process.TransId = null;
        //        process.MdStatus = null;
        //        process.ChargeType = strType;
        //        process.StoreType = vPos.StoreType;
        //        process.TotalAmount = paymentInfoModel.OrderTotal;
        //        #endregion

        //        if (resultXml.Contains("<ReasonCode>00</ReasonCode>") && resultXml.Contains("<Message>Approved</Message>"))
        //        {
        //            process.IsApproved = true;
        //        }
        //        else
        //        {
        //            process.IsApproved = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AddExceptionLog(ex);

        //        process.IsApproved = false;
        //        process.ErrorMessage = "işlem bankaya gönderilirken hata oluştu = > " + ex.Message;
        //    }
        //    return process;
        //}

        //public MoneyPointModel GetPoint(OOSPaymentRequest oosRequest, CreditCardPos vPos)
        //{
        //    #region Parameter
        //    string mode = "PROD";
        //    string type = "rewardinq";
        //    string host = vPos.HostXml;
        //    string strOrderId = _utility.GetOrderNumber();
        //    string name = vPos.ApiUser.Clear();
        //    string password = vPos.ApiPassword.Clear();
        //    string clientId = vPos.MerchantId.Clear();
        //    decimal subtotal = Math.Round(oosRequest.OrderTotal, 2);
        //    string strAmount = subtotal.ToString().Replace(",", "").Replace(".", "");
        //    string version = vPos.ApiVersion.Clear();
        //    string provUserId = vPos.ProvUser.Clear();
        //    string userId = vPos.TerminalUser.Clear();
        //    string terminalId = vPos.TerminalId.Clear();
        //    string merchantId = vPos.MerchantId.Clear();
        //    string ipAddress = _securityHelper.GetClientIpAddress();
        //    string strNumber = string.IsNullOrEmpty(oosRequest.CardNumber) ? oosRequest.CreditCard.CardNumber : oosRequest.CardNumber;
        //    string expireMonth = oosRequest.CreditCard.ExpireMonth;
        //    string expYear = oosRequest.CreditCard.ExpireYear.Substring(oosRequest.CreditCard.ExpireYear.Length - 2, 2);
        //    string total = oosRequest.OrderTotal.ToString();
        //    string currency = vPos.CurrencyCode.Clear();
        //    string email = oosRequest.Email;
        //    string strProvisionPassword = vPos.ProvPassword.Clear();
        //    string strTerminalId = vPos.TerminalId.Clear();
        //    string _strTerminalID = "0" + vPos.TerminalId.Clear();
        //    string securityData = GetSha1(strProvisionPassword + _strTerminalID).ToUpper();
        //    string hashData = GetSha1(strOrderId + strTerminalId + strNumber + strAmount + securityData).ToUpper();

        //    string procReturnCode = string.Empty;
        //    string returnXml, expireDate, errorMessage = string.Empty;

        //    expireMonth = expireMonth.Length == 1 ? "0" + expireMonth : expireMonth;
        //    expYear = expYear.Length == 1 ? "0" + expYear : expYear;
        //    expireDate = expireMonth + expYear;
        //    #endregion

        //    #region GVPSRequest
        //    StringBuilder gvpXml = new StringBuilder();
        //    gvpXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //    gvpXml.Append("<GVPSRequest>");
        //    gvpXml.Append("<Mode>"); gvpXml.Append(mode); gvpXml.Append("</Mode>");
        //    gvpXml.Append("<Version>"); gvpXml.Append(version); gvpXml.Append("</Version>");
        //    gvpXml.Append("<ChannelCode></ChannelCode>");
        //    gvpXml.Append("<Terminal>");
        //    gvpXml.Append("<ProvUserID>"); gvpXml.Append(provUserId); gvpXml.Append("</ProvUserID>");
        //    gvpXml.Append("<HashData>"); gvpXml.Append(hashData); gvpXml.Append("</HashData>");
        //    gvpXml.Append("<UserID>"); gvpXml.Append(userId); gvpXml.Append("</UserID>");
        //    gvpXml.Append("<ID>"); gvpXml.Append(terminalId); gvpXml.Append("</ID>");
        //    gvpXml.Append("<MerchantID>"); gvpXml.Append(merchantId); gvpXml.Append("</MerchantID>");
        //    gvpXml.Append("</Terminal>");
        //    gvpXml.Append("<Customer>");
        //    gvpXml.Append("<IPAddress>"); gvpXml.Append(ipAddress); gvpXml.Append("</IPAddress>");
        //    gvpXml.Append("<EmailAddress>"); gvpXml.Append(email); gvpXml.Append("</EmailAddress>");
        //    gvpXml.Append("</Customer>");
        //    gvpXml.Append("<Card>");
        //    gvpXml.Append("<Number>"); gvpXml.Append(strNumber); gvpXml.Append("</Number>");
        //    gvpXml.Append("<ExpireDate>"); gvpXml.Append(expireDate); gvpXml.Append("</ExpireDate>");
        //    gvpXml.Append("<CVV2>"); gvpXml.Append("</CVV2>");
        //    gvpXml.Append("</Card>");
        //    gvpXml.Append("<Order>");
        //    gvpXml.Append("<OrderID>"); gvpXml.Append(strOrderId); gvpXml.Append("</OrderID>");
        //    gvpXml.Append("<GroupID></GroupID>");
        //    gvpXml.Append("<AddressList>");
        //    gvpXml.Append("<Address>");
        //    gvpXml.Append("<Type>S</Type>");
        //    gvpXml.Append("<Name></Name>");
        //    gvpXml.Append("<LastName></LastName>");
        //    gvpXml.Append("<Company></Company>");
        //    gvpXml.Append("<Text></Text>");
        //    gvpXml.Append("<District></District>");
        //    gvpXml.Append("<City></City>");
        //    gvpXml.Append("<PostalCode></PostalCode>");
        //    gvpXml.Append("<Country></Country>");
        //    gvpXml.Append("<PhoneNumber></PhoneNumber>");
        //    gvpXml.Append("</Address></AddressList>");
        //    gvpXml.Append("</Order>");
        //    gvpXml.Append("<Transaction>");
        //    gvpXml.Append("<Type>"); gvpXml.Append(type); gvpXml.Append("</Type>");
        //    gvpXml.Append("<InstallmentCnt></InstallmentCnt>");
        //    gvpXml.Append("<Amount>"); gvpXml.Append(strAmount); gvpXml.Append("</Amount>");
        //    gvpXml.Append("<CurrencyCode>"); gvpXml.Append("949"); gvpXml.Append("</CurrencyCode>");
        //    gvpXml.Append("<CardholderPresentCode>"); gvpXml.Append("0"); gvpXml.Append("</CardholderPresentCode>");
        //    gvpXml.Append("<MotoInd>"); gvpXml.Append("N"); gvpXml.Append("</MotoInd>");
        //    gvpXml.Append("<Secure3D>");
        //    gvpXml.Append("<AuthenticationCode></AuthenticationCode>");
        //    gvpXml.Append("<SecurityLevel></SecurityLevel>");
        //    gvpXml.Append("<TxnID></TxnID>");
        //    gvpXml.Append("<Md></Md>");
        //    gvpXml.Append("</Secure3D>");
        //    gvpXml.Append("</Transaction>");
        //    gvpXml.Append("</GVPSRequest>");
        //    #endregion

        //    #region SSL/TLS
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.DefaultConnectionLimit = 9999;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
        //    #endregion

        //    string data = gvpXml.ToString();
        //    WebRequest webRequest = WebRequest.Create(host);
        //    webRequest.Method = "POST";

        //    byte[] byteArray = Encoding.UTF8.GetBytes(data);
        //    webRequest.ContentType = "application/x-www-form-urlencoded";
        //    webRequest.ContentLength = byteArray.Length;

        //    Stream dataStream = webRequest.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse webResponse = webRequest.GetResponse();
        //    dataStream = webResponse.GetResponseStream();

        //    StreamReader reader = new StreamReader(dataStream);
        //    returnXml = reader.ReadToEnd();

        //    #region Request And Response Log
        //    XmlDocument gvpsRequest = new XmlDocument();
        //    gvpsRequest.LoadXml(data);
        //    gvpsRequest.DocumentElement.SelectSingleNode("/GVPSRequest/Card/Number").InnerText = strNumber.Substring(0, 6).PadRight(strNumber.Length, '*');
        //    gvpsRequest.DocumentElement.SelectSingleNode("/GVPSRequest/Card/ExpireDate").InnerText = string.Empty;
        //    gvpsRequest.DocumentElement.SelectSingleNode("/GVPSRequest/Card/CVV2").InnerText = string.Empty;

        //    ProjectProcess projectProcess = new ProjectProcess();
        //    projectProcess.ClientIp = _securityHelper.GetClientIpAddress();
        //    projectProcess.Data = gvpsRequest.InnerXml;
        //    projectProcess.Content = returnXml;
        //    projectProcess.Text = vPos.CreditCardBankName + " Money Point Request";
        //    projectProcess.ProjectCode = oosRequest.ProjectCode;
        //    projectProcess.UserName = string.IsNullOrEmpty(oosRequest.Email) ? oosRequest.UserName : oosRequest.Email;
        //    projectProcess.CustomerId = oosRequest.CustomerId;
        //    projectProcess.TransDate = DateTime.Now;

        //    AddLog(projectProcess);
        //    #endregion

        //    XmlDocument gvpsResponse = new XmlDocument();
        //    gvpsResponse.LoadXml(returnXml);
        //    XmlElement xEReasonCode = gvpsResponse.SelectSingleNode("//GVPSResponse/Transaction/Response/ReasonCode") as XmlElement;
        //    if (xEReasonCode != null) procReturnCode = xEReasonCode.InnerText;
        //    decimal garantiPoint = 0;
        //    MoneyPointModel moneyPoint = new MoneyPointModel();

        //    if (returnXml.Contains("<ReasonCode>00</ReasonCode>") && returnXml.Contains("<Message>Approved</Message>"))
        //    {
        //        List<GarantiReward> rewards = new List<GarantiReward>();
        //        XmlNodeList xnList = gvpsResponse.SelectNodes("/GVPSResponse/Transaction/RewardInqResult/RewardList");
        //        foreach (XmlNode xn in xnList)
        //        {
        //            XmlNode reward = xn.SelectSingleNode("Reward");
        //            if (reward != null)
        //            {
        //                rewards.Add(new GarantiReward
        //                {
        //                    Type = reward["Type"].InnerText,
        //                    TotalAmount = reward["TotalAmount"].InnerText,
        //                    LastTxnGainAmount = reward["LastTxnGainAmount"].InnerText
        //                });
        //            }
        //        }
        //        if (rewards.Count == 0)
        //        {
        //            moneyPoint.MoneyPoint = 0;
        //            moneyPoint.IsApproved = true;
        //            return moneyPoint;
        //        }

        //        GarantiReward garantiReward = rewards.FirstOrDefault();
        //        decimal.TryParse(garantiReward.TotalAmount, out garantiPoint);
        //        moneyPoint.MoneyPoint = (garantiPoint / 100);
        //        //moneyPoint.MoneyPoint = Convert.ToDecimal(10.60);//Test için sabit 10 TL

        //        moneyPoint.IsApproved = true;

        //        return moneyPoint;
        //    }
        //    else
        //    {
        //        moneyPoint.ProjectCurrency = oosRequest.CurrencyCode;
        //        moneyPoint.Message = GetErrorMessage(procReturnCode, false, oosRequest.LanguageCulture);
        //        moneyPoint.IsApproved = false;

        //        return moneyPoint;
        //    }
        //}

        //public PaymentProcess UseGarantiMoneyPoint(OOSPaymentRequest oosRequest, CreditCardPos vPos, decimal rewardPoint)
        //{
        //    #region Parameters
        //    string amount = Math.Round(rewardPoint, 2).ToString().Replace(",", "").Replace(".", "");
        //    string orderId = Utils.CreateOrder();
        //    string returnXml = string.Empty;
        //    string host = vPos.HostXml.Clear();
        //    string mode = "PROD";
        //    string version = vPos.ApiVersion.Clear();
        //    string terminalProvUserId = vPos.ProvUser.Clear();
        //    string terminalUserId = vPos.TerminalUser.Clear();
        //    string terminalId = vPos.TerminalId.Clear();
        //    string _terminalId = "0" + vPos.TerminalId.Clear();
        //    string merchantId = vPos.MerchantId.Clear();
        //    string provisionPassword = vPos.ProvPassword.Clear();
        //    string rewardUsedAmount = amount;
        //    string cvv2 = oosRequest.CreditCard.CardCode;
        //    string orderType = "S";
        //    string transactionType = "sales";
        //    string currencyCode = "949";
        //    string cardholderPresentCode = "0";
        //    string motoInd = "N";
        //    string rewardType = "BNS";
        //    string number = string.IsNullOrEmpty(oosRequest.CardNumber) ? oosRequest.CreditCard.CardNumber : oosRequest.CardNumber;
        //    string expireMonth = oosRequest.CreditCard.ExpireMonth;
        //    string expYear = oosRequest.CreditCard.ExpireYear.Substring(oosRequest.CreditCard.ExpireYear.Length - 2, 2);
        //    expireMonth = expireMonth.Length == 1 ? "0" + expireMonth : expireMonth;
        //    expYear = expYear.Length == 1 ? "0" + expYear : expYear;
        //    string expireDate = expireMonth + expYear;

        //    string securityData = GetSha1(provisionPassword + _terminalId).ToUpper();
        //    string hashData = GetSha1(orderId + terminalId + number + amount + securityData).ToUpper();

        //    #endregion

        //    #region GVPSRequest
        //    StringBuilder gvpsXml = new StringBuilder();
        //    gvpsXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //    gvpsXml.Append("<GVPSRequest>");
        //    gvpsXml.Append("<Mode>"); gvpsXml.Append(mode); gvpsXml.Append("</Mode>");
        //    gvpsXml.Append("<Version>"); gvpsXml.Append(version); gvpsXml.Append("</Version>");
        //    gvpsXml.Append("<ChannelCode></ChannelCode>");
        //    gvpsXml.Append("<Terminal>");
        //    gvpsXml.Append("<ProvUserID>"); gvpsXml.Append(terminalProvUserId); gvpsXml.Append("</ProvUserID>");
        //    gvpsXml.Append("<HashData>"); gvpsXml.Append(hashData); gvpsXml.Append("</HashData>");
        //    gvpsXml.Append("<UserID>"); gvpsXml.Append(terminalUserId); gvpsXml.Append("</UserID>");
        //    gvpsXml.Append("<ID>"); gvpsXml.Append(terminalId); gvpsXml.Append("</ID>");
        //    gvpsXml.Append("<MerchantID>"); gvpsXml.Append(merchantId); gvpsXml.Append("</MerchantID>");
        //    gvpsXml.Append("</Terminal>");
        //    gvpsXml.Append("<Customer>");
        //    gvpsXml.Append("<IPAddress>"); gvpsXml.Append("127.0.0.1"); gvpsXml.Append("</IPAddress>");
        //    gvpsXml.Append("<EmailAddress></EmailAddress>");
        //    gvpsXml.Append("</Customer>");
        //    gvpsXml.Append("<Card>");
        //    gvpsXml.Append("<Number>"); gvpsXml.Append(number); ; gvpsXml.Append("</Number>");
        //    gvpsXml.Append("<ExpireDate>"); gvpsXml.Append(expireDate); ; gvpsXml.Append("</ExpireDate>");
        //    gvpsXml.Append("<CVV2>"); gvpsXml.Append(cvv2); gvpsXml.Append("</CVV2>");
        //    gvpsXml.Append("</Card>");
        //    gvpsXml.Append("<Order>");
        //    gvpsXml.Append("<OrderID>"); gvpsXml.Append(orderId); ; gvpsXml.Append("</OrderID>");
        //    gvpsXml.Append("<GroupID></GroupID>");
        //    gvpsXml.Append("<AddressList>");
        //    gvpsXml.Append("<Address>");
        //    gvpsXml.Append("<Type>"); gvpsXml.Append(orderType); ; gvpsXml.Append("</Type>");
        //    gvpsXml.Append("<Name></Name>");
        //    gvpsXml.Append("<LastName></LastName>");
        //    gvpsXml.Append("<Company></Company>");
        //    gvpsXml.Append("<Text></Text>");
        //    gvpsXml.Append("<District></District>");
        //    gvpsXml.Append("<City></City>");
        //    gvpsXml.Append("<PostalCode></PostalCode>");
        //    gvpsXml.Append("<Country></Country>");
        //    gvpsXml.Append("<PhoneNumber></PhoneNumber>");
        //    gvpsXml.Append("</Address></AddressList>");
        //    gvpsXml.Append("</Order>");
        //    gvpsXml.Append("<Transaction>");
        //    gvpsXml.Append("<Type>"); gvpsXml.Append(transactionType); gvpsXml.Append("</Type>");
        //    gvpsXml.Append("<InstallmentCnt></InstallmentCnt>");
        //    gvpsXml.Append("<Amount>"); gvpsXml.Append(amount); gvpsXml.Append("</Amount>");
        //    gvpsXml.Append("<CurrencyCode>"); gvpsXml.Append(currencyCode); ; gvpsXml.Append("</CurrencyCode>");
        //    gvpsXml.Append("<CardholderPresentCode>"); gvpsXml.Append(cardholderPresentCode); ; gvpsXml.Append("</CardholderPresentCode>");
        //    gvpsXml.Append("<MotoInd>"); gvpsXml.Append(motoInd); ; gvpsXml.Append("</MotoInd>");
        //    gvpsXml.Append("<Secure3D>");
        //    gvpsXml.Append("<AuthenticationCode></AuthenticationCode>");
        //    gvpsXml.Append("<SecurityLevel></SecurityLevel>");
        //    gvpsXml.Append("<TxnID></TxnID>");
        //    gvpsXml.Append("<Md></Md>");
        //    gvpsXml.Append("</Secure3D>");
        //    gvpsXml.Append("<RewardList>");
        //    gvpsXml.Append("<Reward>");
        //    gvpsXml.Append("<Type>"); gvpsXml.Append(rewardType); ; gvpsXml.Append("</Type>");
        //    gvpsXml.Append("<UsedAmount>"); gvpsXml.Append(rewardUsedAmount); ; gvpsXml.Append("</UsedAmount>");
        //    gvpsXml.Append("</Reward>");
        //    gvpsXml.Append("</RewardList>");
        //    gvpsXml.Append("</Transaction>");
        //    gvpsXml.Append("</GVPSRequest>");
        //    #endregion

        //    #region SSL/TLS
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.DefaultConnectionLimit = 9999;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
        //        | SecurityProtocolType.Tls11
        //        | SecurityProtocolType.Tls12
        //        | SecurityProtocolType.Ssl3;
        //    #endregion

        //    string data = "data=" + gvpsXml.ToString();

        //    WebRequest webRequest = WebRequest.Create(host);
        //    webRequest.Method = "POST";

        //    byte[] byteArray = Encoding.UTF8.GetBytes(data);
        //    webRequest.ContentType = "application/x-www-form-urlencoded";
        //    webRequest.ContentLength = byteArray.Length;

        //    Stream dataStream = webRequest.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse webResponse = webRequest.GetResponse();
        //    dataStream = webResponse.GetResponseStream();

        //    StreamReader reader = new StreamReader(dataStream);
        //    returnXml = reader.ReadToEnd();

        //    #region Request And Response Log
        //    ProjectProcess projectProcess = new ProjectProcess();
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(gvpsXml.ToString());
        //    xmlDoc.DocumentElement.SelectSingleNode("/GVPSRequest/Card/Number").InnerText = number.Substring(0, 6).PadRight(number.Length, '*');
        //    xmlDoc.DocumentElement.SelectSingleNode("/GVPSRequest/Card/ExpireDate").InnerText = string.Empty;
        //    xmlDoc.DocumentElement.SelectSingleNode("/GVPSRequest/Card/CVV2").InnerText = string.Empty;

        //    projectProcess.ClientIp = _securityHelper.GetClientIpAddress();
        //    projectProcess.Data = xmlDoc.InnerXml;
        //    projectProcess.Content = returnXml;
        //    projectProcess.Text = vPos.CreditCardBankName + "Use Money Point Request & Response";
        //    projectProcess.ProjectCode = oosRequest.ProjectCode;
        //    projectProcess.TransDate = DateTime.Now;
        //    projectProcess.UserName = string.IsNullOrEmpty(oosRequest.Email) ? oosRequest.UserName : oosRequest.Email;
        //    projectProcess.CustomerId = oosRequest.CustomerId;
        //    #endregion

        //    AddLog(projectProcess);

        //    #region Payment Process result
        //    PaymentProcess process = new PaymentProcess();
        //    XmlDocument xDoc = new XmlDocument();
        //    xDoc.LoadXml(returnXml);
        //    XmlElement xEReasonCode = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/ReasonCode") as XmlElement;
        //    XmlElement xEErrorMsg = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/ErrorMsg") as XmlElement;
        //    XmlElement xESysErrMsg = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/SysErrMsg") as XmlElement;
        //    XmlElement xERetrefNum = xDoc.SelectSingleNode("//GVPSResponse/Transaction/RetrefNum") as XmlElement;
        //    XmlElement xEAuthCode = xDoc.SelectSingleNode("//GVPSResponse/Transaction/AuthCode") as XmlElement;
        //    XmlElement xEClientIp = xDoc.SelectSingleNode("//GVPSResponse/Customer/IPAddress") as XmlElement;
        //    XmlElement xEOrderId = xDoc.SelectSingleNode("//GVPSResponse/Order/OrderID") as XmlElement;
        //    XmlElement xECode = xDoc.SelectSingleNode("//GVPSResponse/Transaction/Response/Code") as XmlElement;
        //    XmlElement xEHashData = xDoc.SelectSingleNode("//GVPSResponse/Transaction/HashData") as XmlElement;
        //    XmlElement xECardType = xDoc.SelectSingleNode("//GVPSResponse/Transaction/CardType") as XmlElement;
        //    XmlElement xECardNumberMasked = xDoc.SelectSingleNode("//GVPSResponse/Transaction/CardNumberMasked") as XmlElement;

        //    if (returnXml.Contains("<ReasonCode>00</ReasonCode>") && returnXml.Contains("<Message>Approved</Message>"))
        //    {
        //        process.IsApproved = true;
        //    }
        //    else
        //    {
        //        process.IsApproved = false;
        //    }

        //    process.HostRefNum = xERetrefNum == null ? string.Empty : xERetrefNum.InnerText;
        //    process.HostMessage = xESysErrMsg == null ? string.Empty : xESysErrMsg.InnerText;
        //    process.ErrorMessage = xEErrorMsg == null ? string.Empty : xEErrorMsg.InnerText;
        //    process.ProcReturnCode = xEReasonCode == null ? string.Empty : xEReasonCode.InnerText;
        //    process.AuthCode = xEAuthCode == null ? string.Empty : xEAuthCode.InnerText;
        //    process.BankOrder = xEOrderId == null ? string.Empty : xEOrderId.InnerText;
        //    process.ErrorCode = xECode == null ? string.Empty : xECode.InnerText;
        //    process.Hash = xEHashData == null ? string.Empty : xEHashData.InnerText;
        //    process.CardName = xECardType == null ? string.Empty : xECardType.InnerText;
        //    process.CardNumber = xECardNumberMasked == null ? string.Empty : xECardNumberMasked.InnerText;

        //    process.CardHolder = oosRequest.CreditCard.CardholderName;
        //    //process.CardNumber = number.Substring(0, 6).PadRight(number.Length, '*');
        //    process.TransDate = DateTime.Now;
        //    process.Browser = "";// Utils.GetBrowser();
        //    process.StoreType = vPos.StoreType;
        //    process.ClientIp = xEClientIp.InnerText;
        //    process.CurrencyCode = vPos.CurrencyCode;
        //    process.PaymentTypeId = (int)PaymentTypeCode.MoneyPoint;
        //    process.TransId = null;
        //    process.MdStatus = null;
        //    process.TotalAmount = rewardPoint.ToString();
        //    process.RewardAmount = rewardPoint.ToString();
        //    process.CreditCardBankId = vPos.CreditCardBankId;
        //    process.CreditCardPosId = vPos.Id;
        //    process.CustomerId = oosRequest.CustomerId.ToString();
        //    process.UserName = string.IsNullOrEmpty(oosRequest.Email) ? oosRequest.UserName : oosRequest.Email;
        //    process.LanguageCulture = oosRequest.LanguageCulture;
        //    process.PaymentProjectCode = oosRequest.ProjectCode;
        //    process.InstalmentCount = "0";
        //    process.ChargeType = "sales";
        //    process.HandleMessage = GetErrorMessage(process.ProcReturnCode, process.IsApproved, oosRequest.LanguageCulture);
        //    process.ProjectOrderGuid = oosRequest.OrderGuid.ToString();
        //    #endregion

        //    return process;
        //}

        private static string GetErrorMessage(string errorCode, bool isApproved, string culture)
        {
            if (isApproved)
            {
                return string.Empty;
            }
            errorCode = errorCode.TrimStart('0');

            //IPaymentErrorService paymentErrorService = DependencyResolver.Current.GetService<IPaymentErrorService>();
            //PaymentError error = paymentErrorService.GetPaymentError(errorCode, culture, PaymentErrorTypeCode.Garanti.ToString());

            //if (error != null)
            //{
            //    return error.Message;
            //}
            //else
            //{
            //    return string.Empty;
            //}
            return string.Empty;
            #region Error code comment
            //switch (errorCode.TrimStart('0'))
            //{
            //    case "1":
            //        return "Bankanızdan provizyon alınız.";
            //    case "2":
            //        return "Bankanızdan VISA kartınız için provizyon alınız.";
            //    case "4"://Karta El koyunuz!!!
            //    case "7"://KartaElKoyunuz.
            //    case "9"://KartYenilenmiş.Müşteridenisteyin
            //    case "18"://Kapalı kart
            //    case "34"://MuhtemelenÇalıntıKart!!!ElKoyunuz.
            //    case "36"://SınırlandırılmışKart!!ElKoyunuz.
            //    case "37"://LütfenBankaGüvenliğiniArayınız.
            //    case "38"://ŞifreGirişLimitiAşıldı!!ElKoyunuz.
            //    case "41"://KayıpKart!!!KartaElKoyunuz.
            //    case "43"://ÇalıntıKart!!!KartaElKoyunuz.
            //        return "İşleminiz gerçekleştiremiyoruz. Detaylı bilgi için lütfen bankanızla görüşün. ";
            //    case "5"://İşlem onaylanmadı.
            //        return "İşleminiz onaylanmadı. Kredi kartı bilgilerinizi kontrol ettikten sonra tekrar deneyiniz.";
            //    case "6"://İsteminiz Kabul Edilmedi.
            //        return "İsteminiz kabul edilmedi. Kredi kartı bilgilerinizi kontrol ettikten sonra tekrar deneyiniz.";
            //    case "33"://KartınSüresiDolmuş!KartaElKoyunuz.
            //        return "Kartınızın süresi dolmuş. Detaylı bilgi için lütfen bankanızla görüşün. ";
            //    case "11":
            //        return "İşleminiz gerçekleştirildi (VIP). Bankanızı arayarak teyit ediniz. ";
            //    case "13"://Geçersiz tutar.
            //        return "Gönderdiğiniz tutar geçerli formatta değil. Kredi kartı bilgilerinizi kontrol ettikten sonra tekrar deneyiniz.";
            //    case "14"://kart numarası hatalı
            //    case "15"://Bankası bulunamadı.
            //    case "55"://ŞifresiHatalı.
            //    case "56"://BuKartMevcutDeğil.
            //    case "3":
            //        return "Kredi kartı bilgileriniz hatalı. Kredi kartı bilgilerinizi kontrol ettikten sonra tekrar deneyiniz. ";
            //    case "16":
            //        return "Kredi kartınızın bakiyesi yetersiz. Başka bir kredi kartı ile tekrar deneyiniz.";
            //    case "19"://BirKereDahaProvizyonTalepEdiniz.
            //        return "İşleminizi gerçekleştiremiyoruz. Birkez daha provizyon talep ediniz.";
            //    case "17"://İşlemİptalEdildi.
            //    case "25"://BöyleBirBilgiBulunamadı.
            //    case "28"://Orijinalirededilmiş/Dosyaservisdışı.
            //    case "30"://MesajınFormatıHatalı.
            //    case "31"://Issuersign-onolmamış.
            //    case "77"://Orjinalişlemileuyumsuzbilgialındı.
            //    case "78"://AccountBalanceNotAvailable.
            //    case "81"://Şifreleme/YabancıNetworkhatası.
            //    case "83"://ŞifreDoğrulanamıyor./İletişimhatası.
            //    case "89"://Authenticationhatası.
            //        return "İşleminizi gerçekleştiremiyoruz. Kredi kartı bilgilerinizi kontrol ettikten sonra tekrar deneyiniz.";
            //    case "21":
            //    case "29"://İptalyapılamadı.(Orjinalibulunamadı)
            //        return "İşlem iptal edilemedi. Lütfen daha sonra tekrar deneyiniz.";
            //    case "32":
            //        return "İşleminiz kısmen gerçekleştirildi. Hata ile ilgili lütfen bizimle irtibata geçiniz.";
            //    case "39"://Kredihesabıtanımsız.
            //    case "51"://Hesapmüsaitdeğil.
            //    case "52"://ÇekHesabıTanımsız.
            //    case "53"://HesapTanımsız.
            //        return "Hesabınız tanımsız. Başka bir kredi kartı ile tekrar deneyiniz.";
            //    case "54":
            //        return "Kartınızın son kullanım tarihi hatalı. Başka bir kredi kartı ile tekrar deneyiniz.";
            //    case "57":
            //        return "İşleminizi gerçekleştiremiyoruz. Debit kart veya kart sahibine acık olmayan bir işlem deniyor olabilirsiniz. ";
            //    case "58":
            //        return "İşleminizi gerçekleştiremiyoruz. Mevcut Sanal POS yetkileri kısıtlanmış olabilir.";
            //    case "61":
            //        return "İşleminizi gerçekleştiremiyoruz. Para çekme limitiniz aşılıyor.";
            //    case "63":
            //        return "İşleminizi gerçekleştiremiyoruz. Bu işlemi yapmaya yetkili değilsiniz.";
            //    case "64":
            //        return "İşleminizi gerçekleştiremiyoruz. Kartınız takside uygun değil.";
            //    case "65":
            //        return "İşleminizi gerçekleştiremiyoruz. Günlük işlem adediniz dolmuş.";
            //    case "75":
            //    case "76":
            //        return "İşleminizi gerçekleştiremiyoruz. Şifre giriş limitiniz aşıldı.";
            //    case "80":
            //        return "İşleminizi gerçekleştiremiyoruz. Tarih bilginiz hatalı.";
            //    case "82":
            //        return "Kredi kartı güvenlik kodu hatalı. Kredi kartı bilgilerinizi kontrol ettikten sonra tekrar deneyiniz.";
            //    case "12":
            //        return "İşleminizi gerçekleştiremiyoruz. Kredi kartı bilgilerinizi kontrol ettikten sonra tekrar deneyiniz.";
            //    case "86":
            //    case "88":
            //        return "İşleminizi gerçekleştiremiyoruz. Şifreniz doğrulanamıyor.";
            //    case "90":
            //        return "İşleminizi gerçekleştiremiyoruz. Günsonu işlemleri yapılıyor.";
            //    case "95":
            //        return "İşleminizi gerçekleştiremiyoruz. Günlük toplamlar hatalı.";
            //    case "91":
            //    case "92":
            //    case "96":
            //        return "Bankanızdan cevap alınamıyor. Lütfen daha sonra tekrar deneyiniz.";
            //    case "93":
            //        return "Hukiki nedenlerden dolayı işleminiz reddedildi. Detaylı bilgi için lütfen bankanızla görüşün.";
            //    case "214":
            //        return "Iade tutari, satis tutarindan büyük olamaz";
            //    default:
            //        return "İşleminizi gerçekleştiremiyoruz. Kredi kartı bilgilerinizi kontrol ettikten sonra tekrar deneyiniz.";
            //} 
            #endregion
        }
    }
}
