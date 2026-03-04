namespace Shared.Domain.Entities;

using Shared.Domain.Enums;

public record PaymentMethodDetails
{
    public string? PaymentMethodId { get; init; }
    public string? ProviderName { get; init; }
    public PaymentMethodType? Type { get; init; }

    public PaymentMethodDetails() { }

    public PaymentMethodDetails(string? paymentMethodId, string? providerName, PaymentMethodType? type)
    {
        PaymentMethodId = paymentMethodId;
        ProviderName = providerName;
        Type = type;
    }
}
