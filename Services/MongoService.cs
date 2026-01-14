using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace InvestSite.API.Services;

public class MongoService
{
    public IMongoDatabase Database { get; }

    public MongoService(IOptions<MongoSettings> options)
    {
        var settings = options.Value;

        if (string.IsNullOrEmpty(settings.ConnectionString))
            throw new Exception("MongoDB ConnectionString não configurada.");

        var client = new MongoClient(settings.ConnectionString);
        Database = client.GetDatabase(settings.DatabaseName);
    }
}
