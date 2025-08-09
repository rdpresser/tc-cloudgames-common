using Marten;

namespace TC.CloudGames.SharedKernel.Infrastructure.Repositories
{
    public abstract class BaseRepository
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

        public async Task<T?> GetByIdAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : class
        {
            return await Session.Events.AggregateStreamAsync<T>(id, token: cancellationToken);
        }

        protected virtual async Task SaveChangesAsync<T>(Guid streamId, CancellationToken cancellationToken = default, params object[] events) where T : class
        {
            // Check if this is a new aggregate by reusing GetByIdAsync
            var existingAggregate = await GetByIdAsync<T>(streamId, cancellationToken);
            if (existingAggregate == null)
            {
                // For new aggregates, start a new stream
                _session.Events.StartStream<T>(streamId, events);
            }
            else
            {
                // For existing aggregates, append events to the existing stream
                _session.Events.Append(streamId, events);
            }

            await _session.SaveChangesAsync(cancellationToken);
        }

        protected async Task<T> LoadAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : class
        {
            var entity = await Session.LoadAsync<T>(id, cancellationToken);
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity of type {typeof(T).Name} with ID {id} not found.");
            }
            return entity;
        }

        protected async Task DeleteAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : class
        {
            var entity = await LoadAsync<T>(id, cancellationToken);
            Session.Delete(entity);
            await SaveChangesAsync<T>(id, cancellationToken);
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(Func<IQueryable<T>, IQueryable<T>> query, CancellationToken cancellationToken = default) where T : class
        {
            var result = query(Session.Query<T>());
            return await result.ToListAsync(cancellationToken);
        }
    }
}
