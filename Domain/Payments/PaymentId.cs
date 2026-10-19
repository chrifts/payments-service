using Domain.Payments.Exceptions;

namespace Domain.Payments;

public sealed record PaymentId
{
    public Guid Value { get; }

    public PaymentId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidPaymentIdException();
        }

        Value = value;
    }

    public static PaymentId New()
    {
        return new PaymentId(Guid.CreateVersion7());
    }
}
