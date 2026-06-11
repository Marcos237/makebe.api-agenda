using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IPeriodoDisponivelAgendamentoDomainService
    {
        Task<IEnumerable<PeriodoDTO>> MontarPeriodosDisponiveis(DateTime data, decimal periodoServico, IEnumerable<AgendamentoColaboradorPeriodoDTO> agendas);
    }
}
