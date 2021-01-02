using CheckNoteNet5.Shared.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CheckNoteNet5.Client.Services
{
    public abstract class ClientService
    {
        protected readonly IMemoryCache memoryCache;

        public ClientService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        protected delegate Task<T> CacheAction<T>(ICacheEntry factory);
        protected async Task<T> FromCache<T>(string key, CacheAction<T> action) => await memoryCache.GetOrCreateAsync(key, factory =>
        {
            factory.SlidingExpiration = TimeSpan.FromSeconds(5);

            return action(factory);
        });
        protected static async Task<ServiceResult<T>> Parse<T>(HttpResponseMessage response) => await ServiceResult<T>.Parse(response);
    }
}
