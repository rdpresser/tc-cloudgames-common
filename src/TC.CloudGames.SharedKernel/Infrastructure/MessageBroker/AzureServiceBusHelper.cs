using Microsoft.Extensions.Configuration;

namespace TC.CloudGames.SharedKernel.Infrastructure.MessageBroker
{
    public sealed class AzureServiceBusHelper
    {
        private const string ServiceBusSectionName = "AzureServiceBus";

        // --------------------------------------------------
        // Azure Service Bus configuration loaded from appsettings 
        // and environment variables
        // --------------------------------------------------
        public AzureServiceBusOptions ServiceBusSettings { get; }

        public AzureServiceBusHelper(IConfiguration configuration)
        {
            // Bind section → AzureServiceBusOptions
            ServiceBusSettings = configuration.GetSection(ServiceBusSectionName).Get<AzureServiceBusOptions>()
                                ?? new AzureServiceBusOptions();

            // --------------------------------------------------
            // Override properties with environment variables if present
            // --------------------------------------------------
            ServiceBusSettings.ConnectionString = Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_CONNECTIONSTRING")
                                                    ?? ServiceBusSettings.ConnectionString;

            ServiceBusSettings.TopicName = Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_TOPIC")
                                                ?? ServiceBusSettings.TopicName;

            ServiceBusSettings.SubscriptionName = Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_SUBSCRIPTION")
                                                    ?? ServiceBusSettings.SubscriptionName;

            ServiceBusSettings.AutoProvision = bool.TryParse(Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_AUTOPROVISION"), out var auto)
                                              ? auto
                                              : ServiceBusSettings.AutoProvision;

            ServiceBusSettings.MaxDeliveryCount = int.TryParse(Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_MAXDELIVERY"), out var max)
                                                ? max
                                                : ServiceBusSettings.MaxDeliveryCount;

            ServiceBusSettings.EnableDeadLettering = bool.TryParse(Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_ENABLEDLQ"), out var dlq)
                                                    ? dlq
                                                    : ServiceBusSettings.EnableDeadLettering;
        }

        // --------------------------------------------------
        // Static convenience method to get configured Azure Service Bus options
        // --------------------------------------------------
        public static AzureServiceBusOptions Build(IConfiguration configuration) =>
            new AzureServiceBusHelper(configuration).ServiceBusSettings;
    }
}
