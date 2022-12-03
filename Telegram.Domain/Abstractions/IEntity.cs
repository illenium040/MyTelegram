namespace Telegram.Domain.Abstractions
{
    public interface IEntity : IEquatable<IEntity?>
    {
        public Guid Id { get; }
    }
}
