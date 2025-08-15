namespace TC.CloudGames.SharedKernel.Infrastructure.Caching.HealthCheck;

[ExcludeFromCodeCoverage]
public class RedisHealthCheck : IHealthCheck
{
    private readonly ICacheProvider _cacheProvider;

    public RedisHealthCheck(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = await ConnectionMultiplexer.ConnectAsync(_cacheProvider.ConnectionString);
            var database = connection.GetDatabase();

            // Simple ping test
            var result = await database.PingAsync();

            if (result.TotalMilliseconds > 1000)
            {
                return HealthCheckResult.Degraded($"Redis responded in {result.TotalMilliseconds}ms");
            }

            return HealthCheckResult.Healthy($"Redis is healthy. Response time: {result.TotalMilliseconds}ms");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Redis health check failed: {ex.Message}", ex);
        }
    }
}