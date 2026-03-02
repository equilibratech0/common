namespace Shared.Domain.Events;

using Shared.Domain.Entities;
using Shared.Domain.Enums;

public abstract record DomainEventBase(
    MovementId MovementId,
    MovementEventType EventType,
    bool IsSuccess = true) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    
    public MovementId MovementId { get; init; } = MovementId;
    
    public MovementEventType EventType { get; init; } = EventType;
    
    public bool IsSuccess { get; init; } = IsSuccess;
    
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
