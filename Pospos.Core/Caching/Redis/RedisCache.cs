using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pospos.Core.Caching.Redis
{
    public class RedisCache
    {
        private readonly RedisContext _redisContext;
        public RedisCache(RedisContext redisContext)
        {
            _redisContext = redisContext;
        }

        public async Task RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key is null");

            IDatabase db = _redisContext.Connection.GetDatabase();
            await db.KeyDeleteAsync(key, CommandFlags.PreferMaster);
        }

        public async Task AddAsync<T>(string key,T obj,int timeOutMinute = 60)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key is null");

            if (obj == null) throw new ArgumentNullException("obj is null");
            IDatabase db = _redisContext.Connection.GetDatabase();

            var ts = TimeSpan.FromMinutes(timeOutMinute);
            await db.StringSetAsync(key, SerializeAsJson<T>(obj), ts, When.Always, CommandFlags.PreferMaster);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key is null");

            IDatabase db = _redisContext.Connection.GetDatabase();

            var data = await db.StringGetAsync(key,CommandFlags.PreferReplica);
            return DeSerializeAsJson<T>(data);
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key is null");
            IDatabase db = _redisContext.Connection.GetDatabase();
            return await db.KeyExistsAsync(key, CommandFlags.PreferMaster);
        }

        public List<string> GetKeys(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException("pattern is null");
            var endpoints = _redisContext.Connection.GetEndPoints();
            var master = (IPEndPoint)endpoints.First();
            var server = _redisContext.Connection.GetServer(master.Address, master.Port);
            return server.Keys(0, pattern).Select(x => System.Text.Encoding.UTF8.GetString(x)).ToList();
        }




        private string SerializeAsJson<T>(T obj)
        {
            return System.Text.Json.JsonSerializer.Serialize<T>(obj);
        }

        private T DeSerializeAsJson<T>(string serialized)
        {
            if (string.IsNullOrWhiteSpace(serialized)) return default(T);

            return System.Text.Json.JsonSerializer.Deserialize<T>(serialized);
        }
    }
}
