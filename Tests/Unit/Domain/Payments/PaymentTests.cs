using Domain.Payments;
using Domain.Payments.PaymentMethods;

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
}