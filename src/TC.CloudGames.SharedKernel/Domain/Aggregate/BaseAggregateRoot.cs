using Marten.Schema;
using System.Text.Json.Serialization;
using TC.CloudGames.SharedKernel.Domain.Events;

namespace TC.CloudGames.SharedKernel.Domain.Aggregate
{
    public abstract class BaseAggregateRoot
    {
        [JsonIgnore]
        private readonly List<BaseDomainEvent> _uncommittedEvents = new();

        [Identity]
        public Guid Id { get; protected set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyList<BaseDomainEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();

        // Construtor protegido para ORMs / Marten
        protected BaseAggregateRoot() { }

        protected BaseAggregateRoot(Guid id)
        {
            Id = id;
            CreatedAt = DateTimeOffset.UtcNow;
            IsActive = true;
        }

        public void AddNewEvent(BaseDomainEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event), "Event cannot be null");

            _uncommittedEvents.Add(@event);
        }

        protected void SetId(Guid id) => Id = id;

        protected void SetCreatedAt(DateTimeOffset createdAt) => CreatedAt = createdAt;

        protected void SetUpdatedAt(DateTimeOffset? updatedAt) => UpdatedAt = updatedAt;

        protected void SetDeactivate() => IsActive = false;

        protected void SetActivate() => IsActive = true;

        protected void SetActive(bool isActive) => IsActive = isActive;

        public void MarkEventsAsCommitted() => _uncommittedEvents.Clear();
    }
}
