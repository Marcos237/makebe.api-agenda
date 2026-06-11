using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services.Agendamentos
{
    public class PeriodoDisponivelAgendamentoApplicationService : IPeriodoDisponivelAgendamentoApplicationService
    {
        private readonly IServicosRepository _servicosRepository;
        private readonly IAgendamentoColaboradorRepository _agendamentoColaboradorRepository;
        private readonly IPeriodoDisponivelAgendamentoDomainService _periodoDisponivelAgendamentoDomainService;

        public PeriodoDisponivelAgendamentoApplicationService(
            IServicosRepository servicosRepository,
            IAgendamentoColaboradorRepository agendamentoColaboradorRepository,
            IPeriodoDisponivelAgendamentoDomainService periodoDisponivelAgendamentoDomainService)
        {
            _servicosRepository = servicosRepository;
            _agendamentoColaboradorRepository = agendamentoColaboradorRepository;
            _periodoDisponivelAgendamentoDomainService = periodoDisponivelAgendamentoDomainService;
        }

        public async Task<IEnumerable<PeriodoDTO>> BuscarPeriodosDisponiveis(PeriodoDisponivelRequestDTO request)
        {
            if (request.IdServico == 0 || request.IdColaborador == 0 || request.Data == default)
                return Enumerable.Empty<PeriodoDTO>();

            var servico = await _servicosRepository.BuscarPorId(request.IdServico);
            if (servico.Id == 0 || servico.Periodo <= 0)
                return Enumerable.Empty<PeriodoDTO>();

            var agendas = await _agendamentoColaboradorRepository.BuscarPeriodosPorColaboradorId(request.IdColaborador);
            return await _periodoDisponivelAgendamentoDomainService.MontarPeriodosDisponiveis(request.Data, servico.Periodo, agendas);
        }
    }
}
