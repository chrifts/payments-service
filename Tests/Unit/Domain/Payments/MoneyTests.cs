using Domain.Payments;
using Domain.Payments.Exceptions;

namespace Tests.Unit.Domain.Payments;

public class MoneyTests
{
    private readonly Currency _usd = Currency.FromCode("USD");

    [Theory]
    [InlineData(0)]
    [InlineData(-0.0000000001)]
    public void Constructor_WithZeroOrNegativeAmount_Throws(decimal amount)
    {
        // When / Then
        Assert.Throws<InvalidAmountException>(() => new Money(amount, _usd));
    }

    [Theory]
    [InlineData("EUR", 10.255)]
    [InlineData("JPY", 10.5)]
    public void Constructor_WithMoreDecimalsThanCurrencyPrecision_Throws(string currencyCode, decimal amount)
    {
        // Given
        var currency = Currency.FromCode(currencyCode);

        // When / Then
        Assert.Throws<InvalidAmountException>(() => new Money(amount, currency));
    }

    [Theory]
    [InlineData("EUR", 10.25)]
    [InlineData("JPY", 10)]
    public void Constructor_WithDecimalsWithinCurrencyPrecision_SetsAmount(string currencyCode, decimal amount)
    {
        // Given
        var currency = Currency.FromCode(currencyCode);

        // When
        var money = new Money(amount, currency);

        // Then
        Assert.Equal(amount, money.Amount);
    }

    [Theory]
    [InlineData(1233.99)]
    [InlineData(1233)]
    public void GreaterOrEqualTo_WhenSmaller_ReturnsFalse(decimal value)
    {
        // Given
        var money = new Money(value, _usd);
        var other = new Money(1234, _usd);

        // When
        var result = money.GreaterOrEqualTo(other);

        // Then
        Assert.False(result);
    }

    [Theory]
    [InlineData(1234)]
    [InlineData(1234.01)]
    public void GreaterOrEqualTo_WhenEqualOrGreater_ReturnsTrue(decimal value)
    {
        // Given
        var money = new Money(value, _usd);
        var other = new Money(1234, _usd);

        // When
        var result = money.GreaterOrEqualTo(other);

        // Then
        Assert.True(result);
    }

    [Fact]
    public void GreaterOrEqualTo_WithDifferentCurrency_Throws()
    {
        // Given
        var money = new Money(1234, _usd);
        var other = new Money(1, Currency.FromCode("BTC"));

        // When / Then
        Assert.Throws<CurrencyMismatchException>(() => money.GreaterOrEqualTo(other));
    }
}
