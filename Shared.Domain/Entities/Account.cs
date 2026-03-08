namespace Shared.Domain.Entities;

public class Account : BaseEntity<Guid>
{
    public Guid CompanyId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Account() { }

    public Account(Guid companyId)
    {
        Id = Guid.NewGuid();
        CompanyId = companyId;
        CreatedAt = DateTime.UtcNow;
    }
}
