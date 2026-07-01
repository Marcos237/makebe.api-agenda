using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IUsuarioAutenticadoService
    {
        Task<UsuarioAutenticadoDTO> BuscarUsuarioAutenticado();
    }
}
