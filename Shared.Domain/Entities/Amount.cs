namespace Shared.Domain.Entities;

using Shared.Domain.Enums;

public record Amount
{
    public decimal TotalAmount { get; init; }
    public Currency Currency { get; init; }
    public decimal? GrossAmount { get; init; }
    public decimal? NetAmount { get; init; }
    public decimal? PaymentFee { get; init; }

    private Amount() { }

    public Amount(decimal totalAmount, Currency currency, decimal? grossAmount, decimal? netAmount, decimal? paymentFee)
    {
        TotalAmount = totalAmount;
        Currency = currency;
        GrossAmount = grossAmount;
        NetAmount = netAmount;
        PaymentFee = paymentFee;
    }

    public override string ToString() => $"Total={TotalAmount} {Currency}, Gross={GrossAmount}, Net={NetAmount}, PaymentFee={PaymentFee}";
}
