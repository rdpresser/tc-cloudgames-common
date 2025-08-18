using TC.CloudGames.SharedKernel.Infrastructure.UserClaims;

namespace TC.CloudGames.SharedKernel.Application.Behaviors
{
    [ExcludeFromCodeCoverage]
    public sealed class QueryCachingPreProcessorBehavior<TQuery, TResponse> : IPreProcessor<TQuery>
        where TQuery : ICachedQuery<TResponse>
        where TResponse : class
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<QueryCachingPreProcessorBehavior<TQuery, TResponse>> _logger;
        private const string correlationIdHeader = "x-cache-correlation-id";

        public QueryCachingPreProcessorBehavior(ICacheService cacheService,
            ILogger<QueryCachingPreProcessorBehavior<TQuery, TResponse>> logger)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PreProcessAsync(IPreProcessorContext<TQuery> context, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(context);

            var genericType = _logger.GetType().GenericTypeArguments.FirstOrDefault()?.Name ?? "Unknown";
            var name = context.Request?.GetType().Name
                       ?? genericType;

            if (!context.HasValidationFailures)
            {
                var cachedResult = await _cacheService.GetAsync<TResponse>(
                GenerateCacheKey(context),
                context.Request!.Duration,
                context.Request.DistributedCacheDuration,
                ct).ConfigureAwait(false);

                if (cachedResult is not null)
                {
                    using (LogContext.PushProperty("CacheContent", cachedResult, true))
                    {
                        _logger.LogInformation("Pre-processing returning cached result for request: {Request}, executed successfully", name);
                    }

                    context.HttpContext.Request.Headers[correlationIdHeader] = GenerateCacheKey(context);
                    await context.HttpContext.Response.SendOkAsync(cachedResult, cancellation: ct);

                    // short-circuit: não executa o handler
                    return;
                }

                context.HttpContext.Request.Headers.Remove(correlationIdHeader);
                using (LogContext.PushProperty("RequestContent", context.Request, true))
                {
                    _logger.LogInformation("Pre-processing returning database result for request: {Request}, executed successfully", name);
                }
            }
            else
            {
                var responseValues = new
                {
                    context.Request,
                    Error = context.ValidationFailures
                };

                using (LogContext.PushProperty("RequestContent", responseValues, true))
                {
                    _logger.LogError("Pre-processing Request {Request} validation failed with error", name);
                }
            }

            return;
        }

        private static string GenerateCacheKey(IPreProcessorContext<TQuery> context)
        {
            var _userContext = context.HttpContext.RequestServices.GetRequiredService<IUserContext>();
            context.Request!.SetCacheKey($"-{_userContext.Id}-{_userContext.Username}");

            return context.Request.GetCacheKey;
        }
    }
}
