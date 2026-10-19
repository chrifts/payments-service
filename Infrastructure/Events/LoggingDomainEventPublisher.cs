using Domain.Shared.Events;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Events;

// Placeholder until there are real consumers (e.g. an outbox): publishing only logs the event
public class LoggingDomainEventPublisher(ILogger<LoggingDomainEventPublisher> logger) : IDomainEventPublisher
{
    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain event published: {DomainEvent}", domainEvent);
        return Task.CompletedTask;
    }
}
