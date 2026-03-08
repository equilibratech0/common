using Shared.Domain.Enums;

namespace Shared.Domain.Events;

public class TransactionReceivedEvent
{
    public Guid TransactionId { get; }
    public Guid CompanyId { get; }
    public MovementEventType EventType { get; }
    public string RawPayload { get; }
    public DateTime OccurredOn { get; }

    public TransactionReceivedEvent(
        Guid transactionId,
        Guid companyId,
        MovementEventType eventType,
        string rawPayload)
    {
        TransactionId = transactionId;
        CompanyId = companyId;
        EventType = eventType;
        RawPayload = rawPayload;
        OccurredOn = DateTime.UtcNow;
    }
}
