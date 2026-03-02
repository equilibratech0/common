namespace Shared.Domain.Entities;

using Shared.Domain.Types;

public record MovementId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static MovementId New() => new(Guid.NewGuid());
}
