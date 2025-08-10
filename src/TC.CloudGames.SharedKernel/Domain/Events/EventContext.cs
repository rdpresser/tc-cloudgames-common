namespace TC.CloudGames.SharedKernel.Domain.Events
{
    public record EventContext
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
        public Guid AggregateId { get; set; }
        public string? UserId { get; init; }
        public string? CorrelationId { get; init; }
        public string? Source { get; init; }
        public string? EventType => GetType().Name;
        public int Version { get; init; } = 1;
        public IDictionary<string, object>? Metadata { get; init; }

    }
}
