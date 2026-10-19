using Domain.Payments;
using Domain.Payments.Exceptions;
using Domain.Payments.Events;
using Domain.Payments.PaymentMethods;
using Microsoft.Extensions.Time.Testing;

namespace Tests.Unit.Domain.Payments;

public class PaymentTests
{
    private readonly Money _money;
    private readonly PaymentMethod _method;
    private readonly Payment _payment;
    private readonly FakeTimeProvider _fakeTimeProvider;
    private readonly DateTimeOffset _createdAt;

    public PaymentTests()
    {
        _money = new(2134, Currency.FromCode("BTC"));
        _method = new CashPayment(_money);
        _createdAt = new(2026, 9, 26, 12, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider = new FakeTimeProvider(_createdAt);
        _payment = Payment.Create(_money, _method, _fakeTimeProvider);
    }

    [Fact]
    public void Create_SetsAmount()
    {
        // Then
        Assert.Equal(2134, _payment.AmountDue.Amount);
        Assert.Equal(_method, _payment.Method);
    }

    [Fact]
    public void Pay_MarksAsPaid()
    {
        // When
        _payment.Pay(_fakeTimeProvider);

        // Then
        Assert.True(_payment.IsPaid);
    }

    [Fact]
    public void Pay_RaisesDomainEvent()
    {
        // Given
        _fakeTimeProvider.Advance(TimeSpan.FromMinutes(10));

        // When
        _payment.Pay(_fakeTimeProvider);

        // Then
        PaymentPaidEvent paidEvent = Assert.Single(_payment.Events.OfType<PaymentPaidEvent>());
        Assert.Equal(new PaymentPaidEvent(_payment.Id, _createdAt.AddMinutes(10)), paidEvent);
    }

    [Fact]
    public void ClearEvents_RemovesAllEvents()
    {
        // Given
        _payment.Pay(_fakeTimeProvider);

        // When
        _payment.ClearEvents();

        // Then
        Assert.Empty(_payment.Events);
    }

    [Fact]
    public void Pay_WhenAlreadyPaid_DoesNotRaiseAnotherEvent()
    {
        // When
        _payment.Pay(_fakeTimeProvider);
        _payment.Pay(_fakeTimeProvider);

        // Then
        Assert.Single(_payment.Events.OfType<PaymentPaidEvent>());
    }

    [Fact]
    public void Pay_WithInsufficientTendered_Throws()
    {
        // Given
        Currency currency = Currency.FromCode("BTC");

        Money toPay = new(2134, currency);
        Money tendered = new(1, currency);

        CashPayment method = new(tendered);
        Payment cashPayment = Payment.Create(toPay, method, _fakeTimeProvider);

        // When / Then
        Assert.Throws<InsufficientTenderedAmountException>(() => cashPayment.Pay(_fakeTimeProvider));
        Assert.False(cashPayment.IsPaid);
        Assert.Empty(cashPayment.Events.OfType<PaymentPaidEvent>());
    }

    [Fact]
    public void Create_AssignsUniqueId()
    {
        // When
        Payment otherPayment = Payment.Create(_money, _method, _fakeTimeProvider);

        // Then
        Assert.NotEqual(_payment.Id, otherPayment.Id);
    }

    [Fact]
    public void Create_RaisesDomainEvent()
    {
        // Then
        PaymentCreatedEvent createdEvent = Assert.Single(_payment.Events.OfType<PaymentCreatedEvent>());
        Assert.Equal(new PaymentCreatedEvent(_payment.Id, _createdAt), createdEvent);
    }

    [Fact]
    public void Create_SetsCreatedAt()
    {
        // Then
        Assert.Equal(_createdAt, _payment.CreatedAt);
    }

    [Fact]
    public void Pay_SetsPaidAt()
    {
        // Given
        _fakeTimeProvider.Advance(TimeSpan.FromMinutes(10));

        // When
        _payment.Pay(_fakeTimeProvider);

        // Then
        Assert.Equal(_createdAt.AddMinutes(10), _payment.PaidAt);
    }

    [Fact]
    public void Create_SetsStatusPending()
    {
        // Then
        Assert.Equal(PaymentStatus.Pending, _payment.Status);
    }

    [Fact]
    public void Pay_SetsStatusPaid()
    {
        // When
        _payment.Pay(_fakeTimeProvider);
        // Then
        Assert.Equal(PaymentStatus.Paid, _payment.Status);
    }
}