namespace Application.Payments.CreatePayment;

public record CreatePaymentCommand(
    decimal AmountDue,
    string CurrencyCode,
    PaymentMethodData PaymentMethod
);