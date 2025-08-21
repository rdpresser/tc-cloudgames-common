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
            // Override properties with environment variables if present
            // --------------------------------------------------
            RabbitMqSettings.Host = Environment.GetEnvironmentVariable("RABBITMQ_HOST")
                                    ?? RabbitMqSettings.Host;

            RabbitMqSettings.Port = int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), out var port)
                                    ? port
                                    : RabbitMqSettings.Port;

            RabbitMqSettings.UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")
                                        ?? RabbitMqSettings.UserName;

            RabbitMqSettings.Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")
                                        ?? RabbitMqSettings.Password;

            RabbitMqSettings.VirtualHost = Environment.GetEnvironmentVariable("RABBITMQ_VHOST")
                                           ?? RabbitMqSettings.VirtualHost;

            RabbitMqSettings.Exchange = Environment.GetEnvironmentVariable("RABBITMQ_EXCHANGE")
                                        ?? RabbitMqSettings.Exchange;
        }

        // --------------------------------------------------
        // Static convenience method to get configured RabbitMQ options
        // --------------------------------------------------
        public static RabbitMqOptions Build(IConfiguration configuration) =>
            new RabbitMqHelper(configuration).RabbitMqSettings;
    }
}
