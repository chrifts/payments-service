using Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Payments;

public class PaymentRepository(PaymentsDbContext context) : IPaymentRepository
{
    public async Task AddAsync(Payment payment, CancellationToken cancellationToken)
    {
        context.Payments.Add(payment);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Payment?> GetByIdAsync(PaymentId id, CancellationToken cancellationToken)
    {
        return await context.Payments.FirstOrDefaultAsync(payment => payment.Id == id, cancellationToken);
    }
}
