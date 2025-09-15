using System.Text.Json.Serialization;

namespace TC.CloudGames.SharedKernel.Infrastructure.Messaging
{
    public record EventContext<TEvent>
        where TEvent : class
    {
        public TEvent EventData { get; init; }
        public Guid MessageId { get; init; } = Guid.NewGuid();
        public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
        public Guid AggregateId { get; init; }
        public string? UserId { get; init; }
        public bool IsAuthenticated { get; init; }
        public string? CorrelationId { get; init; }
        public string? Source { get; init; }
        public string EventType { get; init; } = typeof(TEvent).Name;
        public string AggregateType { get; init; } = string.Empty;
        public int Version { get; init; } = 1;
        public IDictionary<string, object>? Metadata { get; init; }

        //Construtor privado existente (mantido para factories)
        private EventContext(
            TEvent data,
            Guid aggregateId,
            string eventType,
            string aggregateType)
        {
            EventData = data;
            AggregateId = aggregateId;
            EventType = eventType ?? typeof(TEvent).Name;
            AggregateType = aggregateType;
        }

        // Construtor público para deserialização pelo System.Text.Json
        [JsonConstructor]
        public EventContext(
            TEvent eventData,
            Guid messageId,
            DateTime occurredAt,
            Guid aggregateId,
            string? userId,
            bool isAuthenticated,
            string? correlationId,
            string? source,
            string eventType,
            string aggregateType,
            int version,
            IDictionary<string, object>? metadata)
        {
            EventData = eventData;
            MessageId = messageId;
            OccurredAt = occurredAt;
            AggregateId = aggregateId;
            UserId = userId;
            IsAuthenticated = isAuthenticated;
            CorrelationId = correlationId;
            Source = source;
            EventType = eventType;
            AggregateType = aggregateType;
            Version = version;
            Metadata = metadata;
        }

        public static EventContext<TEvent> Create<TAggregate>(
            TEvent data,
            Guid aggregateId,
            string? userId = null,
            bool isAuthenticated = false,
            string? correlationId = null,
            string? source = null,
            int version = 1,
            IDictionary<string, object>? metadata = null)
            where TAggregate : BaseAggregateRoot
        {
            return new EventContext<TEvent>(
                data,
                aggregateId,
                typeof(TEvent).Name,
                typeof(TAggregate).Name)
            {
                UserId = userId,
                IsAuthenticated = isAuthenticated,
                CorrelationId = correlationId,
                Source = source,
                Version = version,
                Metadata = metadata
            };
        }

        public static EventContext<TEvent> CreateBasic<TAggregate>(
            TEvent data,
            Guid aggregateId)
            where TAggregate : BaseAggregateRoot
        {
            return Create<TAggregate>(
                data,
                aggregateId,
                userId: null,
                isAuthenticated: false,
                correlationId: null,
                source: null,
                version: 1,
                metadata: null
            );
        }
    }
}
