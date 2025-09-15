namespace TC.CloudGames.SharedKernel.Infrastructure.Messaging
{
    // Envelope de transporte (para mensageria). Não é conhecido pelo domínio.
    public record EventEnvelope<TEvent, TAggregate>
        where TEvent : class
        where TAggregate : BaseAggregateRoot
    {
        public EventContext<TEvent> EventContextData { get; init; }
        public Guid EnvelopeId { get; init; } = Guid.NewGuid();
        public DateTime PublishedAt { get; init; } = DateTime.UtcNow;
        public string RoutingKey { get; init; }
        public IDictionary<string, object>? Headers { get; init; }

        private EventEnvelope(EventContext<TEvent> data, string routingKey, IDictionary<string, object>? headers)
        {
            EventContextData = data;
            RoutingKey = routingKey;
            Headers = headers;
        }

        /// <summary>
        /// Factory method para criar EventEnvelope completo
        /// </summary>
        public static EventEnvelope<TEvent, TAggregate> Create(
            EventContext<TEvent> eventContext,
            string? routingKey = null,
            IDictionary<string, object>? headers = null)
        {
            var finalRoutingKey = routingKey ?? GenerateRoutingKey(eventContext);
            var finalHeaders = MergeHeaders(eventContext, headers);
            return new EventEnvelope<TEvent, TAggregate>(eventContext, finalRoutingKey, finalHeaders);
        }

        /// <summary>
        /// Factory method para eventos de domínio específicos
        /// </summary>
        public static EventEnvelope<TEvent, TAggregate> CreateForDomainEvent(
            EventContext<TEvent> eventContext,
            string? customRoutingKey = null)
        {
            var routingKey = customRoutingKey ?? GenerateRoutingKey(eventContext);
            var headers = new Dictionary<string, object>
            {
                { "aggregate-type", eventContext.AggregateType },
                { "aggregate-id", eventContext.AggregateId.ToString() },
                { "event-type", eventContext.EventType },
                { "event-version", eventContext.Version },
                { "correlation-id", eventContext.CorrelationId ?? string.Empty },
                { "source", eventContext.Source ?? "Unknown" },
                { "occurred-at", eventContext.OccurredAt.ToString("O") },
            };

            return new EventEnvelope<TEvent, TAggregate>(eventContext, routingKey, headers);
        }

        private static string GenerateRoutingKey(EventContext<TEvent> eventContext)
        {
            var aggregateType = eventContext.AggregateType.Replace("Aggregate", "").ToLowerInvariant();
            var eventType = eventContext.EventType.Replace("Event", "").ToLowerInvariant();
            return $"{aggregateType}.{eventType}";
        }

        private static IDictionary<string, object> MergeHeaders(EventContext<TEvent> eventContext, IDictionary<string, object>? customHeaders)
        {
            var defaultHeaders = new Dictionary<string, object>
            {
                { "aggregate-type", eventContext.AggregateType },
                { "aggregate-id", eventContext.AggregateId.ToString() },
                { "event-type", eventContext.EventType },
                { "occurred-at", eventContext.OccurredAt.ToString("O") }
            };

            if (customHeaders != null)
            {
                foreach (var header in customHeaders)
                {
                    defaultHeaders[header.Key] = header.Value;
                }
            }

            return defaultHeaders;
        }
    }
}