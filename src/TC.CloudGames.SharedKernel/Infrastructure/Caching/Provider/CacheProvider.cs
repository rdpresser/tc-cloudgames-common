using System.Text;

namespace TC.CloudGames.SharedKernel.Infrastructure.Caching.Provider
{
    public sealed class CacheProvider : ICacheProvider
    {
        private readonly CacheProviderSettings _cacheSettings;

        public CacheProvider(IOptions<CacheProviderSettings> cacheSettings)
        {
            _cacheSettings = cacheSettings.Value;
        }

        public string InstanceName => _cacheSettings.InstanceName;

        public string ConnectionString
        {
            get
            {
                var host = GetEnvironmentVariableOrDefault("CACHE_HOST", _cacheSettings.Host);
                var port = GetEnvironmentVariableOrDefault("CACHE_PORT", _cacheSettings.Port);
                var password = GetEnvironmentVariableOrDefault("CACHE_PASSWORD", _cacheSettings.Password);
                var secure = GetSecureSettingFromEnvironment();

                return BuildConnectionString(host, port, password, secure);
            }
        }

        private static string GetEnvironmentVariableOrDefault(string environmentVariableName, string defaultValue)
        {
            return Environment.GetEnvironmentVariable(environmentVariableName) ?? defaultValue;
        }

        private bool GetSecureSettingFromEnvironment()
        {
            var secureEnvVar = Environment.GetEnvironmentVariable("CACHE_SECURE");
            return bool.TryParse(secureEnvVar, out var secureFromEnv) ? secureFromEnv : _cacheSettings.Secure;
        }

        private static string BuildConnectionString(string host, string port, string password, bool secure)
        {
            var connectionStringBuilder = new StringBuilder($"{host}:{port}");

            // Add password if available
            if (!string.IsNullOrWhiteSpace(password))
            {
                connectionStringBuilder.Append($",password={password}");
            }

            // Set SSL based on secure flag
            connectionStringBuilder.Append($",ssl={secure}");

            // Always set abortConnect to false for better resilience
            connectionStringBuilder.Append(",abortConnect=False");

            return connectionStringBuilder.ToString();
        }
    }
}
