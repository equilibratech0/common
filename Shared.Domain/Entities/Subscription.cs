using Shared.Domain.Enums;

namespace Shared.Domain.Entities;

public class Subscription : BaseEntity<Guid>
{
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    public SubscriptionStatus Status { get; private set; }
    public decimal Rate { get; private set; }
    public int Limits { get; private set; }
    public decimal PlatformFee { get; private set; }
    public SubscriptionPlan Plan { get; private set; }
    public int UsersMax { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }

    private Subscription() { }

    public Subscription(Guid companyId, string name, SubscriptionPlan plan, decimal rate, int limits, decimal platformFee, int usersMax)
    {
        Id = Guid.NewGuid();
        CompanyId = companyId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Plan = plan;
        Rate = rate;
        Limits = limits;
        PlatformFee = platformFee;
        UsersMax = usersMax;
        Status = SubscriptionStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string? name, SubscriptionPlan? plan, SubscriptionStatus? status, decimal? rate, int? limits, decimal? platformFee, int? usersMax)
    {
        if (name is not null) Name = name;
        if (plan.HasValue) Plan = plan.Value;
        if (status.HasValue) Status = status.Value;
        if (rate.HasValue) Rate = rate.Value;
        if (limits.HasValue) Limits = limits.Value;
        if (platformFee.HasValue) PlatformFee = platformFee.Value;
        if (usersMax.HasValue) UsersMax = usersMax.Value;
        ModifiedAt = DateTime.UtcNow;
    }
}
