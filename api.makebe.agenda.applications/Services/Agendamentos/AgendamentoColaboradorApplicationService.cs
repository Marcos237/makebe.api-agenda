using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using AutoMapper;

namespace api.makebe.agenda.applications.Services.Agendamentos
{
    public class AgendamentoColaboradorApplicationService : IAgendamentoColaboradorApplicationService
    {
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IAgendamentoColaboradorDomainService _agendaColaboradorDomainService;
        private readonly INotificationContext _notificationContext;
        private readonly IColaboradorProfissionalDomainService _colaboradorProfissionalDomainService;
        private readonly IMapper _mapper;
        public AgendamentoColaboradorApplicationService(IContaEventCrossCuttingService contaEventCrossCuttingService, 
            INotificationContext notificationContext, IMapper mapper, IAgendamentoColaboradorDomainService agendaColaboradorDomainService, IColaboradorProfissionalDomainService colaboradorProfissionalDomainService)
        {
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _notificationContext = notificationContext;
            _mapper = mapper;   
            _agendaColaboradorDomainService = agendaColaboradorDomainService;
            _colaboradorProfissionalDomainService = colaboradorProfissionalDomainService;

        }
        public async Task<ResponseModel<ColaboradorDTO>> BuscarColaboladoresPorConta(string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var colaboradoresProfissionais = await _colaboradorProfissionalDomainService.BuscarPorConta(conta?.Id.ToString() ?? string.Empty);
            var colaboradorMap = _mapper.Map<IEnumerable<ColaboradorDTO>>(colaboradoresProfissionais);
            return ResponseModelHelper<ColaboradorDTO>.RetornarResponseModel(colaboradorMap, _notificationContext.Notifications);
        }
    }
}
