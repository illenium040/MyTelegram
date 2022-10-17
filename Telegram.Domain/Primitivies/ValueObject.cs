namespace Telegram.Domain.Primitivies
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public abstract IEnumerable<object> GetAtomicValues();

        public bool Equals(ValueObject? other)
            => other is not null && ValueAreEqual(other);

        public override bool Equals(object? obj)
            => obj is ValueObject other && ValueAreEqual(other);

        public override int GetHashCode()
             => GetAtomicValues().Aggregate(default(int), HashCode.Combine);

        private bool ValueAreEqual(ValueObject other) =>
            GetAtomicValues().SequenceEqual(other.GetAtomicValues());

    }
}
