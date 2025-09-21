namespace TC.CloudGames.Contracts.Events.Payments
{
    public record ChargePaymentResponse(
        bool Success,
        Guid? PaymentId,
        string? ErrorMessage
    );
}
