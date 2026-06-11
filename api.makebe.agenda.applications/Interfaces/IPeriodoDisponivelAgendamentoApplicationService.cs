using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IPeriodoDisponivelAgendamentoApplicationService
    {
        Task<IEnumerable<PeriodoDTO>> BuscarPeriodosDisponiveis(PeriodoDisponivelRequestDTO request);
    }
}
