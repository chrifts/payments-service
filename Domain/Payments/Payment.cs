using Domain.Payments.Events;
using Domain.Payments.PaymentMethods;
using Domain.Shared.Events;

namespace Domain.Payments;

public class Payment
{
    public PaymentId Id { get; } = PaymentId.New();
    public Money Amount { get; }
    public PaymentMethod Method { get; }
    public bool IsPaid { get; private set; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? PaidAt { get; private set; }
    private readonly List<IDomainEvent> _events = [];
    public IReadOnlyList<IDomainEvent> Events => _events;

    private Payment(Money amount, PaymentMethod method, DateTimeOffset createdAt)
    {
        Amount = amount;
        Method = method;
        CreatedAt = createdAt;
    }

    public void Pay(TimeProvider timeProvider)
    {
        if (IsPaid)
        {
            return;
        }

        Method.EnsureCanPay(Amount);

        IsPaid = true;
        PaidAt = timeProvider.GetUtcNow();

        _events.Add(new PaymentPaidEvent());
    }

    public void ClearEvents()
    {
        _events.Clear();
    }

    public static Payment Create(Money amount, PaymentMethod method, TimeProvider timeProvider)
    {
        Payment payment = new(amount, method, timeProvider.GetUtcNow());
        payment._events.Add(new PaymentCreatedEvent());
        return payment;
    }
}