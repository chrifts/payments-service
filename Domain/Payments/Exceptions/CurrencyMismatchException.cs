using Domain.Shared.Exceptions;

namespace Domain.Payments.Exceptions;

public sealed class CurrencyMismatchException(Currency expected, Currency actual)
    : DomainException($"Expected {expected.Code} but got {actual.Code}.")
{
    public Currency Expected { get; } = expected;
    public Currency Actual { get; } = actual;
}
