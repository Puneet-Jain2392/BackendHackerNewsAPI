namespace HackerAPI.Caching
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(string cacheKey, Func<Task<T>> createItem);
    }
}
