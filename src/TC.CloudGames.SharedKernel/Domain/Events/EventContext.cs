//namespace TC.CloudGames.SharedKernel.Domain.Events
//{
//    public record EventContext<TEventType>(TEventType Data) where TEventType : BaseAggregateRoot
//    {
//        public Guid EventId { get; init; } = Guid.NewGuid();
//        public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
//        public Guid AggregateId { get; set; }
//        public string? UserId { get; init; }
//        public string? CorrelationId { get; init; }
//        public string? Source { get; init; }
//        public string? EventType => GetType().Name;
//        public int Version { get; init; } = 1;
//        public IDictionary<string, object>? Metadata { get; init; }
//    }
//}

namespace TC.CloudGames.SharedKernel.Domain.Events
{
    public record EventContext<TEvent> where TEvent : BaseEvent
    {
        public TEvent EventData { get; init; }
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
        public Guid AggregateId { get; init; }
        public string? UserId { get; init; }
        public string? CorrelationId { get; init; }
        public string? Source { get; init; }
        public string EventType { get; init; } = typeof(TEvent).Name;
        public int Version { get; init; } = 1;
        public IDictionary<string, object>? Metadata { get; init; }

        private EventContext(TEvent data, Guid aggregateId, string eventType)
        {
            EventData = data;
            AggregateId = aggregateId;
            EventType = eventType ?? typeof(TEvent).Name;
        }

        /// <summary>
        /// Factory method para criar EventContext com dados completos
        /// </summary>
        public static EventContext<TEvent> Create(
            TEvent data,
            Guid aggregateId,
            string eventType,
            string? userId = null,
            string? correlationId = null,
            string? source = null,
            int version = 1,
            IDictionary<string, object>? metadata = null)
        {
            return new EventContext<TEvent>(data, aggregateId, eventType ?? typeof(TEvent).Name)
            {
                UserId = userId,
                CorrelationId = correlationId,
                Source = source,
                Version = version,
                Metadata = metadata
            };
        }

        /// <summary>
        /// Factory method simplificado para cenários básicos
        /// </summary>
        public static EventContext<TEvent> Create(
            TEvent data,
            string eventType,
            string? userId = null,
            string? correlationId = null,
            string? source = null)
        {
            return Create(
                data: data,
                aggregateId: data.Id,
                eventType: eventType ?? typeof(TEvent).Name,
                userId: userId,
                correlationId: correlationId,
                source: source
            );
        }
    }
}