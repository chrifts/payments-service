namespace Domain.Payments;

public record Money
{
    private readonly decimal _amount;
    public decimal Amount
    {
        get => _amount;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
            _amount = value;
        }
    }
    public required Currency Currency { get; init; }

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
