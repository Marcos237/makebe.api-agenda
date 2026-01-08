using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.domain.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using AutoMapper;
using ContasEvent;

namespace api.makebe.agenda.applications.Services.Agendamentos
{
    public class AgendamentoColaboradorApplicationService : IAgendamentoColaboradorApplicationService
    {
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IAgendamentoColaboradorDomainService _agendaColaboradorDomainService;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;
        public AgendamentoColaboradorApplicationService(IContaEventCrossCuttingService contaEventCrossCuttingService, 
            INotificationContext notificationContext, IMapper mapper, IAgendamentoColaboradorDomainService agendaColaboradorDomainService)
        {
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _notificationContext = notificationContext;
            _mapper = mapper;   
            _agendaColaboradorDomainService = agendaColaboradorDomainService;

        }
        public async Task<ResponseModel<ColaboradorDTO>> BuscarColaboladoresPorConta(string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var usuarioConta = new UsuarioContaConsultadoPorContaEvent() { IdConta = conta?.Id ?? Guid.Empty };
            var usuarioEvent = await _contaEventCrossCuttingService.BuscarUsuarioContaPorIdConta(usuarioConta);
            var permissaoId = ConfigHelper.GetValue(BaseConstant.ClientePermissao ?? string.Empty);
            var usuarioContaFiltro = usuarioEvent.UsuariosEvents?.Where(usuario => usuario.PermissaoId != PropiedadesHelper.ParseGuidOrDefault(permissaoId
                ?? string.Empty));
            var usuarioMap = _mapper.Map<IEnumerable<UsuarioDTO>>(usuarioContaFiltro);
            var colaboradorMap = await _agendaColaboradorDomainService.MontarColaboradores(usuarioMap, conta?.Id.ToString() ?? string.Empty);
            return ResponseModelHelper<ColaboradorDTO>.RetornarResponseModel(colaboradorMap, _notificationContext.Notifications);
        }
    }
}
