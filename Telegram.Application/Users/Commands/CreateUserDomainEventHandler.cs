using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Domain.DomainEvents;

namespace Telegram.Application.Users.Commands
{
    // There is an example of domain event handler
    public class CreateUserDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
    {
        // As an example, you can send an email to the user
        public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"User {notification.UserName} has been create successfully!");
        }
    }
}
