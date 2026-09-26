namespace Domain.Payments;

public record PaymentId
{
    public Guid Value { get; }

    public PaymentId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Empty Guid");
        }

        Value = value;
    }

    public static PaymentId New()
    {
        return new PaymentId(Guid.CreateVersion7());
    }
}
