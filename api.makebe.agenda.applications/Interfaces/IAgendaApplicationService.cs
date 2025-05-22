using api.makebe.agenda.applications.Factorys.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IAgendaApplicationService
    {
        Task<ResponseModel<PaginacaoDTO<AgendaDTO>>> BuscarTodosPaginado(PaginacaoDTO<AgendaPayload> paginacaoDTO, string usuarioId);
        Task<ResponseModel<AgendaDTO>> BuscarPorId(int id, int tipo);
        Task<ResponseModel<AgendaDTO>> Persitir(AgendaPayload item, string usuarioId);
        Task<bool> Desativar(int id, string usuarioId);
    }
}
