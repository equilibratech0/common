using Shared.Domain.Enums;

namespace Shared.Domain.Entities;

public class UserAccount
{
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public AccessLevel AccessLevel { get; private set; }

    private UserAccount() { }

    public UserAccount(Guid userId, Guid accountId, AccessLevel accessLevel)
    {
        UserId = userId;
        AccountId = accountId;
        AccessLevel = accessLevel;
    }
}
