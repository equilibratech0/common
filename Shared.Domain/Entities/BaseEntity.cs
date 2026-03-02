namespace Shared.Domain.Entities;

public abstract class BaseEntity<TId>
{
    public TId Id { get; protected set; } = default!;

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (Id is null || other.Id is null || Id.Equals(default(TId)))
            return false;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(BaseEntity<TId>? a, BaseEntity<TId>? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity<TId>? a, BaseEntity<TId>? b)
        => !(a == b);

    public override int GetHashCode()
        => (GetType().GetHashCode() * 907) + (Id?.GetHashCode() ?? 0);
}
