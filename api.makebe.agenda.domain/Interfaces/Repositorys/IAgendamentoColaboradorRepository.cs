using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IAgendamentoColaboradorRepository
    {
        Task<IEnumerable<ColaboradorDTO>> BuscarAgendamentoColaboradores(string conta);
        Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoColaboradorAgendaBloqueada(int idColaborador, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoColaboradorDatas(int idColaborador, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoColaboradorDisponivel(int idColaborador, DateTime dataInicio, DateTime dataFim, int idAgendamento);


    }
}
