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

    // 🔹 LISTAR TODOS
    public async Task<List<Fii>> GetAllAsync()
    {
        return await _fiis.Find(_ => true).ToListAsync();
    }

    // 🔹 CRIAR
    public async Task<Fii> CreateAsync(Fii fii)
    {
        await _fiis.InsertOneAsync(fii);
        return fii;
    }

    // 🔹 RANKING (DY + P/VP)
    public async Task<List<object>> GetRankingAsync()
    {
        var fiis = await _fiis.Find(_ => true).ToListAsync();

        if (!fiis.Any())
            return new List<object>();

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
