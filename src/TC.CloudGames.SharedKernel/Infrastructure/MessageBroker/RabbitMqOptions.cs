namespace TC.CloudGames.SharedKernel.Infrastructure.MessageBroker
{
    public sealed class RabbitMqOptions
    {
        public string? ConnectionString { get; set; }
        public string Exchange { get; set; } = "user.events";
        public bool AutoProvision { get; set; } = true;
        public bool Durable { get; set; } = true;
        public string VirtualHost { get; set; } = "/";
    }
}
