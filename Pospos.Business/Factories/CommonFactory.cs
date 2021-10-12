using Microsoft.AspNetCore.Http;
using Pospos.Core.Common;
using Pospos.Core.Helpers;
using Pospos.Domain.Entities;
using Pospos.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Business.Factories
{
    public class CommonFactory
    {
        private readonly CommonService _commonService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommonFactory(CommonService commonService, IHttpContextAccessor httpContextAccessor)
        {
            this._commonService = commonService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            var result = await _commonService.getAllCities();

            return result.data;
        }

        public async Task<IEnumerable<District>> GetDisctricts(int cityId)
        {
            var result = await _commonService.getAllDistricts(cityId);

            return result.data;
        }

        public async Task<Tuple<bool, string>> SendWaitingApprovePaymentEMail(string ToEMailAddress, string UserNameSurname, string Token)
        {
            var emailaccount = await _commonService.GetEMailAccountBySystemName("WaitingApprove");

            if (emailaccount.Status)
            {
                var data = emailaccount.data;
                string ApproveLink = string.Format("https://{0}/users/approve-email/token={1}", _httpContextAccessor.HttpContext.Request.Host, Token);
                string message = string.Format(data.MessageFormat, UserNameSurname, ApproveLink);
                string subject = string.Format(data.Subject, UserNameSurname);

                MessageSender eMailSend = new MessageSender();
                var result = eMailSend.SendEMail(data.FromAddress,
                    data.DisplayName,
                    data.SmtpUsername,
                    data.SmtpAddress,
                    data.SmtpPort,
                    data.SmtpPassword,
                    ToEMailAddress,
                    subject,
                    message,
                    true);

                if (result.Item1)
                {
                    return new Tuple<bool, string>(true, result.Item2);
                }
                else
                {
                    return new Tuple<bool, string>(false, result.Item2);
                }
            }

            return new Tuple<bool, string>(false, "");
        }

        public async Task<Tuple<bool, string>> SendWaitingApproveEMail(string ToEMailAddress, string UserNameSurname, string Token)
        {
            var emailaccount = await _commonService.GetEMailAccountBySystemName("ApproveEmailAddress");

            if (emailaccount.Status)
            {
                var data = emailaccount.data;
                string message = string.Format(data.MessageFormat, UserNameSurname, Token);
                string subject = string.Format(data.Subject, UserNameSurname);

                MessageSender eMailSend = new MessageSender();
                var result = eMailSend.SendEMail(data.FromAddress,
                    data.DisplayName,
                    data.SmtpUsername,
                    data.SmtpAddress,
                    data.SmtpPort,
                    data.SmtpPassword,
                    ToEMailAddress,
                    subject,
                    message,
                    true);

                if (result.Item1)
                {
                    return new Tuple<bool, string>(true, result.Item2);
                }
                else
                {
                    return new Tuple<bool, string>(false, result.Item2);
                }
            }

            return new Tuple<bool, string>(false, "");
        }

        public async Task<Tuple<bool, string>> SendChangePasswordEMail(string ToEMailAddress, string UserNameSurname, string Token)
        {
            var emailaccount = await _commonService.GetEMailAccountBySystemName("ChangePassword");

            string ApproveLink = string.Format("https://{0}/reset-password/token={1}", _httpContextAccessor.HttpContext.Request.Host, Token);

            if (emailaccount.Status)
            {
                var data = emailaccount.data;

                string message = string.Format(data.MessageFormat, ApproveLink);
                string subject = string.Format(data.Subject, UserNameSurname);

                MessageSender eMailSend = new MessageSender();
                var result = eMailSend.SendEMail(data.FromAddress,
                    data.DisplayName,
                    data.SmtpUsername,
                    data.SmtpAddress,
                    data.SmtpPort,
                    data.SmtpPassword,
                    ToEMailAddress,
                    subject,
                    message,
                    true);

                if (result.Item1)
                {
                    return new Tuple<bool, string>(true, result.Item2);
                }
                else
                {
                    return new Tuple<bool, string>(false, result.Item2);
                }
            }

            return new Tuple<bool, string>(false, "");
        }

        public async Task<Tuple<bool, string>> SendApproveUserPasswordEMail(string ToEMailAddress, string UserNameSurname, string Password)
        {
            var emailaccount = await _commonService.GetEMailAccountBySystemName("MemberApproved");

            string ApproveLink = string.Format("https://{0}/login", _httpContextAccessor.HttpContext.Request.Host);

            if (emailaccount.Status)
            {
                var data = emailaccount.data;

                string message = string.Format(data.MessageFormat, UserNameSurname, Password, ApproveLink);
                string subject = string.Format(data.Subject, UserNameSurname);

                MessageSender eMailSend = new MessageSender();
                var result = eMailSend.SendEMail(data.FromAddress,
                    data.DisplayName,
                    data.SmtpUsername,
                    data.SmtpAddress,
                    data.SmtpPort,
                    data.SmtpPassword,
                    ToEMailAddress,
                    subject,
                    message,
                    true);

                if (result.Item1)
                {
                    return new Tuple<bool, string>(true, result.Item2);
                }
                else
                {
                    return new Tuple<bool, string>(false, result.Item2);
                }
            }

            return new Tuple<bool, string>(false, "");
        }

        public async Task<Tuple<bool, string>> SendWaitingApproveSms(string PhoneNumber, string Token)
        {
            var smsaccount = await _commonService.GetSmsAccount("RegisterSMSAccount");

            if (smsaccount.Status)
            {
                var data = smsaccount.data;

                string message = string.Format(data.Message, Token);

                SendSMSRequest sendSMS = new SendSMSRequest
                {
                    Isunicode = data.Isunicode,
                    Operator = data.Operator.GetValueOrDefault(2).ToString(),
                    Orginator = data.Orginator,
                    ShortNumber = data.ShortNumber
                };

                sendSMS.MessageList.GSMList.Add(new GSMListSingle { value = PhoneNumber });
                sendSMS.MessageList.ContentList.Add(new ContentListSingle { value = message });

                MessageSender messageSender = new MessageSender();
                var result = await messageSender.SendSMS(sendSMS, data.Url, data.Username, data.Password, data.AccountNumber, PhoneNumber, message);

                if (result.Item1)
                {
                    return new Tuple<bool, string>(true, result.Item2);
                }
                else
                {
                    return new Tuple<bool, string>(false, result.Item2);
                }
            }

            return new Tuple<bool, string>(false, "SMS Hesabı bulunamadı!");
        }

        public async Task<Tuple<bool, string>> SendApproveUserPasswordSms(string PhoneNumber, string Password)
        {
            var smsaccount = await _commonService.GetSmsAccount("SendApprovedPasswordSMS");

            if (smsaccount.Status)
            {
                var data = smsaccount.data;

                string message = string.Format(data.Message, Password);

                SendSMSRequest sendSMS = new SendSMSRequest
                {
                    Isunicode = data.Isunicode,
                    Operator = data.Operator.GetValueOrDefault(2).ToString(),
                    Orginator = data.Orginator,
                    ShortNumber = data.ShortNumber
                };

                sendSMS.MessageList.GSMList.Add(new GSMListSingle { value = PhoneNumber });
                sendSMS.MessageList.ContentList.Add(new ContentListSingle { value = message });

                MessageSender messageSender = new MessageSender();
                var result = await messageSender.SendSMS(sendSMS, data.Url, data.Username, data.Password, data.AccountNumber, PhoneNumber, message);

                if (result.Item1)
                {
                    return new Tuple<bool, string>(true, result.Item2);
                }
                else
                {
                    return new Tuple<bool, string>(false, result.Item2);
                }
            }

            return new Tuple<bool, string>(false, "SMS Hesabı bulunamadı!");
        }

        public async Task<Tuple<bool, string>> SendLoginSms(string PhoneNumber, string Token)
        {
            var smsaccount = await _commonService.GetSmsAccount("LoginSMS");

            if (smsaccount.Status)
            {
                var data = smsaccount.data;

                string message = string.Format(data.Message, Token);

                SendSMSRequest sendSMS = new SendSMSRequest
                {
                    Isunicode = data.Isunicode,
                    Operator = data.Operator.GetValueOrDefault(2).ToString(),
                    Orginator = data.Orginator,
                    ShortNumber = data.ShortNumber
                };

                sendSMS.MessageList.GSMList.Add(new GSMListSingle { value = PhoneNumber });
                sendSMS.MessageList.ContentList.Add(new ContentListSingle { value = message });

                MessageSender messageSender = new MessageSender();
                var result = await messageSender.SendSMS(sendSMS, data.Url, data.Username, data.Password, data.AccountNumber, PhoneNumber, message);

                if (result.Item1)
                {
                    return new Tuple<bool, string>(true, result.Item2);
                }
                else
                {
                    return new Tuple<bool, string>(false, result.Item2);
                }
            }

            return new Tuple<bool, string>(false, "SMS Hesabı bulunamadı!");
        }

        public async Task<string> GetSetting(string Key)
        {
            var result = await _commonService.GetSetting(Key);

            try
            {
                return result.data.Value;
            }
            catch (Exception)
            { return string.Empty; }
        }

        public async Task<SmsAccount> GetSmsAccount(string SystemName)
        {
            var result = await _commonService.GetSmsAccount(SystemName);

            return result.data;
        }
    }
}
