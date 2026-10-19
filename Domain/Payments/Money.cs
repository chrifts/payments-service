namespace Domain.Payments;

public sealed record Money
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    // Used by EF Core to materialize from the database: it can't pass Currency (a complex property) to a constructor
    private Money()
    {
        Currency = null!;
    }

    public Money(decimal amount, Currency currency)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        // Round, not amount.Scale: 10.00m has Scale 2 but is a valid JPY amount
        if (decimal.Round(amount, currency.Precision) != amount)
        {
            throw new ArgumentException($"{currency.Code} allows at most {currency.Precision} decimals", nameof(amount));
        }

        Amount = amount;
        Currency = currency;
    }

    public bool GreaterOrEqualTo(Money money)
    {
        EnsureSameCurrency(money.Currency);
        return Amount >= money.Amount;
    }
    
    private void EnsureSameCurrency(Currency currency)
    {
        if (currency != Currency)
        {
            throw new ArgumentException("Currency mismatch", nameof(currency));
        }
    }
}
