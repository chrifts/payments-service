namespace Domain.Payments;

public record Money
{
    public decimal Amount { get; init; }
}