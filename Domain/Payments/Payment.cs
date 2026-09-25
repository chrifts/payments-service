using Domain.Payments.Events;
using Domain.Payments.PaymentMethods;
using Domain.Shared.Events;

namespace Domain.Payments;

public class Payment(Money amount, PaymentMethod method)
{
    public Money Amount { get; } = amount;
    public PaymentMethod Method { get; } = method;
    public bool IsPaid { get; private set; }
    private readonly List<IDomainEvent> _events = [];
    public IReadOnlyList<IDomainEvent> Events => _events;

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
}