using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Primitivies;

namespace Telegram.Infrastructure.Idempotence
{
    // This handler prevents publisging domain events that are already published
    // For example one of the handlers is failed, so all of the handlers will publish again because of retrying policy.
    // To prevent I can use this handler.
    public sealed class IdempotentDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {

        private readonly INotificationHandler<TDomainEvent> _decoratedHandler;
        private readonly ApplicationDbContext _dbContext;

        public IdempotentDomainEventHandler(
            INotificationHandler<TDomainEvent> decoratedHandler,
            ApplicationDbContext dbContext)
        {
            _decoratedHandler = decoratedHandler;
            _dbContext = dbContext;
        }

        public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
        {
            var consumer = _decoratedHandler.GetType().Name;

            if (await _dbContext.Set<OutboxMessageConsumer>().AnyAsync(omConsumer =>
                    omConsumer.Id == notification.Id &&
                    omConsumer.Name == consumer,
                cancellationToken
            ))
            {
                return;
            }

            await _decoratedHandler.Handle(notification, cancellationToken);

            _dbContext.Set<OutboxMessageConsumer>().Add(
                new OutboxMessageConsumer
                {
                    Id = notification.Id,
                    Name = consumer
                });

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
