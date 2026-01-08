using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IAgendamentoLojaRepository
    {
        Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoLojaAgendaAberta(int colaboradorId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoLojaBloqueada(int colaboradorId, DateTime dataInicio, DateTime dataFim);
    }
}
