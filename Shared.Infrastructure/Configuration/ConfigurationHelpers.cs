namespace Shared.Infrastructure.Configuration;

using Microsoft.Extensions.Configuration;

public static class ConfigurationHelpers
{
    public static T GetConfigValue<T>(this IConfiguration configuration, string key)
    {
        var value = configuration.GetValue<T>(key);
        if (value == null)
        {
            throw new ArgumentNullException(key, $"Configuration key '{key}' is missing or invalid.");
        }
        return value;
    }

    public static T GetConfigOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }
}
