namespace TC.CloudGames.SharedKernel.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorrelationIdGenerator(this IServiceCollection services)
        {
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();

            return services;
        }

        /// <summary>
        /// Configures environment variables from .env files if not running under .NET Aspire orchestration
        /// </summary>
        /// <param name="builder">The web application builder</param>
        /// <returns>The web application builder for chaining</returns>
        public static WebApplicationBuilder ConfigureEnvironmentVariables(this WebApplicationBuilder builder)
        {
            // Check if running under .NET Aspire orchestration
            if (IsRunningUnderAspire())
            {
                var logger = CreateBootstrapLogger();
                logger?.LogInformation("Running under .NET Aspire orchestration - skipping .env file loading");
                return builder;
            }

            LoadEnvironmentFiles(builder.Environment);
            return builder;
        }

        /// <summary>
        /// Detects if the application is running under .NET Aspire orchestration
        /// </summary>
        /// <returns>True if running under Aspire, false otherwise</returns>
        private static bool IsRunningUnderAspire()
        {
            // Check for Aspire-specific environment variables
            var aspireEnvVars = new[]
            {
                "ASPIRE_HOST",
                "ASPIRE_RESOURCE_NAME",
                "DOTNET_DASHBOARD_OTLP_ENDPOINT_URL",
                "OTEL_EXPORTER_OTLP_ENDPOINT",
                "OTEL_SERVICE_NAME"
            };

            if (aspireEnvVars.Any(envVar => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(envVar))))
            {
                return true;
            }

            // Additional check for Aspire project patterns
            var serviceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME");
            if (!string.IsNullOrEmpty(serviceName) && serviceName.Contains("aspire", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads environment variables from .env files
        /// </summary>
        /// <param name="environment">The hosting environment</param>
        private static void LoadEnvironmentFiles(IHostEnvironment environment)
        {
            var environmentName = environment.EnvironmentName.ToLowerInvariant();

            // Find project root by looking for solution file or git directory
            var projectRoot = FindProjectRoot() ?? Directory.GetCurrentDirectory();

            var logger = CreateBootstrapLogger();

            // Load base .env file first (if exists)
            var baseEnvFile = Path.Combine(projectRoot, ".env");
            if (File.Exists(baseEnvFile))
            {
                DotNetEnv.Env.Load(baseEnvFile);
                logger?.LogInformation("Loaded base .env from: {EnvFile}", baseEnvFile);
                Console.WriteLine($"Loaded base .env from: {baseEnvFile}");
            }

            // Load environment-specific .env file (overrides base values)
            var envFile = Path.Combine(projectRoot, $".env.{environmentName}");
            if (File.Exists(envFile))
            {
                DotNetEnv.Env.Load(envFile);
                logger?.LogInformation("Loaded {Environment} .env from: {EnvFile}", environmentName, envFile);
                Console.WriteLine($"Loaded {environmentName} .env from: {envFile}");
            }
            else
            {
                logger?.LogWarning("Environment file not found: {EnvFile}", envFile);
                Console.WriteLine($"Environment file not found: {envFile}");
            }
        }

        /// <summary>
        /// Finds the project root directory by looking for solution file or git directory
        /// </summary>
        /// <returns>The project root directory path or null if not found</returns>
        private static string? FindProjectRoot()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (directory != null)
            {
                // Look for common project root indicators
                if (directory.GetFiles("*.sln").Length > 0 ||
                    directory.GetDirectories(".git").Length > 0 ||
                    HasEnvFiles(directory))
                {
                    return directory.FullName;
                }
                directory = directory.Parent;
            }

            return null;
        }

        /// <summary>
        /// Checks if a directory contains .env files
        /// </summary>
        /// <param name="directory">The directory to check</param>
        /// <returns>True if .env files are found, false otherwise</returns>
        private static bool HasEnvFiles(DirectoryInfo directory)
        {
            return directory.GetFiles(".env").Length > 0 ||
                   directory.GetFiles(".env.*").Length > 0;
        }

        /// <summary>
        /// Creates a bootstrap logger for early logging before DI container is built
        /// </summary>
        /// <returns>A logger instance or null if creation fails</returns>
        private static ILogger? CreateBootstrapLogger()
        {
            try
            {
                using var loggerFactory = LoggerFactory.Create(builder =>
                    builder.AddConsole().SetMinimumLevel(LogLevel.Information));
                return loggerFactory.CreateLogger("EnvironmentConfiguration");
            }
            catch
            {
                // If logger creation fails, return null and fall back to Console.WriteLine
                return null;
            }
        }
    }
}
