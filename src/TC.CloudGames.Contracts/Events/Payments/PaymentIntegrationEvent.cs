namespace TC.CloudGames.Contracts.Events.Payments
{
    public record GamePaymentStatusUpdateIntegrationEvent(
        Guid AggregateId,
        Guid UserId,
        Guid GameId,
        Guid PaymentId,
        string Status,
        bool Success,
        string? ErrorMessage,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(
            Guid.NewGuid(),       // EventId
            AggregateId,          // AggregateId (principal = UserGameLibraryAggregate)
            OccurredOn,           // OccurredOn
            nameof(GamePaymentStatusUpdateIntegrationEvent),
            new Dictionary<string, Guid>     // RelatedIds
            {
                { "UserId", UserId },
                { "GameId", GameId },
                { "PaymentId", PaymentId }
            }
        );
}
