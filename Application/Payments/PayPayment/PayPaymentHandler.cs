using Domain.Payments;
using Domain.Shared;

namespace Application.Payments.PayPayment;

public class PayPaymentHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider
)
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TimeProvider _timeProvider = timeProvider;

    // Null when the payment doesn't exist. The payment rules live in Payment.Pay, not here
    public async Task<PaymentResponse?> HandleAsync(
        PayPaymentCommand command,
        CancellationToken cancellationToken
    )
    {
        Payment? payment = await _paymentRepository.GetByIdAsync(new PaymentId(command.PaymentId), cancellationToken);
        if (payment is null)
        {
            return null;
        }

        payment.Pay(_timeProvider);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return PaymentResponse.FromDomain(payment);
    }
}
