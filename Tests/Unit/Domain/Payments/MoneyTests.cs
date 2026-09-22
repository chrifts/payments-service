using Domain.Payments;

namespace Tests.Unit.Domain.Payments;

public class MoneyTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-0.0000000001)]
    public void Money_Amount_MustBeGreaterThanZero(decimal amount)
    {
        // Given
        // When
        // Then
        Assert.Throws<ArgumentOutOfRangeException>(() => new Money
        {
            Amount = amount,
            Currency = new Currency("USD", 2)
        });
    }
}