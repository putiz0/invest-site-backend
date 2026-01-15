using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InvestSite.API.Models
{
    public class Fii
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;

        public decimal Preco { get; set; }

        // Dividend Yield (quanto maior melhor)
        public decimal DividendYield { get; set; }

        // P/VP (quanto menor melhor)
        public decimal PVP { get; set; }
    }
}
