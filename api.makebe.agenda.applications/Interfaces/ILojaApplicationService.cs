using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface ILojaApplicationService
    {
        Task<ResponseModel<PaginacaoDTO<LojaResponse>>> BuscarTodos(PaginacaoDTO<LojaPayload> paginacaoDTO, string usuarioId);
        Task<ResponseModel<LojaResponse>> BuscarPorId(int id);
        Task<ResponseModel<LojaResponse>> Persitir(LojaPayload item, string usuarioId);
        Task<bool> Desativar(int id, string usuarioId);
    }
}
