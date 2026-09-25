using Domain.Payments.Exceptions;

namespace Domain.Payments.PaymentMethods;

public sealed class CashPayment(Money tendered) : PaymentMethod
{
    public Money Tendered { get; } = tendered;

    public override void EnsureCanPay(Money amountDue)
    {
        if (!Tendered.GreaterOrEqualTo(amountDue))
        {
            throw new InsufficientTenderedAmountException(Tendered, amountDue);
        }
    }
}