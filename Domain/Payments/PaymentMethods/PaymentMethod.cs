namespace Domain.Payments.PaymentMethods;

public abstract class PaymentMethod
{
    private protected PaymentMethod() { }

    public abstract void EnsureCanPay(Money amountDue);
}