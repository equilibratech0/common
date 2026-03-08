using Shared.Domain.Enums;

namespace Shared.Domain.Entities;

public class Company : BaseEntity<Guid>
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    public CompanyStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Company() { }

    public Company(Guid id, string name, string email, string country)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        Status = CompanyStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }
}
