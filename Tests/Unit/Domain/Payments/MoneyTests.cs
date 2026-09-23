using Domain.Payments;

namespace Tests.Unit.Domain.Payments;

public class MoneyTests
{
    private readonly Currency usd = new("USD", 2);

    [Theory]
    [InlineData(0)]
    [InlineData(-0.0000000001)]
    public void Money_Amount_MustBeGreaterThanZero(decimal amount)
    {
        // When / Then
        Assert.Throws<ArgumentOutOfRangeException>(() => new Money
        {
            Amount = amount,
            Currency = usd
        });
    }

    [Theory]
    [InlineData(1233.99)]
    [InlineData(1233)]
    public void GreaterOrEqualTo_WhenSmaller_ReturnsFalse(decimal value)
    {
        // Given
        var money = new Money { Amount = value, Currency = usd };
        var other = new Money { Amount = 1234, Currency = usd };

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
        var money = new Money { Amount = value, Currency = usd };
        var other = new Money { Amount = 1234, Currency = usd };

        // When
        var result = money.GreaterOrEqualTo(other);

        // Then
        Assert.True(result);
    }

    [Fact]
    public void GreaterOrEqualTo_WithDifferentCurrency_Throws()
    {
        // Given
        var money = new Money { Amount = 1234, Currency = usd };
        var other = new Money { Amount = 1, Currency = new Currency("BTC", 8) };

        // When / Then
        Assert.Throws<ArgumentException>(() => money.GreaterOrEqualTo(other));
    }
}
