using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class UsuarioPermissaoDomainService : IUsuarioPermissaoDomainService
    {
        private static readonly HashSet<string> PermissoesPrivilegiadas = new(StringComparer.OrdinalIgnoreCase)
        {
            "Administrador",
            "Gestor"
        };

        private readonly IUsuarioAutenticadoService _usuarioAutenticadoService;
        private readonly IPermissaoPapelRepository _permissaoPapelRepository;

        public UsuarioPermissaoDomainService(
            IUsuarioAutenticadoService usuarioAutenticadoService,
            IPermissaoPapelRepository permissaoPapelRepository)
        {
            _usuarioAutenticadoService = usuarioAutenticadoService;
            _permissaoPapelRepository = permissaoPapelRepository;
        }

        public async Task<UsuarioAutenticadoDTO> BuscarUsuarioAutenticado()
        {
            return await _usuarioAutenticadoService.BuscarUsuarioAutenticado();
        }

        public async Task<bool> PossuiAcessoCompletoConta()
        {
            var usuarioAutenticado = await BuscarUsuarioAutenticado();
            if (usuarioAutenticado.PermissaoId == Guid.Empty)
                return false;

            var permissaoPapel = await _permissaoPapelRepository.BuscarPorPermissaoId(usuarioAutenticado.PermissaoId);
            var descricaoPermissao = permissaoPapel?.Descricao ?? string.Empty;

            return PermissoesPrivilegiadas.Contains(descricaoPermissao);
        }
    }
}
