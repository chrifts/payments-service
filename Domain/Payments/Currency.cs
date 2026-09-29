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

    private Currency(string code, int precision)
    {
        Code = code;
        Precision = precision;
    }

    private static readonly Dictionary<string, int> PrecisionByCode = new()
    {
        ["JPY"] = 0,
        ["CLP"] = 0,
        ["EUR"] = 2,
        ["USD"] = 2,
        ["GBP"] = 2,
        ["BTC"] = 8,
    };

    public static Currency FromCode(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        code = code.ToUpperInvariant();

        if (PrecisionByCode.TryGetValue(code, out int precision))
        {
            return new Currency(code, precision);
        }

        throw new ArgumentException("Currency code not supported", nameof(code));
    }
}