namespace Domain.Payments;

// A collection of payments: it doesn't save, IUnitOfWork does
public interface IPaymentRepository
{
    void Add(Payment payment);
    Task<Payment?> GetByIdAsync(PaymentId id, CancellationToken cancellationToken);
}
