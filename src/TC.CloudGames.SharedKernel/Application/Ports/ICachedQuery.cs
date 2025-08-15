using TC.CloudGames.SharedKernel.Application.Queries;

namespace TC.CloudGames.SharedKernel.Application.Ports
{
    public interface ICachedQuery<TResponse> : IBaseQuery<TResponse>, ICachedQuery;

    public interface ICachedQuery
    {
        string CacheKey { get; }

        TimeSpan? Duration { get; }
        TimeSpan? DistributedCacheDuration { get; }
    }
}
