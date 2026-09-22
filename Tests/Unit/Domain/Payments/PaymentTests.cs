using Domain.Payments;
using Domain.Payments.Events;
using Domain.Payments.PaymentMethods;
using Domain.Shared.Events;

namespace Tests.Unit.Domain.Payments;

public class PaymentTest
{
    readonly Money money;
    readonly PaymentMethod method;
    readonly Payment payment;

    public PaymentTest()
    {
        money = new() { Amount = 2134 };
        method = new CashPayment();
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
        Assert.True(payment.IsPaid());
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
}