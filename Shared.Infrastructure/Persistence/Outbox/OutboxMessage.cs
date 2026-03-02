namespace Shared.Infrastructure.Persistence.Outbox;

/// <summary>
/// Infrastructure entity to hold integration events before they are published to the message broker.
/// This ensures the Outbox Pattern is followed for eventual consistency.
/// </summary>
public class OutboxMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public string EventType { get; init; } = string.Empty;
    
    // The serialized Integration Event payload (usually JSON)
    public string Payload { get; init; } = string.Empty;
    
    public DateTimeOffset OccurredOn { get; init; } = DateTimeOffset.UtcNow;
    
    public DateTimeOffset? ProcessedOn { get; set; }
    
    public string? Error { get; set; }
}
