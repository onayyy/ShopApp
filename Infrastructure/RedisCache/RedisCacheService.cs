using Application.Common.Interfaces.RedisCache;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RedisCache
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _database;
        private readonly RedisCacheSettings _settings;

        public RedisCacheService(IOptions<RedisCacheSettings> options)
        {
            _settings = options.Value;
            var opt = ConfigurationOptions.Parse(_settings.ConnectionString);
            _redisConnection = ConnectionMultiplexer.Connect(opt);
            _database = _redisConnection.GetDatabase();
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.HasValue)
                return JsonConvert.DeserializeObject<T>(value);

            return default;
        }

        public async Task SetAsync<T>(string key, T value, DateTime? expiry = null)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            TimeSpan timeUnitExpiration = expiry.Value - DateTime.Now;
            var jsonData = JsonConvert.SerializeObject(value, settings);
            await _database.StringSetAsync(key, jsonData, timeUnitExpiration);
        }
    }
}
