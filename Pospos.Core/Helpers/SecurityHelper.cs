using Microsoft.AspNetCore.Http;
using Pospos.Core.Common;
using Pospos.Core.Helpers.Jwt;
using Pospos.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pospos.Core.Helpers
{
    public class SecurityHelper
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly AppSettings _settings;
        public SecurityHelper(IHttpContextAccessor httpContextAccesor, AppSettings settings)
        {
            _httpContextAccesor = httpContextAccesor;
            _settings = settings;
        }

        public string GetWebHostAddress()
        {
            if (_httpContextAccesor.HttpContext != null)
            {
                if (_httpContextAccesor.HttpContext.Request.Headers["HTTP_CLIENT_IP"].Count > 0)
                {
                    return _httpContextAccesor.HttpContext.Request.Headers["HTTP_CLIENT_IP"].ToString();
                }
                else
                {
                    return _httpContextAccesor.HttpContext.Request.Headers["REMOTE_ADDR"].ToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetClientIpAddress()
        {
            var hasValueClientIp = _httpContextAccesor.HttpContext.Request.Headers.TryGetValue("client-ip", out Microsoft.Extensions.Primitives.StringValues remoteIpAddress);
            if (hasValueClientIp)
            {
                return remoteIpAddress.ToString();
            }
            else
            {
                var remoteIp = _httpContextAccesor.HttpContext.Connection.RemoteIpAddress;
                if (remoteIp.ToString() == "::1") return "127.0.0.1";
                return remoteIp.ToString();
            }
        }

        public string Authenticate(UserToken model)
        {
            return JsonWebToken.Encode(model, _settings.JwtTokenKey, JwtHashAlgorithm.HS256);
        }

        public string Authenticate(string Id, List<string> roleIds)
        {
            return JsonWebToken.Encode(new UserToken()
            {
                ExpireDateTime = DateTime.Now.AddDays(3),
                Id = Id,
                RoleIds = string.Join(",", roleIds)
            }, _settings.JwtTokenKey, JwtHashAlgorithm.HS256);
        }

        public UserToken GetCurrentUser()
        {
            try
            {
                var claims = _httpContextAccesor.HttpContext.User.Identity as ClaimsIdentity;
                return new UserToken()
                {
                    Id = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value,
                    RoleIds = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value
                };
            }
            catch
            {
                return DecodeToken();
            }
        }

        public UserToken DecodeToken()
        {
            string tokenstring = GetToken();
            return JsonWebToken.DecodeToObject<UserToken>(tokenstring, _settings.JwtTokenKey, true);
        }

        public UserToken DecodeToken(string token)
        {
            return JsonWebToken.DecodeToObject<UserToken>(token, _settings.JwtTokenKey, true);
        }
        public string GetToken()
        {
            return _httpContextAccesor.HttpContext.Request.Headers["token"].ToString();
        }

        public string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Padding = PaddingMode.PKCS7;
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_settings.EncriptionKey, Encoding.UTF8.GetBytes(_settings.SaltBase));
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Padding = PaddingMode.PKCS7;
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_settings.EncriptionKey, Encoding.UTF8.GetBytes(_settings.SaltBase));
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static bool PasswordHighSecurityControl(string password)
        {
            if (SpecialCharacterControl(password) && NumberControl(password) && UpperCharacterControl(password) && LowerCharacterControl(password) && password.Length >= 8)
                return true;

            return false;
        }

        private static bool SpecialCharacterControl(string Password)
        {
            char[] charList = "!'^+%&/()=?_>£#$<½{[]}|-*".ToCharArray();

            foreach (var _char in charList)
            {
                if (Password.Contains(_char))
                    return true;

            }

            return false;
        }

        private static bool NumberControl(string Password)
        {
            char[] charList = "0123456789".ToCharArray();

            foreach (var _char in charList)
            {
                if (Password.Contains(_char))
                    return true;

            }

            return false;
        }

        private static bool UpperCharacterControl(string Password)
        {
            char[] charList = "ABCÇDEFGHIİJKLMNOÖPRSŞTUÜVYZ".ToCharArray();

            foreach (var _char in charList)
            {
                if (Password.Contains(_char))
                    return true;

            }

            return false;
        }

        private static bool LowerCharacterControl(string Password)
        {
            char[] charList = "abcçdefghıijklmnoöprsştuüvyz".ToCharArray();

            foreach (var _char in charList)
            {
                if (Password.Contains(_char))
                    return true;

            }

            return false;
        }
    }
}
