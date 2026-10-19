using Domain.Payments;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Payment> Payments { get; set; }

    // A DbContext already is a unit of work: the port only hides EF from Application
    Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Scans this project (Infrastructure) for every IEntityTypeConfiguration<T>
        // and applies each one. A new configuration class is registered automatically.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentsDbContext).Assembly);
    }
}