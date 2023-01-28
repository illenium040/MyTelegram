using MediatR;

namespace Telegram.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
}
