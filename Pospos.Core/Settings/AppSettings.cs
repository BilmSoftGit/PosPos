namespace Pospos.Core.Settings
{
    public class AppSettings
    {
        public bool EnableCache { get; set; }
        public int RedisConnectionPoolSize { get; set; }
        public string CacheStartKey { get; set; }
        public int TokenExpireMinute { get; set; }
        public string EncriptionKey { get; set; }
        public string SaltBase { get; set; }
        public int DefaultCacheTimeout { get; set; }
        public int LoginFailedTryCount { get; set; }
        public int UserBlockMinute { get; set; }

        public string ApplicationName { get; set; }
        public string JwtTokenKey { get; set; }
        public string Environment { get; set; }
        public string[] SwaggerAllowedIpAddresses { get; set; }
    }

    public class ConnectionConfig
    {
        public string PaymentApiConnection { get; set; }
        public string ManagerConnection { get; set; }
        public string LogConnection { get; set; }
        public string RedisConnection { get; set; }
    }
}
