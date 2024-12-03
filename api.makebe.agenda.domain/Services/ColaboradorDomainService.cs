using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ColaboradorDomainService : IColaboradorDomainService
    {
        public Task<IEnumerable<UsuarioDTO>> BuscarUsuarios(PaginacaoDTO<UsuarioDTO> paginacao)
        {
            throw new NotImplementedException();
        }
    }
}
