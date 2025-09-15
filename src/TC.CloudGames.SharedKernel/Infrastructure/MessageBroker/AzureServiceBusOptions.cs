namespace TC.CloudGames.SharedKernel.Infrastructure.MessageBroker
{
    public sealed class AzureServiceBusOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string TopicName { get; set; } = "user-events";
        public string GameSubscriptionName { get; set; } = "game.events-subscription";
        public string UserSubscriptionName { get; set; } = "user.events-subscription";
        public string PaymentsSubscriptionName { get; set; } = "payments.events-subscription";
        public bool AutoProvision { get; set; } = true;
        public bool AutoPurgeOnStartup { get; set; } = false;
        public bool UseControlQueues { get; set; } = true;
        public int MaxDeliveryCount { get; set; } = 10;
        public bool EnableDeadLettering { get; set; } = true;
    }
}
