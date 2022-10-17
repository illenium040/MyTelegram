namespace Telegram.Domain.Primitivies
{
    public interface IEntity : IEquatable<IEntity?>
    {
        public Guid Id { get; }
    }
}
