using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IUsuarioSessaoDomainService
    {
        Task<UsuarioSessao> BuscarSessao(string sessao);
        Task<bool> AtualizarSessao(UsuarioSessao sessao, string chave);
    }
}
