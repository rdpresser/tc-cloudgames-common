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

            ServiceBusSettings.TopicName = Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_TOPIC_NAME")
                                                ?? ServiceBusSettings.TopicName;

            ServiceBusSettings.UsersTopicName = Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_USERS_TOPIC")
                                                    ?? ServiceBusSettings.UsersTopicName;

            ServiceBusSettings.GamesTopicName = Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_GAMES_TOPIC")
                                                    ?? ServiceBusSettings.GamesTopicName;

            ServiceBusSettings.PaymentsTopicName = Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_PAYMENTS_TOPIC")
                                                    ?? ServiceBusSettings.PaymentsTopicName;

            ServiceBusSettings.AutoProvision = bool.TryParse(Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_AUTO_PROVISION"), out var auto)
                                              ? auto
                                              : ServiceBusSettings.AutoProvision;

            ServiceBusSettings.MaxDeliveryCount = int.TryParse(Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_MAX_DELIVERY_COUNT"), out var max)
                                                ? max
                                                : ServiceBusSettings.MaxDeliveryCount;

            ServiceBusSettings.EnableDeadLettering = bool.TryParse(Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_ENABLE_DEAD_LETTERING"), out var dlq)
                                                    ? dlq
                                                    : ServiceBusSettings.EnableDeadLettering;

            ServiceBusSettings.AutoPurgeOnStartup = bool.TryParse(Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_AUTO_PURGE_ON_STARTUP"), out var purge)
                                                    ? purge
                                                    : ServiceBusSettings.AutoPurgeOnStartup;

            ServiceBusSettings.UseControlQueues = bool.TryParse(Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_USE_CONTROL_QUEUES"), out var control)
                                                    ? control
                                                    : ServiceBusSettings.UseControlQueues;
        }

        // --------------------------------------------------
        // Static convenience method to get configured Azure Service Bus options
        // --------------------------------------------------
        public static AzureServiceBusOptions Build(IConfiguration configuration) =>
            new AzureServiceBusHelper(configuration).ServiceBusSettings;
    }
}
