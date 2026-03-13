namespace Shared.Domain.Entities;

public class Account : BaseEntity<Guid>
{
    public Guid CompanyId { get; private set; }
    public string AccountReference { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private Account() { }

    public Account(Guid companyId, string accountReference, string name)
    {
        Id = Guid.NewGuid();
        CompanyId = companyId;
        AccountReference = accountReference ?? throw new ArgumentNullException(nameof(accountReference));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        CreatedAt = DateTime.UtcNow;
    }
}
