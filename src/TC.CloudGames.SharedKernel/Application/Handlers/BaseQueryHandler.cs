namespace TC.CloudGames.SharedKernel.Application.Handlers
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Base class for query handlers, focused on execution and validation only.
    /// </summary>
    public abstract class BaseQueryHandler<TQuery, TResponse>
        : BaseHandler<TQuery, TResponse>
        where TQuery : IBaseQuery<TResponse>
        where TResponse : class
    {
        /// <summary>
        /// Executes the query asynchronously.
        /// </summary>
        public abstract override Task<Result<TResponse>> ExecuteAsync(TQuery command, CancellationToken ct = default);
    }
}
