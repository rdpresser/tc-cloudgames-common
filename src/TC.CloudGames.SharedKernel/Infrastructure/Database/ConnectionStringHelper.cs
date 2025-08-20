using Microsoft.Extensions.Configuration;

namespace TC.CloudGames.SharedKernel.Infrastructure.Database
{
    public sealed class ConnectionStringHelper
    {
        public string ConnectionString { get; }

        public ConnectionStringHelper(IConfiguration configuration)
        {
            // Monta a string direto do IConfiguration / Environment variables
            var host = Environment.GetEnvironmentVariable("DB_HOST")
                ?? configuration["Database:Host"];

            var database = Environment.GetEnvironmentVariable("DB_OUTBOX_NAME")
                    ?? configuration["Database:OutboxDatabase"];

            var username = Environment.GetEnvironmentVariable("DB_USER")
                ?? configuration["Database:User"];

            var password = Environment.GetEnvironmentVariable("DB_PASS")
                ?? configuration["Database:Password"];

            var port = Environment.GetEnvironmentVariable("DB_PORT")
                ?? configuration["Database:Port"];

            ConnectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
        }

        // Opcional: método estático para gerar string sem instanciar o provider
        public static string BuildConnectionString(IConfiguration configuration)
        {
            var provider = new ConnectionStringHelper(configuration);
            return provider.ConnectionString;
        }
    }
}
