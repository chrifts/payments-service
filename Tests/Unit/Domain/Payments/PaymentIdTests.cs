using Domain.Payments;

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
        // Given
        // When
        // Then
        Assert.Throws<ArgumentException>(() => new PaymentId(Guid.Empty));
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
    public void TwoIds_WithSameGuid_AreEqual()
    {
        // Given
        Guid guid = Guid.CreateVersion7();
        PaymentId paymentId = new(guid);
        PaymentId otherReferenceButSameValue = new(guid);
        // When
        // Then
        Assert.Equal(paymentId, otherReferenceButSameValue);
    }
}
