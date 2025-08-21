namespace TC.CloudGames.SharedKernel.Infrastructure.MessageBroker
{
    public sealed class MessageBrokerOptions
    {
        public BrokerType Type { get; set; } = BrokerType.RabbitMQ;
        public RabbitMqOptions? RabbitMqSettings { get; set; }
        public AzureServiceBusOptions? ServiceBusSettings { get; set; }
    }
}
