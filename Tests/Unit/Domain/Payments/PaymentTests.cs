using Domain.Payments;

namespace Tests.Unit.Domain.Payments;

public class PaymentTest
{
    [Fact]
    public void Constructor_SetsAmount()
    {
        // Given
        // When
        Money money = new() { Amount = 2134 };
        Payment payment = new(money);

        // Then
        Assert.Equal(2134, payment.Amount.Amount);
    }
}