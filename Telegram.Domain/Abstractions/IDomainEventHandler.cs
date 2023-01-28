using MediatR;

namespace Telegram.Domain.Abstractions;

public interface IDomainEventHandler<T> : INotificationHandler<T>
    where T : IDomainEvent
{
}
