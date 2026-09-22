namespace Domain.Payments;

public class Payment
{
    public readonly Money Amount;

    public Payment(Money amount)
    {
        Amount = amount;
    }
}