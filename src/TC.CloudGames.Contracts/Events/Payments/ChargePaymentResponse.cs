namespace TC.CloudGames.Contracts.Events.Payments
{
    public record ChargePaymentResponse
    {
        public bool Success { get; init; }
        public Guid? PaymentId { get; init; }
        public string? ErrorMessage { get; init; }

        public ChargePaymentResponse() { }

        public ChargePaymentResponse(bool success, Guid? paymentId, string? errorMessage)
        {
            Success = success;
            PaymentId = paymentId;
            ErrorMessage = errorMessage;
        }
    }
}
