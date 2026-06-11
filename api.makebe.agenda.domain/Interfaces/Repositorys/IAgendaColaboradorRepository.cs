using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IAgendaColaboradorRepository
    {
        Task<AgendaDTO> BuscarPorIdColaborador(int idColaborador);
        Task<AgendaDTO> BuscarAgendaPorColaboradorId(int idColaborador);
        Task<IEnumerable<AgendaDTO>> BuscarAgendamentosPorColaboradorId(int idColaborador);
    }
}
