using Domain.Payments;
using Domain.Payments.PaymentMethods;
using Domain.Shared.Events;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Microsoft.Extensions.Time.Testing;

namespace Tests.Integration.Infrastructure.Persistence.Interceptors;

public class PublishDomainEventsInterceptorTests(MySqlFixture fixture) : IClassFixture<MySqlFixture>
{
    [Fact]
    public async Task SavedChangesAsync_PublishesAndClearsDomainEvents()
    {
        // Given
        Money money = new(10, Currency.FromCode("EUR"));
        Payment payment = Payment.Create(money, new CashPayment(money), new FakeTimeProvider());
        List<IDomainEvent> raisedEvents = [.. payment.Events];

        SpyDomainEventPublisher publisher = new();
        await using PaymentsDbContext context = fixture.CreateContext(new PublishDomainEventsInterceptor(publisher));
        context.Payments.Add(payment);

        // When
        await context.SaveChangesAsync();

        // Then
        Assert.Equal(raisedEvents, publisher.Published);
        Assert.Empty(payment.Events);
    }

    // Records what the interceptor hands over, instead of logging it
    private sealed class SpyDomainEventPublisher : IDomainEventPublisher
    {
        public List<IDomainEvent> Published { get; } = [];

        public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            Published.Add(domainEvent);
            return Task.CompletedTask;
        }
    }
}
