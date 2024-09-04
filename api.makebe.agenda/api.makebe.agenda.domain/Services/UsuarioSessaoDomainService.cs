using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Services.Interfaces;

namespace api.makebe.agenda.domain.Services
{
    public class UsuarioSessaoDomainService : IUsuarioSessaoDomainService
    {
        public async Task<bool> AtualizarSessao(UsuarioSessao sessao, string chave)
        {
            var retorno = await _usuarioSessaoRedis.AtualizarExpiracaoSessao(sessao, chave);
            return retorno;
        }

        public async Task<UsuarioSessao> BuscarSessao(string sessao)
        {

            var restornoSessao = await _usuarioSessaoRedis.BuscarSessao(sessao);
            var sessaoHash = new SessaoUsuario();
            var sessaoUsuario = await sessaoHash.TranformarHasEntriesSessao(restornoSessao);
            return sessaoUsuario;
        }
    }
}
