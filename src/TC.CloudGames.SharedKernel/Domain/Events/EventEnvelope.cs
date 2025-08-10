namespace TC.CloudGames.SharedKernel.Domain.Events
{
    public record EventEnvelope<TEvent>(TEvent Data) where TEvent : EventContext
    {
        public Guid EnvelopeId { get; init; } = Guid.NewGuid();
        public DateTime PublishedAt { get; init; } = DateTime.UtcNow;
        public string? RoutingKey { get; init; }
        public IDictionary<string, object>? Headers { get; init; }
    }
}
