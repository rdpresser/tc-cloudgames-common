namespace TC.CloudGames.Contracts.Events.Games
{
    // ------------------------------------------------------------------------------------
    // Integration Events
    // ------------------------------------------------------------------------------------
    // These events represent integration contracts between microservices.
    // They are designed to expose only the necessary information for other services,
    // avoiding any sensitive domain data (e.g., internal Value Objects details).
    //
    // Conventions:
    // 1. Each event inherits from BaseIntegrationEvent, which provides:
    //    - EventId: unique identifier for the event (for idempotency and tracing)
    //    - AggregateId: the identifier of the originating aggregate
    //    - OccurredOn: timestamp when the event happened
    //    - EventName: name of the event for logging and auditing
    //
    // 2. Naming convention: [DomainEntity][Action]IntegrationEvent (e.g., GameCreatedIntegrationEvent)
    //
    // 3. These events are used by Application layer to publish to the message bus
    //    (e.g., RabbitMQ, Wolverine) and by other microservices to subscribe/handle them.
    //
    // Purpose:
    // Ensures a clean, stable, and safe contract for integration between bounded contexts
    // while maintaining consistency and traceability across services.

    // External Events (clean and safe contract for integration, exposing only necessary data)

    /// <summary>
    /// Integration event triggered when a new game is created.
    /// Exposes only the necessary information for other services,
    /// avoiding sensitive domain data like internal value objects details.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/> to provide:
    /// - EventId: unique identifier for this event instance
    /// - AggregateId: identifier of the originating aggregate
    /// - OccurredOn: timestamp when the event occurred
    /// - EventName: event name for logging and auditing
    /// 
    /// Naming convention: GameCreatedIntegrationEvent
    /// Used by Application layer to publish to the message bus for other services to consume.
    /// </remarks>
    public record GameCreatedIntegrationEvent(
        Guid AggregateId,
        string Name,
        DateTime ReleaseDate,
        string AgeRating,
        string? Description,
        string Developer,
        string? Publisher,
        decimal DiskSizeInGb,
        decimal PriceAmount,
        string? Genre,
        string[] PlatformList,
        string? Tags,
        string GameMode,
        string DistributionFormat,
        string? AvailableLanguages,
        bool SupportsDlcs,
        string MinimumSystemRequirements,
        string? RecommendedSystemRequirements,
        int? PlaytimeHours,
        int? PlayerCount,
        decimal? RatingAverage,
        string? OfficialLink,
        string? GameStatus,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), AggregateId, OccurredOn, nameof(GameCreatedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when a game's basic information is updated.
    /// Contains essential game information changes for other services.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/> to maintain consistency and traceability.
    /// Naming convention: GameBasicInfoUpdatedIntegrationEvent
    /// </remarks>
    public record GameBasicInfoUpdatedIntegrationEvent(
        Guid Id,
        string Name,
        string? Description,
        string? OfficialLink,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(GameBasicInfoUpdatedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when a game's price is updated.
    /// Contains the new price information for other services like billing or notifications.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/>.
    /// Naming convention: GamePriceUpdatedIntegrationEvent
    /// </remarks>
    public record GamePriceUpdatedIntegrationEvent(
        Guid Id,
        decimal NewPriceAmount,
        decimal? OldPriceAmount,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(GamePriceUpdatedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when a game's status changes.
    /// Important for inventory management and availability tracking in other services.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/>.
    /// Naming convention: GameStatusUpdatedIntegrationEvent
    /// </remarks>
    public record GameStatusUpdatedIntegrationEvent(
        Guid Id,
        string Name,
        string NewStatus,
        string? OldStatus,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(GameStatusUpdatedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when a game's rating is updated.
    /// Useful for recommendation services and analytics.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/>.
    /// Naming convention: GameRatingUpdatedIntegrationEvent
    /// </remarks>
    public record GameRatingUpdatedIntegrationEvent(
        Guid Id,
        string Name,
        decimal? NewRatingAverage,
        decimal? OldRatingAverage,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(GameRatingUpdatedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when a game's details are updated.
    /// Contains comprehensive game information changes for catalog and recommendation services.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/>.
    /// Naming convention: GameDetailsUpdatedIntegrationEvent
    /// </remarks>
    public record GameDetailsUpdatedIntegrationEvent(
        Guid Id,
        string Name,
        string? Genre,
        string[] PlatformList,
        string? Tags,
        string GameMode,
        string DistributionFormat,
        string? AvailableLanguages,
        bool SupportsDlcs,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(GameDetailsUpdatedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when a game is activated.
    /// Signals to other services that the game is now available for purchase/download.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/>.
    /// Naming convention: GameActivatedIntegrationEvent
    /// </remarks>
    public record GameActivatedIntegrationEvent(
        Guid Id,
        string Name,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(GameActivatedIntegrationEvent));

    /// <summary>
    /// Integration event triggered when a game is deactivated.
    /// Signals to other services that the game is no longer available for purchase/download.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseIntegrationEvent"/>.
    /// Naming convention: GameDeactivatedIntegrationEvent
    /// </remarks>
    public record GameDeactivatedIntegrationEvent(
        Guid Id,
        string Name,
        DateTimeOffset OccurredOn
    ) : BaseIntegrationEvent(Guid.NewGuid(), Id, OccurredOn, nameof(GameDeactivatedIntegrationEvent));
}
