using Domain.Payments.PaymentMethods;

namespace Domain.Payments;

public class Payment
{
    public readonly Money Amount;
    public readonly PaymentMethod Method;
    private bool paid = false;

    public Payment(Money amount, PaymentMethod method)
    {
        Amount = amount;
        Method = method;
    }

    public void Pay()
    {
        paid = true;
    }

    public bool IsPaid()
    {
        return paid;
    }
}