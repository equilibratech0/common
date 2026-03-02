namespace Shared.Domain.Entities;

using Shared.Domain.Enums;

public record PaymentMethodDetails
{
    public string PaymentMethodId { get; init; } = string.Empty;
    public string PaymentMethodName { get; init; } = string.Empty;
    public PaymentMethodType Type { get; init; }

    public PaymentMethodDetails() { }

    public PaymentMethodDetails(string paymentMethodId, string paymentMethodName, PaymentMethodType type)
    {
        PaymentMethodId = paymentMethodId;
        PaymentMethodName = paymentMethodName;
        Type = type;
    }
}
