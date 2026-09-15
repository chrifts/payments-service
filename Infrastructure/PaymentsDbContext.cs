using Infrastructure.Payment.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
{
    public DbSet<PaymentModel> Payments => Set<PaymentModel>();
}