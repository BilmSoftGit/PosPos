using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Pospos.Core.Helpers
{
    public class Utility
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public Utility(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetOrderNumber()
        {
            DateTime dt = DateTime.Now;
            return $"{dt.Day}{dt.Month}{dt.Year}{dt.Hour}{dt.Minute}{dt.Second}-{RandomNumer(3)}";
        }
        public string RandomNumer(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[RNGCryptoServiceProvider.GetInt32(s.Length)]).ToArray());
        }

        public string ToTitleCaseTR(string input)
        {
            TextInfo textInfo = new CultureInfo("tr-TR", false).TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        public string ToTitleCaseEN(string input)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        public string MoneyFormat(decimal d)
        {
            return d.ToString("N", new CultureInfo("tr-TR"));
        }

        public string GetSHA512Encrypt(string text)
        {
            SHA512 sha = new SHA512CryptoServiceProvider();

            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }
    }
}
