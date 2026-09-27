using Domain.Payments.Exceptions;

namespace Domain.Payments.PaymentMethods;

public sealed class CashPayment(Money tendered) : PaymentMethod
{
    public Money Tendered { get; } = tendered;

    // Used by EF Core to materialize from the database
    private CashPayment() : this(null!) { }

    public override void EnsureCanPay(Money amountDue)
    {
        if (!Tendered.GreaterOrEqualTo(amountDue))
        {
            throw new InsufficientTenderedAmountException(Tendered, amountDue);
        }
    }
}