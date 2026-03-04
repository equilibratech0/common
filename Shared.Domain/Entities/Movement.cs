namespace Shared.Domain.Entities;

using Shared.Domain.Enums;
using Shared.Domain.Exceptions;

public class Movement : AggregateRoot<MovementId>
{
    public MovementEventType EventType { get; private set; }

    // Core financial details of the movement
    public Amount Amount { get; private set; }
    public string TransactionId { get; private set; } = null!;
    public string? AccountId { get; private set; }
    public string? Country { get; private set; }
    public PaymentMethodDetails? PaymentMethod { get; private set; }
    public MerchantDetails? Merchant { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string? Description { get; private set; }

    protected Movement() { }

    private Movement(MovementId id, MovementEventType eventType, Amount amount, string transactionId, string? accountId, string? country, PaymentMethodDetails? paymentMethod, MerchantDetails? merchant, string? description)
    {
        Id = id;
        EventType = eventType;
        Amount = amount;
        TransactionId = transactionId;
        AccountId = accountId;
        Country = country;
        PaymentMethod = paymentMethod;
        Merchant = merchant;
        Description = description;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Movement Create(MovementEventType eventType, Amount amount, string transactionId, string? accountId, string? country, PaymentMethodDetails? paymentMethod, MerchantDetails? merchant = null, string? description = null)
    {
        if (amount == null)
            throw new DomainException("Amount cannot be null.");

        return new Movement(MovementId.New(), eventType, amount, transactionId, accountId, country, paymentMethod, merchant, description);
    }
}
