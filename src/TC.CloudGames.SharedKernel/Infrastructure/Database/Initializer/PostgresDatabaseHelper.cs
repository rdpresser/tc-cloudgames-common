using Npgsql;

namespace TC.CloudGames.SharedKernel.Infrastructure.Database.Initializer
{
    public static class PostgresDatabaseHelper
    {
        public async static Task EnsureDatabaseExists(IConnectionStringProvider connectionStringProvider)
        {
            var maintenanceConnStr = connectionStringProvider.MaintenanceConnectionString;
            var targetConnStr = connectionStringProvider.ConnectionString;

            // Parse database name and user from connection string
            var builder = new NpgsqlConnectionStringBuilder(targetConnStr);
            var databaseName = builder.Database;
            var user = builder.Username;

            using var conn = new NpgsqlConnection(maintenanceConnStr);
            await conn.OpenAsync().ConfigureAwait(false);

            // Check if database exists
            if (databaseName == null)
                throw new InvalidOperationException("Database name could not be determined from connection string.");

            using var cmd = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname = @dbname", conn);
            cmd.Parameters.AddWithValue("dbname", databaseName);
            var exists = await cmd.ExecuteScalarAsync().ConfigureAwait(false) != null;

            if (!exists)
            {
                using var createCmd = new NpgsqlCommand($"CREATE DATABASE \"{databaseName}\" OWNER \"{user}\" ENCODING 'UTF8';", conn);
                await createCmd.ExecuteNonQueryAsync();
            }
        }
    }
}
