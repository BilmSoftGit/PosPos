using Newtonsoft.Json;
using Pospos.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Pospos.Core.Helpers
{
    public class MessageSender
    {
        public Tuple<bool, string> SendEMail(string FromEMailAddress, string DisplayName, string SmtpUsername, string SmtpAddress, int SmtpPort, string SmtpPassword, string ToEMailAddress, string Subject, string Message, bool EnableSslTls)
        {
            var result = new Tuple<bool, string>(false, "");

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(FromEMailAddress, DisplayName);
            msg.To.Add(new MailAddress(ToEMailAddress));
            msg.Subject = Subject;
            msg.IsBodyHtml = true;
            msg.Body = Message;

            SmtpClient smtp = new SmtpClient(SmtpAddress, SmtpPort);
            NetworkCredential AccountInfo = new NetworkCredential(SmtpUsername, SmtpPassword);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = AccountInfo;
            smtp.EnableSsl = EnableSslTls;

            try
            {
                smtp.Send(msg);
                result = new Tuple<bool, string>(true, "Başarılı");
            }
            catch (Exception ex)
            {
                result = new Tuple<bool, string>(false, "Başarısız giriş!");
            }

            return result;
        }

        private async Task<string> GetRegisterToken(string Username, string Password, string Scope)
        {
            string url = string.Format("https://api.dataport.com.tr/restapi/Register");

            string token = string.Empty;

            string url_parameter = string.Format("UserName={0}&Password={1}&Scope={2}&grant_type=password", Username, Password, Scope);

            var response = await SendRequest<SMSResponseModel>(url, url_parameter);

            if (response != null && !string.IsNullOrWhiteSpace(response.access_token))
                return response.access_token;

            return token;
        }

        public async Task<Tuple<bool, string>> SendSMS(SendSMSRequest SmsAccount, string Url, string Username, string Password, string Scope, string PhoneNumber, string Message)
        {
            Tuple<bool, string> result = new Tuple<bool, string>(false, "İşlem başarısız!");

            var client = new HttpClient();
            string url = Url;

            var json = JsonConvert.SerializeObject(SmsAccount);

            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Authorization", string.Format("Bearer {0}", await GetRegisterToken(Username, Password, Scope)));

            var response = SendRequest<SendSMSResponse>(url, json, header);
            result = new Tuple<bool, string>(response.Result.Error == "0", response.Result.Error);

            return result;
        }

        private async Task<T> SendRequest<T>(string address, string dataObject, IDictionary<string, string> headerParameters = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = "POST";
            var encoding = new UTF8Encoding();

            var byteArray = encoding.GetBytes(dataObject);
            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            if (headerParameters != null)
                foreach (var parameter in headerParameters)
                    request.Headers[parameter.Key] = parameter.Value;

            using (var dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            var response = (HttpWebResponse)await request.GetResponseAsync();

            T data;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                string text = await sr.ReadToEndAsync();
                data = JsonConvert.DeserializeObject<T>(text);
            }

            return data;
        }

    }
}
