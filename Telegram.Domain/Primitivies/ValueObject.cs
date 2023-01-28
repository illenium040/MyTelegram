namespace Telegram.Domain.Primitivies;

public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
{
    public T Value { get; }
    protected ValueObject(T value) => Value = value;

    public abstract IEnumerable<object> GetAtomicValues();

    public bool Equals(ValueObject<T>? other)
        => other is not null && ValueAreEqual(other);

    public override bool Equals(object? obj)
        => obj is ValueObject<T> other && ValueAreEqual(other);

    public override int GetHashCode()
         => GetAtomicValues().Aggregate(default(int), HashCode.Combine);

    private bool ValueAreEqual(ValueObject<T> other) =>
        GetAtomicValues().SequenceEqual(other.GetAtomicValues());

    public static bool operator ==(ValueObject<T> a, ValueObject<T> b) => a.Equals(b);
    public static bool operator !=(ValueObject<T> a, ValueObject<T> b) => !a.Equals(b);

}
