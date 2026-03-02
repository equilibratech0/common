using System;
using Shared.Domain.Enums;

namespace Shared.Domain.Events;

public class TransactionReceivedEvent
{
    public Guid TransactionId { get; }
    public string IdempotencyKey { get; }
    public MovementEventType EventType { get; }
    public string RawPayload { get; }
    public DateTime OccurredOn { get; }

    public TransactionReceivedEvent(
        Guid transactionId,
        string idempotencyKey,
        MovementEventType eventType,
        string rawPayload)
    {
        TransactionId = transactionId;
        IdempotencyKey = idempotencyKey;
        EventType = eventType;
        RawPayload = rawPayload;
        OccurredOn = DateTime.UtcNow;
    }
}
