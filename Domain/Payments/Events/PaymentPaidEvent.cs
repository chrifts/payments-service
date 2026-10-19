using Domain.Shared.Events;

namespace Domain.Payments.Events;

public sealed record PaymentPaidEvent(PaymentId PaymentId, DateTimeOffset OccurredAt) : IDomainEvent;
