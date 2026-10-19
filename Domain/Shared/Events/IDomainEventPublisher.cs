namespace Domain.Shared.Events;

// Port: Infrastructure decides how events leave the service (a log today, an outbox or a bus later)
public interface IDomainEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken);
}
