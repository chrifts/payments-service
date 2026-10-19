using Domain.Payments.Events;
using Domain.Payments.PaymentMethods;
using Domain.Shared.Events;

namespace Domain.Payments;

public class Payment : IHasDomainEvents
{
    public PaymentId Id { get; } = PaymentId.New();
    public Money AmountDue { get; }
    public PaymentMethod Method { get; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public bool IsPaid => Status == PaymentStatus.Paid;
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? PaidAt { get; private set; }
    private readonly List<IDomainEvent> _events = [];
    public IReadOnlyList<IDomainEvent> Events => _events;

    // Used by EF Core to materialize from the database
    private Payment()
    {
        AmountDue = null!;
        Method = null!;
    }

    private Payment(Money amountDue, PaymentMethod method, DateTimeOffset createdAt)
    {
        AmountDue = amountDue;
        Method = method;
        CreatedAt = createdAt;
    }

    public void Pay(TimeProvider timeProvider)
    {
        if (IsPaid)
        {
            return;
        }

        Method.EnsureCanPay(AmountDue);

        DateTimeOffset paidAt = timeProvider.GetUtcNow();
        Status = PaymentStatus.Paid;
        PaidAt = paidAt;

        _events.Add(new PaymentPaidEvent(Id, paidAt));
    }

    public void ClearEvents()
    {
        _events.Clear();
    }

    public static Payment Create(Money amountDue, PaymentMethod method, TimeProvider timeProvider)
    {
        Payment payment = new(amountDue, method, timeProvider.GetUtcNow());
        payment._events.Add(new PaymentCreatedEvent(payment.Id, payment.CreatedAt));
        return payment;
    }
}