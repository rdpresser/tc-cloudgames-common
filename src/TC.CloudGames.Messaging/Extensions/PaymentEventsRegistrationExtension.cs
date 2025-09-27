using TC.CloudGames.Contracts.Events.Payments;

namespace TC.CloudGames.Messaging.Extensions
{
    public static class PaymentEventsRegistrationExtension
    {
        public static void RegisterPaymentEvents(this WolverineOptions opts)
        {
            opts.RegisterMessageType(
                typeof(EventContext<GamePaymentStatusUpdateIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<GamePaymentStatusUpdateIntegrationEvent>))
            );
            ////opts.RegisterMessageType(typeof(ChargePaymentRequest), DefaultFlattenedMessageName(typeof(ChargePaymentRequest)));
            ////opts.RegisterMessageType(typeof(ChargePaymentResponse), DefaultFlattenedMessageName(typeof(ChargePaymentResponse)));
        }
    }
}
