using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IAgendamentoApplicationService
    {
        Task<ResponseModel<PaginacaoDTO<AgendamentoDTO>>> BuscarAgendamentoPaginado(PaginacaoDTO<AgendamentoDTO> paginacao, string usuario);
        Task<ResponseModel<AgendamentoDTO>> BuscarAgendamentoPorId(string id);
        Task<ResponseModel<AgendamentoDTO>> Persistir(ColaboradorPayload usuarioPayload, string usuario);
        Task<ResponseModel<AgendamentoDTO>> Desativar(int id);
    }
}
