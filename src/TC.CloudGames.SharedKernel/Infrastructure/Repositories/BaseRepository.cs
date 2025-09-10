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

        /// <summary>
        /// Retrieves an aggregate by its ID. Returns null if not found.
        /// </summary>
        private async Task<TAggregate?> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken = default)
            => await Session.Events.AggregateStreamAsync<TAggregate>(aggregateId, token: cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Retrieves all aggregates. Implementation depends on concrete repository.
        /// </summary>
        public abstract Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts or appends uncommitted events of the aggregate into the event stream.
        /// </summary>
        protected async Task InsertOrUpdateAsync(Guid aggregateId, CancellationToken cancellationToken = default, params object[] events)
        {
            var existingAggregate = await GetByIdAsync(aggregateId, cancellationToken).ConfigureAwait(false);

            if (existingAggregate == null)
                Session.Events.StartStream<TAggregate>(aggregateId, events);
            else
                Session.Events.Append(aggregateId, events);
        }

        /// <summary>
        /// Loads an aggregate by its ID. Throws an exception if not found.
        /// </summary>
        public async Task<TAggregate> LoadAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var entity = await Session.LoadAsync<TAggregate>(aggregateId, cancellationToken).ConfigureAwait(false)
                         ?? throw new InvalidOperationException($"Entity of type {typeof(TAggregate).Name} with ID {aggregateId} not found.");
            return entity;
        }

        /// <summary>
        /// Deletes an aggregate by its ID and persists the deletion.
        /// </summary>
        public async Task DeleteAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var entity = await LoadAsync(aggregateId, cancellationToken).ConfigureAwait(false);
            Session.Delete(entity);
            await Session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the aggregate's uncommitted events to the event stream.
        /// Does not commit the session.
        /// </summary>
        public async Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));

            var uncommitted = aggregate.UncommittedEvents?.ToArray();
            if (uncommitted is { Length: > 0 })
            {
                await InsertOrUpdateAsync(aggregate.Id, cancellationToken, uncommitted).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Commits the current Marten session, persisting all changes (events/documents) to the database.
        /// Marks aggregate events as committed.
        /// </summary>
        public async Task CommitAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            await Session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            aggregate.MarkEventsAsCommitted();
        }

        /// <summary>
        /// Saves and commits the aggregate in a single operation.
        /// Ensures that uncommitted events are persisted and session changes are committed.
        /// </summary>
        public async Task PersistAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            await SaveAsync(aggregate, cancellationToken);
            await CommitAsync(aggregate, cancellationToken);
        }
    }
}
