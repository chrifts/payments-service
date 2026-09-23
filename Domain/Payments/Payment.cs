using Domain.Payments.Events;
using Domain.Payments.PaymentMethods;
using Domain.Shared.Events;

namespace Domain.Payments;

public class Payment(Money amount, PaymentMethod method)
{
    public readonly Money Amount = amount;
    public readonly PaymentMethod Method = method;
    private bool Paid = false;
    private readonly List<IDomainEvent> _events = [];
    public IReadOnlyList<IDomainEvent> Events => _events;

    public void Pay()
    {
        if (IsPaid())
        {
            return;
        }

        Method.EnsureCanPay(Amount);

        Paid = true;
        _events.Add(new PaymentPaidEvent());
    }

    public bool IsPaid()
    {
        return Paid;
    }

    public void ClearEvents()
    {
        _events.Clear();
    }
}