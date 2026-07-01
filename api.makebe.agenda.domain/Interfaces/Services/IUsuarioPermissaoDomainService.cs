using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IUsuarioPermissaoDomainService
    {
        Task<UsuarioAutenticadoDTO> BuscarUsuarioAutenticado();
        Task<bool> PossuiAcessoCompletoConta();
    }
}
