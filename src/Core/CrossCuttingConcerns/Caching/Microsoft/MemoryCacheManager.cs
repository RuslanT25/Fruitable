using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    internal class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            return await Task.FromResult(_cache.Get<T>(key));
        }

        public async Task<object> GetAsync(string key)
        {
            return await Task.FromResult(_cache.Get(key));
        }

        public async Task AddAsync(string key, object data, int duration)
        {
            await Task.Run(() => _cache.Set(key, data, TimeSpan.FromMinutes(duration)));
        }

        public async Task<bool> IsAddAsync(string key)
        {
            return await Task.FromResult(_cache.TryGetValue(key, out _));
        }

        public async Task RemoveAsync(string key)
        {
            await Task.Run(() => _cache.Remove(key));
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_cache) as dynamic;
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }
    }
}
