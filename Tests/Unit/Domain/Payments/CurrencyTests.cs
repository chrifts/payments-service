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
        Assert.ThrowsAny<ArgumentException>(() => Currency.FromCode(code!));
    }

    [Theory]
    [InlineData("EUR", 2)]
    [InlineData("JPY", 0)]
    [InlineData("BTC", 8)]
    [InlineData("CLP", 0)]
    public void FromCode_WithKnownCode_ReturnsCurrencyWithPrecision(string code, int expectedPrecision)
    {
        // Given
        // When
        var currency = Currency.FromCode(code);
        // Then
        Assert.Equal(expectedPrecision, currency.Precision);
    }

    [Fact]
    public void FromCode_WithLowercaseCode_NormalizesToUppercase()
    {
        // Given
        // When
        var currency = Currency.FromCode("eur");
        // Then
        Assert.Equal("EUR", currency.Code);
        Assert.Equal(2, currency.Precision);
    }

    [Fact]
    public void FromCode_WithUnknownCode_Throws()
    {
        // Given
        // When
        // Then
        Assert.Throws<ArgumentException>(() => Currency.FromCode("xyz"));
    }
}