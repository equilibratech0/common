namespace Shared.Domain.Entities;

public record MerchantDetails
{
    public string? MerchantId { get; init; }
    public string? MerchantName { get; init; }

    public MerchantDetails() { }

    public MerchantDetails(string? merchantId, string? merchantName)
    {
        MerchantId = merchantId;
        MerchantName = merchantName;
    }
}
