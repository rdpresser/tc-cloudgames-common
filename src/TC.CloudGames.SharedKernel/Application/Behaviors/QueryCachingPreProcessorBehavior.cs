namespace TC.CloudGames.SharedKernel.Application.Behaviors
{
    [ExcludeFromCodeCoverage]
    public sealed class QueryCachingPreProcessorBehavior<TQuery, TResponse> : IPreProcessor<TQuery>
        where TQuery : ICachedQuery<TResponse>
        where TResponse : class
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<QueryCachingPreProcessorBehavior<TQuery, TResponse>> _logger;

        public QueryCachingPreProcessorBehavior(ICacheService cacheService, ILogger<QueryCachingPreProcessorBehavior<TQuery, TResponse>> logger)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PreProcessAsync(IPreProcessorContext<TQuery> context, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(context);
            var name = context.Request!.GetType().Name;


            // Buscar usuário logado com interface IUserContext
            // setar cachekey com o id do usuario
            // context.Request.CacheKey = $"{_userContext.UserId}-{context.Request.CacheKey}"


            var cachedResult = await _cacheService.GetAsync<TResponse>(
                context.Request.CacheKey,
                context.Request.Duration,
                context.Request.DistributedCacheDuration,
                ct);

            if (cachedResult is not null)
            {
                _logger.LogInformation("Returning cached result for request: {Request}", name);

                // retorna direto com HTTP 200 usando padrão do FastEndpoints
                await context.HttpContext.Response.SendOkAsync(cachedResult, cancellation: ct);

                // short-circuit: não executa o handler
                return;
            }

            using (LogContext.PushProperty("RequestContent", context.Request, true))
            {
                _logger.LogInformation("Pre-processing request: {Request}", name);
            }

            if (context.HasValidationFailures)
            {
                using (LogContext.PushProperty("Error", context.ValidationFailures, true))
                {
                    _logger.LogError("Pre-processing Request {Request} validation failed with error", name);
                }
                return;
            }

            _logger.LogInformation("Pre-processing Request {Request} executed successfully", name);

            return;
        }
    }
}
