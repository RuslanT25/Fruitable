using Microsoft.Extensions.Options;
using StackExchange.Redis;

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

        public async Task AddAsync<T>(string key, T data, TimeSpan duration)
        {
            var serializedData = System.Text.Json.JsonSerializer.Serialize(data);
            await _database.StringSetAsync(key, serializedData, duration);
        }

        public bool IsAdd(string key)
        {
            return _database.KeyExists(key);
        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
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
