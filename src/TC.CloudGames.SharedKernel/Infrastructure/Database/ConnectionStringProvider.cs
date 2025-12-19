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

            var schema = Environment.GetEnvironmentVariable("DB_SCHEMA") ?? _dbSettings.Schema;

            var connectionTimeout = int.TryParse(Environment.GetEnvironmentVariable("DB_CONNECTION_TIMEOUT"), out var timeout)
                ? timeout
                : _dbSettings.ConnectionTimeout;

            var maxPoolSize = int.TryParse(Environment.GetEnvironmentVariable("DB_MAX_POOL_SIZE"), out var poolSize)
                ? poolSize
                : _dbSettings.MaxPoolSize;

            var minPoolSize = int.TryParse(Environment.GetEnvironmentVariable("DB_MIN_POOL_SIZE"), out var minPool)
                ? minPool
                : _dbSettings.MinPoolSize;

            // Constrain pooling values to avoid negative or zero settings that would break connections
            // Always clamp to safe absolute bounds, regardless of misconfigured app settings
            if (maxPoolSize < 1)
                maxPoolSize = 1;

            if (minPoolSize < 0)
                minPoolSize = 0;

            if (minPoolSize > maxPoolSize)
                minPoolSize = maxPoolSize;

            return $"Host={host};Port={port};Database={database};Username={username};Password={password};SearchPath={schema};Timeout={connectionTimeout};CommandTimeout={connectionTimeout};Pooling=true;Minimum Pool Size={minPoolSize};Maximum Pool Size={maxPoolSize}";
        }
    }
}
