using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IColaboradorApplicationService
    {
        Task<ResponseModel<PaginacaoDTO<ColaboradorDTO>>> BuscarUsuariosPaginado(PaginacaoDTO<UsuarioDTO> paginacao, string usuario);
        Task<ResponseModel<ColaboradorDTO>> BuscarUsuarioPorId(Guid id);
        Task<ResponseModel<ColaboradorDTO>> Persistir(ColaboradorPayload usuarioPayload, string usuario);
        Task<bool> Desativar(int id, string usuarioId);
    }
}
