using Microsoft.Extensions.Configuration;

namespace TC.CloudGames.SharedKernel.Infrastructure.MessageBroker
{
    public sealed class RabbitMqHelper
    {
        private const string RabbitMqSectionName = "RabbitMq";

        // --------------------------------------------------
        // RabbitMQ configuration loaded from appsettings 
        // and environment variables
        // --------------------------------------------------
        public RabbitMqOptions RabbitMqSettings { get; }

        public RabbitMqHelper(IConfiguration configuration)
        {
            // Bind section "RabbitMq" → RabbitMqOptions
            RabbitMqSettings = configuration.GetSection(RabbitMqSectionName).Get<RabbitMqOptions>()
                               ?? new RabbitMqOptions();

            // --------------------------------------------------
            // Override string/int properties with environment variables if present
            // --------------------------------------------------
            RabbitMqSettings.Host = GetEnvOrDefault("RABBITMQ_HOST", RabbitMqSettings.Host);
            RabbitMqSettings.Port = GetEnvIntOrDefault("RABBITMQ_PORT", RabbitMqSettings.Port);
            RabbitMqSettings.UserName = GetEnvOrDefault("RABBITMQ_USERNAME", RabbitMqSettings.UserName);
            RabbitMqSettings.Password = GetEnvOrDefault("RABBITMQ_PASSWORD", RabbitMqSettings.Password);
            RabbitMqSettings.VirtualHost = GetEnvOrDefault("RABBITMQ_VHOST", RabbitMqSettings.VirtualHost);
            RabbitMqSettings.Exchange = GetEnvOrDefault("RABBITMQ_EXCHANGE", RabbitMqSettings.Exchange);
            RabbitMqSettings.ListenUserExchange = GetEnvOrDefault("RABBITMQ_LISTEN_USER_EXCHANGE", RabbitMqSettings.ListenUserExchange);
            RabbitMqSettings.ListenGameExchange = GetEnvOrDefault("RABBITMQ_LISTEN_GAME_EXCHANGE", RabbitMqSettings.ListenGameExchange);
            RabbitMqSettings.ListenPaymentExchange = GetEnvOrDefault("RABBITMQ_LISTEN_PAYMENT_EXCHANGE", RabbitMqSettings.ListenPaymentExchange);

            // --------------------------------------------------
            // Override boolean flags (supports: true/false/1/0/yes/no)
            // --------------------------------------------------
            RabbitMqSettings.AutoProvision = GetEnvBoolOrDefault("RABBITMQ_AUTO_PROVISION", RabbitMqSettings.AutoProvision);
            RabbitMqSettings.Durable = GetEnvBoolOrDefault("RABBITMQ_DURABLE", RabbitMqSettings.Durable);
            RabbitMqSettings.UseQuorumQueues = GetEnvBoolOrDefault("RABBITMQ_USE_QUORUM", RabbitMqSettings.UseQuorumQueues);
            RabbitMqSettings.AutoPurgeOnStartup = GetEnvBoolOrDefault("RABBITMQ_AUTO_PURGE_ON_STARTUP", RabbitMqSettings.AutoPurgeOnStartup);
        }

        // --------------------------------------------------
        // Static convenience method to get configured RabbitMQ options
        // --------------------------------------------------
        public static RabbitMqOptions Build(IConfiguration configuration) =>
            new RabbitMqHelper(configuration).RabbitMqSettings;

        private static string GetEnvOrDefault(string key, string current) =>
            string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(key))
                ? current
                : Environment.GetEnvironmentVariable(key)!;

        private static int GetEnvIntOrDefault(string key, int current)
        {
            var raw = Environment.GetEnvironmentVariable(key);
            return int.TryParse(raw, out var value) ? value : current;
        }

        private static bool GetEnvBoolOrDefault(string key, bool current)
        {
            var raw = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrWhiteSpace(raw)) return current;

            // Accept common truthy/falsy representations
            return raw.Trim().ToLowerInvariant() switch
            {
                "1" or "true" or "yes" or "y" => true,
                "0" or "false" or "no" or "n" => false,
                _ => bool.TryParse(raw, out var parsed) ? parsed : current
            };
        }
    }
}
