using System;
using System.Collections.Generic;
using Shared.Domain.Enums;

namespace Shared.Domain.Entities;

public class TransactionIngestionModel
{
    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public string ClientName { get; private set; } = null!;
    public IReadOnlyList<string> UserIds { get; private set; } = Array.Empty<string>();
    public string IdempotencyKey { get; private set; } = null!;
    public MovementEventType EventType { get; private set; }
    public DateTime ReceivedAt { get; private set; }

    private TransactionIngestionModel() { } // For EF/Serialization

    public TransactionIngestionModel(ClientContext clientContext, string idempotencyKey, MovementEventType eventType)
    {
        ArgumentNullException.ThrowIfNull(clientContext);

        Id = Guid.NewGuid();
        ClientId = clientContext.ClientId;
        ClientName = clientContext.ClientName;
        UserIds = clientContext.UserIds;
        IdempotencyKey = idempotencyKey ?? throw new ArgumentNullException(nameof(idempotencyKey));
        EventType = eventType;
        ReceivedAt = DateTime.UtcNow;
    }
}
