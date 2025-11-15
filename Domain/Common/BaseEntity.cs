
using System.ComponentModel.DataAnnotations;


namespace Domain.Common
{
    public abstract class BaseEntity : IHasId
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        private readonly List<BaseEvent> _domainEvents = new();
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(BaseEvent domainEvent)
        {
            if (domainEvent is null) throw new ArgumentNullException(nameof(domainEvent));
            _domainEvents.Add(domainEvent);
        }

        protected void RemoveDomainEvent(BaseEvent domainEvent)
        {
            if (domainEvent is null) throw new ArgumentNullException(nameof(domainEvent));
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
            => _domainEvents.Clear();
    }
}
