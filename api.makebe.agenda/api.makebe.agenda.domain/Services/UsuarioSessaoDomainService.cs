using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enum;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;

namespace api.makebe.agenda.domain.Services
{
    public class UsuarioSessaoDomainService : IUsuarioSessaoDomainService
    {
        private readonly IUsuarioSessaoRedisRepository _usuarioSessaoRedisRepository;
        public UsuarioSessaoDomainService(IUsuarioSessaoRedisRepository usuarioSessaoRedisRepository)
        {
            _usuarioSessaoRedisRepository = usuarioSessaoRedisRepository;
        }
        public async Task<bool> AtualizarSessao(UsuarioSessao sessao, string chave)
        {
            var retorno = await _usuarioSessaoRedisRepository.AtualizarExpiracaoSessao(sessao, chave);
            return retorno;
        }

        public async Task<UsuarioSessao> BuscarSessao(string sessao)
        {

            var restornoSessao = await _usuarioSessaoRedisRepository.BuscarSessao(sessao);
            var sessaoHash = new UsuarioSessao();
            var sessaoUsuario = await sessaoHash.TranformarHasEntriesSessao(restornoSessao);
            return sessaoUsuario;
        }
    }
}
