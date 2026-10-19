using Domain.Payments;

namespace Application.Payments.GetPayment;

public class GetPaymentHandler(IPaymentRepository paymentRepository)
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;

    // Null when the payment doesn't exist: not finding it is an expected outcome, not an error
    public async Task<PaymentResponse?> HandleAsync(
        GetPaymentQuery query,
        CancellationToken cancellationToken
    )
    {
        Payment? payment = await _paymentRepository.GetByIdAsync(new PaymentId(query.PaymentId), cancellationToken);
        return payment is null ? null : PaymentResponse.FromDomain(payment);
    }
}
