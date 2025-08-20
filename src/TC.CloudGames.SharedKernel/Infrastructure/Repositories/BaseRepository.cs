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

        public async Task<TAggregate?> GetByIdAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            return await Session.Events.AggregateStreamAsync<TAggregate>(aggregateId, token: cancellationToken).ConfigureAwait(false);
        }

        public abstract Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

        ////private async Task SaveChangesAsync(Guid aggregateId, CancellationToken cancellationToken = default, params object[] events)
        ////{
        ////    await InsertOrUpdateAsync(aggregateId, cancellationToken, events).ConfigureAwait(false);

        ////    await Session.SaveChangesAsync(cancellationToken);
        ////}

        public async Task InsertOrUpdateAsync(Guid aggregateId, CancellationToken cancellationToken = default, params object[] events)
        {
            // Check if this is a new aggregate by reusing GetByIdAsync
            var existingAggregate = await GetByIdAsync(aggregateId, cancellationToken);
            if (existingAggregate == null)
            {
                // For new aggregates, start a new stream
                Session.Events.StartStream<TAggregate>(aggregateId, events);
            }
            else
            {
                // For existing aggregates, append events to the existing stream
                Session.Events.Append(aggregateId, events);
            }
        }

        public async Task<TAggregate> LoadAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var entity = await Session.LoadAsync<TAggregate>(aggregateId, cancellationToken);
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity of type {typeof(TAggregate).Name} with ID {aggregateId} not found.");
            }
            return entity;
        }

        public async Task DeleteAsync(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var entity = await LoadAsync(aggregateId, cancellationToken);
            Session.Delete(entity);
            await Session.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            if (aggregate.UncommittedEvents.Any())
            {
                ////await InsertOrUpdateAsync(aggregate.Id, cancellationToken, [.. aggregate.UncommittedEvents]).ConfigureAwait(false);

                await Session.SaveChangesAsync(cancellationToken);

                ////await SaveChangesAsync(aggregate.Id, cancellationToken, [.. aggregate.UncommittedEvents]).ConfigureAwait(false);
                aggregate.MarkEventsAsCommitted();
            }
        }

        ////public async Task SaveAsync<TEvent>(TAggregate aggregate, IEnumerable<EventContext<TEvent, TAggregate>> contexts, CancellationToken cancellationToken = default)
        ////    where TEvent : class
        ////{
        ////    var pureEvents = aggregate.UncommittedEvents.ToArray();
        ////    if (pureEvents.Length == 0)
        ////        return;

        ////    await InsertOrUpdateAsync(aggregate.Id, cancellationToken, pureEvents).ConfigureAwait(false);

        ////    foreach (var ctx in contexts)
        ////    {
        ////        var envelope = EventEnvelope<TEvent, TAggregate>.CreateForDomainEvent(ctx);
        ////        _logger.LogDebug("Publishing envelope Id {EnvelopeId} with routing key {RoutingKey}", envelope.EnvelopeId, envelope.RoutingKey);
        ////        await _bus.PublishAsync(envelope);
        ////    }

        ////    await Session.SaveChangesAsync(cancellationToken);
        ////    aggregate.MarkEventsAsCommitted();
        ////}
    }
}
