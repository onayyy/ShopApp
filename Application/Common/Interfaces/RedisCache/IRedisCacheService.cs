using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.RedisCache
{
    public interface IRedisCacheService
    {
        Task SetAsync<T>(string key, T value, DateTime? expiry);
        Task<T?> GetAsync<T>(string key);
        Task<bool> DeleteAsync(string key);
    }
}
