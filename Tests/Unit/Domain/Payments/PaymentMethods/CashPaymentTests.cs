using Domain.Payments;
using Domain.Payments.Exceptions;
using Domain.Payments.PaymentMethods;

namespace Tests.Unit.Domain.Payments.PaymentMethods;

public class CashPaymentTests
{
    private readonly Currency _usd = Currency.FromCode("USD");

    [Theory]
    [InlineData(1233.99)]
    [InlineData(1)]
    public void EnsureCanPay_WhenTenderedIsLess_Throws(decimal tendered)
    {
        // Given
        var method = new CashPayment(new Money(tendered, _usd));
        var amountDue = new Money(1234, _usd);

        // When / Then
        Assert.Throws<InsufficientTenderedAmountException>(() => method.EnsureCanPay(amountDue));
    }

    [Theory]
    [InlineData(1234)]
    [InlineData(1234.01)]
    public void EnsureCanPay_WhenTenderedIsEqualOrGreater_DoesNotThrow(decimal tendered)
    {
        // Given
        var method = new CashPayment(new Money(tendered, _usd));
        var amountDue = new Money(1234, _usd);

        // When
        var exception = Record.Exception(() => method.EnsureCanPay(amountDue));

        // Then
        Assert.Null(exception);
    }
}
