namespace TC.CloudGames.SharedKernel.Infrastructure.Caching.Service
{
    /// <summary>
    ///     Provides default cache options for FusionCache entries.
    /// </summary>
    public static class CacheServiceOptions
    {
        /// <summary>
        ///     Gets the default expiration settings for cache entries.
        /// </summary>
        /// <remarks>
        ///     - <see cref="FusionCacheEntryOptions.Duration" /> specifies the in-memory cache duration (20 seconds).
        ///     - <see cref="FusionCacheEntryOptions.DistributedCacheDuration" /> specifies the distributed cache duration (30
        ///     seconds).
        /// </remarks>
        public static FusionCacheEntryOptions DefaultExpiration => new()
        {
            Duration = TimeSpan.FromSeconds(20),
            DistributedCacheDuration = TimeSpan.FromSeconds(30)
        };

        public static FusionCacheEntryOptions Create(TimeSpan? duration = null, TimeSpan? distributedCacheDuration = null)
        {
            return new FusionCacheEntryOptions
            {
                Duration = duration ?? DefaultExpiration.Duration,
                DistributedCacheDuration = distributedCacheDuration ?? DefaultExpiration.DistributedCacheDuration
            };
        }
    }
}
