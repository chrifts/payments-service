namespace Application.Payments.CreatePayment;

public record CreatePaymentCommand(
    decimal Amount,
    string CurrencyCode,
    PaymentMethodData PaymentMethod
);