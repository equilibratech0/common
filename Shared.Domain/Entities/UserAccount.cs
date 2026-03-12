namespace Shared.Domain.Entities;

public class UserAccount
{
    public Guid UserId { get; private set; }
    public string AccountId { get; private set; } = null!;

    private UserAccount() { }

    public UserAccount(Guid userId, string accountId)
    {
        UserId = userId;
        AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
    }
}
