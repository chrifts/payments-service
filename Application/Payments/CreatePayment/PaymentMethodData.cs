using Domain.Payments;
using Domain.Payments.PaymentMethods;

namespace Application.Payments.CreatePayment;

public abstract record PaymentMethodData
{
    public abstract PaymentMethod ToDomain(Currency currency);
}
