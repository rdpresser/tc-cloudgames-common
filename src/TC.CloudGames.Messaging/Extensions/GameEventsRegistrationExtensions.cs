using TC.CloudGames.Contracts.Events.Games;
using TC.CloudGames.SharedKernel.Infrastructure.Messaging;
using Wolverine;
using static TC.CloudGames.Messaging.Extensions.MessageNameHelper;

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
        }
    }
}
