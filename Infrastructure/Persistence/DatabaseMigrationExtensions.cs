using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class DatabaseMigrationExtensions
{
    // Applies pending migrations before the app starts serving. Meant for the Docker demo:
    // in development, migrations run explicitly with `dotnet ef database update`
    public static async Task MigrateDatabaseAsync(this IServiceProvider services)
    {
        await using AsyncServiceScope scope = services.CreateAsyncScope();
        PaymentsDbContext context = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
        await context.Database.MigrateAsync();
    }
}
