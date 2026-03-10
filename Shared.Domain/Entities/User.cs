using Shared.Domain.Enums;

namespace Shared.Domain.Entities;

public class User : BaseEntity<Guid>
{
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Login { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public UserStatus Status { get; private set; }
    public string Email { get; private set; } = null!;
    public AccessLevel AccessLevel { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public User(Guid companyId, string name, string login, string password, string email, AccessLevel accessLevel = AccessLevel.ReadOnly)
    {
        Id = Guid.NewGuid();
        CompanyId = companyId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Login = login ?? throw new ArgumentNullException(nameof(login));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        AccessLevel = accessLevel;
        Status = UserStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }
}
