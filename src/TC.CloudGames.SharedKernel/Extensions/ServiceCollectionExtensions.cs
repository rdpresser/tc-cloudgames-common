using Microsoft.Extensions.DependencyInjection;
using TC.CloudGames.SharedKernel.Infrastructure.Middleware;

namespace TC.CloudGames.SharedKernel.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorrelationIdGenerator(this IServiceCollection services)
        {
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();

            return services;
        }
    }
}
