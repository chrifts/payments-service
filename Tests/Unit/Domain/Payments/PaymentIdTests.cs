using Domain.Payments;
using Domain.Payments.Exceptions;

namespace Tests.Unit.Domain.Payments;

public class PaymentIdTests
{
    [Fact]
    public void New_GeneratesNonEmptyId()
    {
        // Given
        PaymentId paymentId;
        // When
        paymentId = PaymentId.New();
        // Then
        Assert.NotEqual(Guid.Empty, paymentId.Value);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_Throws()
    {
        // When / Then
        Assert.Throws<InvalidPaymentIdException>(() => new PaymentId(Guid.Empty));
    }

    [Fact]
    public void New_CreatesUniqueIds()
    {
        // Given
        PaymentId paymentId;
        PaymentId otherPaymentId;
        // When
        paymentId = PaymentId.New();
        otherPaymentId = PaymentId.New();
        // Then
        Assert.NotEqual(paymentId, otherPaymentId);
    }

    [Fact]
    public void Equals_WithSameGuid_ReturnsTrue()
    {
        // Given
        Guid guid = Guid.CreateVersion7();
        PaymentId paymentId = new(guid);
        PaymentId otherReferenceButSameValue = new(guid);
        // Then
        Assert.Equal(paymentId, otherReferenceButSameValue);
    }
}
