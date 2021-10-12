using Pospos.Core.Caching.Redis;
using Pospos.Core.Settings;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Pospos.Core.Modules
{
    public class CacheManager
    {
        private readonly AppSettings _settings;
        private readonly RedisCache _redis;
        public CacheManager(AppSettings settings, RedisCache redis)
        {
            _settings = settings;
            _redis = redis;
        }

        public async Task<T> GetSetAsync<T>(MethodBase method, Func<T> action, string additionalCacheKey = null, int timeoutMinute = 10)
        {
            if (!_settings.EnableCache) return action();
            try
            {
                var cacheKey = $"{_settings.CacheStartKey}:{method.Name}" + (!string.IsNullOrWhiteSpace(additionalCacheKey) ? ":" + additionalCacheKey : "");
                var cached = await _redis.GetAsync<T>(cacheKey);
                if (cached == null)
                {
                    var resp = action();
                    await _redis.AddAsync(cacheKey, resp, timeoutMinute);
                    return resp;
                }
                else return cached;
            }
            catch (Exception ex)
            {
                return action();
            }
        }

        public async Task<T> GetSetAsync<T>(string key, Func<T> action, string additionalCacheKey = null, int timeoutMinute = 10)
        {
            if (!_settings.EnableCache) return action();
            try
            {
                var cacheKey = $"{_settings.CacheStartKey}:{key}" + (!string.IsNullOrWhiteSpace(additionalCacheKey) ? ":" + additionalCacheKey : "");
                var cached = await _redis.GetAsync<T>(cacheKey);
                if (cached == null)
                {
                    var resp = action();
                    await _redis.AddAsync(cacheKey, resp, timeoutMinute);
                    return resp;
                }
                else return cached;
            }
            catch (Exception ex)
            {
                return action();
            }
        }

        public async Task<T> GetAsync<T>(string key, string additionalCacheKey = null)
        {
            if (!_settings.EnableCache) return default(T);
            try
            {
                var cacheKey = $"{_settings.CacheStartKey}:{key}" + (!string.IsNullOrWhiteSpace(additionalCacheKey) ? ":" + additionalCacheKey : "");
                var cached = await _redis.GetAsync<T>(cacheKey);
                if (cached == null)
                {
                    return default(T);
                }
                else return cached;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public async Task SetAsync<T>(string key, T data, string additionalCacheKey = "", int timeoutMinute = 10)
        {
            if (_settings.EnableCache)
            {
                try
                {
                    var cacheKey = $"{_settings.CacheStartKey}:{key}" + (!string.IsNullOrWhiteSpace(additionalCacheKey) ? ":" + additionalCacheKey : "");
                    await _redis.AddAsync<T>(cacheKey, data, timeoutMinute);
                }
                catch
                {
                }
            }
        }

        public async Task ClearAsync(MethodBase method, string additionalCacheKey = "")
        {
            if (_settings.EnableCache)
            {
                var cacheKey = $"{_settings.CacheStartKey}:{method.Name}" + (!string.IsNullOrWhiteSpace(additionalCacheKey) ? ":" + additionalCacheKey : "");
                await _redis.RemoveAsync(cacheKey);
            }
        }

        public async Task ClearAsync(string key = "")
        {
            if (_settings.EnableCache)
            {
                var cacheKey = $"{_settings.CacheStartKey}:{key}";
                await _redis.RemoveAsync(cacheKey);
            }
        }
    }
}
