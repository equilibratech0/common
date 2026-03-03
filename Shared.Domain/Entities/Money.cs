namespace Shared.Domain.Entities;

using Shared.Domain.Exceptions;
using Shared.Domain.Enums;

public record Money
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }

    // Parameterless construct for ORM/Serialization
    private Money()
    {
    }

    public Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Zero(Currency currency) => new(0, currency);

    public override string ToString() => $"{Amount} {Currency}";
}
