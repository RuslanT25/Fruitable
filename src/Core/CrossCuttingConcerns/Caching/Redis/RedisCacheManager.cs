using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly IDatabase _database;

        public RedisCacheManager(IOptions<RedisOptions> options)
        {
            var connection = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
            _database = connection.GetDatabase();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            return value.HasValue ? System.Text.Json.JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task<object> GetAsync(string key)
        {
            var value = await _database.StringGetAsync(key);
            return value.HasValue ? System.Text.Json.JsonSerializer.Deserialize<object>(value) : null;
        }

        public async Task AddAsync(string key, object data, int duration)
        {
            var serializedData = System.Text.Json.JsonSerializer.Serialize(data);
            var expiration = TimeSpan.FromMinutes(duration);
            await _database.StringSetAsync(key, serializedData, expiration);
        }

        public async Task<bool> IsAddAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            var endpoints = _database.Multiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _database.Multiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: $"*{pattern}*");
                foreach (var key in keys)
                {
                    await _database.KeyDeleteAsync(key);
                }
            }
        }
    }
}