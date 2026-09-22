namespace Domain.Payments;

public record Currency
{
    private readonly string _code = null!;
    public string Code
    {
        get => _code;
        init
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _code = value;
        }
    }

    private readonly int _precision;
    public int Precision
    {
        get => _precision;
        init
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _precision = value;
        }
    }

    public Currency(string code, int precision)
    {
        Code = code;
        Precision = precision;
    }
}