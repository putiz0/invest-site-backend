using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InvestSite.API.Models;

public class Fii
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string Ticker { get; set; } = "";
    public double DividendYield { get; set; }
    public double PVP { get; set; }
}
