namespace Domain.Shared.Exceptions;

// Base for broken business rules, so callers can tell them apart from bugs
public abstract class DomainException(string message) : Exception(message);
