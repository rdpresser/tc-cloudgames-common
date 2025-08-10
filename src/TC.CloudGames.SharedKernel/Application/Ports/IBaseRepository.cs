namespace TC.CloudGames.SharedKernel.Application.Ports
{
    public interface IBaseRepository<TAggregate> where TAggregate : BaseAggregateRoot
    {
        Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
        Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
