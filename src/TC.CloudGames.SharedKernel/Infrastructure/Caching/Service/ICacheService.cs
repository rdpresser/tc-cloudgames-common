namespace TC.CloudGames.SharedKernel.Infrastructure.Caching.Service
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key,
            TimeSpan? duration = null,
            TimeSpan? distributedCacheDuration = null,
            CancellationToken cancellationToken = default);

        Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? duration = null,
            TimeSpan? distributedCacheDuration = null,
            CancellationToken cancellationToken = default);

        Task RemoveAsync(
            string key,
            TimeSpan? duration = null,
            TimeSpan? distributedCacheDuration = null,
            CancellationToken cancellationToken = default);
    }
}
