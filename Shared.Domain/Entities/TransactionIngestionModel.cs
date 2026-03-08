namespace Shared.Domain.Entities;

public class TransactionIngestionModel
{
    public Guid Id { get; private set; }
    public string IdempotencyKey { get; private set; } = null!;
    public DateTime ReceivedAt { get; private set; }

    private TransactionIngestionModel() { }

    public TransactionIngestionModel(string idempotencyKey)
    {
        Id = Guid.NewGuid();
        IdempotencyKey = idempotencyKey ?? throw new ArgumentNullException(nameof(idempotencyKey));
        ReceivedAt = DateTime.UtcNow;
    }
}
