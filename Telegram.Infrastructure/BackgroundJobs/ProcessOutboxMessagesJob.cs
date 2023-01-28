using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Quartz;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;

namespace Telegram.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        ApplicationDbContext dbContext,
        IPublisher publisher,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.Set<OutboxMessage>()
            .Where(msg => msg.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (var message in messages)
        {
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                });

            if (domainEvent is null)
            {
                continue;
            }

            var pollyPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(100 * attempt));

            var policyResult = await pollyPolicy.ExecuteAndCaptureAsync(() =>
                _publisher.Publish(domainEvent, context.CancellationToken));

            message.Error = policyResult.FinalException?.ToString();
            message.ProcessedOnUtc = DateTime.UtcNow;

            // Temporary solution to show error
            // This errors is only for backend, so it can be it for now
            if (message.Error is not null)
            {
                _logger.LogError(message.Error.ToString());
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}
