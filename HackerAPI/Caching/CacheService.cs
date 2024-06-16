using Microsoft.Extensions.Caching.Memory;

namespace HackerAPI.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            };
        }

        public async Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> createItem)
        {
            if (!_cache.TryGetValue(cacheKey, out T cacheEntry))
            {
                cacheEntry = await createItem();
                _cache.Set(cacheKey, cacheEntry, _cacheOptions);
            }

            return cacheEntry;
        }
    }
}
