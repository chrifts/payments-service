using Domain.Shared.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

// Publishes domain events only after SaveChanges succeeded, so nothing is announced unless it was saved.
// Only the async pipeline is hooked: the app always calls SaveChangesAsync.
public class PublishDomainEventsInterceptor(IDomainEventPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default
    )
    {
        if (eventData.Context is not null)
        {
            await PublishDomainEventsAsync(eventData.Context, cancellationToken);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEventsAsync(DbContext context, CancellationToken cancellationToken)
    {
        List<IHasDomainEvents> entities = [.. context.ChangeTracker.Entries<IHasDomainEvents>().Select(entry => entry.Entity)];
        List<IDomainEvent> domainEvents = [.. entities.SelectMany(entity => entity.Events)];

        // Cleared before publishing: if a publisher fails, the next SaveChanges won't publish them again
        foreach (IHasDomainEvents entity in entities)
        {
            entity.ClearEvents();
        }

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.PublishAsync(domainEvent, cancellationToken);
        }
    }
}
