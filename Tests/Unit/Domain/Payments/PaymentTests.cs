using Domain.Payments;
using Domain.Payments.Exceptions;
using Domain.Payments.Events;
using Domain.Payments.PaymentMethods;

namespace Tests.Unit.Domain.Payments;

public class PaymentTest
{
    readonly Money money;
    readonly PaymentMethod method;
    readonly Payment payment;

    public PaymentTest()
    {
        money = new() { Amount = 2134, Currency = new Currency("BTC", 8) };
        method = new CashPayment(money);
        payment = new(money, method);
    }

    [Fact]
    public void Constructor_SetsAmount()
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
        payment.Pay();

        // Then
        Assert.True(payment.IsPaid);
    }

    [Fact]
    public void Pay_RaisesDomainEvent()
    {
        // Given
        // When
        payment.Pay();

        // Then
        Assert.Single(payment.Events);
        Assert.IsType<PaymentPaidEvent>(payment.Events[0]);
    }

    [Fact]
    public void ClearEvents_RemovesAllEvents()
    {
        // Given
        payment.Pay();

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
        payment.Pay();
        payment.Pay();

        // Then
        Assert.Single(payment.Events);
        Assert.IsType<PaymentPaidEvent>(payment.Events[0]);
    }

    [Fact]
    public void Pay_WithInsufficientTendered_Throws()
    {
        // Given
        Currency currency = new("BTC", 8);

        Money toPay = new() { Amount = 2134, Currency = currency };
        Money tendered = new() { Amount = 1, Currency = currency };

        CashPayment method = new(tendered);
        Payment cashPayment = new(toPay, method);

        // When / Then
        Assert.Throws<InsufficientTenderedAmountException>(cashPayment.Pay);
        Assert.False(cashPayment.IsPaid);
        Assert.Empty(cashPayment.Events);
    }
}