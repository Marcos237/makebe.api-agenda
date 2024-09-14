using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface ILojaApplicationService
    {
        Task<ResponseModel<LojaResponse>> BuscarTodos(PaginacaoDTO<LojaPayload> paginacaoDTO, string usuarioId);
        Task<ResponseModel<LojaResponse>> BuscarPorId(int id);
        Task<int> Salvar(LojaPayload item);
        Task<LojaResponse> Atualizar(LojaPayload item);
        Task<bool> Desativar(int id);
    }
}
