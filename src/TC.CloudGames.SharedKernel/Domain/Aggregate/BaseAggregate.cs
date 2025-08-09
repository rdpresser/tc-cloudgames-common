namespace TC.CloudGames.SharedKernel.Domain.Aggregate
{
    public abstract class BaseAggregate
    {
        protected readonly List<object> _uncommittedEvents = new();

        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyList<object> UncommittedEvents => _uncommittedEvents.AsReadOnly();

        protected BaseAggregate(Guid id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        protected void SetId(Guid id)
        {
            Id = id;
        }

        protected void SetCreatedAt(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }

        protected void SetUpdatedAt(DateTime? updatedAt)
        {
            UpdatedAt = updatedAt;
        }

        protected void SetDeactivate()
        {
            IsActive = false;
        }

        protected void SetActivate()
        {
            IsActive = true;
        }

        protected void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        /// <summary>
        /// Marks all uncommitted events as committed (called after persistence)
        /// </summary>
        public void MarkEventsAsCommitted()
        {
            _uncommittedEvents.Clear();
        }
    }
}
