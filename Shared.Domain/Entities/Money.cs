namespace Shared.Domain.Entities;

using Shared.Domain.Exceptions;

public record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    // Parameterless construct for ORM/Serialization ensuring non-null
    private Money() 
    { 
        Currency = string.Empty; 
    }

    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency cannot be empty.");
            
        if (currency.Length != 3)
            throw new DomainException("Currency must be a 3-letter ISO code.");

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Money Zero(string currency) => new(0, currency);

    public override string ToString() => $"{Amount} {Currency}";
}
