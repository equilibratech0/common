namespace Shared.Domain.Types;

public abstract record StronglyTypedId<TValue>(TValue Value)
{
    public TValue Value { get; init; } = Value;

    public override string ToString() => Value?.ToString() ?? string.Empty;
}
