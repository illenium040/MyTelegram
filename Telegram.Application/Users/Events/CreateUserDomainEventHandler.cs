using Telegram.Domain.Abstractions;
using Telegram.Domain.DomainEvents;

namespace Telegram.Application.Users.Events
{
    // There is an example of domain event handler
    public class CreateUserDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
    {
        // As an example, you can send an email to the user
        public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"User {notification.UserName} has been create successfully!");
        }
    }
}
