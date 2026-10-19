namespace Domain.Shared.Events;

public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> Events { get; }
    void ClearEvents();
}
