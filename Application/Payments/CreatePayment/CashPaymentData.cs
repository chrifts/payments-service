using Domain.Payments;
using Domain.Payments.PaymentMethods;

namespace Application.Payments.CreatePayment;

public record CashPaymentData(decimal TenderedAmount) : PaymentMethodData
{
    public override CashPayment ToDomain(Currency currency)
    {
        return new CashPayment(
            new Money { Amount = TenderedAmount, Currency = currency }
        );
    }
}