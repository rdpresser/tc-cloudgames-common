namespace TC.CloudGames.SharedKernel.Application.Ports
{
    /// <summary>
    /// Generic abstraction for repositories responsible for persisting aggregate roots.
    /// Defines a contract that hides persistence details (Marten, EF, etc.) 
    /// while exposing consistent operations for aggregates.
    /// </summary>
    public interface IBaseRepository<TAggregate> where TAggregate : BaseAggregateRoot
    {
        /// <summary>
        /// Retrieves an aggregate by its unique identifier. 
        /// Returns null if the aggregate does not exist.
        /// </summary>
        Task<TAggregate?> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves the aggregate state (including new domain events if any).
        /// May be optimized to avoid flushing the session if nothing changed.
        /// </summary>
        Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Explicit semantic alias for <see cref="SaveAsync"/> that always flushes the session,
        /// ensuring all domain events and outbox messages are persisted, 
        /// even if the aggregate has no new changes.
        /// </summary>
        Task PersistAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits any pending operations for the aggregate, ensuring consistency 
        /// across domain events and the outbox (Wolverine integration).
        /// </summary>
        Task CommitAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all aggregates of this type.
        /// Use with caution as this may be expensive for large datasets.
        /// </summary>
        Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Permanently deletes the aggregate identified by its ID.
        /// </summary>
        Task DeleteAsync(Guid aggregateId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Loads the aggregate by its ID, throwing if it does not exist.
        /// Useful when the existence of the aggregate is guaranteed or required.
        /// </summary>
        Task<TAggregate> LoadAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    }
}
