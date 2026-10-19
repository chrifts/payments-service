using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

    public string ConnectionString => _container.GetConnectionString();

    public PaymentsDbContext CreateContext(params IInterceptor[] interceptors)
    {
        DbContextOptions<PaymentsDbContext> options = new DbContextOptionsBuilder<PaymentsDbContext>()
            .UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString))
            .AddInterceptors(interceptors)
            .Options;

        return new PaymentsDbContext(options);
    }
}
