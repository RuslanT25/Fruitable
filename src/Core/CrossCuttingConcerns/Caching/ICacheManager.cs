using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    internal interface ICacheManager
    {
        Task<T> GetAsync<T>(string key);
        Task<object> GetAsync(string key);
        Task AddAsync(string key, object data, int duration);
        Task<bool> IsAddAsync(string key);
        Task RemoveAsync(string key);
        Task RemoveByPatternAsync(string pattern);
    }
}
