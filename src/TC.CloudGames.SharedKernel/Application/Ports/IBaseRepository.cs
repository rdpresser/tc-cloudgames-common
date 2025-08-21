namespace TC.CloudGames.SharedKernel.Application.Ports
{
    public interface IBaseRepository<TAggregate> where TAggregate : BaseAggregateRoot
    {
        Task<TAggregate?> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken = default);
        Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
        /// <summary>
        /// Semantic alias to <see cref="SaveAsync"/> clarifying intention to persist the aggregate state
        /// (domain events + any Wolverine outbox messages enlisted in the current session).
        /// Always flushes the underlying Marten session even when there are no new domain events.
        /// </summary>
        Task PersistAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
        Task Commmit(TAggregate aggregate, CancellationToken cancellationToken = default);
        Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid aggregateId, CancellationToken cancellationToken = default);
        Task<TAggregate> LoadAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    }
}
