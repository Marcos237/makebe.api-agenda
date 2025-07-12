using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.crosscutting.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IAgendamentoDomainService
    {
        Task<PaginacaoDTO<AgendamentoDTO>> MontarAgendamento(PaginacaoDTO<AgendamentoDTO> paginacao, string constaId, IEnumerable<UsuarioEvent>? UsuariosEvents, IEnumerable<UsuarioEvent>? ColaboradoresEvents);
        PaginacaoDTO<AgendamentoDTO> Filtrar(PaginacaoDTO<AgendamentoDTO> paginacao);
        Task<AgendamentoDTO> BuscarPorId(int id);
        Task<int> Salvar(Agendamento agendamento);
        Task<bool> Desativa(int id);
    }
}
