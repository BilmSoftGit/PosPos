using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Pospos.Core.Helpers.Jwt
{
    public class JsonWebToken
    {
        private static readonly IDictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms;
        public static IJsonSerializer JsonSerializer = new DefaultJsonSerializer();
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        static JsonWebToken()
        {
            HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>>
            {
                {JwtHashAlgorithm.HS256,(key,value)=>{using(var sha = new HMACSHA256(key)){ return sha.ComputeHash(value); } } },
                {JwtHashAlgorithm.HS384,(key,value)=>{using(var sha = new HMACSHA384(key)){ return sha.ComputeHash(value); } } },
                {JwtHashAlgorithm.HS512,(key,value)=>{using(var sha = new HMACSHA512(key)){ return sha.ComputeHash(value); } } }
            };
        }

        public static string Encode(object payload,string key,JwtHashAlgorithm algorithm)
        {
            return Encode(new Dictionary<string, object>(), payload, Encoding.UTF8.GetBytes(key), algorithm);
        }

        public static string Encode(IDictionary<string,object> extraHeaders,object payload,string key,JwtHashAlgorithm algorithm)
        {
            return Encode(extraHeaders, payload, Encoding.UTF8.GetBytes(key), algorithm);
        }

        public static string Encode(IDictionary<string, object> extraHeaders, object payload, byte[] key, JwtHashAlgorithm algorithm)
        {
            var segments = new List<string>();
            var header = new Dictionary<string, object>(extraHeaders)
            {
                {"type","JWT" },
                {"alg",algorithm.ToString() }
            };
            var headerBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(header));
            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Encrypt(JsonSerializer.Serialize(payload)));

            string stringToSign = string.Join(".", segments.ToArray());
            var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);
            var signature = HashAlgorithms[algorithm](key, bytesToSign);
            segments.Add(Base64UrlEncode(signature));
            return string.Join(".", segments.ToArray());
        }

        public static string Decode(string token,string key,bool verify = true)
        {
            return Decode(token, Encoding.UTF8.GetBytes(key), verify);
        }

        public static string Decode(string token,byte[] key,bool verify = true)
        {
            try
            {
                var parts = token.Split('.');
                if (parts.Length != 3)
                {
                    throw new ArgumentException("Token must consist from 3 delimited by dot parts");
                }
                var payload = parts[1];
                var payloadJson = Decrypt(payload);
                if(verify)
                {
                    Verify(payload, payloadJson, parts, key);
                }
                return payloadJson;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object DecodeToObject(string token,string key,bool verify = true)
        {
            return DecodeToObject(token, Encoding.UTF8.GetBytes(key), verify);
        }
        public static object DecodeToObject(string token,byte[] key,bool verify = true)
        {
            var payloadJson = Decode(token, key, verify);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
        }
        public static T DecodeToObject<T>(string token,string key,bool verify = true)
        {
            return DecodeToObject<T>(token, Encoding.UTF8.GetBytes(key), verify);
        }
        public static T DecodeToObject<T>(string token, byte[] key, bool verify = true)
        {
            var payloadJson = Decode(token, key, verify);
            return JsonSerializer.Deserialize<T>(payloadJson);
        }

        public static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0];
            output = output.Replace('+', '-');
            output = output.Replace('/', '_');
            return output;
        }

        public static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+');
            output = output.Replace('_','/');
            switch (output.Length % 4)
            {
                case 0: break;
                case 2:  output += "==";break;
                case 3: output += "=";break;
                default:throw new FormatException("Illegal base64url string!");
            }
            return Convert.FromBase64String(output);
        }

        public static void Verify(string payloadJson,string decodedCrypto,string decodedSignature)
        {
            if(decodedCrypto != decodedSignature)
            {
                throw new SignatureVerificationException(string.Format("Invalid signature. Expected {0} got {1}",decodedCrypto,decodedSignature));
            }
            var payloadData = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
            object expObj;
            if(!payloadData.TryGetValue("exp", out expObj) || expObj == null)
            {
                return;
            }
            int expInt;
            try
            {
                expInt = Convert.ToInt32(expObj);
            }
            catch (FormatException)
            {

                throw new SignatureVerificationException("Claim 'exp' must be an integer.");
            }
            var secondsSinceEpoch = Math.Round((DateTime.UtcNow-UnixEpoch).TotalSeconds);
            if(secondsSinceEpoch >= expInt)
            {
                throw new TokenExpiredException("Token has Expired");
            }
        }

        public static DateTime GetTokenExpireDate(string token)
        {
            try
            {
                var parts = token.Split('.');
                if(parts.Length != 3)
                {
                    throw new ArgumentException("Token must consist from 3 delimited by dot parts");
                }
                int expInt = 0;
                var payload = parts[1];
                var payloadJson = Decrypt(payload);

                var payloadData = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
                object expObj;
                if(payloadData.TryGetValue("exp",out expObj) && expObj != null)
                {
                    try
                    {
                        expInt = Convert.ToInt32(expObj);
                    }
                    catch
                    {
                        return DateTime.Now.AddDays(3);
                    }
                }
                return UnixEpoch.AddSeconds(expInt).ToLocalTime();
            }
            catch
            {
                return DateTime.Now.AddDays(3);
            }
        }

        private static void Verify(string payload, string payloadJson, string[] parts, byte[] key)
        {
            var crypto = Base64UrlDecode(parts[2]);
            var decodedCrypto = Convert.ToBase64String(crypto);

            var header = parts[0];
            var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            var headerData = JsonSerializer.Deserialize<Dictionary<string, object>>(headerJson);
            var algorithm = (string)headerData["alg"];
            var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
            var signatureData = HashAlgorithms[GetHashAlgorithm(algorithm)](key, bytesToSign);
            var decodedSignature = Convert.ToBase64String(signatureData);
            Verify(payloadJson, decodedCrypto, decodedSignature);
        }

        private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "HS256":return JwtHashAlgorithm.HS256;
                case "HS384":return JwtHashAlgorithm.HS384;
                case "HS512":return JwtHashAlgorithm.HS512;
                default:throw new SignatureVerificationException("Algorithm not supported");
            }
        }

        private static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Padding = PaddingMode.PKCS7;
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncriptionKey, Encoding.UTF8.GetBytes(SaltBase));
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
        private static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ","+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Padding = PaddingMode.PKCS7;
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncriptionKey, Encoding.UTF8.GetBytes(SaltBase));
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

        private static string EncriptionKey = "AyKflkaSune342aHb2Q14la";
        private static string SaltBase = "Yha4kD2Gx3";
    }
}
