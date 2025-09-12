namespace TC.CloudGames.SharedKernel.Infrastructure.MessageBroker
{
    public sealed class RabbitMqOptions
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5673;
        public string VirtualHost { get; set; } = "/";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";

        public string Exchange { get; set; } = "unknown.events";
        public bool AutoProvision { get; set; } = true;
        public bool Durable { get; set; } = true;
        public bool UseQuorumQueues { get; set; } = false;
        public bool AutoPurgeOnStartup { get; set; } = false;

        // Computed property → builds the full AMQP URI
        public string ConnectionString =>
            $"amqp://{UserName}:{Password}@{Host}:{Port}{VirtualHost}";
    }
}
