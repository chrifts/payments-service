using Domain.Payments.Exceptions;

namespace Domain.Payments.PaymentMethods;

public sealed class CashPayment(Money tendered) : PaymentMethod
{
    public readonly Money Tendered = tendered;

    public override void EnsureCanPay(Money amountDue)
    {
        if (!Tendered.GreaterOrEqualTo(amountDue))
        {
            throw new InsufficientTenderedAmountException(Tendered, amountDue);
        }
    }
}