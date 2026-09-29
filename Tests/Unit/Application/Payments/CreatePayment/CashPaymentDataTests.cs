using Application.Payments.CreatePayment;
using Domain.Payments;
using Domain.Payments.PaymentMethods;

namespace Tests.Unit.Application.Payments.CreatePayment;

public class CashPaymentDataTests
{
    [Fact]
    public void ToDomain_UsesTenderedAmount()
    {
        // Given
        Currency currency = Currency.FromCode("EUR");
        CashPaymentData cashPaymentData = new(TenderedAmount: 150);

        // When
        CashPayment cashPayment = cashPaymentData.ToDomain(currency);

        // Then
        Assert.Equal(150, cashPayment.Tendered.Amount);
        Assert.Equal(currency, cashPayment.Tendered.Currency);
    }
}
