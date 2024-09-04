using api.makebe.agenda.domain.Entidades;
using StackExchange.Redis;

namespace api.makebe.agenda.infra.data.Repositorys.Interfaces
{
    public interface IUsuarioSessaoRedisRepository
    {
        Task<HashEntry[]> BuscarSessao(string usuarioId);
        Task<bool> AtualizarExpiracaoSessao(UsuarioSessao sessaoUsuario, string chave);
    }
}
