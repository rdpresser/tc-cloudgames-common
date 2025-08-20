namespace TC.CloudGames.Contracts.Events
{
    public abstract record BaseIntegrationEvent(
        Guid EventId,
        Guid AggregateId,
        DateTime OccurredOn,
        string EventName
    );
}
