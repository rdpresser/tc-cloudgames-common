namespace TC.CloudGames.Contracts.Events.Payments
{
    public record ChargePaymentRequest(
        Guid UserId,
        Guid GameId,
        decimal Amount,
        string PaymentMethod
    );
}
