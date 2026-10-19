using Domain.Shared.Exceptions;

namespace Domain.Payments.Exceptions;

public sealed class InvalidPaymentIdException() : DomainException("Payment id cannot be empty.");
