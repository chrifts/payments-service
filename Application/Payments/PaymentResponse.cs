using Domain.Payments;

namespace Application.Payments;

public record PaymentResponse(
    Guid Id,
    decimal AmountDue,
    string CurrencyCode,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? PaidAt
)
{
    public static PaymentResponse FromDomain(Payment payment)
    {
        return new PaymentResponse(
            payment.Id.Value,
            payment.AmountDue.Amount,
            payment.AmountDue.Currency.Code,
            payment.Status.ToString(),
            payment.CreatedAt,
            payment.PaidAt
        );
    }
}
