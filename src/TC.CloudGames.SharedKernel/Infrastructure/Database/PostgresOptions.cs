namespace TC.CloudGames.SharedKernel.Infrastructure.Database
{
    public sealed class PostgresOptions
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5432;
        public string Database { get; set; } = "tc-cloudgames-users-db";
        public string MaintenanceDatabase { get; set; } = "postgres";
        public string UserName { get; set; } = "postgres";
        public string Password { get; set; } = "postgres";
        public string Schema { get; set; } = "public";
        public int ConnectionTimeout { get; set; } = 30; // seconds

        // Computed property → normal connection string
        public string ConnectionString =>
            $"Host={Host};Port={Port};Database={Database};Username={UserName};Password={Password};SearchPath={Schema};Timeout={ConnectionTimeout};CommandTimeout={ConnectionTimeout}";

        // Computed property → maintenance connection string
        public string MaintenanceConnectionString =>
            $"Host={Host};Port={Port};Database={MaintenanceDatabase};Username={UserName};Password={Password};Timeout={ConnectionTimeout};CommandTimeout={ConnectionTimeout}";
    }

}
