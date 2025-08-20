namespace TC.CloudGames.SharedKernel.Infrastructure.Message
{
    public record EventContext<TEvent, TAggregate>
        where TEvent : class
        where TAggregate : BaseAggregateRoot
    {
        public TEvent EventData { get; init; }
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
        public Guid AggregateId { get; init; }
        public string? UserId { get; init; }
        public bool IsAuthenticated { get; init; }
        public string? CorrelationId { get; init; }
        public string? Source { get; init; }
        public string EventType { get; init; } = typeof(TEvent).Name;
        public string AggregateType { get; init; } = typeof(TAggregate).Name;
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
        public static EventContext<TEvent, TAggregate> Create(
            TEvent data,
            Guid aggregateId,
            string eventType,
            string? userId = null,
            bool isAuthenticated = false,
            string? correlationId = null,
            string? source = null,
            int version = 1,
            IDictionary<string, object>? metadata = null)
        {
            return new EventContext<TEvent, TAggregate>(data, aggregateId, eventType ?? typeof(TEvent).Name)
            {
                UserId = userId,
                IsAuthenticated = isAuthenticated,
                CorrelationId = correlationId,
                Source = source,
                Version = version,
                Metadata = metadata
            };
        }

        /// <summary>
        /// Factory method simplificado para cenários básicos
        /// </summary>
        public static EventContext<TEvent, TAggregate> Create(
            TEvent data,
            string eventType,
            Guid aggregateId,
            string? userId = null,
            bool isAuthenticated = false,
            string? correlationId = null,
            string? source = null)
        {
            // Call the full Create method with explicit parameters
            return Create(
                data,
                aggregateId,
                eventType,
                userId,
                isAuthenticated,
                correlationId,
                source,
                1,
                null
            );
        }
    }
}