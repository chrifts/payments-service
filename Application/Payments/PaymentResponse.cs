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
        // The column is decimal(28,8) and Money never has more decimals than its currency allows,
        // so rounding to that precision only drops trailing zeros (100.50000000 -> 100.50)
        return new PaymentResponse(
            payment.Id.Value,
            decimal.Round(payment.AmountDue.Amount, payment.AmountDue.Currency.Precision),
            payment.AmountDue.Currency.Code,
            payment.Status.ToString(),
            payment.CreatedAt,
            payment.PaidAt
        );
    }
}
