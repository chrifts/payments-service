namespace Domain.Payments.Exceptions;

public sealed class InsufficientTenderedAmountException(Money tendered, Money amountDue)
    : Exception($"Tendered {tendered.Amount} {tendered.Currency.Code} is less than amount due {amountDue.Amount} {amountDue.Currency.Code}.")
{
    public Money Tendered { get; } = tendered;
    public Money AmountDue { get; } = amountDue;
}
