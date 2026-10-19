using Domain.Payments;
using Domain.Shared;

namespace Application.Payments.CreatePayment;

public class CreatePaymentHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider
)
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<CreatePaymentResponse> HandleAsync(
        CreatePaymentCommand command,
        CancellationToken cancellationToken
    )
    {
        Money amountDue = new(command.AmountDue, Currency.FromCode(command.CurrencyCode));

        Payment payment = Payment.Create(
            amountDue,
            command.PaymentMethod.ToDomain(amountDue.Currency),
            _timeProvider
        );

        _paymentRepository.Add(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new CreatePaymentResponse(payment.Id.Value);
    }
}
