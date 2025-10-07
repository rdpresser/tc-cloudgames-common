namespace TC.CloudGames.Contracts.Events.Users
{
    // ------------------------------------------------------------------------------------
    // Integration Events
    // ------------------------------------------------------------------------------------
    // These events represent integration contracts between microservices.
    // They are designed to expose only the necessary information for other services,
    // avoiding any sensitive domain data (e.g., passwords, internal Value Objects).
    //
    // Conventions:
    // 1. Each event inherits from BaseIntegrationEvent, which provides:
    //    - EventId: unique identifier for the event (for idempotency and tracing)
    //    - AggregateId: the identifier of the originating aggregate
    //    - OccurredOn: timestamp when the event happened
    //    - EventName: name of the event for logging and auditing
    //
    // 2. Naming convention: [DomainEntity][Action]IntegrationEvent (e.g., UserCreatedIntegrationEvent)
    //
    // 3. These events are used by Application layer to publish to the message bus
    //    (e.g., RabbitMQ, Wolverine) and by other microservices to subscribe/handle them.
    //
    // Purpose:
    // Ensures a clean, stable, and safe contract for integration between bounded contexts
    // while maintaining consistency and traceability across services.


    // External Events (clean and safe contract for integration, exposing only necessary data)

    /// <summary>
    /// Integration event triggered when a new user is created.
    /// Exposes only the necessary information for other services,
    /// avoiding sensitive domain data like passwords or internal value objects.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/> to provide:
    /// - EventId: unique identifier for this event instance
    /// - AggregateId: identifier of the originating aggregate
    /// - OccurredOn: timestamp when the event occurred
    /// 
    /// - EventName: event name for logging and auditing
    /// 
    /// Naming convention: UserCreatedIntegrationEvent
    /// Used by Application layer to publish to the message bus for other services to consume.
    /// </remarks>
    public record UserCreatedIntegrationEvent(
        Guid AggregateId,
        string Name,
        string Email,
        string Username,
        string Role,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), AggregateId, OccurredOn, nameof(UserCreatedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when an existing user's details are updated.
    /// Only essential user information is exposed for other services.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/> to maintain consistency and traceability.
    /// Naming convention: UserUpdatedIntegrationEvent
    /// </remarks>
    public record UserUpdatedIntegrationEvent(
        Guid Id,
        string Name,
        string Email,
        string Username,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(UserUpdatedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when a user's role is changed.
    /// Contains only the new role and minimal identifying information.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/>.
    /// Naming convention: UserRoleChangedIntegrationEvent
    /// </remarks>
    public record UserRoleChangedIntegrationEvent(
        Guid Id,
        string NewRole,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(UserRoleChangedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when a user is activated.
    /// Only identifies the user and activation timestamp.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/>.
    /// Naming convention: UserActivatedIntegrationEvent
    /// </remarks>
    public record UserActivatedIntegrationEvent(
        Guid Id,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(UserActivatedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when a user is deactivated.
    /// Contains only the user identifier and deactivation timestamp.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/>.
    /// Naming convention: UserDeactivatedIntegrationEvent
    /// </remarks>
    public record UserDeactivatedIntegrationEvent(
        Guid Id,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(UserDeactivatedIntegrationEvent));

}
