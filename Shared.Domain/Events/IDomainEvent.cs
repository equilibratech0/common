namespace Shared.Domain.Events;

using Shared.Domain.Entities;
using Shared.Domain.Enums;

public interface IDomainEvent
{
    Guid EventId { get; }
    
    // Correlative ID linking this event to its aggregate root
    MovementId MovementId { get; }
    
    // The specific type of movement event that occurred
    MovementEventType EventType { get; }
    
    // Indicates if the outcome of the action was successful or not
    bool IsSuccess { get; }
    
    DateTimeOffset OccurredOn { get; }
}
