namespace TC.CloudGames.SharedKernel.Infrastructure.Database
{
    public sealed class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly PostgresOptions _dbSettings;

        public ConnectionStringProvider(IOptions<PostgresOptions> dbSettings)
        {
            _dbSettings = dbSettings.Value;
        }

        public string ConnectionString => BuildConnectionString(
            envDatabaseName: "DB_NAME",
            fallbackDatabase: _dbSettings.Database
        );

        public string MaintenanceConnectionString => BuildConnectionString(
            envDatabaseName: "DB_MAINTENANCE_NAME",
            fallbackDatabase: _dbSettings.MaintenanceDatabase
        );

        private string BuildConnectionString(string envDatabaseName, string fallbackDatabase)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? _dbSettings.Host;

            var port = int.TryParse(Environment.GetEnvironmentVariable("DB_PORT"), out var p)
                ? p
                : _dbSettings.Port;

            var database = Environment.GetEnvironmentVariable(envDatabaseName) ?? fallbackDatabase;

            var username = Environment.GetEnvironmentVariable("DB_USER") ?? _dbSettings.UserName;

            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? _dbSettings.Password;

            var connectionTimeout = _dbSettings.ConnectionTimeout;

            return $"Host={host};Port={port};Database={database};Username={username};Password={password};Timeout={connectionTimeout};CommandTimeout={connectionTimeout}";
        }
    }
}
