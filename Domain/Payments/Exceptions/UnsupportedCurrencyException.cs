using Domain.Shared.Exceptions;

namespace Domain.Payments.Exceptions;

public sealed class UnsupportedCurrencyException(string? code)
    : DomainException($"Currency '{code}' is not supported.")
{
    public string? Code { get; } = code;
}
