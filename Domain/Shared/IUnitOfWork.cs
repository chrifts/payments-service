namespace Domain.Shared;

// Saves, in one transaction, everything the current use case added or changed
public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
