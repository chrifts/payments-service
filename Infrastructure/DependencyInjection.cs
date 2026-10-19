using Domain.Payments;
using Domain.Shared;
using Domain.Shared.Events;
using Infrastructure.Events;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        AddDatabase(services, configuration);
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IDomainEventPublisher, LoggingDomainEventPublisher>();
        return services;
    }

    private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PaymentsDb");
        // Fixed to the mysql:8.4 image (docker-compose.yml). AutoDetect would query the server every time
        // the options are built (once per request), and `dotnet ef` would need a running database.
        var serverVersion = new MySqlServerVersion(new Version(8, 4, 0));
        services.AddScoped<ISaveChangesInterceptor, PublishDomainEventsInterceptor>();
        services.AddDbContext<PaymentsDbContext>((serviceProvider, options) =>
            options
                .UseMySql(connectionString, serverVersion)
                // Plugs in every registered ISaveChangesInterceptor: adding one doesn't require changing this line
                .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>())
        );
        // Same scoped DbContext as the repositories, so SaveChangesAsync saves what they track
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<PaymentsDbContext>());
        services.AddScoped<IPaymentRepository, PaymentRepository>();
    }
}