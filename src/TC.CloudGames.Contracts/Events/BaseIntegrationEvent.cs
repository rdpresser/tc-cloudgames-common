namespace TC.CloudGames.Contracts.Events
{
    public abstract record BaseIntegrationEvent(
        Guid EventId,
        Guid AggregateId,
        DateTimeOffset OccurredOn,
        string EventName,
        IDictionary<string, Guid>? RelatedIds = null
    );
}
