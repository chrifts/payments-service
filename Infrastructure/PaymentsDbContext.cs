using Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Scans this project (Infrastructure) for every IEntityTypeConfiguration<T>
        // and applies each one. A new configuration class is registered automatically.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentsDbContext).Assembly);
    }
}