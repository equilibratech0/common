namespace Shared.Domain.Entities;

public class Account : BaseEntity<Guid>
{
    public Guid CompanyId { get; private set; }
    public string AccountId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private Account() { }

    public Account(Guid companyId, string accountId, string name)
    {
        Id = Guid.NewGuid();
        CompanyId = companyId;
        AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        CreatedAt = DateTime.UtcNow;
    }
}
