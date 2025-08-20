namespace TC.CloudGames.SharedKernel.Application.Ports
{
    public interface IBaseRepository<TAggregate> where TAggregate : BaseAggregateRoot
    {
        Task<TAggregate?> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken = default);
        Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
        ////Task SaveAsync<TEvent>(TAggregate aggregate, IEnumerable<EventContext<TEvent, TAggregate>> contexts, CancellationToken cancellationToken = default)
        ////    where TEvent : class;
        Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
        Task InsertOrUpdateAsync(Guid aggregateId, CancellationToken cancellationToken = default, params object[] events);
        ////Task SaveChangesAsync(Guid aggregateId, CancellationToken cancellationToken = default, params object[] events);
        Task DeleteAsync(Guid aggregateId, CancellationToken cancellationToken = default);
        Task<TAggregate> LoadAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    }
}
