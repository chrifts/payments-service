namespace Domain.Shared.Events;

public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}
