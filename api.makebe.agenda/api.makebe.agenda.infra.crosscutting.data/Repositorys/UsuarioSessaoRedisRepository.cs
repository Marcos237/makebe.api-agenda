using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class UsuarioSessaoRedisRepository : IUsuarioSessaoRedisRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ConnectionMultiplexer _connection;
        public UsuarioSessaoRedisRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = ConnectionMultiplexer.Connect(_configuration["connectionRedis"] ?? string.Empty);
        }
        public async Task<bool> AtualizarExpiracaoSessao(UsuarioSessao sessaoUsuario, string chave)
        {
            var novaDataExpiracao = DateTime.Now.AddMinutes(25);
            var database = _connection.GetDatabase();
            var chaveRedis = $"sessao:{chave}";
            string novaDataExpiracaoFormatada = novaDataExpiracao.ToString("yyyy-MM-ddTHH:mm:ss");
            return await database.KeyExpireAsync(chaveRedis, novaDataExpiracao);
        }
        public async Task<HashEntry[]> BuscarSessao(string usuarioId)
        {
            var database = _connection.GetDatabase();
            var chaveRedis = $"sessao:{usuarioId}";
            var sessao = await database.HashGetAllAsync(chaveRedis);
            return sessao;
        }
    }
}
