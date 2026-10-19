using Domain.Payments;
using Domain.Payments.PaymentMethods;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Time.Testing;

namespace Tests.Integration.Infrastructure.Persistence.Repositories;

public class PaymentRepositoryTests : IClassFixture<MySqlFixture>
{
    private readonly MySqlFixture _fixture;
    private readonly Money _money;
    private readonly PaymentMethod _method;
    private readonly Payment _payment;
    private readonly FakeTimeProvider _fakeTimeProvider;
    private readonly DateTimeOffset _createdAt;

    public PaymentRepositoryTests(MySqlFixture fixture)
    {
        _fixture = fixture;
        _money = new(2134, Currency.FromCode("BTC"));
        _method = new CashPayment(_money);
        _createdAt = new(2026, 9, 26, 12, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider = new FakeTimeProvider(_createdAt);
        _payment = Payment.Create(_money, _method, _fakeTimeProvider);
    }

    [Fact]
    public async Task Add_ThenGetByIdAsync_ReturnsSamePayment()
    {
        // Given
        // Braces scope the write context: it's disposed right after saving, not at the end of the method
        await using (PaymentsDbContext writeContext = _fixture.CreateContext())
        {
            new PaymentRepository(writeContext).Add(_payment);
            await writeContext.SaveChangesAsync();
        }

        // When
        // A fresh context forces a real read from MySQL instead of EF's in-memory tracked instance
        await using PaymentsDbContext readContext = _fixture.CreateContext();
        Payment? loaded = await new PaymentRepository(readContext).GetByIdAsync(_payment.Id, CancellationToken.None);

        // Then
        Assert.NotNull(loaded);
        Assert.NotSame(_payment, loaded);
        Assert.Equal(_payment.Id, loaded.Id);
        Assert.Equal(_money, loaded.AmountDue);
        Assert.Equal(PaymentStatus.Pending, loaded.Status);
        Assert.Equal(_createdAt, loaded.CreatedAt);
        Assert.Null(loaded.PaidAt);

        CashPayment method = Assert.IsType<CashPayment>(loaded.Method);
        Assert.Equal(_money, method.Tendered);
    }
}
