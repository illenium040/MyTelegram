namespace Telegram.Domain.Entities;

public class OutboxMessageConsumer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
