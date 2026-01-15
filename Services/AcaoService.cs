using InvestSite.API.Models;
using MongoDB.Driver;

namespace InvestSite.API.Services
{
    public class AcaoService
    {
        private readonly IMongoCollection<Acao> _acoes;

        public AcaoService(MongoService mongo)
        {
            _acoes = mongo.Database.GetCollection<Acao>("acoes");
        }

        public async Task<List<Acao>> GetAllAsync() =>
            await _acoes.Find(_ => true).ToListAsync();

        public async Task<Acao> CreateAsync(Acao acao)
        {
            await _acoes.InsertOneAsync(acao);
            return acao;
        }
    }
}
