
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;

namespace Pospos.Core.Helpers
{
    //Session işlemleri...
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.Set(key, JsonSerializer.SerializeToUtf8Bytes(value));
        }

        public static T Get<T>(this ISession session, string key) where T : class
        {
            if (session.TryGetValue(key, out byte[] value))
            {
                return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
            }
            else
                return null;
        }
        public static void SetInt32(this ISession session, string key, int value)
        {
            session.Set(key, JsonSerializer.SerializeToUtf8Bytes(value));
        }

        public static int GetInt32(this ISession session, string key)
        {
            if (session.TryGetValue(key, out byte[] value))
            {
                return value == null ? -1 :
                JsonSerializer.Deserialize<int>(value);
            }
            else
                return -1;
        }
        public static void SetDateTime(this ISession session, string key, DateTime value)
        {
            session.Set(key, JsonSerializer.SerializeToUtf8Bytes(value));
        }

        public static DateTime GetDateTime(this ISession session, string key)
        {
            if (session.TryGetValue(key, out byte[] value))
            {
                return value == null ? DateTime.MinValue :
                JsonSerializer.Deserialize<DateTime>(value);
            }
            else
                return DateTime.MinValue;
        }

        public static bool SessionClear(this ISession session, string key)
        {
            try
            {
                session.Remove(key);
                session.Clear();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
