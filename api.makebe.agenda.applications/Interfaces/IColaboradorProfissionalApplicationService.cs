using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IColaboradorProfissionalApplicationService
    {
        Task<ResponseModel<PaginacaoDTO<ColaboradorProfissionalDTO>>> BuscarUsuariosPaginado(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao, string usuario);
        Task<ResponseModel<ColaboradorProfissionalDTO>> BuscarUsuarioPorId(int id);
        Task<ResponseModel<ColaboradorProfissionalDTO>> Persistir(ColaboradorProfissionalPayload usuarioPayload);
        Task<bool> Desativar(int id);
        Task<ResponseModel<ColaboradorProfissionalDTO>> BuscarPorConta(string usuario);
    }
}
