using Telegram.Domain.Abstractions;

namespace Telegram.Domain.Primitivies;

public abstract class Entity : IEntity
{
    protected Entity(Guid id) => Id = id;
    public Guid Id { get; }

    public override bool Equals(object? obj) => Equals(obj as Entity);

    public bool Equals(IEntity? other) => other is not null && other.GetType() == GetType() && other is IEntity entity && other.Id == entity.Id;

    public override int GetHashCode() => HashCode.Combine(Id);

    public static bool operator ==(Entity? left, Entity? right) =>
        left is not null && right is not null && left.Equals(right);

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);

}
