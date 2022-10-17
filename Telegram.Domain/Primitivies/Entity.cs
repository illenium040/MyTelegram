namespace Telegram.Domain.Primitivies
{
    public abstract class Entity : IEntity
    {
        protected Entity(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }

        public override bool Equals(object? obj) => Equals(obj as Entity);

        public bool Equals(IEntity? other)
        {
            if (other is null) return false;
            if (other.GetType() != GetType()) return false;
            if (other is not IEntity entity) return false;
            return other.Id == entity.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(Entity? left, Entity? right) =>
            left is not null && right is not null && left.Equals(right);

        public static bool operator !=(Entity? left, Entity? right) => !(left == right);

    }
}
