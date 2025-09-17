namespace TC.CloudGames.SharedKernel.Infrastructure.MessageBroker
{
    public sealed class AzureServiceBusOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string TopicName { get; set; } = "unknown-events";
        public string GamesTopicName { get; set; } = "game.events";
        public string UsersTopicName { get; set; } = "user.events";
        public string PaymentsTopicName { get; set; } = "payment.events";
        public bool AutoProvision { get; set; } = true;
        public bool AutoPurgeOnStartup { get; set; } = false;
        public bool UseControlQueues { get; set; } = true;
        public int MaxDeliveryCount { get; set; } = 10;
        public bool EnableDeadLettering { get; set; } = true;
    }
}
