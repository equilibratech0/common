namespace Shared.Domain.Entities;

public record MerchantDetails
{
    public string? MerchantId { get; init; }
    public string? MerchantName { get; init; }
    public ShopDetails? Shop { get; init; }

    public MerchantDetails() { }

    public MerchantDetails(string? merchantId, string? merchantName, ShopDetails? shop = null)
    {
        MerchantId = merchantId;
        MerchantName = merchantName;
        Shop = shop;
    }
}

public record ShopDetails
{
    public string? ShopId { get; init; }
    public string? ShopName { get; init; }

    public ShopDetails() { }

    public ShopDetails(string? shopId, string? shopName)
    {
        ShopId = shopId;
        ShopName = shopName;
    }
}
