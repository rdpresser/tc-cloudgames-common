using TC.CloudGames.Contracts.Events.Payments;

namespace TC.CloudGames.Messaging.Extensions
{
    public static class GameEventsRegistrationExtensions
    {
        public static void RegisterGameEvents(this WolverineOptions opts)
        {
            opts.RegisterMessageType(
                typeof(EventContext<GameBasicInfoUpdatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<GameBasicInfoUpdatedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<GamePriceUpdatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<GamePriceUpdatedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<GameStatusUpdatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<GameStatusUpdatedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<GameRatingUpdatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<GameRatingUpdatedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<GameDetailsUpdatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<GameDetailsUpdatedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<GameActivatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<GameActivatedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<GameDeactivatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<GameDeactivatedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<GamePurchasedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<GamePurchasedIntegrationEvent>))
            );
            ////opts.RegisterMessageType(
            ////    typeof(EventContext<ChargePaymentRequest>),
            ////    DefaultFlattenedMessageName(typeof(EventContext<ChargePaymentRequest>))
            ////);
            opts.RegisterMessageType(typeof(ChargePaymentRequest), nameof(ChargePaymentRequest));
            opts.RegisterMessageType(typeof(ChargePaymentResponse), nameof(ChargePaymentResponse));
        }
    }
}
