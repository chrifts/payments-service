using Domain.Payments;

namespace Tests.Unit.Domain.Payments;

public class CurrencyTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Code_MustNotBeEmpty(string? code)
    {
        // Given
        // When
        // Then
        Assert.ThrowsAny<ArgumentException>(() => new Currency(code!, 2));
    }

    [Fact]
    public void Precision_MustNotBeNegative()
    {
        // Given
        // When
        // Then
        Assert.Throws<ArgumentOutOfRangeException>(() => new Currency("USD", -1));
    }
}