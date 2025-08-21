namespace TC.CloudGames.SharedKernel.Infrastructure.Repositories
{
    public abstract class BaseRepository<TAggregate> : IBaseRepository<TAggregate>
        where TAggregate : BaseAggregateRoot
    {
        private readonly IDocumentSession _session;

        protected BaseRepository(IDocumentSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        protected IDocumentSession Session => _session ?? throw new InvalidOperationException("Document session is not initialized.");

        public async Task<TAggregate?> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken = default)
            => await Session.Events.AggregateStreamAsync<TAggregate>(aggregateId, token: cancellationToken).ConfigureAwait(false);

        public abstract Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

        protected async Task InsertOrUpdateAsync(Guid aggregateId, CancellationToken cancellationToken = default, params object[] events)
        {
            var existingAggregate = await GetByIdAsync(aggregateId, cancellationToken).ConfigureAwait(false);
            if (existingAggregate == null)
                Session.Events.StartStream<TAggregate>(aggregateId, events);
            else
                Session.Events.Append(aggregateId, events);
        }

        public async Task<TAggregate> LoadAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var entity = await Session.LoadAsync<TAggregate>(aggregateId, cancellationToken).ConfigureAwait(false)
                         ?? throw new InvalidOperationException($"Entity of type {typeof(TAggregate).Name} with ID {aggregateId} not found.");
            return entity;
        }

        public async Task DeleteAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var entity = await LoadAsync(aggregateId, cancellationToken).ConfigureAwait(false);
            Session.Delete(entity);
            await Session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));

            var uncommitted = aggregate.UncommittedEvents?.ToArray();
            if (uncommitted is { Length: > 0 })
            {
                await InsertOrUpdateAsync(aggregate.Id, cancellationToken, uncommitted).ConfigureAwait(false);
            }

            ////await Session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            ////if (uncommitted is { Length: > 0 })
            ////{
            ////    aggregate.MarkEventsAsCommitted();
            ////}
        }

        public async Task Commmit(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            await Session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            aggregate.MarkEventsAsCommitted();
        }

        public Task PersistAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
            => SaveAsync(aggregate, cancellationToken);
    }
}
