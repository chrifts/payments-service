using Domain.Shared.Exceptions;

namespace Domain.Payments.Exceptions;

public sealed class InvalidAmountException : DomainException
{
    public decimal Amount { get; }

    private InvalidAmountException(decimal amount, string message) : base(message)
    {
        Amount = amount;
    }

    public static InvalidAmountException NotPositive(decimal amount)
    {
        return new InvalidAmountException(amount, $"Amount must be greater than zero, got {amount}.");
    }

    public static InvalidAmountException TooManyDecimals(decimal amount, Currency currency)
    {
        return new InvalidAmountException(amount, $"{currency.Code} allows at most {currency.Precision} decimals, got {amount}.");
    }
}
