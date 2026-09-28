using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MySql;

namespace Tests.Integration.Infrastructure;

public class MySqlFixture : IAsyncLifetime
{
    private readonly MySqlContainer _container = new MySqlBuilder("mysql:8.4").Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        await using PaymentsDbContext context = CreateContext();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public PaymentsDbContext CreateContext()
    {
        string connectionString = _container.GetConnectionString();

        DbContextOptions<PaymentsDbContext> options = new DbContextOptionsBuilder<PaymentsDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;

        return new PaymentsDbContext(options);
    }
}
