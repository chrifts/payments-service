namespace Domain.Payments;

// sealed + get-only properties: no subclass or `with` expression can bypass FromCode
public sealed record Currency
{
    public string Code { get; }
    public int Precision { get; }

    // No validation here: only FromCode (already validated) and EF Core (loading from the database) call it
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