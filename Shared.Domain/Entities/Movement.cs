namespace Shared.Domain.Entities;

using Shared.Domain.Enums;
using Shared.Domain.Exceptions;

public class Movement : AggregateRoot<MovementId>
{
    public MovementEventType EventType { get; private set; }
    
    // Core financial details of the movement
    public Money Amount { get; private set; }
    public string ReferenceId { get; private set; }
    public string AccountId { get; private set; }
    public string Country { get; private set; }
    public PaymentMethodDetails PaymentMethod { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string? Description { get; private set; }

    protected Movement() { }

    private Movement(MovementId id, MovementEventType eventType, Money amount, string referenceId, string accountId, string country, PaymentMethodDetails paymentMethod, string? description)
    {
        Id = id;
        EventType = eventType;
        Amount = amount;
        ReferenceId = referenceId;
        AccountId = accountId;
        Country = country;
        PaymentMethod = paymentMethod;
        Description = description;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Movement Create(MovementEventType eventType, Money amount, string referenceId, string accountId, string country, PaymentMethodDetails paymentMethod, string? description = null)
    {
        if (amount == null)
            throw new DomainException("Amount cannot be null.");

        if (string.IsNullOrWhiteSpace(referenceId))
            throw new DomainException("ReferenceId cannot be empty.");

        if (string.IsNullOrWhiteSpace(accountId))
            throw new DomainException("AccountId cannot be empty.");

        if (string.IsNullOrWhiteSpace(country))
            throw new DomainException("Country cannot be empty.");

        if (paymentMethod == null)
            throw new DomainException("PaymentMethodDetails cannot be null.");

        return new Movement(MovementId.New(), eventType, amount, referenceId, accountId, country, paymentMethod, description);
    }
}
