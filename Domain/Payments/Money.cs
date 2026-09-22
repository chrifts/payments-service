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
}
