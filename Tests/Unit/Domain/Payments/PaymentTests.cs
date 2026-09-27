using Domain.Payments;
using Domain.Payments.Exceptions;
using Domain.Payments.Events;
using Domain.Payments.PaymentMethods;
using Microsoft.Extensions.Time.Testing;

namespace Tests.Unit.Domain.Payments;

public class PaymentTest
{
    readonly Money money;
    readonly PaymentMethod method;
    readonly Payment payment;
    readonly FakeTimeProvider fakeTimeProvider;
    readonly DateTimeOffset createdAt;

    public PaymentTest()
    {
        money = new() { Amount = 2134, Currency = new Currency("BTC", 8) };
        method = new CashPayment(money);
        createdAt = new(2026, 9, 26, 12, 0, 0, TimeSpan.Zero);
        fakeTimeProvider = new FakeTimeProvider(createdAt);
        payment = Payment.Create(money, method, fakeTimeProvider);
    }

    [Fact]
    public void Create_SetsAmount()
    {
        // Given
        // When
        // Then
        Assert.Equal(2134, payment.Amount.Amount);
        Assert.Equal(method, payment.Method);
    }

    [Fact]
    public void Mark_AsPaid()
    {
        // Given
        // When
        payment.Pay(fakeTimeProvider);

        // Then
        Assert.True(payment.IsPaid);
    }

    [Fact]
    public void Pay_RaisesDomainEvent()
    {
        // Given
        // When
        payment.Pay(fakeTimeProvider);

        // Then
        Assert.Single(payment.Events.OfType<PaymentPaidEvent>());
    }

    [Fact]
    public void ClearEvents_RemovesAllEvents()
    {
        // Given
        payment.Pay(fakeTimeProvider);

        // When
        payment.ClearEvents();

        // Then
        Assert.Empty(payment.Events);
    }

    [Fact]
    public void Pay_WhenAlreadyPaid_DoesNotRaiseAnotherEvent()
    {
        // Given
        // When
        payment.Pay(fakeTimeProvider);
        payment.Pay(fakeTimeProvider);

        // Then
        Assert.Single(payment.Events.OfType<PaymentPaidEvent>());
    }

    [Fact]
    public void Pay_WithInsufficientTendered_Throws()
    {
        // Given
        Currency currency = new("BTC", 8);

        Money toPay = new() { Amount = 2134, Currency = currency };
        Money tendered = new() { Amount = 1, Currency = currency };

        CashPayment method = new(tendered);
        Payment cashPayment = Payment.Create(toPay, method, fakeTimeProvider);

        // When / Then
        Assert.Throws<InsufficientTenderedAmountException>(() => cashPayment.Pay(fakeTimeProvider));
        Assert.False(cashPayment.IsPaid);
        Assert.Empty(cashPayment.Events.OfType<PaymentPaidEvent>());
    }

    [Fact]
    public void Create_AssignsUniqueId()
    {
        // Given
        // When
        Payment otherPayment = Payment.Create(money, method, fakeTimeProvider);

        // Then
        Assert.NotEqual(payment.Id, otherPayment.Id);
    }

    [Fact]
    public void Create_RaisesDomainEvent()
    {
        // Given
        // When
        // Then
        Assert.Single(payment.Events.OfType<PaymentCreatedEvent>());
    }

    [Fact]
    public void Create_SetsCreatedAt()
    {
        // Given
        // When
        // Then
        Assert.Equal(createdAt, payment.CreatedAt);
    }

    [Fact]
    public void Pay_SetsPaidAt()
    {
        // Given
        fakeTimeProvider.Advance(TimeSpan.FromMinutes(10));

        // When
        payment.Pay(fakeTimeProvider);

        // Then
        Assert.Equal(createdAt.AddMinutes(10), payment.PaidAt);
    }

    [Fact]
    public void Create_SetsStatusPending()
    {
        // Given
        // When
        // Then
        Assert.Equal(PaymentStatus.Pending, payment.Status);
    }

    [Fact]
    public void Pay_SetsStatusPaid()
    {
        // Given
        // When
        payment.Pay(fakeTimeProvider);
        // Then
        Assert.Equal(PaymentStatus.Paid, payment.Status);
    }
}