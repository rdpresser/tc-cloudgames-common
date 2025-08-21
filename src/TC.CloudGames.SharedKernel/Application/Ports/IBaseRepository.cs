namespace TC.CloudGames.SharedKernel.Application.Ports
{
    public interface IBaseRepository<TAggregate>
        where TAggregate : BaseAggregateRoot
    {
        /// <summary>
        /// Retrieves an aggregate by its ID. Returns null if not found.
        /// </summary>
        Task<TAggregate?> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves the aggregate's uncommitted events to the Event Store.
        /// Does not necessarily commit the session.
        /// </summary>
        Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Persists the aggregate state, including domain events and any Wolverine outbox messages
        /// enlisted in the current session. Always flushes the Marten session even if there are no new events.
        /// </summary>
        Task PersistAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the previously saved events by applying SaveChanges on the Marten session.
        /// </summary>
        Task CommitAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all aggregates.
        /// </summary>
        Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the aggregate by ID and persists the deletion.
        /// </summary>
        Task DeleteAsync(Guid aggregateId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Loads the aggregate by ID. Throws an exception if not found.
        /// </summary>
        Task<TAggregate> LoadAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    }
}
