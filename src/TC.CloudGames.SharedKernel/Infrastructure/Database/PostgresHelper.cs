using Microsoft.Extensions.Configuration;

namespace TC.CloudGames.SharedKernel.Infrastructure.Database
{
    public sealed class PostgresHelper
    {
        private const string PostgresSectionName = "Database";
        public PostgresOptions PostgresSettings { get; }

        public PostgresHelper(IConfiguration configuration)
        {
            // Bind section Database → PostgresOptions
            PostgresSettings = configuration.GetSection(PostgresSectionName).Get<PostgresOptions>()
                               ?? new PostgresOptions();

            // Override values if environment variables exist
            PostgresSettings.Host = Environment.GetEnvironmentVariable("DB_HOST")
                                    ?? PostgresSettings.Host;

            PostgresSettings.Port = int.TryParse(Environment.GetEnvironmentVariable("DB_PORT"), out var port)
                                    ? port
                                    : PostgresSettings.Port;

            PostgresSettings.Database = Environment.GetEnvironmentVariable("DB_NAME")
                                    ?? PostgresSettings.Database;

            PostgresSettings.MaintenanceDatabase = Environment.GetEnvironmentVariable("DB_MAINTENANCE_NAME")
                                    ?? PostgresSettings.MaintenanceDatabase;

            PostgresSettings.UserName = Environment.GetEnvironmentVariable("DB_USER")
                                    ?? PostgresSettings.UserName;

            PostgresSettings.Password = Environment.GetEnvironmentVariable("DB_PASSWORD")
                                    ?? PostgresSettings.Password;

            PostgresSettings.Schema = Environment.GetEnvironmentVariable("DB_SCHEMA")
                                    ?? PostgresSettings.Schema;
        }

        // Static convenience method
        public static PostgresOptions Build(IConfiguration configuration) =>
            new PostgresHelper(configuration).PostgresSettings;
    }
}
