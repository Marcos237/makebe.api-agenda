using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IColaboradorDomainService
    {
        Task<IEnumerable<UsuarioDTO>> BuscarUsuarios(PaginacaoDTO<UsuarioDTO> paginacao);
    }
}
