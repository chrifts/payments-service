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
    private readonly List<IDomainEvent> _events = [];
    public IReadOnlyList<IDomainEvent> Events => _events;

    private Payment(Money amount, PaymentMethod method)
    {
        Amount = amount;
        Method = method;
    }

    public void Pay()
    {
        if (IsPaid)
        {
            return;
        }

        Method.EnsureCanPay(Amount);

        IsPaid = true;
        _events.Add(new PaymentPaidEvent());
    }

    public void ClearEvents()
    {
        _events.Clear();
    }

    public static Payment Create(Money amount, PaymentMethod method)
    {
        Payment payment = new(amount, method);
        payment._events.Add(new PaymentCreatedEvent());
        return payment;
    }
}