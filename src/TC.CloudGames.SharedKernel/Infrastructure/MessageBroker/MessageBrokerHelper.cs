using Microsoft.Extensions.Configuration;

namespace TC.CloudGames.SharedKernel.Infrastructure.MessageBroker
{
    // --------------------------------------------------
    // Supported broker types
    // --------------------------------------------------
    public enum BrokerType
    {
        RabbitMQ,
        AzureServiceBus
    }

    public sealed class MessageBrokerHelper
    {
        // --------------------------------------------------
        // Selected broker type
        // --------------------------------------------------
        public BrokerType Type { get; }

        // --------------------------------------------------
        // Optional settings for RabbitMQ
        // --------------------------------------------------
        public RabbitMqOptions? RabbitMqSettings { get; }

        // --------------------------------------------------
        // Optional settings for Azure Service Bus
        // --------------------------------------------------
        public AzureServiceBusOptions? ServiceBusSettings { get; }

        public MessageBrokerHelper(IConfiguration configuration)
        {
            // --------------------------------------------------
            // Decide broker type via environment variable first, 
            // then appsettings, fallback to RabbitMQ
            // --------------------------------------------------
            var typeStr = Environment.GetEnvironmentVariable("MESSAGE_BROKER_TYPE")
                          ?? configuration["MessageBroker:Type"]
                          ?? "RabbitMQ";

            Type = Enum.TryParse<BrokerType>(typeStr, true, out var parsed)
                ? parsed
                : BrokerType.RabbitMQ;

            // --------------------------------------------------
            // Load RabbitMQ configuration if selected
            // --------------------------------------------------
            if (Type == BrokerType.RabbitMQ)
            {
                RabbitMqSettings = RabbitMqHelper.Build(configuration);
            }
            // --------------------------------------------------
            // Load Azure Service Bus configuration if selected
            // --------------------------------------------------
            else if (Type == BrokerType.AzureServiceBus)
            {
                ServiceBusSettings = AzureServiceBusHelper.Build(configuration);
            }
        }

        // --------------------------------------------------
        // Static convenience method
        // --------------------------------------------------
        public static MessageBrokerHelper Build(IConfiguration configuration) =>
            new MessageBrokerHelper(configuration);
    }
}
