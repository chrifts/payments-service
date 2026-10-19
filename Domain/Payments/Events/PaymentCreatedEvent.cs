using Domain.Shared.Events;

namespace Domain.Payments.Events;

public sealed record PaymentCreatedEvent(PaymentId PaymentId, DateTimeOffset OccurredAt) : IDomainEvent;
