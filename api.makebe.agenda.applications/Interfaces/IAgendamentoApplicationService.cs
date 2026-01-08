using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IAgendamentoApplicationService
    {
        Task<ResponseModel<PaginacaoDTO<AgendamentoDTO>>> BuscarAgendamentoPaginado(PaginacaoDTO<AgendamentoDTO> paginacao, string usuario);
        Task<ResponseModel<AgendamentoDTO>> BuscarAgendamentoPorId(int id);
        Task<ResponseModel<AgendamentoDTO>> BuscarAgendamentoPorAno(int ano, int id, string conta);
        Task<ResponseModel<AgendamentoDTO>> BuscarAgendamentoPorData(string data, int id, string conta);
        Task<ResponseModel<AgendamentoDTO>> Persistir(AgendamentoDTO agendamentoDTO, string usuario);
        Task<bool> Desativar(int id);
    }
}
