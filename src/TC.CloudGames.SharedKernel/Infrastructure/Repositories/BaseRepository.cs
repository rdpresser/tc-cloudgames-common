using Marten;

namespace TC.CloudGames.SharedKernel.Infrastructure.Repositories
{
    public abstract class BaseRepository<TAggregate>
        where TAggregate : BaseAggregateRoot
    {
        private readonly IDocumentSession _session;

        protected BaseRepository(IDocumentSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        protected IDocumentSession Session
        {
            get
            {
                if (_session == null)
                {
                    throw new InvalidOperationException("Document session is not initialized.");
                }
                return _session;
            }
        }

        public async Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Session.Events.AggregateStreamAsync<TAggregate>(id, token: cancellationToken);
        }

        protected virtual async Task SaveChangesAsync(Guid streamId, CancellationToken cancellationToken = default, params object[] events)
        {
            // Check if this is a new aggregate by reusing GetByIdAsync
            var existingAggregate = await GetByIdAsync(streamId, cancellationToken);
            if (existingAggregate == null)
            {
                // For new aggregates, start a new stream
                _session.Events.StartStream<TAggregate>(streamId, events);
            }
            else
            {
                // For existing aggregates, append events to the existing stream
                _session.Events.Append(streamId, events);
            }

            await _session.SaveChangesAsync(cancellationToken);
        }

        protected async Task<TAggregate> LoadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await Session.LoadAsync<TAggregate>(id, cancellationToken);
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity of type {typeof(TAggregate).Name} with ID {id} not found.");
            }
            return entity;
        }

        protected async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await LoadAsync(id, cancellationToken);
            Session.Delete(entity);
            await SaveChangesAsync(id, cancellationToken);
        }
    }
}
