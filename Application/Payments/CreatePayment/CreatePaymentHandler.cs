using Domain.Payments;

namespace Application.Payments.CreatePayment;

public class CreatePaymentHandler(
    IPaymentRepository paymentRepository,
    TimeProvider timeProvider
)
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<CreatePaymentResponse> HandleAsync(
        CreatePaymentCommand command,
        CancellationToken cancellationToken
    )
    {
        Money amountDue = new()
        {
            Amount = command.Amount,
            Currency = Currency.FromCode(command.CurrencyCode)
        };

        Payment payment = Payment.Create(
            amountDue,
            command.PaymentMethod.ToDomain(amountDue.Currency),
            _timeProvider
        );

        await _paymentRepository.AddAsync(payment, cancellationToken);
        return new CreatePaymentResponse(payment.Id.Value);
    }
}
