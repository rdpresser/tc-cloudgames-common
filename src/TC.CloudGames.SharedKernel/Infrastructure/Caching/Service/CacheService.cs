namespace TC.CloudGames.SharedKernel.Infrastructure.Caching.Service
{
    public sealed class CacheService : ICacheService
    {
        private readonly IFusionCache _fusionCache;
        public CacheService(IFusionCache fusionCache)
        {
            _fusionCache = fusionCache ?? throw new ArgumentNullException(nameof(fusionCache));
        }

        public async Task<T?> GetAsync<T>(string key,
            TimeSpan? duration = null,
            TimeSpan? distributedCacheDuration = null,
            CancellationToken cancellationToken = default)
        {
            return await _fusionCache.GetOrDefaultAsync(
                key,
                default(T),
                CacheServiceOptions.Create(duration, distributedCacheDuration),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? duration = null,
            TimeSpan? distributedCacheDuration = null,
            CancellationToken cancellationToken = default)
        {
            await _fusionCache.SetAsync(
                key,
                value,
                CacheServiceOptions.Create(duration, distributedCacheDuration),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveAsync(
            string key,
            TimeSpan? duration = null,
            TimeSpan? distributedCacheDuration = null,
            CancellationToken cancellationToken = default)
        {
            await _fusionCache.RemoveAsync(
                key,
                CacheServiceOptions.Create(duration, distributedCacheDuration),
                cancellationToken).ConfigureAwait(false);
        }
    }
}
