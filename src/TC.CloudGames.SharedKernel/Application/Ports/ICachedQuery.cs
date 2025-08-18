namespace TC.CloudGames.SharedKernel.Application.Ports
{
    public interface ICachedQuery<TResponse> : IBaseQuery<TResponse>, ICachedQuery;

    public interface ICachedQuery
    {
        string GetCacheKey { get; }
        void SetCacheKey(string cacheKey);

        TimeSpan? Duration { get; }
        TimeSpan? DistributedCacheDuration { get; }
    }
}
