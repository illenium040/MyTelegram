using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Domain.DomainEvents;

namespace Telegram.Domain.Abstractions
{
    public interface IDomainEventHandler<T> : INotificationHandler<T>
        where T : IDomainEvent
    {
    }
}
