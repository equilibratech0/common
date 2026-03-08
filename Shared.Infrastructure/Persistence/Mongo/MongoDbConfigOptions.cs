namespace Shared.Infrastructure.Persistence.Mongo;

public class MongoDbConfigOptions
{
    public const string SectionName = "MongoDbConfig";

    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}
