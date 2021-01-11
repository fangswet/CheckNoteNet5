using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services
{
    public class CacheService
    {
        private readonly IMemoryCache cache;
        private readonly MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

        public CacheService(IMemoryCache cache)
        {
            this.cache = cache;
            options.SlidingExpiration = TimeSpan.FromSeconds(5);
        }

        public delegate Task<T> CacheAction<T>(ICacheEntry factory);
        public async Task<T> Get<T>(string key, CacheAction<T> action) => await cache.GetOrCreateAsync(key, factory => action(factory));
    }
}
