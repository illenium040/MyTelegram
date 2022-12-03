using Telegram.Domain.Abstractions;

namespace Telegram.Domain.Primitivies
{
    public class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _events = new();
        public AggregateRoot(Guid id) : base(id)
        {
        }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _events.ToList();

        public void ClearDomainEvents() => _events.Clear();

        protected void RaiseDomainEvent(IDomainEvent ev)
        {
            _events.Add(ev);
        }
    }
}
