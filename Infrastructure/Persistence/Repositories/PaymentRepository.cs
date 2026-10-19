using Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PaymentRepository(PaymentsDbContext context) : IPaymentRepository
{
    // Only registers the payment: it's inserted when the unit of work saves
    public void Add(Payment payment)
    {
        context.Payments.Add(payment);
    }

    public async Task<Payment?> GetByIdAsync(PaymentId id, CancellationToken cancellationToken)
    {
        return await context.Payments.FirstOrDefaultAsync(payment => payment.Id == id, cancellationToken);
    }
}
