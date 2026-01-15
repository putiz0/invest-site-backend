using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InvestSite.API.Models;

public class Acao
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Nome { get; set;} = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public double DividendosAnuais { get; set; }
    public double LPA { get; set; }
    public double VPA { get; set; }
}
