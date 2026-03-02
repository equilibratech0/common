namespace Shared.Infrastructure.Http;

using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

public abstract class BaseHttpClient
{
    protected readonly HttpClient HttpClient;
    protected readonly ILogger Logger;

    protected BaseHttpClient(HttpClient httpClient, ILogger logger)
    {
        HttpClient = httpClient;
        Logger = logger;
    }

    protected async Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await HttpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Logger.LogError(ex, "HTTP GET request failed for URL: {Url}", url);
            throw;
        }
    }

    protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest requestPayload, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await HttpClient.PostAsJsonAsync(url, requestPayload, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Logger.LogError(ex, "HTTP POST request failed for URL: {Url}", url);
            throw;
        }
    }
}
