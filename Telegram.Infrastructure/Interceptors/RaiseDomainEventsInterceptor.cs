using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using Telegram.Domain.Entities;
using Telegram.Domain.Primitivies;

namespace Telegram.Infrastructure.Interceptors;

public class RaiseDomainEventsInterceptor : SaveChangesInterceptor
{

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {

        var context = eventData.Context;

        if (context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var messages = context.ChangeTracker.Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var events = aggregateRoot.GetDomainEvents();
                aggregateRoot.ClearDomainEvents();
                return events;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccuuredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().ToString(),
                Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                })
            })
            .ToList();

        context.Set<OutboxMessage>().AddRange(messages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
