using InvestSite.API.Models;
using MongoDB.Driver;

namespace InvestSite.API.Services;

public class FiiService
{
    private readonly IMongoCollection<Fii> _fiis;

    public FiiService(MongoService mongo)
    {
        _fiis = mongo.Database.GetCollection<Fii>("fiis");
    }

    public List<object> GetRanking()
    {
        var fiis = _fiis.Find(_ => true).ToList();

        var maxDY = fiis.Max(f => f.DividendYield);
        var minPVP = fiis.Min(f => f.PVP);

        return fiis
            .Select(f => new
            {
                f.Ticker,
                f.DividendYield,
                f.PVP,
                Score = (f.DividendYield / maxDY) + (minPVP / f.PVP)
            })
            .OrderByDescending(f => f.Score)
            .Select((f, index) => new
            {
                Rank = index + 1,
                f.Ticker,
                f.DividendYield,
                f.PVP,
                f.Score
            })
            .ToList<object>();
    }
}
