using System;
using Shared.Domain.Enums;

namespace Shared.Domain.Entities;

public class TransactionIngestionModel
{
    public Guid Id { get; private set; }
    public string IdempotencyKey { get; private set; } = null!;
    public MovementEventType EventType { get; private set; }
    public DateTime ReceivedAt { get; private set; }

    private TransactionIngestionModel() { } // For EF/Serialization

    public TransactionIngestionModel(string idempotencyKey, MovementEventType eventType)
    {
        Id = Guid.NewGuid();
        IdempotencyKey = idempotencyKey ?? throw new ArgumentNullException(nameof(idempotencyKey));
        EventType = eventType;
        ReceivedAt = DateTime.UtcNow;
    }
}
