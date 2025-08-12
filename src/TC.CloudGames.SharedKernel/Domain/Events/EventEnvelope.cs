//namespace TC.CloudGames.SharedKernel.Domain.Events
//{
//    public record EventEnvelope<TEventContext>(TEventContext Data) where TEventContext : EventContext<BaseAggregateRoot>
//    {
//        public Guid EnvelopeId { get; init; } = Guid.NewGuid();
//        public DateTime PublishedAt { get; init; } = DateTime.UtcNow;
//        public string? RoutingKey { get; init; }
//        public IDictionary<string, object>? Headers { get; init; }
//    }
//}

namespace TC.CloudGames.SharedKernel.Domain.Events
{
    public record EventEnvelope<TEventContext> where TEventContext : EventContext<BaseEvent>
    {
        public TEventContext EventContextData { get; init; }
        public Guid EnvelopeId { get; init; } = Guid.NewGuid();
        public DateTime PublishedAt { get; init; } = DateTime.UtcNow;
        public string? RoutingKey { get; init; }
        public IDictionary<string, object>? Headers { get; init; }

        private EventEnvelope(TEventContext data)
        {
            EventContextData = data;
        }

        /// <summary>
        /// Factory method para criar EventEnvelope completo
        /// </summary>
        public static EventEnvelope<TEventContext> Create(
            TEventContext eventContext,
            string? routingKey = null,
            IDictionary<string, object>? headers = null)
        {
            return new EventEnvelope<TEventContext>(eventContext)
            {
                RoutingKey = routingKey ?? GenerateRoutingKey(eventContext),
                Headers = MergeHeaders(eventContext, headers)
            };
        }

        /// <summary>
        /// Factory method para eventos de domínio específicos
        /// </summary>
        public static EventEnvelope<TEventContext> CreateForDomainEvent(
            TEventContext eventContext,
            string? customRoutingKey = null)
        {
            var routingKey = customRoutingKey ?? GenerateRoutingKey(eventContext);
            var headers = new Dictionary<string, object>
            {
                { "aggregate-type", eventContext.EventData.GetType().Name },
                { "aggregate-id", eventContext.AggregateId.ToString() },
                { "event-type", eventContext.EventType },
                { "event-version", eventContext.Version },
                { "correlation-id", eventContext.CorrelationId ?? string.Empty },
                { "source", eventContext.Source ?? "Unknown" }
            };

            return new EventEnvelope<TEventContext>(eventContext)
            {
                RoutingKey = routingKey,
                Headers = headers
            };
        }

        private static string GenerateRoutingKey(TEventContext eventContext)
        {
            var aggregateType = eventContext.EventData.GetType().Name.Replace("Aggregate", "").ToLowerInvariant();
            var eventType = eventContext.EventType.Replace("Event", "").ToLowerInvariant();
            return $"{aggregateType}.{eventType}";
        }

        private static IDictionary<string, object>? MergeHeaders(TEventContext eventContext, IDictionary<string, object>? customHeaders)
        {
            var defaultHeaders = new Dictionary<string, object>
            {
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