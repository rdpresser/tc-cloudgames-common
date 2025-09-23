namespace TC.CloudGames.Messaging.Extensions
{
    public static class UserEventsRegistrationExtensions
    {
        public static void RegisterUserEvents(this WolverineOptions opts)
        {
            opts.RegisterMessageType(
                typeof(EventContext<UserCreatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<UserCreatedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<UserUpdatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<UserUpdatedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<UserRoleChangedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<UserRoleChangedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<UserActivatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<UserActivatedIntegrationEvent>))
            );
            opts.RegisterMessageType(
                typeof(EventContext<UserDeactivatedIntegrationEvent>),
                DefaultFlattenedMessageName(typeof(EventContext<UserDeactivatedIntegrationEvent>))
            );
        }
    }
}
