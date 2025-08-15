namespace TC.CloudGames.SharedKernel.Application.Behaviors
{
    [ExcludeFromCodeCoverage]
    public class QueryCachingPostProcessorBehavior<TQuery, TResponse> : IPostProcessor<TQuery, TResponse>
        where TQuery : ICachedQuery<TResponse>
        where TResponse : class
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<QueryCachingPostProcessorBehavior<TQuery, TResponse>> _logger;
        private const string correlationIdHeader = "x-cache-correlation-id";

        public QueryCachingPostProcessorBehavior(ICacheService cacheService, ILogger<QueryCachingPostProcessorBehavior<TQuery, TResponse>> logger)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PostProcessAsync(IPostProcessorContext<TQuery, TResponse> context, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(context);


            if (context.HttpContext.Request.Headers.TryGetValue(correlationIdHeader, out var cachedInfo) && cachedInfo == GenerateCacheKey(context))
            {
                context.HttpContext.Request.Headers.Remove(correlationIdHeader);
                return;
            }

            var genericType = _logger.GetType().GenericTypeArguments.FirstOrDefault()?.Name ?? "Unknown";
            var name = context.Request?.GetType().Name
                       ?? genericType;

            /************************************************
            // Buscar usuário logado com interface IUserContext
            // setar cachekey com o id do usuario
            // context.Request.CacheKey = $"{_userContext.UserId}-{context.Request.CacheKey}"
            // dessa forma será possivel pegar o cache especifico do usuário logado
            *///////////////////////////////////////////////////////

            if (!context.HasValidationFailures)
            {
                await _cacheService.SetAsync(
                    GenerateCacheKey(context),
                    context.Response,
                    context.Request!.Duration,
                    context.Request.DistributedCacheDuration,
                    ct);

                var responseValues = new
                {
                    context.Request,
                    context.Response,
                };

                using (LogContext.PushProperty("Content", responseValues, true))
                {
                    _logger.LogInformation("Post-processing Request {Request} executed successfully", name);
                }
            }
            else
            {
                var responseValues = new
                {
                    context.Request,
                    context.Response,
                    Error = context.ValidationFailures
                };

                using (LogContext.PushProperty("Content", responseValues, true))
                {
                    _logger.LogError("Post-processing Request {Request} validation failed with error", name);
                }
            }

            return;
        }

        private static string GenerateCacheKey(IPostProcessorContext<TQuery, TResponse> context)
        {
            //add logged user in the future
            return $"{context.Request!.CacheKey}";
        }
    }
}
