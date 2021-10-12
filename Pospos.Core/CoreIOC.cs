using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pospos.Core.Caching.Redis;
using Pospos.Core.Helpers;
using Pospos.Core.Modules;

namespace Pospos.Core
{
    public static class CoreIOC
    {
        public static void AddCore(this IServiceCollection services)
        {
            services.AddSingleton<ILogHelper, LogHelper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<MainConnectionManager>();
            services.AddSingleton<Utility>();
            services.AddSingleton<SecurityHelper>();
            services.AddSingleton<RedisContext>();
            services.AddSingleton<RedisCache>();
            services.AddSingleton<CacheManager>();
            services.AddSingleton<LogManager>();
        }
    }
}
